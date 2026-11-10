# LIMITLESS / Chapter1 UNKNOWN 4종 오리지널 원본 제작 이력 추적 — 실제 라이브러리 PNG ↔ GitHub ↔ Unity

> **최종 마감 / 2026-11-10 (후속 갱신)**: 아래 본문의 '슬라임 픽셀 검사 대기' 문구는 당시 작업 단계 기록입니다. 이후 [Codex Issue #57 최종 보고](https://github.com/HappyPaul812/Project-Limitless/issues/57#issuecomment-6765905198)에서 바울 대표가 **실제 사용 중인** `01_GrassSlime_Walk.png`를 보고 **“그래 이거야. 우리가 만든거야”**라고 팀 원본임을 직접 확인했습니다. 따라서 슬라임의 저작 주체 확인 결과 `ORIGINAL_CONFIRMED`로 승격되어 Ch1 잔여 7종 전체 `ORIGINAL_CONFIRMED_7_OF_7`, 기본 펫 3종 `ORIGINAL_CONFIRMED_3_OF_3`입니다. **사용자의 직접 원본 확인과 파일 변환 픽셀 검증은 다릅니다**: 예고했던 Library 원본 1254² ↔ 현행 1256² 1:1 픽셀 비교 및 우·하 투명 2px 전수검사는 실제로 `NOT_RUN`입니다. CODEX `codex/ch1-monster-provenance-final` 마감 문서 **로컬 commit `1c8f94c9c9a3241a651ed46fb0547e2bf7ed67d1`**, 제품/원본 local main 변경 0, GitHub 원격 Push 없음. ZIP은 실물 PC에 없었으므로 **입력 대기 조건 해제**, 재요청·재제작·반복 감사 불필요. Issue #57 `PROVENANCE_AUDIT_CLOSED`. 외부 몬스터·펫 실사용 확정 0(해당 감사 범위)이며 법적 독점 저작권 보증과는 별개입니다.

작성일: **2026-11-10** · 담당: 총괄 기획 · 브랜치: `docs/planning` · 관련 [Issue #57](https://github.com/HappyPaul812/Project-Limitless/issues/57#issuecomment-6764431147)

> **이번 추적 결론:** 네 종 모두 ChatGPT Library에서 **당시 제작 PNG 원본을 찾았음**. B2 세 종은 원본 라이브러리 PNG SHA256이 Codex가 검증한 **현재 Unity 실사용 Sprite PNG SHA256과 바이트 단위로 완전 동일**. 초원 슬라임은 1254 원본과 등록 커밋 및 1256 패딩 변환 절차를 연결했으며, **현재 Unity 1256 파일의 실제 픽셀 단위 동등성은 사용자 로컬에서 마지막 확인 필요**. 재제작·교체 대상 0.

## 1. 4종 라이브러리 실물 확인 결과

다음 SHA256은 2026-11-10에 **ChatGPT Library 원본 PNG를 실제 바이트로 materialize하여 해시를 직접 계산**한 값. 보관/업로드 시각은 Library 파일의 등록시각으로, 초기 생성 시작시각과 같다고 단정하지 않음. 한국시각 KST(UTC+9) 표기.

| 몬스터 | 라이브러리 원본 PNG 이름 | Library 등록 시각(KST) | 라이브러리 원본 SHA256 | Codex 활성 Unity PNG SHA256 | 동일 여부 |
|---|---|---|---|---|---|
| `grass_slime` | `귀여운 숲 슬라임 16종 스프라이트 시트.png` | **2026-08-19 16:06:30** | `d07ea34216b1e2ff3ecbe1b8afb23d0bada56e5e944109fe8652602a59b21991` | `1c422147f0a8fe7ee234e0ab771ab098ac32c44338d5efecc4f3f08a3174de34` | **규격/변환 경로 확인, 픽셀 1:1 실파일 대조 PENDING**. 원본 1254² → 투명 여백 2px 우·하 추가한 현재 1256²; 변환이므로 PNG SHA 불일치 정상 |
| `monster_silent_echo` | `Silent_Echo_128x128_Sprite_Sheet.png` | **2026-09-28 09:40:31** | `1097c2268baca8b51435edb845dcc2f459aee601a799c47c34e17c87a711ddb4` | **동일 SHA** | **BYTE_EXACT_PASS** |
| `monster_seal_guardian` | `Seal_Guardian_128x128_Sprite_Sheet.png` | **2026-09-28 09:40:34** | `8ec559c56d2d814e5acb3dda45c928f1ad06302f7754b43fb5bc96e95e9b02fc` | **동일 SHA** | **BYTE_EXACT_PASS** |
| `monster_silent_warden` | `Silent_Warden_128x128_Sprite_Sheet.png` | **2026-09-28 09:40:36** | `97ef560e9258cbc17345ae51c017e9e113c1d840515e205698214f444a53e656` | **동일 SHA** | **BYTE_EXACT_PASS** |

- B2 세 시트 실물: 모두 **512×640 RGBA, 128×128 × 4열×5행**, 4/4/4/2/4 프레임 구조.
- Slime 원본 실물: **1254×1254 RGBA, 4×4**. 사용자의 원본이 **동일 1254² → 1256² 변환**된 사실은 당시 게임 구현 문서가 기술함.
- 라이브러리의 추가 시각증거: `던전 B2 침묵의 파수꾼 설계도.png` Library 등록 **2026-09-28 07:47:25 KST** — 침묵의 잔영/봉인 수호체/침묵의 파수꾼의 외형·128px 애니메이션 설계·보스 기획이 담겨 있음. `침묵의 수호자 픽셀 스프라이트 시트.png` **08:56:56 KST**에 보관(1491×1055 RGBA)된 선행 시안도 존재하지만, 이는 **현재 B2 세 장과 SHA 동일한 최종 시트가 아니므로 원본 바이트 증명으로 사용하지 않음**.

## 2. GitHub 최초 등록 커밋 및 생성→게임 연결 경로

### grass_slime
1. `2026-08-19 16:06 KST` 원본 `귀여운 숲 슬라임 16종 스프라이트 시트.png` Library 보관 (1254² RGBA).
2. [`ce32537`](https://github.com/HappyPaul812/Project-Limitless/commit/ce32537ad3d628d735323eed57333580e512b91b) **2026-08-19 16:22 KST**: 슬라임 아트·애니메이션 디렉터리와 `ASSET_REQUIREMENTS.md` 준비. 지침에 “사용자 수동 수정 PNG를 최우선 원본으로 취급.”
3. [`139fba10`](https://github.com/HappyPaul812/Project-Limitless/commit/139fba10c70292d79145f72524f6867172df4774) **2026-08-19 16:35 KST**: `Unity/Client/Assets/_Project/Art/Monsters/01_GrassSlime/01_GrassSlime_Walk.png` 실제 최초 등록, `.meta`, `GrassSlimeAnimationBuilder.cs`, Animator8 및 MonsterDefinition 연결.
4. [당시 구현문서 `첫_몬스터_초원_슬라임.md`](https://github.com/HappyPaul812/Project-Limitless/blob/139fba10c70292d79145f72524f6867172df4774/문서/08_몬스터/첫_몬스터_초원_슬라임.md)에 **“사용자가 제공한 4×4 방향별 Sprite 시트”**, **“원본 1254×1254 이미지는 그림 크기를 바꾸지 않고 오른쪽과 아래에 각각 2픽셀의 투명 Padding을 추가하여 1256×1256”** 명시. 314×314 각 셀, 행 Down/Left/Right/Up; Idle4 + Walk4(6FPS).
5. Codex Issue #57 2026-11-10 실제 스폰/Animator/현행 PNG 해시 확인. **마지막 미완: 실제 PC 현재 PNG의 1254² 상단 좌측 모든 RGBA 픽셀 = Library 원본, 추가 우/하 2px alpha=0인지 확인**.

### monster_silent_echo / monster_seal_guardian / monster_silent_warden
1. Library **2026-09-28 07:47 KST** B2 디자인 설계도; **09:40 KST** 세 최종 시트 원본 보관.
2. [`a393e5c6`](https://github.com/HappyPaul812/Project-Limitless/commit/a393e5c66be04715811bb757686bdb34e591f224) **2026-09-28 12:24 KST**: 세 최종 PNG 원본을 `Unity/Client/Assets/_Project/Resources/MonsterSheets/`에 등록, `08_SilentEcho.asset`/`09_SealGuardian.asset`/`10_SilentWarden.asset` 및 `.meta`/몬스터 스폰/씬/에디터 `Dungeon01B2SpriteSlicer.cs` 함께 작성.
3. `Dungeon01B2SpriteSlicer.cs`는 **사용자 원본 PNG를 변경하지 않고**, Importer의 128×128 Sprite Rect만 등록하도록 코드 주석·실제 구현에 기록.
4. [`5f122a2`](https://github.com/HappyPaul812/Project-Limitless/commit/5f122a2d1c8b4e659a40ca6de5cbd4340a4d51a1) 당시 CURRENT_STATUS: “최종 PNG 4개를 원본 그대로 등록하고 128×128 Sprite Slice” (세 몬스터+전투 아이콘).
5. Codex Issue #57 실사용 Resource GUID / explicit Idle·Walk·Attack·Hit·Defeat Sprite 연결 / 현재 PNG SHA 확인. **원본 Library PNG 세 장은 활성 Unity 이미지 SHA와 3/3 모두 동일**.

## 3. 분류 업데이트 / 저작권 설명

- 바울 대표 직접 진술: 네 몬스터가 **우리 팀에서 제작한 것**, 과거 문서화가 누락됐을 수 있다는 취지. 세 B2 개별 PNG는 프로젝트 이전 생성·Library 보관→원본 등록 커밋→현재 Unity **3중 바이트 계보 입증**. **B2 세 종 내부 아트 출처 `USER_TEAM_ORIGINAL_CONFIRMED_WITH_SOURCE_SHA`**로 관리 가능.
- Slime은 **`USER_SUPPLIED_ORIGIN_FOUND / GIT_TRANSFORM_DOCUMENTED / LOCAL_PIXEL_CONFIRM_PENDING`**. 최초 원화 파일과 당시 편집 방법, 최종 활성 이미지 위치까지 연결됐지만 PNG 포맷·압축 방식 차이와 2픽셀 padding 때문에 직접 SHA 비교는 부적절. 로컬 pixel diff PASS시 **`USER_TEAM_ORIGINAL_TRANSFORM_CONFIRMED`**로 승격.
- 상기 근거는 **사용자가 제공한 독창적 게임 아트라는 내부 제작 계보** 확인용. 제3자 캐릭터의 모방/권리 침해 없음·독점 라이선스 등의 법적 보증과 구별. 외부 실사용 확정0은 Codex의 **이 감사 범위** 내 결과이며 전체 게임의 NPC/아이콘/VFX까지 포함하지 않음.
- 리뉴얼/재제작 작업0, 사용자 승인 기존 4종 Sprite 및 GUID/meta 절대 변경 금지. 기본 펫 Wolf/Bear/Fox는 기존 3/3 ORIGINAL_CONFIRMED, 다른 공식 몬스터도 종전 상태 보호.

## 4. 재현 가능한 읽기 전용 원본 증거 팩

- 생성 파일명: `LIMITLESS_Ch1_Monster4_Original_Provenance_Evidence_20261110.zip` (이 채팅 샌드박스에서 제작; GitHub 바이너리 Push **없음**).
- ZIP 크기 **2,765,032 bytes**, SHA256 **`773893c79511a398a08bd08a3575fd20cca4bd80fcbdb6f9d66a8f8f17103be4`**, CRC PASS. 원본4 PNG + `verify_ch1_provenance.py` 읽기전용 검증 스크립트 + `README_4MONSTERS.md`, 총6파일.
- 사용자 PC의 ZIP을 Codex가 받아 쓰거나, 원본 파일 4개가 이미 로컬에 있으면 그 SHA를 대조해 복제할 것. 검증 스크립트는 **B2 세 파일 SHA 바이트 동일/슬라임 패딩 RGBA 픽셀 동일**을 실제 로컬에서 확인. `conda activate py3_12` / Pillow.
- 검증 실패 시 원본변경0·신규 아트0·실제 diff 및 SHA만 보고. 기존 Unity Play QA/Save/원본 Git 변경 필요 없음. `main` 직접 commit/Push/merge 금지.
- **2026-11-10 이 ChatGPT 작업은 실제 로컬 Unity 파일 접근 없이 Library·GitHub 정본만 확인함**. `LOCAL_SLIME_PIXEL_QA_NOT_RUN`을 PASS로 표현 금지.

**현재 확정 상태:** `ORIGINAL_LIBRARY_SOURCES_FOUND_4_OF_4 / B2_SOURCE_SHA_EXACT_3_OF_3 / SLIME_ORIGIN_GIT_PAD_TRANSFORM_DOCUMENTED / SLIME_LOCAL_PIXEL_CHECK_PENDING / CONFIRMED_EXTERNAL_ACTIVE_0_WITHIN_AUDIT_SCOPE / REMAKE_REQUIRED_0`.
