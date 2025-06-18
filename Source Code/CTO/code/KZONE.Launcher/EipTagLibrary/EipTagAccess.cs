using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using OMRON.Compolet.Variable;
using System.Text;

namespace EipTagLibrary
{
    public enum VariableType
    {
        TIMER = 1, COUNTER = 2, CHANNEL = 3, UINT_BCD = 4, UDINT_BCD = 5, ULINT_BCD = 6,
        BOOL = 193, INT = 195, DINT = 196, LINT = 197, UINT = 199, UDINT = 200, ULINT = 201,
        REAL = 202, LREAL = 203, STRING = 208, WORD = 210, DWORD = 211, LWORD = 212,
        ASTRUCT = 160, STRUCT = 162, ARRAY = 163
    }

    public class TagValueChangedEventArgs : EventArgs
    {
        public string TagGroupName { get; }
        public string BlockType { get; }
        public ItemBase Item { get; }
        public object Value { get; }
        public DateTime Timestamp { get; }
        public int WordOffset { get; }
        public int BitOffset { get; }
        public int ArrayIndex { get; }

        public TagValueChangedEventArgs(string tagGroupName, string blockType, ItemBase item, object value,
            int wordOffset, int bitOffset, int arrayIndex = -1)
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

    public class EipTagAccess : IDisposable
    {
        private readonly VariableCompolet variableCompolet;
        private readonly TagConfig tagConfig;
        private readonly Dictionary<string, TagAddressInfo> tagAddressCache;
        private bool disposed = false;
        private CancellationTokenSource cancellationTokenSource;
        private Dictionary<string, ushort[]> lastValues;
        private readonly object lockObj = new object();

        public delegate void TagValueChangedEventHandler(object sender, TagValueChangedEventArgs e);
        public event TagValueChangedEventHandler OnTagValueChanged;

