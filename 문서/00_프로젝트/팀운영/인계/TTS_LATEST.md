## EOD 2026-10-24 최신: 승인 통합 QA PASS / PR 진행 / 원본 동기화 HOLD

승인162 WAV와 Catalog를 Main23 KEEP 기능과 결합했습니다. 기존1031 전수 재실행0, 신규 결합 Unity 25 PASS. 원본 BuildSettings 충돌 때문에 원본 적용은 미완료입니다. 아래 승인 대기 기록은 해당 시점 HISTORY로 보존합니다.

# LIMITLESS / TTS 담당 최신 인계 — 2026-10-20

## 2026-10-24 20:22 KST 사용자 Codex 결과 스크린샷 — 162 음성 격리 worktree 적용 및 검증 보고 / main 미반영

- **보고 근거:** 사용자 제공 Codex 화면. 별도 worktree `codex/tts-voice-integration-20261024`에서 적용 완료, **원본 작업 폴더 변경0** 보고. 세린 Erinome67 + 태온 Algieba85 + 하렌 Alnilam3 + 기존 KEEP 폴3/미엘2/레온2 = **162/162**.
- 기존 StoryVoiceCatalog **140 클립 교체 + 신규22 ID 등록**, 총 **290→312**. 최종 하렌 01 **R2.1 청취 승인 SHA 일치**. 원래 140 WAV GUID/.meta 및 비대상 150 연결 보호, ID 중복0, Missing Clip0, GUID 충돌(음성)0 보고.
- Unity **6000.5.7f1** (현재 원격 `main`의 `Unity/Client/ProjectSettings/ProjectVersion.txt` 실제 버전과 일치), Compile 오류0, **Play Mode 1,031 PASS**, 재생시작 **162/162** 확인. 핵심 대사 배열·Next/Skip·무음·음량·격리 Save/Load PASS 보고.
- 변경물 **189 파일**, 보호한 원본 WAV·Save·기존 작업 **1,430개 파일 SHA 동일** 보고. branch 구현 커밋 짧은 SHA `1cb99be2`, 최종 기록 `73a1b212b15cd749145a3eccb5075b9f32458db8` **사용자 화면에서 전사**, 자체 원격 확인된 commit SHA는 아님.
- **잔여 QA:** Editor Search 기존 예외1, 비음성 GUID 중복 기존사유 별도, 전용 독립 창/전체 퀘스트 미실행. 음성 통합 관련 새 오류로 단정하지 말되 실제 QA 보고서에서 사전 존재 증거와 영향 범위 분리. 단일 PlayMode 수치로 모든 장면 완전 청취검수 완료라고 주장 금지.
- **원격 현황 별도 확인:** `codex/tts-voice-integration-20261024` remote branch 검색 결과 없음, 해당 최종 SHA remote commit 조회 결과 없음, 동일 작업 PR 검색 결과 없음(2026-10-24 확인시점). 작업 내용이 **로컬에만 있는 것으로 추정**하며 push/PR/merge 완료 주장 금지.
- **사용자 추가 승인 전 금지:** `main` 직접 commit·merge·Push 및 사용자 원본 로컬 작업 폴더 동기화. 후속 Codex는 격리 worktree 산출물의 git status, 189 변경 파일/140+22 mapping diff/보고서/SHA/선행 오류를 **읽기 전용 교차검증** 후 PR 준비 보고. push/PR/merge/원본 sync는 별도 사용자 승인 때만. TTS API/합성 새 호출0.
- 참고: [TTS 162개 Codex 인계](../../TTS_20261024_162WAV_Unity_일괄통합_Codex_인계.md), [GitHub Issue #20](https://github.com/HappyPaul812/Project-Limitless/issues/20).


## 2026-10-24 저녁 7시 Codex용 전체 음성 통합 준비 — 합계162, 하렌3 청취 PASS 사용자 보고

- 사용자 직접 보고: 하렌 R2.1 첫 대사의 평서문 하강 청취 합격 후 `APPROVE_FALLING_R2` 승인 처리, 이어 남은 02/03 제작. 최신 메시지 “좋아 다 잘됐어. 이따 7시쯤에 다 넣게 코덱스 지시문 줘”. **하렌 `main21_haren_haren_01`의 R2.1 승인본 + 02/03 세 대사 청취 PASS는 사용자 보고**. 실제 3/3 SHA·WAV·JSON 검증은 Codex 작업 전 로컬 감사 필요. 과거 의문형 오류 WAV/R1/R2 미승인 후보는 절대 게임에 적용 금지.
- **Unity 통합 합계162 대상:** 세린 `Erinome` 67 + 태온 `Algieba` 85 = 제작 READY152 / 하렌 `Alnilam` 3(신규 승인 보고) / 사용자 KEEP 기존 Main21 폴3·미엘2·레온2=7(재생성 금지). **162=매핑 합집합**, 전부 새 Catalog ID가 아니다. 이전 main 사전 감사에서는 세린58+태온82=140개 기등록 음성 재캐스팅 및 Main20·21 신규22개 예정, 실제 local 최신 런타임 재검증 필수.
- **[2026-10-24 전체 162 WAV Unity 통합용 Codex 작업 인계 정본](../../TTS_20261024_162WAV_Unity_일괄통합_Codex_인계.md)** 준비. Codex는 저녁7시경 사용자가 프롬프트를 **직접 전달한 때**만 작업 착수. 승인 범위는 Unity 기능 브랜치 내 복사·연결·검증 및 문서 보고. **main 병합·원본 로컬 동기화·remote push 자동 승인 아님**.
- 실파일 경로: `F:\Downloads\Renewal_Voices\Serin` / `Taeon`; `F:\Downloads\Voice\Haren_Main21\Haren`(하렌01 정식 R2.1 승인본 SHA 확인), `F:\Downloads\Voice\Limitless_TTS_Main20_Main21_20261019\candidates` 기존 KEEP7. user-provided full path와 Github manifest의 ID, 원문, Voice 사전 감사.
- Codex는 **TTS 합성/Google API 호출 금지**, 기존 WAV 복제·재합성 금지. 로컬 파일 hash/audioclip/GUID/메타·Story Script/CharacterId/Unity Console/편집 범위 먼저 검증. Play Mode는 사용자 전면 화면을 빼앗지 않는 방법만 시행. Scene/Save/Quest/UI·타 챕터 무단 변경 금지.
- 완료 정의: 162개 정확 매핑(기존140 교체·신규22 후보는 실제 등록 상태에 따라 유동), 불합격 WAV 제외, Player/지문 무음, 타 캐릭터/타 음성 보호, git diff + Unity 컴파일/가능하면 Play Mode; 제품 변경은 `codex/tts-voice-integration-20261024` 브랜치에 한정. Codex 결과·커밋 SHA/PR 준비·사용자 별도 최종 승인 상태 보고 후 main 통합 검토.
- 본 안내는 **문서 작성만**: API0 / 사용자 F: 작업0 / Unity0 / 제품 commit0 / main0.


## 2026-10-24 하렌 R2.1 긴급 수정 — 원본 WAV 없음 / JSON 메타+성공 장부 있음

- **사용자 새 콘솔 증거:** Haren `main21_haren_haren_01` 1개는 이전 실행에서 `[HAREN CREATED]`까지 성공했다고 표시됐고 PT 2026-10-24 **9/40**, 세린·태온152/152, `HOLD=False`, 사용자 보존 Main21 old7 **7/7**. 그러나 `Haren_Prosody_Retake_R2.py check`에서 **`Haren first WAV exists=False meta=True`**, 후보 R2는 WAV/meta 모두 미생성. `retake` 재시도 둘 다 `[STOP] Previous Haren first WAV/meta missing`으로 API 추가호출0. 원본 WAV 누락 원인은 미확인(삭제·이동 단정 금지).
- **신규 [하렌 R2.1 기록 및 안내 정본](../../TTS_Main21_Haren_R2_1_기존WAV실종_메타장부검증_20261024.md)**. 파일 ZIP `LIMITLESS_Haren_Prosody_Retake_R2_1_OrphanMetaFix_20261024.zip` 7737B SHA256 `679b825293f19a6524bfae62dbcc39f9d98bca777e72ccc7d16f2a36826c345d`. 새 파일 `Haren_Prosody_Retake_R2_1.py`를 사용자 직접 `F:\Downloads\Voice`에 압축 해제(기존 R2/하렌 프로그램 덮어쓰기 금지).
- R2.1은 `main21_haren_haren_01.json` 원본 보존 + 같은 WAV SHA가 기록된 **공유 `api_events.jsonl`의 원 하렌 SUCCESS 기록 교차 확인** 후만 원본 WAV 부재 상태에서 R2 1회 시험을 허용한다. 메타/이벤트 불일치면 API POST0 차단. 기존 7개 보호·공유 PT/프로젝트 상한/HOLD·LOCK 유지.
- **사용자 직접 명령:** `cd F:\Downloads\Voice` → `python .\Haren_Prosody_Retake_R2_1.py check` → `python .\Haren_Prosody_Retake_R2_1.py retake`, `RUN_RETAKE_01` 승인 후 **R2 신규1 WAV 청취**. 질문형이면 `reject-r2`만 하고 추가 생성 STOP. 평서문 하강 PASS면 `approve` `APPROVE_FALLING_R2` 승인 후에만 `remaining` 하렌02/03 직접 제작. 원본 WAV 없으면 메타만 백업했다는 사실을 기록하고 승인 전 공식 WAV 경로 수정0.
- 새 Python은 오프라인 byte/SHA·mock 1 POST/approve/remaining, 누락 메타·SHA mismatch 안전차단, ZIP/구문 검사 PASS. **실제 Google 요청0, 사용자 F: 수정0, Unity0**. 다른 캐릭터·하렌02/03 최종 청취/Unity 승인 여전히 미완.

## 2026-10-24 하렌 첫 대사 재차 청취 FAIL — 의문형 상승 억양 / 분리 R2 시험 지시

- 사용자 최신 피드백 **“또 의문문이야.”** 대상 `main21_haren_haren_01` “태온. 너 아직 멀었어.” 기존 하렌 Main21 재생성 음성 **질문형 말끝 상승으로 청취 FAIL**. 하렌3 전체 준비 완료로 보고하면 안 됨; 02·03 생성 STOP. 세린67·태온85는 WAV READY=152/152 유지, 별도 기존 레온2/미엘2/폴3=7 KEEP.
- **새 [하렌 R2 교정 정본](../../TTS_Main21_하렌01_의문문재발_교정_R2_20261024.md)** 작성. 별도 패키지 `LIMITLESS_Haren_Prosody_Retake_R2_20261024.zip` 6160bytes SHA256 `f66a1e7ea3fbc44de689a91dceb083fb738486cf7e02a562b0c423e8693286f6`. 사용자 직접 `F:\Downloads\Voice`에 압축 풀어 `Haren_Prosody_Retake_R2.py check` → `retake`에서 `RUN_RETAKE_01` 승인 시만 **새 샘플 최대1 Google POST**. 기존 불합격 공식 WAV/meta는 그대로 두고 신규 R2를 `F:\Downloads\Voice\Haren_Main21\ProsodyRetakes\R2\Haren\main21_haren_haren_01.wav`에 생성.
- **근거/추정 구별:** 기존 스크립트는 공식 Gemini3.8 generateContent REST `speech_metadata.style` 형식이 올바르며, 지시가 무시되었다고 확정 불가. 긴 영어 지시보다 간결한 한국어 **말끝 하강 평서문** 지시를 R2 샘플1에 사용. 모델 운율을 실제로 바로잡을지는 사용자 청취 전 미검증 (공식 https://ai.google.dev/gemini-api/docs/generate-content/speech-generation).
- R2 듣고 실패면 `reject-r2` (추가 생성0/기존 WAV 보존). **실제 청취 합격시에만** `approve` 입력 + `APPROVE_FALLING_R2`으로 원 불합격 WAV SHA 보관/이관, R2 교체, QA 마커 검증. 그 후만 **`remaining`** 실행으로 나머지 하렌2 생성. 구 `Haren_Main21_AfterRenewal_3.py run` 직접 호출하면 승인 여부를 모른 채 2·3을 만들 수 있어 금지.
- 실제 PT 남은 사용량/Google quota/프로젝트 독립성은 사용자 확인. 공유 장부와 HOLD/LOCK을 유지하고 API값 노출 금지. **사용자 직접 합성, Codex 합성 금지**, 후일 게임 통합은 별도 승인. ChatGPT 이번 작업 실제 API0/사용자 F: 수정0/Unity0.

## 2026-10-24 사용자 실제 완료 로그 — 세린·태온 전량 152/152 WAV READY

- **최신 직접 실행 결과:** `[STATUS] PT=2026-10-24 (UTC-7) / attempted=8/40 / remaining=32`; `[KEYS] KEY1=2 KEY2=2 KEY3=2 KEY4=2`, `PROJECT_1~4` 각2(로컬 그룹 라벨). `[READY] Serin=67/67 Taeon=85/85 total=152/152` → **세린/태온 합성 WAV 전량 준비 완료**. 이전 144/152에서 오늘 태온8개가 추가됨.
- `HOLD=False`, `[429 QUARANTINE] []`; 사용량 로그/각 캐릭터 dialogues.csv 유지. 일일 안전 예산은 **총40 중 8회 사용, 32회 로컬 잔여**. 실제 Google 프로젝트별 Free Tier/Billing/원격 당일 사용 가능량이 32회라는 뜻은 아니다.
- **다음 단계(사용자가 직접 제작):** GitHub `docs/tts/Tools/TTS/Haren_Main21_AfterRenewal_3.py`를 본인이 `F:\Downloads\Voice\Haren_Main21_AfterRenewal_3.py`로 저장했는지 확인 → `cd F:\Downloads\Voice` → `python .\Haren_Main21_AfterRenewal_3.py check`(Google POST0/152 완료 및 같은 키·공유일일장부 검사) → `python .\Haren_Main21_AfterRenewal_3.py run --max-calls 1`(사용자 `RUN_HAREN3` 승인) → 하렌 `main21_haren_haren_01` “태온. 너 아직 멀었어.” 하강 평서문 직접 청취 QA → 합격 후 `python .\Haren_Main21_AfterRenewal_3.py run` 나머지2개. 키값 공유 금지. 실제 Google quota·과금상태 확인 및 단일 프로세스 실행.
- 하렌3은 **아직 합성 완료라고 보고받지 않음**. 사용자가 파일을 저장했다거나 API를 실제 호출했다고 가정하지 않음. 기존 Main21 폴3/미엘2/레온2 **KEEP 7 WAV 무재제작**. 제작 완료 ≠ 인간 청취 합격, 전체 음성 통합은 사용자 별도 승인 전 Unity 적용 금지.
- Codex는 TTS 생성/키 사용을 담당하지 않음. 마지막 일괄 Unity 통합/읽기 전용 QA만 추후 사용자 승인을 받아 수행.
- 관련 [10/24 하렌 준비·공유 장부 Runbook](../../TTS_20261024_Post152_Haren3_VoiceFolder_SharedQuota_Runbook.md), [Main21 기존7 보존 목록](../../TTS_Main21_기존보존_7WAV_통합대기_20261022.md). 이번 GitHub 갱신은 `docs/tts` 문서만 수정; 실제 사용자의 WAV/API/Unity 변경0.


## 2026-10-24 사용자 최신 담당 정정 — 음성 생성 실행은 사용자 직접 수행

- 사용자 명시: **“뽑는겅 코덱스가 안하고 내가 할꺼야”**. **TTS API 호출·자동생성 스크립트 실행·하렌 1번 음성 직접 청취·승인 뒤 나머지 2개 실행은 전부 사용자 직접 담당**. Codex가 TTS를 생성하거나 API 키를 사용하도록 지시하지 않는다.
- TTS 담당 ChatGPT 역할: 준비된 실행기와 명령 안내, GitHub/정본 대사 감사, 실행 결과 판독, 보존·문서화. 사용자 Windows `F:` 실제 폴더는 이 세션에서 직접 수정할 수 없으므로 설치/다운로드 시 필요한 단계를 정확하게 안내.
- **Codex 역할은 나중에 사용자가 별도 승인할 경우에만** 로컬 TTS·Story Voice WAV 파일 읽기 전용 전수검수, Unity 연결·테스트·브랜치 통합. 현재는 Unity 변경도 Codex에게 TTS 제작 지시도 금지.
- 사용자 실행 순서: PT 2026-10-24 자정(서울 16시) 이후 `F:\Downloads\Renewal_Voices`에서 `Renewal_Auto_TTS_Failover_v2.py check` → `run` → 152/152 / `HOLD=False` 확인. 그 다음 `F:\Downloads\Voice`에 GitHub `Tools/TTS/Haren_Main21_AfterRenewal_3.py` 소스 파일을 사용자가 직접 내려받아 저장하고, `python .\Haren_Main21_AfterRenewal_3.py check` → `python .\Haren_Main21_AfterRenewal_3.py run --max-calls 1` → 1번 대사를 직접 청취 검수, 최종 청취 승인 후 `python .\Haren_Main21_AfterRenewal_3.py run`으로 나머지2개 생성. 4키 같은 프로젝트·과금·실제 무료quota 조건 확인, 같은 날 전역 장부 공유. 첫 음성 말끝 하강은 사용자 청취가 기준.
- 사용자 KEEP한 폴/미엘/레온 Main21 7 WAV 재생성 금지, 세린/태온 WAV 보존, API값 GitHub/공개 출력 금지, 나중에 사용자 별도 승인 전 Unity 적용 금지.


## 2026-10-24 우선 인계 — 태온 마지막8 이후 하렌3 추가 제작 / F:\Downloads\Voice 작업 준비

- 사용자 직접 요청: “이따 8개 뽑으면 또 뽑을꺼 있니? 있으면 작업 폴더 `F:\Downloads\Voice`로 알아서 해줘. api키도 거기 있어.” **확정 추가 합성 하렌 3개**. 다른 인물/Chapter3 미확정 대사 임의 생성 금지.
- 현재 사용자 마지막 실사용 로그는 **2026-10-23 PT 40/40**, 세린67/67·태온77/85·합계144/152·HOLD=False. 10/24 PT 일자 시작 후 **태온 잔여8을 기존 v2로 먼저 완료**하고 실제 152/152 WAV+메타/SHA 검사되기 전에는 신규 하렌3 API 사용0.
- Main20 VoiceExpected **5(세린5)**, Main21 VoiceExpected **17** = 152 리캐스팅 매니페스트 커버 **7**, 사용자 KEEP 폴3+미엘2+레온2 기존WAV **7**, 신규 하렌 **3**. Main20·21 정본 합집합에서 빠진 ID 0. 하지만 Unity의 전 챕터 추가 음성 결함까지 0이라고 선언하지 말 것.
- **신규 실행기 GitHub:** [`Tools/TTS/Haren_Main21_AfterRenewal_3.py`](https://github.com/HappyPaul812/Project-Limitless/blob/docs/tts/Tools/TTS/Haren_Main21_AfterRenewal_3.py), 사용자 파일 ZIP `LIMITLESS_TTS_Haren3_SharedQuota_AfterTaeon_20261024.zip`(8684B SHA256 `8a44b807cbac8a306de2e9d8b6d4b9a1dd86607e9f734a0607e373c4be001661`). 사용자 PC `F:\Downloads\Voice\Haren_Main21_AfterRenewal_3.py`로 CodeX 배치 예정, 현 대화 도구로 사용자 F: 쓰기 불가이므로 아직 **PC 설치/실제 API 호출0**.
- **정식 실행/감사 문서:** [`TTS_20261024_Post152_Haren3_VoiceFolder_SharedQuota_Runbook.md`](../../TTS_20261024_Post152_Haren3_VoiceFolder_SharedQuota_Runbook.md). 정확 하렌 stable 3 ID/원문은 `태온. 너 아직 멀었어.` (평서문 하강) / `다친 데는 없고?` (질문) / `그럼 됐어.` (평서문 하강). 성우 Alnilam. 기존 하렌 첫 오디션은 말끝 상승 오류로 재사용 금지.
- 핵심 안전: `F:\Downloads\Voice\API.txt`의 4키가 `Renewal_Voices\API.txt`와 **완전히 같은 실제 4키**인지 비밀값 출력 없이 확인(순서만 다르면 기존 슬롯에 맞춤; 키가 다르면 차단). 기존 `Renewal_Voices\RENEWAL_SETTINGS.json`/ `_progress\api_events.jsonl` 및 HOLD/LOCK를 공유하여 마지막 태온8과 하렌3을 **같은 PT날짜 전체40회 및 프로젝트별10회 로컬 안전 예산** 안에서 계수. Google 실제 quota, 프로젝트 독립성과 과금상태 확인 필수. 서로 다른 제작기 동시 실행 금지.
- 오프라인 **4 unittest PASS**(152 완료 게이트, 프로젝트 키 일치, no API check, fake 생성3과 동일PT 8+3 장부·중복호출 방지). **실제 Google POST0, 사용자 F: 변경0, Unity0**. 첫 대사는 `run --max-calls 1` 생성 후 **사용자 청취 QA PASS를 먼저 받은 다음** 남은 2개 제작. 사용자가 새 WAV를 최종 승인하고 통합을 명시 승인하기 전에는 Unity 반영 금지.
- 원본 보존: 사용자 KEEP Main21 레온2/미엘2/폴3 = **7 WAV 재제작 금지**, 세린67/태온 완료본 재생성 금지. 필요 시 Codex 로컬 읽기 전용 조사 후 새로운 확정 누락이 있으면 사전 보고/추가승인.


## 2026-10-23 사용자 실제 로그 — 세린67/67, 태온77/85 / PT 40/40 정상 안전 종료

- 사용자 콘솔 원문: `[DAILY CAP] 2026-10-23 No more calls this PT day.`; `[STATUS] PT=2026-10-23 (UTC-7) / attempted=40/40 / remaining=0`.
- 오늘 슬롯 사용량: `KEY1=10, KEY2=10, KEY3=10, KEY4=10`. 그룹 `PROJECT_1,2,3,4` 모두 로컬10회. 실제 Google 프로젝트 ID 독립성·서버 잔여량을 사용량 로그만으로 확정 불가; 이 값은 *로컬 안전 운영계수*.
- `[READY] Serin=67/67 Taeon=77/85 total=144/152` → 전날104/152 대비 WAV 준비 +40, **남은 태온8개**, 전체 진행률 약 **94.7%**. 세린67개는 추가 생성 대상 없음. READY는 생성/기술파일 준비일 뿐 사용자 전체 청취 QA PASS는 별개.
- `HOLD=False`, `[429 QUARANTINE] []`: API 키 그룹 격리도 잠금도 없이 **하루 로컬 40회 안전예산 정상 소진**. 이미 만든 WAV/Meta/로그/원문 수정·중복 재제작 없음. 복구 프로그램 사용 불필요.
- **다음 PT 날짜:** 2026-10-24 00:00 PDT = 2026-10-24 16:00 KST 이후 서버의 실제 프로젝트·모델별 무료 quota와 과금/키 소속 조건을 확인하고 `cd F:\Downloads\Renewal_Voices` → `python .\Renewal_Auto_TTS_Failover_v2.py check` → `python .\Renewal_Auto_TTS_Failover_v2.py run`. 태온 잔여8개부터 이어 제작. 실제 호출/결과에 따라 8회보다 더 필요하거나 429 가능; 8회 성공 가정 시 152/152.
- **보존해야 할 별도 Main21 7개:** 사용자 보존 지정 `F:\Downloads\Voice\Limitless_TTS_Main20_Main21_20261019\candidates` 내 레온2·미엘2·폴3, [7개 WAV KEEP 정본](../../TTS_Main21_기존보존_7WAV_통합대기_20261022.md) 참고. 신규 합성 금지. 세린·태온·하렌 등 모두 실제 청취 QA 및 사용자 승인 후 Codex가 한번에 Unity 통합. 하렌 R1은 여전히 제작 보류 상태.
- 이번 보고는 사용자 로그/경로만 문서화: 사용자 로컬 `F:` 직접 검사0, Google POST0, WAV 수정0, Unity/main 변경0.


## 2026-10-22 최신 사용자 결정 — Main21 기존 정상 후보 WAV 7개 KEEP / 재생성 금지 / 후일 Unity 일괄 통합

- 사용자: `F:\Downloads\Voice\Limitless_TTS_Main20_Main21_20261019\candidates`에서 **오류 후보들은 직접 정리**했고, 남은 WAV **7개만 보존**한다고 보고. **다시 TTS API로 생성하지 말 것**, 추후 다른 완성 성우 WAV와 **한 번에 Unity에 추가**. 지금 게임 반영 금지.
- **[7개 WAV 보존/후일 통합 지시 정본](../../TTS_Main21_기존보존_7WAV_통합대기_20261022.md)**. 사용자 제공 7 ID 모두 실제 GitHub `main` `Main21_TTS_Manifest_20261010.json`에서 정확 대사/화자/VoiceExpected=true 대조 완료:
  - **레온 Orus 2개:** `main21_carriage_leon_01.wav`, `main21_arbel_leon_01.wav`
  - **미엘 Sulafat 2개:** `main21_aid_miel_02.wav`, `main21_aid_miel_01.wav`
  - **폴 Achird 3개:** `main21_aid_paul_01.wav`, `main21_route_paul_02.wav`, `main21_route_paul_01.wav`
- **상태 분리:** 해당 7개는 사용자 `KEEP` 지정, `TTS_NOT_REQUIRED`, `UNITY_PENDING`. API 다시 사용 금지. 실제 로컬 WAV 바이트/SHA/발화/PCM/사용자 청취 PASS는 ChatGPT에서 F: 직접 확인하지 못했으므로 **최종 통합 전 Codex가 검증**. 기존 WAV 삭제·이동·덮어쓰기 금지. TTS 자동 생성기에서도 대상 제외.
- 현재 다른 제작현황(최근 사용자 로그): 세린 **67/67 READY**, 태온 **37/85 READY**(48 미제작), 하렌3은 별도 제작 대기/원래 `main21_haren_haren_01`의 평서문 하강 억양 주의. 이는 별개 제작 영역. **모든 음성이 준비되고 사용자 일괄 통합 승인 시에만** Codex가 Main20·21 전수 목록/기존 7 SHA 감사 후 하나의 구현 배치로 Unity 연결·검증. `main` 직접 커밋 금지.
- 이번 턴 문서 기록만: Google API0/사용자 로컬 F: 변경0/Unity0. 7개 외에 과거 잘못된 후보가 삭제됐다는 내용은 **사용자 보고이지 직접 파일시스템 감사 결과가 아님**.


## 2026-10-22 실제 사용자 콘솔 최종 상태 — 세린67/67 완료, 태온37/85, PT 40/40 안전 종료

**최신 실행 증거 우선:** 2026-10-22 사용자 `Renewal_Auto_TTS_Failover_v2.py` 최종 출력 `[DAILY CAP] 2026-10-22 No more calls this PT day.`
- `[STATUS] PT=2026-10-22 (UTC-7) / attempted=40/40 / remaining=0`.
- `[KEYS] KEY1=10, KEY2=10, KEY3=10, KEY4=10` / 로컬 그룹 `PROJECT_1~4` 각각10. 이는 **로컬 사용량**이지 Google의 실시간 무료 quota 또는 프로젝트 소속 증거가 아님.
- `[READY] Serin=67/67, Taeon=37/85, total=104/152` = 전체 **68.4%**. 전날 64/152에서 파일 준비 수 **+40**. 남은 제작 **태온48개**이며 세린은 추가 TTS 생성 대상0. READY는 저장된 PCM/메타 기준이며 **전량 사용자 청취 승인 아님**.
- `HOLD=False`, `[429 QUARANTINE] []`: 에러 잠금이나 격리 없이 **정상 일일 안전상한 정지**. `RUNNING.lock` 복구나 `RESUME429`/`SWITCH429` 수동 복구 **불필요**. 기존 104 WAV/원문·SHA·사용량 원장을 보존한다.
- 작업 예산은 *오늘 로컬40회 전부 사용*. Google 서버의 실제 프로젝트별 quota와 과금상태는 별도 확인 필요. 하렌3은 여전히 이 자동화 제외, 제작 보류.
- **다음 실행 계획:** 2026-10-23 **00:00 PDT = 한국 시각 16:00** 이후 제공자 일일 리셋 및 실제 프로젝트/모델 무료 사용가능량 확인 → `cd F:\Downloads\Renewal_Voices` → `python .\Renewal_Auto_TTS_Failover_v2.py check` → `python .\Renewal_Auto_TTS_Failover_v2.py run`. 세린 완료본 skip 후 태온 다음 미제작부터 이어감. 예상대로 하루40이 허용된다면 10/23 +40, 10/24 +8회로 끝나지만 **Google 실제 성공/제작량은 보장 불가**.
- 완료 뒤 104개를 즉시 Unity에 넣지 않는다. 태온 나머지 전량 제조 완료 + 세린/태온 전 챕터 발음·인물성·감정/음색 청취 QA + 사용자 승인 후에만 Codex Unity 무손실 통합. `docs/tts` 문서만 기록, main 직접 수정/Push/merge 금지.


## 2026-10-21 공통 로직 정본 — 앞으로 다른 모든 TTS에 재사용

- 사용자: “고맙고 이 로직은 문서화해놔, 다른 TTS에도 활용하게..”. 대응해 **[TTS 공통 다중 프로젝트 Quota·429 Failover·체크포인트 운영 표준](../../TTS_공통_다중프로젝트_할당량_429자동전환_체크포인트_운영표준_20261021.md)** 신규 작성. 새 캐릭터·새 챕터·다른 Gemini TTS 작업 시작 전 필독.
- 공통 핵심: 정확 대사 Stable ID/원문·화자·Voice/Style 동결 → 사용자 한국어 샘플 청취 승인 → 키→실제 Project ID와 Project/Model quota·Billing 확인 → POST 전에 ATTEMPT 선기록 → 성공 WAV/JSON SHA·원문/음색 QA → 429 프로젝트 단위 태평양 일자 격리 및 **검증된 다른 독립 Project에만** 자동 전환 → CAP 정상중단(잠금 없음) / 결과불명 HOLD+LOCK 및 증거 백업·수동 복구 → PT 다음날 당일 카운트만 새 계산하고 구 데이터·WAV 보존 → 사용자 음성 QA 후 Codex Unity 적용.
- 실사용 검증 10/21: KEY3 HTTP429에서 KEY4로 전환해 세린 Main20 성공, **39/40 정상 안전 종료/HOLD=False, READY 세린63/67·태온1/85**. 40/10은 당일 배치의 로컬 안전 수치, 외부 Google 실제 quota 보장·다른 TTS 기본값이 아님.
- **중요 코드 경계:** 현재 `Renewal_Auto_TTS_Failover_v2.py`는 세린/태온 152개 전용이고, 문서는 다른 담당이 따라야 할 **범용 운영 규약/개념 설계**이지 이미 재사용 가능한 공용 실행기가 테스트 완료됐다는 주장이 아님. 이후 실제 다른 캐릭터에 적용할 때 manifest·폴더·Voice 선택기·공유 프로젝트 장부 분리/재통합 구현·오프라인 검증 필요. 하렌 억양 수정 보류는 변함없음.
- 본 문서 작업으로 Google 생성 POST0, 로컬 사용자 F 드라이브·WAV/Unity 변경0, 브랜치 `docs/tts` 문서만 수정. 후임 담당은 기획·TTS·Codex 역할을 준수.

## 2026-10-21 최종 사용자 화면 — 429 프로젝트 격리 후 일일 안전 상한 정상 종료

- **사용자 실제 출력:** `[GROUP/KEY CAP] All usable project groups at configured per-day safety caps.`; `PT=2026-10-21 UTC-7 attempted=39/40 remaining=1`; `HOLD=False`; `429 QUARANTINE=['PROJECT_3']`.
- **누적 준비 파일:** 세린 **63/67**, 태온 **1/85**, 총 **64/152**. 따라서 아직 미제작은 **세린4 + 태온84 = 88개**. READY는 저장·기술 체크된 WAV 수이며 사용자 최종 청취로 64개 모두 합격했다는 뜻은 아님.
- **키별 오늘 로컬 요청:** KEY1=10 / KEY2=10 / KEY3=9(앞선 HTTP429로 프로젝트 그룹3 격리) / KEY4=10. 남은 로컬 전체예산1회는 전환 가능한 안전 프로젝트 슬롯 부재로 **사용하지 않았음**. 이는 **새 실패가 아닌 정상 안전상한 종료**, HOLD 및 RUNNING.lock 신규 발생 없음. 사용자가 보여준 화면을 정본으로 하되 외부 Google 사용량은 미포함.
- 기존 문서의 **KEY3 429 당시 HOLD 상태는 최신 상태가 아님**. v2 failover 및 수동 switch의 복구·격리 이후 실제 API 성공이 있었고 오늘 사용가능 프로젝트 전부 로컬 cap 도달.
- **다음 조치:** 2026-10-22 미국 태평양 자정 이후(한국 시각 10월22일 오후4시 PDT 기준) 실제 Google 프로젝트/모델 무료 quota와 과금 상태 확인. 기존 `HOLD=False`라 **RESUME429나 SWITCH429 재실행 불필요**. `python .\Renewal_Auto_TTS_Failover_v2.py check` → `python .\Renewal_Auto_TTS_Failover_v2.py run`으로 세린 마지막4개부터 이어감. 로그·기존 WAV·샘플·API.txt·Unity 그대로 보존. 오늘 추가 Google 시도할 필요 없음.
- 프로젝트별 할당량은 여전히 **키 수와 다르며 실제 Google 프로젝트 ID 검증 중요**. PT 날짜 변경으로 로컬 카운터·그날의 격리는 재계산되지만, 원격 Google 실제 quota 허용을 보장하지 않음.


## 2026-10-21 최신 — KEY3 HTTP429 후 타 독립 프로젝트 자동 전환 v2 제작 / 사용 전 프로젝트 실소속 확인

- 사용자: “저기 한 API가 막히면 다른 API가 하게 못해?” **가능하나 독립 Google 프로젝트로 실제 확인된 경우에만**. 기존처럼 같은 프로젝트 내 KEY를 바꿔 쿼터를 우회하는 방식은 허용하지 않음. Google은 RPD/RPM/TPM을 API Key가 아니라 **Google Project** 단위로 산정 (https://ai.google.dev/gemini-api/docs/rate-limits).
- 사용자 마지막 증거 (10/21): **세린59/67·태온1/85**, READY60/152. PT 10/21 요청 **35/40**, 성공34/실패1, KEY1=9/KEY2=9/KEY3=9/KEY4=8. `main20_confront_serin_02`에서 **KEY3 / HTTP429 / HOLD+LOCK**. 원인 RPD인지 RPM/TPM인지는 미확정. `PROJECT_1...4`는 표시 라벨일 뿐 실제 4개 다른 프로젝트라는 공식 검증 아님.
- 직접 만든 **`LIMITLESS_Renewal_Voices_HTTP429_ProjectFailover_v2_20261021.zip`** 22109bytes SHA256 `cd0a063d5c6d2b7463ab37363fbe2c9c9de45ab9ae257899f55347d231b7d643` 제공. 기존 `Renewal_Auto_TTS.py`와 `API.txt`, WAV/설정/로그 덮어쓰기 **없음**. 신규 분리된 `Renewal_Auto_TTS_Failover_v2.py`, `Configure_Failover_v2.py`, `Renewal_429_SameDay_Switch_v2.py`, README, offline tests 2개.
- 사용자가 https://aistudio.google.com/api-keys 에서 실제 Key→Project ID/Model 무료 잔량/과금 확인 후 그룹을 실소속대로 표시, `python .\Configure_Failover_v2.py`에서 `VERIFIED_PROJECTS` 승인하면 원 `RENEWAL_SETTINGS.json` SHA 백업 및 두 설정(`auto_failover_on_429:true`와 `verified_independent_projects_for_failover:true`) 활성화. 실제 공식 프로젝트 확인이 없으면 **동작 활성화 금지**.
- 기존 **같은날** KEY3 429 HOLD/LOCK은 `python .\Renewal_429_SameDay_Switch_v2.py inspect`(API0) 후 조건 PASS면 `... switch` `SWITCH429` 사용자 직접 승인이 있어야 SHA 백업·프로젝트3 PT 당일 격리 이벤트 append·잠금 해제. 사용자 승인 없는 자동 해제·잠금 삭제 불가.
- 이후 **새 runner만** `python .\Renewal_Auto_TTS_Failover_v2.py check` → `... run --max-calls 1` (1건 실제 청취/사용량 검수) → `... run`. 429 시 해당 그룹 격리, 다른 독립 그룹 남은 로컬 예산으로 같은 미완료 DialogueID 시도. 이전 실패는 ATTEMPT1·FAIL, 그룹 PROJECT_QUARANTINE 기록, 성공 WAV/원본 ID·SHA 유지. 다른 프로젝트 없으면 기존 HOLD. HTTP400/500/타임아웃도 기존 HOLD+LOCK. 40회는 로컬 한도이지 Google 쿼터 보장이 아님.
- v2 원본 코드 사용한 **오프라인 테스트 총9개 PASS**, 모의 Google 429+다른 그룹 성공, same-day 엄격한 증거·잠금 보관·승인/계수, 다른 그룹 없거나 미활성 기존 HOLD, 152 원문 검사, WAV 24k/mono/PCM16. 실제 Gemini API POST0, 사용자 F 드라이브 조작0, Unity0.
- [정식 신기능 Runbook](../../TTS_RenewalVoices_HTTP429_ProjectAwareFailover_v2_20261021.md) / [Issue #20](https://github.com/HappyPaul812/Project-Limitless/issues/20). 하렌은 계속 본작업 제외. 완료 보고에 브랜치 docs/tts/커밋 명시.


## 2026-10-20 최신 — Serin Main16 자동생성 중 HTTP429 / HOLD 잠금 유지 / 다음 PT날짜 수동 안전 재개

- 사용자 실제 에러: `[STOP] main16_canyon_default_serin_01 failed (HTTP_429). HOLD+RUNNING.lock preserved; NO AUTO RETRY.`. 프로그램 안전장치가 의도대로 429에서 정지함. 해당 사용자 로컬 시점 누적 성공건·Google 원격 잔여량은 이 1줄만으로 미확인.
- Google 공식 정책: 프로젝트당 RPM/TPM/RPD 제한, RPD 태평양 자정 리셋. **HTTP429 정확 원인은 서버 오류 본문이 원 스크립트에서 남지 않아 불명**. 34회는 자체 로컬 예산이지 실제 쿼터 아님. 4키의 SEPARATE 설정도 실제 4프로젝트 소속 검증 결과 미제출.
- 지금 즉시 사용 가능한 호출0 명령: `python .\Renewal_Auto_TTS.py check`, `Get-Content .\_progress\HOLD.json` (비밀 API.txt 업로드 금지). HOLD/lock 및 실제 로그 수동 삭제·강제리셋 금지.
- **[HTTP429 안전 복구 정본](../../TTS_RenewalVoices_HTTP429_HOLD_ManualRecovery_20261020.md)** 기록, `LIMITLESS_Renewal_429_Safe_Resume_20261020.zip` 5176bytes, SHA256 `3aa70d65e67a4fe0b74d3039ee6fe16729ff06b59d7c3163ad30e389096af0fb` 사용자 전달. 안의 `Renewal_429_Safe_Resume.py`는 기존 원본 스크립트/키/WAV를 건드리지 않고 inspect와 next-Pacific-day resume만 지원.
- 설치후 `python .\Renewal_429_Safe_Resume.py inspect` (당장 가능, API0). **최소 다음 PT 날짜 + Google 실제 무료 쿼터/과금 확인 + 사용자 `RESUME429` 직접 입력** 이후에만 `... resume` 실행. 본문 원문/Voice/ID, journal ATTEMPT/FAIL/HOLD/LOCK 일치, 기존 WAV SHA 검증→`_progress/recovery_snapshots/`로 원본 429 증거 SHA 보관 뒤 lock 해제. 현재 날짜/에러 불일치/partial wav면 차단.
- 잠금 안전 해제 뒤에도 `python .\Renewal_Auto_TTS.py run --max-calls 1` 먼저; 사용자 청취 및 Google 할당량 확인 후 전체 본생산. 재차429면 HOLD 재발, 다음 안내 전 무작정 리셋 금지. 하렌 제외 규칙 유지.
- 복구도구 직접 제작·Python compile/오프라인5유닛테스트/ZIP CRC PASS, **현재 Gemini live API 호출0/신규 WAV0/Unity0** (사용자 실제 PC에서 실행 필요). `watch`가 Google429 자동 우회하도록 바꾸지 않음.


## 2026-10-20 최신 — 세린67·태온85 전용 4-Key 자동 생성기 배포(오늘 로컬 예산34회)

**이 단락이 과거 세린/태온/하렌 3인 1개 샘플 실험 안내와 하렌 수정 R1 즉시실행 지시보다 우선한다.**

- 바울 대표 최신 지시: `F:\Downloads\Renewal_Voices`에 **Serin/** 및 **Taeon/** 두 폴더, 하렌 제외. 사용자 로컬 `API.txt`에 4키를 세미콜론(;) 및 줄바꿈으로 구분; 세린·태온 **전 확정 대사**를 일괄 생성·대사별 WAV/ID·키 슬롯/사용량 로그·일별 카운트 기록. 오늘 남은 횟수 34회 사용자 보고.
- 실제 GitHub main catalog290과 Main07/17~21 및 Story audit 연결하여 **세린67(이미 Unity 등록58+미등록9), 태온85(등록82+미등록3) = 정확 ID152**를 [GitHub JSON 정본](../../TTS_Renewal_Voices_152_OriginalDialogue_Manifest_20261020.json)에 별도 동결. 기록 커밋 `9183376e`, 대사 ID 누락/중복0. Chapter3 미확정·하렌3·다른 인물 제외. 세린부터 생성.
- **직접 제작 ZIP**: `LIMITLESS_Renewal_Voices_Serin67_Taeon85_Auto_20261020.zip` **16492bytes / SHA256 `4fc85d78105f08c21241877c05d351d37eae3092f42e51a5f9c247b46ddeebc2`**. Python `Renewal_Auto_TTS.py` 25921bytes / SHA256 `efb653101f34276483f821a0b3d7eb9b57142a6b999f8c9c74d805dfdf8bdf88`. ZIP에는 Serin/Taeon/_progress 디렉터리, 설정/README/selftest. **API.txt는 기존 키 보호를 위해 ZIP에서 제외**. 사용자 로컬 API 키를 직접 수집/기록/노출하지 않는다.
- `conda activate py3_12` → `cd F:\Downloads\Renewal_Voices` → `python .\Renewal_Auto_TTS.py init`(사용자 로컬 정본 F:\study\codex\Project-Limitless 읽기, Manifest 생성, 기존 승인 세린/태온 WAV 샘플만 사본 복사) → `... check`(키4/152/개별 파일·카운트) → Google 프로젝트별 실제 무료 잔여와 무과금 확인 후 `RENEWAL_SETTINGS.json`의 `free_tier_confirmed:true`로 명시 → `... run`(RUN 승인, 하루 최대34회, 실패 시 HOLD) 또는 `... watch`(프로세스가 켜 있는 동안 PT 날짜가 바뀔 때 카운터 리셋하여 이어가기).
- Key1..Key4 카운트는 **ATTEMPT 성공/실패/결과불확실 전부** 포함. Append-only `_progress/api_events.jsonl`, 보기용 `_progress/USAGE_LOG.csv`, 날짜/PT 일일요약 `USAGE_TODAY.json`. Google 공식 **프로젝트당 RPD/태평양시간 자정 리셋**; 4키가 같은 프로젝트면 합산한도 없음. 최초 run에 SHARED/SEPARATE 실제 프로젝트 소속 확인 필수, 혼합 프로젝트는 설정에 그룹 수동 지정. 보수적 key+project-group 10회/일 가드, 전체34회 글로벌 예산. 과금·실제 잔여는 자동 조회·보증 불가, HTTP400/429/크래시/HOLD 즉시 중단, 자동 retry0.
- `samples/main20_confront_serin_01__Erinome.wav`→`Serin/main20_confront_serin_01.wav`, `samples/main21_route_taeon_01__Algieba.wav`→`Taeon/main21_route_taeon_01.wav` 원본 파일 존재/PCM 검증되면 사본만 복사·WAV+json SHA 기록·API0(각 67/85 제작목표에 포함). 다른 승인/원본 WAV 모두 보호.
- ZIP Python 문법 / unittest5 / mock Google GET·POST4/중복생성 재실행/그룹 safety/WAV 형식·API 키 분리/샘플 격리 복사·ZIP 압축해제 init/check **PASS**. 이 테스트에 실제 Google API 사용 **0회**, 사용자 F 드라이브 파일 변경 **0회**, Unity/Save 수정0. 실사용 전 사용자 PC 실행 및 Google quota/한국어 청취 검증 필요.
- **[상세 Python 제작 Runbook](../../TTS_RenewalVoices_Serin67_Taeon85_4Key_Automation_Runbook_20261020.md)** / [Issue #20](https://github.com/HappyPaul812/Project-Limitless/issues/20). Codex는 사용자가 전 범위 승인한 WAV에 한해 Unity 통합. docs/tts 브랜치만 기록, main 직접커밋·푸시·병합 금지.



## 2026-10-20 최신 사용자 결정 — 하렌 억양 재생성은 나중에 하렌 3대사 제작할 때

**최우선: DEFERRED / NO TTS CALL NOW.** 바울 대표 “나중에 하렌 대사 뽑을 때 할꺼니 문서화 해줘”.

- 하렌 Alnilam의 캐스팅 음색 자체는 긍정 평가. `main21_haren_haren_01` 샘플이 의문문처럼 끝나 **억양만 FAIL**. **지금 R1 `retake`를 실행하지 말고** 하렌 정식 3개를 제작하는 날 함께 다시 만든다.
- `01` = **“태온. 너 아직 멀었어.”** (평서문·어미 하강·단정), `02` = **“다친 데는 없고?”** (자연스러운 의문문), `03` = **“그럼 됐어.”** (평서문·어미 하강). Google 프리셋 **Alnilam 유지**, 세린/태온 한국어 샘플 긍정 결과 및 기존 WAV는 보호.
- 오프라인 준비된 R1 ZIP은 **미실행 참고도구**로 보존, 이전 아래의 FREE/YES1/PowerShell 실행안은 **과거 제안이지 현재 실행 지시가 아님**. 새 R1 WAV 생성0/추가 Google 호출0/Unity0. 다음 하렌 제작 시 실제 무료 quota/과금 확인과 개별 대사 청취 합격 필수.
- [실행 보류 결정 및 세 대사 억양 구분 정본](../../TTS_하렌_평서문_억양_수정R1_20261020.md) · [Issue #20](https://github.com/HappyPaul812/Project-Limitless/issues/20).


## 2026-10-20 19:12 최신 사용자 청취 — 하렌 첫 대사 억양만 불합격 / 독립 수정 R1 준비

- 바울 대표 “다 좋고, 근데 하렌 대사가 '아직 멀었어?'가 아니고 '아직 멀었어.'야”. 즉 세린 Erinome/태온 Algieba 한국어 샘플은 긍정 평가, 하렌 Alnilam 기본 음색도 긍정 평가이나 **하렌 `main21_haren_haren_01` 끝이 상승 억양(의문문)**으로 들려 부분 FAIL.
- 정본 원문은 **`태온. 너 아직 멀었어.` (마침표 평서문)**이며 원문/ID/보이스를 변경할 일이 없다. **정서/운율(말끝 낮춤) 연기 문제**이므로 하렌 첫 대사만 Style을 강화해 1회 재시험, 다른 두 샘플·원본 WAV 재제작 금지.
- **[하렌 단발 억양 수정 R1 정본](../../TTS_하렌_평서문_억양_수정R1_20261020.md)**. 사용자 다운로드 `LIMITLESS_Haren_Declarative_Retake_R1_20261020.zip` SHA256 `bb4624427859d503477de9f73e4d2cc49c3f7388d6125d88c6dcd32a6e8ab697` (6631bytes), Python+README. `F:\Downloads\Renewal_Voices`에 추가 후 `python .\Haren_Declarative_Retake_R1.py check` → `... retake`, **FREE/YES1** 승인 후 단1개 추가 Gemini POST. 성공 시 `samples/retakes/main21_haren_haren_01__Alnilam_DECLARATIVE_R1.wav` 별도 저장, 옛 `samples/main21_haren_haren_01__Alnilam.wav`와 JSON 및 태온/세린 2개 영구보존.
- 오프라인 mock/원본 해시/중복생성 차단/HOLD+lock/ZIP 검사 PASS, **실제 API 호출0/새 한국어 WAV0/Unity0** (사용자 실행 전). 최종 사용자 하강 억양 청취 PASS 대기. 추가 캐스팅이나 제작 전량 재개 승인은 별도.

## 2026-10-20 최우선 — 세린·태온 음색 사고 보고서 및 하렌 포함 3인 캐스팅

**다음 과거 장·시점의 Schedar 유지/“Main18 세린10 재제작 대기” 등의 역사 기록보다 아래 최신 정본을 우선한다.**

- **[2026-10-20 TTS 캐스팅 성별 역전·음색 불일치 사고 보고서](../../TTS_캐스팅_성별역전_음색불일치_사고보고서_및_3인재캐스팅_20261020.md)** — 사용자 피드백, 기술/청취 QA 차이, API400, DSP16 전량 실패, 캐스팅 재선정 및 재발 방지 전 과정. Google의 과거 버그 수정은 *미확정 가설*.
- Google Voice 공식: Schedar=Male(옛 세린), Gacrux=Female(옛 태온). 초기 음성의 실제 청취 승인과 최신 캐스팅의 장기 안정성 별개. TTS 담당 검증·재시도 판단상 실수로 기록.
- 사용자 최신 범위: **하렌3 / 세린67 / 태온85** = 총155개(현 Unity 등록 교체140 + Main20·21 미등록 확정15). Chapter3 미확정 제외. 제작은 **하렌→세린→태온**.
- 최신 후보: 하렌 **Alnilam**, 세린 **Erinome**, 태온 **Algieba**. 각자의 Google **공식 데모를 사용자 “좋네”라며 긍정 평가**했으나 **실제 한국어 시범 생성·최종 캐스팅 승인 전**. 차선은 각각 Algenib/Despina/Iapetus.
- 기존 Voice ID Schedar/Gacrux 재시도 및 DSP 패치 중단. 기존 사용자 승인 Main18 세린10·Main08 태온, 다른 전체 화자 WAV와 290 Catalog/Save/HOLD 원본 보존. 사용자 승인 전 게임에서 기존 성우를 삭제/교체·전체 음성 재녹음 금지.
- 실제 한국어 시험 제작은 사전 사용자 승인·Google 프로젝트+모델 quota 확인 후 TTS 담당. 사용자가 청취 합격하고 3인 각 대사 범위가 준비됐을 때만 Codex가 Unity 별도 구현·회귀. Main 직접 Commit/Push/merge 금지. [작업 Issue #20](https://github.com/HappyPaul812/Project-Limitless/issues/20).
## 오늘 결과
- **Main18 세린10 오류 전용 Schedar 안전 준비 v2** 작성 및 격리 fixture 테스트 완료. GitHub 기록: [TTS_Serin10_Schedar_v2_Preflight_20261013.md](../../TTS_Serin10_Schedar_v2_Preflight_20261013.md).
- 별도 배포 Python: `M18_Serin10_Schedar_v2_SAFE.py`, SHA256 `2edd62ff4c7cbc7fe055c665b39f35aaa5ef126691d1393c692fb42e8e1cd741`, 13,057 bytes. **Gemini/HTTP 함수 없음; sample 명령 항상 차단**.
- 명령: `python .\M18_Serin10_Schedar_v2_SAFE.py audit` (읽기 전용) → 로컬 결과 확인 → `python .\M18_Serin10_Schedar_v2_SAFE.py prepare` (독립 새 폴더에 오류 WAV10 SHA 검증 백업·잠금 보고).
- v1 `F:\Downloads\M18_Serin10_Staged.py` (Erinome/Kore만 허용) 실행 보류, 수정0.

## 확정 보호
- 세린 Voice Schedar 유지. Main18 오류10만 REGEN_REQUIRED, 사용자 청취 정상 Main18 세린5/Main19 세린7/Main17 승인 세린11 및 Unity 정상 통합 음성25 보호.
- 기존 오류10의 실제 당시 API Wire Voice는 UNKNOWN; 원인 확정 없음.
- 샘플 대상 `main18_ground_serin_01`, 원문/DialogueId/SpeakerId 변경 금지.
- 본 도구는 준비만. 샘플 생성 **NOT_EXECUTED**, 실제 사용자 LOCAL Stage 실행 **NOT_EXECUTED**.
- 사용자 무료 quota 잔량, Google Project별 모델 RPD 및 Billing 상태 **NOT_VERIFIED**, 사용 허용 확정 호출 **0**. 키 변경으로 quota 추가되었다고 가정 금지.

## 다음 선행조건
1. Codex 로컬 정적 대조 또는 사용자 `audit` 실행 결과 수령. `prepare` 성공 시 백업10 SHA, BASELINE/LOCKED 보고 확인.
2. 바울 대표가 Google AI Studio Projects/Billing, Dashboard Usage/Rate limits에서 **현재 해당 프로젝트·모델 무료 사용 가능 여부와 과금 위험 없음**을 확인.
3. 바울 대표의 명시적 **샘플 1회 호출 승인** 이후 새 1-call-generator 별도 코드 감사·실행. 실패/429/불확실한 경우 즉시 중단·재시도/키 변경 없음.
4. 사용자가 샘플 WAV의 실제 음색·원문 청취 PASS → 1개 정식 본으로 사용. 나머지9는 **별도 승인** 후 제작.
5. 전체 청취 승인 이후 Codex가 Unity 음성 연결. 담당 TTS는 Unity 수정하지 않음.

## GitHub 협업
- TTS 작업 브랜치: `docs/tts` 전용; `main` 직접 commit/push/merge 금지.
- 문서 갱신을 마쳤다면 해당 담당 커밋 SHA를 완료 보고에서 별도 확인.
- 세션 전환 시 최신 `main` 정본과 `docs/tts` 미병합 내용 양쪽 확인.


## 후속 LOCAL 실행 확인 — 2026-10-13

사용자가 실제 `py3_12` PowerShell 실행 화면을 제공:
- `M18_Serin10_Schedar_v2_SAFE.py audit` → `[AUDIT PASS] target=10, protected=25, Voice=Schedar, API=0`, 읽기 전용.
- `M18_Serin10_Schedar_v2_SAFE.py prepare` → `[PREPARE COMPLETE / API 0]`, 오류 WAV10 백업 SHA 검증·정상25 비교, Unity 수정0.
- 신규 외부 출력: `F:\Downloads\Voice\Limitless_TTS_Serin10_Schedar_v2`.
- **현재 상태 = `AUDIT_PASS / PREPARE_COMPLETE / SAMPLE_NOT_EXECUTED`**. 위쪽 과거 `LOCAL NOT_EXECUTED` 항목은 해당 시점 이전 기록.
- 사용자의 터미널 로그 확인이며 담당자 독립 로컬 파일 접근 검증은 아님.
- Gemini TTS 생성0 / WAV 신규 생성0 / 전체 무료 잔량·Billing 상태 `NOT_VERIFIED`. 생성기 없음·`sample` 실행 차단 유지.
- 다음은 사용자 AI Studio의 현재 해당 Project/모델 한도·과금 상태 확인 및 **샘플 한 번 명시적 승인**. 승인 후 안전한 별도 생성기 → 사용자의 실제 청취 승인 → 이후9개 별도 승인. 샘플 ID: `main18_ground_serin_01`, Voice `Schedar`.
- 현황 상세: [v2 사전 준비 및 LOCAL 실행 후속](../../TTS_Serin10_Schedar_v2_Preflight_20261013.md).


## 2026-10-13 첫 Schedar 샘플 사용자 음색 승인 — 현재 최신

**상태: `SAMPLE_SAVED / USER_VOICE_APPROVED / REMAINING_9_NOT_AUTHORIZED`.**

- 사용자 PC에서 `M18_Serin_Schedar_SAMPLE_ONE.py --key-slot 2 --approve-one-call` 및 `YES`로 `main18_ground_serin_01` 샘플 **1회** 요청·저장.
- 모델 `gemini-3.8-flash-tts`; Voice `Schedar`; 새 WAV `F:\\Downloads\\Voice\\Limitless_TTS_Serin10_Schedar_v2\\sample_candidate\\main18_ground_serin_01.wav`.
- 사용자 로그 SHA256 `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`; 로컬 파일 별도 해시 재검증은 아직 수행하지 않음.
- **사용자 직접 청취 발언: "목소리 제대로 나와." → 음색 `USER_VOICE_APPROVED`.** 원문 누락/단어 정확성까지 사용자가 명시적으로 PASS한 것은 아니므로 `EXACTTEXT_LISTENING_PENDING` 유지.
- 샘플은 정식 재제작 10개 중 1개로 **재사용, 중복 생성 금지**. 남은9개는 **별도 생성 승인 필요**; 아직 호출0. 정상 Main18·19 Unity 음성25, Main17 승인 세린11, 기존 오류 WAV 원본/백업 보호.
- 이후: 로컬 checkpoint와 샘플 hash 대조 → 현재 프로젝트·모델 무료 quota/과금 확인 → **나머지9개 명시적 승인** → 9개만 StoryOrder대로 생성/QA → 사용자의 전체 청취 → Codex 별도 Unity 통합.
- 이 항목이 상단 과거 `SAMPLE_NOT_EXECUTED`를 대체. 현재 상태는 샘플 성공1·잔여9 잠금.


## 2026-10-13 나머지9 생성기 오프라인 준비 — 최신
- **사용자가 남은9개 제작 명시 승인**, 샘플 `main18_ground_serin_01` 재사용. 그러나 이 ChatGPT 작업에서 실제 Gemini 호출0·신규 WAV0 (사용자 PC 실행 대기).
- 사용자 다운로드용 `M18_Serin_Schedar_REMAINING9.py`, SHA256 `94b75a9f0c7901bb4a94b2f0da017985036075809a593ee6c5838196a6a46c76`. SAFE v2와 샘플 생성 Python 2개 SHA 고정, 기존 25 정상/원본10/백업10/샘플 SHA 검증.
- `check`는 API0, `run --key-slot 2 --max-new-calls 9 --approve-remaining-nine`는 사용자가 **실제 무료 잔여량과 Billing을 직접 확인하고 YES** 입력해야 시도. Project+Model quota, 키 변경에 따른 신규 예산 가정 금지. 쿼터 불확실 시 `--max-new-calls` 줄이기.
- 기존 샘플 제외 Story `02,07,08,09,10,11,12,13,14` 순서. 자동 retry0, 호출 25초 간격, 실패/429 즉시 HOLD, 기존 성공 WAV 재생성 차단. 결과 Stage 폴더의 `remaining9_wav`와 StoryOrder CSV.
- 오프라인 mock 정상9회/둘째429 즉시 중단 테스트 통과. 실제 SDK/사용자 PC 진행 여부는 아직 `NOT_EXECUTED`/후속 스크린샷 필요.
- 상세: [나머지9 생성기 보고](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md). 이번 문서는 `docs/tts` 전용, main 변경/병합0, Unity 변경0.


## 2026-10-13 잔여9 실제 제작 부분 성공 / Story13 SAFE HOLD — 최신 상태

사용자 PC PowerShell 스크린샷으로 `M18_Serin_Schedar_REMAINING9.py` 실행 결과 확인:
- 승인 샘플 Story06 1개 **재사용**(신규 호출0).
- Story02,07,08,09,10,11,12 `[SAVED]` **7 WAV 신규 생성**(ID별 SHA 출력, 실제 바이트 검사 별도 대기).
- Story13 `main18_watcher_witness_serin_01` `[API REQUEST]` 후 `[SAFE HOLD] / RuntimeError` **FAILED_OR_UNCERTAIN_HOLD**; 세부 원인 UNKNOWN, 429라고 단정하지 않음. Story14 `main18_watcher_witness_serin_02` **NOT_CALLED**.
- **전체10: SAVED8(샘플1+7), 불확실1, 미호출1.** 지금까지 생성 시도 총9(기존 샘플1+이번8)로 관측. 전체10 사용자 청취 승인 아직 아님.
- **중요: 추가 API0, 같은 ID 자동 재시도/새 Key/다음 Story14 호출/잠금 및 checkpoint 초기화 전부 금지.** 다음은 Codex가 사용자 로컬 오류로그·attempt·HOLD·lock·생성 파일의 SHA를 읽기 전용 대조하고 결과 보고. 재개는 별도 승인과 현재 무료 quota 재확인 후.
- GitHub 결과 문서: [나머지9 생성기 및 SAFE HOLD 기록](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md). 사용자 화면 근거, 독립 검사 미실시. Unity/Main 변경·WAV 적용0.


## 2026-10-13 API KEY Project 귀속 불일치 — 최신 조사 대기
- 사용자 Google AI Studio 10/13 Gemini3.8 Flash TTS RPD: 키1이라고 설명한 Default Gemini Project **9/10**, 키2라고 설명한 Project-Limitless **0/10**. 그래프상 관측값이며 정확한 실시간 잔여 호출 수 확인이 아님.
- 생성 CLI `--key-slot 2`인데 로그상 샘플 포함 API 시도9개. 키2 실제 프로젝트 연결 여부, 스크립트 환경변수 선택, Google 사용량 반영 지연 **모두 미확정**.
- Codex 정적 감사로 `--key-slot` 구현, 키→Project 대응(값 출력 금지), Story13 RuntimeError/잠금 체크포인트를 확인할 것. API 호출·추가 생성·HOLD 해제·키 변경 0.
- 현재 WAV 상태 **8 SAVED(샘플1+7), Story13 실패/불확실1, Story14 미호출1**, 정상25/원본10/백업10 보호. [세부 문서](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md).


## 2026-10-13 Codex 읽기 전용 감사 후 최신 인계 — SAFE HOLD 유지

바울 대표가 Codex 감사 완료 보고 화면 제공:
- 샘플1+신규 Story02/07~12 WAV7 = **8개 SHA256/checkpoint/PCM 대조 PASS**.
- Story13 `main18_watcher_witness_serin_01`: 시도+HOLD 기록 있음, 신규/부분 WAV 및 결과 JSON 없음. `RuntimeError` **예외 클래스만** 기록돼 정확한 실패 지점/429/할당량 여부 **UNKNOWN**.
- Story14 `main18_watcher_witness_serin_02`: 로컬 시도·결과·신규 WAV 없음 → 미호출로 기록.
- Codex 보고: 대상 폴더42 + 프로젝트 보호 파일5,199개 SHA 불변, HOLD·SESSION.lock·시도 기록 보존. 조사 중 API 0/WAV 수정삭제0/Unity0/Push0.
- **현재 상태 SAVED8 / FAILED_OR_UNCERTAIN_HOLD1 / NOT_CALLED1.** 기존 샘플 재생성 금지. Stage, checkpoint, lock 삭제·초기화 금지, Key 변경/Story14 선호출/Story13 재시도 금지.
- Google 두 프로젝트의 RPD 9/10 vs 0/10의 귀속 문제와 실제 KEY2 연동, quota/과금 위험은 확인되지 않았음. 새 요청에 대한 **재개 승인 없음**.
- 남은2개는 조건 확인 후 사용자 **별도 승인**을 받아 새로운 수동 재개 이력으로 처리. 전체 음성 청취/Unity 통합은 이후 별도 승인.
- 기록 정본: [잔여9 생성기 및 Codex 감사 후속](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md).


## 2026-10-13 Story07 사용자 음색 불일치 의심 — 최신 청취 상태
- 사용자 청취 인상: `main18_heat_vent_serin_01.wav` **“감기걸린 여자? 변성기 전 미소년?”**.
- **저장·SHA/PCM PASS와 별개로 음색 `USER_REPORTED_TIMBRE_MISMATCH / USER_REVIEW_REQUIRED / HOLD`**. 기존 승인 샘플 Story06과 음색 일치 확인 전 `KEEP` 또는 Unity 적용 불가. 현재 사용자가 최종 폐기/재제작 판정을 내린 것은 아니므로 `REGEN_REQUIRED`은 사용자 판단 대기.
- 사용자 PC 해당 WAV 자체 직접 재생하지 않았음. 기존8개 저장·Story13 실패불확실·Story14 미호출 상태 불변. 추가 API0, 기존 WAV/잠금/체크포인트 보존, main/Unity 변경0.
- [상세 청취 기록](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md).


## 2026-10-13 Story07 업로드 오디오 직접 비교 후 재제작 권고 — 최신
- 사용자 첨부 `main18_heat_vent_serin_01.wav` 4.52초, SHA256 `2426af7024c27cee7bd2bbd94883a65215a1ef406aedb2bd7dea2bf36a60c3b1`. 승인 샘플 `main18_ground_serin_01.wav` 6.52초, SHA256 `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`는 과거 사용자 샘플 로그 SHA와 동일.
- 실제 업로드 WAV 비교 결과 heat_vent는 사용자가 지적한 비음/탁한 질감이 승인 샘플과 달라 **`TTS_QA_RECOMMENDS_REGEN`**. 사용자 `REGEN 확정`이나 신규 생성 호출 승인은 아직 없음. 원본/체크포인트 보호.
- WAV 중앙 F0 각각 약175/180Hz; 음색 차이는 성별/음높이 단정이 아니라 캐릭터 일관성 문제. 업로드본이 로컬 성공 WAV와 동일한지 SHA 재대조 필요.
- 현재 저장성공8 유지, 그중 승인샘플 음색PASS1·Story07 재제작 권고1·나머지6 청취 대기; Story13 불확실, Story14 미호출. 추가 API0·Unity0·main0. 상세 [생성 및 WAV 비교 QA](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md).


## 2026-10-13 신규 TTS 6개 업로드 — 세린 음색 일관성 검수 확대

사용자가 Story02/08/09/10/11/12 WAV 여섯 개를 업로드하고 “다른 것도 조금씩 다르다”고 보고했다. 업로드 WAV 포맷과 F0 기초 검수: Story02 209.5, 08 198.2, 09 181.1, 10 189.7, 11 207.2, 12 211.0Hz 중앙값(참고용). 기준 승인 샘플 Story06 약180.2Hz. **음높이만으로 화자/재제작 필요 여부 판정 불가.**

- Story06 샘플은 사용자 음색 승인 유지.
- Story07은 사용자 불일치 보고 및 담당 재제작 권고, 기존 파일 보존.
- Story02/08~12 여섯 개는 **`USER_LISTENING_REQUIRED`**. 사용자 최종 KEEP/REGEN 판정 미정; 저장성공을 청취 PASS로 승격하지 않음.
- 전체 저장8개, Story13 실패/불확실 HOLD1, Story14 미호출1. 기존8개 중 사용자 음색 승인1, 재제작 권고1, 청취 판정 대기6.
- 추가 생성·재호출/잠금 해제 금지. 잔여 신규 제작·Story07 재제작은 프로젝트 quota 및 Billing 확인/사용자 명시적 별도 승인 후.
- 신규 음색 QA 상세: [TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md).


## 2026-10-13 정정 — 사용자 신규8 WAV 전부 청취 완료

사용자가 **“난 다 들어봤다고....”**라고 명확히 밝혔으므로 이전 '나머지6 청취 대기'는 오기다.

- 저장 성공8개 모두 사용자 직접 청취: `USER_LISTENED=8`. 다시 들어보라고 요구하지 말 것.
- Story06 ground 1개: `USER_VOICE_APPROVED`.
- Story07 heat_vent 1개: `USER_REPORTED_TIMBRE_MISMATCH / TTS_QA_RECOMMENDS_REGEN`.
- Story02 leon, Story08~11 overheat4, Story12 vibration1의 6개: `USER_REPORTED_TIMBRE_VARIANCE`; 각각 음색이 조금씩 달라 사용자가 직접 지적함. 사용자 파일별 최종 KEEP/REGEN 결정은 미표명, 추측 금지.
- Story13 실패/불확실 HOLD, Story14 미호출. 기존 WAV8/정상25/backup/LOCK 보호. 추가 API0, Unity0, main0.
- 정본: [생성기 및 사용자 청취 정정](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md)


## 2026-10-13 사용자 요청 오프라인 음색 보정 6 WAV — 미리듣기

사용자: **“heat_vent는 빼고 니가 못고쳐?”**. 이전에 업로드한 세린 `leon`, `overheat_01~04`, `vibration` WAV6개만 승인 `ground` 샘플 기준의 경미한 pitch/EQ/음량 보정으로 **`*_PREVIEW.wav`**를 제공했다. `heat_vent`는 제외. 기준 `ground` WAV는 변경 없이 ZIP 안에 동봉. 출력 ZIP `LIMITLESS_Serin6_ConsistentVoice_Previews.zip` (별도 다운로드 산출물), QA CSV 포함.

- WAV6개 샘플 길이 동일, 24kHz mono PCM16, clipping0, ZIP CRC PASS. 기존 원본/체크포인트/Unity WAV25/Story13 HOLD/기타 WAV 모두 무변경. Gemini API0.
- PREVIEW는 확정 교체본 아님. 동일 화자로 들리는지 사람 청취 최종 평가 전 `USER_APPROVAL_PENDING`. TTS 모델이 달리 만든 발성 질감을 DSP로 완전 복구한다는 의미 아님.
- 이전 관례의 기본 후처리 금지 정책에 따라 **정식 WAV를 덮어쓰지 않음**. 사용자 요청에 의한 비교 전용 새 파일만 제작.
- 상세: [생성 및 PREVIEW 기록](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md). `docs/tts`에만 기록, main 변경 없음.


## 2026-10-19 보정본 6개 재전달 / 다음 Main18·20·21 제작 사전준비

사용자: “니가 수정한거 아직 안받았지 나? 일단 수정한 주고 더 뽑을 꺼 준비해줘.”

- 기존 `LIMITLESS_Serin6_ConsistentVoice_Previews.zip` 실제 파일 재확인, SHA256 `a5878e6d4f1b1e858f4b505f23669ab147da2dd466fe30443b0d6116a7246cb1`; ZIP CRC와 보정 WAV6 형식 PASS. 사용자 대화에 재다운로드 링크 제공. `heat_vent` 제외, `ground` 기준 음성 포함. PREVIEW는 아직 정식 Unity 승인 아님.
- 새 `LIMITLESS_TTS_Next_Preflight_20261019.py`(읽기 전용) 및 25행 다음 제작 큐/README/Codex 조사 지시가 포함된 `LIMITLESS_TTS_Next_Preflight_Pack_20261019.zip` 제공. SHA `de2036d1557399fa34aa02257ecd61c6cf887585786c8fbacf6465d9ca5c55e9`. 사용자 PC F: 실행 `NOT_EXECUTED`.
- 격리 fixture 정상 preflight 및 SHA 손상 중단 테스트 PASS. Google TTS SDK/API/키 사용0, 오프라인 읽기만 실행 가능.
- **Main18 다음 3개:** Story07 heat_vent 음색 문제 재제작 후보 (기존 SAVED 유지), Story13 watcher_01 FAILED_OR_UNCERTAIN_HOLD(잠금/시도 기록 보존), Story14 watcher_02 NOT_CALLED (Story13 선호출/건너뛰기 금지).
- **이후 Main20 세린 5개**, **Main21 NPC17개 중 하렌3 Voice 미승인 BLOCK / 기타14 후보**. 3+5+14=22는 향후 잠재 호출 수일 뿐 현재 quota 확인 수치 아님. 정확한 LOCAL Runtime 순서/기존 WAV 존재 먼저 확인.
- 2026-10-19 프로젝트별 실시간 RPD 잔여·Billing/키 소속 `NOT_VERIFIED`, 추가 API 요청 승인0. 이전 성공8 및 Preview6 원본 유지, 승인 없이 LOCK 해제·재시도·다른 Key 사용 금지.
- 상세: [보정본 재전달 및 다음 배치 계획](../../TTS_Serin_Preview6_and_NextBatch_Preflight_20261019.md). 기록은 `docs/tts` 브랜치만, `main` 변경 없음. Unity 수정0.


## 2026-10-19 사용자 로컬 PRE-FLIGHT PASS — 최신 확인

- 사용자 PowerShell 화면에서 실제 `LIMITLESS_TTS_Next_Preflight_20261019.py check`가 `[PRE-FLIGHT PASS]`를 반환.
- Unity 정상25 SHA PASS, 샘플 ground 원본 SHA PASS, Story02/07~12 성공7 WAV·시도 SHA PASS.
- Story13 `SESSION.lock`, failed/uncertain attempt, HOLD 유지; 신규 WAV/result 없음. Story14 새 attempt/result/WAV 없음.
- 추가·중복 attempt 없음. Main20 5 세린 예정, Main21 17 NPC 중 하렌3 BLOCK.
- `[PASS]`는 생성 허가 아님. 지금 quota/project/billing `NOT_VERIFIED`; TTS API0 / writes0 / Unity0 / Git writes0으로 사용자 화면에 표시.
- 사용자에 다시 preflight를 요구할 필요 없음. 이후 수동 재개는 HOLD 원인 판독과 Project+Model 사용 가능 무료 횟수 확인·비용 통제·별도 사용자 승인 후에만 진행.
- 세부 [보정본 및 다음 배치 사전검사](../../TTS_Serin_Preview6_and_NextBatch_Preflight_20261019.md).


## 2026-10-19 Main18 마지막3 수동복구 생성기 준비 (최신)

사용자: **“그래, 그럼 tts 작업 마저 하자”**. 기존 8 WAV·승인 ground/보정 Preview6을 재생성하지 않고 **Story07 heat_vent 재제작, Story13 watcher_01 불확실 실패 신규 수동 시도, Story14 watcher_02 신규 제작**만 진행하도록 생성기 준비.

- 배포: `LIMITLESS_Main18_Serin3_Recovery_20261019.zip`, SHA256 `0b580820b4d0c2bbedd8e86266f38054312f0b641a1bce452b0c64b72a76650d`.
- Python `LIMITLESS_Main18_Serin3_Recovery_20261019.py` SHA256 `799e1492312199de21daad014786ae636350f3944611c621907e37854a7df191`.
- `check`: 기존 10오류 원본+10백업, Unity 정상25, 승인 샘플 SHA, 성공 신규7, Story13 기존 HOLD+SESSION.lock, Story14 미호출, 새로운 복구 기록 검사. API0/쓰기0. 2026-10-19 사용자 PC 실행 상태 `NOT_EXECUTED`.
- `run`: 현재 Key2 연결된 실제 Google Project + model 무료 사용 가능량/비용을 **사용자가 직접 확인**하고 `--confirm-free-quota` + 대화형 `FREE`/`YES` 조건을 만족할 때만 요청. Story13가 포함되면 **`--authorize-manual-story13-retry` + `RETRY13`** 추가 확인. 같은 KEY SLOT2 사용, 자동 재시도0, 최대3회, 최소26초 간격.
- Output `F:\Downloads\Voice\Limitless_TTS_Serin10_Schedar_v2\ManualRecovery_20261019\candidates\`. 기존 `remaining9_*`·오류 시도·HOLD/SESSION.lock **절대 삭제/초기화하지 않음**. 신규 별도 checkpoint만 기록. 실패/429 즉시 새로운 HOLD 잠금 후 뒤 ID 중단.
- 오프라인 mock 3회 연속 성공·두번째429시 즉시중단·기존 HOLD 불변·성공 후보 SHA 변조 감지 PASS. 실제 Gemini TTS 호출0, 실제 무료 quota/Billing `NOT_VERIFIED`, 신규 WAV0.
- 새 후보는 사용자 실제 음색/원문 청취 승인 필요. 승인후 Codex Unity 별도 적용. Main20 세린5, Main21 17 중 하렌3 BLOCK은 향후 별도 제작, 이번 생성기 대상 아님.
- 상세: [Main18 세린3 수동복구 도구와 승인 게이트](../../TTS_Main18_Serin3_ManualRecovery_20261019.md). 이번 작업은 `docs/tts` 전용, `main` 및 Unity 수정0.


## 2026-10-19 최신: Main18 세린3 수동복구 사용자 실행 성공

- 사용자 제공 PowerShell 결과: `Story07 main18_heat_vent_serin_01` SAVED(4.88s), `Story13 main18_watcher_witness_serin_01` SAVED(4.96s), `Story14 main18_watcher_witness_serin_02` SAVED(4.20s). `[SUCCESS] New candidate WAV 3 / no Unity changes / legacy HOLD preserved`.
- 이번 실제 생성 요청 **3회 성공**. 사용자 PC 로그 근거이며 신규 세 WAV의 독립 SHA/PCM/의미 청취 검증은 아직 대기.
- 새로운 세 후보는 `ManualRecovery_20261019\candidates\` 아래 보관하고, 기존 실패 HOLD/SESSION.lock 및 과거 checkpoint 변경하지 않음.
- Main18 세린10 **후보 WAV 10개 확보**: 기존 ground 승인 샘플1 + 보정 PREVIEW6 + 신규 후보3. 사용자 전체10 최종 음색·원문 승인 아직 아님. 정상 Unity25와 Main17 승인본 보호.
- Main18 추가 생성은 현재 불필요. 다음은 새 후보3만 사용자 청취 QA, 필요시 Preview6 정식 승인 분리, 이후 Main20 세린5 / Main21 NPC14(하렌3 음성 미확정 BLOCK) 별도 생성·무료 quota 확인.
- 이번 담당은 Github `docs/tts` 기록 외 신규 API 호출0, Unity/main 변경0.
- 상세: [Main18 3개 복구 완료 후 후보 상태](../../TTS_Main18_Serin3_ManualRecovery_20261019.md).


## 2026-10-19 Main18 세린 10개 최종 사용자 승인 → Codex Unity 통합 위임 — 최신 상태

사용자 최신 명시적 지시: **“다 이상 없으니까 메인18은 끝인거 같으니 게임에 넣자, 지금 파일이 여기저기에 있으니까 알어서 할 수 있게 코덱스한테 시켜줘.”**
- **Main18 세린 교체 TTS10 사용자 최종 청취·Unity 적용 승인**. 과거 `USER_LISTENING_PENDING`/오류 음색 이력은 이 지시로 후보 세트에 대해 변경. 승인된 유일한 최종 소스는 `ground` 1, 오프라인 보정 PREVIEW6, 10/19 최신 수동 복구 후보3. **기존 잘못된 raw `remaining9_wav` 또는 원본 `Limitless_TTS_Output_Main18`의 불합격 TTS를 Unity에 넣지 말 것**.
- **Codex 구현 이슈:** https://github.com/HappyPaul812/Project-Limitless/issues/17
- **TTS→Codex 실제 파일/SHA/검증 인계:** [TTS_Main18_Serin10_Approved_Unity_Handoff_20261019.md](../../TTS_Main18_Serin10_Approved_Unity_Handoff_20261019.md), `docs/tts` 작업 브랜치.
- 샘플 `F:\Downloads\Voice\Limitless_TTS_Serin10_Schedar_v2\sample_candidate\main18_ground_serin_01.wav` SHA `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`. PREVIEW6 ZIP `LIMITLESS_Serin6_ConsistentVoice_Previews.zip` SHA `a5878e6d4f1b1e858f4b505f23669ab147da2dd466fe30443b0d6116a7246cb1`. 복구3 `ManualRecovery_20261019\candidates`의 Story07/13/14, SHA는 로컬 results에서 확인.
- 기존 Main18 정상12+Main19 정상13 **Unity 연결25 보호**, 기존 Catalog/GUID/Save/기존 WAV/Story13 HOLD/lock 보호. **Codex는 `codex/tts-main18-serin10-integration-20261019` 독립 로컬 worktree**로 실제 Unity 추가10·Main18 22/22 QA 수행. `main` 직접 commit/Push/merge0, 선별 로컬 commit만.
- **현재 상태:** `USER_APPROVED_10 / CODEX_ASSIGNED_VIA_GITHUB_ISSUE / UNITY_INTEGRATION_PENDING`. 이슈를 만든 것만으로 Codex 로컬 실행·Unity 통합 성공을 뜻하지 않음. 대표가 Codex 세션에서 이슈 링크를 전달하거나 작업을 시작해야 함.
- TTS API 호출0, 본 TTS 세션 Unity 파일 수정0. Codex 완료 보고에 SHA 매핑·최종 Main18 22/22·QA/브랜치/로컬 commit와 Push 여부 기재 요구.


## 2026-10-19 키1 7회·키2 10회 보고 / Main20·Main21 17회 배분 (최신)

- 사용자 현재 잔여 호출 보고: Key1=7 / Key2=10. 서로 다른 Google Project+gemini-3.8-flash-tts의 실제 FreeTier 사용량·Billing 검증 **NOT_VERIFIED**; 각 키당 독립 사용 가능 시에만 최대17. 동일 Project면 합산 금지.
- 제작 큐: Main20 세린5 + Main21 비하렌 NPC12 = 17. Key1: Main20 5+Main21 2, Key2: Main21 10. Main21 미생성2: `main21_haren_taeon_02`, `main21_prepare_serin_01`. 하렌3 Voice미확정 BLOCK.
- [계획과 Codex 로컬 실행 지시](../../TTS_Main20_Main21_17Call_Plan_20261019.md) 및 [17행 CSV 큐](../../TTS_Main20_Main21_17Call_Queue_20261019.csv) 기록. 기존 Manifest·Main18 세린 승인10·정상25 불변.
- 이번 TTS 세션에서는 Google API 실제 호출0 / 사용자 로컬 WAV0 / Unity0 / main 변경0. Codex 실행 대기, 신규 음성 모두 사용자 청취 승인 전 Unity 적용 금지.


## 2026-10-19 역할 재정정 / 직접 Python 스크립트 제작 (현재 최신)

- 바울 대표가 **“TTS는 니가 파이션 스크립트로 하는건데 누구한테 시키니?”**라고 업무 책임을 명확히 교정. TTS 스크립트 작성/음성 제작 계획/품질 검수는 TTS 담당 책임, 실제 Gemini API는 바울 대표 로컬 PC의 Python에서 실행. **Codex는 사용자 청취 승인 후 WAV Unity 통합·QA만** 담당한다. 이전 Codex 생성 위임 지시는 철회.
- Main20 세린5 + Main21 비하렌 일반12, KEY1=7/KEY2=10 고정 큐. 하렌 Voice 미승인3 BLOCK, 이번 quota 이월 Main21 일반2.
- TTS 담당이 직접 `LIMITLESS_TTS_Main20_Main21_17_Python_Pack_20261019.zip` 작성·다운로드 제공. ZIP 9874 bytes SHA256 `1a5a80afc98023be8324809e5fd6e7dad8cd12eccb852843525bf132c75bf612`. 포함 Python/17행 CSV/README. Google 비밀키 없음.
- 실행: `python .\\LIMITLESS_TTS_Main20_Main21_17.py check` (API0) → 실제 별도 Project+Model 무료 quota/Billing 확인 후 `python .\\LIMITLESS_TTS_Main20_Main21_17.py run --confirm-free`. 실제 사용자 PC API0/WAV0이며 앞으로 사용자가 로컬에서 실행하여 로그 공유.
- Python compile/읽기 전용 CSV SHA·키배분 점검 PASS, 오프라인 모의 성공17/보존, 실패2번째 즉시 HOLD/LOCK·뒤호출0 및 저장 WAV SHA 변조 검출 PASS. **실제 Google 서비스 요청/성공/청취는 미검증**.
- 최초 잘못 만들었던 이슈 #20을 **TTS 담당 제작 추적 이슈**로 수정하여 Codex 생성 요청을 명시적으로 취소. 정정 정본: [17회 계획/스크립트](../../TTS_Main20_Main21_17Call_Plan_20261019.md), [17개 큐](../../TTS_Main20_Main21_17Call_Queue_20261019.csv).
- Main18 세린 승인10 별도 Unity 통합 이슈 #17, Main18/19 기존 정상25·원본/HOLD/WAV/Save 변경0. docs/tts만 문서 갱신, main commit/merge/Push0.


## 2026-10-19 Main20·21 사용자 청취 오류9개 / 샘플 우선 재제작 (현재 최신)

- 바울 대표가 실제 청취 후 **세린8개 남성 음색**(`main20_after_colossus_serin_02/01`, `main20_confront_serin_03/02/01`, `main21_route_serin_01`, `main21_pulse_serin_01`, `main21_heat_serin_01`), **태온1개 여성 음색**(`main21_route_taeon_01`) 보고. 이9개는 청취 불합격·Unity 적용 보류. 나머지 8개는 별도 최종 합격 확정 아님.
- 원래 17회 제작 Python의 Style에 성별 명시 부족 확인. 실제 모델 오류 원인/원격 voice wire/실제 사용자 파일의 바이트 검증은 **미확정**, 성별 Style 지시가 효과를 보증하지 않음.
- TTS 담당 직접 `LIMITLESS_Main20_Main21_VoiceFix9_SampleFirst_20261019.zip` 제작/배포, SHA256 `28c130098d1e33d7889ffd79dce715ace992283885b7e01c8cdca59255160dcb`, 자체 Python `LIMITLESS_TTS_VoiceFix9.py` SHA256 `8440a69fc7bc2031da7c592e257e4bd469efeab16be25f7efdd2dfbc667c50ca`. 기존 Stable ID/원문/Voice Schedar/Gacrux 유지, 명시적 세린 여성/태온 남성 Style 추가. **세린1+태온1 샘플 먼저** 제작·사용자 청취 승인 → 나머지 세린7 별도 실행. 오류 시 즉시 HOLD, 원본17 SHA/체크포인트 변경0.
- 압축 파일과 실행 안내는 사용자 대화에서 직접 제공, README 동봉. 일단 `python .\\LIMITLESS_TTS_VoiceFix9.py check` (API0), 새로운 무료 한도를 현재 해당 프로젝트에서 확인한 뒤에만 `sample-serin`/`sample-taeon`. **초기 키1 7·키2 10은 이미 소비됐을 수 있으므로 추가9회라고 가정 금지**.
- 오프라인 compile, 기존17 SHA mock, 샘플2+나머지7 모의 성공 및 2번째 API 오류에 즉시 HOLD/lock/다음호출0 PASS. 실제 사용자 PC/실제 Gemini API **신규 호출0, WAV 신규0**; 샘플 QA 미확인, Unity0, main0.
- 상세 새 QA 정본: [Main20·21 VoiceFix9 오류/검수](../../TTS_Main20_Main21_VoiceFix9_20261019.md). 기존 이슈 #20은 생산→청취 QA 오류/재제작 추적용으로 갱신. TTS 담당이 Python을 직접 만들고 Codex는 사용자 최종 승인 후 Unity 통합만 한다.


## 2026-10-20 / 세린·태온 보이스 성별 설정 문제 확인 및 새 Python 샘플 팩 (최신)

- 사용자: “tts 되니까 작업하자.” 최신 청취 오류9(세린8 남성처럼 들림, 태온1 여성처럼 들림) 재개.
- **Google 공식 Firebase voice table의 원인 확인:** `Schedar=Male`, `Gacrux=Female`. 전일 작성한 “기존 보이스 유지 + 성별 Style” 실행팩은 추가 재제작에 사용하지 않는다. 해당 팩의 Style 조건만으로 프리셋의 성별 차이 보완은 신뢰할 수 없음.
- TTS 담당이 **새 Python ZIP 직접 제작**: `LIMITLESS_TTS_Main20_Main21_GenderFix9_SampleFirst_20261020.zip`, SHA256 `c9f8e6af586c37c3b881f37cc4ff3db46b3485991022c485a2c3b9f7c752925d`, script `LIMITLESS_TTS_GenderFix9_20261020.py` SHA256 `fd55444cb033d314a462d7f3a41d109f42260c9c12569ea7e1606e8368349c27`. ZIP/CSV/README 사용자 대화에서 제공.
- 샘플 후보 보이스 **세린=Erinome(Female, Clear)**, **태온=Alnilam(Male, Firm)**. 이번 Main20·21 테스트에서만 임시 사용, 기존 Main18 청취 승인 세린10이나 다른 챕터 음성 Voice Registry를 일방 변경하지 않음. 사용자 두 샘플 청취 승인 후 나머지 세린7 생성.
- `F:\Downloads\Voice\LIMITLESS_TTS_GenderFix9_20261020`에 압축 해제한 뒤 `python .\\LIMITLESS_TTS_GenderFix9_20261020.py check` → 실제 Google 무료 quota 및 과금 확인 후 `sample-serin`, `sample-taeon`; 출력 `F:\Downloads\Voice\Limitless_TTS_GenderFix9_20261020\candidates`. 체크에서 기존 17 성공 WAV/결과 SHA 불일치·경로 문제 있으면 호출금지/보고.
- offline Python compile/ZIP CRC, 17원본 SHA mock 보호, 샘플2→승인→나머지7 성공, 2번째 요청 모의 오류 시 HOLD/lock 및 뒤호출0 PASS. **실제 PC API 호출·실제 WAV 신규 생성=0**, 게임 Unity 적용0. 실사용 결과와 청취 평가는 사용자 응답 대기.
- 상세: [2026-10-20 정정 및 실사용 Python 인계](../../TTS_Main20_Main21_VoiceGenderFix9_20261020.md). 기존 Issue #20 보이스 오류 추적 유지. Codex는 사용자 승인 WAV Unity 통합만.


## 2026-10-20 최신 — 파형 보정 오프라인 시험판 (바울 대표 요청)

- 사용자는 성별 프리셋 교체 대신 **이미 생성한 WAV9 파형 보정** 요청. TTS 담당이 `LIMITLESS_TTS_DSP9_Offline_20261020.zip` 직접 작성·배포 (SHA256 `1a91fa078a367c7a6336fdb44209b0cd743ee43d41da4c75c392fd04c99afec9`). Google API 호출0, Unity 수정0.
- `check` 원본9 읽기 전용 검사 → `preview` 세린1·태온1 각각 gentle/medium/strong (6개) → 바울 대표 청취 승인 후 `apply --serin ... --taeon ...`로 오류9개만 새 위치에 후보 제작. 피치+포먼트 근사 DSP, 원본 음색 완벽 복구 보장 없음.
- 샘플용 합성 WAV 기반 compile/check9/preview6/apply9/원본 SHA 불변/24k PCM16/중복 출력 보호 PASS. **실제 사용자 PC WAV 보정·음질 검증 미실행**.
- [정본/기술 상세](../../TTS_Main20_Main21_Offline_DSP9_Pilot_20261020.md). 기존 VoiceFix9/GenderFix9 API 생성 패키지는 계속 사용 보류. 기존 Main18 승인, 나머지 정상음성, 원본 WAV, save/HOLD/lock, Unity 보호.


## 2026-10-20 최종 청취 피드백 — DSP 보정 전량 불합격 (최신 정본)

- 바울 대표의 직접 판정: **“다 못쓰겠다. 안바뀌고 외계인 목소리로 돼..”**. 세린·태온 gentle/medium/strong DSP 미리듣기 음성은 기존 캐릭터 음색을 복구하지 못하고 인공적 왜곡 발생. **DSP_PREVIEW_ALL_USER_FAIL / DSP_STOPPED**.
- 오프라인 가상음성의 테스트 PASS는 처리 오류 없음/원본 SHA 보존만 검증했으며 **청취 품질을 보증하지 못했음**. 기존 `LIMITLESS_TTS_DSP9_Offline.py preview/apply` 결과 공식 음성 사용 및 Unity Import 금지. 반복·보정 강도 증폭 금지.
- 원본 Main20/21 17 WAV, 이미 승인된 Main18 세린10 및 기존 정상25, 사용자 Save와 checkpoint/HOLD 불변. Gemini API를 이 세션에서 새로 호출하지 않음.
- 후속 방침: 기존 정상본과 불량9의 실제 요청 설정/모델/VoiceID/스타일/SDK/대사·WAV SHA를 우선 API0 비교. 일치하면 기존 Voice ID/원문 유지하고 **불량1개만 통제 재생성 후 청취**, 실패 시 추가 비용/쿼터 낭비 중단. 대안 프리셋 무단 변경 금지. 상태·근거는 [DSP9 청취 불합격 정본](../../TTS_Main20_Main21_Offline_DSP9_Pilot_20261020.md).


## 2026-10-20 바울 대표 “오늘 20회 써보자” — 직접 Python 배치 준비 (최신)

- 20회는 **키1 최대10, 키2 최대10** 사용자 희망 예산. 두 키가 독립 Google Project, 모델 gemini-3.8-flash-tts FreeTier 실제 사용 가능/무과금이 확인될 때만 해당 상한 사용. 동일 Project 키 중복합산 금지. 실제 API 호출0 (바울 대표 PC 실행 대기).
- 이전 **DSP 미리듣기 전량 FAIL** 유지. 새로운 Python은 원래 생성 당시 **동일 Schedar/Gacrux + 동일 Style + 동일 REST 요청 JSON + 동일 ExactText**로 제작; 프리셋 교체/피치 보정0. 9개 오류=세린8, 태온1. Main21 추가 미제작2=main21_haren_taeon_02, main21_prepare_serin_01. 하렌 자체 Voice미승인3 BLOCK.
- TTS 담당 직접 `LIMITLESS_TTS_Today20_20261020.zip` 제작, SHA256 `7e2c80cc26f6127dbc22075e975959afab4246aa66f57ef6696e2b7c747ae635`; Python+고정 대사CSV+README. 사용자 대화에서 다운로드 제공. 별도 원본 `Limitless_TTS_Main20_Main21_20261019` 전체 17 결과 SHA 보호; 새로운 결과 폴더 `Limitless_TTS_20Calls_20261020`.
- 안전 단계: `check` API0 → `sample --confirm-free` 세린1/태온1 (KEY1 2) → 둘 다 사용자 청취 PASS면 `approve-samples` → `main --confirm-free` 나머지9 (KEY1 8, KEY2 1) → 실제 청취 FAIL인 ID에 한해 `retry --ids ... --confirm-free` 최대9 (KEY2 9). **실제 불량이 없는 상태에서 예비9를 무조건 소진하지 않음.**
- 오프라인 모의: 17원본 SHA 감사, sample2→승인 게이트→main9→retry9 = 20/Key1 10/Key2 10 PASS. 두 번째 가짜 API 오류 시 HOLD+lock 유지·뒤호출0 PASS, WAV 변조 감지 PASS. **실제 Gemini 신규 호출/WAV0/Unity0**.
- [상세 계획/스크립트 메타](../../TTS_Main20_21_Today20_Staged_Retry_20261020.md). Codex는 여전히 사용자 최종 청취 승인 뒤 Unity 통합만 담당. TTS 업무 책임/실행은 TTS 전담 세션+바울 대표 로컬 Python. docs/tts 브랜치만 갱신, main 변경·Push/merge 없음.


## 2026-10-20 최신 실제 실행 로그 — 샘플 첫 API HTTP400 / HOLD 유지

- 바울 대표 PowerShell 화면: 이전20회 `check` PASS; `sample --confirm-free` KEY1 첫 `main20_confront_serin_01`/`Schedar`/`gemini-3.8-flash-tts` 호출 **HTTP400**, 성공 WAV0, 이후 태온/나머지 요청0. 즉시 `HOLD.json`+`RUNNING.lock` 유지, 중복 실행 차단 정상.
- 신규 Python 스크립트/이전17회 스크립트/Google 공식 Gemini 3.8 REST JSON 구조 일치함을 정적 확인했으나 기존 코드가 **HTTP400 상세 응답 본문 저장하지 않은 결함** 있어 원인은 미확정. Key/Voice/모델/컨텐츠 문제로 단정 금지.
- TTS 담당 직접 안전 진단 ZIP `LIMITLESS_TTS_HTTP400_Diagnose_20261020.zip` 작성·배포 (SHA256 `d1270b807435d8f222e5a976ba00b65c133ea5fa19fb800840b87c645296815a`). 기존 스크립트 옆에 추가하고 `inspect`(API0/쓰기0), `model-info`(Google 모델 GET1/TTS POST0) 순서로 실행, 결과 보고. `diagnose-one`의 단 1회 POST는 상세진단 필요 시 사용자 추가 확인 후에만. 기존 HOLD/lock은 절대 해제/초기화/덮어쓰기 금지.
- 가상 원본17 SHA 및 HOLD/lock 보호, 모델 GET, 오류 400 POST 상세 메시지 추출/키 마스킹, 추가 호출 차단 오프라인 테스트 PASS. **실제 신규 Google API/모델 GET은 이 ChatGPT에서 실행하지 않았으며 사용자 PC 진단 대기**. 첫 시도는 보수적으로 KEY1 소모1회로 기록. Unity0/main0.
- 상세 정본: [2026-10-20 첫 HTTP400 진단](../../TTS_Main20_21_Initial_HTTP400_Diagnostic_20261020.md). Issue #20도 BLOCKED로 갱신.


## 2026-10-20 실제 진단 결론 — KEY1 API 인증 거절 (최신)

- 사용자 진단 `inspect`: HOLD/lock·원본 SHA 정상, 첫 샘플 HTTP400, 신규 WAV0/후속 생성 호출0.
- 사용자 `model-info` 2회: 모두 HTTP400 `INVALID_ARGUMENT` / `API key not valid. Please pass a valid API key.`. 이는 TTS generateContent가 아니라 모델 메타 조회 GET이므로 **이 시점에 입력한 KEY1의 API 인증이 거절된 것이 직접 문제**. 첫 실패에 사용된 키와 두 진단 입력의 동일성은 화면만으로 별도 확인 필요.
- 다음 조치: Google AI Studio에서 해당 Google Project의 유효 API Key(프로젝트 ID와 다름)를 안전하게 재확인, 채팅·화면에 키 노출하지 않은 상태로 `model-info`만 다시 검사. PASS 전 샘플·진단 POST 추가 실행 금지. 기존 HOLD/lock 삭제·재설정 금지, 이전 Main20/21 WAV와 Main18 승인본 보존.
- [정본 상세](../../TTS_Main20_21_Initial_HTTP400_Diagnostic_20261020.md). 이 진단을 통해 새 TTS 호출0, 새 WAV0/Unity0. 기존 음색 오류9는 별도 미해결.


## 2026-10-20 최신 — 진단1회 생성 성공, 세린 남성 음색 청취 FAIL

- 바울 대표 화면으로 유효 KEY1의 단일 generateContent 요청 성공, WAV1개 별도 저장 확인. 대상 main20_confront_serin_01, Schedar, gemini-3.8-flash-tts. result.json 경로 F:\Downloads\Voice\Limitless_TTS_Today20_400Diagnostic_20261020\result.json. 추가 TTS 호출0, 기존 HOLD/lock 유지.
- 사용자 청취: “세린인데 남자..;;” → USER_LISTENING_FAIL. 생성 성공과 캐릭터 음색 합격 구별, 해당 WAV Unity 적용 금지. Main20/21 세린8/태온1 불합격 및 DSP 오프라인 보정 전량 FAIL 유지.
- 관측된 오늘 생성 POST 시도2(처음 HTTP400 1, 진단 성공1), 새 WAV 성공1. 실제 무료 quota 차감은 미확인. 원래 Voice 반복 재생성 및 20회 무조건 소진 중단.
- 다음은 정상 승인 세린 음성(Main18)과 요청 기록 비교, 음색 변경 대안 검토/사용자 승인 후 제한적 제작. 기존 승인 음성 및 모든 Save/WAV/HOLD 보호. 상세 문서 TTS_Main20_21_Initial_HTTP400_Diagnostic_20261020.md.


## 2026-10-20 최신 — 승인된 세린 WAV를 직접 참고하는 1개 Voice Conversion 시범

- 사용자: “내가 OK한 세린 파형으로 니가 바꿔볼래?” 기존 단순 DSP/피치·포먼트 방식은 사용자 청취 FAIL 유지. **사용자 승인 WAV를 참조하는 OpenVoice 기반 화자 음색 변환**을 새 방식으로 1개만 검증 준비.
- 기준 WAV: `F:\Downloads\Voice\Limitless_TTS_Serin10_Schedar_v2\sample_candidate\main18_ground_serin_01.wav`, 승인 SHA256 `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`. 대상: 2026-10-20 단발 성공·음색 FAIL `F:\Downloads\Voice\Limitless_TTS_Today20_400Diagnostic_20261020\main20_confront_serin_01_DIAGNOSTIC.wav`.
- TTS 담당이 `LIMITLESS_Serin_ApprovedVoice_VC_20261020.zip` 직접 제작·사용자 다운로드 제공 (SHA256 `77293681c01e82f7be00fb2029f73ca287c0522dadbb338c8086097c7f567f06`). Python + 한국어 README만 동봉, 제3자 Windows x64 CPU OpenVoice 런타임/131MB 모델은 `setup` 사용자가 명시적으로 승인 후 다운로드, SHA 검사.
- `check` API0/쓰기0 → `setup` 외부 OSS 다운로드(유료 Gemini API0) → `preview` 사용자 로컬 WAV 1개만 독립 `Limitless_TTS_Serin_ReferenceVC_20261020` 폴더에 변환해 청취 대기. 원본 승인·오류 WAV, 그 외17/25, 모든 HOLD/lock·Unity·Save 불변. 이전 Main18 승인 보이스 레지스트리 변경0.
- 스크립트 compile/가짜 WAV 입력·승인 참조 SHA·윈도우 엔진 명령·중복 결과 방지·해시 QA mock PASS, **실제 F: 사용자 WAV, OpenVoice 모델 실행 및 음성 청취 아직 미검증**. 이 방식도 완벽한 원래 음색 복원 보장 없음. 결과 청취 FAIL시 추가7개 연속변환 금지.
- [GitHub 상세 정본](../../TTS_Serin_ApprovedReference_VoiceConversion_Pilot_20261020.md). TTS Python 작성은 TTS 담당, Unity 통합만 Codex. docs/tts만 기록, main 수정0.


## 2026-10-20 최신 — 바울 대표 요청에 따라 실제 세린 WAV 8개 직접 변환해 ZIP 전달

- 사용자 직접 첨부 `candidates.zip` 17 WAV 실제 바이트 확인. ChatGPT 파일 보관함에서 과거 바울 대표 승인 `main18_ground_serin_01.wav`(SHA `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`) 및 `LIMITLESS_Serin6_ConsistentVoice_Previews.zip`(SHA `a5878e6d4f1b1e858f4b505f23669ab147da2dd466fe30443b0d6116a7246cb1`) 확보·해시 확인. 이전에 파일 접근 불가하던 상태 해소.
- TTS 담당이 실제 사용자 오류 세린8에 대해 **A_natural/B_closer 두 버전, 16개 실 WAV**를 작업 컨테이너에서 직접 작성하고 사용자에게 `LIMITLESS_Serin8_ReferenceGuided_WAV_Previews_20261020.zip` 전달. ZIP SHA256 `349d6bf7047a92d04972f68c769595969b3ff69d8b6e03f1ea8bfd7179ea535e`, 크기4,419,922 bytes.
- 공식 승인 WAV7의 F0/평균 스펙트럼을 참조한 Praat PSOLA 피치/포먼트 + 제한적 참조 EQ 방식. **AI OpenVoice 사전학습모델을 다운로드/실행하지 못하므로 ML 목소리 복제라고 주장하지 않음**. 과거 DSP6 사용자 FAIL과 구별되지만 자연스러움 사용자 평가 **대기**, 두 샘플 모두 불합격이면 16개 전체 공식 불합격 처리/Unity 미적용.
- WAV 16개 24kHz mono PCM16·길이·SHA·리드디코딩 PASS; 업로드 원본/승인 기준7 SHA 불변. API0/Unity0. 태온 목소리는 다른 참조가 필요하여 제외.
- 세린 `main20_confront_serin_01` A와 B 샘플 WAV도 ZIP 밖 직접 공유. [상세 정본](../../TTS_Serin8_ReferenceGuided_Offline_Prevs_20261020.md). 브랜치 docs/tts만 기록, main 변경/Push/merge0.


## 2026-10-20 최신 — 승인참조 세린 A/B 16개 사용자 청취 전량 FAIL

- 사용자 직접 피드백 **“다 남자다..;;;;”**. 실자료 기반 reference-guided PSOLA+EQ A/B 16개 전체 **USER_QA_FAIL / DO_NOT_USE / UNITY_APPLY_FORBIDDEN**. 이전 단순 DSP 실패와 마찬가지로 여성 캐릭터 음색 복구 실패.
- 피치/포먼트/평균스펙트럼 참조 보정은 화자 음색 변환이 아니라 음향 통계 보정이며 결과의 남성 지각을 제거하지 못함. 본 계열의 추가 보정·강도 증가 중단. WAV 포맷 검사 PASS≠청취 PASS.
- 향후 사용자가 원하면 실제 화자 임베딩 기반 신경망 Voice Conversion 모델을 **승인 Main18 세린 기준 WAV로 1개 시범** 사용하거나, 신규 여성 보이스 한 개를 실제 청취해 정식 선정. 모델/변환의 성공은 미보장, 사용자 승인 전 기존 공식 보이스 설정·Main18 승인10 및 Unity 모두 불변. 태온1 음색 문제는 별도.
- 상세 결과: [세린 16개 직접 보정 전량 불합격](../../TTS_Serin8_ReferenceGuided_Offline_Prevs_20261020.md). 이번 업데이트 Github docs/tts 문서만, 음성 API0/Unity0.


## 2026-10-20 최신 — 세린 기존 Schedar 안전 재시험 Python 준비

- 바울 대표 “18회 버리더라도 또 뽑아봐?” 지시. TTS 담당은 18회 무조건 소진하지 않고 원래 `Schedar`/원래 스타일/원문과 같은 설정으로 **1회 시험 후 청취 게이트**, 최대2회 수동 재시험을 준비.
- 다운로드 `LIMITLESS_TTS_Serin_Retry2_20261020.zip` SHA256 `6369b14fe7032046111f8fe4f4296e0a6cadf8633299bc579d128bf22fee4180` (279734 bytes), 스크립트+검증된 17행 CSV+승인 Main18 세린 WAV 실파일+한글 README. 기준 WAV SHA `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`.
- 사용자 로컬에서 `check` API0→`trial --attempt 1`(정확히 1회, 모델 GET 먼저, 유료/잔여 수동 확인) 실행. 청취 불합격일 때만 `trial --attempt 2`와 `FIRST_FAIL` 별도 게이트. 기존 HOLD/lock 및 모든 원본 변경 금지, 새 출력 폴더 격리.
- 모의 성공1/2, 중복 번호 차단, 실패 HOLD/lock 및 다음호출0 PASS. **실제 Google API 호출0/실제 음색 미검증**. [상세](../../TTS_Serin_SameVoice_Retry2_20261020.md). TTS 제작 담당 유지, Codex는 최종 사용자 승인 WAV Unity 통합만.


## 2026-10-20 최신 사용자 가설 검증 — 세린/태온 기존 VoiceID의 공식 성별이 역방향 (우선순위 높음)

- 바울 대표: “남잔데? 혹시. 우리가 뽑았던게 오류고 오류수정이 남자 아냐?” 이전 승인 세린 WAV는 여성처럼 자연스러웠으나, 이후 동일 `Schedar`를 사용한 Main20/21 생성/재시도는 계속 남성으로 들린다고 확인. **이전 음성이 예외적인 TTS 출력이었고 현재가 원래 음색으로 돌아온 것**이라는 설명이 충분히 가능. 단, Google의 특정 버그 수정·배포 시점은 직접 확인하지 못했으므로 확정 금지.
- Google 공식 Gemini-TTS/Cloud docs의 **프리셋 성별**: `Schedar = Male`, `Gacrux = Female`. 현재 세린 `Schedar`와 태온 `Gacrux`는 의도한 캐릭터 성별과 **공식 프로필이 반대**다.
  - 출처 https://docs.cloud.google.com/text-to-speech/docs/gemini-tts
  - 출처 https://firebase.google.com/docs/ai-logic/generate-speech?api=vertex
- 과거 담당이 `Schedar`를 “세린 공식”이라 고집하며 세린 8개 재생성·DSP/PSOLA 등 시도를 반복한 것은 **사용자 승인 기준을 VoiceID와 혼동한 오류**였다. 사용자가 승인한 것은 *들린 실제 목소리*였으며 그 정체성을 우선해야 함.
- **새 작업 원칙**: Main18 세린 기존 사용자 승인 WAV10과 기존 정상 태온 오디오는 변경/재생성 금지. 신규 세린 1대사에는 공식 여성 프리셋 중 중복 캐릭터 충돌 없는 후보(예: `Erinome` Female/Clear)를 **1회 시험**하고 과거 승인 샘플과 귀로 비교. 태온은 공식 남성 보이스 후보(예: `Alnilam` Male/Firm) 1회 시험하여 과거 정상 태온 샘플과 비교. 둘 모두 “후보”이지 사용자 청취 승인 전 전역 캐릭터 VoiceID 변경 아님. 이 기능은 앞선 GenderFix9 스크립트를 바로 재사용하라는 지시가 아니고 원본/쿼터 및 요청설정 검증 후 안전 실행 방식 결정 필요.
- '그동안 승인 여성 음색이 원래 버그였고 현재 수정됐다'는 인과는 미검증이며, 실제 요청 wire 파라미터/날짜/모델 업데이트 자료 부재. 더 이상 `Schedar` 동일 설정으로 무의미한 18회 반복 생성하지 말 것. 남은 무료 호출을 보호.
- 이번 확인은 공식 공개 목록을 읽은 **진단/문서화만**; TTS API 추가 호출0, Unity 0, 정상 WAV 수정0. Main20/21 세린8 + 태온1 사용자 불합격 상태 유지.


## 2026-10-20 전역 품질 방향 전환 — 전체 TTS 일관성·전면 재제작 사전판정(최신)

- 바울 대표 강한 문제 제기: “지금 그게 문제가 아니고 이 가설이 맞으면 게임에서 싹 다 거내고 처음부터 다시 해야 한다. 일관성 때문에..”. 세린 8개를 고치는 문제가 아니라 **실제로 게임에 들어가 있는 모든 대사 음성의 캐릭터별/챕터별 동일성 문제**. 세린/태온 전역 Voice ID만 바꿔 새 챕터를 제작하면 이전 챕터들과 음색이 달라질 위험이 큼.
- 지금은 **전체 TTS 생성 및 신규 Unity 반영 HOLD, 원본 삭제/기존 Catalog 변경 금지**. 사용자 발언은 “가설이 맞으면”이라는 조건부 판단으로 취급, 무조건 WAV 삭제나 전면 재제작에 관한 실행 승인은 아님.
- `CURRENT_STATUS.md` 2026-10-19 기준 Main18 승인10 적용 후 **StoryVoiceCatalog280→290** / Intro Narrator18 별도 / Main20·21 22음성 미등록. 과거 Main01~12 102, Main13~15 41, Main18·19 정상25, Main17 14, Main08 Taeon5 등 후속 적용. 현행 290개의 실제 캐릭터/챕터/미묘한 청취 합격은 로컬 전수 실측 필요.
- 공식 성별 매핑: Schedar=Male(세린 불일치), Gacrux=Female(태온 불일치), Achird=Male(폴), Sulafat=Female(미엘), Orus=Male(레온). 실제 WAV 청취 품질은 프리셋 메타만으로 결정하지 않는다. 구세린이 버그로 여성 출력되었는지/고쳐진 시각은 증거 없음.
- **중요 Google Voice Replication 제약:** 2026-10-12 공식 문서상 동일 실제 성인 인간 화자의 reference audio 10–30초 및 직접 낭독한 consent audio 필수. 기존 합성 세린 WAV만으로 복제를 바로 수행할 수 없음. Voice Replication을 합성 샘플 복구 정답으로 제시하지 않는다. https://ai.google.dev/gemini-api/docs/voice-replication
- 전체 기술/운영 및 코덱스 READ-ONLY 인벤토리 지시 정본: [TTS 전체 캐스팅 일관성 감사/재제작 승인게이트](../../TTS_전체_캐스팅_일관성_재검토_및_전면재제작_판정게이트_20261020.md). GitHub **Issue #39** (read-only local Unity 290 카탈로그 + Intro18 전수 감사, 실행 승인은 아직 없음).
- 최종 사용자 선택은 전체 290+성우 라인 등 **전면 Voice Registry v2 리캐스팅** 대 캐릭터별 전 챕터 재더빙 범위. 어느 쪽도 기존 승인본/원본/Save/HOLD를 지우지 않고 분리 제작/검증 후 Unity 연결만 교체. 사용자 승인 전 구현/TTS 호출/비용 지출0.


## 2026-10-20 최신 사용자 지시 — 전체 TTS 폐기 아님, 세린·태온 2인 캐스팅만

- 바울 대표: **“그러지 말고, 태온 세린만 새로 캐스팅 해봐. 인물 성격과 특징에 맞게.”** 전역 TTS 삭제·재녹음 추진 STOP, [전수감사 Issue39](https://github.com/HappyPaul812/Project-Limitless/issues/39)은 **범위 변경으로 closed/not_planned**, 기존 Issue20에서 2인 캐스팅 진행.
- 공식 캐릭터 정본 대조: 세린 여성 약25세·청각의 길 사수·침착/날카로운 관찰·짧고 빠른 판단/가까워지면 건조한 유머. **공식 Female Clear Erinome 1순위**, Female Smooth Despina 차선. 태온 남성·지적의 길 수호자·관찰→이해→보호·차분하고 성숙·형과 가족식 반말/일반 존댓말. **공식 Male Smooth Algieba 1순위**, Male Firm Alnilam 차선.
- [캐스팅 1차안/스타일/샘플 원문/승인게이트](../../TTS_세린_태온_리캐스팅_1차안_20261020.md). 공식 Gender https://docs.cloud.google.com/text-to-speech/docs/gemini-tts, Voice traits https://ai.google.dev/gemini-api/docs/speech-generation. 확정이 아니라 **실제 사용자 청취 전 제안**.
- 시험은 Main20 세린 `main20_confront_serin_01`과 Main21 태온 `main21_route_taeon_01`의 **최우선 후보 1대사씩 최대2회**부터, Google 프로젝트·모델 무료 잔여량/요금 확인 후만 제작. 후보 WAV 청취 승인 전 전역 Voice Registry나 Unity 변경 금지; 보조후보 무단 일괄 시험 금지. 사용자가 원하는 일관성 때문에 최종 2인 VoiceID가 승인되면 두 인물의 **전 챕터** 재제작 범위를 별도 설계하며 폴·미엘·레온·내레이터를 건드리지 않음.
- **이번 턴: 문서화/후보 선정만, 실제 TTS API0·신규 WAV0·Unity0·삭제0**. 기존 Main18 사용자 승인 세린10/Main08 태온 정상 WAV/모든 기존 입력·출력 WAV·HOLD·Save 보호. TTS 담당 스크립트 제작, Codex는 최종 승인 WAV의 Unity 연결만 수행.


## 2026-10-20 최신 — 하렌 추가, 3인 대사량 실제 Unity/Manifest 집계 및 소량 우선순위

- 사용자: “이왕 캐스팅 하는거 하렌도 하고. 또 태온 세린의 대사량 알려줘. 적은 개수 먼저 바꾸게.”
- GitHub main `Unity/Client/Assets/_Project/Resources/Audio/Voice/Story/StoryVoiceCatalog.asset` **290개 엔트리 전체 파싱/ID 중복0**, Main20 CSV/Main21 JSON VoiceExpected 대조.
- **하렌 0 Unity+3 Main21=총3개**, **세린58 Unity+5 Main20+4 Main21=총67개**, **태온82 Unity+3 Main21=총85개**. 합계 현재 등록140+미등록15=155개 재제작/신규 작업 대상. Chapter3 미확정 대사는 제외, 일부 Main20/21 WAV 후보는 이미 만들었으나 새 캐스팅 기준에서 재제작 대상. 현재 적용된 WAV 숫자와 전 챕터의 향후 확정 대사량은 별개.
- **추천 작업 순서: 하렌3 → 세린67 → 태온85**. 먼저 새 Voice ID의 첫 샘플 한 대사 사용자 청취 확인, 후속 대사 수동 진행, 한 캐릭터의 과거/현재 목소리가 Unity 안에서 섞이지 않도록 최종 승인 후 별도 Codex 교체.
- 하렌 캐스팅 1순위 `Alnilam` (Male/Firm), 차선 `Algenib` (Male/Gravelly). 태온 친형·시각의 길 투사·무뚝뚝하고 보호적·깊은 정, 레온 Orus 등 기존 인물과 중복 청취 확인. Main21 정확 원문 3개: `main21_haren_haren_01`/02/03. 미래 Main29 “잘.했.어.”는 확정 Dialogue ID가 아니므로 현재 3개에 미포함·선제녹음 금지.
- 세린 1순위 `Erinome` Female/Clear, 차선 `Despina` Female/Smooth. 태온 1순위 `Algieba` Male/Smooth, 차선 `Iapetus` Male/Clear (하렌 Alnilam과 VoiceID 중복 방지). 모두 실제 한국어 음성 청취 전 **후보/미승인**.
- 정본: [TTS 세린·태온·하렌 3인 캐스팅 및 대사집계](../../TTS_세린_태온_하렌_캐스팅_대사집계_20261020.md). GitHub [Issue20](https://github.com/HappyPaul812/Project-Limitless/issues/20). 실제 신규 API 호출0/WAV0/Unity0, 모든 원본/HOLD/Save/기존 승인 음성 보호.

## 2026-10-20 최신 — 남은 17회 중 3인 한국어 샘플 3회 준비, F:\Downloads\Renewal_Voices 지정

- 바울 대표: “17개 남았으니까. 해보자. 한국어 샘플 먼저.. 작업 폴더 F:\Downloads\Renewal_Voices”. 실제 무료 잔여17회는 사용자 보고이며 Google Project+Model 공식 잔량/과금 무보증.
- **TTS 담당이 직접 새 Python ZIP 제작 완료**: `LIMITLESS_Renewal_Voices_Korean_Audition_3Samples_20261020.zip` SHA256 `8649f90cffe73baeed22e52390666833f2a263f1c4821b525cfa7ba15a0ecd10` (8040 bytes). `Renewal_Voices_Korean_Audition.py`(13504bytes)+`KOREAN_AUDITION_MANIFEST.json`+한글 README. 고정 폴더 `F:\Downloads\Renewal_Voices`, 결과 `samples`.
- **하렌** Alnilam/`main21_haren_haren_01` / “태온. 너 아직 멀었어.” → **세린** Erinome/`main20_confront_serin_01` / 지면 맥동 두 문장 → **태온** Algieba/`main21_route_taeon_01` / 흔들림 재조사 한 문장. 정확 기존 Manifest·Stable ID 사용.
- `conda activate py3_12`; `cd F:\Downloads\Renewal_Voices`; `python .\Renewal_Voices_Korean_Audition.py check`; `python .\Renewal_Voices_Korean_Audition.py sample`. 최대 3 POST, 성공 시 예상 잔여14(기타 호출/결과에 따라 다를 수 있음), 28초 간격, 누락/중복 저장 차단, 오류 시 HOLD/lock 및 호출중단, 자동 retry0. 개별 `sample --speaker haren|serin|taeon`도 가능.
- 실제 사용자 Google 프로젝트/모델의 무료 잔여 및 무과금 여부를 FREE로 확인, 별도 YES3 수동 승인. 키는 가림 입력·비밀 저장 금지. 모델 GET 성공은 실제 무료 quota 검증 아님.
- 자체 compile/ZIP 압축해제 check/mock GET+POST3/HOLD·중복보호 PASS. **실제 Gemini API 호출0/새 한국어 WAV0/Unity0** (로컬 사용자가 실행하기 전). 샘플 생성 후 **바울 대표 청취 승인 필요**, 합격 전 기존 StoryVoiceCatalog/WAV/GUID/Save 변경금지.
- [이번 3개 한국어 오디션 실행 정본](../../TTS_RenewalVoices_3인_한국어오디션_실행계획_20261020.md); [GitHub Issue #20](https://github.com/HappyPaul812/Project-Limitless/issues/20).


---

## EOD 2026-10-24: previous main history retained

## 2026-10-22 최신 사용자 결정 — Main21 기존 정상 후보 WAV 7개 KEEP / 재생성 금지 / 후일 Unity 일괄 통합

- 사용자: `F:\Downloads\Voice\Limitless_TTS_Main20_Main21_20261019\candidates`에서 **오류 후보들은 직접 정리**했고, 남은 WAV **7개만 보존**한다고 보고. **다시 TTS API로 생성하지 말 것**, 추후 다른 완성 성우 WAV와 **한 번에 Unity에 추가**. 지금 게임 반영 금지.
- **[7개 WAV 보존/후일 통합 지시 정본](../../TTS_Main21_기존보존_7WAV_통합대기_20261022.md)**. 사용자 제공 7 ID 모두 실제 GitHub `main` `Main21_TTS_Manifest_20261010.json`에서 정확 대사/화자/VoiceExpected=true 대조 완료:
  - **레온 Orus 2개:** `main21_carriage_leon_01.wav`, `main21_arbel_leon_01.wav`
  - **미엘 Sulafat 2개:** `main21_aid_miel_02.wav`, `main21_aid_miel_01.wav`
  - **폴 Achird 3개:** `main21_aid_paul_01.wav`, `main21_route_paul_02.wav`, `main21_route_paul_01.wav`
- **상태 분리:** 해당 7개는 사용자 `KEEP` 지정, `TTS_NOT_REQUIRED`, `UNITY_PENDING`. API 다시 사용 금지. 실제 로컬 WAV 바이트/SHA/발화/PCM/사용자 청취 PASS는 ChatGPT에서 F: 직접 확인하지 못했으므로 **최종 통합 전 Codex가 검증**. 기존 WAV 삭제·이동·덮어쓰기 금지. TTS 자동 생성기에서도 대상 제외.
- 현재 다른 제작현황(최근 사용자 로그): 세린 **67/67 READY**, 태온 **37/85 READY**(48 미제작), 하렌3은 별도 제작 대기/원래 `main21_haren_haren_01`의 평서문 하강 억양 주의. 이는 별개 제작 영역. **모든 음성이 준비되고 사용자 일괄 통합 승인 시에만** Codex가 Main20·21 전수 목록/기존 7 SHA 감사 후 하나의 구현 배치로 Unity 연결·검증. `main` 직접 커밋 금지.
- 이번 턴 문서 기록만: Google API0/사용자 로컬 F: 변경0/Unity0. 7개 외에 과거 잘못된 후보가 삭제됐다는 내용은 **사용자 보고이지 직접 파일시스템 감사 결과가 아님**.

## 2026-10-22 실제 사용자 콘솔 최종 상태 — 세린67/67 완료, 태온37/85, PT 40/40 안전 종료

**최신 실행 증거 우선:** 2026-10-22 사용자 `Renewal_Auto_TTS_Failover_v2.py` 최종 출력 `[DAILY CAP] 2026-10-22 No more calls this PT day.`
- `[STATUS] PT=2026-10-22 (UTC-7) / attempted=40/40 / remaining=0`.
- `[KEYS] KEY1=10, KEY2=10, KEY3=10, KEY4=10` / 로컬 그룹 `PROJECT_1~4` 각각10. 이는 **로컬 사용량**이지 Google의 실시간 무료 quota 또는 프로젝트 소속 증거가 아님.
- `[READY] Serin=67/67, Taeon=37/85, total=104/152` = 전체 **68.4%**. 전날 64/152에서 파일 준비 수 **+40**. 남은 제작 **태온48개**이며 세린은 추가 TTS 생성 대상0. READY는 저장된 PCM/메타 기준이며 **전량 사용자 청취 승인 아님**.
- `HOLD=False`, `[429 QUARANTINE] []`: 에러 잠금이나 격리 없이 **정상 일일 안전상한 정지**. `RUNNING.lock` 복구나 `RESUME429`/`SWITCH429` 수동 복구 **불필요**. 기존 104 WAV/원문·SHA·사용량 원장을 보존한다.
- 작업 예산은 *오늘 로컬40회 전부 사용*. Google 서버의 실제 프로젝트별 quota와 과금상태는 별도 확인 필요. 하렌3은 여전히 이 자동화 제외, 제작 보류.
- **다음 실행 계획:** 2026-10-23 **00:00 PDT = 한국 시각 16:00** 이후 제공자 일일 리셋 및 실제 프로젝트/모델 무료 사용가능량 확인 → `cd F:\Downloads\Renewal_Voices` → `python .\Renewal_Auto_TTS_Failover_v2.py check` → `python .\Renewal_Auto_TTS_Failover_v2.py run`. 세린 완료본 skip 후 태온 다음 미제작부터 이어감. 예상대로 하루40이 허용된다면 10/23 +40, 10/24 +8회로 끝나지만 **Google 실제 성공/제작량은 보장 불가**.
- 완료 뒤 104개를 즉시 Unity에 넣지 않는다. 태온 나머지 전량 제조 완료 + 세린/태온 전 챕터 발음·인물성·감정/음색 청취 QA + 사용자 승인 후에만 Codex Unity 무손실 통합. `docs/tts` 문서만 기록, main 직접 수정/Push/merge 금지.

## HISTORY — 기존 main·담당 고유 기록 보존

# LIMITLESS / TTS 담당 최신 인계 — 2026-10-20

## 2026-10-21 공통 로직 정본 — 앞으로 다른 모든 TTS에 재사용

- 사용자: “고맙고 이 로직은 문서화해놔, 다른 TTS에도 활용하게..”. 대응해 **[TTS 공통 다중 프로젝트 Quota·429 Failover·체크포인트 운영 표준](../../TTS_공통_다중프로젝트_할당량_429자동전환_체크포인트_운영표준_20261021.md)** 신규 작성. 새 캐릭터·새 챕터·다른 Gemini TTS 작업 시작 전 필독.
- 공통 핵심: 정확 대사 Stable ID/원문·화자·Voice/Style 동결 → 사용자 한국어 샘플 청취 승인 → 키→실제 Project ID와 Project/Model quota·Billing 확인 → POST 전에 ATTEMPT 선기록 → 성공 WAV/JSON SHA·원문/음색 QA → 429 프로젝트 단위 태평양 일자 격리 및 **검증된 다른 독립 Project에만** 자동 전환 → CAP 정상중단(잠금 없음) / 결과불명 HOLD+LOCK 및 증거 백업·수동 복구 → PT 다음날 당일 카운트만 새 계산하고 구 데이터·WAV 보존 → 사용자 음성 QA 후 Codex Unity 적용.
- 실사용 검증 10/21: KEY3 HTTP429에서 KEY4로 전환해 세린 Main20 성공, **39/40 정상 안전 종료/HOLD=False, READY 세린63/67·태온1/85**. 40/10은 당일 배치의 로컬 안전 수치, 외부 Google 실제 quota 보장·다른 TTS 기본값이 아님.
- **중요 코드 경계:** 현재 `Renewal_Auto_TTS_Failover_v2.py`는 세린/태온 152개 전용이고, 문서는 다른 담당이 따라야 할 **범용 운영 규약/개념 설계**이지 이미 재사용 가능한 공용 실행기가 테스트 완료됐다는 주장이 아님. 이후 실제 다른 캐릭터에 적용할 때 manifest·폴더·Voice 선택기·공유 프로젝트 장부 분리/재통합 구현·오프라인 검증 필요. 하렌 억양 수정 보류는 변함없음.
- 본 문서 작업으로 Google 생성 POST0, 로컬 사용자 F 드라이브·WAV/Unity 변경0, 브랜치 `docs/tts` 문서만 수정. 후임 담당은 기획·TTS·Codex 역할을 준수.

## 2026-10-21 최종 사용자 화면 — 429 프로젝트 격리 후 일일 안전 상한 정상 종료

- **사용자 실제 출력:** `[GROUP/KEY CAP] All usable project groups at configured per-day safety caps.`; `PT=2026-10-21 UTC-7 attempted=39/40 remaining=1`; `HOLD=False`; `429 QUARANTINE=['PROJECT_3']`.
- **누적 준비 파일:** 세린 **63/67**, 태온 **1/85**, 총 **64/152**. 따라서 아직 미제작은 **세린4 + 태온84 = 88개**. READY는 저장·기술 체크된 WAV 수이며 사용자 최종 청취로 64개 모두 합격했다는 뜻은 아님.
- **키별 오늘 로컬 요청:** KEY1=10 / KEY2=10 / KEY3=9(앞선 HTTP429로 프로젝트 그룹3 격리) / KEY4=10. 남은 로컬 전체예산1회는 전환 가능한 안전 프로젝트 슬롯 부재로 **사용하지 않았음**. 이는 **새 실패가 아닌 정상 안전상한 종료**, HOLD 및 RUNNING.lock 신규 발생 없음. 사용자가 보여준 화면을 정본으로 하되 외부 Google 사용량은 미포함.
- 기존 문서의 **KEY3 429 당시 HOLD 상태는 최신 상태가 아님**. v2 failover 및 수동 switch의 복구·격리 이후 실제 API 성공이 있었고 오늘 사용가능 프로젝트 전부 로컬 cap 도달.
- **다음 조치:** 2026-10-22 미국 태평양 자정 이후(한국 시각 10월22일 오후4시 PDT 기준) 실제 Google 프로젝트/모델 무료 quota와 과금 상태 확인. 기존 `HOLD=False`라 **RESUME429나 SWITCH429 재실행 불필요**. `python .\Renewal_Auto_TTS_Failover_v2.py check` → `python .\Renewal_Auto_TTS_Failover_v2.py run`으로 세린 마지막4개부터 이어감. 로그·기존 WAV·샘플·API.txt·Unity 그대로 보존. 오늘 추가 Google 시도할 필요 없음.
- 프로젝트별 할당량은 여전히 **키 수와 다르며 실제 Google 프로젝트 ID 검증 중요**. PT 날짜 변경으로 로컬 카운터·그날의 격리는 재계산되지만, 원격 Google 실제 quota 허용을 보장하지 않음.

## 2026-10-21 최신 — KEY3 HTTP429 후 타 독립 프로젝트 자동 전환 v2 제작 / 사용 전 프로젝트 실소속 확인

- 사용자: “저기 한 API가 막히면 다른 API가 하게 못해?” **가능하나 독립 Google 프로젝트로 실제 확인된 경우에만**. 기존처럼 같은 프로젝트 내 KEY를 바꿔 쿼터를 우회하는 방식은 허용하지 않음. Google은 RPD/RPM/TPM을 API Key가 아니라 **Google Project** 단위로 산정 (https://ai.google.dev/gemini-api/docs/rate-limits).
- 사용자 마지막 증거 (10/21): **세린59/67·태온1/85**, READY60/152. PT 10/21 요청 **35/40**, 성공34/실패1, KEY1=9/KEY2=9/KEY3=9/KEY4=8. `main20_confront_serin_02`에서 **KEY3 / HTTP429 / HOLD+LOCK**. 원인 RPD인지 RPM/TPM인지는 미확정. `PROJECT_1...4`는 표시 라벨일 뿐 실제 4개 다른 프로젝트라는 공식 검증 아님.
- 직접 만든 **`LIMITLESS_Renewal_Voices_HTTP429_ProjectFailover_v2_20261021.zip`** 22109bytes SHA256 `cd0a063d5c6d2b7463ab37363fbe2c9c9de45ab9ae257899f55347d231b7d643` 제공. 기존 `Renewal_Auto_TTS.py`와 `API.txt`, WAV/설정/로그 덮어쓰기 **없음**. 신규 분리된 `Renewal_Auto_TTS_Failover_v2.py`, `Configure_Failover_v2.py`, `Renewal_429_SameDay_Switch_v2.py`, README, offline tests 2개.
- 사용자가 https://aistudio.google.com/api-keys 에서 실제 Key→Project ID/Model 무료 잔량/과금 확인 후 그룹을 실소속대로 표시, `python .\Configure_Failover_v2.py`에서 `VERIFIED_PROJECTS` 승인하면 원 `RENEWAL_SETTINGS.json` SHA 백업 및 두 설정(`auto_failover_on_429:true`와 `verified_independent_projects_for_failover:true`) 활성화. 실제 공식 프로젝트 확인이 없으면 **동작 활성화 금지**.
- 기존 **같은날** KEY3 429 HOLD/LOCK은 `python .\Renewal_429_SameDay_Switch_v2.py inspect`(API0) 후 조건 PASS면 `... switch` `SWITCH429` 사용자 직접 승인이 있어야 SHA 백업·프로젝트3 PT 당일 격리 이벤트 append·잠금 해제. 사용자 승인 없는 자동 해제·잠금 삭제 불가.
- 이후 **새 runner만** `python .\Renewal_Auto_TTS_Failover_v2.py check` → `... run --max-calls 1` (1건 실제 청취/사용량 검수) → `... run`. 429 시 해당 그룹 격리, 다른 독립 그룹 남은 로컬 예산으로 같은 미완료 DialogueID 시도. 이전 실패는 ATTEMPT1·FAIL, 그룹 PROJECT_QUARANTINE 기록, 성공 WAV/원본 ID·SHA 유지. 다른 프로젝트 없으면 기존 HOLD. HTTP400/500/타임아웃도 기존 HOLD+LOCK. 40회는 로컬 한도이지 Google 쿼터 보장이 아님.
- v2 원본 코드 사용한 **오프라인 테스트 총9개 PASS**, 모의 Google 429+다른 그룹 성공, same-day 엄격한 증거·잠금 보관·승인/계수, 다른 그룹 없거나 미활성 기존 HOLD, 152 원문 검사, WAV 24k/mono/PCM16. 실제 Gemini API POST0, 사용자 F 드라이브 조작0, Unity0.
- [정식 신기능 Runbook](../../TTS_RenewalVoices_HTTP429_ProjectAwareFailover_v2_20261021.md) / [Issue #20](https://github.com/HappyPaul812/Project-Limitless/issues/20). 하렌은 계속 본작업 제외. 완료 보고에 브랜치 docs/tts/커밋 명시.

## 2026-10-20 최신 — Serin Main16 자동생성 중 HTTP429 / HOLD 잠금 유지 / 다음 PT날짜 수동 안전 재개

- 사용자 실제 에러: `[STOP] main16_canyon_default_serin_01 failed (HTTP_429). HOLD+RUNNING.lock preserved; NO AUTO RETRY.`. 프로그램 안전장치가 의도대로 429에서 정지함. 해당 사용자 로컬 시점 누적 성공건·Google 원격 잔여량은 이 1줄만으로 미확인.
- Google 공식 정책: 프로젝트당 RPM/TPM/RPD 제한, RPD 태평양 자정 리셋. **HTTP429 정확 원인은 서버 오류 본문이 원 스크립트에서 남지 않아 불명**. 34회는 자체 로컬 예산이지 실제 쿼터 아님. 4키의 SEPARATE 설정도 실제 4프로젝트 소속 검증 결과 미제출.
- 지금 즉시 사용 가능한 호출0 명령: `python .\Renewal_Auto_TTS.py check`, `Get-Content .\_progress\HOLD.json` (비밀 API.txt 업로드 금지). HOLD/lock 및 실제 로그 수동 삭제·강제리셋 금지.
- **[HTTP429 안전 복구 정본](../../TTS_RenewalVoices_HTTP429_HOLD_ManualRecovery_20261020.md)** 기록, `LIMITLESS_Renewal_429_Safe_Resume_20261020.zip` 5176bytes, SHA256 `3aa70d65e67a4fe0b74d3039ee6fe16729ff06b59d7c3163ad30e389096af0fb` 사용자 전달. 안의 `Renewal_429_Safe_Resume.py`는 기존 원본 스크립트/키/WAV를 건드리지 않고 inspect와 next-Pacific-day resume만 지원.
- 설치후 `python .\Renewal_429_Safe_Resume.py inspect` (당장 가능, API0). **최소 다음 PT 날짜 + Google 실제 무료 쿼터/과금 확인 + 사용자 `RESUME429` 직접 입력** 이후에만 `... resume` 실행. 본문 원문/Voice/ID, journal ATTEMPT/FAIL/HOLD/LOCK 일치, 기존 WAV SHA 검증→`_progress/recovery_snapshots/`로 원본 429 증거 SHA 보관 뒤 lock 해제. 현재 날짜/에러 불일치/partial wav면 차단.
- 잠금 안전 해제 뒤에도 `python .\Renewal_Auto_TTS.py run --max-calls 1` 먼저; 사용자 청취 및 Google 할당량 확인 후 전체 본생산. 재차429면 HOLD 재발, 다음 안내 전 무작정 리셋 금지. 하렌 제외 규칙 유지.
- 복구도구 직접 제작·Python compile/오프라인5유닛테스트/ZIP CRC PASS, **현재 Gemini live API 호출0/신규 WAV0/Unity0** (사용자 실제 PC에서 실행 필요). `watch`가 Google429 자동 우회하도록 바꾸지 않음.

## 2026-10-20 최신 — 세린67·태온85 전용 4-Key 자동 생성기 배포(오늘 로컬 예산34회)

**이 단락이 과거 세린/태온/하렌 3인 1개 샘플 실험 안내와 하렌 수정 R1 즉시실행 지시보다 우선한다.**

- 바울 대표 최신 지시: `F:\Downloads\Renewal_Voices`에 **Serin/** 및 **Taeon/** 두 폴더, 하렌 제외. 사용자 로컬 `API.txt`에 4키를 세미콜론(;) 및 줄바꿈으로 구분; 세린·태온 **전 확정 대사**를 일괄 생성·대사별 WAV/ID·키 슬롯/사용량 로그·일별 카운트 기록. 오늘 남은 횟수 34회 사용자 보고.
- 실제 GitHub main catalog290과 Main07/17~21 및 Story audit 연결하여 **세린67(이미 Unity 등록58+미등록9), 태온85(등록82+미등록3) = 정확 ID152**를 [GitHub JSON 정본](../../TTS_Renewal_Voices_152_OriginalDialogue_Manifest_20261020.json)에 별도 동결. 기록 커밋 `9183376e`, 대사 ID 누락/중복0. Chapter3 미확정·하렌3·다른 인물 제외. 세린부터 생성.
- **직접 제작 ZIP**: `LIMITLESS_Renewal_Voices_Serin67_Taeon85_Auto_20261020.zip` **16492bytes / SHA256 `4fc85d78105f08c21241877c05d351d37eae3092f42e51a5f9c247b46ddeebc2`**. Python `Renewal_Auto_TTS.py` 25921bytes / SHA256 `efb653101f34276483f821a0b3d7eb9b57142a6b999f8c9c74d805dfdf8bdf88`. ZIP에는 Serin/Taeon/_progress 디렉터리, 설정/README/selftest. **API.txt는 기존 키 보호를 위해 ZIP에서 제외**. 사용자 로컬 API 키를 직접 수집/기록/노출하지 않는다.
- `conda activate py3_12` → `cd F:\Downloads\Renewal_Voices` → `python .\Renewal_Auto_TTS.py init`(사용자 로컬 정본 F:\study\codex\Project-Limitless 읽기, Manifest 생성, 기존 승인 세린/태온 WAV 샘플만 사본 복사) → `... check`(키4/152/개별 파일·카운트) → Google 프로젝트별 실제 무료 잔여와 무과금 확인 후 `RENEWAL_SETTINGS.json`의 `free_tier_confirmed:true`로 명시 → `... run`(RUN 승인, 하루 최대34회, 실패 시 HOLD) 또는 `... watch`(프로세스가 켜 있는 동안 PT 날짜가 바뀔 때 카운터 리셋하여 이어가기).
- Key1..Key4 카운트는 **ATTEMPT 성공/실패/결과불확실 전부** 포함. Append-only `_progress/api_events.jsonl`, 보기용 `_progress/USAGE_LOG.csv`, 날짜/PT 일일요약 `USAGE_TODAY.json`. Google 공식 **프로젝트당 RPD/태평양시간 자정 리셋**; 4키가 같은 프로젝트면 합산한도 없음. 최초 run에 SHARED/SEPARATE 실제 프로젝트 소속 확인 필수, 혼합 프로젝트는 설정에 그룹 수동 지정. 보수적 key+project-group 10회/일 가드, 전체34회 글로벌 예산. 과금·실제 잔여는 자동 조회·보증 불가, HTTP400/429/크래시/HOLD 즉시 중단, 자동 retry0.
- `samples/main20_confront_serin_01__Erinome.wav`→`Serin/main20_confront_serin_01.wav`, `samples/main21_route_taeon_01__Algieba.wav`→`Taeon/main21_route_taeon_01.wav` 원본 파일 존재/PCM 검증되면 사본만 복사·WAV+json SHA 기록·API0(각 67/85 제작목표에 포함). 다른 승인/원본 WAV 모두 보호.
- ZIP Python 문법 / unittest5 / mock Google GET·POST4/중복생성 재실행/그룹 safety/WAV 형식·API 키 분리/샘플 격리 복사·ZIP 압축해제 init/check **PASS**. 이 테스트에 실제 Google API 사용 **0회**, 사용자 F 드라이브 파일 변경 **0회**, Unity/Save 수정0. 실사용 전 사용자 PC 실행 및 Google quota/한국어 청취 검증 필요.
- **[상세 Python 제작 Runbook](../../TTS_RenewalVoices_Serin67_Taeon85_4Key_Automation_Runbook_20261020.md)** / [Issue #20](https://github.com/HappyPaul812/Project-Limitless/issues/20). Codex는 사용자가 전 범위 승인한 WAV에 한해 Unity 통합. docs/tts 브랜치만 기록, main 직접커밋·푸시·병합 금지.

## 2026-10-20 최신 사용자 결정 — 하렌 억양 재생성은 나중에 하렌 3대사 제작할 때

**최우선: DEFERRED / NO TTS CALL NOW.** 바울 대표 “나중에 하렌 대사 뽑을 때 할꺼니 문서화 해줘”.

- 하렌 Alnilam의 캐스팅 음색 자체는 긍정 평가. `main21_haren_haren_01` 샘플이 의문문처럼 끝나 **억양만 FAIL**. **지금 R1 `retake`를 실행하지 말고** 하렌 정식 3개를 제작하는 날 함께 다시 만든다.
- `01` = **“태온. 너 아직 멀었어.”** (평서문·어미 하강·단정), `02` = **“다친 데는 없고?”** (자연스러운 의문문), `03` = **“그럼 됐어.”** (평서문·어미 하강). Google 프리셋 **Alnilam 유지**, 세린/태온 한국어 샘플 긍정 결과 및 기존 WAV는 보호.
- 오프라인 준비된 R1 ZIP은 **미실행 참고도구**로 보존, 이전 아래의 FREE/YES1/PowerShell 실행안은 **과거 제안이지 현재 실행 지시가 아님**. 새 R1 WAV 생성0/추가 Google 호출0/Unity0. 다음 하렌 제작 시 실제 무료 quota/과금 확인과 개별 대사 청취 합격 필수.
- [실행 보류 결정 및 세 대사 억양 구분 정본](../../TTS_하렌_평서문_억양_수정R1_20261020.md) · [Issue #20](https://github.com/HappyPaul812/Project-Limitless/issues/20).

## 2026-10-20 19:12 최신 사용자 청취 — 하렌 첫 대사 억양만 불합격 / 독립 수정 R1 준비

- 바울 대표 “다 좋고, 근데 하렌 대사가 '아직 멀었어?'가 아니고 '아직 멀었어.'야”. 즉 세린 Erinome/태온 Algieba 한국어 샘플은 긍정 평가, 하렌 Alnilam 기본 음색도 긍정 평가이나 **하렌 `main21_haren_haren_01` 끝이 상승 억양(의문문)**으로 들려 부분 FAIL.
- 정본 원문은 **`태온. 너 아직 멀었어.` (마침표 평서문)**이며 원문/ID/보이스를 변경할 일이 없다. **정서/운율(말끝 낮춤) 연기 문제**이므로 하렌 첫 대사만 Style을 강화해 1회 재시험, 다른 두 샘플·원본 WAV 재제작 금지.
- **[하렌 단발 억양 수정 R1 정본](../../TTS_하렌_평서문_억양_수정R1_20261020.md)**. 사용자 다운로드 `LIMITLESS_Haren_Declarative_Retake_R1_20261020.zip` SHA256 `bb4624427859d503477de9f73e4d2cc49c3f7388d6125d88c6dcd32a6e8ab697` (6631bytes), Python+README. `F:\Downloads\Renewal_Voices`에 추가 후 `python .\Haren_Declarative_Retake_R1.py check` → `... retake`, **FREE/YES1** 승인 후 단1개 추가 Gemini POST. 성공 시 `samples/retakes/main21_haren_haren_01__Alnilam_DECLARATIVE_R1.wav` 별도 저장, 옛 `samples/main21_haren_haren_01__Alnilam.wav`와 JSON 및 태온/세린 2개 영구보존.
- 오프라인 mock/원본 해시/중복생성 차단/HOLD+lock/ZIP 검사 PASS, **실제 API 호출0/새 한국어 WAV0/Unity0** (사용자 실행 전). 최종 사용자 하강 억양 청취 PASS 대기. 추가 캐스팅이나 제작 전량 재개 승인은 별도.

## 2026-10-20 최우선 — 세린·태온 음색 사고 보고서 및 하렌 포함 3인 캐스팅

**다음 과거 장·시점의 Schedar 유지/“Main18 세린10 재제작 대기” 등의 역사 기록보다 아래 최신 정본을 우선한다.**

- **[2026-10-20 TTS 캐스팅 성별 역전·음색 불일치 사고 보고서](../../TTS_캐스팅_성별역전_음색불일치_사고보고서_및_3인재캐스팅_20261020.md)** — 사용자 피드백, 기술/청취 QA 차이, API400, DSP16 전량 실패, 캐스팅 재선정 및 재발 방지 전 과정. Google의 과거 버그 수정은 *미확정 가설*.
- Google Voice 공식: Schedar=Male(옛 세린), Gacrux=Female(옛 태온). 초기 음성의 실제 청취 승인과 최신 캐스팅의 장기 안정성 별개. TTS 담당 검증·재시도 판단상 실수로 기록.
- 사용자 최신 범위: **하렌3 / 세린67 / 태온85** = 총155개(현 Unity 등록 교체140 + Main20·21 미등록 확정15). Chapter3 미확정 제외. 제작은 **하렌→세린→태온**.
- 최신 후보: 하렌 **Alnilam**, 세린 **Erinome**, 태온 **Algieba**. 각자의 Google **공식 데모를 사용자 “좋네”라며 긍정 평가**했으나 **실제 한국어 시범 생성·최종 캐스팅 승인 전**. 차선은 각각 Algenib/Despina/Iapetus.
- 기존 Voice ID Schedar/Gacrux 재시도 및 DSP 패치 중단. 기존 사용자 승인 Main18 세린10·Main08 태온, 다른 전체 화자 WAV와 290 Catalog/Save/HOLD 원본 보존. 사용자 승인 전 게임에서 기존 성우를 삭제/교체·전체 음성 재녹음 금지.
- 실제 한국어 시험 제작은 사전 사용자 승인·Google 프로젝트+모델 quota 확인 후 TTS 담당. 사용자가 청취 합격하고 3인 각 대사 범위가 준비됐을 때만 Codex가 Unity 별도 구현·회귀. Main 직접 Commit/Push/merge 금지. [작업 Issue #20](https://github.com/HappyPaul812/Project-Limitless/issues/20).
## 오늘 결과
- **Main18 세린10 오류 전용 Schedar 안전 준비 v2** 작성 및 격리 fixture 테스트 완료. GitHub 기록: [TTS_Serin10_Schedar_v2_Preflight_20261013.md](../../TTS_Serin10_Schedar_v2_Preflight_20261013.md).
- 별도 배포 Python: `M18_Serin10_Schedar_v2_SAFE.py`, SHA256 `2edd62ff4c7cbc7fe055c665b39f35aaa5ef126691d1393c692fb42e8e1cd741`, 13,057 bytes. **Gemini/HTTP 함수 없음; sample 명령 항상 차단**.
- 명령: `python .\M18_Serin10_Schedar_v2_SAFE.py audit` (읽기 전용) → 로컬 결과 확인 → `python .\M18_Serin10_Schedar_v2_SAFE.py prepare` (독립 새 폴더에 오류 WAV10 SHA 검증 백업·잠금 보고).
- v1 `F:\Downloads\M18_Serin10_Staged.py` (Erinome/Kore만 허용) 실행 보류, 수정0.

## 2026-10-20 / 세린·태온 보이스 성별 설정 문제 확인 및 새 Python 샘플 팩 (최신)

- 사용자: “tts 되니까 작업하자.” 최신 청취 오류9(세린8 남성처럼 들림, 태온1 여성처럼 들림) 재개.
- **Google 공식 Firebase voice table의 원인 확인:** `Schedar=Male`, `Gacrux=Female`. 전일 작성한 “기존 보이스 유지 + 성별 Style” 실행팩은 추가 재제작에 사용하지 않는다. 해당 팩의 Style 조건만으로 프리셋의 성별 차이 보완은 신뢰할 수 없음.
- TTS 담당이 **새 Python ZIP 직접 제작**: `LIMITLESS_TTS_Main20_Main21_GenderFix9_SampleFirst_20261020.zip`, SHA256 `c9f8e6af586c37c3b881f37cc4ff3db46b3485991022c485a2c3b9f7c752925d`, script `LIMITLESS_TTS_GenderFix9_20261020.py` SHA256 `fd55444cb033d314a462d7f3a41d109f42260c9c12569ea7e1606e8368349c27`. ZIP/CSV/README 사용자 대화에서 제공.
- 샘플 후보 보이스 **세린=Erinome(Female, Clear)**, **태온=Alnilam(Male, Firm)**. 이번 Main20·21 테스트에서만 임시 사용, 기존 Main18 청취 승인 세린10이나 다른 챕터 음성 Voice Registry를 일방 변경하지 않음. 사용자 두 샘플 청취 승인 후 나머지 세린7 생성.
- `F:\Downloads\Voice\LIMITLESS_TTS_GenderFix9_20261020`에 압축 해제한 뒤 `python .\\LIMITLESS_TTS_GenderFix9_20261020.py check` → 실제 Google 무료 quota 및 과금 확인 후 `sample-serin`, `sample-taeon`; 출력 `F:\Downloads\Voice\Limitless_TTS_GenderFix9_20261020\candidates`. 체크에서 기존 17 성공 WAV/결과 SHA 불일치·경로 문제 있으면 호출금지/보고.
- offline Python compile/ZIP CRC, 17원본 SHA mock 보호, 샘플2→승인→나머지7 성공, 2번째 요청 모의 오류 시 HOLD/lock 및 뒤호출0 PASS. **실제 PC API 호출·실제 WAV 신규 생성=0**, 게임 Unity 적용0. 실사용 결과와 청취 평가는 사용자 응답 대기.
- 상세: [2026-10-20 정정 및 실사용 Python 인계](../../TTS_Main20_Main21_VoiceGenderFix9_20261020.md). 기존 Issue #20 보이스 오류 추적 유지. Codex는 사용자 승인 WAV Unity 통합만.

## 2026-10-20 최신 — 파형 보정 오프라인 시험판 (바울 대표 요청)

- 사용자는 성별 프리셋 교체 대신 **이미 생성한 WAV9 파형 보정** 요청. TTS 담당이 `LIMITLESS_TTS_DSP9_Offline_20261020.zip` 직접 작성·배포 (SHA256 `1a91fa078a367c7a6336fdb44209b0cd743ee43d41da4c75c392fd04c99afec9`). Google API 호출0, Unity 수정0.
- `check` 원본9 읽기 전용 검사 → `preview` 세린1·태온1 각각 gentle/medium/strong (6개) → 바울 대표 청취 승인 후 `apply --serin ... --taeon ...`로 오류9개만 새 위치에 후보 제작. 피치+포먼트 근사 DSP, 원본 음색 완벽 복구 보장 없음.
- 샘플용 합성 WAV 기반 compile/check9/preview6/apply9/원본 SHA 불변/24k PCM16/중복 출력 보호 PASS. **실제 사용자 PC WAV 보정·음질 검증 미실행**.
- [정본/기술 상세](../../TTS_Main20_Main21_Offline_DSP9_Pilot_20261020.md). 기존 VoiceFix9/GenderFix9 API 생성 패키지는 계속 사용 보류. 기존 Main18 승인, 나머지 정상음성, 원본 WAV, save/HOLD/lock, Unity 보호.

## 2026-10-20 최종 청취 피드백 — DSP 보정 전량 불합격 (최신 정본)

- 바울 대표의 직접 판정: **“다 못쓰겠다. 안바뀌고 외계인 목소리로 돼..”**. 세린·태온 gentle/medium/strong DSP 미리듣기 음성은 기존 캐릭터 음색을 복구하지 못하고 인공적 왜곡 발생. **DSP_PREVIEW_ALL_USER_FAIL / DSP_STOPPED**.
- 오프라인 가상음성의 테스트 PASS는 처리 오류 없음/원본 SHA 보존만 검증했으며 **청취 품질을 보증하지 못했음**. 기존 `LIMITLESS_TTS_DSP9_Offline.py preview/apply` 결과 공식 음성 사용 및 Unity Import 금지. 반복·보정 강도 증폭 금지.
- 원본 Main20/21 17 WAV, 이미 승인된 Main18 세린10 및 기존 정상25, 사용자 Save와 checkpoint/HOLD 불변. Gemini API를 이 세션에서 새로 호출하지 않음.
- 후속 방침: 기존 정상본과 불량9의 실제 요청 설정/모델/VoiceID/스타일/SDK/대사·WAV SHA를 우선 API0 비교. 일치하면 기존 Voice ID/원문 유지하고 **불량1개만 통제 재생성 후 청취**, 실패 시 추가 비용/쿼터 낭비 중단. 대안 프리셋 무단 변경 금지. 상태·근거는 [DSP9 청취 불합격 정본](../../TTS_Main20_Main21_Offline_DSP9_Pilot_20261020.md).

## 2026-10-20 바울 대표 “오늘 20회 써보자” — 직접 Python 배치 준비 (최신)

- 20회는 **키1 최대10, 키2 최대10** 사용자 희망 예산. 두 키가 독립 Google Project, 모델 gemini-3.8-flash-tts FreeTier 실제 사용 가능/무과금이 확인될 때만 해당 상한 사용. 동일 Project 키 중복합산 금지. 실제 API 호출0 (바울 대표 PC 실행 대기).
- 이전 **DSP 미리듣기 전량 FAIL** 유지. 새로운 Python은 원래 생성 당시 **동일 Schedar/Gacrux + 동일 Style + 동일 REST 요청 JSON + 동일 ExactText**로 제작; 프리셋 교체/피치 보정0. 9개 오류=세린8, 태온1. Main21 추가 미제작2=main21_haren_taeon_02, main21_prepare_serin_01. 하렌 자체 Voice미승인3 BLOCK.
- TTS 담당 직접 `LIMITLESS_TTS_Today20_20261020.zip` 제작, SHA256 `7e2c80cc26f6127dbc22075e975959afab4246aa66f57ef6696e2b7c747ae635`; Python+고정 대사CSV+README. 사용자 대화에서 다운로드 제공. 별도 원본 `Limitless_TTS_Main20_Main21_20261019` 전체 17 결과 SHA 보호; 새로운 결과 폴더 `Limitless_TTS_20Calls_20261020`.
- 안전 단계: `check` API0 → `sample --confirm-free` 세린1/태온1 (KEY1 2) → 둘 다 사용자 청취 PASS면 `approve-samples` → `main --confirm-free` 나머지9 (KEY1 8, KEY2 1) → 실제 청취 FAIL인 ID에 한해 `retry --ids ... --confirm-free` 최대9 (KEY2 9). **실제 불량이 없는 상태에서 예비9를 무조건 소진하지 않음.**
- 오프라인 모의: 17원본 SHA 감사, sample2→승인 게이트→main9→retry9 = 20/Key1 10/Key2 10 PASS. 두 번째 가짜 API 오류 시 HOLD+lock 유지·뒤호출0 PASS, WAV 변조 감지 PASS. **실제 Gemini 신규 호출/WAV0/Unity0**.
- [상세 계획/스크립트 메타](../../TTS_Main20_21_Today20_Staged_Retry_20261020.md). Codex는 여전히 사용자 최종 청취 승인 뒤 Unity 통합만 담당. TTS 업무 책임/실행은 TTS 전담 세션+바울 대표 로컬 Python. docs/tts 브랜치만 갱신, main 변경·Push/merge 없음.

## 2026-10-20 최신 실제 실행 로그 — 샘플 첫 API HTTP400 / HOLD 유지

- 바울 대표 PowerShell 화면: 이전20회 `check` PASS; `sample --confirm-free` KEY1 첫 `main20_confront_serin_01`/`Schedar`/`gemini-3.8-flash-tts` 호출 **HTTP400**, 성공 WAV0, 이후 태온/나머지 요청0. 즉시 `HOLD.json`+`RUNNING.lock` 유지, 중복 실행 차단 정상.
- 신규 Python 스크립트/이전17회 스크립트/Google 공식 Gemini 3.8 REST JSON 구조 일치함을 정적 확인했으나 기존 코드가 **HTTP400 상세 응답 본문 저장하지 않은 결함** 있어 원인은 미확정. Key/Voice/모델/컨텐츠 문제로 단정 금지.
- TTS 담당 직접 안전 진단 ZIP `LIMITLESS_TTS_HTTP400_Diagnose_20261020.zip` 작성·배포 (SHA256 `d1270b807435d8f222e5a976ba00b65c133ea5fa19fb800840b87c645296815a`). 기존 스크립트 옆에 추가하고 `inspect`(API0/쓰기0), `model-info`(Google 모델 GET1/TTS POST0) 순서로 실행, 결과 보고. `diagnose-one`의 단 1회 POST는 상세진단 필요 시 사용자 추가 확인 후에만. 기존 HOLD/lock은 절대 해제/초기화/덮어쓰기 금지.
- 가상 원본17 SHA 및 HOLD/lock 보호, 모델 GET, 오류 400 POST 상세 메시지 추출/키 마스킹, 추가 호출 차단 오프라인 테스트 PASS. **실제 신규 Google API/모델 GET은 이 ChatGPT에서 실행하지 않았으며 사용자 PC 진단 대기**. 첫 시도는 보수적으로 KEY1 소모1회로 기록. Unity0/main0.
- 상세 정본: [2026-10-20 첫 HTTP400 진단](../../TTS_Main20_21_Initial_HTTP400_Diagnostic_20261020.md). Issue #20도 BLOCKED로 갱신.

## 2026-10-20 실제 진단 결론 — KEY1 API 인증 거절 (최신)

- 사용자 진단 `inspect`: HOLD/lock·원본 SHA 정상, 첫 샘플 HTTP400, 신규 WAV0/후속 생성 호출0.
- 사용자 `model-info` 2회: 모두 HTTP400 `INVALID_ARGUMENT` / `API key not valid. Please pass a valid API key.`. 이는 TTS generateContent가 아니라 모델 메타 조회 GET이므로 **이 시점에 입력한 KEY1의 API 인증이 거절된 것이 직접 문제**. 첫 실패에 사용된 키와 두 진단 입력의 동일성은 화면만으로 별도 확인 필요.
- 다음 조치: Google AI Studio에서 해당 Google Project의 유효 API Key(프로젝트 ID와 다름)를 안전하게 재확인, 채팅·화면에 키 노출하지 않은 상태로 `model-info`만 다시 검사. PASS 전 샘플·진단 POST 추가 실행 금지. 기존 HOLD/lock 삭제·재설정 금지, 이전 Main20/21 WAV와 Main18 승인본 보존.
- [정본 상세](../../TTS_Main20_21_Initial_HTTP400_Diagnostic_20261020.md). 이 진단을 통해 새 TTS 호출0, 새 WAV0/Unity0. 기존 음색 오류9는 별도 미해결.

## 2026-10-20 최신 — 진단1회 생성 성공, 세린 남성 음색 청취 FAIL

- 바울 대표 화면으로 유효 KEY1의 단일 generateContent 요청 성공, WAV1개 별도 저장 확인. 대상 main20_confront_serin_01, Schedar, gemini-3.8-flash-tts. result.json 경로 F:\Downloads\Voice\Limitless_TTS_Today20_400Diagnostic_20261020\result.json. 추가 TTS 호출0, 기존 HOLD/lock 유지.
- 사용자 청취: “세린인데 남자..;;” → USER_LISTENING_FAIL. 생성 성공과 캐릭터 음색 합격 구별, 해당 WAV Unity 적용 금지. Main20/21 세린8/태온1 불합격 및 DSP 오프라인 보정 전량 FAIL 유지.
- 관측된 오늘 생성 POST 시도2(처음 HTTP400 1, 진단 성공1), 새 WAV 성공1. 실제 무료 quota 차감은 미확인. 원래 Voice 반복 재생성 및 20회 무조건 소진 중단.
- 다음은 정상 승인 세린 음성(Main18)과 요청 기록 비교, 음색 변경 대안 검토/사용자 승인 후 제한적 제작. 기존 승인 음성 및 모든 Save/WAV/HOLD 보호. 상세 문서 TTS_Main20_21_Initial_HTTP400_Diagnostic_20261020.md.

## 2026-10-20 최신 — 승인된 세린 WAV를 직접 참고하는 1개 Voice Conversion 시범

- 사용자: “내가 OK한 세린 파형으로 니가 바꿔볼래?” 기존 단순 DSP/피치·포먼트 방식은 사용자 청취 FAIL 유지. **사용자 승인 WAV를 참조하는 OpenVoice 기반 화자 음색 변환**을 새 방식으로 1개만 검증 준비.
- 기준 WAV: `F:\Downloads\Voice\Limitless_TTS_Serin10_Schedar_v2\sample_candidate\main18_ground_serin_01.wav`, 승인 SHA256 `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`. 대상: 2026-10-20 단발 성공·음색 FAIL `F:\Downloads\Voice\Limitless_TTS_Today20_400Diagnostic_20261020\main20_confront_serin_01_DIAGNOSTIC.wav`.
- TTS 담당이 `LIMITLESS_Serin_ApprovedVoice_VC_20261020.zip` 직접 제작·사용자 다운로드 제공 (SHA256 `77293681c01e82f7be00fb2029f73ca287c0522dadbb338c8086097c7f567f06`). Python + 한국어 README만 동봉, 제3자 Windows x64 CPU OpenVoice 런타임/131MB 모델은 `setup` 사용자가 명시적으로 승인 후 다운로드, SHA 검사.
- `check` API0/쓰기0 → `setup` 외부 OSS 다운로드(유료 Gemini API0) → `preview` 사용자 로컬 WAV 1개만 독립 `Limitless_TTS_Serin_ReferenceVC_20261020` 폴더에 변환해 청취 대기. 원본 승인·오류 WAV, 그 외17/25, 모든 HOLD/lock·Unity·Save 불변. 이전 Main18 승인 보이스 레지스트리 변경0.
- 스크립트 compile/가짜 WAV 입력·승인 참조 SHA·윈도우 엔진 명령·중복 결과 방지·해시 QA mock PASS, **실제 F: 사용자 WAV, OpenVoice 모델 실행 및 음성 청취 아직 미검증**. 이 방식도 완벽한 원래 음색 복원 보장 없음. 결과 청취 FAIL시 추가7개 연속변환 금지.
- [GitHub 상세 정본](../../TTS_Serin_ApprovedReference_VoiceConversion_Pilot_20261020.md). TTS Python 작성은 TTS 담당, Unity 통합만 Codex. docs/tts만 기록, main 수정0.

## 2026-10-20 최신 — 바울 대표 요청에 따라 실제 세린 WAV 8개 직접 변환해 ZIP 전달

- 사용자 직접 첨부 `candidates.zip` 17 WAV 실제 바이트 확인. ChatGPT 파일 보관함에서 과거 바울 대표 승인 `main18_ground_serin_01.wav`(SHA `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`) 및 `LIMITLESS_Serin6_ConsistentVoice_Previews.zip`(SHA `a5878e6d4f1b1e858f4b505f23669ab147da2dd466fe30443b0d6116a7246cb1`) 확보·해시 확인. 이전에 파일 접근 불가하던 상태 해소.
- TTS 담당이 실제 사용자 오류 세린8에 대해 **A_natural/B_closer 두 버전, 16개 실 WAV**를 작업 컨테이너에서 직접 작성하고 사용자에게 `LIMITLESS_Serin8_ReferenceGuided_WAV_Previews_20261020.zip` 전달. ZIP SHA256 `349d6bf7047a92d04972f68c769595969b3ff69d8b6e03f1ea8bfd7179ea535e`, 크기4,419,922 bytes.
- 공식 승인 WAV7의 F0/평균 스펙트럼을 참조한 Praat PSOLA 피치/포먼트 + 제한적 참조 EQ 방식. **AI OpenVoice 사전학습모델을 다운로드/실행하지 못하므로 ML 목소리 복제라고 주장하지 않음**. 과거 DSP6 사용자 FAIL과 구별되지만 자연스러움 사용자 평가 **대기**, 두 샘플 모두 불합격이면 16개 전체 공식 불합격 처리/Unity 미적용.
- WAV 16개 24kHz mono PCM16·길이·SHA·리드디코딩 PASS; 업로드 원본/승인 기준7 SHA 불변. API0/Unity0. 태온 목소리는 다른 참조가 필요하여 제외.
- 세린 `main20_confront_serin_01` A와 B 샘플 WAV도 ZIP 밖 직접 공유. [상세 정본](../../TTS_Serin8_ReferenceGuided_Offline_Prevs_20261020.md). 브랜치 docs/tts만 기록, main 변경/Push/merge0.

## 2026-10-20 최신 — 승인참조 세린 A/B 16개 사용자 청취 전량 FAIL

- 사용자 직접 피드백 **“다 남자다..;;;;”**. 실자료 기반 reference-guided PSOLA+EQ A/B 16개 전체 **USER_QA_FAIL / DO_NOT_USE / UNITY_APPLY_FORBIDDEN**. 이전 단순 DSP 실패와 마찬가지로 여성 캐릭터 음색 복구 실패.
- 피치/포먼트/평균스펙트럼 참조 보정은 화자 음색 변환이 아니라 음향 통계 보정이며 결과의 남성 지각을 제거하지 못함. 본 계열의 추가 보정·강도 증가 중단. WAV 포맷 검사 PASS≠청취 PASS.
- 향후 사용자가 원하면 실제 화자 임베딩 기반 신경망 Voice Conversion 모델을 **승인 Main18 세린 기준 WAV로 1개 시범** 사용하거나, 신규 여성 보이스 한 개를 실제 청취해 정식 선정. 모델/변환의 성공은 미보장, 사용자 승인 전 기존 공식 보이스 설정·Main18 승인10 및 Unity 모두 불변. 태온1 음색 문제는 별도.
- 상세 결과: [세린 16개 직접 보정 전량 불합격](../../TTS_Serin8_ReferenceGuided_Offline_Prevs_20261020.md). 이번 업데이트 Github docs/tts 문서만, 음성 API0/Unity0.

## 2026-10-20 최신 — 세린 기존 Schedar 안전 재시험 Python 준비

- 바울 대표 “18회 버리더라도 또 뽑아봐?” 지시. TTS 담당은 18회 무조건 소진하지 않고 원래 `Schedar`/원래 스타일/원문과 같은 설정으로 **1회 시험 후 청취 게이트**, 최대2회 수동 재시험을 준비.
- 다운로드 `LIMITLESS_TTS_Serin_Retry2_20261020.zip` SHA256 `6369b14fe7032046111f8fe4f4296e0a6cadf8633299bc579d128bf22fee4180` (279734 bytes), 스크립트+검증된 17행 CSV+승인 Main18 세린 WAV 실파일+한글 README. 기준 WAV SHA `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`.
- 사용자 로컬에서 `check` API0→`trial --attempt 1`(정확히 1회, 모델 GET 먼저, 유료/잔여 수동 확인) 실행. 청취 불합격일 때만 `trial --attempt 2`와 `FIRST_FAIL` 별도 게이트. 기존 HOLD/lock 및 모든 원본 변경 금지, 새 출력 폴더 격리.
- 모의 성공1/2, 중복 번호 차단, 실패 HOLD/lock 및 다음호출0 PASS. **실제 Google API 호출0/실제 음색 미검증**. [상세](../../TTS_Serin_SameVoice_Retry2_20261020.md). TTS 제작 담당 유지, Codex는 최종 사용자 승인 WAV Unity 통합만.

## 2026-10-20 최신 사용자 가설 검증 — 세린/태온 기존 VoiceID의 공식 성별이 역방향 (우선순위 높음)

- 바울 대표: “남잔데? 혹시. 우리가 뽑았던게 오류고 오류수정이 남자 아냐?” 이전 승인 세린 WAV는 여성처럼 자연스러웠으나, 이후 동일 `Schedar`를 사용한 Main20/21 생성/재시도는 계속 남성으로 들린다고 확인. **이전 음성이 예외적인 TTS 출력이었고 현재가 원래 음색으로 돌아온 것**이라는 설명이 충분히 가능. 단, Google의 특정 버그 수정·배포 시점은 직접 확인하지 못했으므로 확정 금지.
- Google 공식 Gemini-TTS/Cloud docs의 **프리셋 성별**: `Schedar = Male`, `Gacrux = Female`. 현재 세린 `Schedar`와 태온 `Gacrux`는 의도한 캐릭터 성별과 **공식 프로필이 반대**다.
  - 출처 https://docs.cloud.google.com/text-to-speech/docs/gemini-tts
  - 출처 https://firebase.google.com/docs/ai-logic/generate-speech?api=vertex
- 과거 담당이 `Schedar`를 “세린 공식”이라 고집하며 세린 8개 재생성·DSP/PSOLA 등 시도를 반복한 것은 **사용자 승인 기준을 VoiceID와 혼동한 오류**였다. 사용자가 승인한 것은 *들린 실제 목소리*였으며 그 정체성을 우선해야 함.
- **새 작업 원칙**: Main18 세린 기존 사용자 승인 WAV10과 기존 정상 태온 오디오는 변경/재생성 금지. 신규 세린 1대사에는 공식 여성 프리셋 중 중복 캐릭터 충돌 없는 후보(예: `Erinome` Female/Clear)를 **1회 시험**하고 과거 승인 샘플과 귀로 비교. 태온은 공식 남성 보이스 후보(예: `Alnilam` Male/Firm) 1회 시험하여 과거 정상 태온 샘플과 비교. 둘 모두 “후보”이지 사용자 청취 승인 전 전역 캐릭터 VoiceID 변경 아님. 이 기능은 앞선 GenderFix9 스크립트를 바로 재사용하라는 지시가 아니고 원본/쿼터 및 요청설정 검증 후 안전 실행 방식 결정 필요.
- '그동안 승인 여성 음색이 원래 버그였고 현재 수정됐다'는 인과는 미검증이며, 실제 요청 wire 파라미터/날짜/모델 업데이트 자료 부재. 더 이상 `Schedar` 동일 설정으로 무의미한 18회 반복 생성하지 말 것. 남은 무료 호출을 보호.
- 이번 확인은 공식 공개 목록을 읽은 **진단/문서화만**; TTS API 추가 호출0, Unity 0, 정상 WAV 수정0. Main20/21 세린8 + 태온1 사용자 불합격 상태 유지.

## 2026-10-20 전역 품질 방향 전환 — 전체 TTS 일관성·전면 재제작 사전판정(최신)

- 바울 대표 강한 문제 제기: “지금 그게 문제가 아니고 이 가설이 맞으면 게임에서 싹 다 거내고 처음부터 다시 해야 한다. 일관성 때문에..”. 세린 8개를 고치는 문제가 아니라 **실제로 게임에 들어가 있는 모든 대사 음성의 캐릭터별/챕터별 동일성 문제**. 세린/태온 전역 Voice ID만 바꿔 새 챕터를 제작하면 이전 챕터들과 음색이 달라질 위험이 큼.
- 지금은 **전체 TTS 생성 및 신규 Unity 반영 HOLD, 원본 삭제/기존 Catalog 변경 금지**. 사용자 발언은 “가설이 맞으면”이라는 조건부 판단으로 취급, 무조건 WAV 삭제나 전면 재제작에 관한 실행 승인은 아님.
- `CURRENT_STATUS.md` 2026-10-19 기준 Main18 승인10 적용 후 **StoryVoiceCatalog280→290** / Intro Narrator18 별도 / Main20·21 22음성 미등록. 과거 Main01~12 102, Main13~15 41, Main18·19 정상25, Main17 14, Main08 Taeon5 등 후속 적용. 현행 290개의 실제 캐릭터/챕터/미묘한 청취 합격은 로컬 전수 실측 필요.
- 공식 성별 매핑: Schedar=Male(세린 불일치), Gacrux=Female(태온 불일치), Achird=Male(폴), Sulafat=Female(미엘), Orus=Male(레온). 실제 WAV 청취 품질은 프리셋 메타만으로 결정하지 않는다. 구세린이 버그로 여성 출력되었는지/고쳐진 시각은 증거 없음.
- **중요 Google Voice Replication 제약:** 2026-10-12 공식 문서상 동일 실제 성인 인간 화자의 reference audio 10–30초 및 직접 낭독한 consent audio 필수. 기존 합성 세린 WAV만으로 복제를 바로 수행할 수 없음. Voice Replication을 합성 샘플 복구 정답으로 제시하지 않는다. https://ai.google.dev/gemini-api/docs/voice-replication
- 전체 기술/운영 및 코덱스 READ-ONLY 인벤토리 지시 정본: [TTS 전체 캐스팅 일관성 감사/재제작 승인게이트](../../TTS_전체_캐스팅_일관성_재검토_및_전면재제작_판정게이트_20261020.md). GitHub **Issue #39** (read-only local Unity 290 카탈로그 + Intro18 전수 감사, 실행 승인은 아직 없음).
- 최종 사용자 선택은 전체 290+성우 라인 등 **전면 Voice Registry v2 리캐스팅** 대 캐릭터별 전 챕터 재더빙 범위. 어느 쪽도 기존 승인본/원본/Save/HOLD를 지우지 않고 분리 제작/검증 후 Unity 연결만 교체. 사용자 승인 전 구현/TTS 호출/비용 지출0.

## 2026-10-20 최신 사용자 지시 — 전체 TTS 폐기 아님, 세린·태온 2인 캐스팅만

- 바울 대표: **“그러지 말고, 태온 세린만 새로 캐스팅 해봐. 인물 성격과 특징에 맞게.”** 전역 TTS 삭제·재녹음 추진 STOP, [전수감사 Issue39](https://github.com/HappyPaul812/Project-Limitless/issues/39)은 **범위 변경으로 closed/not_planned**, 기존 Issue20에서 2인 캐스팅 진행.
- 공식 캐릭터 정본 대조: 세린 여성 약25세·청각의 길 사수·침착/날카로운 관찰·짧고 빠른 판단/가까워지면 건조한 유머. **공식 Female Clear Erinome 1순위**, Female Smooth Despina 차선. 태온 남성·지적의 길 수호자·관찰→이해→보호·차분하고 성숙·형과 가족식 반말/일반 존댓말. **공식 Male Smooth Algieba 1순위**, Male Firm Alnilam 차선.
- [캐스팅 1차안/스타일/샘플 원문/승인게이트](../../TTS_세린_태온_리캐스팅_1차안_20261020.md). 공식 Gender https://docs.cloud.google.com/text-to-speech/docs/gemini-tts, Voice traits https://ai.google.dev/gemini-api/docs/speech-generation. 확정이 아니라 **실제 사용자 청취 전 제안**.
- 시험은 Main20 세린 `main20_confront_serin_01`과 Main21 태온 `main21_route_taeon_01`의 **최우선 후보 1대사씩 최대2회**부터, Google 프로젝트·모델 무료 잔여량/요금 확인 후만 제작. 후보 WAV 청취 승인 전 전역 Voice Registry나 Unity 변경 금지; 보조후보 무단 일괄 시험 금지. 사용자가 원하는 일관성 때문에 최종 2인 VoiceID가 승인되면 두 인물의 **전 챕터** 재제작 범위를 별도 설계하며 폴·미엘·레온·내레이터를 건드리지 않음.
- **이번 턴: 문서화/후보 선정만, 실제 TTS API0·신규 WAV0·Unity0·삭제0**. 기존 Main18 사용자 승인 세린10/Main08 태온 정상 WAV/모든 기존 입력·출력 WAV·HOLD·Save 보호. TTS 담당 스크립트 제작, Codex는 최종 승인 WAV의 Unity 연결만 수행.

## 2026-10-20 최신 — 하렌 추가, 3인 대사량 실제 Unity/Manifest 집계 및 소량 우선순위

- 사용자: “이왕 캐스팅 하는거 하렌도 하고. 또 태온 세린의 대사량 알려줘. 적은 개수 먼저 바꾸게.”
- GitHub main `Unity/Client/Assets/_Project/Resources/Audio/Voice/Story/StoryVoiceCatalog.asset` **290개 엔트리 전체 파싱/ID 중복0**, Main20 CSV/Main21 JSON VoiceExpected 대조.
- **하렌 0 Unity+3 Main21=총3개**, **세린58 Unity+5 Main20+4 Main21=총67개**, **태온82 Unity+3 Main21=총85개**. 합계 현재 등록140+미등록15=155개 재제작/신규 작업 대상. Chapter3 미확정 대사는 제외, 일부 Main20/21 WAV 후보는 이미 만들었으나 새 캐스팅 기준에서 재제작 대상. 현재 적용된 WAV 숫자와 전 챕터의 향후 확정 대사량은 별개.
- **추천 작업 순서: 하렌3 → 세린67 → 태온85**. 먼저 새 Voice ID의 첫 샘플 한 대사 사용자 청취 확인, 후속 대사 수동 진행, 한 캐릭터의 과거/현재 목소리가 Unity 안에서 섞이지 않도록 최종 승인 후 별도 Codex 교체.
- 하렌 캐스팅 1순위 `Alnilam` (Male/Firm), 차선 `Algenib` (Male/Gravelly). 태온 친형·시각의 길 투사·무뚝뚝하고 보호적·깊은 정, 레온 Orus 등 기존 인물과 중복 청취 확인. Main21 정확 원문 3개: `main21_haren_haren_01`/02/03. 미래 Main29 “잘.했.어.”는 확정 Dialogue ID가 아니므로 현재 3개에 미포함·선제녹음 금지.
- 세린 1순위 `Erinome` Female/Clear, 차선 `Despina` Female/Smooth. 태온 1순위 `Algieba` Male/Smooth, 차선 `Iapetus` Male/Clear (하렌 Alnilam과 VoiceID 중복 방지). 모두 실제 한국어 음성 청취 전 **후보/미승인**.
- 정본: [TTS 세린·태온·하렌 3인 캐스팅 및 대사집계](../../TTS_세린_태온_하렌_캐스팅_대사집계_20261020.md). GitHub [Issue20](https://github.com/HappyPaul812/Project-Limitless/issues/20). 실제 신규 API 호출0/WAV0/Unity0, 모든 원본/HOLD/Save/기존 승인 음성 보호.

## 2026-10-20 최신 — 남은 17회 중 3인 한국어 샘플 3회 준비, F:\Downloads\Renewal_Voices 지정

- 바울 대표: “17개 남았으니까. 해보자. 한국어 샘플 먼저.. 작업 폴더 F:\Downloads\Renewal_Voices”. 실제 무료 잔여17회는 사용자 보고이며 Google Project+Model 공식 잔량/과금 무보증.
- **TTS 담당이 직접 새 Python ZIP 제작 완료**: `LIMITLESS_Renewal_Voices_Korean_Audition_3Samples_20261020.zip` SHA256 `8649f90cffe73baeed22e52390666833f2a263f1c4821b525cfa7ba15a0ecd10` (8040 bytes). `Renewal_Voices_Korean_Audition.py`(13504bytes)+`KOREAN_AUDITION_MANIFEST.json`+한글 README. 고정 폴더 `F:\Downloads\Renewal_Voices`, 결과 `samples`.
- **하렌** Alnilam/`main21_haren_haren_01` / “태온. 너 아직 멀었어.” → **세린** Erinome/`main20_confront_serin_01` / 지면 맥동 두 문장 → **태온** Algieba/`main21_route_taeon_01` / 흔들림 재조사 한 문장. 정확 기존 Manifest·Stable ID 사용.
- `conda activate py3_12`; `cd F:\Downloads\Renewal_Voices`; `python .\Renewal_Voices_Korean_Audition.py check`; `python .\Renewal_Voices_Korean_Audition.py sample`. 최대 3 POST, 성공 시 예상 잔여14(기타 호출/결과에 따라 다를 수 있음), 28초 간격, 누락/중복 저장 차단, 오류 시 HOLD/lock 및 호출중단, 자동 retry0. 개별 `sample --speaker haren|serin|taeon`도 가능.
- 실제 사용자 Google 프로젝트/모델의 무료 잔여 및 무과금 여부를 FREE로 확인, 별도 YES3 수동 승인. 키는 가림 입력·비밀 저장 금지. 모델 GET 성공은 실제 무료 quota 검증 아님.
- 자체 compile/ZIP 압축해제 check/mock GET+POST3/HOLD·중복보호 PASS. **실제 Gemini API 호출0/새 한국어 WAV0/Unity0** (로컬 사용자가 실행하기 전). 샘플 생성 후 **바울 대표 청취 승인 필요**, 합격 전 기존 StoryVoiceCatalog/WAV/GUID/Save 변경금지.
- [이번 3개 한국어 오디션 실행 정본](../../TTS_RenewalVoices_3인_한국어오디션_실행계획_20261020.md); [GitHub Issue #20](https://github.com/HappyPaul812/Project-Limitless/issues/20).

## HISTORY — 기존 main·담당 고유 기록 보존

## 2026-10-19 EOD 통합 현재 상태 (과거 기록보다 우선)

바울 대표 마감 통합 지시에 따라 Main09 승인27·재회순서, Main10 승인18·조사 NPC 우선권, Field06 v1.1 B42/Ground49/Spawn, Field07 v1 B42/투명 Ash7만 통합 브랜치에 준비했습니다. Main18·Field05·Main09 퀘스트 UX는 기존 main 유지/중복 적용0입니다. 아래의 이전 승인대기·미적용·API 계획은 당시 HISTORY이며 현재 실행 허가가 아닙니다.

Field09는 ZIP 재포장 성공과 별개로 지면 반복띠·소품 흰선 미술 FAIL이므로 후보 PNG/Scene/손상·재포장ZIP은 통합 제외. 독사/늑대 v1 반려·v2 HOLD·v3 미승인 sandbox, Main20/21 TTS22 미등록(17후보/5부재·음색불합격9), Chapter3는 제안 단계로 유지합니다. 신규TTS/API0. 원본 Editor 자연Import/손플레이·Main15/16 전체 재주행 NOT_VERIFIED, Search/URP OPEN.

통합 실행·PR·최종 main 및 원본 동기화의 확정 수치는 [EOD 통합 보고](../../EOD_Integration_LocalSync_Closeout_20261019.md)를 따릅니다.

## HISTORY — 기존 main 및 담당별 고유 기록

# LIMITLESS / TTS 담당 최신 인계 — 2026-10-13

## EOD 현재 상태 — 역사적 보류와 구분

Main18 세린 생성8개 모두 USER_LISTENED 기록. 샘플1 음색 승인, heat_vent1 불일치, 나머지6 음색변동/최종 KEEP·REGEN 미정. DSP6 PREVIEW는 미승인, Unity 적용하지 않음. Story13 RuntimeError 원인UNKNOWN/HOLD, Story14 로컬 미호출. 추가API0, HOLD/lock 보존.

## 오늘 결과

- **Main18 세린10 오류 전용 Schedar 안전 준비 v2** 작성 및 격리 fixture 테스트 완료. GitHub 기록: [TTS_Serin10_Schedar_v2_Preflight_20261013.md](../../TTS_Serin10_Schedar_v2_Preflight_20261013.md).

- 별도 배포 Python: `M18_Serin10_Schedar_v2_SAFE.py`, SHA256 `2edd62ff4c7cbc7fe055c665b39f35aaa5ef126691d1393c692fb42e8e1cd741`, 13,057 bytes. **Gemini/HTTP 함수 없음; sample 명령 항상 차단**.

- 명령: `python .\M18_Serin10_Schedar_v2_SAFE.py audit` (읽기 전용) → 로컬 결과 확인 → `python .\M18_Serin10_Schedar_v2_SAFE.py prepare` (독립 새 폴더에 오류 WAV10 SHA 검증 백업·잠금 보고).

- v1 `F:\Downloads\M18_Serin10_Staged.py` (Erinome/Kore만 허용) 실행 보류, 수정0.

## 확정 보호

- 세린 Voice Schedar 유지. Main18 오류10만 REGEN_REQUIRED, 사용자 청취 정상 Main18 세린5/Main19 세린7/Main17 승인 세린11 및 Unity 정상 통합 음성25 보호.

- 기존 오류10의 실제 당시 API Wire Voice는 UNKNOWN; 원인 확정 없음.

- 샘플 대상 `main18_ground_serin_01`, 원문/DialogueId/SpeakerId 변경 금지.

- 본 도구는 준비만. 샘플 생성 **NOT_EXECUTED**, 실제 사용자 LOCAL Stage 실행 **NOT_EXECUTED**.

- 사용자 무료 quota 잔량, Google Project별 모델 RPD 및 Billing 상태 **NOT_VERIFIED**, 사용 허용 확정 호출 **0**. 키 변경으로 quota 추가되었다고 가정 금지.

## 다음 선행조건

1. Codex 로컬 정적 대조 또는 사용자 `audit` 실행 결과 수령. `prepare` 성공 시 백업10 SHA, BASELINE/LOCKED 보고 확인.

2. 바울 대표가 Google AI Studio Projects/Billing, Dashboard Usage/Rate limits에서 **현재 해당 프로젝트·모델 무료 사용 가능 여부와 과금 위험 없음**을 확인.

3. 바울 대표의 명시적 **샘플 1회 호출 승인** 이후 새 1-call-generator 별도 코드 감사·실행. 실패/429/불확실한 경우 즉시 중단·재시도/키 변경 없음.

4. 사용자가 샘플 WAV의 실제 음색·원문 청취 PASS → 1개 정식 본으로 사용. 나머지9는 **별도 승인** 후 제작.

5. 전체 청취 승인 이후 Codex가 Unity 음성 연결. 담당 TTS는 Unity 수정하지 않음.

## GitHub 협업

- TTS 작업 브랜치: `docs/tts` 전용; `main` 직접 commit/push/merge 금지.

- 문서 갱신을 마쳤다면 해당 담당 커밋 SHA를 완료 보고에서 별도 확인.

- 세션 전환 시 최신 `main` 정본과 `docs/tts` 미병합 내용 양쪽 확인.

## 후속 LOCAL 실행 확인 — 2026-10-13

사용자가 실제 `py3_12` PowerShell 실행 화면을 제공:

- `M18_Serin10_Schedar_v2_SAFE.py audit` → `[AUDIT PASS] target=10, protected=25, Voice=Schedar, API=0`, 읽기 전용.

- `M18_Serin10_Schedar_v2_SAFE.py prepare` → `[PREPARE COMPLETE / API 0]`, 오류 WAV10 백업 SHA 검증·정상25 비교, Unity 수정0.

- 신규 외부 출력: `F:\Downloads\Voice\Limitless_TTS_Serin10_Schedar_v2`.

- **현재 상태 = `AUDIT_PASS / PREPARE_COMPLETE / SAMPLE_NOT_EXECUTED`**. 위쪽 과거 `LOCAL NOT_EXECUTED` 항목은 해당 시점 이전 기록.

- 사용자의 터미널 로그 확인이며 담당자 독립 로컬 파일 접근 검증은 아님.

- Gemini TTS 생성0 / WAV 신규 생성0 / 전체 무료 잔량·Billing 상태 `NOT_VERIFIED`. 생성기 없음·`sample` 실행 차단 유지.

- 다음은 사용자 AI Studio의 현재 해당 Project/모델 한도·과금 상태 확인 및 **샘플 한 번 명시적 승인**. 승인 후 안전한 별도 생성기 → 사용자의 실제 청취 승인 → 이후9개 별도 승인. 샘플 ID: `main18_ground_serin_01`, Voice `Schedar`.

- 현황 상세: [v2 사전 준비 및 LOCAL 실행 후속](../../TTS_Serin10_Schedar_v2_Preflight_20261013.md).

## 2026-10-13 첫 Schedar 샘플 사용자 음색 승인 — 현재 최신

**상태: `SAMPLE_SAVED / USER_VOICE_APPROVED / REMAINING_9_NOT_AUTHORIZED`.**

- 사용자 PC에서 `M18_Serin_Schedar_SAMPLE_ONE.py --key-slot 2 --approve-one-call` 및 `YES`로 `main18_ground_serin_01` 샘플 **1회** 요청·저장.

- 모델 `gemini-3.8-flash-tts`; Voice `Schedar`; 새 WAV `F:\\Downloads\\Voice\\Limitless_TTS_Serin10_Schedar_v2\\sample_candidate\\main18_ground_serin_01.wav`.

- 사용자 로그 SHA256 `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`; 로컬 파일 별도 해시 재검증은 아직 수행하지 않음.

- **사용자 직접 청취 발언: "목소리 제대로 나와." → 음색 `USER_VOICE_APPROVED`.** 원문 누락/단어 정확성까지 사용자가 명시적으로 PASS한 것은 아니므로 `EXACTTEXT_LISTENING_PENDING` 유지.

- 샘플은 정식 재제작 10개 중 1개로 **재사용, 중복 생성 금지**. 남은9개는 **별도 생성 승인 필요**; 아직 호출0. 정상 Main18·19 Unity 음성25, Main17 승인 세린11, 기존 오류 WAV 원본/백업 보호.

- 이후: 로컬 checkpoint와 샘플 hash 대조 → 현재 프로젝트·모델 무료 quota/과금 확인 → **나머지9개 명시적 승인** → 9개만 StoryOrder대로 생성/QA → 사용자의 전체 청취 → Codex 별도 Unity 통합.

- 이 항목이 상단 과거 `SAMPLE_NOT_EXECUTED`를 대체. 현재 상태는 샘플 성공1·잔여9 잠금.

## 2026-10-13 나머지9 생성기 오프라인 준비 — 최신

- **사용자가 남은9개 제작 명시 승인**, 샘플 `main18_ground_serin_01` 재사용. 그러나 이 ChatGPT 작업에서 실제 Gemini 호출0·신규 WAV0 (사용자 PC 실행 대기).

- 사용자 다운로드용 `M18_Serin_Schedar_REMAINING9.py`, SHA256 `94b75a9f0c7901bb4a94b2f0da017985036075809a593ee6c5838196a6a46c76`. SAFE v2와 샘플 생성 Python 2개 SHA 고정, 기존 25 정상/원본10/백업10/샘플 SHA 검증.

- `check`는 API0, `run --key-slot 2 --max-new-calls 9 --approve-remaining-nine`는 사용자가 **실제 무료 잔여량과 Billing을 직접 확인하고 YES** 입력해야 시도. Project+Model quota, 키 변경에 따른 신규 예산 가정 금지. 쿼터 불확실 시 `--max-new-calls` 줄이기.

- 기존 샘플 제외 Story `02,07,08,09,10,11,12,13,14` 순서. 자동 retry0, 호출 25초 간격, 실패/429 즉시 HOLD, 기존 성공 WAV 재생성 차단. 결과 Stage 폴더의 `remaining9_wav`와 StoryOrder CSV.

- 오프라인 mock 정상9회/둘째429 즉시 중단 테스트 통과. 실제 SDK/사용자 PC 진행 여부는 아직 `NOT_EXECUTED`/후속 스크린샷 필요.

- 상세: [나머지9 생성기 보고](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md). 이번 문서는 `docs/tts` 전용, main 변경/병합0, Unity 변경0.

## 2026-10-13 잔여9 실제 제작 부분 성공 / Story13 SAFE HOLD — 최신 상태

사용자 PC PowerShell 스크린샷으로 `M18_Serin_Schedar_REMAINING9.py` 실행 결과 확인:

- 승인 샘플 Story06 1개 **재사용**(신규 호출0).

- Story02,07,08,09,10,11,12 `[SAVED]` **7 WAV 신규 생성**(ID별 SHA 출력, 실제 바이트 검사 별도 대기).

- Story13 `main18_watcher_witness_serin_01` `[API REQUEST]` 후 `[SAFE HOLD] / RuntimeError` **FAILED_OR_UNCERTAIN_HOLD**; 세부 원인 UNKNOWN, 429라고 단정하지 않음. Story14 `main18_watcher_witness_serin_02` **NOT_CALLED**.

- **전체10: SAVED8(샘플1+7), 불확실1, 미호출1.** 지금까지 생성 시도 총9(기존 샘플1+이번8)로 관측. 전체10 사용자 청취 승인 아직 아님.

- **중요: 추가 API0, 같은 ID 자동 재시도/새 Key/다음 Story14 호출/잠금 및 checkpoint 초기화 전부 금지.** 다음은 Codex가 사용자 로컬 오류로그·attempt·HOLD·lock·생성 파일의 SHA를 읽기 전용 대조하고 결과 보고. 재개는 별도 승인과 현재 무료 quota 재확인 후.

- GitHub 결과 문서: [나머지9 생성기 및 SAFE HOLD 기록](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md). 사용자 화면 근거, 독립 검사 미실시. Unity/Main 변경·WAV 적용0.

## 2026-10-13 API KEY Project 귀속 불일치 — 최신 조사 대기

- 사용자 Google AI Studio 10/13 Gemini3.8 Flash TTS RPD: 키1이라고 설명한 Default Gemini Project **9/10**, 키2라고 설명한 Project-Limitless **0/10**. 그래프상 관측값이며 정확한 실시간 잔여 호출 수 확인이 아님.

- 생성 CLI `--key-slot 2`인데 로그상 샘플 포함 API 시도9개. 키2 실제 프로젝트 연결 여부, 스크립트 환경변수 선택, Google 사용량 반영 지연 **모두 미확정**.

- Codex 정적 감사로 `--key-slot` 구현, 키→Project 대응(값 출력 금지), Story13 RuntimeError/잠금 체크포인트를 확인할 것. API 호출·추가 생성·HOLD 해제·키 변경 0.

- 현재 WAV 상태 **8 SAVED(샘플1+7), Story13 실패/불확실1, Story14 미호출1**, 정상25/원본10/백업10 보호. [세부 문서](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md).

## 2026-10-13 Codex 읽기 전용 감사 후 최신 인계 — SAFE HOLD 유지

바울 대표가 Codex 감사 완료 보고 화면 제공:

- 샘플1+신규 Story02/07~12 WAV7 = **8개 SHA256/checkpoint/PCM 대조 PASS**.

- Story13 `main18_watcher_witness_serin_01`: 시도+HOLD 기록 있음, 신규/부분 WAV 및 결과 JSON 없음. `RuntimeError` **예외 클래스만** 기록돼 정확한 실패 지점/429/할당량 여부 **UNKNOWN**.

- Story14 `main18_watcher_witness_serin_02`: 로컬 시도·결과·신규 WAV 없음 → 미호출로 기록.

- Codex 보고: 대상 폴더42 + 프로젝트 보호 파일5,199개 SHA 불변, HOLD·SESSION.lock·시도 기록 보존. 조사 중 API 0/WAV 수정삭제0/Unity0/Push0.

- **현재 상태 SAVED8 / FAILED_OR_UNCERTAIN_HOLD1 / NOT_CALLED1.** 기존 샘플 재생성 금지. Stage, checkpoint, lock 삭제·초기화 금지, Key 변경/Story14 선호출/Story13 재시도 금지.

- Google 두 프로젝트의 RPD 9/10 vs 0/10의 귀속 문제와 실제 KEY2 연동, quota/과금 위험은 확인되지 않았음. 새 요청에 대한 **재개 승인 없음**.

- 남은2개는 조건 확인 후 사용자 **별도 승인**을 받아 새로운 수동 재개 이력으로 처리. 전체 음성 청취/Unity 통합은 이후 별도 승인.

- 기록 정본: [잔여9 생성기 및 Codex 감사 후속](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md).

## 2026-10-13 Story07 사용자 음색 불일치 의심 — 최신 청취 상태

- 사용자 청취 인상: `main18_heat_vent_serin_01.wav` **“감기걸린 여자? 변성기 전 미소년?”**.

- **저장·SHA/PCM PASS와 별개로 음색 `USER_REPORTED_TIMBRE_MISMATCH / USER_REVIEW_REQUIRED / HOLD`**. 기존 승인 샘플 Story06과 음색 일치 확인 전 `KEEP` 또는 Unity 적용 불가. 현재 사용자가 최종 폐기/재제작 판정을 내린 것은 아니므로 `REGEN_REQUIRED`은 사용자 판단 대기.

- 사용자 PC 해당 WAV 자체 직접 재생하지 않았음. 기존8개 저장·Story13 실패불확실·Story14 미호출 상태 불변. 추가 API0, 기존 WAV/잠금/체크포인트 보존, main/Unity 변경0.

- [상세 청취 기록](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md).

## 2026-10-13 Story07 업로드 오디오 직접 비교 후 재제작 권고 — 최신

- 사용자 첨부 `main18_heat_vent_serin_01.wav` 4.52초, SHA256 `2426af7024c27cee7bd2bbd94883a65215a1ef406aedb2bd7dea2bf36a60c3b1`. 승인 샘플 `main18_ground_serin_01.wav` 6.52초, SHA256 `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`는 과거 사용자 샘플 로그 SHA와 동일.

- 실제 업로드 WAV 비교 결과 heat_vent는 사용자가 지적한 비음/탁한 질감이 승인 샘플과 달라 **`TTS_QA_RECOMMENDS_REGEN`**. 사용자 `REGEN 확정`이나 신규 생성 호출 승인은 아직 없음. 원본/체크포인트 보호.

- WAV 중앙 F0 각각 약175/180Hz; 음색 차이는 성별/음높이 단정이 아니라 캐릭터 일관성 문제. 업로드본이 로컬 성공 WAV와 동일한지 SHA 재대조 필요.

- 현재 저장성공8 유지, 그중 승인샘플 음색PASS1·Story07 재제작 권고1·나머지6 청취 대기; Story13 불확실, Story14 미호출. 추가 API0·Unity0·main0. 상세 [생성 및 WAV 비교 QA](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md).

## 2026-10-13 신규 TTS 6개 업로드 — 세린 음색 일관성 검수 확대

사용자가 Story02/08/09/10/11/12 WAV 여섯 개를 업로드하고 “다른 것도 조금씩 다르다”고 보고했다. 업로드 WAV 포맷과 F0 기초 검수: Story02 209.5, 08 198.2, 09 181.1, 10 189.7, 11 207.2, 12 211.0Hz 중앙값(참고용). 기준 승인 샘플 Story06 약180.2Hz. **음높이만으로 화자/재제작 필요 여부 판정 불가.**

- Story06 샘플은 사용자 음색 승인 유지.

- Story07은 사용자 불일치 보고 및 담당 재제작 권고, 기존 파일 보존.

- Story02/08~12 여섯 개는 **`USER_LISTENING_REQUIRED`**. 사용자 최종 KEEP/REGEN 판정 미정; 저장성공을 청취 PASS로 승격하지 않음.

- 전체 저장8개, Story13 실패/불확실 HOLD1, Story14 미호출1. 기존8개 중 사용자 음색 승인1, 재제작 권고1, 청취 판정 대기6.

- 추가 생성·재호출/잠금 해제 금지. 잔여 신규 제작·Story07 재제작은 프로젝트 quota 및 Billing 확인/사용자 명시적 별도 승인 후.

- 신규 음색 QA 상세: [TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md).

## 2026-10-13 정정 — 사용자 신규8 WAV 전부 청취 완료

사용자가 **“난 다 들어봤다고....”**라고 명확히 밝혔으므로 이전 '나머지6 청취 대기'는 오기다.

- 저장 성공8개 모두 사용자 직접 청취: `USER_LISTENED=8`. 다시 들어보라고 요구하지 말 것.

- Story06 ground 1개: `USER_VOICE_APPROVED`.

- Story07 heat_vent 1개: `USER_REPORTED_TIMBRE_MISMATCH / TTS_QA_RECOMMENDS_REGEN`.

- Story02 leon, Story08~11 overheat4, Story12 vibration1의 6개: `USER_REPORTED_TIMBRE_VARIANCE`; 각각 음색이 조금씩 달라 사용자가 직접 지적함. 사용자 파일별 최종 KEEP/REGEN 결정은 미표명, 추측 금지.

- Story13 실패/불확실 HOLD, Story14 미호출. 기존 WAV8/정상25/backup/LOCK 보호. 추가 API0, Unity0, main0.

- 정본: [생성기 및 사용자 청취 정정](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md)

## 2026-10-13 사용자 요청 오프라인 음색 보정 6 WAV — 미리듣기

사용자: **“heat_vent는 빼고 니가 못고쳐?”**. 이전에 업로드한 세린 `leon`, `overheat_01~04`, `vibration` WAV6개만 승인 `ground` 샘플 기준의 경미한 pitch/EQ/음량 보정으로 **`*_PREVIEW.wav`**를 제공했다. `heat_vent`는 제외. 기준 `ground` WAV는 변경 없이 ZIP 안에 동봉. 출력 ZIP `LIMITLESS_Serin6_ConsistentVoice_Previews.zip` (별도 다운로드 산출물), QA CSV 포함.

- WAV6개 샘플 길이 동일, 24kHz mono PCM16, clipping0, ZIP CRC PASS. 기존 원본/체크포인트/Unity WAV25/Story13 HOLD/기타 WAV 모두 무변경. Gemini API0.

- PREVIEW는 확정 교체본 아님. 동일 화자로 들리는지 사람 청취 최종 평가 전 `USER_APPROVAL_PENDING`. TTS 모델이 달리 만든 발성 질감을 DSP로 완전 복구한다는 의미 아님.

- 이전 관례의 기본 후처리 금지 정책에 따라 **정식 WAV를 덮어쓰지 않음**. 사용자 요청에 의한 비교 전용 새 파일만 제작.

- 상세: [생성 및 PREVIEW 기록](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md). `docs/tts`에만 기록, main 변경 없음.

## 2026-10-19 담당 인계 추가 기록 (origin/docs/tts)

## 오늘 결과
- **Main18 세린10 오류 전용 Schedar 안전 준비 v2** 작성 및 격리 fixture 테스트 완료. GitHub 기록: [TTS_Serin10_Schedar_v2_Preflight_20261013.md](../../TTS_Serin10_Schedar_v2_Preflight_20261013.md).
- 별도 배포 Python: `M18_Serin10_Schedar_v2_SAFE.py`, SHA256 `2edd62ff4c7cbc7fe055c665b39f35aaa5ef126691d1393c692fb42e8e1cd741`, 13,057 bytes. **Gemini/HTTP 함수 없음; sample 명령 항상 차단**.
- 명령: `python .\M18_Serin10_Schedar_v2_SAFE.py audit` (읽기 전용) → 로컬 결과 확인 → `python .\M18_Serin10_Schedar_v2_SAFE.py prepare` (독립 새 폴더에 오류 WAV10 SHA 검증 백업·잠금 보고).
- v1 `F:\Downloads\M18_Serin10_Staged.py` (Erinome/Kore만 허용) 실행 보류, 수정0.

## 확정 보호
- 세린 Voice Schedar 유지. Main18 오류10만 REGEN_REQUIRED, 사용자 청취 정상 Main18 세린5/Main19 세린7/Main17 승인 세린11 및 Unity 정상 통합 음성25 보호.
- 기존 오류10의 실제 당시 API Wire Voice는 UNKNOWN; 원인 확정 없음.
- 샘플 대상 `main18_ground_serin_01`, 원문/DialogueId/SpeakerId 변경 금지.
- 본 도구는 준비만. 샘플 생성 **NOT_EXECUTED**, 실제 사용자 LOCAL Stage 실행 **NOT_EXECUTED**.
- 사용자 무료 quota 잔량, Google Project별 모델 RPD 및 Billing 상태 **NOT_VERIFIED**, 사용 허용 확정 호출 **0**. 키 변경으로 quota 추가되었다고 가정 금지.

## 다음 선행조건
1. Codex 로컬 정적 대조 또는 사용자 `audit` 실행 결과 수령. `prepare` 성공 시 백업10 SHA, BASELINE/LOCKED 보고 확인.
2. 바울 대표가 Google AI Studio Projects/Billing, Dashboard Usage/Rate limits에서 **현재 해당 프로젝트·모델 무료 사용 가능 여부와 과금 위험 없음**을 확인.
3. 바울 대표의 명시적 **샘플 1회 호출 승인** 이후 새 1-call-generator 별도 코드 감사·실행. 실패/429/불확실한 경우 즉시 중단·재시도/키 변경 없음.
4. 사용자가 샘플 WAV의 실제 음색·원문 청취 PASS → 1개 정식 본으로 사용. 나머지9는 **별도 승인** 후 제작.
5. 전체 청취 승인 이후 Codex가 Unity 음성 연결. 담당 TTS는 Unity 수정하지 않음.

## GitHub 협업
- TTS 작업 브랜치: `docs/tts` 전용; `main` 직접 commit/push/merge 금지.
- 문서 갱신을 마쳤다면 해당 담당 커밋 SHA를 완료 보고에서 별도 확인.
- 세션 전환 시 최신 `main` 정본과 `docs/tts` 미병합 내용 양쪽 확인.

사용자가 실제 `py3_12` PowerShell 실행 화면을 제공:
- `M18_Serin10_Schedar_v2_SAFE.py audit` → `[AUDIT PASS] target=10, protected=25, Voice=Schedar, API=0`, 읽기 전용.
- `M18_Serin10_Schedar_v2_SAFE.py prepare` → `[PREPARE COMPLETE / API 0]`, 오류 WAV10 백업 SHA 검증·정상25 비교, Unity 수정0.
- 신규 외부 출력: `F:\Downloads\Voice\Limitless_TTS_Serin10_Schedar_v2`.
- **현재 상태 = `AUDIT_PASS / PREPARE_COMPLETE / SAMPLE_NOT_EXECUTED`**. 위쪽 과거 `LOCAL NOT_EXECUTED` 항목은 해당 시점 이전 기록.
- 사용자의 터미널 로그 확인이며 담당자 독립 로컬 파일 접근 검증은 아님.
- Gemini TTS 생성0 / WAV 신규 생성0 / 전체 무료 잔량·Billing 상태 `NOT_VERIFIED`. 생성기 없음·`sample` 실행 차단 유지.
- 다음은 사용자 AI Studio의 현재 해당 Project/모델 한도·과금 상태 확인 및 **샘플 한 번 명시적 승인**. 승인 후 안전한 별도 생성기 → 사용자의 실제 청취 승인 → 이후9개 별도 승인. 샘플 ID: `main18_ground_serin_01`, Voice `Schedar`.
- 현황 상세: [v2 사전 준비 및 LOCAL 실행 후속](../../TTS_Serin10_Schedar_v2_Preflight_20261013.md).

- 사용자 PC에서 `M18_Serin_Schedar_SAMPLE_ONE.py --key-slot 2 --approve-one-call` 및 `YES`로 `main18_ground_serin_01` 샘플 **1회** 요청·저장.
- 모델 `gemini-3.8-flash-tts`; Voice `Schedar`; 새 WAV `F:\\Downloads\\Voice\\Limitless_TTS_Serin10_Schedar_v2\\sample_candidate\\main18_ground_serin_01.wav`.
- 사용자 로그 SHA256 `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`; 로컬 파일 별도 해시 재검증은 아직 수행하지 않음.
- **사용자 직접 청취 발언: "목소리 제대로 나와." → 음색 `USER_VOICE_APPROVED`.** 원문 누락/단어 정확성까지 사용자가 명시적으로 PASS한 것은 아니므로 `EXACTTEXT_LISTENING_PENDING` 유지.
- 샘플은 정식 재제작 10개 중 1개로 **재사용, 중복 생성 금지**. 남은9개는 **별도 생성 승인 필요**; 아직 호출0. 정상 Main18·19 Unity 음성25, Main17 승인 세린11, 기존 오류 WAV 원본/백업 보호.
- 이후: 로컬 checkpoint와 샘플 hash 대조 → 현재 프로젝트·모델 무료 quota/과금 확인 → **나머지9개 명시적 승인** → 9개만 StoryOrder대로 생성/QA → 사용자의 전체 청취 → Codex 별도 Unity 통합.
- 이 항목이 상단 과거 `SAMPLE_NOT_EXECUTED`를 대체. 현재 상태는 샘플 성공1·잔여9 잠금.

## 2026-10-13 나머지9 생성기 오프라인 준비 — 최신
- **사용자가 남은9개 제작 명시 승인**, 샘플 `main18_ground_serin_01` 재사용. 그러나 이 ChatGPT 작업에서 실제 Gemini 호출0·신규 WAV0 (사용자 PC 실행 대기).
- 사용자 다운로드용 `M18_Serin_Schedar_REMAINING9.py`, SHA256 `94b75a9f0c7901bb4a94b2f0da017985036075809a593ee6c5838196a6a46c76`. SAFE v2와 샘플 생성 Python 2개 SHA 고정, 기존 25 정상/원본10/백업10/샘플 SHA 검증.
- `check`는 API0, `run --key-slot 2 --max-new-calls 9 --approve-remaining-nine`는 사용자가 **실제 무료 잔여량과 Billing을 직접 확인하고 YES** 입력해야 시도. Project+Model quota, 키 변경에 따른 신규 예산 가정 금지. 쿼터 불확실 시 `--max-new-calls` 줄이기.
- 기존 샘플 제외 Story `02,07,08,09,10,11,12,13,14` 순서. 자동 retry0, 호출 25초 간격, 실패/429 즉시 HOLD, 기존 성공 WAV 재생성 차단. 결과 Stage 폴더의 `remaining9_wav`와 StoryOrder CSV.
- 오프라인 mock 정상9회/둘째429 즉시 중단 테스트 통과. 실제 SDK/사용자 PC 진행 여부는 아직 `NOT_EXECUTED`/후속 스크린샷 필요.
- 상세: [나머지9 생성기 보고](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md). 이번 문서는 `docs/tts` 전용, main 변경/병합0, Unity 변경0.

사용자 PC PowerShell 스크린샷으로 `M18_Serin_Schedar_REMAINING9.py` 실행 결과 확인:
- 승인 샘플 Story06 1개 **재사용**(신규 호출0).
- Story02,07,08,09,10,11,12 `[SAVED]` **7 WAV 신규 생성**(ID별 SHA 출력, 실제 바이트 검사 별도 대기).
- Story13 `main18_watcher_witness_serin_01` `[API REQUEST]` 후 `[SAFE HOLD] / RuntimeError` **FAILED_OR_UNCERTAIN_HOLD**; 세부 원인 UNKNOWN, 429라고 단정하지 않음. Story14 `main18_watcher_witness_serin_02` **NOT_CALLED**.
- **전체10: SAVED8(샘플1+7), 불확실1, 미호출1.** 지금까지 생성 시도 총9(기존 샘플1+이번8)로 관측. 전체10 사용자 청취 승인 아직 아님.
- **중요: 추가 API0, 같은 ID 자동 재시도/새 Key/다음 Story14 호출/잠금 및 checkpoint 초기화 전부 금지.** 다음은 Codex가 사용자 로컬 오류로그·attempt·HOLD·lock·생성 파일의 SHA를 읽기 전용 대조하고 결과 보고. 재개는 별도 승인과 현재 무료 quota 재확인 후.
- GitHub 결과 문서: [나머지9 생성기 및 SAFE HOLD 기록](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md). 사용자 화면 근거, 독립 검사 미실시. Unity/Main 변경·WAV 적용0.

## 2026-10-13 API KEY Project 귀속 불일치 — 최신 조사 대기
- 사용자 Google AI Studio 10/13 Gemini3.8 Flash TTS RPD: 키1이라고 설명한 Default Gemini Project **9/10**, 키2라고 설명한 Project-Limitless **0/10**. 그래프상 관측값이며 정확한 실시간 잔여 호출 수 확인이 아님.
- 생성 CLI `--key-slot 2`인데 로그상 샘플 포함 API 시도9개. 키2 실제 프로젝트 연결 여부, 스크립트 환경변수 선택, Google 사용량 반영 지연 **모두 미확정**.
- Codex 정적 감사로 `--key-slot` 구현, 키→Project 대응(값 출력 금지), Story13 RuntimeError/잠금 체크포인트를 확인할 것. API 호출·추가 생성·HOLD 해제·키 변경 0.
- 현재 WAV 상태 **8 SAVED(샘플1+7), Story13 실패/불확실1, Story14 미호출1**, 정상25/원본10/백업10 보호. [세부 문서](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md).

바울 대표가 Codex 감사 완료 보고 화면 제공:
- 샘플1+신규 Story02/07~12 WAV7 = **8개 SHA256/checkpoint/PCM 대조 PASS**.
- Story13 `main18_watcher_witness_serin_01`: 시도+HOLD 기록 있음, 신규/부분 WAV 및 결과 JSON 없음. `RuntimeError` **예외 클래스만** 기록돼 정확한 실패 지점/429/할당량 여부 **UNKNOWN**.
- Story14 `main18_watcher_witness_serin_02`: 로컬 시도·결과·신규 WAV 없음 → 미호출로 기록.
- Codex 보고: 대상 폴더42 + 프로젝트 보호 파일5,199개 SHA 불변, HOLD·SESSION.lock·시도 기록 보존. 조사 중 API 0/WAV 수정삭제0/Unity0/Push0.
- **현재 상태 SAVED8 / FAILED_OR_UNCERTAIN_HOLD1 / NOT_CALLED1.** 기존 샘플 재생성 금지. Stage, checkpoint, lock 삭제·초기화 금지, Key 변경/Story14 선호출/Story13 재시도 금지.
- Google 두 프로젝트의 RPD 9/10 vs 0/10의 귀속 문제와 실제 KEY2 연동, quota/과금 위험은 확인되지 않았음. 새 요청에 대한 **재개 승인 없음**.
- 남은2개는 조건 확인 후 사용자 **별도 승인**을 받아 새로운 수동 재개 이력으로 처리. 전체 음성 청취/Unity 통합은 이후 별도 승인.
- 기록 정본: [잔여9 생성기 및 Codex 감사 후속](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md).

## 2026-10-13 Story07 사용자 음색 불일치 의심 — 최신 청취 상태
- 사용자 청취 인상: `main18_heat_vent_serin_01.wav` **“감기걸린 여자? 변성기 전 미소년?”**.
- **저장·SHA/PCM PASS와 별개로 음색 `USER_REPORTED_TIMBRE_MISMATCH / USER_REVIEW_REQUIRED / HOLD`**. 기존 승인 샘플 Story06과 음색 일치 확인 전 `KEEP` 또는 Unity 적용 불가. 현재 사용자가 최종 폐기/재제작 판정을 내린 것은 아니므로 `REGEN_REQUIRED`은 사용자 판단 대기.
- 사용자 PC 해당 WAV 자체 직접 재생하지 않았음. 기존8개 저장·Story13 실패불확실·Story14 미호출 상태 불변. 추가 API0, 기존 WAV/잠금/체크포인트 보존, main/Unity 변경0.
- [상세 청취 기록](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md).

## 2026-10-13 Story07 업로드 오디오 직접 비교 후 재제작 권고 — 최신
- 사용자 첨부 `main18_heat_vent_serin_01.wav` 4.52초, SHA256 `2426af7024c27cee7bd2bbd94883a65215a1ef406aedb2bd7dea2bf36a60c3b1`. 승인 샘플 `main18_ground_serin_01.wav` 6.52초, SHA256 `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`는 과거 사용자 샘플 로그 SHA와 동일.
- 실제 업로드 WAV 비교 결과 heat_vent는 사용자가 지적한 비음/탁한 질감이 승인 샘플과 달라 **`TTS_QA_RECOMMENDS_REGEN`**. 사용자 `REGEN 확정`이나 신규 생성 호출 승인은 아직 없음. 원본/체크포인트 보호.
- WAV 중앙 F0 각각 약175/180Hz; 음색 차이는 성별/음높이 단정이 아니라 캐릭터 일관성 문제. 업로드본이 로컬 성공 WAV와 동일한지 SHA 재대조 필요.
- 현재 저장성공8 유지, 그중 승인샘플 음색PASS1·Story07 재제작 권고1·나머지6 청취 대기; Story13 불확실, Story14 미호출. 추가 API0·Unity0·main0. 상세 [생성 및 WAV 비교 QA](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md).

- Story06 샘플은 사용자 음색 승인 유지.
- Story07은 사용자 불일치 보고 및 담당 재제작 권고, 기존 파일 보존.
- Story02/08~12 여섯 개는 **`USER_LISTENING_REQUIRED`**. 사용자 최종 KEEP/REGEN 판정 미정; 저장성공을 청취 PASS로 승격하지 않음.
- 전체 저장8개, Story13 실패/불확실 HOLD1, Story14 미호출1. 기존8개 중 사용자 음색 승인1, 재제작 권고1, 청취 판정 대기6.
- 추가 생성·재호출/잠금 해제 금지. 잔여 신규 제작·Story07 재제작은 프로젝트 quota 및 Billing 확인/사용자 명시적 별도 승인 후.
- 신규 음색 QA 상세: [TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md).

- 저장 성공8개 모두 사용자 직접 청취: `USER_LISTENED=8`. 다시 들어보라고 요구하지 말 것.
- Story06 ground 1개: `USER_VOICE_APPROVED`.
- Story07 heat_vent 1개: `USER_REPORTED_TIMBRE_MISMATCH / TTS_QA_RECOMMENDS_REGEN`.
- Story02 leon, Story08~11 overheat4, Story12 vibration1의 6개: `USER_REPORTED_TIMBRE_VARIANCE`; 각각 음색이 조금씩 달라 사용자가 직접 지적함. 사용자 파일별 최종 KEEP/REGEN 결정은 미표명, 추측 금지.
- Story13 실패/불확실 HOLD, Story14 미호출. 기존 WAV8/정상25/backup/LOCK 보호. 추가 API0, Unity0, main0.
- 정본: [생성기 및 사용자 청취 정정](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md)

- WAV6개 샘플 길이 동일, 24kHz mono PCM16, clipping0, ZIP CRC PASS. 기존 원본/체크포인트/Unity WAV25/Story13 HOLD/기타 WAV 모두 무변경. Gemini API0.
- PREVIEW는 확정 교체본 아님. 동일 화자로 들리는지 사람 청취 최종 평가 전 `USER_APPROVAL_PENDING`. TTS 모델이 달리 만든 발성 질감을 DSP로 완전 복구한다는 의미 아님.
- 이전 관례의 기본 후처리 금지 정책에 따라 **정식 WAV를 덮어쓰지 않음**. 사용자 요청에 의한 비교 전용 새 파일만 제작.
- 상세: [생성 및 PREVIEW 기록](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md). `docs/tts`에만 기록, main 변경 없음.

## 2026-10-19 보정본 6개 재전달 / 다음 Main18·20·21 제작 사전준비

사용자: “니가 수정한거 아직 안받았지 나? 일단 수정한 주고 더 뽑을 꺼 준비해줘.”

- 기존 `LIMITLESS_Serin6_ConsistentVoice_Previews.zip` 실제 파일 재확인, SHA256 `a5878e6d4f1b1e858f4b505f23669ab147da2dd466fe30443b0d6116a7246cb1`; ZIP CRC와 보정 WAV6 형식 PASS. 사용자 대화에 재다운로드 링크 제공. `heat_vent` 제외, `ground` 기준 음성 포함. PREVIEW는 아직 정식 Unity 승인 아님.
- 새 `LIMITLESS_TTS_Next_Preflight_20261019.py`(읽기 전용) 및 25행 다음 제작 큐/README/Codex 조사 지시가 포함된 `LIMITLESS_TTS_Next_Preflight_Pack_20261019.zip` 제공. SHA `de2036d1557399fa34aa02257ecd61c6cf887585786c8fbacf6465d9ca5c55e9`. 사용자 PC F: 실행 `NOT_EXECUTED`.
- 격리 fixture 정상 preflight 및 SHA 손상 중단 테스트 PASS. Google TTS SDK/API/키 사용0, 오프라인 읽기만 실행 가능.
- **Main18 다음 3개:** Story07 heat_vent 음색 문제 재제작 후보 (기존 SAVED 유지), Story13 watcher_01 FAILED_OR_UNCERTAIN_HOLD(잠금/시도 기록 보존), Story14 watcher_02 NOT_CALLED (Story13 선호출/건너뛰기 금지).
- **이후 Main20 세린 5개**, **Main21 NPC17개 중 하렌3 Voice 미승인 BLOCK / 기타14 후보**. 3+5+14=22는 향후 잠재 호출 수일 뿐 현재 quota 확인 수치 아님. 정확한 LOCAL Runtime 순서/기존 WAV 존재 먼저 확인.
- 2026-10-19 프로젝트별 실시간 RPD 잔여·Billing/키 소속 `NOT_VERIFIED`, 추가 API 요청 승인0. 이전 성공8 및 Preview6 원본 유지, 승인 없이 LOCK 해제·재시도·다른 Key 사용 금지.
- 상세: [보정본 재전달 및 다음 배치 계획](../../TTS_Serin_Preview6_and_NextBatch_Preflight_20261019.md). 기록은 `docs/tts` 브랜치만, `main` 변경 없음. Unity 수정0.

## 2026-10-19 사용자 로컬 PRE-FLIGHT PASS — 최신 확인

- 사용자 PowerShell 화면에서 실제 `LIMITLESS_TTS_Next_Preflight_20261019.py check`가 `[PRE-FLIGHT PASS]`를 반환.
- Unity 정상25 SHA PASS, 샘플 ground 원본 SHA PASS, Story02/07~12 성공7 WAV·시도 SHA PASS.
- Story13 `SESSION.lock`, failed/uncertain attempt, HOLD 유지; 신규 WAV/result 없음. Story14 새 attempt/result/WAV 없음.
- 추가·중복 attempt 없음. Main20 5 세린 예정, Main21 17 NPC 중 하렌3 BLOCK.
- `[PASS]`는 생성 허가 아님. 지금 quota/project/billing `NOT_VERIFIED`; TTS API0 / writes0 / Unity0 / Git writes0으로 사용자 화면에 표시.
- 사용자에 다시 preflight를 요구할 필요 없음. 이후 수동 재개는 HOLD 원인 판독과 Project+Model 사용 가능 무료 횟수 확인·비용 통제·별도 사용자 승인 후에만 진행.
- 세부 [보정본 및 다음 배치 사전검사](../../TTS_Serin_Preview6_and_NextBatch_Preflight_20261019.md).

## 2026-10-19 Main18 마지막3 수동복구 생성기 준비 (최신)

사용자: **“그래, 그럼 tts 작업 마저 하자”**. 기존 8 WAV·승인 ground/보정 Preview6을 재생성하지 않고 **Story07 heat_vent 재제작, Story13 watcher_01 불확실 실패 신규 수동 시도, Story14 watcher_02 신규 제작**만 진행하도록 생성기 준비.

- 배포: `LIMITLESS_Main18_Serin3_Recovery_20261019.zip`, SHA256 `0b580820b4d0c2bbedd8e86266f38054312f0b641a1bce452b0c64b72a76650d`.
- Python `LIMITLESS_Main18_Serin3_Recovery_20261019.py` SHA256 `799e1492312199de21daad014786ae636350f3944611c621907e37854a7df191`.
- `check`: 기존 10오류 원본+10백업, Unity 정상25, 승인 샘플 SHA, 성공 신규7, Story13 기존 HOLD+SESSION.lock, Story14 미호출, 새로운 복구 기록 검사. API0/쓰기0. 2026-10-19 사용자 PC 실행 상태 `NOT_EXECUTED`.
- `run`: 현재 Key2 연결된 실제 Google Project + model 무료 사용 가능량/비용을 **사용자가 직접 확인**하고 `--confirm-free-quota` + 대화형 `FREE`/`YES` 조건을 만족할 때만 요청. Story13가 포함되면 **`--authorize-manual-story13-retry` + `RETRY13`** 추가 확인. 같은 KEY SLOT2 사용, 자동 재시도0, 최대3회, 최소26초 간격.
- Output `F:\Downloads\Voice\Limitless_TTS_Serin10_Schedar_v2\ManualRecovery_20261019\candidates\`. 기존 `remaining9_*`·오류 시도·HOLD/SESSION.lock **절대 삭제/초기화하지 않음**. 신규 별도 checkpoint만 기록. 실패/429 즉시 새로운 HOLD 잠금 후 뒤 ID 중단.
- 오프라인 mock 3회 연속 성공·두번째429시 즉시중단·기존 HOLD 불변·성공 후보 SHA 변조 감지 PASS. 실제 Gemini TTS 호출0, 실제 무료 quota/Billing `NOT_VERIFIED`, 신규 WAV0.
- 새 후보는 사용자 실제 음색/원문 청취 승인 필요. 승인후 Codex Unity 별도 적용. Main20 세린5, Main21 17 중 하렌3 BLOCK은 향후 별도 제작, 이번 생성기 대상 아님.
- 상세: [Main18 세린3 수동복구 도구와 승인 게이트](../../TTS_Main18_Serin3_ManualRecovery_20261019.md). 이번 작업은 `docs/tts` 전용, `main` 및 Unity 수정0.

## 2026-10-19 최신: Main18 세린3 수동복구 사용자 실행 성공

- 사용자 제공 PowerShell 결과: `Story07 main18_heat_vent_serin_01` SAVED(4.88s), `Story13 main18_watcher_witness_serin_01` SAVED(4.96s), `Story14 main18_watcher_witness_serin_02` SAVED(4.20s). `[SUCCESS] New candidate WAV 3 / no Unity changes / legacy HOLD preserved`.
- 이번 실제 생성 요청 **3회 성공**. 사용자 PC 로그 근거이며 신규 세 WAV의 독립 SHA/PCM/의미 청취 검증은 아직 대기.
- 새로운 세 후보는 `ManualRecovery_20261019\candidates\` 아래 보관하고, 기존 실패 HOLD/SESSION.lock 및 과거 checkpoint 변경하지 않음.
- Main18 세린10 **후보 WAV 10개 확보**: 기존 ground 승인 샘플1 + 보정 PREVIEW6 + 신규 후보3. 사용자 전체10 최종 음색·원문 승인 아직 아님. 정상 Unity25와 Main17 승인본 보호.
- Main18 추가 생성은 현재 불필요. 다음은 새 후보3만 사용자 청취 QA, 필요시 Preview6 정식 승인 분리, 이후 Main20 세린5 / Main21 NPC14(하렌3 음성 미확정 BLOCK) 별도 생성·무료 quota 확인.
- 이번 담당은 Github `docs/tts` 기록 외 신규 API 호출0, Unity/main 변경0.
- 상세: [Main18 3개 복구 완료 후 후보 상태](../../TTS_Main18_Serin3_ManualRecovery_20261019.md).

## 2026-10-19 Main18 세린 10개 최종 사용자 승인 → Codex Unity 통합 위임 — 최신 상태

사용자 최신 명시적 지시: **“다 이상 없으니까 메인18은 끝인거 같으니 게임에 넣자, 지금 파일이 여기저기에 있으니까 알어서 할 수 있게 코덱스한테 시켜줘.”**
- **Main18 세린 교체 TTS10 사용자 최종 청취·Unity 적용 승인**. 과거 `USER_LISTENING_PENDING`/오류 음색 이력은 이 지시로 후보 세트에 대해 변경. 승인된 유일한 최종 소스는 `ground` 1, 오프라인 보정 PREVIEW6, 10/19 최신 수동 복구 후보3. **기존 잘못된 raw `remaining9_wav` 또는 원본 `Limitless_TTS_Output_Main18`의 불합격 TTS를 Unity에 넣지 말 것**.
- **Codex 구현 이슈:** https://github.com/HappyPaul812/Project-Limitless/issues/17
- **TTS→Codex 실제 파일/SHA/검증 인계:** [TTS_Main18_Serin10_Approved_Unity_Handoff_20261019.md](../../TTS_Main18_Serin10_Approved_Unity_Handoff_20261019.md), `docs/tts` 작업 브랜치.
- 샘플 `F:\Downloads\Voice\Limitless_TTS_Serin10_Schedar_v2\sample_candidate\main18_ground_serin_01.wav` SHA `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`. PREVIEW6 ZIP `LIMITLESS_Serin6_ConsistentVoice_Previews.zip` SHA `a5878e6d4f1b1e858f4b505f23669ab147da2dd466fe30443b0d6116a7246cb1`. 복구3 `ManualRecovery_20261019\candidates`의 Story07/13/14, SHA는 로컬 results에서 확인.
- 기존 Main18 정상12+Main19 정상13 **Unity 연결25 보호**, 기존 Catalog/GUID/Save/기존 WAV/Story13 HOLD/lock 보호. **Codex는 `codex/tts-main18-serin10-integration-20261019` 독립 로컬 worktree**로 실제 Unity 추가10·Main18 22/22 QA 수행. `main` 직접 commit/Push/merge0, 선별 로컬 commit만.
- **현재 상태:** `USER_APPROVED_10 / CODEX_ASSIGNED_VIA_GITHUB_ISSUE / UNITY_INTEGRATION_PENDING`. 이슈를 만든 것만으로 Codex 로컬 실행·Unity 통합 성공을 뜻하지 않음. 대표가 Codex 세션에서 이슈 링크를 전달하거나 작업을 시작해야 함.
- TTS API 호출0, 본 TTS 세션 Unity 파일 수정0. Codex 완료 보고에 SHA 매핑·최종 Main18 22/22·QA/브랜치/로컬 commit와 Push 여부 기재 요구.

## 2026-10-19 키1 7회·키2 10회 보고 / Main20·Main21 17회 배분 (최신)

- 사용자 현재 잔여 호출 보고: Key1=7 / Key2=10. 서로 다른 Google Project+gemini-3.8-flash-tts의 실제 FreeTier 사용량·Billing 검증 **NOT_VERIFIED**; 각 키당 독립 사용 가능 시에만 최대17. 동일 Project면 합산 금지.
- 제작 큐: Main20 세린5 + Main21 비하렌 NPC12 = 17. Key1: Main20 5+Main21 2, Key2: Main21 10. Main21 미생성2: `main21_haren_taeon_02`, `main21_prepare_serin_01`. 하렌3 Voice미확정 BLOCK.
- [계획과 Codex 로컬 실행 지시](../../TTS_Main20_Main21_17Call_Plan_20261019.md) 및 [17행 CSV 큐](../../TTS_Main20_Main21_17Call_Queue_20261019.csv) 기록. 기존 Manifest·Main18 세린 승인10·정상25 불변.
- 이번 TTS 세션에서는 Google API 실제 호출0 / 사용자 로컬 WAV0 / Unity0 / main 변경0. Codex 실행 대기, 신규 음성 모두 사용자 청취 승인 전 Unity 적용 금지.

## 2026-10-19 역할 재정정 / 직접 Python 스크립트 제작 (현재 최신)

- 바울 대표가 **“TTS는 니가 파이션 스크립트로 하는건데 누구한테 시키니?”**라고 업무 책임을 명확히 교정. TTS 스크립트 작성/음성 제작 계획/품질 검수는 TTS 담당 책임, 실제 Gemini API는 바울 대표 로컬 PC의 Python에서 실행. **Codex는 사용자 청취 승인 후 WAV Unity 통합·QA만** 담당한다. 이전 Codex 생성 위임 지시는 철회.
- Main20 세린5 + Main21 비하렌 일반12, KEY1=7/KEY2=10 고정 큐. 하렌 Voice 미승인3 BLOCK, 이번 quota 이월 Main21 일반2.
- TTS 담당이 직접 `LIMITLESS_TTS_Main20_Main21_17_Python_Pack_20261019.zip` 작성·다운로드 제공. ZIP 9874 bytes SHA256 `1a5a80afc98023be8324809e5fd6e7dad8cd12eccb852843525bf132c75bf612`. 포함 Python/17행 CSV/README. Google 비밀키 없음.
- 실행: `python .\\LIMITLESS_TTS_Main20_Main21_17.py check` (API0) → 실제 별도 Project+Model 무료 quota/Billing 확인 후 `python .\\LIMITLESS_TTS_Main20_Main21_17.py run --confirm-free`. 실제 사용자 PC API0/WAV0이며 앞으로 사용자가 로컬에서 실행하여 로그 공유.
- Python compile/읽기 전용 CSV SHA·키배분 점검 PASS, 오프라인 모의 성공17/보존, 실패2번째 즉시 HOLD/LOCK·뒤호출0 및 저장 WAV SHA 변조 검출 PASS. **실제 Google 서비스 요청/성공/청취는 미검증**.
- 최초 잘못 만들었던 이슈 #20을 **TTS 담당 제작 추적 이슈**로 수정하여 Codex 생성 요청을 명시적으로 취소. 정정 정본: [17회 계획/스크립트](../../TTS_Main20_Main21_17Call_Plan_20261019.md), [17개 큐](../../TTS_Main20_Main21_17Call_Queue_20261019.csv).
- Main18 세린 승인10 별도 Unity 통합 이슈 #17, Main18/19 기존 정상25·원본/HOLD/WAV/Save 변경0. docs/tts만 문서 갱신, main commit/merge/Push0.

## 2026-10-19 Main20·21 사용자 청취 오류9개 / 샘플 우선 재제작 (현재 최신)

- 바울 대표가 실제 청취 후 **세린8개 남성 음색**(`main20_after_colossus_serin_02/01`, `main20_confront_serin_03/02/01`, `main21_route_serin_01`, `main21_pulse_serin_01`, `main21_heat_serin_01`), **태온1개 여성 음색**(`main21_route_taeon_01`) 보고. 이9개는 청취 불합격·Unity 적용 보류. 나머지 8개는 별도 최종 합격 확정 아님.
- 원래 17회 제작 Python의 Style에 성별 명시 부족 확인. 실제 모델 오류 원인/원격 voice wire/실제 사용자 파일의 바이트 검증은 **미확정**, 성별 Style 지시가 효과를 보증하지 않음.
- TTS 담당 직접 `LIMITLESS_Main20_Main21_VoiceFix9_SampleFirst_20261019.zip` 제작/배포, SHA256 `28c130098d1e33d7889ffd79dce715ace992283885b7e01c8cdca59255160dcb`, 자체 Python `LIMITLESS_TTS_VoiceFix9.py` SHA256 `8440a69fc7bc2031da7c592e257e4bd469efeab16be25f7efdd2dfbc667c50ca`. 기존 Stable ID/원문/Voice Schedar/Gacrux 유지, 명시적 세린 여성/태온 남성 Style 추가. **세린1+태온1 샘플 먼저** 제작·사용자 청취 승인 → 나머지 세린7 별도 실행. 오류 시 즉시 HOLD, 원본17 SHA/체크포인트 변경0.
- 압축 파일과 실행 안내는 사용자 대화에서 직접 제공, README 동봉. 일단 `python .\\LIMITLESS_TTS_VoiceFix9.py check` (API0), 새로운 무료 한도를 현재 해당 프로젝트에서 확인한 뒤에만 `sample-serin`/`sample-taeon`. **초기 키1 7·키2 10은 이미 소비됐을 수 있으므로 추가9회라고 가정 금지**.
- 오프라인 compile, 기존17 SHA mock, 샘플2+나머지7 모의 성공 및 2번째 API 오류에 즉시 HOLD/lock/다음호출0 PASS. 실제 사용자 PC/실제 Gemini API **신규 호출0, WAV 신규0**; 샘플 QA 미확인, Unity0, main0.
- 상세 새 QA 정본: [Main20·21 VoiceFix9 오류/검수](../../TTS_Main20_Main21_VoiceFix9_20261019.md). 기존 이슈 #20은 생산→청취 QA 오류/재제작 추적용으로 갱신. TTS 담당이 Python을 직접 만들고 Codex는 사용자 최종 승인 후 Unity 통합만 한다.
