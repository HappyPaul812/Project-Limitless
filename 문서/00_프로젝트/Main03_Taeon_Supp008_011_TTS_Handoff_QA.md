# Main03 태온 008~011 TTS 재생성 전달 QA

## 2026-10-06 Main03 008~011 사용자 Runtime 의미 불일치 확정·TTS 전달

사용자 실제 Runtime 청취에서 008~011 모두 자막과 발화 불일치 확인. 네 건 모두 **WRONG_AUDIO_CONTENT / RUNTIME_SEMANTIC_MISMATCH**이며 Metadata 정상 여부로 의미 판정을 되돌리지 않는다. **TTS_REGEN_REQUIRED 1→4**, NEEDS_LISTENING **244는 기존 관리 범위**로 유지(이번 확정 문제/이미 해결된 항목을 포함하므로 미확인 244건이라는 뜻 아님). 012는 새 WAV·사용자 직접 청취·Unity Runtime 의미/전체 문장/정상 음량 PASS 해결 유지. 005/006/007/003/004 PASS와 001/002 기존 상태, Player 정상 무음 보호.

LOCAL MainQuest03FieldFlow.AfterBattleConversation 및 DialogueLine의 구형 화자 접두어 제거 규칙에서 전체 표시 본문을 추출한다. 현재 Manifest와 네 건 모두 줄바꿈까지 일치. 실제 AfterBattle 페이지 순서는 1/3/5/6(index 0/2/4/5); Player 페이지를 제외한 전달 4행이다. 009는 `네.` 뒤 줄바꿈을 보존한다. Actual Spoken Text는 **USER_CONFIRMED_MISMATCH**로 기록하며 잘못된 문장을 추측하지 않는다. 이전 008 직접청취의 별도 증거는 보존한다.

정본 전달: [4행 Handoff](Main03_Taeon_Supp008_011_TTS_Handoff.csv), [원본/참조 감사](Main03_Taeon_Supp008_011_Voice_Audit.csv), [공통 연기/설정/검증](Main03_Taeon_Supp008_011_TTS_Handoff_QA.md). 기존 7행 Remaining Handoff는 원본 청취 감사 목록이며 새 재생성 전달에는 이 4행만 사용한다. 문서화→감사/전달 생성→백그라운드 정적 검증 순서. WAV/meta/Unity 코드/Catalog/설정 변경 및 TTS API/Push 0. 이번 Editor/Play 재실행 없음; 새 음성 제공 후 직접/Runtime 의미 청취가 다음 작업이다.

마지막 선행 관련 commit: `bd4bffeca1efb1789079197f3b215e19ddb0fe48`. 이번 문서 commit은 최종 보고 참조. 아래 기존 기록은 당시 이력이며 현재 판정은 이 단락과 최신 Matrix/재생성 목록을 따른다.

## 제작 설정과 연기 지침

Speaker Taeon / 태온 / companion_taeon, Voice Gacrux / Gacrux. LOCAL 보충팩 Manifest 및 CASTING.json의 모델 `gemini-3.8-flash-tts`, `voice_design=false`, ko-KR를 전달한다. 이는 로컬 제작 기록이며 현재 제공 API 지원 여부를 조회/검증한 것이 아니다. 새 Voice Design 없음. 기본 속도·Tone 수치·Batch는 기존 TBD, 임의 수치 지정 없음.

공통: 차분함·진지함·절제됨·존댓말·관찰력·논리적 연결, 과장된 감정 표현 없음. 개별 힌트는 Handoff notes에 기록한다. output_filename은 Manifest output_file의 basename 그대로 유지한다(Manifest의 taeon/ 제작 폴더와 Unity Main03 폴더를 혼동하지 않는다). 본문 줄바꿈과 문장 전체를 보존한다.

감사 CSV의 RMS/Peak 단위는 dBFS, PCM16의 기준 진폭은 32768이다. WAV 존재/Path/meta GUID/SHA256/길이/RMS/Peak 및 Catalog fileID/GUID/Speaker 전체 Entry를 원본 읽기만으로 기록한다. 새 WAV 채택은 별도 후속 작업이다.

## 검증 결과

전달 목록 생성 후 자동 검사 결과를 아래에 기록한다.

- 자동 검사 PASS: CSV 정확히4행·ID중복0·Speaker 전부태온·Exact Text/Output Filename 빈값0·012/Player/003~007 포함0·Runtime↔Manifest 줄바꿈 포함 mismatch0.
- Matrix331행/NEEDS_LISTENING marker244 유지·TTS_REGEN_REQUIRED 정확히4행·012 Runtime 의미PASS 보존. 기존3개 CSV 대상 외 모든 행을 HEAD와 필드별 비교해 동일 확인.
- Unity Assets/ProjectSettings 보호 해시 3018개 전부 동일. WAV/meta/Unity C#/Catalog/기존 정상 Voice 변경0. TTS API0/Editor 실행0/포커스전환0.
- 직접 변경 문서/CSV의 git diff --check 및 staged 검사 PASS. 저장소 전체 diff --check에는 작업 전부터 존재한 unrelated Unity Asset 공백 오류가 있어 별도로 남겨둔다. 사용자 변경은 Stage/수정하지 않는다.
- 다음 작업: 이4행으로 새 원본 제작→원본 직접 청취→명시적으로 승인된 후속 적용에서 기존 GUID/meta 보존 교체→Runtime 의미/전체문장/음량 확인.012 재생성 금지.
