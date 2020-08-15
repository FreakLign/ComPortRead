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

            foreach (string str in LegalMessageType.LoadTypes())
            {
                OutputBox.Text += str + "\n";
            }
            Spliter.Split(new byte[] {0x9e, 0x8f, 0x11, 0x12, 0x13, 0x11, 0x12, 0x13, 0x11, 0x12, 0x13, 0x11, 0x12, 0x13, 0x11, 0x12, 0x13, 0x11, 0x12, 0x13, 0xcc }, LegalMessageType.MessageTypesDictionaryWithHead);
        }
    }
}
