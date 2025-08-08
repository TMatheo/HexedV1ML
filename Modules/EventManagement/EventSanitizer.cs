using LUXED.Core;
using LUXED.Modules.Standalone;
using LUXED.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using VRC;
using VRC.SDKBase;

namespace LUXED.Modules.EventManagement
{
    internal class EventSanitizer
    {
        private static readonly Dictionary<int, List<int>> EventBlocks = new Dictionary<int, List<int>>();
        private static readonly List<string> blacklistedBytes = new List<string>(); 
        public static readonly Dictionary<string, string> KnownBadVoicePayloads = new Dictionary<string, string>
        {
            { "A F LOUD SOUND", "AAAAAAAAAAC7hjsA+H3owFygUv4w5B67lcSx14zff9FCPADiNbSwYWgE+O7Dhiy5tkRecs21ljjofvebe6xsYlA4cVmght0=" },
            { "Mid Beep", "AAAAAJUHsUBfEz0A+B80r2YBf6G3uJzbiJoizx+4Bljxa0rVUK5/Lf1Rj15iAP6yBwhrX6DxVBcdXvKUrqPb5qssK5pb1UkiF2ATPQD4F/MAKZNSQQGpIa7abvlOZBLtX+klVW4vNCEtSrLvB5ongbLk/XqKZpwz8G7k9lEpRuJfTIEBbkadn3QWYRM9APgCwU8JpzYDfCq/lFv+WI4k25NwviENP3W7QtP8MCm5FsCP3Oa7oh/TVwtj5VDc3F6Iid4VRkdi/yamfkU=" },
            { "Mid Beep 2", "AAAAAPzTskBkKjoAeJgW+rwmaGVriTYsi2Mv54RUQFBCjnvlgm+KbwqY3YMjJW5k0ptAS1ISnNC78WS3JlRZkYFgUe1V0WUqPAB4l/U5EL1nYbXu6XVDCjLOmAzO75IyJ1Twc1WNUPSFk9xTr1+deYHAAaYN/4gAhcG6NkwjPDMhrsqQceU=" },
            { "Fart E1", "AAAAAI/mtUC3UTwA+EWpRGS+HS1hiWZJuQpytB1pkj+Ms7CC8MWmic+qIDB011LYW8m3pg63AsFy3smkiKbPvgCe6x/9EEzUuFE8APg0Jdslgds9dRYajK2Dx0hynQiOoaZbEOoLKVutEjJOx95U5H7vWtvhd0QznmvmiGPcx2cnHxbVMoUlcblRPAD4LJtYnc7LHkMh29srHi6Yp7+5mnoKhED4lCXTY1pYI/hkrLeBaipy9DGGviW0DN3ULdgFJz+sx3TZ0x0=" },
            { "Win XP Error", "AAAAACCrukC+jlUA+HxIZok3S0MrTwpWQv+geRj4meMUeagJIMbmwhvd/+/Lrv1SWuveZyHfxlbYLRp5uURk/vGgXSLUZ+xPvSJlgOjKTnM1uYi9H+SdY8GWU8aSjUE2rL+OQAD4NTicAgSrpmLPe2J4xPzZJ80dHa3P7jQN0CJm513IvNs0ijneuXgEkJber2qEKs65gJN1hLOb+28fgrOucicOwI49APhI/hjx00q8UukSvH40E58TJLkwkGcemziN2gbGR+ht7EAVRKnnKbAgDQ/g9tBcDkAlmdYLfr/Cuav6asnBjj8A+O8N12YGKyTtRPzMAHdN8o2TmnaSTwDtPH4Aj97976Qu3yOqqSTRoCSMt5AXaTof+xVLW/lF2358O3YkFvTY" },
            { "Fax Machine", "AAAAANZP+kCzPj0A+FJxg4xV2UVVupPxYyUePAve/W591uOkhbgyBv7lOCMeD+q/UXM4bJ4FnijiQcZZOwSjDqeFdNXGqXLSH7Q+PQD4Q3bgdnZn/RakYlUUaYczsogkWjtra2Z+bTwXLuOepZjsMIQ4g7BRsWkt1nN9L+2MWgyvtBFDDocVH9pTtT49APg+mDXSdk3OMZmDIPuIzLYSEMLwwEPBpjcEod1b2CjUSeUtaOtoCXC9Ak0Liar/nbDKih9SrNnSRnS3VN+2Pj0A+AIlVAjgVvQ0bAkesZYB/z/k9DG0urdXUTWXJxj8qvL7XjLRgQ0QUw2SatG9msidvcjpkQ/ICWfhOhLwZbc+PQD4U2iF5moZNzvJ2W9nRQvg5INs2O0FmuXIbk+7fy2QpoOy85h/WaS/FXpdv6YZd9jcqq8Nc656RQuqJ/lnuD49APgTDtAEVhZfTXXUarpUTxDSx9NjXj5W8PqO8ZFjsVVjkN8nenUbFMbtD5vvfl1teb9FtJtN5UtHIWiEjuo=" },
            { "Allah", "CAAAAB6sMG+OMjgA+Ew2XhVENFt9Pw/nluaR8++/5AHXfZS2pJSm3mR4C+YPcSTw0aNlNF1QMZsNUjRacb6jbX72hYyPMjYA+CcXQwcTMmRUX67YOXNyS2k1uZ439wj4jN3mAtzmwc8zu0nbKq630stQjxXv/elqZ14xs42FkDI5APgKajHDZEPzuflj8tiYupYxK3HFRDLkXgqJXtaVsJsMOaLb67VbuvvA6xCJmwLoaCdG4R216fHKOJEyPQD4rljzuAB6XQ87CNllMC1v+QJUaktNnfMZ8ZAHDFDQoxdzVSKd8ISv+YMG1af2hLt7WXytQZiT2KSZfbKtkjI9APhLiT2EkPkKwlI9AV5FPwahnDjaubg8dQ/gHdRCose78ifGcDhorluTpkVOD+ZdkrxtO9Omwgxg+LQrVsw=" },
            { "RIP", "CAAAAFH5O282wz0A+AkrsGvu5PD5lPp1oKNwIeJlXwbVObHvJwHvYfBXBiEf4pF0nmxTBWED+lcsUQiKmi2tCD8lTSHu2lBxbzfDPQD4Cj9TsFYx6bG17v1jubydiutowa+UjxqBEcKfQUgj07K+nFd158O7iWOUry9fp22r3m22xDrPeNWj1PD4OMM9APgsBXxGNvNQshWrj5I/CEiHEzyTq+tul6qhiaA+79MZDw76xoFbbD5y8ipRUCoQa8ZAmE3kQX/7bVo9m4Q5wz0A+CDiY6M7yb5PN/STD3OWfBBujH7Li3Pyob5j1Hq8Zbes6RnDxUF0fKwrgeLk/YNXj1f2xjsQq3ekaXMMxDrDPQD4TmcEclvEWKW0clxTM2QzIgeTJLHVZ/E7qWET+rK4xfE6Fewas5+RC56x9Jm6Bw/7XKYa9lNK0bM3oCec" },
            { "Idk", "AQAAAKh2XkRCPz0A+BYmROZrIqqFuGP981im5G5heqP1ZuzzHdJ0hxRCg7ftMX6nlEfsq6COb2Nlzl9Umrw5VCw/IgdrH1Yh40M/PQD4K0LwKdB2nYPK+c4KFs/7BwPlD/OUiCh5DgE0KaxW1lmgIvMfDZfq/sDEr2XLbMQuEnMVUjwF5DbP7W+c" },
            { "Dog Whistle", "HQYAACn1xx/FfT0A+F37hMDFLrkxGu2r8LNqvwGwggNrSoGy8kG6EKvZ6hWn07loNILezkDwyTs8NTfhzCVlTlVmSJ8eKFGHt8Z9PQD4YFK3leII55bRP/cZ4RG2C47710R9q4MBNO9oi1VzgRj9oTcjdvwrlFxfYz52mxtgtsXd9sev3WQEIQACx309APhd/tIiyhf1R9nzhvi2MAIgFAzRDOVS8YoAfHsMKM74GEqB0gV2QkQHC+eSOIM05/ajP7ZbbRJ1OQxqC7PIfT0A+F4i+XJ8AlQrDaAhAw0wLGTyEYm6iYXjyVDsC8OjOLJDNoHg+Y1l+T2iz0mreVeNRjlNlJ9rd0JyLNO0Osl9PQD4XfFlGrVi630WvNHch0iRu6hiNSeiLfaffkVxeDdEA+CytHZ+aChPPb49kUiEql38NO+IidAwvfeCEIYIyn09APhd+3Tsjewfm5J0gfe/zJ5PcJ9EXkw1sxRhMbs4McYLxJYZJ3f+BB8jNaqNfGKWGgsigiRDxrwidXHkgSg=" },
            { "Higher Pitched e1", "CAAAAFDtQW9rDzkA+K723bfG26TNTQ5onsbVBYIB+wG+9HzmMLrZ5PxYlhTcq3V/fce+VLye8+pn0lZiIRfOiROAkWDhbA8/APgYHBjHFx3L9ThvpK73IrbCnElYjFLKyuoGnwHL+x07oeNlLJZ/6ezUNjW7+HHv2RaTmFfmBrUiQm9zypCauW0PTQD4rwNeAyr4V9s9KtRb4PHt0I+fdrabCvyZgZQNmRmso7+ZW2Lg00hAwIhaIs3z4moXSPJC2tk1+5AyFe7tS2jorGLg32TDYmlfhhJnIW4PPQD4r0qWoSf/+DHeywwK/92DXcObT1sedenlG4Pq9kem0nU7EeuUJ1RgBwX82MjzEGl7pDp4CrjxXDTCiTCe" },
            { "Guns", "CAAAADx3SG8eYzoAeJYLkm09XMKdEYOE+l/Enz9tFP5pplHGi/o+XDjCGrpnUeqSW8wWfhfzPXkD9ReI8ioo7fXi1GOdZB9jMgB4la4Y5JoaLUt0dZXF/aHDuQFpIj73SHaA0FyjQrgVoIU81HPlhGZy3NqVLFXilJVY4Q==" },
            { "VOID", "BQAAAP51Dcw7CEgAeL1iJ2Bkj3KRCONbQGclHWF0qJ8JbM6WARDYeJ25oH2Gb+gA7mZ4aEbkznWVce9LjcNa5uMef1QpNRjwkbVYM3VVE3mFHbpLPAhDAHi//50nTm569Waiu/yxAamDw0GX8pimEBAILbZJthSkvj3qaSXeJcSOQKdcJe57KBn1tBx8CUJz1s09JOJzP+nHsmM9CDsAeL1w7SDWm09Uc/WynowuxSIh5GhyIxo4rvjOpPT9KDQIlGz2DBcPQVJByj+Hhh120NFh4Mn7OgcKsUo=" },
            { "Loud VOID", "AAAAAGfp+Lv2GRkA+DrJaWerbtZm+SX2//kATwCqqvu/z6rLog==" },
            { "Loud VOID 2", "AAAAAGfp+Lv2GRkA+DrDusChW99guelWc00gcgDuhh911CBUpe==" },
            { "Loud", "AAAAAGfp+Lv2GRkA+DrDusChW99tttlWc00gcgDuhh911CBUpe==" },
            { "Loud 2", "AAAAAGfp+Lv2GRkA+DrDussssss99wssstssssstttsswwsese==" },
            { "Loud 3", "AAAAAGfp+Lv2GRkA+DrJaWerbistduJawerbistdujawerbist==" },
            { "Loud 4", "AAAAAGfp+Lv2GRkA+DrDusChW9QFHJlWc00gcgDuhh911CBUpe==" },
            { "Loud 5", "AAAAAGfp+Lv2GRkA+DrDufffffftrhehfgdhgdrgerer1CBUpe==" },
            { "UnNamed 1", "AAAAAAAAAABwAIEA+H0pLImhhNcqqrJngAiSsMsfrJYUoX+c9CxzzIUKRiOG0p1k1A9pz4LFu7ia7FS05EQNYYXm6ho65hy/XKFPlNjT3UwXUKfZjzH8JBwmWc1zSoi+/D9X25fto5tbE7jFvkD1zBF1r9Qe6WUebLWJNlTJIGLt58xd6nsoK1EXrOi7cQByAPhGoG3um9LrR/t19XRWsSu9nM57TWdVudXixVGWN+EUxcGpC+UteaweG+rnFGEO/GVw92mdyDyLacQJ4+MZxvX+pRlQde1C5wDbyu1XCgYfYqJIdVdCvOLhUJeDqPMmWv0Yb/iVEgJB/wYVgHdKfH57gQ==" },
            { "UnNamed 2", "AAAAAAAAAAB0AHoA+Ew266WILK0IkUCWmqKh6wkYZSRcHpaYQeG82KGQapXNemv/4688ZEWEPpLzJRvHHs7GaZUVQzXVVjsrs6XzT0JeD/4v5dq/0VEbk3svawjPEZN1owYrBrIJ+IGIk2r6oIFbgIfjMcgH2hioWJIgLUKGq5Ij90X+Z2J1AHoA+EaeGi0JVtSzJdvu6sEBunJlilJSrkdX7pSPpzZLMBjaCbs2fsHOkUVbLCh1nAR+/GUKZ4AgjcP9jCzMOHK6Psi9QK4buQzf4MxlemtzlI4yRJIcU1TK4EYWXiLWw4If9ry+kf0rzlMG6RXc+EWel5lT8se1My1B5HA=" },
            { "UnMamed 3", "eQAAAAQYL3oADjsA+H3owCVgPq4w5B67lcSx14zff9FCPAAgVkFOSUxMQUNMSUVOVE9OVE9QTE9PU0VSUyDdwCVgPq4w3cA=" },
            { "Kemono E1", "AAAAAGfp+Lv2GRkA+MrI08yxTwBkxqwATk9LRU0wTk9LM00wTg==" },
            { "quiet e1", "ZgYAAGuEBygADjsA+H3owCVgPq4w5B67lcSx14zff9FCPAAgS0VNT05PLlBFVCBYIElOVkFMSUQuR0cgICAgICnAJWA+rjA=" },
            { "VRChat sound", "QwYAAEWhESjzrD0A+PTUA4+bi+0LaUxtDTCBf75zt9hhu0RMSn256S+Z5UFCa3TTpz7Vn+dqmsK22eM1c3QV2OkEnvb+V/VMgfSsPQD49NQ67K6//enjQ8caLBaso6feWZyjV1q6GQ09u6w6bw91CJzBBv8QxGNMEa8S0ZHgYsGLpNZYHzhn03iA9aw9APj0cbv6WD3sl6rbmZYvfDksrFMhDuaBoQeYWfXNDDFik9egcVcvAPfocJkwpJ7vRPS5QgCfiNUdn/AGbIH2rD0A+PTUNLlMaIau6JuUEFFVYpv/yWOVDLSshOI1mmUB9ujkr8KEmIu3keB87DekOFGRmaNgu8TWVvVXjTLogPesPQD49HQVoW8ADMH2KouFZ8eZB3tv/2X+ld6MklOeIE7HE+cY+m1QEkeUgdM0Fc+vQi5ZI21+sAEnmaXx1WqB" },
            { "EW Abyss User", "AgAAAKWkyYm7hjsA+H3owFygUv4w5B67lcSx14zff9FCPADiNbSwYWgE+O7DrSy5tkRecs21ljjofvebe6xsYlA4cVmgrd0=" },
            { "Random", "wAMAAN904mKB/DoAeIWe0qXAP5fMnbbWkp3KRy8riLQRLQu+bY2+P4zfGqlMcVMAld+ortl80FxyqrXqqQ9txshOqaEHvoL8QgB4hfZeKRQ9KpPrkRBVu9RcyvFPz+wiStlj3IJNaxcXuuKqGT1SGz675GUGav69vFUx5ah7poOxh3Kc313ASKP/6Eo=" },
        };
        public static List<string> BlacklistedBytes => blacklistedBytes;

