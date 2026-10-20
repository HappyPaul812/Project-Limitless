# LIMITLESS / 세린 자동 생성 HTTP429 HOLD 및 안전 재개 (2026-10-20)

**담당:** TTS / **상태:** 사용자 로컬 HTTP429 관측 / 사고 데이터 원본 미수령 / 오프라인 복구 도구 테스트 PASS / 사용자 로컬 복구 미실행  
**사용자 메시지:** `[STOP] main16_canyon_default_serin_01 failed (HTTP_429). HOLD+RUNNING.lock preserved; NO AUTO RETRY.`

## 확정된 사실과 불명확한 사항

- `Serin`의 Stable Dialogue ID `main16_canyon_default_serin_01` 요청에 대해 Google HTTP 429가 발생했으며 기존 프로그램이 의도대로 **HOLD.json + RUNNING.lock**을 보존하고 자동 재시도를 중단함.
- 성공한 기존 WAV/메타는 그대로 유지되며 재실행 시 SHA 검증 후 건너뛰도록 원 프로그램이 작성되어 있음. **실제 사용자 폴더의 현재 완성 대사 수·남은 Google 무료 quota 수는 아직 이 로그 한 줄만으로 확정 불가**.
- HTTP429는 RPD/RPM/TPM 등 요청 제한 중 하나가 원인일 수 있음. 기존 프로그램은 429 오류 본문을 보존하지 않으므로 **정확한 차단 차원과 원격 잔여량은 확인할 수 없다**. 모델 메타 GET 성공도 남은 합성 횟수를 보장하지 않는다.
- Google 공식 지침상 Gemini API 요청 제한은 API 키가 아니라 **프로젝트 단위**, 일일 사용량(RPD)은 **태평양 표준/서머타임 자정** 리셋. 한국 기준 미국 PDT 때 16:00, PST 때 17:00. https://ai.google.dev/gemini-api/docs/rate-limits
- 사용자가 이전 실행에서 `SEPARATE`를 입력했으나 네 키가 정말 별도 프로젝트인지 **실제 프로젝트 ID 검증 근거는 이 세션에 없음**. 기존 운영 예산 `daily_request_budget=34`는 사용자가 원한 **로컬 안전 상한**이지 원격 구글 무료 잔여 보증이 아님. 이전 샘플/외부 스크립트 Google 호출은 로컬 자동화 장부에 포함되지 않을 수 있다.

## 무변경 현황 확인

```powershell
cd F:\Downloads\Renewal_Voices
python .\Renewal_Auto_TTS.py check
Get-Content .\_progress\HOLD.json
```

`_progress/api_events.jsonl` 원본, `_progress/USAGE_LOG.csv`, `_progress/USAGE_TODAY.json`, `Serin/dialogues.csv` 확인. 직접 강제 `HOLD.json`/`RUNNING.lock` 삭제, `API.txt` 공개, 키 전환을 통한 429 우회, 동일 ID를 새 이름으로 반복 생성, 타 캐릭터 WAV 변경 **금지**.

## 복구 프로그램 직접 제작 및 검증

**사용자 다운로드 ZIP:** `LIMITLESS_Renewal_429_Safe_Resume_20261020.zip`, **5,176 bytes**, SHA256 `3aa70d65e67a4fe0b74d3039ee6fe16729ff06b59d7c3163ad30e389096af0fb`.  
내부:
- `Renewal_429_Safe_Resume.py`, 9,024 bytes, SHA256 `63af0741530ec964c49dfc70111850ec3e1d25db91300a6662f174c90cac7af4`.
- `README_KO.txt`, 2,721 bytes.

**오프라인 Python unit test5 PASS**: 검수만 하는 inspect, 다음 PT날짜의 일치한 429에 한해 로그/잠금 무손실 백업 후 resume, 같은 PT날짜 복구 거부, HOLD/LOCK 불일치 거부, 명시승인 부재 거부. `zipfile.testzip()` PASS. Python 문법 검사 PASS. **실제 Google API POST0, 사용자 F: 파일 변경0, WAV/Unity 변경0**.

복구 도구는 기존 `Renewal_Auto_TTS.py`를 수정·덮어쓰지 않고 동일 폴더에서 가져와 manifest 152개·원문·기존 WAV SHA를 읽기 전용 검증함. 실패한 ID의 WAV 또는 임시 파일이 발견되면 **해제 거부**. 원시 `HOLD.json` + `RUNNING.lock`을 `_progress/recovery_snapshots/<PT일자__ID__해시>/` 아래 원본 바이트/SHA 그대로 복사·검증하고, 결함 이벤트 장부는 절대 변경하지 않음. 사용자 입력 `RESUME429`이 있어야만 원래 HOLD/LOCK 두 개 해제. 해당 복구 자체는 Google 호출 없음.

## 안전한 사용 방법 — 오늘은 inspect만

ZIP 안의 `Renewal_429_Safe_Resume.py`를 `F:\Downloads\Renewal_Voices` 폴더에 추가. `API.txt`나 기존 ZIP의 스크립트 덮어쓰기 금지.

```powershell
conda activate py3_12
cd F:\Downloads\Renewal_Voices
python .\Renewal_429_Safe_Resume.py inspect
```

**같은 태평양 날짜면 `resume`이 강제로 거부된다.** 차단된 Google 프로젝트의 원격 실제 quota와 결제 위험을 확인하고 날짜가 바뀐 후:
```powershell
python .\Renewal_429_Safe_Resume.py resume
```
→ `RESUME429` 승인 → archive 보존 후 잠금 제거 → `python .\Renewal_Auto_TTS.py check` → 
```powershell
python .\Renewal_Auto_TTS.py run --max-calls 1
```

1대사 성공/실패 결과와 청취/사용량을 확인한 다음 나머지 제작 재개 판단. **Google 429 재발 시 또 HOLD하므로 즉시 반복 해제/재시도하지 말 것.**

이는 안전측 보수적인 **수동 재개**이고, 구글 실제 quota가 리셋됐는지 도구가 보증하지 않는다. 다음 PT 자정 전에 일시적인 RPM 제한이 풀렸을 가능성은 있으나 오류 본문이 유실되어 지금은 무리한 새 호출을 하지 않음. 자동제작기의 `watch`는 HOLD가 존재할 때 별도 판단 없이 건너뛰도록 바꾸지 않는다.

## 기존 프로젝트 정본

- [세린67·태온85 4키 자동 생성 Runbook](TTS_RenewalVoices_Serin67_Taeon85_4Key_Automation_Runbook_20261020.md)
- [원문152개 Manifest](TTS_Renewal_Voices_152_OriginalDialogue_Manifest_20261020.json)
- [TTS LATEST](팀운영/인계/TTS_LATEST.md)
- [GitHub Issue #20](https://github.com/HappyPaul812/Project-Limitless/issues/20)

**예산/키 원칙:** 실제 현재 할당량과 프로젝트 그룹은 원격 대시보드로 확인한 것만 신뢰한다. 실패 시 1 ATTEMPT 기록되어 로컬 그날 예산에서 1로 계산. HOLD를 무시한 무단 key cycling과 같은 날 즉시 retry 금지. 하렌 제외, 세린·태온 완료 시에만 사용자가 청취 승인 후 Codex Unity 통합.
