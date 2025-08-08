using HarmonyLib;
using LUXED.Wrappers;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LUXED.Hooking
{
    internal class UserContentCell_CreateCell
    {
        public void Initialize()
        {
            EasyHooking.LuxedInstance.Patch(typeof(MainMenuSelectedUser).GetMethods().LastOrDefault((MethodInfo x) => x.Name.Equals("OnEnable")), new HarmonyMethod(AccessTools.Method(typeof(UserContentCell_CreateCell), "Hook", null, null)), null, null, null, null);
        }
        private static void Hook(IntPtr instance, IntPtr __0)
        {
            MainMenuSelectedUser MMUserCell = instance != IntPtr.Zero ? new MainMenuSelectedUser(instance) : null;

            if (MMUserCell == null) return;

            IUser user = __0 != IntPtr.Zero ? new IUser(__0) : MMUserCell.field_Private_IUser_0;

            IUser activeUser = user ?? MMUserCell.field_Private_IUser_0;
            if (activeUser == null) return;

            Object1PublicOb1ApStLo1BoStSiBoUnique IUser = activeUser.TryCast<Object1PublicOb1ApStLo1BoStSiBoUnique>();
            if (IUser == null || IUser.prop_TYPE_0 == null) return;

            Color RankColor = IUser.prop_TYPE_0.GetRank().GetRankColor();
            string richColor = ColorUtility.ToHtmlStringRGB(RankColor);

            if (MMUserCell._defaultUserIconText == null || MMUserCell._defaultUserIconText == null) return;

            MMUserCell._defaultUserIconText.richText = true;
            MMUserCell._defaultUserIconText.richText = true;
            MMUserCell._defaultUserIconText.text = $"<color=#{richColor}>{MMUserCell._defaultUserIconText.text}</color>";
            MMUserCell._defaultUserIconText.text = $"<color=#{richColor}>{MMUserCell._defaultUserIconText.text}</color>";
            MMUserCell._defaultUserIconText.color = RankColor;
            MMUserCell._defaultUserIconText.color = RankColor;
        }
    }
}
