using System.Diagnostics;
using System.Text;
using cn.bmob.api;
using KyGreenPet.bean;
using KyGreenPet.util;

namespace KyGreenPet.presenter
{
    public class Presenter
    {
        private MainWindow _mainWindow;
        private BitkyTcpClient _tcpClient;

        public Presenter(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            _tcpClient = new BitkyTcpClient(this);
        }

        public void CommunicateMessageShow(string msg)
        {
            _mainWindow.CommunicateMessageShow(msg);
        }

        public void ControlMessageShow(string msg)
        {
            _mainWindow.ControlMessageShow(msg);
        }

        public void initTcpClient(string ip, int port)
        {
            _tcpClient.Build(ip, port);
        }

        public void processReceivedData(byte[] replyData)
        {
            string msg = Encoding.ASCII.GetString(replyData);
            if (msg.Length == 7 && msg.Substring(0, 1).Equals("t"))
            {
                BmobWindows bmobWindows = CloudServiceHelper.BmobBuilder();
                int lowValue = int.Parse(msg.Substring(1, 2));
                int highValue = int.Parse(msg.Substring(4, 2));
                int value = (lowValue + highValue)/2;
                bool humid = msg.Substring(6, 1).Equals("s") ? true : false;


                KyOperation kyOperation = new KyOperation(value, humid);
                kyOperation.objectId = "429af4fea3";
                bmobWindows.UpdateTaskAsync(kyOperation);
                KyCommand command = new KyCommand();
                bmobWindows.Get<KyCommand>("KyCommand", "6402c5a401", (result, ex) =>
                {
                    if (ex == null)
                    {
                        if (result.isOpenedLed.Get())
                        {
                            _tcpClient.Send("opled");
                            result.isOpenedLed = false;
                            bmobWindows.UpdateTaskAsync(result);
                        }
                    }
                });
            }
        }

        public void setReceiveBytes(byte[] replyData)
        {
            _mainWindow.setReceiveBytes(replyData);
        }

        public void Send(byte[] getBytes)
        {
            _tcpClient.Send(getBytes);
        }

        public void close()
        {
            _tcpClient.Close();
        }

        public void stopCloudData()
        {
            KyOperation kyOperation = new KyOperation();
            kyOperation.isOpened = false;
            kyOperation.objectId = "429af4fea3";
            BmobWindows bmobWindows = CloudServiceHelper.BmobBuilder();
            bmobWindows.UpdateTaskAsync(kyOperation);
        }
    }
}