        public static bool IsActorEventBlocked(int Actor, int Code)
        {
            if (!EventBlocks.ContainsKey(Actor)) EventBlocks[Actor] = new List<int>();
            if (EventBlocks[Actor].Contains(Code)) return true;
            return false;
        }

        public static void RemoveActorBlocks(int Actor)
        {
            if (EventBlocks.ContainsKey(Actor)) EventBlocks.Remove(Actor);
        }
        public static void ClearEventBlocks()
        {
            EventBlocks.Clear();
            BlacklistedBytes.Clear();
        }
        public static bool SanitizeEvent4(int Actor, byte[][] Data, byte Code)
        {
            try
            {
                foreach (byte[] Array in Data)
                {
                    Il2CppSystem.Object obj = new VRC_EventLog.ObjectNPublicInVrInStSiInObSiByVrUnique();
                    var evtLogEntry = obj.TryCast<VRC_EventLog.ObjectNPublicInVrInStSiInObSiByVrUnique>();
                    if (evtLogEntry == null)
                    {
                        LimitActor(Actor, Code, "Invalid RPC Entry");
                        return false;
                    }

                    if (evtLogEntry.prop_VrcEvent_0 == null)
                    {
                        LimitActor(Actor, Code, "Invalid VRCEvent");
                        return false;
                    }

                    if (evtLogEntry.prop_VrcEvent_0.EventType > VRC_EventHandler.VrcEventType.CallUdonMethod || evtLogEntry.prop_VrcEvent_0.EventType < VRC_EventHandler.VrcEventType.MeshVisibility)
                    {
                        LimitActor(Actor, Code, "Invalid Type");
                        return false;
                    }

                    if (evtLogEntry.prop_String_0 == null)
                    {
                        LimitActor(Actor, Code, "Invalid Path");
                        return false;
                    }
                    if (!evtLogEntry.prop_String_0.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == ':' ||c == '/' || c == ' ' || c == '(' || c == ')' || c == '-' || c == '_'))
                    {
                        LimitActor(Actor, Code, "Invalid Path");
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static bool SanitizeEvent6(int Actor, byte[] Data, byte Code)
        {
            try
            {
                if (!IsValidServertime(BitConverter.ToInt32(Data, 1)))
                {
                    LimitActor(Actor, Code, "Invalid Time");
                    return false;
                }

                if (BitConverter.ToInt32(Data, 5) != Actor) // on object instantiation its something like actor + 0000 whatever?
                {
                    LimitActor(Actor, Code, "Invalid Actor");
                    return false;
                }

                Il2CppSystem.Object obj = new VRC_EventLog.ObjectNPublicInVrInStSiInObSiByVrUnique();
                var evtLogEntry = obj.TryCast<VRC_EventLog.ObjectNPublicInVrInStSiInObSiByVrUnique>(); 
                if (evtLogEntry == null)
                {
                    LimitActor(Actor, Code, "Invalid RPC Entry");
                    return false;
                }

                if (evtLogEntry.prop_VrcEvent_0 == null)
                {
                    LimitActor(Actor, Code, "Invalid VRCEvent");
                    return false;
                }

                if (evtLogEntry.prop_VrcEvent_0.EventType > VRC_EventHandler.VrcEventType.CallUdonMethod || evtLogEntry.prop_VrcEvent_0.EventType < VRC_EventHandler.VrcEventType.MeshVisibility) // add check if its sendrpc cuz things can be different there? atleast requi made this in network sanity lol
                {
                    LimitActor(Actor, Code, "Invalid Type");
                    return false;
                }

                if (evtLogEntry.prop_String_0 == null)
                {
                    LimitActor(Actor, Code, "Invalid Path");
                    return false;
                }

                if (!evtLogEntry.prop_String_0.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == ':' || c == '/' || c == ' ' || c == '(' || c == ')' || c == '-' || c == '_'))
                {
                    LimitActor(Actor, Code, "Invalid Path");
                    return false;
                }

                string Base64 = Convert.ToBase64String(Data);
                if (BlacklistedBytes.Contains(Base64))
                {
                    if (evtLogEntry.prop_VrcEvent_0.ParameterString != "UdonSyncRunProgramAsRPC") LimitActor(Actor, Code, "Blacklisted"); // fix udon one day
                    return false;
                }
                BlacklistedBytes.Add(Base64);

                return true;
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static void LimitActor(int Actor, byte EventCode, string Reason, int Seconds = 20)
        {
            if (IsActorEventBlocked(Actor, EventCode)) return;

            EventBlocks[Actor].Add(EventCode);
            GeneralUtils.DelayAction(Seconds, delegate
            {
                if (EventBlocks.ContainsKey(Actor) && EventBlocks[Actor].Contains(EventCode)) EventBlocks[Actor].Remove(EventCode);
            }).Start();

            Photon.Realtime.Player PhotonPlayer = GameHelper.VRCNetworkingClient.GetPhotonPlayer(Actor);
            if (PhotonPlayer != null)
            {
                string DisplayName = PhotonPlayer.DisplayName() ?? "NO NAME";
                HDLogger.Log($"Limiting {DisplayName} Event {EventCode} for {Seconds} Seconds [{Reason}]", HDLogger.LogsType.Protection);
                VRConsole.Log($"{DisplayName} > Bad Event [{EventCode}] for {Seconds}s - [{Reason}]", VRConsole.LogsType.Protection);
            }
        }
        public static bool SanitizeEvent7(int Actor, byte[] Data, byte Code)
        {
            try
            {
                int viewID = BitConverter.ToInt32(Data, 0);
                if (viewID.ToString().EndsWith("00001")) return SanitizeEvent12(Actor, Data, Code);
                else return SanitizeEvent17(Actor, Data, Code); // idfk if this is right 
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }
        public static bool IsValidServertime(int Time, int difference = 60000)
        {
            if (Time < GameHelper.VRCNetworkingClient.prop_LoadBalancingPeer_0.ServerTimeInMilliSeconds - difference || Time > GameHelper.VRCNetworkingClient.prop_LoadBalancingPeer_0.ServerTimeInMilliSeconds + difference) return false;
            return true;
        }
        public static bool SanitizeEvent1(int Actor, byte[] Data, byte Code)
        {
            try
            {
                Photon.Realtime.Player PhotonPlayer = GameHelper.VRCNetworkingClient.GetPhotonPlayer(Actor);
                if (PhotonPlayer == null)
                {
                    LimitActor(Actor, Code, "Invalid PhotonPlayer");
                    return false;
                }

                Player p = PhotonPlayer.GetPlayer();
                if (p == null)
                {
                    LimitActor(Actor, Code, "Invalid VRCPlayer");
                    return false;
                }

                if (p._USpeaker.field_Private_Single_22 < 0.05f)
                {
                    p._USpeaker.field_Private_Single_22 = 0.5f;
                    LimitActor(Actor, Code, "Invalid Scale");
                    return false;
                }

                string Base64 = Convert.ToBase64String(Data.Skip(8).ToArray());

                // Try to find the payload name that matches this Base64 string
                var match = KnownBadVoicePayloads.FirstOrDefault(kv => kv.Value == Base64);
                if (!string.IsNullOrEmpty(match.Value))
                {
                    LimitActor(Actor, Code, $"Known Malicious Voice Payload: {match.Key}");
                    return false;
                }

                if (BlacklistedBytes.Contains(Base64))
                {
                    LimitActor(Actor, Code, "Repeated Voice Payload (Blacklisted)");
                    return false;
                }

                BlacklistedBytes.Add(Base64);

                return true;
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static bool SanitizeEvent16(int Actor, byte[] Data, byte Code)
        {
            try
            {
                if (BitConverter.ToInt32(Data, 0) < 0)
                {
                    LimitActor(Actor, Code, "Invalid Actor");
                    return false;
                }

                if (!IsValidServertime(BitConverter.ToInt32(Data, 4)))
                {
                    LimitActor(Actor, Code, "Invalid Time");
                    return false;
                }

                int posOffset = Data[16] + 16 + 1;
                float x = BitConverter.ToSingle(Data, posOffset);
                float y = BitConverter.ToSingle(Data, posOffset + 4);
                float z = BitConverter.ToSingle(Data, posOffset + 8);
                UnityEngine.Vector3 Position = new UnityEngine.Vector3(x, y, z);
                if (UnityUtils.IsBadPosition(Position) || UnityUtils.IsBadDistance(GameHelper.CurrentPlayer.transform.position, Position))
                {
                    LimitActor(Actor, Code, "Invalid Position");
                    return false;
                }

                UnityEngine.Quaternion Rotation = new UnityEngine.Quaternion();
                if (UnityUtils.IsBadRotation(Rotation))
                {
                    LimitActor(Actor, Code, "Invalid Rotation");
                    return false;
                }

                string Base64 = Convert.ToBase64String(Data);
                if (BlacklistedBytes.Contains(Base64))
                {
                    LimitActor(Actor, Code, "Blacklisted");
                    return false;
                }
                BlacklistedBytes.Add(Base64);

                return true;
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }
        public static bool SanitizeEvent10(int Actor, byte[] Data, byte Code)
        {
            try
            {
                if (BitConverter.ToInt32(Data, 0) < 0)
                {
                    LimitActor(Actor, Code, "Invalid Actor");
                    return false;
                }

                if (!IsValidServertime(BitConverter.ToInt32(Data, 4)))
                {
                    LimitActor(Actor, Code, "Invalid Time");
                    return false;
                }

                string Base64 = Convert.ToBase64String(Data);
                if (BlacklistedBytes.Contains(Base64))
                {
                    LimitActor(Actor, Code, "Blacklisted");
                    return false;
                }
                BlacklistedBytes.Add(Base64);

                return true;
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static bool SanitizeEvent11(int Actor, byte[] Data, byte Code)
        {
            try
            {
                int EventActor = BitConverter.ToInt32(Data, 0); // ive only seen this at 1000 yet, might be the type what it syncs?
                if (EventActor < 0)
                {
                    LimitActor(Actor, Code, "Invalid Actor");
                    return false;
                }

                if (!IsValidServertime(BitConverter.ToInt32(Data, 4)))
                {
                    LimitActor(Actor, Code, "Invalid Time");
                    return false;
                }

                int FoundIndex = GeneralUtils.lastIndexOfByteArray(Data, new byte[] { 4, 0, 0, 0 });
                if (FoundIndex != -1)
                {
                    int Offset = FoundIndex + 4;
                    int typeFromIndex = BitConverter.ToInt32(Data, Offset);

                    if (typeFromIndex > 1500 || typeFromIndex < 0)
                    {
                        LimitActor(Actor, Code, "Invalid Type");
                        return false;
                    }
                }

                string Base64 = Convert.ToBase64String(Data);
                if (BlacklistedBytes.Contains(Base64))
                {
                    LimitActor(Actor, Code, "Blacklisted");
                    return false;
                }
                BlacklistedBytes.Add(Base64);

                return true;
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }
        public static bool SanitizeEvent12(int Actor, byte[] Data, byte Code)
        {
            try
            {
                int viewID = BitConverter.ToInt32(Data, 0);
                if (viewID != int.Parse(Actor + "00001"))
                {
                    LimitActor(Actor, Code, "Invalid Actor");
                    return false;
                }

                if (!IsValidServertime(BitConverter.ToInt32(Data, 4)))
                {
                    LimitActor(Actor, Code, "Invalid Time");
                    return false;
                }

                int PoseRecorderOffset = 17 + Data[17];
                int PlayerNetOffset = 18 + Data[18];
                int PlayerSyncOffset = 19 + Data[19];

                if (Data.Length < PoseRecorderOffset || Data.Length < PlayerNetOffset || Data.Length < PlayerSyncOffset)
                {
                    LimitActor(Actor, Code, "Invalid Offsets");
                    return false;
                }

                int syncOffset = PlayerSyncOffset + 1;
                float playerX = BitConverter.ToSingle(Data, syncOffset);
                float playerY = BitConverter.ToSingle(Data, syncOffset + 4);
                float playerZ = BitConverter.ToSingle(Data, syncOffset + 8);
                UnityEngine.Vector3 playerPos = new UnityEngine.Vector3(playerX, playerY, playerZ);
                if (UnityUtils.IsBadPosition(playerPos) || UnityUtils.IsBadDistance(GameHelper.CurrentPlayer.transform.position, playerPos))
                {
                    LimitActor(Actor, Code, "Invalid Player Position");
                    return false;
                }

                string Base64 = Convert.ToBase64String(Data.Skip(8).ToArray());
                if (BlacklistedBytes.Contains(Base64))
                {
                    LimitActor(Actor, Code, "Blacklisted");
                    return false;
                }
                BlacklistedBytes.Add(Base64);

                return true;
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }
        public static bool SanitizeEvent13(int Actor, byte[] Data, byte Code)
        {
            try
            {
                if (BitConverter.ToInt32(Data, 0) != int.Parse(Actor + "00003"))
                {
                    LimitActor(Actor, Code, "Invalid Actor");
                    return false;
                }

                if (!IsValidServertime(BitConverter.ToInt32(Data, 4)))
                {
                    LimitActor(Actor, Code, "Invalid Time");
                    return false;
                }

                string Base64 = Convert.ToBase64String(Data);
                if (BlacklistedBytes.Contains(Base64))
                {
                    LimitActor(Actor, Code, "Blacklisted");
                    return false;
                }
                BlacklistedBytes.Add(Base64);

                return true;
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static bool SanitizeEvent17(int Actor, byte[] Data, byte Code)
        {
            try
            {
                if (BitConverter.ToInt32(Data, 0) < 0)
                {
                    LimitActor(Actor, Code, "Invalid Actor");
                    return false;
                }

                if (!IsValidServertime(BitConverter.ToInt32(Data, 4)))
                {
                    LimitActor(Actor, Code, "Invalid Time");
                    return false;
                }

                string Base64 = Convert.ToBase64String(Data);
                if (BlacklistedBytes.Contains(Base64))
                {
                    LimitActor(Actor, Code, "Blacklisted");
                    return false;
                }
                BlacklistedBytes.Add(Base64);

                return true;
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static bool SanitizeEvent202(int Actor, Il2CppSystem.Collections.Hashtable Data, byte Code)
        {
            try
            {
                if (Data.ContainsKey("6") && !IsValidServertime(Data["6"].Unbox<int>()))
                {
                    LimitActor(Actor, Code, "Invalid Time");
                    return false;
                }

                return true;
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static bool SanitizeEvent21(int Actor, int[] ViewIDs, byte Code)
        {
            try
            {
                if (ViewIDs.Length != 2)
                {
                    LimitActor(Actor, Code, "Invalid Lenght");
                    return false;
                }

                if (ViewIDs[0] < 0)
                {
                    LimitActor(Actor, Code, "Invalid ViewID");
                    return false;
                }

                if (ViewIDs[1] < 0) // maybe add check if actor is existing but i remember it can set to 0 or maybe it was a viewid idk tbh
                {
                    LimitActor(Actor, Code, "Invalid Actor");
                    return false;
                }

                return true;
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static bool SanitizeEvent22(int Actor, int[] ViewIDs, byte Code)
        {
            try
            {
                if (ViewIDs.Length != 2)
                {
                    LimitActor(Actor, Code, "Invalid Lenght");
                    return false;
                }

                if (ViewIDs[0] < 0)
                {
                    LimitActor(Actor, Code, "Invalid ViewID");
                    return false;
                }

                if (ViewIDs[1] < 0) // maybe add check if actor is existing but i remember it can set to 0 or maybe it was a viewid idk tbh
                {
                    LimitActor(Actor, Code, "Invalid Actor");
                    return false;
                }

                return true;
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static bool SanitizeEvent210(int Actor, int[] ViewIDs, byte Code)
        {
            try
            {
                if (ViewIDs.Length != 2)
                {
                    LimitActor(Actor, Code, "Invalid Lenght");
                    return false;
                }

                if (ViewIDs[0] < 0)
                {
                    LimitActor(Actor, Code, "Invalid ViewID");
                    return false;
                }

                if (ViewIDs[1] < 0) // maybe add check if actor is existing but i remember it can set to 0 or maybe it was a viewid idk tbh
                {
                    LimitActor(Actor, Code, "Invalid Actor");
                    return false;
                }

                return true;
            }
            catch
            {
                LimitActor(Actor, Code, "Exception");
                return false;
            }
        }

        public static bool CheckDecodedRPC(Player player, VRC_EventHandler.VrcEvent ev, VRC_EventHandler.VrcBroadcastType type, int instagatorId, float fastForward, object[] decoded)
        {
            try
            {
                int actorId = player.GetPhotonPlayer().ActorID();
                int localId = GameHelper.VRCNetworkingClient.GetCurrentPlayer().ActorID();

                switch (ev.EventType)
                {
                    case VRC_EventHandler.VrcEventType.AddDamage:
                        if (InternalSettings.GodMode && actorId == localId)
                        {
                            HDLogger.Log($"{player.DisplayName()} AddDamage blocked by GodMode", HDLogger.LogsType.Protection);
                            return false;
                        }
                        break;

                    case VRC_EventHandler.VrcEventType.TeleportPlayer:
                        if (InternalSettings.NoTeleport && actorId == localId)
                        {
                            HDLogger.Log($"{player.DisplayName()} Teleport blocked", HDLogger.LogsType.Protection);
                            return false;
                        }
                        HDLogger.Log($"{player.DisplayName()} used Teleport", HDLogger.LogsType.Room);
                        break;

                    case VRC_EventHandler.VrcEventType.SendRPC:
                        if (string.IsNullOrEmpty(ev.ParameterString))
                        {
                            HDLogger.Log($"Missing ParameterString from {player.DisplayName()}", HDLogger.LogsType.Protection);
                            return false;
                        }

                        switch (ev.ParameterString)
                        {
                            case "PlayEmoteRPC":
                                if (decoded.Length >= 1)
                                {
                                    int emote = decoded[0] is float f ? (int)f : decoded[0] is int i ? i : -1;
                                    if (actorId != localId && (emote < 1 || emote > 8))
                                    {
                                        HDLogger.Log($"{player.DisplayName()} used invalid emote: {emote}", HDLogger.LogsType.Protection);
                                        LimitActor(actorId, 6, "Invalid Emote RPC");
                                        return false;
                                    }
                                    HDLogger.Log($"{player.DisplayName()} played emote: {emote}", HDLogger.LogsType.Room);
                                }
                                else
                                {
                                    HDLogger.Log($"{player.DisplayName()} sent invalid Emote RPC (no params)", HDLogger.LogsType.Protection);
                                    return false;
                                }
                                break;

                            default:
                                HDLogger.LogWarning($"Unhandled RPC from {player.DisplayName()}: {ev.ParameterString}");
                                HDLogger.LogRPC(player, ev, type, instagatorId, fastForward, decoded);
                                return false;
                        }
                        break;

                    default:
                        HDLogger.LogWarning($"Unhandled event type: {ev.EventType} from {player.DisplayName()}");
                        HDLogger.LogRPC(player, ev, type, instagatorId, fastForward, decoded);
                        return false;
                }
            }
            catch (Exception ex)
            {
                HDLogger.LogError($"Exception in CheckDecodedRPC: {ex}");
                LimitActor(player.GetPhotonPlayer().ActorID(), 6, "RPC Exception");
                return false;
            }

            return true;
        }

        public static bool CheckWorldID(string ID)
        {
            if (!ID.Contains(':')) return false;
            string WorldID = ID.Split(':')[0];
            string InstanceID = ID.Split(':')[1];

            if (WorldID.Length != 41 || InstanceID.Length < 1) return false;
            if (!WorldID.StartsWith("wrld_")) return false;
            if (!WorldID.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '-')) return false;
            if (!InstanceID.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '-' || c == '(' || c == ')' || c == '~' || c == '&')) return false;

            return true;
        }

        public static bool CheckGroupID(string ID)
        {
            if (ID.Length != 40) return false;
            if (!ID.StartsWith("grp_")) return false;
            if (!ID.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '-')) return false;

            return true;
        }

        public static bool CheckFileID(string ID)
        {
            if (ID.Length != 41) return false;
            if (!ID.StartsWith("file_")) return false;
            if (!ID.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '-')) return false;

            return true;
        }
    }
}
