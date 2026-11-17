# Issue103 — 승인 IceBeetle 5종 실물 검수 및 격리 재생

**ART_APPROVED / UNITY_NOT_APPLIED(CONTRACT_HOLD) / ISOLATED_PREVIEW_QA_PASS / PRODUCTION_QA_PARTIAL / REMOTE_NOT_PUSHED**

## 승인과 계약 판단

최신 원격 docs/planning의 PLANNING_LATEST·COMMON_RULES, 로컬 AGENTS/CURRENT_STATUS/CODEX_LATEST, Issue103 최종 댓글6941419315 및 승인 커밋17468d797c850828b83754b9ff3130438bdbdd9d의 기획 인계를 확인했습니다. KeyPose/Idle4/Attack4/Hit4/Defeat4 미술 승인 완료이며 납품 Manifest/CANDIDATE 파일명의 과거 REVIEW_PENDING 표기는 최신 댓글로 대체됩니다. 납품 원본을 고쳐 상태를 덮어쓰지 않았습니다.

기획의 ‘기존 M4 등록 또는 승인된 적용 계약이 없으면 격리 미리보기만’ 조건을 적용했습니다. 원본 Editor Resources에서 MonsterDefinition21종을 실제 열거해 M4 일치0, Assets/_Project의 IceBeetle 검색0. 원본 Assets 코드/asset/controller/anim/prefab/meta 검색도 M4 제품 연결 없음입니다. 상세기획의 F3~4 전열 얼음갑충 설명은 기획 문서이며 제품 등록 계약으로 임의 해석하지 않았습니다. 기존 4종 승인 시트 역시 원본 게임에 아직 반영되지 않았고, 이전 오프라인 Temp 사본만 있습니다.

범용 MonsterDefinition의 explicitIdle/Attack/Hit/DefeatFrames와 MonsterSpriteSheetAnimation은 이미 존재합니다. 하지만 M4 Definition·Sprite/meta/GUID·Clip·Controller·Animator 연결 자산은 없습니다. 따라서 새 MonsterID/Spawn/스탯/전투/펫 등록 또는 원본 제품 자산 생성 없이 격리 미리보기까지만 수행했습니다. LOCAL_APPLIED/PARTIAL_APPLIED로 보고하지 않습니다.

## 실제 파일 검사

- 입력 `F:/Downloads/IceBeetle_Issue103_Defeat4_20261117.zip`, 3,134,616bytes, 12파일. SHA256 `c04ad4e6528f3ea9aa36971b8ef9ce033711d2e96ba0d13589f3adb0fb676bbc`.
- CRC PASS, Manifest payload10/10 bytes+SHA, SHA256SUMS11/11, 전체 파일 커버리지·비순환·경로 안전성 PASS.
- `Sprite/IceBeetle_M4_Defeat4_2048x512_RGBA_CANDIDATE.png`, 912,389bytes, 2048×512 RGBA, 가로4×512셀. SHA256 `9cd3fd4c1925f9953830d9d857b96f11f8bb754dc7b57fe34640936f4cb1c5fb`.
- 분리 QA4장 전셀 RGBA 일치·각 셀 외곽alpha0. alpha>8 bbox 프레임1~4: (28,126,483,464)/(28,151,484,464)/(21,163,491,464)/(20,178,491,464). 하단464 공통, 점점 낮아지는 최종 패배 자세 유지. 승인 원본 픽셀 수정/재생성0.
- PNG8장(시트1+QA4+128/40스트립2+512/128/40비교판1), GIF1, 문서3. 128/40 직접 LANCZOS 축소 visibleRGBA/alpha 일치. GIF 실제4프레임 160/190/220/1600ms·loop확장없음.

| 기존 승인 사본(모두 RGBA) | 규격 | bytes | SHA256 |
|---|---|---:|---|
| IceBeetle_M4_KeyPose_512_RGBA_CANDIDATE.png | 512×512 | 241066 | `b7ff0592226c2a1f28270053cf03483b6051c69edb412b59a7965ccf8699b3ad` |
| IceBeetle_M4_Idle4_2048x512_RGBA_CANDIDATE.png | 2048×512 | 801439 | `bd06c6d9a901d4c3550882ad4ce061629195b60e75f04636ec4814ebb0967793` |
| IceBeetle_M4_Attack4_2048x512_RGBA_CANDIDATE.png | 2048×512 | 616816 | `d8dab5e411f7eeb0aa73b1e412b8d820ecfeb38d514c60cf5fef7eb29af4b236` |
| IceBeetle_M4_Hit4_2048x512_RGBA_CANDIDATE.png | 2048×512 | 893849 | `cf1dd6c78ffeffd12f8d026dda7ae362e09ffd0a7a3dee536d86569df4f51a3c` |

