using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectLimitless.Core
{
    /// <summary>
    /// 길 이름·특성·공식 아이콘을 한 데이터에서 찾습니다. UI마다 PathId switch와 경로를 반복하면
    /// 아이콘 교체나 새 길 추가 때 화면마다 서로 다른 상징이 남을 수 있어 이 Resolver를 공용으로 사용합니다.
    /// </summary>
    public static class PathPresentationResolver
    {
        private const string ResourcePath = "PathDefinitions";
        private static PlayerPathDefinition[] cached;

        public static IReadOnlyList<PlayerPathDefinition> All => Load();
        public static PlayerPathDefinition Find(string pathId) => string.IsNullOrWhiteSpace(pathId)
            ? null : Load().FirstOrDefault(item => item.Id == pathId);

        private static PlayerPathDefinition[] Load()
        {
            if (cached == null || cached.Any(item => item == null))
                cached = Resources.LoadAll<PlayerPathDefinition>(ResourcePath)
                    .Where(item => item != null).ToArray();
            return cached;
        }
    }
}
