# LIMITLESS Chapter3 서리들쥐 A — 전투 3 Strip v1 미술 불합격 표적 수정 v2 납품·QA (2026-10-20)

> **ART_FIX_V2_DELIVERED / CODEX_DELTA_QA_PENDING / USER_COMBAT_ART_APPROVAL_PENDING / NOT_UNITY_INTEGRATED**

- **브랜치:** `docs/art-new`
- **발주:** `docs/planning/문서/00_프로젝트/Chapter3_FrostFieldMouse_Combat_v1_VisualFail_3Strip_TargetedFix_20261020.md`
- **Issue:** [#30](https://github.com/HappyPaul812/Project-Limitless/issues/30)
- **이전 Codex 격리 QA:** 기술 36 PASS/0 FAIL, 시각 Attack FAIL, Hit FAIL, Defeat HOLD, 승인 Idle과의 색상/크기 불연속. 이 수치는 *이전 v1의 검사 실적*이며 이번 v2 Unity 검사 결과가 아님.

## 1. 입력 원본(바이트 보호)

- 이전 Combat v1 `Limitless_Chapter3_FrostFieldMouse_Combat_3Strips_v1.zip` SHA256 `24ef8dddf76fb5843e8e7d3fac05942f1450c9f39cf366b2422a0ab82cba6741` 확인.
- 승인 Idle `Sprites/FrostFieldMouse_A_Idle_4Frames.png` SHA256 `5afebfa1f95ca5673162513fdea63b025546aa836550c0ceb17f0334659988f2` 재확인.
- 승인 1포즈 PNG SHA256 `0516914ddad36af5854065d66d01b9664329aa89eea8b394480eed6af92e32c2`는 기획 정본값이며 파일 변경 없음.
- 설빙의 폭군 B·F1·마을·Chapter1/2 리뉴얼·Unity/Save/UserData·기존 원본 메타/GUID 변경 없음.

## 2. 실제 납품

- ZIP: **`Limitless_Chapter3_FrostFieldMouse_Combat_3Strips_Fix_v2.zip`**
- ZIP **SHA256 `518d210a0e5b29c7214ad960c0baa3d23a88d13d4f789b5f7420982ce701bfe3`**, 10,784,249bytes, 20파일, ZIP `testzip()==None` CRC PASS.
- **실물은 이 ChatGPT 대화에 클릭 가능한 ZIP 다운로드 첨부로 전달.** GitHub에는 QA/해시 문서만 등록하며, PNG/ZIP 바이너리를 GitHub나 사용자 Windows PC F:에 배치했다고 주장하지 않는다.

| 게임용 PNG | 크기/색상 | SHA256 |
|---|---|---|
| `Sprites/FrostFieldMouse_A_Attack_4Frames.png` | 2048×512 RGBA | `c83e35b5aebbce908b6025d5a071fafc463cfe301763f68806ecddb8bd4f576b` |
| `Sprites/FrostFieldMouse_A_Hit_4Frames.png` | 2048×512 RGBA | `695ed7db1f794c3f71df1b058eb5e91f01de7ecb125369264f3624b36c9fd3c2` |
| `Sprites/FrostFieldMouse_A_Defeat_4Frames.png` | 2048×512 RGBA | `3f6a52039b766c57a5367c7145e0234b68ac0a552940de1e88ffe8f2404f3395` |

나머지: Preview 각 4프레임 확인 PNG×3, APNG×3/GIF×3 (Attack/Hit 정확 8FPS, Defeat 마지막 프레임에 1750ms 검토용 hold), Idle→Attack/Idle→Hit 전환 APNG×2, 112px 전후 비교 PNG, Attack02/Defeat03 QA 확대 이미지, 128px 12프레임 비교 PNG, Manifest.md, QA/Offline_QA.json, SHA256SUMS.txt. **총 20파일**.

## 3. 표적 수정 내용·수치

| 프레임 | v1 알파 bbox [L,T,R,B] | v2 알파 bbox | 보완 |
|---|---|---|---|
| 승인 Idle00 | [22,124,490,490] | 변경 없음 | 공식 경계 자세 |
| Attack00 | [63,189,448,489] = 385×300 | **[22,124,490,490] = 468×366** | 승인 Idle00 RGBA 전체 셀 **픽셀 동일**, 위치/색/접지 오차0 |
| Hit03 | [66,197,445,489] = 379×292 | **[22,124,490,490] = 468×366** | 승인 Idle00 RGBA 전체 셀 **픽셀 동일**, 위치/색/접지 오차0 |
| Attack02 | [63,214,449,489] | [22,172,491,490] | 최고 전방 할퀴기를 기존 v1 Attack01 동작에서 이동/재배치. 기술 타격 이벤트 코드 변경 없음 |
| Defeat03 | [47,312,464,489] | **[47,312,464,489]** | 기존 알파 마스크 픽셀 **100% 동일**; 눕는 사망 실루엣·최종 프레임 Hold 보호 |

- Attack 흐름: **00 Idle 동일 준비 → 01 기존 몸통/앞발 준비 동작 → 02 최대 전방 할퀴기 → 03 승인 Idle03 회복**. 03 직후 런타임 Idle00 복귀. v1 Attack02의 약한 파츠 포즈는 가장 강한 공격을 02로 이동하면서 제외.
- Hit 흐름: 00 피격 반사 → 01 움찔/웅크림 → 02 회복 → **03 Idle00 픽셀 동일**.
- Defeat 00~03: 원래 몸이 쓰러지는 4단계 구조 유지. 03은 형상/위치/알파 불변, **털 영역만 국소 색상 정합**. 정식 Unity의 frame03 hold 규칙은 변경하지 않음.
- **전체 일괄 색상 필터/일괄 120% 확대 미사용**. 각 전투 자세의 털·등·머리 영역에서 국소 명암/냉회백 색상을 조정하고, 붉은 귀·살색 다리·긴 꼬리·눈·앞니·발톱/외곽선을 보존하도록 마스크 적용. 공격 준비/할퀴기 및 피격 웅크림 자세의 제한적인 배치·크기 조절만 수행. **시각 완성도는 Codex/대표 검토 대기**.

## 4. 오프라인 QA (PASS 범위 한정)

- PNG3 모두 2048×512 RGBA, 4×[512×512] 정상 분리; 각 셀 불투명 픽셀 존재, 투명 경계 최소 1px 이상, 머리/꼬리/장비 영역 캔버스 바깥 접촉0.
- Attack00 및 Hit03 `Image.tobytes()` == 승인 Idle00 **True**.
- Defeat03 `A` 채널 512×512 전 픽셀 == 기존 v1 Defeat03 **True**. 이를 얼굴/털 RGB 동일이라고 주장하지 않음.
- Attack/Hit/Defeat 각 4프레임 모두 데이터 상이, 8FPS 정확 125ms APNG 미리보기; GIF는 포맷 10ms 제약으로 120/130ms 교대.
- Defeat APNG/GIF은 끝 자세를 길게 보여주는 검토용 미리보기로만 루프함. **실게임 재생은 마지막 03 고정**.
- 승인 Idle의 PNG 자체와 다른 공식 게임 에셋은 수정하지 않았고, 외부 Unity Editor/Runtime/Compile 검증은 이번 작업에서 실행하지 않음.

## 5. Codex 변경분 Delta QA 및 사용자 승인

1. Codex는 이전 **36 PASS 전체 재실행 금지**. `Attack00==Idle00`, `Hit03==Idle00`, Attack02 타격 프레임, Defeat03 사망 실루엣 및 Hold, 112/128px 동일 Unity UI 배율의 털색/시각 연속성만 짧게 검사할 것.
2. 보정 색감의 최종 스타일 합격·할퀴기 강약 및 Attack→Idle 자연스러움은 **바울 대표 최종 미술 승인 대기**. 자동 패스 판정 금지.
3. 기존 Unity 게임·MonsterDefinition/Animator/Scene, F1/서릿길 마을 환경, 원본 승인 Idle/1포즈, 보스B 변경/정식 통합 금지. `main` 직접 Commit·Push·merge 없음.
