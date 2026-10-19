# Main10 승인18 정식 연결·원본 적용 (2026-10-19)

상태: **APPROVED18_OFFICIAL_CONNECTED / ISOLATED_DELTA164_PASS_0_FAIL / ORIGINAL_DISK_APPLIED / USER_EDITOR_PLAY_NOT_VERIFIED / NO_PUSH_NO_MAIN_MERGE**.

전용 `codex/main10-voice-quest-interaction-fix-20261019`, 기능 `4cf37b0809cb2f278f03214114dde5ed91cfd716`. 기존 입력 수정5c36a653와132 PASS는 보존·재실행하지 않았습니다. Main09 승인27 최종424b1912를 포함한 브랜치이므로 향후 PR 범위에 선행 미병합 변경이 포함됨을 확인해야 합니다.

사용자 승인 후보18 SHA/PCM 그대로 `Audio/Voice/Story/Main10Approved20261019`에 WAV18+meta18+폴더meta1을 새로 추가했습니다. 기존 Main10 WAV/meta36은 덮어쓰지 않았고 Catalog290 중 Main10 18 GUID만 변경, 나머지272(승인 Main09 27 포함) 불변입니다. DialogueId/화자/자막 원문 불변. 승인된 폴 마지막 “음…”도 그대로 보존, API/신규TTS0.

Unity6000.6.5f1 격리 Batch **164 PASS / 0 FAIL**: 18개 자막·Catalog·정식Asset경로·PCM 해시·Play clock·Voice Mixer 설정, Player6 무음의 이전Clip 해제, Next/Stop/취소, 동프레임 빠른Next1페이지만 이동, 긴2파일 완주 후 자동Next 없음, Quest무변경·Continue·잔류음성0, Main09 승인 참조 확인. 기존 Search IndexOutOfRange/URP Host type mismatch 기준선은 남고 신규Runtime오류0. 음향 출력은 백그라운드 무음으로 실행했으므로 사용자 원본 Editor 실제 손플레이 PASS를 대신하지 않습니다.

원본 적용: 최신 원격main/원본HEAD `675988c9f2c5c507d9a696b0378be27c0bd3309b`. 기존3파일(Catalog/InteractionSystem/MainQuest10FieldFlow)의 내용이 예상 부모 버전과 일치함을 확인한 뒤 백업104 SHA 고정. 신규37+기존3만 비파괴 반영, 다른4232파일 SHA불변/UserData23 보존. 원본Editor/Play/Refresh/포커스0, 원본index/staged 변경0. 원본 미커밋 상태에는 이번 승인 파일이 추가되며 mainHEAD는 유지됩니다.

정확한 연결18 및 SHA: [ApprovedMapping](Evidence/Main10_Approved18_20261019/ApprovedMapping.json). [Runtime checks](Evidence/Main10_Approved18_20261019/checks.txt). 원음/백업은 로컬 `Temp/Main10Approved18_20261019/Backup`에 보존했습니다. 다음: 대표님 평소 원본Editor 자연Import·Main10 실제 청취/진행 확인, 별도 승인 시 선행Main09 포함 기능PR 준비. Push/PR/main병합은 이번 지시로 허용되지 않았습니다.
