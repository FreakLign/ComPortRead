using HexDataHandle;
using MessageHandle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCompare
{
    public class Comparer
    {
        public class avoidException : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                return x != y;
            }

            public int GetHashCode(string obj)
            {
                return obj.GetHashCode();
            }
        }
        public static void Reset()
        {
            SendDatas = new Dictionary<string, MessageData>(new avoidException());
            RecvDatas = new Dictionary<string, MessageData>(new avoidException());
        }
        public static Dictionary<string, MessageData> SendDatas = new Dictionary<string, MessageData>(new avoidException())
            , RecvDatas = new Dictionary<string, MessageData>(new avoidException());
        public static void RecordSendData(MessageData data)
        {
            if (SendDatas == null) SendDatas = new Dictionary<string, MessageData>(new avoidException());
            SendDatas.Add(HexCode.GetString(data.CompleteData), data);
        }
        public static string RecordRecvData(MessageData data,Action<int> whenLostData)
        {
            if (RecvDatas == null) RecvDatas = new Dictionary<string, MessageData>(new avoidException());
            RecvDatas.Add(HexCode.GetString(data.CompleteData), data);
            return GetCompareResult(whenLostData);
        }
        private static string GetCompareResult(Action<int> /* 丢失差值 */ whenLostData)
        {
            if(SendDatas.Count == RecvDatas.Count)
            {
                if(SendDatas.Last().Key == RecvDatas.Last().Key)
                {
                    SendDatas.Remove(SendDatas.Last().Key);
                    RecvDatas.Remove(RecvDatas.Last().Key);
                    return "正确";
                }
                else
                {
                    SendDatas.Remove(SendDatas.Last().Key);
                    RecvDatas.Remove(RecvDatas.Last().Key);
                    return "错误";
                }
            }
            else
            {
                if (SendDatas.Count < RecvDatas.Count)
                {
                    SendDatas = new Dictionary<string, MessageData>(new avoidException());
                    RecvDatas = new Dictionary<string, MessageData>(new avoidException());
                    return "异常";
                }
                MessageData temp;
                if(SendDatas.Last().Key == RecvDatas.Last().Key)
                {
                    whenLostData(SendDatas.Count - RecvDatas.Count);
                    SendDatas = new Dictionary<string, MessageData>(new avoidException());
                    RecvDatas = new Dictionary<string, MessageData>(new avoidException());
                    return "正确";
                }
                if (SendDatas.TryGetValue(RecvDatas.Last().Key,out temp))
                {
                    temp = null;
                    SendDatas.Remove(RecvDatas.Last().Key);
                    RecvDatas.Remove(RecvDatas.Last().Key);
                    return "超时";
                }
                temp = null;
                whenLostData(SendDatas.Count - RecvDatas.Count);
                SendDatas = new Dictionary<string, MessageData>(new avoidException());
                RecvDatas = new Dictionary<string, MessageData>(new avoidException());
                return "错误";
            }
        }
    }
}
