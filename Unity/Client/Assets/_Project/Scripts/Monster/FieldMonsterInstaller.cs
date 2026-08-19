using System;
using System.Linq;
using ProjectLimitless.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.Monster
{
    /// <summary>
    /// World/Field Scene이 열릴 때 Resources의 배치 데이터를 읽어 해당 Scene의 몬스터를 생성합니다.
    /// Scene Asset을 직접 덮어쓰지 않으며, 새 몬스터는 데이터 Asset을 추가하는 방식으로 확장할 수 있습니다.
    /// </summary>
    public static class FieldMonsterInstaller
    {
        private const string SpawnResourcePath = "MonsterSpawns";
        private const string RootName = "FieldMonsters";

        // 게임 시작 전에 Scene 로드 Event를 중복 없이 등록합니다.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SceneManager.sceneLoaded -= Install;
            SceneManager.sceneLoaded += Install;
        }

        /// <summary>로드된 Scene 이름과 일치하는 모든 배치 데이터를 찾아 몬스터를 생성합니다.</summary>
        private static void Install(Scene scene, LoadSceneMode mode)
        {
            if (scene.GetRootGameObjects().Any(item => item.name == RootName)) return;

            FieldMonsterSpawnDefinition[] spawns = Resources.LoadAll<FieldMonsterSpawnDefinition>(SpawnResourcePath)
                .Where(item => item.SceneName == scene.name)
                .OrderBy(item => item.SpawnId)
                .ToArray();
            if (spawns.Length == 0) return;

            GameObject root = new GameObject(RootName);
            SceneManager.MoveGameObjectToScene(root, scene);
            foreach (FieldMonsterSpawnDefinition spawn in spawns)
            {
                if (spawn.Monster == null)
                {
                    Debug.LogError($"몬스터 배치 '{spawn.SpawnId}'에 MonsterDefinition이 없습니다.");
                    continue;
                }

                CreateMonster(root.transform, spawn);
            }
        }

        /// <summary>배치 데이터 한 개를 Rigidbody2D, Collider, Visual과 배회 Controller가 있는 GameObject로 만듭니다.</summary>
        private static void CreateMonster(Transform parent, FieldMonsterSpawnDefinition spawn)
        {
            MonsterDefinition monster = spawn.Monster;
            GameObject monsterObject = new GameObject(
                $"Monster_{spawn.SpawnId}_{monster.MonsterId}",
                typeof(Rigidbody2D),
                typeof(CircleCollider2D),
                typeof(MonsterFieldController));
            monsterObject.transform.SetParent(parent, false);
            monsterObject.transform.position = spawn.Position;
            monsterObject.GetComponent<CircleCollider2D>().radius = .4f;
            monsterObject.GetComponent<MonsterFieldController>().Configure(monster, spawn.Position, spawn.ActivityRadius);

            GameObject visual = new GameObject("Visual");
            // PlaceholderVisual의 Awake보다 먼저 색·크기·이름표를 설정할 수 있도록 준비 중에는 비활성화합니다.
            visual.SetActive(false);
            visual.transform.SetParent(monsterObject.transform, false);
            SpriteRenderer renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sortingOrder = 3;
            if (monster.FieldSprite != null)
            {
                renderer.sprite = monster.FieldSprite;
            }
            else
            {
                // 적절한 Slime Sprite가 아직 없으므로 최종 그림을 임의 제작하지 않고
                // 프로젝트의 공통 Placeholder와 이름표로 개발 중인 대상임을 분명히 표시합니다.
                visual.AddComponent<PlaceholderVisual>().Configure(
                    monster.PlaceholderColor,
                    monster.VisualSize,
                    monster.DisplayName,
                    3);
            }

            visual.SetActive(true);
        }
    }
}
