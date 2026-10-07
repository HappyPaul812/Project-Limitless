# LOCAL Story 정식 구현 대사 원문 부록

> 2026-10-05 중앙 화자 정책: Player의 실제 Runtime Speaker ID는 player, 이름은 현재 PlayerName, Voice/Portrait는 NONE이다. 구형 Main03의 raw 표시 이름 대화와 본문 첫 줄 화자는 중앙 DialogueLine에서 분리한다. 아래 Source 위치와 발화 본문은 보존하며 실제 처리 후 값은 Story_Dialogue_Audit_Matrix.csv를 따른다.

2026-10-05 현재 LOCAL Runtime 작성 대사의 추적 원문이다. 기존 Story 문서의 사건/기획/Path 설계를 변경하지 않는다. 기존 문서가 요약만 제공한 308페이지의 정확한 표시 문장을 보완한다. Speaker 메타데이터 오류는 감사 계획의 중앙 규칙으로 수정하며 본문 발화 내용을 바꾸지 않는다. WAV 발화 의미는 별도 NEEDS_LISTENING이다.

무음 페이지의 Source 위치 키는 감사용이며 Save/Runtime용 새 Dialogue ID가 아니다. Main02 Path 표현은 각각 작성 변형이며 공통 결론은 한 번 센다. Main16 shared 대사는 양 분기의 재사용으로 중복 계산하지 않는다. Intro 무음 마지막 제목과 일반 NPC 공용 fallback은 작성 대사 분모 밖의 별도 Coverage 항목이다.

## Intro

### opening_001

- Source: Unity/Client/Assets/_Project/Scripts/Core/OpeningIntroSequence.cs:26 / Intro
- Speaker: narrator / Narrator

`	ext
태초에, 신은 세상을 창조했다.
`
### opening_002

- Source: Unity/Client/Assets/_Project/Scripts/Core/OpeningIntroSequence.cs:27 / Intro
- Speaker: narrator / Narrator

`	ext
그 세상은 완전했다.
`
### opening_003

- Source: Unity/Client/Assets/_Project/Scripts/Core/OpeningIntroSequence.cs:28 / Intro
- Speaker: narrator / Narrator

`	ext
아픔도 없었고,
슬픔도 없었으며,
부족함도 없었다.
`
### opening_004

- Source: Unity/Client/Assets/_Project/Scripts/Core/OpeningIntroSequence.cs:29 / Intro
- Speaker: narrator / Narrator

`	ext
모든 존재는 강했고,
누구의 도움도 필요로 하지 않았다.
`
### opening_005

- Source: Unity/Client/Assets/_Project/Scripts/Core/OpeningIntroSequence.cs:30 / Intro
- Speaker: narrator / Narrator

`	ext
완벽한 존재는 누구도 필요로 하지 않는다.
`
### opening_006

- Source: Unity/Client/Assets/_Project/Scripts/Core/OpeningIntroSequence.cs:31 / Intro
- Speaker: narrator / Narrator

`	ext
그러나 시간이 흐를수록
세상은 조금씩 멈춰 갔다.
`
### opening_007

- Source: Unity/Client/Assets/_Project/Scripts/Core/OpeningIntroSequence.cs:32 / Intro
- Speaker: narrator / Narrator

`	ext
누구도 서로를 필요로 하지 않았기 때문이다.
`
### opening_008

- Source: Unity/Client/Assets/_Project/Scripts/Core/OpeningIntroSequence.cs:33 / Intro
- Speaker: narrator / Narrator

`	ext
신은 자신이 만든 세상을 바라보았다.
`
### opening_009

- Source: Unity/Client/Assets/_Project/Scripts/Core/OpeningIntroSequence.cs:34 / Intro
- Speaker: narrator / Narrator

`	ext
그리고 세상에 하나의 선물을 남겼다.
`
### opening_010

- Source: Unity/Client/Assets/_Project/Scripts/Core/OpeningIntroSequence.cs:35 / Intro
- Speaker: narrator / Narrator

`	ext
Limit
`
### opening_011

- Source: Unity/Client/Assets/_Project/Scripts/Core/OpeningIntroSequence.cs:36 / Intro
- Speaker: narrator / Narrator

`	ext
사람들은 서로 달라졌다.
`
### opening_012

- Source: Unity/Client/Assets/_Project/Scripts/Core/OpeningIntroSequence.cs:37 / Intro
- Speaker: narrator / Narrator

`	ext
혼자서는 할 수 없는 일이 생겼고,
때로는 누군가의 손이 필요해졌다.
`
### opening_013

- Source: Unity/Client/Assets/_Project/Scripts/Core/OpeningIntroSequence.cs:38 / Intro
- Speaker: narrator / Narrator

`	ext
그리고 아주 오랜 시간이 흐른 뒤
사람들은 조금씩 깨닫기 시작했다.
`
### opening_014

- Source: Unity/Client/Assets/_Project/Scripts/Core/OpeningIntroSequence.cs:39 / Intro
- Speaker: narrator / Narrator

`	ext
한계는 단지 약함이 아니었다.
`
### opening_015

- Source: Unity/Client/Assets/_Project/Scripts/Core/OpeningIntroSequence.cs:40 / Intro
- Speaker: narrator / Narrator

`	ext
서로를 만나게 하는 이유였고,
서로 다른 힘을 이어 주는 시작이었다.
`
### opening_016

- Source: Unity/Client/Assets/_Project/Scripts/Core/OpeningIntroSequence.cs:41 / Intro
- Speaker: narrator / Narrator

`	ext
모든 사람에게는 한계가 있다.
`
### opening_017

- Source: Unity/Client/Assets/_Project/Scripts/Core/OpeningIntroSequence.cs:42 / Intro
- Speaker: narrator / Narrator

`	ext
그리고 모든 사람에게는
그 너머로 나아갈 길이 있다.
`
### opening_018

- Source: Unity/Client/Assets/_Project/Scripts/Core/OpeningIntroSequence.cs:43 / Intro
- Speaker: narrator / Narrator

`	ext
이제, 당신의 길을 선택할 시간이다.
`

## Main01

### M01:Unity/Client/Assets/_Project/Scripts/NPC/MainQuest01NpcFlow.cs:517

- Source: Unity/Client/Assets/_Project/Scripts/NPC/MainQuest01NpcFlow.cs:15 / RepresentativeDialogue
- Speaker: starter-village-main-guide / 주민 대표

`	ext
처음 보는 얼굴이군요. 여행자이십니까?
`
### M01:Unity/Client/Assets/_Project/Scripts/NPC/MainQuest01NpcFlow.cs:554

- Source: Unity/Client/Assets/_Project/Scripts/NPC/MainQuest01NpcFlow.cs:16 / RepresentativeDialogue
- Speaker: starter-village-main-guide / 주민 대표

`	ext
마침 잘 오셨다고 해야 할지 모르겠군요.
요 며칠 초원의 몬스터들이 마을 가까이까지 내려오고 있습니다.
`
### M01:Unity/Client/Assets/_Project/Scripts/NPC/MainQuest01NpcFlow.cs:628

- Source: Unity/Client/Assets/_Project/Scripts/NPC/MainQuest01NpcFlow.cs:17 / RepresentativeDialogue
- Speaker: starter-village-main-guide / 주민 대표

`	ext
아직 큰 피해는 없지만, 평소와는 분명히 달라요.
`
### M01:Unity/Client/Assets/_Project/Scripts/NPC/MainQuest01NpcFlow.cs:671

- Source: Unity/Client/Assets/_Project/Scripts/NPC/MainQuest01NpcFlow.cs:18 / RepresentativeDialogue
- Speaker: starter-village-main-guide / 주민 대표

`	ext
남문을 지키는 경비가 상황을 가장 잘 알고 있을 겁니다.
시간이 괜찮으시다면 한번 이야기를 들어주시겠습니까?
`
### M01:Unity/Client/Assets/_Project/Scripts/NPC/MainQuest01NpcFlow.cs:748

- Source: Unity/Client/Assets/_Project/Scripts/NPC/MainQuest01NpcFlow.cs:19 / RepresentativeDialogue
- Speaker: starter-village-main-guide / 주민 대표

`	ext
무슨 일이 있었는지 들어보겠습니다.
`
### M01:Unity/Client/Assets/_Project/Scripts/NPC/MainQuest01NpcFlow.cs:861

- Source: Unity/Client/Assets/_Project/Scripts/NPC/MainQuest01NpcFlow.cs:24 / GuardDialogue
- Speaker: starter-village-gate-guard / 남문 경비병

`	ext
주민 대표에게 이야기를 들으셨군요.
`
### M01:Unity/Client/Assets/_Project/Scripts/NPC/MainQuest01NpcFlow.cs:896

- Source: Unity/Client/Assets/_Project/Scripts/NPC/MainQuest01NpcFlow.cs:25 / GuardDialogue
- Speaker: starter-village-gate-guard / 남문 경비병

