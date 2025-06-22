using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using OMRON.Compolet.Variable;
using System.Text;

namespace EipTagLibrary
{
    /// <summary>
    /// Copyright (c) 2025 All Rights Reserved.	
    /// 描述：EIP标签访问类
    /// 创建人： Himma
    /// 创建时间：2025/6/15 13:14:52
    /// </summary>
    public enum VariableType
    {
        TIMER = 1,
        COUNTER = 2,
        CHANNEL = 3,
        UINT_BCD = 4,
        UDINT_BCD = 5,
        ULINT_BCD = 6,
        BOOL = 193,
        INT = 195,
        DINT = 196,
        LINT = 197,
        UINT = 199,
        UDINT = 200,
        ULINT = 201,
        REAL = 202,
        LREAL = 203,
        STRING = 208,
        WORD = 210,
        DWORD = 211,
        LWORD = 212,
        ASTRUCT = 160,
        STRUCT = 162,
        ARRAY = 163
    }

    /// <summary>
    /// Tag值变化事件的参数类
    /// </summary>
    public class TagValueChangedEventArgs : EventArgs
    {
        /// <summary>TagGroup的名称</summary>
        public string TagGroupName { get; }
        /// <summary>Block的类型</summary>
        public string BlockType { get; }
        /// <summary>发生变化的Item</summary>
        public ItemBase Item { get; }
        /// <summary>变化后的值</summary>
        public object Value { get; }
        /// <summary>变化发生的时间戳</summary>
        public DateTime Timestamp { get; }
        /// <summary>字偏移量</summary>
        public int WordOffset { get; }
        /// <summary>位偏移量</summary>
        public int BitOffset { get; }
        /// <summary>数组索引（如果是数组项）</summary>
        public int ArrayIndex { get; }

        public TagValueChangedEventArgs(
            string tagGroupName,
            string blockType,
            ItemBase item,
            object value,
            int wordOffset,
            int bitOffset,
            int arrayIndex = -1)
        {
            TagGroupName = tagGroupName;
            BlockType = blockType;
            Item = item;
            Value = value;
            Timestamp = DateTime.Now;
            WordOffset = wordOffset;
            BitOffset = bitOffset;
            ArrayIndex = arrayIndex;
        }
    }

    /// <summary>
    /// EIP通信访问类，用于读写PLC数据
    /// </summary>
    public class EipTagAccess : IDisposable
    {
        /// <summary>
        /// OMRON PLC通信组件
        /// </summary>
        private readonly VariableCompolet variableCompolet;

        /// <summary>
        /// Tag配置信息
        /// </summary>
        private readonly TagConfig tagConfig;

        /// <summary>
        /// Tag地址缓存，用于快速查找Tag的地址信息
        /// </summary>
        private readonly Dictionary<string, TagAddressInfo> tagAddressCache;

        /// <summary>
        /// 对象是否已释放标志
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// 监控取消令牌源
        /// </summary>
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// 上次读取的值缓存，用于变化检测
        /// </summary>
        private Dictionary<string, ushort[]> lastValues;

        /// <summary>
        /// Tag值变化事件委托
        /// </summary>
        public delegate void TagValueChangedEventHandler(object sender, TagValueChangedEventArgs e);

        /// <summary>
        /// Tag值变化事件
        /// </summary>
        public event TagValueChangedEventHandler OnTagValueChanged;

