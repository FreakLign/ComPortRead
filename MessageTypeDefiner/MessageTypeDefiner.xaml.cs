using HexDataHandle;
using FileHandle;
using FormHandle;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace MessageTypeDefiner
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageTypeDefinerClass : Window
    {
        private BindControls _controls = new BindControls();

        MessageType type = new MessageType();
        public MessageTypeDefinerClass()
        {
            InitializeComponent();
            this.DataContext = _controls;
        }
        /// <summary>
        /// 拖动窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DropWindow(object sender, MouseButtonEventArgs e)
        {
            WindowHandle.MoveForm(this);
        }
        internal class BindControls : INotifyPropertyChanged
        {
            private void DoBind(string typeName)
            {
                if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(typeName));
            }
            /// <summary>
            /// 类型名
            /// </summary>
            private string _typeName = "";
            public string TypeName
            {
                get { return _typeName; }
                set
                {
                    _typeName = value;
                    DoBind("TypeName");
                }
            }
            /// <summary>
            /// 类型头部
            /// </summary>
            private string _typeHead = "";
            public string TypeHead
            {
                get { return _typeHead; }
                set
                {
                    _typeHead = value;
                    DoBind("TypeHead");
                }
            }
            /// <summary>
            /// 类型尾部
            /// </summary>
            private string _typeFoot = "";
            public string TypeFoot
            {
                get { return _typeFoot; }
                set
                {
                    _typeFoot = value;
                    DoBind("TypeFoot");
                }
            }
            /// <summary>
            /// 类型长度
            /// </summary>
            private int _typeLength;
            public int TypeLength
            {
                get { return _typeLength; }
                set
                {
                    _typeLength = value;
                    DoBind("TypeLength");
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }
        /// <summary>
        /// 导出报文类型文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportJSON(object sender, RoutedEventArgs e)
        {
            if (_controls.TypeFoot != "" && _controls.TypeHead!="" && _controls.TypeName != "")
            {
                type = new MessageType
                {
                    MessageLength = _controls.TypeLength,
                    TypeFoot = Encryption.AESEncrypter.Encrypt(HexCode.GetHex(_controls.TypeFoot), _controls.TypeName),
                    TypeFootLength = HexCode.GetHex(_controls.TypeFoot).Length,
                    TypeHead = Encryption.AESEncrypter.Encrypt(HexCode.GetHex(_controls.TypeHead), _controls.TypeName),
                    TypeHeadLength = HexCode.GetHex(_controls.TypeFoot).Length,
                    TypeName = _controls.TypeName,
                };
                FileHandler.WriteJson(type,(message)=> {
                    SnackbarThree.MessageQueue.Enqueue(message);
                    return "";
                });
            }
        }
        /// <summary>
        /// 退出程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        /// <summary>
        /// 限制只能输入0-9，a-f
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LimitContent(object sender, KeyEventArgs e)
        {
            if((e.Key < Key.D0 ||e.Key > Key.F) && (e.Key <Key.NumPad0 || e.Key > Key.NumPad9) && e.Key!= Key.Back && e.Key != Key.Tab)
            {
                e.Handled = true;
            }
        }
        /// <summary>
        /// 限制只能输入0-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LimitContentInDigital(object sender, KeyEventArgs e)
        {
            if ((e.Key < Key.D0 || e.Key > Key.D9) && (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) && e.Key != Key.Back && e.Key != Key.Tab)
            {
                e.Handled = true;
            }
        }
    }
}