`	ext
평소라면 초원 안쪽에서나 보이던 녀석들입니다.
그런데 며칠 전부터 길 근처까지 내려오기 시작했습니다.
`
### M01:Unity/Client/Assets/_Project/Scripts/NPC/MainQuest01NpcFlow.cs:969

- Source: Unity/Client/Assets/_Project/Scripts/NPC/MainQuest01NpcFlow.cs:26 / GuardDialogue
- Speaker: starter-village-gate-guard / 남문 경비병

`	ext
쫓아내도 다시 나타나고요.
`
### M01:Unity/Client/Assets/_Project/Scripts/NPC/MainQuest01NpcFlow.cs:999

- Source: Unity/Client/Assets/_Project/Scripts/NPC/MainQuest01NpcFlow.cs:27 / GuardDialogue
- Speaker: starter-village-gate-guard / 남문 경비병

`	ext
이상한 건, 마을을 노리고 오는 것 같지는 않다는 겁니다.
`
### M01:Unity/Client/Assets/_Project/Scripts/NPC/MainQuest01NpcFlow.cs:1047

- Source: Unity/Client/Assets/_Project/Scripts/NPC/MainQuest01NpcFlow.cs:28 / GuardDialogue
- Speaker: starter-village-gate-guard / 남문 경비병

`	ext
놈들이 어디서부터 내려오는 건지 확인할 수 있다면 도움이 될 텐데요.
`

## Main02

### M02:Unity/Client/Assets/_Project/Scripts/World/MainQuest02FieldFlow.cs:4362

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest02FieldFlow.cs:92 / all_paths/common_conclusion
- Speaker:  / 조사

`	ext
이 흔적을 보면, 몬스터들이 마을을 향해 몰려온 것이라기보다 무언가를 피해 이쪽으로 밀려온 것 같다.
`
### M02:Unity/Client/Assets/_Project/Scripts/World/MainQuest02FieldFlow.cs:7486

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest02FieldFlow.cs:167 / path.emotional-scar
- Speaker:  / 조사

`	ext
위험은 지나간 듯하지만, 이곳에는 아직 불안하게 흩어진 흔적이 남아 있다.
`
### M02:Unity/Client/Assets/_Project/Scripts/World/MainQuest02FieldFlow.cs:7582

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest02FieldFlow.cs:168 / path.hearing
- Speaker:  / 조사

`	ext
주변은 다시 조용해졌지만, 움직임이 한쪽으로 급히 쏠렸던 분위기가 남아 있다.
`
### M02:Unity/Client/Assets/_Project/Scripts/World/MainQuest02FieldFlow.cs:7673

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest02FieldFlow.cs:169 / path.vision
- Speaker:  / 조사

`	ext
흩어진 흔적을 차례로 정리해 보니, 움직임이 한 방향으로 이어져 있다.
`
### M02:Unity/Client/Assets/_Project/Scripts/World/MainQuest02FieldFlow.cs:7759

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest02FieldFlow.cs:170 / path.mobility
- Speaker:  / 조사

`	ext
눌린 풀과 비켜 난 자리를 따라가 보니, 여러 몬스터가 열린 쪽으로 급히 움직인 듯하다.
`
### M02:Unity/Client/Assets/_Project/Scripts/World/MainQuest02FieldFlow.cs:7857

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest02FieldFlow.cs:171 / path.intellectual
- Speaker:  / 조사

`	ext
여러 흔적의 순서를 하나씩 맞춰 보니, 같은 방향으로 급히 움직인 흐름이 보인다.
`
### M02:Unity/Client/Assets/_Project/Scripts/World/MainQuest02FieldFlow.cs:7955

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest02FieldFlow.cs:172 / default
- Speaker:  / 조사

`	ext
흩어진 흔적을 천천히 살펴보니, 여러 몬스터가 같은 방향으로 급히 움직인 듯하다.
`

## Main03

### main03_taeon_supp_001

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest03FieldFlow.cs:136 / TryReach
- Speaker: companion_taeon / 태온

`	ext
바닥에 간단히 치료한 흔적과 떨어진 붕대가 남아 있습니다.
`
### main03_taeon_supp_002

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest03FieldFlow.cs:137 / TryReach
- Speaker: companion_taeon / 태온

`	ext
누군가 먼저 이곳을 지나간 것 같습니다.
`
### main03_taeon_supp_003

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest03FieldFlow.cs:218 / TryInteract
- Speaker: companion_taeon / 태온

`	ext
옵니다.
`
### main03_taeon_supp_004

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest03FieldFlow.cs:219 / TryInteract
- Speaker: companion_taeon / 태온

`	ext
제가 앞을 막겠습니다.
뒤를 부탁드리겠습니다.
`
### main03_taeon_supp_005

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest03FieldFlow.cs:239 / FirstConversation
- Speaker: companion_taeon / 대화

`	ext
잠깐만요. 더 가까이 가지 않는 게 좋겠습니다.
`
### M03:Unity/Client/Assets/_Project/Scripts/World/MainQuest03FieldFlow.cs:11764

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest03FieldFlow.cs:240 / FirstConversation
- Speaker: player / 대화

`	ext
무슨 일이 있습니까?
`
### main03_taeon_supp_006

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest03FieldFlow.cs:241 / FirstConversation
- Speaker: companion_taeon / 대화

`	ext
저 몬스터들 말입니다.
그냥 돌아다니는 것 같지만…
계속 같은 쪽을 피하고 있어요.
`
### M03:Unity/Client/Assets/_Project/Scripts/World/MainQuest03FieldFlow.cs:11998

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest03FieldFlow.cs:242 / FirstConversation
- Speaker: player / 대화

`	ext
저도 조금 전에 이상한 흔적을 발견했습니다.
마을 쪽으로 몰려온 흔적이었습니다.
`
### main03_taeon_supp_007

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest03FieldFlow.cs:243 / FirstConversation
- Speaker: companion_taeon / 대화

`	ext
그렇군요.
그러면 제가 보고 있던 움직임하고
이어질지도 모르겠습니다.
`
### main03_taeon_supp_008

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest03FieldFlow.cs:250 / AfterBattleConversation
- Speaker: companion_taeon / 대화

`	ext
역시 이상합니다.
`
### M03:Unity/Client/Assets/_Project/Scripts/World/MainQuest03FieldFlow.cs:12575

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest03FieldFlow.cs:251 / AfterBattleConversation
- Speaker: player / 대화

`	ext
방금 몬스터들도 같은 방향을 피했습니까?
`
### main03_taeon_supp_009

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest03FieldFlow.cs:252 / AfterBattleConversation
- Speaker: companion_taeon / 대화

`	ext
네.
싸우는 동안에도 몇 번이나
그쪽으로 움직이지 않으려고 했어요.
`
### M03:Unity/Client/Assets/_Project/Scripts/World/MainQuest03FieldFlow.cs:12811

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest03FieldFlow.cs:253 / AfterBattleConversation
- Speaker: player / 대화

`	ext
제가 본 흔적도
그 반대쪽에서 시작됐습니다.
`
### main03_taeon_supp_010

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest03FieldFlow.cs:254 / AfterBattleConversation
- Speaker: companion_taeon / 대화

`	ext
그렇다면 우연은 아닌 것 같습니다.
`
### main03_taeon_supp_011

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest03FieldFlow.cs:255 / AfterBattleConversation
- Speaker: companion_taeon / 대화

`	ext
저도 저쪽을 확인하려던 참이었습니다.
`
### main03_taeon_supp_012

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest03FieldFlow.cs:256 / AfterBattleConversation
- Speaker: companion_taeon / 대화

`	ext
목적이 같다면 잠시 함께 가시죠.
혼자 움직이는 것보다는 안전할 겁니다.
`

## Main04

### main04_miel_supp_001

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:206 / FirstConversation
- Speaker: companion_miel / 미엘

`	ext
조금만 참으세요.
출혈은 멎었습니다.
`
### main04_taeon_supp_001

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:207 / FirstConversation
- Speaker: companion_taeon / 태온

`	ext
괜찮으십니까?
`
### main04_miel_supp_002

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:208 / FirstConversation
- Speaker: companion_miel / 미엘

`	ext
저보다 이분이 먼저예요.
`
### main04_taeon_supp_002

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:209 / FirstConversation
- Speaker: companion_taeon / 태온

`	ext
당신도 다친 것 같은데요.
`
### main04_miel_supp_003

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:210 / FirstConversation
- Speaker: companion_miel / 미엘

`	ext
알아요.
그래도 아직 움직일 수 있어요.
`
### M04:Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:11049

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:211 / FirstConversation
- Speaker: player / <PlayerName>

`	ext
여기서 무슨 일이 있었습니까?
`
### main04_miel_supp_004

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:212 / FirstConversation
- Speaker: companion_miel / 미엘

`	ext
몬스터들에게 습격받았습니다.
그런데 조금 이상했어요.
`
### main04_miel_supp_005

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:213 / FirstConversation
- Speaker: companion_miel / 미엘

