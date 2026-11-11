# LIMITLESS / 신규 아트 / Issue #89 — 승인 VFX·상태 아이콘 가독성 표적 검토 (2026-11-11)

**담당 브랜치:** `docs/art-new`  
**상태:** `APPROVED_ART_6_KEEP / TARGETED_LAYOUT_FIX_RECOMMENDED / VISUAL_PARTIAL / CURRENT_UNITY_SCREENSHOT_NOT_AVAILABLE / NOT_UNITY_APPLIED_THIS_TURN`.

## 1. 확인한 최신 정본 및 납품 실물

- [총괄 기획 2026-11-11 표적 검토 지시](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6786953257)
- [실제 Unity 99PASS/49 PNG, VISUAL_PARTIAL](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6622135223)
- [실제 격리 B 레이아웃 7쌍/50PASS](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6624142188)
- [최종 승인 아이콘 + B 렌더 8장, VISUAL_PARTIAL](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6628636928)
- [사용자 승인 VFX v2](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6621573660) / [사용자 승인 아이콘3](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6627893169).

**새 게임용 VFX·Sprite 생성 0, 원본 PNG 수정/재인코딩 0. 납품은 2개 파일만**:
1. `Issue89_ColdVisual_TargetedReview_OFFLINE.png` **1680×1090 RGB**, SHA256 **`19d23af2782b2840222db8e391ddb3bc92b64f8e9b0cf4f217b81ec998019bdf`** — 승인 아이콘3을 **실제 40px**으로 줄여 설원/어두운 HUD 색면에 동일크기 배치한 작은 비교판 + B안 수치 및 휠체어/HUD 추상 도식. OFFLINE이며 실제 Unity 캡처가 아님.
2. `Issue89_ColdVFX_Readability_TargetedRecommendations_20261111.md`, SHA256 **`d9fbd5bfd693e21f6560865830bf9b29c56bedba08a156165744dd38c11f5ec0`** — 6종 SHA 보존표·시각 결함 3개·수정 담당 구분 상세 보고서. ChatGPT Library `/LIMITLESS/Issue89_ColdVisual_Readability_20261111/`에 두 실물 업로드·등록 성공.

## 2. 승인 PNG 6종 SHA256 보존

| 종류 | 실물 승인 해시 SHA256 | 검증 출처 |
|---|---|---|
| VFX Chill v2 | `e50831520d21ca51b51bca0fab030446f5c15f034d699c6ee3fd79304d1691c2` | 기존 #89 Codex v2 납품·승인, 이 세션에 바이너리 없음 |
| VFX Frostbite v2 | `5b8ca776ec14c52fd671117182fa299fed31b8774d85f6843f8f62ec70b96415` | 상동 |
| VFX Frost Necrosis v2 | `59621e0b912e7c30544e559cb99446dcfb7f9c7c8dd1c846a07f32dce2dfd299` | 상동 |
| HUD Chill icon 128RGBA | `1f3ac724590a2d490cbe4a7cd28856c8ce5d888b130dde46a7d0008e6a3d0a16` | **이번 세션 실제 파일 바이트 재해시 일치** |
| HUD Frostbite icon 128RGBA | `b814b1df3b0a5dbba46205d3b6edf91339cc4f45d118ec19ac53afab43c89a03` | 상동 |
| HUD Frost Necrosis icon 128RGBA | `da2f175d50c47961df4c2fe6a227219a594ac9c3b0874c2c50e9edf39644be9d` | 상동 |

**정확성 주의:** VFX3은 GitHub/Codex가 이미 검사한 값이지 본 세션 신규 바이너리 SHA 재검사가 아니다. 승인 6종을 이번에 변경하지 않았으며 기존 승인도 무효화하지 않는다.

## 3. 남은 VISUAL_PARTIAL 3건 — 새 아트 아닌 기존 UI 배치 개선

