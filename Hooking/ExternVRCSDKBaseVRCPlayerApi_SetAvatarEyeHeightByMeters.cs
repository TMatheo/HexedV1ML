﻿using LUXED.Core;
using LUXED.Interfaces;
using LUXED.Modules.Standalone;
using LUXED.Wrappers;
using System.Reflection;
using VRC.Udon.Wrapper.Modules;

namespace LUXED.Hooking
{
    internal class ExternVRCSDKBaseVRCPlayerApi_SetAvatarEyeHeightByMeters : IHook
    {
        public void Initialize()
        {
            EasyHooking.Patcher(typeof(ExternVRCSDKBaseVRCPlayerApi).GetMethod("__SetAvatarEyeHeightByMeters__SystemSingle__SystemVoid", BindingFlags.Public | BindingFlags.Instance), typeof(ExternVRCSDKBaseVRCPlayerApi_SetAvatarEyeHeightByMeters).GetMethod(nameof(SetManualAvatarScalingAllowed), BindingFlags.NonPublic | BindingFlags.Static));
        }
        private static bool SetManualAvatarScalingAllowed()
        {
            if (InternalSettings.NoUdonScaling)
            {
                HDLogger.Log("Prevented Server from scaling your Avatar", HDLogger.LogsType.Protection);
                VRConsole.Log("Server --> Avatar Scaling", VRConsole.LogsType.Protection);
                return false;
            }
            return true;
        }
    }
}
