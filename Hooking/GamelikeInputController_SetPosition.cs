using LUXED.Interfaces;
using LUXED.Modules;
using System.Reflection;

namespace LUXED.Hooking
{
    internal class GamelikeInputController_SetPosition : IHook, IDesktopOnly
    {
        public void Initialize()
        {
            EasyHooking.Patcher(typeof(GamelikeInputController).GetMethod("Method_Private_Void_0", BindingFlags.Public | BindingFlags.Instance), typeof(GamelikeInputController_SetPosition).GetMethod(nameof(Hook), BindingFlags.NonPublic | BindingFlags.Static));
        }
        private static void Hook()
        {
            if (!Movement.isRotateEnabled) return;
        }
    }
}
