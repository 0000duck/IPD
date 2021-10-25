using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPD.Common;
using Newtonsoft.Json;

namespace IPD.Model
{
    public class Limit : ModuleBase
    {
        private readonly static object lockObj = new object();
        private static Limit instance = null;

        public static Limit GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new Limit();
                        }
                    }
                }
                return instance;
            }
        }

        private LimitRoot limits;
        public LimitRoot Limits { get => limits; set => UpdateProperty(ref limits, value); }

        public void LimitChange(string data)
        {
            LimitRoot v = JsonConvert.DeserializeObject<LimitRoot>(data);
            Limits = v;
        }
    }

    public class CraftLimit
    {
        /// <summary>
        /// </summary>
        public bool lasercut { get; set; }

        /// <summary>
        /// </summary>
        public bool pallet { get; set; }

        /// <summary>
        /// </summary>
        public bool polish { get; set; }

        /// <summary>
        /// </summary>
        public bool pun { get; set; }

        /// <summary>
        /// </summary>
        public bool search { get; set; }

        /// <summary>
        /// </summary>
        public bool spray { get; set; }

        /// <summary>
        /// </summary>
        public bool vision { get; set; }

        /// <summary>
        /// </summary>
        public bool weld { get; set; }
    }

    public class RobottypeLimit
    {
        /// <summary>
        /// </summary>
        public bool R_FOURAXIS { get; set; }

        /// <summary>
        /// </summary>
        public bool R_FOURAXIS_PALLET { get; set; }

        /// <summary>
        /// </summary>
        public bool R_FOURAXIS_PALLET_1 { get; set; }

        /// <summary>
        /// </summary>
        public bool R_FOUR_CARTESIAN_COORDINATE { get; set; }

        /// <summary>
        /// </summary>
        public bool R_FOUR_POLAR_COORDINATE_1 { get; set; }

        /// <summary>
        /// </summary>
        public bool R_GENERAL_1S { get; set; }

        /// <summary>
        /// </summary>
        public bool R_GENERAL_5S { get; set; }

        /// <summary>
        /// </summary>
        public bool R_GENERAL_6S { get; set; }

        /// <summary>
        /// </summary>
        public bool R_GENERAL_6S_1 { get; set; }

        /// <summary>
        /// </summary>
        public bool R_GENERAL_6S_2 { get; set; }

        /// <summary>
        /// </summary>
        public bool R_GENERAL_7S { get; set; }

        /// <summary>
        /// </summary>
        public bool R_SCARA { get; set; }

        /// <summary>
        /// </summary>
        public bool R_SCARA_1 { get; set; }

        /// <summary>
        /// </summary>
        public bool R_SCARA_THREEAXIS { get; set; }

        /// <summary>
        /// </summary>
        public bool R_SCARA_TWOAXIS { get; set; }

        /// <summary>
        /// </summary>
        public bool R_SIXAXIS_SPRAY_BBR { get; set; }

        /// <summary>
        /// </summary>
        public bool R_THREE_CARTESIAN_COORDINATE { get; set; }

        /// <summary>
        /// </summary>
        public bool R_THREE_CARTESIAN_COORDINATE_1 { get; set; }
    }

    public class LimitRoot
    {
        /// <summary>
        /// </summary>
        public CraftLimit craft { get; set; }

        /// <summary>
        /// </summary>
        public int robotsum { get; set; }

        /// <summary>
        /// </summary>
        public RobottypeLimit robottype { get; set; }
    }
}