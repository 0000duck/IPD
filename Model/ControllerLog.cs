using IPD.Common;
using Newtonsoft.Json;
using XmlLogLibrary;

namespace IPD.Model
{
    public class ControllerLog : ModuleBase
    {
        private readonly static object lockObj = new object();
        private static ControllerLog instance = null;

        private ControllerLogParameter logParameter;
        private XmlLog log = XmlLog.Instance;

        public static ControllerLog GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new ControllerLog();
                        }
                    }
                }
                return instance;
            }
        }

        public ControllerLog()
        {
            ControllerLogParameter log = new ControllerLogParameter();
            log.code = 0;
            log.data = "无信息";
            log.kind = 0;
            log.robot = 1;
            LogParameter = log;
        }

        public ControllerLogParameter LogParameter { get => logParameter; set => UpdateProperty(ref logParameter, value); }

        public void LogParameterChange(string data)
        {
            ControllerLogParameter clp = JsonConvert.DeserializeObject<ControllerLogParameter>(data);
            LogParameter = clp;
            log.AddLog(clp.data, "controller", clp.kind.ToString());
        }
    }

    public class ControllerLogParameter
    {
        /// <summary>
        ///
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 伺服未连接
        /// </summary>
        public string data { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int kind { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int robot { get; set; }
    }
}