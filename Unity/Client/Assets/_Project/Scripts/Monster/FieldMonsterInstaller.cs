using System;
using System.Collections.Generic;
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
        private const string SafetyZoneResourcePath = "FieldEntranceSafetyZones";
        private const string RootName = "FieldMonsters";

        // 게임 시작 전에 Scene 로드 Event를 중복 없이 등록합니다.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SceneManager.sceneLoaded -= Install;
            SceneManager.sceneLoaded += Install;
        }

        /// <summary>로드된 Scene 이름과 일치하는 모든 배치 데이터를 찾아 독립 리스폰 관리기에 전달합니다.</summary>
        private static void Install(Scene scene, LoadSceneMode mode)
        {
            if (scene.GetRootGameObjects().Any(item => item.name == RootName)) return;

            FieldMonsterSpawnDefinition[] spawns = Resources.LoadAll<FieldMonsterSpawnDefinition>(SpawnResourcePath)
                .Where(item => item.SceneName == scene.name)
                .OrderBy(item => item.SpawnId)
                .ToArray();
            if (spawns.Length == 0) return;
            FieldEntranceSafetyZoneDefinition[] safetyZones = Resources.LoadAll<FieldEntranceSafetyZoneDefinition>(SafetyZoneResourcePath)
                .Where(item => item.SceneName == scene.name)
                .OrderBy(item => item.ZoneId)
                .ToArray();

            string[] invalidSpawnIds = spawns
                .Where(item => string.IsNullOrWhiteSpace(item.SpawnId))
                .Select(item => item.name)
                .Concat(spawns.GroupBy(item => item.SpawnId).Where(group => group.Count() > 1).Select(group => group.Key))
                .ToArray();
            if (invalidSpawnIds.Length > 0)
            {
                Debug.LogError($"{scene.name} 몬스터 배치 ID가 비었거나 중복됐습니다: {string.Join(", ", invalidSpawnIds)}");
                return;
            }

            Debug.Log($"필드 몬스터 배치 로드: {scene.name}, {spawns.Length}개 [{string.Join(", ", spawns.Select(item => item.SpawnId))}]");
            GameObject root = new GameObject(RootName, typeof(FieldMonsterSpawnRuntime));
            SceneManager.MoveGameObjectToScene(root, scene);
            root.GetComponent<FieldMonsterSpawnRuntime>().Configure(spawns, safetyZones);
        }

        /// <summary>배치 데이터 한 개를 Rigidbody2D, Collider, Visual과 배회 Controller가 있는 GameObject로 만듭니다.</summary>
        internal static GameObject CreateMonster(Transform parent, FieldMonsterSpawnDefinition spawn, Vector2 resolvedPosition)
        {
            MonsterDefinition monster = spawn.Monster;
            GameObject monsterObject = new GameObject(
                $"Monster_{spawn.SpawnId}_{monster.MonsterId}",
                typeof(Rigidbody2D),
                typeof(CircleCollider2D),
                typeof(MonsterFieldController));
            monsterObject.transform.SetParent(parent, false);
            monsterObject.transform.position = resolvedPosition;
            monsterObject.GetComponent<CircleCollider2D>().radius = .4f;
            monsterObject.GetComponent<MonsterFieldController>().Configure(spawn, resolvedPosition);
            monsterObject.AddComponent<MonsterNameplate>().Configure(monster.DisplayName);

            GameObject visual = new GameObject("Visual");
            // PlaceholderVisual의 Awake보다 먼저 색·크기·이름표를 설정할 수 있도록 준비 중에는 비활성화합니다.
            visual.SetActive(false);
            visual.transform.SetParent(monsterObject.transform, false);
            SpriteRenderer renderer = visual.AddComponent<SpriteRenderer>();
            Animator animator = null;
            renderer.sortingOrder = 3;
            if (monster.UsesSpriteSheetAnimation)
            {
                // Field의 개체는 스폰 데이터가 만들지만 외형은 MonsterDefinition에서 읽습니다.
                // 같은 독침벌 정의를 세 스폰과 Battle이 공유해도 각 개체의 위치·리스폰 상태는 서로 독립입니다.
                visual.transform.localScale = Vector3.one * monster.VisualScale;
                MonsterSpriteSheetAnimation frameAnimation = visual.AddComponent<MonsterSpriteSheetAnimation>();
                frameAnimation.Configure(renderer, monster);
                monsterObject.GetComponent<MonsterFieldController>().ConfigureFrameAnimation(frameAnimation);
            }
            else if (monster.FieldSprite != null)
            {
                renderer.sprite = monster.FieldSprite;
                if (monster.FieldAnimatorController != null)
                {
                    animator = visual.AddComponent<Animator>();
                    animator.runtimeAnimatorController = monster.FieldAnimatorController;
                }
            }
            else
            {
                // 최종 그림이 없는 몬스터만 프로젝트의 공통 Placeholder로 표시합니다.
                visual.AddComponent<PlaceholderVisual>().Configure(
                    monster.PlaceholderColor,
                    monster.VisualSize,
                    monster.DisplayName,
                    3);
            }

            visual.SetActive(true);
            if (animator != null)
                monsterObject.GetComponent<MonsterFieldController>().ConfigureAnimator(animator);
            return monsterObject;
        }
    }

    /// <summary>
    /// 현재 Field Scene에 속한 각 스폰을 독립적으로 확인하고, 처치 시간이 끝난 스폰만 다시 생성합니다.
    /// </summary>
    internal sealed class FieldMonsterSpawnRuntime : MonoBehaviour
    {
        private readonly Dictionary<FieldMonsterSpawnDefinition, GameObject> instances = new Dictionary<FieldMonsterSpawnDefinition, GameObject>();
        private FieldMonsterSpawnDefinition[] spawns = Array.Empty<FieldMonsterSpawnDefinition>();
        private FieldEntranceSafetyZoneDefinition[] safetyZones = Array.Empty<FieldEntranceSafetyZoneDefinition>();

        public void Configure(FieldMonsterSpawnDefinition[] definitions, FieldEntranceSafetyZoneDefinition[] zones)
        {
            spawns = definitions ?? Array.Empty<FieldMonsterSpawnDefinition>();
            safetyZones = zones ?? Array.Empty<FieldEntranceSafetyZoneDefinition>();
            RefreshSpawns();
        }

        private void Update() => RefreshSpawns();

        private void RefreshSpawns()
        {
            foreach (FieldMonsterSpawnDefinition spawn in spawns)
            {
                if (spawn == null || instances.TryGetValue(spawn, out GameObject instance) && instance != null) continue;
                if (spawn.Monster == null)
                {
                    Debug.LogError($"몬스터 배치 '{spawn.SpawnId}'에 MonsterDefinition이 없습니다.");
                    instances[spawn] = gameObject;
                    continue;
                }
                if (!MonsterEncounterService.IsSpawnAvailable(spawn)) continue;

                Vector2 safePosition = ResolveSafeActivityCenter(spawn);
                instances[spawn] = FieldMonsterInstaller.CreateMonster(transform, spawn, safePosition);
            }
        }

        private Vector2 ResolveSafeActivityCenter(FieldMonsterSpawnDefinition spawn)
        {
            Vector2 center = spawn.Position;
            foreach (FieldEntranceSafetyZoneDefinition zone in safetyZones)
            {
                // 몬스터 한 점만 밖으로 밀면 배회 중 다시 출입구에 들어옵니다. 따라서 안전 반경과
                // 활동 반경을 더한 거리만큼 중심을 밀어 배회 원 전체가 안전지대를 침범하지 않게 합니다.
                float requiredDistance = zone.Radius + Mathf.Max(.5f, spawn.ActivityRadius);
                Vector2 offset = center - zone.Center;
                if (offset.sqrMagnitude >= requiredDistance * requiredDistance) continue;
                if (offset.sqrMagnitude < .0001f)
                    offset = StableDirection(spawn.SpawnId);
                center = zone.Center + offset.normalized * requiredDistance;
            }
            return center;
        }

        private static Vector2 StableDirection(string spawnId)
        {
            int value = 0;
            foreach (char character in spawnId ?? string.Empty) value = (value * 31 + character) & 0x7fffffff;
            float angle = (value % 360) * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
    }
}
