# Player Appearance 검증본 50종 등록 계약

2026-10-05 Hearing v3 새160Frame QA: **READY10/BLOCKED0·전체READY40/BLOCKED10**. Mobility 포함다른40종/기존READY30 보존. [최신QA](Hearing_RestoredV3Ten_QA.md). 백그라운드88PASS/0FAIL·최종Console Error0/Warning0.

2026-10-05 Hearing v210종 원본 반영·새160Frame QA: READY0/BLOCKED_ART10, 전체 **READY30/BLOCKED20 유지**. Mobility 포함 다른40종 보존. [최신 QA](Hearing_RestoredV2Ten_QA.md).

2026-10-05 Hearing 최종10종 원본 반영·새160Frame QA: READY0/BLOCKED_ART10, 전체 **READY30/BLOCKED20 유지**. Mobility 포함 다른40종 보존. [최신 QA](Hearing_RestoredFinal10_QA.md).

2026-10-05 Vision512Final 새48Frame QA: 지정3종 READY3/BLOCKED0·전체 **READY30/BLOCKED20**. 다른47종 및 이전 READY27 보존. [최신 QA](Vision_Restored512Final3_QA.md). 백그라운드90PASS/0FAIL·최종Console Error0/Warning0.

2026-10-05 Vision Final3 형식검사 보류: `F:/Downloads/Limitless_Vision_3_Restored_Final.zip` 지정3종이 모두1254×1254 RGBA여서512×512·Cell128 규격 불일치. 게임 자산 반영0/READY 승격0, 전체 **READY27/BLOCKED23 유지**. 다른47종 및 기존 READY/사용자 변경 보존. [최신 QA](Vision_RestoredFinal3_QA.md).

2026-10-05 Restored8 원본 반영: 대상 READY5/BLOCKED_ART3, 전체 **Ready27/Blocked23**. 다른42종 보존. [최신 QA](Intellectual_Vision_Restored8_QA.md). 아래는 이전 이력이다.

2026-10-05 최신 셀 정밀 수정: 지정8종 문제41셀만 수정, 정상84셀 Pixel Diff0. 기존 절단 윤곽 잔존으로 READY승격0, 전체 Ready22/Blocked28 유지. 다른42종 보존. [최신 개별 QA](Intellectual_Vision_CellCleanup_QA.md). 아래는 이전 이력이다.

2026-10-05 Cleanup v2: 지정8종 READY0/BLOCKED_ART8, 전체 **Ready22/Blocked28 유지**. 다른42종 보존. [최신 개별 QA](Intellectual_Vision_CleanupV2_QA.md). 아래는 이전 이력이다.

Cleanup8 지정8종 적용: READY0/BLOCKED_ART8, 전체 Ready22/Blocked28 유지. 기존42종·자동 Mapping/Save/fallback 보존. [최신 QA](Intellectual_Vision_Cleanup8_QA.md).

2026-10-03 최신 V2: Intellectual3/Vision5 대상8종 BLOCKED_ART, 신규READY0·전체 Ready22/Blocked28 유지. 다른42종과 기존 Intellectual Fighter Male READY 보존. [최신 V2 QA](Intellectual_Vision_V2Eight_QA.md). 아래 Revised/Fixed 집계 설명은 이전 작업 이력이며 표/JSON은 최신 판정이다.

2026-10-03 최신 Revised9: Intellectual Fighter Male READY 승격, 대상 Ready1/Blocked8·전체 Ready22/Blocked28. Vision Fighter Male은 방향 해결 후 BLOCKED_ART. 다른41종 판정 유지. [최신 QA](Intellectual_Vision_Revised9_QA.md). 아래 집계 설명은 이전 작업 이력이며 표/JSON은 최신 판정이다.

Intellectual4/Vision5 수정판9종을 기존 ID로 교체했다. Intellectual 원본512 적용·Vision 승인1254→512 변환, PASS0/Blocked9·전체21/29 유지. 다른41종 미변경. [최신9종 QA](Intellectual_Vision_Fixed9_QA.md).

Mobility10종은 제공512×512 수정판 원본으로 교체했다. Mobility Ready0/Blocked_ART10, 전체 Ready21/Blocked29 유지. 기존 ID·160 Sprite 참조·meta/Import·Mapping/Save와 다른40종 QA/Asset은 유지했다. [Mobility 수정판 QA](Mobility_Player_Sprite_Fixed_QA.md)를 최신 입력·판정으로 사용한다.

