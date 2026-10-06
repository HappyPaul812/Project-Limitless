## 2026-10-06 Main04 후속 Voice 3개 최종 반영 및 연속 Runtime PASS

`Limitless_TTS_Regen_Main04_PostFirstEncounter_3`의 태온004·미엘007·008을 기존 Unity WAV 경로에 반영했다. 사용자 최종 지시 “맡겠습니다 로 해.”에 따라 태온004 본문은 “또 옵니다.\n제가 앞을 맡겠습니다.”로 유지했다. Runtime·Manifest·Handoff·제작 보고서가 일치하며 본문/Registry/Catalog 변경은 없다.

실제 첫 조우10페이지 종료 → 태온004 → 미엘007 → Player “갑시다.” → Story Battle → QA 전용 승리 처리/실제 결과 버튼 → Field 복귀 → 미엘008 → 태온005 경계까지 연속 실행했다. **221개 기술 검사 PASS / FAIL 0**, 새3개 전체 PCM 원본 오차0. 사용자 이번 답변 **“다 잘 들린다”**로 새3개와 태온005의 의미·전체발화·화자·음량 PASS를 확인했다. **TTS_REGEN_REQUIRED 3 → 0**, **NEEDS_LISTENING 244 유지**. 경계005 WAV는 교체하지 않았으며 NEXT_AUDIT_BOUNDARY_FAIL은 없다.

Player Voice/Portrait0, 태온·미엘 Portrait 복원, UI Wrap/동적높이/본문·Footer·Portrait 비중첩, 자연 종료 후 Next, Battle 전후 잔류 제거와 최종 Cleanup PASS. 전략 전투 조작 및 Main04 전체 Quest 완료 검사는 수행하지 않았다. 경계 페이지의 로그 NEXT 표시는 공통 QA 기록이며 실제 Advance는 호출하지 않고 검증 후 Hide로 종료했다.

보호 대상 기존 3,044개 파일 중 변경은 대상 WAV3개뿐이다. 정상 첫 조우9개/Main03/태온005/meta/GUID/Importer/Catalog/게임 C#/설정/사용자 Save 변경0. 새 Editor QA와 meta만 추가했다. Compile Error0 · Console Error0, 기존 CS0618 경고2개는 별도 이력이다. 종료 clean Bootstrap Edit Mode/격리 Save 해제/is_focused=false, 포커스 전환0.

[Source 및 Unity PCM 감사](Main04_PostFirstEncounter_Final_Source_Audit.csv) · [연속 Runtime 결과](Main04_PostFirstEncounter_Final_Runtime_Results.txt) · [전체 Sequence](Main04_PostFirstEncounter_PreSelfCare_Sequence_Audit.csv). 다음 권장 작업은 범위 밖 Voice의 별도 청취 감사다. 선행 관련 commit `83d0b4fd684434a219d1c1a6f23d9445f6e4a0fa`, 이번 완료 commit은 최종 보고에 기록한다. GitHub Push 없음. 아래 준비 단계는 당시 이력이며 현재 판정은 이 단락을 따른다.

## 2026-10-06 본문 충돌 해소 및 새 Voice 3개 적용

사용자가 최종 답변 “맡겠습니다 로 해.”로 태온 004의 기존 정본 “또 옵니다.\n제가 앞을 맡겠습니다.” 유지를 지시했다. 중간 “막겠습니다” 의도는 이 지시로 대체되었다. Runtime·Manifest·Handoff·새 제작 보고서 본문이 일치하며 본문 변경은 없다. 새 WAV 3개를 기존 경로에 반영했고 연속 Runtime 검증 대기다. 의미 해결 및 TTS_REGEN_REQUIRED 3→0은 이번 Runtime 청취 확인 이후 판정한다. 첫 조우 9개/Main03/태온 005/meta/GUID/Catalog/Registry 보호.

## 2026-10-06 Main04 후속 Voice 3개 적용 준비

새 폴더 `Limitless_TTS_Regen_Main04_PostFirstEncounter_3`에 대상 3개가 제작되어 있다. 미엘 007·008은 Unity 반영 대기다. 태온 004는 사용자가 이번 답변에서 정식 본문을 “또 옵니다.\n제가 앞을 막겠습니다.”로 변경하는 의도를 확인했다. 현재 로컬 Flow·Manifest·Handoff와 새 제작 보고서는 “맡겠습니다”이므로 MANIFEST_RUNTIME_DRIFT에 앞서 사용자 최신 의도와 기존 정본의 충돌이 확인되었다. “막겠습니다” 원본을 준비하기 전 태온 004를 적용하거나 의미 PASS 처리하지 않는다.

