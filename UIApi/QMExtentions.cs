using LUXED.Wrappers;
using UnityEngine;

namespace LUXED.UIApi
{
    public class QMExInfoBar
    {
        public QMExInfoBar(string text, System.Action action)
        {
            Transform transform = MenuHelper.QuickMenu.transform.Find("CanvasGroup/Container/Window/Panel_QM_Widget/Header_StreamerMode");
            GameObject gameObject = GameObject.Instantiate<Transform>(transform, transform.parent).gameObject;
            gameObject.name = $"Hexed_QMExInfBar_{EncryptUtils.RandomString(10)}-{EncryptUtils.RandomStringNumber(10)}";
            gameObject.transform.Find("Header/LeftItemContainer/Text_Title").GetComponent<TextMeshProUGUIEx>().text = text;
        }
    }
}
