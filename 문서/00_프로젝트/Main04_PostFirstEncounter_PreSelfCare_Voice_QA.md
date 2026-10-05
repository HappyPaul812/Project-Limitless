# Main04 후속 SelfCare 직전 Voice 감사

## 2026-10-06 Main04 첫 조우 이후 SelfCare 경계 직전 Voice 3건 감사

직전 FirstConversation 10페이지 / 9 Voice의 사용자 Runtime PASS를 유지한다. 감사 시작은 `main04_taeon_supp_004` (EncounterConversation index 0)이다. 종료 경계는 `main04_taeon_supp_005` (AfterBattleConversation index 1), 태온의 “본인부터 보셔야 하는 것 아닙니까?”이며 **HANDOFF_INCLUDED=NO**이다. 범위는 EncounterConversation 3페이지 → Story Battle → AfterBattleConversation index 0까지다. 총 Dialogue 4개, Voice 3개(미엘 2 · 태온 1), Player Silent 1개이며 전투는 Dialogue 수에 포함하지 않는다.

사용자가 실제 Runtime에서 이 범위의 전반적인 자막/발화 불일치를 확인했다. Voice 3개를 **SEMANTIC_INTEGRITY_SUSPECT / TTS_REGEN_REQUIRED 후보**로 일괄 등록했다. 각 WAV의 정확한 오발화 내용과 Wrong/Partial 개수는 개별 청취 증거가 없어 미확정이다. Metadata 정상만으로 의미 PASS를 확정하지 않는다. 기존 WAV를 재배열하지 않고 현재 Runtime 본문으로 새 원본을 제작한다. 재생성 목록은 **0 → 3**, 기존 NEEDS_LISTENING 관리 범위는 **244**로 유지한다.

[단일 3행 TTS 전달](Main04_PostFirstEncounter_PreSelfCare_TTS_Handoff.csv) · [전체 4행 Sequence 및 원본 감사](Main04_PostFirstEncounter_PreSelfCare_Sequence_Audit.csv) · [해시/PCM 교차 비교](Main04_PostFirstEncounter_PreSelfCare_CrossComparison.csv). 이번 작업의 WAV, Unity 코드, Catalog, 본문, Main03 및 정상 Main04 Voice 변경은 모두 0이다. TTS API 실행, Editor/Play 재실행, 포커스 전환도 없다. 마지막 관련 commit은 `ca103570983e18711184f4d63eaa5e8e396a2056`이며 이번 문서 commit은 최종 보고에 기록한다. 다음 작업은 이 3개 원본 제작 → 청취 채택 → 별도 반영 및 연속 Runtime 검증이다.

## 검증 결과

정적 검사 PASS: Dialogue 4개 / Voice 3개 / Player 1개. Runtime과 Manifest 본문은 줄바꿈까지 일치하며 Missing은 0이다. Catalog GUID와 Speaker가 일치한다. Handoff는 3행(미엘 2 · 태온 1 · 기타 0)이며 중복 ID, 필수 빈 값, Player, 정상 첫 조우 9개, 종료 경계, Main03 ID 포함은 모두 0이다.

Main03 12개와 정상 Main04 9개, 총 21개 원본을 비교했다. 교차 비교 63쌍과 대상 내부 비교 3쌍, 총 66쌍에서 동일 SHA/PCM duplicate는 0건이다. 해시가 다르다는 사실은 발화 내용의 유사성이나 의미 일치를 판단하는 근거가 아니다. Wrong/Partial 개별 판정은 미확정이며 suspect는 3개다. 저음량 역시 청취로 확정하지 않았고 측정값은 Sequence CSV에 기록했다.

보호 대상 기존 파일 3,044개는 모두 해시가 동일하다. Matrix 대상 외 모든 행은 HEAD와 같고 244 관리 범위를 유지했다. 문서와 CSV만 변경했으므로 이번 Compile/Console/Runtime 검증은 재실행하지 않았다. 직전 Main04 최종 149개 검사 및 사용자 청취 9개 PASS 기록을 보존했다. 로컬 Flow의 EncounterConversation 종료 callback StartStoryEncounter → Story Battle 및 Quest 전투 목표 → MielActor.AfterBattleConversation 연결로 순서를 확인했다. 경계인 AfterBattleConversation index 1은 Handoff에 포함하지 않는다.
