# LIMITLESS / 메인 신규 아트 / Chapter3 M2 설원족제비 Defeat4 파일럿 — 2026-11-17

**담당 브랜치:** `docs/art-new`  
**Issue:** [#100](https://github.com/HappyPaul812/Project-Limitless/issues/100)  
**근거:** [11/17 Defeat4 발주](https://github.com/HappyPaul812/Project-Limitless/issues/100#issuecomment-6931011521), [11/16 Hit4 사용자 승인](https://github.com/HappyPaul812/Project-Limitless/issues/100#issuecomment-6913468404).

**상태:** `KEYPOSE_IDLE4_ATTACK4V2_HIT4_USER_APPROVED / DEFEAT4_ART_CANDIDATE_DELIVERED / OFFLINE_FILE_QA_PASS / USER_DEFEAT4_ART_REVIEW_PENDING / UNITY_NOT_APPLIED`.

## 1. 사용자 미술 승인 원본 보호

기존 네 원본을 **Read-only**로 읽어 이번 작업 시작·마감에 실제 SHA256을 재측정, 아래 값과 완전 일치. 기존 원본 PNG·게임 자산 덮어쓰기 0.

| 승인 원본 | 실제 원본 SHA256 |
|---|---|
| KeyPose 512 RGBA | `fd25ef50528a5f630e44382e3fc40a3df607a267176f758706962a2cbc41e778` |
| Idle4 2048×512 RGBA | `ee6951b0333ba6586b4009ccf51cfd3a397d69f839552fe88a709f9c34bf0633` |
| Attack4 v2 2048×512 RGBA | `5175483e486889736f4f89402ec8a04595491c5c587e51a3f6bbd1365391364a` |
| Hit4 2048×512 RGBA | `dc2c2588b4ec3d06766ab042c4297898aebca4f907593a5820ce8cbad2f6aab5` |

## 2. 신규 Defeat4 시트 1종

- **신규 게임용 아트 후보**: `Sprite/SnowStoat_M2_Defeat4_2048x512_RGBA_CANDIDATE.png` — **2048×512 투명 RGBA / 가로4프레임·각512×512 / 우향**, **403,477bytes**, SHA256 **`8bc97ce0af5d4896d2259df39972ea018ea1a6e494883349cc3b3065926bfe4f`**.
- 동작: **00 균형 상실 → 01 앞다리 굽혀 몸 낮춤 → 02 쓰러짐 → 03 완전히 엎드린 자세 유지(HP 0)**. Hit4의 Idle 복귀가 아닌 쓰러짐의 끝자세로 분리.
- 검증용 독립 `Sprite/SnowStoat_M2_Defeat_00_512_RGBA_QA.png`~`03` 4개. 4프레임 실제 시각·RGBA 데이터가 서로 다름. 셀 좌우·상하 끝에서 알파0. 네 셀 발·몸 아래쪽 표면은 공통 바닥 y459를 목표로 정렬, 마지막은 본체 중심 높이 하강(가시 bbox y=336~459), HP0 이후 자동 복귀 동작이 없음.
- **소스 및 조합 방식:** 본 세션에서 생성한 투명 신규 Defeat4 4포즈 원화 `눈족제비_패배_애니메이션_4프레임.png` (2172×724 RGBA)를 **동작 소스로만** 사용. 서로 겹치지 않는 네 포즈를 알파 분리한 다음 비율 유지 축소·공통 셀에 배치하여 게임 규격으로 정규화. 기존 승인 네 PNG에서 프레임을 복사하여 Defeat라고 속이거나 제3자 게임 에셋을 가져오지 않음. 신규 생성 동작은 승인 디자인의 흰 털/회청 명암, 짧은 다리, 검은 꼬리끝, 귀·주둥이 및 긴 몸체를 동일하게 유지하도록 구성. `Docs/DesignNotes_SourceDeclaration_QA.md`에 원화 출처·Crop/QA 기록.
- Preview: `Preview/SnowStoat_Defeat4_Actual_128px_4Frames_RGBA.png`, `...40px_4Frames_RGBA.png`, `Preview/SnowStoat_Defeat4_512_128_40_Review_OFFLINE.png` (512/128/40 실제 다운스케일+밝은 눈/어두운 화면 시뮬레이션). **Unity 실제 GameView가 아니라 OFFLINE**.
- `Preview/SnowStoat_Defeat4_8FPS_FinalHold_OFFLINE.gif`: **프레임 시간 120/120/120/1750ms**, 첫 3프레임은 GIF 10ms 제한에 따라 약8.33FPS(8FPS 요구를 10ms 단위로 근사), 마지막 포즈 **1.75초 HOLD**; GIF는 **Loop Extension 없음(재생 종료 후 마지막 프레임 유지)**. 실제 Unity Animator 설정을 의미하지 않음.

## 3. 실제 ZIP / Manifest 무결성

- 결과 ZIP `SnowStoat_Issue100_Defeat4_20261117.zip` — **1,454,055bytes, 12 entries**, SHA256 **`34f23d12ee4d4580e72f00ec0deb143b1d5b33aaa6d8d132c6495579961d6ad2`**.
- Python zipfile `testzip()` **CRC PASS**; 압축파일을 다시 읽어서 재검증한 **Manifest payload 10/10 파일크기+SHA256 PASS**, **SHA256SUMS 11/11 PASS**. Manifest에는 내용물10개만, SUMS에는 내용물10개+확정 Manifest SHA만 기재. Manifest/SUMS 자기참조 및 순환 참조0. ZIP 안에서 시트 RGBA·2048×512·개별 셀 추출 4개·각 512×512 일치 PASS.
- 사용자에게 ZIP, SpritePNG, 비교판, GIF 전달. GitHub에는 Markdown 보고만. **User F:\Downloads 직접 복사0, Unity 적용0, 신규 대형 QA 복제0**.

## 4. 승인 대기 및 보호

**대표님 Defeat4 미술 시각 승인 필요.** 기존 KeyPose·Idle4·Attack4 v2·Hit4는 모두 **USER_ART_APPROVED**이며 원본 변경0. Defeat4 승인 전 공식 등록·추가 펫동작·MonsterID·Spawn·전투 수치·EXP·Drop·Unity Import/Animator/씬 구현 금지.

Unity **6000.6.5f1**, Issue **#85 HOLD**, 서리들쥐/서리올빼미/환경/보스 승인 자산, Save/UserData, Discord, 기존 공식 원본/Scene/Prefab/GUID/meta/main 원형 보호. `main` 직접 commit·Push·Merge 0. 다른 팀 파일 변경0.