Hearing10종은 사용자 승인 수정 ZIP1254→512 변환본으로 교체했다. Hearing0 Ready/10 Blocked, 전체21/29 유지. 기존40종은 미변경이다. [수정판 QA](Hearing_Player_Sprite_Fixed_QA.md)를 최신 Hearing 입력·판정으로 사용한다.

## 현재 정식 정책 (2026-10-03 설계 정정)

Gender + Path + Job → 대응 Sprite 자동 결정. 수동 Appearance 선택은 제거하며 50조합 중 Ready21/Blocked29를 유지한다. Blocked와 미선택은 기본 성별 Sprite의 명시적 임시 fallback을 사용한다. 상세 정본은 [자동 매핑·Save 계약](Player_Appearance_선택_Save.md)을 따른다.

## 이전 구현·검증 이력 (정식 정책 아님)


## 후속 선택·Save 구현 (2026-10-03)

등록 완료 뒤 CharacterCreation의 성별/테마 필터·큰 Preview·이전/다음 선택과 Stable Appearance ID Save/Continue, 공용 Animator Clip Override를 연결했다. 현재 **35종 ready=true / 15종 false**이며 전체50종을 Catalog에 보존한다. 외형 테마는 실제 Player Path/Job/Story 조건과 독립이다. ID 누락/invalid/blocked은 기존 Male/Female+Path Variant로 fallback한다. 진행 중 미용실/외형 변경 NPC는 미구현이다.

선택·저장 정책은 [정식 계약](Player_Appearance_선택_Save.md), 경계 경고10종157셀의 판정과 보류15종·실제 검증/한계는 [QA 보고서](Player_Appearance_선택_QA.md)를 따른다. 아래 Runtime 미구현/ready=false 설명은 최초 등록 당시의 이력이며 현재 상태는 이 후속 항목을 기준으로 한다.

2026-10-03 사용자 제공 `F:\Downloads\Limitless_Player_Sprites_Validated_50.zip`을 정식 등록 입력으로 사용한다. 문서화→구현→백그라운드 검증 순서이며 원본 ZIP·PNG 픽셀을 수정하거나 재생성하지 않는다.

## 입력 확인

ZIP SHA-256: `68dd9d2288398325ab93020ed8b0031c92215f7e995963010179c09a24343573`.

실제 파일52개: PNG50개, README.txt1개, VALIDATION.csv1개다. README는512×512·4×4·128px Cell·RGBA/투명으로 정규화한 패키지라고 명시한다. 직접 PNG를 디코딩해50개 모두 규격과 투명 Alpha를 확인했다. 이름은 `{Theme}_{JobTheme}_{Male|Female}.png`이다.

| 입력 테마 | Catalog 테마 | 남 | 여 | 합계 |
| --- | --- | --- | --- | --- |
| Visual | Vision | 5 | 5 | 10 |
| Hearing | Hearing | 5 | 5 | 10 |
| Physical | Mobility | 5 | 5 | 10 |
| Intellectual | Intellectual | 5 | 5 | 10 |
| Heartscar | EmotionalScar | 5 | 5 | 10 |

각 테마는 Fighter/Guardian/Healer/Mage/Marksman × Male/Female이다. 기존5개 ZIP의50개 테마·직업·성별 조합과 정확히 대응하며 CSV에 원본 파일명이 있다. 원본 이름27개는 기존 Inventory와 일치하고 일부 나머지는 인코딩이 깨진 형태다. PNG 파일 바이트는 기존 편입본과50개 모두 다르다. **통합·정규화된50개 구성은 확인했으나 기존5개 ZIP 전체의 변환 이력까지 증명한 것은 아니다**. CSV/README는 입력 그대로 보존하고 이름을 추측해 고치지 않는다.

800개 Cell은 비어 있지 않다. Alpha 경계 검사에서10시트의157개 Cell이 경계에 닿는다. 접촉만으로 잘림을 확정할 수 없지만 방향·발 위치·미술 품질까지 합격 처리하지 않는다. 이미지 재보정 없이 고정 그리드로 Slice하고 미술 검수는 남긴다.

## Import와 Catalog 계약

