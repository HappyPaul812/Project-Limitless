# LIMITLESS / 2026-11-04 / Chapter1·2 리뉴얼 — Organic ForestSpider Attack 핵심 3키포즈 오프라인 납품

**담당 브랜치:** `docs/art-renewal` · **Issue:** [#90](https://github.com/HappyPaul812/Project-Limitless/issues/90) · **상태:** `ART_PILOT_3_POSES / ATTACK8_NOT_COMPLETE / SHOOT8_NOT_STARTED / CONTACT_NOT_VERIFIED / USER_VISUAL_REVIEW_PENDING / NOT_UNITY_APPLIED`.

## 1. 확인한 정본 / 변경 범위

- `main`의 `START_HERE.md`, `COMMON_RULES.md`, `docs/art-renewal`의 `WORLD_ART_RENEWAL.md`, `WORLD_ART_RENEWAL_LATEST.md`, [Issue #90 발주](https://github.com/HappyPaul812/Project-Limitless/issues/90), 기존 승인 [#60](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6598038404) 참조.
- **정지 원화 v5 USER_ART_APPROVED** `ForestSpider_Organic_v5_FORELEG_CONTOUR_ALPHA_512.png` SHA256 **`c600cd2264f6a0323771288666d16b49aa213ad938036d0746ed6f7f1d50adb9`**. 기존 Idle8·Walk8은 이 회차 수정하지 않음. approved Walk8 ZIP SHA256 `771bd1813608aa3e350c6124ab0d43293c6d2988b1833014da64484d5272c8da`. 기존 Unity 계약은 각 Idle/Walk/Attack/Shoot 8프레임·10FPS·PPU256·scale 0.7·우향. 제작은 **Attack3만**.
- 독침벌 VenomBee v3, 그 기존20 및 `RIGHTS_HOLD / TARGET_CONTACT_NOT_VERIFIED`, Chapter2 승인 공식 몬스터·보스, Wolf/Bear/Fox, Owl, 승인 환경·NPC, Chapter4 메카닉 Spider 후보 전체 보호. `main`, 원본 Unity/Scene/Prefab/GUID/meta/AI·Drop·Spawn·Save/UserData/Collider 변경 없음.

## 2. 실제 제작 PNG 3개 — 각 512×512 RGBA, 파일별 독립 바이트

| 신규 포즈 | 실제 파일명 (`Attack3/`) | SHA256 | 가시 알파 bbox, x0/y0/x1/y1 |
|---|---|---|---|
| Prepare | `ForestSpider_Attack_00_Prepare_512_RGBA.png` | `439ef26e4528e9f57877b471f602f9cfaa2f7bcd2a86b2fce991ec2e17ae5bf2` | 21/116/490/411 |
| Contact | `ForestSpider_Attack_01_Contact_512_RGBA.png` | `4a4d4418590db7d3b0f4eeb32dc79bb620822991ab4d08c978f10e538307de04` | 21/148/490/411 |
| Recover | `ForestSpider_Attack_02_Recover_512_RGBA.png` | `9fad6682201222f90e8f908d3c4eb497012c9f567cf40504dc19d080c71609d6` | 29/99/482/411 |

- **가시 알파(>8) 캔버스 경계 접촉: 3개 전부 좌·우·상·하 = 0**. 프레임당 512 RGBA 디코드 PASS. 서로 다른 원화 픽셀과 SHA를 가진 신규 포즈 3개로, 예전 파일 이름만 변경한 것 아님.
- 오프라인 검토 자료: `Evidence/ApprovedIdleWalk_vs_Attack3_TRUE512_OFFLINE.png`(기승인 v5/Idle00/Walk07과 세 신규 PNG 실물 512px 동일 크기), `Evidence/ApprovedIdleWalk_vs_Attack3_128_40_OFFLINE.png`(모든 PNG의 실제 128×128/40×40 축소), `Evidence/Attack8_3Produced_5NotProduced_Storyboard.png`(미래 8슬롯 아이디어, **3개 실제 신규·5개 NOT_PRODUCED**). 실제 접촉점·애니메이션 이벤트·Hit 타임은 임의 확정하지 않음.
- 기존 승인 원화 원본·Idle00·Walk07을 `Reference_KEEP/`에 **복사만** 하여 실제 SHA 검증: approved v5/Idle00=`c600cd...d50adb9`, Walk07=`edf725d87aafafcb81d01f09b121bf1e20a71b4fbfd829dbd1b0b6a3abf1b189`. 라이브러리 보관본 원본을 덮어쓴 적 없음.

## 3. 제작 방식 / SourceDeclaration / 미술 품질과 한계

- 새 아트 3개는 이번 대화에서 생성한 **2172×724 RGBA 3포즈 시트**의 원화를, 수평 3등분으로 잘라 다리를 절단하지 않도록 각각 연결된 투명 실루엣 단위로 독립 추출·종횡비 유지 리샘플하여 512×512 RGBA로 배치한 후보. **소스 시트 원본 SHA256 `2533b3bbc04a78bb496083a7d1bec73179ec1e7fd7a9741ab5d4480a764e9c0a`**는 ZIP의 `Reference_KEEP/Generated_3Pose_Original_Source_UNMODIFIED.png`에 원래 바이트 그대로 포함.
- 새 시안은 기존 v5의 녹갈색 유기형 갑각, 주황 다중 눈, 거미 다리와 발톱 등의 시각 테마를 참조함. 별도의 이미지 생성으로 만들어져 **승인 v5와 얼굴/몸통/다리 관절의 모델 동일성 및 실제 접지 품질이 완전히 보존됐다고 할 수 없음**. 특히 Contact의 전방지지/실전 피격 표현은 바울 대표 시각 판단 필요. 기존 외부 Spider 스프라이트 직접 수정/트레이싱/색만 교체한 결과로 주장하지 않음.
- 명시적 `Evidence/SourceDeclaration.md`에 생성 원본·가공 방식·기존 승인 참조·외부 권리와 구분을 기재. 독립 법적 권리 보증 아님. 기존 외부 원본을 본편에서 제거/교체한 게 아님.
- **정적 기술 판정 `PNG_3_TECH_PASS` / 미술 `USER_VISUAL_REVIEW_PENDING` / `MOTION_INTERPOLATION_NOT_PRODUCED` / `FOOT_LOCK_NOT_VERIFIED` / `TARGET_CONTACT_NOT_VERIFIED` / `NO_UNITY_QA`**. 10FPS·PPU256·scale.7은 기존 엔진 계약으로만 참고; 이번 신규 PNG는 아직 Sprite Import/애니메이션 등록 안 함.

## 4. ZIP·SHA·전달 위치

- **파일:** `LIMITLESS_SPIDER_ATTACK3_ISSUE90.zip` / **4,122,683 bytes** / **SHA256 `91411d159d401f1772e80e848d78550dd62552f65c1a76d659c15d8b0df44d3e`**.
- **ZIP 21 entries, CRC PASS**(압축 전수 `testzip`), `Manifest.json` **19/19 페이로드 bytes·SHA256 일치**. `SHA256SUMS.txt`, `Evidence/QA_Results.json`, 3개 PNG, 각각 128/40 프리뷰, 승인 대비 비교 2종, 타임라인 도식, 승인 참조 원본·SourceDeclaration 포함.
- 채팅 다운로드가 여러 차례 실패한 이력이 있어 **ChatGPT 라이브러리 `/LIMITLESS_Art_Delivery/`에 직접 업로드 성공**: ZIP, Prepare/Contact/Recover 개별 PNG, 512px 실물·128/40 비교판, Attack8 3/8 도식 총 7개. 라이브러리 업로드 후 실제 목록에서 재확인 필요. 사용자 PC `F:\Downloads` 자동 복사한 것 아님. GitHub는 Markdown만 커밋, ZIP/PNG 바이너리 GitHub 등록 0.

## 5. 완료 게이트 / 다음 단계

1. 바울 대표가 실제 3포즈 및 128/40 비교를 보고 **Attack3 디자인 승인/표적 보완/반려** 결정할 때까지 미술 승인 대기.
2. 3포즈가 승인되고 나서 필요한 경우 Codex가 **추가 전수 QA 없이** 검토용 최소 Delta만 별도 수행; 현재 3포즈만으로 Attack8·Hit·접지 애니메이션이 완성된 것은 아님.
3. **Attack8의 나머지 5독립프레임, Shoot8 전체, Unity 적용 및 `main` 직접 commit/Push/merge 금지 유지**. VenomBee v3 관련 권리·아트 상태 보호.