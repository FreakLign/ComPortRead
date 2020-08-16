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
        /// <summary>
        /// 实例化接收串口，注册接收事件
        /// </summary>
        /// <param name="portName">串口名</param>
        /// <param name="baudRate">串口波特率</param>
        /// <param name="receivedDataAction">当串口接收到数据时bool 为True，object为byte[]，当返回错误信息时，bool为false，object为string</param>
        /// <param name="receivedMessageData">当内容被识别为报文时，进行此方法</param>
        public Receiver(string portName, int baudRate, Action<bool, bool, object> receivedDataAction, Action<object> receivedMessageData)
        {
            Ports.InitialEnt(portName, baudRate, (isConnected, status, data) =>
            {
                if (isConnected && status)
                {
                    receivedDataAction(true, true, (byte[])data);//接收实际数据回传
                    Spliter.Split((byte[])data, StaticMessageTypeInRuntime.LegalMessageType.MessageTypesDictionaryWithHead, (msgData) =>
                    {
                        receivedMessageData((MessageData)msgData);//接收数据中识别报文回传
                    });
                }
                else
                {
                    receivedDataAction(isConnected, status, (string)data);//返回状态信息
                }
            });
        }
    }
}
