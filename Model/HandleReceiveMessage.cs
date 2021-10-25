using NrcTcpLibrary;
using XmlLogLibrary;

namespace IPD.Model
{
    public class HandleReceiveMessage : MessageHandler
    {
        public HandleReceiveMessage()
        {
        }

        private XmlLog log = XmlLog.Instance;

        public override void ConnectState(bool state)
        {
            Connect connect = Connect.GetInstance;
            connect.ConnectState = state;
            if (state == true)
            {
                log.AddLog("连接到控制器", "local", "0");
            }
            else
            {
                log.AddLog("与控制器断开连接", "local", "1");
            }
        }

        private Model.RobotCommon robotCommon = Model.RobotCommon.GetInstance;
        private Model.RobotParameter robotParameter = Model.RobotParameter.GetInstance;
        private Model.ControllerLog controllerLog = Model.ControllerLog.GetInstance;
        private Model.Limit limit = Model.Limit.GetInstance;
        private Model.Versions versions = Model.Versions.GetInstance;
        private Model.ToolParameter toolParameter = Model.ToolParameter.GetInstance;
        private Model.PosVar posVar = Model.PosVar.GetInstance;

        public override void Handler(Message message)
        {
            System.Diagnostics.Debug.WriteLine("接收到:{0:x} 数据:{1:G}", message.command, message.data);
            string m = string.Format("接收到:{0:x} 数据:{1:G}", message.command, message.data);
            log.AddLog(m, "local", "0");
            int command = message.command;
            string data = message.data;
            switch (command)
            {
                case 0x7267:
                    break;

                case 0x5003:
                    robotCommon.CurrentRobotChange(data);
                    break;

                case 0x2a03:
                    robotCommon.ChangePos(data);
                    break;

                case 0x2e03:
                    robotCommon.RobotTypeChange(data);
                    break;

                case 0x2e06:
                    robotCommon.RobotSumChange(data);
                    break;

                case 0x2e16:
                    robotParameter.SlaveConfigChange(data);
                    break;

                case 0x3403:
                    versions.VersionChange(data);
                    break;

                case 0x3807:
                    toolParameter.ChangeToolParameter(data);
                    break;

                case 0x380c:
                    robotCommon.ChangeToolNum(data);
                    break;

                case 0x5606:
                    posVar.ChangeRobotPos(data);
                    break;

                case 0x3a03:
                    robotParameter.DhParameterChange(data);
                    break;

                case 0x3b03:
                    robotParameter.JointParameterChange(data);
                    break;

                case 0x2e09:
                    robotParameter.ControlCycleChange(data);
                    break;

                case 0x2e0f:
                    robotParameter.SlaveTypeListChange(data);
                    break;

                case 0x2e0c:
                    limit.LimitChange(data);
                    break;

                case 0x2e1c:
                    robotParameter.EniNameChange(data);
                    break;

                case 0x2b03:
                    controllerLog.LogParameterChange(data);
                    break;

                case 0x2b04:
                    controllerLog.LogParameterChange(data);
                    break;

                case 0x2b05:
                    controllerLog.LogParameterChange(data);
                    break;

                default:
                    break;
            }
        }
    }
}