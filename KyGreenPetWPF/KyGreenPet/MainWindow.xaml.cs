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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KyGreenPet.presenter;
using KyGreenPet.util;

namespace KyGreenPet
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private CloudServiceHelper _cloudServiceHelper;
        private Presenter _presenter;

        public MainWindow()
        {
            InitializeComponent();
            _presenter = new Presenter(this);
            _cloudServiceHelper = new CloudServiceHelper();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
           
            _presenter.initTcpClient(textBoxIP.Text.Trim(), 5000);
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

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string msg = textBoxSend.Text;
            _presenter.Send(Encoding.ASCII.GetBytes(msg));
        }

        public void setReceiveBytes(byte[] replyData)
        {
            _presenter.processReceivedData(replyData);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            _presenter.close();
        }
    }
}