        public EipTagAccess(string xmlConfigPath)
        {
            variableCompolet = new VariableCompolet
            {
                Active = true,
                PlcEncoding = Encoding.UTF8
            };

            tagConfig = LoadXmlConfig(xmlConfigPath);
            tagAddressCache = new Dictionary<string, TagAddressInfo>();
            lastValues = new Dictionary<string, ushort[]>();
            InitializeTagAddressCache();
            cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task StartMonitoring(int intervalMs = 100)
        {
            ValidateNotDisposed();
            try
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    var groupsToMonitor = tagConfig.TagGroups
                        .Where(g => string.Equals(g.Direction, "Input", StringComparison.OrdinalIgnoreCase) ||
                                    string.Equals(g.Direction, "Output", StringComparison.OrdinalIgnoreCase));

                    foreach (var group in groupsToMonitor)
                    {
                        try
                        {
                            var rawValues = ReadRawValues(group.Name);
                            if (rawValues == null || rawValues.Length == 0)
                                continue;

                            if (HasValuesChanged(group.Name, rawValues))
                            {
                                lock (lockObj)
                                {
                                    ConvertRawValuesToTagGroup(group, rawValues);
                                }
                                ProcessTagGroupMonitoring(group, rawValues);
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
                // 正常取消退出
            }
        }

        public void StopMonitoring()
        {
            cancellationTokenSource?.Cancel();
        }

        private ushort[] ReadRawValues(string groupName)
        {
            var obj = variableCompolet.ReadVariable(groupName);
            if (obj is ushort[] ushortArray)
                return ushortArray;

            if (!(obj is Array array))
                throw new NotSupportedException($"Expected array type but got {obj.GetType().Name}");

            var elementType = array.GetType().GetElementType();

            if (elementType.IsClass && elementType != typeof(string))
                throw new NotSupportedException($"Complex type arrays are not supported: {elementType.Name}");

            try
            {
                if (elementType == typeof(short))
                {
                    var shorts = (short[])array;
                    return Array.ConvertAll(shorts, i => unchecked((ushort)i));
                }
                if (elementType == typeof(ushort))
                {
                    return (ushort[])array;
                }
                if (elementType == typeof(int))
                {
                    var ints = (int[])array;
                    if (ints == null || ints.Length == 0)
                        return Array.Empty<ushort>();
                    var result = new ushort[ints.Length * 2];
                    for (int i = 0; i < ints.Length; i++)
                    {
                        var bytes = BitConverter.GetBytes(ints[i]);
                        result[2 * i] = BitConverter.ToUInt16(bytes, 0);
                        result[2 * i + 1] = BitConverter.ToUInt16(bytes, 2);
                    }
                    return result;
                }
                if (elementType == typeof(uint))
                {
                    var uints = (uint[])array;
                    if (uints == null || uints.Length == 0)
                        return Array.Empty<ushort>();
                    var result = new ushort[uints.Length * 2];
                    for (int i = 0; i < uints.Length; i++)
                    {
                        var bytes = BitConverter.GetBytes(uints[i]);
                        result[2 * i] = BitConverter.ToUInt16(bytes, 0);
                        result[2 * i + 1] = BitConverter.ToUInt16(bytes, 2);
                    }
                    return result;
                }
                if (elementType == typeof(float))
                {
                    var floats = (float[])array;
                    if (floats == null || floats.Length == 0)
                        return Array.Empty<ushort>();
                    var result = new ushort[floats.Length * 2];
                    for (int i = 0; i < floats.Length; i++)
                    {
                        var bytes = BitConverter.GetBytes(floats[i]);
                        result[2 * i] = BitConverter.ToUInt16(bytes, 0);
                        result[2 * i + 1] = BitConverter.ToUInt16(bytes, 2);
                    }
                    return result;
                }
                if (elementType == typeof(double))
                {
                    var doubles = (double[])array;
                    if (doubles == null || doubles.Length == 0)
                        return Array.Empty<ushort>();
                    var result = new ushort[doubles.Length * 4];
                    for (int i = 0; i < doubles.Length; i++)
                    {
                        var bytes = BitConverter.GetBytes(doubles[i]);
                        for (int j = 0; j < 4; j++)
                            result[4 * i + j] = BitConverter.ToUInt16(bytes, j * 2);
                    }
                    return result;
                }
                if (elementType == typeof(bool))
                {
                    var bools = (bool[])array;
                    if (bools == null || bools.Length == 0)
                        return Array.Empty<ushort>();
                    var result = new ushort[(bools.Length + 15) / 16];
                    for (int i = 0; i < bools.Length; i++)
                    {
                        if (bools[i])
                            result[i / 16] |= (ushort)(1 << (i % 16));
                    }
                    return result;
                }
                if (elementType == typeof(string))
                {
                    throw new NotSupportedException("String arrays cannot be converted to ushort[]");
                }

                throw new NotSupportedException($"Unsupported element type: {elementType.Name}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error converting array of type {elementType.Name} to ushort[]: {ex.Message}", ex);
            }
        }

        private TagConfig LoadXmlConfig(string xmlPath)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(TagConfig));
                using (var reader = new StreamReader(xmlPath))
                {
                    var config = (TagConfig)serializer.Deserialize(reader);
                    if (config.TagGroups == null || config.TagGroups.Count == 0)
                        throw new Exception("No TagGroups found in configuration");
                    return config;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load XML configuration from {xmlPath}: {ex.Message}", ex);
            }
        }

        private void InitializeTagAddressCache()
        {
            foreach (var group in tagConfig.TagGroups)
            {
                foreach (var block in group.Blocks)
                {
                    foreach (var item in block.Items)
                    {
                        if (item is ArrayItem arrayItem)
                        {
                            for (int i = 0; i < arrayItem.Count; i++)
                            {
                                string itemName = $"{arrayItem.Name}[{i}]";
                                string address = CalculateAddress(block.Offset + arrayItem.BaseOffset + i * arrayItem.Item.Length, arrayItem.Item.Area, arrayItem.Item.Format);
                                tagAddressCache[$"{group.Name}.{block.Type}.{itemName}"] = new TagAddressInfo
                                {
                                    TagGroup = group,
                                    Block = block,
                                    Item = arrayItem.Item,
                                    Address = address
                                };
                            }
                        }
                        else
                        {
                            string[] offsetParts = item.Offset.Split(':');
                            int offset = int.Parse(offsetParts[0]);
                            int bitOffset = offsetParts.Length > 1 ? int.Parse(offsetParts[1]) : -1;
                            string address = CalculateAddress(block.Offset + offset, item.Area, item.Format, bitOffset);
                            tagAddressCache[$"{group.Name}.{block.Type}.{item.Name}"] = new TagAddressInfo
                            {
                                TagGroup = group,
                                Block = block,
                                Item = item,
                                Address = address
                            };
                        }
                    }
                }
            }
        }

        private string CalculateAddress(int absoluteOffset, string area, string format, int bitOffset = -1)
        {
            string address = $"{area}{absoluteOffset}";
            if (string.Equals(format, "BIT", StringComparison.OrdinalIgnoreCase) && bitOffset >= 0)
                address += $".{bitOffset}";
            return address;
        }

        public T ReadTag<T>(string tagAddress)
        {
            ValidateNotDisposed();

            if (!tagAddressCache.TryGetValue(tagAddress, out var tagInfo))
                throw new ArgumentException($"Tag address '{tagAddress}' not found in configuration");

            try
            {
                object val = variableCompolet.ReadVariable(tagInfo.Address);
                return ConvertValue<T>(val, tagInfo.Item.Format);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to read tag '{tagAddress}' at '{tagInfo.Address}': {ex.Message}", ex);
            }
        }

        public void WriteTag<T>(string tagAddress, T value)
        {
            ValidateNotDisposed();

            if (!tagAddressCache.TryGetValue(tagAddress, out var tagInfo))
                throw new ArgumentException($"Tag address '{tagAddress}' not found in configuration");

            try
            {
                object writeVal = ConvertToTargetType(value, tagInfo.Item.Format);
                variableCompolet.WriteVariable(tagInfo.Address, writeVal);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to write tag '{tagAddress}' at '{tagInfo.Address}': {ex.Message}", ex);
            }
        }

        private T ConvertValue<T>(object value, string format)
        {
            try
            {
                switch (format.ToUpperInvariant())
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
                throw new Exception($"Failed to convert value from format '{format}' to '{typeof(T).Name}'", ex);
            }
        }

        private object ConvertToTargetType(object value, string format)
        {
            try
            {
                switch (format.ToUpperInvariant())
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

        private object[] ConvertRawValuesToTagGroup(TagGroup group, ushort[] rawValues)
        {
            lock (lockObj)
            {
                var result = new object[rawValues.Length];
                foreach (var block in group.Blocks)
                {
                    int currentOffset = block.Offset;
                    int lastWordOffset = 0;
                    foreach (var item in block.Items)
                    {
                        if (item is ArrayItem arrayItem)
                        {
                            if (arrayItem.Item.Format.Equals("BIT", StringComparison.OrdinalIgnoreCase))
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
                                        bool val = (rawValues[actualWordOffset] & (1 << actualBitOffset)) != 0;
                                        result[currentOffset + i] = val;
                                        arrayItem.Item.Value = val;
                                    }
                                    currentOffset += arrayItem.Count;
                                }
                            }
                            else
                            {
                                for (int i = 0; i < arrayItem.Count; i++)
                                {
                                    var val = ConvertValueByFormat(arrayItem.Item.Format, rawValues, currentOffset, arrayItem.Item.Length);
                                    result[currentOffset] = val;
                                    arrayItem.Item.Value = val;
                                    currentOffset += arrayItem.Item.Length;
                                }
                            }
                        }
                        else
                        {
                            if (item.Format.Equals("BIT", StringComparison.OrdinalIgnoreCase))
                            {
                                string[] offsetParts = item.Offset.Split(':');
                                if (offsetParts.Length == 2)
                                {
                                    int wordOffset = int.Parse(offsetParts[0]);
                                    if (wordOffset != lastWordOffset)
                                    {
                                        currentOffset = block.Offset + wordOffset;
                                        lastWordOffset = wordOffset;
                                    }
                                    int bitOffset = int.Parse(offsetParts[1]);
                                    bool val = (rawValues[currentOffset] & (1 << bitOffset)) != 0;
                                    result[currentOffset] = val;
                                    item.Value = val;
                                }
                                else
                                {
                                    item.Value = ConvertValueByFormat(item.Format, rawValues, currentOffset, item.Length);
                                }
                            }
                            else
                            {
                                var val = ConvertValueByFormat(item.Format, rawValues, currentOffset, item.Length);
                                result[currentOffset] = val;
                                item.Value = val;
                                currentOffset += item.Length;
                            }
                        }
                    }
                }
                return result;
            }
        }

