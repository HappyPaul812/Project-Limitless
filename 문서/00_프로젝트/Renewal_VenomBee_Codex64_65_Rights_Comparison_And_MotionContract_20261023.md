# LIMITLESS / Chapter1 VenomBee — Codex #64·#65 후속: 원본 권리·방향 비교·모션 제작 계약 (2026-10-23)

- 담당: 리뉴얼 아트 / 작업 브랜치 `docs/art-renewal`.
- 근거: [Issue #63](https://github.com/HappyPaul812/Project-Limitless/issues/63), [Codex 실제 로컬 검수 보고 #64](https://github.com/HappyPaul812/Project-Limitless/issues/64#issuecomment-6357901621), [실 Unity 비교판 인계 의뢰 #65](https://github.com/HappyPaul812/Project-Limitless/issues/65); 기존 [v1 정식 보고](Renewal_VenomBee_v1_Formal_Delivery_Verification_20261023.md).
- **명확한 구분:** #64의 41 신규 Unity PASS / 캡처 보정 15 PASS는 바울 대표에게 전달된 **Codex 로컬 검수 요약**. 이 리뉴얼 세션에서 Codex 로컬 비교 스크린샷의 픽셀/파일 SHA를 직접 확인한 것은 아님. 새 독침벌 1포즈 독립 표시 `TECH_PASS`, 원본 전투 애니메이션 교체 `ART_HOLD`, 사용자 시각 승인 `PENDING`이다. 기존 41/15 PASS 재실행하지 않음.

## 1. 실제 사용 중 외부 Pilot Bee 출처·권리 기록

| 항목 | 근거·현황 |
|---|---|
| 원본명 / 업로더 | `Pilot_Bee_Game_Character` / `2DPIXX` — https://opengameart.org/content/pilotbeegamecharacter (게시일 2016-08-27) |
| 원본 LICENSE 제작자 | **Jana Ochse / 2D!PIXX** — `Assets/ThirdParty/2DPIXX/PilotBee/Original/LICENSE.TXT` 실제 GitHub 원문 확인 (`www.2dpixx.de`) |
| OpenGameArt 웹 표시 | **CC BY 3.0** |
| GitHub에 보존된 제작자 원본 `LICENSE.TXT` | **CC BY 4.0**; 웹 3.0과 **표기 충돌 미해결**. 어느 버전의 법적 적용이 최종인지 임의 단정 금지 |
| 사용자 보존 ZIP | `Assets/ThirdParty/2DPIXX/PilotBee/Original/pilot_bee.zip`; README 기록 SHA256 `C979B691E76E850FB8C1949D8BF53FE55B0B085B0A5012D83D5F6262AE48A2B6` (이번 세션 ZIP 본체 재계산 아님) |
| 원본 idle/attack | `Original/pilot_bee_idle.png` 1904×430 (238×215, 8열/2행 중 10프레임); `Original/pilot_bee_attack.png` 3150×253 (315×253, 10열/10프레임). 원본 `.meta` GUID는 **Idle `21000000000000000000000000000007` / Attack `21000000000000000000000000000008`** |
| 현재 MonsterDefinition 참조 | `Unity/Client/Assets/_Project/Resources/MonsterDefinitions/02_VenomBee.asset`: Idle GUID `24000000000000000000000000000004`, Attack GUID `24000000000000000000000000000005`. **원본 보관용 GUID와 현행 참조 GUID는 서로 다름**. Codex #64가 현행 활성 PNG와 외부 원본의 **바이트 일치**를 보고했으나, 현행 참조 PNG의 실제 경로·SHA 대조표가 원격에 공개됐는지는 아직 불명 |
| 원본의 런타임 구분 | 과거 PilotBee `README.md`가 ‘미연결’이라 적은 것은 이전 시점 설명. 후속 Codex #64의 **현행 사용·바이트 동일** 검수를 우선하되 원본 보관 폴더 `ThirdParty` 자체를 변경하거나 삭제하지 않음 |

**권리·크레딧 처리:** CC BY 계열은 저작권 표시를 조건으로 상업적 이용 자체가 금지된 것은 아니지만, 적용 라이선스 **버전/권리자 확인과 최종 배포 환경의 라이선스 준수 검토가 끝날 때까지 `RIGHTS_VERSION_CONFLICT_OPEN`**. 임시 크레딧 후보: `Pilot_Bee_Game_Character — Jana Ochse (2D!PIXX), https://opengameart.org/content/pilotbeegamecharacter, CC BY 3.0 (web) / CC BY 4.0 (packaged LICENSE), version confirmation pending`. 제작자가 부여한 변경 사항 표시와 라이선스 링크, 실제 포함 여부를 별도 출시 크레딧 대장에 남길 것. 기존 파일/원본 LICENSE 무단 수정·배포 금지. **현재 후보 v1은 외부 PNG를 리컬러/트레이스한 것이 아니라 신규 생성 콘셉트에서 분리한 별도 그림**이며, 기존 사용분의 attribution 의무가 자동으로 소급 소멸하는 것으로 쓰지 않음.

## 2. 신규 v1 ↔ 현행 애니메이션 기술 차이

| 비교 | 기존 게임 독침벌 | v1 신규 디자인 |
|---|---|---|
| 방향 | 우향 `sourceFacesRight=1` | **좌향** 단독 1포즈 |
| 대기 | **Idle 10프레임**, cell 238×215, 8열 분할 | 대기 애니메이션 0 |
| 공격 | **Attack 10프레임**, cell 315×253, 10열 | 공격 애니메이션 0 |
| 속도·크기 | **10FPS**, `visualScale=0.85`, 게임 전투 시트 두 장 | 캔버스 **512×512 RGBA** 한 장; PPU/Pivot/전투 스케일·좌우 플립 재계산 미승인 |
| 필드·전투·독 | Field01 Spawn 3개·기존 AI/독 DoT/드롭/전투 로직 사용 | 실제 MonsterDefinition/GUID/Runtime 교체 0 |

- 납품 v1 ZIP `Limitless_Chapter1_VenomBee_OriginalKeyArt_Pilot_v1_20261023.zip` **2,291,579 bytes, SHA256 `2e65662d68c77cfb90d7e39d9d40c2cd2816ee33882debe8a8e93ace782bd478`, 8 files / CRC PASS**. 이번 세션 작업 컨테이너의 ZIP SHA를 재계산하고 PNG 세 장의 디코딩을 확인.
- `VenomBee_OriginalKeyArt_Pilot_v1_512.png` RGBA512 bbox (11,39,501,472); `VenomBee_Preview_128.png` 128×128, `VenomBee_Preview_40.png` 40×40. **128/40 미리보기는 투명 영역이 양쪽 캔버스 가장자리까지 뻗으며**, 날개 끝·더듬이/발·독침의 40px 판독성과 크롭/알파 외곽을 실제 게임과 다시 비교해야 함. ZIP 파일 무결성과 실제 미술 PASS를 혼동 금지.
- 10FPS×10프레임은 각 시퀀스의 **표준 재생 길이 1초**에 해당하나 게임의 루프/대기/공격 재생 시점은 **기존 코드 유지**. 기존 20프레임은 재제작 미착수.
- Codex #64 신규 v1 1포즈 격리 Unity 표시 **`TECH_PASS`**. 기존 우향 애니메이션 교체는 **`ART_HOLD`**.

## 3. #65 동배율 실제 Unity 비교판 — 증거 확보 상태와 시각 검토

- #65는 Codex 로컬 `codex/art-pilots-isolated-qa-20261023` 보고 제품 commit 단축 `3bb4f983`, QA 보고 `91510554`와 #64 기생성 캡처를 **재사용**하도록 주문한 별도 이슈이다. `#65`에서 확인한 원격 GitHub 댓글은 현재 **0개**, `art-pilots`/`review-boards` 이름의 GitHub 원격 브랜치 검색 결과 없음. **실 Unity 비교 이미지(원본·Ch2 공식·v1 동배율)의 원격 공개 링크와 실제 파일 SHA는 확보하지 못함.** 리뉴얼 담당은 Codex Windows 로컬 경로에 직접 접근할 수 없다. `BOARD_EVIDENCE_REMOTE_PENDING`; 실제 파일을 봤다고 주장하지 않음.
- 보유 v1 128/40 실제 미리보기 **정적 관찰**의 장점(가설): 검정-황금 갑각과 날개의 넓은 형태, 강한 침·붉은 눈으로 ‘공격적 벌’ 식별성. 위험: 40px 세부 다리·독침/더듬이 시인성 약화, 후보가 기존 우향과 반대라 공격 프레임 구성이 아직 없음, 알파 테두리와 Field01·Ch2 공식 몬스터 동배율 일관성은 **실제 비교판 판독 전 결정 금지**.
- **Codex 증거 인계 요청:** #65에서 이미 생성한 `VenomBee 현행 Idle/Attack ↔ v1 좌향 ↔ Ch2 공식 몬스터` **동일 카메라·동일 배율 GameView 원본 40px/128px/전투 배치** 비교 이미지 (각 파일명·실제 SHA256·캡처 유형·PPU/Pivot/Scale/FPS·출처)만 비기밀 판독 가능한 GitHub 첨부/문서 링크 또는 바울 대표가 이미 확인할 수 있는 경로로 전달. 퀘스트/Save/개인정보 제외. **#64의 41PASS·15PASS 및 기존 148/136PASS 반복 실행 금지**. 실제 이미지 미수령 동안 `COMPARISON_ART_REVIEW_PENDING`.

## 4. 바울 대표가 **v1 디자인을 승인한 경우에만** 발효할 다음 제작 계약 — 현재 제작 금지

**승인 게이트:** 바울 대표가 실제 #65 비교판을 본 후 `VENOMBEE_DESIGN_V1_USER_APPROVED`를 명시해야 **새 애니메이션 20프레임 개발 착수**. 확인 전에는 모두 `NOT_STARTED`.

1. **원화 일관성:** 승인된 v1 실루엣·복부독침·날개/갑각·광원·색 조합을 기준으로 **독립 동작 프레임**. 기존 2DPIXX 아트 트레이스·리컬러 금지. 네이티브 디테일과 40/128px 실가독성 확보.
2. **좌우 계약:** 현행 정의 `sourceFacesRight=1`이므로 **기본 우향** Idle10·Attack10을 설계하는 것이 안전하다. v1 좌향은 별도 기준으로 두고 원화의 머리/침/눈·다리 비례를 유지해 우향 재구성; 무조건 기계적 미러링으로 완료 판정하지 않음. 왼/오른방향 시각 자료 준비하고 런타임 플립·공격 표적 표시/기능은 Codex가 검증, **전투 로직 수정 금지**.
3. **기술 시트 규격:** 현재 계약 기준 **Idle 10프레임 @10FPS, cell 238×215, 8열×2행**; **Attack 10프레임 @10FPS, cell 315×253, 10열×1행**. **현행 Sprite 원본에 덮어쓰지 않고** 신규 후보 Sprite Sheet/개별 PNG로 별도 납품. 디자인 품질상 셀 규격 변경 필요하면 20프레임 일괄 생산 전 **Codex 격리 기술 검토·바울 대표 별도 합의**로 먼저 확정. Pivot·PPU·중심/발접지·프레임 bbox/실루엣·파일 알파·방향·축척 일관성을 QA.
4. **동작 기획:** Idle에서 날개 진동·상하 부유·다리와 독침 호흡이 10프레임 주기로 자연스럽게 돌아오게 하고 09→00 루프 이음 체크. Attack은 00준비 → 전진/침 조준 → 명중 강조 → 회복 → 복귀의 10프레임 동작을 구성하되 **현재 독 DoT/공격 판정 타이밍은 원본 코드 값 그대로**. 날개의 점멸/급격한 밝기 변화로 광과민 부담을 만들지 않음.
5. **샘플 게이트:** 먼저 Idle 2~3개 포즈와 Attack 2~3개 키 포즈만 **격리 미술 비교 및 기술 판단** → Codex 40/128px·Unity 양방향 시인성/프레임 안정성 Delta QA → 대표 중간 승인 후 나머지 20프레임 제작; **바울 대표 승인 전에는 키포즈도 제작하지 않음**.
6. **최종 납품·검증:** alpha PNG, 개별 프레임/strip/sheet, 10FPS 재생프리뷰, 128/40 비교, Manifest·SHA256SUMS·ZIP CRC, 실동일 GameView 비교판·기존 효과/독·드랍·Spawn/AI/Quest/Save/접근성 보호 검증. 승인 전 사용자 Unity 원본·MonsterDefinition·GUID/meta·기존 Sprite 변조/본편 적용·main 직접 병합 금지.

## 범위·상태
- 새 독침벌 애니메이션 **0프레임** / 새 아트 **0파일** / 기존 1포즈 ZIP **바이트 변경0**.
- 기존 Wolf·Bear·Fox Run6, Ch2 공식 몬스터·보스, Eldiran NPC OGA03/09/20 시안, GateGuard·태온 구분 검토, forest_spider 제작 HOLD, 기존 54행 감사 및 게임 전투·독·드롭·Spawn·AI·세이브 전부 보존.
- `SOURCE_IDENTIFIED / RIGHTS_VERSION_CONFLICT_OPEN / V1_TECH_PASS_REPORTED / DESIGN_APPROVAL_PENDING / COMPARISON_BOARD_NOT_AVAILABLE_REMOTELY / MOTION_NOT_STARTED / NOT_UNITY_APPLIED / MAIN_UNCHANGED`.
