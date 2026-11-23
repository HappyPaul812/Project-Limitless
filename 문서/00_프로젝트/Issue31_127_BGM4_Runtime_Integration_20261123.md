# Chapter 3 승인 BGM 4곡 Unity 연결 · 2026-11-23

- 브랜치 `codex/ch3-approved-bgm4-runtime-20261123`
- 제품 커밋 `c24f3bec02b0c4511032cfb26e7a7452efdaabea`
- 베이스 `736df561adb408c0225ed9052c37f2d45b7e24f5`. #126 승인 작업은 독립 브랜치 `b2580729ac6c57f22195bef83094b256e96dca60`에 유지했다.
- [공식 실행 지시](https://github.com/HappyPaul812/Project-Limitless/blob/48bd99d081625bd517e80ab14a3dea93a2131f6a/%EB%AC%B8%EC%84%9C/00_%ED%94%84%EB%A1%9C%EC%A0%9D%ED%8A%B8/20261123_Chapter3_Approved4BGM_Unity_Integration_Order.md) · [#31](https://github.com/HappyPaul812/Project-Limitless/issues/31) · [#127](https://github.com/HappyPaul812/Project-Limitless/issues/127)

| 구간 | 승인 곡 | 씬 배정 |
|---|---|---|
| F1 | Where the Grass Bends | `World_NorthF1` |
| 서릿길 마을 | Hearth in the Low Valley | `World_FrostwayVillage` |
| F2 | Beneath the Frozen Boughs | 빌드에서 활성화된 `World_NorthF2_CANDIDATE` |
| F3 | Steps on Frozen Stone | 카탈로그 등록 완료 · 실제 씬 배정 HOLD |

`BgmPlaybackService.cs`, `BgmSceneCatalog.cs`, `BgmSceneCatalog.asset`을 변경하고 `Resources/Audio/Music/Chapter3Approved/`에 승인 WAV 8개와 meta 8개, 폴더 meta를 추가했다. ZIP 안의 WAV 바이트를 추출해 사용했다. 이전 WAV 검사와 Import 결과는 재사용했다. Intro의 샘플 수를 기준으로 두 AudioSource를 동일한 DSP 시계에 예약한다. 씬 변경과 전투 전환에서 두 예약을 정리한다. Chapter 1·2의 단일곡 및 전투 선곡을 유지한다. Chapter 3 전투곡은 기존 미배정 상태를 유지하고, 탐색 씬 복귀 시 Intro를 다시 재생한다.

## Unity 6000.6.5f1 검증

**24 PASS / 0 FAIL.** 전환·회귀 20항목, 곡별 DSP 반복 4항목을 통과했다. 네 곡 각각 Intro 종료 후 Loop 지점에서 재생을 시작하고 세 차례 샘플 위치가 랩어라운드하는 동안 Intro가 다시 시작되지 않았다. 빠른 씬 이동, 리스너 일시정지, 전투 진입 시 예약 취소 및 복귀, 승인 대사 `main05_taeon_supp_003`과 BGM·SFX 동시재생, Mixer 채널별 수치와 Master 음소거, 기존 Chapter 1·2 단일곡과 Chapter 1 전투곡을 확인했다. 첫 회귀 실행에서 격리 프로젝트의 기존 MP3 참조 3개가 누락됐고, 이를 보완한 재실행에서 통과했다. 제품 코드 컴파일 오류는 없었으며 Unity Editor Search 초기화 예외가 두 격리 실행의 로그에 각각 남았다.

[전환·회귀 로그](Evidence/Chapter3Bgm4_Runtime_20261123/runtime.txt) · [4곡 실재생 반복 로그](Evidence/Chapter3Bgm4_Runtime_20261123/repeats.txt)

백그라운드 Unity Play에서 제품 BGM 서비스, 카탈로그, Mixer, 설정 서비스, VoicePlaybackSource를 사용했다. 전환 검증에는 실제 씬 ID로 만든 격리 테스트 씬과 테스트 전투 진입 호출을 사용했다. 제품 씬 전체, 전투 UI, Save Continue, 실제 장치 청취와 TTS 명료도는 미검증이다. 종료 시 두 소스를 정리했다. 개인 Save/UserData, Discord, 기존 음원, Mixer 파일, 씬, #126 브랜치를 보호했다. #85 HOLD 유지. 원본 main 변경·병합·Push 없음.

대표 직접 청취와 F3 정식 씬 배정이 남았다. 상용 배포 이용 권리 확인 상태는 `COMMERCIAL_USE_EXPLICIT_LICENSE_PENDING`이다.
