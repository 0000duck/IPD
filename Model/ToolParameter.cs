using System.Threading.Tasks;

using IPD.Common;
using Newtonsoft.Json;

namespace IPD.Model
{
    public class ToolParameter : ModuleBase
    {
        private readonly static object lockObj = new object();
        private static ToolParameter instance = null;

        public static ToolParameter GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new ToolParameter();
                        }
                    }
                }
                return instance;
            }
        }

        private ToolRoot tool = new ToolRoot() { tool = new Tool() { A = 0, B = 0, C = 0, x = 0, y = 0, z = 0, note = "" }, toolDeg = new ToolDeg() { A = 0, B = 0, C = 0, x = 0, y = 0, z = 0, note = "" }, toolNum = 0 };

        public ToolRoot Tool
        {
            get => tool; set => UpdateProperty(ref tool, value);
        }

        public void ChangeToolParameter(string data)
        {
            ToolRoot v = JsonConvert.DeserializeObject<ToolRoot>(data);
            Tool = v;
        }
    }

    public class Tool
    {
        /// <summary>
        /// </summary>
        public double A { get; set; }

        /// <summary>
        /// </summary>
        public double B { get; set; }

        /// <summary>
        /// </summary>
        public double C { get; set; }

        /// <summary>
        /// </summary>
        public string note { get; set; }

        /// <summary>
        /// </summary>
        public double x { get; set; }

        /// <summary>
        /// </summary>
        public double y { get; set; }

        /// <summary>
        /// </summary>
        public double z { get; set; }
    }

    public class ToolDeg
    {
        /// <summary>
        /// </summary>
        public double A { get; set; }

        /// <summary>
        /// </summary>
        public double B { get; set; }

        /// <summary>
        /// </summary>
        public double C { get; set; }

        /// <summary>
        /// </summary>
        public string note { get; set; }

        /// <summary>
        /// </summary>
        public double x { get; set; }

        /// <summary>
        /// </summary>
        public double y { get; set; }

        /// <summary>
        /// </summary>
        public double z { get; set; }
    }

    public class ToolRoot
    {
        /// <summary>
        /// </summary>
        public Tool tool { get; set; }

        /// <summary>
        /// </summary>
        public ToolDeg toolDeg { get; set; }

        /// <summary>
        /// </summary>
        public int toolNum { get; set; }
    }
}