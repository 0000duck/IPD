using System.Threading.Tasks;

using IPD.Common;
using IPD.Tcp;

namespace IPD.Model
{
    public class Connect : ModuleBase
    {
        private readonly static object lockObj = new object();
        private static Connect instance = null;

        private Client client = Client.GetInstance();

        private bool connectState = false;
        private bool connect7000State = false;

        public static Connect GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new Connect();
                        }
                    }
                }
                return instance;
            }
        }

        public bool ConnectState
        {
            get => connectState; set
            {
                UpdateProperty(ref connectState, value);
                if (value == true)
                {
                    Task.Run(() => initRobot());
                }
            }
        }

        public bool Connect7000State
        {
            get => connect7000State; set
            {
                UpdateProperty(ref connect7000State, value);
            }
        }

        private async Task initRobot()
        {
            await Task.Delay(100);
            client.SendMessage(0x3402, "{}");
            client.SendMessage(0x2e05, "{}");
            client.SendMessage(0x5002, "{}");
            client.SendMessage(0x2e15, "{}");
        }
    }
}