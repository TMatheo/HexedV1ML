using LUXED.Extensions;
using LUXED.UIApi;
using LUXED.Wrappers;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

namespace LUXED.CustomUI.QM
{
    internal class UdonMenu
    {
        public static void Init()
        {
            QMScrollButton UdonScroll = new QMScrollButton(MainMenu.ClientPage, 3, 2, "Udon", "Udon/Trigger Options", ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Udon"));
            QMScrollButton UdonInteract = new QMScrollButton(MainMenu.ClientPage, 2, 2, "Udon Interact", "Udon Interact Options", ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Udon"));
            UdonInteract.MainButton.GetGameObject().SetActive(false);

            UdonScroll.SetAction(delegate
            {
                if (UnityEngine.Object.FindObjectOfType<VRCSceneDescriptor>() != null)
                {
                    UdonScroll.Add("Send \nCustom", "Send Custom Event", delegate
                    {
                        GameHelper.VRCUiPopupManager.AskInGameInput("Custom Event", "Ok", delegate (string text)
                        {
                            foreach (UdonBehaviour Udon in UnityEngine.Object.FindObjectsOfType<UdonBehaviour>()) // replace with resources to get inactive ones?
                            {
                                PhotonHelper.SendUdonRPC(Udon, text);
                            }
                        });
                    });

                    foreach (UdonBehaviour Udon in UnityEngine.Object.FindObjectsOfType<UdonBehaviour>()) // replace with resources to get inactive ones?
                    {
                        UdonScroll.Add(Udon.name, "Open Event Menu", delegate
                        {
                            UdonInteract.SetAction(delegate
                            {
                                foreach (string UdonEvent in Udon._eventTable.Keys)
                                {
                                    UdonInteract.Add(UdonEvent, "Trigger Event", delegate
                                    {
                                        PhotonHelper.SendUdonRPC(Udon, UdonEvent);
                                    });
                                }
                            });
                            UdonInteract.MainButton.ClickMe();
                        });
                    }
                }
                else
                {
                    HDLogger.Log("SDK2 World found, using Trigger Menu instead", HDLogger.LogsType.Info);
                    foreach (VRC_Trigger Trigger in UnityEngine.Object.FindObjectsOfType<VRC_Trigger>())
                    {
                        UdonScroll.Add(Trigger.name, "Interact", delegate
                        {
                            Trigger.Interact();
                        });
                    }
                }
            });
        }
    }
}
