using LUXED.Extensions;
using LUXED.Modules.Standalone;
using LUXED.UIApi;
using LUXED.Wrappers;
using VRC.Udon;

namespace LUXED.CustomUI.SelectedUserMenu
{
    internal class UtilsMenu
    {
        private static QMMenuPage UtilsPage;
        public static void Init()
        {
            UtilsPage = new QMMenuPage("Player Utils");

            QMSingleButton OpenMenu = new QMSingleButton(MainMenu.ClientPage, 1, 0, "Utils", UtilsPage.OpenMe, "Utils Options", ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Utils"));

            new QMSingleButton(UtilsPage, 1, 0, "Teleport", delegate
            {
                GameHelper.CurrentPlayer.transform.position = PlayerSimplifier.GetSelectedPlayer().transform.position;
            }, "Teleport to the Player", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(UtilsPage, 1, 0.5f, "Copy \nUserID", delegate
            {
                GeneralUtils.CopyToClipboard(PlayerSimplifier.GetSelectedPlayer().UserID());
            }, "Copy the UserID", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(UtilsPage, 2, 0, "Dump \nProps", delegate
            {
                var jsonSettings = new Il2CppNewtonsoft.Json.JsonSerializerSettings
                {
                    Formatting = Il2CppNewtonsoft.Json.Formatting.Indented
                };
                string json = Il2CppNewtonsoft.Json.JsonConvert.SerializeObject((Il2CppSystem.Object)CPP2IL.BinaryConverter.IL2CPPToManaged(PlayerSimplifier.GetSelectedPlayer().GetPhotonPlayer().GetProperties()),jsonSettings);
                HDLogger.Log(json, HDLogger.LogsType.Info);
            }, "Dump the Player properties", ButtonAPI.ButtonSize.Half);

            QMScrollButton UdonScroll = new QMScrollButton(UtilsPage, 2, 0.5f, "Udon", "Udon Options", ButtonAPI.ButtonSize.Half);
            QMScrollButton UdonInteract = new QMScrollButton(UtilsPage, 2, 2, "Udon Interact", "Udon Interact Options");
            UdonInteract.MainButton.GetGameObject().SetActive(false);

            UdonScroll.SetAction(delegate
            {
                UdonScroll.Add("Send \nCustom", "Send Custom Event", delegate
                {
                    GameHelper.VRCUiPopupManager.AskInGameInput("Custom Event", "Ok", delegate (string text)
                    {
                        foreach (UdonBehaviour Udon in UnityEngine.Object.FindObjectsOfType<UdonBehaviour>())
                        {
                            PhotonHelper.SendUdonRPC(Udon, text, PlayerSimplifier.GetSelectedPlayer());
                        }
                    });
                });

                foreach (UdonBehaviour Udon in UnityEngine.Object.FindObjectsOfType<UdonBehaviour>())
                {
                    UdonScroll.Add(Udon.name, "Open Event Menu", delegate
                    {
                        UdonInteract.SetAction(delegate
                        {
                            foreach (string UdonEvent in Udon._eventTable.Keys)
                            {
                                UdonInteract.Add(UdonEvent, "Trigger Event", delegate
                                {
                                    PhotonHelper.SendUdonRPC(Udon, UdonEvent, PlayerSimplifier.GetSelectedPlayer());
                                });
                            }
                        });
                        UdonInteract.MainButton.ClickMe();
                    });
                }
            });

            new QMSingleButton(UtilsPage, 3, 0, "Copy \nActorID", delegate
            {
                GeneralUtils.CopyToClipboard(PlayerSimplifier.GetSelectedPlayer().GetPhotonPlayer().ActorID().ToString());
            }, "Copy the ActorID", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(UtilsPage, 3, 0.5f, "Copy \nName", delegate
            {
                GeneralUtils.CopyToClipboard(PlayerSimplifier.GetSelectedPlayer().DisplayName());
            }, "Copy the Name", ButtonAPI.ButtonSize.Half);

            new QMToggleButton(UtilsPage, 4, 0, "Always \nHear", delegate
            {
                ExploitHandler.ListenPlayer(PlayerSimplifier.GetSelectedPlayer(), true);
            }, delegate
            {
                ExploitHandler.ListenPlayer(PlayerSimplifier.GetSelectedPlayer(), false);
            }, "Always listen to the Player");
        }
    }
}
