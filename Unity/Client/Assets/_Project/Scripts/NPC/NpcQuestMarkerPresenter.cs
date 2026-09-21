using ProjectLimitless.Core;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.NPC
{
    /// <summary>stable NPC ID에 해당하는 퀘스트 상태를 머리 위에 표시합니다.</summary>
    [RequireComponent(typeof(NpcController), typeof(VillageNpcRole))]
    public sealed class NpcQuestMarkerPresenter : MonoBehaviour
    {
        private VillageNpcRole roleData;
        private GameObject markerRoot;
        private Text markerText;

        public static NpcQuestMarkerPresenter GetOrAdd(NpcController npc, VillageNpcRole role)
        {
            NpcQuestMarkerPresenter presenter = npc.GetComponent<NpcQuestMarkerPresenter>()
                ?? npc.gameObject.AddComponent<NpcQuestMarkerPresenter>();
            presenter.roleData = role;
            presenter.EnsureView();
            presenter.Refresh();
            return presenter;
        }

        private void OnEnable()
        {
            QuestService.Changed += Refresh;
            EnsureView();
            Refresh();
        }

        private void OnDisable() => QuestService.Changed -= Refresh;

        private void Refresh()
        {
            roleData ??= GetComponent<VillageNpcRole>();
            EnsureView();
            NpcQuestMarkerState state = QuestService.GetNpcMarkerState(roleData.NpcId);
            markerRoot.SetActive(state != NpcQuestMarkerState.None);
            if (state == NpcQuestMarkerState.Available)
            { markerText.text = "✦ 새 이야기"; markerText.color = new Color(1f, 0.82f, 0.28f); }
            else if (state == NpcQuestMarkerState.ActiveObjective)
            { markerText.text = "◆ 현재 목표"; markerText.color = new Color(0.4f, 0.85f, 1f); }
            else if (state == NpcQuestMarkerState.ReadyToTurnIn)
            { markerText.text = "✓ 완료 보고"; markerText.color = new Color(0.5f, 1f, 0.58f); }
        }

        private void EnsureView()
        {
            if (markerRoot != null) return;
            markerRoot = new GameObject("QuestMarker", typeof(Canvas), typeof(CanvasScaler));
            markerRoot.transform.SetParent(transform, false);
            // 이름표(1.02)와 상호작용 안내(1.48) 위에 충분한 간격을 둡니다.
            markerRoot.transform.localPosition = new Vector3(0f, 1.92f, 0f);
            markerRoot.transform.localScale = Vector3.one * 0.01f;
            Canvas canvas = markerRoot.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.sortingOrder = 21;
            markerRoot.GetComponent<RectTransform>().sizeDelta = new Vector2(250f, 50f);

            GameObject backgroundObject = new GameObject("Background", typeof(Image));
            backgroundObject.transform.SetParent(markerRoot.transform, false);
            Image background = backgroundObject.GetComponent<Image>();
            background.color = new Color(0.025f, 0.04f, 0.07f, 0.88f);
            Stretch(background.rectTransform, Vector2.zero, Vector2.zero);

            GameObject textObject = new GameObject("MarkerText", typeof(Text));
            textObject.transform.SetParent(backgroundObject.transform, false);
            markerText = textObject.GetComponent<Text>();
            markerText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            markerText.fontSize = 27;
            markerText.fontStyle = FontStyle.Bold;
            markerText.alignment = TextAnchor.MiddleCenter;
            Stretch(markerText.rectTransform, new Vector2(8f, 3f), new Vector2(-8f, -3f));
        }

        private static void Stretch(RectTransform rect, Vector2 min, Vector2 max)
        { rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one; rect.offsetMin = min; rect.offsetMax = max; }
    }
}
