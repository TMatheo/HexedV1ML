using HarmonyLib;
using LUXED.Interfaces;
using LUXED.Modules.Standalone;
using LUXED.Wrappers;
using System;
using System.Reflection;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace LUXED.Hooking
{
    internal class UdonBehaviour_SendCustomNetworkEvent : IHook
    {
        public void Initialize()
        {
            EasyHooking.LuxedInstance.Patch(typeof(UdonBehaviour).GetMethod("SendCustomNetworkEvent", new Type[]
            {
                typeof(NetworkEventTarget),
                typeof(string)
            }), new HarmonyMethod(typeof(UdonBehaviour_SendCustomNetworkEvent).GetMethod("OnUdonNetworkEvent", BindingFlags.Static | BindingFlags.NonPublic)), null, null, null, null);
        }
        private static bool OnUdonNetworkEvent(UdonBehaviour __instance, NetworkEventTarget target, string eventName)
        {
            bool flag = eventName != "ListPatrons";
            bool result;
            if (flag)
            {
                result = true;
            }
            else
            {
                VRCPlayer componentInParent = __instance.GetComponentInParent<VRCPlayer>();
                bool flag2 = componentInParent == null || componentInParent == VRCPlayer.field_Internal_Static_VRCPlayer_0;
                if (flag2)
                {
                    result = true;
                } 
                else
                {
                    HDLogger.Log($"Crash attempt from: {componentInParent.field_Private_VRCPlayerApi_0.displayName} has been blocked.", HDLogger.LogsType.Udon);
                    VRConsole.Log($"Crash attempt from: {componentInParent.field_Private_VRCPlayerApi_0.displayName} has been blocked.", VRConsole.LogsType.Udon);
                    result = false;
                }
            }
            return result;
        }
    }
}