        private object ConvertValueByFormat(string format, ushort[] rawValues, int offset, int length)
        {
            switch (format.ToUpperInvariant())
            {
                case "BIN":
                    return string.Concat(rawValues.Skip(offset).Take(length).Select(w => Convert.ToString(w, 2).PadLeft(16, '0')));
                case "INT":
                case "SINT":
                    if (length == 1)
                        return unchecked((short)rawValues[offset]);
                    if (length == 2)
                        return unchecked((int)((rawValues[offset] << 16) | rawValues[offset + 1]));
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
                        long value = 0;
                        for (int i = 0; i < 4; i++)
                            value |= (long)rawValues[offset + i] << (16 * i);
                        return value;
                    }
                    break;
                case "DOUBLE":
                    if (length == 4)
                    {
                        var bytes = new byte[8];
                        for (int i = 0; i < 4; i++)
                        {
                            bytes[i * 2] = (byte)(rawValues[offset + i] & 0xFF);
                            bytes[i * 2 + 1] = (byte)(rawValues[offset + i] >> 8);
                        }
                        return BitConverter.ToDouble(bytes, 0);
                    }
                    break;
                case "BIT":
                    return (rawValues[offset] & 0x1) != 0;
            }
            throw new NotSupportedException($"Unsupported format or length: {format} - {length}");
        }

