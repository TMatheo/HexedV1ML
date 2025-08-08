using ExitGames.Client.Photon;
using LUXED.Wrappers;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using static VRC.SDKBase.VRC_EventHandler;

namespace LUXED.Extensions
{
    internal class PhotonHelper
    {
        public static void OpRaiseEvent(byte code, object customObject, RaiseEventOptions RaiseEventOptions, SendOptions sendOptions)
        {
            Il2CppSystem.Object customObject2 = CPP2IL.BinaryConverter.ManagedToIL<Il2CppSystem.Object>(customObject);
            OpRaiseEvent(code, customObject2, RaiseEventOptions, sendOptions);
        }
        public static void OpRaiseEventOld(byte code, object customObject, RaiseEventOptions RaiseEventOptions, byte Channel, DeliveryMode deliveryMode)
        {
            SendOptions options = new SendOptions()
            {
                Channel = Channel,
                DeliveryMode = deliveryMode,
                Encrypt = true,
                Reliability = deliveryMode == DeliveryMode.Reliable || deliveryMode == DeliveryMode.ReliableUnsequenced,
            };

            OpRaiseEventOld2(code, CPP2IL.BinaryConverter.ManagedToIL2CPP(customObject), RaiseEventOptions, options);
        }
        public static void OpRaiseEventOld2(byte code, Il2CppSystem.Object customObject, RaiseEventOptions RaiseEventOptions, SendOptions sendOptions)
        {
            GameHelper.VRCNetworkingClient.Method_Public_Virtual_New_Boolean_Byte_Object_RaiseEventOptions_SendOptions_0(code, customObject, RaiseEventOptions, sendOptions);
        }
        public static void SpawnPortal(UnityEngine.Vector3 positon, string worldSecureCode)
        {
            ObjectPublicAbstractSealedSiInSiUIBoSiGaTrDi2Unique.Method_Public_Static_Boolean_String_Boolean_Vector3_Quaternion_String_Action_1_LocalizableString_0(worldSecureCode, true, positon, new UnityEngine.Quaternion(0, 0, 0, 0));
        }
        public static void OpRaiseEvent(byte code, Il2CppSystem.Object customObject, RaiseEventOptions RaiseEventOptions, SendOptions sendOptions)
        {
            PhotonNetwork.Method_Public_Static_Boolean_Byte_Object_RaiseEventOptions_SendOptions_0(code, customObject, RaiseEventOptions, sendOptions);
        }

        public static void SendRPC(VrcEventType EventType, string Name, bool ParameterBool, VrcBooleanOp BoolOP, GameObject ParamObject, GameObject[] ParamObjects, string ParamString, float Float, int Int, byte[] bytes, VrcBroadcastType BroadcastType, float Fastforward = 0)
        {
            var a = new VrcEvent();
            a.EventType = EventType;
            a.Name = Name;
            a.ParameterBool = ParameterBool;
            a.ParameterBoolOp = BoolOP;
            a.ParameterBytes = bytes;
            a.ParameterObject = ParamObject;
            a.ParameterObjects = ParamObjects;
            a.ParameterString = ParamString;
            a.ParameterFloat = Float;
            a.ParameterInt = Int;
            Networking.SceneEventHandler.TriggerEvent(a, BroadcastType, ParamObject, Fastforward);
        }
        public static byte[] Vector3ToBytes(Vector3 vector3)
        {
            byte[] array = new byte[12];
            Buffer.BlockCopy(BitConverter.GetBytes(vector3.x), 0, array, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(vector3.y), 0, array, 4, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(vector3.z), 0, array, 8, 4);
            return array;
        }
        public static void SendUdonRPC(UdonBehaviour behaviour, string EventName, VRC.Player Target = null, bool Local = false)
        {
            // TODO: Replace with manual send like this? to send _ events
            // SendRPC(VrcEventType.SendRPC, "", false, VrcBooleanOp.Unused, behaviour.gameObject, null, "UdonSyncRunProgramAsRPC", 0, 2, Encoding.UTF8.GetBytes(EventName), VrcBroadcastType.AlwaysUnbuffered, 0);

            if (behaviour != null)
            {
                if (Target != null)
                {
                    if (!Networking.IsOwner(Target.GetVRCPlayerApi(), behaviour.gameObject)) Networking.SetOwner(Target.GetVRCPlayerApi(), behaviour.gameObject);
                    behaviour.SendCustomNetworkEvent(NetworkEventTarget.Owner, EventName);
                }
                else if (!Local) behaviour.SendCustomNetworkEvent(NetworkEventTarget.All, EventName);
                else behaviour.SendCustomEvent(EventName);
            }
            else
            {
                foreach (UdonBehaviour Behaviour in UnityEngine.Object.FindObjectsOfType<UdonBehaviour>())
                {
                    if (Behaviour._eventTable.ContainsKey(EventName)) Behaviour.SendCustomNetworkEvent(NetworkEventTarget.All, EventName);
                }
            }
        }

