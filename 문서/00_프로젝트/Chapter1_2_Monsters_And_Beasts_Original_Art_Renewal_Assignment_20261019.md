# LIMITLESS — Chapter1·2 몬스터 + 야수 동료(펫) 오리지널 아트 리뉴얼 발주 (2026-10-19)

- 기획 담당 `docs/planning`, 실 제작 총괄 `docs/art-renewal`, Codex = 로컬/Unity Import·검증, Chapter3 신규 아트 = `docs/art-new` 별도.
- 발주 근거: 바울 대표 “그럼 리뉴얼에서는 맵은 완료지? 다음 리뉴얼은 몬스터들이랑 펫으로 하자.”
- 상태: `ENVIRONMENT_NEW_ART_PRODUCTION_PAUSED / ENVIRONMENT_TECH_CLOSEOUT_PENDING / MONSTER_AND_BEAST_AUDIT_AUTHORIZED / NONOFFICIAL_REPLACEMENT_ART_PRODUCTION_AUTHORIZED_AFTER_SOURCE_AUDIT / ORIGINAL_UNITY_NOT_APPLIED / USER_ART_APPROVAL_REQUIRED`.
- 역할 충돌 방지: 본 지시는 **환경 맵의 전체 Unity 완료 선언이 아니다**. Field06 Spawn 편차 약0.455(P1), Field07 Runtime Ash7 사각형 1FAIL, 두 지역 B42 격리 검증·원본 통합, Field09 지면/가이드 확인, Field10~11 최종 미술 마감 필요성을 별도 보류·감사 게이트로 유지. 환경 새 PNG를 자동 발주하지 않으며 기존 공식 자산은 보존.
- 정본: `문서/00_프로젝트/LIMITLESS_Chapter2_Original_Art_Roadmap_20261012.md`, `문서/00_프로젝트/팀운영/담당/WORLD_ART_RENEWAL.md`, `CURRENT_STATUS.md`, `AGENTS.md`와 관련 몬스터·펫 QA/아트 승인 문서.

## 1. 실제 소스 확인 — 새 그림 제작 전 재사용·승인 감사 필수

GitHub `main` 기준 `Unity/Client/Assets/_Project/Resources/MonsterDefinitions`에는 **01~18 총18개 정의**가 등록돼 있다. 몬스터별 `MonsterId/displayName/Level/fieldSprite/FieldAnimatorController/idleSheet/walkSheet/attackSheet/explicitIdle/Walk/Attack/Hit/Defeat frame GUID+fileID/ResourcePath/Spawn refs/Monster animation importer`를 읽기 전용으로 감사하고, 실제 씬·전투 런타임에 그려지는 리소스를 분류한다.

1차 목록:
- Chapter1/기초: `grass_slime`, `venom_bee`, `forest_spider`, `venom_snake`, `moss_beetle`, `shade_bat`, `grave_wight`, `monster_silent_echo`, `seal_guardian`, `silent_warden`.
- Chapter2/후반: `soot_hound`, `heatwind_hawk`, `fissure_lizard`, `ember_beetle`, `ember_wraith`, `obsidian_beetle`, `scorching_watcher`, `veinfire_colossus`.

**주요 경계:** `Art/Monsters/11_SootHound ... 15_EmberWraith`에는 전용 공식 시트5개가 이미 존재. `MonsterDefinitions/16_ObsidianBeetle`·`17_ScorchingWatcher`·`18_VeinfireColossus`도 Sprite GUID와 Idle/Walk/Attack/Hit/Defeat 등 실제 프레임 배열을 참조하며, Main20 보스는 v3 공인 아트가 있다. `01_GrassSlime_Walk`, `05_Moss_Beetle_Battle_Final`, `06_Shade_Bat_Battle_Final`, `07_Grave_Wight_Battle_Final`, `MonsterSheets`의 봉인 수호자/침묵 잔영 관련 시트도 이미 존재. **존재만으로 사용자 최종 승인이라고 단정하지 말고 승인 근거/원본 저작권/현 Runtime 모습 구분**. 기존 공식 승인 주요 5종·보스·캐릭터를 명시 승인 없이 재작업하거나 지우지 않는다. 없는 프레임 수나 일괄 128px·4×4시트 규격을 임의 강제하지 않는다.

