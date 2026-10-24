# TTS 162개 Unity 통합 QA — 구현 완료 / 병합·원본 동기화 승인 대기

2026-10-24 저녁 대표의 구현 승인을 적용했다. 작업 브랜치 `codex/tts-voice-integration-20261024`, 기준 main `47ad2eecb40135b2ee28e4947d642ebdbf0bf68a`, 확인한 docs/tts `84b0aec27ada334604b9bf8a75b86615cfd2ec1d` 및 [Issue20](https://github.com/HappyPaul812/Project-Limitless/issues/20).

작업 worktree: `F:\study\codex\Project-Limitless\Temp\TTSVoiceIntegration20261024\Worktree`.

제품 구현 commit: `1cb99be267f1653e5bd6e8d2e3fb06d783a75ba3`. main 직접 commit/merge·Push·사용자 원본 동기화는 **미실행/별도 승인 대기**다.

## 적용 결과

| 인물 | Voice | 적용 | 기존 교체 | 신규 |
|---|---|---:|---:|---:|
| 세린 companion_serin | Erinome | 67 | 58 | 9 |
| 태온 companion_taeon | Algieba | 85 | 82 | 3 |
| 하렌 story_haren | Alnilam | 3 | 0 | 3 |
| 폴 companion_paul | Achird | 3 | 0 | 3 |
| 미엘 companion_miel | Sulafat | 2 | 0 | 2 |
| 레온 arbel-leon | Orus | 2 | 0 | 2 |
| **합계** | | **162** | **140** | **22** |

실제 최신 Catalog 대조 결과 290→312항목이다. ID 중복0, Missing Clip0, 음성 GUID 충돌0. 비대상150개 Catalog 연결 동일. 기존140개 WAV만 승인 source 바이트로 교체하고 **기존 meta/GUID 모두 보존**했다. 원래 WAV/meta는 worktree `Temp/TTS162/originals`에도 백업했으며 사용자 다운로드 원본·원본 프로젝트·Git 이전 commit은 건드리지 않았다.

신규22개는 Unity가 생성한 GUID/meta로 연결했다. 새 AudioImporter는 기존 음성처럼 PCM/원래 샘플레이트/DecompressOnLoad를 사용하며 소스 WAV 바이트를 가공하지 않았다. 제품 변경은 WAV162·신규meta22·Main20/21폴더meta2·Catalog1·Editor 감사도구와meta2 = **Unity189파일**이다. [정확한 목록과 SHA](Evidence/TTS162Integration20261024/unity_changed_files.json).

세린/태온은 현재 등록된 모든 ID가 이번152개의 부분집합과 정확히 일치하여 Catalog에서 구음성 혼용0이다. 예전 비참조 후보/다른 화자의 자산을 삭제하지 않았다. 현재 Story 런타임은 중앙 DialoguePresenter→StoryVoiceCatalog로 조회하며 World 코드에 별도 AudioClip 직접 로드 경로는 검색되지 않았다. Main20/21 Manifest는 본문·화자·ID·VoiceExpected를 보존하고 실제 적용 상태만 갱신했다.

## 승인 원본·대사 매핑 근거

- 세린67/태온85: 지정 폴더의 WAV와152개 sidecar의 ID/화자/Voice/정확 본문·text SHA/WAV SHA 일치. source Manifest는 docs/tts의152개 정본을 그대로 보존했다.
- 하렌3: 화자 story_haren/Alnilam·정확 세 문장. 01은 “태온. 너 아직 멀었어.” 평서문 원문 그대로이며 승인 마커/정식 WAV/R2 경로 SHA 모두 **d6188257ac6d72a86336c89c211e6e3701dec6dedaa1d3516ffccd3db61cb3c2**. R2.1 사용자 승인 기록을 근거로 선택했다. 불합격/임시 파일은 스캔 편입하지 않았다. 02/03은 사용자 청취 합격 보고와 sidecar 일치로 연결했다.
- KEEP7: 지정된 기존 WAV 그대로 사용. 개별 JSON sidecar는 없으므로 Main21 정본 및 기존 attempts.jsonl의 DialogueId/TextSha256/VoiceId를 대조했다. 누락 sidecar를 만들어 원래 존재한 것처럼 처리하지 않았다.
- 전체162 PCM24kHz/mono/16bit, 디코딩 길이 일치·빈 음성0·총782.52초. `abs(sample)>=32760` 비율0.1% 초과 후보0. 이것은 기술 검증이며 실제 발음/연기 품질을 자동 보증하지 않는다.
- 실제 Unity 대사 생성기로135개 ID/화자/본문을 대조했다. 배열 getter 밖에 직접 구성되는27개는 **컴파일된 실제 C# constructor 네 인수 및 화자 상수**를 정확 파싱해 대조했다. Main03 화면용 “태온\n” prefix는 별도 기록하며 음성 본문·런타임을 수정하지 않았다. 접두사만으로 ID 존재를 추정하지 않았다.
- [162개 source/원문/기술 근거](../../Tools/TTS/tts162_integration_manifest.json), [최종 SHA/GUID 매핑](Evidence/TTS162Integration20261024/mapping_verified.json), [Unity 생성 대사](Evidence/TTS162Integration20261024/runtime_dialogues.json).

## Unity Compile·Play Mode

설치 Editor는 **6000.6.5f1**, main ProjectVersion은 **6000.5.7f1**이다. 제품 Packages/ProjectSettings는 그대로 유지했다. 별도 `Temp/TTS162Validation` 복제본에서만 설치 버전과 기존 로컬의 호환 manifest/lock을 사용했다. 최초 main 패키지 그대로의 검증에서는 2D Animation15.1의 SetChecked/IsChecked API 호환 오류로 중단됐다. 호환 복제본은 제품 C# Compile 오류0이며 최종 Unity 종료0이다. main 지정6000.5.7f1의 직접 검증은 설치본 부재로 미수행이다.

최종 숨김 batch Play Mode **1,031 PASS / 테스트 오류0**, 162/162 AudioSource.IsPlaying 관찰:

- 162 Clip 로드/PCM 디코딩·정확 Speaker guard·Presenter 매핑·자막·Hide 후 정리.
- 실제 Main20 보스 후 및 Main21 우회로/마차/하렌 형제 대사 배열의 페이지 진행과 종료.
- Next→Player 무음, 지문이 등록ID를 가졌어도 무음, 수동 완료 callback, Skip/Hide 음성 정리.
- Voice0/100 상태와 기본·청각 경로 Main21 음성 커버리지.
- 별도 Temp Save/Settings에서 실제 Save/Load·복원 후 Voice 참조 유지.
- Play Mode 프레임 진행 확인. 전체162개를 자연 종료까지 청취한 시험은 아니다. AudioListener 출력은0으로 두어 사용자 작업을 방해하지 않았다.

첫 Play Mode의1,027 검사는 통과했으나 테스트가 잘못 지정한 `Field_10` Scene ID 때문에 Save 단계에서 실패했다. 테스트 fixture를 유효한 World_StarterVillage/Spawn_From_Field01로 수정했고 게임 저장 코드는 변경하지 않았다. 신규22 무손실 importer 수정과 함께 재검증하여 최종1,031 PASS했다. [첫 실패 보존](Evidence/TTS162Integration20261024/runtime-first-fixture-fail.json), [최종 결과](Evidence/TTS162Integration20261024/runtime.json), [Unity 임포트 결과](Evidence/TTS162Integration20261024/applied.json).

최종 로그에는 **UnityEditor.Search.SearchDatabase 인덱스 ArgumentOutOfRangeException 1건**이 있다. TTS/게임 코드의 compile·runtime 테스트 오류는0이고 저장 오류도0이지만, Console 전체 오류0이라고 주장하지 않는다. [분류/로그 SHA/환경](Evidence/TTS162Integration20261024/unity_environment_and_console.json). 원본 로그는 worktree `Temp/TTS162Validation/integration-final.log`에 보존했다.

## 보호·미검증·승인 대기

- 원본 WAV/metadata·Save/UserData·기존 dirty 파일·기존 음성 참조를 포함한 보호1,430파일 SHA 동일, 원본 Git dirty 상태 동일. [보호 결과](Evidence/TTS162Integration20261024/original_protection.json). 원본 보호 목록은 원본repo `Temp/TTSVoiceIntegration20261024/protection_before.json`에 있다.
- 기존 main에는 비음성 투사체6개 meta가 `ea7264dba5af46cda070ad3e3aa3eac3`를 공유하는 결함이 이미 있다. 이번 음성 GUID와 겹치지 않으며 원본 비교로 기존 상태임을 확인했다. 해당 미승인 파일을 수정하지 않았고 전체 프로젝트 GUID 충돌0이라고 주장하지 않는다. 신규 GUID 충돌은0이다.
- Google API/TTS 재생성0, 키 파일 열람0, Quest/Battle/Scene/사용자 설정 변경0. 화면·키보드·마우스·Editor 포커스 전환0.
- 실제 전체 Quest/Battle/Scene 이동 주행, 독립적인162개 전량 청취, 설치되지 않은6000.5.7f1 검증은 남아 있다. 이번에는 실제 Story 대사 배열과 중앙 Presenter/저장 경계만 검사했다.
- 앱 관리 worktree 등록이 장시간 대기하여 저장소 Temp의 지정 브랜치 worktree를 사용했다. 뒤늦게 등록 완료된 C:\Users\windows\.codex\worktrees\tts-voice-integration-20261024\Project-Limitless는 미사용 원본 정본 체크아웃이며, 앱이 primary checkout으로 보호하여 정리 요청을 거부했다. 해당 경로를 수동 삭제하거나 우회하지 않고 보존했다. 실제 구현 위치는 상단 경로이며 원본 main 작업 폴더가 아니다.
- **main 반영과 원본 동기화는 각각 대표의 후속 승인 대상**. 동기화 전 원본의6000.6.5 환경 차이와 미커밋 작업을 재확인하고 승인된 음성189파일만 선별해야 한다. 제품 Packages/ProjectSettings 또는 기존 투사체 GUID를 이 음성 작업에 섞지 않는다.
