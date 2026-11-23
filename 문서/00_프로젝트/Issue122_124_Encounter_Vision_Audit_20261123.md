# #122·#124 읽기 전용 조사 및 실물 비교 — 2026-11-23

기준 등록 제품 `9588d75cfbe46e656ee6dcc4b5e8f85c4aa04cc0`, 등록 보고 `01c8f197b27d30508b6517adb6b7d560da9df5e4`. [11/19 복구](EOD_20261119_SafeIntegration_Recovery.md)와 [등록 검증](Issue122_Registration_20261123.md)을 재사용했습니다. 신규 Unity 실행 및 기존 32/56/39 PASS 재실행 0. 제품 변경 0, Unity 6000.6.5f1 및 #85 HOLD 유지.

## #122 · 정식 연동 조사

**MonsterDefinition 0/6 · 공식 조우 ID 0/6 · DROP_LINK_HOLD 6/6.** `Resources/MonsterDefinitions` 18개, 별도 MonsterDefinition 데이터, MonsterSpawns/StoryEncounterReturns 및 North Scene·런타임 조우 경로에서 조사했습니다. F1과 F2 후보 Scene만 확인됐습니다. `prototype_main22_frost_field_mouse`는 StoryEncounterReturns의 프로토타입, `CANDIDATE_frost_owl_basic_qa`는 별도 QA, `BlizzardWisp_Visual.prefab`은 시각 자산입니다. 셋 모두 정식 등록에 재사용할 조우 키가 아닙니다. [등록 상태 비교판](Evidence/Issue122_124_20261123/Ch3_Loot_Encounter_Registration_Matrix.png)

아래 수치는 [북부 7종 설계 정본](../08_몬스터/Chapter3_북부몬스터7종_전투밸런스_20261118.md)입니다. MonsterId 표기는 정본의 미확정 후보이며, ItemId는 등록 브랜치의 승인 ID입니다.

| 몬스터 / ID 후보 | Lv / HP / ATK / AGI / EXP / 탈렌트 | 편성·지역 계약 | 승인 ItemId / 확률 |
|---|---|---|---|
| 서리들쥐 `frost_vole` | 17 / 200 / 28 / 12 / 60 / 16 | 전열 근접, F1 단독 학습 | `material_frost_vole_frosted_fur` / .50 |
| 설원족제비 `snow_stoat` | 18 / 220 / 34 / 19 / 68 / 18 | 전열 근접, F1–F2 | `material_snow_stoat_snow_down` / .50 |
| 서리올빼미 `frost_owl` | 18 / 205 / 30 / 17 / 70 / 18 | 후열 원거리, F2 첫 단독, 이후 혼합 | `material_frost_owl_frost_feather` / .45 |
| 얼음갑충 `ice_beetle` | 19 / 360 / 32 / 7 / 76 / 20 | 전열 근접, F3 첫 단독, 이후 혼합 | `material_ice_beetle_ice_carapace` / .45 |
| 눈보라 정령 `blizzard_wisp` | 20 / 255 / 36 / 12 / 84 / 22 | 후열 마법, F4–F5, 동상 대응 확보 | `material_blizzard_wisp_blizzard_crystal` / .45 |
| 빙각 수호체 `rime_sentinel` | 21 / 820 / 40 / 9 / 125 / 32 | 전열 근접, F5 단독 단회, 도주 허용 | `material_rime_sentinel_magic_blocking_crystal` / 1.00 |

### 데이터 준비 및 승인 순서

1. `MonsterDefinition`에 고유 `monsterId`, `displayName`, `monsterLevel`, `maxHp`, `baseExperience`, `currencyReward`, `battleAgility` 및 `useDefinitionBattleAgility`를 준비합니다. 현재 일반 참가자 생성식은 `max(1, ceil(10 × battleAttackPercent / 100))`입니다. 이 경로를 승인받으면 ATK 28/34/30/32/36/40의 직렬화 비율은 각각 280/340/300/320/360/400입니다. 기존 EXP 성장 산식은 유지합니다. 열과 공격 유형은 참가자 생성 계약이며 현재 일반 생성 경로는 열을 호출부에서 받고 일부 기존 종만 원거리·마법으로 분기합니다. 올빼미·정령을 기존 기본 근접으로 등록할 수 없으므로 정식 편성 및 행동 연결을 먼저 승인합니다.
2. 승인 Sprite/Animator·전투 프레임 경로를 실제 승인 원본과 대조해 연결합니다. 필드 이동 속도·대기 시간·시각 크기·프레임 배열과 스케일, 혼성 참조는 종별 근거가 있어야 합니다. 설계 정본의 한기·동상 예고 및 얼음갑충 방어를 데이터 참고로 사용하되 기본값이 해당 행동을 구현했다고 간주하지 않습니다.
3. 실제 Scene, Barrier/출입구, 좌표 및 활동 반경 승인 후 `FieldMonsterSpawnDefinition`의 `sceneName`, `spawnId`, `monster`, `position`, `activityRadius`, `respawnSeconds`, `nonRespawningBoss` 계약을 확인합니다. 일반 리스폰 35초는 기획 방향입니다. `BattleEncounterContext.ResolveStableEncounterId`는 현재 특정 스폰만 해석하며 스토리 경로는 전달된 `storyEncounterId`를 사용합니다. 정식 ID는 승인된 조우 단위로 배정해 기존 정식 키와 중복 검사합니다. 좌표·퀘스트 번호·키 값은 미확정입니다.
4. 연결이 승인되면 종별 `lootEntries`에 위의 `itemId`, `dropChance`, `minCount=maxCount=1`을 적용합니다. 승리 확정 후 실제 전투불능 개체당 1회, 패배·도주·같은 승리 중복 지급 0의 계약입니다. 보스 일반 드롭은 비워 둡니다. 전체 지급보다 단회 엘리트의 저장 계약을 먼저 확정해야 100% 드롭을 연결할 수 있습니다.

