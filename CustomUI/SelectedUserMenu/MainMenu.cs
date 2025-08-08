using LUXED.UIApi;
using LUXED.Wrappers;
using UnityEngine;

namespace LUXED.CustomUI.SelectedUserMenu
{
    internal class MainMenu
    {
        public static QMMenuPage ClientPage;
        public static UITextMeshText InfoText;
        public static void Init()
        {
            ClientPage = new QMMenuPage("Melexed");

            new QMIconButton(MenuHelper.selectedMenuTemplate, 1.25f, -0.8f, ClientPage.OpenMe, "Melexed Client", UnityUtils.GetSprite("moon"));

            UtilsMenu.Init();
            AvatarMenu.Init();
            ExploitMenu.Init();
            BotMenu.Init();

            InfoText = new UITextMeshText(ClientPage.MenuObject, "Informations", new Vector2(-440, 155), 29, false);
            InfoText.text.color = Color.gray;
        }
    }
}
