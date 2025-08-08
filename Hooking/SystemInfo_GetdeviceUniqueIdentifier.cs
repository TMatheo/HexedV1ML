using LUXED.Interfaces;
using LUXED.Wrappers;
using System;
using System.Linq;
using System.Threading;
using UnhollowerBaseLib;
using UnityEngine;
using VRC.Core;

namespace LUXED.Hooking
{
    internal class SystemInfo_GetdeviceUniqueIdentifier : IHook
    {
        private static Il2CppSystem.Object FakeHWID;
        private static bool SetFreshHWID = false;
        private static int OriginalLenght = 0;
        public void Initialize()
        {
            OriginalLenght = SystemInfo.deviceUniqueIdentifier.Length;
            if (OriginalLenght == 0)
            {
                HDLogger.LogError("Failed to get HWID lenght");
                return;
            }

            if (APIUser.CurrentUser != null)
            {
                HDLogger.LogWarning($"Already logged in, skipping HWID Spoof");
                return;
            }
            var mainmethod = IL2CPP.il2cpp_resolve_icall("UnityEngine.SystemInfo::GetDeviceUniqueIdentifier");
            Patch();
        }
        private static IntPtr Patch()
        {
            if (!APIUser.IsLoggedIn)
            {
                if (IsTokenExisting())
                {
                    if (Core.ConfigManager.Ini.GetString("Spoof", "HWID").Length != OriginalLenght || Core.ConfigManager.Ini.GetString("Spoof", "Token") != ApiCredentials.GetString("authToken"))
                    {
                        GenerateAndSetHWID();
                        SetToken();
                    }

                    else if (FakeHWID == null) SetHWIDFromConfig();
                }

                else if (!SetFreshHWID)
                {
                    SetFreshHWID = true;
                    GenerateAndSetHWID();
                }
            }

            else if (SetFreshHWID)
            {
                SetFreshHWID = false;
                SetToken();
            }

            if (FakeHWID == null || OriginalLenght != FakeHWID.ToString().Length || OriginalLenght == 0)
            {
                HDLogger.LogError("Error generating HWID, restart your game");
                Thread.Sleep(-1);
            }

            return FakeHWID.Pointer;
        }
        private static string GenerateHWID()
        {
            byte[] bytes = new byte[OriginalLenght / 2];
            EncryptUtils.Random.NextBytes(bytes);
            return string.Join("", bytes.Select(it => it.ToString("x2")));
        }
        private static bool IsTokenExisting()
        {
            return ApiCredentials.GetString("authToken").StartsWith("authcookie_");
        }

        private static void GenerateAndSetHWID()
        {
            Core.ConfigManager.Ini.SetString("Spoof", "HWID", GenerateHWID());
            FakeHWID = new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(Core.ConfigManager.Ini.GetString("Spoof", "HWID")));
            HDLogger.Log($"Generated and Spoofed HWID: {FakeHWID.ToString()}", HDLogger.LogsType.Protection);
        }
        private static void SetToken()
        {
            Core.ConfigManager.Ini.SetString("Spoof", "Token", ApiCredentials.GetString("authToken"));
            VRC.Tools.ClearCookies();
            VRC.Tools.ClearExpiredBestHTTPCache();
            ApiCache.Clear();
            ApiCache.Cleanup();
        }

        private static void SetHWIDFromConfig()
        {
            FakeHWID = new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(Core.ConfigManager.Ini.GetString("Spoof", "HWID")));
            HDLogger.Log($"Spoofed HWID from Config: {FakeHWID.ToString()}", HDLogger.LogsType.Protection);
        }
    }
}
