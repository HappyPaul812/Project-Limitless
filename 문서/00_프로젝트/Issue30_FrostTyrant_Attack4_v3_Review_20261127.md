# Issue #30 설빙의 폭군 Attack4 v3 표적 검수

2026-11-27 · [v2 PARTIAL 기준](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-7206929072) · [v3 납품](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-7207146992)

**파일·알파 정리·Idle 진입·체급/접지/타격 방향·격리 배율 PASS. 02→03→Idle00 회복 PARTIAL. 미술 승인·본편 반영 HOLD.**

| 항목 | 관찰 |
| --- | --- |
| 밝은 매트 | 동일 밝고 어두운 배경 및 Unity에서 v2의 넓은 회백색 띠가 뿔 사이·발톱 틈·하체 아래에서 사라졌습니다. 주요 뿔·발톱 끝은 읽힙니다. 일부 가는 서리 모서리가 얇아졌지만 표시 배율에서 큰 형상 손실은 보이지 않습니다. |
| Idle03→Attack00 | Idle의 중앙 지지 발과 아래로 늘어진 앞발톱 윤곽이 이어집니다. v2에서 왼쪽 아래에 갑자기 나타났던 넓은 발 점프가 크게 줄었습니다. 01에서 자세를 넓히는 변화는 준비 동작으로 읽힙니다. |
| Attack02→03 | 03에 중간 회수 자세를 주려 한 변화는 보이지만 02의 화면 오른쪽까지 뻗은 발톱이 한 프레임 만에 왼쪽 몸통 쪽으로 크게 수축합니다. 되감기 완화는 PARTIAL입니다. |
| Attack03→Idle00 | 03의 지지 발이 Idle 위치에 가까워졌습니다. 03의 끌어올린 앞팔이 Idle00에서 다시 왼쪽 아래로 내려오고 앞발톱 아래 가는 잔상이 바뀌어 복귀 단차가 남습니다. |
| 체급·접지·타격 | v3 알파 높이 443/475/428/443px, 셀 하단 좌표 y489–490. 02는 v2와 같은 좌표의 양쪽 가시 픽셀 116,984개에서 RGB가 일치하고 신규 가시 픽셀 0개입니다. 오른쪽 타격 방향 및 체급 유지. |
| 셀 경계 | 외곽 alpha 0, 프레임 경계 잘림 없음. 최소 여백 좌 13/우 13/상 15/하 22px. |

ZIP 실물 SHA256 `07de830f1e299c7545121d5669d19e55cf2a9db028ede02bc6d491530ef7ba32`, 시트 `41bd76bf537e053cadc790939643259ba162d4bccb5653847fb47a770f42f132`로 납품 및 Manifest와 일치. CRC 18/18, SUMS 17/17, 2048×512 RGBA 시트와 512×512 개별 셀 4/4 픽셀 일치. KeyPose·Idle4·v2 시트의 동봉 참조 3/3은 직전 검수 ZIP의 원본과 바이트 일치. 공격 단독 및 Idle→Attack→Idle APNG는 4/12프레임, 각 125ms로 일치했습니다.

**Unity PASS.** 6000.6.5f1, 이전 검수와 같은 격리 1280×720 SpriteRenderer에서 PPU512, 하단 중앙 Pivot (256,0), Scale3·336px 셀로 승인 Idle 및 v3 총 8장 실제 렌더, 종료 0. v2는 직전 Unity 캡처를 재사용했습니다. 연속 비교는 캡처를 125ms로 엮은 프리뷰입니다. 본편 보스 씬, Animator, 타격 판정은 미검증입니다.

| 로컬 증거 `Temp/i30-attack-v3-qa/` | SHA256 |
| --- | --- |
| `Unity_v2_v3_336px.png` | `d2763543b7314cc1db2fa5a6e9514403ffb98d9b549be776608da1cb3c51c14d` |
| `Unity_v2_v3_Idle_Attack_Idle.png` | `a0185be1d56dc814ba1cc8229d35f2eb0da0fe3a25378a76a983ed459d30b211` |
| `Targeted_336px_Dark.png` | `a8d98169579e083d7397f1f169345d86121b64739f17a6240b4e8595ff8f40f8` |
| `Targeted_336px_Light.png` | `5d07f122f66a0297e0c7ea4d3490401c944e1c69c04b22c48067dd64700ba41b` |

격리 스크립트 원복. 승인 KeyPose·Idle4와 #85/#131/#132, 승인 자산·Save/UserData·원본 main 보호. Hit/Defeat 제작, 본편 적용, main 병합, Push 없음.
