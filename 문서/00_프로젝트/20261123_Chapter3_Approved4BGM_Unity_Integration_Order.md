# LIMITLESS / Chapter 3 / 사용자 승인 BGM 4곡 실제 Unity 연결 발주 — 2026-11-23

- **바울 대표 직접 발주:** “bgm도 넣자.” / 기존 대표 청취 승인과 B LoopPlan 승인, Master QA PASS 결과를 이어받아 **Chapter3 F1·서릿길 마을·F2·F3 승인 BGM 4곡(Intro/Loop WAV 8개) 최소 게임 연결 구현 착수 승인**.
- 담당: **Codex** (Audio 구현/실제 Unity 검증). BGM 담당은 WAV 제작/사운드 QA까지 완료했으므로 Gemini/MP3 새 생성·재마스터링/재합성 **금지**.
- 작업 시작 전 `AGENTS.md`, `START_HERE.md`, `COMMON_RULES.md`, `CODEX.md`, `CODEX_LATEST.md`, #31/#127 최신 댓글, #126 **오늘 로컬 결과** 및 실제 `git status`/브랜치/미커밋 변경을 읽는다. 원격 `main` `bfd28154...`는 오래된 시점이고 오늘 보고된 `#126` 제품 커밋 `a2faf0b02c960ddc795cc3054ad4beebbeb49d5f`/보고 `b2580729ac6c57f22195bef83094b256e96dca60`는 **사용자 화면 보고이며 최신 로컬에서 재확인 필수**. 임의 `reset`/덮어쓰기 금지.
- 새 독립 worktree/브랜치 권장 `codex/ch3-approved-bgm4-runtime-20261123`, 실제 최신 안전 베이스에서 작업하고 #126/몬스터 #122 등 다른 로컬 브랜치와 파일 충돌 확인. `main` 직접 변경/병합·원격 Push는 사용자 별도 승인 전 **금지**.
- Unity **6000.6.5f1**만 사용. 기존 Chapter1·2 음악·TTS·SFX·AudioMixer·Save/UserData·Discord·#85 HOLD·기존 Scene/Prefab/GUID/meta 변경 최소화.

## 1. 승인 WAV 8개 — 출처 및 Hash 잠금
| 장면 | Master ZIP | Intro WAV SHA256 | Loop WAV SHA256 |
|---|---|---|---|
| F1 북풍의 들머리 `Where_the_Grass_Bends` | #31 | `Where_the_Grass_Bends_Intro_FINAL.wav` `55443ce14f9d7fd86e53a95d03d04ab98eb2a70ce3f8ed260cba81e0e1713261` | `Where_the_Grass_Bends_Loop_FINAL.wav` `eb49138171a85e95856c2d1d5f96ae21de92f4a5bafe4c5726c46f8f499e69cd` |
| 서릿길 마을 `Hearth_in_the_Low_Valley` | #31 | `Hearth_in_the_Low_Valley_Intro_FINAL.wav` `f6fb974b363bcb881cfbb7264a8a97c87c49fd4d6d298f11bc69b4a7329c2561` | `Hearth_in_the_Low_Valley_Loop_FINAL.wav` `0164f502afda08f17b4a13ec5922e8feadcef7aa5d932300fa097fb45bf747ce` |
| F2 첫눈의 숲 `Beneath_the_Frozen_Boughs` | #127 | `Beneath_the_Frozen_Boughs_Intro_FINAL.wav` `090bf9417981173fb9cfc5fd998786413aa8b06c58c7e5b3157bb4e45b050504` | `Beneath_the_Frozen_Boughs_Loop_FINAL.wav` `8cefeeea2735d28efa796b987254ab6af96be1cbe297062622f029529a55327f` |
| F3 눈 덮인 고갯길 `Steps_on_Frozen_Stone` | #127 | `Steps_on_Frozen_Stone_Intro_FINAL.wav` `9b157be7560f30c5401a7a225431cc791214d1933e4154355e6fca6cb8296036` | `Steps_on_Frozen_Stone_Loop_FINAL.wav` `09609b95da9f907a98079ad659c4ef8b559e9324b410d22e0e9c96df0662c1f8` |

- 납품 ZIP #31 `LIMITLESS_Issue31_FinalLoopMasters_20261123.zip` SHA256 `3c9810962e1a1db11117d5a3fe6ee8ce5caa4fcc62cc6889656aff7ac7d39c94`; #127 `LIMITLESS_Issue127_FinalLoopMasters_20261123.zip` SHA256 `0d0a3a098440edd9f8805cff02891db67530561d63b715bf4d46288d9fe32c2b`. **Windows 실PC F:/Downloads 및 현 프로젝트 실제 경로는 실행 시 확인**. ChatGPT Library/과거 채팅 납품 경로가 PC에 자동 존재한다고 단정 금지.
- #31/#127 기존 ZIP CRC/SHA 전수검증과 **격리 Unity Import 9 PASS/4 PASS**는 기완료이므로 반복하지 않는다. 이번 신규 직접 결합분만 검사.
- 전부 PCM16 44.1kHz Stereo. 원본 승인 볼륨·EQ/압축을 재설계하지 말 것. WAV PCM DecompressOnLoad, preload 등의 임포트는 기존 검증 정본/기존 AudioMixer 정책 우선. 기존 승인 곡 WAV 바이트 수정0.

