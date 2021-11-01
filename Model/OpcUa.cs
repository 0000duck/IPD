using System.Collections.Generic;
using System.Xml;
using IPD.Common;
using Opc.Ua;

namespace IPD.Model
{
    public class OpcUa : ModuleBase
    {
        private readonly static object lockObj = new object();
        private static OpcUa instance = null;

        public static OpcUa GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new OpcUa();
                        }
                    }
                }
                return instance;
            }
        }

        private OpcUaAttributeList opcUaAttributeList;
        public OpcUaAttributeList OpcUaAttributeList { get => opcUaAttributeList; set => UpdateProperty(ref opcUaAttributeList, value); }

        public void ChangeOpcUaAttributeList(OpcUaAttributeList opcUaAttrributeList)
        {
            OpcUaAttributeList = opcUaAttrributeList;
        }
    }

    public class OpcUaAttribute
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string NodeId { get; set; }
    }

    public class OpcUaAttributeList
    {
        public List<OpcUaAttribute> List { get; set; }
    }
}