using LUXED.Interfaces;
using System.Reflection;
using VRC.Economy.Internal;

namespace LUXED.Hooking
{
    internal class StoreManager_DoesPlayerOwnProduct : IHook
    {
        public void Initialize()
        {
            EasyHooking.Patcher(typeof(Store).GetMethod("Method_Private_Boolean_VRCPlayerApi_IProduct_PDM_0", BindingFlags.Public | BindingFlags.Instance), typeof(StoreManager_DoesPlayerOwnProduct).GetMethod(nameof(Hook), BindingFlags.NonPublic | BindingFlags.Static));
        }
        private static bool Hook(ref bool __result)
        {
            __result = true;
            return false;
        }
    }
}
