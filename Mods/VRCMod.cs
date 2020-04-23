using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QoL.Mods
{
    public class VRCMod
    {
        public virtual string Name => "Example Module";

        public virtual string Description => "Example Description";

        public VRCMod()
        {
            MelonModLogger.Log($"VRC Mod {this.Name} has Loaded. {this.Description}");
        }

        public virtual void OnStart()
        {
            new Thread(() =>
            {
                for(; ;)
                {
                    Thread.Sleep(15000);
                    OnUpdate();
                }
            })
            { IsBackground = true }.Start();
        }

        public virtual void OnUpdate()
        {

        }
    }
}