`	ext
처음부터 사람을 노리고
온 것 같지는 않았습니다.
`
### main04_taeon_supp_003

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:214 / FirstConversation
- Speaker: companion_taeon / 태온

`	ext
무언가를 피하고 있었습니까?
`
### main04_miel_supp_006

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:215 / FirstConversation
- Speaker: companion_miel / 미엘

`	ext
네.
갑자기 길을 가로막게 되자
공격한 것처럼 보였어요.
`
### main04_taeon_supp_004

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:220 / EncounterConversation
- Speaker: companion_taeon / 태온

`	ext
또 옵니다.
제가 앞을 맡겠습니다.
`
### main04_miel_supp_007

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:221 / EncounterConversation
- Speaker: companion_miel / 미엘

`	ext
다친 곳은 제가 보겠습니다.
`
### M04:Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:11836

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:222 / EncounterConversation
- Speaker: player / <PlayerName>

`	ext
갑시다.
`
### main04_miel_supp_008

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:228 / AfterBattleConversation
- Speaker: companion_miel / 미엘

`	ext
괜찮으세요?
다친 곳부터 확인할게요.
`
### main04_taeon_supp_005

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:229 / AfterBattleConversation
- Speaker: companion_taeon / 태온

`	ext
본인부터 보셔야 하는 것 아닙니까?
`
### main04_miel_supp_009

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:230 / AfterBattleConversation
- Speaker: companion_miel / 미엘

`	ext
저도 볼 겁니다.
이번에는 순서대로요.
`
### main04_taeon_supp_006

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:232 / AfterBattleConversation
- Speaker: companion_taeon / 태온

`	ext
제가 본 움직임과
이곳에서 있었던 일까지 합치면…
`
### M04:Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:12541

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:233 / AfterBattleConversation
- Speaker: player / <PlayerName>

`	ext
몬스터들이 마을을 노리고
내려오는 건 아닌 것 같습니다.
`
### main04_miel_supp_010

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:234 / AfterBattleConversation
- Speaker: companion_miel / 미엘

`	ext
초원보다 더 안쪽에서
무언가가 벌어지고 있는 것 같아요.
`
### main04_taeon_supp_007

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:235 / AfterBattleConversation
- Speaker: companion_taeon / 태온

`	ext
여기서 더 들어가는 건
지금은 위험할 것 같습니다.
`
### main04_miel_supp_011

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:236 / AfterBattleConversation
- Speaker: companion_miel / 미엘

`	ext
마을에도 이 상황을 알려야 해요.
`
### M04:Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:12945

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs:237 / AfterBattleConversation
- Speaker: player / <PlayerName>

`	ext
돌아가죠.
`

## Main05

### M05:Unity/Client/Assets/_Project/Scripts/World/MainQuest05ReturnFlow.cs:3619

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest05ReturnFlow.cs:80 / GuardReport
- Speaker: starter-village-gate-guard / 남문 경비병

`	ext
돌아오셨군요.
두 분은…?
`
### M05:Unity/Client/Assets/_Project/Scripts/World/MainQuest05ReturnFlow.cs:3694

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest05ReturnFlow.cs:81 / GuardReport
- Speaker: player / <PlayerName>

`	ext
초원에서 만났습니다.
함께 상황을 조사했습니다.
`
### main05_taeon_supp_001

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest05ReturnFlow.cs:82 / GuardReport
- Speaker: companion_taeon / 태온

`	ext
몬스터들이 마을 쪽으로 움직이는 건 맞습니다.
하지만 마을을 노리고 있는 것 같지는 않습니다.
`
### main05_miel_supp_001

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest05ReturnFlow.cs:83 / GuardReport
- Speaker: companion_miel / 미엘

`	ext
무언가를 피해서 내려오고 있는 것 같아요.
`
### M05:Unity/Client/Assets/_Project/Scripts/World/MainQuest05ReturnFlow.cs:4013

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest05ReturnFlow.cs:84 / GuardReport
- Speaker: starter-village-gate-guard / 남문 경비병

`	ext
피해서… 말입니까?
`
### M05:Unity/Client/Assets/_Project/Scripts/World/MainQuest05ReturnFlow.cs:4083

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest05ReturnFlow.cs:85 / GuardReport
- Speaker: player / <PlayerName>

`	ext
아직 원인은 모릅니다.
`
### M05:Unity/Client/Assets/_Project/Scripts/World/MainQuest05ReturnFlow.cs:4234

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest05ReturnFlow.cs:90 / RepresentativeReport
- Speaker: starter-village-main-guide / 주민 대표

`	ext
그렇다면 단순히 몬스터를 쫓아내는 것만으로
해결될 일은 아니겠군요.
`
### M05:Unity/Client/Assets/_Project/Scripts/World/MainQuest05ReturnFlow.cs:4340

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest05ReturnFlow.cs:91 / RepresentativeReport
- Speaker: starter-village-main-guide / 주민 대표

`	ext
초원 너머 숲에서도
비슷한 일이 있다는 이야기가 있었습니다.
`
### M05:Unity/Client/Assets/_Project/Scripts/World/MainQuest05ReturnFlow.cs:4442

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest05ReturnFlow.cs:92 / RepresentativeReport
- Speaker: starter-village-main-guide / 주민 대표

`	ext
세 분 덕분에 적어도
어디부터 살펴봐야 할지는 알게 됐습니다.
`
### main05_taeon_supp_002

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest05ReturnFlow.cs:93 / RepresentativeReport
- Speaker: companion_taeon / 태온

`	ext
저도 이 움직임이
어디서 시작됐는지 확인하고 싶습니다.
`
### main05_taeon_supp_003

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest05ReturnFlow.cs:94 / RepresentativeReport
- Speaker: companion_taeon / 태온

`	ext
괜찮으시다면…
조금 더 함께 가겠습니다.
`
### main05_miel_supp_002

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest05ReturnFlow.cs:95 / RepresentativeReport
- Speaker: companion_miel / 미엘

`	ext
이대로 두면
다치는 사람이 더 생길 겁니다.
`
### main05_miel_supp_003

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest05ReturnFlow.cs:96 / RepresentativeReport
- Speaker: companion_miel / 미엘

`	ext
저도 같이 가겠습니다.
제가 할 수 있는 일이 있을 거예요.
`
### M05:Unity/Client/Assets/_Project/Scripts/World/MainQuest05ReturnFlow.cs:4994

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest05ReturnFlow.cs:97 / RepresentativeReport
- Speaker: player / <PlayerName>

`	ext
그럼 앞으로도 잘 부탁드립니다.
`

## Main06

### main06_taeon_001

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest06ForestFlow.cs:128 / OnInteract
- Speaker: companion_taeon / 태온

`	ext
초원에서 봤던 움직임과 비슷합니다.
`
### main06_taeon_002

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest06ForestFlow.cs:129 / OnInteract
- Speaker: companion_taeon / 태온

`	ext
하지만 여기서는 더 넓게 퍼져 있어요.
`
### main06_miel_001

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest06ForestFlow.cs:130 / OnInteract
- Speaker: companion_miel / 미엘

`	ext
그럼 초원의 문제만은 아니었던 거네요.
`
### M06:Unity/Client/Assets/_Project/Scripts/World/MainQuest06ForestFlow.cs:5927

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest06ForestFlow.cs:131 / OnInteract
- Speaker: player / <PlayerName>

`	ext
숲 안쪽을 더 확인해봐야겠습니다.
`

## Main07

### main07_taeon_supp_001

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:177 / GetLines
- Speaker: companion_taeon / 태온

`	ext
수레가 지나간 흔적은 아닌 것 같습니다.
`
### main07_taeon_supp_002

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:177 / GetLines
- Speaker: companion_taeon / 태온

`	ext
폭이 일정하고… 한쪽이 계속 더 깊게 눌려 있어요.
`
### main07_miel_supp_001

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:177 / GetLines
- Speaker: companion_miel / 미엘

`	ext
누군가 이쪽으로 지나간 것 같네요.
`
### M07:Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:11364

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:177 / GetLines
- Speaker: player / <PlayerName>

`	ext
따라가 보죠.
`
### main07_miel_supp_002

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:178 / GetLines
- Speaker: companion_miel / 미엘

`	ext
이분을 그냥 두고 갈 수는 없어요. 제가 상태를 볼게요.
`
### main07_taeon_supp_003

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:178 / GetLines
- Speaker: companion_taeon / 태온

`	ext
혼자 괜찮겠습니까?
`
### main07_miel_supp_003

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:178 / GetLines
- Speaker: companion_miel / 미엘

`	ext
네. 두 분은 흔적을 확인해주세요.
`
### main07_miel_supp_004

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:178 / GetLines
- Speaker: companion_miel / 미엘

`	ext
상황이 안 좋으면 바로 돌아오시고요.
`
### M07:Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:11914

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:181 / GetLines
- Speaker: companion_paul / 폴

