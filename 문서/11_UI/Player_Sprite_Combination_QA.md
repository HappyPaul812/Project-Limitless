# Player Sprite 자동 매핑 QA (2026-10-03)

2026-10-05 QA정책 정정: Mobility9종 READY승격, 전체READY50/BLOCKED0. 저Alpha 고립노이즈는비차단경고, 실제Art오류는차단. 기존READY41종 재판정/PNG변경 없음. [정책](Player_Sprite_QA_Policy.md)·[재판정QA](Mobility_LowAlphaPolicy_QA.md).

2026-10-05 Mobility 최종10종: READY1/BLOCKED_ART9, 전체READY41/BLOCKED9. 다른40종 보존. [최신 QA](Mobility_RestoredFinal10_QA.md).

2026-10-05 Hearing v3 새160Frame QA: **READY10/BLOCKED0·전체READY40/BLOCKED10**. Mobility 포함다른40종/기존READY30 보존. [최신QA](Hearing_RestoredV3Ten_QA.md). 백그라운드88PASS/0FAIL·최종Console Error0/Warning0.

2026-10-05 Hearing v210종 원본 반영·새160Frame QA: READY0/BLOCKED_ART10, 전체 **READY30/BLOCKED20 유지**. Mobility 포함 다른40종 보존. [최신 QA](Hearing_RestoredV2Ten_QA.md).

2026-10-05 Hearing 최종10종 원본 반영·새160Frame QA: READY0/BLOCKED_ART10, 전체 **READY30/BLOCKED20 유지**. Mobility 포함 다른40종 보존. [최신 QA](Hearing_RestoredFinal10_QA.md).

2026-10-05 Vision512Final 새48Frame QA: 지정3종 READY3/BLOCKED0·전체 **READY30/BLOCKED20**. 다른47종 및 이전 READY27 보존. [최신 QA](Vision_Restored512Final3_QA.md). 백그라운드90PASS/0FAIL·최종Console Error0/Warning0.

2026-10-05 Vision Final3 형식검사 보류: `F:/Downloads/Limitless_Vision_3_Restored_Final.zip` 지정3종이 모두1254×1254 RGBA여서512×512·Cell128 규격 불일치. 게임 자산 반영0/READY 승격0, 전체 **READY27/BLOCKED23 유지**. 다른47종 및 기존 READY/사용자 변경 보존. [최신 QA](Vision_RestoredFinal3_QA.md).

2026-10-05 Restored8 원본 반영: 대상 READY5/BLOCKED_ART3, 전체 **Ready27/Blocked23**. 다른42종 보존. [최신 QA](Intellectual_Vision_Restored8_QA.md). 아래는 이전 이력이다.

2026-10-05 최신 셀 정밀 수정: 지정8종 문제41셀만 수정, 정상84셀 Pixel Diff0. 기존 절단 윤곽 잔존으로 READY승격0, 전체 Ready22/Blocked28 유지. 다른42종 보존. [최신 개별 QA](Intellectual_Vision_CellCleanup_QA.md). 아래는 이전 이력이다.

2026-10-05 Cleanup v2: 지정8종 READY0/BLOCKED_ART8, 전체 **Ready22/Blocked28 유지**. 다른42종 보존. [최신 개별 QA](Intellectual_Vision_CleanupV2_QA.md). 아래는 이전 이력이다.

## 결과

LOCAL Unity6000.5.7f1, 새 `PlayerSpriteCombinationAudit`에서 백그라운드 PlayUnfocused로 **346 PASS / 0 FAIL**. UI 후속 렌더 검사 **7 PASS / 0 FAIL**. [Runtime 로그](Player_Sprite_Combination_Runtime_Results.txt), [Render 로그](Player_Sprite_Combination_Visual_Results.txt), [50조합 Matrix](Player_Sprite_Combination_Matrix.md), [JSON](Player_Sprite_Combination_Matrix.json).

