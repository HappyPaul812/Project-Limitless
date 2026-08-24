using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace ProjectLimitless.Battle
{
    /// <summary>
    /// Battle Scene의 사이드뷰 전장과 남색·금색 HUD를 구성합니다.
    /// 전투 규칙은 BattleCore에 그대로 두고 이 클래스는 Sprite, HUD, 대상 강조와 명령 입력만 담당합니다.
    /// </summary>
    public sealed class BattleSceneController : MonoBehaviour
    {
        private sealed class CombatantView
        {
            public Button HitArea;
            public RectTransform ActionRoot;
            public Image SpriteImage;
            public Sprite IdleSprite;
            public Image GroundMarker;
            public Text TargetArrow;
            public Text TurnMarker;
            public Text HudName;
            public Text HudHp;
            public Text HudStatus;
            public Image HudHpFill;
        }

        private readonly List<Selectable> commandButtons = new List<Selectable>();
        private readonly Dictionary<Combatant, CombatantView> combatantViews = new Dictionary<Combatant, CombatantView>();
        private readonly Dictionary<Combatant, JobDefinition> combatantJobs = new Dictionary<Combatant, JobDefinition>();
        private readonly BattleSkillCooldowns skillCooldowns = new BattleSkillCooldowns();
        private readonly BattleStatusEffectRuntime statusEffects = new BattleStatusEffectRuntime();
        private readonly List<Button> skillMenuButtons = new List<Button>();
        private readonly Color navy = new Color(.018f, .03f, .06f, 1f);
        private readonly Color panel = new Color(.055f, .08f, .13f, .97f);
        private readonly Color gold = new Color(.88f, .7f, .32f, 1f);
        private readonly Color focusGold = new Color(1f, .86f, .48f, 1f);
        private Formation allies;
        private Formation enemies;
        private TurnOrderQueue turnOrder;
        private Combatant currentActor;
        private Text timelineText;
        private Text messageText;
        private Button attackButton;
        private Button skillButton;
        private Button defendButton;
        private Button fleeButton;
        private Image skillMenuPanel;
        private bool choosingTarget;
        private bool choosingSkill;
        private bool battleEnded;
        private bool actionPlaying;
        private BattleActionPresenter actionPresenter;
        private BattleSkillExecutor skillExecutor;
        private Font battleFont;

        private IEnumerable<Combatant> AllCombatants => allies.Members.Concat(enemies.Members);

        private void Awake()
        {
            skillExecutor = new BattleSkillExecutor(skillCooldowns, statusEffects);
            CreateParticipants();
            CreateEventSystem();
            CreateInterface();
            turnOrder = new TurnOrderQueue();
            turnOrder.Build(AllCombatants);
            AdvanceTurn();
        }

        private void Update()
        {
            if (Keyboard.current == null || battleEnded || actionPlaying) return;
            if (!Keyboard.current.escapeKey.wasPressedThisFrame) return;
            if (choosingSkill)
            {
                CloseSkillMenu();
                return;
            }
            if (choosingTarget)
            {
                choosingTarget = false;
                SetCommandButtons(true);
                RefreshCombatantViews(null);
                messageText.text = $"{currentActor.DisplayName}의 행동을 선택하세요.";
                if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(attackButton.gameObject);
            }
        }

        /// <summary>캐릭터 생성 능력치와 조우 몬스터를 1차 검증용 전투 참가자로 변환합니다.</summary>
        private void CreateParticipants()
        {
            PlayerPathDefinition path = Resources.LoadAll<PlayerPathDefinition>("PathDefinitions").FirstOrDefault(item => item.Id == GameSessionData.SelectedPlayerPathId);
            JobDefinition job = Resources.LoadAll<JobDefinition>("JobDefinitions").FirstOrDefault(item => item.JobId == GameSessionData.SelectedJobId);
            int health = CharacterCreationStatsCalculator.GetFinalStat(path, job, CharacterStatType.Health);
            int strength = CharacterCreationStatsCalculator.GetFinalStat(path, job, CharacterStatType.Strength);
            int agility = CharacterCreationStatsCalculator.GetFinalStat(path, job, CharacterStatType.Agility);
            string playerName = string.IsNullOrWhiteSpace(GameSessionData.PlayerName) ? "플레이어" : GameSessionData.PlayerName;
            TargetRangeType playerRange = GetBasicAttackRange(GameSessionData.SelectedJobId);
            int playerAttack = 12 + Math.Max(0, strength - 10) * 2;
            // 치유사의 기본 공격은 같은 능력치 기준에서도 다른 직업보다 낮은 피해를 주는 1차 검증값을 사용합니다.
            if (GameSessionData.SelectedJobId == "healer") playerAttack = Math.Max(1, playerAttack - 4);

            allies = new Formation(BattleSide.Allies);
            enemies = new Formation(BattleSide.Enemies);
            // HP와 공격력 환산은 전투 흐름 검증용 임시값이며 최종 밸런스 데이터가 아닙니다.
            Combatant player = new Combatant("player", playerName, BattleSide.Allies, new FormationSlot(FormationRow.Front, 1), 80 + health * 4, playerAttack, agility, 0, playerRange, true);
            allies.Place(player);
            if (job != null) combatantJobs[player] = job;

            MonsterDefinition monster = BattleEncounterContext.Monster ?? Resources.LoadAll<MonsterDefinition>("MonsterDefinitions").FirstOrDefault();
            string monsterId = monster == null ? "grass_slime" : monster.MonsterId;
            string monsterName = monster == null ? "초원 슬라임" : monster.DisplayName;
            enemies.Place(new Combatant(monsterId, monsterName, BattleSide.Enemies, new FormationSlot(FormationRow.Front, 1), 55, 10, 11, 0, TargetRangeType.MeleePhysical, false));
        }

        /// <summary>선택한 기본 직업을 확정된 기본 공격 사거리로 변환합니다.</summary>
        private static TargetRangeType GetBasicAttackRange(string jobId)
        {
            switch (jobId)
            {
                case "sharpshooter": return TargetRangeType.RangedPhysical;
                case "mage":
                case "healer": return TargetRangeType.Magic;
                case "guardian":
                case "fighter":
                default: return TargetRangeType.MeleePhysical;
            }
        }

        private void CreateInterface()
        {
            CreateCameraIfMissing();
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            battleFont = font;
            GameObject canvasObject = new GameObject("BattleCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas canvas = canvasObject.GetComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; scaler.referenceResolution = new Vector2(1280, 720); scaler.matchWidthOrHeight = .5f;
            Image background = MakeImage(canvasObject.transform, "Background", navy); Stretch(background.rectTransform);

            Text title = MakeText(canvasObject.transform, "Title", "전투", font, 31, new Vector2(.5f, .965f), new Vector2(260, 42)); title.color = gold; title.fontStyle = FontStyle.Bold;
            CreateTimeline(canvasObject.transform, font);
            Image battlefield = CreateBattlefield(canvasObject.transform);
            CreateFormationViews(battlefield.transform, canvasObject.transform, font, enemies);
            CreateFormationViews(battlefield.transform, canvasObject.transform, font, allies);
            CreateCommandPanel(canvasObject.transform, font);
        }

        private void CreateTimeline(Transform parent, Font font)
        {
            Image timelinePanel = MakeImage(parent, "TurnTimelinePanel", new Color(.035f, .055f, .09f, .98f)); SetRect(timelinePanel.rectTransform, new Vector2(.5f, .895f), new Vector2(1080, 62)); AddOutline(timelinePanel.gameObject, new Color(.35f, .43f, .56f, 1f), 1);
            timelineText = MakeText(timelinePanel.transform, "Timeline", string.Empty, font, 17, Vector2.one * .5f, new Vector2(1040, 54)); timelineText.supportRichText = true;
        }

        /// <summary>외부 배경 에셋 없이 하늘·원경·지면 층을 만들어 임시 전투 공간을 표현합니다.</summary>
        private Image CreateBattlefield(Transform parent)
        {
            Image battlefield = MakeImage(parent, "SideViewBattlefield", new Color(.045f, .08f, .12f, 1f)); SetRect(battlefield.rectTransform, new Vector2(.5f, .56f), new Vector2(1180, 410)); AddOutline(battlefield.gameObject, new Color(.28f, .37f, .5f, 1f), 2);
            Image sky = MakeImage(battlefield.transform, "NightSky", new Color(.055f, .105f, .16f, 1f)); sky.rectTransform.anchorMin = new Vector2(0, .42f); sky.rectTransform.anchorMax = Vector2.one; sky.rectTransform.offsetMin = Vector2.zero; sky.rectTransform.offsetMax = Vector2.zero;
            Image distance = MakeImage(battlefield.transform, "DistantField", new Color(.075f, .13f, .16f, 1f)); distance.rectTransform.anchorMin = new Vector2(0, .3f); distance.rectTransform.anchorMax = new Vector2(1, .55f); distance.rectTransform.offsetMin = Vector2.zero; distance.rectTransform.offsetMax = Vector2.zero;
            Image ground = MakeImage(battlefield.transform, "BattleGround", new Color(.055f, .095f, .105f, 1f)); ground.rectTransform.anchorMin = Vector2.zero; ground.rectTransform.anchorMax = new Vector2(1, .42f); ground.rectTransform.offsetMin = Vector2.zero; ground.rectTransform.offsetMax = Vector2.zero;
            Image horizon = MakeImage(battlefield.transform, "GoldenHorizon", new Color(.65f, .5f, .22f, .7f)); horizon.rectTransform.anchorMin = new Vector2(0, .415f); horizon.rectTransform.anchorMax = new Vector2(1, .425f); horizon.rectTransform.offsetMin = Vector2.zero; horizon.rectTransform.offsetMax = Vector2.zero;
            MakeText(battlefield.transform, "EnemySide", "적군", Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"), 15, new Vector2(.06f, .94f), new Vector2(70, 24)).color = new Color(.78f, .82f, .88f, .8f);
            MakeText(battlefield.transform, "AllySide", "아군", Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"), 15, new Vector2(.94f, .94f), new Vector2(70, 24)).color = new Color(.78f, .82f, .88f, .8f);
            return battlefield;
        }

        private void CreateFormationViews(Transform battlefield, Transform canvas, Font font, Formation formation)
        {
            foreach (Combatant combatant in formation.Members)
            {
                Vector2 anchor = GetBattlefieldAnchor(combatant.Side, combatant.Slot);
                CombatantView view = CreateCombatantView(battlefield, canvas, font, combatant, anchor);
                combatantViews.Add(combatant, view);
            }
        }

        /// <summary>논리 2×3 슬롯을 사이드뷰 전장의 실제 화면 좌표로 변환합니다.</summary>
        private static Vector2 GetBattlefieldAnchor(BattleSide side, FormationSlot slot)
        {
            float x;
            if (side == BattleSide.Enemies) x = slot.Row == FormationRow.Rear ? .14f : .32f;
            else x = slot.Row == FormationRow.Front ? .68f : .86f;
            float y = .72f - slot.Column * .22f;
            return new Vector2(x, y);
        }

        private CombatantView CreateCombatantView(Transform battlefield, Transform canvas, Font font, Combatant combatant, Vector2 anchor)
        {
            GameObject hitObject = new GameObject($"Combatant_{combatant.Id}", typeof(Image), typeof(Button)); hitObject.transform.SetParent(battlefield, false); SetRect(hitObject.GetComponent<RectTransform>(), anchor, new Vector2(165, 150));
            Image hitImage = hitObject.GetComponent<Image>(); hitImage.color = new Color(1f, 1f, 1f, .001f);
            Button hitArea = hitObject.GetComponent<Button>(); hitArea.targetGraphic = hitImage; hitArea.interactable = false; hitArea.transition = Selectable.Transition.None; hitArea.onClick.AddListener(() => SelectTarget(combatant));

            Image marker = MakeImage(hitObject.transform, "GroundMarker", new Color(.4f, .47f, .56f, .45f)); SetRect(marker.rectTransform, new Vector2(.5f, .12f), new Vector2(104, 15)); AddOutline(marker.gameObject, new Color(.65f, .72f, .8f, .7f), 1);
            Sprite sprite = combatant.Side == BattleSide.Allies ? BattleEncounterContext.PlayerBattleSprite : BattleEncounterContext.MonsterBattleSprite;
            Image spriteImage = MakeImage(hitObject.transform, "CharacterSprite", sprite == null ? Color.clear : Color.white); spriteImage.sprite = sprite; spriteImage.preserveAspect = true; SetRect(spriteImage.rectTransform, new Vector2(.5f, .49f), combatant.Side == BattleSide.Allies ? new Vector2(112, 126) : new Vector2(136, 112));
            Text targetArrow = MakeText(hitObject.transform, "TargetArrow", "▼", font, 28, new Vector2(.5f, .98f), new Vector2(50, 34)); targetArrow.color = focusGold; targetArrow.fontStyle = FontStyle.Bold; targetArrow.gameObject.SetActive(false);
            Text turnMarker = MakeText(hitObject.transform, "TurnMarker", "◆ 행동 중", font, 14, new Vector2(.5f, .9f), new Vector2(110, 26)); turnMarker.color = gold; turnMarker.fontStyle = FontStyle.Bold; turnMarker.gameObject.SetActive(false);

            EventTrigger trigger = hitObject.AddComponent<EventTrigger>();
            AddTrigger(trigger, EventTriggerType.Select, _ => targetArrow.gameObject.SetActive(choosingTarget && hitArea.interactable));
            AddTrigger(trigger, EventTriggerType.Deselect, _ => targetArrow.gameObject.SetActive(false));

            CombatantView view = new CombatantView { HitArea = hitArea, ActionRoot = hitObject.GetComponent<RectTransform>(), SpriteImage = spriteImage, IdleSprite = sprite, GroundMarker = marker, TargetArrow = targetArrow, TurnMarker = turnMarker };
            CreateCombatantHud(canvas, font, combatant, view);
            return view;
        }

        private void CreateCombatantHud(Transform canvas, Font font, Combatant combatant, CombatantView view)
        {
            Vector2 anchor;
            Vector2 size;
            if (combatant.Side == BattleSide.Enemies)
            {
                anchor = new Vector2(.11f + combatant.Slot.Column * .13f, combatant.Slot.Row == FormationRow.Front ? .805f : .745f);
                size = new Vector2(190, 76);
            }
            else
            {
                anchor = new Vector2(.63f + combatant.Slot.Column * .13f, combatant.Slot.Row == FormationRow.Front ? .265f : .205f);
                size = new Vector2(190, 76);
            }

            Image hud = MakeImage(canvas, $"Hud_{combatant.Id}", new Color(.035f, .055f, .09f, .96f)); SetRect(hud.rectTransform, anchor, size); AddOutline(hud.gameObject, combatant.Side == BattleSide.Allies ? gold : new Color(.48f, .55f, .65f, 1f), 1);
            view.HudName = MakeText(hud.transform, "Name", combatant.DisplayName, font, 15, new Vector2(.5f, .8f), new Vector2(size.x - 14, 22)); view.HudName.alignment = TextAnchor.MiddleLeft; view.HudName.fontStyle = FontStyle.Bold;
            view.HudHp = MakeText(hud.transform, "HpText", string.Empty, font, 13, new Vector2(.5f, .53f), new Vector2(size.x - 14, 20)); view.HudHp.alignment = TextAnchor.MiddleRight;
            view.HudStatus = MakeText(hud.transform, "StatusText", string.Empty, font, 13, new Vector2(.5f, .28f), new Vector2(size.x - 14, 18)); view.HudStatus.alignment = TextAnchor.MiddleLeft; view.HudStatus.color = focusGold; view.HudStatus.fontStyle = FontStyle.Bold;
            Image hpBackground = MakeImage(hud.transform, "HpBarBackground", new Color(.08f, .1f, .13f, 1f)); SetRect(hpBackground.rectTransform, new Vector2(.5f, .1f), new Vector2(size.x - 18, 10)); AddOutline(hpBackground.gameObject, new Color(.25f, .3f, .38f, 1f), 1);
            view.HudHpFill = MakeImage(hpBackground.transform, "HpFill", new Color(.25f, .72f, .46f, 1f)); Stretch(view.HudHpFill.rectTransform);
        }

        private void CreateCommandPanel(Transform parent, Font font)
        {
            Image commandPanel = MakeImage(parent, "CommandPanel", panel); SetRect(commandPanel.rectTransform, new Vector2(.5f, .12f), new Vector2(1100, 148)); AddOutline(commandPanel.gameObject, gold, 2);
            messageText = MakeText(commandPanel.transform, "Message", "행동을 선택하세요.", font, 17, new Vector2(.5f, .8f), new Vector2(1030, 30)); messageText.fontStyle = FontStyle.Bold;
            attackButton = MakeCommandButton(commandPanel.transform, "AttackButton", "공격", font, new Vector2(.17f, .42f), BeginAttack);
            skillButton = MakeCommandButton(commandPanel.transform, "SkillButton", "스킬", font, new Vector2(.39f, .42f), ShowSkillMenu);
            defendButton = MakeCommandButton(commandPanel.transform, "DefendButton", "방어", font, new Vector2(.61f, .42f), Defend);
            fleeButton = MakeCommandButton(commandPanel.transform, "FleeButton", "도망", font, new Vector2(.83f, .42f), Flee);
            MakeText(commandPanel.transform, "Help", "마우스 또는 방향키: 이동   Enter/Space: 선택   Esc: 대상 선택 취소   행동 시간제한 없음", font, 14, new Vector2(.5f, .11f), new Vector2(1030, 22)).color = new Color(.68f, .75f, .84f, 1f);
            skillMenuPanel = MakeImage(commandPanel.transform, "SkillMenu", new Color(.045f, .075f, .12f, 1f));
            SetRect(skillMenuPanel.rectTransform, new Vector2(.5f, .42f), new Vector2(1030, 62));
            AddOutline(skillMenuPanel.gameObject, gold, 1);
            skillMenuPanel.gameObject.SetActive(false);
        }

        private void AdvanceTurn()
        {
            if (battleEnded) return;
            if (enemies.IsDefeated) { EndBattle("승리! 초원 슬라임을 쓰러뜨렸습니다.", true); return; }
            if (allies.IsDefeated) { EndBattle("전투불능. Field_01로 복귀합니다.", false); return; }

            currentActor = turnOrder.TakeNext(AllCombatants);
            if (currentActor == null) return;
            currentActor.BeginTurn();
            skillCooldowns.BeginActorTurn(currentActor);
            UpdateTimeline();
            RefreshCombatantViews(null);
            if (currentActor.IsPlayerControlled)
            {
                messageText.text = $"{currentActor.DisplayName}의 행동을 선택하세요.";
                SetCommandButtons(true);
                EventSystem.current.SetSelectedGameObject(attackButton.gameObject);
            }
            else
            {
                SetCommandButtons(false);
                messageText.text = $"{currentActor.DisplayName}의 행동입니다.";
                StartCoroutine(EnemyAction());
            }
        }

        private void BeginAttack()
        {
            if (actionPlaying || currentActor == null || !currentActor.IsPlayerControlled) return;
            IReadOnlyList<Combatant> targets = TargetResolver.ResolveHostileTargets(currentActor, enemies, currentActor.BasicRange);
            if (targets.Count == 0) { messageText.text = "현재 기본 공격으로 지정할 수 있는 대상이 없습니다."; return; }
            choosingTarget = true;
            SetCommandButtons(false);
            RefreshCombatantViews(targets);
            messageText.text = "공격할 대상을 선택하세요. 금색으로 밝게 표시된 적을 선택할 수 있습니다.";
            EventSystem.current.SetSelectedGameObject(combatantViews[targets[0]].HitArea.gameObject);
        }

        private void SelectTarget(Combatant target)
        {
            if (actionPlaying || !choosingTarget || target == null || !target.IsAlive) return;
            IReadOnlyList<Combatant> valid = TargetResolver.ResolveHostileTargets(currentActor, enemies, currentActor.BasicRange);
            if (!valid.Contains(target)) return;
            choosingTarget = false;
            PlayBasicAttack(currentActor, target);
        }

        private void ShowSkillMenu()
        {
            if (actionPlaying || currentActor == null || !currentActor.IsPlayerControlled) return;
            choosingSkill = true;
            SetCommandButtons(false);
            RefreshCombatantViews(null);
            RebuildSkillMenu();
            skillMenuPanel.gameObject.SetActive(true);
            messageText.text = "사용할 스킬을 선택하세요. Esc로 이전 명령 메뉴로 돌아갑니다.";

            Button focus = skillMenuButtons.FirstOrDefault(button => button.interactable) ?? skillMenuButtons.LastOrDefault();
            if (EventSystem.current != null && focus != null) EventSystem.current.SetSelectedGameObject(focus.gameObject);
        }

        private void RebuildSkillMenu()
        {
            foreach (Transform child in skillMenuPanel.transform) Destroy(child.gameObject);
            skillMenuButtons.Clear();

            combatantJobs.TryGetValue(currentActor, out JobDefinition job);
            IReadOnlyList<BattleSkillDefinition> skills = BattleSkillCatalog.GetSkills(job);
            int itemCount = Math.Max(1, skills.Count) + 1;
            for (int index = 0; index < skills.Count; index++)
            {
                BattleSkillDefinition skill = skills[index];
                int remaining = skillCooldowns.GetRemaining(currentActor, skill.Id);
                string label = !skill.IsImplemented ? $"{skill.DisplayName}\n[미구현]"
                    : remaining > 0 ? $"{skill.DisplayName}\n[재사용 {remaining}턴]" : skill.DisplayName;
                BattleSkillDefinition selectedSkill = skill;
                Button button = MakeSkillMenuButton(skillMenuPanel.transform, $"Skill_{skill.Id}", label,
                    new Vector2((index + .5f) / itemCount, .5f), () => UseSkill(selectedSkill));
                button.interactable = skill.IsImplemented && remaining == 0;
                skillMenuButtons.Add(button);
            }

            if (skills.Count == 0)
            {
                Button empty = MakeSkillMenuButton(skillMenuPanel.transform, "NoSkills", "사용 가능한 스킬 없음",
                    new Vector2(.25f, .5f), () => messageText.text = "아직 사용할 수 없습니다.");
                empty.interactable = false;
                skillMenuButtons.Add(empty);
            }

            Button back = MakeSkillMenuButton(skillMenuPanel.transform, "Back", "돌아가기",
                new Vector2((itemCount - .5f) / itemCount, .5f), CloseSkillMenu);
            skillMenuButtons.Add(back);
        }

        private void CloseSkillMenu()
        {
            choosingSkill = false;
            skillMenuPanel.gameObject.SetActive(false);
            SetCommandButtons(true);
            messageText.text = $"{currentActor.DisplayName}의 행동을 선택하세요.";
            if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(skillButton.gameObject);
        }

        private void UseSkill(BattleSkillDefinition skill)
        {
            if (actionPlaying || !choosingSkill) return;
            if (!skillExecutor.CanUse(currentActor, skill, out string reason))
            {
                messageText.text = reason;
                RebuildSkillMenu();
                return;
            }

            choosingSkill = false;
            skillMenuPanel.gameObject.SetActive(false);
            actionPlaying = true;
            SetCommandButtons(false);
            RefreshCombatantViews(null);
            if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);

            Combatant actor = currentActor;
            Formation opponents = actor.Side == BattleSide.Allies ? enemies : allies;
            CombatantView actorView = combatantViews[actor];
            if (actionPresenter == null) actionPresenter = gameObject.AddComponent<BattleActionPresenter>();
            bool executed = false;
            StartCoroutine(actionPresenter.PlaySkillEmphasis(
                actorView.ActionRoot,
                actorView.SpriteImage,
                battleFont,
                skill.EffectType == BattleSkillEffectType.Taunt ? "도발!" : skill.DisplayName,
                () =>
                {
                    executed = skillExecutor.Execute(actor, skill, opponents, out string result);
                    messageText.text = result;
                    RefreshCombatantViews(null);
                },
                () =>
                {
                    RestoreBattleIdle(actorView);
                    actionPlaying = false;
                    if (executed) FinishCurrentAction();
                    else
                    {
                        SetCommandButtons(true);
                        if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(skillButton.gameObject);
                    }
                }));
        }

        private void Defend()
        {
            if (actionPlaying) return;
            currentActor.Defend();
            messageText.text = $"{currentActor.DisplayName}이(가) 방어합니다. 다음 행동 차례까지 받는 피해가 50% 감소합니다.";
            FinishCurrentAction();
        }

        private void Flee()
        {
            if (actionPlaying) return;
            if (enemies.Members.Any(item => item.IsBoss)) { messageText.text = "보스전에서는 도망칠 수 없습니다."; return; }
            battleEnded = true;
            SetCommandButtons(false);
            messageText.text = "도망에 성공했습니다. Field_01로 복귀합니다.";
            StartCoroutine(ReturnAfterDelay(false));
        }

        private IEnumerator EnemyAction()
        {
            yield return new WaitForSeconds(.55f);
            IReadOnlyList<Combatant> targets = TargetResolver.ResolveHostileTargets(currentActor, allies, currentActor.BasicRange);
            Combatant target = targets.FirstOrDefault();
            if (target != null)
            {
                PlayBasicAttack(currentActor, target);
                yield break;
            }
            FinishCurrentAction();
        }

        /// <summary>
        /// 기본 공격 계산은 기존 Combatant에 맡기고, 공용 Presenter에는 표시 대상과 타격 시점만 전달합니다.
        /// 현재 1단계 범위에서는 플레이어와 초원 슬라임의 근거리 기본 공격에 같은 연출을 사용합니다.
        /// </summary>
        private void PlayBasicAttack(Combatant actor, Combatant target)
        {
            actionPlaying = true;
            SetCommandButtons(false);
            RefreshCombatantViews(null);
            if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);

            CombatantView actorView = combatantViews[actor];
            CombatantView targetView = combatantViews[target];
            if (actionPresenter == null) actionPresenter = gameObject.AddComponent<BattleActionPresenter>();
            Func<int> applyImpact = () => target.TakeDamage(actor.Attack);
            Action<int> onImpact = damage =>
            {
                messageText.text = $"{actor.DisplayName}의 공격! {target.DisplayName}에게 {damage} 피해.";
                RefreshCombatantViews(null);
            };
            Action onComplete = () =>
            {
                RestoreBattleIdle(actorView);
                RestoreBattleIdle(targetView);
                actionPlaying = false;
                FinishCurrentAction();
            };

            if (actor.BasicRange == TargetRangeType.RangedPhysical)
            {
                StartCoroutine(actionPresenter.PlayProjectileAttack(
                    actorView.ActionRoot,
                    targetView.ActionRoot,
                    targetView.SpriteImage,
                    battleFont,
                    LoadProjectileFrames("BattleProjectiles/GoldenArrow"),
                    .08f,
                    .35f,
                    new Vector2(52f, 52f),
                    true,
                    Color.white,
                    BattleProjectileStyle.Arrow,
                    .2f,
                    applyImpact,
                    onImpact,
                    onComplete));
                return;
            }

            if (actor.BasicRange == TargetRangeType.Magic)
            {
                bool healerAttack = actor.IsPlayerControlled && GameSessionData.SelectedJobId == "healer";
                StartCoroutine(actionPresenter.PlayProjectileAttack(
                    actorView.ActionRoot,
                    targetView.ActionRoot,
                    targetView.SpriteImage,
                    battleFont,
                    healerAttack ? LoadProjectileFrames("BattleProjectiles/HealerMagicalProjectile") : LoadProjectileFrames("BattleProjectiles/Fireball"),
                    healerAttack ? .05f : .06f,
                    healerAttack ? .24f : .35f,
                    healerAttack ? new Vector2(72f, 72f) : new Vector2(52f, 52f),
                    true,
                    Color.white,
                    BattleProjectileStyle.Orb,
                    .26f,
                    applyImpact,
                    onImpact,
                    onComplete));
                return;
            }

            StartCoroutine(actionPresenter.PlayMeleeAttack(
                actorView.ActionRoot,
                targetView.ActionRoot,
                targetView.SpriteImage,
                battleFont,
                applyImpact,
                onImpact,
                onComplete));
        }
        /// <summary>파일명 순서로 추출된 Projectile 프레임을 불러와 공용 Presenter에 전달합니다.</summary>
        private static Sprite[] LoadProjectileFrames(string resourcesPath)
        {
            return Resources.LoadAll<Sprite>(resourcesPath).OrderBy(sprite => sprite.name).ToArray();
        }
        /// <summary>공격 연출 뒤 필드 프레임이 아닌 진영별 전투 Idle Sprite를 다시 적용합니다.</summary>
        private static void RestoreBattleIdle(CombatantView view)
        {
            if (view?.SpriteImage != null) view.SpriteImage.sprite = view.IdleSprite;
        }

        private void FinishCurrentAction()
        {
            currentActor?.CompleteAction();
            statusEffects.RemoveInvalidTaunts(AllCombatants);
            RefreshCombatantViews(null);
            SetCommandButtons(false);
            StartCoroutine(AdvanceAfterDelay());
        }

        private IEnumerator AdvanceAfterDelay() { yield return new WaitForSeconds(.65f); AdvanceTurn(); }

        private void EndBattle(string message, bool defeatedEncounteredMonster)
        {
            battleEnded = true;
            SetCommandButtons(false);
            messageText.text = message + " 전투 상태를 초기화하고 Field_01로 복귀합니다.";
            StartCoroutine(ReturnAfterDelay(defeatedEncounteredMonster));
        }

        private IEnumerator ReturnAfterDelay(bool defeatedEncounteredMonster)
        {
            yield return new WaitForSeconds(1.2f);
            BattleSceneFlow.ReturnToField(defeatedEncounteredMonster);
        }

        private void RefreshCombatantViews(IReadOnlyList<Combatant> attackable)
        {
            statusEffects.RemoveInvalidTaunts(AllCombatants);
            foreach (KeyValuePair<Combatant, CombatantView> pair in combatantViews)
            {
                Combatant combatant = pair.Key;
                CombatantView view = pair.Value;
                bool targetSelection = attackable != null && combatant.Side == BattleSide.Enemies;
                bool canAttack = targetSelection && attackable.Contains(combatant);
                view.HitArea.interactable = canAttack && combatant.IsAlive;
                view.SpriteImage.color = view.SpriteImage.sprite == null ? Color.clear : !combatant.IsAlive ? new Color(.35f, .35f, .4f, .45f) : targetSelection && !canAttack ? new Color(.42f, .45f, .5f, .42f) : Color.white;
                view.GroundMarker.color = canAttack ? new Color(1f, .78f, .28f, .92f) : new Color(.4f, .47f, .56f, combatant.IsAlive ? .45f : .18f);
                view.TargetArrow.gameObject.SetActive(false);
                view.TurnMarker.gameObject.SetActive(combatant == currentActor && combatant.IsAlive);
                view.HudName.text = combatant.DisplayName + (combatant.IsAlive ? string.Empty : "  [전투불능]");
                view.HudHp.text = $"HP {combatant.CurrentHp} / {combatant.MaxHp}";
                bool taunted = combatant.ForcedTargetActionsRemaining > 0 && combatant.ForcedTarget != null && combatant.ForcedTarget.IsAlive;
                view.HudStatus.text = taunted ? $"도발 {combatant.ForcedTargetActionsRemaining}" : string.Empty;
                float healthRatio = combatant.MaxHp <= 0 ? 0f : Mathf.Clamp01((float)combatant.CurrentHp / combatant.MaxHp);
                RectTransform hpFillRect = view.HudHpFill.rectTransform;
                hpFillRect.anchorMin = Vector2.zero;
                hpFillRect.anchorMax = new Vector2(healthRatio, 1f);
                hpFillRect.offsetMin = Vector2.zero;
                hpFillRect.offsetMax = Vector2.zero;
                view.HudHpFill.color = healthRatio <= .3f ? new Color(.82f, .28f, .24f, 1f) : new Color(.25f, .72f, .46f, 1f);
            }
        }

        private void UpdateTimeline()
        {
            string upcoming = string.Join("  →  ", turnOrder.Upcoming.Take(7).Select(item => item.DisplayName));
            if (string.IsNullOrEmpty(upcoming)) upcoming = "다음 라운드 순서 갱신";
            timelineText.text = $"<color=#FFD66F><b>현재 행동  ◆ {currentActor.DisplayName}</b></color>\n<color=#C7D4E6>다음 순서  {upcoming}</color>";
        }

        private void SetCommandButtons(bool enabled)
        {
            foreach (Selectable selectable in commandButtons) selectable.interactable = enabled;
        }

        private Button MakeCommandButton(Transform parent, string name, string label, Font font, Vector2 anchor, Action action)
        {
            GameObject obj = new GameObject(name, typeof(Image), typeof(Button), typeof(Outline)); obj.transform.SetParent(parent, false); SetRect(obj.GetComponent<RectTransform>(), anchor, new Vector2(205, 48));
            Image image = obj.GetComponent<Image>(); image.color = new Color(.12f, .32f, .5f, 1f);
            Button button = obj.GetComponent<Button>(); button.targetGraphic = image; button.onClick.AddListener(() => action()); button.colors = ColorBlock.defaultColorBlock;
            Outline outline = obj.GetComponent<Outline>(); outline.effectColor = gold; outline.effectDistance = new Vector2(2, -2);
            Text text = MakeText(obj.transform, "Label", $"[ {label} ]", font, 20, Vector2.one * .5f, new Vector2(195, 42)); text.fontStyle = FontStyle.Bold;
            commandButtons.Add(button); return button;
        }

        private Button MakeSkillMenuButton(Transform parent, string name, string label, Vector2 anchor, Action action)
        {
            GameObject obj = new GameObject(name, typeof(Image), typeof(Button), typeof(Outline));
            obj.transform.SetParent(parent, false);
            SetRect(obj.GetComponent<RectTransform>(), anchor, new Vector2(225, 48));
            Image image = obj.GetComponent<Image>();
            image.color = new Color(.1f, .27f, .43f, 1f);
            Button button = obj.GetComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(() => action());
            Outline outline = obj.GetComponent<Outline>();
            outline.effectColor = gold;
            outline.effectDistance = new Vector2(1, -1);
            Text text = MakeText(obj.transform, "Label", label, battleFont, 15, Vector2.one * .5f, new Vector2(215, 44));
            text.fontStyle = FontStyle.Bold;
            return button;
        }
        private static void CreateEventSystem() { if (EventSystem.current != null) return; InputSystemUIInputModule module = new GameObject("EventSystem", typeof(EventSystem)).AddComponent<InputSystemUIInputModule>(); module.AssignDefaultActions(); }
        private static void CreateCameraIfMissing() { if (Camera.main != null) { Camera.main.backgroundColor = new Color(.018f, .03f, .06f, 1f); return; } GameObject obj = new GameObject("Main Camera"); obj.tag = "MainCamera"; obj.transform.position = new Vector3(0, 0, -10); Camera camera = obj.AddComponent<Camera>(); camera.clearFlags = CameraClearFlags.SolidColor; camera.backgroundColor = new Color(.018f, .03f, .06f, 1f); camera.orthographic = true; }
        private static void AddTrigger(EventTrigger trigger, EventTriggerType type, Action<BaseEventData> action) { EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type }; entry.callback.AddListener(data => action(data)); trigger.triggers.Add(entry); }
        private static Text MakeText(Transform parent, string name, string value, Font font, int size, Vector2 anchor, Vector2 dimensions) { GameObject obj = new GameObject(name, typeof(Text)); obj.transform.SetParent(parent, false); Text text = obj.GetComponent<Text>(); text.font = font; text.fontSize = size; text.color = Color.white; text.alignment = TextAnchor.MiddleCenter; text.text = value; text.raycastTarget = false; SetRect(text.rectTransform, anchor, dimensions); return text; }
        private static Image MakeImage(Transform parent, string name, Color color) { GameObject obj = new GameObject(name, typeof(Image)); obj.transform.SetParent(parent, false); Image image = obj.GetComponent<Image>(); image.color = color; image.raycastTarget = false; return image; }
        private static Outline AddOutline(GameObject target, Color color, float distance) { Outline outline = target.AddComponent<Outline>(); outline.effectColor = color; outline.effectDistance = new Vector2(distance, -distance); return outline; }
        private static void SetRect(RectTransform rect, Vector2 anchor, Vector2 size) { rect.anchorMin = anchor; rect.anchorMax = anchor; rect.pivot = Vector2.one * .5f; rect.anchoredPosition = Vector2.zero; rect.sizeDelta = size; }
        private static void Stretch(RectTransform rect) { rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one; rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero; }
    }
}
