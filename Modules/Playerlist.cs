using LUXED.Interfaces;
using LUXED.Modules.EventManagement;
using LUXED.UIApi;
using LUXED.Wrappers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LUXED.Modules
{
    internal class Playerlist : IGlobalModule, IDelayModule
    {
        private static UITextMeshText PlayerlistText;
        private static UITextMeshText HearlistText;

        public void Initialize()
        {

        }
        public void OnUpdate()
        {
            if (!GameUtils.IsInWorld() || !MenuHelper.QuickMenu.isActiveAndEnabled) return;

            if (PlayerlistText == null)
            {
                Transform PlayerlistParent = MenuHelper.QuickMenu.transform.Find("CanvasGroup/Container/Window/Wing_Left/Button/Icon");
                PlayerlistText = new UITextMeshText(PlayerlistParent.gameObject, "Playerlist", new Vector2(-890, 570f), 29, false);
                PlayerlistText.text.color = Color.gray;
            }
            /*
            if (HearlistText == null)
            {
                Transform HearlistParent = MenuHelper.QuickMenu.transform.Find("CanvasGroup/Container/Window/Wing_Right/Button/Icon");
                HearlistText = new UITextMeshText(HearlistParent.gameObject, "Hearlist", new Vector2(215, 570f), 29, false);
                HearlistText.text.color = Color.gray;
            }
            */
            List<string> playerStrings = GetPlayerStrings();

            PlayerlistText.SetText($"In Room: {playerStrings.Count} / {RoomManager.prop_ApiWorldInstance_1.capacity}\n{string.Join("\n", playerStrings)}");
            //HearlistText.SetText($"Hear You: 100 / {playerStrings.Count - 1}\n{string.Join("\n")}");
        }

        public static List<string> GetPlayerStrings()
        {
            return GameHelper.VRCNetworkingClient
                .GetAllPhotonPlayers()
                .OrderBy(p => p.ActorID())
                .Select(GetPlayerString)
                .ToList();
        }

        public static string GetPlayerString(Photon.Realtime.Player photonPlayer)
        {
            VRC.Player player = photonPlayer.GetPlayer();
            if (player == null || player.GetAPIUser() == null) return HandleNullPlayer(photonPlayer);

            string actorIdColor = PlayerUtils.IsFriend(player.UserID()) ? "<color=#ebc400>" : "<color=#7f7f7f>";
            string actorIdString = $"{actorIdColor}{photonPlayer.ActorID()}</color>";

            string deviceColor = $"<color={UnityUtils.ColorToHex(PlayerUtils.GetPlatformColor(player.GetPhotonPlayer().GetPlatform()))}>";
            string deviceTag = deviceColor + (player.GetVRCPlayer().IsPlayerFBT() ? "FBT " : player.GetDevice().GetDeviceTag()) + "</color>";

            string frozenString = PlayerUtils.IsFrozen(photonPlayer.ActorID()) ? "<color=#fc0352>S</color> | " : "";

            string instanceOwnerString = player.IsInstanceOwner() ? "<color=#3c1769>O</color> | " : photonPlayer.IsInstanceModerator() ? "<color=#3c1769>P</color> | " : "";
            string masterString = photonPlayer.IsMaster() ? "<color=#8719fc>M</color> | " : "";

            string displayNameColor = $"<color={UnityUtils.ColorToHex(player.GetAPIUser().GetRankColor())}>";
            string displayNameString = displayNameColor + player.DisplayName() + "</color>";

            return $"{actorIdString} - {deviceTag} | {frozenString}{instanceOwnerString}{masterString}{displayNameString}";
        }

        public static string HandleNullPlayer(Photon.Realtime.Player photonPlayer)
        {
            string userId = photonPlayer.UserID();
            string displayName = photonPlayer.DisplayName() ?? "NO NAME";

            string actorIdColor = userId != null ? PlayerUtils.IsFriend(userId) ? "<color=#ebc400>" : "<color=#7f7f7f>" : "<color=#7f7f7f>";
            string actorIdString = $"{actorIdColor}{photonPlayer.ActorID()}</color>";

            string botString = "<color=#a33333>BOT</color>";

            string deviceColor = $"<color={UnityUtils.ColorToHex(PlayerUtils.GetPlatformColor(photonPlayer.GetPlatform()))}";

            string frozenString = PlayerUtils.IsFrozen(photonPlayer.ActorID()) ? "<color=#fc0352>S</color> | " : "";

            string instanceModeratorString = photonPlayer.IsInstanceModerator() ? "<color=#3c1769>P</color> | " : "";
            string masterString = photonPlayer.IsMaster() ? "<color=#8719fc>M</color> | " : "";

            string displayNameString = "<color=#a33333>" + displayName + "</color>";

            return $"{actorIdString} - {botString} | Average (P:0 | F:0)\n{displayName}";
        }
        
        public static string GetBlockMuteString(int actorId, string userId)
        {
            if (ModerationHandler.BlockList.Contains(actorId)) return "<color=#ff0000>B</color> | ";
            else if (ModerationHandler.MuteList.Contains(actorId)) return "<color=#ff0000>M</color> | ";
            else if (userId != null && GameHelper.ModerationManager.IsBlocked(userId)) return "<color=#424242>B</color> | ";
            else if (userId != null && GameHelper.ModerationManager.IsMuted(userId)) return "<color=#424242>M</color> | ";
            return "";
        }
    }
}