### 빙각 수호체 단회 보상

| 구분 | 조건 |
|---|---|
| 완료 키 | 슬롯 안에서 승인된 불변 고정 조우 ID 1개 사용. 승리 이후 `수령 대기`와 `수령 완료` 모두 처치 완료로 보고 재조우 차단. 현재 메모리 비재생성만으로는 재접속을 차단할 수 없습니다. 패배·도주 기록 0. |
| 수령 대기 | 만석이면 같은 슬롯에 처치 완료·ItemId·수량 1의 대기 상태를 함께 저장. 별도 승인된 안전 거점 또는 인벤토리 청구 UI에서 공간 확인 후 인벤토리 증가와 수령 완료를 한 번에 확정. 수령 위치 미확정. |
| 저장 실패 | 교체 전 실패는 이전 슬롯 보존과 동일 결과 재시도, 교체 여부 불명은 유효 슬롯/키 재조회 후 적용 여부 판정. 성공 전 판매·소비·후속 저장 차단. 재접속 시 대기는 복원, 완료는 차단. 기존 `File.Copy` 덮어쓰기와 무시되는 저장 반환값을 실제 Save 연동 승인 후 검증합니다. |

## #124 · 실제 픽셀 비교

[기존 ZIP 검증 및 전달](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7088219907)의 SHA256 `f8d0d09986131cf68d19d72a2b6546fe4324583758777e06a2c80b53d11e3a9c`, CRC/Manifest/SUMS를 재사용했습니다. 로컬 지정 ZIP에서 신규 128 PNG만 읽었습니다. `PathDefinitions/01~05` 각각의 두 GUID를 `.png.meta`에 일대일 매핑해 공식 문장 5, 외부 White Trait 5의 실제 PNG를 읽었습니다.

| 길 | 공식 문장 실물 | 외부 Trait 실물 |
|---|---|---|
| 시각 | PathSymbol_Vision.png | eye-target.png |
| 청각 | PathSymbol_Hearing.png | sound-waves.png |
| 지적 | Path_Intellectual_Companion_Emblem.png | mesh-network.png |
| 지체 | PathSymbol_Mobility.png | cog.png |
| 마음 | Path_EmotionalScar_Heart.png | heart-shield.png |

[로컬 원본 10종·신규 후보 동일 배율 비교판](Evidence/Issue122_124_20261123/Vision_Trait_Actual_Pixels_OFFLINE.png). 캔버스 여백을 포함하여 비율 유지로 128/40/30 px에 맞추고, 길 상세 패널의 RGB 근삿값 #0D131F 위에 합성했습니다. 제품 코드상 PathSelection 상세·전투 상세는 30×30, 선택 카드 24×24, 최종 확인 26×26입니다. 40px는 비교 기준입니다. CanvasScaler 및 게임 화면 실출력, 입력/포커스 검증은 수행하지 않았습니다.

- **40px**에서 신규의 눈과 원형 표적, 바깥 4방 실루엣을 읽을 수 있습니다. **30px**에서도 밝은 중앙과 흰 외곽은 배경과 구분됩니다. 얇은 냉청색 동심원은 작은 크기에서 약해져, 24/26px에서는 중앙 눈보다 원형 조준 표식이 먼저 읽힙니다.
- 기존 eye-target의 채워진 흰 눈 실루엣이 작은 크기에서 더 굵습니다. 신규는 바깥 4방 표식과 선형 동심원이 달라 직접 동일 디자인으로 보이지 않지만 눈·중앙 조준 모티프는 공유합니다. 공식 시각 문장의 금색 장식과는 역할과 실루엣을 구분할 수 있습니다. 색상에 덜 의존하도록 최종 UI에서 기존 특성명 텍스트를 함께 유지해야 합니다.
- 판정 **실물 대조 PASS / 30px 세부 가독성 PARTIAL / Unity GameView NOT_VERIFIED / 미술·교체 승인 HOLD**. 30px와 24/26px의 얇은 선 판독과 시각 문장과의 역할 구분을 대표가 확인해야 합니다. 후보 PNG 신규 제작, 기존 GUID 변경, 나머지 4종 제작, 본편 적용 0입니다.

공통 작업 브랜치 `codex/ch3-loot6-encounter-vision-audit-20261123`. 원본 main, Save/UserData·Discord 및 승인 자산 보호. 실제 SHA는 두 Issue 댓글에 기록합니다.
