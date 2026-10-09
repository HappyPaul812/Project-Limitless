using System;
using UnityEngine;

namespace ProjectLimitless.Battle
{
    /// <summary>파일명과 LOCAL 지역 문서로 확인된 배경만 연결합니다. 미확정 배경은 Inventory에 보관합니다.</summary>
    [CreateAssetMenu(menuName = "Project Limitless/Battle/Background Catalog")]
    public sealed class BattleBackgroundCatalog : ScriptableObject
    {
        [Serializable]
        public struct Entry { public string sceneName; public Sprite sprite; }
        [SerializeField] private Entry[] scenes = Array.Empty<Entry>();
        [SerializeField] private Sprite silentWarden;

        public Sprite Find(string originScene, string encounterId)
        {
            // 같은 B2 안에서도 파수꾼은 일반 묘지 배경보다 Boss 전용 배경이 우선입니다.
            if (encounterId == ProjectLimitless.World.MainQuest11DungeonFlow.BossId) return silentWarden;
            // 별도 신규 배경 그림 없이 승인된 심부 지면을 Main20 전투의 동일 지역 배경으로 사용합니다.
            if (originScene == ProjectLimitless.World.Chapter2Main20Flow.Field)
                return Resources.Load<Sprite>("Main20/Environment/DeepCore_Ground_Base");
            foreach (Entry entry in scenes)
                if (entry.sceneName == originScene) return entry.sprite;
            return null;
        }
    }
}
