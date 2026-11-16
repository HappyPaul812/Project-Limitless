> 2026-11-16 EOD 선별 인계: 출처 `origin/docs/art-new`, Git blob `1974292438913ea0609bd5d87569ef13db1a7662`. 이하 보고의 승인/미적용/PENDING 표시는 작성 당시 상태입니다. 최신 결재는 EOD 보고 우선: Stoat KeyPose·Idle4·Attack4v2·Hit4 미술 승인/Unity미적용, NPC02/17·10/07 로컬적용, Issue85 Import/GUID HOLD. NPC 손 한정 지시는 기획 해석 오류였으며 최신 정정대로 인계합니다. 기존 제품·미완료 자산을 새로 적용하지 않습니다.

# LIMITLESS / 메인 신규 아트 / Chapter3 M2 설원족제비 Attack4 파일럿 — 2026-11-16

**담당:** `docs/art-new` · **이슈:** [#100](https://github.com/HappyPaul812/Project-Limitless/issues/100)  
**판정:** `KEYPOSE_USER_APPROVED / IDLE4_USER_APPROVED / ATTACK4_ART_CANDIDATE / FILE_ZIP_QA_PASS / USER_ATTACK_ART_REVIEW_PENDING / NOT_UNITY_APPLIED`.

## 1. 확정 승인/범위
- 바울 대표의 1포즈 승인(“족제비 보니까 좋구만...”) 및 이번 직접 지시 **Idle4 승인**을 각각 확정으로 인계합니다. 두 원본은 수정하지 않았습니다.
- **이번 신규 아트**는 가로 Attack4 시트1개뿐입니다. Hit / Defeat / Walk / Run / 펫 시트·능력, MonsterID / Spawn / EXP / Loot / Unity 구현은 제작하지 않았습니다.
- 참고: 기존 비규격 2508×627 RGB 이미지 생성 결과는 검토용으로만 사용했고 **게임용 PNG를 그 파일에서 리사이즈하거나 검은 배경 제거해 납품하지 않았습니다**. 실제 납품 시트는 기존 승인 1포즈·Idle4 **투명 원본 RGBA**에서 머리/목 국소 변위 및 공격용 입/송곳니만 추가해 만들었습니다. 원본 파일 SHA 보존.

## 2. 실제 신작 파일/규격
- `Sprite/SnowStoat_M2_Attack4_2048x512_RGBA_CANDIDATE.png`  
  **2048×512 PNG / RGBA / 4셀(각 512×512) / 우향**, SHA256 `a5f5856115473329d1b1a00a5db1022f1831ec48955e5e34f29b711dfb12141a`.
- Frame00 `READY`(작은 몸 낮춤) → Frame01 `OPEN BITE`(입 벌려 단일 물기 준비) → Frame02 `SNAP`(전방 순간 물기) → Frame03 `RECOVER`(원위치). 비행·과장된 점프 없음. **4프레임 RGBA 상이/512 정렬 PASS**.
- 검수용 `Sprite/SnowStoat_M2_Attack_00_512_RGBA_QA.png`~`03_512_RGBA_QA.png` 4장 (추가 게임용 동작 아님).
- **진짜 축소 출력** `Preview/SnowStoat_M2_Attack4_Actual_128px_RGBA.png` / `...40px_RGBA.png`, `Preview/SnowStoat_Attack4_512_128_40_Review_OFFLINE.png`, `Preview/SnowStoat_Attack4_8FPS_OFFLINE.gif` (확인용; Unity FPS 확정 아님).
- 각 프레임 alpha>8 bbox: F00 `[28,244,481,455]`, F01 `[29,240,490,455]`, F02 `[29,242,494,455]`, F03 `[29,241,483,455]`. 프레임 캔버스 가장자리 비투명 픽셀0. 각 프레임 사용 승인 소스와의 y396~511(발 접지) RGBA 변경 픽셀0.

## 3. 무결성/보호
- 전체 ZIP **`SnowStoat_Issue100_Attack4.zip` / 1,092,522 bytes / entries12**, SHA256 `2683a8bdb84595e648f56914944441b856986efee8647c0d2b92a7e5f9b13daf`.
- 압축 `CRC PASS`, ZIP 내부 `SHA256SUMS 11/11 PASS`, `Manifest.json`, `Docs/DesignNotes_SourceDeclaration_QA.md` 포함. Sheet RGBA/2048×512 및 셀 분리/고정 발 접지 PASS.
- 원본 보존 SHA: 승인 1포즈 `fd25ef50528a5f630e44382e3fc40a3df607a267176f758706962a2cbc41e778`; 승인 Idle4 `ee6951b0333ba6586b4009ccf51cfd3a397d69f839552fe88a709f9c34bf0633`; 두 승인 소스 수정0.
- 기존 서리들쥐/서리올빼미/Chapter2 몬스터/환경/보스 아트, Unity **6000.6.5f1**, Issue #85 HOLD, Save/UserData, Discord, GUID/meta, Scene/Prefab/BuildSettings 변경0. 기존 대규모 Unity QA 반복0.
- **실파일은 이 ChatGPT 대화에 PNG/ZIP 링크로 제공, GitHub에는 텍스트 보고서만**. F:\ 원본 PC에 자동 저장했다거나 GitHub에 PNG/ZIP 업로드했다고 주장하지 않음. 512/128/40판은 **OFFLINE**, 실제 GameView의 40px 전투 가독성/PPU·Pivot·타격판정 검증 NOT RUN.
- `main` 직접 커밋·Push·Merge 금지 준수.

## 4. 다음 단계
**Attack4의 대표 시각 승인 필요.** 승인 전 Hit / Defeat 제작·Unity 본편 적용·새 펫/몬스터 수치/스폰 구현 금지. 사용자 승인 후에도 실제 Unity import·Animator·전투 타격 타이밍은 별도 Codex 계약에서 결정.
