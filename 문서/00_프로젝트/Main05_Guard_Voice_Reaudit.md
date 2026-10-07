# Main05 Guard 태온001 재감사

## 구현 전 계약과 계획 — 2026-10-07

최신 LOCAL d50dabe를 기준으로 한다. 기존 당시 자동 검증 범위에서는 발견되지 않았고 사용자 실제 Play에서 발견된 태온001의 추가 발화를 조사한다. 이전 CLOSED/PASS 기록은 역사로 보존하며 이번 발화 의미의 증거로 재사용하지 않는다.

GuardReport는 경비병0 → Player1 → 태온001 2 → 미엘001 3 → 경비병4 → Player5의 6페이지다. 태온001의 허용 본문은 “몬스터들이 마을 쪽으로 움직이는 건 맞습니다.\n하지만 마을을 노리고 있는 것 같지는 않습니다.”뿐이다. 무입력 중에는 페이지2/화자/본문을 유지하고 해당 Clip만 자연 종료해야 한다. 다음 명시 입력 한 번은 미엘 페이지3만 연다.

Guard 종료 후 대화를 닫고 대표 보고 목표로 전환한다. 대표와 별도 상호작용해야 RepresentativeReport 8페이지를 연다. “저도 이 움직임이\n어디서 시작됐는지 확인하고 싶습니다.”(태온002)는 대표 보고 페이지3에서만 재생한다. 대표 보고 완료 후에만 두 동료를 정식 해금한다.

수정 전 WAV6의 독립 ASR/PCM/무음 경계와 원본을 조사하고, 격리 저장 Runtime에서 입력 callback·페이지·화자·본문·Clip·timeSamples·자연종료·목표를 기록한다. WAV 내부 혼입이면 명확한 무음 경계에서 해당 파일만 원본 PCM으로 복구한다. 불명확한 경계는 재생성/사용자 청취 필요로 남긴다. 연결/SHA 검증과 의미 판정을 구분한다. 다른 정상 Voice5, 원본 폴더, GUID/Catalog, 대사/Quest/Input/Save/동료 성장/Main07 수정, 사용자 작업을 보호한다. TTS API 호출과 Push는 하지 않는다.

수정 후 Main04 완료→Guard6→독립 대표8, 무입력 유지/1입력1페이지/단일Clip/정리, 대표 보고 전과 완료 후 실제 Bootstrap Continue, Compile/Console/Missing Script와 보호 기준을 확인한다. 모든 자동 검증은 화면 포커스를 바꾸지 않는다.


## 최종 결과 — 기술 검증 완료 / USER_CONFIRMED_REPAIRED

사용자 관찰을 재현했다. 수정 전 Runtime에서 page2/태온/본문/main05_taeon_supp_001 Clip은 그대로였고 입력 callback0이었다. 해당 Clip의 원본 PCM 자체에 정상 두 문장 뒤 “저도 이 움직임이 어디서 시작됐는지 확인하고 싶습니다.”가 포함됐다. 잘못된 내용은 원본과 Unity 양쪽 동일SHA이며 배치 full WAV의 75.66초 위치에서 기존001 전체PCM이 정확히 발견된다. 현재 승인된 태온002는 별도 재생성 PCM으로, 잘못 붙은 구간과 문장은 같지만 PCM 바이트는 다르다. Dialogue/입력 자동진행·Clip 교체가 원인이 아니다.

기존001 ASR: 정상 문장0~2.56초 / 정상 문장3.24~6.06초 / 잘못된 대표 보고6.94~9.94초. RMS -45dBFS 이하 6.17~7.00초 경계 안의 **6.60초(158400 sample)**에서 자른다. 정상 원본 prefix PCM 전sample 동일, 마지막 정상 발화 뒤 약0.4초 여백을 보존한다. 길이10.55→6.60초, 리샘플/증폭/페이드/TTS0. 원본/meta/GUID/Catalog/태온002와 정상5개를 변경하지 않았다. [복구 증거](Main05_Guard_PCM_Repair.json).

