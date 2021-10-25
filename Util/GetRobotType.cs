using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using IPD.Model;

namespace IPD.Util
{
    public class GetRobotType
    {
        public static RobotTypes GetRobot(int num)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(Resource.RobotType);
            XmlNode rootNode = xml.SelectSingleNode("a");
            XmlNodeList rootChildList = rootNode.ChildNodes;
            RobotTypes robotTypes = new RobotTypes();
            for (int i = 0; i < rootChildList.Count; i++)
            {
                XmlNode cnode = rootChildList.Item(i);
                XmlNodeList cnodeChild = cnode.ChildNodes;
                if (int.Parse(cnodeChild.Item(3).InnerText) == num)
                {
                    robotTypes.AxisSum = int.Parse(cnodeChild.Item(0).InnerText);
                    robotTypes.En = cnodeChild.Item(2).InnerText;
                    robotTypes.Name = cnodeChild.Item(1).InnerText;
                    robotTypes.Num = int.Parse(cnodeChild.Item(3).InnerText);
                    break;
                }
            }
            return robotTypes;
        }

        public static RobotTypes GetRobot(string En)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(Resource.RobotType);
            XmlNode rootNode = xml.SelectSingleNode("a");
            XmlNodeList rootChildList = rootNode.ChildNodes;
            RobotTypes robotTypes = new RobotTypes();
            for (int i = 0; i < rootChildList.Count; i++)
            {
                XmlNode cnode = rootChildList.Item(i);
                XmlNodeList cnodeChild = cnode.ChildNodes;
                if (cnodeChild.Item(2).InnerText == En)
                {
                    robotTypes.AxisSum = int.Parse(cnodeChild.Item(0).InnerText);
                    robotTypes.En = cnodeChild.Item(2).InnerText;
                    robotTypes.Name = cnodeChild.Item(1).InnerText;
                    robotTypes.Num = int.Parse(cnodeChild.Item(3).InnerText);
                    break;
                }
            }
            return robotTypes;
        }

        public static List<RobotTypes> getRobotTypeList()
        {
            List<RobotTypes> robotTypeList = new List<RobotTypes>();
            XDocument doc = XDocument.Parse(Resource.RobotType);
            XElement root = doc.Element("a");
            root.Elements().ToList().ForEach(v =>
            {
                RobotTypes r = new RobotTypes();
                r.AxisSum = int.Parse(v.Element("AxisSum").Value);
                r.En = v.Element("En").Value;
                r.Name = v.Element("Name").Value;
                r.Num = int.Parse(v.Element("Num").Value);
                robotTypeList.Add(r);
            });
            return robotTypeList;
        }
    }
}