using LUXED.Core;
using LUXED.Interfaces;
using LUXED.Wrappers;
using VRC.Networking;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

namespace LUXED.Hooking
{
    internal class UdonClientStringDownloader_LoadUrlInternal : IHook
    {
        public void Initialize()
        {
            EasyHooking.EasyPatchMethodPost(typeof(UdonClientStringDownloader), "Method_Private_Static_Void_VRCUrl_IUdonEventReceiver_PDM_0", typeof(UdonClientStringDownloader_LoadUrlInternal), "Hook");
        }
        private static void Hook(VRCUrl __0, IUdonEventReceiver __1)
        {
            if (__0 != null)
            {
                if (InternalSettings.NoUdonDownload)
                {
                    HDLogger.Log($"Udon tried to download a string from {__0.url}", HDLogger.LogsType.Protection);
                    return;
                }
                HDLogger.Log($"Udon downloaded a string from {__0.url}", HDLogger.LogsType.Room);
            }
        }
    }
}
