# #127·#126·#124 통합 기술 검수 — 2026-11-23

Unity **6000.6.5f1 (3ff58d469c8a)** 소형 숨김 배치. BGM/Attack4 신규 Delta, 리뉴얼 두 후보 오프라인 픽셀 대조. 제품 변경·main 병합·Push 0, #85 HOLD. **Unity 신규 검사 9 PASS / 0 FAIL**(WAV 4, Attack 타임라인 샘플 5).

## #127 F2/F3 BGM

**납품 무결성 PASS / Unity Import PASS / 게임 적용 HOLD.** 지정 ZIP SHA-256 `0d0a3a098440edd9f8805cff02891db67530561d63b715bf4d46288d9fe32c2b`, CRC PASS, 내부 SHA256SUMS 6/6 PASS. [승인 B LoopReview](https://github.com/HappyPaul812/Project-Limitless/issues/127#issuecomment-7089411448)와 [Master 납품](https://github.com/HappyPaul812/Project-Limitless/issues/127#issuecomment-7089452467)의 정책·구간을 스냅샷으로 사용했습니다. 이전 리뷰 MP3 디코딩, 음원 클릭/무음 분석, 기존 #31 Unity QA는 반복하지 않았습니다.

| 곡 | 승인 논리 시작–끝 / 크로스페이드 | Intro Unity 샘플 / 초 | Loop Unity 샘플 / 초 |
|---|---|---|---|
| F2 Beneath the Frozen Boughs | 13.560–62.775s / 0.55s | 622,271 / 14.110454 | 2,146,113 / 48.664694 |
| F3 Steps on Frozen Stone | 24.102–81.107s / 0.55s | 1,087,167 / 24.652313 | 2,489,665 / 56.454989 |

WAV PCM16 / 44.1kHz / Stereo. Intro 길이는 논리 시작+0.55s, Loop 길이는 논리 끝에서 Intro 길이를 뺀 값이며, 실물 샘플과 차이는 1샘플 미만입니다. 크로스페이드가 파일에 반영되어 있어 Intro 한 번, 즉시 Loop 시작, Loop만 반복하는 구조입니다. 이 검수의 일관성 범위는 승인 구간·크로스페이드·파일 구조입니다. 기존 청취 승인은 재사용했습니다. [파일 검증](Evidence/Issue127_126_124_20261123/audio-check.txt) · [Unity Import](Evidence/Issue127_126_124_20261123/unity-audio.txt)

Unity 격리 설정은 PCM / DecompressOnLoad / PreserveSampleRate / Preload on / Stereo 유지입니다. 기존 단일 Source/단일 Clip 서비스에는 두 클립의 DSP 예약, F2/F3 배정, 취소/전투 복귀 및 BGM·Voice Mixer 라우팅의 구현·검증이 필요합니다. F3 정식 Scene 미확인. 실제 Intro→Loop 및 Loop→Loop 연속 출력, 재진입, TTS 가독성·클리핑은 미검증. 승인된 마스터는 구현 입력으로 사용할 수 있습니다. 제품 연결 승인과 상용 출시 전 권리 확인(`COMMERCIAL_USE_EXPLICIT_LICENSE_PENDING`)은 별도입니다.

## #126 빙각 수호체 Attack4

**미술 방향 PASS / 40px 타격 분리 PARTIAL / 격리 Animator PASS / 실제 게임 공격 연동 및 Hit·Defeat HOLD.** [승인 Idle4](https://github.com/HappyPaul812/Project-Limitless/issues/126#issuecomment-7089455631) 및 [Attack4 납품](https://github.com/HappyPaul812/Project-Limitless/issues/126#issuecomment-7089592511) 기준. 기존 Idle4 재생 QA는 재실행하지 않았습니다.

- 공격 준비→짧은 주먹 전진→타격 정점→회복. 승인 KeyPose·Idle4 첫 자세와 외형·크기 계열 유지. 네 프레임 하단 y424–511 픽셀 일치, 회복 프레임 전체 KeyPose 일치. 알파 기준 중심 이동 폭 원본 약 10.59×2.40px, 112px 기준 약 2.32×0.53px이며 접지는 고정됩니다. 몸통과 반대쪽 주먹을 유지한 단일 주먹 동작입니다. 112px에서 밝은 어깨·앞주먹 변화가 보이고, 40px에서 덩치와 어깨는 유지되나 전진과 타격 정점이 비슷하게 읽힙니다. [픽셀 근거](Evidence/Issue127_126_124_20261123/attack-geometry.txt)
- Unity Play Mode의 실제 SpriteRenderer Animator 비루프 클립(512 PPU·Point·중앙 Pivot, 170/120/150/220ms, 총 660ms)을 직접 표본 시각에 샘플링했습니다. 0.05/0.20/0.34/0.55초에서 해당 프레임, 0.80초에서 마지막 자세 유지 **5 PASS**. 112/40px 카메라 캡처이며 실제 F5/제품 전투가 아닙니다. [Animator 샘플](Evidence/Issue127_126_124_20261123/unity-attack.txt) · [Unity 실제 프레임](Evidence/Issue127_126_124_20261123/Attack4_Unity_112_40.png) · [캡처로 재구성한 GIF](Evidence/Issue127_126_124_20261123/Attack4_Unity_660ms.gif)
- 실제 제품 전투 플레이어는 공통 FPS로 균등 유지하고 근접 Presenter가 타격과 Idle 복귀를 처리합니다. 납품 타이밍을 그대로 사용하려면 공격만 비균등 유지하고, 타격 구간 **290–440ms**에 실제 피해/VFX를 별도 승인·확인합니다. 440ms 회복 시작 및 660ms 종료, 260ms Idle 복귀, Presenter 이동의 중복 여부를 확인해야 합니다. 이 Animator 클립만으로 피해/조우를 적용할 수 없습니다. **대표 Attack4 미술 승인 확인 전 Hit/Defeat HOLD**. 작은 화면 타격력을 더 강조하고 싶을 경우 첫 후보 비교에 근거해 타격 프레임의 앞주먹 명암/겹침만 국소 조정 범위로 제안합니다.

## #124 Vision Trait v2 및 GrassSlimeGel

두 후보는 오프라인. Unity Import·GameView·본편 교체 0. [납품 파일 및 기존 기술 QA](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7089737097) 재사용.

- **Vision v2: 40px PASS / 30px 개선 PASS / 26·24px PARTIAL / 승인 전 양산 HOLD.** 로컬 eye-target, 공식 Vision Symbol, v1과 동일 배율·어두운 배경으로 비교했습니다. 30px에서 약해진 바깥 청색 호와 어두운 동공이 흰 눈 외곽을 더 잘 드러냅니다. 26/24px에서는 개선 폭이 작아 조준점·4방 마름모가 여전히 강합니다. 금색 공식 문장과 역할 구분은 유지되며 eye-target와 공유하는 눈·조준 모티프는 남습니다. 30px 개선을 근거로 **대표 미술 승인 후보로 제출 가능**. 24/26px에서 눈 우선 판독까지 승인 기준이면 국소 보완 또는 실제 UI 검증 후 결정합니다. [실물 비교판](Evidence/Issue127_126_124_20261123/Vision_v2_Actual_Pixels_OFFLINE.png)
- **GrassSlimeGel: 소재 판독 PASS / 스타일·틴트 PARTIAL / 승인 전 양산 HOLD.** 현행 `dripping-goo`에 실제 ItemDefinition 틴트(0.55,1,0.38)를 재현했습니다. 새 젤은 128/40/30px에서 얼굴 없는 둥근 젤 덩어리로 일관되게 읽히며 30px 내부 방울은 약해집니다. 현행 흰 실루엣보다 소재의 투명감이 풍부하고, Ch2 샘플과 굵은 윤곽·분명한 단일 실루엣을 공유합니다. 새 후보가 더 매끈하고 둥글며 기존 초록 틴트를 곱하면 명암·색이 눌립니다. **원본 색(흰 틴트)으로 대표 미술 승인 후보 제출 가능**. 실제 적용의 tint 교체는 별도 승인입니다. [실물 비교판](Evidence/Issue127_126_124_20261123/GrassSlimeGel_Actual_Pixels_OFFLINE.png)
- 다른 길 특성 4종 및 남은 외부 전리품 양산은 각각 파일럿 최종 승인과 범위 지정 후 진행합니다. 후보 및 기존 PNG·GUID 원본 유지.

로컬 브랜치 `codex/issue127-126-124-delivery-delta-20261123`. 실제 검수 SHA는 각 Issue 댓글 참조. Unity 버전, #85, 승인 자산, main 및 Save/UserData·Discord 보존.
