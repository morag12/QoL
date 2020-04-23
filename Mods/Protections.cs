using Harmony;
using MelonLoader;
using NET_SDK;
using NET_SDK.Harmony;
using NET_SDK.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VRC;

namespace QoL.Mods
{
    public class Protections : VRCMod
    {
        public override string Name => "Protections";

        public override string Description => "A module for QoL which gives you the basic necessities of not being forcekicked and yadadada";

        public Protections() : base() { }

        public override void OnStart()
        {
            try
            {
                var HarmonyInstance = Manager.CreateInstance("Quality Assurance");
                var API = SDK.GetClass("VRC.Core", "API");
                var USpeak = SDK.GetClass("USpeakPhotonSender3D");
                var Amp = NET_SDK.SDK.GetClass("AmplitudeSDKWrapper", "AmplitudeWrapper");
                var moderationManager = SDK.GetClass("ModerationManager");
                HarmonyInstance.Patch(API.GetMethod("DeviceID"), AccessTools.Method(typeof(QoL), "HWIDSpoofer"));
                HarmonyInstance.Patch(Amp.GetMethod("InitializeDeviceId"), AccessTools.Method(typeof(QoL), "HWIDSpoofer"));
                HarmonyInstance.Patch(API.GetMethod("IsOffline"), AccessTools.Method(typeof(QoL), "TruePrefix"));
                HarmonyInstance.Patch(moderationManager.GetMethod("KickUserRPC"), AccessTools.Method(typeof(Protections), "KickUserPatch"));
                IL2CPP_Method[] methods = moderationManager.GetMethods(x => (x.HasFlag(NET_SDK.Reflection.IL2CPP_BindingFlags.METHOD_PUBLIC) && (x.GetParameterCount() == 3) && x.GetReturnType().Name.Equals("System.Boolean")));
                for (int i = 0; i < methods.Length; i++)
                {
                    HarmonyInstance.Patch(methods[i], AccessTools.Method(typeof(Main), "falsePatch"));
                }
            }
            catch (Exception e)
            {
                MelonModLogger.LogError("An exception has occurred. Dm Yaekith#1337 on discord. This mod may be outdated.");
                MelonModLogger.LogError(e.ToString());
            }
            finally
            {
                MelonModLogger.Log("Protections have been applied.");
                MelonModLogger.Log("Your New HWID: " + VRC.Core.API.DeviceID);
                MelonModLogger.Log("IsOffline: " + VRC.Core.API.IsOffline());
            }
        }
        public void KickUserPatch(string _1, string _2, string _3, string _4, Player _5)
        {
            return;
        }

        public bool falsePatch(string _1, string _2, string _3)
        {
            return false;

        }
        public static bool TruePrefix(ref bool __result)
        {
            __result = true;
            return true;
        }

        public static string HWIDSpoofer()
        {
            var crypt = new SHA256Managed();
            string hash = string.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(new Random().Next(1, 999999999).ToString()));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }
    }
}
