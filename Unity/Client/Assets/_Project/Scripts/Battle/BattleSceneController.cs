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
            public Text HudStatus;
            public Image HudHpFill;
            public bool UsesPlaceholderVisual;
        }

        private readonly List<Selectable> commandButtons = new List<Selectable>();
        private readonly Dictionary<Combatant, CombatantView> combatantViews = new Dictionary<Combatant, CombatantView>();
        private readonly Dictionary<Combatant, JobDefinition> combatantJobs = new Dictionary<Combatant, JobDefinition>();
        private readonly Dictionary<Combatant, BattleParticipantSetup> participantSetups = new Dictionary<Combatant, BattleParticipantSetup>();
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
        private IReadOnlyList<Combatant> selectableTargets = Array.Empty<Combatant>();
        private Action<Combatant> targetSelectedAction;
        private BattleActionPresenter actionPresenter;
        private BattleSkillExecutor skillExecutor;
        private Font battleFont;
        private RectTransform battleCanvasRect;
        private Image detailPopup;
        private Text detailPopupText;
        private Combatant hoveredCombatant;
        private Combatant focusedCombatant;
        private Combatant detailCombatant;

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
                selectableTargets = Array.Empty<Combatant>();
                targetSelectedAction = null;
                SetCommandButtons(true);
                RefreshCombatantViews(null);
                messageText.text = $"{currentActor.DisplayName}의 행동을 선택하세요.";
                if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(attackButton.gameObject);
            }
        }

        /// <summary>캐릭터 생성 정보와 전투 전용 3대3 Encounter 데이터를 실제 Formation 참가자로 변환합니다.</summary>
        private void CreateParticipants()
        {
            PlayerPathDefinition path = Resources.LoadAll<PlayerPathDefinition>("PathDefinitions").FirstOrDefault(item => item.Id == GameSessionData.SelectedPlayerPathId);
            JobDefinition playerJob = Resources.LoadAll<JobDefinition>("JobDefinitions").FirstOrDefault(item => item.JobId == GameSessionData.SelectedJobId);
            int health = CharacterCreationStatsCalculator.GetFinalStat(path, playerJob, CharacterStatType.Health);
            int strength = CharacterCreationStatsCalculator.GetFinalStat(path, playerJob, CharacterStatType.Strength);
            int agility = CharacterCreationStatsCalculator.GetFinalStat(path, playerJob, CharacterStatType.Agility);
            string playerName = string.IsNullOrWhiteSpace(GameSessionData.PlayerName) ? "플레이어" : GameSessionData.PlayerName;
            int playerAttack = 12 + Math.Max(0, strength - 10) * 2;
            // 치유사의 기본 공격은 같은 능력치 기준에서도 다른 직업보다 낮은 피해를 주는 기존 검증값을 유지합니다.
            if (GameSessionData.SelectedJobId == "healer") playerAttack = Math.Max(1, playerAttack - 4);

            allies = new Formation(BattleSide.Allies);
            enemies = new Formation(BattleSide.Enemies);
            MonsterDefinition monster = BattleEncounterContext.Monster ?? Resources.LoadAll<MonsterDefinition>("MonsterDefinitions").FirstOrDefault();
            string monsterId = monster == null ? "grass_slime" : monster.MonsterId;
            string monsterName = monster == null ? "초원 슬라임" : monster.DisplayName;
            BattleEncounterSetup setup = BattlePrototypeEncounterFactory.CreateThreeVsThree(
                playerName, GameSessionData.SelectedJobId, 80 + health * 4, playerAttack, agility, monsterId, monsterName);

            Dictionary<string, JobDefinition> jobs = Resources.LoadAll<JobDefinition>("JobDefinitions")
                .ToDictionary(item => item.JobId, StringComparer.Ordinal);
            foreach (BattleParticipantSetup participant in setup.Allies.Concat(setup.Enemies))
            {
                Combatant combatant = new Combatant(participant.Id, participant.DisplayName, participant.Side,
                    participant.Slot, participant.MaxHp, participant.Attack, participant.Agility,
                    participant.ActionPriority, participant.BasicRange, participant.IsPlayerControlled);
                (participant.Side == BattleSide.Allies ? allies : enemies).Place(combatant);
                participantSetups.Add(combatant, participant);
                if (!string.IsNullOrEmpty(participant.JobId) && jobs.TryGetValue(participant.JobId, out JobDefinition participantJob))
                    combatantJobs.Add(combatant, participantJob);
            }
        }
        private void CreateInterface()
        {
            CreateCameraIfMissing();
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            battleFont = font;
            GameObject canvasObject = new GameObject("BattleCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas canvas = canvasObject.GetComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            battleCanvasRect = canvasObject.GetComponent<RectTransform>();
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; scaler.referenceResolution = new Vector2(1280, 720); scaler.matchWidthOrHeight = .5f;
            Image background = MakeImage(canvasObject.transform, "Background", navy); Stretch(background.rectTransform);

            Text title = MakeText(canvasObject.transform, "Title", "전투", font, 31, new Vector2(.5f, .965f), new Vector2(260, 42)); title.color = gold; title.fontStyle = FontStyle.Bold;
            CreateTimeline(canvasObject.transform, font);
            Image battlefield = CreateBattlefield(canvasObject.transform);
            CreateFormationViews(battlefield.transform, canvasObject.transform, font, enemies);
            CreateFormationViews(battlefield.transform, canvasObject.transform, font, allies);
            CreateDetailPopup(canvasObject.transform, font);
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
            BattleParticipantSetup setup = participantSetups[combatant];
            Sprite sprite = setup.VisualType == BattleParticipantVisualType.Player ? BattleEncounterContext.PlayerBattleSprite
                : setup.VisualType == BattleParticipantVisualType.EncounterMonster ? BattleEncounterContext.MonsterBattleSprite : null;
            bool placeholder = setup.VisualType == BattleParticipantVisualType.PrototypeCompanion;
            Image spriteImage = MakeImage(hitObject.transform, "CharacterSprite", placeholder ? new Color(.12f, .3f, .48f, 1f) : sprite == null ? Color.clear : Color.white);
            spriteImage.sprite = sprite;
            spriteImage.preserveAspect = true;
            SetRect(spriteImage.rectTransform, new Vector2(.5f, .49f), placeholder ? new Vector2(82, 104) : combatant.Side == BattleSide.Allies ? new Vector2(112, 126) : new Vector2(136, 112));
            if (placeholder)
            {
                AddOutline(spriteImage.gameObject, gold, 2);
                Text initial = MakeText(spriteImage.transform, "PrototypeInitial", setup.PlaceholderLabel, font, 34, Vector2.one * .5f, new Vector2(72, 72));
                initial.color = focusGold;
                initial.fontStyle = FontStyle.Bold;
            }
            Text targetArrow = MakeText(hitObject.transform, "TargetArrow", "▼", font, 28, new Vector2(.5f, 1.2f), new Vector2(50, 34)); targetArrow.color = focusGold; targetArrow.fontStyle = FontStyle.Bold; targetArrow.gameObject.SetActive(false);
            Text turnMarker = MakeText(hitObject.transform, "TurnMarker", "◆ 행동 중", font, 14, new Vector2(.5f, 1.14f), new Vector2(110, 26)); turnMarker.color = gold; turnMarker.fontStyle = FontStyle.Bold; turnMarker.gameObject.SetActive(false);

            EventTrigger trigger = hitObject.AddComponent<EventTrigger>();
            AddTrigger(trigger, EventTriggerType.PointerEnter, _ => OnCombatantPointerEnter(combatant));
            AddTrigger(trigger, EventTriggerType.PointerExit, _ => OnCombatantPointerExit(combatant));
            AddTrigger(trigger, EventTriggerType.Select, _ => OnCombatantFocused(combatant));
            AddTrigger(trigger, EventTriggerType.Deselect, _ => OnCombatantFocusLost(combatant));

            CombatantView view = new CombatantView { HitArea = hitArea, ActionRoot = hitObject.GetComponent<RectTransform>(), SpriteImage = spriteImage, IdleSprite = sprite, GroundMarker = marker, TargetArrow = targetArrow, TurnMarker = turnMarker, UsesPlaceholderVisual = placeholder };
            CreateCombatantMiniHud(font, combatant, view);
            return view;
        }
        /// <summary>캐릭터 가까이에 이름과 작은 HP Bar, 즉시 확인할 상태 표식만 배치합니다.</summary>
        private void CreateCombatantMiniHud(Font font, Combatant combatant, CombatantView view)
        {
            Image miniHud = MakeImage(view.ActionRoot, $"MiniHud_{combatant.Id}", new Color(.025f, .045f, .075f, .88f));
            SetRect(miniHud.rectTransform, new Vector2(.5f, .98f), new Vector2(144, 52));
            AddOutline(miniHud.gameObject, combatant.Side == BattleSide.Allies ? gold : new Color(.48f, .55f, .65f, 1f), 1);

            view.HudName = MakeText(miniHud.transform, "Name", combatant.DisplayName, font, 13,
                new Vector2(.5f, .77f), new Vector2(134, 19));
            view.HudName.fontStyle = FontStyle.Bold;
            Image hpBackground = MakeImage(miniHud.transform, "HpBarBackground", new Color(.08f, .1f, .13f, 1f));
            SetRect(hpBackground.rectTransform, new Vector2(.5f, .45f), new Vector2(126, 9));
            AddOutline(hpBackground.gameObject, new Color(.25f, .3f, .38f, 1f), 1);
            view.HudHpFill = MakeImage(hpBackground.transform, "HpFill", new Color(.25f, .72f, .46f, 1f));
            Stretch(view.HudHpFill.rectTransform);
            view.HudStatus = MakeText(miniHud.transform, "ImportantStatus", string.Empty, font, 12,
                new Vector2(.5f, .15f), new Vector2(134, 18));
            view.HudStatus.color = focusGold;
            view.HudStatus.fontStyle = FontStyle.Bold;
        }

        /// <summary>모든 참가자가 공유하는 상세 상태 팝업을 하나만 생성합니다.</summary>
        private void CreateDetailPopup(Transform canvas, Font font)
        {
            detailPopup = MakeImage(canvas, "CombatantDetailPopup", new Color(.035f, .06f, .1f, .98f));
            SetRect(detailPopup.rectTransform, Vector2.one * .5f, new Vector2(270, 140));
            AddOutline(detailPopup.gameObject, gold, 2);
            detailPopupText = MakeText(detailPopup.transform, "DetailText", string.Empty, font, 16,
                Vector2.one * .5f, new Vector2(244, 118));
            detailPopupText.alignment = TextAnchor.MiddleLeft;
            detailPopupText.lineSpacing = 1.15f;
            detailPopup.gameObject.SetActive(false);
        }

        private void OnCombatantPointerEnter(Combatant combatant)
        {
            hoveredCombatant = combatant;
            RefreshDetailPopupPreference();
        }

        private void OnCombatantPointerExit(Combatant combatant)
        {
            if (hoveredCombatant == combatant) hoveredCombatant = null;
            RefreshDetailPopupPreference();
        }

        private void OnCombatantFocused(Combatant combatant)
        {
            focusedCombatant = combatant;
            if (combatantViews.TryGetValue(combatant, out CombatantView view))
                view.TargetArrow.gameObject.SetActive(choosingTarget && view.HitArea.interactable);
            RefreshDetailPopupPreference();
        }

        private void OnCombatantFocusLost(Combatant combatant)
        {
            if (focusedCombatant == combatant) focusedCombatant = null;
            if (combatantViews.TryGetValue(combatant, out CombatantView view))
                view.TargetArrow.gameObject.SetActive(false);
            RefreshDetailPopupPreference();
        }

        /// <summary>키보드 포커스를 Hover보다 우선하여 동시에 하나의 팝업만 표시합니다.</summary>
        private void RefreshDetailPopupPreference()
        {
            Combatant preferred = focusedCombatant ?? hoveredCombatant;
            if (preferred == null)
            {
                detailCombatant = null;
                if (detailPopup != null) detailPopup.gameObject.SetActive(false);
                return;
            }
            ShowDetailPopup(preferred);
        }

        private void ShowDetailPopup(Combatant combatant)
        {
            if (detailPopup == null || detailPopupText == null || !combatantViews.TryGetValue(combatant, out CombatantView view)) return;
            combatantJobs.TryGetValue(combatant, out JobDefinition job);
            participantSetups.TryGetValue(combatant, out BattleParticipantSetup setup);
            BattleCombatantStatusViewModel model = BattleCombatantStatusViewModelFactory.Create(combatant, job, setup, skillCooldowns);
            detailCombatant = combatant;
            detailPopupText.text = model.DetailText;
            int lineCount = model.DetailText.Count(character => character == '\n') + 1;
            detailPopup.rectTransform.sizeDelta = new Vector2(270, Mathf.Max(116, 34 + lineCount * 23));
            detailPopupText.rectTransform.sizeDelta = new Vector2(244, detailPopup.rectTransform.sizeDelta.y - 20);
            detailPopup.gameObject.SetActive(true);
            detailPopup.rectTransform.SetAsLastSibling();
            PositionDetailPopup(view.ActionRoot);
        }

        /// <summary>화면 좌우 가장자리에서는 캐릭터 반대쪽 안쪽에 놓고 Canvas 경계 안으로 위치를 제한합니다.</summary>
        private void PositionDetailPopup(RectTransform source)
        {
            if (battleCanvasRect == null || detailPopup == null || source == null) return;
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, source.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(battleCanvasRect, screenPoint, null, out Vector2 localPoint);
            RectTransform popupRect = detailPopup.rectTransform;
            float direction = screenPoint.x < Screen.width * .5f ? 1f : -1f;
            localPoint.x += direction * (source.rect.width * .5f + popupRect.rect.width * .5f + 14f);

            float halfWidth = popupRect.rect.width * .5f;
            float halfHeight = popupRect.rect.height * .5f;
            Rect canvasBounds = battleCanvasRect.rect;
            localPoint.x = Mathf.Clamp(localPoint.x, canvasBounds.xMin + halfWidth + 12f, canvasBounds.xMax - halfWidth - 12f);
            localPoint.y = Mathf.Clamp(localPoint.y, canvasBounds.yMin + halfHeight + 12f, canvasBounds.yMax - halfHeight - 12f);
            popupRect.anchoredPosition = localPoint;
        }
        private void CreateCommandPanel(Transform parent, Font font)
        {
            Image commandPanel = MakeImage(parent, "CommandPanel", panel); SetRect(commandPanel.rectTransform, new Vector2(.5f, .12f), new Vector2(1100, 148)); AddOutline(commandPanel.gameObject, gold, 2);
            messageText = MakeText(commandPanel.transform, "Message", "행동을 선택하세요.", font, 17, new Vector2(.5f, .8f), new Vector2(1030, 30)); messageText.fontStyle = FontStyle.Bold;
            attackButton = MakeCommandButton(commandPanel.transform, "AttackButton", "공격", font, new Vector2(.17f, .42f), BeginAttack);
            skillButton = MakeCommandButton(commandPanel.transform, "SkillButton", "스킬", font, new Vector2(.39f, .42f), ShowSkillMenu);
            defendButton = MakeCommandButton(commandPanel.transform, "DefendButton", "방어", font, new Vector2(.61f, .42f), Defend);
            fleeButton = MakeCommandButton(commandPanel.transform, "FleeButton", "도망", font, new Vector2(.83f, .42f), Flee);
            MakeText(commandPanel.transform, "Help", "마우스/방향키: 이동   캐릭터 Hover/포커스: 상세   Esc: 취소   시간제한 없음", font, 14, new Vector2(.5f, .11f), new Vector2(1030, 22)).color = new Color(.68f, .75f, .84f, 1f);
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
            Formation opponents = currentActor.Side == BattleSide.Allies ? enemies : allies;
            IReadOnlyList<Combatant> targets = TargetResolver.ResolveHostileTargets(currentActor, opponents, currentActor.BasicRange);
            BeginTargetSelection(targets, target => PlayBasicAttack(currentActor, target),
                "공격할 대상을 선택하세요. 금색으로 밝게 표시된 적을 선택할 수 있습니다.");
        }

        /// <summary>
        /// 적 공격과 향후 아군 치유가 같은 마우스·키보드 대상 선택 흐름을 공유하도록 후보와 완료 동작을 받습니다.
        /// 사거리나 스킬 대상 규칙은 호출자가 계산하며 이 메서드는 UI 선택만 담당합니다.
        /// </summary>
        private void BeginTargetSelection(IReadOnlyList<Combatant> targets, Action<Combatant> onSelected, string prompt)
        {
            if (targets == null || targets.Count == 0)
            {
                messageText.text = "현재 지정할 수 있는 대상이 없습니다.";
                return;
            }
            choosingTarget = true;
            selectableTargets = targets;
            targetSelectedAction = onSelected;
            SetCommandButtons(false);
            RefreshCombatantViews(targets);
            messageText.text = prompt;
            if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(combatantViews[targets[0]].HitArea.gameObject);
        }

        private void SelectTarget(Combatant target)
        {
            if (actionPlaying || !choosingTarget || target == null || !target.IsAlive || !selectableTargets.Contains(target)) return;
            Action<Combatant> selectedAction = targetSelectedAction;
            choosingTarget = false;
            selectableTargets = Array.Empty<Combatant>();
            targetSelectedAction = null;
            selectedAction?.Invoke(target);
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
            Combatant target = ChooseEnemyTarget(currentActor, targets);
            if (target != null)
            {
                PlayBasicAttack(currentActor, target);
                yield break;
            }
            FinishCurrentAction();
        }

        /// <summary>
        /// 도발로 후보가 한 명이면 그 대상을 그대로 사용합니다.
        /// 평상시에는 적의 진형 열을 이용해 여러 유효 아군에게 공격이 분산되어 강제 타깃 여부를 확인할 수 있게 합니다.
        /// </summary>
        private static Combatant ChooseEnemyTarget(Combatant actor, IReadOnlyList<Combatant> targets)
        {
            if (targets == null || targets.Count == 0) return null;
            if (targets.Count == 1) return targets[0];
            int targetIndex = Mathf.Abs(actor.Slot.Column) % targets.Count;
            return targets[targetIndex];
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
                bool healerAttack = combatantJobs.TryGetValue(actor, out JobDefinition actorJob) && actorJob.JobId == "healer";
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
            if (view?.SpriteImage != null && !view.UsesPlaceholderVisual) view.SpriteImage.sprite = view.IdleSprite;
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
                bool targetSelection = attackable != null;
                bool canAttack = targetSelection && attackable.Contains(combatant);
                bool canInspect = !battleEnded && !actionPlaying && currentActor != null && currentActor.IsPlayerControlled && !choosingSkill;
                view.HitArea.interactable = combatant.IsAlive && (targetSelection ? canAttack : canInspect);
                Color normalVisual = view.UsesPlaceholderVisual ? new Color(.12f, .3f, .48f, 1f) : view.SpriteImage.sprite == null ? Color.clear : Color.white;
                view.SpriteImage.color = !combatant.IsAlive ? new Color(.35f, .35f, .4f, .45f) : targetSelection && !canAttack ? new Color(.42f, .45f, .5f, .42f) : normalVisual;
                view.GroundMarker.color = canAttack ? new Color(1f, .78f, .28f, .92f) : new Color(.4f, .47f, .56f, combatant.IsAlive ? .45f : .18f);
                view.TargetArrow.gameObject.SetActive(false);
                view.TurnMarker.gameObject.SetActive(combatant == currentActor && combatant.IsAlive);
                view.HudName.text = combatant.DisplayName + (combatant.IsAlive ? string.Empty : " [전투불능]");
                combatantJobs.TryGetValue(combatant, out JobDefinition statusJob);
                participantSetups.TryGetValue(combatant, out BattleParticipantSetup statusSetup);
                BattleCombatantStatusViewModel statusModel = BattleCombatantStatusViewModelFactory.Create(combatant, statusJob, statusSetup, skillCooldowns);
                view.HudStatus.text = statusModel.CompactStatus;
                float healthRatio = combatant.MaxHp <= 0 ? 0f : Mathf.Clamp01((float)combatant.CurrentHp / combatant.MaxHp);
                RectTransform hpFillRect = view.HudHpFill.rectTransform;
                hpFillRect.anchorMin = Vector2.zero;
                hpFillRect.anchorMax = new Vector2(healthRatio, 1f);
                hpFillRect.offsetMin = Vector2.zero;
                hpFillRect.offsetMax = Vector2.zero;
                view.HudHpFill.color = healthRatio <= .3f ? new Color(.82f, .28f, .24f, 1f) : new Color(.25f, .72f, .46f, 1f);
            }
            if (detailCombatant != null) ShowDetailPopup(detailCombatant);
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
