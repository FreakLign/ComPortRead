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
            MessageType typeTest = LegalMessageType.MessageTypesDictionaryWithName["B13"];
            MessageBox.Show(HexCode.GetString(typeTest.Head));
        }
    }
}
