# Field09 신규 리디자인 Sample3 v1 — 규격 PASS / Unity 시각 FAIL

2026-10-20, 브랜치 `codex/field09-redesign-sample3-qa-20261020`. **원본 적용: 보류. Push·main 직접 커밋·병합0.** 샘플만 검수했으며 신규 제작·리터칭·제품 코드 수정0.

## 1. 정본과 납품 파일

GitHub 최신 main `12c69c72f9c09f716ee6242418b1d62f83a4a0d2`의 AGENTS/CURRENT_STATUS/CODEX_WORKFLOW/COMMON_RULES/CODEX_LATEST를 직접 조회했습니다. `docs/planning`의 `Field09_Art_Redesign_Sample3_Technical_Contract_20261020.md`(blob `45aedb21aa64737efc0d59c916f1823d1c2a7a64`)와 같은 날짜의 재제작 전환안·과거 v2 제작지시도 확인했습니다. 최신 Sample3 계약과 바울 대표의 이번 검수 요청을 우선했습니다. 정본 조회 사본은 RemoteDocs에 보존했습니다. 과거 문서의 샘플 미납품 상태는 이번 로컬 실물 수신으로 갱신합니다.

- 원본 ZIP: `F:/Downloads/Limitless_Field09_Redesign_Sample3_v1.zip`, **4,207,395 bytes**.
- SHA256: **`4ca6ea65f6315b0e35011a902d520549ba7d944145e6ec0b05962ff945f893b4`**.
- Python 표준 zipfile 열기·testzip()==None·14파일 실제 해제 PASS. Assets PNG3 / Manifest1 / Preview7 / QA JSON2 / README1. 목록·파일별 CRC/크기/SHA는 [ArchivePNGValidation.json](Evidence/Field09Sample3_20261020/ArchivePNGValidation.json)에 기록했습니다. Preview는 납품자의 합성 자료이며 Unity 합격 근거로 사용하지 않았습니다.

| 실제 포함 자산 | SHA256 |
|---|---|
| `LL_Field09_Redesign_Ground_Base_v1.png` | `1bd8a614988ee182640a71b9771646c3009545140e0522a3290ee3b0e3138203` |
| `LL_Field09_Redesign_Ground_Cracked_Overlay_v1.png` | `56d8e0ea9bf3ca2167227362b87bb219a21d4470f317456f36eb8aba99caedeb` |
| `LL_Field09_Redesign_Obsidian_Slab_v1.png` | `dfa3abb2c6c86abc3f94baa213428038a1c15b4c963e76fef69491a6deb09692` |

## 2. PNG 규격 — PASS

PNG3 모두 512×512 RGBA 디코딩·Manifest 바이트/SHA·파일명/역할 일치. PPU128·중앙 Pivot(256,256) Sprite Single/FullRect·Point·Mipmap OFF로 실제 Unity Import PASS. 새 GUID/새 폴더를 사용했습니다.

- Base alpha255, 전체 불투명. 양축 맞은편 edge RGBA 차이0. 다만 edge 일치 자체를 시각 합격 근거로 삼지 않았습니다.
- Overlay alpha0~226, 반투명. 양축 맞은편 edge 차이0. 외곽의 alpha>0 픽셀392개는 반복 균열의 일부이며 외곽 전체를 투명 처리한 이미지는 아닙니다. 이것이 유한 영역 끝의 자연스러운 마무리를 보장하지 않습니다.
- Slab alpha0~255, 외곽 alpha0. 실제 불투명/반투명 실루엣 bbox `(67,158)-(454,438)`(끝 좌표 제외). 8방향 연결 성분1개, 고립된 1~4픽셀 조각0. 흰 사각 테두리/가이드0. 중앙 Pivot 사용에 기술 문제 없음. 2×2는 여백 포함 이미지 전체로 실제 실루엣은 약1.51×1.09유닛입니다.

## 3. 격리 Unity 변경분 — 31 PASS / 0 FAIL

기존 격리 Client `Temp/Field05Review20261019/Client`에 원본 Field09 Scene의 복사본을 두고 **런타임에서 Sprite3만 교체**했습니다. 원본/격리 Scene의 제품용 정식 저장0. Unity **6000.6.5f1 Batch Play Mode**, 기존 GameView 해상도 지정·ScreenCapture로 창/포커스 활성화 없이 실제 GameView 전후 **16:9 1280×720, 4:3 960×720, 21:9 1680×720** 확보. 별도 실제 RenderTexture 전체·접합·Slab 확대도 확보했습니다. 같은 배율/카메라 조건으로 전후 비교했습니다.

