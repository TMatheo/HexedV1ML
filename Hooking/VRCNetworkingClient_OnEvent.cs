using HarmonyLib;
using Il2CppSystem.Collections;
using LUXED.Core;
using LUXED.Extensions;
using LUXED.Interfaces;
using LUXED.Modules.EventManagement;
using LUXED.Modules.Standalone;
using LUXED.Wrappers;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LUXED.Hooking
{
    internal class VRCNetworkingClient_OnEvent : IHook
    {
        public void Initialize()
        {
            EasyHooking.LuxedInstance.Patch(typeof(LoadBalancingClient).GetMethods().LastOrDefault((MethodInfo x) => x.Name.Equals("OnEvent")), new HarmonyMethod(AccessTools.Method(typeof(VRCNetworkingClient_OnEvent), "OnEvent", null, null)), null, null, null, null);
        }
        internal static bool OnEvent(object __0)
        {
            var eventData = (ExitGames.Client.Photon.EventData)__0;
            switch (eventData.Code)
            {
                case 1:
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code))
                            return false;
                        byte[] Data = (byte[])CPP2IL.BinaryConverter.IL2CPPToManaged(eventData.CustomData);
                        if (!EventSanitizer.SanitizeEvent1(eventData.Sender, Data, eventData.Code))
                            return false;
                        if (eventData.Sender == InternalSettings.RepeatVoiceActor)
                            ExploitHandler.RepeatVoiceEvents(Data);
                        break;
                    }
                case 2:
                    {
                        if (eventData.CustomData == null)
                            return false;
                        string customData = CPP2IL.BinaryConverter.IL2CPPToManaged(eventData.CustomData) as string;
                        HDLogger.Log($"The Server disconnected you: {customData}", HDLogger.LogsType.Room);
                        VRConsole.Log($"Server --> Disconnect", VRConsole.LogsType.Moderation);
                    }
                    break;

                case 3:
                    {
                        Player PhotonPlayer = GameHelper.VRCNetworkingClient.GetPhotonPlayer(eventData.Sender);
                        string Displayname = PhotonPlayer.DisplayName() ?? "NO NAME";

                        HDLogger.Log($"{Displayname} requested Cache", HDLogger.LogsType.Room);
                        VRConsole.Log($"{Displayname} --> Cache request", VRConsole.LogsType.Room);
                    }
                    break;

                case 4:
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code))
                            return false;

                        byte[][] Data = (byte[][])CPP2IL.BinaryConverter.IL2CPPToManaged(eventData.CustomData);
                        Player PhotonPlayer = GameHelper.VRCNetworkingClient.GetPhotonPlayer(eventData.Sender);
                        string Displayname = PhotonPlayer.DisplayName() ?? "NO NAME";
                        HDLogger.Log($"{Displayname} sent Cache with {Data.Length} Events", HDLogger.LogsType.Room);
                        VRConsole.Log($"{Displayname} --> Cache [{Data.Length}]", VRConsole.LogsType.Room);
                        return EventSanitizer.SanitizeEvent4(eventData.Sender, Data, eventData.Code);
                    }

                case 5:
                    {
                        Player PhotonPlayer = GameHelper.VRCNetworkingClient.GetPhotonPlayer(eventData.Sender);
                        string Displayname = PhotonPlayer.DisplayName() ?? "NO NAME";
                        HDLogger.Log($"{Displayname} confirmed join", HDLogger.LogsType.Room);
                        VRConsole.Log($"{Displayname} --> Join confirmation", VRConsole.LogsType.Room);
                    }
                    break;

                case 6:
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code))
                            return false;
                        byte[] Data = (byte[])CPP2IL.BinaryConverter.IL2CPPToManaged(eventData.CustomData);
                        return EventSanitizer.SanitizeEvent6(eventData.Sender, Data, eventData.Code);
                    }

                case 7:
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code))
                            return false;
                        byte[] Data = (byte[])CPP2IL.BinaryConverter.IL2CPPToManaged(eventData.CustomData);
                        return EventSanitizer.SanitizeEvent7(eventData.Sender, Data, eventData.Code);
                    }

                case 10: // Unknown, some udon stuff
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code))
                            return true;

                        // Fixed: Consistent method IL2CPPToManaged instead of IL2ManagedObject
                        byte[] Data = (byte[])CPP2IL.BinaryConverter.IL2CPPToManaged(eventData.CustomData);
                        return EventSanitizer.SanitizeEvent10(eventData.Sender, Data, eventData.Code);
                    }
                    break;

                case 11: // Udon Sync Objects
                    {
                        if (InternalSettings.NoUdonSync || eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code))
                            return true;

                        byte[] Data = (byte[])CPP2IL.BinaryConverter.IL2CPPToManaged(eventData.CustomData);
                        return EventSanitizer.SanitizeEvent11(eventData.Sender, Data, eventData.Code);
                    }
                    break;

                case 13: // Reliable Serialization / SDK3 Avatar
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code))
                            return true;

                        byte[] Data = (byte[])CPP2IL.BinaryConverter.IL2CPPToManaged(eventData.CustomData);
                        return EventSanitizer.SanitizeEvent13(eventData.Sender, Data, eventData.Code);
                    }
                    break;

                case 16: // Unreliable Item Serialization
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code))
                            return true;

                        byte[] Data = (byte[])CPP2IL.BinaryConverter.IL2CPPToManaged(eventData.CustomData);
                        return EventSanitizer.SanitizeEvent16(eventData.Sender, Data, eventData.Code);
                    }
                    break;

                case 17: // Unknown, maybe unreliable object serialization or sync or something?
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code))
                            return true;

                        byte[] Data = (byte[])CPP2IL.BinaryConverter.IL2CPPToManaged(eventData.CustomData);
                        return EventSanitizer.SanitizeEvent17(eventData.Sender, Data, eventData.Code);
                    }
                    break;

                case 21: // Udon Ownership (Unknown Object rn i guess its Sync)
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code))
                            return true;

                        int[] OwnershipData = (int[])CPP2IL.BinaryConverter.IL2CPPToManaged(eventData.CustomData);

                        if (!EventSanitizer.SanitizeEvent21(eventData.Sender, OwnershipData, eventData.Code))
                        {
                            return false;
                        }

                        switch (InternalSettings.AntiPickup) // idk if needed for this event tbh
                        {
                            case InternalSettings.AntiPickupMode.Self:
                                if (ItemHelper.IsViewIdPickupable(OwnershipData[0]) && OwnershipData[1] != GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID())
                                {
                                    PhotonHelper.RaiseSyncOwnership(OwnershipData[0], GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID());
                                }
                                break;

                            case InternalSettings.AntiPickupMode.Nobody:
                                if (ItemHelper.IsViewIdPickupable(OwnershipData[0]) && OwnershipData[1] != 0)
                                {
                                    PhotonHelper.RaiseSyncOwnership(OwnershipData[0], 0);
                                }
                                break;
                        }
                    }
                    break;

                case 22: // Udon Ownership (Items)
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code))
                            return true;

                        int[] OwnershipData = (int[])CPP2IL.BinaryConverter.IL2CPPToManaged(eventData.CustomData);

                        if (!EventSanitizer.SanitizeEvent22(eventData.Sender, OwnershipData, eventData.Code))
                            return true;

                        switch (InternalSettings.AntiPickup)
                        {
                            case InternalSettings.AntiPickupMode.Self:
                                if (ItemHelper.IsViewIdPickupable(OwnershipData[0]) && OwnershipData[1] != GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID())
                                {
                                    PhotonHelper.RaiseItemOwnership(OwnershipData[0], GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID());
                                }
                                break;

                            case InternalSettings.AntiPickupMode.Nobody:
                                if (ItemHelper.IsViewIdPickupable(OwnershipData[0]) && OwnershipData[1] != 0)
                                {
                                    PhotonHelper.RaiseItemOwnership(OwnershipData[0], 0);
                                }
                                break;
                        }
                    }
                    break;

                case 33: // Moderations
                    {
                        Dictionary<byte, object> moderationData = (Dictionary<byte, object>)CPP2IL.BinaryConverter.IL2CPPToManaged(eventData.CustomData);

                        return ModerationHandler.ReceivedModerationEvent(ref moderationData);
                    }

                case 34: //Apply Ratelimits
                    {
                        HDLogger.Log($"Prevented Server from adjusting Ratelimits", HDLogger.LogsType.Protection);
                        VRConsole.Log($"Server --> Ratelimit Adjust", VRConsole.LogsType.Protection);
                        __0 = eventData.Pointer;
                    }
                    break;

                case 35: // Reset Ratelimits
                    {
                        EventSanitizer.BlacklistedBytes.Clear();
                    }
                    break;

                case 42: // CustomProp Update
                    {
                        // Fix: Should return false if CustomData is null, else attempt processing
                        if (eventData.CustomData != null)
                        {
                            return UserPropHandler.ReceivedPropEvent(eventData.CustomData.TryCast<Hashtable>(), GameHelper.VRCNetworkingClient.GetPhotonPlayer(eventData.Sender));
                        }
                        return false;
                    }
                    break;

                case 43: // Chatbox message
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code))
                            return false;

                        Player PhotonPlayer = GameHelper.VRCNetworkingClient.GetPhotonPlayer(eventData.Sender);

                        return ChatHandler.ReceivedChatEvent(eventData.CustomData.ToString(), PhotonPlayer, eventData.Code);
                    }

                case 202: // Instantiation
                    {
                        if (eventData.CustomData != null)
                        {
                            return EventSanitizer.SanitizeEvent202(eventData.Sender, eventData.CustomData.Cast<Hashtable>(), eventData.Code);
                        }
                        return false;
                    }

                case 210:
                    {
                        if (eventData.CustomData == null || EventSanitizer.IsActorEventBlocked(eventData.Sender, eventData.Code))
                            break;

                        int[] OwnershipData = (int[])CPP2IL.BinaryConverter.IL2CPPToManaged(eventData.CustomData);

                        if (!EventSanitizer.SanitizeEvent210(eventData.Sender, OwnershipData, eventData.Code))
                            break;

                        switch (InternalSettings.AntiPickup)
                        {
                            case InternalSettings.AntiPickupMode.Self:
                                if (ItemHelper.IsViewIdPickupable(OwnershipData[0]) && OwnershipData[1] != GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID())
                                {
                                    PhotonHelper.RaiseLegacyOwnership(OwnershipData[0], GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID());
                                    break;
                                }
                                break;

                            case InternalSettings.AntiPickupMode.Nobody:
                                if (ItemHelper.IsViewIdPickupable(OwnershipData[0]) && OwnershipData[1] != 0)
                                {
                                    PhotonHelper.RaiseLegacyOwnership(OwnershipData[0], 0);
                                }
                                break;
                        }
                    }
                    break;

                case 254: // Player Disconnect
                    {
                        Player player = GameHelper.VRCNetworkingClient.GetPhotonPlayer(eventData.Sender);

                        if (player != null)
                        {
                            string Displayname = player.DisplayName() ?? "NO NAME";
                            string UserID = player.UserID() ?? "NO USERID";

                            GameManager.DestroyPlayerModules(player.ActorID());

                            EventSanitizer.RemoveActorBlocks(eventData.Sender);
                            if (InternalSettings.ActorsWithLastActiveTime.ContainsKey(eventData.Sender)) InternalSettings.ActorsWithLastActiveTime.Remove(eventData.Sender);
                            if (PhotonUtils.networkedProperties.ContainsKey(eventData.Sender)) PhotonUtils.networkedProperties.Remove(eventData.Sender); // is that even still used, needed whatever??

                            HDLogger.Log($"[ - ] {Displayname} [{UserID}]", HDLogger.LogsType.Room);
                            VRConsole.Log($"{Displayname}", VRConsole.LogsType.Disconnect);
                        }

                        if (eventData.Parameters.ContainsKey(203))
                        {
                            int newMasterActor = eventData[203].Unbox<int>();

                            Player master = GameHelper.VRCNetworkingClient.GetPhotonPlayer(newMasterActor);
                            if (master == null)
                            {
                                HDLogger.Log($"Prevented Server from setting invalid Master", HDLogger.LogsType.Protection);
                                VRConsole.Log($"Server --> Invalid Master Client", VRConsole.LogsType.Protection);
                                return false;
                            }
                            else
                            {
                                string Displayname = master.DisplayName() ?? "NO NAME";
                                HDLogger.Log($"{Displayname} is now Master", HDLogger.LogsType.Room);
                                VRConsole.Log($"{Displayname} --> Master Client", VRConsole.LogsType.Room);
                            }
                        }
                    }
                    break;

                case 255: // Player Connect
                    {
                        string Displayname = "NO NAME";
                        string UserID = "NO USERID";

                        Hashtable props = eventData[249].TryCast<Hashtable>();
                        if (props != null && props.ContainsKey("user"))
                        {
                            var Table = props["user"].TryCast<Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>>();
                            if (Table != null)
                            {
                                if (Table.ContainsKey("displayName")) Displayname = Table["displayName"]?.ToString();
                                if (Table.ContainsKey("id")) UserID = Table["id"]?.ToString();
                            }
                        }
                        HDLogger.Log($"[ + ] {Displayname} [{UserID}]", HDLogger.LogsType.Room);
                        VRConsole.Log($"{Displayname}", VRConsole.LogsType.Connect);
                    }
                    break;
            }
            return true;
        }
    }
}