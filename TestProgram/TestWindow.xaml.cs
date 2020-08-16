using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using StaticMessageTypeInRuntime;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FileHandle;
using HexDataHandle;
using DataSplitAndRecognize;
using System.Collections.Concurrent;
using DataSend;
using MessageHandle;
using DataReceiver;

namespace TestProgram
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();

            //foreach (string str in LegalMessageType.LoadTypes())
            //{
            //    OutputBox.Text += str + "\n";
            //}
            Sender sender = new Sender("Com2", 4800, (b, e) => { OutputBox.Text += e + "\n"; });
            Receiver receiver = new Receiver("Com3", 4800, (b1, b, o) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    OutputBox.Text += DateTime.Now.ToString("yyyy-MM-hh-mm-ss-fffffff") + "\t接收数据：" + (b ? HexCode.GetString((byte[])o) + "\n" : (string)o + "\n");
                });
            }, (o) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    OutputBox.Text += DateTime.Now.ToString("yyyy-MM-hh-mm-ss-fffffff") + "\t接收到报文:" + HexCode.GetString(((MessageData)o).CompleteData) + "\n";
                });
            });
            sender.SendMessage(new MessageData(StaticMessageTypeInRuntime.LegalMessageType.MessageTypesDictionaryWithName["B13+"], new byte[] { 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa }), (e) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    OutputBox.Text += DateTime.Now.ToString("yyyy-MM-hh-mm-ss-fffffff") + "\t错误信息" + (string)e;
                });
            });
            sender.SendMessage(new MessageData(StaticMessageTypeInRuntime.LegalMessageType.MessageTypesDictionaryWithName["c7"], new byte[] { 0xdd, 0xdd, 0xcc }), (e) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    OutputBox.Text += DateTime.Now.ToString("yyyy-MM-hh-mm-ss-fffffff") + "\t错误信息" + (string)e;
                });
            });
            sender.SendMessage(new MessageData(StaticMessageTypeInRuntime.LegalMessageType.MessageTypesDictionaryWithName["B13+"], new byte[] { 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa, 0xaa }), (e) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    OutputBox.Text += DateTime.Now.ToString("yyyy-MM-hh-mm-ss-fffffff") + "\t错误信息" + (string)e;
                });
            });
            sender.SendMessage(new MessageData(StaticMessageTypeInRuntime.LegalMessageType.MessageTypesDictionaryWithName["c7"], new byte[] { 0xdd, 0xdd, 0xcc }), (e) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    OutputBox.Text += DateTime.Now.ToString("yyyy-MM-hh-mm-ss-fffffff") + "\t错误信息" + (string)e;
                });
            });
            sender.SendMessage(new MessageData(StaticMessageTypeInRuntime.LegalMessageType.MessageTypesDictionaryWithName["c7"], new byte[] { 0xdd, 0xdd, 0xcc }), (e) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    OutputBox.Text += "错误信息" + (string)e;
                });
            });
        }
    }
}
