using IPD.Common;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace IPD.Model
{
    public class ServoParameter : ModuleBase
    {
        private static readonly object lockObj = new object();
        private static ServoParameter instance = null;

        public static ServoParameter GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new ServoParameter();
                        }
                    }
                }
                return instance;
            }
        }

        public class ServoParameterItem
        {
            public string name { get; set; }
            public string comment { get; set; }
        }

        public class ServoParameterRoot
        {
            public List<ServoParameterItem> PA { get; set; }
        }
    }
}