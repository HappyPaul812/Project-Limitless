using System.Collections;
using ProjectLimitless.Monster;
using ProjectLimitless.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.Battle
{
    /// <summary>필드 조우에서 Battle Scene으로 넘겨야 하는 최소 런타임 정보입니다.</summary>
    public static class BattleEncounterContext
    {
        public static MonsterDefinition Monster { get; private set; }
        public static FieldMonsterSpawnDefinition Spawn { get; private set; }
        public static Sprite PlayerBattleSprite { get; private set; }
        public static Sprite MonsterBattleSprite { get; private set; }
        public static Vector2 PlayerFieldPosition { get; private set; }
        public static Vector2 MonsterFieldPosition { get; private set; }
        private static bool pendingFieldReturn;

        public static void Set(MonsterDefinition monster, FieldMonsterSpawnDefinition spawn, RuntimeAnimatorController playerAnimatorController, Vector2 playerPosition, Vector2 monsterPosition)
        {
            Monster = monster;
            Spawn = spawn;
            PlayerBattleSprite = BattleVisualResolver.ResolveIdleSprite(playerAnimatorController, BattleVisualResolver.AllyIdleState, null);
            MonsterBattleSprite = BattleVisualResolver.ResolveIdleSprite(monster?.FieldAnimatorController, BattleVisualResolver.EnemyIdleState, monster?.FieldSprite);
            PlayerFieldPosition = playerPosition;
            MonsterFieldPosition = monsterPosition;
            pendingFieldReturn = false;
        }

        public static void PrepareFieldReturn() => pendingFieldReturn = true;

        public static bool ConsumeFieldReturn()
        {
            if (!pendingFieldReturn) return false;
            pendingFieldReturn = false;
            return true;
        }
    }

    /// <summary>
    /// 기존 MonsterEncounterService를 Battle Scene 진입과 Field_01 복귀에 연결합니다.
    /// Field_01 Scene Asset을 수정하지 않아 기존 사용자 배치와 에셋 변경을 보존합니다.
    /// </summary>
    public static class BattleSceneFlow
    {
        public const string BattleSceneName = "Battle";
        public const string FieldSceneName = "Field_01";
        private static bool transitioning;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            transitioning = false;
            MonsterEncounterService.EncounterStarted -= EnterBattle;
            MonsterEncounterService.EncounterStarted += EnterBattle;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void EnterBattle(MonsterDefinition monster, FieldMonsterSpawnDefinition spawn)
        {
            if (transitioning || monster == null || spawn == null) return;
            PlayerController player = Object.FindAnyObjectByType<PlayerController>();
            if (player == null)
            {
                Debug.LogError("Battle 진입: Field Player를 찾지 못했습니다.");
                return;
            }

            Animator animator = player.GetComponentInChildren<Animator>();
            MonsterFieldController fieldMonster = FindEncounteredMonster(spawn);
            Vector2 monsterPosition = fieldMonster == null ? (Vector2)player.transform.position : fieldMonster.transform.position;
            BattleEncounterContext.Set(monster, spawn, animator == null ? null : animator.runtimeAnimatorController, player.transform.position, monsterPosition);
            transitioning = true;
            SceneManager.LoadSceneAsync(BattleSceneName, LoadSceneMode.Single);
        }

        private static MonsterFieldController FindEncounteredMonster(FieldMonsterSpawnDefinition spawn)
        {
            foreach (MonsterFieldController controller in Object.FindObjectsByType<MonsterFieldController>())
            {
                if (controller.SpawnDefinition == spawn) return controller;
            }
            return null;
        }

        /// <summary>승리 또는 도망 뒤 필드 전투 상태를 새로 만들고 즉시 재조우를 막으며 복귀합니다.</summary>
        public static void ReturnToField(bool defeatedEncounteredMonster)
        {
            if (transitioning) return;
            if (defeatedEncounteredMonster) MonsterEncounterService.MarkDefeated(BattleEncounterContext.Spawn);
            transitioning = true;
            MonsterEncounterService.SuppressForSeconds(2f);
            BattleEncounterContext.PrepareFieldReturn();
            SceneManager.LoadSceneAsync(FieldSceneName, LoadSceneMode.Single);
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            transitioning = false;
            if (scene.name == BattleSceneName && Object.FindAnyObjectByType<BattleSceneController>() == null)
                new GameObject("BattleSystem", typeof(BattleSceneController));
            else if (scene.name == FieldSceneName && BattleEncounterContext.ConsumeFieldReturn())
                new GameObject("BattleReturnSafety", typeof(BattleReturnSafety));
        }
    }

    /// <summary>Field_01 복귀 뒤 Player를 접촉 지점에서 조금 떼어 즉시 같은 슬라임과 다시 충돌하지 않게 합니다.</summary>
    public sealed class BattleReturnSafety : MonoBehaviour
    {
        private IEnumerator Start()
        {
            for (int frame = 0; frame < 30; frame++)
            {
                PlayerController player = Object.FindAnyObjectByType<PlayerController>();
                if (player != null)
                {
                    Vector2 away = BattleEncounterContext.PlayerFieldPosition - BattleEncounterContext.MonsterFieldPosition;
                    if (away.sqrMagnitude < .01f) away = Vector2.down;
                    player.transform.position = BattleEncounterContext.PlayerFieldPosition + away.normalized * 1.2f;
                    Destroy(gameObject);
                    yield break;
                }
                yield return null;
            }
            Debug.LogWarning("Battle 복귀 안전 처리: Field Player를 찾지 못했습니다.");
            Destroy(gameObject);
        }
    }
}
