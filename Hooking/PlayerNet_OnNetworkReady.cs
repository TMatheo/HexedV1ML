using LUXED.Core;
using LUXED.Interfaces;
using VRC;

namespace LUXED.Hooking
{
    internal class PlayerNet_OnNetworkReady : IHook
    {
        public void Initialize()
        {
            EasyHooking.EasyPatchMethodPost(typeof(NetworkManager), "Method_Public_Void_Player_PDM_1", typeof(PlayerNet_OnNetworkReady), "OnJoinEvent");
        }
        internal static void OnJoinEvent(Player __0)
        {
            GameManager.CreatePlayerModules(__0._vrcplayer); 
        }
    }
}