- README/VALIDATION.csv/Inventory로 각 파일의 성별·Path·Job을 확인하고 PNG50개가 Inventory SHA256과 같음을 확인했다. 그림으로 매핑을 추측하지 않았다.
- Male25/Female25, Path별10/Job별10, Duplicate0/Missing0. Stable ID 기반 조회이며 Marksman→sharpshooter, Physical→path.mobility, Visual→path.vision, Heartscar→path.emotional-scar 명명 차이만 대응했다.
- 실제 Path UI 선택 및 Job UI 버튼 선택으로 50개 조합의 예상 Appearance ID·Preview·Blocked 표시·Story 실제 Path 판정을 검사했다. Job 미선택 단계에서 조합을 선점하지 않는다.
- 대표 Male/Vision/Fighter, Female/Hearing/Sharpshooter, Male/Intellectual/Guardian, Female/Mobility/Mage, Male/EmotionalScar/Healer 각각 기본 정보→Path→Job→Confirm→World, 첫 Save, 실제 Bootstrap Continue, World Controller의 Left Idle 및 실제 Battle Scene 전달을 확인했다. Battle 입력·전체 전투 playthrough를 재검사한 것은 아니다.
- 이전 수동 AppearanceId의 누락/불명/다른 조합은 정본 조합에서 재계산했다. Path/Job이 비어 있는 Version1 데이터는 기본 성별 Sprite로 복원했고 비어 있지 않은 알 수 없는 Path/Job은 기존처럼 거부한다. 사용자 Save 삭제·초기화 없이 임시 격리 경로만 사용했다.
- 외형 필터/이전·다음/수동 선택·번호를 제거했다. 실제 선택 대상은 Gender/Name/Path/Job이며 기본 정보 Navigation은 성별2·이름·다음 총4개다. 성별 미확정 초안은 기존 이름 검증 후 확정하고 직업 변경은 Preview를 즉시 갱신한다.
- 기본 정보·Job·FinalConfirmation을 별도 Camera/RenderTexture에서 1280×720 및800×600으로 렌더해 확인했다. GameView 활성화/foreground/OS 포커스 전환은 실행하지 않았다. [기본 정보1280](Player_Sprite_CharacterCreation_1280x720.png), [기본 정보800](Player_Sprite_CharacterCreation_800x600.png).

## 미술 상태

Mapping50개는 모두 정확하지만 RuntimeReady21 / ArtBlocked29는 유지된다. Blocked 조합은 해당 ID를 저장하면서 기본 성별 Sprite의 명시적 임시 fallback을 사용한다. 다른 Path/Job Sprite·공통 Mobility Sprite로 대체하지 않는다. 기술 QA의 기존 readyForSelection 필드는 직렬화 호환을 유지하고 RuntimeReady로 노출한다.

| Path | 조합 | Ready | Blocked |
|---|---:|---:|---:|
| path.vision | 10 | 5 | 5 |
| path.hearing | 10 | 0 | 10 |
| path.intellectual | 10 | 6 | 4 |
| path.mobility | 10 | 0 | 10 |
| path.emotional-scar | 10 | 10 | 0 |

Blocked29의 정확한 성별/길/직업·ID·Definition은 Matrix의 ValidationStatus에 표시하고 프레임·사유는 [기존 Foreground QA](Player_Appearance_Foreground_QA.md) 및 Matrix JSON에 보존한다. 원본 미술 수정은 별도 작업이다. Sprite 미술 재검수나 PASS 판정 갱신은 수행하지 않았다.

## 보존·한계

원본 PNG/metadata/Catalog/기존 Player Animator 보호282개 파일 SHA256 모두 동일. 따라서 Catalog Stable ID 및800 Sprite의 GUID/fileID 참조도 그대로 유지한다. Import·Slice·50 Controller 생성 없음. Main16/BGM/Voice 코드는 변경하지 않았다. 기존 수동 외형 감사 도구 이름은 새 조합 감사의 호환 진입점으로 유지한다.

최종 Compile Error0. 기존 ExternalAssetImportEditor.cs deprecated spritesheet 경고2건이며 작업 중 MCP WebSocket 초기화 경고1건은 재연결 후 진행됐다. clean Bootstrap Edit Mode, Save Audit 경로null, 기존 Settings 경로 및 Play 옵션/runInBackground 복원 확인.

실제 키보드·게임패드 물리 입력과 foreground GameView는 미검증이다. 800×600 최종 확인 화면의 기존 중앙/능력치 패널 겹침과 직업 이름/상세 텍스트 간격은 후속 UI 레이아웃 작업 대상이다. 이번 기본 정보 화면과 새 Preview/fallback 라벨은 두 해상도에서 확인했다. 사용자 원본 미술29종 수정 및 해당 조합의 실제 움직임 재검수는 별도 작업이다.

GitHub Push 없음. 이번 변경만 stage하며 기존 사용자 Scene·Prefab·Animator·ThirdParty 변경을 포함하지 않는다.

관련 commit: 문서98dd23a / 자동 Mapping·UI dc9feff / Save·Continue6102952. 관련 stage diff 검사 통과. 기존 사용자 변경은 작업 트리에 유지하며 stage하지 않았다.
