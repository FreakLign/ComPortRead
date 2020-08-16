using ComPort;
using DataSplitAndRecognize;
using MessageHandle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReceiver
{
    public class Receiver
    {
        public Receiver(string portName, int  baudRate, Action<bool,object> receivedDataAction, Action<object> receivedMessageData)
        {
            Ports.InitialEnt(portName, baudRate, (status,data) =>
            {
                if (status)
                {
                    receivedDataAction(true, (byte[])data);//接收实际数据回传
                    Spliter.Split((byte[])data, StaticMessageTypeInRuntime.LegalMessageType.MessageTypesDictionaryWithHead,(msgData)=> {
                        receivedMessageData((MessageData)msgData);//接收数据中识别报文回传
                    });
                }
                else
                {
                    receivedDataAction(false, (string)data);//返回错误信息
                }
            });
        }
        public static void DoIt()
        {
        }
    }
}
