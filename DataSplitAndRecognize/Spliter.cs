using FileHandle;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using HexDataHandle;
using System.IO.Ports;
using System.Windows;
using MessageHandle;

namespace DataSplitAndRecognize
{
    /// <summary>
    /// 消息内容识别和拼接
    /// </summary>
    public class Spliter
    {
        private static int _currentDataLength = 0;
        private static Stack<byte> _currentDataCollection;
        private static MessageType _currentType;
        /// <summary>
        /// 识别原始数据中的报文
        /// </summary>
        /// <param name="data"></param>
        /// <param name="valuePairs"></param>
        /// <param name="receivedMessageCallback"></param>
        public static void Split(byte[] data, ConcurrentDictionary<string, MessageType> valuePairs, Action<MessageData> receivedMessageCallback)
        {
            int startIndex;
            if (_currentDataLength == 0)//若无缓存数据
            {
                #region 报文头部识别
                Dictionary<string, int> pairs = new Dictionary<string, int>();//新 < 报文头， 头部位置索引值 > 键值对
                foreach (MessageType mt in valuePairs.Values)//读取所有类型
                {
                    //监测数据中是否存在报文头
                    int refIndex = HexCode.GetString(data).IndexOf(HexCode.GetString(mt.Head), 0);
                    if (refIndex != -1)
                    {
                        pairs.Add(HexCode.GetString(mt.Head), refIndex);//将报文头位置索引值和报文头添加到键值对中
                    }
                }
                if (pairs.Count != 0)
                {
                    var keyValuePairs = from pair in pairs orderby pair.Value ascending select pair;
                    _currentType = valuePairs[keyValuePairs.First().Key]; //使用第一个头
                }
                else
                {
                    return;
                }
                startIndex = pairs[HexCode.GetString(_currentType.Head)] / 3 + _currentType.TypeHeadLength;
                #endregion
                _currentDataCollection = new Stack<byte>();
            }
            else
            {
                startIndex = 0;
            }
            int readLength = 0;//已读取长度
            for (; _currentDataLength + readLength < _currentType.MessageLength + _currentType.TypeFootLength; readLength++)
            {
                if (startIndex + readLength >= data.Length) break;
                _currentDataCollection.Push(data[startIndex + readLength]);
            }
            _currentDataLength = _currentDataCollection.Count();
            if (_currentDataLength == _currentType.MessageLength + _currentType.TypeFootLength)
            {
                List<byte> popFoot = new List<byte>();
                for (int i = 0; i < _currentType.TypeFootLength; i++)
                {
                    popFoot.Add(_currentDataCollection.Pop());//弹出尾部
                }
                if (popFoot.SequenceEqual(_currentType.Foot))//监测尾部是否是合法数据
                {
                    //合法数据，回调传回消息
                    receivedMessageCallback(new MessageData(_currentType, _currentDataCollection.Reverse().ToArray()));
                    _currentType = null;
                    _currentDataCollection = null;
                    _currentDataLength = 0;
                }
                else
                {
                    _currentType = null;
                    _currentDataCollection = null;
                    _currentDataLength = 0;
                }
                if (readLength + startIndex < data.Length)//若还有过剩数据，则进行下一次拼接
                {
                    List<byte> anotherData = data.ToList();
                    lock (anotherData)
                    {
                        anotherData.RemoveRange(0, readLength + startIndex);
                    }
                    Split(anotherData.ToArray(), valuePairs, receivedMessageCallback);
                }
            }
        }
    }
}

