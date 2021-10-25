using IPD.Common;
using Newtonsoft.Json;

namespace IPD.Model
{
    public class Versions : ModuleBase
    {
        private readonly static object lockObj = new object();
        private static Versions instance = null;

        public static Versions GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new Versions();
                        }
                    }
                }
                return instance;
            }
        }

        private string ver = "";
        private string rtlVer = "";
        private string jobFileVer = "";
        private bool configFileVersionMisMatch;

        public string Version { get => ver; set => UpdateProperty(ref ver, value); }
        public string RtlVersion { get => rtlVer; set => UpdateProperty(ref rtlVer, value); }
        public string JobFileVersion { get => jobFileVer; set => UpdateProperty(ref jobFileVer, value); }
        public bool ConfigFileVersionMismatch { get => configFileVersionMisMatch; set => UpdateProperty(ref configFileVersionMisMatch, value); }

        public void VersionChange(string data)
        {
            Rootobject v = JsonConvert.DeserializeObject<Rootobject>(data);
            Version = v.version;
            RtlVersion = v.rtlVersion;
            JobFileVersion = v.jobFileVersion;
            ConfigFileVersionMismatch = v.configFileVersionMismatch;
        }
    }

    public class Rootobject
    {
        public bool configFileVersionMismatch { get; set; }
        public string jobFileVersion { get; set; }
        public string rtlVersion { get; set; }
        public string version { get; set; }
    }
}