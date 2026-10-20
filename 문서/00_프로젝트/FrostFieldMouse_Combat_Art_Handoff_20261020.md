# 메인 아트 담당 전달문 — 서리들쥐 A 전투 3모션

서리들쥐 A의 승인된 디자인과 Idle을 유지하여 **앞발 할퀴기 Attack / Hit / Defeat**만 제작해 주세요. 물기·Skill·설빙의 폭군 제작은 이번 범위에 없습니다.

| 납품 파일 | 크기 | 프레임 | FPS | 00 → 01 → 02 → 03 |
|---|---|---|---|---|
| FrostFieldMouse_A_Attack_4Frames_v1.png | 2048×512 RGBA | 가로4 / 셀512 | 8 | 승인 Idle00 복사 → 준비 → 오른쪽 앞발 할퀴기 타격 → 회수 |
| FrostFieldMouse_A_Hit_4Frames_v1.png | 동일 | 동일 | 8 | 즉시 움찔 → 최대 피격 반응 → 회복 → 승인 Idle00 복사 |
| FrostFieldMouse_A_Defeat_4Frames_v1.png | 동일 | 동일 | 8 | 힘 풀림 → 몸 낮아짐 → 옆으로 쓰러짐 → 완전히 쓰러진 몸 |

- 통합 4×4 시트 대신 **독립 Strip 3장**입니다. 기존 `FrostFieldMouse_A_Idle_4Frames.png`는 재사용하며 수정·재납품하지 마세요.
- PPU512 / 각 Sprite Pivot (0.5,0) / VisualScale1 / 오른쪽1종. 반대 방향은 Unity X 반전입니다. 각 모션0.5초, Attack/Hit 후 Idle00, Defeat03은 유지합니다.
- 각 셀 좌상단 기준 Pivot (256,512), 승인 접지 가이드 y490(하단22px 여백)를 유지해 주세요. 지지 발/루트는 안정적으로, 공격 앞발은 자유롭게 움직입니다. Defeat도 같은 지면에 쓰러져야 합니다. 프레임별 크롭·리사이즈 금지, 셀 경계 최소2px 투명 여유를 확보해 주세요.
- 긴 꼬리·앞니·발톱·거친 털·야생성과 승인 색상을 유지해 주세요. Hit은 공격과 구분되게, Defeat03은 식별 가능한 쓰러진 몸으로 그려 주세요. 몸 전체 이동만으로 채우거나 4장 복제, 마스코트화, 알파 소멸, 과도한 이펙트/궤적/플래시를 넣지 마세요.
- 실제 전투 표시는 정사각 셀112px(기준 Canvas1280×720)입니다. 128px 비교판도 제출해 주세요. 512px 원화 정보를 실사용 축소 화면에서 확인합니다.
- Unity 이름은 `FrostFieldMouse_A_Attack_00`~`03` / `Hit_00`~`03` / `Defeat_00`~`03`이며, Import는 Codex가 Multiple/Point/Clamp/MipMap Off/Compression None/개별 하단 Pivot로 수행합니다. 아트 납품에 meta·Scene·코드를 넣지 마세요.

납품 ZIP: **`Limitless_Chapter3_FrostFieldMouse_A_Attack_Hit_Defeat_v1.zip`**

`Sprites/`에 위 PNG3장, `Manifest.json`, `README.md`, `SHA256SUMS.txt`, `QA/Offline_QA.json`, `Preview/`를 포함해 주세요. Manifest에 실제 SHA/bytes/크기/셀/순서/FPS/PPU/Pivot/방향/Scale과 아래 기준선 SHA를 기록하고 ZIP SHA는 납품 보고서에 적어 주세요. Preview는 125ms 간격 APNG/HTML과 4프레임/128px 비교판이며 Attack/Hit 후 기존 Idle, Defeat03 최소1초 유지로 보여 주세요.

승인 기준선(수정 금지):

- 1포즈 SHA256: `0516914ddad36af5854065d66d01b9664329aa89eea8b394480eed6af92e32c2`
- Idle SHA256: `5afebfa1f95ca5673162513fdea63b025546aa836550c0ceb17f0334659988f2`

제작 규격 미결 사항은 없습니다. 타격02와 피해 시점의 정확한 동기, 연속 피격 상태 전환은 납품 후 Codex 통합 검증 사항입니다. 이번 전달은 미술 제작 계약이며 본게임 구현·Push·main 병합 승인이 아닙니다. [상세 계약](FrostFieldMouse_Combat_Production_Contract_20261020.md)