        private bool HasValuesChanged(string groupName, ushort[] currentValues)
        {
            if (!lastValues.TryGetValue(groupName, out var lastValueArray))
            {
                lastValues[groupName] = currentValues.ToArray();
                return false;
            }
            if (lastValueArray.Length != currentValues.Length)
            {
                lastValues[groupName] = currentValues.ToArray();
                return true;
            }
            bool hasChanges = false;
            for (int i = 0; i < currentValues.Length; i++)
            {
                if (!AreValuesEqual(lastValueArray[i], currentValues[i]))
                {
                    hasChanges = true;
                    lastValueArray[i] = currentValues[i];
                }
            }
            if (hasChanges)
                lastValues[groupName] = lastValueArray;
            return hasChanges;
        }

        private bool AreValuesEqual(object value1, object value2)
        {
            if (ReferenceEquals(value1, value2)) return true;
            if (value1 == null || value2 == null) return false;
            if (value1 is Array arr1 && value2 is Array arr2)
            {
                if (arr1.Length != arr2.Length) return false;
                for (int i = 0; i < arr1.Length; i++)
                {
                    if (!AreValuesEqual(arr1.GetValue(i), arr2.GetValue(i)))
                        return false;
                }
                return true;
            }
            if (value1 is float f1 && value2 is float f2)
                return Math.Abs(f1 - f2) < 1e-6f;
            if (value1 is double d1 && value2 is double d2)
                return Math.Abs(d1 - d2) < 1e-12;
            return value1.Equals(value2);
        }