- 전체6 새 ASR/무음 경계/중복PCM/Unity native PCM을 감사했다. 복구001 정상 두 문장만, 나머지5 각본문과 일치하고 원본 승인 바이트 그대로. 중복PCM0, 앞뒤 발화 혼입·긴 무음 뒤 숨은 문장의 ASR 증거0. 자동 전사는 사용자 청취와 구분하며, 새 복구001의 자연스러운 끝은 **USER_CONFIRMED_REPAIRED**. [6행 Matrix](Main05_Guard_Voice_Reaudit_Matrix.csv), [수정 전](Main05_Guard_Before_ASR.json), [수정 후](Main05_Guard_After_ASR.json).
- 수정 전 Guard6 71PASS, 수정 후 실제 Main04 완료→Guard6→별도 대표8→정식 동료 해금→Main06 첫001 **304PASS/FAIL0**. 페이지2의 전체 재생 후 추가5초 무입력 유지, callback0, Clip 교체0·supp002 자동재생0, 자연종료 PASS. 다음 가상 Enter 한 번은 미엘 page3만 연다. Guard 종료→대표목표→대화닫힘, 자동연결0. 대표와 별도 상호작용해야 태온002가 재생된다.
- 같은프레임 Enter+Space/A 닫힘 중복 방지, 다음프레임 입력, 단일Source/각페이지 재생1/이전Clip정리 PASS. QA 코드만 재감사 추적을 추가하고 저장 위치 기록 누락을 수정했다. 게임 Dialogue/Quest/입력 코드 수정0. 프레임/페이지/화자/본문/ID/Clip/timeSamples/길이/isPlaying/목표/callback은 [연속 playback](Main05_Guard_After_Playback.jsonl)에 기록했다.
- 대표 보고 전과 Main05 완료 후, Main06 경계 실제 Bootstrap Continue3회 PASS. Guard 재생/Voice 잔류/대표 자동시작0, 목표·동료해금 보존. CompileError0, 기존CS0618 Warning16/신규0, 최종 Runtime ConsoleError/Warning0, MissingScript0.
- 첫 재현 fixture는 직접 Scene로드가 저장 위치를 기록하지 않아 빈Scene 자동Save 오류2건이 있었다. 재현 페이지 관찰은 유효하나 그 실행을 Console0으로 기록하지 않는다. QA Load에 위치기록을 추가했고, 중단된 첫후속QA와 오류기록을 Temp에 보존했다. 최종 전/후 QA는 오류0.
- 보호 baseline 3497중 WAV001/QA helper2개만 변경, 3495개 byte동일. 다른Voice5·Main04/06+·Main07전투재도전/동료성장/미엘복구/Early7/Paul20·Player·Battle·Beast·SaveVersion·Main17/18·Art/BGM·사용자Save·TTS원본 보호. 기존 역사적 NEEDS_LISTENING244를 수정하지 않았다.

기존 당시 자동 검증 범위에서는 발견되지 않았고 사용자 실제 Play에서 발견됐다. 과거 기존001 `USER_CONFIRMED_NORMAL_EXISTING_ANCHOR`는 이번 의미 PASS 근거가 아니다. [과거 Matrix 보존본](Main05_Voice_Matrix_Before_Guard_Reaudit.csv)을 남겼고 정본 Main05 Matrix는 새 결과로 갱신했다. 기존5 승인 매핑 CSV와 과거Runtime239PASS/FinalVerification/CLOSED는 당시 기록으로 보존한다.

변경 게임Asset1: Main05/main05_taeon_supp_001.wav. 변경 Editor QA1: Main05ReturnVoiceAudit.cs. Tools/TTS 아래 audit_main05_guard_voice.py / repair_main05_guard_pcm.py / report_main05_guard_reaudit.py는 재감사·PCM복구·문서보고 도구다. CURRENT_STATUS, Main05_FinalVoice_QA, Main05_Return_Voice_QA와 본문서/위 링크의 QA 증거를 생성·갱신했다. Commit은 WAV Fix와 QA/Docs로 나누며 Push하지 않는다.

사용자 최종 청취 답변: “해당 두 문장만 나오고 끝도 자연스러움”. Main05 USER_LISTENING_REQUIRED0, TTS_REGEN_REQUIRED0. 다른 작업의 기존 청취 대기는 이 결과로 해소하지 않는다. 관련 구현 commit `b96e8b3`, QA/문서 commit은 이 문서를 포함한 `Docs: Main05 태온001 혼입 재감사 및 Runtime QA`다. 마지막 Editor는 clean Bootstrap/Edit Mode/비포커스, 격리 Save=null, 기본 InputSettings 원복. Bootstrap/Field01/StarterVillage/Field02를 포커스 없는 PreviewScene으로 추가 확인하여 MissingScript 각각0을 확인했다.
