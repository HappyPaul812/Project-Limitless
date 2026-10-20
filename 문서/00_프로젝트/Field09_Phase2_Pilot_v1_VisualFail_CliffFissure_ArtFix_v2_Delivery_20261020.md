# LIMITLESS — Field09 Phase2 Pilot Cliff/Fissure 두 PNG 국소 미술 보완 v2 납품 (2026-10-20)

## 업무 근거 및 명확한 상태

- GitHub [Issue #38](https://github.com/HappyPaul812/Project-Limitless/issues/38), 기획 정본 `docs/planning/문서/00_프로젝트/Field09_Phase2_Pilot_v1_VisualFail_CliffFissure_TargetedArtFix_20261020.md` 직접 검토.
- Codex 전차 검수: 기술 30PASS/0FAIL, 미술 실패 두 종 **Cliff 발치 회백·홍흑 매트**, **Fissure 외곽 매트 및 기존 Glow 붉은 띠 노출**. 이번 담당은 Unity 직접 실행 안 함.
- **이번 실제 수정 범위:** `LL_Field09_Phase2_Cliff_Blocker_v1.png` 및 `LL_Field09_Phase2_Fissure_Edge_v1.png` 각각 1개를 새 v2 512×512 RGBA 후보로 미술 수정. 다른 자산 변경0.
- **실제 파일 상태:** `ART_FIXED_PNG2_DELIVERED / OFFLINE_FILE_AND_ZIP_QA_PASS / GLOW_OVERHANG_RENDER_RISK_UNRESOLVED / CODEX_ISOLATED_UNITY_VISUAL_QA_PENDING / USER_ART_APPROVAL_PENDING / OFFICIAL_SCENE_UNCHANGED`.

## 1. 실제 수정된 PNG 파일 2종

| 새 PNG | 원본 v1 SHA256 | 새 v2 SHA256 | 변경 픽셀·BBox |
|---|---|---|---|
| `LL_Field09_Phase2_Cliff_Blocker_v2.png` | `2e7a863547688773dbb3d6672623e0958fbab01c8241d2665d8739a0777a231c` | `ad995f3cc4914759d6c88fa47c80227cb1a7c5f1a9a549679e06472402fee9f1` | 13,918px; x41..465, y265..471 |
| `LL_Field09_Phase2_Fissure_Edge_v2.png` | `7c892de74b1f3327b37f4fb3ff7673954d64542498b32f6ab76826e6ca53b1d0` | `c3dc0c131c11fef72e4d210a1a43528f9882d8c50486e8da0bc15b6984ea145a` | 14,574px; x53..462, y210..456 |

방법: 기존 두 v1 완성 스프라이트의 색·Alpha·외곽을 그대로 입력으로 사용. 바닥 부근의 밝고 탈색된 회백·홍흑 매트 후보를 **하단 국소 영역 + 실제 원화의 Alpha 실루엣 가장자리 거리 + RGB 채도·명도**를 함께 활용해 선별하고, 그 지점만 투명도를 감쇠·정리. 명암 접지부는 실제 바위의 불규칙 발치 마지막 약16px 안쪽만 미세 암부 강화; 검은 사각 그림자 신규 생성0. 전체 대상 픽셀의 구/신 RGBA는 `QA/Changed_Pixels.csv`에 개별 기록. 구형 Field09 v1/v2 수정 ZIP 재압축이나 무관 새 원화 생성 없음.

**원본 Unity Scene Transform 계약 고정:**
- Cliff `ObsidianScar_Cliff` (x=-9,y=0), Scale(0.65,0.65), Sorting 2.
- Fissure Edge `ObsidianScar_Fissure` (x=-5,y=3.8), Scale(0.525,0.625), Sorting -8. 기존 Glow `ObsidianScar_Glow` 동일 좌표/Scale, Sorting -7 **절대 변경 금지**.

## 2. 승인 Sample3 및 공식 Glow 보호

- Base Ground v1 `1bd8a614988ee182640a71b9771646c3009545140e0522a3290ee3b0e3138203`
- Cracked Overlay v2 `0147e6aff58e5304fd71e7474a9c018958a4234ec4c2dc2cf7382a905a7be9b7`
- Obsidian Slab v1 `dfa3abb2c6c86abc3f94baa213428038a1c15b4c963e76fef69491a6deb09692`

위 승인된 3개 PNG는 대화 컨테이너의 실제 기존 파일에 대해 SHA256 재계산 **3/3 일치**, 바이트 덮어쓰기0. Glow는 원본 `main/Unity/Client/Assets/_Project/Resources/Main18/Environment/ObsidianScar_Fissure_Glow.png`, Git Blob SHA1 `08eff9a25a1484aaf0085b5da10809c6c5737290` (SHA256과 혼동 금지). **Glow 자체 수정·재압축·재등록0**.

**남은 중요한 시각 위험 / 기술 원인:** 현행 SpriteRenderer는 **Glow sorting -7 > Fissure Edge -8**, 즉 Glow가 Fissure 앞에 그려진다. 오프라인 Edge 외곽 매트 정리만으로 Glow Sprite가 Edge 바깥으로 드러나는 붉은 띠를 **원천 차단할 수 없음**. Glow를 바꾸거나 임의 불투명 검정판으로 덮지 않았음. 새 ZIP의 `Preview/OriginalGlow_FissureEdge_1to1_OnlineCompare.html`는 전후 Edge 실제 바이트를 포함하여 공식 GitHub 원본 Glow를 **브라우저에서 변경 없이 실시간 로드, 원본 동일 크기/좌표에 겹치는** 비교 파일이다. 인터넷/GitHub 이미지 접근이 없으면 Glow가 로드되지 않으므로, **이번 세션의 오프라인 정적 PNG만으로 Glow 결합 PASS라고 주장하지 않는다**. 이때 Codex 현장 GameView 확인이 필요. 실제 Glow 노출이 계속되면 별도 승인 게이트로 Sorting/Material/SpriteMask 또는 Glow+Edge 결합형 재설계 **기술원인 분리 제안**하고 현재 Glow는 KEEP한다.

## 3. 실제 납품 파일 및 무결성

**실물 ZIP**: `Limitless_Field09_Redesign_Phase2_Pilot_Fix_v2.zip`
- ChatGPT Sandbox 작업 파일: `/mnt/data/Limitless_Field09_Redesign_Phase2_Pilot_Fix_v2.zip`.
- **3,602,303 bytes, ZIP SHA256 `0e4b7bfb3784b5b97a4a719c669f9564f502d67e88c2096d32475fa69a6876c8`.**
- 내부 13 entries; **`Assets/`에는 v2 PNG 2장만**, 원본 승인3/공식Glow는 미포함.
- `Manifest.json` 원본·수정 SHA, PPU128/CenterPivot, Scene Mapping, 승인3 유지 SHA, Glow Git Blob SHA.
- `QA/Changed_Pixels.csv` 구·신 RGBA 28,492개 변동 픽셀; `QA/Offline_Validation.json` 검사 명세.
- `Preview/*` : Cliff 접지 / Fissure 외곽 v1↔v2 실제 PNG의 승인 Base 합성 미리보기와 하단 확대컷, 두 후보 합성 PNG (**Unity GameView 아님**), **공식 기존 Glow를 온라인 참조해 정확히 겹치는 HTML 미리보기**.
- `Tools/build_Field09_Phase2_Fix_v2.py`: 기존 v1 SHA 확인→후보 2장만 다시 만들 수 있는 재현 코드(해당 Python environment numpy/scipy/Pillow 필요), README.

**실제 파일 기반 자체 검사:** PNG2 Pillow Decode / 512x512 RGBA / 출력 픽셀 roundtrip PASS, ZIP `zipfile.testzip()==None` CRC PASS, ZIP 재열기 모든 entry 파일과 byte-for-byte 동일, Manifest 두 PNG SHA256 직접 재대조 PASS, 승인3 정본 SHA 3/3 일치. **실제 Unity 3화면비 시각 재검수는 아직 하지 않았으므로 `UNITY_VISUAL_PASS` 표기 금지.**

**유통 위치:** 이번 메시지 Sandbox 링크로 ZIP 실제 파일 전달. GitHub에는 납품 문서와 재현 가이드를 기록하며 ZIP 바이너리를 등록했거나 사용자 F:에 자동 복사했다고 주장하지 않음. 기존 GitHub main/원본 PNG/meta/GUID·Scene·Save·UserData/Collider/Quest/과열/몬스터 전부 변경0.

## 4. Codex 후속 미술 검증 최소 범위

1. 실제 v2 ZIP SHA 대조, 새 PNG2 Import만 별도 codex/* 격리 작업공간에서 테스트, 기존 승인3 및 Glow 원본 SHA 보호.
2. Cliff x=-9,0 발치/하단 밝은 매트 0 여부; Fissure x=-5,3.8 외곽 밝은 매트 0 여부; **Glow와 결합한 붉은 띠**, 실제 원본 Base 위 정렬, 조사/과열 UI 단절 여부.
3. 기존 30PASS/17PASS/92PASS 전체 기능 재검사 중복금지. 변경분만 3비율 동일 GameView Delta/Before&After 스크린샷 제출.
4. Glow red strip 문제가 남으면 **불합격인 2번 영역을 Renderer 순서(-7/-8)와 Glow 자체 이미지 형상 불일치로 분리**, 바울 대표 승인 전 임의 Glow 변경/Sorting 수정 금지.
5. Codex 실제 미술 PASS + 바울 대표 최종 미술 승인 전 나머지 소품 제작·원본 본게임 적용·main merge 금지.
