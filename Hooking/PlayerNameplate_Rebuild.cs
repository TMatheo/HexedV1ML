using HarmonyLib;
using LUXED.Core;
using LUXED.Interfaces;
using LUXED.Modules;
using LUXED.Wrappers;
using System;
using System.Linq;
using System.Reflection;

namespace LUXED.Hooking
{
    internal class PlayerNameplate_Rebuild : IHook
    {
        public void Initialize()
        {
            EasyHooking.LuxedInstance.Patch(typeof(PlayerNameplate).GetMethods().LastOrDefault((MethodInfo x) => x.Name.Equals("Method_Private_Void_Single_Boolean_0")), new HarmonyMethod(AccessTools.Method(typeof(PlayerNameplate_Rebuild), "Hook", null, null)), null, null, null, null);
        }
        private static void Hook(PlayerNameplate __instance)
        {
            if (__instance == null) return;

            VRCPlayer player = __instance.field_Private_VRCPlayer_0;
            if (player == null) return;

            IPlayerModule module = GameManager.GetPlayerModuleByClass(player.GetPhotonPlayer().ActorID(), typeof(PlayerNameplates));
            if (module is PlayerNameplates nameplateModule)
            {
                nameplateModule.UpdatePlayerPlate(__instance);
            }
        }

        public static HarmonyMethod GetLocalPatch(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            try
            {
                MethodInfo method = typeof(PlayerNameplate_Rebuild).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic);
                if (method == null)
                {
                    HDLogger.Log($"Method '{name}' not found in PlayerNameplate_Rebuild.", HDLogger.LogsType.Info);
                    return null;
                }
                return new HarmonyMethod(method);
            }
            catch (Exception ex)
            {
                HDLogger.LogError($"Error in GetLocalPatch '{name}': {ex}");
                return null;
            }
        }
    }
}
