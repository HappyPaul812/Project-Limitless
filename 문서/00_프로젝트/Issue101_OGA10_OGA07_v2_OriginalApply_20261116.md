# Issue #101 — 승인 OGA10·OGA07 v2 원본 게임 적용 (2026-11-16)

**APPROVED_V2_LOCAL_ORIGINAL_APPLIED / DELTA_53_PASS / ORIGINAL_CONSOLE_ERRORS_0 / USER_REALPLAY_PENDING / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

## 승인·범위·로컬 반영

바울 대표의 이번 “OGA10 파티관리인과 OGA07 주민1 최종 미술 승인 / 실제 로컬 원본 안전 적용” 지시를 승인 근거로 삼습니다. [Issue101 최신 기획 검수](https://github.com/HappyPaul812/Project-Limitless/issues/101#issuecomment-6912055996)와 [Codex F: 최종 검수](https://github.com/HappyPaul812/Project-Limitless/issues/101#issuecomment-6911981684)를 먼저 확인했습니다. 해당 문서의 이전 PENDING보다 이번 직접 승인 지시가 우선합니다. 다른 아트 승인/적용으로 확대하지 않습니다.

- Unity **6000.6.5f1** 유지. 기존 #57/#95/#99의 독립 GUID·Resources 외형 매핑 방식 재사용.
- 시작 원본 main: `669adc275fc173f3292b2f714710434ce9895509`.
- 브랜치: `codex/npc101-approved-original-20261116`. 기존 소형 제품 worktree `Temp/Issue99NPC20261116/Worktree`를 전환해 재사용, 신규 프로젝트/worktree 복제 없음.
- 제품 커밋: **`81f8a45df4182ad2aa64a71b5408217616b5b3d0`**. Delta PASS 뒤 원본에 FF-only/no-autostash 반영 완료. main 직접 commit·병합 commit·강제 reset·stash·원격 Push/PR Merge 없음.
- 본 보고/CODEX_LATEST/CURRENT_STATUS는 후속 Docs 커밋 후 원본에 FF-only 동기화. 최종 SHA는 Issue101 완료 댓글과 `Temp/Issue101NPC20261116/Final.json`에 기록합니다.

## 입력과 실제 변경 파일

`F:/Downloads/npc101_fix.zip`: **287887bytes /2entries /SHA256 ee98f2bde1062193f3b9100d9af67ca3b9fc2f17342c5ee30449193d20b2be3e /CRC PASS**. 재포장본이므로 Manifest/SUMS 미포함은N/A. OGA10.png/OGA07.png 개별 SHA가 승인 정본과 일치합니다.

신규 PNG(+각 .meta) 경로 기준: `Unity/Client/Assets/_Project/Resources/VillageNpcSprites/Eldiran/`.

| 신규 PNG | 승인 원본 SHA256 | bytes | 새 GUID |
|---|---|---:|---|
|OGA10_PartyManager_HandFix_v2_Approved.png|6be1a2d777a312dedd392b8ad6fbb2ba77b350d46d7bd2113d9d11bf09c7e6f3|146101|012b7ee363574fe3900bbd56a472b984|
|OGA07_Resident01_HandFix_v2_Approved.png|640b82a236afd5ca6025d5eb28970009f40941d39eadedeeeea0e180d5034d28|143720|d3efb2534cda4b699e8f0ba236325209|

PNG 원본 바이트 그대로 등록. crop·resample·그림 수정0. 추가1파일 `Unity/Client/Assets/_Project/Scripts/NPC/VillageNpcAppearanceCatalog.cs`에서 `starter-village-party-manager`, `starter-village-resident-01`의 Resources 문자열 **2개만** 교체했습니다. 총 제품5파일(신규PNG2/meta2+기존카탈로그1). 기존 구 OGA10/07 PNG/meta는 그대로 유지.

## 가시 높이·Pivot

두 PNG 각각512×512 RGBA. alpha>8 bbox: OGA10 `[140,58,371,470)`, OGA07 `[165,58,346,470)`. 가시높이412px, 기존32/28=1.142857WU에 맞춰 **둘 다 PPU360.5**. 가시영역 아래중앙을 기준으로 **정규화 Pivot(.4990234375,.08203125), 픽셀Pivot(255.5,42)**. 다른 승인 NPC와 같은 높이/지면 기준이며 가로 비율/의상 실루엣은 원화대로 보존. 미세 alpha halo는 제거하지 않았습니다. Point/FullRect/SingleSprite/mipmap없음, 독립 GUID 중복0.

## 최소 Delta QA 결과

기존 `Temp/Art95_91Unity20261111/QA/Client`와 기존 배치 캡처/가상입력 도구 재사용. 제품5파일과 임시QA Probe만 반영했습니다. 원본 Editor의 Scene/Play/선택/포커스는 변경하지 않았습니다.

- **53/53 PASS**: 두 NPC Catalog 자동선택·Importer/PPU/Pivot·가시높이/발 접지·960/1280/1680×720 전후12장·Nameplate/Collider/transform/sorting·키보드E/Esc·패드A/B 대화·기존6승인NPC/다른참조·Item가격·Arbel 구OGA10·마을재진입.
- OGA10 파티 안내인의 이름표는 기존대로 표시. OGA07 일반주민의 상시 이름표는 **기존 정책대로 숨김**, 대화창 화자는 ‘주민’으로 정상 표시. 숨김을 오류로 보고 임의 표시하지 않았습니다.
- 실제 대화: OGA10 초기진행 “동료와 함께 돌아오시면 편성을 도와드리겠습니다.” / OGA07 “광장은 누구나 쉬어 갈 수 있는 곳이에요.”. 기존 진행조건에 따른 파티 UI 분기 자체는 변경0, 이번 검사는 초기진행 대화 경로 중심이며 해금 이후 파티 편성 전수 QA는 반복하지 않았습니다.
- 첫2회 검사 실패는 QA 도구가 동적 PartyManager 대사를 NPC에 저장된 정적 안내문과 비교했기 때문입니다. 진단에서 실제 대화 열림/문구를 확인하고 기존 `VillageNpcRole` 진행분기와 일치하도록 **검사 기대값만 수정**. 중간에 의뢰 선택창 가능성도 진단했으나 실제 실패 지점에서는 side=False, dialogue=True였습니다. 최종53PASS, 게임코드 보정0.
- 실제6해상도 화면과 대화2장 육안 확인. 승인 원화의 세밀한 눈/옷 질감은 그대로이며 기존 승인6종 대비 가시높이·접지·화면 배치에 새로운 스케일 이질감/잘림 없음. 이는 미술 재승인이나 기존 화풍을 완전히 동일하게 바꿨다는 뜻이 아닙니다.
- QA 게임 Runtime 오류0. 재사용 배치환경의 기존 `UnityEditor.Search.SearchDatabase` 색인 ArgumentOutOfRangeException은 **환경 오류로 별도 보관**(전체 QA Console 무오류라고 주장하지 않음). 패키지·Search 캐시를 임의 수정/삭제하지 않았습니다.
- **원본 Unity 표적 Import 후 Console error0 / compile idle / 실제 Resources.Load·새GUID·PPU·Pivot·컴파일된StableID매핑 PASS**. Issue85 패키지 변경0/HOLD 유지, 이번 표적 Import 차단없음.
- `arbel-party-guide`, `safezone-catacomb-party`는 구OGA10/PPU28/기존GUID 연결 유지. 원본 Resources에서도 확인했습니다. 이번 적용 범위는 시작마을2StableID뿐입니다.

증거: `Temp/Issue101NPC20261116/Evidence/checks.txt`, status.txt, Errors.txt(0), EnvironmentErrors.txt, InteractionDiagnostics.txt, 전후/대화/Overview15PNG. `Unity-final.log`, original-resources.json, original-console.json, original-state-after.json, protection-after.json. QA 코드/합성 Save/그림 증거는 제품 커밋에서 제외.

## 보호·디스크·바울 대표 확인

- 원본 Assets/Packages/ProjectSettings/UserData **4678파일 SHA 대조**, 허용 카탈로그 외 기존파일 변경0. 사용자 dirty/untracked git status 바이트 동일. 신규PNG SHA 원본일치.
- 승인6종 OGA02/03/06/09/16/17·HOLD OGA20·Scene/대사/상호작용/Collider/StableID/가격·기존Save/UserData 보존. QA Save/Settings는 `Evidence/SyntheticSave`, `Evidence/SyntheticSettings`에 격리. 외부 개인정보 저장파일 전수 해시 감사는 수행하지 않았으며 원본 외부 Save/Settings 사용/쓰기0.
- Discord 파일 접근/변경0. 기존백업/Library/Temp 대량복사·삭제0. 기존 QA Import캐시/소형worktree 재사용, 새Unity패키지 설치0. F: 시작322476666880bytes → 원본Import후322470690816bytes 여유(약5.98MB 감소). 최종 실제값은 Final.json.
- 원본 Editor 버전/기존외부Bootstrap Scene경로/dirtyFalse/PlayFalse/selectionNull 유지. 원본에서 강제 Play/Scene전환 검증0, 실제 실행은 격리 배치에서 수행.
- **바울 대표 실게임 확인 위치: 시작마을 광장 서쪽**, 파티 안내인 **(-6.4,-2.1)**, 주민1 **(-3.2,-1.2)**. E/F 또는 패드A로 기존 대화, Esc/패드B로 닫기. 주민1은 상시 이름표가 없는 기존 정책입니다.
- 사용자 미술승인과 이번 로컬 적용은 완료, 바울 대표 직접 실게임 확인은 **PENDING**. 원격Push/PRMerge 없음.
