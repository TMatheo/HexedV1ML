using LUXED.Core;
using LUXED.Extensions;
using LUXED.Interfaces;
using LUXED.Wrappers;
using System.Collections.Generic;
using UnityEngine;
using VRC.Core;

namespace LUXED.Modules
{
    internal class PlayerESP : IPlayerModule, IDelayModule
    {
        private VRCPlayer targetPlayer;

        public void Initialize(VRCPlayer player)
        {
            targetPlayer = player;
        }

        public void OnUpdate()
        {
            if (targetPlayer == null) return;

            if (InternalSettings.PlayerESP) ToggleCapsuleHighlight(true);
            else ToggleCapsuleHighlight(false);
        }

        public void ToggleCapsuleHighlight(bool State)
        {
            string HighlightName = "PlayerCapsule-" + targetPlayer.UserID();

            if (State)
            {
                HighlightsFXStandalone highlightFx = GetOrAddHighlight(HighlightName);

                if (targetPlayer.prop_PlayerSelector_0 == null) return;

                MeshFilter filter = targetPlayer.prop_PlayerSelector_0.GetComponent<MeshFilter>();
                if (filter == null) return;

                APIUser ApiUser = targetPlayer.GetAPIUser();
                if (ApiUser == null) return;

                highlightFx.field_Protected_Color_0 = ApiUser.GetRankColor();

                HighlightHelper.ToggleHighlightFx(highlightFx, filter, State);
            }
            else
            {
                DestroyHighlightFx(HighlightName);
            }
        }

        private HighlightsFXStandalone GetOrAddHighlight(string name)
        {
            if (!presentHighlights.TryGetValue(name, out HighlightsFXStandalone highlightFx))
            {
                highlightFx.gameObject.AddComponent<HighlightsFXStandalone>();
                highlightFx.blurSize = highlightFx.blurSize / 2;
                presentHighlights[name] = highlightFx;
            }

            return highlightFx;
        }

        private HighlightsFXStandalone GetHighlight(string name)
        {
            if (presentHighlights.TryGetValue(name, out HighlightsFXStandalone highlightFx)) return highlightFx;

            return null;
        }

        private void DestroyHighlightFx(string name)
        {
            HighlightsFXStandalone highlights = GetHighlight(name);
            if (highlights != null)
            {
                highlights.field_Protected_HashSet_1_MeshFilter_0.Clear();
            }
        }
        private readonly Dictionary<string, HighlightsFXStandalone> presentHighlights = new Dictionary<string, HighlightsFXStandalone>();
    }
}
