using LUXED.Core;
using LUXED.Extensions;
using LUXED.Modules.Standalone;
using LUXED.Wrappers;
using Photon.Realtime;
using System;
using System.Text;

namespace LUXED.Modules.EventManagement
{
    internal class ChatHandler
    {
        public static bool ReceivedChatEvent(string Data, Player PhotonPlayer, byte EventCode)
        {
            try
            {
                if (Data == null)
                {
                    EventSanitizer.LimitActor(PhotonPlayer.ActorID(), EventCode, "No Data");
                    return false;
                }

                if (Data.Contains("\n") || Data.Contains("\r") || Data.Contains("\t") || Data.Contains("\0") || Data.Contains("\v"))
                {
                    EventSanitizer.LimitActor(PhotonPlayer.ActorID(), EventCode, "Invalid Char");
                    return false;
                }

                string Base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(Data));
                if (EventSanitizer.BlacklistedBytes.Contains(Base64))
                {
                    EventSanitizer.LimitActor(PhotonPlayer.ActorID(), EventCode, "Blacklisted");
                    return false;
                }
                EventSanitizer.BlacklistedBytes.Add(Base64);

                HDLogger.Log($"{PhotonPlayer.GetPlayer().DisplayName()}: {Data}", HDLogger.LogsType.Chat);

                if (PhotonPlayer.ActorID() == InternalSettings.RepeatChatActor) PhotonHelper.RaiseChatMessage(Data);

                return true;
            }
            catch
            {
                EventSanitizer.LimitActor(PhotonPlayer.ActorID(), EventCode, "Exception");
                return false;
            }
        }

        public static bool RaisedChatEvent(string Data)
        {
            if (FakeSerialize.BotSerialize)
            {
                BotConnection.SelfbotChat(Data);
            }
            return true;
        }
    }
}