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
    public class Spliter
    {
        private static int _currentDataLength = 0;
        private static Stack<byte> _currentDataCollection;
        private static MessageType _currentType;
        public static void Split(byte[] data, ConcurrentDictionary<string, MessageType> valuePairs, Action<MessageData> receivedMessageCallback)
        {
            if(_currentDataLength == 0)
            {
                Dictionary<string, int> pairs = new Dictionary<string, int>();
                foreach(MessageType mt in valuePairs.Values)
                {
                    int refIndex = HexCode.GetString(data).IndexOf(HexCode.GetString(mt.Head), 0);
                    if(refIndex != -1)
                    {
                        pairs.Add(HexCode.GetString(mt.Head), refIndex);
                    }
                }
                if (pairs.Count != 0)
                {
                    var keyValuePairs = from pair in pairs orderby pair.Value ascending select pair;
                    _currentType = valuePairs[keyValuePairs.First().Key];
                }
                else
                {
                    return;
                }
                int startIndex = pairs[HexCode.GetString(_currentType.Head)] / 3;
                _currentDataCollection = new Stack<byte>();

                for(int i = 0; i < _currentType.MessageLength + _currentType.TypeFootLength; i++)
                {
                    if (startIndex + i + _currentType.TypeHeadLength >= data.Length) break;
                    _currentDataCollection.Push(data[pairs[HexCode.GetString(_currentType.Head)] / 3 + i + _currentType.TypeHeadLength]);
                    _currentDataLength++;
                }
                if(_currentDataLength == _currentType.MessageLength + _currentType.TypeFootLength)
                {
                    List<byte> popFoot = new List<byte>();
                    for(int i = 0; i < _currentType.TypeFootLength; i++)
                    {
                        popFoot.Add(_currentDataCollection.Pop());
                    }
                    if (popFoot.SequenceEqual(_currentType.Foot))
                    {
                        receivedMessageCallback(new MessageData(_currentType, _currentDataCollection.ToArray()));
                    }
                    else
                    {
                        _currentType = null;
                        _currentDataCollection = null;
                        _currentDataLength = 0;
                    }
                }
                MessageBox.Show(_currentType.TypeName + "\n" + HexCode.GetString(_currentDataCollection.ToArray()) + "\n" + _currentDataLength.ToString());
            }
        }
    }
}
