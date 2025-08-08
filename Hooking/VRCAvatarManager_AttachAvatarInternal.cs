using HarmonyLib;
using LUXED.Interfaces;
using LUXED.Modules.Standalone;
using System.Linq;
using System.Reflection;
using UnityEngine;
using VRC.SDKBase;

namespace LUXED.Hooking
{
    internal class VRCAvatarManager_AttachAvatarInternal : IHook
    {
        public void Initialize()
        {
            (from m in typeof(VRCPlayer).GetMethods()
             where m.Name.StartsWith("Method_Private_Void_GameObject_VRC_AvatarDescriptor_")
             select m).ToList<MethodInfo>().ForEach(delegate (MethodInfo m)
             {
                 EasyHooking.LuxedInstance.Patch(typeof(VRCPlayer).GetMethod(m.Name), EasyHooking.GetLocalPatch<VRCAvatarManager_AttachAvatarInternal>("Hook"), null, null, null, null);
             });
        }
        private static void Hook(GameObject __0, VRC_AvatarDescriptor __1, VRCPlayer __instance)
        {
            if (__instance == null)
            {
                return;
            }
            AvatarSanitizer.SanitizeAvatarObject(__0, __instance);
        }
    }
}
