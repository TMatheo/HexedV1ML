using MelonLoader;
using System.Collections;
using LUXED.Core;
using LUXED.CustomUI;
using LUXED.CustomUI.QM;
using LUXED.Extensions;
using LUXED.Modules.Standalone;
using LUXED.UIApi;
using LUXED.Wrappers;

namespace LUXED
{
    public class Main : MelonMod
    {
        public static bool IsDebug = false;
        public override void OnInitializeMelon() 
        {
            LoadMenu("UserData").Start();
        } 
        private static IEnumerator LoadMenu(string UnityLoaderPath)
        {
            while (VRCUiManager.prop_VRCUiManager_0 == null) yield return null;

            ConfigManager.Init(UnityLoaderPath);
            ExternalConsole.Init();

            HDLogger.Log($"VRChat running in {(GameUtils.IsInVr() ? "VR" : "DESKTOP")} mode", HDLogger.LogsType.Info);

            GameManager.CreateHooks();
            GameManager.CreateGlobalModules();
            LoadExtensions();

            while (MenuHelper.QuickMenu == null || MenuHelper.MainMenu == null || !MenuHelper.QuickMenu.isActiveAndEnabled) yield return null;

            MainMenu.Init();
            UIQuickChanges.ApplyLateChanges();
        }
        private static void LoadExtensions()
        {
            HighlightHelper.Init();
            UIQuickChanges.ChangeTrustColors();
            AssetChanger.Initialize();
            UIQuickChanges.ApplyEarlyChanges();

            if (!GameUtils.IsInVr())
            {
                //LeapMain.Init();
            }
        }
        public override void OnUpdate()
        {
            GameManager.UpdateGlobalModules();
        }

        public override void OnApplicationQuit()
        {
            BotConnection.StopBot();
            HDLogger.Log("Goodbye :3", HDLogger.LogsType.Info);
        }
    }
}