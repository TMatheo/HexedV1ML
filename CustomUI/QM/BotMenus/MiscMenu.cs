using LUXED.Modules.Standalone;
using LUXED.UIApi;
using LUXED.Wrappers;
using System;

namespace LUXED.CustomUI.QM.BotMenus
{
    internal class MiscMenu
    {
        private static QMMenuPage MiscPage;
        public static void Init()
        {
            MiscPage = new QMMenuPage("Bot Misc");

            QMSingleButton OpenMenu = new QMSingleButton(BotMenu.BotsPage, 4, 1.5f, "Misc", MiscPage.OpenMe, "Bot Misc Options", ButtonAPI.ButtonSize.Default, UnityUtils.GetSprite("Debug"));

            new QMSingleButton(MiscPage, 1, 0, "Send \nChat", delegate
            {
                GameHelper.VRCUiPopupManager.AskInGameInput("Text Message", "Ok", delegate (string text)
                {
                    BotConnection.ChatMessage(text);
                });
            }, "Send a Chatbox Message", ButtonAPI.ButtonSize.Half);

            new QMSingleButton(MiscPage, 1, 0.5f, "Avatar \nSize", delegate
            {
                GameHelper.VRCUiPopupManager.AskInGameInput("100 - 100000", "Ok", delegate (string text)
                {
                    int Height = Convert.ToInt32(text);

                    BotConnection.AvatarHeight(Height);
                });
            }, "Change the Avatar Size", ButtonAPI.ButtonSize.Half);

            new QMToggleButton(MiscPage, 2, 0, "Chat \nCommands", delegate
            {
                BotConnection.ChatCommands(true);
            }, delegate
            {
                BotConnection.ChatCommands(false);
            }, "Handle the Bot with Chatbox Messages");
        }
    }
}
