using System.Collections.Generic;
using System.Xml;
using IPD.Common;
using IPD.Tcp;
using XmlLogLibrary;

namespace IPD.Model
{
    public class ConnectParameter : ModuleBase
    {
        private readonly static object lockObj = new object();
        private static ConnectParameter instance = null;
        private string ip;
        private int port;

        public static ConnectParameter GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new ConnectParameter();
                        }
                    }
                }
                return instance;
            }
        }

        public string IP { get => ip; set => UpdateProperty(ref ip, value); }
        public int Port { get => port; set => UpdateProperty(ref port, value); }

        public void ChangeConnected(string ipa, int porta)
        {
            IP = ipa;
            Port = porta;
        }
    }
}