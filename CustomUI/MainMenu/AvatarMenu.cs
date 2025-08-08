using LUXED.UIApi;
using LUXED.Wrappers;
using UnityEngine;
using VRC.Core;

namespace LUXED.CustomUI.MainMenu
{
    internal class AvatarMenu
    {
        private static Transform AvatarPage;
        public static void Init()
        {
            AvatarPage = MenuHelper.MainMenu.transform.Find("Container/MMParent/HeaderOffset/Menu_Avatars/Menu_MM_DynamicSidePanel/Panel_SectionList/ScrollRect_Navigation_Container/ScrollRect_Content/Panel_SelectedAvatar/ScrollRect/Viewport/VerticalLayoutGroup");

            new MMSingleButton(AvatarPage, 0, 0, "Copy ID", delegate
            {
                ApiAvatar selectedAvatar = MenuHelper.MainMenu.transform.Find("Container/MMParent/HeaderOffset/Menu_Avatars/Menu_MM_DynamicSidePanel/Panel_SectionList/ScrollRect_Navigation_Container/ScrollRect_Content/Panel_SelectedAvatar/Panel_MM_AvatarViewer/Avatar").GetComponent<VRC.SimpleAvatarPedestal>().field_Internal_ApiAvatar_0;
                if (selectedAvatar != null) GeneralUtils.CopyToClipboard($"{selectedAvatar.id}");
            }, "Copy the AvatarID", MMButtonType.Small, UnityUtils.GetSprite("History"));
        }
    }
}
