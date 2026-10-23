# LIMITLESS / 2026-10-23 아침 — Chapter1·2 비고유 자산 잔여 브리핑 + Chapter3 Main23 First Playable 착수

작성 브랜치: `docs/planning`. 바울 대표 직접 지시: **“굿모닝~ 오늘도 열심히 하자 우리. 먼저 리뉴얼 쪽은 우리의 고유 자산 말고 뭐가 남아있나 브리핑해주고, 챕터3는 계속 진해.”**

**출발점:** 2026-10-22 EOD PR#54 제품·PR#55 문서 병합 및 사용자 원본 SYNC_COMPLETE. GitHub `main` `47ad2eecb40135b2ee28e4947d642ebdbf0bf68a`. 실제 원본 Editor 자연 Import/사용자 손플레이는 미검증. 사용자 Windows 로컬 Git/Unity를 기획이 직접 조작하지 않음. 다음 Codex 단계는 main의 승인 자산·기존 Quest/Save/원본 dirty를 보호한다.

## A. Chapter1·2 리뉴얼 — 고유 자산 제외 후 실제 남은 후보

이 표는 **파일이 존재**하는 것과 **실게임에서 참조·렌더**되는 것, **바꿔도 안전한 것**을 구분한다. `Assets/ThirdParty` 라이브러리에 원본이 남아 있다고 해서 제품이 전부 외부 스타일인 것은 아니다. 오리지널화가 필요한 대상 수는 런타임 참조·권리 감사 뒤에만 확정.

