using HarmonyLib;
using LUXED.Core;
using LUXED.Interfaces;
using LUXED.Modules.EventManagement;
using LUXED.Wrappers;
using System.Linq;
using System.Reflection;

namespace LUXED.Hooking
{
    internal class VRC_EventDispatcherRFC_ValidateAndTriggerEvent : IHook
    {
        void IHook.Initialize()
        {
            EasyHooking.LuxedInstance.Patch(typeof(VRC_EventDispatcherRFC).GetMethods().LastOrDefault(x => x.Name.Equals("Method_Public_Void_Player_VrcEvent_VrcBroadcastType_Int32_Single_0")), new HarmonyMethod(typeof(VRC_EventDispatcherRFC_ValidateAndTriggerEvent).GetMethod(nameof(Hook), BindingFlags.Static | BindingFlags.NonPublic)),null);
        }
        private static bool Hook(VRC_EventDispatcherRFC __instance,[HarmonyArgument(0)] VRC.Player player,[HarmonyArgument(1)] VRC.SDKBase.VRC_EventHandler.VrcEvent vrcEvent,[HarmonyArgument(2)] VRC.SDKBase.VRC_EventHandler.VrcBroadcastType broadcastType,[HarmonyArgument(3)] int instantiationId,[HarmonyArgument(4)] float floatParam)
        {
            if (player == null || vrcEvent == null || vrcEvent.ParameterBytes == null)
                return true;
            object[] decodedParameterBytes = new object[vrcEvent.ParameterBytes.Length];
            if (InternalSettings.RPCLog)
                HDLogger.LogRPC(player, vrcEvent, broadcastType, instantiationId, floatParam, decodedParameterBytes);
            if (EventSanitizer.IsActorEventBlocked(player.GetPhotonPlayer().ActorID(), 6))
                return false;
            if (!EventSanitizer.CheckDecodedRPC(player, vrcEvent, broadcastType, instantiationId, floatParam, decodedParameterBytes))
                return false;
            return true;
        }
    }
}
