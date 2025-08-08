using Harmony;
using LUXED.Wrappers;
using System;
using System.Reflection;

namespace LUXED.Hooking
{
    internal class EasyHooking
    {
        public static HarmonyLib.Harmony LuxedInstance = new HarmonyLib.Harmony("DeePatch");
        public static void EasyPatchPropertyPost(Type inputclass, string InputMethodName, Type outputclass, string outputmethodname)
        {
            LuxedInstance.Patch(AccessTools.Property(inputclass, InputMethodName).GetMethod, null, new HarmonyMethod(outputclass, outputmethodname, null), null, null, null);
        }
        public static void EasyPatchPropertyPre(Type inputclass, string InputMethodName, Type outputclass, string outputmethodname)
        {
            LuxedInstance.Patch(AccessTools.Property(inputclass, InputMethodName).GetMethod, new HarmonyMethod(outputclass, outputmethodname, null), null, null, null, null);
        }
        public static void EasyPatchMethodPre(Type inputclass, string InputMethodName, Type outputclass, string outputmethodname)
        {
            LuxedInstance.Patch(inputclass.GetMethod(InputMethodName), new HarmonyMethod(AccessTools.Method(outputclass, outputmethodname, null, null)), null, null, null, null);
        }
        public static void EasyPatchMethodPost(Type inputclass, string InputMethodName, Type outputclass, string outputmethodname)
        {
            LuxedInstance.Patch(inputclass.GetMethod(InputMethodName), null, new HarmonyMethod(AccessTools.Method(outputclass, outputmethodname, null, null)), null, null, null);
        }
        public static void Patcher(MethodInfo methodToPatch, MethodInfo prefixMethod)
        {
            try
            {
                if (methodToPatch == null)
                    throw new ArgumentNullException(nameof(methodToPatch));
                if (prefixMethod == null)
                    throw new ArgumentNullException(nameof(prefixMethod));

                LuxedInstance.Patch(methodToPatch, prefix: new HarmonyMethod(prefixMethod));
            }
            catch (Exception ex)
            {
                HDLogger.LogError($"Failed to hook method '{methodToPatch?.Name}'");
            }
        }

        [Obsolete]
        internal static HarmonyMethod GetLocalPatch<T>(string name)
        {
            return new HarmonyMethod(typeof(T).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic));
        }
    }
}
