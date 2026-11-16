> 2026-11-16 EOD 선별 인계: 출처 `origin/docs/art-new`, Git blob `6a9797c633814a04d6e8f0d8b9c229bc9dde771b`. 이하 보고의 승인/미적용/PENDING 표시는 작성 당시 상태입니다. 최신 결재는 EOD 보고 우선: Stoat KeyPose·Idle4·Attack4v2·Hit4 미술 승인/Unity미적용, NPC02/17·10/07 로컬적용, Issue85 Import/GUID HOLD. NPC 손 한정 지시는 기획 해석 오류였으며 최신 정정대로 인계합니다. 기존 제품·미완료 자산을 새로 적용하지 않습니다.

# LIMITLESS / 메인 신규 아트 / Chapter3 M2 설원족제비 Hit4 파일럿 — 2026-11-16

**담당:** 메인 신규 아트 `docs/art-new`  
**Issue:** [#100](https://github.com/HappyPaul812/Project-Limitless/issues/100)  
**사용자 최신 확정:** [Attack4 v2 시각 승인](https://github.com/HappyPaul812/Project-Limitless/issues/100#issuecomment-6912398648)  
**최종 상태:** `KEYPOSE_USER_APPROVED / IDLE4_USER_APPROVED / ATTACK4_V2_USER_APPROVED / HIT4_ART_CANDIDATE / USER_HIT4_ART_REVIEW_PENDING / OFFLINE_FILE_QA_PASS / UNITY_NOT_APPLIED`.

## 1. 신규 제작 범위

- **Hit4만** 2048×512 투명 RGBA 가로4셀(각 512×512), 우향, **Sprite PNG 1장**. Frame00 움찔/머리 젖힘, Frame01 무게중심 약간 뒤로 움츠림, Frame02 자세 회복, Frame03 원자세 복귀. 과도한 공중 점프·비정상적인 체형 변경·쓰러지는 Defeat 동작 없음.
- 승인 1포즈를 **변경 없이 소스로 읽어** 국소적인 머리/목/상체 반응만 적용. 긴 몸통, 작은 귀, 주둥이, 흰 털 회청 명암, 검은 꼬리끝을 기존 원화에서 보존. **Frame03은 승인 KeyPose / Idle00의 전체 RGBA 픽셀과 동일**.
- **기존 승인 원본 SHA256 (변경 전·후 확인 동일):** KeyPose `fd25ef50528a5f630e44382e3fc40a3df607a267176f758706962a2cbc41e778`, Idle4 `ee6951b0333ba6586b4009ccf51cfd3a397d69f839552fe88a709f9c34bf0633`, Attack4 v2 `5175483e486889736f4f89402ec8a04595491c5c587e51a3f6bbd1365391364a`.
- 참고: 이전 이미지 생성기로 출력된 **2172×724** 원화 시안은 정확한 스프라이트 규격과 기존 캐릭터 비율을 보장하지 못하므로 **최종 게임용 PNG로 사용하지 않음**. 최종 Hit4는 승인된 512 RGBA 원본에서만 파생.

## 2. 실제 파일 및 무결성 검증

- **Sprite** `Sprite/SnowStoat_M2_Hit4_2048x512_RGBA_CANDIDATE.png` — 2048×512 RGBA / 4셀512×512, **SHA256 `dc2c2588b4ec3d06766ab042c4297898aebca4f907593a5820ce8cbad2f6aab5`**, 333,581bytes.
- **단독 QA 프레임** `Sprite/SnowStoat_M2_Hit_00_512_RGBA_QA.png` ~ `03_512_RGBA_QA.png`, 각 512×512 RGBA, 4프레임 구별 **PASS**.
- **실축소** `Preview/SnowStoat_M2_Hit4_Actual_128px_RGBA.png`, `...40px_RGBA.png`, `Preview/SnowStoat_Hit4_512_128_40_Review_OFFLINE.png` (밝은 설원/어두운 배경). **실제 Unity GameView가 아닌 오프라인 시각판**.
- **움직임 확인** `Preview/SnowStoat_Hit4_8FPS_OFFLINE.gif` 125ms ×4 프레임 = 8FPS 미리보기. 실제 Unity 애니메이션 스피드/타격판정 확정 아님.
- 보호 QA: 캔버스 네 변 불투명픽셀0, 원본 y425..511의 발 접지·하단 RGBA 각 프레임 동일 **4/4 PASS**. Frame03 전체 KeyPose RGBA 동일 **PASS**. 이미지 원본/재시각화 교체 없음.
- **ZIP `SnowStoat_Issue100_Hit4_20261116.zip`**, **1,156,630bytes / 12 entries**, SHA256 **`e058e9610ec90cf9c2f7e8b0f48b5cde42a70fe7319f861d5c53d1736ecaadfc`**. Python zipfile `testzip()` CRC **PASS**, Manifest payload 크기·SHA **10/10 PASS**, SHA256SUMS payload10+Manifest = **11/11 PASS**.
- **해시 구조 비순환:** Manifest.files = payload 10개만, SUMS = payload 10개+최종 Manifest(11개), SUMS 자기 자신 제외. Manifest가 Manifest 또는 SUMS를 참조하는 순환 해시 없음.
- ZIP 내부 `Docs/DesignNotes_SourceAndQA.md`, `Manifest.json`, `SHA256SUMS.txt` 포함. 이번 대화에 실제 ZIP/PNG/Preview/GIF 제공. 개인 PC F:\Downloads 자동 복사하거나 GitHub에 PNG/ZIP 바이너리 업로드했다고 주장하지 않음.

## 3. 보존·승인 구분

- **승인 완료:** KeyPose / Idle4 / Attack4 MouthFix v2. **이번 Hit4는 사용자 시각 승인 필요** (`USER_HIT4_ART_REVIEW_PENDING`).
- Defeat4·펫 전용 동작/효과·전투 수치/EXP/드롭·MonsterID/Spawn·Unity 적용/실플레이 **미착수**.
- Unity **6000.6.5f1**, Issue #85 HOLD, 기존 공식 Mouse/Owl·Ch1/2·환경·보스, 승인 KeyPose/Idle4/Attack4, Scene/Prefab/GUID/meta/Save/UserData/Discord/main **변경0**. 기존 Unity 대형 QA 복제0, 직접 main commit·원격 Push·Merge0.
