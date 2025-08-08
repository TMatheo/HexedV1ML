using HarmonyLib;
using LUXED.Interfaces;
using LUXED.Wrappers;
using System;
using System.Reflection;
using UnityEngine;

namespace LUXED.Hooking
{
    internal class VRCUiPageLoading_OnEnable : IHook
    {
        public void Initialize()
        {
            EasyHooking.LuxedInstance.Patch(typeof(VRCUiPageLoading).GetMethod("OnEnable"), GetLocalPatch("Patch"));
        }
        private static void Patch()
        {
            if (VRCUiPageLoading.field_Internal_Static_VRCUiPageLoading_0 == null) return;
            GameObject.Find("MenuContent/Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/SkyCube_Baked").transform.localScale = Vector3.zero;
            GameObject.Find("MenuContent/Popups/LoadingPopup/3DElements/LoadingInfoPanel").transform.localScale = Vector3.zero;
            GameObject.Find("MenuContent/Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Panel_Backdrop").transform.localScale = Vector3.zero;
            GameObject.Find("MenuContent/Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Decoration_Left").transform.localScale = Vector3.zero;
            GameObject.Find("MenuContent/Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Decoration_Right").transform.localScale = Vector3.zero;
            ParticleSystem FarParticles = GameObject.Find("MenuContent/Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/_FX_ParticleBubbles/FX_snow").GetComponent<ParticleSystem>();
            var farMain = FarParticles.main;
            farMain.startColor = Color.white;
            farMain.startSpeed = 0.5f;
            farMain.gravityModifier = 0.2f;
            farMain.startLifetime = 5f;

            var farVelocityOverLifetime = FarParticles.velocityOverLifetime;
            farVelocityOverLifetime.enabled = true;    
        }
        public static HarmonyMethod GetLocalPatch(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            try
            {
                MethodInfo method = typeof(VRCUiPageLoading_OnEnable).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic);
                if (method == null)
                {
                    HDLogger.Log($"Method '{name}' not found in VRCUiPageLoading_OnEnable.",HDLogger.LogsType.Info);
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
