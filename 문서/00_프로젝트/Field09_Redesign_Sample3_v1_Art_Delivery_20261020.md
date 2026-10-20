# LIMITLESS / Field09 새 환경 원화 기술계약 대응 3종 샘플 v1 실제 납품 — 2026-10-20

## 발주/상태

- 발주: 바울 대표 Field09 **기존 부분 보완 v1/v2 중단 → 환경 원화 신규 제작**, 맵 구조·Unity 기능 유지. **본 계약은 샘플 3종에 한함**.
- 읽은 기획 정본: `docs/planning/문서/00_프로젝트/Field09_Art_Redesign_Sample3_Technical_Contract_20261020.md`.
- Codex의 로컬 보고 커밋 `0dcc13ab`는 기획 정본에서 전달된 정보이며 이 세션이 해당 로컬 커밋 자체를 직접 읽거나 Unity를 실행한 것은 **아님**.
- 기준 GitHub `main`: `12c69c72f9c09f716ee6242418b1d62f83a4a0d2`; 담당 브랜치 `docs/art-renewal`.
- **상태**: `NEW_ORIGINAL_SAMPLE3_PNG_DELIVERED / OFFLINE_PNG_QA_PASS / ZIP_TESTZIP_PASS / UNITY_VISUAL_QA_PENDING / USER_ART_APPROVAL_PENDING / OFFICIAL_UNITY_UNCHANGED`.

## 실제 납품 파일

`Limitless_Field09_Redesign_Sample3_v1.zip` — **4,207,395 bytes**, **SHA256 `4ca6ea65f6315b0e35011a902d520549ba7d944145e6ec0b05962ff945f893b4`**, **14 entries**, Python 표준 `zipfile.ZIP_DEFLATED`. **실제 ChatGPT 샌드박스 다운로드 파일**로 제공. GitHub 저장소에 ZIP 바이너리를 업로드했다고 주장하지 않으며 바울 대표 F: 디스크에도 자동 복사하지 않았음.

| PNG 실제 이름 | 512×512 RGBA / 해시 SHA256 | 납품 바이트 | 기능·위치 |
|---|---|---:|---|
| `Assets/LL_Field09_Redesign_Ground_Base_v1.png` | `1bd8a614988ee182640a71b9771646c3009545140e0522a3290ee3b0e3138203` | 342154 | 지면 1종, `(0,0)`, **Tiled 21×15** |
| `Assets/LL_Field09_Redesign_Ground_Cracked_Overlay_v1.png` | `56d8e0ea9bf3ca2167227362b87bb219a21d4470f317456f36eb8aba99caedeb` | 76226 | 균열 오버레이 1종, `(-6,0)`, **Tiled 9×15**, alpha0~226 |
| `Assets/LL_Field09_Redesign_Obsidian_Slab_v1.png` | `dfa3abb2c6c86abc3f94baa213428038a1c15b4c963e76fef69491a6deb09692` | 140714 | **낮고 넓은 현무암 Slab**, `(5.2,1)`, **표시 약 2×2** |

신규 3종은 512² 원생 해상도에서 불규칙 현무암 셀·지질 결·조명, 용암균열 네트워크와 새 Slab 입체 면을 설계한 **새 이미지 자산**. 이번 생성 콘셉트 이미지는 Slab 표면 마모·질감의 생성 참고로만 활용했으며, 과거 v1/v2 공식 PNG 픽셀 재색칠/복제, 이전 샘플의 첨탑 재압축·단순 확대 **아님**. Field08/Field09 기존 공식 이미지의 물체/이미지를 복제하지 않음.

## 패키지 구성

