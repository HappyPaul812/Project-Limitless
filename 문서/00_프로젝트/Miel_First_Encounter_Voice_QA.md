# 미엘 첫 조우 전체 Dialogue/Voice 감사 (2026-10-06)

문서화→격리 QA 구현→검증. LOCAL Main04 FirstConversation10페이지(미엘6/태온3/Player1)를 첫 등장·치료 대사부터 해당 첫 대화 종료까지 검사한다. Main03 정상 완료와 Main04의 전투전3페이지/전투후9페이지는 별도 Sequence이며 첫 대화와 섞지 않는다.

사용자 Runtime 보고: main04_miel_supp_001 자막 `조금만 참으세요.
출혈은 멎었습니다.` 중 첫 문장만 들림. PARTIAL_VOICE/POSSIBLE_TRUNCATION으로 기록하고 CaseA 원본내용누락/CaseB Playback조기중단을 직접WAV 청취+Runtime시간추적으로 구분한다. 다른줄 의미를 Metadata로 추측하지 않는다.

모든 WAV/Catalog/Runtime본문 보존,TTS API/Push0. cleanBootstrap EditMode/dirty=false 확인후 기존 Partial9FixedSpriteAudit의 PlayUnfocused·격리Save/Settings·종료복원 경로 사용. 순서별ID/화자/Manifest/GUID/해시/PCM·Play/자연종료/Next·Player 정리·UI·첫대화종료/Quest 다음전투Objective·격리Save읽기 검사. Runtime 실제사람청취는 별도요청. 수동Next는 즉시가능한 기존skip정책이며 대기검사에서는 QA가clip전체종료후에만Next를호출한다.

## 결과

검증 후 갱신한다.

## 원인 확정 / 전체 결과

문제001: companion_miel / Sulafat, Runtime/Manifest 정확한 전체본문 `조금만 참으세요.
출혈은 멎었습니다.` 일치. 사용자 직접WAV 답변 **‘조금만 참으세요.’만 발화**, 기존Runtime보고도동일. **Case A PARTIAL_AUDIO_CONTENT / TTS_REGEN_REQUIRED**. 현재WAV `main04_miel_supp_001.wav`,Unity Path `Assets/_Project/Audio/Voice/Story/Main04/main04_miel_supp_001.wav`,GUID `e0fff6b9e1ff1ce438e69bc2582506a8`,SHA256 `4f7985fe337e71ace9cf935d6f50c840a79bc8fa13c4ba7ab8f0971463e75801`,2.00초/RMS−17.1421/Peak−2.5306dBFS.

Runtime001:observed start3567.1007961/자연종료3569.1266261,Clip2초/마지막관측sample47104.001 Resolve==AudioSource.clip==001,PCM전체48000샘플 원본오차0.현재페이지/Source가Clip전체길이+.5초대기동안유지되고완료후수동Next.문장부호분할/typewriter/자동Next구현없음.수동Advance즉시가능/skip허용기존정책,RefreshSequenceText의Play는기존Source.Stop→새Clip설정→전체Play.거리초과Hide/Scene전환/OnDisable/ClearSequence는별도정리경로이며이번정상거리실행에서발생없음.원본누락을자막삭제·분할이나Playback패치로고치지않는다.

전체10페이지(Miel6/Taeon3/Player1):148 assertions PASS/FAIL0·TRACE9/NEXT10별도.9개Voice전체PCM원본오차0·Missing0/Unmapped0/Clip중복0/Speaker mismatch0/Manifest본문 mismatch0,Player Voice0/Portrait0·Miel/TaeonPortrait교체·UI Wrap/동적높이/원문잘림0/영역비중첩·종료Dialogue/Voice/Portrait정리·다음win_three_people_encounter Objective·격리자동Save읽기PASS.첫대화종료만검사했으며Main04Quest전체완료/Battle검사는아님.

기술재생정상9개를의미정상9개로해석하지않는다.현재의미정상확정0/Partial확정1/추가의미청취대기8(미엘5+태온3).Wrong/Low Volume추정없음·Missing0/PlaybackCut재현0.추가문제는청취로확인한뒤갱신한다. [전체순서CSV](Miel_First_Encounter_Sequence_Audit.csv)·[Runtime Trace](Miel_First_Encounter_Runtime_Results.txt)·[확정1건 TTS전달](Miel_First_Encounter_TTS_Handoff.csv).재생성필요0→1,태온Main03정상상태보호/244관리범위유지.

보호3042기존Asset/설정/Packages/Save파일해시변경0.새Editor QA helper만추가,게임C#/WAV/meta/Catalog/본문수정0,TTS API0·포커스전환0.Compile/최종Console Error0·Warning0,cleanBootstrap EditMode·audit해제/설정복원.초기helper참조는컴파일전이어서해석실패했으나컴파일후전체실행PASS;게임CompileError아님.비동기Screenshot는Next후프레임에저장돼페이지파일명과내용이밀릴수있어시각증거로사용하지않는다.UI검증은해당페이지의실제Rect/Text측정이다.
