# Main07 Early7 적용 및 Paul 전체 WAV 복구

## 구현 전 계획 — 2026-10-07

신규 Early7 원본의 ID/화자/본문/checkpoint SHA/PCM/형식을 확인하고 원본을 수정하지 않은 채 Main07 정식 경로에 복사·Catalog 등록한다. 원본 보고서의 청취 PENDING은 자동 QA와 구분한다.

사용자 Page0에서001+002, Page1에서이전+현재 음성이 들린 관찰에 따라 Paul001~020 전수로 원본·Archive·생성 경로·PCM 중복·무음 구간·문장 내용을 조사한다. 정상 개별 원본을 우선한다. 발화와 경계가 정확히 확인된 경우에만 PCM sample slice로 복구하고, 애매한 경계는 자르지 않는다. TTS API 생성/소비0. 기존 Source와 수정전 Unity PCM을 보존한다.

Early7 실제페이지 Resolve와 Paul001/002 무입력 및 명시Next, 전체Paul 경계를 격리 비포커스 Runtime으로 검증한다. 다른Voice/meta/본문/Quest/244목록/UserSave 보호. 최종 상태는 실제 확인과 남은 미검증을 구분하며 관련파일만 commit·Push0.

## 최종 결과 — PCM 복구·Runtime 완료

### Early7

`Limitless_TTS_Main07_Early_7` 원본7을 checkpoint/manifest ID·화자·본문/SHA/PCM/24kHz Mono16bit와 대조해 복사했다. 신규 Asset이므로 GUID7 생성, 기존 Main07 Importer 정책을 복제했고 Catalog에는7만 추가했다. 기존 ID/GUID/Speaker 참조 모두 보존했다. 원본 변경0, NPC Runtime resolve/PCM7PASS, Player Voice/Portrait0. 독립 한국어 ASR7은 해당 본문과 대응하며 사용자 실제 청취7은 원본 보고서대로 PENDING이다. [7개 Unity 매핑/SHA](Main07_Early7_Applied_Mapping.csv).

### Paul 원인과 전수 감사

**UNCUT_MULTI_SENTENCE_WAV + WRONG_SEGMENT_BOUNDARY / SEGMENT_INDEX_SHIFT**. Catalog ID→GUID 연결은 정상이나 잘못 분할된 chunk 번호가 개별 Dialogue ID에 배정돼 있었다. 수정전001은 Paul001/002/003 세페이지를 포함(13.37초),002는 실제Paul004/005(9.99초),003은 실제Paul006/007(8.57초).004는 “아…” 단편,005는Paul008 나머지,014는 실제Paul017/018/019,015는 실제Paul020,016~020에는 Main09 대사가 들어 있었다. 동일 fullPCM 복제는 아니며 개별WAV hash/PCM은 서로 달랐다. “누적음성” 사용자 관찰은 새 Runtime Clip에서 잘못된 묶음이 반복 재생된 결과와 부합한다.

본래 API 호출·분할 스크립트는 LOCAL/Downloads/조회 가능한 “게임 음성 tts” 최근 기록에서 발견하지 못했다. **여러 문장 일괄 요청 여부, API context 이어읽기, 당시 silence threshold/segment index 구현은 미확인**이다. 현재 출력의 묶음/단편/번호밀림은 전수 PCM·ASR로 확인했지만 과거 요청 구현까지 추측으로 확정하지 않는다. 범위밖 Main09 Asset은 수정하지 않았으며 같은 구원본 batch의 Paul 후속 구간은 별도 감사 권장.

[원본20 감사 CSV](Main07_Paul_Full_Original_Audit.csv): 예상본문/실제ASR/포함페이지수·문장segment수/SHA/PCM/rate/channel/bit/duration/시작·끝·내부무음. ASR segment 수는 grammatical sentence나 DialoguePage 수와 동일하지 않으므로 각각 구분했다. [원본 ASR 단어시각](Main07_Paul_Original_ASR.json).

### 복구

