using System.Collections.Generic;
using IPD.Common;
using Newtonsoft.Json;

namespace IPD.Model
{
    public class PosVar : ModuleBase
    {
        private readonly static object lockObj = new object();
        private static PosVar instance = null;

        public static PosVar GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new PosVar();
                        }
                    }
                }
                return instance;
            }
        }

        private RobotPosRoot robotPos;
        public RobotPosRoot RobotPos { get => robotPos; set => UpdateProperty(ref robotPos, value); }

        public void ChangeRobotPos(string data)
        {
            RobotPosRoot v = JsonConvert.DeserializeObject<RobotPosRoot>(data);
            RobotPos = v;
        }
    }

    public class RobotPosRoot
    {
        /// <summary>
        /// </summary>
        public string note { get; set; }

        /// <summary>
        /// </summary>
        public string posName { get; set; }

        /// <summary>
        /// </summary>
        public List<double> posValue { get; set; }

        /// <summary>
        /// </summary>
        public List<double> posValueDeg { get; set; }

        /// <summary>
        /// </summary>
        public int robot { get; set; }
    }
}