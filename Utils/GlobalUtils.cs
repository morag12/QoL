using Notorious;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRCSDK2;

namespace QoL.Utils
{
    public static class GlobalUtils
    {
        public static bool DirectionalFlight = false;

        public static Vector3 Grav = Physics.gravity;

        public static bool SelectedPlayerESP = false;

        public static bool ThirdPerson = false;

        public static bool ForceClone = false;

        public static bool Serialise = true;

        public static void ToggleColliders(bool toggle)
        {
            Collider[] array = UnityEngine.Object.FindObjectsOfType<Collider>();
            Component component = PlayerWrappers.GetCurrentPlayer().GetComponents(Collider.Il2CppType).FirstOrDefault<Component>();

            for (int i = 0; i < array.Length; i++)
            {
                Collider collider = array[i];
                bool flag = collider.GetComponent<PlayerSelector>() != null || collider.GetComponent<VRC.SDKBase.VRC_Pickup>() != null || collider.GetComponent<QuickMenu>() != null || collider.GetComponent<VRC_Station>() != null || collider.GetComponent<VRC.SDKBase.VRC_AvatarPedestal>() != null;
                
                if (!flag && collider != component)
                {
                    collider.enabled = toggle;
                }
            }
        }
    }
}
