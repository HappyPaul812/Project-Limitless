# LIMITLESS TTS / 2026-10-24 태온 잔여 8개 이후 제작할 음성 감사 및 하렌 3개 실행 준비

**담당 최신 정정:** 아래 절차의 **TTS 합성·PowerShell 실행·API 사용자는 사용자 본인**이다. “Codex가 로컬 파일 담당”이라는 구 표현이 있어도 TTS 생성을 Codex에게 위임하는 의미로 사용하지 않는다. Codex는 별도 승인 후 읽기 전용 조사 및 최종 Unity 연동만 담당.


**사용자 요청:** “이따 8개 뽑으면 또 뽑을꺼 있니? 있으면 작업 폴더 `F:\Downloads\Voice`로 알아서 해줘. api키도 거기 있어.”

**상태:** NEXT_AFTER_TAEON_8 / HAREN3_SCRIPT_PREPARED_AND_OFFLINE_QA_PASS / GOOGLE_API_0 / LOCAL_F_DRIVE_NOT_ACCESSED / UNITY_PENDING
**브랜치:** `docs/tts`(스크립트 및 기록), `main` 변경 금지.

## 1. 본작업 확정 범위 — Main20·21 누락을 중복 없이 전수 매핑

- 2026-10-23 사용자 마지막 실제 로그: 세린67/67, 태온77/85, 전체144/152, 10월23일 PT 시도40/40, HOLD=False. **남은 태온 8개**는 2026-10-24 PT 날짜 시작 후 기존 `Renewal_Auto_TTS_Failover_v2.py`로 먼저 완료. 완료 예상152/152은 **사용자 다음 실행 증거가 필요**하며 아직 완료로 기록하지 않는다.
- [Main20 TTS 정본](Main20_TTS_Manifest.csv): `VoiceExpected=true` **세린5개**, 152 manifest에 모두 포함.
- [Main21 TTS 정본](../03_스토리/Main21_TTS_Manifest_20261010.json): `VoiceExpected=true` **17개** 전수 검토 — 세린/태온 신규 캐스팅 manifest **7개**, 사용자 KEEP한 기존 폴3·미엘2·레온2 **7개**, 하렌 `story_haren` **3개**. **누락된 Main21 DialogueId 0개**(이 셋의 합집합으로 전부 커버). Main20+21 정본 신규/재사용 계획에 빠진 `VoiceExpected=true` ID 없음.
- **따라서 태온8개 이후 추가 확정 제작 대상은 하렌3개뿐.** 기존 Main21 7개는 `KEEP / TTS_NOT_REQUIRED / UNITY_PENDING`, 재생성 금지. 다른 챕터 Story 보이스의 전체 누락·오류는 이 한정 감사로 완전 부정할 수 없으며, Unity 일괄 통합 직전 Codex가 실제 StoryVoiceCatalog 및 로컬 WAV를 최신 정본과 읽기 전용 전수 대조한다.
- Chapter3 Main22·23/북부 마을은 일부 NPC 이름, DialogueId/TTS 보이스가 TBD 또는 기획·구현 분리 상태인 문서가 있으므로 **정확한 원문과 Voice 정식 승인 전 자동 생성 금지**. 일반적인 다수 후보를 임의로 제작하거나 다른 캐릭터를 전면 재캐스팅하지 않는다.

## 2. 하렌 정식 3대사(사용자 기존 승인 범위)

| 번호 | Stable Dialogue ID | 화자 | Voice | 게임 정본 텍스트 | 청취 QA |
| --- | --- | --- | --- | --- | --- |
| 01 | `main21_haren_haren_01` | `story_haren` | `Alnilam` | **태온. 너 아직 멀었어.** | 평서문, 말끝 *하강*, 되묻거나 조롱하는 억양 금지 |
| 02 | `main21_haren_haren_02` | `story_haren` | `Alnilam` | **다친 데는 없고?** | 자연스러운 의문문 |
| 03 | `main21_haren_haren_03` | `story_haren` | `Alnilam` | **그럼 됐어.** | 짧고 담담한 평서문 하강 |

기존 첫 하렌 시험 WAV는 끝이 질문처럼 올라가 **재사용 금지**(원본 샘플 삭제나 덮어쓰기 금지). 첫 음성 1건 시범 생성→사용자 실제 청취 PASS 후 02/03. 이 합성은 ChatGPT 환경에서는 실제 사용자 F:나 API 키에 접근할 수 없어 수행하지 않았다.

## 3. 작동 가능한 로컬 실행기 및 납품

