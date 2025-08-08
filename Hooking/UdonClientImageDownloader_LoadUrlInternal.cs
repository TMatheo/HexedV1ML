using LUXED.Core;
using LUXED.Interfaces;
using LUXED.Wrappers;
using VRC.Networking;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

namespace LUXED.Hooking
{
    internal class UdonClientImageDownloader_LoadUrlInternal : IHook
    {
        public void Initialize()
        {
            EasyHooking.EasyPatchMethodPost(typeof(UdonClientImageDownloader), "Method_Public_Static_IVRCImageDownload_VRCUrl_Material_IUdonEventReceiver_TextureInfo_PDM_0", typeof(UdonClientImageDownloader_LoadUrlInternal), "Hook");
        }
        private static void Hook(VRCUrl __0, IUdonEventReceiver __1)
        {
            if (__0.url != null)
            {
                if (!InternalSettings.NoUdonDownload)
                {
                    HDLogger.Log($"Udon tried to download a image from {__0.url}", HDLogger.LogsType.Protection);
                    return;
                }
                HDLogger.Log($"Udon downloaded a image from {__0.url}", HDLogger.LogsType.Room);
            }
        }
    }
}
