# Field09 OverlayFix v2 — 단일 자산 재검수 PASS

2026-10-20. 브랜치 `codex/field09-overlay-v2-qa-20261020`. **SAMPLE3_VISUAL_QA_PASS / USER_ART_APPROVAL_PENDING / ORIGINAL_APPLY_HOLD**. 제품 코드·원화 임의 수정0, 원본 local main·Scene 변경0, Push/main 병합0.

## 정본·납품·범위

AGENTS/PROJECT_CONTEXT/CURRENT_STATUS/CODEX_WORKFLOW/COMMON_RULES/CODEX_LATEST 및 앞선 Sample3 QA를 확인했습니다. GitHub `docs/planning` 최신 목록과 `Field09_Redesign_Sample3_UnityQA_OverlayOnly_Fix_20261020.md`(blob `9a8bcf04a1173f934c1be3aa2bdf51769d1f57c9`)를 직접 조회했습니다. Sample3 계약·재제작 전환안은 이전 조회본과 blob 동일하며, 이번 단일 Overlay 지시가 후속 정본입니다.

- ZIP: `F:/Downloads/Limitless_Field09_Redesign_Sample3_OverlayFix_v2.zip`, **3,338,697 bytes / 11파일**.
- ZIP SHA256: **`ec831166a753eef179da8fc283faf1fe81d1880941bce10c8d93225269cd418c`**.
- Overlay: `Assets/LL_Field09_Redesign_Ground_Cracked_Overlay_v2.png`, **79,880 bytes**.
- Overlay SHA256: **`0147e6aff58e5304fd71e7474a9c018958a4234ec4c2dc2cf7382a905a7be9b7`**.
- 표준 zipfile.testzip()==None·실제 해제·파일별CRC/SHA 기록 PASS. 구성: 실제 자산 PNG1, Manifest1, Preview5, QA2(CSV/JSON), README1, 재현 스크립트1. 납품 스크립트는 실행하지 않았습니다. 오프라인 Preview를 Unity 결과로 대신하지 않았습니다.

PNG 512×512 RGBA·Manifest SHA PASS. 이전 Overlay SHA `56d8e0ea9bf3ca2167227362b87bb219a21d4470f317456f36eb8aba99caedeb`와 실제 픽셀 비교: **RGB 동일, 알파만3,705픽셀 변경**, bbox x100~155/y30~389(양끝 포함). Changed_Pixels.csv의 좌표/구·신 알파3,705건이 실제 차분과 완전히 일치합니다.

Base와 Slab은 ZIP에 실물을 재포함하지 않았고 기존 파일을 그대로 재사용했습니다. 격리 Client에서 다음 SHA 일치 확인, 이미지 수정/재생성/리사이즈0:

| 기존 PASS 자산 | 유지 SHA256 |
|---|---|
| `LL_Field09_Redesign_Ground_Base_v1.png` | `1bd8a614988ee182640a71b9771646c3009545140e0522a3290ee3b0e3138203` |
| `LL_Field09_Redesign_Obsidian_Slab_v1.png` | `dfa3abb2c6c86abc3f94baa213428038a1c15b4c963e76fef69491a6deb09692` |

## 격리 Unity Delta — 17 PASS / 0 FAIL

기존 격리 Client `Temp/Field05Review20261019/Client` 재사용. Base·Slab은 기존 합격 Sprite, 비교 중 바꾸는 것은 **Overlay v1→v2 참조1개뿐**입니다. v2는 새 Asset/GUID로 Import했습니다. 원본 Scene/기존 PNG/meta/GUID 덮어쓰기0.

Unity6000.6.5f1 Batch Play Mode, **GameView ScreenCapture 16:9(1280×720) / 4:3(960×720) / 21:9(1680×720)**에서 동일 카메라 조건으로 전후 확인. x=-1.5 접합과 서쪽 반복 영역은 실제 RenderTexture 확대를 추가했습니다. OS 창/포커스·원본 Editor 활성화0.

Overlay (-6,0), scale1, Tiled9×15, PPU128·중앙Pivot·Point·NoMip 유지. Tint RGBA1/Sorting-19, Sprite-Lit-Default·URP2D Sprite-Lit-Default 유지. Base/Slab 참조 동일·Collider 동일, 컴파일 오류0·신규 Runtime Console 오류0. 기존 Search/URP 환경 진단은 별도 유지했습니다. 기존31/40/92 및 수백 건 기능QA 재실행0이며 새17에 합산하지 않습니다.

## 시각 판정 — PASS

| 항목 | 결과 |
|---|---|
| x≈-1.5 밝은 균열의 직선 절단 | **PASS** — v1의 밝은 핵심 균열이 경계에서 끊기던 부분이 v2에서 가늘게 약해져 끝남 |
| Base↔Overlay 접합 | **PASS** — 검사한3개 GameView/접합 확대에서 인위적인 밝은 수직 절단선이 보이지 않음 |
| 신규 네모 자국·알파 끊김·회색 반복띠 | **PASS** — 새 결함 관찰되지 않음 |
| 기존 PASS Base·Slab | **재사용 유지** — SHA 및 참조 동일, 별도 전수 재검수하지 않음 |

전후 상세 비교는 `Unity/v1_Join.png`와 `v2_Join.png`, 검토 영역 bbox **(620,0)-(660,720)**(위쪽 원점0, 끝 좌표 제외)입니다. 기존 밝은 선 절단 예 `(620,430)-(650,540)`도 함께 확인했습니다. 실제 렌더 경계 안쪽6px의 밝은 주황색 픽셀 보조 측정은 Result.json에 기록했으며, 임계값 수치만으로 시각 합격을 결정하지 않았습니다.

재질은 정상 반투명 합성되며 Renderer·Material·Sorting 변경 없이 PNG의 국소 알파 변경만으로 이번 실패가 해소됐습니다. Base 무늬 반복과 유지한 기존 소품 가이드는 기존 상태이며, **이번 PASS는 Sample3 방향 검증/Overlay 수정 범위에 한정**됩니다. 나머지 소품까지 교체된 전체 Field09 완성 판정은 아닙니다.

## 보호·승인·다음 단계

원본 Assets/_Project·UserData·Settings·Packages의 SHA, 원본 Git HEAD/status 동일 검증은 [Protection.json](Evidence/Field09OverlayV2_20261020/Protection.json)에 기록했습니다. 원본 Editor는 Bootstrap/Play OFF/비포커스 유지. 원본 Scene·Save/UserData·기존 공식 PNG/.meta 보호. 새 Quest/이동/과열/Save 기능을 구현하거나 전주행하지 않았습니다.

**바울 대표의 Sample3 미술 승인이 필요합니다.** 승인 후 나머지 Field09 환경 아트 제작 여부를 결정합니다. **원본 적용은 보류**, 전체 적용/Push/main 병합은 별도 명시적 요청 전까지 수행하지 않습니다. 외부 댓글/메시지 전송0. CURRENT_STATUS와 CODEX_LATEST는 전용 격리 브랜치에만 갱신합니다.

[실제 v1/v2 전후 화면](Evidence/Field09OverlayV2_20261020/index.html) · [ZIP·변경범위 검사](Evidence/Field09OverlayV2_20261020/ArchiveScopeValidation.json) · [판정/수치](Evidence/Field09OverlayV2_20261020/Result.json) · [실행 로그](Evidence/Field09OverlayV2_20261020/UnityLog.txt)
