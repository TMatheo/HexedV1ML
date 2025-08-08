using LUXED.Interfaces;
using VRC.Core;

namespace LUXED.Hooking
{
    internal class RoomManager_SetupWorld : IHook
    {
        public void Initialize()
        {
            EasyHooking.EasyPatchMethodPost(typeof(RoomManager), "Method_Public_Static_Boolean_ApiWorld_ApiWorldInstance_String_Int32_0", typeof(RoomManager_SetupWorld), "EnterWorldHook");
        }
        private static void EnterWorldHook(ApiWorld __0, ApiWorldInstance __1)
        {
            if (__0 != null && __0.tags.Contains("feature_avatar_scaling_disabled")) __0.tags.Remove("feature_avatar_scaling_disabled");
        }
    }
}
