using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectLimitless.Core
{
    /// <summary>
    /// PathId 하나로 길 공식 문장과 전투 특성 아이콘을 함께 찾습니다. SaveData에는 바뀔 수 있는 Sprite 대신
    /// 안정적인 PathId만 저장하고, 모든 화면이 이 Resolver를 사용해 같은 표현 자료를 읽습니다.
    /// </summary>
    public static class PathPresentationResolver
    {
        private const string ResourcePath = "PathDefinitions";
        private static PlayerPathDefinition[] cached;

        public static IReadOnlyList<PlayerPathDefinition> All => Load();
        public static PlayerPathDefinition Find(string pathId) => string.IsNullOrWhiteSpace(pathId)
            ? null : Load().FirstOrDefault(item => item.Id == pathId);

        /// <summary>
        /// 전투 HUD의 상태 ID에 대응하는 Trait 아이콘을 찾습니다. Path 공식 문장은 고정 정체성 화면에 쓰고,
        /// 전투 중 생기고 사라지는 상태에는 이 기능 아이콘만 사용해 두 의미가 섞이지 않게 합니다.
        /// </summary>
        public static Sprite FindTraitIcon(string statusMarkerId) => string.IsNullOrWhiteSpace(statusMarkerId)
            ? null : Load().FirstOrDefault(item => item.TraitStatusMarkerIds.Contains(statusMarkerId))?.TraitIcon;

        private static PlayerPathDefinition[] Load()
        {
            if (cached == null || cached.Any(item => item == null))
                cached = Resources.LoadAll<PlayerPathDefinition>(ResourcePath)
                    .Where(item => item != null).ToArray();
            return cached;
        }
    }
}