        public static void RaiseChatMessage(string Message)
        {
            if (Message == null || Message.Length > 144) return;
            OpRaiseEvent(43, Message, new RaiseEventOptions
            {
                field_Public_EventCaching_0 = 0,
                field_Public_ReceiverGroup_0 = 0
            }, default(SendOptions));
        }

        public static void RaiseTypingIndicator(byte Type)
        {
            OpRaiseEvent(44, Type, new RaiseEventOptions
            {
                field_Public_EventCaching_0 = 0,
                field_Public_ReceiverGroup_0 = 0
            }, default(SendOptions));
        }

        public static void RaiseVRMode(bool VR)
        {
            OpRaiseEventOld(42, new Hashtable { { "inVRMode", VR }, }, new RaiseEventOptions(), 0, DeliveryMode.Reliable);
        }

        public static void RaiseAvatarHeight(int Height)
        {
            OpRaiseEventOld(42, new Hashtable { { "avatarEyeHeight", Height }, }, new RaiseEventOptions(), 0, DeliveryMode.Reliable);
        }

        public static void RaiseGroupOnNameplate(string GroupID)
        {
            if (GroupID == null) return;

            OpRaiseEventOld(42, new Hashtable { { "groupOnNameplate", GroupID }, }, new RaiseEventOptions(), 0, DeliveryMode.Reliable);
        }

        public static void RaiseBlock(string userid, bool Block)
        {
            if (!userid.StartsWith("usr_")) return;
            OpRaiseEventOld(33, new Dictionary<byte, object>{{ 3, Block },{ 0, (byte)22 },{ 1, userid },}, new RaiseEventOptions(), 0, DeliveryMode.Reliable);
        }

        public static void RaisePortalCreate(string InstanceID, Vector3 Position, float Rotation)
        {
            if (InstanceID == null) return;
            PhotonHelper.SpawnPortal(Position, InstanceID);
        }

        public static void RaiseSyncOwnership(int viewId, int Owner)
        {
            //OpRaiseEvent(21, new int[2] { viewId, Owner }, new RaiseEventOptions() { field_Public_ReceiverGroup_0 = ReceiverGroup.All }, 0, DeliveryMode.Reliable);
        }

        public static void RaiseItemOwnership(int viewId, int Owner)
        {
            //OpRaiseEvent(22, new int[2] { viewId, Owner }, new RaiseEventOptions() { field_Public_ReceiverGroup_0 = ReceiverGroup.All }, 0, DeliveryMode.Reliable);
        }

        public static void RaiseLegacyOwnership(int viewId, int Owner)
        {
            //OpRaiseEvent(210, new int[2] { viewId, Owner }, new RaiseEventOptions() { field_Public_ReceiverGroup_0 = ReceiverGroup.All }, 0, DeliveryMode.Reliable);
        }
    }
}
