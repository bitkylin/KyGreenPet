using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace KyGreenPetServer.util
{
    public class BitkyTcpServer
    {
        private IPAddress _ipAddress;
        private int _port;
        private Socket _socketServer;
        private Socket _socketServerSub;
        private Thread _threadServer;
        private Thread _threadServerSub;
        private readonly Random _random;
        private MainWindow _window;

        public string _defaultText { set; get; } = "t12a23s";

        public BitkyTcpServer(MainWindow window)
        {
            _random = new Random();
            _window = window;
        }

        /// <summary>
        ///     客户端操作：开始监听
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <param name="port">端口号</param>
        public void StartListening(IPAddress ipAddress, int port)
        {
            _ipAddress = ipAddress;
            _port = port;

            _threadServer = new Thread(ListentingServerStart); //新建线程
            _threadServer.Start();
            Debug.WriteLine("准备开启TCP服务");
            _window.ControlMessageShow("准备开启TCP服务");
        }

        /// <summary>
        ///     客户端操作：结束监听，关闭服务器连接
        /// </summary>
        public void StopListening()
        {
            _socketServer?.Close();
            _socketServerSub?.Close();
            Debug.WriteLine("已与TCP客户端断开连接");
            _window.ControlMessageShow("已与TCP客户端断开连接");
            _socketServer = null;
            _socketServerSub = null;
        }

        /// <summary>
        ///     新线程运行方法：开启监听本机ip及端口号
        /// </summary>
        private void ListentingServerStart()
        {
            var ipep = new IPEndPoint(_ipAddress, _port); //IPV4地址、端口号
            _socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socketServer.Bind(ipep); //将所创建的套接字与IPEndPoint绑定
            _socketServer.Listen(10);
            try
            {
                //在套接字上接收接入的连接
                Debug.WriteLine("等待TCP客户端响应");
                _window.ControlMessageShow("等待TCP客户端响应");
                _socketServerSub = _socketServer.Accept();
                _threadServerSub = new Thread(ReceiveData); //新建接收数据线程
                _threadServerSub.Start(); //启动线程
            }
            catch (Exception ex)
            {
                Debug.WriteLine("服务端监听链接中断：" + ex.Message);
                _window.ControlMessageShow("服务端监听链接中断：" + ex.Message);
                StopListening();
            }
        }

        /// <summary>
        ///     新的子线程：接收当前Socket的数据
        /// </summary>
        private void ReceiveData() //接收数据
        {
            Debug.WriteLine("开启监听进程");
            _window.ControlMessageShow("开启监听进程");
            var buffer = new byte[1024];
            //根据收听到的客户端套接字向客户端发送信息
            var clienTcp = (IPEndPoint) _socketServerSub.RemoteEndPoint;
            Debug.WriteLine("服务端与客户端（" + clienTcp.Address + " : " + clienTcp.Port + "）连接成功！");
            _window.ControlMessageShow("服务端与客户端（" + clienTcp.Address + " : " + clienTcp.Port + "）连接成功！");
            Debug.WriteLine("成功建立与TCP客户端的连接");
            _window.ControlMessageShow("成功建立与TCP客户端的连接");
            while (true)
            {
                //在套接字上接收客户端发送的信息
                int bufLen;
                try
                {
                    bufLen = _socketServerSub.Available;
                    _socketServerSub.Receive(buffer, 0, bufLen, SocketFlags.None);
                    if (bufLen == 0)
                        continue;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("已与TCP客户端断开连接");
                    _window.ControlMessageShow("已与TCP客户端断开连接");
                    Debug.WriteLine("服务端接收链接中断：" + ex.Message);
                    _window.ControlMessageShow("服务端接收链接中断：" + ex.Message);
                    StopListening();
                    return;
                }

                var replyData = new byte[bufLen];
                Array.Copy(buffer, 0, replyData, 0, bufLen);
                string receivedData = Encoding.ASCII.GetString(replyData);
                _window.CommunicateMessageShow("接收：" + receivedData); //返回接收到的byte[]
                if (receivedData.Equals("read"))
                {
                    Send(_defaultText);
                }
            }
        }

        public void Send(byte[] bytes)
        {
            _socketServerSub?.Send(bytes);
            _window.CommunicateMessageShow("发送：" + Encoding.ASCII.GetString(bytes)); //返回接收到的byte[]
        }

        public void Send(string msg)
        {
            Send(Encoding.ASCII.GetBytes(msg));
        }
    }
}