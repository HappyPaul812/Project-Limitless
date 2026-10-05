# Main04 첫 조우9건 TTS 재생성 전달 QA

## 2026-10-06 Main04 첫 조우 Voice9개 전체 재생성 전달 확정

사용자 실제Runtime:001 첫문장만정상/두번째문장이다른위치에서발화,이후미엘·태온Voice전반적자막순서불일치/Main03태온처럼들리는발화재등장.9건모두 **SEMANTIC_INTEGRITY_SUSPECT / TTS_REGEN_REQUIRED**,001은기존직접WAV청취로 **PARTIAL_AUDIO_CONTENT** 추가확정.나머지각파일의틀린정확한문장/Character를추측하지않는다.이전Runtime Playback/PCM/148검사PASS는기술증거로유지하고의미PASS로사용하지않는다.

LOCAL FirstConversation10페이지/Voice9(미엘6 Sulafat·태온3 Gacrux)/Player1 정상무음.원문을Flow에서재추출하고현재Manifest와줄바꿈포함9건일치,본문Drift0.기존WAV재배열/Mapping수정없이9개전체정식본문재제작방향확정. **현재LOCAL 재생성필요1→9**(사용자요청의0→9는이전001등록전기준;001중복등록없이전체9). NEEDS_LISTENING244관리범위유지/Main03해결상태보호.

정본전달 [9행 Handoff](Main04_FirstEncounter_TTS_Regen_Handoff.csv),[WAV9개감사](Main04_FirstEncounter_WAV_Audit.csv),[Main03비교원본목록](Main04_FirstEncounter_Main03_Comparison_Inventory.csv),[교차비교](Main04_FirstEncounter_CrossQuest_Comparison.csv),[Main04내부비교](Main04_FirstEncounter_Internal_PCM_Comparison.csv).현재Main03 WAV12개와9×12=108쌍/첫조우내36쌍비교.동일SHA/PCM cross duplicate 0,내부PCM duplicate 0.내용이유사한발화는해시불일치여도배제할수없어사람청취증거와구분한다.

WAV/TTS API/Unity코드/Catalog/정식본문/Player/Main03Voice변경0.이번Editor/Play재실행0/포커스변경0,문서화→감사·전달생성→정적검증.다음:이9행으로온전한원본제작→개별청취→별도승인된반영작업에서기존GUID보존교체·연속Runtime검증.마지막관련commit `3ab8e67a71fc684d55796b012aade22ea943ad07`,이번문서commit은최종보고참조.아래는이전감사이력이다.


## 설정/측정

CASTING.json/보충팩Manifest/기존Registry QA:미엘 Sulafat,태온 Gacrux,모델 gemini-3.8-flash-tts,voice_design=false,ko-KR.로컬제작기록이며현재API지원검증이아님.속도/Tone수치기존TBD/새Design0.공통연기는CSV acting_note에기록.원본파일명은Manifest output_file basename유지.

RMS/Peak=dBFS,PCM16분모32768;PCM hash는wave 모듈로읽은data chunk전체(헤더/추가메타데이터제외).PCM샘플레이트/채널/폭도별도비교.001 Partial확정1,기타Partial/정확한Wrong문장개수미확정.저음량은자동후보RMS<-40dBFS 기준이며확정청취결함과구분.자동후보 0건.각행의측정값을감사CSV에공개한다.

## 검증

검증후결과추가.

자동검증PASS:정확히9행/Miel6/Taeon3/Player0/중복ID0/빈본문0/빈파일명0/본문Drift0/SpeakerVoice불일치0/Main03 ID0.9개WAV 존재/Catalog GUID-Speaker exact reference정상.331 Matrix 대상외행모두HEAD동일/NEEDS_LISTENING244.보호3044파일해시변경0.이번UnityCompile/Console재검사없음(코드/Asset변경0);이전148QA기술증거보존.
