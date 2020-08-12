using System;
using System.IO.Ports;

namespace ComPort
{
    public class ComPort
    {
        private static SerialPort _portOut;//出串口
        private static SerialPort _portEnt;//入串口

        /// <summary>
        /// 初始化接收串口，注册串口接收事件
        /// </summary>
        /// <param name="baudRateOfOut">输入串口名</param>
        /// <param name="baudRateOfEnt">输入串口波特率</param>
        /// <param name="initialEntCallback">输入串口数据接收回调</param>
        public static void InitialEnt(string portNameOfEnt, int baudRateOfEnt, Func<string,byte[], byte[]> initialEntCallback)
        {

            if (portNameOfEnt == null || portNameOfEnt == "")
            {
                initialEntCallback("串口名为空", null); ;
                return;
            } 
            _portEnt = new SerialPort(portNameOfEnt);
            _portEnt.BaudRate = baudRateOfEnt;
            try
            {
                _portEnt.Open();
            }
            catch(Exception ex)
            {
                initialEntCallback(ex.Message,null);
                return;
            }
            _portEnt.DataReceived += (o, e) =>
            {
                byte[] recvData = new byte[_portEnt.BytesToRead];
                while(_portEnt.BytesToRead > 0)
                {
                    _portEnt.Read(recvData, 0, _portEnt.BytesToRead);
                }
                if(recvData.Length > 0) initialEntCallback("接收到数据", recvData);
            };
        }
        /// <summary>
        /// 初始化接收串口，注册串口接收事件
        /// </summary>
        /// <param name="portNameOfOut">输出串口名</param>
        /// <param name="baudRateOfOut">输出串口波特率</param>
        /// <param name="initialOutCallback">初始化回调</param>
        public static void InitialOut(string portNameOfOut, int baudRateOfOut, string portNameOfEnt, int baudRateOfEnt, Func<string ,byte[]> initialOutCallback)
        {

            if (portNameOfOut != null && portNameOfOut != "") _portOut = new SerialPort(portNameOfOut);
            if (portNameOfEnt != null && portNameOfEnt != "") _portEnt = new SerialPort(portNameOfEnt);
            if (portNameOfOut != null && portNameOfOut != "") _portOut.BaudRate = baudRateOfOut;
            if (portNameOfEnt != null && portNameOfEnt != "") _portEnt.BaudRate = baudRateOfEnt;
            try
            {
                _portOut.Open();
            }
            catch (Exception ex)
            {
                initialOutCallback(ex.Message);
                return;
            }
            initialOutCallback("发送串口连接成功");
        }
        /// <summary>
        /// 关闭输入串口
        /// </summary>
        public static void CloseEntryPort()
        {
            if (_portEnt != null && _portEnt.IsOpen) _portEnt.Close();
        }
        /// <summary>
        /// 关闭输出串口
        /// </summary>
        public static void CloseOutPort()
        {
            if (_portOut != null && _portOut.IsOpen) _portOut.Close();
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data">发送的数据</param>
        /// <param name="sendCallback">发送回调</param>
        public static void SendOut(byte[] data, Func<string, byte[]> sendCallback)
        {
            if (_portOut == null)
            {
                sendCallback("串口未初始化");
                return;
            }
            if (!_portOut.IsOpen)
            {
                sendCallback("串口未打开");
                return;
            }
            _portOut.Write(data, 0, data.Length);
            sendCallback("");
        }
    }
}
