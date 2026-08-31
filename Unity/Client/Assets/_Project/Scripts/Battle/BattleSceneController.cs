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
            public bool UsesPlaceholderVisual;
            public MonsterSpriteSheetAnimation MonsterAnimation;
        }

        /// <summary>
        /// 상단 HP HUD에서 참가자 한 명을 표현합니다.
        /// 전장 위 Sprite와 목록 행을 Combatant 하나로 연결해 두므로, 피해를 받거나 키보드 포커스가
        /// 이동해도 별도의 전투 계산 없이 같은 참가자의 표시만 함께 갱신할 수 있습니다.
        /// </summary>
        private sealed class CombatantHpRow
        {
            public Image Background;
            public Outline Border;
            public Text Name;
            public Image HpFill;
            public Text FallenStatus;
            public List<BattleHudStatusBadge> StatusBadges;
        }

        /// <summary>상단 HP 한 칸 안에서 작은 Sprite와 짧은 설명을 함께 보여 주는 고정 자리입니다.</summary>
        private sealed class BattleHudStatusBadge
        {
            public GameObject Root;
            public Image Icon;
            public Text Label;
        }

        private readonly List<Selectable> commandButtons = new List<Selectable>();
        private readonly Dictionary<Combatant, CombatantView> combatantViews = new Dictionary<Combatant, CombatantView>();
        private readonly Dictionary<Combatant, CombatantHpRow> combatantHpRows = new Dictionary<Combatant, CombatantHpRow>();
        private readonly Dictionary<Combatant, JobDefinition> combatantJobs = new Dictionary<Combatant, JobDefinition>();
        private readonly Dictionary<Combatant, BattleParticipantSetup> participantSetups = new Dictionary<Combatant, BattleParticipantSetup>();
        private readonly BattleSkillCooldowns skillCooldowns = new BattleSkillCooldowns();
        private readonly BattleFighterResourceRuntime fighterResources = new BattleFighterResourceRuntime();
        private readonly BattleStatusEffectRuntime statusEffects = new BattleStatusEffectRuntime();
        private readonly List<Button> skillMenuButtons = new List<Button>();
        private readonly Color navy = new Color(.018f, .03f, .06f, 1f);
        private readonly Color panel = new Color(.055f, .08f, .13f, .97f);
        private readonly Color gold = new Color(.88f, .7f, .32f, 1f);
        private readonly Color focusGold = new Color(1f, .86f, .48f, 1f);
        // 1280×720 기준으로 HP HUD 아래와 명령 패널 위에 확보한 상세 팝업 전용 세로 범위입니다.
        // CanvasScaler가 화면 비율에 맞춰 함께 확대·축소하므로 16:9 해상도에서도 같은 UI 관계를 유지합니다.
        private const float DetailAreaBottom = 171f;
        private const float DetailAreaTop = 491f;
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
        private Button cancelButton;
        private Image skillMenuPanel;
        private bool choosingTarget;
        private bool choosingSkill;
        // 기본 공격 대상 취소는 명령으로, 정조준·치유처럼 스킬이 연 대상 취소는 스킬 목록으로 돌아갑니다.
        // 같은 대상 선택 UI를 쓰되 돌아갈 화면만 기억하면 향후 아군 버프·다른 단일 공격도 이 흐름을 재사용할 수 있습니다.
        private bool targetSelectionReturnsToSkillMenu;
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
        private Image skillDetailPopup;
        private Image skillDetailIcon;
        private Text skillDetailText;
        private Combatant hoveredCombatant;
        private Combatant focusedCombatant;
        private Combatant detailCombatant;
        private BattleSkillDefinition hoveredSkill;
        private BattleSkillDefinition focusedSkill;

        private IEnumerable<Combatant> AllCombatants => allies.Members.Concat(enemies.Members);

        private void Awake()
        {
            skillExecutor = new BattleSkillExecutor(skillCooldowns, statusEffects, fighterResources);
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
            CancelCurrentSelection();
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
            MonsterDefinition[] monsterDefinitions = Resources.LoadAll<MonsterDefinition>("MonsterDefinitions");
            MonsterDefinition slime = monsterDefinitions.FirstOrDefault(item => item.MonsterId == "grass_slime");
            MonsterDefinition venomBee = monsterDefinitions.FirstOrDefault(item => item.MonsterId == "venom_bee");
            BattleEncounterSetup setup = BattlePrototypeEncounterFactory.CreateThreeVsThree(
                playerName, GameSessionData.SelectedJobId, 80 + health * 4, playerAttack, agility, slime, venomBee);

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
            CreateTopHpHud(canvasObject.transform, font);
            CreateDetailPopup(canvasObject.transform, font);
            CreateCommandPanel(canvasObject.transform, font);
            CreateSkillDetailPopup(canvasObject.transform, font);
        }

        private void CreateTimeline(Transform parent, Font font)
        {
            Image timelinePanel = MakeImage(parent, "TurnTimelinePanel", new Color(.035f, .055f, .09f, .98f)); SetRect(timelinePanel.rectTransform, new Vector2(.5f, .895f), new Vector2(1080, 62)); AddOutline(timelinePanel.gameObject, new Color(.35f, .43f, .56f, 1f), 1);
            timelineText = MakeText(timelinePanel.transform, "Timeline", string.Empty, font, 17, Vector2.one * .5f, new Vector2(1040, 54)); timelineText.supportRichText = true;
        }

        /// <summary>외부 배경 에셋 없이 하늘·원경·지면 층을 만들어 임시 전투 공간을 표현합니다.</summary>
        private Image CreateBattlefield(Transform parent)
        {
            // HP 정보는 바로 위의 전용 HUD가 맡습니다. 전장은 HUD 아래와 명령 패널 위에만 배치해
            // 참가자가 6명으로 늘어나도 HP 항목이 Sprite나 도발 표시 위에 그려지지 않게 합니다.
            Image battlefield = MakeImage(parent, "SideViewBattlefield", new Color(.045f, .08f, .12f, 1f)); SetRect(battlefield.rectTransform, new Vector2(.5f, .46f), new Vector2(1180, 320)); AddOutline(battlefield.gameObject, new Color(.28f, .37f, .5f, 1f), 2);
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
            // 세로 공간이 줄어든 전장 안에서도 상태 Anchor가 위쪽 HUD와 닿지 않도록 첫 줄을 조금 내립니다.
            float y = .68f - slot.Column * .21f;
            return new Vector2(x, y);
        }

        private CombatantView CreateCombatantView(Transform battlefield, Transform canvas, Font font, Combatant combatant, Vector2 anchor)
        {
            GameObject hitObject = new GameObject($"Combatant_{combatant.Id}", typeof(Image), typeof(Button)); hitObject.transform.SetParent(battlefield, false); SetRect(hitObject.GetComponent<RectTransform>(), anchor, new Vector2(165, 150));
            Image hitImage = hitObject.GetComponent<Image>(); hitImage.color = new Color(1f, 1f, 1f, .001f);
            Button hitArea = hitObject.GetComponent<Button>(); hitArea.targetGraphic = hitImage; hitArea.interactable = false; hitArea.transition = Selectable.Transition.None; hitArea.onClick.AddListener(() => SelectTarget(combatant));

            // GroundMarker의 RectTransform은 대상 표시 기준으로 계속 보관하지만 평상시 Graphic은 숨깁니다.
            // 근거리 이동·Projectile·피격 연출은 ActionRoot/SpriteImage를 사용하므로 회색 발판을 없애도 좌표가 바뀌지 않습니다.
            Image marker = MakeImage(hitObject.transform, "GroundMarker", Color.clear);
            SetRect(marker.rectTransform, new Vector2(.5f, .12f), new Vector2(104, 7));
            BattleParticipantSetup setup = participantSetups[combatant];
            Sprite sprite = setup.VisualType == BattleParticipantVisualType.Player ? BattleEncounterContext.PlayerBattleSprite
                : setup.VisualType == BattleParticipantVisualType.EncounterMonster
                    ? BattleVisualResolver.ResolveMonsterIdleSprite(setup.MonsterDefinition, BattleEncounterContext.MonsterBattleSprite)
                    : null;
            bool placeholder = setup.VisualType == BattleParticipantVisualType.PrototypeCompanion;
            Image spriteImage = MakeImage(hitObject.transform, "CharacterSprite", placeholder ? new Color(.12f, .3f, .48f, 1f) : sprite == null ? Color.clear : Color.white);
            spriteImage.sprite = sprite;
            spriteImage.preserveAspect = true;
            // Sprite를 HitArea 중앙보다 조금 아래에 두어 위쪽 StatusAnchor와 시각적으로 분리합니다.
            Vector2 displaySize = placeholder ? new Vector2(82, 104) : combatant.Side == BattleSide.Allies
                ? new Vector2(112, 126) : new Vector2(136, 112);
            if (setup.MonsterDefinition != null && setup.MonsterDefinition.UsesSpriteSheetAnimation)
                displaySize *= setup.MonsterDefinition.VisualScale;
            SetRect(spriteImage.rectTransform, new Vector2(.5f, .45f), displaySize);
            MonsterSpriteSheetAnimation monsterAnimation = null;
            if (setup.MonsterDefinition != null && setup.MonsterDefinition.UsesSpriteSheetAnimation)
            {
                monsterAnimation = spriteImage.gameObject.AddComponent<MonsterSpriteSheetAnimation>();
                monsterAnimation.Configure(spriteImage, setup.MonsterDefinition);
                // 적은 화면 왼쪽에서 오른쪽의 아군을 바라봅니다. Pilot Bee 원본이 우향이므로 그대로 두고,
                // 반대 방향 원본인 새 몬스터만 X축을 런타임에 뒤집어 별도 PNG를 만들지 않습니다.
                float direction = setup.MonsterDefinition.SourceFacesRight ? 1f : -1f;
                spriteImage.rectTransform.localScale = new Vector3(direction, 1f, 1f);
            }
            if (placeholder)
            {
                AddOutline(spriteImage.gameObject, gold, 2);
                Text initial = MakeText(spriteImage.transform, "PrototypeInitial", setup.PlaceholderLabel, font, 34, Vector2.one * .5f, new Vector2(72, 72));
                initial.color = focusGold;
                initial.fontStyle = FontStyle.Bold;
            }
            // 대상 화살표는 넓은 HitArea나 상태 표시판이 아니라 SpriteImage의 자식입니다.
            // 부모 Sprite의 가로 중앙(anchor x = 0.5)과 위쪽(anchor y = 1)을 기준으로 삼기 때문에
            // 전열/후열이나 Sprite 크기가 달라도 선택 대상의 실제 표시 영역 중앙 위에 정확히 따라갑니다.
            Text targetArrow = MakeText(spriteImage.transform, "TargetArrow", "▼", font, 25,
                new Vector2(.5f, 1f), new Vector2(34, 30));
            targetArrow.rectTransform.anchoredPosition = new Vector2(0f, 13f);
            targetArrow.color = focusGold;
            targetArrow.fontStyle = FontStyle.Bold;
            targetArrow.gameObject.SetActive(false);

            // 전장 주변에는 꼭 필요한 행동 중 표시만 남깁니다. 도발·방어·재사용 정보는 상단 HP HUD가
            // 담당하므로 Sprite를 가리지 않고, 상세 수치는 Hover/포커스 팝업에서 다시 확인할 수 있습니다.
            GameObject statusAnchorObject = new GameObject("StatusAnchor", typeof(RectTransform));
            statusAnchorObject.transform.SetParent(hitObject.transform, false);
            RectTransform statusAnchor = statusAnchorObject.GetComponent<RectTransform>();
            SetRect(statusAnchor, new Vector2(.5f, .98f), new Vector2(150, 24));
            Text turnMarker = MakeText(statusAnchor, "TurnMarker", "◆ 행동 중", font, 13,
                new Vector2(.5f, .5f), new Vector2(126, 18));
            turnMarker.color = gold;
            turnMarker.fontStyle = FontStyle.Bold;
            turnMarker.gameObject.SetActive(false);

            EventTrigger trigger = hitObject.AddComponent<EventTrigger>();
            AddTrigger(trigger, EventTriggerType.PointerEnter, _ => OnCombatantPointerEnter(combatant));
            AddTrigger(trigger, EventTriggerType.PointerExit, _ => OnCombatantPointerExit(combatant));
            AddTrigger(trigger, EventTriggerType.Select, _ => OnCombatantFocused(combatant));
            AddTrigger(trigger, EventTriggerType.Deselect, _ => OnCombatantFocusLost(combatant));

            // 공용 시트 재생기는 Configure 시점에 첫 Idle 프레임을 넣으므로, 최초 지역 변수보다
            // 실제 Image에 표시된 프레임을 복귀 기준으로 보관해야 Attack 뒤 그림이 비지 않습니다.
            CombatantView view = new CombatantView { HitArea = hitArea, ActionRoot = hitObject.GetComponent<RectTransform>(), SpriteImage = spriteImage, IdleSprite = spriteImage.sprite, GroundMarker = marker, TargetArrow = targetArrow, TurnMarker = turnMarker, UsesPlaceholderVisual = placeholder, MonsterAnimation = monsterAnimation };
            return view;
        }

        /// <summary>
        /// 타임라인과 전장 사이에 HP만 담당하는 상단 HUD를 만듭니다.
        /// HP HUD와 전장의 부모를 분리하면 어느 진영의 참가자가 늘어나도 목록이 캐릭터 상태표시를 침범하지 않습니다.
        /// </summary>
        private void CreateTopHpHud(Transform canvas, Font font)
        {
            Image hpHud = MakeImage(canvas, "TopHpHud", new Color(.025f, .045f, .075f, .97f));
            SetRect(hpHud.rectTransform, new Vector2(.5f, .765f), new Vector2(1080, 116));
            AddOutline(hpHud.gameObject, new Color(.35f, .43f, .56f, 1f), 1);

            CreateHpRoster(hpHud.transform, font, enemies, 6, 3, .74f, .31f, "적군");
            CreateHpRoster(hpHud.transform, font, allies, 3, 3, .14f, 0f, "아군");
        }

        /// <summary>
        /// 한 진영의 참가자를 상단 HUD의 가로 격자에 배치합니다.
        /// index를 열 수(3)로 나눈 나머지는 가로 위치, 몫은 세로 줄이 됩니다.
        /// 따라서 적 1~3명은 첫 줄만, 4~6명은 둘째 줄까지 사용하고 아군 1~3명은 항상 한 줄만 사용합니다.
        /// Formation은 읽기만 하므로 이 표시 순서와 크기는 N대N 전투 판정에 영향을 주지 않습니다.
        /// </summary>
        private void CreateHpRoster(Transform hpHud, Font font, Formation formation, int maximumItems,
            int columnsPerRow, float firstRowY, float rowStep, string title)
        {
            List<Combatant> members = formation.Members.Take(maximumItems).ToList();
            if (members.Count == 0) return;

            Text rosterTitle = MakeText(hpHud, $"{formation.Side}Title", title, font, 13,
                new Vector2(.055f, firstRowY), new Vector2(72, 24));
            rosterTitle.fontStyle = FontStyle.Bold;

            for (int index = 0; index < members.Count; index++)
            {
                Combatant combatant = members[index];
                int column = index % columnsPerRow;
                int rowIndex = index / columnsPerRow;
                float x = .22f + column * .29f;
                float y = firstRowY - rowIndex * rowStep;
                Image row = MakeImage(hpHud, $"HpRow_{combatant.Id}", new Color(.055f, .08f, .13f, .96f));
                SetRect(row.rectTransform, new Vector2(x, y), new Vector2(286, 32));
                Outline border = AddOutline(row.gameObject, new Color(.2f, .27f, .36f, 1f), 1);
                Text name = MakeText(row.transform, "Name", combatant.DisplayName, font, 12,
                    new Vector2(.27f, .68f), new Vector2(140, 17));
                name.fontStyle = FontStyle.Bold;
                Image hpBackground = MakeImage(row.transform, "HpBarBackground", new Color(.08f, .1f, .13f, 1f));
                SetRect(hpBackground.rectTransform, new Vector2(.74f, .68f), new Vector2(128, 9));
                AddOutline(hpBackground.gameObject, new Color(.25f, .3f, .38f, 1f), 1);
                Image hpFill = MakeImage(hpBackground.transform, "HpFill", new Color(.25f, .72f, .46f, 1f));
                Stretch(hpFill.rectTransform);
                // 상단 HUD는 한눈에 판단할 짧은 요약만 담당합니다. 정확한 HP 숫자와 모든 현재 정보는
                // 기존 상세 팝업에 남겨 두어 작은 칸이 긴 설명으로 복잡해지지 않게 합니다.
                Text fallenStatus = MakeText(row.transform, "FallenStatus", string.Empty, font, 10,
                     new Vector2(.5f, .19f), new Vector2(274, 13));
                fallenStatus.color = focusGold;
                fallenStatus.fontStyle = FontStyle.Bold;
                fallenStatus.gameObject.SetActive(false);
                combatantHpRows.Add(combatant, new CombatantHpRow
                {
                    Background = row,
                    Border = border,
                    Name = name,
                    HpFill = hpFill,
                    FallenStatus = fallenStatus,
                    StatusBadges = CreateStatusBadgeSlots(row.transform, font)
                });
            }
        }

        /// <summary>
        /// 상태는 최대 네 칸을 미리 만들어 두고 필요한 칸만 켭니다. 매 갱신마다 오브젝트를 만들고 지우지 않아
        /// 화면 깜박임을 피하며, 도발·방어·행동·재사용이 한 줄 안에서 서로 밀어내지 않게 합니다.
        /// 상단 HUD는 즉시 판단할 요약이고 상세 팝업은 정확한 수치와 설명을 담당하므로 둘의 역할도 유지됩니다.
        /// </summary>
        private List<BattleHudStatusBadge> CreateStatusBadgeSlots(Transform parent, Font font)
        {
            List<BattleHudStatusBadge> badges = new List<BattleHudStatusBadge>();
            for (int index = 0; index < 4; index++)
            {
                GameObject root = new GameObject($"StatusBadge_{index}", typeof(RectTransform));
                root.transform.SetParent(parent, false);
                SetRect(root.GetComponent<RectTransform>(), new Vector2(.145f + index * .235f, .19f), new Vector2(66, 13));
                Image icon = MakeSpriteIcon(root.transform, "Icon", null, new Vector2(.11f, .5f), new Vector2(12, 12));
                Text label = MakeText(root.transform, "Label", string.Empty, font, 10,
                    new Vector2(.62f, .5f), new Vector2(50, 13));
                label.color = focusGold;
                label.fontStyle = FontStyle.Bold;
                label.alignment = TextAnchor.MiddleLeft;
                label.resizeTextForBestFit = true;
                label.resizeTextMinSize = 8;
                label.resizeTextMaxSize = 10;
                root.SetActive(false);
                badges.Add(new BattleHudStatusBadge { Root = root, Icon = icon, Label = label });
            }
            return badges;
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
            // Hover 이벤트는 마우스가 HUD 위에 남아 있으면 연출 중에도 다시 전달될 수 있습니다. 이때 단순히
            // 팝업만 숨기면 다음 화면 갱신에서 이전 대상을 다시 열 수 있으므로 포커스 정보까지 함께 비웁니다.
            if (actionPlaying)
            {
                HideCombatantDetailPopupForAction();
                return;
            }

            Combatant preferred = focusedCombatant ?? hoveredCombatant;
            RefreshHpRowHighlights(preferred);
            if (preferred == null)
            {
                detailCombatant = null;
                if (detailPopup != null) detailPopup.gameObject.SetActive(false);
                return;
            }
            ShowDetailPopup(preferred);
        }

        /// <summary>
        /// 캐릭터 Hover보다 키보드/대상 선택 포커스를 우선하는 기존 규칙을 HP 목록에도 적용합니다.
        /// 색상만 바꾸지 않고 밝은 테두리와 배경을 함께 바꿔 현재 행을 알아보기 쉽게 합니다.
        /// </summary>
        private void RefreshHpRowHighlights(Combatant preferred)
        {
            foreach (KeyValuePair<Combatant, CombatantHpRow> pair in combatantHpRows)
            {
                bool highlighted = pair.Key == preferred;
                pair.Value.Background.color = highlighted
                    ? new Color(.14f, .23f, .34f, 1f)
                    : new Color(.055f, .08f, .13f, .96f);
                pair.Value.Border.effectColor = highlighted ? focusGold : new Color(.2f, .27f, .36f, 1f);
                pair.Value.Border.effectDistance = highlighted ? new Vector2(2f, -2f) : new Vector2(1f, -1f);
            }
        }

        private void ShowDetailPopup(Combatant combatant)
        {
            // 실제 공격·회복·Projectile·VFX를 보는 동안에는 정보창이 연출을 덮지 않게 합니다.
            // 이벤트 경로를 놓치더라도 마지막 표시 함수에서 한 번 더 막아 연출 중 재오픈을 방지합니다.
            if (actionPlaying || detailPopup == null || detailPopupText == null ||
                !combatantViews.TryGetValue(combatant, out CombatantView view)) return;
            combatantJobs.TryGetValue(combatant, out JobDefinition job);
            participantSetups.TryGetValue(combatant, out BattleParticipantSetup setup);
            BattleCombatantStatusViewModel model = BattleCombatantStatusViewModelFactory.Create(
                combatant, job, setup, skillCooldowns, fighterResources, statusEffects);
            detailCombatant = combatant;
            detailPopupText.text = model.DetailText;
            int lineCount = model.DetailText.Count(character => character == '\n') + 1;
            detailPopup.rectTransform.sizeDelta = new Vector2(270, Mathf.Max(116, 34 + lineCount * 23));
            detailPopupText.rectTransform.sizeDelta = new Vector2(244, detailPopup.rectTransform.sizeDelta.y - 20);
            detailPopup.gameObject.SetActive(true);
            detailPopup.rectTransform.SetAsLastSibling();
            PositionDetailPopup(view.ActionRoot);
        }

        /// <summary>
        /// 화면 좌우 가장자리에서는 캐릭터 반대쪽에 팝업을 놓습니다.
        /// 세로 위치는 HP HUD 아래와 명령 패널 위로 제한해 상세 정보가 항상 전장 내부에서만 열리게 합니다.
        /// </summary>
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
            float safeBottom = canvasBounds.yMin + DetailAreaBottom;
            float safeTop = canvasBounds.yMin + DetailAreaTop;
            localPoint.y = Mathf.Clamp(localPoint.y, safeBottom + halfHeight + 10f, safeTop - halfHeight - 10f);
            popupRect.anchoredPosition = localPoint;
        }
        private void CreateCommandPanel(Transform parent, Font font)
        {
            Image commandPanel = MakeImage(parent, "CommandPanel", panel); SetRect(commandPanel.rectTransform, new Vector2(.5f, .105f), new Vector2(1100, 126)); AddOutline(commandPanel.gameObject, gold, 2);
            messageText = MakeText(commandPanel.transform, "Message", "행동을 선택하세요.", font, 16, new Vector2(.5f, .79f), new Vector2(1030, 26)); messageText.fontStyle = FontStyle.Bold;
            // 기존 205×48 버튼에서 가로·세로를 약 17% 줄였습니다. 글자는 18px을 유지하고
            // 네 버튼 간 중심 간격을 다시 맞춰 클릭 영역은 충분하면서 패널이 덜 답답하게 보이게 합니다.
            attackButton = MakeCommandButton(commandPanel.transform, "AttackButton", BattleUiIconCatalog.Attack, "공격", font, new Vector2(.15f, .42f), BeginAttack);
            skillButton = MakeCommandButton(commandPanel.transform, "SkillButton", BattleUiIconCatalog.Skill, "스킬", font, new Vector2(.35f, .42f), ShowSkillMenu);
            defendButton = MakeCommandButton(commandPanel.transform, "DefendButton", BattleUiIconCatalog.Defend, "방어", font, new Vector2(.55f, .42f), Defend);
            fleeButton = MakeCommandButton(commandPanel.transform, "FleeButton", BattleUiIconCatalog.Flee, "도망", font, new Vector2(.75f, .42f), Flee);
            cancelButton = MakeAuxiliaryButton(commandPanel.transform, "CancelButton", "취소", font,
                new Vector2(.92f, .42f), BattleUiIconCatalog.Cancel, CancelCurrentSelection);
            cancelButton.gameObject.SetActive(false);
            MakeText(commandPanel.transform, "Help", "마우스/방향키: 이동   캐릭터 Hover/포커스: 상세   Esc/취소: 이전   시간제한 없음", font, 13, new Vector2(.5f, .1f), new Vector2(1030, 20)).color = new Color(.68f, .75f, .84f, 1f);
            skillMenuPanel = MakeImage(commandPanel.transform, "SkillMenu", new Color(.045f, .075f, .12f, 1f));
            SetRect(skillMenuPanel.rectTransform, new Vector2(.45f, .42f), new Vector2(850, 54));
            AddOutline(skillMenuPanel.gameObject, gold, 1);
            skillMenuPanel.gameObject.SetActive(false);
        }

        /// <summary>
        /// 스킬 버튼은 빠른 선택을 위해 이름만 보여 주고, 긴 설명은 이 공용 팝업 한 개가 담당합니다.
        /// 전장 중앙의 안전 영역에 고정해 하단 명령 패널과 상단 HP HUD를 가리지 않으며, 여러 스킬을
        /// 오갈 때 오브젝트를 새로 만들지 않고 내용만 교체합니다.
        /// </summary>
        private void CreateSkillDetailPopup(Transform canvas, Font font)
        {
            skillDetailPopup = MakeImage(canvas, "SkillDetailPopup", new Color(.035f, .06f, .1f, .98f));
            SetRect(skillDetailPopup.rectTransform, new Vector2(.5f, .37f), new Vector2(430, 220));
            AddOutline(skillDetailPopup.gameObject, gold, 2);
            skillDetailIcon = MakeSpriteIcon(skillDetailPopup.transform, "SkillIcon", null,
                new Vector2(.08f, .86f), new Vector2(30, 30));
            skillDetailText = MakeText(skillDetailPopup.transform, "SkillDetailText", string.Empty, font, 14,
                new Vector2(.56f, .48f), new Vector2(360, 194));
            skillDetailText.alignment = TextAnchor.UpperLeft;
            skillDetailText.lineSpacing = 1.08f;
            skillDetailPopup.gameObject.SetActive(false);
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
                SetCancelButtonVisible(false);
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
        private void BeginTargetSelection(IReadOnlyList<Combatant> targets, Action<Combatant> onSelected, string prompt,
            bool returnToSkillMenuOnCancel = false)
        {
            if (targets == null || targets.Count == 0)
            {
                messageText.text = "현재 지정할 수 있는 대상이 없습니다.";
                return;
            }
            // 대상 선택은 스킬 정보를 읽는 단계가 아니라 실제 전장을 보며 목표를 정하는 단계입니다.
            // 어떤 스킬이 이 메서드를 호출하더라도 상세 팝업을 다시 닫아, 전장과 대상 표시를 가리지 않게 합니다.
            HideSkillDetailPopup();
            choosingTarget = true;
            targetSelectionReturnsToSkillMenu = returnToSkillMenuOnCancel;
            selectableTargets = targets;
            targetSelectedAction = onSelected;
            SetCommandButtons(false);
            SetCancelButtonVisible(true);
            RefreshCombatantViews(targets);
            messageText.text = prompt;
            if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(combatantViews[targets[0]].HitArea.gameObject);
        }

        private void SelectTarget(Combatant target)
        {
            if (actionPlaying || !choosingTarget || target == null || !target.IsAlive || !selectableTargets.Contains(target)) return;
            Action<Combatant> selectedAction = targetSelectedAction;
            choosingTarget = false;
            targetSelectionReturnsToSkillMenu = false;
            selectableTargets = Array.Empty<Combatant>();
            targetSelectedAction = null;
            SetCancelButtonVisible(false);
            selectedAction?.Invoke(target);
        }

        private void ShowSkillMenu()
        {
            if (actionPlaying || currentActor == null || !currentActor.IsPlayerControlled) return;
            choosingSkill = true;
            SetCommandButtons(false);
            SetCancelButtonVisible(true);
            RefreshCombatantViews(null);
            RebuildSkillMenu();
            skillMenuPanel.gameObject.SetActive(true);
            messageText.text = "사용할 스킬을 선택하세요. Esc 또는 취소로 이전 명령 메뉴로 돌아갑니다.";

            Button focus = skillMenuButtons.FirstOrDefault(button => button.interactable) ?? skillMenuButtons.LastOrDefault();
            if (EventSystem.current != null && focus != null) EventSystem.current.SetSelectedGameObject(focus.gameObject);
        }

        private void RebuildSkillMenu()
        {
            HideSkillDetailPopup();
            foreach (Transform child in skillMenuPanel.transform) Destroy(child.gameObject);
            skillMenuButtons.Clear();

            combatantJobs.TryGetValue(currentActor, out JobDefinition job);
            IReadOnlyList<BattleSkillDefinition> skills = BattleSkillCatalog.GetSkills(job);
            int itemCount = Math.Max(1, skills.Count) + 1;
            for (int index = 0; index < skills.Count; index++)
            {
                BattleSkillDefinition skill = skills[index];
                int remaining = skillCooldowns.GetRemaining(currentActor, skill.Id);
                // 버튼은 빠르게 훑는 선택 목록이므로 이름만 기본 표시합니다. 재사용 중일 때만 현재 조작
                // 가능 여부를 즉시 알 수 있도록 짧은 남은 턴을 붙이고, 나머지 설명은 공용 팝업으로 옮깁니다.
                string label = remaining > 0 ? $"{skill.DisplayName}\n재사용 {remaining}턴" : skill.DisplayName;
                BattleSkillDefinition selectedSkill = skill;
                Button button = MakeSkillMenuButton(skillMenuPanel.transform, $"Skill_{skill.Id}", label, skill.IconId,
                    new Vector2((index + .5f) / itemCount, .5f), () => UseSkill(selectedSkill), selectedSkill);
                bool canExecute = skill.IsImplemented && remaining == 0;
                // Unity의 interactable=false 버튼은 키보드 포커스도 받을 수 없어 설명을 읽을 수 없습니다.
                // 버튼 자체는 포커스 가능하게 두되 어둡게 표시하고, 실행 시 기존 CanUse가 미구현·쿨타임을
                // 차단합니다. 즉 접근 가능한 설명과 실제 사용 가능 여부를 서로 분리한 것입니다.
                if (!canExecute)
                {
                    button.targetGraphic.color = new Color(.055f, .1f, .15f, 1f);
                    button.GetComponent<Outline>().effectColor = new Color(.28f, .3f, .34f, 1f);
                }
                skillMenuButtons.Add(button);
            }

            if (skills.Count == 0)
            {
                Button empty = MakeSkillMenuButton(skillMenuPanel.transform, "NoSkills", "사용 가능한 스킬 없음", null,
                    new Vector2(.25f, .5f), () => messageText.text = "아직 사용할 수 없습니다.");
                empty.interactable = false;
                skillMenuButtons.Add(empty);
            }

            Button back = MakeSkillMenuButton(skillMenuPanel.transform, "Back", "돌아가기", null,
                new Vector2((itemCount - .5f) / itemCount, .5f), CloseSkillMenu);
            skillMenuButtons.Add(back);
        }

        private void CloseSkillMenu()
        {
            choosingSkill = false;
            skillMenuPanel.gameObject.SetActive(false);
            HideSkillDetailPopup();
            SetCancelButtonVisible(false);
            SetCommandButtons(true);
            messageText.text = $"{currentActor.DisplayName}의 행동을 선택하세요.";
            if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(skillButton.gameObject);
        }

        /// <summary>
        /// Esc 키와 화면의 취소 버튼이 함께 사용하는 단 하나의 복귀 흐름입니다.
        /// 스킬 메뉴에서는 기존 CloseSkillMenu를 재사용하고, 대상 선택에서는 후보와 완료 함수를 비운 뒤
        /// 기본 명령으로 돌아갑니다. 두 입력이 같은 메서드를 호출하므로 마우스와 키보드 결과가 달라지지 않습니다.
        /// </summary>
        private void CancelCurrentSelection()
        {
            if (battleEnded || actionPlaying) return;
            if (choosingSkill)
            {
                CloseSkillMenu();
                return;
            }
            if (!choosingTarget) return;

            bool returnToSkillMenu = targetSelectionReturnsToSkillMenu;
            choosingTarget = false;
            targetSelectionReturnsToSkillMenu = false;
            selectableTargets = Array.Empty<Combatant>();
            targetSelectedAction = null;
            SetCancelButtonVisible(false);
            // 치유의 빛에서 대상을 고르던 중이면 스킬 자체를 취소한 것이 아니므로 스킬 목록으로 돌아갑니다.
            // Esc와 화면 취소 버튼 모두 이 메서드를 호출해 키보드와 마우스의 복귀 단계가 동일합니다.
            if (returnToSkillMenu)
            {
                ShowSkillMenu();
                return;
            }
            SetCommandButtons(true);
            RefreshCombatantViews(null);
            messageText.text = $"{currentActor.DisplayName}의 행동을 선택하세요.";
            if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(attackButton.gameObject);
        }

        /// <summary>취소할 수 있는 하위 UI 단계에서만 보조 버튼을 보여 기본 명령 화면의 복잡도를 줄입니다.</summary>
        private void SetCancelButtonVisible(bool visible)
        {
            if (cancelButton != null) cancelButton.gameObject.SetActive(visible);
        }

        private void OnSkillPointerEnter(BattleSkillDefinition skill)
        {
            hoveredSkill = skill;
            RefreshSkillDetailPopupPreference();
        }

        private void OnSkillPointerExit(BattleSkillDefinition skill)
        {
            if (hoveredSkill == skill) hoveredSkill = null;
            RefreshSkillDetailPopupPreference();
        }

        private void OnSkillFocused(BattleSkillDefinition skill)
        {
            focusedSkill = skill;
            RefreshSkillDetailPopupPreference();
        }

        private void OnSkillFocusLost(BattleSkillDefinition skill)
        {
            if (focusedSkill == skill) focusedSkill = null;
            RefreshSkillDetailPopupPreference();
        }

        /// <summary>
        /// 키보드 포커스를 마우스 Hover보다 우선합니다. 두 입력이 같은 표시 메서드를 사용하므로
        /// 마우스를 쓸 수 없는 사용자도 방향키로 동일한 설명을 읽을 수 있고, 팝업은 항상 하나만 열립니다.
        /// </summary>
        private void RefreshSkillDetailPopupPreference()
        {
            BattleSkillDefinition preferred = focusedSkill ?? hoveredSkill;
            if (actionPlaying || !choosingSkill || preferred == null)
            {
                if (skillDetailPopup != null) skillDetailPopup.gameObject.SetActive(false);
                return;
            }

            ShowSkillDetailPopup(preferred);
        }

        private void ShowSkillDetailPopup(BattleSkillDefinition skill)
        {
            if (actionPlaying || skillDetailPopup == null || skillDetailText == null || skill == null) return;

            List<string> lines = new List<string> { skill.DisplayName, skill.Description };
            if (!string.IsNullOrWhiteSpace(skill.TypeDescription)) lines.Add(skill.TypeDescription);
            if (!string.IsNullOrWhiteSpace(skill.TargetDescription)) lines.Add(skill.TargetDescription);
            if (!string.IsNullOrWhiteSpace(skill.EffectDescription)) lines.Add(skill.EffectDescription);
            if (!string.IsNullOrWhiteSpace(skill.DurationDescription)) lines.Add(skill.DurationDescription);
            // 기세별 피해 데이터가 있는 스킬만 현재값을 덧붙입니다. 스킬 이름을 비교하지 않으므로 같은 데이터
            // 구조를 쓰는 후속 기술도 자동으로 현재 기세와 예상 배율을 표시할 수 있습니다.
            if (skill.MomentumDamagePercents.Count == BattleFighterResourceRuntime.MaxMomentum + 1 && currentActor != null)
            {
                int momentum = fighterResources.GetMomentum(currentActor);
                lines.Add($"현재 기세: {momentum}");
                lines.Add($"현재 예상 피해: 일반 공격의 {skill.MomentumDamagePercents[momentum]}%");
            }
            lines.Add(skill.CooldownTurns > 0 ? $"재사용: {skill.CooldownTurns}턴" : "재사용: 없음");
            lines.Add(skill.IsImplemented ? "구현: 사용 가능" : "구현: 미구현");

            skillDetailText.text = string.Join("\n", lines.Where(line => !string.IsNullOrWhiteSpace(line)));
            Sprite sprite = BattleUiIconCatalog.Load(skill.IconId);
            skillDetailIcon.sprite = sprite;
            skillDetailIcon.gameObject.SetActive(sprite != null);
            skillDetailPopup.gameObject.SetActive(true);
            skillDetailPopup.rectTransform.SetAsLastSibling();
        }

        private void HideSkillDetailPopup()
        {
            hoveredSkill = null;
            focusedSkill = null;
            if (skillDetailPopup != null) skillDetailPopup.gameObject.SetActive(false);
        }

        /// <summary>
        /// 행동 연출은 피해·회복 시점과 캐릭터 움직임을 읽어야 하는 화면이므로 Hover 정보창과 동시에
        /// 보여 주지 않습니다. 팝업만 끄지 않고 기존 Hover·키보드 포커스 대상도 비워야, 마우스가 HUD 위에
        /// 계속 머물러 있어도 화면 갱신이 과거 대상을 사용해 팝업을 다시 여는 일을 막을 수 있습니다.
        /// 연출 종료 뒤에는 사용자가 새로 Hover하거나 포커스할 때 이벤트가 다시 대상을 설정합니다.
        /// </summary>
        private void HideCombatantDetailPopupForAction()
        {
            hoveredCombatant = null;
            focusedCombatant = null;
            detailCombatant = null;
            if (detailPopup != null) detailPopup.gameObject.SetActive(false);
            RefreshHpRowHighlights(null);
        }

        /// <summary>
        /// 스킬 설명과 캐릭터 상태 설명은 내용은 다르지만 둘 다 전장 위에 뜨는 정보 팝업입니다.
        /// 공용 actionPlaying 상태에서 한 메서드로 닫아 기본 공격·회복·모든 스킬 Presenter가 같은 규칙을 따릅니다.
        /// </summary>
        private void SuppressInformationPopupsDuringAction()
        {
            HideSkillDetailPopup();
            HideCombatantDetailPopupForAction();
        }

        /// <summary>
        /// 스킬 설명은 선택하기 전에 판단을 돕는 정보 UI이고, 대상 선택과 행동 연출은 전장을 보여 주는 실행 UI입니다.
        /// 두 역할을 분리하기 위해 스킬을 확정한 순간 메뉴·Hover·키보드 포커스·상세 팝업을 한 번에 정리합니다.
        /// 이 공통 경계를 사용하면 동료의 습격뿐 아니라 도발·치유·단일기·광역기 모두 같은 규칙을 따릅니다.
        /// </summary>
        private void CloseSkillInspectionForAction()
        {
            choosingSkill = false;
            if (skillMenuPanel != null) skillMenuPanel.gameObject.SetActive(false);
            HideSkillDetailPopup();
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
            if (skill.EffectType == BattleSkillEffectType.AreaAllyHeal)
            {
                Formation friendlyFormation = currentActor.Side == BattleSide.Allies ? allies : enemies;
                bool hasRecoverableAlly = friendlyFormation.LivingMembers
                    .Any(ally => ally.CurrentHp < ally.MaxHp);
                if (!hasRecoverableAlly)
                {
                    // 메뉴를 닫기 전에 거절하므로 행동·턴·쿨타임을 전혀 소비하지 않고, 사용자는 다른
                    // 스킬을 바로 고를 수 있습니다. 전투불능자는 LivingMembers에 들어오지 않습니다.
                    messageText.text = "HP를 회복할 수 있는 살아 있는 아군이 없습니다. 행동은 소비되지 않았습니다.";
                    RebuildSkillMenu();
                    return;
                }
            }

            // 여기부터는 정보를 살펴보는 단계가 끝났습니다. 이후 스킬 종류와 관계없이 대상 선택과 연출이
            // 같은 넓은 전장 화면을 사용하도록 메뉴와 상세 팝업을 공통으로 닫습니다.
            CloseSkillInspectionForAction();
            if (skill.EffectType == BattleSkillEffectType.SingleAllyHeal)
            {
                BeginSingleAllyHealSelection(skill);
                return;
            }
            if (skill.EffectType == BattleSkillEffectType.AreaAllyHeal)
            {
                PlayHealingWave(skill);
                return;
            }
            if (skill.EffectType == BattleSkillEffectType.SingleRangedPhysicalAttack)
            {
                BeginSingleRangedAttackSelection(skill);
                return;
            }
            if (skill.EffectType == BattleSkillEffectType.SingleBeastPhysicalAttack)
            {
                BeginCompanionAssaultSelection(skill);
                return;
            }
            if (skill.EffectType == BattleSkillEffectType.SingleMagicAttackWithBurn)
            {
                BeginFireballSelection(skill);
                return;
            }
            if (skill.EffectType == BattleSkillEffectType.SingleMeleePhysicalAttackWithMomentumGain ||
                skill.EffectType == BattleSkillEffectType.SingleMeleePhysicalAttackConsumingMomentum)
            {
                BeginFighterMeleeSkillSelection(skill);
                return;
            }
            if (skill.EffectType == BattleSkillEffectType.AreaMeleePhysicalAttackWithMomentumGain)
            {
                PlayFighterWhirlwind(skill);
                return;
            }
            if (skill.EffectType == BattleSkillEffectType.AreaRangedPhysicalAttack)
            {
                PlaySharpshooterArrowRain(skill);
                return;
            }
            if (skill.EffectType == BattleSkillEffectType.AreaMagicAttackWithShock)
            {
                PlayMageThunderbolt(skill);
                return;
            }
            if (skill.EffectType == BattleSkillEffectType.SelfDamageReduction)
            {
                PlayMageGaiaWall(skill);
                return;
            }

            choosingSkill = false;
            skillMenuPanel.gameObject.SetActive(false);
            SetCancelButtonVisible(false);
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

        /// <summary>
        /// 공격은 TargetResolver가 적 진형·사거리·도발을 계산하지만, 단일 회복은 같은 편의 살아 있는
        /// 참가자 전체가 후보입니다. 따라서 공격 규칙을 억지로 바꾸지 않고 Formation.LivingMembers를
        /// 공용 대상 선택 UI에 전달합니다. 자신도 같은 Formation 구성원이므로 자연스럽게 포함됩니다.
        /// </summary>
        private void BeginSingleAllyHealSelection(BattleSkillDefinition skill)
        {
            Combatant actor = currentActor;
            Formation friendlyFormation = actor.Side == BattleSide.Allies ? allies : enemies;
            IReadOnlyList<Combatant> livingAllies = friendlyFormation.LivingMembers.ToArray();
            choosingSkill = false;
            skillMenuPanel.gameObject.SetActive(false);
            BeginTargetSelection(livingAllies, target => PlayHealingLight(actor, target, skill),
                "치유의 빛 대상을 선택하세요. 자신을 포함한 살아 있는 아군을 선택할 수 있습니다.", true);
        }

        /// <summary>
        /// 대상이 가득 찬 경우에는 안내 후 스킬 메뉴로 돌아가 행동을 소비하지 않습니다. 실제 회복은
        /// Radiant Heal의 peak 프레임 콜백에서 실행되고, 곧바로 RefreshCombatantViews를 호출하므로
        /// Combatant HP를 읽는 상단 Bar와 상세 팝업 숫자가 같은 프레임에 갱신됩니다.
        /// </summary>
        private void PlayHealingLight(Combatant actor, Combatant target, BattleSkillDefinition skill)
        {
            if (battleEnded || actionPlaying || actor == null || !actor.IsAlive || target == null || !target.IsAlive || target.Side != actor.Side)
            {
                ShowSkillMenu();
                messageText.text = "살아 있는 아군만 치유할 수 있습니다.";
                return;
            }
            if (target.CurrentHp >= target.MaxHp)
            {
                ShowSkillMenu();
                messageText.text = $"{target.DisplayName}은(는) 이미 HP가 가득 찼습니다. 행동은 소비되지 않았습니다.";
                return;
            }

            actionPlaying = true;
            SetCommandButtons(false);
            SetCancelButtonVisible(false);
            RefreshCombatantViews(null);
            if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);

            CombatantView targetView = combatantViews[target];
            if (actionPresenter == null) actionPresenter = gameObject.AddComponent<BattleActionPresenter>();
            bool executed = false;
            StartCoroutine(actionPresenter.PlayHealingEffect(
                targetView.ActionRoot,
                battleFont,
                LoadProjectileFrames("BattleSkillEffects/RadiantHeal"),
                .05f,
                7,
                new Vector2(96f, 96f),
                () =>
                {
                    executed = skillExecutor.ExecuteSingleAllyHeal(actor, target, skill, out int recoveredHp, out string result);
                    messageText.text = result;
                    return recoveredHp;
                },
                recoveredHp => RefreshCombatantViews(null),
                () =>
                {
                    actionPlaying = false;
                    if (executed) FinishCurrentAction();
                    else
                    {
                        string failureMessage = messageText.text;
                        ShowSkillMenu();
                        messageText.text = string.IsNullOrEmpty(failureMessage) ? "치유의 빛을 사용할 수 없습니다." : failureMessage;
                    }
                }));
        }

        /// <summary>
        /// 같은 편 Formation의 살아 있는 구성원을 배열로 고정한 뒤 중앙 파동 1회와 대상별 회복 이펙트를
        /// 함께 재생합니다. 특정 파티 슬롯이나 3명이라는 숫자를 사용하지 않으므로 Formation이 수용하는
        /// 인원이 늘어나도 대상 수만 자연스럽게 증가하며, 연출 시간은 대상 수와 관계없이 동일합니다.
        /// </summary>
        private void PlayHealingWave(BattleSkillDefinition skill)
        {
            Combatant actor = currentActor;
            Formation friendlyFormation = actor.Side == BattleSide.Allies ? allies : enemies;
            Combatant[] targets = friendlyFormation.LivingMembers.ToArray();
            if (!targets.Any(target => target.CurrentHp < target.MaxHp))
            {
                ShowSkillMenu();
                messageText.text = "HP를 회복할 수 있는 살아 있는 아군이 없습니다. 행동은 소비되지 않았습니다.";
                return;
            }

            actionPlaying = true;
            SetCommandButtons(false);
            SetCancelButtonVisible(false);
            RefreshCombatantViews(null);
            if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);

            CombatantView actorView = combatantViews[actor];
            RectTransform[] targetRects = targets.Select(target => combatantViews[target].ActionRoot).ToArray();
            if (actionPresenter == null) actionPresenter = gameObject.AddComponent<BattleActionPresenter>();
            bool executed = false;
            StartCoroutine(actionPresenter.PlayHealingWave(
                actorView.ActionRoot,
                actorView.SpriteImage,
                targetRects,
                battleFont,
                BattleGaiaWallVisuals.LoadFrames(),
                LoadProjectileFrames("BattleSkillEffects/RadiantHeal"),
                () =>
                {
                    executed = skillExecutor.ExecuteAreaAllyHeal(
                        actor, targets, skill, out IReadOnlyList<int> recovered, out string result);
                    messageText.text = result;
                    return recovered;
                },
                recovered => RefreshCombatantViews(null),
                () =>
                {
                    RestoreBattleIdle(actorView);
                    actionPlaying = false;
                    if (executed)
                    {
                        // 성공한 광역 회복이 모두 끝난 뒤에만 쿨타임을 등록합니다. 공용 저장소가 참가자와
                        // Skill ID를 함께 키로 삼고 자신의 행동 시작에만 감소시키는 기존 규칙을 그대로 씁니다.
                        skillExecutor.RegisterCooldownAfterSuccessfulUse(actor, skill);
                        FinishCurrentAction();
                    }
                    else
                    {
                        string failureMessage = messageText.text;
                        ShowSkillMenu();
                        messageText.text = string.IsNullOrEmpty(failureMessage)
                            ? "회복의 파동을 사용할 수 없습니다." : failureMessage;
                    }
                }));
        }

        /// <summary>
        /// 정조준 후보는 기존 원거리 물리 TargetResolver에 맡깁니다. Resolver는 후열 생존자가 하나라도
        /// 있으면 후열 목록만 반환하고, 후열이 모두 전투불능일 때만 전열을 반환합니다. 따라서 3명이나
        /// 향후 최대 6명 적 모두 같은 Formation 규칙을 사용하며 정조준 전용 열 판정을 중복 작성하지 않습니다.
        /// </summary>
        private void BeginSingleRangedAttackSelection(BattleSkillDefinition skill)
        {
            Combatant actor = currentActor;
            Formation opponents = actor.Side == BattleSide.Allies ? enemies : allies;
            IReadOnlyList<Combatant> targets = TargetResolver.ResolveHostileTargets(
                actor, opponents, TargetRangeType.RangedPhysical);
            choosingSkill = false;
            skillMenuPanel.gameObject.SetActive(false);
            BeginTargetSelection(targets, target => PlayAimedShot(actor, target, skill),
                "정조준 대상을 선택하세요. 후열이 살아 있으면 후열만 선택할 수 있습니다.", true);
        }

        /// <summary>
        /// 짧은 "정조준!" 강조 뒤 기존 golden_arrow Projectile을 발사합니다. 실제 피해 함수는 화살이
        /// 도착할 때 Presenter가 호출하므로 선택 순간에는 HP가 줄지 않습니다. 도착 콜백에서 전투 데이터를
        /// 갱신한 뒤 HUD와 상세 팝업이 같은 Combatant.CurrentHp를 다시 읽어 즉시 같은 값을 보여 줍니다.
        /// </summary>
        private void PlayAimedShot(Combatant actor, Combatant target, BattleSkillDefinition skill)
        {
            if (battleEnded || actionPlaying || actor == null || !actor.IsAlive || target == null || !target.IsAlive || target.Side == actor.Side)
            {
                ShowSkillMenu();
                messageText.text = "공격할 수 있는 살아 있는 적이 아닙니다.";
                return;
            }

            actionPlaying = true;
            SetCommandButtons(false);
            SetCancelButtonVisible(false);
            RefreshCombatantViews(null);
            if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);

            CombatantView actorView = combatantViews[actor];
            CombatantView targetView = combatantViews[target];
            if (actionPresenter == null) actionPresenter = gameObject.AddComponent<BattleActionPresenter>();
            bool executed = false;
            StartCoroutine(actionPresenter.PlaySkillEmphasis(
                actorView.ActionRoot,
                actorView.SpriteImage,
                battleFont,
                "정조준!",
                null,
                () => StartCoroutine(actionPresenter.PlayProjectileAttack(
                    actorView.ActionRoot,
                    targetView.ActionRoot,
                    targetView.SpriteImage,
                    battleFont,
                    LoadProjectileFrames("BattleProjectiles/GoldenArrow"),
                    .08f,
                    .42f,
                    new Vector2(52f, 52f),
                    true,
                    Color.white,
                    BattleProjectileStyle.Arrow,
                    .08f,
                    () =>
                    {
                        executed = skillExecutor.ExecuteSingleRangedPhysicalAttack(
                            actor, target, skill, out int damage, out string result);
                        messageText.text = result;
                        return damage;
                    },
                    damage => RefreshCombatantViews(null),
                    () =>
                    {
                        RestoreBattleIdle(actorView);
                        RestoreBattleIdle(targetView);
                        actionPlaying = false;
                        if (executed) FinishCurrentAction();
                        else
                        {
                            string failureMessage = messageText.text;
                            ShowSkillMenu();
                            messageText.text = string.IsNullOrEmpty(failureMessage) ? "정조준을 사용할 수 없습니다." : failureMessage;
                        }
                    }))));
        }

        /// <summary>
        /// 동료의 습격은 전열/후열을 자유롭게 고르므로 Magic의 자유 대상 범위만 재사용합니다. 다만 이 호출도
        /// 공용 TargetResolver를 통과하므로 사수에게 유효한 도발 강제 대상이 있으면 그 한 명만 반환됩니다.
        /// Wolf 자체는 대상 목록이나 Formation에 넣지 않습니다.
        /// </summary>
        private void BeginCompanionAssaultSelection(BattleSkillDefinition skill)
        {
            Combatant actor = currentActor;
            Formation opponents = actor.Side == BattleSide.Allies ? enemies : allies;
            IReadOnlyList<Combatant> targets = TargetResolver.ResolveHostileTargets(
                actor, opponents, TargetRangeType.Magic);
            choosingSkill = false;
            skillMenuPanel.gameObject.SetActive(false);
            BeginTargetSelection(targets, target => PlayCompanionAssault(actor, target, skill),
                "동료의 습격 대상을 선택하세요. 전열과 후열 모두 선택할 수 있으며 도발은 우선 적용됩니다.", true);
        }

        /// <summary>
        /// 파이어 볼은 전열과 후열을 자유롭게 고르는 단일 마법입니다. 공용 TargetResolver의 Magic 범위를
        /// 사용하면 자유 대상 규칙을 복사하지 않으면서도, 단일 적대 행동에 적용되는 기존 도발이 먼저 처리됩니다.
        /// </summary>
        private void BeginFireballSelection(BattleSkillDefinition skill)
        {
            Combatant actor = currentActor;
            Formation opponents = actor.Side == BattleSide.Allies ? enemies : allies;
            IReadOnlyList<Combatant> targets = TargetResolver.ResolveHostileTargets(
                actor, opponents, TargetRangeType.Magic);
            BeginTargetSelection(targets, target => PlayFireball(actor, target, skill),
                "파이어 볼 대상을 선택하세요. 전열과 후열 모두 선택할 수 있으며 도발은 우선 적용됩니다.", true);
        }

        /// <summary>
        /// 충전과 Projectile은 화면 연출만 담당하고, Warm Explosion index 4 콜백에서만 Executor를 호출합니다.
        /// 따라서 대상 선택이나 비행 중에는 HP·화상·쿨타임이 바뀌지 않으며 성공한 명중만 행동을 소비합니다.
        /// </summary>
        private void PlayFireball(Combatant actor, Combatant target, BattleSkillDefinition skill)
        {
            if (battleEnded || actionPlaying || actor == null || !actor.IsAlive || target == null ||
                !target.IsAlive || target.Side == actor.Side)
            {
                ShowSkillMenu();
                messageText.text = "공격할 수 있는 살아 있는 적이 아닙니다.";
                return;
            }

            actionPlaying = true;
            SetCommandButtons(false);
            SetCancelButtonVisible(false);
            RefreshCombatantViews(null);
            if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);

            CombatantView actorView = combatantViews[actor];
            CombatantView targetView = combatantViews[target];
            if (actionPresenter == null) actionPresenter = gameObject.AddComponent<BattleActionPresenter>();
            bool executed = false;
            StartCoroutine(actionPresenter.PlayFireballSkill(
                actorView.ActionRoot,
                targetView.ActionRoot,
                targetView.SpriteImage,
                battleFont,
                LoadProjectileFrames("BattleProjectiles/Fireball"),
                BattleFireballVisuals.LoadChargeFrames(),
                BattleFireballVisuals.LoadExplosionFrames(),
                () =>
                {
                    executed = skillExecutor.ExecuteSingleMagicAttackWithBurn(
                        actor, target, skill, out int damage, out string result);
                    messageText.text = result;
                    return damage;
                },
                damage => RefreshCombatantViews(null),
                () =>
                {
                    RestoreBattleIdle(actorView);
                    RestoreBattleIdle(targetView);
                    actionPlaying = false;
                    if (executed) FinishCurrentAction();
                    else
                    {
                        string failureMessage = messageText.text;
                        ShowSkillMenu();
                        messageText.text = string.IsNullOrEmpty(failureMessage)
                            ? "파이어 볼을 사용할 수 없습니다." : failureMessage;
                    }
                }));
        }

        /// <summary>
        /// 사수는 제자리에 남고, 장착 야수 데이터가 제공한 Run SpriteSheet만 별도 UI 오브젝트로 재생합니다.
        /// Wolf가 대상에 닿은 순간 Executor가 180% 피해를 한 번 적용하고, Wolf가 왼쪽 화면 밖으로 완전히
        /// 퇴장하고 피격 반응도 끝난 뒤에만 입력 잠금을 풀고 다음 턴으로 진행합니다.
        /// </summary>
        private void PlayCompanionAssault(Combatant actor, Combatant target, BattleSkillDefinition skill)
        {
            if (battleEnded || actionPlaying || actor == null || !actor.IsAlive || target == null || !target.IsAlive || target.Side == actor.Side)
            {
                ShowSkillMenu();
                messageText.text = "공격할 수 있는 살아 있는 적이 아닙니다.";
                return;
            }

            choosingSkill = false;
            skillMenuPanel.gameObject.SetActive(false);
            actionPlaying = true;
            SetCommandButtons(false);
            SetCancelButtonVisible(false);
            RefreshCombatantViews(null);
            if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);

            CombatantView actorView = combatantViews[actor];
            CombatantView targetView = combatantViews[target];
            BeastCompanionDefinition beast = BeastCompanionCatalog.GetEquippedOrDefault(actor);
            if (actionPresenter == null) actionPresenter = gameObject.AddComponent<BattleActionPresenter>();
            bool executed = false;
            StartCoroutine(actionPresenter.PlayBeastCompanionAssault(
                actorView.ActionRoot, targetView.ActionRoot, targetView.SpriteImage, battleFont, beast,
                () =>
                {
                    executed = skillExecutor.ExecuteSingleBeastPhysicalAttack(
                        actor, target, skill, out int damage, out string result);
                    messageText.text = result;
                    return damage;
                },
                damage => RefreshCombatantViews(null),
                () =>
                {
                    RestoreBattleIdle(actorView);
                    RestoreBattleIdle(targetView);
                    actionPlaying = false;
                    if (executed) FinishCurrentAction();
                    else
                    {
                        string failureMessage = messageText.text;
                        ShowSkillMenu();
                        messageText.text = string.IsNullOrEmpty(failureMessage)
                            ? "동료의 습격을 사용할 수 없습니다." : failureMessage;
                    }
                }));
        }

        /// <summary>
        /// 난도는 투사의 기본 근거리 사거리와 같은 TargetResolver 결과를 사용합니다. 따라서 전열 보호와
        /// 같은 열의 후열 개방 규칙을 스킬 코드에 복제하지 않고, Formation 변경도 기존 공격과 똑같이 반영됩니다.
        /// </summary>
        private void BeginFighterMeleeSkillSelection(BattleSkillDefinition skill)
        {
            Combatant actor = currentActor;
            Formation opponents = actor.Side == BattleSide.Allies ? enemies : allies;
            IReadOnlyList<Combatant> targets = TargetResolver.ResolveHostileTargets(
                actor, opponents, TargetRangeType.MeleePhysical);
            choosingSkill = false;
            skillMenuPanel.gameObject.SetActive(false);
            BeginTargetSelection(targets, target => PlayFighterMeleeSkill(actor, target, skill),
                $"{skill.DisplayName}(으)로 공격할 적을 선택하세요. 기존 근거리 공격 범위를 따릅니다.", true);
        }

        /// <summary>
        /// 기존 근거리 기본 공격 Presenter를 그대로 사용해 전진, 타격, 피격, 복귀 순서를 유지합니다.
        /// Executor는 타격 콜백에서만 피해와 기세 변경을 적용하므로 대상 선택 순간에는 HP와 자원이
        /// 바뀌지 않으며, 실패하면 행동을 소비하지 않고 스킬 메뉴로 돌아갑니다.
        /// </summary>
        private void PlayFighterMeleeSkill(Combatant actor, Combatant target, BattleSkillDefinition skill)
        {
            if (battleEnded || actionPlaying || actor == null || !actor.IsAlive || target == null ||
                !target.IsAlive || target.Side == actor.Side)
            {
                ShowSkillMenu();
                messageText.text = "공격할 수 있는 살아 있는 적이 아닙니다.";
                return;
            }

            actionPlaying = true;
            SetCommandButtons(false);
            SetCancelButtonVisible(false);
            RefreshCombatantViews(null);
            if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);

            CombatantView actorView = combatantViews[actor];
            CombatantView targetView = combatantViews[target];
            if (actionPresenter == null) actionPresenter = gameObject.AddComponent<BattleActionPresenter>();
            bool executed = false;
            StartCoroutine(actionPresenter.PlayMeleeAttack(
                actorView.ActionRoot,
                targetView.ActionRoot,
                targetView.SpriteImage,
                battleFont,
                () =>
                {
                    int damage;
                    string result;
                    if (skill.EffectType == BattleSkillEffectType.SingleMeleePhysicalAttackWithMomentumGain)
                    {
                        executed = skillExecutor.ExecuteSingleMeleePhysicalAttackWithMomentumGain(
                            actor, target, skill, out damage, out result);
                    }
                    else if (skill.EffectType == BattleSkillEffectType.SingleMeleePhysicalAttackConsumingMomentum)
                    {
                        executed = skillExecutor.ExecuteSingleMeleePhysicalAttackConsumingMomentum(
                            actor, target, skill, out damage, out _, out result);
                    }
                    else
                    {
                        damage = 0;
                        result = "아직 사용할 수 없습니다.";
                    }
                    messageText.text = result;
                    return damage;
                },
                damage => RefreshCombatantViews(null),
                () =>
                {
                    RestoreBattleIdle(actorView);
                    RestoreBattleIdle(targetView);
                    actionPlaying = false;
                    if (executed) FinishCurrentAction();
                    else
                    {
                        string failureMessage = messageText.text;
                        ShowSkillMenu();
                        messageText.text = string.IsNullOrEmpty(failureMessage)
                            ? $"{skill.DisplayName}을(를) 사용할 수 없습니다." : failureMessage;
                    }
                }));
        }

        /// <summary>
        /// 광역기는 기본 공격용 TargetResolver가 아니라 스킬 정의의 TargetRange를 공용 스킬 대상 해석기에
        /// 전달합니다. 따라서 Controller가 "회오리 베기"라는 이름을 비교하지 않으며, 향후 화살비와
        /// 썬더볼트도 같은 흐름에서 후열 전체·적 전체 범위를 데이터로 선택할 수 있습니다.
        /// 목록은 연출 시작 전에 고정하여 계산 대상과 화면 대상의 순서가 일치하게 유지합니다.
        /// </summary>
        private void PlayFighterWhirlwind(BattleSkillDefinition skill)
        {
            Combatant actor = currentActor;
            Formation opponents = actor.Side == BattleSide.Allies ? enemies : allies;
            Combatant[] targets = BattleSkillTargetResolver.ResolveHostileAreaTargets(skill, opponents).ToArray();
            if (targets.Length == 0)
            {
                // 기본 근거리 공격은 전열이 비면 일부 후열을 공격할 수 있지만 회오리 베기의 공간은 전열로
                // 고정됩니다. 여기서 스킬 메뉴를 유지하므로 안내만 보이고 행동과 턴은 소비하지 않습니다.
                messageText.text = "회오리 베기로 공격할 전열 적이 없습니다.";
                RebuildSkillMenu();
                return;
            }

            choosingSkill = false;
            skillMenuPanel.gameObject.SetActive(false);
            actionPlaying = true;
            SetCommandButtons(false);
            SetCancelButtonVisible(false);
            RefreshCombatantViews(null);
            if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);

            CombatantView actorView = combatantViews[actor];
            CombatantView[] targetViews = targets.Select(target => combatantViews[target]).ToArray();
            RectTransform[] targetRects = targetViews.Select(view => view.ActionRoot).ToArray();
            Image[] targetSprites = targetViews.Select(view => view.SpriteImage).ToArray();
            if (actionPresenter == null) actionPresenter = gameObject.AddComponent<BattleActionPresenter>();
            bool executed = false;

            StartCoroutine(actionPresenter.PlayWhirlwindAttack(
                actorView.ActionRoot,
                actorView.SpriteImage,
                targetRects,
                targetSprites,
                battleFont,
                () =>
                {
                    executed = skillExecutor.ExecuteAreaMeleePhysicalAttackWithMomentumGain(
                        actor, targets, skill, out IReadOnlyList<int> damages, out _, out string result);
                    messageText.text = result;
                    return damages;
                },
                damages => RefreshCombatantViews(null),
                () =>
                {
                    RestoreBattleIdle(actorView);
                    foreach (CombatantView targetView in targetViews) RestoreBattleIdle(targetView);
                    actionPlaying = false;
                    if (executed) FinishCurrentAction();
                    else
                    {
                        string failureMessage = messageText.text;
                        ShowSkillMenu();
                        messageText.text = string.IsNullOrEmpty(failureMessage)
                            ? "회오리 베기를 사용할 수 없습니다." : failureMessage;
                    }
                }));
        }

        /// <summary>
        /// 화살비는 기본 사수 공격의 TargetResolver를 호출하지 않고 스킬 데이터의 EnemyRearRowAll을
        /// 해석합니다. 기본 원거리 공격은 후열이 없으면 전열로 전환되지만, 화살비는 "후열이라는 공간"을
        /// 공격하는 기술이므로 후열이 비었다고 다른 행을 대신 공격하지 않습니다.
        /// </summary>
        private void PlaySharpshooterArrowRain(BattleSkillDefinition skill)
        {
            Combatant actor = currentActor;
            Formation opponents = actor.Side == BattleSide.Allies ? enemies : allies;
            Combatant[] targets = BattleSkillTargetResolver.ResolveHostileAreaTargets(skill, opponents).ToArray();
            if (targets.Length == 0)
            {
                messageText.text = "화살비로 공격할 후열 적이 없습니다.";
                RebuildSkillMenu();
                return;
            }

            choosingSkill = false;
            skillMenuPanel.gameObject.SetActive(false);
            actionPlaying = true;
            SetCommandButtons(false);
            SetCancelButtonVisible(false);
            RefreshCombatantViews(null);
            if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);

            CombatantView actorView = combatantViews[actor];
            CombatantView[] targetViews = targets.Select(target => combatantViews[target]).ToArray();
            RectTransform[] targetRects = targetViews.Select(view => view.ActionRoot).ToArray();
            Image[] targetSprites = targetViews.Select(view => view.SpriteImage).ToArray();
            if (actionPresenter == null) actionPresenter = gameObject.AddComponent<BattleActionPresenter>();
            bool executed = false;

            StartCoroutine(actionPresenter.PlayProjectileVolleyAttack(
                actorView.ActionRoot,
                actorView.SpriteImage,
                targetRects,
                targetSprites,
                battleFont,
                LoadProjectileFrames("BattleProjectiles/GoldenArrow"),
                .08f,
                () =>
                {
                    // 여러 화살 인스턴스는 시각 효과일 뿐입니다. Executor를 공유 타격 시점에 한 번만 호출해
                    // 각 후열 대상이 화살 개수와 관계없이 정확히 120% 피해를 한 번 받게 합니다.
                    executed = skillExecutor.ExecuteAreaRangedPhysicalAttack(
                        actor, targets, skill, out IReadOnlyList<int> damages, out string result);
                    messageText.text = result;
                    return damages;
                },
                damages => RefreshCombatantViews(null),
                () =>
                {
                    RestoreBattleIdle(actorView);
                    foreach (CombatantView targetView in targetViews) RestoreBattleIdle(targetView);
                    actionPlaying = false;
                    if (executed)
                    {
                        // 낙하·피해·모든 피격 반응이 끝난 이 시점이 화살비 사용 성공의 경계입니다.
                        // 여기서만 2턴을 등록하므로 후열 0명 거절에는 쿨타임이 없고, HUD도 실제 계산을
                        // 직접 만들지 않고 BattleSkillCooldowns에 저장된 결과를 다음 갱신에서 읽습니다.
                        skillExecutor.RegisterCooldownAfterSuccessfulUse(actor, skill);
                        FinishCurrentAction();
                    }
                    else
                    {
                        string failureMessage = messageText.text;
                        ShowSkillMenu();
                        messageText.text = string.IsNullOrEmpty(failureMessage)
                            ? "화살비를 사용할 수 없습니다." : failureMessage;
                    }
                }));
        }

        /// <summary>
        /// EnemyAll 데이터를 공용 광역 해석기에 전달하여 살아 있는 전열·후열 적을 한 목록으로 고정합니다.
        /// 단일 적대 행동용 TargetResolver를 거치지 않으므로 도발자가 있어도 전체 범위를 그대로 공격합니다.
        /// 이는 도발을 예외 처리한 것이 아니라 "한 명을 고르는 기술만 강제 대상 적용"이라는 기존 규칙입니다.
        /// </summary>
        private void PlayMageThunderbolt(BattleSkillDefinition skill)
        {
            Combatant actor = currentActor;
            Formation opponents = actor.Side == BattleSide.Allies ? enemies : allies;
            Combatant[] targets = BattleSkillTargetResolver.ResolveHostileAreaTargets(skill, opponents).ToArray();
            if (targets.Length == 0)
            {
                messageText.text = "썬더볼트로 공격할 살아 있는 적이 없습니다.";
                RebuildSkillMenu();
                return;
            }

            choosingSkill = false;
            skillMenuPanel.gameObject.SetActive(false);
            actionPlaying = true;
            SetCommandButtons(false);
            SetCancelButtonVisible(false);
            RefreshCombatantViews(null);
            if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);

            CombatantView actorView = combatantViews[actor];
            CombatantView[] targetViews = targets.Select(target => combatantViews[target]).ToArray();
            RectTransform[] targetRects = targetViews.Select(view => view.ActionRoot).ToArray();
            Image[] targetSprites = targetViews.Select(view => view.SpriteImage).ToArray();
            if (actionPresenter == null) actionPresenter = gameObject.AddComponent<BattleActionPresenter>();
            bool executed = false;

            StartCoroutine(actionPresenter.PlayThunderboltAttack(
                actorView.ActionRoot, targetRects, targetSprites, battleFont,
                BattleThunderboltVisuals.LoadImpactFrames(),
                () =>
                {
                    // electric-impact가 가장 강한 index 1에 도달한 한 순간에 Executor를 한 번만 호출합니다.
                    // 모든 대상의 HP와 감전이 같은 콜백에서 바뀌므로 순차 타격처럼 턴 상태가 끼어들지 않습니다.
                    executed = skillExecutor.ExecuteAreaMagicAttackWithShock(
                        actor, targets, skill, out IReadOnlyList<int> damages, out string result);
                    messageText.text = result;
                    return damages;
                },
                damages => RefreshCombatantViews(null),
                () =>
                {
                    RestoreBattleIdle(actorView);
                    foreach (CombatantView targetView in targetViews) RestoreBattleIdle(targetView);
                    actionPlaying = false;
                    if (executed)
                    {
                        skillExecutor.RegisterCooldownAfterSuccessfulUse(actor, skill);
                        FinishCurrentAction();
                    }
                    else
                    {
                        string failureMessage = messageText.text;
                        ShowSkillMenu();
                        messageText.text = string.IsNullOrEmpty(failureMessage)
                            ? "썬더볼트를 사용할 수 없습니다." : failureMessage;
                    }
                }));
        }

        /// <summary>
        /// 자기 보호 스킬은 대상 선택 화면이나 TargetResolver가 필요하지 않습니다. 따라서 도발·Formation을
        /// 건드리지 않고 현재 행동 중인 마도사에게만 상태를 적용하며, VFX가 끝날 때까지 입력을 잠급니다.
        /// </summary>
        private void PlayMageGaiaWall(BattleSkillDefinition skill)
        {
            Combatant actor = currentActor;
            if (battleEnded || actionPlaying || actor == null || !actor.IsAlive) return;

            choosingSkill = false;
            skillMenuPanel.gameObject.SetActive(false);
            actionPlaying = true;
            SetCommandButtons(false);
            SetCancelButtonVisible(false);
            RefreshCombatantViews(null);
            if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);

            CombatantView actorView = combatantViews[actor];
            if (actionPresenter == null) actionPresenter = gameObject.AddComponent<BattleActionPresenter>();
            bool executed = false;
            StartCoroutine(actionPresenter.PlayGaiaWall(
                actorView.ActionRoot, battleFont, BattleGaiaWallVisuals.LoadFrames(),
                () =>
                {
                    executed = skillExecutor.ExecuteSelfDamageReduction(actor, skill, out string result);
                    messageText.text = result;
                    RefreshCombatantViews(null);
                },
                () =>
                {
                    RestoreBattleIdle(actorView);
                    actionPlaying = false;
                    if (executed)
                    {
                        // 성공한 연출 완료 뒤에만 4턴 쿨타임을 등록합니다. 현재 행동 종료는 상태 저장소가
                        // 건너뛰므로 가이아 2가 즉시 1로 줄지 않고 다음 두 행동을 온전히 보호합니다.
                        skillExecutor.RegisterCooldownAfterSuccessfulUse(actor, skill);
                        FinishCurrentAction();
                    }
                    else
                    {
                        string failureMessage = messageText.text;
                        ShowSkillMenu();
                        messageText.text = string.IsNullOrEmpty(failureMessage)
                            ? "가이아 웰을 사용할 수 없습니다." : failureMessage;
                    }
                }));
        }

        private void Defend()
        {
            if (actionPlaying) return;
            if (statusEffects.GetGaiaWallRemaining(currentActor) > 0)
            {
                // 가이아 웰은 공용 방어보다 강한 마도사 전용 생존기입니다. 두 효과를 겹치면 생존력이
                // 의도보다 커지고 규칙도 읽기 어려워지므로 입력만 거절하고 행동·턴은 그대로 남깁니다.
                messageText.text = "더 강한 방어 효과가 이미 적용 중이라 방어를 사용할 수 없습니다.";
                return;
            }
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
            // 감전은 행동자의 "주는 피해"를 줄입니다. 기본 공격도 스킬과 같은 상태 저장소를 통과해야
            // 다음 행동 1회 감소가 공격 종류와 관계없이 일관되게 적용됩니다.
            Func<int> applyImpact = () => statusEffects.ApplyIncomingDamage(target,
                statusEffects.ModifyOutgoingDamage(actor, actor.Attack));
            Action<int> onImpact = damage =>
            {
                messageText.text = $"{actor.DisplayName}의 공격! {target.DisplayName}에게 {damage} 피해.";
                RefreshCombatantViews(null);
            };
            Action onComplete = () =>
            {
                actorView.MonsterAnimation?.StopAttackAndReturnToIdle();
                RestoreBattleIdle(actorView);
                RestoreBattleIdle(targetView);
                actionPlaying = false;
                FinishCurrentAction();
            };

            // 독침벌처럼 Attack 시트를 가진 몬스터만 실제 공격 프레임으로 전환합니다.
            // 피해 계산과 이동 연출은 기존 Presenter가 그대로 담당하므로 독 효과를 미리 만들지 않습니다.
            actorView.MonsterAnimation?.PlayAttack();

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
            // 행동 완료 뒤 기본 명령으로 돌아갈 때 이전 스킬의 Hover/포커스 정보가 되살아나지 않게 합니다.
            // 다음에 사용자가 스킬 메뉴를 다시 열고 버튼에 새로 포커스할 때만 설명이 표시됩니다.
            HideSkillDetailPopup();
            Combatant completedActor = currentActor;
            completedActor?.CompleteAction();
            // 감전은 공격 여부가 아니라 행동 기회를 약화시키는 상태이므로 방어·회복 행동도 여기서 소비합니다.
            // 피해 계산과 광역 타격이 모두 끝난 다음 제거해야 해당 행동의 모든 주는 피해가 15% 감소합니다.
            statusEffects.CompleteActorAction(completedActor);
            statusEffects.RemoveInvalidPersistentEffects(AllCombatants);
            SetCommandButtons(false);
            // 화상은 "대상 행동 종료 시" 피해이므로 CompleteAction 직후 확인합니다. 작은 불꽃과 피격이
            // 끝나기 전에는 actionPlaying을 유지해 입력과 정보 팝업이 다음 턴보다 먼저 열리지 않게 합니다.
            if (completedActor != null && statusEffects.HasActiveBurn(completedActor) && combatantViews.TryGetValue(completedActor, out CombatantView burnedView))
            {
                actionPlaying = true;
                RefreshCombatantViews(null);
                if (actionPresenter == null) actionPresenter = gameObject.AddComponent<BattleActionPresenter>();
                StartCoroutine(actionPresenter.PlayBurnTick(
                    burnedView.ActionRoot,
                    burnedView.SpriteImage,
                    battleFont,
                    LoadProjectileFrames("BattleProjectiles/Fireball"),
                    () => statusEffects.ApplyBurnTickAtActionEnd(completedActor, out _),
                    damage =>
                    {
                        int remaining = statusEffects.GetBurnRemaining(completedActor);
                        messageText.text = $"{completedActor.DisplayName}의 화상 피해 {damage}. 남은 화상 {remaining}회.";
                        statusEffects.RemoveInvalidPersistentEffects(AllCombatants);
                        RefreshCombatantViews(null);
                    },
                    () =>
                    {
                        RestoreBattleIdle(burnedView);
                        actionPlaying = false;
                        RefreshCombatantViews(null);
                        StartCoroutine(AdvanceAfterDelay());
                    }));
                return;
            }

            RefreshCombatantViews(null);
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
            // 모든 Battle Action은 actionPlaying을 켠 뒤 전투 화면을 갱신합니다. 따라서 이 한 경계에서
            // 두 종류 정보 팝업을 함께 닫으면 Wolf뿐 아니라 기본 공격·치유·Projectile·광역 VFX도 보호됩니다.
            if (actionPlaying) SuppressInformationPopupsDuringAction();
            statusEffects.RemoveInvalidPersistentEffects(AllCombatants);
            attackable = ReconcileTargetSelection(attackable);
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
                // 평상시 회색 발판은 완전히 숨기고, 대상 선택 중 공격 가능한 참가자에게만 얇은 금색 선을 보여 줍니다.
                view.GroundMarker.gameObject.SetActive(canAttack && combatant.IsAlive);
                view.GroundMarker.color = new Color(1f, .78f, .28f, .92f);
                view.TargetArrow.gameObject.SetActive(false);
                view.TurnMarker.gameObject.SetActive(combatant == currentActor && combatant.IsAlive);
                combatantJobs.TryGetValue(combatant, out JobDefinition statusJob);
                participantSetups.TryGetValue(combatant, out BattleParticipantSetup statusSetup);
                BattleCombatantStatusViewModel statusModel = BattleCombatantStatusViewModelFactory.Create(
                    combatant, statusJob, statusSetup, skillCooldowns, fighterResources, statusEffects);
                RefreshHpRow(combatant, statusModel);
            }
            RefreshHpRowHighlights(focusedCombatant ?? hoveredCombatant);
            if (!actionPlaying && detailCombatant != null) ShowDetailPopup(detailCombatant);
        }

        /// <summary>
        /// 대상 선택 도중 HP가 0이 된 참가자를 후보와 포커스에서 제거합니다.
        /// 살아 있는 다음 후보가 있으면 키보드 포커스를 옮기고, 없으면 선택 단계만 종료합니다.
        /// 이는 유효 대상 판정을 새로 계산하는 기능이 아니라 이미 계산된 후보에서 전투불능 표시를 정리하는 UI 안전장치입니다.
        /// </summary>
        private IReadOnlyList<Combatant> ReconcileTargetSelection(IReadOnlyList<Combatant> attackable)
        {
            if (!choosingTarget || attackable == null) return attackable;

            List<Combatant> livingTargets = attackable.Where(combatant => combatant != null && combatant.IsAlive).ToList();
            selectableTargets = livingTargets;
            if (livingTargets.Count == 0)
            {
                bool returnToSkillMenu = targetSelectionReturnsToSkillMenu;
                choosingTarget = false;
                targetSelectionReturnsToSkillMenu = false;
                targetSelectedAction = null;
                SetCancelButtonVisible(false);
                if (returnToSkillMenu)
                {
                    ShowSkillMenu();
                    messageText.text = "현재 치유할 수 있는 살아 있는 아군이 없습니다.";
                    return null;
                }
                SetCommandButtons(true);
                messageText.text = "현재 지정할 수 있는 대상이 없습니다.";
                if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(attackButton.gameObject);
                return null;
            }

            if (focusedCombatant != null && !livingTargets.Contains(focusedCombatant))
            {
                focusedCombatant = null;
                if (EventSystem.current != null)
                    EventSystem.current.SetSelectedGameObject(combatantViews[livingTargets[0]].HitArea.gameObject);
            }
            return livingTargets;
        }

        /// <summary>
        /// Combatant의 현재 HP를 고정 목록 행에 반영합니다. 전투 로직은 CurrentHp만 변경하고,
        /// 이 UI 메서드가 실제 비율을 0~1 범위로 바꿔 Bar의 가로 길이를 줄입니다.
        /// </summary>
        private void RefreshHpRow(Combatant combatant, BattleCombatantStatusViewModel statusModel)
        {
            if (!combatantHpRows.TryGetValue(combatant, out CombatantHpRow row)) return;
            row.Name.text = combatant.DisplayName + (combatant.IsAlive ? string.Empty : " [전투불능]");
            float healthRatio = combatant.MaxHp <= 0 ? 0f : Mathf.Clamp01((float)combatant.CurrentHp / combatant.MaxHp);
            RectTransform hpFillRect = row.HpFill.rectTransform;
            hpFillRect.anchorMin = Vector2.zero;
            hpFillRect.anchorMax = new Vector2(healthRatio, 1f);
            hpFillRect.offsetMin = Vector2.zero;
            hpFillRect.offsetMax = Vector2.zero;
            row.HpFill.color = healthRatio <= .3f
                ? new Color(.82f, .28f, .24f, 1f)
                : new Color(.25f, .72f, .46f, 1f);

            if (!combatant.IsAlive)
            {
                // 전투불능은 하나의 결과 상태이므로 도발·방어·행동 중·재사용 아이콘을 모두 끕니다.
                // 실제 Combatant 상태와 화면이 어긋나지 않도록 별도의 회색 결과 텍스트만 남깁니다.
                HideStatusBadges(row);
                row.FallenStatus.text = "전투불능";
                row.FallenStatus.color = new Color(.62f, .65f, .7f, 1f);
                row.FallenStatus.gameObject.SetActive(true);
                return;
            }

            row.FallenStatus.gameObject.SetActive(false);
            // HUD는 전투 값을 변경하지 않고 읽기 전용 ViewModel의 결과에 Sprite와 짧은 글자를 붙입니다.
            // 아이콘만으로 뜻을 전달하지 않도록 한글을 함께 두며, 상세 팝업은 기존 텍스트 중심 설명을 유지합니다.
            List<(string IconId, string Label)> summaries = new List<(string, string)>();
            if (combatant == currentActor) summaries.Add((BattleUiIconCatalog.Acting, "행동 중"));
            foreach (BattleStatusMarker marker in statusModel.Markers)
            {
                string iconId = marker.Id == "defend" ? BattleUiIconCatalog.Defend
                    : marker.Id == "taunt" ? BattleUiIconCatalog.Taunt
                    : marker.Id == "fighter.momentum" ? BattleUiIconCatalog.FighterMomentum
                    : marker.Id == "burn" ? BattleUiIconCatalog.Burn
                    : marker.Id == "shock" ? BattleUiIconCatalog.Shock
                    : marker.Id == "gaia" ? BattleUiIconCatalog.GaiaWall : null;
                summaries.Add((iconId, marker.DisplayText));
            }
            // ViewModel이 계산된 남은 턴과 총 턴을 함께 주므로 HUD는 숫자를 바꾸지 않고 그림만 고릅니다.
            // Sprite 파일이 빠진 경우 RefreshStatusBadges가 아이콘만 숨기고 같은 재사용 텍스트를 유지합니다.
            summaries.AddRange(statusModel.Cooldowns.Select(cooldown =>
                (BattleUiIconCatalog.GetCooldownIconId(cooldown.RemainingTurns, cooldown.TotalTurns), cooldown.DisplayText)));
            RefreshStatusBadges(row, summaries);
        }

        /// <summary>
        /// 상태 자료와 미리 만든 배지 자리를 순서대로 연결합니다. Sprite가 없으면 해당 Image만 숨기고
        /// 텍스트 폭을 넓혀 표시하므로, 외부 에셋 누락이 전투 입력이나 상태 판정에 영향을 주지 않습니다.
        /// </summary>
        private void RefreshStatusBadges(CombatantHpRow row, IReadOnlyList<(string IconId, string Label)> summaries)
        {
            for (int index = 0; index < row.StatusBadges.Count; index++)
            {
                BattleHudStatusBadge badge = row.StatusBadges[index];
                if (index >= summaries.Count)
                {
                    badge.Root.SetActive(false);
                    continue;
                }

                (string iconId, string label) = summaries[index];
                Sprite sprite = BattleUiIconCatalog.Load(iconId);
                badge.Icon.sprite = sprite;
                badge.Icon.gameObject.SetActive(sprite != null);
                badge.Label.text = label;
                SetRect(badge.Label.rectTransform, sprite == null ? new Vector2(.5f, .5f) : new Vector2(.62f, .5f),
                    sprite == null ? new Vector2(64, 13) : new Vector2(50, 13));
                badge.Root.SetActive(true);
            }
        }

        private static void HideStatusBadges(CombatantHpRow row)
        {
            foreach (BattleHudStatusBadge badge in row.StatusBadges) badge.Root.SetActive(false);
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
            if (!enabled || defendButton == null) return;

            bool gaiaBlocksDefend = statusEffects.GetGaiaWallRemaining(currentActor) > 0;
            // interactable=false로 만들면 마우스 클릭과 키보드 Submit이 모두 사라져 차단 이유를 안내할 수
            // 없습니다. 입력은 Defend()까지 전달하되 색상을 비활성처럼 바꿔 "사용 불가"를 미리 보여 줍니다.
            ColorBlock colors = ColorBlock.defaultColorBlock;
            if (gaiaBlocksDefend)
            {
                Color unavailable = new Color(.42f, .45f, .5f, 1f);
                colors.normalColor = unavailable;
                colors.highlightedColor = unavailable;
                colors.selectedColor = unavailable;
                colors.pressedColor = new Color(.36f, .38f, .42f, 1f);
            }
            defendButton.colors = colors;
        }

        private Button MakeCommandButton(Transform parent, string name, string iconId, string label, Font font, Vector2 anchor, Action action)
        {
            GameObject obj = new GameObject(name, typeof(Image), typeof(Button), typeof(Outline)); obj.transform.SetParent(parent, false); SetRect(obj.GetComponent<RectTransform>(), anchor, new Vector2(170, 40));
            Image image = obj.GetComponent<Image>(); image.color = new Color(.12f, .32f, .5f, 1f);
            Button button = obj.GetComponent<Button>(); button.targetGraphic = image; button.onClick.AddListener(() => action()); button.colors = ColorBlock.defaultColorBlock;
            Outline outline = obj.GetComponent<Outline>(); outline.effectColor = gold; outline.effectDistance = new Vector2(2, -2);
            // 실제 Kenney Sprite를 텍스트보다 작은 18px 보조 요소로 왼쪽에 둡니다. preserveAspect를 켜서
            // 검이나 방패의 원본 비율을 유지하고, 기존 170×40 클릭 영역은 키우지 않습니다.
            MakeSpriteIcon(obj.transform, "Icon", BattleUiIconCatalog.Load(iconId), new Vector2(.22f, .5f), new Vector2(18, 18));
            Text text = MakeText(obj.transform, "Label", label, font, 18, new Vector2(.62f, .5f), new Vector2(112, 34)); text.fontStyle = FontStyle.Bold;
            commandButtons.Add(button); return button;
        }

        /// <summary>메인 명령과 구분되는 작은 보조 버튼을 만듭니다. 취소는 전투 행동이 아니라 UI 단계만 되돌립니다.</summary>
        private Button MakeAuxiliaryButton(Transform parent, string name, string label, Font font, Vector2 anchor, string iconId, Action action)
        {
            GameObject obj = new GameObject(name, typeof(Image), typeof(Button), typeof(Outline));
            obj.transform.SetParent(parent, false);
            SetRect(obj.GetComponent<RectTransform>(), anchor, new Vector2(118, 36));
            Image image = obj.GetComponent<Image>();
            image.color = new Color(.22f, .25f, .3f, 1f);
            Button button = obj.GetComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(() => action());
            Outline outline = obj.GetComponent<Outline>();
            outline.effectColor = new Color(.65f, .72f, .8f, 1f);
            outline.effectDistance = new Vector2(1, -1);
            MakeSpriteIcon(obj.transform, "Icon", BattleUiIconCatalog.Load(iconId), new Vector2(.2f, .5f), new Vector2(16, 16));
            Text text = MakeText(obj.transform, "Label", label, font, 16, new Vector2(.62f, .5f), new Vector2(76, 30));
            text.fontStyle = FontStyle.Bold;
            return button;
        }

        /// <summary>
        /// Unity UI Image에 외부 Sprite를 연결하는 공통 도우미입니다. Sprite가 없으면 빈 사각형을 보여 주지
        /// 않도록 Image만 숨기며, 한글 Label은 별도 오브젝트라 그대로 남습니다.
        /// </summary>
        private Image MakeSpriteIcon(Transform parent, string name, Sprite sprite, Vector2 anchor, Vector2 size)
        {
            GameObject obj = new GameObject(name, typeof(Image));
            obj.transform.SetParent(parent, false);
            Image image = obj.GetComponent<Image>();
            image.sprite = sprite;
            image.color = focusGold;
            image.preserveAspect = true;
            image.raycastTarget = false;
            SetRect(image.rectTransform, anchor, size);
            image.gameObject.SetActive(sprite != null);
            return image;
        }

        /// <summary>
        /// BattleSkillDefinition이 가진 아이콘 ID를 실제 Sprite로 바꾸어 스킬 이름 왼쪽에 놓습니다.
        /// 이 메서드는 "도발인지 정조준인지"를 판단하지 않습니다. 데이터가 전달한 ID만 읽기 때문에
        /// 새 직업 스킬이 늘어나도 메뉴 배치 코드를 계속 수정하지 않아도 됩니다.
        /// Sprite를 찾지 못하면 아이콘만 숨기고 한글 이름을 가운데 표시해 조작 기능은 유지합니다.
        /// </summary>
        private Button MakeSkillMenuButton(Transform parent, string name, string label, string iconId,
            Vector2 anchor, Action action, BattleSkillDefinition skill = null)
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
            Sprite iconSprite = BattleUiIconCatalog.Load(iconId);
            bool hasIcon = iconSprite != null;
            if (hasIcon)
            {
                // 18px 아이콘은 현재 225x48 버튼 안에서 보조 정보로 읽히면서도 버튼 크기를 늘리지 않습니다.
                MakeSpriteIcon(obj.transform, "SkillIcon", iconSprite, new Vector2(.14f, .5f), new Vector2(18, 18));
            }
            Text text = MakeText(obj.transform, "Label", label, battleFont, 15,
                hasIcon ? new Vector2(.6f, .5f) : Vector2.one * .5f,
                hasIcon ? new Vector2(174, 44) : new Vector2(215, 44));
            text.fontStyle = FontStyle.Bold;
            if (skill != null)
            {
                // PointerEnter/Exit은 마우스, Select/Deselect는 키보드·게임패드 포커스입니다.
                // 어느 입력이든 같은 BattleSkillDefinition을 전달하므로 UI에 스킬 이름 비교가 생기지 않습니다.
                EventTrigger trigger = obj.AddComponent<EventTrigger>();
                AddTrigger(trigger, EventTriggerType.PointerEnter, _ => OnSkillPointerEnter(skill));
                AddTrigger(trigger, EventTriggerType.PointerExit, _ => OnSkillPointerExit(skill));
                AddTrigger(trigger, EventTriggerType.Select, _ => OnSkillFocused(skill));
                AddTrigger(trigger, EventTriggerType.Deselect, _ => OnSkillFocusLost(skill));
            }
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
