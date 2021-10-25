using NrcTcpLibrary;
using XmlLogLibrary;

namespace IPD.Model
{
    public class HandleReceive7000 : MessageHandler
    {
        public HandleReceive7000()
        {
        }

        private XmlLog log = XmlLog.Instance;

        public override void ConnectState(bool state)
        {
            Connect connect = Connect.GetInstance;
            connect.Connect7000State = state;
            if (state == true)
            {
                log.AddLog("7000端口连接成功", "local", "0");
            }
            else
            {
                log.AddLog("7000端口连接失败", "local", "2");
            }
        }

        private Charts charts = Charts.GetInstance;

        public override void Handler(Message message)
        {
            System.Diagnostics.Debug.WriteLine("7000接收到:{0:x} 数据:{1:G}", message.command, message.data);
            //string m = string.Format("7000接收到:{0:x} 数据:{1:G}", message.command, message.data);
            //log.AddLog(m, "local", "0");
            int command = message.command;
            string data = message.data;
            switch (command)
            {
                case 0x9513:
                    charts.ChangeValues(data);
                    break;

                default:
                    break;
            }
        }
    }
}