using LUXED.Modules.Standalone;
using LUXED.Wrappers;

namespace LUXED.Modules.EventManagement
{
    internal class UserPropHandler
    {
        public static bool ReceivedPropEvent(Il2CppSystem.Collections.Hashtable Data, Photon.Realtime.Player PhotonPlayer)
        {
            string oldAvatarID = PhotonPlayer.AvatarID();
            string oldGroupID = PhotonPlayer.RepresentedGroupID();
            bool oldVRMode = PhotonPlayer.IsVR();
            bool oldShowGroup = PhotonPlayer.ShowGroupToOthers();
            bool oldShowRank = PhotonPlayer.ShowSocialRankToOthers();
            bool oldUseImpostor = PhotonPlayer.UseImpostorAsFallback();
            int oldAvatarHeight = PhotonPlayer.AvatarHeight();

            PhotonUtils.networkedProperties[PhotonPlayer.ActorID()] = Data;

            string newAvatarID = PhotonPlayer.AvatarID();
            string newGroupID = PhotonPlayer.RepresentedGroupID();
            bool newVRMode = PhotonPlayer.IsVR();
            bool newShowGroup = PhotonPlayer.ShowGroupToOthers();
            bool newShowRank = PhotonPlayer.ShowSocialRankToOthers();
            bool newUseImpostor = PhotonPlayer.UseImpostorAsFallback();
            int newAvatarHeight = PhotonPlayer.AvatarHeight();

            string DisplayName = PhotonPlayer.DisplayName() ?? "NO NAME";

            if (oldAvatarID != newAvatarID)
            {
                string AvatarName = PhotonPlayer.AvatarName() ?? "NO NAME";
                string releaseStatus = PhotonPlayer.AvatarReleaseStatus() ?? "NO RELEASE";

                HDLogger.Log($"{DisplayName} changed Avatar from {oldAvatarID} to {newAvatarID}", HDLogger.LogsType.Room);
                VRConsole.Log($"{DisplayName} --> {AvatarName} [{releaseStatus}]", VRConsole.LogsType.Avatar);
            }

            if (oldGroupID != newGroupID)
            {
                HDLogger.Log($"{DisplayName} changed represented Group from {oldGroupID} to {newGroupID}", HDLogger.LogsType.Group);
                VRConsole.Log($"{DisplayName} --> Group update", VRConsole.LogsType.Group);
            }

            if (oldVRMode != newVRMode)
            {
                HDLogger.Log($"{DisplayName} changed VR State from {oldVRMode} to {newVRMode}", HDLogger.LogsType.Protection);
                VRConsole.Log($"{DisplayName} --> {(newVRMode ? "VR Switch" : "Desktop Switch")}", VRConsole.LogsType.Protection);
            }

            if (oldShowGroup != newShowGroup)
            {
                HDLogger.Log($"{DisplayName} changed Group visibility from {oldShowGroup} to {newShowGroup}", HDLogger.LogsType.Group);
                VRConsole.Log($"{DisplayName} --> {(newShowGroup ? "Show Group" : "Hide Group")}", VRConsole.LogsType.Group);
            }

            if (oldShowRank != newShowRank)
            {
                HDLogger.Log($"{DisplayName} changed Rank visibility from {oldShowRank} to {newShowRank}", HDLogger.LogsType.Room);
                VRConsole.Log($"{DisplayName} --> {(newShowRank ? "Show Rank" : "Hide Rank")}", VRConsole.LogsType.Room);
            }

            if (oldUseImpostor != newUseImpostor)
            {
                HDLogger.Log($"{DisplayName} changed Impostor fallback from {oldUseImpostor} to {newUseImpostor}", HDLogger.LogsType.Room);
                VRConsole.Log($"{DisplayName} --> {(newUseImpostor ? "Impostor Fallback" : "Default Fallback")}", VRConsole.LogsType.Avatar);
            }

            if (oldAvatarHeight != newAvatarHeight)
            {
                HDLogger.Log($"{DisplayName} changed Avatar height from {oldAvatarHeight} to {newAvatarHeight}", HDLogger.LogsType.Room);
                VRConsole.Log($"{DisplayName} --> Avatar Height [{newAvatarHeight}]", VRConsole.LogsType.Avatar);
            }
            return true;
        }
    }
}
