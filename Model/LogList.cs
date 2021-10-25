using System.Collections.Generic;
using System.Xml;
using IPD.Common;
using IPD.Tcp;
using XmlLogLibrary;

namespace IPD.Model
{
    public class LogList : ModuleBase
    {
        private readonly static object lockObj = new object();
        private static LogList instance = null;

        private List<LogBase> logs;

        public static LogList GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new LogList();
                        }
                    }
                }
                return instance;
            }
        }

        public List<LogBase> Logs { get => logs; set => UpdateProperty(ref logs, value); }

        public void LogListChange(List<LogBase> list)
        {
            Logs = list;
        }
    }
}