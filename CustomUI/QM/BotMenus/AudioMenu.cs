using LUXED.Modules.Standalone;
using LUXED.UIApi;
using LUXED.Wrappers;
using System.IO;

namespace LUXED.CustomUI.QM.BotMenus
{
    internal class AudioMenu
    {
        private static QMMenuPage AudioPage;
        public static void Init()
        {
            AudioPage = new QMMenuPage("Bot Audio");

            QMSingleButton OpenMenu = new QMSingleButton(BotMenu.BotsPage, 1, 1.5f, "Audio", AudioPage.OpenMe, "Bot Audio Options", ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Options"));

            QMScrollButton AudioScroll = new QMScrollButton(AudioPage, 1, 0, "Audio \nFiles", "Audio Files", ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Options"));
            QMScrollButton DirScroll = new QMScrollButton(AudioPage, 1, 0, "Audio Options", "Audio File Options");
            DirScroll.MainButton.GetGameObject().SetActive(false);

            AudioScroll.SetAction(delegate
            {
                foreach (string Dir in Directory.GetDirectories("Luxed\\Bots\\Audios"))
                {
                    string FolderName = Dir.Replace("Luxed\\Bots\\Audios\\", "");
                    AudioScroll.Add(FolderName, "Open the Subfolder", delegate
                    {
                        DirScroll.SetAction(delegate
                        {
                            foreach (string AudioFile in Directory.GetFiles($"Luxed\\Bots\\Audios\\{FolderName}"))
                            {
                                string AudioName = AudioFile.Replace($"Luxed\\Bots\\Audios\\{FolderName}\\", "").Split('.')[0];

                                DirScroll.Add(AudioName, "Play the Audio", delegate
                                {
                                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), AudioFile);
                                    BotConnection.PlayAudioFile(EncryptUtils.ToBase64(fullPath));
                                });
                            }
                        });
                        DirScroll.MainButton.ClickMe();
                    });
                }

                foreach (string AudioFile in Directory.GetFiles("Luxed\\Bots\\Audios"))
                {
                    string AudioName = AudioFile.Replace("Luxed\\Bots\\Audios\\", "").Split('.')[0];

                    AudioScroll.Add(AudioName, "Play the Audio", delegate
                    {
                        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), AudioFile);
                        BotConnection.PlayAudioFile(EncryptUtils.ToBase64(fullPath));
                    });
                }
            });

            new QMSingleButton(AudioPage, 2, 0, "TTS", delegate
            {
                GameHelper.VRCUiPopupManager.AskInGameInput("Text Message", "Ok", delegate (string text)
                {
                    BotConnection.PlayTextToVoice(text);
                });
            }, "Send a Text to Speech message", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(AudioPage, 2, 0.5f, "Youtube", delegate
            {
                GameHelper.VRCUiPopupManager.AskInGameInput("Youtube URL", "Ok", delegate (string text)
                {
                    BotConnection.PlayYoutube(text);
                });
            }, "Send a Text to Speech message", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(AudioPage, 3, 0, "Stop \nAudio", BotConnection.StopAudioReplay, "Stop the currently played Audio", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(AudioPage, 3.75f, 0, "+", delegate
            {
                BotConnection.ChangeAudioVolume(true);
            }, "Increase the Volume", ButtonAPI.ButtonSize.Square);

            new QMSingleButton(AudioPage, 4.25f, 0, "-", delegate
            {
                BotConnection.ChangeAudioVolume(false);
            }, "Decrease the Volume", ButtonAPI.ButtonSize.Square);

            new QMSingleButton(AudioPage, 4, 0.5f, "Volume", BotConnection.ResetAudioVolume, "Reset the Volume", ButtonAPI.ButtonSize.Half);
        }
    }
}
