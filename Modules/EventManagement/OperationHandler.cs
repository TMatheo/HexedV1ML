using LUXED.Core;
using System;
using System.Reflection;
using UnhollowerBaseLib;

namespace LUXED.Modules.EventManagement
{
    internal class OperationHandler
    {
        public static void ChangeOperation226(Il2CppSystem.Collections.Generic.Dictionary<byte, Il2CppSystem.Object> param)
        {
            if (!param.ContainsKey(249)) return;

            Il2CppSystem.Collections.Hashtable hashtable = param[249].TryCast<Il2CppSystem.Collections.Hashtable>();

            if (hashtable == null) return;

            if (hashtable.ContainsKey("inVRMode"))
            {
                switch (InternalSettings.VRSpoof)
                {
                    case InternalSettings.VRMode.VR:
                        hashtable["inVRMode"] = CreateIl2CppBoolean(true).BoxIl2CppObject();
                        break;

                    case InternalSettings.VRMode.Desktop:
                        hashtable["inVRMode"] = CreateIl2CppBoolean(false).BoxIl2CppObject();
                        break;
                }
            }

            if (hashtable.ContainsKey("groupOnNameplate"))
            {
                if (InternalSettings.GroupSpoof) 
                    hashtable["groupOnNameplate"] = new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(InternalSettings.FakeGroupValue));
            }
        }
        public static Il2CppSystem.Boolean CreateIl2CppBoolean(bool value)
        {
            var boolType = typeof(Il2CppSystem.Boolean);
            var ctor = boolType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, Type.EmptyTypes, null);
            if (ctor == null)
                throw new Exception("Constructor not found on Il2CppSystem.Boolean");
            var il2CppBool = (Il2CppSystem.Boolean)ctor.Invoke(null);
            var mValueField = boolType.GetField("m_value", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (mValueField == null)
                throw new Exception("Field 'm_value' not found on Il2CppSystem.Boolean");
            mValueField.SetValue(il2CppBool, value);
            return il2CppBool;
        }
    }
}
