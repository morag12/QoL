using MelonLoader;
using Notorious;
using Notorious.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using QoL.Utils;

namespace QoL.Mods
{
    public class UIButtons : VRCMod
    {
        private static List<GameObject> Buttons = new List<GameObject>();

        public override string Name => "UI Buttons";

        public override string Description => "This module adds more buttons to the menu.";

        public UIButtons() : base() {}

        public override void OnStart() 
        {
            try
            {
                if (Buttons.Count() == 0)
                {
                    Transform parent = Wrappers.GetQuickMenu().transform.Find("ShortcutMenu");

                    if (parent != null)
                    {
                        var button = ButtonAPI.CreateButton(ButtonType.Toggle, "Flight", "Enable/Disable Flight", Color.white, Color.cyan, 1, -1, parent, new Action(() =>
                        {
                            GlobalUtils.DirectionalFlight = true;
                            Physics.gravity = GlobalUtils.DirectionalFlight ? Vector3.zero : GlobalUtils.Grav;
                            MelonModLogger.Log($"Flight has been {(GlobalUtils.DirectionalFlight ? "Enabled" : "Disabled")}.");
                        }), new Action(() =>
                        {
                            GlobalUtils.DirectionalFlight = false;
                            Physics.gravity = GlobalUtils.DirectionalFlight ? Vector3.zero : GlobalUtils.Grav;
                            MelonModLogger.Log($"Flight has been {(GlobalUtils.DirectionalFlight ? "Enabled" : "Disabled")}.");
                        }));

                        var teleportButton = ButtonAPI.CreateButton(ButtonType.Default, "Teleport", "Teleport to the selected player", Color.white, Color.cyan, 0, 0, Wrappers.GetQuickMenu().transform.Find("UserInteractMenu"), new Action(() =>
                        {
                            MelonModLogger.Log("Teleporting to selected player.");
                            Wrappers.GetPlayerManager().GetCurrentPlayer().transform.position = Wrappers.GetQuickMenu().transform.position;
                        }));

                        Buttons.Add(button.gameObject);
                    }
                }
            }
            catch (Exception) { }
        }

        public override void OnUpdate()
        {

        }
    }
}
