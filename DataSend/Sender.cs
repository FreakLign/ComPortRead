using ComPort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSend
{
    public class Sender
    {
        public Sender(string portName, int baudRate) 
        {
            Ports.InitialOut(portName, baudRate, (e) => { });
        }
    }
}
