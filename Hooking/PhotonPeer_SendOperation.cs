using ExitGames.Client.Photon;
using HarmonyLib;
using LUXED.Interfaces;
using LUXED.Modules.EventManagement;
using System.Linq;
using System.Reflection;

namespace LUXED.Hooking
{
    internal class PhotonPeer_SendOperation : IHook
    {
        public void Initialize()
        {
            EasyHooking.LuxedInstance.Patch(typeof(PhotonPeer).GetMethods().LastOrDefault((MethodInfo x) => x.Name.Equals("SendOperation")), new HarmonyMethod(AccessTools.Method(typeof(PhotonPeer_SendOperation), "Hook", null, null)), null, null, null, null);
            //EasyHooking.Patcher(typeof(PhotonPeer).GetMethod("SendOperation", BindingFlags.Public | BindingFlags.Instance), typeof(PhotonPeer_SendOperation).GetMethod(nameof(Hook), BindingFlags.NonPublic | BindingFlags.Static));
        }
        private static bool Hook(ExitGames.Client.Photon.PhotonPeer __instance, bool __result, byte __0, Il2CppSystem.Collections.Generic.Dictionary<byte, Il2CppSystem.Object> __1, ExitGames.Client.Photon.SendOptions __2)
        {
            switch (__0)
            {
                case 253: // OpRaise
                    break;

                case 230: // Auth?
                    break;

                case 231: // Auth Websocket?

                case 226: // OpJoinRoom
                    OperationHandler.ChangeOperation226(__1);
                    break;

                case 252: // SetProperties
                    break;
            }
            return true;
        }
    }
}
