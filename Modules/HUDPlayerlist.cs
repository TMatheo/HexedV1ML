/*using LUXED.Interfaces;
using LUXED.Wrappers;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LUXED.Modules
{
    internal class HUDPlayerlist : IGlobalModule, IDesktopOnly,IDelayModule
    {
        private GameObject canvasObj;
        private TextMeshProUGUI tmpText;
        public void Initialize()
        {
            CreateHUD();
        }
        public void OnUpdate()
        {
            try
            {
                if (tmpText != null)
                {
                    if (GameHelper.CurrentPlayer)
                    {
                        List<string> playerStrings = Playerlist.GetPlayerStrings();
                        tmpText.SetText($"In Room: {playerStrings.Count} / {RoomManager.prop_ApiWorldInstance_1.capacity}\n{string.Join("\n", playerStrings)}");
                    }
                    else
                    {
                        tmpText.SetText("");
                    }
                }
            }
            catch (Exception ex)
            {
                //HDLogger.LogError($"HDHUD OnUpdate error: {ex.Message}");
            }
        }

        private void CreateHUD()
        {
            canvasObj = new GameObject("HDPLHUD_Canvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.pixelPerfect = true;
            canvas.sortingOrder = 999;
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            canvasObj.AddComponent<GraphicRaycaster>();
            GameObject textObj = new GameObject("HDHUD_Text");
            textObj.transform.SetParent(canvasObj.transform, false);
            tmpText = textObj.AddComponent<TextMeshProUGUI>();
            tmpText.text = "";
            tmpText.fontSize = 12;
            tmpText.color = Color.gray;
            tmpText.richText = true;
            tmpText.fontStyle = FontStyles.Bold;
            tmpText.alignment = TextAlignmentOptions.TopLeft;
            tmpText.enableWordWrapping = false;
            tmpText.raycastTarget = false;

            RectTransform rect = tmpText.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
            rect.pivot = new Vector2(0, 1);
            rect.anchoredPosition = new Vector2(10, -10);
            rect.sizeDelta = new Vector2(600, 100);
            UnityEngine.Object.DontDestroyOnLoad(canvasObj);
        }
    }
}
*/