4종 모두 Defeat Manifest가 명시한 승인 SHA와 일치. 이전 파일 자체를 재검수해 재사용했으며 F:/Downloads에 이 4 ZIP을 현재 새로 발견했다고 주장하지 않습니다. 사본 위치는 audit.json의 approved_references입니다.

## 격리 최소 QA 및 한계

기존 `Temp/Art95_91Unity20261111/QA/Client` 재사용, 숨김 Batch Unity6000.6.5f1. QA 전용 폴더에 5 PNG를 바이트 그대로 복사하고 Unity Importer로 원본 해상도 유지/alpha투명/무압축/mipmapOFF/max2048/Bilinear 설정. 원본 프로젝트 Import0, meta 수동 작성0. 이미지별 런타임 Sprite.Create로 512셀 분리·PPU512/Pivot(256,48)·FullRect를 적용했습니다. 이것은 승인 시트의 하단464를 미리보기 바닥0으로 맞추는 **QA 임시 기준**이며 제품 PPU/Pivot 확정이 아닙니다. 영구 Sprite Editor 슬라이스·제품 GUID·클립·Animator Controller는 생성하지 않았습니다.

원본과 SHA가 같은 기존 MonsterDefinition/MonsterSpriteSheetAnimation 코드 사용. 메모리 전용 빈 MonsterDefinition에 프레임 필드와 미리보기8fps만 넣고 ID는 비워 두었으며, Resources/등록/스탯 계약을 새로 만들지 않았습니다. 기존 재생기의 SpriteRenderer와 전투용 Image 양쪽 프레임 출력 일치44PASS:
- 모든 모션 실제4프레임 경과, Idle 반복, Attack/Hit 비루프 후 Idle 복귀.
- Defeat 0→1→2→3 후 마지막 프레임 고정·추가1.6초 유지, PlayAttack/PlayHit 호출에도 마지막 자세 유지.
- 실제 Camera.Render PNG16장 및 최종유지 캡처. `Temp/Issue103Defeat113Review20261117/IceBeetle_ActualUnityPreview.png`는 실제 Unity 캡처 비교판입니다.

미리보기 **런타임 오류0**, compile완료. 격리 환경의 UnityEditor.Search.SearchDatabase 색인 ArgumentOutOfRangeException1건은 EnvironmentErrors.txt에 분리 기록했습니다. 제품 오류0과 격리 전체 Console 오류0을 혼동하지 않습니다. 전체 격리 프로젝트 컴파일 로그에 기존 warning 701개 고유 행이 존재하며 신규 Issue103PreviewProbe 경고0; 전역 경고 수리는 범위 밖입니다. 첫 실행은 셰이더 최초 렌더 준비가 프레임 관측 시간을 넘겨 Idle 검사가 중단되어 워밍업 후 재실행했습니다. 최종44PASS가 근거입니다.

실제 M4 전투·피해 시점·퇴장/보상·필드 크기 및 기존 몬스터 전투 전수 회귀는 **NOT_VERIFIED**입니다. M4 계약이 없으므로 실제 전투 성공을 주장하지 않습니다. 기존 원본 바이트·dirty 상태 보호 대조로 비변경을 확인합니다. 원본 Editor는 Bootstrap/PlayFalse/dirtyFalse/compileFalse·포커스null 유지, Console오류0/경고42.

## 다음 단계·확인 절차

미술 재제작은 필요 없습니다. 기존 제품 등록이 없으므로 향후 M4 등록/전투 계약의 별도 확정 후 그 안에서 제품 PPU·배치·모션 연결 및 실제 전투 Delta QA를 수행해야 합니다. 현재 게임 씬/좌표에서 얼음갑충을 볼 수 있다고 안내하지 않습니다. 지금 바울 대표는 실제 Unity 캡처 비교판 또는 Defeat4 비루프 GIF를 바로 확인하실 수 있습니다. 격리 재실행은 기존 QA의 Issue103PreviewBatch.Run 전용이며 사용 중인 원본 Editor를 열거나 Play로 전환하지 않습니다.

제품 변경0 / 제품 커밋없음. 브랜치 `codex/issue103-defeat113-review-20261117`; 문서 커밋은 최종 Final.json 및 Issue103 댓글. 원본 root main52f84e643d5d860665eadfe1614a30c745209f00 유지. 새 대형 복제/main 직접 commit/Push/Merge0. Save/UserData/Discord/#85 HOLD·Main04/07/레온/NPC12/Placeholder/승인 원본·기존 미커밋 상태 보호.

실제 증거: Temp/Issue103Defeat113Review20261117/{audit.json,original-before.json,original-after.json,PreviewQA/checks.txt,PreviewQA/Errors.txt,PreviewQA/EnvironmentErrors.txt,preview-summary.json,Unity-Preview-R2.log,Final.json}. 비공개 Temp 증거 및 원화 바이너리는 원격 업로드하지 않았습니다.