| 분류 | GitHub/기존 문서에서 확인한 내용 | 우선순위·판정 |
|---|---|---|
| 마을 일반 NPC | `Resources/VillageNpcSprites/Eldiran/`에 **공용/무료 NPC 12 PNG** (상점·치유·은행·파티·훈련·남문 경비·주민대표·주민4). 원본 `ThirdParty/Eldiran` CC0. 시작마을 환경 v4는 승인·적용됐으나 NPC12 외형이 자동 오리지널화된 것은 아님 | **가장 명확한 제작 후보**. 실제 NPC Scene 렌더12 및 Stable ID/Quest 보호 확인 뒤 2종(안내/상인) 미술 파일럿 → 대표 시각승인 → 전체 12확장 |
| 몬스터 | `Resources/MonsterDefinitions`에 Chapter1·2 **18종**. 11~15 공식 시트5와 16~18 보스·후반 정의, 01·05·06·07 일부 개별 PNG 존재. 현행 출처·사용 승인 미확정인 Chapter1 초반 `venom_bee`, `forest_spider`, `venom_snake` 등은 우선 출처 감사. `ThirdParty/2DPIXX/PilotBee`는 README상 **실 Field/전투 미연결**·CC BY 라이선스 버전 표기 불일치 | **실사용 비고유 몬스터만** 아트 후보. 18종 전면 교체 금지. VenomSnake v3는 이전 PASS 권고 수준이지 사용자 정식 승인·실전 최종시트 아님, 저알파 잔여 및 실제 포즈/프레임 계약 점검 |
| 야수 동료 | 기존 `ThirdParty/ScratchIO/AnimatedWildAnimals` 기록이 있으나 **Wolf v5·Bear v4·Fox v7의 승인 Run6 제품 적용·10/22 main 통합 완료**. 예전 CompanionAssaultValidation README는 낡은 후보 설명이라 실제 Resource 코드/참조가 정본 | **기본 3종 Run6은 리뉴얼 완료·재작업 금지**. 확장형 몬스터 펫이 별도 외부 미술 쓰는지 실제 PetService와 Runtime 참조를 조회할 때만 후보 선정 |
| 전투·상태 VFX | `_Project/Resources/BattleSkillEffects/`에 PVFX Foundry CC0를 기반으로 `SpectralBloom`, `RadiantHeal`, `WarmExplosion`, `ElectricImpact`, `EarthRupture` 등 그리드·추출 PNG 실제 등록. 아이콘과 타격 frame 연동은 `PROJECT_USAGE.md`에 근거 | **고유 VFX 제작 장기 대상**, 현재는 타이밍·상태이상·데미지 마커 보호 후 효과군별 교체. 단순 CC0 자산이란 이유로 긴급 제거 필요 없음 |
| 아이콘·UI | `ThirdParty/GameIconsNet/Material/White`의 **재료 아이콘4**, Path trait White/Black **10**, Kenney Board/Game BattleIcon·화폐 토큰; Game-icons.net은 **CC BY 3.0 크레딧 유지**, Kenney CC0 | **이질감이 심한 사용중 UI만 교체 대상**. Path의 자체 공식 문장5와 외부 trait아이콘5는 별개. 출처/크레딧 보존 |
| 환경 소품·외부 원본 라이브러리 | Kenney RPGBase/Schwarnhild BasicHandDrawn/Xariami EssentialRPG 원본이 `ThirdParty`에 보존. 시작마을 환경은 별도 V4 오리지널 Sprite15·Renderer300 갱신. Xariami EssentialRPG 폴더에 라이선스 근거 미확보 기록 존재 | 보관된 원본 전체 삭제/재제작 금지. **실제 활성 Sprite GUID 참조**가 남은 위치만 잔여 분류, 라이선스 불명은 외부 배포·추가 사용 보류 |
| 남은 환경 QA | Field09 승인 5+B Glow는 main 반영; **HeatVent/Obsidian Spire** 미술 잔여 [#38], Field10 공식 13아트 런타임 등록 후 **사용자 최종 미술 검토 필요**. Field08 기존 공식7 KEEP, Field11 기존 공식 Boss/환경 KEEP, Chapter1 이미 적용 Scene 재생성 금지 | 새 맵 전면 재제작0. Scene별 국소 결함/최종 확인만 개별 backlog |

**추천 생산 순서:** ① Codex *읽기전용* 런타임 외부참조/저작권 매핑 (우선 Eldiran NPC12·몬스터18·VFX·아이콘) → ② 리뉴얼 아트 담당 **Eldiran NPC 2종 독립 고유 미술 샘플** (사용자 승인은 별도) → ③ 외부 원본 사용 중인 초반 몬스터 1종(조사로 확정) → ④ VFX·아이콘 → ⑤ Field09·Field10 잔여 시각 QA 병행. 사용자 최종 승인 없이 기존 NPC·MonsterDef/AI·Quest·GUID/meta·3펫 공식 자산/Save 변경 금지.

## B. Chapter3 — Main23 「따뜻한 땅을 떠난 자들」 첫 플레이 구현 착수

현재 main에 **FrostwayVillageV10 승인 Sprite9·Tile9(+meta) 리소스 등록 완료**, 그러나 **`World_FrostwayVillage.unity`는 아직 main에 없음**, Main23 Quest·NPC·SafeZone·마을 Scene 자체 미구현. `World_NorthF1.unity`와 Main22 첫 플레이는 10/21 병합됨. 오래된 프리플라이트 문서의 ‘마을 미술 미승인’은 과거이며 **현재는 지면 Candidate9만 바울 대표 최종 승인**, 건물·서비스용 49패키지 전체 최종승인/실제 Scene 배치와 구별.

**2026-10-23 Codex 별도 격리 범위 FirstPlayable 단계:**
1. 실제 main 최신 QuestService/MonsterDefs/Main22 F1, SaveService, Tilemap/PPU128/셀2WU, BuildSettings/Unity 6.6 importer를 재조사하고 Main23 기술 예약 ID 20종/충돌·기존 BuildSettings 개인설정 보호 확인. 이전 `Chapter3_Main23_Village_Technical_Contract_20261021.md` 참조하되 실제 구현 필요가 증명된 부분만 수정.
2. `World_NorthF1` 기존 Main22 남부/쉼터/지정전투를 재사용하며 북부 심부 동선+두 개 서로 다른 조사 흔적을 배치. **Main23 5목표**: 심부 도착→동물·짐수레/장작 흔적2 독립조사→서릿길 마을 남문 입장→주민 기온/남하 증언과 태온·하렌 판단 장면→안전거점 등록/Save 후 완료. 기존 Main22 Quest/hud/Save 내용·서리들쥐 전투/몬스터 보상 변경금지.
3. 승인된 Sprite9+Tile9로 신규 `World_FrostwayVillage` **격리 첫 플레이용 지면·기본 길/문·출입·안전서비스 구역**을 구성. 아직 승인되지 않은 건물·상점 전체 환경49 및 NPC 외형은 승인된 공식 자산으로 오인/영구승격 금지: 필요한 표현은 임시 기능표시/코덱스 test-only Gizmo 또는 기존 허용 공식 자산을 변경 없이 참조하고 `VISUAL_PLACEHOLDER` 표시. 새 건물을 AI로 생성하지 않는다. 실 타일 WorldUnit=2, PPU/Pivot 맞춤과 반복 무늬/길 판독성, 16:9·4:3·21:9 동배율 카메라 검사.
4. 남문=F1 왕복 및 진입 gate(명시입력); 북문 F2와 동쪽 포탈/7×7WU 예약 부지는 **물리/입력/SceneTransition 비활성**. 마을 내부 전투·Spawn0, NPC 안내/거점 등록/기존 파티·펫 서비스 가능한 동작만 실제 코드 계약에 맞춰 최소 연결. 장작·신규 옷/집 아트 승인 전 기능 표식으로 대체 가능. 스킬 상점은 안내·예정표시만, **구매/탈렌트 차감·가짜 스킬상품 구현 금지**.
5. Main23 주민/하렌/태온 서사 **임시 텍스트**는 기획 story v1의 실제 5목표에 맞추되 캐릭터·화자 ID/TTS 확정이라고 주장하지 않음. 미엘의 과거 공개·‘잘.했.어.’·F2 한기·설원족제비 지정전투·추가 신화/온기의 화로 보스 설정 금지. 전투 파티3명 고정.
6. Main22 완료 Save에서 Main23 Available/시작, F1 중간흔적 Save/Continue, 중복흔적·역순·도망/패배·Scene 간 왕복·주민 증언 후 자동완료 방지·체크포인트 등록/패배복귀·Continue·안전구역/Spawn, 휠체어/마우스 클릭/키보드 E/F·취소를 **별도 QA Save**로 검사. 구버전 세이브 optional fields 기본값. Main24/skill ownership save persistence **Issue #46**는 별도 개발 게이트로 남김; Main23과 병행 조사 가능해도 코드를 무단 대량 개편 금지.
7. **격리 작업만** `codex/chapter3-main23-first-playable-20261023` 및 문서·QA evidence. 사용자 원본 Unity/Editor/Play/포커스/Save/UserData/미커밋·HEAD/index, Wolf/Bear/Fox/F1·Chapter1/2 Scene을 변경하지 않으며 main 직접 commit/Push/PR merge 금지. 기존 Main22 136/F1 64/Mouse40/Bear154/Fox162/EOD Unity15 PASS는 변경 영향 외 재검사0.
8. **보고**: 신규 Scene 및 타일 Sprite 참조/5목표/실제 Unity 화면·3비율/남문 왕복/Save, 신규 PASS/FAIL, 반려/HOLD인 미술·미구현 가격·대사·Main24 차단 목록, 브랜치·커밋·원본 SHA 보존. `MAIN23_ISOLATED_FIRST_PLAYABLE`은 격리 검증 전까지 미선언. 첫 플레이 완료 후 **바울 대표 시각·플레이 확인을 거쳐** 별도 본게임 적용/PR 승인.

## C. 역할/추적

- 기획: `docs/planning` 상태·프로토타입 게이트·마감 인계 관리.
- Codex: Chapter3 격리 구현/QA 및 Chapter1·2 read-only 비고유 Sprite GUID 맵핑 독립 worktree. 가능하면 두 분석과 구현을 병렬, 사용자 Editor/CPU 충돌 방지.
- 리뉴얼 아트: Chapter1·2 신규 샘플만 `docs/art-renewal`; Chapter3 신규 아트와 중복 제작 금지.
- 신규 아트: Chapter3 미완 건물·NPC 신규 원화는 후속 미술 심사 분리. 마을 승인 Candidate9을 전면 재작업하지 않음.
- 끝에 main EOD 병합/로컬 Sync는 별도 사용자 지시. 문서 입력만 `docs/planning`, 작업 담당은 자체 브랜치로 보존.

**정본 문서:** `LIMITLESS_Chapter2_Original_Art_Roadmap_20261012.md`, `Chapter1_2_Monsters_And_Beasts_Original_Art_Renewal_Assignment_20261019.md`, `Chapter3_Main23_Village_Technical_Contract_20261021.md`, `Chapter3_Main23_24_서릿길마을_스토리시퀀스_v1_20261019.md`, 10/22 EOD 완료보고, [Issue #45](https://github.com/HappyPaul812/Project-Limitless/issues/45), [Issue #46](https://github.com/HappyPaul812/Project-Limitless/issues/46).
