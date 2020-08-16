using FileHandle;
using HexDataHandle;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticMessageTypeInRuntime
{
    public class LegalMessageType
    {
        private static ConcurrentDictionary<string, MessageType> _messageTypesDictionaryWithHead;
        public static ConcurrentDictionary<string, MessageType> MessageTypesDictionaryWithHead
        {
            get
            {
                return _messageTypesDictionaryWithHead;
            }
        }
        private static ConcurrentDictionary<string, MessageType> _messageTypesDictionaryWithName;
        public static ConcurrentDictionary<string, MessageType> MessageTypesDictionaryWithName
        {
            get
            {
                return _messageTypesDictionaryWithName;
            }
        }
        /// <summary>
        /// 加载报文类型
        /// </summary>
        /// <returns>类型名称</returns>
        public static bool LoadTypes(Action<string[]> action)
        {
            /***********************************************************
             *      1. 读取文件夹中所有JSON文件
             *      2. 使用FileHandler.ReadJson()读取出内容，若读取错误，
             *         输出错误消息
             *      3. 若读取出的类型不为空，则将其置入字典中，并将类型
             *         名称添加到返回字符串数组。
             *      4. 返回读取到的类型名称。
             **********************************************************/
            List<string> typeNames = new List<string>();
            if (Directory.Exists(@"./MessageTypes"))
            {
                foreach (string path in Directory.GetFiles(@"./MessageTypes/", @"*.json"))
                {
                    MessageType type = FileHandler.ReadJson(path, (str) =>
                    {
                        Debug.WriteLine(str);
                    });
                    if (type != null)
                    {
                        if (_messageTypesDictionaryWithHead == null) _messageTypesDictionaryWithHead = new ConcurrentDictionary<string, MessageType>();
                        if (_messageTypesDictionaryWithName == null) _messageTypesDictionaryWithName = new ConcurrentDictionary<string, MessageType>();
                        if ((!_messageTypesDictionaryWithHead.ContainsKey(HexCode.GetString(type.Head))) && (!_messageTypesDictionaryWithName.ContainsKey(type.TypeName)))
                        {
                            _messageTypesDictionaryWithHead.TryAdd(HexCode.GetString(type.Head), type);
                            _messageTypesDictionaryWithName.TryAdd(type.TypeName, type);
                            typeNames.Add(type.TypeName);
                        }
                    }
                }
            }
            if (typeNames.Count == 0) return false;
            action(typeNames.ToArray());
            return true;
        }
    }
}
