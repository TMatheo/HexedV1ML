using LUXED.Core;
using LUXED.CustomUI;
using LUXED.Interfaces;
using LUXED.UIApi;
using LUXED.Wrappers;
using System;
using UnityEngine;
using VRC;

namespace LUXED.Modules
{
    internal class Misc : IGlobalModule
    {
        public void Initialize()
        {
            DestroyAnalytics();
        }

        public void OnUpdate()
        {
            if (!GameUtils.IsInWorld()) return;

            if (MenuHelper.QuickMenu != null && MenuHelper.QuickMenu.isActiveAndEnabled)
            {
                TimeSpan span = TimeSpan.FromMilliseconds(GeneralUtils.GetUnixTimeInMilliseconds() - InternalSettings.joinedRoomTime);
                string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}", span.Hours, span.Minutes, span.Seconds);
                if (UIQuickChanges.CustomDebugPanel != null) UIQuickChanges.CustomDebugPanel.text = $"Room: {elapsedTime}";
            }

            if (VRCUiCursorManager.field_Private_Static_VRCUiCursorManager_0.field_Private_EnumNPublicSealedvaNoMoGaHa5vUnique_0 == VRCUiCursorManager.EnumNPublicSealedvaNoMoGaHa5vUnique.None) Camera.main.nearClipPlane = 0.001f;
            else Camera.main.nearClipPlane = 0.01f;
        }

        private static void DestroyAnalytics()
        {
            GameObject AnalyticsManager = VRCApplication.prop_VRCApplication_0?.transform.Find("AnalyticsManager")?.gameObject;

            if (AnalyticsManager == null)
            {
                HDLogger.LogError("AnalyticsManager not found");
                return;
            }

            UnityEngine.Object.DestroyImmediate(AnalyticsManager);
            HDLogger.Log("Removed AnalyticsManager from Scene", HDLogger.LogsType.Protection);
        }

        public static void ReloadAvatars()
        {
            foreach (Player player in GameHelper.PlayerManager.GetAllPlayers())
            {
                player.GetAPIUser().ReloadAvatar();
            }
        }
    }
}