정상 개별명 파일은 없었지만 기존15개의 PCM 안에 Main07 Paul20페이지가 순서대로 모두 존재했다. 복구할 원문페이지 경계와 -45dB RMS10ms 긴 무음 경계를 대조했다.

- 원본001→정식001/002/003,002→004/005,003→006/007,014→017/018/019: sample slice. 원본001의002앞 불필요한 “아”는4.83~5.09 무음 중간4.96초부터 취해 제외했다.
- 원본004 “아…” + 원본005 후속문장은 원문Paul008 한페이지이므로 rawPCM 그대로 연결했다. 새 무음이나 crossfade를 만들지 않았다.
- 나머지는 정상 해당페이지가 담긴 기존chunk PCM을 올바른 ID에 재배치했다. 기존 GUID/meta 유지,재인코딩0/리샘플링0/음소추정합성0. 원본PreSerin과수정전Unity20 snapshot(`Temp/Main07Recovery/original-unity`) 보존.
- [복구20 Matrix](Main07_Paul_Recovery_Matrix.csv): 예상본문/복구PCM구간/방법/전후길이/SHA/Runtime/ASR/청취상태. [sample 단위 증거](Main07_Paul_PCM_Recovery_Proof.json)는 각 원본rawsample slice hash와 최종concatPCM byte일치를 기록한다.

최종 Paul001 **4.03초**, Paul002 **3.56초**. [복구 ASR](Main07_Paul_Recovered_ASR.json)는20개 모두 해당페이지 발화만 대응한다. ASR의 수레/수례,깊게/쉽게,바퀴 등의 인식 오차를 실제발화 오류로 자동확정하지 않았으며 이를 사람 청취 PASS로 승격하지 않는다. **사용자가 이번대화에서 복구001/002를 직접 들은 뒤 “두 파일 모두 해당 대사만 나옴” 확인: USER_LISTENED_PASS2.** [청취확인·파일SHA](Main07_Paul_User_Listening_Confirmation.json). 나머지Paul18·Early7의 사람청취는PENDING.

### Runtime / 보호

Main06 실제조사완료→기존Field02재진입설치→Main07→WheelTracks4→WoundedTraveler4→실제PaulTrail trigger/PaulFirst0·1·입력회귀, 그리고 같은Scene의 원문Paul배열에서20Voice 전부를 자연종료까지 검사했다. 전체Paul20 검사는 StoryBattle를 임의진행하지 않는 Voice fixture이며 실제Paul0/1 및 초반7 상호작용 연속검사와 구분한다.

**227PASS/FAIL0**, 27RuntimePCM 전sample오차0, 단일Source/각페이지한번/재시작0/직전Clip정리PASS. Paul0 무입력 **10.036초** page0/001유지·002Playback0, 명시Next1회 page1/002. 사용자확인001은첫문장만,002는002문장만. 같은프레임E/A 재열림0·복합입력1page·held입력반복0. CompileError0,ConsoleError/Warning0,MissingScript0. 기존 입력수정89ff828 유지, 게임대사/순서/Quest/SaveVersion 변경0.

보호 baseline 3240byte동일, 기존변경은PaulWAV20/Catalog1/QAhelper1, 신규7WAV+meta14. Main03~06/Main17/다른Voice/meta/244NEEDS_LISTENING전체 목록/원본/UserSave 보호. 최종Bootstrap EditMode/is_focused=false/격리Save·Settings해제/InputSettings값복원. [검증JSON](Main07_Recovery_Verification.json),[로그](Main07_Recovery_Runtime_Results.txt),[Trace](Main07_Recovery_Runtime_Trace.jsonl).

**TTS_REGEN_REQUIRED0 / 신규TTS API0 / 남은TTS3 소비0.** Early7+Paul20 기술복구완료, Paul001/002 사용자청취PASS. 27전체 사람청취 완료라고 보고하지 않는다. 복구확신을 높이기 위한남은청취25와과거API생성코드미확인만구분한다. Asset commit `c231565`(Early7),`5ed20c2`(Paul20), QA/문서는이기록포함최신Docs commit. Push0.
