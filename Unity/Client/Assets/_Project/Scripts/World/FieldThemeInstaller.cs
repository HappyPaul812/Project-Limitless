using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
    /// <summary>Scene 이름에 맞는 테마 데이터가 있으면 기존 환경을 훼손하지 않고 색과 장식을 더합니다.</summary>
    public static class FieldThemeInstaller
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SceneManager.sceneLoaded -= Install;
            SceneManager.sceneLoaded += Install;
        }

        private static void Install(Scene scene, LoadSceneMode mode)
        {
            FieldThemeDefinition theme = Resources.LoadAll<FieldThemeDefinition>("FieldThemes")
                .FirstOrDefault(item => item.SceneName == scene.name);
            if (theme == null) return;
            foreach (SpriteRenderer renderer in Object.FindObjectsByType<SpriteRenderer>())
                if (renderer.gameObject.scene == scene && (renderer.name.StartsWith("Grass_") || renderer.name.StartsWith("Road")))
                    renderer.color *= theme.GroundTint;
            Camera camera = Camera.main;
            if (camera != null && camera.gameObject.scene == scene) camera.backgroundColor = theme.CameraColor;
            CloneDecorations(scene, theme.TreeTemplateName, theme.ExtraTreePositions, "ForestTree");
            CloneDecorations(scene, theme.BushTemplateName, theme.ExtraBushPositions, "ForestBush");
        }

        private static void CloneDecorations(Scene scene, string templateName, Vector2[] positions, string prefix)
        {
            if (string.IsNullOrWhiteSpace(templateName)) return;
            GameObject template = scene.GetRootGameObjects().SelectMany(root => root.GetComponentsInChildren<Transform>(true))
                .FirstOrDefault(item => item.name == templateName)?.gameObject;
            if (template == null) return;
            for (int index = 0; index < positions.Length; index++)
            {
                GameObject clone = Object.Instantiate(template, positions[index], template.transform.rotation);
                clone.name = $"{prefix}_{index + 1:00}";
                SceneManager.MoveGameObjectToScene(clone, scene);
            }
        }
    }
}
