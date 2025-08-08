using HarmonyLib;
using LUXED.Interfaces;
using LUXED.Modules.EventManagement;
using LUXED.Wrappers;
using Photon.Realtime;
using System;
using System.Linq;
using System.Reflection;
using VRC.SDKBase;

namespace LUXED.Hooking
{
    internal class VRC_EventDispatcherRFC_Hook
    {
        public void Initialize()
        {
            EasyHooking.LuxedInstance.Patch(typeof(VRC_EventDispatcherRFC).GetMethods().LastOrDefault((MethodInfo x) => x.Name.Equals("Method_Public_Boolean_Player_VrcEvent_VrcBroadcastType_0")), new HarmonyMethod(AccessTools.Method(typeof(VRC_EventDispatcherRFC_Hook), "RPCPatch", null, null)), null, null, null, null);
        }
        private static bool RPCPatch(VRC.Player param_1, VRC_EventHandler.VrcEvent param_2, VRC_EventHandler.VrcBroadcastType param_3)
        {
            try
            {
                HDLogger.LogDebug($"[RPCPatch] Player: {param_1.prop_String_0 ?? "null"}");
                HDLogger.LogDebug($"[RPCPatch] Event Name: {param_2.EventType.ToString() ?? "null"}");
                HDLogger.LogDebug($"[RPCPatch] BroadcastType: {param_3}");

                var result = EventSanitizer.CheckDecodedRPC(param_1, param_2, param_3, 1, 1f, new object[0]);

                HDLogger.LogDebug($"[RPCPatch] CheckDecodedRPC returned: {result}");
            }
            catch (Exception ex)
            {
                HDLogger.LogError(ex.ToString());
            }
            return true;
        }
    }
}
