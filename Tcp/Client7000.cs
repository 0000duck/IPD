using System.Threading.Tasks;
using IPD.Model;
using IPD.Util;
using NrcTcpLibrary;
using XmlLogLibrary;

namespace IPD.Tcp
{
    internal class Client7000
    {
        private readonly static object lockObj = new object();
        private static Client7000 instance = null;
        private XmlLog log = XmlLog.Instance;
        private Model.ConnectParameter connectParameter = Model.ConnectParameter.GetInstance;

        public static Client7000 GetInstance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new Client7000();
                    }
                }
            }
            return instance;
        }

        public ClientBase clientBase = new ClientBase(new HandleReceive7000());

        public void Connect()
        {
            log.AddLog("连接7000端口", "local", "0");
            Task.Run(() => clientBase.RunClientAsync(connectParameter.IP, 7000));
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