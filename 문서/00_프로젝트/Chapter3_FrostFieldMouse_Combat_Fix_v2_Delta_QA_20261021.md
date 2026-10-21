# 서리들쥐 전투 Fix v2 변경분 격리 검수 — 2026-10-21

바울 대표께 보고: **변경분 QA PASS. Attack PASS / Hit PASS / Defeat PASS**입니다. 이는 Codex의 표적 결함 검수 판정이며 **바울 대표 최종 미술 승인 대기 / 정식 적용 없음**입니다.

| 대상 | 미술 판정 | 변경분 확인 |
|---|---|---|
| Attack | PASS | 00이 승인 Idle00과 RGBA 전체 픽셀 동일. 01 준비→02 최대 전방 할퀴기→03 회수→Idle. 03은 승인 Idle03과 동일하며 경계 확대 점프 해소 |
| Hit | PASS | 반사→웅크림→회복→03. Hit03이 승인 Idle00과 RGBA 전체 픽셀 동일하여 Idle 복귀 확대 점프 해소 |
| Defeat | PASS | 단계적 쓰러짐과 최종 누운 자세 유지. 03 알파 마스크가 v1과 동일하며 실제 런타임 1.25초 이상 03 고정 |
| 공통 정체성·접지 | PASS | 동일 112px/128px 캡처에서 냉회백 털·갈색 음영·눈·앞니·귀·긴 꼬리가 이어짐. 자세 변화와 구별되는 뚜렷한 전체 크기 점프·바닥 미끄러짐은 관찰되지 않음 |

중간 자세의 앞발 이동·몸 낮아짐·꼬리 들림은 남아 있습니다. 알파 bbox 하단 일치만으로 발 고정을 판정하지 않고 각 프레임 실제 Unity 캡처를 같은 배율로 비교했습니다. 최종 색감·할퀴기 강약·동작 취향의 승인은 바울 대표에게 남깁니다.

## 근거·원본

