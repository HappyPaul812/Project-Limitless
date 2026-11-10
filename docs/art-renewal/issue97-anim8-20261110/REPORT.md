# Issue #97 — 승인 VenomSnake 8프레임 실제 게임 적용

**VENOMSNAKE_ANIM8 / UNITY_QA_PASS / LOCAL_ORIGINAL_APPLIED / USER_REALPLAY_REVIEW_PENDING / REMOTE_NOT_PUSHED**

2026-11-10. [최신 기획 승인](https://github.com/HappyPaul812/Project-Limitless/issues/97#issuecomment-6763021207) 및 docs/planning의 PLANNING_LATEST를 확인했습니다. 기획 SHA `b71bd700037de5984ddb86dc8aaa642ca971b9d9`.
작업 브랜치 `codex/venomsnake-anim8-unity-20261110`, 구현 SHA **`485a8dffad37c02ffcef67ee9abd4ec92a3bdefe`**. 로컬 main `def786a3d28d239ceb0a539b42d77834828a3a8a`에서 구현 커밋으로 ff-only 반영했습니다. 문서 포함 최종 HEAD는 Issue #97 결과 댓글과 `Temp/Issue97/Evidence/final-result.json`에 기록합니다. main 직접 커밋·원격 Push·원격 Merge 없음.

## 최종 원본과 무결성

기본 ZIP은 지정 위치 바로 아래에 없고 `F:/Downloads/Limitless_Sprites/VenomSnake_Anim8_Issue97_DELIVERY_ONLY.zip`에서 발견했습니다. 지정 SHA와 일치한 동일 납품입니다.

| ZIP | SHA256 | 검사 |
|---|---|---|
| 기본 Anim8 | `07f5922d89078b869b95abcb165a3f66355cc49a9ea63667f631c438bad74256` | 39 entries, CRC PASS, SUMS37/Manifest37 일치 |
| Continuity_v2_2POSE_SMALL | `9a6b0315fd66eb8a66992085c4a7bbcfb7558759e00dab9a3de54050efe3cc73` | 16 entries, CRC PASS, SUMS14/Manifest14 일치 |

v2 ZIP은 `F:/Downloads/`에 있습니다. 미채택 Attack03 FinalReview ZIP은 사용하지 않았습니다. 최종 선택은 아래 8장입니다. 경로·제작 납품 출처·크기·alpha 경계는 [sources.json](sources.json), ZIP 전체 검사 목록은 [zip-integrity.json](zip-integrity.json)에 있습니다.

| 파일 (512 RGBA) | SHA256 |
|---|---|
| Idle00 | `ab03751c9ab91569c56dbb775730829a63e5c7d61f99ce29b349d63e2694fcac` |
| Idle01 | `d60e3f25c6acb2c359e0fa1452489ac731afe32f7e1669f9952c9582272e51da` |
| Idle02 | `710463c54fe6484fc12393fe7f571597ccd38ccfb68e1d17e79508146a1da420` |
| Idle03 | `ffb2cec3a2d7c6327c7f584dd675befcf4052b898cdad4c045331e9bbf976258` |
| Attack00 | `cb485208e5ab663ac7872ae4d496af466bd155684b5ae92651907b665b777d2c` |
| Attack01_Continuity_v2 | `7caeb3e9f622dc2e5567233ef5c52b7303b6f85dbe8621cb2ff68dc97984db2c` |
| Attack02 | `db3369eb1ba733f25a84fd373e598f5ab6f926c23b09a3e1e7a5dba9ba582efa` |
| Attack03_Continuity_v2 | `4e93ef15bd26b8692f56f93aa97dd01434f3c995156704575c45f28e9da6ad0a` |

## 제품 변경 6파일

아래 경로 기준은 `Unity/Client/Assets/_Project/`입니다.

| 파일 | 변경 / GUID |
|---|---|
| `Resources/MonsterAnimations/Snake/VenomSnake_Idle4_Approved97_640cell.png` 및 `.meta` | 신규 / `3c8f42044c3641a39e7ad1bd97cfe032` |
| `Resources/MonsterAnimations/Snake/VenomSnake_Attack4_Approved97_640cell.png` 및 `.meta` | 신규 / `0614bfb33b3840e7ae4ef02a33cb1f5d` |
| `Resources/MonsterDefinitions/04_VenomSnake.asset` | 두 시트·640셀·scale1.05·우향만 변경. 기존 GUID `2b234173bb51439ab9cfc940b6409d34` 및 meta 보존 |
| `Scripts/Monster/MonsterSpriteSheetAnimation.cs` | 기존 승인 공격 완료 대기에 venom_snake 추가. script meta 보존 |

각 PNG는 2560×640, 640셀 4열 1행입니다. 각 셀의 좌상단 기준 (64,128)에 승인 512 원본을 무손실 배치했습니다. **8/8 원본 RGBA 픽셀 동일**을 확인했으며, 리사이즈·재드로잉은 없습니다. Idle00/01/02/03 및 Attack00/01_v2/02/03_v2 순서입니다.
Point / Clamp / 무압축 / no-mipmap / max4096 / Import PPU640. Runtime Sprite.Create 중앙 Pivot(320,320)·PPU640도 실조회했습니다. [시트 SHA/GUID](derived.json), [반영6파일 SHA](applied-files.json).

기존 Idle 불투명 폭23/32=0.71875WU, 신규438/640×1.05=0.71859WU로 크기를 유지합니다. Idle 바닥 -0.461~-0.494WU로 기존 -0.5WU 근처입니다. 스폰·Collider·visualSize는 변경하지 않았습니다. [정적 계약 비교](static-contract.json).
원래 `snake_move_sheet.png`와 meta/GUID `4eebc05f001d494daecb632a39bcfb95`는 보존했습니다.

## Unity 실제 Delta QA

**Unity 6000.6.5f1, 기본61 PASS + 보완24 PASS, 신규 Runtime 오류0.** [최종 판정](final-verdict.json), [보완 검사](SupplementChecks.txt), [실제 프레임 추적](animation-trace.csv).

- Field_02 실제 배회 AI·Idle4 반복·Attack4 8FPS(0.498초)·Idle 복귀·우향·접지·PPU/Pivot PASS.
- Battle 실제 조우·Idle4·Attack4 8FPS(0.516초)·기존 기본공격의 4프레임·Idle 복귀·맹독 부여·2행동 쿨다운 PASS.
- 기존 Presenter의 접근0.16+대기0.09초를 유지합니다. 정상 프레임에서 scaled time 타격0.279초로 프레임 허용 내 PASS. 피해·독 계산 코드는 수정하지 않았습니다.
- HP95·공격력160%·EXP20·재화6·독 power7/strength7/지속3·드롭·AI 등 비시각 정의 모두 동일합니다.
- Bee/Spider 실제 Idle·Attack 슬롯 변화 PASS. 다른 시트 몬스터 참조·타이밍 조건과 몬스터 펫 프레임 PASS. Wolf/Bear/Fox 승인 Run 참조·12FPS PASS. 다른 퀘스트나 펫 전투 전체 QA는 반복하지 않았습니다.
- 1280×720 Field에서 실제 폭 약51.7px로 기존 크기를 유지합니다. 추가 실제 GameView **800×450=32.34px / 960×540=38.80px**에서 실루엣·머리·몸통 곡선 가독성 PASS. 32px에서는 비늘 세부가 줄어듭니다.
- 전체 시트/셀 잘림·새 참조 누락 없음. 원화 픽셀 보존.

검증기 정정 내역: [첫 FullChecks](FullChecks_FirstRun.txt)의 타격 실시간 하한1건은 RenderView 직후의 긴 delta를 고려하지 않은 계측 조건이었습니다. 정상 프레임 시작과 기존 scaled-time Presenter 계약으로 다시 측정해 PASS했습니다. 첫 보완의 Bee/Spider 파일명 비교 역시 승인 반복 그림·기존 파일명을 잘못 가정한 것으로, 실제 슬롯 변화로 정정해 PASS했습니다. 제품 결함을 덮어쓴 것이 아닙니다. 초기 동률 연출에 가려진 사진은 최종 증거에서 제외했습니다. 최종 캡처는 실제 동률 연출 종료 후 정상 플레이어 외형 전달을 포함합니다.
시작 시 MCP Editor assembly reference EndOfStream 등 기존 환경 초기화 로그는 Runtime 오류와 구분했습니다. 검증기 변수 이름 충돌1건은 수정 후 컴파일했으며 제품 컴파일 오류는 없습니다.

## 실제 GameView

모두 격리 Editor의 `UnityEditor.GameView.RenderView`이며 Camera.Render·오프라인 합성이 아닙니다. D3D 저장 상하 방향만 정규화했습니다. 검증기는 제품 Assets에 포함하지 않고 `Tools/Issue97/SnakeProbe.cs`에 보존했습니다.

- [필드1280×720](Field_GameView_1280x720.png)
- [32px 실제800×450](Field_Readability_GameView_800x450.png)
- [39px 실제960×540](Field_Readability_GameView_960x540.png)
- [전투 Idle](Battle_VisibleIdle_GameView_1280x720.png)
- [실제 공격·피해·맹독](Battle_VisibleAttack_GameView_1280x720.png)

## 로컬 원본 반영과 보호

반영 전 보호3921 SHA 변경0·dirty302/untracked550·제품6파일과 사용자 변경 겹침0. 기존 대상2파일은 `Temp/Issue97/Evidence/OriginalBackup`에 보존했습니다. 격리 Save/설정은 SyntheticSaves/Settings를 사용했습니다.
원본에 구현 커밋을 ff-only 적용하고 두 PNG·정의를 표적 Import했습니다. 원본 Editor 컴파일 완료·Console Error0·실제 프레임 생성8/8·컴파일된 승인 타이밍 ID Bee/Spider/Snake를 확인했습니다.
원본 Bootstrap Scene 경로·dirty false·Play false·선택 null·포커스 false를 유지했습니다. 반영6파일 바이트 동일 복제본의 최소 Field/Battle 재실행도 **24 PASS / 0 FAIL / Runtime Error0**입니다. 원본 Editor에서 새 Play를 켠 검증과는 구분합니다. [반영 후 검사](PostApply_checks.txt), [보호 재검사](protection-after.json), [원본 실조회](original-final.json).
최종 보호3921 SHA 변경0, 사용자 dirty302/untracked550 동일. Save/UserData·사용자 미커밋 파일·승인 PNG/WAV/meta·Packages·BuildSettings 보존. F2·NPC·다른 아트 제작/적용 없음.

## 바울 대표 확인 방법과 남은 승인

원본 `F:/study/codex/Project-Limitless/Unity/Client`에서 새 Play로 Chapter1 **Field_02** 맹독뱀을 확인하시면 됩니다. 기존 스폰 `venom_snake_01=(-2.2,0.3)`, `venom_snake_02=(1.4,-3.8)`, 배회 반경1.1WU입니다. 접촉하면 기존 Battle로 진입하며 맹독뱀의 행동에서 공격4장을 확인할 수 있습니다. 기존 Save 초기화나 진행도 강제 변경은 필요하지 않습니다.
기술 HOLD는 없습니다. 바울 대표의 최종 손 플레이·미술 수용 승인은 남아 있습니다.
직전 #75 B/C branch `codex/f2-bc-unity-delta-20261110` / `17b045e020153658e1e597aa689e076a27c40fa6`의 원본 미적용 상태도 유지했습니다.
