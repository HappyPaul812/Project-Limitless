# LIMITLESS TTS — HTTP429 발생 시 독립 Google 프로젝트로만 안전 자동 전환(v2)

**작성일:** 2026-10-21
**브랜치:** `docs/tts`
**요청:** “저기 한 API가 막히면 다른 API가 하게 못해?”
**상태:** 안전 전환 프로그램 및 구 429 같은날 수동 복구 프로그램 ZIP 제작·오프라인 총9 테스트 PASS. 실제 Google API 요청 0, 사용자 F 드라이브 수정 0, Unity 변경 0. 사용자 프로젝트별 소속 및 실제 무료 할당량 추가 확인 필수.

## 사고 현황과 목표

- 2026-10-21 사용자 PowerShell 증거: 세린 **59/67**, 태온 **1/85**, 전체 **60/152** WAV READY. 이날 로컬 요청 **35/40**(성공34+실패1), KEY1=9, KEY2=9, KEY3=9, KEY4=8. 실패: `main20_confront_serin_02` / `KEY3` / `HTTP_429`. 기존 프로그램은 `HOLD.json` + `RUNNING.lock`으로 전체 작업 중단, 자동재시도0.
- Google 공식 정책: Gemini API RPM/TPM/RPD 한도는 **API Key가 아닌 Google 프로젝트 단위**. 일일 RPD 태평양 자정 리셋. **실제 4키가 4개의 Google 프로젝트에 각각 속했는지 증거는 아직 채팅에 제출되지 않았다.** 기존 프로젝트 그룹 `PROJECT_1...4`는 사용자가 입력한 라벨이지 원격 인증값이 아님. Google 문서: https://ai.google.dev/gemini-api/docs/rate-limits , https://ai.google.dev/gemini-api/docs/api-errors .
- 따라서 **동일 프로젝트의 다른 키로 무단 우회 금지**. `HTTP429`가 단순 RPM/TPM 일시 제한일 수도 있으므로 오류본문 원인이 없을 때 프로젝트 일일 할당량 초과라고 확정하지 않는다.
- 목표: 서로 **실제로 독립되어 있고 적법하게 쓸 수 있는** Google 프로젝트로 확인된 경우에만 현재 프로젝트를 PT 당일 격리하고 다른 프로젝트의 독립 한도 이내에서 자동으로 작업을 이어 간다. 총 요청·성공·실패 별도 계수, 대사 ID·원문·WAV 해시·API 슬롯 및 기존 승인 파일 보호.

## 새로운 안전 패키지

**파일:** `LIMITLESS_Renewal_Voices_HTTP429_ProjectFailover_v2_20261021.zip`
- 크기 **22,109바이트**
- SHA256: `cd0a063d5c6d2b7463ab37363fbe2c9c9de45ab9ae257899f55347d231b7d643`
- **기존 `Renewal_Auto_TTS.py`는 유지**, 새 이름 `Renewal_Auto_TTS_Failover_v2.py`로 동작.
- `Renewal_429_SameDay_Switch_v2.py`: 기존 KEY3 429+HOLD+LOCK에 대해 **같은 PT 날짜**에도 다른 독립 프로젝트 쿼터가 실제 확인되면 수동 승인 후 일회성 잠금 안전 해제. 보존 스냅샷 만들고 프로젝트 격리 이벤트 원본 장부에 append; API0·WAV0.
- `Configure_Failover_v2.py`: 실제 Google Key→Project 매핑/무료quota 확인 후 사용자 `VERIFIED_PROJECTS` 입력. 원본 `RENEWAL_SETTINGS.json` SHA 백업 후 `auto_failover_on_429:true`와 `verified_independent_projects_for_failover:true`만 추가/수정. 다른 음성/키/장부는 불변.
- `README_KO.txt`, `selftest_failover_v2.py` (기존5+신규3=8 검사), `test_same_day_recovery_v2.py` (같은 PT 429 수동복구1 검사). ZIP 무결성 테스트 PASS.

### 429의 정확한 안전 동작

- `request`는 Google `POST` 이전에 `ATTEMPT` 기록 및 원문+KEY+그룹, `RUNNING.lock` 파일을 보존. HTTP429 발생 시 `FAIL`도 장부에 append, **실패 호출도 1회로 집계**.
- `auto_failover_on_429:true`이고 `verified_independent_projects_for_failover:true`이며 다른 독립 프로젝트 그룹에 **남은 로컬 슬롯/전체 일일 예산**이 있는 경우, 실패 그룹에 `PROJECT_QUARANTINE` 기록 후 그 태평양 날짜에 그룹 전체를 차단. 이전 성공 WAV가 있으면 채택/재생성하지 않음. 실패한 DialogueID는 WAV/meta 미생성 확인 후 다른 그룹이 다음 생성 시도. 다른 프로젝트에서도 실패하면 각각 장부에 계수한다.
- 설정 중 하나라도 승인되지 않았거나 동일 프로젝트밖에 없다면 **기존과 같이 HOLD+LOCK**. 다른 그룹 cap이 다 소진되거나 HTTP400/500/결과불명·타임아웃이면 기존처럼 HOLD/LOCK 보존하고 수동 복구 전 재시도0.
- 최초부터 끝까지 기존 세린 Erinome67/태온 Algieba85, 하렌 제외. 기존 152원문 manifest, 승인 샘플, WAV/JSON, 파일 해시, 모델 유지. 일일 40회는 **로컬 예산**이며 Google 실잔여/무과금 보증 아님. KEY 같은 프로젝트 공유시 그 그룹을 함께 차단한다.
- `_progress/USAGE_TODAY.json`에 `quarantined_projects_today` 추가. `USAGE_LOG.csv`/원본 이벤트 장부는 `ATTEMPT`, `FAIL`, `PROJECT_QUARANTINE`, `SUCCESS`를 구분. 날짜가 PT 자정 다음날이 되면 격리 기록은 역사 기록으로 남되 선택 차단은 해제. 단 **Google 서버의 실사용 가능 상태를 보증하지 않음**.

