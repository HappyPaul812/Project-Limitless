using System.Linq;
using UnityEngine;

namespace ProjectLimitless.Core
{
    public static class PathVisualCatalog
    {
        private const string ResourcePath = "PathVisualDefinitions";
        public static PlayerPathVisualDefinition Find(string pathId) => Resources.LoadAll<PlayerPathVisualDefinition>(ResourcePath).FirstOrDefault(item => item != null && item.PathId == pathId);
    }
}