Base (0,0) Tiled21×15/-20, Overlay (-6,0) Tiled9×15/-19, Slab (5.2,1) Simple/scale0.5/-5 유지. 셋 모두 Tint RGBA1, **Sprite-Lit-Default / URP 2D Sprite-Lit-Default**. 반투명 합성 정상. 원본 Collider와 휠체어 Collider 크기·Quest 상태 동일 확인. 이번에는 이동 입력·퀘스트 전주행·SaveContinue를 새로 실행하지 않았습니다. 기존 Main18 92PASS/v2 기술40PASS는 기존 이력으로 재사용했으며 새31PASS와 합산하지 않습니다.

컴파일 오류0, 새 Runtime Console 오류0. 기존 Search/URP 환경 진단은 별도 유지했습니다. 퀘스트/진행 안내·세린·플레이어·몬스터가 표시되며 새 샘플로 추가 가림이 생긴 것은 보이지 않습니다. 장시간 플레이/전체 기능 회귀를 새로 검증한 뜻은 아닙니다.

## 4. 실제 화면 판정

| 대상 | 결과 | 관찰 |
|---|---|---|
| Base 기존 회색 반복띠6 | **PASS** | 이전 회색 직선 격자띠가 사라짐. 대신 같은 검은 무늬·잔열 패턴 반복은 눈에 띄므로 밀도/미술 방향은 대표 확인 대상 |
| Base↔Overlay 접합 | **FAIL** | x=-1.5에서 밝은 균열이 수직 경계로 잘려 끝남. 반투명이어도 유한 Tiled9×15 사각형 끝에서 균열이 자연스럽게 사라지지 않음 |
| 신규 Slab 가이드/테두리 | **PASS** | 흰 가이드·네모 프레임·외곽 부유 픽셀 없음 |
| 신규 Slab 크기/접지/배치 | **기술 PASS / 미술 승인 대기** | 기존 위치·scale에서 공중에 분리된 픽셀/직사각 배경 없이 표시. 여백 때문에 실제 바위는 2×2보다 작음. 시점·질감·접지 미술의 최종 적합성은 대표 확인 필요 |
| 기존 분위기/가독성 | **미술 승인 대기** | 검은 바위·붉은 잔열은 유지. 새 지면과 기존 미교체 소품의 질감 차이는 전체 제작 전 대표 검토 필요 |

**불합격 자산:** `LL_Field09_Redesign_Ground_Cracked_Overlay_v1.png`의 현재 유한 영역 연결. [Sample_Join.png](Evidence/Field09Sample3_20261020/Unity/Sample_Join.png)의 검토 bbox **(620,0)-(660,720)**, 밝은 선 절단 예 **(620,430)-(650,540)**, 위쪽 원점0/끝 좌표 제외. 세계 x=-1.5. 이는 Base의 회색 seam 문제가 아닙니다. 무한 반복 PNG의 맞은편 edge 일치는 통과하지만 Renderer가 영역 끝을 자르는 조건까지 대응하지 못했습니다. Material alpha 무시나 Sprite 참조 오류로 판단할 증거는 없습니다. **균열 패턴의 영역 끝 처리/배치 방식은 기술·미술 협의 대상이며 이번에 임의 shader/mask/원화 수정을 하지 않았습니다.**

기존 Cliff/Edge/Glow/HeatVent/Spire 등 미교체 소품의 흰 선은 화면에 그대로 남아 있습니다. **샘플 Slab의 결함과 혼동하지 않았으며**, 기존 소품까지 해결된 전체 Field09 합격을 주장하지 않습니다.

## 5. 보호와 다음 단계

원본 프로젝트 전용 Assets·UserData·Settings·Packages **3,546파일 SHA변경0**, 원본 Git status/HEAD 동일. 원본 Editor는 Bootstrap/Play OFF/비포커스 상태 유지, Refresh·포커스·OS 입력 조작0. 기존 공식 PNG10/.meta/GUID·원본 Field09 Scene·Save·UserData 보호. [보호 증거](Evidence/Field09Sample3_20261020/Protection.json).

**종합: ZIP PASS / 규격 PASS / 격리 기술 PASS / Unity 시각 FAIL / 사용자 미술 승인 PENDING / 원본 적용 보류.** 접합 FAIL이 있어 나머지 환경 자산 양산으로 넘기지 않습니다. 샘플 Overlay의 영역 끝 연결만 먼저 보완·합의 후 같은 화면으로 재검수합니다. 그다음 Sample3 미술 방향을 바울 대표가 승인해야 전체 환경 제작 단계로 진행할 수 있습니다. 샘플 승인도 원본 적용·main 병합 허가와 별개입니다. Push0, 외부 댓글/메시지0.

[실제 전후 화면 갤러리](Evidence/Field09Sample3_20261020/index.html) · [판정 JSON](Evidence/Field09Sample3_20261020/Result.json) · [실행 로그](Evidence/Field09Sample3_20261020/UnityLog.txt)
