using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace ComPort
{
    /// <summary>
    /// 串口初始化（初始化接收串口，注册串口接收事件）| 关闭串口 | 发送数据
    /// </summary>
    public class Ports
    {
        private static SerialPort _portOut;//出串口
        private static SerialPort _portEnt;//入串口

        /// <summary>
        /// 初始化接收串口，注册串口接收事件
        /// </summary>
        /// <param name="baudRateOfOut">输入串口名</param>
        /// <param name="baudRateOfEnt">输入串口波特率</param>
        /// <param name="initialEntCallback">输入串口数据接收回调</param>
        public static void InitialEnt(string portNameOfEnt, int baudRateOfEnt, Action<bool, bool, object> initialEntCallback)
        {
            if (portNameOfEnt == null || portNameOfEnt == "")
            {
                initialEntCallback(false, false, "串口初始化失败"); ;
                return;
            }
            _portEnt = new SerialPort(portNameOfEnt);
            _portEnt.BaudRate = baudRateOfEnt;
            try
            {
                _portEnt.Open();
                initialEntCallback(true, false, "接收串口连接成功");
            }
            catch (Exception ex)
            {
                initialEntCallback(false, false, ex.Message);
                return;
            }
            _portEnt.DataReceived += (o, e) =>
            {
                List<byte> recvData = new List<byte>(); ;
                while (_portEnt.BytesToRead > 0)
                {
                    recvData.Add((byte)_portEnt.ReadByte());
                }
                if (recvData.Count > 0) initialEntCallback(true, true, recvData.ToArray());
            };
        }
        /// <summary>
        /// 初始化接收串口，注册串口接收事件
        /// </summary>
        /// <param name="portNameOfOut">输出串口名</param>
        /// <param name="baudRateOfOut">输出串口波特率</param>
        /// <param name="initialOutCallback">初始化回调</param>
        public static void InitialOut(string portNameOfOut, int baudRateOfOut, Action<bool, string> initialOutCallback)
        {

            if (portNameOfOut != null && portNameOfOut != "") _portOut = new SerialPort(portNameOfOut);
            if (portNameOfOut != null && portNameOfOut != "") _portOut.BaudRate = baudRateOfOut;
            try
            {
                _portOut.Open();
            }
            catch (Exception ex)
            {
                initialOutCallback(false, ex.Message);
                return;
            }
            initialOutCallback(true, "发送串口连接成功");
        }
        /// <summary>
        /// 关闭输入串口
        /// </summary>
        public static void CloseEntryPort(Action<bool, string> closedAction)
        {
            try
            {
                if (_portEnt != null && _portEnt.IsOpen) _portEnt.Close();
                closedAction(true, "已关闭接收串口");
            }
            catch (Exception ex)
            {
                closedAction(false, ex.Message);
            }
        }
        /// <summary>
        /// 关闭输出串口
        /// </summary>
        public static void CloseOutPort(Action<bool, string> closedAction)
        {
            try
            {
                if (_portOut != null && _portOut.IsOpen) _portOut.Close();
                closedAction(true, "已关闭发送接口");
            }
            catch (Exception ex)
            {
                closedAction(false, ex.Message);
            }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data">发送的数据</param>
        /// <param name="sendCallback">发送回调</param>
        public static void SendOut(byte[] data, Action<string> sendCallback)
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
        }
    }
}