- `Assets/*.png` 3개 (역할별 별도 PNG 원화)
- `Manifest.json` (위 원화 SHA256/Alpha/Canvas·PPU128/중앙 Pivot과 Scene 배치·미술 보존 규칙)
- `Preview/Base_3x3_Repeat.png`, `Preview/BasePlusOverlay_3x3_Repeat.png` — 실제 PNG 픽셀의 오프라인 3×3 반복 비교
- `Preview/Base_Tiled21x15_Simulation_NotUnity.png` — PPU128/현재 현장 world size를 투영해 축소한 21×15 반복 모식도
- `Preview/Overlay_Tiled9x15_Simulation_NotUnity.png` — 9×15 반복 모식도
- `Preview/Tiled21x15_Overlay9x15_SlabWorld2x2_Simulation_NotUnity.png` — 베이스 (21×15), 서쪽 Overlay (-6,0, 9×15), Slab (5.2,1, 약2×2) **수학적 크기/위치 기반 합성 모식도**, 실제 Unity GameView/원본 동일 카메라 화면 **아님**
- `Preview/Slab_Transparent_Preview.png` — 투명 배경과 실루엣 확인
- `QA/Offline_Validation.json`, `QA/WorldSpace_Simulation_Readme.json`, `README.md` — 파일 검증/배치 인계.

## 실제 파일 기반 QA

- ZIP `zipfile.testzip() is None`: **CRC PASS**, 독립 재열기 후 3개 ZIP 엔트리 바이트와 개별 PNG 파일 바이트 완전 동일.
- Pillow `Image.open/load` PNG 3/3 **PASS**, 모두 `512×512 RGBA`. Base Alpha255 (완전 불투명), Overlay Alpha0~226, Slab Alpha0~255 (투명 배경/부드러운 접지 후보).
- 두 Tiled 후보의 **x=0 vs x=511, y=0 vs y=511 RGBA 바이트 정확 일치**. 즉 픽셀 끝단 연속성 확인, **회색 띠 시각 판정 또는 이종 Material 경계의 Unity PASS라고 주장하지 않음**.
- 원래 Tiled 21×15, 9×15 전역 모식도 생성. 정해진 WorldSize와 Position 계약을 사용했지만 실제 Unity SpriteRenderer Tiled/Pivot/Material/Tint/Sorting/Camera/컴프레션과 양쪽 Region 알파 전환은 **미실측**.
- 사람 미술 위험요인 공개: 현 원화는 돌 표면·균열 형태가 반복되므로 넓은 영역에서 **패턴 주기성**이 관찰될 수 있음. 서쪽 `Overlay` 직사각형 끝선의 이종 재질 투명 알파 전환/Slab 접지·가독성은 Codex 실제 GameView 비교가 필요. Unity QA FAIL 시 **샘플만 수정**, 전체 제작 금지.

## Codex 후속 게이트·보호

1. 바울 대표에게 **샘플 3종 미술검토**. 사용자 스타일 승인과 실제 Unity 기술/시각 QA는 별개의 승인 게이트.
2. Codex `codex/*` 별도 worktree에서 신규 GUID에만 Import, **PPU128, Center Pivot**. `GroundBase Renderer Tiled (0,0), Size21×15`, `GroundCrackedWest Renderer Tiled (-6,0), Size9×15`, `Slab (5.2,1), 약2×2` 기준으로 기존 배치·Collider 절대 이동/변경하지 않음.
3. **기존 v1/v2 QA 40PASS / Main18 delta92PASS 재수행 금지**. 동일 카메라 3개 화면비에서 지면 6개 반복라인·이종 Overlay 끝선·Guide 잔류0·Slab/과열·Main18 조사 마커/도보·휠체어/SaveContinue **변경 영향 범위만** 델타 검증.
4. 하나라도 시각 실패 시 공식 적용 금지·이번 샘플만 보완. **실제 Unity 시각 QA와 바울 대표 미술 승인 모두 통과하기 전 나머지 Field09 환경 아트 제작0**.
5. 기존 `main`의 `Resources/Main18/Environment` 공식 PNG10·meta10·GUID, Field08 공식 이미지, Main18 퀘스트13 목표·과열/전투/몬스터/세린/콜라이더/Save/Continue/C#/Scene/Editor 작업 상태 그대로 보호. 이번에 `main` Commit·Push·병합0.

**현재 GitHub에는 본 납품·기술 인계 문서만 기록. ZIP 바이너리는 이번 채팅에서 실제 파일 다운로드로 전달.**
