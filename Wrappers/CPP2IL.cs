using System;
using System.IO;

namespace LUXED.Wrappers
{
    internal class CPP2IL
    {
        public class BinaryConverter
        {
            public static object IL2CPPToManaged(Il2CppSystem.Object il2cppObj)
            {
                if (il2cppObj == null) return null;

                try
                {
                    var formatter = new Il2CppSystem.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    var ms = new Il2CppSystem.IO.MemoryStream();
                    formatter.Serialize(ms, il2cppObj);
                    var data = ms.ToArray();

                    return DeserializeManaged(data);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            public static T ManagedToIL<T>(object obj)
            {
                return CPP2IL.BinaryConverter.IL2CPPFromByteArray<T>(CPP2IL.BinaryConverter.ToByteArray(obj));
            }
            public static T IL2CPPFromByteArray<T>(byte[] data)
            {
                T result;
                if (data == null)
                {
                    result = default(T);
                }
                else
                {
                    Il2CppSystem.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new Il2CppSystem.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    Il2CppSystem.IO.MemoryStream serializationStream = new Il2CppSystem.IO.MemoryStream(data);
                    result = (T)((object)binaryFormatter.Deserialize(serializationStream));
                }
                return result;
            }
            public static byte[] ToByteArray(object obj)
            {
                byte[] result;
                if (obj == null)
                {
                    result = null;
                }
                else
                {
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                    binaryFormatter.Serialize(memoryStream, obj);
                    result = memoryStream.ToArray();
                }
                return result;
            }
            public static Il2CppSystem.Object ManagedToIL2CPP(object managedObj)
            {
                if (managedObj == null) return null;

                try
                {
                    var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    var ms = new MemoryStream();
                    formatter.Serialize(ms, managedObj);
                    var data = ms.ToArray();

                    return DeserializeIL2CPP(data);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            public static object ILToManagedObject(Il2CppSystem.Object il2cppObj)
            {
                if (il2cppObj == null) return null;

                try
                {
                    var ilFormatter = new Il2CppSystem.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    var ilStream = new Il2CppSystem.IO.MemoryStream();
                    ilFormatter.Serialize(ilStream, il2cppObj);

                    var bytes = ilStream.ToArray();

                    var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    var ms = new MemoryStream(bytes);
                    return formatter.Deserialize(ms);
                }
                catch
                {
                    return null;
                }
            }
            private static object ManagedObjectFromArray(byte[] data)
            {
                if (data == null) return default;
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                MemoryStream ms = new MemoryStream(data);
                object obj = bf.Deserialize(ms);
                return obj;
            }
            public static object Serialize(Il2CppSystem.Object obj)
            {
                return ManagedObjectFromArray(IL2CPPObjectToByteArray(obj));
            }
            private static byte[] IL2CPPObjectToByteArray(Il2CppSystem.Object obj)
            {
                if (obj == null) return null;
                var bf = new Il2CppSystem.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                var ms = new Il2CppSystem.IO.MemoryStream();
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }

            private static object DeserializeManaged(byte[] data)
            {
                try
                {
                    var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    var ms = new MemoryStream(data);
                    return formatter.Deserialize(ms);
                }
                catch
                {
                    return null;
                }
            }

            private static Il2CppSystem.Object DeserializeIL2CPP(byte[] data)
            {
                try
                {
                    var formatter = new Il2CppSystem.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    var ms = new Il2CppSystem.IO.MemoryStream(data);
                    return (Il2CppSystem.Object)formatter.Deserialize(ms);
                }
                catch
                {
                    return null;
                }
            }

            public static T ConvertManagedToIL2CPP<T>(object managedObj)
            {
                var bytes = SerializeToBytes(managedObj);
                return DeserializeIL2CPPGeneric<T>(bytes);
            }

            private static T DeserializeIL2CPPGeneric<T>(byte[] data)
            {
                if (data == null) return default;

                var formatter = new Il2CppSystem.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                var ms = new Il2CppSystem.IO.MemoryStream(data);
                return (T)(object)formatter.Deserialize(ms);
            }

            private static byte[] SerializeToBytes(object obj)
            {
                if (obj == null) return null;
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                var ms = new MemoryStream();
                formatter.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}
