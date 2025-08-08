using LUXED.Core;
using LUXED.Interfaces;
using LUXED.Modules.EventManagement;
using LUXED.Modules.Standalone;
using LUXED.Wrappers;
using System;
using System.Reflection;

namespace LUXED.Hooking
{
    internal class VRCNetworkingClient_OnOperationResponse : IHook
    {
        public void Initialize()
        {
           EasyHooking.Patcher(typeof(VRCNetworkingClient).GetMethod("OnOperationResponse", BindingFlags.Public | BindingFlags.Instance), typeof(VRCNetworkingClient_OnOperationResponse).GetMethod(nameof(Hook), BindingFlags.NonPublic | BindingFlags.Static));
        }
        private static void Hook(VRCNetworkingClient __instance, ExitGames.Client.Photon.OperationResponse __0)
        {
            switch (__0.OperationCode)
            {
                case 226:
                    {
                        {
                            InternalSettings.joinedRoomTime = GeneralUtils.GetUnixTimeInMilliseconds();
                            string WorldID = GameUtils.GetCurrentWorldID();
                            if (WorldID != null)
                            {
                                if (InternalSettings.InstanceHistory.Count > 150) InternalSettings.InstanceHistory.Clear();
                                InternalSettings.InstanceHistory[WorldID] = DateTime.Now;
                            }

                            HDLogger.Log("Connected to Room", HDLogger.LogsType.Room);
                            VRConsole.Log("Connected to Room", VRConsole.LogsType.Room);

                            GameManager.DestroyAllPlayerModules();
                            EventSanitizer.ClearEventBlocks();
                            ModerationHandler.ClearModerations();
                            InternalSettings.ActorsWithLastActiveTime.Clear();
                            PhotonUtils.networkedProperties.Clear();
                        }
                    }
                    break;
                case 254:
                    {
                        HDLogger.Log("Disconnected from Room", HDLogger.LogsType.Room);
                        VRConsole.Log("Disconnected from Room", VRConsole.LogsType.Room);
                    }
                    break;
            }
        }
    }
}
