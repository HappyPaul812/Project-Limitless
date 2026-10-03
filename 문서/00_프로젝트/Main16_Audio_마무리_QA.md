# Main16 Voice / BGM 마무리 QA — 2026-10-03

## 적용

정책 선행 commit `f0a7129`, 기능 commit `012be7d`. 정식 입력은 `F:/Downloads/Where_the_Earth_Breathes.mp3` 한 파일이며 Unity 저장 위치는 `Assets/_Project/Audio/Music/Where_the_Earth_Breathes.mp3`다. 원본 MP3 44.1kHz stereo 192kbps·180.166458초(디코더별 Unity 길이180.166534초), 4,330,073바이트를 그대로 복사했다. SHA256/GUID는 [입력 JSON](Main16_Audio_입력.json)을 따른다. Streaming·원본 rate·강제 모노/Normalize 없음. Trim/EQ/Re-encode/Gain/속도/Pitch 편집 없음.

기존 BgmPlaybackService와 BGM Mixer를 사용하며 Catalog에 Field07 한 Scene 배정만 추가했다. Scene Asset·Quest·Voice Mapping·Appearance·전역 Audio 기본값은 수정하지 않았다. Source gain1/pitch1/Loop, Voice100/BGM80을 유지한다. Ducking/Crossfade를 추가하지 않는다.

사용자가 최초 Field07 전투 Steel 승인 응답을 철회한 뒤 **기존 null/TBD 유지**를 선택했다. 따라서 Field07 일반전·불씨망령 Story Battle은 탐색곡을 정리하고 음악 미정 상태로 유지한다. Steel and Sunlight는 기존 Chapter1에 유지한다. Chapter2 전체 전투곡을 새로 확정하지 않는다.

## 후보

Trail_of_the_Ember_Wraith.mp3(179.983625초)는 최초 목격 연출 후보, Beneath_The_Cracked_Earth.mp3(176.483208초)는 Main17·협곡 이후 후보다. 모두 Downloads에서 단일 파일을 확인했고 기존 후보 보관 Convention을 따라 Import/Runtime 배정을 하지 않았다. 기존 후보4곡·Beneath the Ashen Sun 소개 Hook·Paths of Cracked Earth Field04~06·The Weight of Obsidian 최초 Scene 미정 경계를 유지한다.

## 검증 방식과 범위

Main16 전용 Editor 감사 도구에서 Save/Settings를 격리하고 사용자가 승인한 Unity Foreground/Game View에서 실행했다. 기존 Appearance 검수 도구 호출은 자동 승인 검토에서 범위 밖으로 거부되어 사용하지 않았다. Main16 전용 진입으로 바꾸어 허용된 Audio 범위만 검증했다.

- 실제 Field06 서쪽/Field07 동쪽 Trigger를 통한 왕복과 곡 교체. Title·Arbel·Field06·Field07의 기존/신규 곡, 단일 서비스·Listener·BGM Group·Loop/gain/pitch 확인.
- Field07 실제 Save→Bootstrap Continue와 곡 복원. 음악 재생 위치를 Save에 추가하지 않았다.
- Field07 일반 전투 진입은 공용 EnterBattle에 실제 spawn을 전달했다. 음악 null과 ReturnToField(false)의 탐색곡 복귀를 확인한다. 일반전 승리 검증으로 표현하지 않는다.
- Story Battle은 실제 EnterStoryBattle·공격/타겟 버튼·VictoryReturn을 사용한다. 복귀 후 잔향 Dialogue factory를 기존 대사 UI에서 재생한다. Quest의 전체 Objective 검증을 반복하지 않았다.
- Defeat는 기존 ReturnAfterDefeat API를 호출한 Audio 경로 검증이다. 실제 명령 전멸 검증으로 표현하지 않는다.
- 실제 Path ID에 따라 기존 Hearing/Default factory의 서로 다른 대사만 선택한다. 전체 LOCAL 고유44개·Voice28개를 실제 DialoguePresenter에서 재생한다. 시작/보고 대사도 Field07에서 제공 factory/UI를 호출한 통제 재생이므로 전체 Story playthrough로 표현하지 않는다.

감사 Fixture에서 슬롯 선택 누락으로 Save 실패가 한 번 있었다. 감사 도구에 SelectSlot(1)을 추가했다. 출구 근처에서 수동 Story Battle을 시작해 복귀 안전 이동 후 Field06으로 다시 넘어간 Fixture 실패도 있었다. 실제 목격 지점(-3,0)처럼 맵 안쪽으로 배치한 뒤 해당 Story 복귀부터 재개했다. 기존 게임 코드의 오류로 판정하지 않았으며 Runtime 코드를 수정하지 않았다. 앞서 통과한 Scene·Save·일반전 결과는 보존한다.

## 결과

최종 기록 **217 PASS / 0 FAIL**. 앞서 통과한 Scene/Save/일반전62개와 보정 후 Story 복귀/Voice155개 기록을 합쳤다. 중복 상태 체크가 포함되므로 서로 다른 기능217개를 뜻하지 않는다. Fixture 실패2건은 위 이력대로 수정했으며 게임 Runtime 오류 수정은 없다. [텍스트 로그](Main16_Audio_Runtime_Results.txt)·[요약 JSON](Main16_Audio_Runtime_Results.json)을 따른다.

44개 고유 Dialogue UI 재생 중 Voice28개는 새 BGM과 함께 자연 종료까지 재생했다. Hearing/Default 실제 Path 판별과 첫 발견자 순서, 화자/Clip 일치, Next/종료 정리를 확인했다. 추가 Source 관찰은20개 Clip에서 playing=true·Voice Group·gain1을 기록했다. 원본 MP3·기존 Voice/Appearance·사용자 파일 등 보호750개 해시 동일, 기존28 Mapping도 보존했다.

기본 Voice100/BGM80의 Mixer 값은0dB/-1.93820012dB, BGM Source1개다. 출력 peak는 동시0.8409187, BGM0/Voice 단독0.5678347, BGM 단독0.548252, Mute 전체0, 해제 동시0.7985017, SFX 단독0.01767767. SFX는 원본 변경 없는 임시 메모리 신호로 채널 독립성만 확인하고 제거했다. 채널 설정은100/100/80으로 유지한 뒤 Play 종료 시 사용자 실제 Settings를 복원했다.

같은 곡 요청 시 timeSamples가 초기화되지 않고, 끝1초 전으로 이동해 실제 Loop 순환을 확인했다. Loop 경계가 귀에 자연스러운지는 미검증이다. Unity 컴파일 오류0, 최종 Console Error0/Warning2(기존 ExternalAssetImportEditor의 deprecated spritesheet API 경고)다. clean Bootstrap Edit Mode와 격리 Save/Settings·Play 옵션·runInBackground 복원을 확인했다.

## 실제 청취

**실제 사람 귀 청취0/28**. 발음·감정·호흡·속도·문장 끝·Segment boundary·Text Audio 의미 일치·Speaker 음색·음량 취향·BGM masking·장면 적합성과 BGM Loop 경계·복귀 자연스러움은 모두 미검증이다. PCM 출력과 Mixer dB를 근거로 청취 PASS를 주장하지 않는다. [28개 ID/화자/파일/본문 체크리스트](Main16_Audio_청취_체크리스트.md)에 개별 미검증 상태를 기록했다. 문제 수0·재생성 필요0으로 확정하지 않으며 재생성 필요 목록은 미정이다. TTS 재생성 없음.
