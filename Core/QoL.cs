using Harmony;
using MelonLoader;
using NET_SDK;
using NET_SDK.Harmony;
using Notorious;
using Notorious.API;
using QoL.API;
using QoL.Mods;
using QoL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnhollowerBaseLib;
using UnityEngine;
using VRC;
using VRC.SDKBase;
using Random = System.Random;

namespace QoL
{
    public class QoL : MelonMod
    {
        public static List<VRCMod> Modules = new List<VRCMod>();
        public override void OnApplicationStart()
        {
            Modules.Add(new Protections());
            Modules.Add(new UIButtons());
            Modules.Add(new InputHandler());
            Modules.ForEach(y => y.OnStart());

            MelonModLogger.Log("====================== KEYBINDS ====================");
            MelonModLogger.Log("Press F9 to enable/disable Avatar Force Clone");
            MelonModLogger.Log("Press F10 to enable/disable Selected ESP");
            MelonModLogger.Log("Press F11 to enable/disable noclip.");
            MelonModLogger.Log("====================== KEYBINDS ====================");

        }
        public override void OnUpdate()
        {
            Modules.Find(x => x.Name == "Input Handler").OnUpdate();
        }
    }
}