## 2. 구현 계약 — 기존 음악 보호/Intro→Loop

1. 기존 로컬 Unity 음악 서비스, `Scene`별 BGM Catalog, `AudioSource`와 AudioMixer/TTS/전투 UI 경로 읽기 전용 감사. #31 Codex 선행 보고는 **단일 AudioSource/Clip 구조, 신규 F1/서릿길 배정0** 및 DSP Intro→Loop 예약 기능 미구현. 오늘 현장 코드가 이미 바뀌었다면 현재 최신 실물 우선하고 중복 구현 금지.
2. 게임의 기존 **단일곡 BGM 방식은 유지**. 신규 Intro/Loop 쌍인 경우에만 두 오디오 소스 또는 동등한 **DSP 기반 샘플 정밀 스케줄링** 사용. 장면 진입시 Intro 단 한 번 → Loop 무한반복; 중간에 Scene 변경/전투 시작/게임 일시정지/재진입 시 예정된 다음 소스의 취소·페이드·오디오 리스너 전환을 정리해 음악 중복재생·침묵·스파이크 방지. 실제 기존 BGM 관리자가 Unity 전체 Scene 생명주기 중복 생성하지 않도록.
3. F1·서릿길 마을·F2·F3에 상기 승인 곡만 연결. **실제 Scene 및 등록 ID 확인**. 아직 F3 Scene/실제 활성 맵 없으면 곡을 Catalog에만 안전 등록하고 `SCENE_ASSIGNMENT_HOLD` 보고, 가짜 F3 씬·좌표 생성 금지. 기존 Ch1·2 BGM/다른 장면·보스곡 변경 금지. F4·F5·빙각·설빙의 폭군 BGM 새 창작·무단 배정 금지.
4. 전투 진입/복귀, 대사 TTS 동시 재생, 음량 설정/음소거, 씬 왕복, Save Continue, 플레이어 조작 타이밍에서 **이전 곡 예약 중단**, Intro 매번 재시작할지(기존 SceneEnter 계약) 실제 기존 서비스 정책에 맞춰 정합성 유지. 외부 오디오 장치 및 Unity Editor foreground 포커스 변경 금지(별도 사전 승인 전).
5. **최소 델타 QA**: 4곡 WAV Link/Import/Scene Catalog, 각 Intro→Loop 전환 경계(빠른 소리 변화·무음·중복 없음), Loop 3회 이상, 전투 전환 왕복, 2개 Scene 빠른 전환, TTS/UI/SFX 동시재생, AudioMixer/음량·일시정지/종료, 기존 단일곡 장면 1개 이상의 회귀, Console/컴파일0. 테스트 가능한 Scene에 한해 실제 Unity6000.6.5f1 **격리 Play**. 실재하지 않는 Scene은 HOLD, 기존 QA 재실행 과다 금지.
6. 결과를 `문서/00_프로젝트/Issue31_127_BGM4_Runtime_Integration_20261123.md` + `CODEX_LATEST.md` + Issue #31/#127 댓글로 `브랜치/제품 SHA/보고 SHA/추가·수정 파일/QA PASS·FAIL·PARTIAL/미연결 Scene/원본보호/대표 직접 청취 승인 필요` 보고. Unity 제품에 들어갔다면 **격리 worktree의 구현**임을 표기; 원본 로컬 main·원격에 반영했다고 보고 금지.

## 3. 상업권 및 승인 게이트

- #127 BGM 담당의 상태 `COMMERCIAL_USE_EXPLICIT_LICENSE_PENDING` (상업 사용 불가 판정이 아니라 **유료 게임 배포 라이선스 명시 확인 미완료**). 이번 승인으로 내부 개발·격리 음악 적용 및 QA는 진행 가능. **상용 출시 확정 승인 아님**. 최종 배포 전에 Gemini/Lyria 이용 권리 근거 검토 및 대표 결정을 별도로 요청한다.
- 본 요청 `AUTHORIZED_ISOLATED_UNITY_BGM_INTEGRATION` ≠ `INTEGRATED_MAIN` ≠ `RELEASE_APPROVED`. 

관련 이슈: https://github.com/HappyPaul812/Project-Limitless/issues/31 / https://github.com/HappyPaul812/Project-Limitless/issues/127