- GitHub **[`Tools/TTS/Haren_Main21_AfterRenewal_3.py`](../../Tools/TTS/Haren_Main21_AfterRenewal_3.py)** 는 본 브랜치에 신규 등록. (상대 경로는 프로젝트 루트 기준 `Tools/TTS/Haren_Main21_AfterRenewal_3.py`.)
- 사용자 전달 ZIP: `LIMITLESS_TTS_Haren3_SharedQuota_AfterTaeon_20261024.zip` (8684 bytes, SHA256 `8a44b807cbac8a306de2e9d8b6d4b9a1dd86607e9f734a0607e373c4be001661`). 파일 3개: 실행기 Python, 한글 README, Codex용 지시문. **비밀키, 생성 WAV 없음**.
- 사용자 PC 배치: `F:\Downloads\Voice\Haren_Main21_AfterRenewal_3.py`. 출력: `F:\Downloads\Voice\Haren_Main21\Haren\{StableId}.wav` + 동일명 JSON + `dialogues.csv` + frozen 3 manifest. 기존 `candidates` 7개를 확인만 하고 건드리지 않음.
- 기존 `F:\Downloads\Renewal_Voices\Renewal_Auto_TTS_Failover_v2.py`를 **라이브러리로 두 번 불러와** 모델 페이로드/PCM/기존 HTTP429 quarantine/원자 파일 생성 함수를 재사용한다. **152개 기존 WAV/원문/SHA/메타를 전수 검증**하고 152/152가 아니면 하렌 API 요청 **0**.
- `F:\Downloads\Voice\API.txt`가 `Renewal_Voices\API.txt`의 **동일한 4개 실제 키**인지 비교(값/해시 출력하지 않음). 키 순서가 달라도 기존 Renewal ledger KEY 슬롯 순서로 맞춘다. 다른 키면 자동 중단해 **프로젝트별 호출한도를 임의로 추측하지 않는다**.
- 설정과 이벤트 장부는 기존 `Renewal_Voices\RENEWAL_SETTINGS.json`, `Renewal_Voices\_progress\api_events.jsonl`, `HOLD.json`, `RUNNING.lock`를 **공유**한다. 마지막 태온8개가 같은 PT 날짜에 쓴 키/프로젝트/전역 로컬 요청량을 하렌3이 이어서 계산해 **중복 40회 할당 방지**. 실패 시 API ATTEMPT는 최초 POST 이전에 기록, HTTP429는 실제 독립 프로젝트 그룹만 당일 격리, 다른 위험오류는 증거·잠금 보존.
- 설정된 40회/그룹10회는 **로컬 안전 상한**이고 Google 실제 무료 할당량·독립 프로젝트 여부·과금은 **사용자/Codex가 실소속/서버 상태 확인 후** 진행해야 한다. 같은날 다른 batch/도구의 외부 호출이 있으면 ledger만으로 모두 세지 못할 수 있어 추가 확인 필요.
- 스크립트 `check`는 API POST **0**, `run --max-calls 1`은 최초 **최대1 POST 시도**, 이후 첫 합격 시 `run`은 남은 **최대 3 POST 시도**(중복 결과 skip)한다. 실시간 사용자 PC 파일/키/API는 ChatGPT가 조작한 적 없다.
- 오프라인 검증: `python -m py_compile` PASS, fake 기존 152 파일/Meta SHA, 키/슬롯 일치 검사, 152 아닌 경우 안전 차단, Google POST 없는 점검, mock 제작 3건의 **공유 장부 8+3=11** 반영 및 재실행 중복0 — 총 **4개 unittest PASS**. 실제 Google API POST **0**, `F:` 쓰기 **0**, Unity **0**.
- 동일 프로젝트의 API 키 간 무단 우회 금지. 장부/잠금/기존 WAV/사용자가 KEEP한 Main21 7개 보존, 새 스크립트의 429 HOLD는 기존 **세린·태온 전용** 복구 스크립트를 무검증으로 적용하지 않는다.

## 4. 실행/통합 순서 — 사용자 직접 TTS 생성, Codex는 추후 별도 승인 시 Unity 검수

**태온 마지막 8개 우선**

```powershell
conda activate py3_12
cd F:\Downloads\Renewal_Voices
python .\Renewal_Auto_TTS_Failover_v2.py check
python .\Renewal_Auto_TTS_Failover_v2.py run
python .\Renewal_Auto_TTS_Failover_v2.py check
```

**콘솔에서 Serin67/67+Taeon85/85=152/152 확인, HOLD=False 뒤에만 하렌**

```powershell
cd F:\Downloads\Voice
python .\Haren_Main21_AfterRenewal_3.py check
python .\Haren_Main21_AfterRenewal_3.py run --max-calls 1
```

첫 `01`의 질문형 상승 오류가 없는지 **청취 검수**하고 승인되면:

```powershell
python .\Haren_Main21_AfterRenewal_3.py run
```

음성 생성은 **실행 전 명시적 승인 `RUN_HAREN3` 입력이 필요**하고 Google 프로젝트/모델 무료 할당량·과금이 확인되지 않았으면 POST 금지. 현재 `F:\Downloads\Voice`로 ChatGPT가 파일을 직접 복사하거나 API를 사용한 것은 아니다. Codex가 위 도구를 실제 로컬 디스크에 놓고 점검·실행하도록 지시 가능.

**기본적으로 Unity는 아직 적용하지 않는다.** 하렌3 사용자 청취 QA, 세린/태온 전체 인물별 일관성 QA, Main21 기존7 WAV 기술·청취 QA, Main20·21 실사용 ID/원문/GUID 일치 감사와 Codex Play Mode 확인 후, **사용자가 “이제 게임에 한 번에 넣자” 승인 시** 별도 `codex/작업명` worktree에서 통합.

## 5. 관련 문서

- [TTS 공통 Quota·429·체크포인트 운영표준](TTS_공통_다중프로젝트_할당량_429자동전환_체크포인트_운영표준_20261021.md)
- [Main21 기존 폴·미엘·레온 7개 KEEP](TTS_Main21_기존보존_7WAV_통합대기_20261022.md)
- [하렌 첫 대사 하강 억양 보류 및 정식제작 계획](TTS_하렌_평서문_억양_수정R1_20261020.md)
- [152대사 기존 원문 Manifest](TTS_Renewal_Voices_152_OriginalDialogue_Manifest_20261020.json)
- [TTS 최신 인계](팀운영/인계/TTS_LATEST.md) / [Issue #20](https://github.com/HappyPaul812/Project-Limitless/issues/20)

**완료 보고 필수:** 브랜치 `docs/tts` 및 코드용 `codex/작업명` 구분, 실제 커밋 SHA, 사용자 로컬 파일 생성 수/위치·SHA, API 프로젝트별 시도·실패, 3개별 청취 승인 여부, 현재 Unity 적용 여부.
