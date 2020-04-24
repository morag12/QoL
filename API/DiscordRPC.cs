using System;
using System.Runtime.InteropServices;
using System.IO;


public class DiscordRPC
{
    public struct EventHandlers
    {
        public DiscordRPC.ReadyCallback readyCallback;
        public DiscordRPC.DisconnectedCallback disconnectedCallback;
        public DiscordRPC.ErrorCallback errorCallback;
        public DiscordRPC.JoinCallback joinCallback;
        public DiscordRPC.SpectateCallback spectateCallback;
        public DiscordRPC.RequestCallback requestCallback;
    }


    [Serializable]
    public struct RichPresence
    {
        public string state;
        public string details;
        public long startTimestamp;
        public long endTimestamp;
        public string largeImageKey;
        public string largeImageText;
        public string smallImageKey;
        public string smallImageText;
        public string partyId;
        public int partySize;
        public int partyMax;
        public string matchSecret;
        public string joinSecret;
        public string spectateSecret;
        public bool instance;
    }


    [Serializable]
    public struct JoinRequest
    {
        public string userId;
        public string username;
        public string discriminator;
        public string avatar;
    }


    public enum Reply
    {
        No,
        Yes,
        Ignore
    }

    [DllImport("Dependencies/discord-rpc.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_Initialize")]
    public static extern void Initialize(string applicationId, ref DiscordRPC.EventHandlers handlers, bool autoRegister, string optionalSteamId);

    [DllImport("Dependencies/discord-rpc.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_Shutdown")]
    public static extern void Shutdown();

    [DllImport("Dependencies/discord-rpc.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_RunCallbacks")]
    public static extern void RunCallbacks();

    [DllImport("Dependencies/discord-rpc.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_UpdatePresence")]
    public static extern void UpdatePresence(ref DiscordRPC.RichPresence presence);

    [DllImport("Dependencies/discord-rpc.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_ClearPresence")]
    public static extern void ClearPresence();

    [DllImport("Dependencies/discord-rpc.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_Respond")]
    public static extern void Respond(string userId, DiscordRPC.Reply reply);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ReadyCallback();


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DisconnectedCallback(int errorCode, string message);


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ErrorCallback(int errorCode, string message);



    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void JoinCallback(string secret);



    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SpectateCallback(string secret);



    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void RequestCallback(ref DiscordRPC.JoinRequest request);


}