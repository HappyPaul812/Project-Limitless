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
            foreach (Entry entry in scenes)
                if (entry.sceneName == originScene) return entry.sprite;
            return null;
        }
    }
}
