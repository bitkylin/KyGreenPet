using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KyGreenPetServer.util;

namespace KyGreenPetServer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private BitkyTcpServer _bitkyTcpServer;

        public MainWindow()
        {
            InitializeComponent();
            _bitkyTcpServer = new BitkyTcpServer(this);
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            _bitkyTcpServer.StartListening(IPAddress.Parse(textBoxIP.Text.Trim()), 5000);
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (_bitkyTcpServer != null)
            {
                string msg = textBoxSend.Text;
                if (msg.Length == 7 && msg.Substring(0, 1).Equals("t"))
                {
                    _bitkyTcpServer._defaultText = msg;
                }
            }
        }


        /// <summary>
        ///     控制信息的显示
        /// </summary>
        /// <param name="message">输入所需显示的信息</param>
        public void ControlMessageShow(string message)
        {
            Dispatcher.Invoke(() =>
            {
                ListBoxControlText.Items.Add(message);
                ListBoxControlText.SelectedIndex = ListBoxControlText.Items.Count - 1;
                ListBoxControlText.ScrollIntoView(ListBoxControlText.Items[ListBoxControlText.Items.Count - 1]);
            });
        }

        /// <summary>
        ///     控制信息的显示
        /// </summary>
        /// <param name="message">输入所需显示的信息</param>
        public void CommunicateMessageShow(string message) //通信信息
        {
            Dispatcher.Invoke(() =>
            {
                ListBoxCommunicationText.Items.Add(message);
                ListBoxCommunicationText.SelectedIndex = ListBoxCommunicationText.Items.Count - 1;
                ListBoxCommunicationText.ScrollIntoView(
                    ListBoxCommunicationText.Items[ListBoxCommunicationText.Items.Count - 1]);
            });
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            _bitkyTcpServer.StopListening();
        }
    }
}