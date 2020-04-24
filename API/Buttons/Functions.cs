using QoL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MelonLoader;
using Notorious;
using VRC.SDKBase;

namespace QoL.API
{
    public class Functions : QMNestedButton
    {
        public Functions() : base("ShortcutMenu", 5, -1, "QoL", "VRChat Quality of Life, made by Yaekith and the IL2CPP Research Team.", Color.cyan, Color.white, Color.cyan, Color.white)
        {
            new QMToggleButton(this, 1, 0, "Flight\nON", new Action(() => 
            {
                GlobalUtils.DirectionalFlight = true;
                Physics.gravity = GlobalUtils.DirectionalFlight ? Vector3.zero : GlobalUtils.Grav;
                MelonModLogger.Log($"Flight has been {(GlobalUtils.DirectionalFlight ? "Enabled" : "Disabled")}.");
            }), "Flight\nOFF", new Action(() =>
            {
                GlobalUtils.DirectionalFlight = false;
                Physics.gravity = GlobalUtils.DirectionalFlight ? Vector3.zero : GlobalUtils.Grav;
                MelonModLogger.Log($"Flight has been {(GlobalUtils.DirectionalFlight ? "Enabled" : "Disabled")}.");
            }), "Come join us!", Color.cyan, Color.white);

            new QMToggleButton(this, 1, 0, "ESP\nON", new Action(() =>
            {
                GlobalUtils.SelectedPlayerESP = !GlobalUtils.SelectedPlayerESP;

                MelonModLogger.Log($"Selected ESP has been {(GlobalUtils.SelectedPlayerESP ? "Enabled" : "Disabled")}.");


                GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].transform.Find("SelectRegion"))
                    {
                        array[i].transform.Find("SelectRegion").GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.magenta);
                        HighlightsFX.prop_HighlightsFX_0.EnableOutline(array[i].transform.Find("SelectRegion").GetComponent<Renderer>(), GlobalUtils.SelectedPlayerESP);
                    }
                }

                foreach (VRC_Pickup pickup in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
                {
                    if (pickup.gameObject.transform.Find("SelectRegion"))
                    {
                        pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.magenta);
                        Wrappers.GetHighlightsFX().EnableOutline(pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>(), GlobalUtils.SelectedPlayerESP);
                    }
                }
            }), "ESP\nOFF", new Action(() =>
            {
                GlobalUtils.SelectedPlayerESP = !GlobalUtils.SelectedPlayerESP;

                MelonModLogger.Log($"Selected ESP has been {(GlobalUtils.SelectedPlayerESP ? "Enabled" : "Disabled")}.");


                GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].transform.Find("SelectRegion"))
                    {
                        array[i].transform.Find("SelectRegion").GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.magenta);
                        HighlightsFX.prop_HighlightsFX_0.EnableOutline(array[i].transform.Find("SelectRegion").GetComponent<Renderer>(), GlobalUtils.SelectedPlayerESP);
                    }
                }

                foreach (VRC_Pickup pickup in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
                {
                    if (pickup.gameObject.transform.Find("SelectRegion"))
                    {
                        pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.magenta);
                        Wrappers.GetHighlightsFX().EnableOutline(pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>(), GlobalUtils.SelectedPlayerESP);
                    }
                }
            }), "Come join us!", Color.cyan, Color.white);
        }
    }
}
