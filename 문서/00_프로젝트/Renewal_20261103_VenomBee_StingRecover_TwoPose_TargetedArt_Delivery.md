# LIMITLESS / 2026-11-03 / Issue #63 — VenomBee 신규 Sting·Recover 2포즈 실제 제작·미술 검수 보고

**브랜치:** `docs/art-renewal` · **이번 범위:** 오프라인 후보 이미지 두 장, 비교·타이밍 검토, SHA/Manifest/CRC, 출처 기록. **승인된 본게임 교체/Unity 적용 아님**.

## 1. 최신 코드 및 기획 계약의 실제 근거

- GitHub [Issue #63 / Codex 격리 공격 타이밍 QA 153 PASS](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6599482389), [기획 후보 A 제작 기준](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6599597527), `COMMON_RULES.md`, `WORLD_ART_RENEWAL_LATEST.md` 확인.
- **코드로 확인된 방향:** 적 왼쪽 → 아군 오른쪽 **+X**, venom_bee 일반근접, sourceFacesRight=1, 화면에서 전체 Sprite를 갑자기 flip하지 않음. 기존 실제 피해 **약 0.25초**. 후보 A 오프라인 기준 Attack10 **10FPS, 총 1초**, Sting index02 (0.2–0.3초), Recover index04–08, 제자리 마감 index06–09, 1초 후 Idle. 후보 A는 **격리 Unity 153 PASS/0 FAIL**이나 **제품 Runtime 반영/실 Battle Scene 전체 회귀 승인 아님**.
- 외부 PilotBee 원본 아트는 현재 게임에서 활성 참조되어 있고, 웹 OpenGameArt 페이지 **CC BY 3.0**, 동봉 ZIP `LICENSE.TXT` **CC BY 4.0** 충돌 **`RIGHTS_HOLD`**. 새 그림이 생겼다는 것만으로 문제 해결·상업적 권리 PASS로 처리하지 않음.

## 2. 새로 생성된 원화와 실제 파일 납품

**새 ZIP:** `LIMITLESS_BEE_STING_RECOVER_NEW_20261103.zip`, **4,086,641 bytes / 24 entries / SHA256 `b05fcbd17572863494216499dc2befa0806361742d3c67f7866f5b871cef2c9e` / ZIP CRC PASS**. ZIP 내부 `Evidence/Manifest.json`에 **22개 파일 SHA256·bytes 모두 일치**; `Evidence/SHA256SUMS.txt`, `QA_Results.json`, `SourceDeclaration.md` 포함.

**실제 새 파일(기존 PNG 복제/이름 변경 아님):**

| 역할 | 새 512px RGBA 마스터 SHA256 | 새 315×253 RGBA 키포즈 SHA256 | 이전 동일 키포즈 SHA256 | 315×253 RGBA 변경 픽셀 |
|---|---|---|---|---:|
| Sting | `1d0c5c8d8653db3e1023bcd1655181fae0e3110a18b9b25ceb00a92aa59b95a8` | `a10fefc6db6ff2289d298ab8b3def2263283eb163d946e7b2cd7641428485de8` | `391ed94e50ffc4ddcf3bc4442e9913e0b947cc9d5d9a57aaf865f46556e021ce` | 35,668 |
| Recover | `5206076f556c23636b165b42a6b3555c2875b0f0b39539c4089887baa43b18ef` | `c5792c1c7c5f1b3be788dea96b013865f7f42162ca67e7d85d09add65bf9bb25` | `2afc69e803334b1ededdadb39e786f297d8fe3d9918e72997943c9f1593c740e` | 40,725 |

**제작/출처 사실관계:** 새 Sting 및 Recover 원형은 이번 ChatGPT 이미지 생성에서 새로 제작된 1399×1124 투명 PNG이며, ZIP `OriginalSource_KEEP/`에 원본 바이트 그대로 포함. 해당 원형을 **비율 유지 크롭/리샘플**하여 `Candidates/`에 512×512 마스터 및 기존 Attack 셀 **315×253** 2장으로 정렬. 기존 PilotBee 런타임 PNG를 재색칠·합성해서 산출한 것이 아님. 다만 생성형 원화의 독립 권리·타 자산과 유사성은 법적 보증이 아니며 상세 **`Evidence/SourceDeclaration.md`** 참조. 이 단계에서 **완성형 수작업 관절 모션이라고 주장하지 않음**.

**기존 바이트 보호:**
- `Reference_KEEP/Prepare_UNCHANGED.png`, `OldSting_UNCHANGED.png`, `OldRecover_UNCHANGED.png`, `IdleFolded_UNCHANGED.png`, `IdleSpread_UNCHANGED.png`는 2026-11-03 이전 오프라인 ZIP에서 읽어 **원래 SHA 바이트 보존**.
- 기존 `Limitless_Chapter12_ForestSpider_VenomBee_OfflineArtPilot_20261029.zip`의 오프라인 `Idle10 + Attack10 = 20개` PNG는 수정0·전량 재생산0. ZIP `Evidence/QA_Results.json`에 그 20개 기존 원본 SHA256 목록을 파일별 기록; 20개 전부를 이 ZIP에 다시 대량 복사하지 않음.
- Chapter2 공식 몬스터·보스/환경, 승인 Owl/Organic Spider·펫 Wolf/Bear/Fox, Unity/Scene/Prefab/MonsterDefinition/GUID/meta·AI/Spawn/독/드롭/Save/UserData·main **변경0**.

## 3. 미리보기 및 판정의 엄격한 구분

**비교:** `Review/Sting_Recover_Old_New_315_128_40_OFFLINE.png`는 같은 전투 셀의 실제 기존/신규 PNG를 비교. `Candidates/VenomBee_{Sting,Recover}_Preview_{128,40}.png`는 게임 셀의 종횡비를 유지하여 축소(40×40 사각에 강제 늘이기 없음). 새 315×253 프레임 네 변 가시 알파(>8) 접촉 0으로 정적 clipping 검사 PASS.

**10FPS 검토용 GIF**:
- `Review/VenomBee_10FPS_10Slot_128px_REVIEW_ONLY.gif`
- `Review/VenomBee_10FPS_10Slot_40px_REVIEW_ONLY.gif`
- 각 **10 슬롯 × 정확히 100ms = 1.0초**, 표시용 진행 막대가 있어 동일 키포즈 보유 구간도 10개 이미지 슬롯으로 분리. 타임라인은 Prepare 00–01, Sting 02–03, Recover 04–09의 **동일 포즈 반복**이며 **완성된 Attack10 신규 서로 다른 10프레임이 아님**. `Review/DamageTiming_10FPS_Storyboard_OFFLINE.png`에 타격 0.25초 및 +X 예상 방향 명시. 실제 타격 접점/게임 collision/피격 alpha와 동기화하지 않은 **오프라인 검토판**.

**최종 미술 품질 판정:**
- 새로운 Sting 자세는 복부 뒤 독침을 우측 +X로 길게 뻗어 공격 의도를 보여주지만 **머리/복부의 상대 비율 및 날개·다리 관절 정합이 기존 Prepare와 다를 가능성**이 크다.
- 새로운 Recover는 원래 복부 뒤의 독침을 되돌린 후보이나 **Sting→Recover 사이 날개/다리 수·체축 비율·실루엣 변화와 정상 Idle 복귀 연속성이 완성되지 않음**. 해당 결함은 단순 1초 타이밍이 맞다고 해결되지 않는다.
- 따라서 **`NEW_ART_BYTES_PASS / PNG_RGBA_DIMENSION_PASS / ZIP_HASH_CRC_PASS / ART_REVISE / KEYPOSE_CONTINUITY_HOLD / TARGET_PIXEL_CONTACT_NOT_VERIFIED / RIGHTS_HOLD / USER_ART_APPROVAL_PENDING / NOT_COMBAT_READY / NOT_UNITY_APPLIED`**.
- 유기형 Spider v5·Walk8 및 Owl 승인 자산은 전혀 변경하지 않았고, Bee 전체20 재양산·게임 적용·main commit/Push/merge 0.

## 4. 다음 단계

바울 대표에게는 **새 Sting·Recover 비교판**과 두 포즈 사이 형태 변화가 보이도록 보고. 미술적으로 동물 형태/날개/다리 관절을 현재 게임의 Prepare와 연결하는 수작업 표적수정이 필요하면 **이 2포즈에 한해서만** 추가 작업. Codex는 대표의 다음 검토 결정 뒤 격리 후보 A 런타임에서 최소 Delta QA. PilotBee 출처·LICENSE 버전 불일치를 먼저 해결하지 않고 **권리 PASS 처리·본게임 상용 적용 금지**.

**다운로드:** ZIP/PNG는 ChatGPT 작업 공간 `/mnt/data`에서 실제 생성·검증했으나 바울 대표 Windows `F:\Downloads`에 자동 복사 또는 GitHub 바이너리 Push된 것이 아님. 반복 다운로드 장애가 있을 수 있으므로 문서·SHA·SourceDeclaration로 내용 근거를 별도 보존.
