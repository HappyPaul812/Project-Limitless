# LIMITLESS / Main24 · Issue #74 · Frost Owl Attack02→03 연결 표적 보완

**작업일:** 2026-11-03  
**담당 브랜치:** `docs/art-new`  
**상태:** `ATTACK02_TARGETED_REVISED_CANDIDATE / OFFLINE_FILE_QA_PASS / USER_MOTION_ART_REVIEW_PENDING / UNITY_NOT_VERIFIED / NOT_UNITY_APPLIED`

## 1. 최신 지시·검수 근거

- [Codex #74 실물 검수](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6595705479): 직전 Continuity ZIP은 CRC/SHA PASS, Attack00·01의 동작 대비·얼굴 복원은 개선됐지만 **Attack02→Attack03의 몸통·발·꼬리·깃털 전환이 급격**하여 `DESIGN_CONTINUITY_REVISE`. Attack03=승인 정지 v3=Idle00의 SHA가 동일함은 검증. 별도 SEAM GIF는 당시 실제 1개 정지 프레임으로 병합되어 애니메이션 증거로 사용할 수 없었음.
- [Issue #74 기획 최소후속 지시](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6595843983): 승인 v3·Idle8·Attack03 원본 및 Attack00·01을 보호하고 **Attack02만 실제 수정**하여 01→02→03→Idle00 시퀀스를 확인. 냉기 VFX 별도 HOLD.
- **본 회차는 로컬 오프라인 이미지 제작 및 파일 QA**. Codex 실제 Unity Scene/Animator 검수, 본편 적용, 기존 Main24 #83 QA 전수 반복은 하지 않음.

## 2. 실제 납품·SHA

- **ZIP:** `Limitless_FrostOwl_Attack02_Continuity_20261103.zip`, **3,992,293 bytes, 20 entries**.
- **ZIP SHA256:** `d6648b7451dc2184c2187c0b1c2f8e0f52fc800ac6975753d0f6d38f6fbf9192`.
- **ZIP CRC:** `testzip() PASS`; `SHA256SUMS.txt` **19/19 PASS**, 파일명 중복0.
- **사용자 전달:** ChatGPT Library **`/LIMITLESS/Main24_Issue74_Owl_Attack02_20261103/`**에 ZIP과 전후 비교 PNG 실제 업로드 완료. 대화 작업 파일 `/mnt/data/Limitless_FrostOwl_Attack02_Continuity_20261103.zip` 존재. 사용자의 Windows F:\Downloads 자동 복사 또는 수령 후 로컬 SHA 검사는 미실시. GitHub에는 PNG/GIF/ZIP **바이너리 업로드0**, 보고서·인계 문서만 커밋.

| 프레임 | 이번 처리 | SHA256 |
|---|---|---|
| `Candidate/Attack/FrostOwl_v3_Attack_00_512_RGBA_CANDIDATE.png` | **KEEP** — 기존 바이트 복사 | `b6029c30bf76e8b3f32843184f812c7ca6254f07b79214eca4069cd42a8fe547` |
| `Candidate/Attack/FrostOwl_v3_Attack_01_512_RGBA_CANDIDATE.png` | **KEEP** — 기존 바이트 복사 | `a98aab1c6a14caae1ed255882e90ec7fc3f1bdfa28693409d307a1cc1fed7142` |
| `Candidate/Attack/FrostOwl_v3_Attack_02_512_RGBA_CANDIDATE.png` | **이번 유일한 새 모션 원본 PNG** | `6dcb1799e0970cd08ceadc111e7e2645f21eb973ae75f9b859c2ce356d07e936` |
| `Candidate/Attack/FrostOwl_v3_Attack_03_512_RGBA_CANDIDATE.png` | **KEEP** — 승인 v3/Idle00과 바이트 완전 동일 | `81a31cf37cb834799ab5c9428da612ab13a5a496025805685daefed7b5d3c467` |

- 입력 파일 **이전 ZIP**: `Limitless_FrostOwl_Attack4_Continuity_20261103.zip`, SHA256 `dd50da39530380047c8d4a6dd728d624ff828fc2dde089abc708be0558bc6856` — 읽기 전용으로 비교.
- 원본 `Idle8` 프레임 8개는 **수정·재양산·ZIP 재등록0**. 승인 v3 원화도 입력의 SHA로만 참조, 변경0. Attack03 바이트가 승인 Idle00과 정확히 같음.
- 이전 **분리된 미승인 냉기 VFX**는 이전 ZIP의 `VFX_UNAPPROVED_HOLD/FrostOwl_IceArc_Detached_VFX_HOLD_512_RGBA.png` SHA256 `46215c1cf7fd3fa3f94af92f4dd457436cf70711ca95b1bed86dc527f49d761c`로 **독립 보존**. 본 ZIP에 중복 포함하지 않으며 Attack01 본체에 재삽입하지 않음.

## 3. 실제 수정 — 디자인 연속성과 기존 모션 보호

- **새 Attack02 소스 한 장만 512×512 투명 RGBA**로 생성. 기존 Attack02의 몸통·발·꼬리·얼굴·깃 질감이 승인 Attack03과 크게 불일치하므로, **승인 Attack03 픽셀을 기준으로 얼굴·몸통·발·꼬리를 고정하고 바깥 날개만 공간적으로 중간 각도로 변형**한 후속/복귀 포즈를 만들었음. 다른 Attack 프레임·기존 정상 Idle8을 전혀 재작업하지 않음.
- **승인 03과 새 02의 하단 y345..511 RGB/알파 픽셀 배열이 100% 동일**: 다리·발·꼬리의 급변 방지. 중앙 얼굴과 몸통 중심은 날개 변형 영역 외의 원본 픽셀을 유지. 바깥 날개의 국소 비강체 변위는 최대 대략 35px 가로, 50px 세로이며 **전신 일괄 축소/캔버스 확장 없음**.
- 기존 Attack02와 새 Attack02 사이 RGBA가 다른 픽셀은 **126,301개**. 이는 작은 RGB 톤 조정만 한 것이 아니라 한 프레임의 몸통·하체 앵커를 승인 v3 기준으로 교체한 효과를 포함하므로 `SMALL_PIXEL_DELTA`라고 주장하지 않음. 대신 **수정하는 게임용 PNG 파일 수는 정확히1**, 디자인 전면 재설계/다른 프레임 양산0.
- **실루엣(alpha>8) IoU 오프라인 기준**:
  - Attack01→02 **기존 0.4377 → 후보 0.6761**
  - Attack02→03 **기존 0.4415 → 후보 0.8654**
  - Attack03→Idle00 **완전 동일**.
  이 측정은 인접 프레임의 형태 차이가 줄었다는 정량 참고이지 관절 운동·자연스러운 8FPS 모션의 최종 미술 PASS는 아니다.
- **기술 검사:** Attack00~03 모두 512×512 RGBA / alpha>8 기준 상·하·좌·우 접촉 0, 시트셀 4/4 **실제 PNG와 픽셀 exact 일치**. GIF 4프레임, 모든 프레임 순서00→03, **120/130/120/130ms = 500ms/4프레임 = 평균8FPS**. 디더링 배경 GIF는 미리보기이고 PNG가 실제 투명 원본.

## 4. 시트·GIF·전후 검토 자료

- `Candidate/Sheet/FrostOwl_v3_Attack4_8FPS_2048x512_RGBA.png`: **2048×512 RGBA**, 4×1, 변경된 02 포함, 00/01/03은 원본 SHA 보호.
- `Preview/Attack4_8FPS_ACTUAL.gif`: **실제 4개 다른 애니메이션 프레임**을 같은 시간 간격으로 재생. 기존 실패한 단일 정지 SEAM GIF가 아님.
- `Preview/Attack01_02_03_Idle00_TRUE_SEQUENCE_8FPS.gif`: 사용자 검토 전용 **4단계 재생 마커가 움직이는 4프레임**. Attack03과 Idle00의 이미지 자체는 정확히 같지만 진행 라벨/마커가 달라 4단계가 구분됨. 마커는 **게임 스프라이트 본체에 포함하지 않음**.
- `Preview/Attack02_03_Idle00_BeforeAfter_128_40_OnePage.png`: 기존02/수정02/승인03/Idle00을 **실제 PNG에서 축소한 128px·40px**과 함께 표시하고, 아래에 01→02→03→Idle00 순서·승인/보존 상태를 한눈에 보여주는 **OFFLINE** 판.
- `Preview/Attack00~03_{128,40}px_DERIVED.png` **8개**는 실제 512 PNG에서 LANCZOS 축소한 검토용 이미지. `Manifest.json`에 원본·출력 SHA/치수·PPU512/Pivot(0.5,0.5)/Scale1.0/8FPS/DeltaQA, `QA/DeltaQA.json`, `README_KR.md`, `SHA256SUMS.txt` 포함.

## 5. 안전·최종 승인

- **Unity Import 계약만 명시**: PPU512/Pivot중앙(0.5,0.5)/Scale1.0/8FPS. 실제 `.meta`·Animator·Scene·Prefab·MonsterDefinition·Save/UserData·BuildSettings·Main24 기능 미수정. `main` 직접 commit/Push/merge0.
- 기존 Main24 #83 왕복90PASS/23PASS, F2 승인 자연형 Stump·Sign/Fence KEEP_CANDIDATE, F1·Main23 환경43+승인지면9, NPC·몬스터·펫 및 리뉴얼 Bee/Spider 변경0.
- **`OFFLINE_FILE_QA_PASS`**, 그러나 Attack01→02 움직임과 02→03의 날개·몸통의 자연스러운 연결을 **바울 대표가 직접 미술 검토할 필요**가 있음. Unity에서 512/128/40 화면 표시·전투 UI·SpriteRenderer·8FPS 재생을 실제 테스트하지 않았으므로 `UNITY_TECH_PASS`로 표기하지 않음.
- **바울 대표 최종 모션 미술 승인 필요: `USER_MOTION_ART_REVIEW_PENDING`**. 최종 승인 시 Codex 격리 Unity에서 **변경 Attack02 및 01→02→03→Idle00 최소 DeltaQA**를 먼저 하고, 사용자 별도 지시 전 본편 게임 적용 및 `main` 병합 금지.
