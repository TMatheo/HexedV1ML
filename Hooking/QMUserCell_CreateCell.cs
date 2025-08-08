using HarmonyLib;
using LUXED.Interfaces;
using LUXED.Wrappers;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LUXED.Hooking
{
    internal class QMUserCell_CreateCell : IHook
    {
        public void Initialize()
        {
            EasyHooking.LuxedInstance.Patch(typeof(MonoBehaviourPublicObTe_cTeObObObObUnique).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).LastOrDefault(x => x.Name.Equals("Method_Private_Void_IUser_0")), new HarmonyMethod(typeof(QMUserCell_CreateCell).GetMethod(nameof(Prefix), BindingFlags.Static | BindingFlags.NonPublic)));
        }

        private static bool Prefix(object __instance, object param_1)
        {
            var QMUserCell = __instance as MonoBehaviourPublicObTe_cTeObObObObUnique;
            var user = param_1 as IUser;
            if (QMUserCell == null)
                return true;
            IUser activeUser = user ?? QMUserCell.field_Private_IUser_0;
            if (activeUser == null)
                return true;
            var convertedUser = activeUser.TryCast<Object1PublicOb1ApStLo1BoStSiBoUnique>();
            if (convertedUser == null || convertedUser.prop_TYPE_0 == null)
                return true;
            Color rankColor = convertedUser.prop_TYPE_0.IsFriend() ? VRCPlayer.field_Internal_Static_Color_1 : convertedUser.prop_TYPE_0.GetRank().GetRankColor();
            string richColor = ColorUtility.ToHtmlStringRGB(rankColor);
            var textComponent = QMUserCell.field_Public_TextMeshProUGUI_0;
            if (textComponent != null)
            {
                textComponent.richText = true;
                textComponent.text = $"<color=#{richColor}>{textComponent.text}</color>";
                textComponent.color = rankColor;
            }
            return true;
        }
    }
}
