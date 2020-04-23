using MelonLoader;
using Notorious;
using QoL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC.SDKBase;

namespace QoL.Mods
{
    public class InputHandler : VRCMod
    {
        public override string Name => "Input Handler";

        public override string Description => "This module handles all the given user input.";

        public InputHandler() : base() { }

        public override void OnStart() {}
        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F9))
            {
                GlobalUtils.ForceClone = !GlobalUtils.ForceClone;
            }

            if (GlobalUtils.ForceClone)
            {
                UserInteractMenu userInteractMenu = Wrappers.GetUserInteractMenu();

                userInteractMenu.cloneAvatarButton.interactable = true;
            }

            if (Input.GetKeyDown(KeyCode.F10))
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
            }

            if (GlobalUtils.DirectionalFlight)
            {
                GameObject gameObject = Wrappers.GetPlayerCamera();
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
}