문서화 → 구현 → 검증 순서로 진행한다. 첫 조우 정상 Voice 9개, Main03 전체, 종료 경계 태온 005 WAV, GUID·meta·Importer·Catalog·Registry를 보호한다. 연속 Runtime 완료 전 TTS_REGEN_REQUIRED 3 및 NEEDS_LISTENING 244를 유지한다. 선행 관련 commit `83d0b4fd684434a219d1c1a6f23d9445f6e4a0fa`.

# Main04 후속 SelfCare 직전 Voice 감사

## 2026-10-06 Main04 첫 조우 이후 SelfCare 경계 직전 Voice 3건 감사

직전 FirstConversation 10페이지 / 9 Voice의 사용자 Runtime PASS를 유지한다. 감사 시작은 `main04_taeon_supp_004` (EncounterConversation index 0)이다. 종료 경계는 `main04_taeon_supp_005` (AfterBattleConversation index 1), 태온의 “본인부터 보셔야 하는 것 아닙니까?”이며 **HANDOFF_INCLUDED=NO**이다. 범위는 EncounterConversation 3페이지 → Story Battle → AfterBattleConversation index 0까지다. 총 Dialogue 4개, Voice 3개(미엘 2 · 태온 1), Player Silent 1개이며 전투는 Dialogue 수에 포함하지 않는다.

사용자가 실제 Runtime에서 이 범위의 전반적인 자막/발화 불일치를 확인했다. Voice 3개를 **SEMANTIC_INTEGRITY_SUSPECT / TTS_REGEN_REQUIRED 후보**로 일괄 등록했다. 각 WAV의 정확한 오발화 내용과 Wrong/Partial 개수는 개별 청취 증거가 없어 미확정이다. Metadata 정상만으로 의미 PASS를 확정하지 않는다. 기존 WAV를 재배열하지 않고 현재 Runtime 본문으로 새 원본을 제작한다. 재생성 목록은 **0 → 3**, 기존 NEEDS_LISTENING 관리 범위는 **244**로 유지한다.

[단일 3행 TTS 전달](Main04_PostFirstEncounter_PreSelfCare_TTS_Handoff.csv) · [전체 4행 Sequence 및 원본 감사](Main04_PostFirstEncounter_PreSelfCare_Sequence_Audit.csv) · [해시/PCM 교차 비교](Main04_PostFirstEncounter_PreSelfCare_CrossComparison.csv). 이번 작업의 WAV, Unity 코드, Catalog, 본문, Main03 및 정상 Main04 Voice 변경은 모두 0이다. TTS API 실행, Editor/Play 재실행, 포커스 전환도 없다. 마지막 관련 commit은 `ca103570983e18711184f4d63eaa5e8e396a2056`이며 이번 문서 commit은 최종 보고에 기록한다. 다음 작업은 이 3개 원본 제작 → 청취 채택 → 별도 반영 및 연속 Runtime 검증이다.

## 검증 결과

정적 검사 PASS: Dialogue 4개 / Voice 3개 / Player 1개. Runtime과 Manifest 본문은 줄바꿈까지 일치하며 Missing은 0이다. Catalog GUID와 Speaker가 일치한다. Handoff는 3행(미엘 2 · 태온 1 · 기타 0)이며 중복 ID, 필수 빈 값, Player, 정상 첫 조우 9개, 종료 경계, Main03 ID 포함은 모두 0이다.

Main03 12개와 정상 Main04 9개, 총 21개 원본을 비교했다. 교차 비교 63쌍과 대상 내부 비교 3쌍, 총 66쌍에서 동일 SHA/PCM duplicate는 0건이다. 해시가 다르다는 사실은 발화 내용의 유사성이나 의미 일치를 판단하는 근거가 아니다. Wrong/Partial 개별 판정은 미확정이며 suspect는 3개다. 저음량 역시 청취로 확정하지 않았고 측정값은 Sequence CSV에 기록했다.

보호 대상 기존 파일 3,044개는 모두 해시가 동일하다. Matrix 대상 외 모든 행은 HEAD와 같고 244 관리 범위를 유지했다. 문서와 CSV만 변경했으므로 이번 Compile/Console/Runtime 검증은 재실행하지 않았다. 직전 Main04 최종 149개 검사 및 사용자 청취 9개 PASS 기록을 보존했다. 로컬 Flow의 EncounterConversation 종료 callback StartStoryEncounter → Story Battle 및 Quest 전투 목표 → MielActor.AfterBattleConversation 연결로 순서를 확인했다. 경계인 AfterBattleConversation index 1은 Handoff에 포함하지 않는다.