`	ext
아무래도 저희가 보고 있는 게 같은 현상 같기는 하네요.
`
### M07:Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:11984

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:181 / GetLines
- Speaker: companion_paul / 폴

`	ext
그런데 저는 확인해볼 곳이 하나 더 있습니다.
`
### M07:Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:12048

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:181 / GetLines
- Speaker: player / <PlayerName>

`	ext
혼자 가시려고요?
`
### M07:Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:12074

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:181 / GetLines
- Speaker: companion_paul / 폴

`	ext
이번에는 진흙 없는 길로요.
`
### M07:Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:12128

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:181 / GetLines
- Speaker: companion_paul / 폴

`	ext
아까 충분히 배웠습니다. 헤헤.
`
### M07:Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:12184

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:181 / GetLines
- Speaker: companion_paul / 폴

`	ext
다시 만나게 되면 그때 정보부터 맞춰보죠.
`
### main07_paul_001

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:186 / PaulFirst
- Speaker: companion_paul / 폴

`	ext
아, 사람이다. 반갑습니다. 진짜로요.
`
### main07_paul_002

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:186 / PaulFirst
- Speaker: companion_paul / 폴

`	ext
원래 계획은 저 진흙을 피해 가는 거였는데요.
`
### main07_paul_003

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:186 / PaulFirst
- Speaker: companion_paul / 폴

`	ext
보시다시피 계획이 아주 성공적입니다.
`
### main07_taeon_001

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:186 / PaulFirst
- Speaker: companion_taeon / 태온

`	ext
바퀴가 빠지셨는데요.
`
### main07_paul_004

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:186 / PaulFirst
- Speaker: companion_paul / 폴

`	ext
네, 그러게요. 그것도 아주 깊~게요...;;
`
### main07_paul_005

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:186 / PaulFirst
- Speaker: companion_paul / 폴

`	ext
하여튼 바로 잡아당기시면 안 됩니다. 그러면 앞바퀴까지 빠질 거예요.
`
### main07_paul_006

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:186 / PaulFirst
- Speaker: companion_paul / 폴

`	ext
저쪽에 넓은 돌 보이시죠? 그걸 오른쪽 바퀴 뒤에 받쳐주시고요.
`
### main07_paul_007

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:186 / PaulFirst
- Speaker: companion_paul / 폴

`	ext
제가 바퀴를 돌릴 때 같이 밀면 될 것 같습니다.
`
### main07_taeon_002

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:186 / PaulFirst
- Speaker: companion_taeon / 태온

`	ext
옵니다.
`
### main07_paul_008

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:186 / PaulFirst
- Speaker: companion_paul / 폴

`	ext
아… 저건 계획에 없었는데요.
`
### main07_paul_009

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:186 / PaulFirst
- Speaker: companion_paul / 폴

`	ext
뭐, 바퀴가 안 움직이는 거지 마법까지 안 나가는 건 아니니까요.
`
### main07_paul_010

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:191 / MielMeeting
- Speaker: companion_paul / 폴

`	ext
후우… 됐네요.
`
### main07_paul_011

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:191 / MielMeeting
- Speaker: companion_paul / 폴

`	ext
다음에는 저 길로 안 갑니다. 경험을 했으면 계획을 수정해야죠.
`
### main07_paul_012

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:191 / MielMeeting
- Speaker: companion_paul / 폴

`	ext
아, 도와주셔서 감사합니다. 저는 폴이라고 합니다.
`
### main07_paul_013

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:191 / MielMeeting
- Speaker: companion_paul / 폴

`	ext
저도 이 숲을 좀 조사하고 있었습니다.
`
### main07_paul_014

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:191 / MielMeeting
- Speaker: companion_paul / 폴

`	ext
며칠 전부터 숲 안쪽의 마력 흐름이 평소랑 달라졌거든요.
`
### main07_paul_015

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:191 / MielMeeting
- Speaker: companion_paul / 폴

`	ext
처음에는 별일 아닌 줄 알았는데… 몬스터들 움직임까지 달라지는 걸 보고 생각이 바뀌었습니다.
`
### main07_taeon_003

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:191 / MielMeeting
- Speaker: companion_taeon / 태온

`	ext
저희도 같은 현상을 따라 여기까지 왔습니다.
`
### main07_paul_016

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:191 / MielMeeting
- Speaker: companion_paul / 폴

`	ext
그러면 제가 진흙에 빠진 건 몰라도, 방향은 제대로 잡은 것 같네요.
`
### main07_miel_001

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:192 / MielMeeting
- Speaker: companion_miel / 미엘

`	ext
돌아오셨네요. 그리고… 처음 보는 분도 계시네요.
`
### main07_paul_017

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:192 / MielMeeting
- Speaker: companion_paul / 폴

`	ext
폴이라고 합니다.
`
### main07_paul_018

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:192 / MielMeeting
- Speaker: companion_paul / 폴

`	ext
원래는 조금 더 멀쩡한 모습으로 처음 뵐 예정이었는데요. 그늘숲이 협조를 안 해주더라고요.
`
### main07_miel_002

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:192 / MielMeeting
- Speaker: companion_miel / 미엘

`	ext
혹시 다치신 곳은 없으세요?
`
### main07_paul_019

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:192 / MielMeeting
- Speaker: companion_paul / 폴

`	ext
저는 괜찮습니다. 저분부터 봐주세요.
`
### main07_miel_003

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:192 / MielMeeting
- Speaker: companion_miel / 미엘

`	ext
저분은 이제 괜찮으세요. 그러니까 폴 씨도 확인해야죠.
`
### main07_paul_020

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs:192 / MielMeeting
- Speaker: companion_paul / 폴

`	ext
…아, 순서가 벌써 제 차례인가요?
`

## Main08

### main08_taeon_supp_001

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:65 / DrawTracks
- Speaker: companion_taeon / 태온

`	ext
잠깐만요.
`
### main08_taeon_supp_002

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:65 / DrawTracks
- Speaker: companion_taeon / 태온

`	ext
여기부터 흔적이 거의 없습니다.
`
### main08_miel_supp_001

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:65 / DrawTracks
- Speaker: companion_miel / 미엘

`	ext
몬스터가 적다는 건 좋은 일 아닌가요?
`
### main08_taeon_supp_003

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:66 / DrawTracks
- Speaker: companion_taeon / 태온

`	ext
평소라면 그렇겠죠.
`
### main08_taeon_supp_004

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:66 / DrawTracks
- Speaker: companion_taeon / 태온

`	ext
그런데 조금 전까지 이쪽으로 이어지던 흔적들이 전부 방향을 바꾸고 있습니다.
`
### main08_miel_supp_002

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:67 / DrawTracks
- Speaker: companion_miel / 미엘

`	ext
없어진 게 아니네요.
`
### main08_miel_supp_003

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:67 / DrawTracks
- Speaker: companion_miel / 미엘

`	ext
다들 이쪽을 피해서 지나가고 있어요.
`
### main08_taeon_supp_005

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:67 / DrawTracks
- Speaker: companion_taeon / 태온

`	ext
초원에서는 무언가를 피해 밀려오는 것처럼 보였습니다.
`
### main08_taeon_supp_006

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:67 / DrawTracks
- Speaker: companion_taeon / 태온

`	ext
여기서는 그 방향이 조금 더 분명합니다.
`
### M08:Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:5531

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:67 / DrawTracks
- Speaker: player / <PlayerName>

`	ext
그러면 우리가 찾던 방향은 맞는 것 같습니다.
`
### main08_miel_supp_004

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:68 / DrawTracks
- Speaker: companion_miel / 미엘

`	ext
이 흔적… 폴 씨 것 아닐까요?
`
### main08_taeon_supp_007

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:68 / DrawTracks
- Speaker: companion_taeon / 태온

`	ext
폭이 같습니다. 아마 맞을 겁니다.
`
### main08_taeon_supp_008

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:69 / DrawTracks
- Speaker: companion_taeon / 태온

`	ext
누군가 이곳을 조사한 흔적입니다.
`
### main08_miel_supp_005

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:69 / DrawTracks
- Speaker: companion_miel / 미엘

`	ext
폴 씨일까요?
`
### M08:Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:5993

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:69 / DrawTracks
- Speaker: player / <PlayerName>

`	ext
아마 먼저 안쪽으로 간 것 같습니다.
`
### main08_miel_supp_006

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:70 / DrawTracks
- Speaker: companion_miel / 미엘

`	ext
조용하네요.
`
### main08_taeon_supp_009

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:70 / DrawTracks
- Speaker: companion_taeon / 태온

`	ext
너무 조용합니다.
`
### main08_miel_supp_007

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:70 / DrawTracks
- Speaker: companion_miel / 미엘

`	ext
몬스터들이 여기를 피하는 이유가 있는 거겠죠?
`
### main08_taeon_supp_010

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:70 / DrawTracks
- Speaker: companion_taeon / 태온

`	ext
그 이유가 무엇인지는 아직 모르겠습니다.
`
### M08:Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:6382

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:70 / DrawTracks
- Speaker: player / <PlayerName>

