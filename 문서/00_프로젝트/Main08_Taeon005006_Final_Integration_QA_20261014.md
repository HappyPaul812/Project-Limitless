# Main08 태온005·006 최종 음성 마감·연결 QA — 2026-10-14

바울 대표 보고: **대상 기능 QA PASS(224/0), 원본 디스크 연결 완료, 전체 PARTIAL(기존 Editor/실물 출력 미검증)**.

## 승인과 정본

- docs/planning `2cf3d7d6`의 Main08_Trace03_Taeon005006_Voice_Boundary_Followup_20261014.md 최신 청취 승인 기록 확인. 바울 대표가 두 분리 후보의 발화·음색을 승인했고005 시작 여백만 소폭 보정 요청,006은 승인 후보 WAV 전체 그대로 사용.
- 최신 origin/main `bddca540d2fdd9e93f642f8d0f387b34730bd2d8`에서 별도 worktree/브랜치 준비. 신규TTS/API0·대사/화자/게임플레이 변경0.

## 005 공백 원인·편집 결과

- DialoguePresenter.RefreshSequenceText는 페이지 갱신 때 VoicePlaybackSource.Play→AudioSource.Play를 동기 호출합니다. 별도 PlayDelayed/PlayScheduled 대기 없음. 실제 격리 Field02 재생 시계의 첫 양수 timeSamples까지005 **12.896ms**,006 **39.729ms**입니다. 긴 체감 여백의 주원인은 승인 후보 내부 신호/무음 구간입니다.
- 시작0~185.833ms(4460프레임)는 RMS **-64.03dBFS**, Peak **-54.05dBFS**의 저신호 배경 영역입니다. 0.3초 부근부터의 약한 활동도 자연스러운 숨소리일 수 있어 보존했습니다. 첫 절단점 직전/직후 모두PCM0, 최종 첫 샘플0.
- **005 길이6.729458→6.543625초**. 약한 활동(RMS>-50dBFS/10ms윈도) 시작 약0.32→0.14초. 강한 발화 신호(RMS>-35dBFS)는 약2.52→2.33초입니다. **0.14초를 ‘초원에서는’ 첫 음절 시작이라고 주장하지 않습니다.** 호흡·중간 여백을 남겨 본격 발화 전 시간이 여전히 약2.33초 있습니다. 더 줄이기 위해 호흡 후보나 내부 구간을 삭제하지 않았습니다.
- 남은005 PCM=승인 후보의 [4460,161507) 샘플 suffix **100%동일**. 첫 음절부터 말끝·호흡·종료 여운 전체 보존. fade/Normalize/denoise/EQ/속도변경/재샘플링0. clip0·zero-crossing 시작·급격한 시작 변환0. offline ASR 정본문장 정확일치. 사람의 신규 청취를 대신했다고 주장하지 않습니다.
- **006 길이3.800542초·승인 WAV 바이트100%동일**, SHA256 `796f05398dbfe8de2cfb892edaa52bdf3679d500e10156e5640230a8658bbd00`.
- 최종005 WAV SHA256 `29fa3905e58ad23a0a3108922526e356147dbffbd478d68f765c777cee6d25b7`, PCM SHA256 `6ec392032b7031d18b5b3c017cd01ac345adf9dc222bfc01f4d0f98502a31c31`. 이전005+006 재결합=합본원음 증명은 기존 후보 자료에 보존합니다. 이번005는 외곽4460샘플을 제거했으므로 최종두파일 재결합이 합본과byte-identical하다고 주장하지 않습니다.

## 연결·QA

| 항목 | 결과 |
|---|---|
| 새 등록 | _Project/Main08/ApprovedTaeon00500620261014 WAV2·고유GUID/meta3, GUID충돌0 |
| Catalog | main08_taeon_supp_005/006 두 clip GUID만 변경. speakerId/fileID/나머지행 동일 |
| Runtime | Field02 실제 QuestInteractable→DialoguePresenter로 Trace01/02/03·WheelTracks 실행 |
| 승인8 보호/재생 | 기존6+신규2 GetData PCM·CatalogGUID·화자·자막·초상화 각일치 |
| 페이지경계 | 005→006→Player무음→미엘004 및WheelTracks 기존후속. 자연끝 자동넘김없음·동일프레임Next중복차단·빠른Next/닫기·Pause/Resume PASS |
| 저장 | 격리SaveVersion1·실제BootstrapContinue→Field02·Quest동일·staleClip0·Trace03 재호출005재생·취소정리 PASS |
| Compile/Runtime | 신규Compile0·게임오류0. 기존UnityEditor.Search 시작 인덱스 예외 별도PARTIAL |
| 결과 | 224PASS/FAIL0·실제UnityRT캡처4장. 합성Preview가아님 |
| 검증제한 | AudioSource시계/PCM을검사, AudioListener0으로스피커출력없음. 물리스피커·OS키입력·원본Editor메모리강제Import·정본6000.5.7f1 미검증 |
| 조우 | 대화중일반MonsterEncounter기존OPEN은별도. 이번음성시험은SuppressForSeconds를사용, 해결/자연조우PASS로보고하지않음 |

## 원본 보호·반영

- 원본디스크새WAV2/meta3·Catalog2참조 반영. 기존소스005(다른대사)/006(합본)을overwrite/rename/delete하지 않음. 기존 승인6·미엘004·다른 모든WAV/GUID/meta·Scene·Quest·Save·TTS/BGM·Field04·Arbel·Sprite복구 보호.
- baseline5223파일 중 Catalog와CURRENT_STATUS의 승인된 변경2개를 제외한5221 SHA 불변, 기존WAV/meta 변경0, 원본main HEAD `ff022e63dfe364f559fcc29819d27b123ad31127` 유지. 기존 dirty작업을스테이징하거나되돌리지않음.
- 원본Editor 시작/종료 모두Bootstrap·PlayOFF·비포커스. Refresh/Play/Scene전환/OS창조작0. 디스크연결과현재Editor 캐시갱신은별개이며 외부변경dirty만조회했습니다.
- 롤백자료: Temp/Main08TaeonFinal20261014/OriginalBackup의Catalog·CURRENT_STATUS 원본바이트, 기존원본WAV/meta는현경로그대로보존. 되돌릴때Catalog두참조및이번새등록만범위로삼아기존작업을보호해야합니다.
- 기능Commit `fe588f9af1369ec825a9b9820ebfb0a92dc9499a`, 브랜치 `codex/main08-taeon005006-final-20261014`. 문서/증거/상태는 별도DocsCommit으로기록하며요청된전용브랜치만Push. main직접Commit/Push·PR병합0. 원격Push완료SHA는최종보고와Temp의push_receipt.json에기록합니다.

## 다시 청취

최종 파일은 [005](../../Unity/Client/Assets/_Project/Audio/Voice/Story/Main08/ApprovedTaeon00500620261014/main08_taeon_supp_005.wav)·[006](../../Unity/Client/Assets/_Project/Audio/Voice/Story/Main08/ApprovedTaeon00500620261014/main08_taeon_supp_006.wav) 등록 경로 및 Temp/Main08TaeonFinal20261014/Delivery/FinalCandidates에 보존했습니다. 독립 청취 페이지는 Delivery/Listen_Final.html입니다. 원본 작업 재개 후 정상Import가 끝나면 해당두페이지와Player무음 경계를직접확인하실수있습니다.
