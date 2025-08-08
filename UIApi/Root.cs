using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using VRC.Localization;

namespace LUXED.UIApi
{
    public class Root
    {
        public string _text;
        public string Text
        {
            get => _text; set
            {
                _text = value;
                if (TMProCompnt != null)
                    TMProCompnt.text = _text;
            }
        }

        public GameObject gameObject { get; internal set; }
        public Transform transform { get; internal set; }
        public TextMeshProUGUI TMProCompnt { get; internal set; }

        public virtual void SetActive(bool Active) => gameObject.SetActive(Active);
        public void SetTextColor(Color color) => TMProCompnt.color = color;
        public void SetTextColor(string Hex) => TMProCompnt.text = $"<color={Hex}>{Text}</color>";
        public void SetRotation(Vector3 Poz) => gameObject.transform.localRotation = Quaternion.Euler(Poz);
        public void SetPostion(Vector3 Poz) => gameObject.transform.localPosition = Poz;
        public GameObject GetGameObject() => gameObject;
        public Transform GetTransform() => gameObject.transform;
        public Transform ChangeParent(GameObject newParent) => gameObject.transform.parent = newParent.transform;

        public virtual string SetToolTip(string tip)
        {
            bool Fi = false;
            foreach (var s in gameObject.GetComponentsInChildren<ToolTip>())
            {
                if (!Fi)
                {
                    LocalizableString text2 = LocalizableStringExtensions.Localize(tip, null, null, null);
                    Fi = true;
                    s._localizableString = text2;
                    s.enabled = !string.IsNullOrEmpty(tip);
                }
                else s.enabled = false;
            }
            return tip;
        }
    }
}
