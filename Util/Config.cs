using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using IPD.Common;
using System.Globalization;

namespace IPD.Util
{
    internal class Config
    {
        private readonly static object lockObj = new object();
        private static Config instance = null;

        public static Config GetInstance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new Config();
                    }
                }
            }
            return instance;
        }

        private readonly string configPath = Directory.GetCurrentDirectory() + "\\config.xml";

        public Config()
        {
        }

        public void InitConfig()
        {
            if (File.Exists(configPath))
            {
                return;
            }
            else

            {
                string initConfigString = Resource.initConfig;
                FileStream newConfigFile = File.Create(configPath);
                newConfigFile.Write(System.Text.Encoding.UTF8.GetBytes(initConfigString));
                newConfigFile.Close();
            }
        }

        public void AddConnected(string ip, int port)
        {
            XDocument configFile = XDocument.Load(configPath);
            XElement connected = configFile.Element("root").Element("connected");
            List<XElement> connectedList = connected.Elements().ToList();
            for (int i = 0; i < connectedList.Count; i++)
            {
                if (connectedList[i].Element("ip").Value == ip && int.Parse(connectedList[i].Element("port").Value) == port)
                {
                    connected.Elements().ElementAt(i).Remove();
                    break;
                }
            }
            if (connected.Elements().Count() == 5)
            {
                connected.Elements().ElementAt(4).Remove();
            }
            connected.AddFirst(new XElement("item", new XElement("ip", ip), new XElement("port", port.ToString())));
            configFile.Save(configPath);
        }

        public List<ConnectParameter> ConnectParameterList()
        {
            List<ConnectParameter> connectParameters = new List<ConnectParameter>();
            XDocument configFile = XDocument.Load(configPath);
            XElement connected = configFile.Element("root").Element("connected");
            List<XElement> connectedItems = connected.Elements().ToList();
            connectedItems.ForEach(action =>
            {
                ConnectParameter connectParameter = new ConnectParameter();
                connectParameter.ip = action.Element("ip").Value;
                connectParameter.port = int.Parse(action.Element("port").Value, CultureInfo.InvariantCulture);
                connectParameters.Add(connectParameter);
            });
            return connectParameters;
        }
    }
}