# Audio Volume Runtime QA — 2026-09-30

## 구현과 환경

정책 commit `f628e11`, 기능 commit `cd656e0`. Unity6000.5.7f1 실제 Play Mode 두 회차에서 검증했다. 기존 통합 설정 화면·Mixer·정식 BGM/SFX Source가 없어 Master→BGM/SFX/Voice Mixer와 공용 uGUI 설정 화면을 추가했다. Scene 파일은 변경하지 않았다.

설정 진입은 화면 오른쪽 위 `설정 · F10` 버튼, F10 또는 게임패드 Start다. Toggle과 Slider3개는 정수0–100·실시간 숫자를 표시한다. Tab/위아래 항목 이동, Slider 좌우 음량 조절, Enter/Confirm 선택, Esc/게임패드 B 닫기를 지원한다. 기존 InputSystemUIInputModule의 키보드·게임패드 Stick/Dpad/Submit 바인딩을 확인했다. 패널 모달 소유권과 월드 이동 잠금·해제, 생성 화면/Intro의 단축키 차단을 연결했다. 닫는 Esc 프레임도 Intro Skip으로 전달하지 않는다.

MasterVolume은 전체 Mute만 담당한다. Voice/SFX/BGMVolume은0에서-80dB, 나머지는 `20*log10(value/100)`이다. 기본100/100/80·Mute OFF. 노출 이름을 실제 Group의 Volume GUID와 대조해 Master=MasterVolume, BGM=BGMVolume, SFX=SFXVolume, Voice=VoiceVolume을 확인했다. Mixer Asset은 실제 연결된 Editor에서 생성·직렬화했다. Intro Source.volume은1이고 Voice Group으로 라우팅한다. 공용 볼륨을 Source에서 다시 곱하지 않는다.

`UserSettingsService`의 기존 JSON을 Version2로 확장했다. 시작 시 읽은 캐시를 Scene 간 공유하고, 변경은 기존 임시 파일 저장 방식으로 즉시 기록한다. Version1은 SkipOpeningIntro를 보존하고 신규 Audio 기본값을 적용한다. Mixer 최초 적용은 실행 순서가 빠른 Presenter.Start에서 수행한다. AudioService.Route는 향후 정식 BGM/SFX/Voice Source의 명시적인 Group 연결 진입점이다.

## 실제 출력

Intro의 실제 WAV와 메모리에서만 생성한 SFX440Hz/BGM220Hz 임시 신호를 사용했다. 각 Source를 정식 Group에 연결하고 대상 하나만 출력하며 AudioListener.GetOutputData의 RMS를 측정했다. QA용 신호는 Asset이나 게임 기능으로 저장하지 않았으며 검증 후 삭제했다. 비교는 무음 RMS<0.00005, 정상 RMS>0.00005로 판정했다. 다음9조건×3채널=27검사가 모두 통과했다.

| 설정(Voice/SFX/BGM) | Mute | Voice RMS | SFX RMS | BGM RMS |
| --- | --- | ---: | ---: | ---: |
| 100/100/80 | OFF | .11913 | .10033 | .080856 |
| 100/100/100 | OFF | .050906 | .10033 | .10053 |
| 0/100/100 | OFF | 0 | .10033 | .10053 |
| 100/100/0 | OFF | .21954 | .10033 | 0 |
| 100/0/100 | OFF | .12182 | 0 | .10053 |
| 63/47/72 | ON | 0 | 0 | 0 |
| 63/47/72 | OFF | .10034 | .047269 | .072719 |
| 50/47/72 | ON | 0 | 0 | 0 |
| 50/47/72 | OFF | .061563 | .047269 | .072718 |

Voice는 음성 파형의 서로 다른 구간을 측정하므로 RMS 크기를 직접 비교하지 않고 출력 유무와 채널 독립성을 판정했다.

## UI・保存・Intro

- 실제 Slider.value 변경으로63/47/72를 저장하고 숫자 Text도 같은 값을 표시했다. Mute ON에서도 값을 보존하고 Voice50 변경 후 OFF에서 새 값을 사용했다. Slider.OnMove의 Right 이벤트로50→51, 정수 단위와 Navigation의 위아래 연결을 확인했다.
- JSON 재Load로63/47/72·Mute OFF 복원. Mute ON도 저장·재Load 확인. Scene 이동에서도 유지. Play Mode 종료·재시작한 두 번째 회차에서63/47/72·Mute ON, Master-80dB/Voice-4.013189dB/SFX-6.558043dB/BGM-2.85335dB 확인. Presenter는 항상1개였다.
- Version1 `{Version:1, SkipOpeningIntro:true}`를 격리 파일로 검증해 Skip=true와 Audio100/100/80·Mute OFF로 이행했다. Version2의 Voice-20/SFX150은0/100으로 제한했다.
- 1016×569에서 Canvas 갱신 후 패널 네 모서리는(261.94,94)–(754.06,475)로 화면 안에 있다. 중앙 anchor·오른쪽 위 버튼 anchor와 ScaleWithScreenSize를 확인했다. 배치 수치 검증이며 Game View 시각 검사는 아니다.
- Intro를 선두부터 실행해18음성과 무음 제목의19블록 모두 Text/ID/Clip/Voice Group을 확인했다. 각 음성 재생 중 Next로 이동하고 마지막 Next 후 Bootstrap, Source0을 확인했다.
- 별도 새 캐릭터 시작에서 재생 중8회 연속 Next→opening_009, Skip 직후 Clip=null/IsPlaying=false, CharacterCreation에서 Source0. 설정63/47/72 유지. TTS WAV/meta18쌍과 기존 Catalog는 변경하지 않았다.

## 제약과 최종 상태

검증 시작 때 백그라운드에서 Player Loop가 멈춰 있었으므로 QA 동안만 Application.runInBackground를 켜고 종료 때 복원했다. OS/Unity/Game View 포커스 전환은 없다. 설정·Save는 UserData/AudioVolumeQA 안에 격리했다. 초기화 완료 전 QA 도구 호출 실패와 중간 블록부터 시작한 관찰은 다시 실행했고, 최종 판정은 초기화 후·선두 시작 결과만 사용했다.

실제 게임 BGM/SFX는 미구현이므로 임시 신호의 Group 출력을 검증했다. 실제 음악·효과음을 청취한 검증은 아니다. 물리 키보드/게임패드/마우스 입력, Game View 시각 QA, 청취 음량, 배포 Player의 저장, Intro 실발화·분할 경계는 미검증이다.

재컴파일 때 MCP WebSocket 초기화 Warning1건, 게임/Mixer Parameter Error0. 기록 후 Console을 정리하고 최종 재조회 Error0/Warning0을 확인했다. 기존 변경·사용자 저장·Intro 원본을 포함한109파일의 SHA-256이 시작 때와 같다. Bootstrap clean Edit Mode로 돌아오고 QA 설정/Save 경로와 Game View 진입 동작을 복원했다. QA 설정 JSON과 임시 신호를 삭제했다. 이번 차이 검사 통과, 기존 사용자 변경의 trailing whitespace는 수정하지 않았다. GitHub push 없음.