- 새 PNG: `Assets/_Project/Art/Characters/Player/Validated50/{CatalogTheme}/`. 기존 `External50/` PNG·meta는 보존한다.
- 현재 Player_Male.controller의 실제128px Clip이 참조하는 `Player_Male_Base_Walk_128.png`를 기준으로 한다. PPU128·Point·Uncompressed·mipmap 없음·512 크기 유지·RGBA 투명, Pivot은 기존128px Sprite와 동일한 아래 중앙 `(0.5,0)`이다. 별도 옛 Base_v1의 PPU100·자동 Slice와 구분한다.
- Multiple Sprite,128×128,16개. 위→아래/왼쪽→오른쪽 중립 이름 `Frame_00`~`Frame_15`를 사용한다. Rect는 Unity 아래 원점 기준 `(column*128,(3-row)*128,128,128)`이다. README에 방향 행 순서가 없으므로 Down/Left/Right/Up을 임의로 확정하지 않는다.
- Sprite Editor Data Provider와 Create/Delete·Rect·Name·Pivot capability 확인 뒤 Importer API로 처리한다. PNG나 Sprite meta를 손으로 작성해 Slice하지 않는다. 재실행 시 Sprite ID를 보존한다.
- 기존 `Resources/PlayerAppearances/External50.asset`의50 Entry를 갱신한다. Catalog GUID와 기존 `appearance.external.v1.{theme}.{jobTheme}.{gender}` ID50개를 유지해 중복 Catalog를 만들지 않는다. sheet/frames는 새 검증본을 참조한다. 이전 `External50.json`은 최초 입력 이력이며 새 입력 Inventory는 `Validated50.json`이다.
- 테마·직업·성별은 제작 메타데이터로 선택 제한이 아니다. 기존 `Find(stableId)`로 새 Texture/16 Sprite를 조회할 수 있는 등록 준비 범위다.

## Runtime 경계와 미구현 범위

공용 PlayerSpriteAnimator는 기존 Controller의 Idle/Walk 상태를 재생한다. 이번에는50개 Controller를 복제하거나 기존 Player/Path Variant Controller·Clip·Prefab을 교체하지 않는다. 방향/발 위치 검수 후 공용 Controller의 Sprite Clip Override를 연결하는 후속 구조를 사용한다.

Import 준비와 실제 게임 선택 가능 상태를 구분한다. `readyForSelection=false`와 reviewReason을 유지한다. Character Creation50종 선택 UI·World/Battle 자동 교체·Save appearanceId 직접 선택/저장/복원·방향별 Clip/Override·foreground 육안 QA는 미구현이다. 기존 Male/Female·Path Visual·Save 형식과 fallback을 유지한다. 향후 알 수 없는 ID/미검수 외형은 기존 외형으로 fallback해야 한다.

## 검증 계획

ZIP/SHA-256·PNG 디코딩·Alpha·800개 셀, Unity Import 설정·16 Sprite Rect/Pivot·GUID/ID 유일성·Catalog 참조를 Edit Mode에서 검사한다. 기존 사용자 변경·Player Asset·Animator/Prefab/Scene/Save를 해시로 보호한다. 컴파일/Console·관련 diff 검사를 완료하고 결과를 이 문서와 CURRENT_STATUS에 기록한다. foreground/창 활성화/OS 입력은 사용하지 않는다.

## 편입 및 검증 결과

- 기능 commit `d6937e4`. `ValidatedPlayerAppearanceImport.Import()`를 Editor MCP에서 실행했다. 이 도구는 검증본50개만 처리하며 기존 Catalog와 동일한 고정 ID 집합을 확인한 뒤 등록한다. 이전 `ExternalAssetImportEditor.Import()`는 BGM/배경/구형 External50 입력을 함께 재편입하는 과거 도구이므로 이번 검증본 재편입에 사용하지 않는다.
- Unity6000.5.7f1 Edit Mode에서50 Entry·800 Sprite·50개 유일 ID·16 Rect/Pivot·PPU128/Point/Uncompressed/Alpha·참조를 검사해 실패0을 확인했다. 새 Asset의 meta 누락0, Catalog GUID `5d34e6efc319dba46b15f1df6baf5236` 유지. 두 번째 편입 후800개 Sprite GUID/local file ID가 모두 동일했다.
- PNG50개와 ZIP의 SHA-256은 입력과 동일하다. 시작 시 보호한212개 파일 중 의도한 `External50.asset`만 변경됐다. 나머지 사용자 변경·기존 Player 원본·Save/Settings는 유지했다.
- 컴파일 실패false, Console 오류0. 기존 `ExternalAssetImportEditor.cs`의 `TextureImporter.spritesheet` 사용 중단 경고 CS0618 두 건이 재컴파일로 표시됐다. 새 편입 도구는 Data Provider API를 사용한다.
- Bootstrap clean Edit Mode·Editor unfocused를 유지했다. foreground/Play Mode 육안 QA는 실행하지 않았다. 경계 접촉157 Cell, 방향 행 순서 및 발 위치는 후속 미술 검수 대상으로 남긴다. UI/Save/실제 외형 전환은 위 Runtime 경계대로 미구현이다.