`	ext
안쪽을 확인해보죠.
`
### main08_miel_supp_008

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:70 / DrawTracks
- Speaker: companion_miel / 미엘

`	ext
…저쪽에서 빛이 났어요.
`
### main08_taeon_supp_011

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:70 / DrawTracks
- Speaker: companion_taeon / 태온

`	ext
마법 같습니다.
`
### M08:Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:6561

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs:70 / DrawTracks
- Speaker: player / <PlayerName>

`	ext
가보죠.
`

## Main09

### main09_miel_001

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:167 / Lines
- Speaker: companion_miel / 미엘

`	ext
빛은 이쪽에서 보였어요. 지금은 사라졌네요.
`
### main09_taeon_001

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:167 / Lines
- Speaker: companion_taeon / 태온

`	ext
빛의 정체는 아직 모릅니다. 남아 있는 흔적부터 보죠.
`
### M09:Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:9497

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:167 / Lines
- Speaker: player / <PlayerName>

`	ext
발밑을 확인하면서 따라가겠습니다.
`
### main09_taeon_002

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:168 / Lines
- Speaker: companion_taeon / 태온

`	ext
두 줄의 폭이 일정합니다. 이번에는 깊게 빠진 자국이 아니에요.
`
### main09_miel_002

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:168 / Lines
- Speaker: companion_miel / 미엘

`	ext
단단한 지면을 따라 이어져 있네요. 폴 씨도 이쪽으로 가셨을까요?
`
### main09_paul_001

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:168 / Lines
- Speaker: companion_paul / 폴

`	ext
그 길은 괜찮습니다! 오른쪽 가장자리만 피해주세요.
`
### main09_paul_002

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:169 / Lines
- Speaker: companion_paul / 폴

`	ext
또 뵙네요. 이번에는 제가 길을 안내해드릴 차례인가 봅니다.
`
### main09_miel_003

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:169 / Lines
- Speaker: companion_miel / 미엘

`	ext
혼자 여기까지 오신 거예요? 다치신 곳은요?
`
### main09_paul_003

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:169 / Lines
- Speaker: companion_paul / 폴

`	ext
괜찮습니다. 오늘 계획표에는 진흙에 빠지는 일정도 빼뒀고요.
`
### main09_taeon_003

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:169 / Lines
- Speaker: companion_taeon / 태온

`	ext
지나오신 길을 확인하고 계셨습니까?
`
### main09_paul_004

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:169 / Lines
- Speaker: companion_paul / 폴

`	ext
네. 단단한 땅과 돌아갈 길을 먼저 표시해뒀습니다. 여러분은 괜찮으세요?
`
### M09:Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:10062

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:169 / Lines
- Speaker: player / <PlayerName>

`	ext
저희도 무사합니다. 서로 확인한 것을 이야기해보죠.
`
### M09:Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:10138

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:170 / Lines
- Speaker: player / <PlayerName>

`	ext
몬스터들이 깊은 곳을 둥글게 피하고 있습니다. 안쪽으로 갈수록 수도 줄었고요.
`
### main09_taeon_004

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:170 / Lines
- Speaker: companion_taeon / 태온

`	ext
이동 흔적이 방향을 바꾸는 지점이 있었습니다.
`
### main09_miel_004

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:170 / Lines
- Speaker: companion_miel / 미엘

`	ext
조금 전에는 짧게 푸른 빛도 봤어요.
`
### main09_paul_005

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:170 / Lines
- Speaker: companion_paul / 폴

`	ext
빛은 저도 봤습니다. 제 마법인지부터 묻고 싶으시겠지만, 저도 답을 찾는 중입니다.
`
### main09_paul_006

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:170 / Lines
- Speaker: companion_paul / 폴

`	ext
제가 지나온 길에서는 마력 흐름이 한 방향으로 치우쳐 있었습니다. 평소 숲의 지형과 다른 석재도 발견했고요.
`
### main09_paul_007

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:170 / Lines
- Speaker: companion_paul / 폴

`	ext
제가 확인한 건 여기까지입니다. 서로 관련이 있는지, 원인이 무엇인지는 아직 모릅니다.
`
### main09_taeon_005

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:170 / Lines
- Speaker: companion_taeon / 태온

`	ext
사실과 추측을 나눠두는 게 좋겠습니다.
`
### main09_paul_008

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:170 / Lines
- Speaker: companion_paul / 폴

`	ext
동의합니다. 석재는 저쪽 낮은 지점에 있습니다. 가장자리 지반은 밟지 마세요.
`
### main09_taeon_006

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:171 / Lines
- Speaker: companion_taeon / 태온

`	ext
모서리의 간격이 반복됩니다. 자연 암반과는 다릅니다.
`
### main09_paul_009

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:171 / Lines
- Speaker: companion_paul / 폴

`	ext
표면에 가공한 자국도 있습니다. 오래된 구조물 일부로 보이지만, 용도는 모르겠습니다.
`
### main09_miel_005

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:171 / Lines
- Speaker: companion_miel / 미엘

`	ext
흙 아래로 더 이어지는 것 같아요.
`
### M09:Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:10850

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:171 / Lines
- Speaker: player / <PlayerName>

`	ext
지금 보이는 부분만 기록하죠. 아래가 안전한지는 아직 모르니까요.
`
### main09_paul_010

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:171 / Lines
- Speaker: companion_paul / 폴

`	ext
좋습니다. 무너진 부분과 돌아갈 길을 표시해두겠습니다.
`
### main09_paul_011

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:172 / Lines
- Speaker: companion_paul / 폴

`	ext
각자 본 것만으로는 놓치는 게 있네요. 현상도 여기서 끝난 것 같지 않고요.
`
### main09_paul_012

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:172 / Lines
- Speaker: companion_paul / 폴

`	ext
같은 방향을 조사하고 있으니, 계속 정보를 나누는 편이 합리적이겠습니다. 저도 함께 가도 될까요?
`
### M09:Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:11139

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:172 / Lines
- Speaker: player / <PlayerName>

`	ext
함께 가죠. 확인한 것을 서로 알려주면 좋겠습니다.
`
### main09_taeon_007

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:172 / Lines
- Speaker: companion_taeon / 태온

`	ext
안전한 경로부터 같이 확인하겠습니다.
`
### main09_miel_006

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:172 / Lines
- Speaker: companion_miel / 미엘

`	ext
쉬어야 할 때도 말씀해주세요. 그건 모두에게 하는 말이에요.
`
### main09_paul_013

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:172 / Lines
- Speaker: companion_paul / 폴

`	ext
그럼 쉬는 시간도 계획에 넣겠습니다. 빈칸을 남겨둔 보람이 있네요.
`
### main09_paul_014

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs:172 / Lines
- Speaker: companion_paul / 폴

`	ext
우선 여기서 정리하고, 더 깊은 곳은 준비해서 확인하죠.
`

## Main10

### M10:Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:7694

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:151 / TryInteract
- Speaker: player / <PlayerName>

`	ext
입구를 확인했다. 내려가기 전 준비를 마치고 다시 돌아오자.
`
### main10_taeon_001

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:204 / Lines
- Speaker: companion_taeon / 태온

`	ext
폴이 가리킨 석재가 여기입니다. 주변 흙과 모서리의 방향이 다릅니다.
`
### main10_paul_001

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:205 / Lines
- Speaker: companion_paul / 폴

`	ext
네. 전에 본 자리를 다시 확인해보죠. 기억보다 돌이 도망가지는 않았을 겁니다.
`
### M10:Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:10819

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:205 / Lines
- Speaker: player / <PlayerName>

`	ext
이어지는 부분을 살펴보겠습니다.
`
### main10_taeon_002

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:206 / Lines
- Speaker: companion_taeon / 태온

`	ext
모서리 간격이 반복됩니다. 자연 암반과는 형태가 다릅니다.
`
### main10_paul_002

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:207 / Lines
- Speaker: companion_paul / 폴

`	ext
표면에 손댄 흔적이 있습니다. 오래된 구조의 일부일 가능성이 높지만, 누가 언제 만들었는지는 모르겠습니다.
`
### main10_miel_001

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:208 / Lines
- Speaker: companion_miel / 미엘

`	ext
흙 아래쪽으로도 이어지는 것 같아요.
`
### M10:Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:11114

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:208 / Lines
- Speaker: player / <PlayerName>

`	ext
보이는 범위만 기록하고 안쪽으로 가보죠.
`
### main10_miel_002

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:209 / Lines
- Speaker: companion_miel / 미엘

`	ext
이쪽으로 올수록 벌레와 새 소리가 거의 들리지 않아요.
`
### main10_taeon_003

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:210 / Lines
- Speaker: companion_taeon / 태온

`	ext
몬스터의 흔적도 안쪽을 피해 돌아갑니다. 먼저 안전한 길을 확인하죠.
`
### main10_paul_003

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:211 / Lines
- Speaker: companion_paul / 폴

