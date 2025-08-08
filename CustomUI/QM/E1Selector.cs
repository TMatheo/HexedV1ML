using LUXED.Modules.EventManagement;
using LUXED.Modules.Standalone;
using LUXED.UIApi;
using LUXED.Wrappers;

namespace LUXED.CustomUI.QM
{
    internal class E1Selector
    {
        public static void Meke()
        {
            QMScrollButton E1Scroll = new QMScrollButton(ExploitMenu.ExploitPage, 3, 1, "E1 Selector", "E1 Options", ButtonAPI.ButtonSize.Half);
            E1Scroll.SetAction(delegate
            {
                foreach (var kvp in EventSanitizer.KnownBadVoicePayloads)
                {
                    string name = kvp.Key;
                    string payload = kvp.Value;
                    E1Scroll.Add(kvp.Key, $"Select: {kvp.Key} payload.", delegate
                    {
                        ExploitHandler.E1 = kvp.Value;
                        PopupAPI.HudToast("M E L E X E D",$"Selected: {kvp.Key} payload.", UnityUtils.GetSprite("moon"));
                    });
                }
            });
        }
    }
}
