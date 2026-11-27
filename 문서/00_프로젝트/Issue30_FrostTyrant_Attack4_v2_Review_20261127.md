# Issue #30 설빙의 폭군 Attack4 v2 표적 검수

2026-11-27 · [리뉴얼 납품](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-7206843285) · [기획 검수 지시](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-7206872251)

**파일·Unity 배율 PASS / 애니메이션 PARTIAL / 미술 승인·본편 반영 HOLD.**

- ZIP 실물 `F:/Downloads/LIMITLESS_Issue30_FrostTyrant_Attack4_v2_CANDIDATE_20261127.zip` SHA256 `f2b69aa8739b4d7dc1d9e61df9b51fc972cf4895cd6d12a549a7d38d6ad1a3d1`. CRC 16/16, SHA256SUMS 15/15, Manifest의 시트 SHA·프레임 규격·경계값과 실제 파일 대조 PASS. Manifest에 기록된 편집 전 생성 소스는 ZIP에 없어 그 원천 SHA는 미검증입니다.
- 2048×512 RGBA 시트 SHA256 `addd5627ed7c854a9a95b95b66845b836a466da1cff4972e007e53c426fd95e9`. 512×512 투명 RGBA 개별 프레임과 셀 픽셀 4/4 일치. 승인 KeyPose `f1a3c5f91a5fef69cd881e27b578c32253d0124d5d8628a8ca748f7479333830`, Idle4 `9d9b8d05be1813c013a5d784d749ce2d8ef751541ac36ce90712f3255cb53f43`는 승인 기록과 일치합니다. APNG 12프레임은 Idle 4 → Attack 4 → Idle 4 원화 픽셀과 매칭되며 각 125ms입니다.
- **체급·접지.** 동일 배율에서 Attack 높이 443/475/428/442px, Idle 465–466px. 타격 프레임은 낮아지고 폭 487px로 벌어져 전진이 읽힙니다. 알파 하단은 y489–490(Idle y490)로 바닥 높이가 유지됩니다. Idle의 좁은 하체에서 Attack00/03의 드러난 두 발로 바뀌고 Attack02의 지지 다리 위치가 크게 달라져 진입·복귀가 튑니다.
- **외형·타격.** 뿔과 흉곽, 긴 팔·큰 발톱의 정체성은 남습니다. 01의 들어올린 발톱과 02의 오른쪽으로 내지르는 발톱 덕분에 준비→타격은 명확합니다. 02→03에서 앞팔이 크게 되감기고 03→Idle00에서 하체와 발톱의 외곽이 다시 바뀌어 회복이 끊겨 보입니다. 스킬 발동·판정 타이밍은 이번 시각 비교 범위 밖입니다.
- **알파 정리 필요.** 네 프레임의 셀 테두리 알파는 모두 0입니다. 최소 여백 좌 12/우 13/상 15/하 22px이며 프레임 경계 잘림은 보이지 않습니다. 반면 어두운 배경에서 뿔 사이, 턱·발톱 틈, 하체 아래에 연회색 매트 잔류가 보입니다. Unity에서도 같은 부위가 밝게 붙어 Idle과 번갈아 나타납니다.
- **격리 Unity 배율 PASS.** 6000.6.5f1, 1280×720 오프스크린 카메라에서 동일 PPU512, 피벗 (256,0), SpriteRenderer Scale3/월드 폭 3, 셀 336px로 승인 Idle4·Attack4 모두 8장을 캡처했습니다. 이 결과는 시험 씬의 표시 계약 확인입니다. 본편 보스 씬과 Animator 연결은 미검증입니다. 종료 0, 격리 스크립트 원복.

비교 근거 `Temp/i30-attack-v2-qa/`

| 실물 | SHA256 |
| --- | --- |
| `Unity_336px_Comparison.png` | `4625805ebf02d042a2b2c8a4f8c9ffbfc607fd37050378666248d3d1a94e0510` |
| `Unity_Idle_Attack_Idle_8FPS.png` | `280ac02b7f63eb096a3533ba36693514f127491590a792d8c782277b294d9b11` |
| `Attack_Alpha_1to1.png` | `8630470f7b997e3390c600cce27efcaca7578fc75df16ffe7783bfe40050a73` |

APNG는 Unity 개별 렌더를 125ms 간격으로 엮은 비교본입니다. 승인 재검토 범위는 매트 제거와 03→Idle00의 하체·앞팔 연속성입니다. #85/#131/#132, 승인 자산, Save/UserData 보호. 본편 적용, main 병합, Push 없음.
