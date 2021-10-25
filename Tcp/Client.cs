using System.Threading.Tasks;
using IPD.Model;
using IPD.Util;
using NrcTcpLibrary;
using XmlLogLibrary;

namespace IPD.Tcp
{
    internal class Client
    {
        private readonly static object lockObj = new object();
        private static Client instance = null;
        private XmlLog log = XmlLog.Instance;
        private ConnectParameter connectParameter = ConnectParameter.GetInstance;

        public static Client GetInstance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new Client();
                    }
                }
            }
            return instance;
        }

        public ClientBase clientBase = new ClientBase(new HandleReceiveMessage());

        public void Connect(string ip, int port)
        {
            Config config = new Config();
            config.AddConnected(ip, port);
            log.AddLog("尝试连接ip:" + ip + " port:" + port.ToString(), "local", "0");
            connectParameter.ChangeConnected(ip, port);
            Task.Run(() => clientBase.RunClientAsync(ip, port));
        }

        public void SendMessage(int command, string data)
        {
            clientBase.SendMessage(command, data);
        }

        public void SendMessage(int command, byte[] data)
        {
            clientBase.SendMessage(command, data);
        }

        public void DisConnect()
        {
            clientBase.clientChannel.CloseAsync();
        }
    }
}