        /// <summary>
        /// 初始化EipTagAccess实例
        /// </summary>
        /// <param name="xmlConfigPath">XML配置文件路径</param>
        public EipTagAccess(string xmlConfigPath)
        {
            variableCompolet = new VariableCompolet();
            variableCompolet.Active = true;
            //utf-8
            variableCompolet.PlcEncoding = Encoding.UTF8;


            tagConfig = LoadXmlConfig(xmlConfigPath);
            tagAddressCache = new Dictionary<string, TagAddressInfo>();
            lastValues = new Dictionary<string, ushort[]>();
            InitializeTagAddressCache();
            cancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// 开始监控数据变化
        /// </summary>
        /// <param name="intervalMs">监控间隔（毫秒），默认为100ms</param>
        /// <returns>监控任务</returns>
        /// <remarks>
        /// 此方法会定期读取所有Input方向的TagGroup数据，
        /// 当检测到值变化时触发OnTagValueChanged事件。
        /// 只监控以"Report"或"Reply"结尾的Block。
        /// </remarks>
        public async Task StartMonitoring(int intervalMs = 100)
        {
            ValidateNotDisposed();

            try
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    var inputGroups = tagConfig.TagGroups
                        .Where(g => g.Direction.Equals("Input", StringComparison.OrdinalIgnoreCase));

                    foreach (var group in inputGroups)
                    {
                        try
                        {
                            // 读取当前值
                            var rawValues = ReadRawValues(group.Name);
                            //  var convertedValues = ConvertRawValuesToTagGroup(group, rawValues);

                            ushort[] lastRawValues;
                            if (lastValues.ContainsKey(group.Name))
                            {
                                lastRawValues = (ushort[])lastValues[group.Name].Clone();
                            }
                            else
                            {
                                lastRawValues = new ushort[rawValues.Length];
                            }
                            if (group.Name == "RV_CIMToEQ_RecipeManagement_01_03_00")
                            { 
                            
                            }
                            // 检查值变化并触发事件
                            if (HasValuesChanged(group.Name, rawValues))
                            {

                                ConvertRawValuesToTagGroup(group, rawValues);
                                ProcessTagGroupMonitoring(group, lastRawValues);

                                lastValues[group.Name] = rawValues;
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error reading TagGroup {group.Name}: {ex.Message}");
                        }
                    }

                    await Task.Delay(intervalMs, cancellationTokenSource.Token);
                }
            }
            catch (OperationCanceledException)
            {
                // 正常取消，不需要处理
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Monitoring error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 停止监控
        /// </summary>
        public void StopMonitoring()
        {
            cancellationTokenSource?.Cancel();
        }

        /// <summary>
        /// 从PLC读取并转换原始数据为short数组
        /// </summary>
        /// <param name="groupName">TagGroup名称</param>
        /// <returns>转换后的short数组</returns>
        /// <exception cref="NotSupportedException">
        /// 当读取到不支持的数据类型时抛出
        /// </exception>
        /// <exception cref="OverflowException">
        /// 当数值转换发生溢出时抛出
        /// </exception>
        /// <remarks>
        /// 此方法支持以下数据类型的转换：
        /// - 有符号整数：short(INT)、int(DINT)、long(LINT)
        /// - 无符号整数：ushort(UINT)、uint(UDINT)、ulong(ULINT)
        /// - 浮点数：float(REAL)、double(LREAL)
        /// - 布尔值：bool(BOOL)
        /// - 字符串：string(STRING)
        /// 所有类型最终都会被转换为short数组
        /// </remarks>
        private ushort[] ReadRawValues(string groupName)
        {
            var obj = variableCompolet.ReadVariable(groupName);

            // 如果直接是short数组，直接返回
            if (obj is ushort[] shortArray)
            {
                return shortArray;
            }
          
            // 判断是否为数组类型
            if (!obj.GetType().IsArray)
            {
                throw new NotSupportedException($"Expected array type but got {obj.GetType().Name}");
            }

            var array = (Array)obj;
            Type elementType = array.GetType().GetElementType();

            // 判断是否为结构体数组
            if (elementType.IsClass && elementType != typeof(string))
            {
                throw new NotSupportedException($"Complex type arrays are not supported: {elementType.Name}");
            }

            // 获取变量类型
            VariableType varType;
            if (elementType == typeof(short))
            {
                varType = VariableType.INT;
            }
            else if (elementType == typeof(int))
            {
                varType = VariableType.INT;
            }
            else if (elementType == typeof(long))
            {
                varType = VariableType.LINT;
            }
            else if (elementType == typeof(float))
            {
                varType = VariableType.REAL;
            }
            else if (elementType == typeof(double))
            {
                varType = VariableType.LREAL;
            }
            else if (elementType == typeof(bool))
            {
                varType = VariableType.BOOL;
            }
            else if (elementType == typeof(string))
            {
                varType = VariableType.STRING;
            }
            else if (elementType == typeof(ushort))
            {
                varType = VariableType.UINT;
            }
            else if (elementType == typeof(uint))
            {
                varType = VariableType.UDINT;
            }
            else if (elementType == typeof(ulong))
            {
                varType = VariableType.ULINT;
            }
            else
            {
                throw new NotSupportedException($"Unsupported array element type: {elementType.Name}");
            }

            // 根据变量类型进行转换
            try
            {
                switch (varType)
                {
                    case VariableType.INT:
                        //return (ushort[])array;
                        int[] intArray1 = (int[])array;
                        return Array.ConvertAll(intArray1, item => (ushort)item);

                    case VariableType.UINT:
                        ushort[] ushortArray = (ushort[])array;
                        return Array.ConvertAll(ushortArray, item => (ushort)item);

                    case VariableType.DINT:
                    case VariableType.UDINT:
                        int[] intArray = (int[])array;
                        return Array.ConvertAll(intArray, item => (ushort)item);
                      //  return Array.ConvertAll(intArray, item => Convert.ToInt16(item));

                    //case VariableType.LINT:
                    //case VariableType.ULINT:
                    //    long[] longArray = (long[])array;
                    //    return Array.ConvertAll(longArray, item => Convert.ToInt16(item));

                    //case VariableType.REAL:
                    //    float[] floatArray = (float[])array;
                    //    return Array.ConvertAll(floatArray, item => Convert.ToInt16(item));

                    //case VariableType.LREAL:
                    //    double[] doubleArray = (double[])array;
                    //    return Array.ConvertAll(doubleArray, item => Convert.ToInt16(item));

                    //case VariableType.BOOL:
                    //    bool[] boolArray = (bool[])array;
                    //    return Array.ConvertAll(boolArray, item => Convert.ToInt16(item));

                    //case VariableType.STRING:
                    //    string[] stringArray = (string[])array;
                    //    return Array.ConvertAll(stringArray, item => Convert.ToInt16(item));

                    default:
                        throw new NotSupportedException($"Unsupported variable type: {varType}");
                }
            }
            catch (OverflowException ex)
            {
                throw new OverflowException($"Value overflow when converting {varType} {groupName} to INT16", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error converting {varType} array to INT16 array", ex);
            }
        }

        // 获取指定TagGroup的最新值
        public object[] GetTagGroupValues(string tagGroupName)
        {
            ValidateNotDisposed();

            var group = tagConfig.TagGroups.FirstOrDefault(g =>
                g.Name.Equals(tagGroupName, StringComparison.OrdinalIgnoreCase));

            if (group == null)
                throw new ArgumentException($"TagGroup '{tagGroupName}' not found");

            var rawValues = ReadRawValues(tagGroupName);
            return ConvertRawValuesToTagGroup(group, rawValues);
        }

        /// <summary>
        /// 加载XML配置文件
        /// </summary>
        /// <param name="xmlPath">XML文件路径</param>
        /// <returns>解析后的TagConfig对象</returns>
        /// <exception cref="Exception">
        /// 当配置文件格式错误或缺少必要信息时抛出
        /// </exception>
        /// <remarks>
        /// 此方法会：
        /// 1. 反序列化XML文件为TagConfig对象
        /// 2. 验证配置的完整性
        /// 3. 计算每个Block的实际大小
        /// </remarks>
        private TagConfig LoadXmlConfig(string xmlPath)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(TagConfig));
                using (var reader = new StreamReader(xmlPath))
                {
                    var config = (TagConfig)serializer.Deserialize(reader);

                    // 验证配置
                    if (config.TagGroups == null || config.TagGroups.Count == 0)
                        throw new Exception("No TagGroups found in configuration");

                    foreach (var group in config.TagGroups)
                    {
                        if (string.IsNullOrEmpty(group.Name))
                            throw new Exception("TagGroup name cannot be empty");

                        if (group.Blocks == null || group.Blocks.Count == 0)
                            throw new Exception($"No Blocks found in TagGroup '{group.Name}'");

                        // 计算每个Block的实际大小
                        foreach (var block in group.Blocks)
                        {
                            if (block.Items == null || block.Items.Count == 0)
                                throw new Exception($"No Items found in Block '{block.Type}' of TagGroup '{group.Name}'");

                            // 计算Block的实际大小
                            //int maxOffset = 0;
                            //foreach (var item in block.Items)
                            //{
                            //    if (item is ArrayItem arrayItem)
                            //    {
                            //        int itemSize = arrayItem.Count * arrayItem.Item.Length;
                            //        maxOffset = Math.Max(maxOffset, arrayItem.BaseOffset + itemSize);
                            //    }
                            //    else
                            //    {
                            //        string[] offsetParts = item.Offset.Split(':');
                            //        int wordOffset = int.Parse(offsetParts[0]);
                            //        maxOffset = Math.Max(maxOffset, wordOffset + item.Length);
                            //    }
                            //}
                            //block.Size = maxOffset;
                        }
                    }

                    return config;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load XML configuration from {xmlPath}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 初始化Tag地址缓存
        /// </summary>
        /// <remarks>
        /// 遍历所有TagGroup、Block和Item，
        /// 计算并缓存每个Tag的实际地址，
        /// 用于后续快速访问。
        /// </remarks>
        private void InitializeTagAddressCache()
        {
            foreach (var tagGroup in tagConfig.TagGroups)
            {
                ProcessTagGroup(tagGroup);
            }
        }

        /// <summary>
        /// 处理单个TagGroup的配置
        /// </summary>
        /// <param name="tagGroup">要处理的TagGroup</param>
        /// <remarks>
        /// 遍历TagGroup中的所有Block，
        /// 调用ProcessBlock方法处理每个Block。
        /// </remarks>
        private void ProcessTagGroup(TagGroup tagGroup)
        {
            foreach (var block in tagGroup.Blocks)
            {
                ProcessBlock(tagGroup, block);
            }
        }

        /// <summary>
        /// 处理Block的配置
        /// </summary>
        /// <param name="tagGroup">所属的TagGroup</param>
        /// <param name="block">要处理的Block</param>
        /// <remarks>
        /// 遍历Block中的所有Item，
        /// 根据Item类型调用相应的处理方法。
        /// </remarks>
        private void ProcessBlock(TagGroup tagGroup, Block block)
        {
            foreach (var item in block.Items)
            {
                if (item is ArrayItem arrayItem)
                {
                    ProcessArrayItem(tagGroup, block, arrayItem);
                }
                else
                {
                    ProcessSingleItem(tagGroup, block, item);
                }
            }
        }

        /// <summary>
        /// 处理数组类型的Item
        /// </summary>
        /// <param name="tagGroup">所属的TagGroup</param>
        /// <param name="block">所属的Block</param>
        /// <param name="arrayItem">要处理的ArrayItem</param>
        /// <remarks>
        /// 为数组的每个元素计算地址并添加到缓存中。
        /// 支持位数组的特殊处理。
        /// </remarks>
        private void ProcessArrayItem(TagGroup tagGroup, Block block, ArrayItem arrayItem)
        {
            for (int i = 0; i < arrayItem.Count; i++)
            {
                string itemName = $"{arrayItem.Name}[{i}]";
                string tagAddress = $"{tagGroup.Name}.{block.Type}.{itemName}";
                int itemOffset = arrayItem.BaseOffset + (i * arrayItem.Item.Length);

                tagAddressCache[tagAddress] = new TagAddressInfo
                {
                    TagGroup = tagGroup,
                    Block = block,
                    Item = arrayItem.Item,
                    Address = CalculateAddress(block.Offset + itemOffset, arrayItem.Item.Area, arrayItem.Item.Format)
                };
            }
        }

        /// <summary>
        /// 处理单个Item
        /// </summary>
        /// <param name="tagGroup">所属的TagGroup</param>
        /// <param name="block">所属的Block</param>
        /// <param name="item">要处理的Item</param>
        /// <remarks>
        /// 计算Item的实际地址并添加到缓存中。
        /// 支持位类型的特殊处理。
        /// </remarks>
        private void ProcessSingleItem(TagGroup tagGroup, Block block, ItemBase item)
        {
            string tagAddress = $"{tagGroup.Name}.{block.Type}.{item.Name}";
            string[] offsetParts = item.Offset.Split(':');
            int offset = int.Parse(offsetParts[0]);

            tagAddressCache[tagAddress] = new TagAddressInfo
            {
                TagGroup = tagGroup,
                Block = block,
                Item = item,
                Address = CalculateAddress(block.Offset + offset, item.Area, item.Format,
                    offsetParts.Length > 1 ? int.Parse(offsetParts[1]) : -1)
            };
        }

        /// <summary>
        /// 计算PLC地址
        /// </summary>
        /// <param name="absoluteOffset">绝对偏移量</param>
        /// <param name="area">PLC区域</param>
        /// <param name="format">数据格式</param>
        /// <param name="bitOffset">位偏移量（对于BIT类型）</param>
        /// <returns>格式化的PLC地址字符串</returns>
        /// <remarks>
        /// 根据不同的数据格式生成相应的地址格式。
        /// 对于BIT类型，会包含位偏移量。
        /// </remarks>
        private string CalculateAddress(int absoluteOffset, string area, string format, int bitOffset = -1)
        {
            string address = $"{area}{absoluteOffset}";
            if (format == "BIT" && bitOffset >= 0)
            {
                address += $".{bitOffset}";
            }
            return address;
        }

        public T ReadTag<T>(string tagAddress)
        {
            ValidateNotDisposed();

            if (!tagAddressCache.TryGetValue(tagAddress, out var tagInfo))
            {
                throw new ArgumentException($"Tag address '{tagAddress}' not found in configuration");
            }

            try
            {
                object value = variableCompolet.ReadVariable(tagInfo.Address);
                return ConvertValue<T>(value, tagInfo.Item.Format);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to read tag '{tagAddress}' at address '{tagInfo.Address}': {ex.Message}", ex);
            }
        }

        public void WriteTag<T>(string tagAddress, T value)
        {
            ValidateNotDisposed();

            if (!tagAddressCache.TryGetValue(tagAddress, out var tagInfo))
            {
                throw new ArgumentException($"Tag address '{tagAddress}' not found in configuration");
            }

            try
            {
                object convertedValue = ConvertToTargetType(value, tagInfo.Item.Format);
                variableCompolet.WriteVariable(tagInfo.Address, convertedValue);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to write tag '{tagAddress}' at address '{tagInfo.Address}': {ex.Message}", ex);
            }
        }

        private T ConvertValue<T>(object value, string format)
        {
            try
            {
                switch (format)
                {
                    case "BIT":
                        return (T)Convert.ChangeType(value, typeof(bool));
                    case "INT":
                    case "SINT":
                        return (T)Convert.ChangeType(value, typeof(int));
                    case "FLOAT":
                        return (T)Convert.ChangeType(value, typeof(float));
                    case "ASCII":
                        return (T)Convert.ChangeType(value?.ToString().Trim(), typeof(string));
                    default:
                        throw new NotSupportedException($"Unsupported format: {format}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to convert value from format '{format}' to type '{typeof(T).Name}'", ex);
            }
        }

        private object ConvertToTargetType(object value, string format)
        {
            try
            {
                switch (format)
                {
                    case "BIT":
                        return Convert.ToBoolean(value);
                    case "INT":
                    case "SINT":
                        return Convert.ToInt32(value);
                    case "FLOAT":
                        return Convert.ToSingle(value);
                    case "ASCII":
                        return value.ToString();
                    default:
                        throw new NotSupportedException($"Unsupported format: {format}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to convert value to format '{format}'", ex);
            }
        }

        /// <summary>
        /// 将原始数据转换为TagGroup的值数组
        /// </summary>
        /// <param name="group">TagGroup对象</param>
        /// <param name="rawValues">原始short数组</param>
        /// <returns>转换后的对象数组</returns>
        /// <remarks>
        /// 根据每个Item的格式将原始数据转换为相应的类型。
        /// 支持数组项和位操作的特殊处理。
        /// </remarks>
        object lock1 = new object();
        private object[] ConvertRawValuesToTagGroup(TagGroup group, ushort[] rawValues)
        {
            lock (lock1)
            {
                var result = new object[rawValues.Length];
                int currentOffset = 0;

                foreach (var block in group.Blocks)
                {
                    if (block.Type == "JobDataRequestReplyBlock")
                    {

                    }
                    currentOffset = block.Offset;
                    int wordOffset = 0;



                    foreach (var item in block.Items)
                    {

                        if (item is ArrayItem arrayItem)
                        {
                            ProcessArrayItem(arrayItem, rawValues, result, ref currentOffset);
                        }
                        else
                        {
                            if (item.Format == "BIT")
                            {
                                string[] offsetParts = item.Offset.Split(':');

                                int owordOffset = int.Parse(offsetParts[0]);
                                if(offsetParts.Length==1)
                                {
                                    currentOffset = block.Offset + int.Parse(item.Offset);

                                }
                                if (owordOffset != wordOffset && offsetParts.Length == 2)
                                {
                                    currentOffset = currentOffset + owordOffset;
                                    wordOffset = owordOffset;
                                }
                            }
                            else
                            {
                                if (item.Name == "ProcessingFlagMachineLocalNo")
                                {

                                }

                                currentOffset = block.Offset + int.Parse(item.Offset);
                            }
                            if (currentOffset == 563 )
                            {

                            }

                            ProcessSingleItem(item, rawValues, result, currentOffset);

                        }
                    }
                }

                return result;
            }
        }

        private void ProcessArrayItem(ArrayItem arrayItem, ushort[] rawValues, object[] result, ref int currentIndex)
        {
            if (arrayItem.Item.Format == "BIT")
            {
                string[] offsetParts = arrayItem.Item.Offset.Split(':');
                if (offsetParts.Length == 2)
                {
                    int wordOffset = int.Parse(offsetParts[0]);
                    int bitOffset = int.Parse(offsetParts[1]);

                    for (int i = 0; i < arrayItem.Count; i++)
                    {
                        int totalBits = bitOffset + i;
                        int actualWordOffset = wordOffset + (totalBits / 16);
                        int actualBitOffset = totalBits % 16;

                        // 读取位值并更新Value属性
                        bool value = (rawValues[actualWordOffset] & (1 << actualBitOffset)) != 0;
                        result[currentIndex + i] = value;
                        arrayItem.Item.Value = value;  // 更新Value属性
                    }
                }
                currentIndex += arrayItem.Count;
            }
            else
            {
                for (int i = 0; i < arrayItem.Count; i++)
                {
                    var value = ConvertValueByFormat(arrayItem.Item.Format, rawValues, currentIndex, arrayItem.Item.Length);
                    result[currentIndex] = value;
                    arrayItem.Item.Value = value;  // 更新Value属性
                    currentIndex += arrayItem.Item.Length;
                }
            }
        }

        private void ProcessSingleItem(ItemBase item, ushort[] rawValues, object[] result,  int currentIndex)
        {
            try
            {
                if (item.Format == "BIT")
                {
                   
                   
                    int wordOffset = 0;
                    int bitOffset = 0;
                    string[] offsetParts = item.Offset.Split(':');
                    if (offsetParts.Length == 2)
                    {
                        wordOffset = int.Parse(offsetParts[0]);
                        bitOffset = int.Parse(offsetParts[1]);

                        // 读取位值并更新Value属性
                        bool value = (rawValues[currentIndex] & (1 << bitOffset)) != 0;
                        result[currentIndex] = value;
                        item.Value = value;  // 更新Value属性
                    }
                    if (offsetParts.Length == 1)
                    {
                        item.Value = ConvertValueByFormat(item.Format, rawValues, currentIndex, item.Length);// rawValues[currentIndex];


                    }
                        // currentIndex = currentIndex+wordOffset;
                    }
                else
                {
                    if (currentIndex == 563)
                    {

                    }
                    if (item.Name == "ProcessingFlagMachineLocalNo")
                    {

                    }
                    var value = ConvertValueByFormat(item.Format, rawValues, currentIndex, item.Length);
                    result[currentIndex] = value;
                    item.Value = value;  // 更新Value属性
                  //  currentIndex += item.Length;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error processing item '{item.Name}' at offset {currentIndex}: {ex.Message}", ex);
            }
        }

        //指定长度的Ushort数组转二进制字符串
        static string UshortArrayToBinary(ushort[] values, int offset, int length)
        {
            string bit = "";

            // 确保 offset 和 length 在合理范围内
            if (offset < 0 || length < 0 || offset + length > values.Length)
            {
                throw new ArgumentException("offset 和 length 参数超出数组范围");
            }

            for (int i = offset; i < offset + length; i++)
            {
                string sub = Convert.ToString(values[i], 2).PadLeft(16, '0');
                bit += sub;
            }

            return bit;
        }

        /// <summary>
        /// 根据格式转换值
        /// </summary>
        /// <param name="format">数据格式</param>
        /// <param name="rawValues">原始数据数组</param>
        /// <param name="offset">偏移量</param>
        /// <param name="length">数据长度</param>
        /// <param name="offsetStr">位偏移字符串（可选）</param>
        /// <returns>转换后的值</returns>
        /// <exception cref="NotSupportedException">
        /// 当指定了不支持的格式时抛出
        /// </exception>
        /// <remarks>
        /// 支持的格式：
        /// - BIT：布尔值
        /// - INT/SINT：16/32位整数
        /// - FLOAT：32位浮点数
        /// - ASCII：字符串
        /// </remarks>
        private object ConvertValueByFormat(string format, ushort[] rawValues, int offset, int length, string offsetStr = null)
        {
            try
            {
                switch (format)
                {
                    case "BIN":

                       return UshortArrayToBinary(rawValues, offset, length);

                    case "INT":
                    case "SINT":
                        if (length == 1)
                            return (int)rawValues[offset];
                        else if (length == 2)
                            return (int)((rawValues[offset] << 16) | (rawValues[offset + 1] & 0xFFFF));
                        break;

                    case "FLOAT":
                        if (length == 2)
                        {
                            var bytes = new byte[4];
                            bytes[0] = (byte)(rawValues[offset] & 0xFF);
                            bytes[1] = (byte)(rawValues[offset] >> 8);
                            bytes[2] = (byte)(rawValues[offset + 1] & 0xFF);
                            bytes[3] = (byte)(rawValues[offset + 1] >> 8);
                            return BitConverter.ToSingle(bytes, 0);
                        }
                        break;

                    case "ASCII":
                        var chars = new char[length * 2];
                        for (int i = 0; i < length; i++)
                        {
                            chars[i * 2] = (char)(rawValues[offset + i] & 0xFF);
                            chars[i * 2 + 1] = (char)(rawValues[offset + i] >> 8);
                        }
                        return new string(chars).TrimEnd('\0');
                        
                    case "LONG":
                        if (length == 4)
                        {
                            long value = (long)(rawValues[offset] & 0xFFFF) |
                                         (long)(rawValues[offset + 1] & 0xFFFF) << 16 |
                                         (long)(rawValues[offset + 2] & 0xFFFF) << 32 |
                                         (long)(rawValues[offset + 3] & 0xFFFF) << 48;
                            return value;
                        }
                        break;

                    case "DOUBLE":
                        if (length == 4)
                        {
                            var bytes = new byte[8];
                            bytes[0] = (byte)(rawValues[offset] & 0xFF);
                            bytes[1] = (byte)(rawValues[offset] >> 8);
                            bytes[2] = (byte)(rawValues[offset + 1] & 0xFF);
                            bytes[3] = (byte)(rawValues[offset + 1] >> 8);
                            bytes[4] = (byte)(rawValues[offset + 2] & 0xFF);
                            bytes[5] = (byte)(rawValues[offset + 2] >> 8);
                            bytes[6] = (byte)(rawValues[offset + 3] & 0xFF);
                            bytes[7] = (byte)(rawValues[offset + 3] >> 8);
                            return BitConverter.ToDouble(bytes, 0);
                        }
                        break;
                }

                throw new NotSupportedException($"Unsupported format: {format}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error converting value at offset {offset} to format {format}", ex);
            }
        }

      

        /// <summary>
        /// 检查值是否发生变化
        /// </summary>
        private bool HasValuesChanged(string groupName, ushort[] currentValues)
        {
            // 如果是第一次读取，直接返回true
            if (!lastValues.ContainsKey(groupName))
            {
                lastValues[groupName] = currentValues.ToArray();
                return true;
            }

            var lastValueArray = lastValues[groupName];
            bool hasChanges = false;

            // 检查长度是否一致
            if (lastValueArray.Length != currentValues.Length)
            {
                lastValues[groupName] = currentValues.ToArray();
                return true;
            }

            // 逐个比较值
            for (int i = 0; i < currentValues.Length; i++)
            {
                if (AreValuesEqual(lastValueArray[i], currentValues[i]))
                {
                    hasChanges = true;
                    // 更新变化的值
                    lastValueArray[i] = currentValues[i];
                }
            }

            // 如果有变化，更新lastValues
            if (hasChanges)
            {
                //  lastValues[groupName] = lastValueArray;
            }

            return hasChanges;
        }

        /// <summary>
        /// 比较两个值是否相等
        /// </summary>
        private bool AreValuesEqual(object value1, object value2)
        {
            if (value1 == null && value2 == null)
                return true;
            if (value1 == null || value2 == null)
                return false;

            // 处理数组类型
            if (value1 is Array arr1 && value2 is Array arr2)
            {
                if (arr1.Length != arr2.Length)
                    return false;

                for (int i = 0; i < arr1.Length; i++)
                {
                    if (!AreValuesEqual(arr1.GetValue(i), arr2.GetValue(i)))
                        return false;
                }
                return true;
            }

            // 处理浮点数
            if (value1 is float f1 && value2 is float f2)
            {
                return Math.Abs(f1 - f2) < float.Epsilon;
            }
            if (value1 is double d1 && value2 is double d2)
            {
                return Math.Abs(d1 - d2) < double.Epsilon;
            }

            // 其他类型直接比较取反
            return !value1.ToString().Equals(value2.ToString());
        }

        /// <summary>
        /// 获取Block的总长度
        /// </summary>
        /// <param name="block">要计算的Block</param>
        /// <returns>Block的总字长度</returns>
        /// <remarks>
        /// 计算Block中所有Items的长度总和，
        /// 对于数组项会计算数组长度 * 元素长度。
        /// </remarks>
        private int GetBlockTotalLength(Block block)
        {
            int totalLength = 0;
            foreach (var item in block.Items)
            {
                if (item is ArrayItem arrayItem)
                {
                    totalLength += arrayItem.Count * arrayItem.Item.Length;
                }
                else
                {
                    totalLength += item.Length;
                }
            }
            return totalLength;
        }

        /// <summary>
        /// 检查对象是否已被释放
        /// </summary>
        private void ValidateNotDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(EipTagAccess));
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <remarks>
        /// 此方法会：
        /// 1. 停止所有监控
        /// 2. 释放取消令牌源
        /// 3. 关闭和释放PLC通信组件
        /// </remarks>
        public void Dispose()
        {
            if (!disposed)
            {
                try
                {
                    StopMonitoring();
                    cancellationTokenSource?.Dispose();

                    if (variableCompolet != null)
                    {
                        variableCompolet.Active = false;
                        variableCompolet.Dispose();
                    }
                }
                catch (Exception)
                {
                    // Ignore disposal errors
                }
                finally
                {
                    disposed = true;
                }
            }
        }

        /// <summary>
        /// 处理TagGroup的数据变化监控
        /// </summary>
        /// <param name="group">要监控的TagGroup</param>
        /// <param name="convertedValues">转换后的值数组</param>
        /// <returns>如果有值变化返回true，否则返回false</returns>
        private bool ProcessTagGroupMonitoring(TagGroup group, ushort[] convertedValues)
        {
            bool hasChanges = false;
            int currentIndex = 0;

            foreach (var block in group.Blocks)
            {
                //if (!ShouldMonitorBlock(block))
                //{
                //    currentIndex += GetBlockTotalLength(block);
                //    continue;
                //}

                currentIndex = ProcessBlockMonitoring(group.Name, block, convertedValues, currentIndex, ref hasChanges);
            }

            return hasChanges;
        }

        /// <summary>
        /// 处理Block的数据变化监控
        /// </summary>
        /// <param name="groupName">TagGroup名称</param>
        /// <param name="block">要监控的Block</param>
        /// <param name="convertedValues">转换后的值数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <param name="hasChanges">值变化标志</param>
        /// <returns>处理后的当前索引</returns>
        private int ProcessBlockMonitoring(string groupName, Block block, ushort[] convertedValues, int startIndex, ref bool hasChanges)
        {
            int currentIndex = startIndex;

            foreach (var item in block.Items)
            {
                if (item is ArrayItem arrayItem)
                {
                    currentIndex = ProcessArrayItemMonitoring(groupName, block, arrayItem, convertedValues, currentIndex, ref hasChanges);
                }
                else
                {
                    currentIndex = ProcessSingleItemMonitoring(groupName, block, item, convertedValues, currentIndex, ref hasChanges);
                }
            }

            return currentIndex;
        }

        /// <summary>
        /// 处理单个项的数据变化监控
        /// </summary>
        private int ProcessSingleItemMonitoring(
            string groupName,
            Block block,
            ItemBase item,
            ushort[] convertedValues,
            int currentIndex,
            ref bool hasChanges)
        {
            currentIndex = block.Offset;
            int wordOffset = 0;
            int bitOffset = 0;

            if (item.Format == "BIT")
            {
                if (item.Name == "RecipeParameterRequestCommand")
                { 
                
                }

                // 解析位偏移量
                string[] offsetParts = item.Offset.Split(':');

                if (offsetParts.Length == 2)
                {
                    wordOffset = int.Parse(offsetParts[0]);
                    bitOffset = int.Parse(offsetParts[1]);

                    // 获取字值并转换为short
                    //short wordValue = Convert.ToInt16(convertedValues[block.Offset + wordOffset]);
                    short wordValue = Convert.ToInt16(convertedValues[wordOffset * 16 + bitOffset]);

                    // 获取指定位的布尔值
                    bool bitValue = (wordValue & (1 << bitOffset)) != 0;
                    bool oldValue = (bool)item.Value;
                    if (item.Value != null && oldValue != (bitValue))
                    {
                        ProcessBitItem(
                            groupName,
                            block.Type,
                            item,
                            bitValue,
                            block.Offset + wordOffset,
                            bitOffset
                        );
                        hasChanges = true;
                    }
                }
            }
            return currentIndex + wordOffset;
        }

        /// <summary>
        /// 处理数组项的数据变化监控
        /// </summary>
        private int ProcessArrayItemMonitoring(
            string groupName,
            Block block,
            ArrayItem arrayItem,
            ushort[] convertedValues,
            int currentIndex,
            ref bool hasChanges)
        {
            if (arrayItem.Item.Format == "BIT")
            {
                // 解析位偏移量
                string[] offsetParts = arrayItem.Item.Offset.Split(':');
                int baseWordOffset = 0;
                int baseBitOffset = 0;

                if (offsetParts.Length == 2)
                {
                    baseWordOffset = int.Parse(offsetParts[0]);
                    baseBitOffset = int.Parse(offsetParts[1]);

                    for (int i = 0; i < arrayItem.Count; i++)
                    {
                        // 计算实际的位偏移
                        int totalBits = baseBitOffset + i;
                        int actualWordOffset = baseWordOffset + (totalBits / 16);
                        int actualBitOffset = totalBits % 16;

                        // 获取字值并转换为short
                        short wordValue = Convert.ToInt16(convertedValues[actualWordOffset]);

                        // 获取指定位的布尔值
                        bool bitValue = (wordValue & (1 << actualBitOffset)) != 0;

                        if (ShouldNotifyValueChange(arrayItem.Item, bitValue))
                        {
                            ProcessBitItem(
                                groupName,
                                block.Type,
                                arrayItem.Item,
                                bitValue,
                                actualWordOffset,
                                actualBitOffset,
                                i
                            );
                            hasChanges = true;
                        }
                    }
                }
                return currentIndex + arrayItem.Count;
            }

            return currentIndex + arrayItem.Count * arrayItem.Item.Length;
        }

        /// <summary>
        /// 检查Block是否需要监控
        /// </summary>
        /// <param name="block">要检查的Block</param>
        /// <returns>如果需要监控返回true，否则返回false</returns>
        private bool ShouldMonitorBlock(Block block)
        {
            return block.Type.EndsWith("Report", StringComparison.OrdinalIgnoreCase) ||
                   block.Type.EndsWith("Reply", StringComparison.OrdinalIgnoreCase) ||
                   block.Type.EndsWith("Command", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 检查是否需要通知值变化
        /// </summary>
        /// <param name="item">要检查的Item</param>
        /// <param name="value">当前值</param>
        /// <returns>如果需要通知返回true，否则返回false</returns>
        private bool ShouldNotifyValueChange(ItemBase item, object value)
        {
            if (item.Format != "BIT")
                return false;

            // 对于BIT类型，检查值是否发生变化
            if (item.Value == null)
                return true;

            return !AreValuesEqual(item.Value, value);
        }

        /// <summary>
        /// 处理BIT类型的Item数据
        /// </summary>
        /// <param name="groupName">TagGroup名称</param>
        /// <param name="blockType">Block类型</param>
        /// <param name="item">Item对象</param>
        /// <param name="value">当前值</param>
        /// <param name="currentIndex">当前索引</param>
        /// <param name="arrayIndex">数组索引（如果是数组项）</param>
        /// <remarks>
        /// 此方法处理BIT类型数据的读取和事件触发。
        /// 对于数组项，会计算实际的字和位偏移量。
        /// </remarks>
        private void ProcessBitItem(
            string groupName,
            string blockType,
            ItemBase item,
            object value,
            int wordOffset,
            int bitOffset,
            int arrayIndex = -1)
        {
            try
            {
                // 验证偏移量范围
                if (bitOffset < 0 || bitOffset > 15)
                {
                    throw new ArgumentException($"Bit offset {bitOffset} must be between 0 and 15");
                }

                // 更新Item的Value属性
                // item.Value = value;

                // 触发值变化事件
                OnTagValueChanged?.Invoke(this, new TagValueChangedEventArgs(
                    groupName,
                    blockType,
                    item,
                    value,
                    wordOffset,
                    bitOffset,
                    arrayIndex
                ));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing BIT item {item.Name}: {ex.Message}");
            }
        }

        /// <summary>
        /// 读取指定Block的所有数据
        /// </summary>
        /// <param name="groupName">TagGroup名称</param>
        /// <param name="blockType">Block类型</param>
        /// <returns>包含所有Item值的Block对象</returns>
        public Block ReadBlockValues(string groupName, string blockType)
        {
            ValidateNotDisposed();

            try
            {
                // 查找指定的TagGroup
                var group = tagConfig.TagGroups.FirstOrDefault(g =>
                    g.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));

                if (group == null)
                    throw new ArgumentException($"TagGroup '{groupName}' not found");
                // 读取原始数据
                ushort[] rawValues = ReadRawValues(group.Name); //(short[])variableCompolet.ReadVariable(groupName);

                ConvertRawValuesToTagGroup(group, rawValues);

                // 查找指定的Block
                var block = group.Blocks.FirstOrDefault(b =>
                    b.Type.Equals(blockType, StringComparison.OrdinalIgnoreCase));

                if (block == null)
                    throw new ArgumentException($"Block '{blockType}' not found in TagGroup '{groupName}'");



                //}

                return block;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading block values for {groupName}.{blockType}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 读取单个Item的值
        /// </summary>
        /// <param name="groupName">TagGroup名称</param>
        /// <param name="blockType">Block类型</param>
        /// <param name="itemName">Item名称</param>
        /// <returns>Item的值</returns>
        public object ReadItemValue(string groupName, string blockType, string itemName)
        {
            ValidateNotDisposed();

            try
            {
                // 查找指定的TagGroup和Block
                var group = tagConfig.TagGroups.FirstOrDefault(g =>
                    g.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));

                if (group == null)
                    throw new ArgumentException($"TagGroup '{groupName}' not found");

                var block = group.Blocks.FirstOrDefault(b =>
                    b.Type.Equals(blockType, StringComparison.OrdinalIgnoreCase));

                if (block == null)
                    throw new ArgumentException($"Block '{blockType}' not found in TagGroup '{groupName}'");

                // 读取当前值
                var rawValues = ReadRawValues(groupName);
                int currentIndex = block.Offset;
                // 查找并读取指定的Item

                ConvertRawValuesToTagGroup(group, rawValues);

                //
                var item = block.Items.FirstOrDefault(i =>
                    i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));

                if (item == null)
                    throw new ArgumentException($"Item '{itemName}' not found in block '{blockType}'");
                return item.Value;

                //// 查找并读取指定的Item
                //foreach (var item in block.Items)
                //{
                //    if (item is ArrayItem arrayItem)
                //    {
                //        if (arrayItem.Name == itemName)
                //        {
                //            if (arrayItem.Item.Format == "BIT")
                //            {
                //                var boolArray = new bool[arrayItem.Count];
                //                string[] offsetParts = arrayItem.Item.Offset.Split(':');
                //                if (offsetParts.Length == 2)
                //                {
                //                    int wordOffset = int.Parse(offsetParts[0]);
                //                    int bitOffset = int.Parse(offsetParts[1]);

                //                    for (int i = 0; i < arrayItem.Count; i++)
                //                    {
                //                        int totalBits = bitOffset + i;
                //                        int actualWordOffset = wordOffset + (totalBits / 16);
                //                        int actualBitOffset = totalBits % 16;
                //                        boolArray[i] = (rawValues[actualWordOffset] & (1 << actualBitOffset)) != 0;
                //                    }
                //                }
                //                arrayItem.Item.Value = boolArray;
                //                return boolArray;
                //            }
                //            else
                //            {
                //                var values = new object[arrayItem.Count];
                //                for (int i = 0; i < arrayItem.Count; i++)
                //                {
                //                    values[i] = ConvertValueByFormat(
                //                        arrayItem.Item.Format,
                //                        rawValues,
                //                        currentIndex + i * arrayItem.Item.Length,
                //                        arrayItem.Item.Length
                //                    );
                //                }
                //                arrayItem.Item.Value = values;
                //                return values;
                //            }
                //        }
                //        currentIndex += arrayItem.Count * arrayItem.Item.Length;
                //    }
                //    else if (item.Name == itemName)
                //    {
                //        if (item.Format == "BIT")
                //        {
                //            string[] offsetParts = item.Offset.Split(':');
                //            if (offsetParts.Length == 2)
                //            {
                //                int wordOffset = int.Parse(offsetParts[0]);
                //                int bitOffset = int.Parse(offsetParts[1]);
                //                bool value = (rawValues[wordOffset] & (1 << bitOffset)) != 0;
                //                item.Value = value;
                //                return value;
                //            }
                //        }
                //        else
                //        {
                //            var value = ConvertValueByFormat(item.Format, rawValues, currentIndex, item.Length);
                //            item.Value = value;
                //            return value;
                //        }
                //    }
                //    currentIndex += item is ArrayItem arr ? arr.Count * arr.Item.Length : item.Length;
                //}

                //throw new ArgumentException($"Item '{itemName}' not found in block '{blockType}'");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading item value: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 写入单个Item的值
        /// </summary>
        /// <param name="groupName">TagGroup名称</param>
        /// <param name="blockType">Block类型</param>
        /// <param name="itemName">Item名称</param>
        /// <param name="value">要写入的值</param>
        public void WriteItemValue(string groupName, string blockType, string itemName, object value)
        {
            ValidateNotDisposed();

            try
            {
                // 查找指定的TagGroup和Block
                var group = tagConfig.TagGroups.FirstOrDefault(g =>
                    g.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));

                if (group == null)
                    throw new ArgumentException($"TagGroup '{groupName}' not found");

                var block = group.Blocks.FirstOrDefault(b =>
                    b.Type.Equals(blockType, StringComparison.OrdinalIgnoreCase));

                if (block == null)
                    throw new ArgumentException($"Block '{blockType}' not found in TagGroup '{groupName}'");

                // 读取当前值
                // short[] rawValues = (short[])variableCompolet.ReadVariable(groupName);
                ushort[] rawValues = ReadRawValues(groupName);
                int currentIndex = block.Offset;

                // 查找并更新指定的Item
                bool itemFound = false;
                int wordOffset = 0;
                foreach (var item in block.Items)
                {
                    if (item is ArrayItem arrayItem && arrayItem.Name == itemName)
                    {
                        itemFound = true;
                        if (arrayItem.Item.Format == "BIT")
                        {
                            if (value is bool[] boolArray)
                            {
                                if (boolArray.Length != arrayItem.Count)
                                    throw new ArgumentException($"Array length mismatch. Expected {arrayItem.Count}, got {boolArray.Length}");

                                string[] offsetParts = arrayItem.Item.Offset.Split(':');
                                if (offsetParts.Length == 2)
                                {
                                    wordOffset = int.Parse(offsetParts[0]);
                                    int bitOffset = int.Parse(offsetParts[1]);

                                    for (int i = 0; i < arrayItem.Count; i++)
                                    {
                                        int totalBits = bitOffset + i;
                                        int actualWordOffset = wordOffset + (totalBits / 16);
                                        int actualBitOffset = totalBits % 16;

                                        if (boolArray[i])
                                            rawValues[actualWordOffset] |= (ushort)(1 << actualBitOffset);
                                        else
                                            rawValues[actualWordOffset] &= (ushort)~(1 << actualBitOffset);
                                    }
                                }
                            }
                            else
                                throw new ArgumentException("Value must be bool[] for BIT array");
                        }
                        else
                        {
                            if (value is object[] values)
                            {
                                if (values.Length != arrayItem.Count)
                                    throw new ArgumentException($"Array length mismatch. Expected {arrayItem.Count}, got {values.Length}");

                                for (int i = 0; i < arrayItem.Count; i++)
                                {
                                    WriteValueToRawData(
                                        arrayItem.Item.Format,
                                        values[i],
                                        rawValues,
                                        currentIndex + i * arrayItem.Item.Length, arrayItem.Item.Length
                                    );
                                }
                                arrayItem.Item.Value = values;
                            }
                            else
                                throw new ArgumentException("Value must be array for array item");
                        }
                        break;
                    }
                    else if (item.Name == itemName)
                    {
                        lock (rawValues)
                        {
                            itemFound = true;

                            if (item.Format == "BIT"&& item.Offset.Split(':').Length == 2)
                            {
                                string[] offsetParts = item.Offset.Split(':');

                                int owordOffset = int.Parse(offsetParts[0]);

                                if (owordOffset != wordOffset)
                                {
                                    currentIndex = block.Offset + owordOffset;
                                    wordOffset = owordOffset;
                                }
                            }
                            else
                            {
                                currentIndex = block.Offset + int.Parse(item.Offset);
                            }

                            if (item.Format == "BIT"&& item.Offset.Split(':').Length == 2)
                            {
                                string[] offsetParts = item.Offset.Split(':');
                                if (offsetParts.Length == 2)
                                {
                                    wordOffset = int.Parse(offsetParts[0]);

                                    
                                    int bitOffset = int.Parse(offsetParts[1]);

                                    bool bitValue = Convert.ToBoolean(value);

                                    var offset = wordOffset * 16 + bitOffset;

                                    if (bitValue)
                                        rawValues[offset] |= (ushort)(1 << bitOffset);
                                    else
                                        rawValues[offset] &= (ushort)~(1 << bitOffset);
                                    item.Value = bitValue;
                                }
                            }
                            else
                            {
                                WriteValueToRawData(item.Format, value, rawValues, currentIndex, item.Length);
                                item.Value = value;
                            }
                            break;
                        }
                    }
                    // currentIndex += item is ArrayItem arr ? arr.Count * arr.Item.Length : item.Length;
                }

                if (!itemFound)
                    throw new ArgumentException($"Item '{itemName}' not found in block '{blockType}'");

                // 写回PLC
                variableCompolet.WriteVariable(groupName, rawValues);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error writing item value: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 写入整个Block的数据
        /// </summary>
        /// <param name="groupName">TagGroup名称</param>
        /// <param name="block">要写入的Block对象</param>
        public void WriteBlockValues(string groupName, Block block)
        {
            ValidateNotDisposed();

            try
            {
                // 查找指定的TagGroup
                var group = tagConfig.TagGroups.FirstOrDefault(g =>
                    g.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));

                if (group == null)
                    throw new ArgumentException($"TagGroup '{groupName}' not found");

                // 验证Block是否属于该TagGroup
                var existingBlock = group.Blocks.FirstOrDefault(b =>
                    b.Type.Equals(block.Type, StringComparison.OrdinalIgnoreCase));

                if (existingBlock == null)
                    throw new ArgumentException($"Block '{block.Type}' not found in TagGroup '{groupName}'");

                // 读取当前值
                var rawValues = ReadRawValues(groupName);
                int currentIndex = block.Offset;

                // 更新Block中的所有Items
                foreach (var item in block.Items)
                {
                    if (item is ArrayItem arrayItem)
                    {
                        if (arrayItem.Item.Format == "BIT")
                        {
                            if (arrayItem.Item.Value is bool[] boolArray)
                            {
                                string[] offsetParts = arrayItem.Item.Offset.Split(':');
                                if (offsetParts.Length == 2)
                                {
                                    int wordOffset = int.Parse(offsetParts[0]);
                                    int bitOffset = int.Parse(offsetParts[1]);

                                    for (int i = 0; i < arrayItem.Count; i++)
                                    {
                                        int totalBits = bitOffset + i;
                                        int actualWordOffset = wordOffset + (totalBits / 16);
                                        int actualBitOffset = totalBits % 16;

                                        if (boolArray[i])
                                            rawValues[actualWordOffset] |= (ushort)(1 << actualBitOffset);
                                        else
                                            rawValues[actualWordOffset] &= (ushort)~(1 << actualBitOffset);
                                    }
                                }
                            }
                            currentIndex += arrayItem.Count;
                        }
                        else
                        {
                            if (arrayItem.Item.Value is object[] values)
                            {
                                for (int i = 0; i < arrayItem.Count && i < values.Length; i++)
                                {
                                    WriteValueToRawData(
                                        arrayItem.Item.Format,
                                        values[i],
                                        rawValues,
                                        currentIndex + i * arrayItem.Item.Length, arrayItem.Item.Length
                                    );
                                }
                            }
                            currentIndex += arrayItem.Count * arrayItem.Item.Length;
                        }
                    }
                    else
                    {
                        if (item.Format == "BIT"&& item.Offset.Split(':').Length == 2)
                        {
                            string[] offsetParts = item.Offset.Split(':');
                            if (item.Value != null)
                            {
                               // string[] offsetParts = item.Offset.Split(':');
                                if (offsetParts.Length == 2)
                                {
                                    int wordOffset = int.Parse(offsetParts[0]);
                                    int bitOffset = int.Parse(offsetParts[1]);

                                    bool bitValue = Convert.ToBoolean(item.Value);
                                    if (bitValue)
                                        rawValues[block.Offset + wordOffset] |= (ushort)(1 << bitOffset);
                                    else
                                        rawValues[block.Offset + wordOffset] &= (ushort)~(1 << bitOffset);

                                    currentIndex = block.Offset + int.Parse(offsetParts[0]);
                                }
                            }
                         
                        }
                        else
                        {
                            if (item.Value != null)
                            {
                               
                                WriteValueToRawData(
                                    item.Format,
                                    item.Value,
                                    rawValues,
                                    currentIndex,
                                    item.Length
                                );
                            }
                            currentIndex += item.Length;
                        }
                    }
                }

                // 写回PLC
                variableCompolet.WriteVariable(groupName, rawValues);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error writing block values for {groupName}.{block.Type}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 将值写入原始数据数组
        /// </summary>
        /// <param name="format">数据格式</param>
        /// <param name="value">要写入的值</param>
        /// <param name="rawValues">原始数据数组</param>
        /// <param name="offset">写入位置的偏移量</param>
        /// <exception cref="NotSupportedException">
        /// 当指定了不支持的数据格式时抛出
        /// </exception>
        /// <remarks>
        /// 支持的格式包括：
        /// - INT/SINT：写入16位整数
        /// - FLOAT：写入32位浮点数
        /// - ASCII：写入ASCII字符串
        /// - BIT：写入布尔值
        /// </remarks>
        private void WriteValueToRawData(string format, object value, ushort[] rawValues, int offset)
        {
            switch (format)
            {
                case "INT":
                case "SINT":
                    rawValues[offset] = Convert.ToUInt16(value);
                    break;

                case "FLOAT":
                    var bytes = BitConverter.GetBytes(Convert.ToSingle(value));
                    rawValues[offset] = BitConverter.ToUInt16(bytes, 0);
                    rawValues[offset + 1] = BitConverter.ToUInt16(bytes, 2);
                    break;

                case "ASCII":
                    string strValue = value.ToString();
                    for (int i = 0; i < strValue.Length && i < rawValues.Length * 2; i += 2)
                    {
                        ushort charValue = 0;
                        charValue |= (ushort)(strValue[i] & 0xFF);
                        if (i + 1 < strValue.Length)
                            charValue |= (ushort)((strValue[i + 1] & 0xFF) << 8);
                        rawValues[offset + i / 2] = charValue;
                    }
                    break;

                default:
                    throw new NotSupportedException($"Unsupported format: {format}");
            }
        }
        private void WriteValueToRawData(string format, object value, ushort[] rawValues, int offset, int length)
        {
            // 清空从offset到offset + length的数据
            for (int i = offset; i < offset + length && i < rawValues.Length; i++)
            {
                rawValues[i] = 0;
            }

            switch (format)
            {
                case "BIN":
                    if (length < 1)
                    {
                        throw new ArgumentException("length must be at least 1 for BIT format", nameof(length));
                    }
                    if (!(value is string bitString) || bitString.Length < length)
                    {
                        throw new ArgumentException("value must be a valid binary string of at least length", nameof(value));
                    }
                    // 确保二进制字符串长度是16的倍数
                    if (bitString.Length % 16 != 0)
                    {
                        throw new ArgumentException("二进制字符串长度必须是16的倍数");
                    }

                    // 计算需要解析的 ushort 数量
                    int ushortCount = bitString.Length / 16;



                    int j = 0;
                    for (int i = offset; i < offset + length; i++)
                    {
                        string sub = bitString.Substring(j * 16, 16);
                        rawValues[i] = Convert.ToUInt16(sub, 2);
                        j++;
                    }

                    break;

                case "INT":
                case "SINT":
                    if (length < 1)
                    {
                        throw new ArgumentException("length must be at least 1 for INT or SINT format", nameof(length));
                    }
                    if (offset >= rawValues.Length)
                    {
                        throw new ArgumentOutOfRangeException("offset", "指定的偏移量超出了 rawValues 的范围");
                    }
                    rawValues[offset] = Convert.ToUInt16(value);
                    break;

                case "FLOAT":
                    if (length < 2)
                    {
                        throw new ArgumentException("length must be at least 2 for FLOAT format", nameof(length));
                    }
                    if (offset + 1 >= rawValues.Length)
                    {
                        throw new ArgumentOutOfRangeException("offset", "指定的偏移量超出了 rawValues 的范围");
                    }
                    var floatBytes = BitConverter.GetBytes(Convert.ToSingle(value));
                    rawValues[offset] = BitConverter.ToUInt16(floatBytes, 0);
                    rawValues[offset + 1] = BitConverter.ToUInt16(floatBytes, 2);
                    break;

                case "DOUBLE":
                    if (length < 4)
                    {
                        throw new ArgumentException("length must be at least 4 for DOUBLE format", nameof(length));
                    }
                    if (offset + 3 >= rawValues.Length)
                    {
                        throw new ArgumentOutOfRangeException("offset", "指定的偏移量超出了 rawValues 的范围");
                    }
                    var doubleBytes = BitConverter.GetBytes(Convert.ToDouble(value));
                    rawValues[offset] = BitConverter.ToUInt16(doubleBytes, 0);
                    rawValues[offset + 1] = BitConverter.ToUInt16(doubleBytes, 2);
                    rawValues[offset + 2] = BitConverter.ToUInt16(doubleBytes, 4);
                    rawValues[offset + 3] = BitConverter.ToUInt16(doubleBytes, 6);
                    break;

                case "LONG":
                    if (length < 4)
                    {
                        throw new ArgumentException("length must be at least 4 for LONG format", nameof(length));
                    }
                    if (offset + 3 >= rawValues.Length)
                    {
                        throw new ArgumentOutOfRangeException("offset", "指定的偏移量超出了 rawValues 的范围");
                    }
                    var longBytes = BitConverter.GetBytes(Convert.ToInt64(value));
                    rawValues[offset] = BitConverter.ToUInt16(longBytes, 0);
                    rawValues[offset + 1] = BitConverter.ToUInt16(longBytes, 2);
                    rawValues[offset + 2] = BitConverter.ToUInt16(longBytes, 4);
                    rawValues[offset + 3] = BitConverter.ToUInt16(longBytes, 6);
                    break;

                case "ASCII":
                    string strValue = value.ToString();
                    int maxChars = Math.Min(length * 2, strValue.Length);
                    for (int i = 0; i < maxChars; i += 2)
                    {
                        ushort charValue = 0;
                        charValue |= (ushort)(strValue[i] & 0xFF);
                        if (i + 1 < maxChars)
                        {
                            charValue |= (ushort)((strValue[i + 1] & 0xFF) << 8);
                        }
                        if (offset + i / 2 < rawValues.Length)
                        {
                            rawValues[offset + i / 2] = charValue;
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("offset", "指定的偏移量超出了 rawValues 的范围");
                        }
                    }
                    break;

                default:
                    throw new NotSupportedException($"Unsupported format: {format}");
            }
        }


        // 添加公共属性
        public TagConfig TagConfig => tagConfig;
    }

    internal class TagAddressInfo
    {
        public TagGroup TagGroup { get; set; }
        public Block Block { get; set; }
        public ItemBase Item { get; set; }
        public string Address { get; set; }
    }

    [XmlRoot("TagConfig")]
    public class TagConfig
    {
        [XmlArray("TagGroups")]
        [XmlArrayItem("TagGroup")]
        public List<TagGroup> TagGroups { get; set; }
    }

    public class TagGroup
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Size")]
        public int Size { get; set; }

        [XmlAttribute("Direction")]
        public string Direction { get; set; }

        [XmlArray("Blocks")]
        [XmlArrayItem("Block")]
        public List<Block> Blocks { get; set; }
    }

    public class Block
    {
        [XmlAttribute("Type")]
        public string Type { get; set; }

        [XmlAttribute("Size")]
        public int Size { get; set; }

        [XmlAttribute("Index")]
        public int Index { get; set; }

        [XmlAttribute("Offset")]
        public int Offset { get; set; }

        [XmlArray("Items")]
        [XmlArrayItem("SimpleItem", typeof(SimpleItem))]
        [XmlArrayItem("ArrayItem", typeof(ArrayItem))]
        public List<ItemBase> Items { get; set; } = new List<ItemBase>();

        // 修改索引器，添加set访问器
        public ItemBase this[string itemName]
        {
            get
            {
                var item = Items.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
                if (item == null)
                {
                    throw new KeyNotFoundException($"Item '{itemName}' not found in block '{Type}'");
                }
                return item;
            }
            set
            {
                var index = Items.FindIndex(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
                if (index == -1)
                {
                    throw new KeyNotFoundException($"Item '{itemName}' not found in block '{Type}'");
                }

                // 验证类型匹配
                if (Items[index].GetType() != value.GetType())
                {
                    throw new ArgumentException($"Cannot assign {value.GetType().Name} to {Items[index].GetType().Name}");
                }

                Items[index] = value;
            }
        }
        // 基于整数索引的索引器
        public ItemBase this[int index]
        {
            get
            {
                if (index < 0 || index >= Items.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), $"Index '{index}' is out of range in block '{Type}'");
                }
                return Items[index];
            }
            set
            {
                if (index < 0 || index >= Items.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), $"Index '{index}' is out of range in block '{Type}'");
                }

                Items[index] = value;
            }
        }
    }

    [XmlInclude(typeof(SimpleItem))]
    [XmlInclude(typeof(ArrayItem))]
    public abstract class ItemBase
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Area")]
        public string Area { get; set; }

        [XmlAttribute("Length")]
        public int Length { get; set; }

        [XmlAttribute("Format")]
        public string Format { get; set; }

        [XmlAttribute("Offset")]
        public string Offset { get; set; }

        [XmlIgnore]
        public object Value { get; set; }
    }

    public class SimpleItem : ItemBase
    {
    }

    public class ArrayItem : ItemBase
    {
        [XmlAttribute("BaseOffset")]
        public int BaseOffset { get; set; }

        [XmlAttribute("Count")]
        public int Count { get; set; }

        [XmlElement("SimpleItem")]
        public SimpleItem Item { get; set; }
    }
}