`	ext
귀를 쉬게 할 계획은 없었는데, 조용해지는 방향이 분명하군요.
`
### M10:Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:11446

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:212 / Lines
- Speaker: player / <PlayerName>

`	ext
무너진 돌 아래로 경사가 이어집니다.
`
### main10_taeon_004

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:213 / Lines
- Speaker: companion_taeon / 태온

`	ext
발 디딜 곳부터 살펴야 합니다. 흙이 밀린 자리는 피하세요.
`
### main10_paul_004

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:214 / Lines
- Speaker: companion_paul / 폴

`	ext
단단한 가장자리를 표시해두겠습니다. 안쪽 구조는 아직 보이지 않습니다.
`
### main10_miel_003

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:215 / Lines
- Speaker: companion_miel / 미엘

`	ext
덩굴 아래에 빈 공간이 있어요.
`
### main10_paul_005

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:216 / Lines
- Speaker: companion_paul / 폴

`	ext
오래된 석재와 안치 공간처럼 보이는 자리가 드러났습니다. 정확한 용도는 더 확인해야 합니다.
`
### M10:Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:11839

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:217 / Lines
- Speaker: player / <PlayerName>

`	ext
침묵의 지하묘지로 이어지는 입구 같습니다. 지금 보이는 부분을 조사하죠.
`
### main10_taeon_005

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:218 / Lines
- Speaker: companion_taeon / 태온

`	ext
몬스터들이 피하던 방향과 일치합니다.
`
### main10_paul_006

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:219 / Lines
- Speaker: companion_paul / 폴

`	ext
마력의 흐름도 아래쪽으로 향합니다. 관련은 있어 보이지만 이것이 원인인지는 아직 모릅니다.
`
### main10_miel_004

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:220 / Lines
- Speaker: companion_miel / 미엘

`	ext
주변 생물 소리도 거의 들리지 않아요.
`
### M10:Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:12137

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:220 / Lines
- Speaker: player / <PlayerName>

`	ext
서두르지 말고 내려갈 경로를 확인하죠.
`
### main10_paul_007

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:221 / Lines
- Speaker: companion_paul / 폴

`	ext
돌아갈 길과 단단한 가장자리는 표시했습니다. 아래쪽은 준비해서 확인하는 편이 좋겠습니다.
`
### main10_taeon_006

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:222 / Lines
- Speaker: companion_taeon / 태온

`	ext
무너진 돌을 밟지 않도록 입구 위치를 기억해두겠습니다.
`
### main10_miel_005

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:223 / Lines
- Speaker: companion_miel / 미엘

`	ext
모두 상태를 확인하고 움직여요.
`
### M10:Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:12422

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs:223 / Lines
- Speaker: player / <PlayerName>

`	ext
입구를 확인했습니다. 준비를 마친 뒤 안쪽을 조사하겠습니다.
`

## Main11

### main11_paul_001

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:240 / AfterBoss
- Speaker: companion_paul / 폴

`	ext
멈췄는데도 주변의 빛은 그대로군요.
`
### main11_taeon_001

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:240 / AfterBoss
- Speaker: companion_taeon / 태온

`	ext
이 존재가 빛의 근원은 아니었던 것 같습니다.
`
### main11_miel_001

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:241 / AfterBoss
- Speaker: companion_miel / 미엘

`	ext
뒤쪽을 보세요. 길이 열린 것 같아요.
`
### main11_taeon_002

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:247 / Lines
- Speaker: companion_taeon / 태온

`	ext
석관과 묘표가 남아 있습니다. 실제 안치 공간으로 쓰였던 곳입니다.
`
### main11_miel_002

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:248 / Lines
- Speaker: companion_miel / 미엘

`	ext
오래된 흔적이지만 누군가 머물렀던 자리였겠네요.
`
### M11:Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:13316

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:248 / Lines
- Speaker: player / <PlayerName>

`	ext
서쪽 공간을 기록하겠습니다.
`
### main11_taeon_003

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:249 / Lines
- Speaker: companion_taeon / 태온

`	ext
무너진 돌 사이의 긁힌 자국은 다른 흔적보다 새롭습니다.
`
### main11_paul_002

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:250 / Lines
- Speaker: companion_paul / 폴

`	ext
비슷한 선이 반복되네요. 뜻은 아직 알 수 없습니다.
`
### M11:Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:13512

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:250 / Lines
- Speaker: player / <PlayerName>

`	ext
북쪽 장치도 확인해보죠.
`
### main11_taeon_004

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:251 / Lines
- Speaker: companion_taeon / 태온

`	ext
양쪽 구조를 확인했습니다. 이제 이 장치를 움직일 수 있겠습니다.
`
### main11_paul_003

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:252 / Lines
- Speaker: companion_paul / 폴

`	ext
아래로 이어지는 길입니다. 단단한 가장자리를 따라 내려가죠.
`
### main11_miel_003

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:253 / Lines
- Speaker: companion_miel / 미엘

`	ext
쓰이지 않은 안치 공간이 많아요. 위층과는 다르네요.
`
### main11_taeon_005

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:254 / Lines
- Speaker: companion_taeon / 태온

`	ext
돌을 다듬은 방식도 더 정교합니다. 안쪽부터 확인하죠.
`
### main11_paul_004

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:255 / Lines
- Speaker: companion_paul / 폴

`	ext
바닥은 지나갈 수 있습니다. 거친 부분은 표시해두겠습니다.
`
### main11_paul_005

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:256 / Lines
- Speaker: companion_paul / 폴

`	ext
닫힌 묘실인데 안치 흔적은 거의 없습니다. 다른 용도였을 수도 있겠네요.
`
### main11_miel_004

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:257 / Lines
- Speaker: companion_miel / 미엘

`	ext
폴, 이쪽 바닥은 괜찮아요?
`
### main11_paul_006

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:257 / Lines
- Speaker: companion_paul / 폴

`	ext
네. 조금 거칠지만 지나갈 수 있습니다.
`
### main11_taeon_006

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:258 / Lines
- Speaker: companion_taeon / 태온

`	ext
석재의 홈과 문양이 반복됩니다. 의미는 아직 모르겠습니다.
`
### main11_paul_007

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:259 / Lines
- Speaker: companion_paul / 폴

`	ext
용도를 정하기에는 단서가 모자랍니다. 본 것만 기록하죠.
`
### main11_paul_008

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:260 / Lines
- Speaker: companion_paul / 폴

`	ext
푸른 흐름은 이 방에서 시작되는 것 같지 않습니다. 더 안쪽에서 들어오는 듯하군요.
`
### main11_taeon_007

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:261 / Lines
- Speaker: companion_taeon / 태온

`	ext
방향은 확인했습니다. 근원은 아직 모릅니다.
`
### main11_miel_005

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:261 / Lines
- Speaker: companion_miel / 미엘

`	ext
무리하지 말고 앞쪽을 살펴봐요.
`
### main11_paul_009

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:262 / Lines
- Speaker: companion_paul / 폴

`	ext
빛이 이 구조물에서 만들어지는 건 아닌 것 같습니다. 아래에서 올라와 틈을 지나갑니다.
`
### main11_taeon_008

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:263 / Lines
- Speaker: companion_taeon / 태온

`	ext
그렇다면 지금 보이는 균열로 내려갈 수 있겠습니까?
`
### main11_miel_006

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:264 / Lines
- Speaker: companion_miel / 미엘

`	ext
아래쪽은 많이 무너져 있어요.
`
### main11_taeon_009

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:265 / Lines
- Speaker: companion_taeon / 태온

`	ext
지금 억지로 들어가는 건 위험합니다.
`
### main11_paul_010

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:266 / Lines
- Speaker: companion_paul / 폴

`	ext
동의합니다. 무너진 지하에서 길을 개척하는 취미는 없거든요.
`
### main11_paul_011

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:267 / Lines
- Speaker: companion_paul / 폴

`	ext
석판은 대부분 부서졌습니다. 글자를 읽기는 어렵겠네요.
`
### main11_miel_007

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:268 / Lines
- Speaker: companion_miel / 미엘

`	ext
밖에서 알아볼 수 있는 사람이 있을지도 몰라요.
`
### main11_taeon_010

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:269 / Lines
- Speaker: companion_taeon / 태온

`	ext
지하묘지에서 본 것과 같은 문양입니다. 가져가는 편이 좋겠습니다.
`
### M11:Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:15280

- Source: Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs:270 / Lines
- Speaker: player / <PlayerName>

`	ext
주변을 더 살펴보겠습니다.
`

## Main12

### M12:Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:13327

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:237 / TryHandleNpc
- Speaker: starter-village-general-store / 잡화 상인

`	ext
예전에 서쪽에서 온 상인이 비슷한 문양이 새겨진 돌을 가져온 적이 있어요. 뜻까지는 모릅니다.
`
### main12_paul_supp_001

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:403 / field03_main12_tablet
- Speaker: companion_paul / 폴