- [Issue #30 최신 재개 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-6304671456), v1 미술 불합격 보고 및 docs/art-new 최신 v2 납품 보고(blob `45ea476de435c6e102fdf736a5b4912a0816296a`)를 원격 조회하여 로컬 문서와 대조했습니다. v2 보고 commit `3833a3262dd9966344c7ee9f946a896b37b39d6f`, 후임 인계 `ed439bd41bfba67f58c88a3c2fd45750a0e5c860`.
- 사용 ZIP: `Temp/EOD20261020/Archives/PendingQA/Limitless_Chapter3_FrostFieldMouse_Combat_3Strips_Fix_v2.zip`.
- ZIP SHA256 **`518d210a0e5b29c7214ad960c0baa3d23a88d13d4f789b5f7420982ce701bfe3`**, 10,784,249 bytes, 20파일. Downloads 검색·이동·재다운로드·ZIP 재생성 없음.
- 승인 Idle은 기존 격리 QA 보관 PNG를 읽고 SHA `5afebfa1f95ca5673162513fdea63b025546aa836550c0ceb17f0334659988f2` 확인. 승인 1포즈·Idle 원본 수정 없음.
- 검수 브랜치 `codex/chapter3-mouse-combat-v2-review-20261021`, 기준 SHA `e4e9891f6f01ea95cada9e63b1f09946d94d6041`. 원본 main을 이동하지 않고 별도 worktree의 문서·증거만 커밋합니다.

## ZIP·PNG 기술 PASS

ZIP 전체 CRC 정상, SHA256SUMS 19항목 일치. PNG3 모두 2048×512 RGBA / 512셀4개, 12셀 비어있지 않음·외곽 투명·각 모션4프레임 데이터 서로 다름. PNG SHA는 납품 보고와 일치합니다.

| PNG | SHA256 |
|---|---|
| Attack | `c83e35b5aebbce908b6025d5a071fafc463cfe301763f68806ecddb8bd4f576b` |
| Hit | `695ed7db1f794c3f71df1b058eb5e91f01de7ecb125369264f3624b36c9fd3c2` |
| Defeat | `3f6a52039b766c57a5367c7145e0234b68ac0a552940de1e88ffe8f2404f3395` |

Attack00=Hit03=Idle00의 alpha>16 bbox는 (22,124)–(490,490), 468×366입니다. 112px 셀에서 가시 크기는 약102.4×80.1px로 동일합니다. Defeat03은 v1과 알파512×512 전체 동일이며 RGB까지 같다는 의미는 아닙니다.

## 실제 격리 Unity·8 FPS

Unity **6000.6.5f1**의 기존 격리 Client `Temp/Field05Review20261019/Client`만 숨김 Batch Mode로 실행했습니다. 신규 `QA/MouseCombatV2` 복사본을 PPU512 / bottom-center / Point / 무압축으로 Import하고 메모리 전용 Definition에 연결했습니다. 기존 MonsterSpriteSheetAnimation을 그대로 사용했습니다. 원본 Editor 조작·Game View 활성화·OS 포커스 전환 없음.

**변경분 런타임 23 PASS / 0 FAIL. 기존 36 PASS 전체 재실행 없음.** 변경된 셀 관측·8 FPS 설정·Idle 복귀·Defeat03 유지·UI 루트 보존·신규 제품 오류에 한정했습니다. 실제 1280×720 QA Canvas에서 전투 Image rect136×112, preserveAspect, scale1(셀112px) 및 보조128px로 캡처했습니다. 정식 Battle Scene 등록이나 실제 피해 이벤트 검증은 수행하지 않았습니다.

- Attack 00→01→02→03→Idle, 호출부터 Idle 관측 약0.45677초.
- Hit 00→01→02→03→Idle, 약0.50287초.
- Defeat03 도달 후1.25초 이상 고정, Idle로 돌아가지 않음.
- 프레임 간격은 설정상125ms이며 관측 안정 구간은 대략117~139ms. 첫 Attack00 관측→01은 약69ms, Defeat00→01은 약86ms로 짧게 기록됐습니다. Editor Update/최초 관측 시점의 영향을 포함한 값이며 **정확한 모든 프레임125ms 실측 PASS로 확대해석하지 않습니다**. 이번 작업에서 공용 재생기 변경·타이밍 회귀검사 확대는 하지 않았습니다.
- 제공 APNG는 실제 Unity 캡처를 모션 셀125ms로 재구성한 검토 자료이며 실시간 녹화가 아닙니다. 실측은 FrameTrace.tsv를 따릅니다. Defeat 미리보기 반복은 검토 편의이며 런타임은03고정입니다.
- 컴파일 오류0·신규 제품 런타임 오류0. 기존 UnityEditor.Search 예외, SpeedTree shadergraph/URP 설정 진단 및 deprecated 경고는 로그에 남았습니다. 전체 Console 오류0이라고 보고하지 않습니다.

[실제 Unity 프레임 순서](Evidence/MouseCombatV2_20261021/UnitySequence112.png) · [Idle/v1/v2 전후 비교](Evidence/MouseCombatV2_20261021/BeforeAfter112.png) · [8 FPS 동작 비교 페이지](Evidence/MouseCombatV2_20261021/index.html) · [검사23건](Evidence/MouseCombatV2_20261021/Runtime/checks.txt) · [실측 시각](Evidence/MouseCombatV2_20261021/Runtime/FrameTrace.tsv) · [기술 검증](Evidence/MouseCombatV2_20261021/Validation.json)

## 보호·다음 단계

원본 Assets/_Project·UserData·ProjectSettings·Packages **3,559파일 SHA 변경0**, 원본 Git status 바이트 및 HEAD 동일. 원본 Editor는 전후 Bootstrap / Play OFF / Paused OFF / 비포커스 / compiling false. 승인 Idle·1포즈·기존 제품 아트·Save 보호. 제품 코드·Scene·MonsterDefinition·Animator 변경0, 아트 수정0, Push·정식 적용·main 병합0.

바울 대표께서 비교 이미지와 움직임을 최종 승인하신 뒤에만 별도 지시로 정식 적용을 진행할 수 있습니다. 현재 판정은 **DELTA_QA_PASS / USER_COMBAT_ART_APPROVAL_PENDING / NOT_UNITY_INTEGRATED**입니다.
