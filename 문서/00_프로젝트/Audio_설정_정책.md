# LIMITLESS Audio 설정 정책

## 확정 설정과 출력

2026-09-30 사용자 요청으로 전체 음소거, 음성(Voice), 효과음(SFX), 배경음(BGM)을 정식 환경 설정으로 추가한다. UI는 각 채널을 정수 0–100으로 표시하며 실시간 적용한다. 기본값은 Voice100 / SFX100 / BGM80 / Mute OFF다.

정식 `LimitlessAudioMixer`는 Master 아래 BGM·SFX·Voice를 둔다. 노출 파라미터는 `MasterVolume`, `BGMVolume`, `SFXVolume`, `VoiceVolume`이다. 채널값은 `20 * log10(value / 100)`으로 변환하고 0은 -80dB로 처리한다. 전체 음소거는 Master만 -80dB로 바꾼다. 음소거 해제 시 Master는 0dB로 돌아오며 각 슬라이더 값은 보존한다. 음소거 중에도 채널값 변경을 허용한다.

Narration/Character Dialogue/Intro TTS는 Voice, 검격·폭발·UI·환경 효과는 SFX, Intro/Field/Battle/Dungeon/Town 음악은 BGM이다. 새 AudioSource는 해당 Mixer Group을 명시적으로 배정한다. 공용 채널 볼륨을 Source.volume에 다시 곱하지 않는다. Source의 고유 연출 gain은 별도이며 Intro Voice Source는 1로 유지한다. 기존 AudioClip·WAV·Import 설정은 변경하지 않는다.

## LOCAL 조사와 구현 계획

현재 `_Project`에는 Intro Voice AudioSource와 WAV18개만 존재한다. Scene/Prefab AudioSource, BGM/SFX 재생 코드·음원·AudioMixer, 통합 Settings/Options 화면은 없다. 기존 `UserSettingsService`는 슬롯과 독립된 JSON에 SkipOpeningIntro를 저장한다. Intro의 AudioListener 초기화는 계속 사용하며 전체 Mute는 Listener.volume 대신 Master Mixer에 적용한다.

- 기존 사용자 설정 JSON을 Version2로 확장한다. Version1의 SkipOpeningIntro는 보존하고 새 Audio값은 기본값으로 이행한다. 잘못된 범위는 0–100으로 제한한다. 저장 실패는 메모리 설정을 유지하고 진단한다.
- Editor는 `UserData/Settings/user_settings.json`, Player는 `persistentDataPath/Settings/user_settings.json`을 재사용한다. Save Slot과 분리하며 채널 조작 즉시 기존 임시 파일 저장 방식으로 기록한다.
- 공용 서비스가 시작 시 한 번 Load하고 Mixer를 적용한다. Scene 전환은 설정을 초기화하지 않는다. 모든 Scene에서 사용할 설정 진입 버튼과 공용 uGUI Audio 패널을 하나만 유지한다.
- 전체 음소거 Toggle, 채널 Slider3개와 숫자, 닫기 버튼. Unity Selectable Navigation과 좌우 입력을 유지하고 Tab 이동도 지원한다. 설정 중 월드 이동/상호작용을 기존 모달 소유권으로 차단한다. Intro Enter/Space/Esc가 설정 조작과 동시에 진행/Skip되지 않게 한다.
- BGM/SFX의 실제 게임 음원은 현재 없으므로 새 음악·효과음을 임의 추가하지 않는다. Group 연결용 공용 라우팅 API를 제공하고 임시 메모리 테스트 신호로 각 채널의 독립 출력만 검증한다.

## 검증 계획

백그라운드 Unity 컴파일·Mixer 파라미터/Group·실제 Intro Voice와 임시 BGM/SFX 출력·0/100·Mute ON/OFF 및 음소거 중 변경·63/47/72 저장/Scene 유지/재Load·Version1 이행·UI 이벤트/Navigation·Intro Next/Skip/전환 정리/18개 참조를 확인한다. 테스트 설정·Save는 격리하며 사용자 파일과 TTS 원본을 보존한다. 실제 청취·Game View 시각 QA와 물리 입력은 별도 검증이다.

## 적용 결과

정책 commit `f628e11`, 구현 commit `cd656e0`. Mixer·Voice 라우팅·공용 설정 UI·Version2 사용자 JSON 저장을 구현했다. 실제 백그라운드 Play Mode의27개 독립 출력 검사, UI 값/이벤트·Mute 값 보존, Scene 유지·재Load·Play 재시작 복원, 구버전 이행, Intro 회귀를 통과했다. 실제 게임 BGM/SFX는 아직 없으며 해당 Group은 임시 메모리 신호로 검증했다. 세부 결과와 미검증은 [Runtime QA](Audio_Volume_Runtime_QA_2026_09_30.md)를 따른다.