`	ext
지하묘지와 같은 반복 문양입니다. 아주 오래된 기록이지만 의미는 아직 알 수 없습니다.
`
### main12_paul_supp_002

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:404 / village_main12_records
- Speaker: companion_paul / 폴

`	ext
같은 문양은 맞습니다. 다만 같은 원인이라는 뜻인지는 아직 모르겠군요.
`
### main12_taeon_supp_001

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:405 / field03_main12_party_decision
- Speaker: companion_taeon / 태온

`	ext
직접 확인하는 편이 좋겠습니다. 미엘도 서쪽으로 가볼 이유는 충분하다고 합니다.
`

## Main13

### main13_serin_004

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:240 / TryHandleNpc
- Speaker: companion_serin / 세린

`	ext
저도 아르벨로 갑니다. 그곳까지 함께 움직이죠.
`
### main13_miel_001

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:243 / TryHandleNpc
- Speaker: companion_miel / 미엘

`	ext
괜찮으세요?
`
### main13_serin_001

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:244 / TryHandleNpc
- Speaker: companion_serin / 세린

`	ext
잠시만요. 바람이 강하면 말소리가 흐려져서요.
`
### main13_serin_002

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:245 / TryHandleNpc
- Speaker: companion_serin / 세린

`	ext
이 장치가 땅의 진동을 제가 알아볼 수 있는 신호로 바꿔 줘요. 중요한지는 제가 판단하고요.
`
### main13_serin_003

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:246 / TryHandleNpc
- Speaker: companion_serin / 세린

`	ext
잠깐만요. 앞에 둘, 오른쪽 뒤에 하나 더 있어요. 발밑으로 울립니다.
`
### main13_leon_001

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:254 / TryHandleNpc
- Speaker: arbel-leon / 레온

`	ext
아르벨에 오신 걸 환영합니다. 세린은 잠시 자기 조사를 마치고 오겠다고 했습니다.
`
### main13_paul_001

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:406 / field04_main13_dry_soil
- Speaker: companion_paul / 폴

`	ext
서쪽으로 갈수록 흙이 더 말라 있습니다. 계절 탓인지는 아직 판단할 수 없겠어요.
`
### main13_paul_002

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:407 / field04_main13_shallow_stream
- Speaker: companion_paul / 폴

`	ext
물길이 눈에 띄게 줄었습니다. 주변의 살아 있는 나무와 비교해 기록해 두죠.
`

## Main14

### main14_serin_001

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:250 / TryHandleNpc
- Speaker: companion_serin / 세린

`	ext
마을 안에서는 약한데 서쪽으로 갈수록 진동이 커져요. 일정한 간격으로 반복되고요.
`
### main14_taeon_001

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:251 / TryHandleNpc
- Speaker: companion_taeon / 태온

`	ext
열기는 계속 이어집니다. 같은 원인인지는 더 확인해야겠습니다.
`
### main14_leon_001

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:256 / TryHandleNpc
- Speaker: arbel-leon / 레온

`	ext
서쪽 땅이 마른 건 오래됐지만 최근 속도가 빠릅니다. 주민들이 쓸 우물부터 확인해 주시겠습니까?
`
### main14_leon_002

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:258 / TryHandleNpc
- Speaker: arbel-leon / 레온

`	ext
건조, 열기, 주기적인 진동, 도망친 야수까지 확인됐군요. 원인을 단정하지 않고 주민을 대비시키겠습니다.
`
### main14_serin_002

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:339 / Update
- Speaker: companion_serin / 세린

`	ext
그 야수는 이쪽으로 오던 게 아니에요. 서쪽에서 도망치고 있었어요.
`
### main14_miel_001

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:340 / Update
- Speaker: companion_miel / 미엘

`	ext
그럼 저쪽에 무언가 있나요?
`
### main14_serin_003

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:341 / Update
- Speaker: companion_serin / 세린

`	ext
이번 건 큽니다.
`
### main14_taeon_002

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:342 / Update
- Speaker: companion_taeon / 태온

`	ext
이번에는 저도 느꼈습니다. 아직 원인은 알 수 없습니다.
`
### main14_paul_001

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:408 / arbel_main14_old_well
- Speaker: companion_paul / 폴

`	ext
서쪽 우물의 수위가 더 낮습니다. 지하 흐름 차이는 가능성일 뿐입니다.
`
### main14_paul_002

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:409 / field05_main14_irrigation
- Speaker: companion_paul / 폴

`	ext
수로는 거의 말랐고 그늘의 바닥도 미세하게 따뜻합니다.
`
### main14_paul_003

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2IntroFlow.cs:410 / field05_main14_watchpost
- Speaker: companion_paul / 폴

`	ext
타버린 풀과 갈라진 돌입니다. 열이 잠깐 발생했거나 무언가 지나갔을 수 있습니다.
`

## Main15

### main15_leon_001

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs:152 / TryHandleNpc
- Speaker: arbel-leon / 레온

`	ext
서쪽 길을 다녀온 사람들이 땅이 뜨겁다고 합니다. 작은 불 흔적은 있지만 큰 불길을 본 사람은 없어요.
`
### main15_serin_001

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs:153 / TryHandleNpc
- Speaker: companion_serin / 세린

`	ext
저도 계속 서쪽을 확인할 생각이에요. 같이 살펴보죠.
`
### main15_serin_004

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs:156 / TryHandleNpc
- Speaker: companion_serin / 세린

`	ext
또 시작됐어요. 지난번보다 간격이 조금 짧아요. 서쪽에서 오지만 위치까지는 모르겠습니다.
`
### main15_paul_002

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs:157 / TryHandleNpc
- Speaker: companion_paul / 폴

`	ext
이 지면은 제 휠에도 별로 반갑지 않네요.
`
### main15_serin_005

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs:158 / TryHandleNpc
- Speaker: companion_serin / 세린

`	ext
땅이 뜨거워서요?
`
### main15_paul_003

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs:159 / TryHandleNpc
- Speaker: companion_paul / 폴

`	ext
타이어도 오늘은 기분이 나쁜 모양입니다.
`
### main15_leon_002

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs:162 / TryHandleNpc
- Speaker: arbel-leon / 레온

`	ext
변한 동물과 균열, 더 강한 열기와 진동이 있었군요. 원인을 단정하지 않고 주민들에게 대비를 알리겠습니다.
`
### main15_serin_007

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs:165 / TryHandleNpc
- Speaker: companion_serin / 세린

`	ext
저도 계속 서쪽을 확인할 생각이에요. 혼자보다 함께 움직이는 편이 낫겠습니다.
`
### main15_taeon_004

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs:166 / TryHandleNpc
- Speaker: companion_taeon / 태온

`	ext
그렇다면 앞으로도 함께하시겠습니까?
`
### main15_serin_008

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs:167 / TryHandleNpc
- Speaker: companion_serin / 세린

`	ext
네. 당분간이 아니어도 괜찮다면요.
`
### main15_miel_002

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs:168 / TryHandleNpc
- Speaker: companion_miel / 미엘

`	ext
물론이에요.
`
### main15_paul_006

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs:169 / TryHandleNpc
- Speaker: companion_paul / 폴

`	ext
제 일이 줄어든다면 언제나 환영입니다.
`
### main15_miel_001

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs:254 / Lines
- Speaker: companion_miel / 미엘

`	ext
몸에 불이 붙은 건가요?
`
### main15_paul_001

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs:255 / Lines
- Speaker: companion_paul / 폴

`	ext
불이 붙었다기보다 몸 자체가 변한 것 같습니다.
`
### main15_taeon_001

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs:256 / Lines
- Speaker: companion_taeon / 태온

`	ext
가까이 가기 전에 상태를 확인하죠.
`
### main15_serin_002

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs:257 / Lines
- Speaker: companion_serin / 세린

`	ext
움직임은 보통 야생동물과 비슷해요.
`
### main15_taeon_002

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs:259 / Lines
- Speaker: companion_taeon / 태온

`	ext
발자국이 동쪽으로 향합니다. 이곳에서 도망치는 듯합니다.
`
### main15_serin_003

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs:260 / Lines
- Speaker: companion_serin / 세린

`	ext
또 시작됐어요. 간격은 조금 짧고 강해졌지만 완전히 일정하지는 않아요.
`
### main15_paul_004

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs:263 / Lines
- Speaker: companion_paul / 폴

`	ext
이 아래에 열원이 있는 건 맞는 것 같습니다.
`
### main15_taeon_003

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs:264 / Lines
- Speaker: companion_taeon / 태온

`	ext
그렇다고 여기가 시작점이라고 단정할 수는 없습니다.
`
### main15_serin_006

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs:265 / Lines
- Speaker: companion_serin / 세린

`	ext
진동은 더 서쪽에서 옵니다.
`
### main15_paul_005

- Source: Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs:267 / Lines
- Speaker: companion_paul / 폴

`	ext
바위는 일부 검고 식물은 말랐습니다. 큰 산불 흔적 없이 지면의 열이 오른 것 같군요.
`

