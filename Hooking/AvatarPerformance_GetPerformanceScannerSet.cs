using HarmonyLib;
using LUXED.Core;
using LUXED.Interfaces;
using LUXED.Wrappers;
using System;
using System.Reflection;    
using VRC.SDKBase.Validation.Performance;

namespace LUXED.Hooking
{
    internal class AvatarPerformance_GetPerformanceScannerSet : IHook
    {
        public void Initialize()
        {
            EasyHooking.LuxedInstance.Patch(typeof(AvatarPerformance).GetMethod("GetPerformanceScannerSet"), GetLocalPatch("Patch"));
        }
        private static bool Patch(bool __0)
        {
            return !InternalSettings.ShowPerformanceStats;
        }
        public static HarmonyMethod GetLocalPatch(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            try
            {
                MethodInfo method = typeof(AvatarPerformance_GetPerformanceScannerSet).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic);
                if (method == null)
                {
                    HDLogger.Log($"Method '{name}' not found in AvatarPerformance_GetPerformanceScannerSet.", HDLogger.LogsType.Info);
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
