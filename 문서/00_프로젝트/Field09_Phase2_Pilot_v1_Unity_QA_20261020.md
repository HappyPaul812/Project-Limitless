# Field09 Phase2 Pilot v1 — PNG2 기술 PASS / 시각 FAIL

2026-10-20, `codex/field09-phase2-pilot-qa-20261020`. **원본 적용 보류 / USER_ART_APPROVAL_PENDING / Push·main 병합0**. 이번 대상은 Cliff Blocker와 Fissure Edge만이며 원화·Glow·맵·Quest를 수정하지 않았습니다.

## 납품 검증 — PASS

GitHub [Issue #38](https://github.com/HappyPaul812/Project-Limitless/issues/38) 본문/댓글2와 `docs/art-renewal/Field09_Redesign_Phase2_Pilot_v1_ActualArt_Delivery_20261020.md`(blob `366a437ac178f6e1378e4eb17998e803c1b41a2d`)를 직접 확인했습니다. Issue의 Sample3 대표 미술 승인 완료 상태를 반영했습니다.

ZIP `F:/Downloads/Limitless_Field09_Redesign_Phase2_Pilot_v1.zip`, **1,672,537 bytes / 9파일**, 표준 zipfile CRC/testzip()==None·실제 해제 PASS. 신규PNG2/Manifest/매핑CSV2/Preview2/QA JSON/README. Manifest의 크기·SHA와 PNG2 실제 디코딩 모두 일치합니다.

| 대상 | SHA256 |
|---|---|
| ZIP | `f12b056924665eeb593d084d5a5b73ba67a1a3a335d2bd9f882a547e5b8a7ff6` |
| `LL_Field09_Phase2_Cliff_Blocker_v1.png` | `2e7a863547688773dbb3d6672623e0958fbab01c8241d2665d8739a0777a231c` |
| `LL_Field09_Phase2_Fissure_Edge_v1.png` | `7c892de74b1f3327b37f4fb3ff7673954d64542498b32f6ab76826e6ca53b1d0` |

PNG2 모두 **512×512 RGBA**, PPU128·중앙 Pivot(256,256)·Sprite Single/FullRect·Point·NoMip 실제 Import PASS. 투명 외곽/전체 사각 매트 없음, 7px 미만 고립 알파 군집0. 그러나 이것은 외곽 매트 색감의 시각 합격을 뜻하지 않습니다. alpha bbox: Cliff `(41,42)-(466,475)`, Fissure `(47,79)-(466,460)`(위쪽 원점0, 끝 제외).

승인3종은 재생성/재압축 없이 격리 Client 원본 그대로 사용했고 SHA 유지 확인:

- Base Ground v1 `1bd8a614988ee182640a71b9771646c3009545140e0522a3290ee3b0e3138203`
- Cracked Overlay v2 `0147e6aff58e5304fd71e7474a9c018958a4234ec4c2dc2cf7382a905a7be9b7`
- Obsidian Slab v1 `dfa3abb2c6c86abc3f94baa213428038a1c15b4c963e76fef69491a6deb09692`

## 격리 Unity 변경분 — 30 PASS / 0 FAIL

기존 격리 Client `Temp/Field05Review20261019/Client`에 원본 Field09 Scene을 복사했습니다. 승인3종을 양쪽의 동일 기준으로 사용한 뒤, **비교 전후 Sprite가 바뀐 Renderer는 아래2개뿐**임을 실제 확인했습니다. 새 PNG는 별도 폴더·새 GUID 사용, Scene 정식 저장0.

| Renderer | Position | Scale | Sorting | 처리 |
|---|---|---|---:|---|
| ObsidianScar_Cliff | (-9,0) | (.65,.65) | 2 | 새 Cliff Sprite만 교체 |
| ObsidianScar_Fissure | (-5,3.8) | (.525,.625) | -8 | 새 Fissure Sprite만 교체 |
| ObsidianScar_Glow | (-5,3.8) | (.525,.625) | -7 | Sprite·설정·PNG 그대로 |

모든 환경 Transform/Scale/Sorting/Tint/Material·Collider·Navigation Target 개수 동일. Glow 이미지의 격리 Client/원본 SHA도 일치합니다. Unity6000.6.5f1 숨김 Batch Play Mode, 신규 컴파일/Runtime Console 오류0. 기존 Search/URP 환경 진단은 별도 유지했습니다.

실제 **GameView ScreenCapture 1280×720(16:9), 960×720(4:3), 1680×720(21:9)** 전후와 Cliff/FissureGlow/조사/열기 카메라 RenderTexture 확대를 확보했습니다. 첫 일반 화면에서 서쪽 소품이 화면 가장자리에 걸려 격리 플레이어를 서쪽(-5,1)에 배치한 보완 캡처를 추가했습니다. 같은30개 검사도 다시 통과했으나 **60PASS로 중복 합산하지 않았습니다**. 캡처 설정 인용 오류로 잘못 시작된 보완 Batch1개는 초기 중단 후 수정했으며 원본 Editor에는 영향 없습니다.

Main18 조사 단계8과 열기 조사 단계5의 **격리 세션 스냅샷**으로 실제 조사 TextMesh·Quest 안내·HeatVent/연기 표시를 확인했습니다. 13단계 전주행이나 전투 과열 기능을 새로 검증한 것은 아닙니다. 기존 Sample3 **17PASS**, Main18 **92PASS**는 재사용했고 전체 회귀0. 충돌 배치와 이동 코드 보호 PASS이며 물리 입력 전체 이동 시험은 반복하지 않았습니다.

## 시각 판정 — FAIL

| 항목 | 결과 / 근거 |
|---|---|
| 신규 PNG2의 흰 직선 가이드·사각 프레임 | **PASS** — 신규2종에서는 관찰되지 않음 |
| 전체 투명 사각 매트 | **PASS** — 캔버스 전체를 채우는 배경 사각형 없음 |
| Cliff 접지/외곽 | **FAIL** — 바닥 돌과 오른쪽 하단에 회백·분홍빛 잔여 매트가 보여 지면과 붙지 않고 오려 붙인 윤곽으로 읽힘 |
| Fissure 자체 외곽 | **FAIL** — 용암 옆/발밑 회백·연분홍 바탕이 윤곽에 남아 보임 |
| Fissure↔기존 Glow | **FAIL** — 새 대각선 입체 균열과 기존 수직 붉은 벽 형태가 맞지 않아 Glow가 아래/옆으로 돌출, 이중 형태와 기존 프레임 잔존 |
| Main18 조사·열기 표식 | **PASS(확인 스냅샷 한정)** — 조사 문구·안내 화살표·HeatVent/연기 확인, 새2종의 추가 가림은 관찰되지 않음 |
| 크기 | 설정 유지 PASS — Cliff 이미지전체2.6×2.6/가시약2.16×2.20유닛, Fissure전체2.1×2.5/가시약1.72×1.86유닛. 최종 미술 체급 평가는 대표 확인 대상 |

정확한 검토 영역(스크린샷 위쪽 원점0, 끝 제외; 자동 삭제 mask 아님):

- `After/Phase2_Cliff.png`: **(80,410)-(515,585)** 하단·우하단의 밝은 매트. 소스 PNG 하단 검토 범위 x41~465/y300~474.
- `After/Phase2_FissureGlow.png`: **(470,330)-(815,555)** 새 균열의 밝은 외곽. 소스 PNG x47~465/y250~459 주변을 검토.
- 같은 확대의 **(440,140)-(840,580)**: 기존 Glow의 가이드/붉은 벽과 새 Fissure의 형태가 겹치는 전체 영역. Fissure의 세계 위치는(-5,3.8), Glow도 동일, Sorting은-8/-7입니다.

**원인 분리:** 밝은 외곽은 새 PNG의 RGBA에도 존재하고 같은 Renderer 설정에서 드러납니다. 색이 밝다는 이유만으로 바위 하이라이트 전체를 없애라는 뜻은 아닙니다. EdgeSamples.json은 검토용 원본 RGBA 표본입니다. Glow의 직선 프레임은 기존 이미지에 있던 것으로 새 PNG에 들어 있다고 오인하지 않았습니다. 다만 현재 고정 배치에서 두 실루엣의 호환성이 나빠 **합성 상태는 불합격**입니다. Glow를 숨기거나 Sorting/Scale/Tint를 바꿔 실패를 가리지 않았습니다.

## 보호·승인·다음 단계

원본 Assets/_Project·UserData·Settings·Packages **3,546파일 SHA변경0**, 원본 Git HEAD/status 동일, 원본 Editor Bootstrap/Play OFF/비포커스 유지. 원본 Unity/공식 PNG/.meta·Save·UserData 보호, 원본 Refresh/포커스·OS 입력 조작0. [보호 증거](Evidence/Field09Phase2_20261020/Protection.json).

**파일·기술 PASS / Phase2 시각 FAIL / 사용자 미술 승인 대기 / 원본 적용 보류.** 다음은 신규2종의 외곽 품질과 Fissure–현행 Glow 호환성을 샘플 범위에서 보완·협의한 뒤 같은 화면으로 재검수하는 것입니다. 임의 미술 수정/후속 소품 양산/원본 적용/Push/main 병합0. 승인3종은 다시 만들지 않습니다.

[실제 전후 화면](Evidence/Field09Phase2_20261020/index.html) · [파일/SHA/규격](Evidence/Field09Phase2_20261020/Validation.json) · [판정·결함 bbox](Evidence/Field09Phase2_20261020/Result.json) · [실행 로그](Evidence/Field09Phase2_20261020/Unity.log.txt)
