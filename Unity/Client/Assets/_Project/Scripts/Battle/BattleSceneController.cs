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
    /// Battle Scene의 1차 전투 흐름과 남색·금색 UI를 구성합니다.
    /// 전투 규칙은 BattleCore에 두고 이 클래스는 화면 표시와 플레이어 명령 연결만 담당합니다.
    /// </summary>
    public sealed class BattleSceneController : MonoBehaviour
    {
        private sealed class SlotView
        {
            public Button Button;
            public Image Background;
            public Text Label;
        }

        private readonly List<Selectable> commandButtons = new List<Selectable>();
        private readonly Dictionary<Combatant, SlotView> slotViews = new Dictionary<Combatant, SlotView>();
        private readonly Color navy = new Color(.018f, .03f, .06f, 1f);
        private readonly Color panel = new Color(.055f, .08f, .13f, .97f);
        private readonly Color gold = new Color(.88f, .7f, .32f, 1f);
        private readonly Color available = new Color(.12f, .35f, .32f, 1f);
        private readonly Color unavailable = new Color(.22f, .24f, .29f, 1f);
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
        private bool choosingTarget;
        private bool battleEnded;

        private IEnumerable<Combatant> AllCombatants => allies.Members.Concat(enemies.Members);

        private void Awake()
        {
            CreateParticipants();
            CreateEventSystem();
            CreateInterface();
            turnOrder = new TurnOrderQueue();
            turnOrder.Build(AllCombatants);
            AdvanceTurn();
        }

        private void Update()
        {
            if (Keyboard.current == null || battleEnded) return;
            if (Keyboard.current.escapeKey.wasPressedThisFrame && choosingTarget)
            {
                choosingTarget = false;
                SetCommandButtons(true);
                RefreshSlotViews(null);
                messageText.text = "행동을 선택하세요.";
                EventSystem.current.SetSelectedGameObject(attackButton.gameObject);
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
            TargetRangeType playerRange = GameSessionData.SelectedJobId == "sharpshooter" ? TargetRangeType.RangedPhysical : GameSessionData.SelectedJobId == "mage" ? TargetRangeType.Magic : TargetRangeType.MeleePhysical;

            allies = new Formation(BattleSide.Allies);
            enemies = new Formation(BattleSide.Enemies);
            // HP와 공격력 환산은 전투 흐름 검증용 임시값이며 최종 밸런스 데이터가 아닙니다.
            allies.Place(new Combatant("player", playerName, BattleSide.Allies, new FormationSlot(FormationRow.Front, 1), 80 + health * 4, 12 + Math.Max(0, strength - 10) * 2, agility, 0, playerRange, true));

            MonsterDefinition monster = BattleEncounterContext.Monster ?? Resources.LoadAll<MonsterDefinition>("MonsterDefinitions").FirstOrDefault();
            string monsterId = monster == null ? "grass_slime" : monster.MonsterId;
            string monsterName = monster == null ? "초원 슬라임" : monster.DisplayName;
            enemies.Place(new Combatant(monsterId, monsterName, BattleSide.Enemies, new FormationSlot(FormationRow.Front, 1), 55, 10, 11, 0, TargetRangeType.MeleePhysical, false));
        }

        private void CreateInterface()
        {
            CreateCameraIfMissing();
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            GameObject canvasObject = new GameObject("BattleCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas canvas = canvasObject.GetComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; scaler.referenceResolution = new Vector2(1280, 720); scaler.matchWidthOrHeight = .5f;
            Image background = MakeImage(canvasObject.transform, "Background", navy); Stretch(background.rectTransform);
            Text title = MakeText(canvasObject.transform, "Title", "BATTLE", font, 32, new Vector2(.5f, .95f), new Vector2(400, 42)); title.color = gold; title.fontStyle = FontStyle.Bold;
            timelineText = MakeText(canvasObject.transform, "Timeline", string.Empty, font, 17, new Vector2(.5f, .89f), new Vector2(1050, 36)); timelineText.color = new Color(.78f, .84f, .92f, 1f);

            CreateFormationPanel(canvasObject.transform, font, allies, "아군 진형", new Vector2(.25f, .57f));
            CreateFormationPanel(canvasObject.transform, font, enemies, "적 진형", new Vector2(.75f, .57f));

            Image commandPanel = MakeImage(canvasObject.transform, "CommandPanel", panel); SetRect(commandPanel.rectTransform, new Vector2(.5f, .17f), new Vector2(1080, 180)); AddOutline(commandPanel.gameObject, new Color(.3f, .4f, .54f, 1f), 2);
            messageText = MakeText(commandPanel.transform, "Message", "행동을 선택하세요.", font, 18, new Vector2(.5f, .73f), new Vector2(1000, 42));
            attackButton = MakeCommandButton(commandPanel.transform, "AttackButton", "공격", font, new Vector2(.17f, .31f), BeginAttack);
            skillButton = MakeCommandButton(commandPanel.transform, "SkillButton", "스킬", font, new Vector2(.39f, .31f), ShowSkillPlaceholder);
            defendButton = MakeCommandButton(commandPanel.transform, "DefendButton", "방어", font, new Vector2(.61f, .31f), Defend);
            fleeButton = MakeCommandButton(commandPanel.transform, "FleeButton", "도망", font, new Vector2(.83f, .31f), Flee);
            MakeText(canvasObject.transform, "Help", "마우스 또는 방향키: 이동   Enter/Space: 선택   Esc: 대상 선택 취소   행동 시간제한 없음", font, 15, new Vector2(.5f, .025f), new Vector2(1000, 24)).color = new Color(.7f, .77f, .86f, 1f);
        }

        private void CreateFormationPanel(Transform parent, Font font, Formation formation, string heading, Vector2 anchor)
        {
            Image root = MakeImage(parent, heading.Replace(" ", string.Empty), panel); SetRect(root.rectTransform, anchor, new Vector2(540, 360)); AddOutline(root.gameObject, gold, 2);
            Text title = MakeText(root.transform, "Heading", heading, font, 22, new Vector2(.5f, .93f), new Vector2(300, 34)); title.color = gold; title.fontStyle = FontStyle.Bold;
            MakeText(root.transform, "RearLabel", "후열", font, 16, new Vector2(.08f, .67f), new Vector2(58, 28)).color = new Color(.7f, .77f, .86f, 1f);
            MakeText(root.transform, "FrontLabel", "전열", font, 16, new Vector2(.08f, .29f), new Vector2(58, 28)).color = new Color(.7f, .77f, .86f, 1f);
            for (int rowIndex = 0; rowIndex < 2; rowIndex++)
            {
                FormationRow row = rowIndex == 0 ? FormationRow.Rear : FormationRow.Front;
                float y = row == FormationRow.Rear ? .67f : .29f;
                for (int column = 0; column < 3; column++)
                {
                    Combatant combatant = formation.Get(row, column);
                    SlotView view = CreateSlot(root.transform, font, combatant, new Vector2(.28f + column * .27f, y), row, column);
                    if (combatant != null) slotViews.Add(combatant, view);
                }
            }
        }

        private SlotView CreateSlot(Transform parent, Font font, Combatant combatant, Vector2 anchor, FormationRow row, int column)
        {
            GameObject obj = new GameObject($"Slot_{row}_{column}", typeof(Image), typeof(Button), typeof(Outline)); obj.transform.SetParent(parent, false); SetRect(obj.GetComponent<RectTransform>(), anchor, new Vector2(128, 112));
            Image image = obj.GetComponent<Image>(); image.color = unavailable;
            Button button = obj.GetComponent<Button>(); button.targetGraphic = image; button.interactable = false;
            obj.GetComponent<Outline>().effectColor = new Color(.3f, .4f, .54f, 1f); obj.GetComponent<Outline>().effectDistance = new Vector2(2, -2);
            string label = combatant == null ? $"빈 슬롯\n{(row == FormationRow.Front ? "전열" : "후열")} {column + 1}" : CombatantLabel(combatant);
            Vector2 labelAnchor = combatant == null ? Vector2.one * .5f : new Vector2(.5f, .25f);
            Vector2 labelSize = combatant == null ? new Vector2(120, 104) : new Vector2(120, 58);
            Text text = MakeText(obj.transform, "Label", label, font, 16, labelAnchor, labelSize); text.fontStyle = combatant == null ? FontStyle.Normal : FontStyle.Bold;
            if (combatant != null)
            {
                Sprite sprite = combatant.Side == BattleSide.Allies ? BattleEncounterContext.PlayerSprite : BattleEncounterContext.Monster?.FieldSprite;
                Image portrait = MakeImage(obj.transform, "Portrait", sprite == null ? Color.clear : Color.white); portrait.sprite = sprite; portrait.preserveAspect = true; SetRect(portrait.rectTransform, new Vector2(.5f, .72f), new Vector2(58, 48));
                button.onClick.AddListener(() => SelectTarget(combatant));
            }
            return new SlotView { Button = button, Background = image, Label = text };
        }

        private void AdvanceTurn()
        {
            if (battleEnded) return;
            if (enemies.IsDefeated) { EndBattle("승리! 초원 슬라임을 쓰러뜨렸습니다."); return; }
            if (allies.IsDefeated) { EndBattle("전투불능. Field_01로 복귀합니다."); return; }

            currentActor = turnOrder.TakeNext(AllCombatants);
            if (currentActor == null) return;
            currentActor.BeginTurn();
            UpdateTimeline();
            RefreshSlotViews(null);
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
            if (currentActor == null || !currentActor.IsPlayerControlled) return;
            IReadOnlyList<Combatant> targets = TargetResolver.ResolveHostileTargets(currentActor, enemies, currentActor.BasicRange);
            if (targets.Count == 0) { messageText.text = "현재 기본 공격으로 지정할 수 있는 대상이 없습니다."; return; }
            choosingTarget = true;
            SetCommandButtons(false);
            RefreshSlotViews(targets);
            messageText.text = "공격할 대상을 선택하세요. 공격 불가능한 대상은 비활성화됩니다.";
            EventSystem.current.SetSelectedGameObject(slotViews[targets[0]].Button.gameObject);
        }

        private void SelectTarget(Combatant target)
        {
            if (!choosingTarget || target == null || !target.IsAlive) return;
            IReadOnlyList<Combatant> valid = TargetResolver.ResolveHostileTargets(currentActor, enemies, currentActor.BasicRange);
            if (!valid.Contains(target)) return;
            choosingTarget = false;
            int damage = target.TakeDamage(currentActor.Attack);
            messageText.text = $"{currentActor.DisplayName}의 공격! {target.DisplayName}에게 {damage} 피해.";
            RefreshSlotViews(null);
            FinishCurrentAction();
        }

        private void ShowSkillPlaceholder()
        {
            messageText.text = "스킬 메뉴 확장 구조만 준비되어 있으며 직업별 효과는 아직 구현되지 않았습니다.";
            EventSystem.current.SetSelectedGameObject(attackButton.gameObject);
        }

        private void Defend()
        {
            currentActor.Defend();
            messageText.text = $"{currentActor.DisplayName}이(가) 방어합니다. 다음 행동 차례까지 받는 피해가 50% 감소합니다.";
            FinishCurrentAction();
        }

        private void Flee()
        {
            if (enemies.Members.Any(item => item.IsBoss)) { messageText.text = "보스전에서는 도망칠 수 없습니다."; return; }
            battleEnded = true;
            SetCommandButtons(false);
            messageText.text = "도망에 성공했습니다. Field_01로 복귀합니다.";
            StartCoroutine(ReturnAfterDelay());
        }

        private IEnumerator EnemyAction()
        {
            yield return new WaitForSeconds(.55f);
            IReadOnlyList<Combatant> targets = TargetResolver.ResolveHostileTargets(currentActor, allies, currentActor.BasicRange);
            Combatant target = targets.FirstOrDefault();
            if (target != null)
            {
                int damage = target.TakeDamage(currentActor.Attack);
                messageText.text = $"{currentActor.DisplayName}의 공격! {target.DisplayName}에게 {damage} 피해.";
                RefreshSlotViews(null);
            }
            FinishCurrentAction();
        }

        private void FinishCurrentAction()
        {
            currentActor?.CompleteAction();
            SetCommandButtons(false);
            StartCoroutine(AdvanceAfterDelay());
        }

        private IEnumerator AdvanceAfterDelay() { yield return new WaitForSeconds(.65f); AdvanceTurn(); }

        private void EndBattle(string message)
        {
            battleEnded = true;
            SetCommandButtons(false);
            messageText.text = message + " 전투 상태를 초기화하고 Field_01로 복귀합니다.";
            StartCoroutine(ReturnAfterDelay());
        }

        private IEnumerator ReturnAfterDelay() { yield return new WaitForSeconds(1.2f); BattleSceneFlow.ReturnToField(); }

        private void RefreshSlotViews(IReadOnlyList<Combatant> attackable)
        {
            foreach (KeyValuePair<Combatant, SlotView> pair in slotViews)
            {
                Combatant combatant = pair.Key; SlotView view = pair.Value;
                bool canAttack = attackable != null && attackable.Contains(combatant);
                view.Label.text = CombatantLabel(combatant) + (attackable == null ? string.Empty : canAttack ? "\n[공격 가능]" : "\n[보호됨/사거리 밖]");
                view.Background.color = !combatant.IsAlive ? new Color(.16f, .12f, .14f, 1f) : canAttack ? available : unavailable;
                view.Button.interactable = canAttack && combatant.IsAlive;
            }
        }

        private void UpdateTimeline()
        {
            string upcoming = string.Join("  →  ", turnOrder.Upcoming.Take(7).Select(item => item.DisplayName));
            timelineText.text = $"현재: {currentActor.DisplayName}" + (string.IsNullOrEmpty(upcoming) ? string.Empty : $"   |   다음: {upcoming}");
        }

        private void SetCommandButtons(bool enabled)
        {
            foreach (Selectable selectable in commandButtons) selectable.interactable = enabled;
        }

        private Button MakeCommandButton(Transform parent, string name, string label, Font font, Vector2 anchor, Action action)
        {
            GameObject obj = new GameObject(name, typeof(Image), typeof(Button), typeof(Outline)); obj.transform.SetParent(parent, false); SetRect(obj.GetComponent<RectTransform>(), anchor, new Vector2(205, 58));
            Image image = obj.GetComponent<Image>(); image.color = new Color(.12f, .32f, .5f, 1f);
            Button button = obj.GetComponent<Button>(); button.targetGraphic = image; button.onClick.AddListener(() => action()); button.colors = ColorBlock.defaultColorBlock;
            Outline outline = obj.GetComponent<Outline>(); outline.effectColor = gold; outline.effectDistance = new Vector2(2, -2);
            Text text = MakeText(obj.transform, "Label", $"[ {label} ]", font, 21, Vector2.one * .5f, new Vector2(195, 50)); text.fontStyle = FontStyle.Bold;
            commandButtons.Add(button); return button;
        }

        private static string CombatantLabel(Combatant combatant) => $"{combatant.DisplayName}\nHP {combatant.CurrentHp} / {combatant.MaxHp}" + (combatant.IsAlive ? string.Empty : "\n[전투불능]");
        private static void CreateEventSystem() { if (EventSystem.current != null) return; InputSystemUIInputModule module = new GameObject("EventSystem", typeof(EventSystem)).AddComponent<InputSystemUIInputModule>(); module.AssignDefaultActions(); }
        private static void CreateCameraIfMissing() { if (Camera.main != null) { Camera.main.backgroundColor = new Color(.018f, .03f, .06f, 1f); return; } GameObject obj = new GameObject("Main Camera"); obj.tag = "MainCamera"; obj.transform.position = new Vector3(0, 0, -10); Camera camera = obj.AddComponent<Camera>(); camera.clearFlags = CameraClearFlags.SolidColor; camera.backgroundColor = new Color(.018f, .03f, .06f, 1f); camera.orthographic = true; }
        private static Text MakeText(Transform parent, string name, string value, Font font, int size, Vector2 anchor, Vector2 dimensions) { GameObject obj = new GameObject(name, typeof(Text)); obj.transform.SetParent(parent, false); Text text = obj.GetComponent<Text>(); text.font = font; text.fontSize = size; text.color = Color.white; text.alignment = TextAnchor.MiddleCenter; text.text = value; text.raycastTarget = false; SetRect(text.rectTransform, anchor, dimensions); return text; }
        private static Image MakeImage(Transform parent, string name, Color color) { GameObject obj = new GameObject(name, typeof(Image)); obj.transform.SetParent(parent, false); Image image = obj.GetComponent<Image>(); image.color = color; image.raycastTarget = false; return image; }
        private static Outline AddOutline(GameObject target, Color color, float distance) { Outline outline = target.AddComponent<Outline>(); outline.effectColor = color; outline.effectDistance = new Vector2(distance, -distance); return outline; }
        private static void SetRect(RectTransform rect, Vector2 anchor, Vector2 size) { rect.anchorMin = anchor; rect.anchorMax = anchor; rect.pivot = Vector2.one * .5f; rect.anchoredPosition = Vector2.zero; rect.sizeDelta = size; }
        private static void Stretch(RectTransform rect) { rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one; rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero; }
    }
}