**펫 실제 코드:** `Scripts/Battle/BeastCompanionSystem.cs`는 기본 `wolf/bear/fox`에 대해 `Resources/CompanionAssaultValidation/{Wolf,Bear,Fox}_Run` 사용. 이 Run 시트는 `Resources/CompanionAssaultValidation/README.md`에 의하면 `ThirdParty/ScratchIO/AnimatedWildAnimals/All.zip` 유래의 **기존 임시 검증 자산**, 각 가로 64px/12fps/기본 왼쪽 향함/밑중앙 pivot/전투표시 2배. 현재 코드가 실제 `CompanionAssaultValidation` 경로를 사용하므로, 옛 README 문구의 “전투와 연결되지 않음”을 **최신 코드보다 우선하지 않는다**. 꼭 `BeastCompanionCatalog`, `BeastCompanionService` 실제 선언 위치, `PetFacilityPresenter`, `SafeZonePetServiceRollout`, `CompanionAssault`와 `MonsterDefinitions`의 펫 가능 종을 감사해 동작·상업적 라이선스 위험을 확인한다. 향후 월드/전투/작은 UI 아이콘에서 각 동물의 구별 가능성 테스트.

**펫 자격/규칙:** 사수 전용이지만 세린이 사수이므로 **주인공이 다른 직업이라도 세린 보유 시 접근 가능**. Wolf/Bear/Fox 기본3종, 발견/획득한 야수형 몬스터 확장 여부는 `BeastCompanionService.IsMonsterPet`의 최신 실제 데이터로 판단. 불씨망령은 펫이 아님. 펫 파티 슬롯0, 독립 턴0, HP0, 종별 전투 패시브/구매비용·탈렌트100/보유/장착/Save·동료의 습격 스킬 기존 동작 유지.

## 2. Phase A — 전체 판정표(몬스터18 + 펫3), 소스 실사 우선

두 라인은 **병렬 분석**하고 납품은 별도 ZIP/문서. 표 1행당 1개 실제 작품:
- `id / 종류 / 챕터 / 실제 Runtime 표시 위치(필드·전투·UI·펫 Run) / Asset paths / GUID+fileID / 권리 출처(공식 자체/승인 외부/ThirdParty/불명) / 사용자 기존 승인 여부(근거 문서·커밋) / Sprite 사양/프레임수 및 Grid/Pivot·PPU / 현재 QA 근거 / 결함(실제 증거) / 권장 KEEP·TOUCH_UP·ORIGINAL_NEW·HOLD_UNVERIFIED / 예상 통합위험`.
- 상태 `OFFICIAL_KEEP` 기존 승인/사용성 정상, `NEEDS_SMALL_FIX` 승인 아트 국소 이상, `THIRD_PARTY_REPLACEMENT_CANDIDATE` 아직 독자 그림 아님, `PLACEHOLDER` 진짜 임시 시각, `RIGHTS_UNKNOWN_HOLD` 권리·근거 불확실. 모든 정의를 일괄 교체 대상으로 삼지 않는다.
- 기존 원본 PNG/메타/Shader/AnimationSheet·ThirdParty 원본·Unity Scene/MonsterDefinition/Quest/Save/Voice·사용자 local dirty 파일 변경하지 말 것.

## 3. Phase B — 실제 오리지널 시안·PNG 제작 (중복 금지)

**B1. 몬스터 파일럿**: Phase A 분류에서 `OFFICIAL_KEEP`가 아니고 실제 Unity 플레이에 사용되는 것 중 **최상위 1종**을 선정(우선 비교 후보 `venom_bee`, `forest_spider`, `venom_snake` 및 다른 초반 임시 1~4종). 해당 종의 원본·현행 애니메이터/전투 프레임을 실물 확인하고 **고유 실루엣·Idle/Walk/Attack/Hit/Defeat 등 실제 요구 동작을 포함한 실사용 PNG/시트 파일럿**을 제작한다. 초반 몬스터의 안전한 저대비 자연 톤/문양과 공격 판독성·눈에 띄는 원형·소형 UI 표기를 강조하되 체력·속도·공격 범위·AI는 그대로. **기존 11~18 공식 승인 몬스터/보스 전면 재디자인 불허**. 파일럿 종 선정 및 시안은 보고하여 대표 미술 승인 뒤 나머지 확장, 공식 자산 개별 교체는 별도 명시 승인받을 것.

