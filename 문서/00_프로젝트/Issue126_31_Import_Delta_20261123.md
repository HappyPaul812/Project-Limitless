# #126·#31 Unity 격리 기술 검수 — 2026-11-23

Unity **6000.6.5f1 (3ff58d469c8a)**, 숨김 BatchMode, 소형 격리 프로젝트. **12 PASS / 0 FAIL**(Sprite 3, WAV 채널·샘플/설정 8, Master 수량 1). 제품 Unity 실행·변경 0, #85 HOLD. [Unity Delta 로그](Evidence/Issue126_31_20261123/delta.txt)

## #126 빙각 수호체 KeyPose

[바울 대표 외형·1포즈 승인](https://github.com/HappyPaul812/Project-Limitless/issues/126#issuecomment-7088805208). 지정 ZIP 내부 512×512 RGBA 원본 SHA256 `c34ea53a95aa6a1fdba4f5e355715fbc25797d6d51e4d544cb9e4dab52f7073c` 일치. ZIP CRC/SUMS 기존 증거 재사용. 알파 바운딩 박스 `[31,481) × [65,490)`, 캔버스 가장자리 알파 0, 최하단 여백 22px. 외곽이 잘리지 않았고 두 발이 지면 기준점 후보로 읽힙니다. 128px에서 주먹·어깨·발 분리가 뚜렷하며 40px에서도 두 주먹과 어깨 윤곽이 남고 내부 빙각 세부는 약해집니다. 전달된 [축소판](Evidence/Issue126_31_20261123/RimeSentinel_512_128_40_ActualReadability_OFFLINE.png), [M4·M5 동배율판](Evidence/Issue126_31_20261123/RimeSentinel_vs_M4_M5_Actual512Canvas_OFFLINE.png)의 미술 승인 대기 표기는 당시 제작 상태이며 대표가 이후 1포즈를 승인했습니다.

| 비교 항목 | 얼음갑충 M4 | 눈보라 정령 M5 | 빙각 수호체 M6 |
|---|---|---|---|
| Sprite PPU/Pivot | Recovery 원본. 격리 시험 512/중앙 | 제품 KeyPose meta 512/중앙, 격리에서도 동일 | 격리 시험 512/중앙, Point, 1×1 WU 캔버스 |
| 필드 Sorting | 공식 SpriteRenderer 미확인 | 기존 시각 Prefab Default/order 0/scale 1. 공식 조우 미연결 | 공식 배치·정렬 미확정. 발 기준점의 렌더 정렬은 필요 시 후속 승인 |
| 전투 표시 | 정식 MonsterDefinition/실측 배율 없음 | 정식 MonsterDefinition/실측 배율 없음 | 정식 배율 미확정. 일반 적 UI 136×112 기준, 가로세로비 유지 시 정사각형 캔버스 약 112×112, 실루엣 약 98×93 UI 단위 |

전투는 SpriteRenderer order/PPU가 아닌 Canvas Image이며, 기존 코드에서 SpriteSheet 사용 시에만 `MonsterDefinition.VisualScale`을 곱합니다. 위 값은 동일 기본값에서 비교한 계산값입니다. 격리 PPU/Pivot **PASS**, 캔버스 알파·축소 판독 **PASS**, 제품 필드 Sorting·접지·전투 GameView **NOT_VERIFIED**. 제품에서 F5 공식 Scene을 찾지 못해 F5 실제 배치·접지·겹침·전열 타깃 표시를 검증하지 못했습니다. 최종 KeyPose 승인 확보. 본편 적용과 실제 배율·Pivot/Sorting 결정, 후속 모션·MonsterDefinition/조우/단회 Save는 별도입니다.

## #31 Final Loop Masters

[기존 납품](https://github.com/HappyPaul812/Project-Limitless/issues/31#issuecomment-7088342969) 및 [CRC·SHA 교차검사](https://github.com/HappyPaul812/Project-Limitless/issues/31#issuecomment-7088807352) 재사용. ZIP `3c9810962e1a1db11117d5a3fe6ee8ce5caa4fcc62cc6889656aff7ac7d39c94`. 기존 파일 CRC/SHA·클릭·무음 검사를 반복하지 않았습니다. Unity 신규 Import 기본값은 네 파일 모두 DecompressOnLoad/Vorbis/Preserve/preload off입니다. 제품 기존 스트리밍 음악 설정은 보존합니다.

| Master | Intro Unity 샘플 / 시간 | Loop Unity 샘플 / 시간 |
|---|---:|---:|
| Where the Grass Bends | 668,115 / 15.15s | 2,335,095 / 52.95s |
| Hearth in the Low Valley | 216,090 / 4.90s | 2,465,190 / 55.90s |

네 파일 모두 격리 시험 **PCM, DecompressOnLoad, Preserve Sample Rate, Stereo 44.1kHz, Preload on, Force Mono/Background Load/Ambisonic off**로 임포트·설정 왕복 PASS. 제품 적용 전 실제 메모리/전환 비용을 측정하고 플랫폼별 오디오 설정을 확인합니다. 소스 PCM16 Master는 변경하지 않았습니다.

### 연결 가능 조건과 실제 검증 계획

- 현재 `BgmSceneCatalog.Entry`는 Scene당 AudioClip 하나, `BgmPlaybackService`는 지속 AudioSource 하나에서 loop=true로 재생합니다. 기존 서부 Intro는 외부 종료 호출로 탐색곡을 재생하는 기능입니다. `World_NorthF1`과 `World_FrostwayVillage`에는 현 카탈로그 배정이 없습니다. 현 구성으로 두 클립의 샘플 경계 연결을 할 수 없습니다.
- 승인 후 두 Scene의 쌍을 별도 데이터로 배정하는 안을 검토합니다. Intro는 한 번, Loop는 두 번째 BGM Source에 loop=true, 두 Source 모두 동일 BGM Mixer 그룹에 연결합니다. Intro 시작 DSP 시각과 실제 `samples / frequency`로 Loop 시작을 예약하고, 이미 마스터에 반영된 0.55s 처리를 런타임에서 추가하지 않습니다. 다른 Scene·전투·전환 취소에서 두 예약을 모두 정리하고 재진입 시 Intro 재생 정책을 확정합니다. 기존 단일곡 경로를 유지합니다. [Unity PlayScheduled](https://docs.unity.com/en-us/engine/6000.0/script-reference/unityengine/audiosource/playscheduled)
- **루프 실제 출력**. 두 곡 각각 Intro→Loop 1회·Loop→Loop 3회 이상을 오디오 시간으로 캡처해 공백·클릭·박자 미끄러짐·의도치 않은 Intro 재생을 확인합니다. 프레임 지연과 실제 DSP 출력 버퍼 조건, 기본/빠른 Scene 이탈, 전투 진입·복귀, pause/resume을 추가합니다.
- **TTS·Mixer**. 승인된 테스트 음성으로 Voice 대사와 BGM을 함께 재생해 문장 명료도와 클리핑을 확인합니다. 두 BGM Source의 BGM 라우팅, VoicePlaybackSource의 Voice 라우팅, BGM/Voice/Master 독립 조절과 mute, Listener 중복 0을 점검합니다. 제품 Mixer에서 자동 ducking 설정은 확인되지 않았으므로 우선 기존 믹스를 기준으로 하고, 필요성이 확인되면 별도 승인 범위로 제안합니다.
- **상태** Unity Audio Import PASS, 실제 DSP 재생·Mixer/TTS·전환 **NOT_VERIFIED**. 제품 Scene 배정, 스케줄러 변경, 본편 반영 별도 승인.

원본 main, 승인 자산, Save/UserData·Discord 보존. 브랜치 `codex/issue126-31-import-delta-20261123`, 실제 보고 SHA는 각 Issue에 기록합니다.
