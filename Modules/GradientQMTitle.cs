using LUXED.Interfaces;
using LUXED.UIApi;
using MelonLoader;
using System.Collections;
using TMPro;
using UnityEngine;

namespace LUXED.Modules
{
    internal class GradientQMTitle : IGlobalModule
    {
        private TextMeshProUGUI textMeshProUGUI;
        private float hueOffset = 0f;

        public void Initialize()
        {
            MelonCoroutines.Start(WaitForTitleText());
        }

        private IEnumerator WaitForTitleText()
        {
            GameObject textObject = null;

            while (textObject == null)
            {
                textObject = GameObject.Find("Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/QMParent/Menu_Dashboard/Header_H1/LeftItemContainer/Text_Title");
                yield return null;
            }

            textMeshProUGUI = textObject.GetComponent<TextMeshProUGUI>();
        }

        public void OnUpdate()
        {
            if (textMeshProUGUI == null) return;
            if (MenuHelper.menuStateController.isActiveAndEnabled)
            {
                AnimateGradient();
            }
        }

        private void AnimateGradient()
        {
            textMeshProUGUI.ForceMeshUpdate();
            var textInfo = textMeshProUGUI.textInfo;

            int charCount = textInfo.characterCount;
            if (charCount == 0) return;

            float time = Time.time;

            for (int i = 0; i < charCount; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                int vertexIndex = charInfo.vertexIndex;
                int materialIndex = charInfo.materialReferenceIndex;

                var vertices = textInfo.meshInfo[materialIndex].vertices;
                var colors = textInfo.meshInfo[materialIndex].colors32;

                float t = (float)i / (charCount - 1);
                float hue = Mathf.Repeat(t + hueOffset, 1f);
                Color color = Color.HSVToRGB(hue, 1f, 1f);
                Color32 c = color;

                colors[vertexIndex + 0] = c;
                colors[vertexIndex + 1] = c;
                colors[vertexIndex + 2] = c;
                colors[vertexIndex + 3] = c;

                float bounce = Mathf.Sin(time * 4f + i * 0.3f) * 2f; // 4Hz speed, 2px amplitude
                Vector3 offset = new Vector3(0, bounce, 0);

                vertices[vertexIndex + 0] += offset;
                vertices[vertexIndex + 1] += offset;
                vertices[vertexIndex + 2] += offset;
                vertices[vertexIndex + 3] += offset;
            }

            // Apply changes
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                textMeshProUGUI.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            textMeshProUGUI.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            hueOffset += Time.deltaTime * 0.1f;
            if (hueOffset > 1f) hueOffset -= 1f;
        }
    }
}