        private void ProcessTagGroupMonitoring(TagGroup group, ushort[] rawValues)
        {
            foreach (var block in group.Blocks)
            {
                foreach (var item in block.Items)
                {
                    if (item.Format.Equals("BIT", StringComparison.OrdinalIgnoreCase))
                    {
                        if (item is ArrayItem arrayItem)
                        {
                            string[] offsetParts = arrayItem.Item.Offset.Split(':');
                            if (offsetParts.Length != 2) continue;
                            int baseWordOffset = int.Parse(offsetParts[0]);
                            int baseBitOffset = int.Parse(offsetParts[1]);
                            for (int i = 0; i < arrayItem.Count; i++)
                            {
                                int totalBits = baseBitOffset + i;
                                int actualWordOffset = baseWordOffset + (totalBits / 16);
                                int actualBitOffset = totalBits % 16;
                                bool bitValue = (rawValues[actualWordOffset] & (1 << actualBitOffset)) != 0;
                                if (ShouldNotifyValueChange(arrayItem.Item, bitValue))
                                {
                                    arrayItem.Item.Value = bitValue;
                                    OnTagValueChanged?.Invoke(this,
                                        new TagValueChangedEventArgs(group.Name, block.Type, arrayItem.Item,
                                            bitValue, actualWordOffset, actualBitOffset, i));
                                }
                            }
                        }
                        else
                        {
                            string[] offsetParts = item.Offset.Split(':');
                            if (offsetParts.Length != 2) continue;
                            int wordOffset = int.Parse(offsetParts[0]);
                            int bitOffset = int.Parse(offsetParts[1]);
                            bool bitValue = (rawValues[wordOffset] & (1 << bitOffset)) != 0;
                            if (ShouldNotifyValueChange(item, bitValue))
                            {
                                item.Value = bitValue;
                                OnTagValueChanged?.Invoke(this,
                                    new TagValueChangedEventArgs(group.Name, block.Type, item, bitValue, wordOffset, bitOffset));
                            }
                        }
                    }
                }
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

            var group = tagConfig.TagGroups.FirstOrDefault(g =>
                g.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));
            if (group == null)
                throw new ArgumentException($"TagGroup '{groupName}' not found");

            var block = group.Blocks.FirstOrDefault(b =>
                b.Type.Equals(blockType, StringComparison.OrdinalIgnoreCase));
            if (block == null)
                throw new ArgumentException($"Block '{blockType}' not found in TagGroup '{groupName}'");

            // 读取当前原始数据
            ushort[] rawValues = ReadRawValues(groupName);

            bool itemFound = false;

            foreach (var item in block.Items)
            {
                if (item.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                {
                    itemFound = true;

                    if (item.Format.Equals("BIT", StringComparison.OrdinalIgnoreCase))
                    {
                        // 处理BIT格式单个位
                        var offsetParts = item.Offset.Split(':');
                        if (offsetParts.Length != 2)
                            throw new ArgumentException($"Invalid BIT offset format: {item.Offset}");

                        int wordOffset = int.Parse(offsetParts[0]);
                        int bitOffset = int.Parse(offsetParts[1]);

                        bool bitValue = Convert.ToBoolean(value);

                        if (bitValue)
                            rawValues[wordOffset] |= (ushort)(1 << bitOffset);
                        else
                            rawValues[wordOffset] &= (ushort)~(1 << bitOffset);

                        item.Value = bitValue;
                    }
                    else
                    {
                        // 处理非BIT格式，按WORD长度写入
                        int wordOffset = int.Parse(item.Offset.Split(':')[0]); // 只取字地址部分
                        WriteValueToRawData(item.Format, value, rawValues, wordOffset, item.Length);
                        item.Value = value;
                    }
                    break;
                }
            }

            if (!itemFound)
                throw new ArgumentException($"Item '{itemName}' not found in block '{blockType}'");

            // 写回设备
            variableCompolet.WriteVariable(groupName, rawValues);
        }

        /// <summary>
        /// 将指定格式的值写入到ushort数组中的指定偏移位置
        /// </summary>
        /// <param name="format">数据格式，如 INT、FLOAT、ASCII、BIN 等</param>
        /// <param name="value">要写入的值</param>
        /// <param name="rawValues">目标ushort数组</param>
        /// <param name="offset">写入起始偏移（字地址）</param>
        /// <param name="length">数据长度（字数）</param>
        private void WriteValueToRawData(string format, object value, ushort[] rawValues, int offset, int length)
        {
            if (offset < 0 || offset + length > rawValues.Length)
                throw new ArgumentOutOfRangeException(nameof(offset), "写入偏移超出数组范围");

            // 先清空目标区域，避免残留旧数据
            for (int i = offset; i < offset + length; i++)
                rawValues[i] = 0;

            switch (format.ToUpperInvariant())
            {
                case "INT":
                case "WORD":
                case "SINT":
                    // 16位整数直接写入第一个字
                    rawValues[offset] = Convert.ToUInt16(value);
                    break;

                case "BIN":
                    // value为二进制字符串，长度应为 length*16
                    if (!(value is string bitString))
                        throw new ArgumentException("BIN格式的值必须是二进制字符串", nameof(value));
                    if (bitString.Length != length * 16)
                        throw new ArgumentException($"BIN格式字符串长度应为 {length * 16} 位", nameof(value));

                    for (int i = 0; i < length; i++)
                    {
                        string segment = bitString.Substring(i * 16, 16);
                        rawValues[offset + i] = Convert.ToUInt16(segment, 2);
                    }
                    break;

                case "FLOAT":
                    if (length < 2)
                        throw new ArgumentException("FLOAT格式至少需要2个字长度", nameof(length));
                    var floatBytes = BitConverter.GetBytes(Convert.ToSingle(value));
                    rawValues[offset] = BitConverter.ToUInt16(floatBytes, 0);
                    rawValues[offset + 1] = BitConverter.ToUInt16(floatBytes, 2);
                    break;

                case "DOUBLE":
                    if (length < 4)
                        throw new ArgumentException("DOUBLE格式至少需要4个字长度", nameof(length));
                    var doubleBytes = BitConverter.GetBytes(Convert.ToDouble(value));
                    for (int i = 0; i < 4; i++)
                        rawValues[offset + i] = BitConverter.ToUInt16(doubleBytes, i * 2);
                    break;

                case "LONG":
                    if (length < 4)
                        throw new ArgumentException("LONG格式至少需要4个字长度", nameof(length));
                    var longBytes = BitConverter.GetBytes(Convert.ToInt64(value));
                    for (int i = 0; i < 4; i++)
                        rawValues[offset + i] = BitConverter.ToUInt16(longBytes, i * 2);
                    break;

                case "ASCII":
                    string str = value.ToString();
                    int maxChars = Math.Min(length * 2, str.Length);
                    for (int i = 0; i < maxChars; i += 2)
                    {
                        ushort charVal = (ushort)(str[i] & 0xFF);
                        if (i + 1 < maxChars)
                            charVal |= (ushort)((str[i + 1] & 0xFF) << 8);
                        rawValues[offset + i / 2] = charVal;
                    }
                    break;

                default:
                    throw new NotSupportedException($"不支持的数据格式: {format}");
            }
        }

        /// <summary>
        /// 读取指定TagGroup中指定Block的所有数据，并填充Block中Items的Value属性
        /// </summary>
        /// <param name="groupName">TagGroup名称</param>
        /// <param name="blockType">Block类型</param>
        /// <returns>包含最新值的Block对象</returns>
        public Block ReadBlockValues(string groupName, string blockType)
        {
            ValidateNotDisposed();

            // 查找TagGroup
            var group = tagConfig.TagGroups.FirstOrDefault(g =>
                g.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));
            if (group == null)
                throw new ArgumentException($"TagGroup '{groupName}' not found");

            // 查找Block
            var block = group.Blocks.FirstOrDefault(b =>
                b.Type.Equals(blockType, StringComparison.OrdinalIgnoreCase));
            if (block == null)
                throw new ArgumentException($"Block '{blockType}' not found in TagGroup '{groupName}'");

            // 读取该TagGroup的原始数据
            ushort[] rawValues = ReadRawValues(groupName);
            if (rawValues == null || rawValues.Length == 0)
                throw new Exception($"No data read from group '{groupName}'");

            // 线程安全转换更新Items的值
            lock (lockObj)
            {
                // 遍历Block的所有Item，转换并赋值
                int currentIndex = block.Offset;
                foreach (var item in block.Items)
                {
                    if (item is ArrayItem arrayItem)
                    {
                        if (arrayItem.Item.Format.Equals("BIT", StringComparison.OrdinalIgnoreCase))
                        {
                            string[] offsetParts = arrayItem.Item.Offset.Split(':');
                            if (offsetParts.Length != 2)
                                throw new Exception($"Invalid BIT array item offset format: {arrayItem.Item.Offset}");

                            int baseWordOffset = int.Parse(offsetParts[0]);
                            int baseBitOffset = int.Parse(offsetParts[1]);

                            bool[] bitValues = new bool[arrayItem.Count];
                            for (int i = 0; i < arrayItem.Count; i++)
                            {
                                int totalBits = baseBitOffset + i;
                                int actualWordOffset = baseWordOffset + (totalBits / 16);
                                int actualBitOffset = totalBits % 16;
                                bitValues[i] = (rawValues[actualWordOffset] & (1 << actualBitOffset)) != 0;
                            }
                            arrayItem.Item.Value = bitValues;
                        }
                        else
                        {
                            object[] arrValues = new object[arrayItem.Count];
                            for (int i = 0; i < arrayItem.Count; i++)
                            {
                                arrValues[i] = ConvertValueByFormat(arrayItem.Item.Format, rawValues, currentIndex, arrayItem.Item.Length);
                                currentIndex += arrayItem.Item.Length;
                            }
                            arrayItem.Item.Value = arrValues;
                        }
                    }
                    else
                    {
                        if (item.Format.Equals("BIT", StringComparison.OrdinalIgnoreCase))
                        {
                            string[] offsetParts = item.Offset.Split(':');
                            if (offsetParts.Length == 2)
                            {
                                int wordOffset = int.Parse(offsetParts[0]);
                                int bitOffset = int.Parse(offsetParts[1]);
                                bool bitVal = (rawValues[wordOffset] & (1 << bitOffset)) != 0;
                                item.Value = bitVal;
                            }
                            else
                            {
                                // 非位格式，直接转换
                                item.Value = ConvertValueByFormat(item.Format, rawValues, currentIndex, item.Length);
                                currentIndex += item.Length;
                            }
                        }
                        else
                        {
                            item.Value = ConvertValueByFormat(item.Format, rawValues, currentIndex, item.Length);
                            currentIndex += item.Length;
                        }
                    }
                }
            }
            return block;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="block"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void WriteBlockValues(string groupName, Block block)
        {
            if (block == null)
                throw new ArgumentNullException(nameof(block));

            object[] valuesToWrite = block.Items.Select(item =>
            {
                if (item is ArrayItem arrayItem)
                    return arrayItem.Item.Value;
                else
                    return item.Value;
            }).ToArray();

            WriteBlockValues(groupName, block.Type, valuesToWrite);
        }
        /// <summary>
        /// 批量写入指定TagGroup下指定Block的所有项的值
        /// </summary>
        /// <param name="groupName">TagGroup名称</param>
        /// <param name="blockType">Block类型</param>
        /// <param name="values">包含要写入的值，顺序应与Block.Items对应</param>
        public void WriteBlockValues(string groupName, string blockType, object[] values)
        {
            ValidateNotDisposed();

            if (values == null)
                throw new ArgumentNullException(nameof(values));

            var group = tagConfig.TagGroups.FirstOrDefault(g =>
                g.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));
            if (group == null)
                throw new ArgumentException($"TagGroup '{groupName}' not found.");

            var block = group.Blocks.FirstOrDefault(b =>
                b.Type.Equals(blockType, StringComparison.OrdinalIgnoreCase));
            if (block == null)
                throw new ArgumentException($"Block '{blockType}' not found in TagGroup '{groupName}'.");

            if (values.Length != block.Items.Count)
                throw new ArgumentException($"传入值数量({values.Length})与Block中Item数量({block.Items.Count})不匹配。");

            // 读取当前原始数据，准备修改
            ushort[] rawValues = ReadRawValues(groupName);
            if (rawValues == null || rawValues.Length == 0)
                throw new Exception($"读取TagGroup '{groupName}'数据失败或为空。");

            lock (lockObj)
            {
                int currentOffset = block.Offset;

                for (int i = 0; i < block.Items.Count; i++)
                {
                    var item = block.Items[i];
                    var val = values[i];

                    if (item is ArrayItem arrayItem)
                    {
                        if (string.Equals(arrayItem.Item.Format, "BIT", StringComparison.OrdinalIgnoreCase))
                        {
                            if (!(val is bool[] boolArr))
                                throw new ArgumentException($"Item '{arrayItem.Name}' 期望 bool[] 类型的值。");

                            if (boolArr.Length != arrayItem.Count)
                                throw new ArgumentException($"Item '{arrayItem.Name}' 数组长度不匹配，预期 {arrayItem.Count}，实际 {boolArr.Length}。");

                            string[] offsetParts = arrayItem.Item.Offset.Split(':');
                            if (offsetParts.Length != 2)
                                throw new Exception($"ArrayItem BIT格式偏移不合法: {arrayItem.Item.Offset}");

                            int baseWordOffset = int.Parse(offsetParts[0]);
                            int baseBitOffset = int.Parse(offsetParts[1]);

                            for (int bitIndex = 0; bitIndex < boolArr.Length; bitIndex++)
                            {
                                int totalBits = baseBitOffset + bitIndex;
                                int wordOffset = baseWordOffset + (totalBits / 16);
                                int bitPos = totalBits % 16;

                                if (boolArr[bitIndex])
                                    rawValues[wordOffset] |= (ushort)(1 << bitPos);
                                else
                                    rawValues[wordOffset] &= (ushort)~(1 << bitPos);
                            }

                            arrayItem.Item.Value = boolArr;
                        }
                        else
                        {
                            if (!(val is object[] objArr))
                                throw new ArgumentException($"Item '{arrayItem.Name}' 期望 object[] 类型的值。");

                            if (objArr.Length != arrayItem.Count)
                                throw new ArgumentException($"Item '{arrayItem.Name}' 数组长度不匹配，预期 {arrayItem.Count}，实际 {objArr.Length}。");

                            for (int idx = 0; idx < arrayItem.Count; idx++)
                            {
                                WriteValueToRawData(arrayItem.Item.Format, objArr[idx], rawValues, currentOffset, arrayItem.Item.Length);
                                currentOffset += arrayItem.Item.Length;
                            }

                            arrayItem.Item.Value = objArr;
                        }
                    }
                    else
                    {
                        if (string.Equals(item.Format, "BIT", StringComparison.OrdinalIgnoreCase))
                        {
                            string[] offsetParts = item.Offset.Split(':');
                            if (offsetParts.Length != 2)
                            {
                                // 非位格式按普通处理
                                WriteValueToRawData(item.Format, val, rawValues, currentOffset, item.Length);
                                currentOffset += item.Length;
                            }
                            else
                            {
                                int wordOffset = int.Parse(offsetParts[0]);
                                int bitOffset = int.Parse(offsetParts[1]);

                                bool bitVal = Convert.ToBoolean(val);
                                if (bitVal)
                                    rawValues[wordOffset] |= (ushort)(1 << bitOffset);
                                else
                                    rawValues[wordOffset] &= (ushort)~(1 << bitOffset);

                                item.Value = bitVal;
                            }
                        }
                        else
                        {
                            WriteValueToRawData(item.Format, val, rawValues, currentOffset, item.Length);
                            currentOffset += item.Length;
                            item.Value = val;
                        }
                    }
                }
            }
            // 
            variableCompolet.WriteVariable(groupName, rawValues);
        }

        private bool ShouldNotifyValueChange(ItemBase item, object value)
        {
            if (item.Value == null) return true;
            return !AreValuesEqual(item.Value, value);
        }

        private void ValidateNotDisposed()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(EipTagAccess));
        }

        public void Dispose()
        {
            if (disposed) return;
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
            catch { }
            disposed = true;
        }
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

        public ItemBase this[string itemName]
        {
            get => Items.First(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
            set
            {
                var idx = Items.FindIndex(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
                if (idx == -1)
                    throw new KeyNotFoundException($"Item '{itemName}' not found");
                if (Items[idx].GetType() != value.GetType())
                    throw new ArgumentException("Type mismatch");
                Items[idx] = value;
            }
        }

        public ItemBase this[int index]
        {
            get => Items[index];
            set => Items[index] = value;
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

    public class SimpleItem : ItemBase { }

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
