using HarmonyLib;
using LUXED.Modules.Standalone;
using LUXED.Wrappers;
using System;
using System.Reflection;
using Transmtn;
using VRC.Core;

namespace LUXED.Hooking
{
    internal class PhoneBook_Handle
    {
        public void Initialize()
        {
            var targetMethod = typeof(PhoneBook).GetMethod("Handle", BindingFlags.Public | BindingFlags.Static) ?? typeof(PhoneBook).GetMethod("Handle", BindingFlags.Public | BindingFlags.Instance);
            var prefixMethod = typeof(PhoneBook_Handle).GetMethod("PhotonPatch", BindingFlags.NonPublic | BindingFlags.Static);
            if (prefixMethod != null)
            {
                EasyHooking.LuxedInstance.Patch(targetMethod, prefix: new HarmonyMethod(prefixMethod));
            } 
        }
        private static void PhotonPatch(Transmtn.DTO.User user, FriendMessageType __1)
        {
            switch (__1)
            {
                case FriendMessageType.Add:
                    {
                        HDLogger.Log($"You and {user.displayName} friended", HDLogger.LogsType.Friends);
                        VRConsole.Log($"{user.displayName} --> Friend", VRConsole.LogsType.Friend);
                    }
                    break;
                case FriendMessageType.Delete:
                    {
                        HDLogger.Log($"You and {user.displayName} unfriended", HDLogger.LogsType.Friends);
                        VRConsole.Log($"{user.displayName} --> Unfriend", VRConsole.LogsType.Friend);
                    }
                    break;
                case FriendMessageType.Location:
                    {
                        if (user.location.isPrivate)
                        {
                            HDLogger.Log($"{user.displayName} is now in PRIVATE", HDLogger.LogsType.Friends);
                            VRConsole.Log($"{user.displayName} --> PRIVATE", VRConsole.LogsType.World);
                        }
                        else if (user.location != null && user.location.isTraveling && user.location.WorldId != null)
                        {
                            InstanceAccessType Type = GameUtils.GetWorldType(user.location.WorldId);
                            string LocalizedType = GameUtils.GetLocalizedWorldType(Type);

                            ApiWorld world = new ApiWorld() { id = user.location.WorldId.Split(':')[0] };
                            world.Fetch(new Action<ApiContainer>((container) =>
                            {
                                ApiModelContainer<ApiWorld> apiWorld = new ApiModelContainer<ApiWorld>();
                                apiWorld.setFromContainer(container);
                                ApiWorld World = container.Model.Cast<ApiWorld>();

                                HDLogger.Log($"{user.displayName} is now in {World.name} [{LocalizedType}] [{user.location.WorldId}]", HDLogger.LogsType.Friends);
                                VRConsole.Log($"{user.displayName} --> {World.name} [{LocalizedType}]", VRConsole.LogsType.World);
                            }), new Action<ApiContainer>((container) =>
                            {
                                HDLogger.Log($"{user.displayName} is now in UNKNOWN [{LocalizedType}] [{user.location.WorldId}]", HDLogger.LogsType.Friends);
                                VRConsole.Log($"{user.displayName} --> UNKNOWN [{LocalizedType}]", VRConsole.LogsType.World);
                            }));
                        }
                    }
                    break;
                case FriendMessageType.Online:
                    {
                        PlayerUtils.APIDevice Platform = PlayerUtils.GetAPIDevice(user.last_platform);
                        HDLogger.Log($"{user.displayName} is now Online [{Platform}]", HDLogger.LogsType.Friends);
                        VRConsole.Log($"{user.displayName} [{Platform}]", VRConsole.LogsType.Online);
                    }
                    break;
                case FriendMessageType.Offline:
                    {
                        HDLogger.Log($"{user.displayName} is now Offline", HDLogger.LogsType.Friends);
                        VRConsole.Log(user.displayName, VRConsole.LogsType.Offline);
                    }
                    break;
                case FriendMessageType.Active:
                    {
                        HDLogger.Log($"{user.displayName} is now Online [Website]", HDLogger.LogsType.Friends);
                        VRConsole.Log($"{user.displayName} [Website]", VRConsole.LogsType.Online);
                    }
                    break;
            }
        }
    }
}
