using System.Linq;
using UnityEngine;
using VRC.UI.Controls;
using VRC.UI.Elements;

namespace LUXED.UIApi
{
    public static class MenuHelper
    {
        private static GameObject _menuPageTemplate;
        public static GameObject menuPageTemplate
        {
            get
            {
                if (_menuPageTemplate == null) _menuPageTemplate = QuickMenu.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_Dashboard").gameObject;
                return _menuPageTemplate;
            }
        }

        private static GameObject _iconButtonTemplate;
        public static GameObject iconButtonTemplate
        {
            get
            {
                if (_iconButtonTemplate == null) _iconButtonTemplate = QuickMenu.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_Dashboard/Header_H1/RightItemContainer/Button_QM_Expand").gameObject;
                return _iconButtonTemplate;
            }
        }

        private static GameObject _singleButtonTemplate;
        public static GameObject singleButtonTemplate
        {
            get
            {
                if (_singleButtonTemplate == null) _singleButtonTemplate = QuickMenu.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickLinks/Button_Worlds").gameObject;
                return _singleButtonTemplate;
            }
        }

        private static GameObject _mainBigSingleButtonTemplate;
        public static GameObject mainBigSingleButtonTemplate
        {
            get
            {
                if (_mainBigSingleButtonTemplate == null) _mainBigSingleButtonTemplate = MainMenu.transform.Find("Container/MMParent/HeaderOffset/Menu_WorldDetail/ScrollRect/Viewport/VerticalLayoutGroup/Actions/ViewOnWebsite").gameObject;
                return _mainBigSingleButtonTemplate;
            }
        }

        private static GameObject _mainMediumSingleButtonTemplate;
        public static GameObject mainMediumSingleButtonTemplate
        {
            get
            {
                if (_mainMediumSingleButtonTemplate == null) _mainMediumSingleButtonTemplate = MainMenu.transform.Find("Container/MMParent/HeaderOffset/Menu_UserDetail/ScrollRect/Viewport/VerticalLayoutGroup/Row3/CellGrid_MM_Content/AddANote").gameObject;
                return _mainMediumSingleButtonTemplate;
            }
        }

        private static GameObject _mainSmallSingleButtonTemplate;
        public static GameObject mainSmallSingleButtonTemplate
        {
            get
            {
                if (_mainSmallSingleButtonTemplate == null) _mainSmallSingleButtonTemplate = MainMenu.transform.Find("Container/MMParent/HeaderOffset/Menu_Avatars/Menu_MM_DynamicSidePanel/Panel_SectionList/ScrollRect_Navigation_Container/ScrollRect_Content/Panel_SelectedAvatar/ScrollRect/Viewport/VerticalLayoutGroup/Button_AvatarDetails").gameObject;
                return _mainSmallSingleButtonTemplate;
            }
        }

        public static MenuStateController _menuStateController;
        public static MenuStateController menuStateController
        {
            get
            {
                if (_menuStateController == null) _menuStateController = QuickMenu.GetComponent<MenuStateController>();
                return _menuStateController;
            }
        }
        public static MenuStateController _mmmenuStateController;
        public static MenuStateController mmmenuStateController
        {
            get
            {
                if (_mmmenuStateController == null) _mmmenuStateController = MainMenu.GetComponent<MenuStateController>();
                return _mmmenuStateController;
            }
        }
        private static VRC.UI.Elements.QuickMenu _QuickMenu;
        public static VRC.UI.Elements.QuickMenu QuickMenu
        {
            get
            {
                if (_QuickMenu == null) _QuickMenu = Resources.FindObjectsOfTypeAll<VRC.UI.Elements.QuickMenu>()?.FirstOrDefault(x => x.gameObject?.name == "Canvas_QuickMenu(Clone)");
                return _QuickMenu;
            }
        }

        private static MainMenu _MainMenu;
        public static MainMenu MainMenu
        {
            get
            {
                if (_MainMenu == null) _MainMenu = Resources.FindObjectsOfTypeAll<MainMenu>()?.FirstOrDefault(x => x.gameObject?.name == "Canvas_MainMenu(Clone)");
                return _MainMenu;
            }
        }
        private static GameObject _MainMenuPageTemplater;
        public static GameObject _MainMenuPageTemplate
        {
            get
            {
                if (_MainMenuPageTemplate == null) _MainMenuPageTemplater = MainMenu.transform.Find("Container/MMParent/HeaderOffset/Menu_Dashboard").gameObject;
                return _MainMenuPageTemplate;
            }
        }
        private static GameObject _MainMenuTabTemplater;
        public static GameObject _MainMenuTabTemplate
        {
            get
            {
                if (_MainMenuTabTemplate == null) _MainMenuTabTemplater = MainMenu.transform.Find("Container/PageButtons/HorizontalLayoutGroup/Launch_Pad_Button_Tab").gameObject;
                return _MainMenuTabTemplate;
            }
        }
        private static Transform _userInterface;
        public static Transform userInterface
        {
            get
            {
                if (_userInterface == null) _userInterface = QuickMenu?.transform?.parent;
                return _userInterface;
            }
        }

        private static Transform _selectedMenuTemplate;
        public static Transform selectedMenuTemplate
        {
            get
            {
                if (_selectedMenuTemplate == null) _selectedMenuTemplate = QuickMenu.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_SelectedUser_Local");
                return _selectedMenuTemplate;
            }
        }

        private static SelectedUserMenuQM _selectedUserMenu;
        public static SelectedUserMenuQM selectedUserMenu
        {
            get
            {
                if (_selectedUserMenu == null) _selectedUserMenu = selectedMenuTemplate.GetComponent<SelectedUserMenuQM>();
                return _selectedUserMenu;
            }
        }

        private static GameObject _sliderButtonTemplate;
        public static GameObject sliderButtonTemplate
        {
            get
            {
                if (_sliderButtonTemplate == null) _sliderButtonTemplate = QuickMenu.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_QM_GeneralSettings/Panel_QM_ScrollRect/Viewport/VerticalLayoutGroup/DisplayAndVisualAdjustments/QM_Settings_Panel/VerticalLayoutGroup/ScreenBrightness").gameObject;
                return _sliderButtonTemplate;
            }
        }
    }
}
