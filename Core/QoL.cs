using Harmony;
using MelonLoader;
using NET_SDK;
using NET_SDK.Harmony;
using Notorious;
using Notorious.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC;
using Random = System.Random;

namespace QoL
{
    public class QoL : MelonMod
    {
        public static bool Flight = false;

        public static List<GameObject> gameObjects = new List<GameObject>();

        public static Vector3 Grav = Physics.gravity;

        public override void OnApplicationStart()
        {
            try
            { 
                var HarmonyInstance = Manager.CreateInstance("Quality Assurance");
                var API = SDK.GetClass("VRC.Core", "API");
                var Amp = NET_SDK.SDK.GetClass("AmplitudeSDKWrapper", "AmplitudeWrapper");
                var moderationManager = SDK.GetClass("ModerationManager");
                HarmonyInstance.Patch(API.GetMethod("DeviceID"), AccessTools.Method(typeof(QoL), "HWIDSpoofer"));
                HarmonyInstance.Patch(Amp.GetMethod("InitializeDeviceId"), AccessTools.Method(typeof(QoL), "HWIDSpoofer"));
                HarmonyInstance.Patch(API.GetMethod("IsOffline"), AccessTools.Method(typeof(QoL), "TruePrefix"));
            }
            catch(Exception e)
            {
                MelonModLogger.LogError("An exception has occurred. Dm Yaekith#1337 on discord. This mod may be outdated.");
                MelonModLogger.LogError(e.ToString());
            }
            finally
            {
                MelonModLogger.Log("Quality of Life has been applied. You can now fucking play this game without being forcekicked, with a hwid spoofer and you can also see people who have blocked you!");
                MelonModLogger.Log("Your New HWID: " + VRC.Core.API.DeviceID);
                MelonModLogger.Log("IsOffline: " + VRC.Core.API.IsOffline());
            }
          
        }
        public override void OnUpdate()
        {
            if (gameObjects.Count() == 0)
            {
                Transform parent = Wrappers.GetQuickMenu().transform.Find("ShortcutMenu");

                if (parent != null)
                {
                    var button = ButtonAPI.CreateButton(ButtonType.Toggle, "Flight", "Enable/Disable Flight (now in vr kek)", Color.white, Color.cyan, 1, 1, parent, new Action(() =>
                    {
                        Flight = true;
                        Physics.gravity = Flight ? Vector3.zero : Grav;
                        MelonModLogger.Log($"Flight has been {(Flight ? "Enabled." : "Disabled")}");
                    }), new Action(() =>
                    {
                        Flight = false;
                        Physics.gravity = Flight ? Vector3.zero : Grav;
                        MelonModLogger.Log($"Flight has been {(Flight ? "Enabled." : "Disabled")}");
                    }));

                    gameObjects.Add(button.gameObject);
                }
            }
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
