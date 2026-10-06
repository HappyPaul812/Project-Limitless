# Main17 TTS 제외 최종 마감 계획

2026-10-06. 기준 commit c182a04, LOCAL 정본과 기존309PASS를 재사용한다.

1. Scene/import의 실제 규격을 추출해 환경 Art Handoff 작성. 새 PNG 생성/수정 없이 ART_WAITING_EXTERNAL로 관리한다.
2. Main12~17 Reward 감사: Main12~15 EXP60/탈렌트50, Main16~17 0/0. Main16 문서는 미확정 임시값임을 명시한다. 사용자 보상 결정 전 수치를 수정하지 않는다.
3. 실제 성장곡선·필수 Quest/Encounter 보상으로 Chapter2 레벨 범위를 계산한다. 5Job×Low/Typical/High×Story/General 대표 전투를 기존 생산 코드/Controller에서 검증한다. 자동 전투 정책과 표본 승률의 한계를 명시한다.
4. Battle Audio Handoff: 현재 null, AUDIO_WAITING_EXTERNAL. 실제 Cue/Mixer/Loop/Fade 구현 계약을 추출한다.
5. UI/연출의 영향 범위 회귀. 기존 Save5지점/Path309PASS를 반복 합산하지 않는다. 코드 변경 시 해당 경계 재검증.
6. 완료 문서·Checkpoint·CURRENT_STATUS 갱신, 관련 파일만 commit. 기존103작업 유지·Push0.

제외: TTS/WAV/사용자PNG/외부아트생성/Main18/Field09/Overheat/Elite/Boss/StandaloneBuild. 실물 입력3건 USER_INPUT_REQUIRED, 음악 적합성 USER_LISTENING_REQUIRED.

## 완료

Reward60/50 사용자승인·최소 구현·실제 완료/Continue와 레벨업경계 검증 완료. 5Job 대표40회+치유4회 모두승리. 고유180PASS/FAIL0, Console0·MissingScript0. Art/Audio 실제 추출 인계 문서 완료, TTS14/외부환경/외부Battle음악/수동입력 분리. 최종 체크포인트와 CURRENT_STATUS에 구현 관련 commit을 기록한다.
