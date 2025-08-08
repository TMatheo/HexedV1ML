using HarmonyLib;
using LUXED.Core;
using LUXED.Wrappers;
using System;
using System.Reflection;
using VRC.SDK3.Video.Components;
using VRC.SDKBase;

namespace LUXED.Hooking
{
    internal class VRCUnityVideoPlayer_LoadURL
    {
        private delegate void _LoadURLDelegate(int instance, int __0);
        private static _LoadURLDelegate originalMethod;
        public void Initialize()
        {
            EasyHooking.LuxedInstance.Patch(typeof(VRCUnityVideoPlayer).GetMethod("LoadURL"), GetLocalPatch("Patch"));
        }
        private static void Patch(string __0)
        {
            if (string.IsNullOrEmpty(__0)) return;
            VRCUrl url = new VRCUrl(__0);
            if (url == null) return;

            if (InternalSettings.NoVideoPlayer)
            {
                HDLogger.Log($"Video tried loading from {url.url.ToString()}", HDLogger.LogsType.Protection);
                return;
            }
            HDLogger.Log($"Video loading from {url.url.ToString()}", HDLogger.LogsType.Room);
        }
        public static HarmonyMethod GetLocalPatch(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            try
            {
                MethodInfo method = typeof(VRCUnityVideoPlayer_LoadURL).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic);
                if (method == null)
                {
                    HDLogger.Log($"Method '{name}' not found in VRCUnityVideoPlayer_LoadURL.", HDLogger.LogsType.Info);
                    return null;
                }
                return method != null ? new HarmonyMethod(method) : null;
            }
            catch (Exception ex)
            {
                HDLogger.LogError($"Error in GetLocalPatch '{name}': {ex}");
                return null;
            }
        }
    }
}
