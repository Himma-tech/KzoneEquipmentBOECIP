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
        private Dictionary<string, short[]> lastValues;

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
            lastValues = new Dictionary<string, short[]>();
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

            try {
                while (!cancellationTokenSource.Token.IsCancellationRequested) {
                    var inputGroups = tagConfig.TagGroups
                        .Where(g => g.Direction.Equals("Output", StringComparison.OrdinalIgnoreCase)
                        || g.Direction.Equals("Input", StringComparison.OrdinalIgnoreCase));

                    foreach (var group in inputGroups) {
                        try {
                            // 读取当前值
                            var rawValues = ReadRawValues(group.Name);
                            //  var convertedValues = ConvertRawValuesToTagGroup(group, rawValues);

                            short[] lastRawValues;
                            if (lastValues.ContainsKey(group.Name)) {
                                lastRawValues = (short[])lastValues[group.Name].Clone();
                            }
                            else {
                                lastRawValues = new short[rawValues.Length];
                            }

                            // 检查值变化并触发事件
                            if (HasValuesChanged(group.Name, rawValues)) {

                                ConvertRawValuesToTagGroup(group, rawValues);
                                ProcessTagGroupMonitoring(group, lastRawValues);

                                lastValues[group.Name] = rawValues;
                            }

                        }
                        catch (Exception ex) {
                            Console.WriteLine($"Error reading TagGroup {group.Name}: {ex.Message}");
                        }
                    }

                    await Task.Delay(intervalMs, cancellationTokenSource.Token);
                }
            }
            catch (OperationCanceledException) {
                // 正常取消，不需要处理
            }
            catch (Exception ex) {
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
        /// 从PLC读取并转换原始数据为short数组，保证字节顺序和位准确，支持W区ASCII类型
        /// </summary>
        /// <param name="groupName">TagGroup名称</param>
        /// <returns>转换后的short数组</returns>
        /// <exception cref="NotSupportedException">当读取到不支持的数据类型时抛出</exception>
        /// <exception cref="OverflowException">当数值转换发生溢出时抛出</exception>
        private short[] ReadRawValues(string groupName)
        {
            var obj = variableCompolet.ReadVariable(groupName);

            if (obj == null)
                throw new Exception($"ReadVariable returned null for group '{groupName}'");

            // 先处理单独的short[]类型
            if (obj is short[] shortArray) {
                return shortArray;
            }

            // 处理单个short值（如读取单个INT）
            if (obj is short shortValue) {
                return new short[] { shortValue };
            }

            // 处理ushort[]数组
            if (obj is ushort[] ushortArray) {
                return Array.ConvertAll(ushortArray, item => unchecked((short)item));
            }

            // 处理单个ushort值
            if (obj is ushort ushortValue) {
                return new short[] { unchecked((short)ushortValue) };
            }

            // 处理int[]数组，直接截断为short（谨慎使用，确认标签类型）
            if (obj is int[] intArray) {
                var result = new short[intArray.Length];
                for (int i = 0; i < intArray.Length; i++) {
                    result[i] = unchecked((short)intArray[i]);
                }
                return result;
            }

            // 处理单个int值
            if (obj is int intValue) {
                return new short[] { unchecked((short)intValue) };
            }

            // 处理字符串（ASCII）类型：转换字符串为对应short数组
            if (obj is string str) {
                // 这里用Encoding.ASCII编码，也可以改成Encoding.UTF8或其他需要的编码
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(str);

                int shortCount = (bytes.Length + 1) / 2;
                short[] result = new short[shortCount];

                for (int i = 0; i < shortCount; i++) {
                    // 低字节
                    byte low = bytes[i * 2];
                    // 高字节，如果越界则补0
                    byte high = (i * 2 + 1) < bytes.Length ? bytes[i * 2 + 1] : (byte)0;

                    // 组合成short，低字节在低位，高字节在高位
                    result[i] = (short)((high << 8) | low);
                }

                return result;
            }

            // 处理数组类型
            if (obj.GetType().IsArray) {
                var array = (Array)obj;
                Type elementType = array.GetType().GetElementType();

                if (elementType == typeof(short)) {
                    return (short[])array;
                }
                else if (elementType == typeof(ushort)) {
                    ushort[] uArr = (ushort[])array;
                    return Array.ConvertAll(uArr, item => unchecked((short)item));
                }
                else if (elementType == typeof(int)) {
                    int[] iArr = (int[])array;
                    var result = new short[iArr.Length];
                    for (int i = 0; i < iArr.Length; i++) {
                        result[i] = unchecked((short)iArr[i]);
                    }
                    return result;
                }
                else if (elementType == typeof(uint)) {
                    uint[] uIntArr = (uint[])array;
                    var result = new short[uIntArr.Length * 2];
                    for (int i = 0; i < uIntArr.Length; i++) {
                        uint val = uIntArr[i];
                        result[2 * i] = unchecked((short)(val & 0xFFFF));
                        result[2 * i + 1] = unchecked((short)((val >> 16) & 0xFFFF));
                    }
                    return result;
                }
                else if (elementType == typeof(float)) {
                    float[] fArr = (float[])array;
                    var result = new short[fArr.Length * 2];
                    for (int i = 0; i < fArr.Length; i++) {
                        byte[] bytes = BitConverter.GetBytes(fArr[i]);
                        result[2 * i] = BitConverter.ToInt16(bytes, 0);
                        result[2 * i + 1] = BitConverter.ToInt16(bytes, 2);
                    }
                    return result;
                }
                else if (elementType == typeof(double)) {
                    double[] dArr = (double[])array;
                    var result = new short[dArr.Length * 4];
                    for (int i = 0; i < dArr.Length; i++) {
                        byte[] bytes = BitConverter.GetBytes(dArr[i]);
                        result[4 * i] = BitConverter.ToInt16(bytes, 0);
                        result[4 * i + 1] = BitConverter.ToInt16(bytes, 2);
                        result[4 * i + 2] = BitConverter.ToInt16(bytes, 4);
                        result[4 * i + 3] = BitConverter.ToInt16(bytes, 6);
                    }
                    return result;
                }
                else if (elementType == typeof(bool)) {
                    bool[] bArr = (bool[])array;
                    int shortCount = (bArr.Length + 15) / 16;
                    var result = new short[shortCount];
                    for (int i = 0; i < bArr.Length; i++) {
                        if (bArr[i]) {
                            int wordIndex = i / 16;
                            int bitIndex = i % 16;
                            result[wordIndex] |= (short)(1 << bitIndex);
                        }
                    }
                    return result;
                }
                else if (elementType == typeof(string)) {
                    // 字符串数组，拼接所有字符串后转换
                    var sb = new System.Text.StringBuilder();
                    foreach (string s in array) {
                        sb.Append(s);
                    }
                    string allStr = sb.ToString();
                    byte[] bytes = System.Text.Encoding.ASCII.GetBytes(allStr);
                    int shortCount = (bytes.Length + 1) / 2;
                    short[] result = new short[shortCount];
                    for (int i = 0; i < shortCount; i++) {
                        byte low = bytes[i * 2];
                        byte high = (i * 2 + 1) < bytes.Length ? bytes[i * 2 + 1] : (byte)0;
                        result[i] = (short)((high << 8) | low);
                    }
                    return result;
                }
                else {
                    throw new NotSupportedException($"Unsupported array element type: {elementType.Name}");
                }
            }

            throw new NotSupportedException($"Unsupported data type: {obj.GetType().Name}");
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
            for (int i = 0; i < arrayItem.Count; i++) {
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
            if (format == "BIT" && bitOffset >= 0) {
                address += $".{bitOffset}";
            }
            return address;
        }

        public T ReadTag<T>(string tagAddress)
        {
            ValidateNotDisposed();

            if (!tagAddressCache.TryGetValue(tagAddress, out var tagInfo)) {
                throw new ArgumentException($"Tag address '{tagAddress}' not found in configuration");
            }

            try {
                object value = variableCompolet.ReadVariable(tagInfo.Address);
                return ConvertValue<T>(value, tagInfo.Item.Format);
            }
            catch (Exception ex) {
                throw new Exception($"Failed to read tag '{tagAddress}' at address '{tagInfo.Address}': {ex.Message}", ex);
            }
        }

        public void WriteTag<T>(string tagAddress, T value)
        {
            ValidateNotDisposed();

            if (!tagAddressCache.TryGetValue(tagAddress, out var tagInfo)) {
                throw new ArgumentException($"Tag address '{tagAddress}' not found in configuration");
            }

            try {
                object convertedValue = ConvertToTargetType(value, tagInfo.Item.Format);
                variableCompolet.WriteVariable(tagInfo.Address, convertedValue);
            }
            catch (Exception ex) {
                throw new Exception($"Failed to write tag '{tagAddress}' at address '{tagInfo.Address}': {ex.Message}", ex);
            }
        }

        private T ConvertValue<T>(object value, string format)
        {
            try {
                switch (format) {
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
            catch (Exception ex) {
                throw new Exception($"Failed to convert value from format '{format}' to type '{typeof(T).Name}'", ex);
            }
        }

        private object ConvertToTargetType(object value, string format)
        {
            try {
                switch (format) {
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
            catch (Exception ex) {
                throw new Exception($"Failed to convert value to format '{format}'", ex);
            }
        }

        /// <summary>
        /// 将原始short数组转换为TagGroup中各Item对应的值，支持BIT、数组及多种格式
        /// </summary>
        /// <param name="group">TagGroup对象</param>
        /// <param name="rawValues">PLC读取到的原始short数组</param>
        /// <returns>转换后的对象数组（长度为TagGroup.Size）</returns>
        private object[] ConvertRawValuesToTagGroup(TagGroup group, object rawValues)
        {
            var result = new object[group.Size];

            foreach (var block in group.Blocks) {
                int blockBaseOffset = block.Offset;

                foreach (var item in block.Items) {
                    int baseOffset = blockBaseOffset + int.Parse(item.Offset);

                    if (item.Format.ToUpper() == "BIT") {
                        string[] parts = item.Offset.Split(':');
                        if (parts.Length != 2)
                            throw new ArgumentException($"Invalid BIT offset for {item.Name}");

                        int wordOffsetBase = int.Parse(parts[0]);
                        int bitOffsetBase = int.Parse(parts[1]);

                        bool bitValue = ReadBitValue(rawValues, blockBaseOffset + wordOffsetBase, bitOffsetBase);
                        item.Value = bitValue;
                        result[blockBaseOffset + wordOffsetBase] = bitValue;
                    }
                    else {
                        object val = ReadValueByFormat(rawValues, item.Format, baseOffset, item.Length);
                        item.Value = val;
                        result[baseOffset] = val;
                    }
                }
            }

            return result;
        }
        private object ReadValueByFormat(object rawValues, string format, int absoluteWordOffset, int length)
        {
            // length单位为short字数量

            if (rawValues is short[] shortArray) {
                if (absoluteWordOffset + length > shortArray.Length)
                    throw new IndexOutOfRangeException("shortArray length exceeded");

                switch (format.ToUpper()) {
                    case "INT":
                    case "WORD":
                        return shortArray[absoluteWordOffset];
                    case "DINT":
                    case "DWORD":
                        // 2个short合成int，低位字在前
                        int low = (ushort)shortArray[absoluteWordOffset];
                        int high = (ushort)shortArray[absoluteWordOffset + 1];
                        return (high << 16) | low;
                    case "REAL": {
                            // 2个short转float
                            byte[] bytes = new byte[4];
                            Buffer.BlockCopy(shortArray, absoluteWordOffset * 2, bytes, 0, 4);
                            return BitConverter.ToSingle(bytes, 0);
                        }
                    // 增加其他格式处理...
                    default:
                        throw new NotSupportedException($"Format {format} not supported for short[]");
                }
            }
            else if (rawValues is int[] intArray) {
                int intIndex = absoluteWordOffset / 2;
                int shortInInt = absoluteWordOffset % 2;

                if (intIndex >= intArray.Length)
                    throw new IndexOutOfRangeException("intArray length exceeded");

                switch (format.ToUpper()) {
                    case "INT": {
                            // 取对应16位
                            int val = intArray[intIndex];
                            ushort wordValue = (shortInInt == 0) ? (ushort)(val & 0xFFFF) : (ushort)((val >> 16) & 0xFFFF);
                            return (short)wordValue;
                        }
                    case "DINT": {
                            if (shortInInt != 0)
                                throw new ArgumentException("DINT读取必须从偶数字边界开始");
                            return intArray[intIndex];
                        }
                    case "REAL": {
                            if (shortInInt != 0)
                                throw new ArgumentException("REAL读取必须从偶数字边界开始");
                            byte[] bytes = BitConverter.GetBytes(intArray[intIndex]);
                            return BitConverter.ToSingle(bytes, 0);
                        }
                    // 其他格式根据需求添加
                    default:
                        throw new NotSupportedException($"Format {format} not supported for int[]");
                }
            }
            else {
                throw new NotSupportedException($"Unsupported rawValues type for format read: {rawValues.GetType().Name}");
            }
        }
        /// <summary>
        /// 读取单个位的方法 
        /// </summary>
        /// <param name="rawValues"></param>
        /// <param name="absoluteWordOffset"></param>
        /// <param name="bitOffset"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        private bool ReadBitValue(object rawValues, int absoluteWordOffset, int bitOffset)
        {
            if (rawValues is short[] shortArray) {
                if (absoluteWordOffset >= shortArray.Length)
                    throw new IndexOutOfRangeException($"shortArray index {absoluteWordOffset} out of range");
                return (shortArray[absoluteWordOffset] & (1 << bitOffset)) != 0;
            }
            else if (rawValues is ushort[] ushortArray) {
                if (absoluteWordOffset >= ushortArray.Length)
                    throw new IndexOutOfRangeException($"ushortArray index {absoluteWordOffset} out of range");
                return (ushortArray[absoluteWordOffset] & (1 << bitOffset)) != 0;
            }
            else if (rawValues is int[] intArray) {
                int intIndex = absoluteWordOffset / 2;
                int shortInInt = absoluteWordOffset % 2;
                if (intIndex >= intArray.Length)
                    throw new IndexOutOfRangeException($"intArray index {intIndex} out of range");

                int val = intArray[intIndex];
                int wordValue = (shortInInt == 0) ? (val & 0xFFFF) : ((val >> 16) & 0xFFFF);
                return (wordValue & (1 << bitOffset)) != 0;
            }
            else {
                throw new NotSupportedException($"Unsupported rawValues type for bit read: {rawValues.GetType().Name}");
            }
        }

        private void ProcessArrayItem(ArrayItem arrayItem, short[] rawValues, object[] result, ref int currentIndex)
        {
            if (arrayItem.Item.Format == "BIT") {
                string[] offsetParts = arrayItem.Item.Offset.Split(':');
                if (offsetParts.Length == 2) {
                    int wordOffset = int.Parse(offsetParts[0]);
                    int bitOffset = int.Parse(offsetParts[1]);

                    for (int i = 0; i < arrayItem.Count; i++) {
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
            else {
                for (int i = 0; i < arrayItem.Count; i++) {
                    var value = ConvertValueByFormat(arrayItem.Item.Format, rawValues, currentIndex, arrayItem.Item.Length);
                    result[currentIndex] = value;
                    arrayItem.Item.Value = value;  // 更新Value属性
                    currentIndex += arrayItem.Item.Length;
                }
            }
        }

        private void ProcessSingleItem(ItemBase item, short[] rawValues, object[] result, ref int currentIndex)
        {
            if (item.Format == "BIT") {
                if (currentIndex == 0) {

                }
                int wordOffset = 0;
                int bitOffset = 0;
                string[] offsetParts = item.Offset.Split(':');
                if (offsetParts.Length == 2) {
                    wordOffset = int.Parse(offsetParts[0]);
                    bitOffset = int.Parse(offsetParts[1]);

                    // 读取位值并更新Value属性
                    bool value = (rawValues[currentIndex] & (1 << bitOffset)) != 0;
                    result[currentIndex] = value;
                    item.Value = value;  // 更新Value属性
                }
                // currentIndex = currentIndex+wordOffset;
            }
            else {
                if (currentIndex == 641) {

                }

                var value = ConvertValueByFormat(item.Format, rawValues, currentIndex, item.Length);
                result[currentIndex] = value;
                item.Value = value;  // 更新Value属性
                currentIndex += item.Length;
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
        private object ConvertValueByFormat(string format, short[] rawValues, int offset, int length, string offsetStr = null)
        {
            switch (format) {
                case "BIT":
                    if (string.IsNullOrEmpty(offsetStr))
                        return (rawValues[offset / 16] & (1 << (offset % 16))) != 0;
                    return ConvertBitValue(offsetStr, rawValues);

                case "INT":
                case "SINT":
                    if (length == 1)
                        return (int)rawValues[offset];
                    else if (length == 2)
                        return (int)((rawValues[offset + 1] << 16) | (rawValues[offset] & 0xFFFF));

                    break;

                case "FLOAT":
                    if (length == 2) {
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
                    for (int i = 0; i < length; i++) {
                        chars[i * 2] = (char)(rawValues[offset + i] & 0xFF);
                        chars[i * 2 + 1] = (char)(rawValues[offset + i] >> 8);
                    }
                    return new string(chars).TrimEnd('\0');
            }

            throw new NotSupportedException($"Unsupported format: {format}");
        }
        /// <summary>
        /// 读取位值辅助方法
        /// </summary>
        private bool ConvertBitValue(string offsetStr, short[] rawValues)
        {
            var parts = offsetStr.Split(':');
            if (parts.Length != 2)
                throw new ArgumentException($"Invalid BIT offset format: {offsetStr}");

            int wordOffset = int.Parse(parts[0]);
            int bitOffset = int.Parse(parts[1]);

            if (wordOffset >= rawValues.Length)
                throw new IndexOutOfRangeException($"Word offset {wordOffset} exceeds rawValues length {rawValues.Length}");

            if (bitOffset < 0 || bitOffset > 15)
                throw new ArgumentException($"Bit offset {bitOffset} must be between 0 and 15");

            return (rawValues[wordOffset] & (1 << bitOffset)) != 0;
        }


        /// <summary>
        /// 检查值是否发生变化
        /// </summary>
        private bool HasValuesChanged(string groupName, short[] currentValues)
        {
            // 如果是第一次读取，直接返回true
            if (!lastValues.ContainsKey(groupName)) {
                lastValues[groupName] = currentValues.ToArray();
                return true;
            }

            var lastValueArray = lastValues[groupName];
            bool hasChanges = false;

            // 检查长度是否一致
            if (lastValueArray.Length != currentValues.Length) {
                lastValues[groupName] = currentValues.ToArray();
                return true;
            }

            // 逐个比较值
            for (int i = 0; i < currentValues.Length; i++) {
                if (AreValuesEqual(lastValueArray[i], currentValues[i])) {
                    hasChanges = true;
                    // 更新变化的值
                    lastValueArray[i] = currentValues[i];
                }
            }

            // 如果有变化，更新lastValues
            if (hasChanges) {
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
            if (value1 is Array arr1 && value2 is Array arr2) {
                if (arr1.Length != arr2.Length)
                    return false;

                for (int i = 0; i < arr1.Length; i++) {
                    if (!AreValuesEqual(arr1.GetValue(i), arr2.GetValue(i)))
                        return false;
                }
                return true;
            }

            // 处理浮点数
            if (value1 is float f1 && value2 is float f2) {
                return Math.Abs(f1 - f2) < float.Epsilon;
            }
            if (value1 is double d1 && value2 is double d2) {
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
            foreach (var item in block.Items) {
                if (item is ArrayItem arrayItem) {
                    totalLength += arrayItem.Count * arrayItem.Item.Length;
                }
                else {
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
            if (disposed) {
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
            if (!disposed) {
                try {
                    StopMonitoring();
                    cancellationTokenSource?.Dispose();

                    if (variableCompolet != null) {
                        variableCompolet.Active = false;
                        variableCompolet.Dispose();
                    }
                }
                catch (Exception) {
                    // Ignore disposal errors
                }
                finally {
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
        private bool ProcessTagGroupMonitoring(TagGroup group, short[] convertedValues)
        {
            bool hasChanges = false;
            int currentIndex = 0;

            foreach (var block in group.Blocks) {
                if (!ShouldMonitorBlock(block)) {
                    currentIndex += GetBlockTotalLength(block);
                    continue;
                }

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
        private int ProcessBlockMonitoring(string groupName, Block block, short[] convertedValues, int startIndex, ref bool hasChanges)
        {
            int currentIndex = startIndex;

            foreach (var item in block.Items) {
                if (item is ArrayItem arrayItem) {
                    currentIndex = ProcessArrayItemMonitoring(groupName, block, arrayItem, convertedValues, currentIndex, ref hasChanges);
                }
                else {
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
            short[] convertedValues,
            int currentIndex,
            ref bool hasChanges)
        {
            currentIndex = block.Offset;
            int wordOffset = 0;
            int bitOffset = 0;

            if (item.Format == "BIT") {
                // 解析位偏移量
                string[] offsetParts = item.Offset.Split(':');

                if (offsetParts.Length == 2) {
                    wordOffset = int.Parse(offsetParts[0]);
                    bitOffset = int.Parse(offsetParts[1]);

                    // 获取字值并转换为short
                    short wordValue = Convert.ToInt16(convertedValues[block.Offset + wordOffset]);

                    // 获取指定位的布尔值
                    bool bitValue = (wordValue & (1 << bitOffset)) != 0;
                    bool oldValue = (bool)item.Value;
                    if (item.Value != null && oldValue != (bitValue)) {
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
            short[] convertedValues,
            int currentIndex,
            ref bool hasChanges)
        {
            if (arrayItem.Item.Format == "BIT") {
                // 解析位偏移量
                string[] offsetParts = arrayItem.Item.Offset.Split(':');
                int baseWordOffset = 0;
                int baseBitOffset = 0;

                if (offsetParts.Length == 2) {
                    baseWordOffset = int.Parse(offsetParts[0]);
                    baseBitOffset = int.Parse(offsetParts[1]);

                    for (int i = 0; i < arrayItem.Count; i++) {
                        // 计算实际的位偏移
                        int totalBits = baseBitOffset + i;
                        int actualWordOffset = baseWordOffset + (totalBits / 16);
                        int actualBitOffset = totalBits % 16;

                        // 获取字值并转换为short
                        short wordValue = Convert.ToInt16(convertedValues[actualWordOffset]);

                        // 获取指定位的布尔值
                        bool bitValue = (wordValue & (1 << actualBitOffset)) != 0;

                        if (ShouldNotifyValueChange(arrayItem.Item, bitValue)) {
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
                   block.Type.EndsWith("Reply", StringComparison.OrdinalIgnoreCase);
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
            try {
                // 验证偏移量范围
                if (bitOffset < 0 || bitOffset > 15) {
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
            catch (Exception ex) {
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

            try {
                // 查找指定的TagGroup
                var group = tagConfig.TagGroups.FirstOrDefault(g =>
                    g.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));

                if (group == null)
                    throw new ArgumentException($"TagGroup '{groupName}' not found");

                // 读取原始数据
                short[] rawValues = ReadRawValues(group.Name);

                // 查找指定的Block
                var block = group.Blocks.FirstOrDefault(b =>
                    b.Type.Equals(blockType, StringComparison.OrdinalIgnoreCase));

                if (block == null)
                    throw new ArgumentException($"Block '{blockType}' not found in TagGroup '{groupName}'");

                // 计算Block的起始位置
                int currentIndex = block.Offset;

                // 更新Block中所有Items的值
                foreach (var item in block.Items) {
                    if (item is ArrayItem arrayItem) {
                        if (arrayItem.Item.Format == "BIT") {
                            string[] offsetParts = arrayItem.Item.Offset.Split(':');
                            if (offsetParts.Length == 2) {
                                int wordOffset = int.Parse(offsetParts[0]);
                                int bitOffset = int.Parse(offsetParts[1]);

                                var boolArray = new bool[arrayItem.Count];
                                for (int i = 0; i < arrayItem.Count; i++) {
                                    int totalBits = bitOffset + i;
                                    int actualWordOffset = wordOffset + (totalBits / 16);
                                    int actualBitOffset = totalBits % 16;

                                    boolArray[i] = (rawValues[currentIndex + actualWordOffset] & (1 << actualBitOffset)) != 0;
                                }
                                arrayItem.Item.Value = boolArray;
                            }
                            currentIndex += arrayItem.Count; // 位数组长度即为位数
                        }
                        else {
                            var arrayValues = new object[arrayItem.Count];
                            int startIndex = currentIndex + arrayItem.BaseOffset; // 计算数组实际起始索引
                            for (int i = 0; i < arrayItem.Count; i++) {
                                arrayValues[i] = ConvertValueByFormat(
                                    arrayItem.Item.Format,
                                    rawValues,
                                    startIndex + i * arrayItem.Item.Length,
                                    arrayItem.Item.Length
                                );
                            }
                            arrayItem.Item.Value = arrayValues;
                            currentIndex += arrayItem.Count * arrayItem.Item.Length;
                        }
                    }
                    else {
                        if (item.Format == "BIT") {
                            string[] offsetParts = item.Offset.Split(':');
                            if (offsetParts.Length == 2) {
                                int wordOffset = int.Parse(offsetParts[0]);
                                int bitOffset = int.Parse(offsetParts[1]);

                                item.Value = (rawValues[currentIndex + wordOffset] & (1 << bitOffset)) != 0;
                            }
                            currentIndex += item.Length; // 位类型长度通常为1字
                        }
                        else {
                            item.Value = ConvertValueByFormat(
                                item.Format,
                                rawValues,
                                currentIndex,
                                item.Length
                            );
                            currentIndex += item.Length;
                        }
                    }
                }

                return block;
            }
            catch (Exception ex) {
                throw new Exception($"Error reading block values for {groupName}.{blockType}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 读取单个Item的值，支持SimpleItem和ArrayItem
        /// </summary>
        /// <param name="groupName">TagGroup名称</param>
        /// <param name="blockType">Block类型</param>
        /// <param name="itemName">Item名称</param>
        /// <returns>Item的值</returns>
        public object ReadItemValue(string groupName, string blockType, string itemName)
        {
            ValidateNotDisposed();

            try {
                // 查找指定的TagGroup和Block
                var group = tagConfig.TagGroups.FirstOrDefault(g =>
                    g.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));
                if (group == null)
                    throw new ArgumentException($"TagGroup '{groupName}' not found");

                var block = group.Blocks.FirstOrDefault(b =>
                    b.Type.Equals(blockType, StringComparison.OrdinalIgnoreCase));
                if (block == null)
                    throw new ArgumentException($"Block '{blockType}' not found in TagGroup '{groupName}'");

                // 读取原始数据
                short[] rawValues = ReadRawValues(groupName);

                // 查找SimpleItem或ArrayItem
                var item = block.Items.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
                if (item == null)
                    throw new ArgumentException($"Item '{itemName}' not found in block '{blockType}'");
                int blockIndex = block.Index;
                int blockBaseOffset = block.Offset;

                // 处理ArrayItem
                if (item is ArrayItem arrayItem) {
                    if (arrayItem.Item.Format == "BIT") {
                        // 位数组读取
                        string[] offsetParts = arrayItem.Item.Offset.Split(':');
                        if (offsetParts.Length != 2)
                            throw new ArgumentException($"Invalid BIT offset format for array item '{itemName}'");

                        int wordOffsetBase = int.Parse(offsetParts[0]);
                        int bitOffsetBase = int.Parse(offsetParts[1]);

                        bool[] boolArray = new bool[arrayItem.Count];
                        for (int i = 0; i < arrayItem.Count; i++) {
                            int totalBitIndex = bitOffsetBase + i;
                            int actualWordOffset = wordOffsetBase + (totalBitIndex / 16);
                            int actualBitOffset = totalBitIndex % 16;

                            int rawIndex = blockBaseOffset + actualWordOffset;
                            if (rawIndex >= rawValues.Length)
                                throw new IndexOutOfRangeException($"RawValues index {rawIndex} out of range");

                            boolArray[i] = (rawValues[rawIndex] & (1 << actualBitOffset)) != 0;
                        }
                        arrayItem.Item.Value = boolArray;
                        return boolArray;
                    }
                    else {
                        // 其他数组类型
                        object[] arrayValues = new object[arrayItem.Count];
                        int startIndex = blockBaseOffset + arrayItem.BaseOffset;
                        for (int i = 0; i < arrayItem.Count; i++) {
                            int valueOffset = startIndex + i * arrayItem.Item.Length;
                            if (valueOffset >= rawValues.Length)
                                throw new IndexOutOfRangeException($"RawValues index {valueOffset} out of range");

                            arrayValues[i] = ConvertValueByFormat(arrayItem.Item.Format, rawValues, valueOffset, arrayItem.Item.Length);
                        }
                        arrayItem.Item.Value = arrayValues;
                        return arrayValues;
                    }
                }
                else {
                    // SimpleItem处理
                    if (item.Format == "BIT") {
                        string[] offsetParts = item.Offset.Split(':');
                        if (offsetParts.Length != 2)
                            throw new ArgumentException($"Invalid BIT offset format for item '{itemName}'");

                        int wordOffset = int.Parse(offsetParts[0]);
                        int bitOffset = int.Parse(offsetParts[1]);

                        int rawIndex = blockBaseOffset + wordOffset;
                        if (rawIndex >= rawValues.Length)
                            throw new IndexOutOfRangeException($"RawValues index {rawIndex} out of range");

                        bool bitValue = (rawValues[rawIndex] & (1 << bitOffset)) != 0;
                        item.Value = bitValue;
                        return bitValue;
                    }
                    else {
                        int valueOffset = blockIndex + blockBaseOffset + int.Parse(item.Offset);
                        if (valueOffset >= rawValues.Length)
                            throw new IndexOutOfRangeException($"RawValues index {valueOffset} out of range");

                        var val = ConvertValueByFormat(item.Format, rawValues, valueOffset, item.Length);
                        item.Value = val;
                        return val;
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception($"Error reading item value: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 写入单个Item的值（支持BIT位和数组）
        /// </summary>
        /// <param name="groupName">TagGroup名称</param>
        /// <param name="blockType">Block类型</param>
        /// <param name="itemName">Item名称</param>
        /// <param name="value">要写入的值（bool、bool[]、object[]、单值等）</param>
        public void WriteItemValue(string groupName, string blockType, string itemName, object value)
        {
            ValidateNotDisposed();

            try {
                // 查找指定的TagGroup
                var group = tagConfig.TagGroups.FirstOrDefault(g =>
                    g.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));
                if (group == null)
                    throw new ArgumentException($"TagGroup '{groupName}' not found");

                // 查找指定的Block
                var block = group.Blocks.FirstOrDefault(b =>
                    b.Type.Equals(blockType, StringComparison.OrdinalIgnoreCase));
                if (block == null)
                    throw new ArgumentException($"Block '{blockType}' not found in TagGroup '{groupName}'");

                // 读取当前PLC数据，short[]格式
                short[] rawValuesShort = ReadRawValues(groupName);
                if (rawValuesShort == null || rawValuesShort.Length == 0)
                    throw new InvalidOperationException("Failed to read raw PLC values.");

                // 转成ushort[]方便无符号位操作
                ushort[] rawValues = Array.ConvertAll(rawValuesShort, v => unchecked((ushort)v));

                int currentIndex = block.Offset;
                bool itemFound = false;

                foreach (var item in block.Items) {
                    if (item is ArrayItem arrayItem && arrayItem.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase)) {
                        itemFound = true;

                        if (arrayItem.Item.Format.ToUpper() == "BIT") {
                            // BIT数组必须传入bool[]
                            if (!(value is bool[] boolArray))
                                throw new ArgumentException("Value must be bool[] for BIT array item");

                            if (boolArray.Length != arrayItem.Count)
                                throw new ArgumentException($"Array length mismatch for BIT array item. Expected {arrayItem.Count}, got {boolArray.Length}");

                            string[] offsetParts = arrayItem.Item.Offset.Split(':');
                            if (offsetParts.Length != 2)
                                throw new ArgumentException("Invalid offset format for BIT array item");

                            int wordOffset = int.Parse(offsetParts[0]);
                            int bitOffset = int.Parse(offsetParts[1]);

                            for (int i = 0; i < arrayItem.Count; i++) {
                                int totalBits = bitOffset + i;
                                int actualWordOffset = wordOffset + (totalBits / 16);
                                int actualBitOffset = totalBits % 16;

                                ushort mask = (ushort)(1 << actualBitOffset);

                                if (boolArray[i])
                                    rawValues[actualWordOffset] |= mask;
                                else
                                    rawValues[actualWordOffset] &= (ushort)~mask;
                            }
                        }
                        else {
                            // 非BIT数组，必须传object[]，长度匹配
                            if (!(value is object[] values))
                                throw new ArgumentException("Value must be object[] for array item");

                            if (values.Length != arrayItem.Count)
                                throw new ArgumentException($"Array length mismatch for array item. Expected {arrayItem.Count}, got {values.Length}");

                            for (int i = 0; i < arrayItem.Count; i++) {
                                int writeIndex = currentIndex + i * arrayItem.Item.Length;
                                WriteValueToRawData(arrayItem.Item.Format, values[i], rawValues, writeIndex);
                            }
                            arrayItem.Item.Value = values;
                        }
                        break;
                    }
                    else if (item.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase)) {
                        itemFound = true;

                        if (item.Format.ToUpper() == "BIT") {
                            string[] offsetParts = item.Offset.Split(':');
                            if (offsetParts.Length != 2)
                                throw new ArgumentException("Invalid offset format for BIT item");

                            int wordOffset = int.Parse(offsetParts[0]);
                            int bitOffset = int.Parse(offsetParts[1]);

                            currentIndex = block.Offset + wordOffset;

                            bool bitValue = Convert.ToBoolean(value);
                            ushort mask = (ushort)(1 << bitOffset);

                            if (bitValue)
                                rawValues[currentIndex] |= mask;
                            else
                                rawValues[currentIndex] &= (ushort)~mask;

                            item.Value = bitValue;
                        }
                        else {
                            int offsetInt = int.Parse(item.Offset);
                            currentIndex = block.Index + block.Offset + offsetInt;

                            WriteValueToRawData(item.Format, value, rawValues, currentIndex);
                            item.Value = value;
                        }
                        break;
                    }
                }

                if (!itemFound)
                    throw new ArgumentException($"Item '{itemName}' not found in block '{blockType}'");

                // 写回PLC，直接传 ushort[] 数组，避免转换异常
                variableCompolet.WriteVariable(groupName, rawValues);
            }
            catch (Exception ex) {
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

            try {
                // 查找指定TagGroup
                var group = tagConfig.TagGroups.FirstOrDefault(g =>
                    g.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));

                if (group == null)
                    throw new ArgumentException($"TagGroup '{groupName}' not found");

                // 查找Block
                var existingBlock = group.Blocks.FirstOrDefault(b =>
                    b.Type.Equals(block.Type, StringComparison.OrdinalIgnoreCase));

                if (existingBlock == null)
                    throw new ArgumentException($"Block '{block.Type}' not found in TagGroup '{groupName}'");

                // 读取PLC当前数据，short[] -> ushort[]
                short[] rawValuesShort = ReadRawValues(groupName);
                if (rawValuesShort == null || rawValuesShort.Length == 0)
                    throw new InvalidOperationException("Failed to read raw PLC values.");

                ushort[] rawValues = Array.ConvertAll(rawValuesShort, v => unchecked((ushort)v));

                int currentIndex = block.Offset;

                foreach (var item in block.Items) {
                    if (item is ArrayItem arrayItem) {
                        if (arrayItem.Item.Format.ToUpper() == "BIT") {
                            // BIT数组写入bool[]值
                            if (arrayItem.Item.Value is bool[] boolArray) {
                                string[] offsetParts = arrayItem.Item.Offset.Split(':');
                                if (offsetParts.Length != 2)
                                    throw new ArgumentException($"Invalid BIT offset format for array item '{arrayItem.Name}'");

                                int wordOffset = int.Parse(offsetParts[0]);
                                int bitOffset = int.Parse(offsetParts[1]);

                                for (int i = 0; i < arrayItem.Count; i++) {
                                    int totalBits = bitOffset + i;
                                    int actualWordOffset = wordOffset + (totalBits / 16);
                                    int actualBitOffset = totalBits % 16;

                                    ushort mask = (ushort)(1 << actualBitOffset);

                                    if (boolArray[i])
                                        rawValues[actualWordOffset] |= mask;
                                    else
                                        rawValues[actualWordOffset] &= (ushort)~mask;
                                }
                            }
                            else {
                                throw new ArgumentException($"BIT array item '{arrayItem.Name}' value must be bool[]");
                            }

                            // BIT数组按位存储，currentIndex按位数增加
                            currentIndex += arrayItem.Count;
                        }
                        else {
                            // 非BIT数组写入object[]值
                            if (arrayItem.Item.Value is object[] values) {
                                if (values.Length != arrayItem.Count)
                                    throw new ArgumentException($"Array length mismatch for array item '{arrayItem.Name}'. Expected {arrayItem.Count}, got {values.Length}");

                                for (int i = 0; i < arrayItem.Count; i++) {
                                    int writeIndex = currentIndex + i * arrayItem.Item.Length;
                                    WriteValueToRawData(arrayItem.Item.Format, values[i], rawValues, writeIndex);
                                }
                            }
                            else {
                                throw new ArgumentException($"Array item '{arrayItem.Name}' value must be object[]");
                            }

                            currentIndex += arrayItem.Count * arrayItem.Item.Length;
                        }
                    }
                    else {
                        // 单个Item处理
                        if (item.Format.ToUpper() == "BIT") {
                            if (item.Value != null) {
                                string[] offsetParts = item.Offset.Split(':');
                                if (offsetParts.Length != 2)
                                    throw new ArgumentException($"Invalid BIT offset format for item '{item.Name}'");

                                int wordOffset = int.Parse(offsetParts[0]);
                                int bitOffset = int.Parse(offsetParts[1]);

                                bool bitValue = Convert.ToBoolean(item.Value);
                                ushort mask = (ushort)(1 << bitOffset);

                                if (bitValue)
                                    rawValues[wordOffset] |= mask;
                                else
                                    rawValues[wordOffset] &= (ushort)~mask;
                            }

                            currentIndex += item.Length;
                        }
                        else {
                            if (item.Value != null) {
                                WriteValueToRawData(item.Format, item.Value, rawValues, currentIndex);
                            }

                            currentIndex += item.Length;
                        }
                    }
                }

                // 写回PLC，直接传 ushort[]，避免转换异常
                variableCompolet.WriteVariable(groupName, rawValues);
            }
            catch (Exception ex) {
                throw new Exception($"Error writing block values for {groupName}.{block.Type}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 根据格式写入单个值到 rawValues 数组指定索引（ushort[]）
        /// </summary>
        /// <param name="format">数据格式，如 INT, UINT, FLOAT 等</param>
        /// <param name="value">要写入的值</param>
        /// <param name="rawValues">ushort[] 原始数据数组</param>
        /// <param name="index">写入起始索引</param>
        private void WriteValueToRawData(string format, object value, ushort[] rawValues, int index)
        {
            switch (format.ToUpper()) {
                case "INT":
                case "SHORT": {
                        short v = Convert.ToInt16(value);
                        rawValues[index] = unchecked((ushort)v);
                        break;
                    }
                case "UINT":
                case "USHORT": {
                        ushort v = Convert.ToUInt16(value);
                        rawValues[index] = v;
                        break;
                    }
                case "DINT":
                case "INT32":
                case "LONG": {
                        int v = Convert.ToInt32(value);
                        rawValues[index] = unchecked((ushort)(v & 0xFFFF));
                        rawValues[index + 1] = unchecked((ushort)((v >> 16) & 0xFFFF));
                        break;
                    }
                case "UDINT":
                case "UINT32":
                case "ULONG": {
                        uint v = Convert.ToUInt32(value);
                        rawValues[index] = unchecked((ushort)(v & 0xFFFF));
                        rawValues[index + 1] = unchecked((ushort)((v >> 16) & 0xFFFF));
                        break;
                    }
                case "FLOAT": {
                        float v = Convert.ToSingle(value);
                        byte[] bytes = BitConverter.GetBytes(v);
                        rawValues[index] = BitConverter.ToUInt16(bytes, 0);
                        rawValues[index + 1] = BitConverter.ToUInt16(bytes, 2);
                        break;
                    }
                case "DOUBLE": {
                        double v = Convert.ToDouble(value);
                        byte[] bytes = BitConverter.GetBytes(v);
                        rawValues[index] = BitConverter.ToUInt16(bytes, 0);
                        rawValues[index + 1] = BitConverter.ToUInt16(bytes, 2);
                        rawValues[index + 2] = BitConverter.ToUInt16(bytes, 4);
                        rawValues[index + 3] = BitConverter.ToUInt16(bytes, 6);
                        break;
                    }
                case "BYTE": {
                        byte v = Convert.ToByte(value);
                        rawValues[index] = v;
                        break;
                    }
                case "WORD": {
                        ushort v = Convert.ToUInt16(value);
                        rawValues[index] = v;
                        break;
                    }
                default:
                    throw new NotSupportedException($"Unsupported data format: {format}");
            }
        }
        /// <summary>
        /// 写入数组项的值
        /// </summary>
        public void WriteArrayItemValue(string groupName, string blockType, string arrayItemName, int index, object value)
        {
            ValidateNotDisposed();

            try {
                // 查找指定的TagGroup
                var group = tagConfig.TagGroups.FirstOrDefault(g =>
                    g.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));

                if (group == null)
                    throw new ArgumentException($"TagGroup '{groupName}' not found");

                // 查找指定的Block
                var block = group.Blocks.FirstOrDefault(b =>
                    b.Type.Equals(blockType, StringComparison.OrdinalIgnoreCase));

                if (block == null)
                    throw new ArgumentException($"Block '{blockType}' not found in TagGroup '{groupName}'");

                // 查找指定的ArrayItem
                var arrayItem = block.Items.OfType<ArrayItem>().FirstOrDefault(i =>
                    i.Name.Equals(arrayItemName, StringComparison.OrdinalIgnoreCase));

                if (arrayItem == null)
                    throw new ArgumentException($"ArrayItem '{arrayItemName}' not found in Block '{blockType}'");

                if (index < 0 || index >= arrayItem.Count)
                    throw new ArgumentOutOfRangeException(nameof(index), $"Index {index} is out of range for ArrayItem '{arrayItemName}'");

                // 读取当前值 short[]
                short[] rawValuesShort = ReadRawValues(groupName);
                if (rawValuesShort == null || rawValuesShort.Length == 0)
                    throw new InvalidOperationException("Failed to read raw PLC values.");

                // 转换为 ushort[] 方便位操作
                ushort[] rawValues = Array.ConvertAll(rawValuesShort, v => unchecked((ushort)v));

                // 计算实际偏移量
                int baseOffset = block.Offset + arrayItem.BaseOffset;

                if (arrayItem.Item.Format.ToUpper() == "BIT") {
                    string[] offsetParts = arrayItem.Item.Offset.Split(':');
                    if (offsetParts.Length == 2) {
                        int wordOffset = int.Parse(offsetParts[0]);
                        int bitOffset = int.Parse(offsetParts[1]);
                        int totalBits = bitOffset + index;
                        int actualWordOffset = baseOffset + wordOffset + (totalBits / 16);
                        int actualBitOffset = totalBits % 16;

                        bool bitValue = Convert.ToBoolean(value);
                        ushort mask = (ushort)(1 << actualBitOffset);

                        if (bitValue)
                            rawValues[actualWordOffset] |= mask;
                        else
                            rawValues[actualWordOffset] &= (ushort)~mask;
                    }
                    else {
                        throw new FormatException($"Invalid BIT offset format: {arrayItem.Item.Offset}");
                    }
                }
                else {
                    int actualOffset = baseOffset + (index * arrayItem.Item.Length);
                    WriteValueToRawData(arrayItem.Item.Format, value, rawValues, actualOffset);
                }

                // 写回PLC前转换为 short[]
                short[] rawValuesToWrite = Array.ConvertAll(rawValues, v => unchecked((short)v));
                variableCompolet.WriteVariable(groupName, rawValuesToWrite);
            }
            catch (Exception ex) {
                throw new Exception($"Error writing array item value for {groupName}.{blockType}.{arrayItemName}[{index}]: {ex.Message}", ex);
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