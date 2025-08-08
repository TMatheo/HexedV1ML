using HarmonyLib;
using LUXED.Core;
using LUXED.Interfaces;
using LUXED.Wrappers;
using System.Linq;
using System.Reflection;

namespace LUXED.Hooking
{
    internal class VRCAvatarManager_Update : IHook
    {
        public void Initialize()
        {
            EasyHooking.LuxedInstance.Patch(typeof(VRCAvatarManager).GetMethods().LastOrDefault((MethodInfo x) => x.Name.Equals("Update")), new HarmonyMethod(AccessTools.Method(typeof(VRCAvatarManager_Update), "Hook", null, null)), null, null, null, null);
        }
        internal static void Hook(VRCAvatarManager __instance)
        {
            if (__instance == null) return;
            VRCPlayer player = __instance.field_Private_VRCPlayer_0;
            if (player == null) return;
            GameManager.UpdatePlayerModules(player.GetPhotonPlayer().ActorID());
        }
    }
}
