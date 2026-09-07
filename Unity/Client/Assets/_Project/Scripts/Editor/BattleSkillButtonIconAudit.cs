using System;
using System.Reflection;
using System.Collections.Generic;
using ProjectLimitless.Battle;
using ProjectLimitless.Core;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.EditorTools
{
    /// <summary>실제 스킬 버튼 생성 경로의 15개 아이콘과 캐시 수명 회귀를 독립적으로 검증합니다.</summary>
    [InitializeOnLoad]
    public static class BattleSkillButtonIconAudit
    {
        private const string BatchKey = "ProjectLimitless.SkillButtonAudit.PlayRound";

        static BattleSkillButtonIconAudit()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        // 비활성 컨트롤러로 전투 Awake를 실행하지 않고 실제 버튼 생성 메서드만 호출합니다.
        public static void RunBatch()
        {
            try
            {
                Audit();
                // 저장된 Scene과 세이브를 건드리지 않는 빈 Scene에서 Play/Stop을 두 번 검증합니다.
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                SessionState.SetInt(BatchKey, 1);
                EditorApplication.EnterPlaymode();
            }
            catch (Exception error)
            {
                Debug.LogException(error);
                EditorApplication.Exit(1);
            }
        }

        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            int round = SessionState.GetInt(BatchKey, 0);
            if (round == 0) return;
            try
            {
                if (state == PlayModeStateChange.EnteredPlayMode)
                {
                    Audit();
                    Debug.Log($"SKILL_BUTTON_AUDIT Play Mode {round} PASS");
                    EditorApplication.ExitPlaymode();
                }
                else if (state == PlayModeStateChange.EnteredEditMode)
                {
                    if (round < 2)
                    {
                        SessionState.SetInt(BatchKey, round + 1);
                        EditorApplication.delayCall += EditorApplication.EnterPlaymode;
                    }
                    else
                    {
                        SessionState.EraseInt(BatchKey);
                        Debug.Log("SKILL_BUTTON_AUDIT ALL PASS");
                        EditorApplication.Exit(0);
                    }
                }
            }
            catch (Exception error)
            {
                SessionState.EraseInt(BatchKey);
                Debug.LogException(error);
                EditorApplication.Exit(1);
            }
        }

        private static void Audit()
        {
            GameObject owner = new GameObject("SkillButtonAuditController");
            owner.SetActive(false);
            var controller = owner.AddComponent<BattleSceneController>();
            GameObject panel = new GameObject("SkillButtonAuditPanel", typeof(RectTransform));
            var makeButton = typeof(BattleSceneController).GetMethod("MakeSkillMenuButton",
                BindingFlags.NonPublic | BindingFlags.Instance);
            int count = 0;
            try
            {
                foreach (JobDefinition job in Resources.LoadAll<JobDefinition>("JobDefinitions"))
                foreach (BattleSkillDefinition skill in BattleSkillCatalog.GetSkills(job))
                {
                    Sprite previous = null;
                    for (int rebuild = 0; rebuild < 3; rebuild++)
                    {
                        Button button = (Button)makeButton.Invoke(controller, new object[] {
                            panel.transform, "Skill_" + skill.Id, skill.DisplayName, skill.IconId,
                            Vector2.one * .5f, (Action)(() => { }), skill });
                        Image icon = button.transform.Find("SkillIcon")?.GetComponent<Image>();
                        if (icon == null || icon.sprite == null || !icon.enabled ||
                            !icon.gameObject.activeSelf || icon.color.a <= 0f)
                            throw new Exception($"{skill.DisplayName}: rebuild {rebuild} 아이콘 누락/비활성/투명");
                        if (previous != null && icon.sprite != previous)
                            throw new Exception($"{skill.DisplayName}: 정상 캐시 재사용 실패");
                        previous = icon.sprite;
                        panel.SetActive(false);
                        panel.SetActive(true);
                        UnityEngine.Object.DestroyImmediate(button.gameObject);
                    }
                    Debug.Log($"SKILL_BUTTON_AUDIT {skill.DisplayName}: {previous.name}, rect={previous.rect}, 3 rebuilds PASS");
                    count++;
                }
                if (count != 15) throw new Exception($"15개 대신 {count}개 스킬 검증됨");

                // Unity가 런타임 Sprite를 파괴했지만 정적 캐시 참조는 남은 상황을 재현합니다.
                foreach (string id in new[] { BattleUiIconCatalog.MageFireballSkill,
                    BattleUiIconCatalog.MageThunderboltSkill, BattleUiIconCatalog.HealerCleanseSkill })
                {
                    Sprite stale = BattleUiIconCatalog.LoadSkillButtonIcon(id);
                    UnityEngine.Object.DestroyImmediate(stale);
                    Sprite restored = BattleUiIconCatalog.LoadSkillButtonIcon(id);
                    if (restored == null) throw new Exception(id + ": 파괴된 캐시 복구 실패");
                    var cache = (Dictionary<string, Sprite>)typeof(BattleUiIconCatalog)
                        .GetField("SkillButtonCache", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
                    UnityEngine.Object.DestroyImmediate(restored);
                    cache[id] = null;
                    restored = BattleUiIconCatalog.LoadSkillButtonIcon(id);
                    if (restored == null) throw new Exception(id + ": null 캐시 복구 실패");
                    Button recovered = (Button)makeButton.Invoke(controller, new object[] {
                        panel.transform, "Recovered", id, id, Vector2.one * .5f, (Action)(() => { }), null });
                    Image recoveredIcon = recovered.transform.Find("SkillIcon")?.GetComponent<Image>();
                    if (recoveredIcon == null || recoveredIcon.sprite != restored || !recoveredIcon.enabled ||
                        !recoveredIcon.gameObject.activeSelf || recoveredIcon.color.a <= 0f)
                        throw new Exception(id + ": 복구한 Sprite 버튼 할당 실패");
                    UnityEngine.Object.DestroyImmediate(recovered.gameObject);
                    Debug.Log("SKILL_BUTTON_AUDIT " + id + ": destroyed cache recovery PASS");
                }
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(panel);
                UnityEngine.Object.DestroyImmediate(owner);
            }
        }
    }
}
