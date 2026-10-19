# 로컬 문서5 충돌 복구·실제 main 동기화·다음 단계 점검 (2026-10-19)

## 결과

**문서5 의미 병합 PASS / 원본 Git SYNC_COMPLETE / 원본 보호 PASS / TTS 읽기 전용 점검 DONE / Main22 구현 보류.** Unity 재실행·Play·포커스·Refresh·테스트 빌드·API 호출·신규 음성 생성은 모두 0입니다. Main09 427 PASS/0 FAIL, Field05 416+84 및 정식 적용 회귀66 PASS/0 FAIL은 기존 결과를 재사용했습니다.

## 문서와 이력 보존

- 문서 브랜치 `codex/local-doc5-semantic-merge-20261019`, 기능 커밋 `bc0ffac095746a499a31792aea2bef0a7e39691b`, [PR #25](https://github.com/HappyPaul812/Project-Limitless/pull/25), main 병합 `30954827ebbe9d2f2fd4e6bca22abd2f50e6acc7`.
- 비교 대상은 로컬 작업복사·로컬 HEAD·공통 조상·원격 main 네 버전입니다. 원격 본문을 유지하고 로컬 고유 6개 문단을 ‘과거 당시’ 이력으로 구분했습니다. 네 버전의 모든 본문 문단 포함 검증 PASS, diff --check PASS, 변경5파일/추가85줄/삭제0줄입니다.
- CURRENT_STATUS: 10/14 최종 마감·아르벨v3 정식 적용·Main08 태온007~011 수동 복구 후보 인계 3문단 보존. EOD: PR14/15 후 원격/로컬 SHA와 당시 PARTIAL 경계 1문단 보존. CODEX: 10/14 최종 마감 및 A/B 완료·재개 경계 2문단 보존. PLANNING/WORLD_ART 로컬 본문은 이미 원격에 포함돼 중복 추가0, 다른 담당자 원격 고유 내용 유지.
- 당시 Main18 미완료·Main08 승인 대기·로컬 PARTIAL 표시는 역사 기록이며 최신 완료 상태를 되돌리지 않습니다. 기존 OPEN/QA/승인 수치와 근거는 삭제하지 않았습니다.

## 실제 원본 동기화와 보호

- 시작 main `ff022e63dfe364f559fcc29819d27b123ad31127`, 원격 `af21edbf5f0cdc0f241b09228ede960ab83e9b6c`. 시작 tracked dirty327 / untracked1055.
- `Temp/Doc5Recovery20261019/Backup`에 dirty·미추적·UserData·ProjectSettings·UserSettings 1,430파일 SHA256 검증 복사. 원본 index/문서4버전/줄바꿈/수정시각도 보존. 원격 갱신 대상 기존 clean11파일은 `CleanFFBackup`에 추가 SHA 백업했습니다.
- 격리 Simulation에 기존 HEAD/index와 백업 작업파일을 재현했습니다. 원격과 이미 동일한 내용만 index에 대응시키고 정상 fast-forward하는 방식입니다. 첫 실행은 복사된 index stat의 clean파일4개에서 Git 보호 중단; 내용이 기존 HEAD와 같음을 확인한 clean11파일의 index stat만 다시 기록해 해결했습니다. 원본에서 충돌을 우회하거나 강제 덮어쓰지 않았습니다.
- 격리 결과 HEAD/index 목표 일치, 보호1,425파일 SHA 불일치0. 원본 적용 직전 1,430파일 SHA 재확인 후 문서5개만 승인된 의미 병합으로 갱신하고 같은 정상 fast-forward를 완료했습니다.
- 원본 확인점: HEAD=origin/main=GitHub main=`30954827ebbe9d2f2fd4e6bca22abd2f50e6acc7`, index tree=HEAD tree, staged0. 보호1,425파일 SHA 불일치0, UserData23파일 포함. Main09 승인9파일·Field05 Scene/아트·Main18 및 기존 WAV/Catalog·Settings·사용자 이미지/메타는 백업 시점 그대로입니다.
- 기존 로컬 문서 커밋 `ff022e63...` 및 `3a37aa2e9a972b1ff9de7c051c6c744b057c918f`의 main 조상 관계 PASS. 별도 재적용하지 않았습니다. 남은 tracked dirty308 / untracked548은 사용자 변경으로 보존하며 clean 상태라고 주장하지 않습니다.
- reset/clean/stash/강제 Push/사용자 파일 임의 교체0. 후속 보고 PR 병합 뒤 동일 보호 확인을 거쳐 정상 fast-forward할 예정이며, 최종 SHA는 최종 보고와 로컬 `FinalVerification.json`을 따릅니다.
- 증거: `BackupManifest.json`, `DocumentVersions.json`, `SemanticInventory.json`, `SyncPlan.json`, `SimulationResult.json`, `OriginalSyncResult.json`. 모두 위 Temp 폴더에 있으며 원본 보존용 백업 자체는 Git에 넣지 않습니다.

## 원본 Editor 확인 경계

원본 Editor 상태·자연 Import·실제 화면·신규 Console는 이번에 직접 확인하지 않았습니다. 기존 Search/URP 환경 OPEN과 로컬 Unity6000.6.5f1/원격6000.5.7f1 차이는 그대로 보존했습니다. 바울 대표가 평소 원본 Unity를 사용하실 때 자연 Import 이후 Main09 목표/마커와 승인 Field05 B42 화면을 확인하시면 됩니다. Codex가 창 전환·Play·강제 Refresh하지 않습니다. 별도 플레이 검수 환경이나 빌드를 만들지 않았습니다.

## Main20·21 TTS 정확한 인계

- Catalog290개 중 Main20=0, Main21=0. Manifest 총31행 중 음성대상22(Main20 세린5 + Main21 17), 비음성9. 음성대상22개는 DialogueCatalog의 ID/화자/원문과 정확히 일치합니다(CSV의 CRLF만 LF로 비교).
- 비음성 `main21_north_direction_01`은 Manifest의 화자명 ‘지문’ 및 본문과 Runtime의 빈 화자명+`<지문>` 접두 표시 차이1개가 있습니다. TTS 제외 상태이며 임의 수정하지 않았습니다. 나머지30행 일치.
- 기존 로컬 `F:\Downloads\Voice\Limitless_TTS_Main20_Main21_20261019`에서 후보17개 발견. WAV SHA/원문 SHA/24kHz mono16bit 읽기 검증17/17 PASS. 저장 성공과 청취 승인은 별개입니다. 원본 후보/results/checkpoint/lock 변경0.
- 최신 `docs/tts`의 `TTS_Main20_Main21_VoiceFix9_20261019.md` 기준 세린8+태온1 음색 오류9개는 사용자 보고 FAIL. 나머지8개는 미보고이며 청취 PASS로 간주하지 않습니다. 현재 승인 가능한 신규 통합0개입니다.
- 파일 자체가 없는5개: 태온 `main21_haren_taeon_02`, 세린 `main21_prepare_serin_01`, 하렌 `main21_haren_haren_01/02/03`. 하렌 Voice 미정. 기존17개를 처음부터 다시 제작하도록 발주하지 않습니다.
- 아래 CSV는 전체22개 원문·화자·ID·등록 상태·후보 경로·WAV SHA·PCM·초수·검수 상태를 포함합니다. TTS 담당이 먼저 오류9개의 샘플 검수와 나머지8개 청취 승인 여부를 확정하고, 없는5개를 별도로 정리해야 합니다. Codex는 생성 스크립트/API를 실행하지 않았고 외부 메시지를 발송하지 않았습니다.

[전체 누락22개 CSV](Main20_Main21_TTS_Missing_ReadOnly_20261019.csv)

| Dialogue ID | 화자 | 로컬 후보 상태 |
|---|---|---|
| `main20_confront_serin_01` | 세린 | 음색 오류 보고·재검수 대기 |
| `main20_confront_serin_02` | 세린 | 음색 오류 보고·재검수 대기 |
| `main20_confront_serin_03` | 세린 | 음색 오류 보고·재검수 대기 |
| `main20_after_colossus_serin_01` | 세린 | 음색 오류 보고·재검수 대기 |
| `main20_after_colossus_serin_02` | 세린 | 음색 오류 보고·재검수 대기 |
| `main21_heat_serin_01` | 세린 | 음색 오류 보고·재검수 대기 |
| `main21_pulse_serin_01` | 세린 | 음색 오류 보고·재검수 대기 |
| `main21_route_paul_01` | 폴 | 후보 있음·청취 승인 미확인 |
| `main21_route_serin_01` | 세린 | 음색 오류 보고·재검수 대기 |
| `main21_route_paul_02` | 폴 | 후보 있음·청취 승인 미확인 |
| `main21_route_taeon_01` | 태온 | 음색 오류 보고·재검수 대기 |
| `main21_arbel_leon_01` | 레온 | 후보 있음·청취 승인 미확인 |
| `main21_aid_miel_01` | 미엘 | 후보 있음·청취 승인 미확인 |
| `main21_aid_paul_01` | 폴 | 후보 있음·청취 승인 미확인 |
| `main21_aid_miel_02` | 미엘 | 후보 있음·청취 승인 미확인 |
| `main21_carriage_leon_01` | 레온 | 후보 있음·청취 승인 미확인 |
| `main21_haren_haren_01` | 하렌 | 파일 없음·Voice 미정 |
| `main21_haren_taeon_01` | 태온 | 후보 있음·청취 승인 미확인 |
| `main21_haren_haren_02` | 하렌 | 파일 없음·Voice 미정 |
| `main21_haren_taeon_02` | 태온 | 파일 없음 |
| `main21_haren_haren_03` | 하렌 | 파일 없음·Voice 미정 |
| `main21_prepare_serin_01` | 세린 | 파일 없음 |

## Chapter3 Main22 준비 상태와 바로 다음 순서

- 최신 기획: `docs/planning`의 Main22 상세기획(10/14)은 STORY_DETAIL_DRAFT / USER_REVIEW_PENDING / OBJECTIVE_IDS_TBD / ART_TTS_NOT_COMMISSIONED입니다. 최신10/19 Main23~24 서릿길 마을 시퀀스는 Main22 F1 첫 서리들쥐 전투를 반복하지 않으며, Main23은 F1심부→서릿길 마을 남문, Main24는 F2 첫눈/한기입니다. 이전10/14 Main23 F2 첫눈 메모보다 이 후속 경계를 우선합니다.
- 실제 Unity에 Main22 Quest/Flow/북부전용 Scene/서리들쥐·설원족제비 자산은 아직 발견되지 않았습니다. 기존 FrostNova VFX와 다른 Field의 RockNorth는 신규 북부 아트 납품으로 간주하지 않습니다. 필요 아트는 F1 푸른 풀+얇은 서리·발자국 조사점·통행 가능한 길, 서리들쥐, 선택 설원족제비, 필요한 표식입니다. 공식 ID/좌표/스탯/대사/납품규격은 승인 전 확정하지 않습니다.
- 기존 QuestService·SceneTransitionService/Trigger·SceneSpawnPoint·WorldBounds2D·기존3인 전투·SafeZonePetServiceRollout을 재사용할 수 있습니다. `Main21_ReturningWarmth.asset`의 nextMainQuestId는 실제 빈 값입니다. Main21 완료13단계/보상/Save를 보존하면서 Available/Continue 연결이 필요합니다.
- 마을 북쪽 실제 Portal·Collider·Spawn·Bounds 연결은 이번 준비 점검에서 완전 감사하지 않았으므로 OPEN입니다. 신규 Portal을 추가하지 않았습니다. Main22에서 Main21 하렌 첫 만남·마차 귀환을 반복하거나 강제4인 전투/한기 시스템을 끼워 넣지 않습니다.

1. TTS 담당에게 위22개 CSV와 최신 VoiceFix9 인계를 전달해 청취 승인/오류/파일미존재5개를 구분합니다. Codex 통합은 승인 WAV와 해당 SHA가 확정된 뒤 별도 지시로 시작합니다.
2. 바울 대표와 기획 담당이 Main22 목표ID·북부 F1 정식 Scene/왕복 동선·조사점·보상·승인 아트 계약을 확정합니다. 현재 문서 초안만으로 대량 구현하지 않습니다.
3. 다음 Codex는 별도 기능 worktree에서 기존 마을 북쪽 Portal/Collider/Spawn을 읽기 전용 조사하고 Main21→Main22 Available/Save 경계를 먼저 설계합니다. 승인된 최소 범위에서 마을북문→F1 조사→서리들쥐1기 지정전투→복귀/저장 순으로 진행합니다.

**재개 지시:** “AGENTS·CURRENT_STATUS·CODEX_LATEST와 본 복구 보고를 읽고 FinalVerification.json의 HEAD/index/보호 결과를 확인하세요. 기존 Main09/Field05/Main18 QA는 반복하지 마세요. Main20·21은 CSV22개와 docs/tts 최신 VoiceFix9 및 사용자 청취 승인을 대조만 하며 API를 호출하지 마세요. Main22는 최신 docs/planning과 승인된 ID/아트 계약을 확인한 뒤 북문 기존 동선 감사부터 별도 worktree에서 시작하세요. 원본 dirty/Save/Editor는 보호하세요.”
