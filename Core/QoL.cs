using Harmony;
using MelonLoader;
using NET_SDK;
using NET_SDK.Harmony;
using Notorious;
using Notorious.API;
using QoL.API;
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
        public static bool DirectionalFlight = false;

        public static List<GameObject> gameObjects = new List<GameObject>();

        public static Vector3 Grav = Physics.gravity;

        public static bool SelectedPlayerESP = false;

        public static List<Component> Components = new List<Component>();

        public static List<Collider> Colliders = new List<Collider>();

        public static bool Noclip = false;

        public static Player TargetRAM { get; set; }

        public override void OnApplicationStart()
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
            }
            catch (Exception e)
            {
                MelonModLogger.LogError("An exception has occurred. Dm Yaekith#1337 on discord. This mod may be outdated.");
                MelonModLogger.LogError(e.ToString());
            }
            finally
            {
                MelonModLogger.Log("Quality of Life has been applied. You can now fucking play this game without being forcekicked, with a hwid spoofer and you can also see people who have blocked you!");
                MelonModLogger.Log("Your New HWID: " + VRC.Core.API.DeviceID);
                MelonModLogger.Log("IsOffline: " + VRC.Core.API.IsOffline());
                MelonModLogger.Log("====================== KEYBINDS ====================");
                MelonModLogger.Log("Press F10 to enable/disable Selected ESP");
                MelonModLogger.Log("Press F9 to enable/disable directional flight.");
                MelonModLogger.Log("Press F11 to enable/disable noclip.");
                MelonModLogger.Log("====================== KEYBINDS ====================");
            }
          
        }
        public override void OnUpdate()
        {
            if (gameObjects.Count() == 0)
            {
                Transform parent = Wrappers.GetQuickMenu().transform.Find("ShortcutMenu");

                if (parent != null)
                {
                    //1, 1 for og
                    var button = ButtonAPI.CreateButton(ButtonType.Toggle, "Flight", "Enable/Disable Flight", Color.white, Color.cyan, 1, -1, parent, new Action(() =>
                    {
                        DirectionalFlight = true;
                        Physics.gravity = DirectionalFlight ? Vector3.zero : Grav;
                        MelonModLogger.Log($"Flight has been {(DirectionalFlight ? "Enabled" : "Disabled")}.");
                    }), new Action(() =>
                    {
                        DirectionalFlight = false;
                        Physics.gravity = DirectionalFlight ? Vector3.zero : Grav;
                        MelonModLogger.Log($"Flight has been {(DirectionalFlight ? "Enabled" : "Disabled")}.");
                    }));

                    var teleportButton = ButtonAPI.CreateButton(ButtonType.Default, "Teleport", "Teleport to the selected player", Color.white, Color.cyan, 0, 0, Wrappers.GetQuickMenu().transform.Find("UserInteractMenu"), new Action(() =>
                    {
                        MelonModLogger.Log("Teleporting to selected player.");
                        Wrappers.GetPlayerManager().GetCurrentPlayer().transform.position = Wrappers.GetQuickMenu().GetSelectedPlayer().transform.position;
                    }));
                    gameObjects.Add(button.gameObject);

                }
            }

            if (Input.GetKeyDown(KeyCode.F10))
            {
                SelectedPlayerESP = !SelectedPlayerESP;
                MelonModLogger.Log($"Selected ESP has been {(SelectedPlayerESP ? "Enabled" : "Disabled")}.");


                GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].transform.Find("SelectRegion"))
                    {
                        array[i].transform.Find("SelectRegion").GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.magenta);
                        HighlightsFX.prop_HighlightsFX_0.EnableOutline(array[i].transform.Find("SelectRegion").GetComponent<Renderer>(), SelectedPlayerESP);
                    }
                }

                foreach (VRC_Pickup pickup in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
                {
                    if (pickup.gameObject.transform.Find("SelectRegion"))
                    {
                        pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.magenta);
                        Wrappers.GetHighlightsFX().EnableOutline(pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>(), SelectedPlayerESP);
                    }
                }

                if (DirectionalFlight)
                {
                    GameObject gameObject = GameObject.Find("Camera (eye)");
                    var currentSpeed = (Input.GetKey(KeyCode.LeftShift) ? 16f : 8f);

                    if (Input.GetKey(KeyCode.W))
                    {
                        Wrappers.GetPlayerManager().GetCurrentPlayer().transform.position += gameObject.transform.forward * currentSpeed * Time.deltaTime;
                    }
                    if (Input.GetKey(KeyCode.A))
                    {
                        Wrappers.GetPlayerManager().GetCurrentPlayer().transform.position += Wrappers.GetPlayerManager().GetCurrentPlayer().transform.right * -1f * currentSpeed * Time.deltaTime;
                    }
                    if (Input.GetKey(KeyCode.S))
                    {
                        Wrappers.GetPlayerManager().GetCurrentPlayer().transform.position += gameObject.transform.forward * -1f * currentSpeed * Time.deltaTime;
                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        Wrappers.GetPlayerManager().GetCurrentPlayer().transform.position += Wrappers.GetPlayerManager().GetCurrentPlayer().transform.right * currentSpeed * Time.deltaTime;
                    }
                    if (Input.GetKey(KeyCode.Space))
                    {
                        Wrappers.GetPlayerManager().GetCurrentPlayer().transform.position += Wrappers.GetPlayerManager().GetCurrentPlayer().transform.up * currentSpeed * Time.deltaTime;
                    }
                    if (Math.Abs(Input.GetAxis("Joy1 Axis 2")) > 0f)
                    {
                        Wrappers.GetPlayerManager().GetCurrentPlayer().transform.position += gameObject.transform.forward * currentSpeed * Time.deltaTime * (Input.GetAxis("Joy1 Axis 2") * -1f);
                    }
                    if (Math.Abs(Input.GetAxis("Joy1 Axis 1")) > 0f)
                    {
                        Wrappers.GetPlayerManager().GetCurrentPlayer().transform.position += gameObject.transform.right * currentSpeed * Time.deltaTime * Input.GetAxis("Joy1 Axis 1");
                    }
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