**B2. 기본 펫 3종**: 제3자 Run 원본을 베껴서 픽셀 재색칠하는 대신 **독창적인 Wolf/Bear/Fox의 오리지널 Run 실루엣 콘셉트**를 병렬 기획. 첫 실사용 파일럿 **Wolf 1종**을 우선 납품, 필드/전투 화면·펫 관리 UI에서 식별성과 적절한 속도감 확인. 기존 `BeastCompanionDefinition` 실제 속성/소스 Run 64px·12fps·left-facing·bottom pivot은 기능 호환 기준으로 감사, **최종 프레임 수·시트 크기는 현 실제 프레임수에서 결정**. 어떤 새 실루엣도 ThirdParty 리깅/이미지 복제/추적 불가. Wolf 미술 승인 뒤 Bear·Fox를 **같은 스타일로 병렬 제작**. 사용자가 한 번에 세 가지를 선호하면 선행 파일럿 QA는 지키되 같은 ZIP 내 두 후보를 함께 보일 수 있음.
- 늑대: 빠르고 날렵한 전면/후면 4족 실루엣, 잔상·반짝임 남발 없이 '동료의 습격' 타이밍에서 눈에 띄게.
- 곰: 크고 묵직한 전진과 착지감, Wolf와 포즈·바운스 차별화.
- 여우: 작은 체구/긴 꼬리·기민한 점프와 회복, 세린의 Fox 등 실제 소유·장착 표현 일치.
- 몬스터를 펫으로 해금하는 종은 **같은 공식 MonsterDefinition의 기존 프레임을 활용**하는 것이 기본. 기존 사수 동료 수치·펫 패시브·스킬/종별 ID 보존, 별도 펫 슬롯/턴 도입 금지.

**산출물 분리:** `Limitless_Monster_Renewal_Pilot_<ID>_v1.zip` 및 `Limitless_Beast_Companions_Wolf_v1.zip`(영문명 예시), 추후 Bear/Fox 정식 버전. 각 ZIP = 실제 원화 PNG/명세, `Manifest.json` 개별 SHA/프레임 Rect·Pivot·PPU, `Source_Mapping.csv`(기존 GUID/Animation/Runtime), `Animation_Contract.csv`(가시 프레임·방향·동작·fps), `README.md`, 사용권/출처 `Rights_and_Provenance.md`, Preview/ContactSheet, `QA/Offline_Validation.json`, Codex 통합 지시. 미리보기만 납품한 것은 `ART_COMPLETE` 아님.

## 4. 검증·접근성·승인 경계

- 몬스터와 펫 기존 전투 규칙/FieldSpawner/Collider/Target/DoT/Encounter/AI·보상·드롭/Stage·세린·BeastService/Shop/Save/Controller·TTS/BGM 보호.
- 가시성: 필드 탑다운/전투 HUD 16:9·4:3·21:9에서 몬스터와 펫이 녹색/재빛/검은색 배경에 묻히지 않고 **색상만 아닌 실루엣·몸/다리/방향/움직임**으로 구별. 광과민 유발 점멸, 깜빡임/가혹한 화면 흔들림 금지. 적/펫 오인되지 않는 팀 소속 표시, 작은 사수 스킬 아이콘(약40px) 검수.
- 모든 신규 실사용 자산은 파일별 알파 고립픽셀/프레임 경계 침범·sprite bleed/깜빡임/충돌정렬/전투 프레임 이벤트 보존의 **오프라인 QA** 후 납품. 실제 Unity/OS/플레이 QA는 Codex 전담.
- 제작 담당은 `docs/art-renewal`만 수정, 본 기획 담당은 `docs/planning`. 사용자 미술 승인 전 Unity 본파일 반영 금지, Codex는 승인 이후만 각 `codex/*` worktree에서 Import·규격/Animator·동료의 습격·펫 분양·필드/전투 및 Save 회귀 검사. Main10 공식 승인WAV18/Main09 승인27/Field04~09 공식 아트와 환경 버그 별도 보존. main 직접 커밋/Push/병합 금지.
- 리뉴얼 담당 작업 순서: **정본/권리/실자산 감사 → MONSTER18/PET3 판정표 → 파일럿(비공식 몬스터 1 + Wolf 1) 실제 PNG 제작 → Preview/오프라인 QA → 대표 아트 승인 → Bear/Fox 확장/나머지 교체 필요한 종 순차 제안.** Codex의 환경 map 기술수정과 병렬; Codex 한도를 아트 제작에 사용하지 않음.
- 완료 보고: branch, commit SHA, 교체 후보 ID·근거, ZIP 이름·용량·SHA, 사용자 미술 승인 필요 여부, Unity 미적용·미검증 영역 및 다음 할 일. 세션이 길어지면 차기 세션 전 인계 선행.

