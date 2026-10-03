# Main16 Audio 마무리 계약 — 2026-10-03

LOCAL 프로젝트와 기존 Main16 구현·Voice28/28·Runtime428 PASS 결과를 기준으로 한다. 순서는 문서화 → BGM 구현 → 범위 내 Foreground Audio 재생 검증 → 실제 오류 최소 수정 → 재검증이다. Main16 Quest 전체 감사와 Appearance 판정을 반복하지 않는다. 기존 사용자 변경과 원본 음악·WAV를 보존하며 GitHub Push는 하지 않는다.

## 정식 배정과 후보

| 곡 | 정식 역할 / 상태 |
| --- | --- |
| Where the Earth Breathes | Field_07_AshenReach 재바람 황야 기본 탐색·조사·전투 후 잔향·협곡 입구 발견 |
| Trail of the Ember Wraith | 최초 형성/목격 연출 후보. Runtime 미적용 |
| Beneath The Cracked Earth | Main17·붉은 균열 협곡 이후 후보. Runtime 미적용 |
| Steel and Sunlight | 기존 Chapter1 일반전. Field07 적용 여부는 아래 LOCAL 충돌 확인을 따른다 |
| Stone Without Memory | Dungeon B1 |
| Beneath The Forgotten Hall | Dungeon B2 |
| The Warden’s Final Stand | Silent Warden |
| Beneath the Ashen Sun | Chapter2 첫 진입·지역 소개 Hook |
| Paths of Cracked Earth | 기존 Chapter2 일반 서쪽 Field04~06 |
| The Weight of Obsidian | 깊은 화염/흑요석 지역. 최초 Scene 미확정 상태 유지 |

기존 후보 Blade and Gambit·Beneath the Sunken Hall·Iron and Incantation·The Weight of Crowns도 Runtime 미배정으로 유지한다. Where the Earth Breathes를 Chapter2 전체 Field에 확대하지 않는다. 불씨망령 전투 후 같은 탐색곡으로 돌아와 현상이 끝나지 않았음을 표현한다.

## 입력과 구현

Downloads에서 세 제목 모두 단일 파일을 확인했다. 모두 MP3 / 44.1kHz / stereo / 192kbps다.

| 실제 파일 | 길이(초) | 바이트 |
| --- | ---: | ---: |
| F:/Downloads/Where_the_Earth_Breathes.mp3 | 180.166458 | 4330073 |
| F:/Downloads/Trail_of_the_Ember_Wraith.mp3 | 179.983625 | 4325685 |
| F:/Downloads/Beneath_The_Cracked_Earth.mp3 | 176.483208 | 4241675 |

정식곡만 기존 `Assets/_Project/Audio/Music/` Convention에 원본 바이트 그대로 Import한다. 후보는 Downloads 보관 정책을 유지한다. Streaming·원본 sample rate·forceToMono0·normalize0, 기존 단일 BgmPlaybackService와 BGM Mixer·Source gain1/pitch1/Loop를 사용한다. 새 Framework·Ducking·전역 음량 변경·MP3 편집은 없다. Catalog에 Field07 배정만 추가하고 Scene 자체는 저장하지 않는다.

## LOCAL 정책 충돌 확인

요청서는 일반 Battle의 Steel and Sunlight를 언급하지만 현재 `BgmSceneCatalog.FindBattle`은 Chapter1만 배정하며 Field07을 포함한 Chapter2는 null/TBD다. 처음 받은 Field07 전투 적용 응답은 사용자가 잘못 승인했다고 철회했다. 재확인 답변은 **기존 유지: Field07 전투는 null/TBD**다. 일반전·불씨망령 Story Battle 모두 기존 정책을 유지하며 FindBattle 코드는 수정하지 않는다. Field07에서 Battle 진입 시 탐색곡을 정리하고, 복귀하면 Where the Earth Breathes를 재생한다.

## 검증과 청취 한계

Field06↔07·일반/Story Battle 진입과 Field 복귀·Defeat→Arbel·Field07 Save→Continue에서 실제 Scene/Context가 곡을 결정하는지 확인한다. Title·Arbel·Field06 음악, Source1개·Loop·Mixer Group·BGM0에서 Voice 독립·Mute/복원을 최소 범위로 검증한다. 기존 Main16 Mapping은 재생성하지 않는다. Voice100/BGM80에서 기존 Story Dialogue Context의28 Clip을 가능한 범위에서 재생한다. Hearing/Default는 실제 Path와 해당 factory ID를 사용하며 Player 무음을 유지한다.

사용자가 Main16 Audio 범위 Foreground/Game View를 승인했다. 그러나 현재 도구는 오디오를 직접 인지하지 못한다. 실제 재생·출력·화자/Clip 참조와 사람 귀의 발음·감정·호흡·속도·Loop 경계·Voice Masking 판정을 구분한다. 청취를 수행했다고 주장하거나 문제0·재생성 필요0으로 확정하지 않는다. 청취 품질과 TTS 재생성 필요 목록은 미검증/미정으로 기록한다.