## 사용자 PC 복구·사용 절차 (네 키가 다른 프로젝트인지 확인한 경우에만)

1. ZIP 안의 6개 파일만 기존 `F:\Downloads\Renewal_Voices` 폴더에 추가. API.txt/RENEWAL_SETTINGS.json/기존 프로그램/음성/로그 덮어쓰기 금지.
2. `python .\Renewal_Auto_TTS_Failover_v2.py check` — 원본 152개 및 기존 READY WAV 확인, API 호출0.
3. https://aistudio.google.com/api-keys 의 **각 키 프로젝트 이름·Google 프로젝트 ID**, 해당 프로젝트/모델의 무료 잔여 및 과금 상태를 확인. 혼합 프로젝트라면 `RENEWAL_SETTINGS.json`의 `project_groups`를 실소속에 맞게 수동 교정. 다른 프로젝트가 두 개 이상 검증되지 않았다면 Failover **활성화 금지**. 실시간 원격 확인은 스크립트 자체로 불가.
4. `python .\Configure_Failover_v2.py` → 실제 검증을 완료한 경우만 `VERIFIED_PROJECTS` 승인; 원 설정 백업, 새 옵션 둘 활성화. API0.
5. 현재 **2026-10-21 KEY3 429 HOLD가 아직 원본 그대로 있을 경우에만** `python .\Renewal_429_SameDay_Switch_v2.py inspect`, 이후 `... switch`에서 `SWITCH429` 수동 승인. 동일 PT 날짜, 원문·GUID/메타·WAV SHA/마지막 `ATTEMPT`·`FAIL`·기존 `HOLD`+LOCK 원문 교차 검증 후 `_progress/recovery_snapshots/`에 백업, 실패 PROJECT_3를 당일 격리. 안전조건 하나라도 충족 못하면 해제 거부.
6. `python .\Renewal_Auto_TTS_Failover_v2.py check` → `python .\Renewal_Auto_TTS_Failover_v2.py run --max-calls 1` → `RUN` 승인 및 **한 개의 실제 신규 WAV 청취/카운트 확인** → `python .\Renewal_Auto_TTS_Failover_v2.py run`으로 나머지 일일 용량 이내 진행.
7. 날짜가 이미 다음 PT 날짜로 넘어가거나 이전 HOLD가 해제되었다면 같은날 switch를 사용하지 않는다. 이전 `Renewal_429_Safe_Resume.py resume`의 날짜 교차검사를 따르거나 현재 파일 상태에 맞게 진행.
8. 이후 이어 작업은 **v2 runner만 사용**. 구 프로그램은 `PROJECT_QUARANTINE`을 모른다. `watch`도 프로세스가 실행중일 때만 동작.

## 사용자 안내가 필요한 핵심 위험

- **프로젝트 ID 확인되지 않았는데 4키를 SEPARATE로 표시한 설정은 실제 안전 보장이 아님.** 프로그램의 신규 옵션은 키가 독립 프로젝트임을 사용자가 확인한 경우에만 켜야 한다.
- 429 서버 오류 내용은 이전 스크립트에서 저장하지 않았으므로 정확한 RPM/RPD 원인은 아직 미확정.
- 기존 누적 성공 60개 등 사용자 화면 수치는 해당 스크린샷 시점 결과다. 이후 사용자 로컬 변화는 새로운 `check`로 재검증.
- 같은 날 키3에서 429가 나도 키1/키2/키4가 실제 독립 프로젝트라면 *로컬 설정*상 키1=1, 키2=1, 키4=2 정도 추가 호출 여지가 있을 수 있다(당시 시점 9/9/9/8와 그룹별 제한10에 따른 산술), 다만 **서버 실제 성공 보장 아님**.
- TTS API 키 내용 공유/업로드 금지, 마스킹만 허용.

## 근거·인계

- [기존 자동 제작 runbook](TTS_RenewalVoices_Serin67_Taeon85_4Key_Automation_Runbook_20261020.md)
- [기존 429 안전 재개 문서](TTS_RenewalVoices_HTTP429_HOLD_ManualRecovery_20261020.md)
- [TTS 최신 인계](팀운영/인계/TTS_LATEST.md)
- [GitHub Issue #20](https://github.com/HappyPaul812/Project-Limitless/issues/20)

**검증:** 오프라인 합성 mock HTTP GET+POST/429 시 그룹격리 대체경로, 같은 프로젝트 자동전환 금지, 비활성 시 기존 HOLD+LOCK, same-day 기록·보존·승인과 original 152 manifest, UTF8 API.txt 및 WAV SHA 검증까지 합계9개 Unit PASS. 실제 사용자 로컬 Google POST0, PC 파일 변경0, Unity0.