## Main16

### main16_start_leon_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:25 / Get/shared
- Speaker: arbel-leon / 레온

`	ext
바람이 거의 없는데 재가 움직였다는 증언이 있습니다. 큰 불길을 보지 못했는데 땅은 뜨겁다고 하고요. 검은 연기 같은 형체를 봤다는 소문도 들립니다.
`
### main16_start_leon_02

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:26 / Get/shared
- Speaker: arbel-leon / 레온

`	ext
평소 같으면 잘못 본 거라고 생각했겠지만, 요즘은 그렇게 넘기기가 어렵군요.
`
### main16_start_paul_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:27 / Get/shared
- Speaker: companion_paul / 폴

`	ext
확인할 수 있는 건 직접 확인하죠. 소문이 사실인지부터요.
`
### main16_ash_hearing_player_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:31 / hearing
- Speaker: player / 플레이어

`	ext
진동하고 재가 움직이는 간격이 같아요.
`
### main16_ash_hearing_serin_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:32 / hearing
- Speaker: companion_serin / 세린

`	ext
저도 그렇게 잡혀요.
`
### main16_ash_hearing_serin_02

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:33 / hearing
- Speaker: companion_serin / 세린

`	ext
다만 어느 쪽이 먼저인지는 아직 모르겠습니다.
`
### main16_ash_default_observation_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:36 / default
- Speaker:  / 현장 관찰

`	ext
세린이 장치를 조절해 지면 신호를 확인한다.
`
### main16_ash_default_serin_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:37 / default
- Speaker: companion_serin / 세린

`	ext
땅이 울릴 때마다 움직여요.
`
### main16_ash_default_taeon_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:38 / default
- Speaker: companion_taeon / 태온

`	ext
진동과 관련이 있다는 말씀이십니까?
`
### main16_ash_default_serin_02

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:39 / default
- Speaker: companion_serin / 세린

`	ext
같이 일어나는 건 맞아요. 어느 쪽이 원인인지는 모르겠습니다.
`
### main16_ash_default_player_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:40 / default
- Speaker: player / 플레이어

`	ext
움직이는 재와 바닥의 변화를 함께 기록해 두죠.
`
### main16_vibration_hearing_player_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:44 / hearing
- Speaker: player / 플레이어

`	ext
서쪽으로 갈수록 신호가 강해져요. 반복되는 간격도 앞에서 확인한 것과 달라요.
`
### main16_vibration_hearing_serin_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:45 / hearing
- Speaker: companion_serin / 세린

`	ext
네. 방향은 같습니다. 다만 어디에서 시작되는지는 아직 모르겠습니다.
`
### main16_vibration_default_serin_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:48 / default
- Speaker: companion_serin / 세린

`	ext
서쪽에서 오는 진동이 더 강해요. 간격에도 변화가 있습니다. 발생원은 아직 모르겠습니다.
`
### main16_vibration_default_player_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:49 / default
- Speaker: player / 플레이어

`	ext
그러면 서쪽 흔적과 비교하며 확인하죠.
`
### main16_tracks_paul_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:53 / Get/shared
- Speaker: companion_paul / 폴

`	ext
여기까지는 계속 서쪽으로 갔습니다.
`
### main16_tracks_miel_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:54 / Get/shared
- Speaker: companion_miel / 미엘

`	ext
그런데 여기서 전부 방향을 바꿨네요.
`
### main16_tracks_taeon_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:55 / Get/shared
- Speaker: companion_taeon / 태온

`	ext
무언가를 피한 것으로 보입니다.
`
### main16_witness_observation_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:59 / Get/shared
- Speaker:  / 현장 관찰

`	ext
갈라진 지면과 그 위의 재를 조사한다. 발밑에 약한 진동이 전해진다.
`
### main16_witness_observation_02

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:60 / Get/shared
- Speaker:  / 현장 관찰

`	ext
바람은 거의 없는데 주변의 재가 한곳으로 모인다.
`
### main16_witness_observation_03

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:61 / Get/shared
- Speaker:  / 현장 관찰

`	ext
모인 재 속에 작은 불씨가 섞인다. 검은 연기와 재가 느슨한 형체를 이루기 시작한다.
`
### main16_witness_observation_04

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:65 / Get/shared
- Speaker:  / 현장 관찰

`	ext
검은 연기와 재 속 붉은 불씨가 움직인다. 불씨망령이 모습을 드러낸다. 정확한 정체는 알 수 없다.
`
### main16_witness_taeon_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:66 / Get/shared
- Speaker: companion_taeon / 태온

`	ext
모두 준비하세요. 저 형체가 다가옵니다.
`
### main16_retry_taeon_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:68 / Get/shared
- Speaker: companion_taeon / 태온

`	ext
아직 그 형체가 남아 있습니다. 준비가 되면 다시 상대하죠.
`
### main16_afterimage_hearing_player_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:72 / hearing
- Speaker: player / 플레이어

`	ext
아직 계속돼요.
`
### main16_afterimage_hearing_serin_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:73 / hearing
- Speaker: companion_serin / 세린

`	ext
네. 저도 잡힙니다.
`
### main16_afterimage_default_serin_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:76 / default
- Speaker: companion_serin / 세린

`	ext
아직 계속됩니다.
`
### main16_afterimage_default_player_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:77 / default
- Speaker: player / 플레이어

`	ext
형체는 사라졌는데 진동은 남았군요.
`
### main16_afterimage_paul_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:81 / Get/shared
- Speaker: companion_paul / 폴

`	ext
그러면 저게 진동을 만든 건 아니군요.
`
### main16_afterimage_taeon_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:82 / Get/shared
- Speaker: companion_taeon / 태온

`	ext
적어도 모든 원인은 아니라는 뜻이겠습니다.
`
### main16_afterimage_observation_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:83 / Get/shared
- Speaker:  / 현장 관찰

`	ext
균열과 열기, 진동은 남아 있다. 균열 안쪽에서 미세한 따뜻한 공기 흐름이 올라온다. 표면에는 그을림이 있지만 큰 산불 흔적은 없다.
`
### main16_afterimage_taeon_02

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:84 / Get/shared
- Speaker: companion_taeon / 태온

`	ext
열기의 일부가 지하에서 올라오는 것일 수 있습니다. 아직 가설입니다.
`
### main16_canyon_observation_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:90 / Get/shared
- Speaker:  / 현장 관찰

`	ext
서쪽 끝에 붉게 갈라진 절벽과 재, 강한 아지랑이가 보인다. 가칭 붉은 균열 협곡의 입구다. 오늘은 안쪽으로 들어가지 않는다.
`
### main16_canyon_hearing_player_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:94 / hearing
- Speaker: player / 플레이어

`	ext
협곡 방향에서 오는 진동이 더 강해요.
`
### main16_canyon_hearing_serin_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:95 / hearing
- Speaker: companion_serin / 세린

`	ext
네. 그 방향과 반복이 같이 잡힙니다. 정확한 발생 위치는 아직 모릅니다.
`
### main16_canyon_default_serin_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:99 / default
- Speaker: companion_serin / 세린

`	ext
진동은 붉은 균열 협곡 방향에서 더 강해요. 정확한 발생 위치는 아직 모르겠습니다.
`
### main16_canyon_default_player_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:100 / default
- Speaker: player / 플레이어

`	ext
입구 위치를 기록하고 아르벨에 알리죠.
`
### main16_canyon_taeon_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:102 / Get/shared
- Speaker: companion_taeon / 태온

`	ext
오늘은 여기까지 확인하는 편이 좋겠습니다.
`
### main16_canyon_paul_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:103 / Get/shared
- Speaker: companion_paul / 폴

`	ext
찬성입니다. 확인과 무모함은 다른 일이니까요.
`
### main16_canyon_miel_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:104 / Get/shared
- Speaker: companion_miel / 미엘

`	ext
아르벨에 먼저 알려야겠어요.
`
### main16_report_player_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:108 / Get/shared
- Speaker: player / 플레이어

`	ext
바람 없이 재가 움직였고 진동과 동시에 일어났습니다. 불씨망령을 발견해 쓰러뜨렸지만 진동은 남았습니다. 더 서쪽에는 붉은 균열 협곡 입구가 있습니다.
`
### main16_report_taeon_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:109 / Get/shared
- Speaker: companion_taeon / 태온

`	ext
열기의 일부가 지하에서 올라올 가능성은 있습니다. 가설일 뿐이며 재와 진동의 원인, 형체의 정체는 아직 모릅니다.
`
### main16_report_leon_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:110 / Get/shared
- Speaker: arbel-leon / 레온

`	ext
그러면 동물들만 변하고 있는 게 아니군요.
`
### main16_report_paul_01

- Source: Unity/Client/Assets/_Project/Scripts/World/Main16DialogueCatalog.cs:111 / Get/shared
- Speaker: companion_paul / 폴

`	ext
네. 적어도 이제는 그렇게 보는 편이 맞겠습니다.
`
