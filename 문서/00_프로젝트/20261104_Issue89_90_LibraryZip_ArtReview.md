# LIMITLESS / 2026-11-04 / Issue #89·#90 라이브러리 ZIP 파일·시각 QA 결과

- 작성: 총괄 기획 담당 / `docs/planning`
- 사용자 제출 로컬 Windows 경로: `F:\Downloads\LIMITLESS_SPIDER_ATTACK3_ISSUE90.zip`, `F:\Downloads\Limitless_Ch3_ColdVFX_Issue89_Pilot_20261104.zip`.
- **실제 검증 대상:** 해당 파일들과 이름/바이트 수가 같은 ChatGPT 라이브러리의 보관 ZIP 사본. **사용자 F: 로컬 원본의 SHA는 본 검수자가 직접 열어보지 않았으며 `F_DRIVE_BYTES_NOT_VERIFIED`.** 라이브러리 자료를 컨테이너에 복사하여 ZIP CRC/SHA/Manifest/PNG 알파 실제 검사 + 라이브러리 저장 비교판 직접 시각 검토.
- 관련 보고: [메인 아트 #89](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6619789789), [리뉴얼 #90](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6619839416).

## 1. Archive/PNG 실검사

| 항목 | 냉기 VFX #89 | Organic Spider Attack3 #90 |
|---|---|---|
| ZIP 이름 | `Limitless_Ch3_ColdVFX_Issue89_Pilot_20261104.zip` | `LIMITLESS_SPIDER_ATTACK3_ISSUE90.zip` |
| 크기 | 1,535,561 bytes | 4,122,683 bytes |
| 전체 SHA256 | `83b4efeee62cdb4cabf7dbcc4d2aea21cbefdb4507e03f53b8c59d8079e9e4ba` | `91411d159d401f1772e80e848d78550dd62552f65c1a76d659c15d8b0df44d3e` |
| ZIP entries / testzip | 17 / CRC PASS | 21 / CRC PASS |
| SHA256SUMS 대조 | 16/16 PASS | 19/19 PASS |
| Manifest 개별 bytes+SHA | 15/15 PASS (후보3/3 별도 확인) | 19/19 PASS |
| 후보 독립 PNG | 3장, 512×512 RGBA, 투명 알파·4변 가시알파 접촉0 | 3장, 512×512 RGBA, 투명 알파·4변 가시알파 접촉0 |
| 실제 Unity·원본 F: | NOT_VERIFIED | NOT_VERIFIED |

## 2. 미술 검토 — 대표 최종 시각 승인 아님

### VFX #89
- 직접 본 `Issue89_ColdVFX_Comparison.png`과 `Issue89_Background_Comparison.png`(라이브러리): 한기는 안개·눈송이 원형, 동상은 붙은 얼음결정, 동결괴사는 검푸른 쐐기/균열이 구분됨. 512/128 대표 정적 디자인은 **세 상태의 미술 언어 후보로 적합**.
- **주의 A:** 밝은 설원 배경에서 한기의 옅은 연무가 대비를 잃음. 상태명/아이콘/남은 턴 UI가 반드시 함께 보여야 하며, 독립 VFX도 40/128 게임 크기에서 필요한 최소 윤곽대비 확인/소량 보정 후보.
- **주의 B:** 동상·동결괴사는 둘 다 작은 40px에서 빽빽한 뾰족 얼음 결정을 포함해 **색만으로 구분되는 경향**이 일부 있음. 동상은 몸을 덮는/붙는 구조, 동결괴사는 어두운 균열/분리된 위험 덩어리로 실루엣을 더 분리하는 표적 제안을 검토.
- **HOLD:** 아이콘·턴/중첩 숫자는 PNG 자체에 구워 넣지 않음. Unity 실제 PPU/Pivot/Sorting/상태 HUD 위치·FX 생명주기/성능/광과민·화로와 정화 동작 검증 전. 전체 애니메이션 양산/원본 적용 금지.

### Organic Spider Attack3 #90
- 직접 본 `ForestSpider_Attack3_512_Comparison.png`, `ForestSpider_Attack3_128_40_Comparison.png`: 기존 승인 v5/Idle00/Walk07에 비해 Prepare(낮춤)→Contact(앞다리 전개/전방 찌름)→Recover(복귀) 의도가 표현됨. 작은 40px에서도 공격 자세 차이는 어느 정도 보임.
- **주의:** Prepare→Contact에서 상체의 회전/두부 위치·전완 실루엣이 크게 바뀌고 Recover에서 되돌아와, **10FPS 연속 프레임으로 직접 연결하면 점프/방향 전환이 도드라질 가능성**. 아직 실제 Attack8 전체 영상이나 공격 대상과의 실접점 QA는 없음. Attack 3 키포즈의 연속성/중심점/지지 다리를 먼저 검토해야 함.
- 신규 Attack8 나머지5장과 Shoot8은 미제작. 사용자 최종미술 승인 전에 프레임 전체 양산 금지.
- 승인 v5/Idle8/Walk8 SHA 바이트 보존, 기존 `WALK_CONTACT_HOLD` 유지. Bee v3 라이선스 `RIGHTS_HOLD`도 별도 유지.

## 3. 후속 권고 (일반적인 자동 실행 지시 아님)

1. 아트 담당에게 **#89 한기 대비/3종 40px 실루엣** 및 **#90 Contact 전후 회전·몸통 높이·중심점**만 최소 표적 검토를 요청. 재작화 필요가 없으면 비교/타임라인 근거로 그대로 제출해도 됨. 기존 정상 PNG 전량 재작업 금지.
2. 바울 대표가 현 후보 시각 승인/소량 수정 요청 여부를 결정하면, Codex는 실제 `F:\Downloads` ZIP 해시를 위와 대조하고 **Unity 6000.6.5f1 격리 장면에서만** 40/128·3비율 HUD 충돌 및 3키포즈 이어짐을 표적 검증. 이미 완료 #84/#85/#87/#88 37/81/36/63 PASS 전수 QA 반복 금지.
3. 새로운 FPS/PPU/시전 간격/중첩 피해·MonsterID/아이템/상품·VFX 실 구현/본편 원본 적용은 승인 전 HOLD. Codex 5시간 사용량 불필요 재소모 금지.

## 판정
- #89: `LIBRARY_ZIP_TECH_PASS / VFX_ART_CANDIDATE / CHILL_SNOW_VISIBILITY_REVIEW / 40PX_SHAPE_DISTINCTION_REVIEW / USER_VISUAL_APPROVAL_PENDING / UNITY_NOT_TESTED / NOT_ORIGINAL_APPLIED`
- #90: `LIBRARY_ZIP_TECH_PASS / ATTACK3_KEYPOSE_CANDIDATE / MOTION_CONTINUITY_REVIEW / TARGET_CONTACT_NOT_VERIFIED / ATTACK8_INCOMPLETE / USER_VISUAL_APPROVAL_PENDING / UNITY_NOT_TESTED / NOT_ORIGINAL_APPLIED`
- 본 회차 GitHub 문서화 외 제품파일/Unity/Save/UserData/원본 Workspace 변경0, main 직접커밋·Push·merge0. 라이브러리 ZIP의 검증을 F: 파일 검증으로 확대하지 않음.