| 대상 | 기존 실제 결과·위험 | 최소 보완 추천 / 책임 |
|---|---|---|
| 밝은 설원 **40px** 아이콘 | B안 실제 캡처에서 VFX/휠체어 경계8px 분리. 그러나 작은 한기 아이콘·VFX만으로 즉시 구별 PASS는 미확정 | **Codex UI**: 승인 아이콘+상태명+남은 턴, 괴사1/3·2/3·다음 중첩 즉시KO 별도 문자 유지; 960 실제 실픽셀·문자폭/명도만 표적 재검증. **아트 KEEP** |
| **128px** 휠체어 뒤 VFX 잔여 가림 | B안은 CharacterSprite 뒤 렌더/하향24px으로 캐릭터 본체 차폐를 줄임. 효과 일부가 휠체어에 가려지는 것은 남음 | **Codex Sorting/오프셋**: 기존 128px 크기/알파와 B순서를 유지하며 주변 효과의 식별성/가시 알파 잔존 검토; 캐릭터 앞으로 다시 올려 하체를 덮지 말 것. **아트 KEEP** |
| 태온·휠체어/좌하단 안내 UI 충돌 | 최종8캡처에서 잔여 시각 간섭 보고. 960 두 행/1680 HUD 상단 경계8px 개선됨에도 완전 PASS 아님 | **Codex HUD 배치/앵커**: 태온 실제 Sprite/휠체어/상태·안내 Rect 안전영역, 960 하단 그룹·가려짐을 표적 비교. HP/MP/2×3 버튼/선택 강조 유지. 실제 충돌 픽셀 좌표 **NOT_VERIFIED** |

**B안 기존 검증 수치 재사용:** 40px VFX↔휠체어 가시 알파 경계8px / 128px VFX 캐릭터 뒤·기존 시험점 하향24px / 960 상태배지2행 / 1680 HUD 경계8px. 특정 격리 조건 실측이며 보편적 모든 캐릭터 Import 공식값은 아니다. #89 99PASS/50PASS는 반복하지 않았으며 새로운 Unity QA PASS를 기록하지 않음.

## 4. 실제 Unity 증거와 오프라인 비교판의 명확한 구별

- **기존 Unity 캡처(이번 세션에서 실물 획득 못함):** 11/04 B안 A/B 실제 PNG14장, 로컬 `F:/study/codex/Project-Limitless/Temp/Issue89LayoutDelta20261104/index.html` 및 `Evidence/`. 정식 아이콘+B 8캡처는 로컬 `문서/00_프로젝트/Evidence/EODApproved20261104/`; GitHub 원격 바이너리 업로드 없음. 본 세션은 기존 Codex 결과의 **수치·상태만 재사용**.
- **이번 제작한 오프라인 판:** 승인 128아이콘에서 직접 생성된 **40×40 실축소 아이콘3 이미지** 및 추상 휠체어·HUD 도식. 실제 태온/휠체어/승인 VFX 효과를 배경에 합성한 Unity 스크린샷이 아니며, 실제 충돌 ROI나 픽셀 면적을 추정 수치로 발명하지 않음.
- **증거 판정:** `ICON_3_LOCAL_SHA_VERIFIED / VFX_3_PRIOR_CODEX_SHA_RECORDED / LAYOUT_B_PRIOR_UNITY_QA_REUSED / CURRENT_UNITY_SCREENSHOT_NOT_AVAILABLE / OFFLINE_BOARD_ONLY`.

## 5. 결론 / 승인 경계

- 6개 아트는 **이미 USER_ART_APPROVED**이므로 **재승인 요청 불필요**. `ART_KEEP` 6/6.
- 문제는 아트보다는 **Codex 표시 계층·앵커·Sorting·文字/안내 UI 배치** 우선. `TARGETED_LAYOUT_FIX_RECOMMENDED`. 실제 11/04 Unity 캡처를 가진 Codex가 격리 환경에서 필요 최소 표적 Delta만 확인해야 하며 이번 회차는 **Unity 실행·실제 코드 변경·새 그림 제작 0**.
- **필요 승인:** 향후 실제 UI 배치 변경/제품 원본 반영은 별도 사용자 승인. 기존 아트 재승인이나 새 VFX 제작 요청 없음.
- 기존 승인 아트/태온·휠체어/Owl/배경/Save/UserData/Unity Scene·Prefab/Battle UI·main **무변경**, main 직접 commit·Push/merge 0.

**최종:** `APPROVED_ART_6_BYTE_KEEP / OFFLINE_BOARD_1 / TARGETED_LAYOUT_FIX_RECOMMENDED / VISUAL_PARTIAL / NOT_UNITY_APPLIED_THIS_TURN`.
