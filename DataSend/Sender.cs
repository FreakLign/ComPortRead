using ComPort;
using MessageHandle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSend
{
    public class Sender
    {
        public enum SendMode { ManualMode, AutoSendMode}
        public enum MessageMode { FixedMessage, RandomMessage}
        public Sender(string portName, int baudRate, Action<bool,string> errorAction)
        {
            Ports.InitialOut(portName, baudRate, errorAction);
        }
        public void SendMessage(MessageData data, Action<string> sendDataErrorCallback)
        {
            Ports.SendOut(data.CompleteData, sendDataErrorCallback);
        }
    }
}
