using HarmonyLib;
using LUXED.Core;
using LUXED.Interfaces;
using LUXED.Modules.EventManagement;
using LUXED.Modules.Standalone;
using LUXED.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LUXED.Hooking
{
    internal class VRCNetworkingClient_OpRaiseEvent
    {
        public void Initialize()
        {
            var targetMethod = typeof(VRCNetworkingClient).GetMethods()
        .LastOrDefault(m => m.Name == "Method_Public_Virtual_New_Boolean_Byte_Object_RaiseEventOptions_SendOptions_0");

            if (targetMethod == null)
                throw new Exception("Target method not found.");

            var prefixMethod = typeof(VRCNetworkingClient_OpRaiseEvent).GetMethod("Hook", BindingFlags.NonPublic | BindingFlags.Static);

            EasyHooking.LuxedInstance.Patch(targetMethod, new HarmonyMethod(prefixMethod));
        }
        private static bool Hook(Photon.Realtime.LoadBalancingClient __instance,bool __result,byte eventCode,Il2CppSystem.Object eventData,RaiseEventOptions raiseEventOptions,ExitGames.Client.Photon.SendOptions sendOptions)
        {
            switch (eventCode)
            {
                case 12: // Unreliable Player Serialization
                    {
                        byte[] originalEvent = (byte[])CPP2IL.BinaryConverter.IL2CPPToManaged(eventData);

                        // Calculate offsets safely
                        int playerSyncOffset = originalEvent.Length > 19 ? originalEvent[19] + 19 : -1;
                        int playerNetOffset = originalEvent.Length > 18 ? originalEvent[18] + 18 : -1;
                        int poseRecorderOffset = originalEvent.Length > 17 ? originalEvent[17] + 17 : -1;

                        // Infinity Position Spoof
                        if (playerSyncOffset >= 0 && originalEvent.Length > playerSyncOffset)
                        {
                            if (InternalSettings.InfinityPosition)
                            {
                                int offsetInStruct = 1;
                                int lengthInStruct = 12; // 3 floats * 4 bytes each
                                int offsetToOverride = playerSyncOffset + offsetInStruct;

                                if (originalEvent.Length >= offsetToOverride + lengthInStruct)
                                {
                                    float infValue = UnityUtils.InfPosition.InfValue;
                                    Buffer.BlockCopy(BitConverter.GetBytes(infValue), 0, originalEvent, offsetToOverride, 4);
                                    Buffer.BlockCopy(BitConverter.GetBytes(infValue), 0, originalEvent, offsetToOverride + 4, 4);
                                    Buffer.BlockCopy(BitConverter.GetBytes(infValue), 0, originalEvent, offsetToOverride + 8, 4);
                                }
                            }
                        }

                        // Ping and Latency spoofing
                        if (playerNetOffset >= 0 && originalEvent.Length > playerNetOffset)
                        {
                            // Ping spoofing
                            if (InternalSettings.PingMode == InternalSettings.FrameAndPingMode.Custom ||
                                InternalSettings.PingMode == InternalSettings.FrameAndPingMode.Realistic)
                            {
                                int offsetInStruct = 0;
                                int lengthInStruct = 2;
                                int offsetToOverride = playerNetOffset + offsetInStruct;

                                if (originalEvent.Length >= offsetToOverride + lengthInStruct)
                                {
                                    short value = InternalSettings.PingMode == InternalSettings.FrameAndPingMode.Custom
                                        ? InternalSettings.FakePingValue
                                        : (short)EncryptUtils.Random.Next(4, 16);

                                    Buffer.BlockCopy(BitConverter.GetBytes(value), 0, originalEvent, offsetToOverride, lengthInStruct);
                                }
                            }

                            // Latency spoofing
                            if (InternalSettings.LatencySpoof == InternalSettings.LatencyMode.Custom ||
                                InternalSettings.LatencySpoof == InternalSettings.LatencyMode.Low ||
                                InternalSettings.LatencySpoof == InternalSettings.LatencyMode.High)
                            {
                                int offsetInStruct = 2;
                                int lengthInStruct = 1;
                                int offsetToOverride = playerNetOffset + offsetInStruct;

                                if (originalEvent.Length >= offsetToOverride + lengthInStruct)
                                {
                                    byte value;

                                    switch (InternalSettings.LatencySpoof)
                                    {
                                        case InternalSettings.LatencyMode.Custom:
                                            value = (byte)InternalSettings.FakeLatencyValue;
                                            break;
                                        case InternalSettings.LatencyMode.Low:
                                            value = byte.MinValue; // 0
                                            break;
                                        case InternalSettings.LatencyMode.High:
                                            value = byte.MaxValue; // 255
                                            break;
                                        default:
                                            value = 0;
                                            break;
                                    }
                                    originalEvent[offsetToOverride] = value;
                                }
                            }

                            // Frame mode spoofing
                            if (InternalSettings.FrameMode == InternalSettings.FrameAndPingMode.Custom ||
                                InternalSettings.FrameMode == InternalSettings.FrameAndPingMode.Realistic)
                            {
                                int offsetInStruct = 3;
                                int lengthInStruct = 1;
                                int offsetToOverride = playerNetOffset + offsetInStruct;

                                if (originalEvent.Length >= offsetToOverride + lengthInStruct)
                                {
                                    byte value = InternalSettings.FrameMode == InternalSettings.FrameAndPingMode.Custom
                                        ? (byte)(InternalSettings.FakeFrameValue == 0
                                            ? 0
                                            :UnityEngine.Mathf.Clamp(1000 / InternalSettings.FakeFrameValue, 0, 255))
                                        : (byte)EncryptUtils.Random.Next(5, 9);

                                    originalEvent[offsetToOverride] = value;
                                }
                            }
                        }

                        // Mic and Earmuff Spoof
                        if (poseRecorderOffset >= 0 && originalEvent.Length > poseRecorderOffset)
                        {
                            int offsetInStruct = 7;
                            int offsetToOverride = poseRecorderOffset + offsetInStruct;
                            if (originalEvent.Length > offsetToOverride)
                            {
                                // Mic Spoof
                                switch (InternalSettings.MicSpoof)
                                {
                                    case InternalSettings.MicStateMode.Muted:
                                        originalEvent[offsetToOverride] |= 1 << 6;
                                        break;
                                    case InternalSettings.MicStateMode.Unmuted:
                                        originalEvent[offsetToOverride] &= (byte)(~(1 << 6) & 0xFF);
                                        break;
                                }

                                // Earmuff Spoof
                                switch (InternalSettings.EarmuffSpoof)
                                {
                                    case InternalSettings.EarmuffStateMode.Enabled:
                                        originalEvent[offsetToOverride] |= 1 << 5;
                                        break;
                                    case InternalSettings.EarmuffStateMode.Disabled:
                                        originalEvent[offsetToOverride] &= (byte)(~(1 << 5) & 0xFF);
                                        break;
                                }
                            }
                        }

                        // Fake Serialize and Obfuscate Movement
                        int maxOffset = Math.Max(playerSyncOffset, playerNetOffset);
                        if (maxOffset >= 0 && originalEvent.Length > maxOffset)
                        {
                            if (FakeSerialize.NoSerialize)
                            {
                                if (FakeSerialize.CachedMovement == null)
                                {
                                    FakeSerialize.CachedMovement = originalEvent;
                                }
                                else
                                {
                                    if (FakeSerialize.BotSerialize)
                                        BotConnection.SelfbotMovement(Convert.ToBase64String(originalEvent));

                                    int viewID = BitConverter.ToInt32(originalEvent, 0);
                                    int serverTime = BitConverter.ToInt32(originalEvent, 4);

                                    int ogOffset = playerNetOffset;
                                    if (originalEvent.Length >= ogOffset + 5)
                                    {
                                        byte ping1 = originalEvent[ogOffset++];
                                        byte ping2 = originalEvent[ogOffset++];
                                        byte latency = originalEvent[ogOffset++];
                                        byte frame = originalEvent[ogOffset++];
                                        byte quality = originalEvent[ogOffset++];

                                        originalEvent = FakeSerialize.CachedMovement;
                                        Buffer.BlockCopy(BitConverter.GetBytes(viewID), 0, originalEvent, 0, 4);
                                        Buffer.BlockCopy(BitConverter.GetBytes(serverTime), 0, originalEvent, 4, 4);

                                        int offset = originalEvent[18] + 18;
                                        if (originalEvent.Length >= offset + 5)
                                        {
                                            originalEvent[offset++] = ping1;
                                            originalEvent[offset++] = ping2;
                                            originalEvent[offset++] = latency;
                                            originalEvent[offset++] = frame;
                                            originalEvent[offset++] = quality;
                                        }
                                    }
                                }
                            }
                            else if (InternalSettings.ObfuscateMovement)
                            {
                                int adj = EncryptUtils.Random.Next(1, 9);
                                if (originalEvent.Length > 20)
                                {
                                    originalEvent[17] += (byte)adj;
                                    originalEvent[18] += (byte)adj;
                                    originalEvent[19] += (byte)adj;

                                    byte doubleByte = originalEvent[20];
                                    originalEvent[20] = (byte)EncryptUtils.Random.Next(0, 256);

                                    var byteList = originalEvent.ToList();
                                    var randomBytes = Enumerable.Range(0, adj - 1)
                                        .Select(_ => (byte)EncryptUtils.Random.Next(0, 256))
                                        .ToList();
                                    randomBytes.Add(doubleByte);
                                    byteList.InsertRange(21, randomBytes);

                                    originalEvent = byteList.ToArray();
                                }
                            }
                        }

                        // Movement Redirect
                        if (InternalSettings.MovementRedirect)
                        {
                            eventCode = 7;
                        }

                        break;
                    }
                case 13: // Reliable Serialization
                    {
                        if (FakeSerialize.BotSerialize)
                        {
                            byte[] data = (byte[])CPP2IL.BinaryConverter.IL2CPPToManaged(eventData);
                            BotConnection.SelfbotAvatarSync(Convert.ToBase64String(data));
                            return false; // block further processing
                        }
                        break;
                    }
                case 33: // Moderations
                    {
                        var dict = (Dictionary<byte, object>)CPP2IL.BinaryConverter.IL2CPPToManaged(eventData);
                        if (!ModerationHandler.RaisedModerationEvent(dict))
                            return false;
                        break;
                    }
                case 43: // Chatbox message
                    {
                        string chatMessage = CPP2IL.BinaryConverter.IL2CPPToManaged(eventData).ToString();
                        if (!ChatHandler.RaisedChatEvent(chatMessage))
                            return false;
                        break;
                    }
                case 44: // Typing Indicator
                    {
                        if (FakeSerialize.BotSerialize)
                            return false;
                        break;
                    }
            }

            return true;
        }
    }
}
