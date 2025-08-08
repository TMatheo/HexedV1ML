using LUXED.Core;
using LUXED.Interfaces;
using LUXED.Wrappers;

namespace LUXED.Hooking
{
    internal class ExternVRCSDKBaseVRCPlayerApi_get_displayname : IGlobalModule
    {
        public void Initialize()
        {
        }

        public void OnUpdate()
        {
            try
            {
                // Check critical references first
                if (RoomManager.field_Private_Static_ApiWorldInstance_1 == null)
                    return;

                var currentPlayer = GameHelper.CurrentPlayer;
                if (currentPlayer == null)
                    return;

                var vrcPlayerApi = currentPlayer.field_Private_VRCPlayerApi_0;
                var apiUser = currentPlayer.field_Private_APIUser_0;

                if (vrcPlayerApi == null || apiUser == null)
                    return;

                string newName = null;

                switch (InternalSettings.UdonSpoof)
                {
                    case InternalSettings.UdonSpoofMode.Owner:
                        if (RoomManager.field_Internal_Static_ApiWorld_0 != null)
                            newName = RoomManager.field_Internal_Static_ApiWorld_0.authorName;
                        break;

                    case InternalSettings.UdonSpoofMode.Random:
                        newName = EncryptUtils.RandomString(13);
                        break;

                    case InternalSettings.UdonSpoofMode.Custom:
                        newName = InternalSettings.FakeUdonValue;
                        break;
                }

                if (!string.IsNullOrEmpty(newName))
                {
                    vrcPlayerApi.displayName = newName;
                    apiUser.displayName = newName;
                }
            }
            catch (System.Exception ex)
            {
                HDLogger.LogError($"exception: {ex.Message}");
            }
        }
    }
}
