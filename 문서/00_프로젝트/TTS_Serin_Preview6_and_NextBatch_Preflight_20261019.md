# LIMITLESS TTS — 세린 보정본 재전달 및 다음 제작 안전 준비 (2026-10-19)

**PREVIEW_ZIP_REVERIFIED / OFFLINE_PREFLIGHT_BUILT / USER_LOCAL_NOT_EXECUTED / GEMINI_API_0 / UNITY_0 / MAIN_UNCHANGED**

## 오늘 사용자 요청과 결과

사용자: “니가 수정한거 아직 안받았지 나? 일단 수정한 주고 더 뽑을 꺼 준비해줘.”

1. 이미 만들었던 보정 WAV 6개 ZIP `LIMITLESS_Serin6_ConsistentVoice_Previews.zip`을 재검사해서 다시 전달. SHA256 `a5878e6d4f1b1e858f4b505f23669ab147da2dd466fe30443b0d6116a7246cb1`, 1,465,870 bytes. ZIP CRC PASS, `*_PREVIEW.wav` 6개 전부 24kHz/mono/PCM16 디코딩 PASS. `ground` 사용자 승인 기준 WAV와 CSV/README 포함, `heat_vent` 제외.
2. 오프라인 읽기 전용 `LIMITLESS_TTS_Next_Preflight_20261019.py` 작성. SHA256 `e80b91b4bb912f39bd39210d327f2bac34da6b077e4c3a0f32683e04febe6599`. `check` 외에 API/생성 모드 없음. Google SDK/네트워크/API 키·환경변수 읽기/파일 쓰기/Unity/Git 동작 없음.
3. 준비 ZIP `LIMITLESS_TTS_Next_Preflight_Pack_20261019.zip`, SHA256 `de2036d1557399fa34aa02257ecd61c6cf887585786c8fbacf6465d9ca5c55e9`, 9,533 bytes. Python + 다음 대상 목록25행 CSV + 사용자 README + Codex 조사 지시문.
4. 안전검사: Python compile PASS, ZIP CRC PASS, 합성 격리 프로젝트에서 정상 10원본/10백업/25보호/샘플/저장7/HOLD/미호출14를 모사한 사전검사 PASS. 인위적 기존 성공 WAV 결과 SHA 불일치 시 `REVIEW_REQUIRED` 차단 PASS. **실제 사용자 F:에서 실행한 결과 아님**.

## 보정본 6개 (기존 승인/원본 구분)

`main18_leon_serin_01_PREVIEW.wav`,
`main18_overheat_serin_01_PREVIEW.wav` ~ `main18_overheat_serin_04_PREVIEW.wav`,
`main18_vibration_serin_01_PREVIEW.wav`.

사용자가 이전에 보정 방식을 긍정적으로 평가했으나 **여섯 파일 전체의 최종 Unity 적용 승인으로 간주하지 않음**. 원본 성공본·Main18 오류10 원본/백업·정상25 Unity·Main17 승인 음성·체크포인트는 변경하지 않음.

## 다음 실제 제작을 위한 정확한 재개 순서

| 범위 | 대상 | 상태 | 승인 |
|---|---|---|---|
| Main18 Story07 | `main18_heat_vent_serin_01` | 저장 성공 WAV의 음색 불일치, **재제작 후보**. 기존 성공본 및 사용자 음성보호 | 추가 호출 미승인 |
| Main18 Story13 | `main18_watcher_witness_serin_01` | **FAILED_OR_UNCERTAIN_HOLD**. 시도 기록, 성공 WAV 없음; 실패 원인 UNKNOWN | 독립 수동 재개 필요 |
| Main18 Story14 | `main18_watcher_witness_serin_02` | NOT_CALLED, Story13 건너뛰기 금지 | 미승인 |
| Main20 | 세린 NPC 5개 | 기존 Manifest 기반 향후 작업 | 미승인 |
| Main21 | NPC17개 중 하렌3개 Voice 미승인, 나머지14개 | 하렌 BLOCK. Player/지문은 TTS 제외 | 미승인 |

**최대 향후 호출 후보 3+5+14=22개**는 오늘 사용할 수 있는 무료 quota가 아니다. 현재 프로젝트·모델별 활성 FreeTier/Billing/잔여량은 `NOT_VERIFIED`. 과거 10/13 그래프 수치를 10/19 예산으로 재사용하지 않음.

2026-10-13 사용자 로그·Codex 감사: 승인 샘플 Story06 1개 + Story02/07~12 저장7개 = **SAVED8**. Story13는 `REMAINING9_HOLD.json`와 `REMAINING9_SESSION.lock` 유지, Story14는 신규 시도 없음. 지금은 SHA 확인용 READ-ONLY 검수 외 기존 잠금 해제/재호출/Key 변경/Story14 선호출 금지. 기존 보정 PREVIEW6은 **TTS 호출 없이 만든 후처리 파일**.

## 사용자 실행

1. ZIP 다운로드 후 기존 보정 파일은 별도 미리듣기 폴더에서 사용; **원본 WAV 덮어쓰기 금지**.
2. 준비 ZIP을 `F:\Downloads\Voice`에 압축 해제.
3. 다음 명령만 실행 가능:

```powershell
python .\LIMITLESS_TTS_Next_Preflight_20261019.py check
```

읽기 전용 결과 `[PRE-FLIGHT PASS]`는 **API 재시도 허가가 아님**. 실패하면 Codex 보고 후 중단하고 HOLD/잠금/체크포인트를 유지할 것.

## 권한·전달

- **이번 턴 TTS API 호출0, WAV 신규 TTS 생성0, 로컬 Unity 수정0, GitHub main 수정/merge0**.
- TTS 기록은 `docs/tts` 브랜치에만 작성. 본 패키지 다운로드는 사용자 대화에서 제공.
- 이후 실제 신규 호출은 Codex 정적 결과·해당 프로젝트 무료 quota/과금 위험·사용자 명시적 호출 승인 후 새 별도 재개 프로그램으로 수행.
- 샘플·성공 WAV 중복 요청0, HTTP429/실패 즉시 중단, 승인 전 Unity 교체0.
