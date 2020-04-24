
using Notorious;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VRC;

namespace QoL.API
{
    public static class QOLDiscordRPC
    {
        public static DiscordRPC.RichPresence Presence = new DiscordRPC.RichPresence();

        public static string CachedInstance { get; set; }

        public static bool Started = false;

        public static void Start()
        {
            DiscordRPC.EventHandlers eventHandler = default(DiscordRPC.EventHandlers);

            DiscordRPC.Initialize("702340191289081857", ref eventHandler, false, null);

            Started = true;
        }

        public static void Update()
        {
            if (!Started) return;

            if (RoomManager.field_ApiWorld_0.instanceId != CachedInstance)
            {
                Presence.details = "Playing VRChat";

                if (Wrappers.GetPlayerManager().GetAllPlayers().Count > 0)
                {
                    Presence.state = $"With {Wrappers.GetPlayerManager().GetAllPlayers().Count - 1} other player(s)";
                }
                else
                {
                    Presence.state = $"With myself";
                }

                switch (RoomManager.field_ApiWorld_0.currentInstanceAccess)
                {
                    default:
                        Presence.largeImageKey = "il2cpp-dev";
                        Presence.smallImageKey = "unity-logo";
                        Presence.largeImageText = "Using QoL by IL2CPP Dev Team.";
                        Presence.smallImageText = "In the loading screen";
                        break;
                    case VRC.Core.ApiWorldInstance.AccessType.Public:
                        Presence.largeImageKey = "il2cpp-dev";
                        Presence.smallImageKey = "unity-logo";
                        Presence.largeImageText = "Using QoL by IL2CPP Dev Team.";
                        Presence.smallImageText = "In a Public Instance";
                        break;
                    case VRC.Core.ApiWorldInstance.AccessType.FriendsOnly:
                        Presence.largeImageKey = "cute-girl";
                        Presence.smallImageKey = "unity-logo";
                        Presence.largeImageText = "Using QoL by IL2CPP Dev Team.";
                        Presence.smallImageText = "In a Friends Only Instance";
                        break;
                    case VRC.Core.ApiWorldInstance.AccessType.InviteOnly:
                        Presence.largeImageKey = "il2cpp-dev";
                        Presence.smallImageKey = "unity-logo";
                        Presence.largeImageText = "Using QoL by IL2CPP Dev Team.";
                        Presence.smallImageText = "In an Invite Only Instance";
                        break;
                    case VRC.Core.ApiWorldInstance.AccessType.FriendsOfGuests:
                        Presence.largeImageKey = "cute-girl";
                        Presence.smallImageKey = "unity-logo";
                        Presence.largeImageText = "Using QoL by IL2CPP Dev Team.";
                        Presence.smallImageText = "In a Friends+ Instance";
                        break;
                    case VRC.Core.ApiWorldInstance.AccessType.InvitePlus:
                        Presence.largeImageKey = "il2cpp-dev";
                        Presence.smallImageKey = "unity-logo";
                        Presence.largeImageText = "Using QoL by IL2CPP Dev Team.";
                        Presence.smallImageText = "In an Invite+ Instance";
                        break;
                    case VRC.Core.ApiWorldInstance.AccessType.Counter:
                        Presence.largeImageKey = "il2cpp-dev";
                        Presence.smallImageKey = "unity-logo";
                        Presence.largeImageText = "Using QoL by IL2CPP Dev Team.";
                        Presence.smallImageText = "In an Counter Instance";
                        break;
                }

                CachedInstance = RoomManager.field_ApiWorld_0.instanceId;
            }
        }
    }
}
