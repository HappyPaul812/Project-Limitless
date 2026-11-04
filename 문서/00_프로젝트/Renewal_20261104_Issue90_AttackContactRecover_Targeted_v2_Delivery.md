# LIMITLESS / 리뉴얼 아트 / Issue #90 — Organic Spider Contact01·Recover02 표적수정 v2 (2026-11-04)

**브랜치:** `docs/art-renewal` · **제품 적용:** 없음 · **상태:** `CONTACT_RECOVER_TARGETED_ART_CANDIDATE / FILE_TECH_PASS / JOINT_CONTINUITY_ART_REVIEW / TARGET_CONTACT_NOT_VERIFIED / ATTACK8_INCOMPLETE / NOT_UNITY_APPLIED`.

## 1. 작업 근거

- [Issue #90 최신 총괄 기획 발주](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6620386396), [Codex 실제 F: 512px·중심/관절 비교](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6620212147), [기존 Attack3 납품](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6619839416), `START_HERE.md`, `COMMON_RULES.md` 및 `WORLD_ART_RENEWAL_LATEST.md` 참조.
- Codex 기존 관찰: 알파 가중중심 Prepare `(249.15,273.97)`, Contact `(238.34,275.24)`, Recover `(247.78,251.50)` px. 이것은 실제 관절/Pivot이나 게임 접점이 아니므로 좌표값을 억지로 맞추지 않음. Contact 두부·복부·앞다리 불연속, Recover 상단 높이·지지다리 불연속을 표적 수정.
- **승인된 Organic Spider v5 정지 원화, Idle8, Walk8, Attack Prepare00은 변경하지 않음.** 원본 Attack3 ZIP도 바이트 보존. 신규 Attack8 나머지 5프레임·Shoot8 0장, VenomBee 변경0.

## 2. 납품 실물과 무결성

- **ZIP:** `LIMITLESS_SPIDER_ATTACK3_TARGETED_ISSUE90_v2.zip`, **1,719,271 bytes / SHA256 `804f8ebf85dbe7f7301cbfdecc6e85adc4e7d8888e23c3ee132bfca95854499b` / 12 entries / ZIP CRC PASS / Manifest 10/10 SHA256·bytes PASS**.
- **실제 신규 후보 PNG는 2개만**. 각각 **512×512 RGBA**, 네 변 가시알파 접촉0.

| 파일 (`Candidates/`) | 원본 SHA256 | 신규 v2 SHA256 | RGBA 변경 픽셀 | 변경 bbox (xyxy) |
|---|---|---|---:|---|
| `ForestSpider_Attack_01_Contact_Targeted_v2_512_RGBA.png` | `4a4d4418590db7d3b0f4eeb32dc79bb620822991ab4d08c978f10e538307de04` | **`0d0e24c2fae579f2f944255fe3bd3841216299b14f439cc3612906269020f697`** | **47,381** | `135,153,472,413` |
| `ForestSpider_Attack_02_Recover_Targeted_v2_512_RGBA.png` | `9fad6682201222f90e8f908d3c4eb497012c9f567cf40504dc19d080c71609d6` | **`0501a452467a07f34688abdfff5ea5ef49b0dfd93e1c99bdc368229986d44096`** | **54,747** | `133,107,480,396` |

- **보호 SHA:** 기존 `Prepare00` `439ef26e4528e9f57877b471f602f9cfaa2f7bcd2a86b2fce991ec2e17ae5bf2`; 승인 v5·Idle00 `c600cd2264f6a0323771288666d16b49aa213ad938036d0746ed6f7f1d50adb9`; Walk07 `edf725d87aafafcb81d01f09b121bf1e20a71b4fbfd829dbd1b0b6a3abf1b189`; 기존 3포즈 ZIP `91411d159d401f1772e80e848d78550dd62552f65c1a76d659c15d8b0df44d3e`. 작업 전 SHA 검증만 수행, 원본 수정/덮어쓰기0.
- **비교판:** `Evidence/Contact_Recover_BeforeAfter_{512,128,40}px_OFFLINE.png` 각각 같은 512 기준점에서 원본/수정본을 축소한 실제 이미지, `Evidence/Joint_Continuity_BeforeAfter_512_Annotated.png` 머리·복부·전방 어깨→관절→발끝 참고선, `Evidence/Attack8_ThreePoses_FiveNotProduced.png` 3개 시안과 미제작5개를 분명히 구분. `SourceDeclaration.md`, `QA_Results.json`, `SHA256SUMS.txt`, `Manifest.json` 수록.
- **다운로드 장애 대응:** ChatGPT **라이브러리 `/LIMITLESS_Art_Delivery/`에 ZIP·PNG2장·512/128/40 비교판·관절 주석·8슬롯도식 총8개 직접 업로드 성공**. 사용자 Windows `F:\Downloads`에 자동 저장한 것 아님. GitHub에는 이 보고 문서만 기록, 바이너리 업로드0.

## 3. 실제 수정과 한계 — 명시적 구분

- **Contact01:** 복부 상단의 높이를 국소적으로 조절하고, 오른쪽을 향한 3/4 두부 각도를 Prepare쪽으로 완화, 전방 다리 어깨/중간 관절/발끝 및 오른쪽 위 지지다리의 연결을 작은 범위로 재배치. 공격 다리를 전신 뒤집기/전신 스케일로 맞추지 않았음.
- **Recover02:** Recover에서 과도하게 높아지는 복부 윗부분을 **국소적으로 낮추고**, 두부 높이와 전방 지지다리 무릎·발끝의 복귀를 조정. 실제 발끝 위치가 지면에 고정되는 애니메이션 물리 검증을 했다는 의미는 아님.
- **정직한 제작 방식:** 기존 원본의 지정 부위만 **부드러운 타원형 국소 변위장 + premultiplied alpha 리샘플링**으로 수정함. RGB/alpha 실제 파일 픽셀이 변했으며 SHA도 서로 다르지만 **수작업 해부학적 8다리 재작화나 IK/발접지 구현을 완료한 것은 아님**. 미술 품질·비례 동일성은 바울 대표의 직접 비교와 후속 선택적 검수가 필요하다. 이 한계를 `JOINT_CONTINUITY_ART_REVIEW`로 유지.
- 기존 게임 `forest_spider` 우향·10FPS·PPU256·Scale0.7 계약은 **참고만 했으며 임포트/실런타임 확인0**. 8슬롯의 Prepare(0)·Contact(3)·Recover(6)는 **설명용 후보 배치**이지 공격 판정이나 타격 시점을 확정한 값이 아님. 실제 타깃/표적 위치 `NOT_VERIFIED`.

## 4. 보호·승인 게이트

- Chapter1·2 승인 공식 몬스터/보스·환경, Wolf/Bear/Fox, Chapter3 Owl·한기 VFX, Chapter4 메카닉 Spider 후보, VenomBee v3 기존 포즈·`RIGHTS_HOLD`, 원본 Unity 6000.6.5f1 프로젝트/Scene/Prefab/MonsterDefinition/GUID/meta·Spawn/AI·Save/UserData/`main` **변경0**.
- **이번 회차 Codex 재검증 요청0**. 기존 Codex #90 좌표 검증 결과만 참조. `main` 직접 커밋·Push·Merge, Unity 본편 적용0.
- **사용자 승인 필요:** 바울 대표의 Contact01·Recover02 512/128/40 시각 검토 → 승인/부분수정/보류. 승인 전 Attack8 나머지 5장·Shoot8 제작 및 본편 교체 금지.

최종 판정: **`TWO_PNGS_REAL_DELTA_PASS / SOURCE_BYTES_PROTECTED / STATIC_ALPHA_HASH_CRC_PASS / JOINT_CONTINUITY_ART_REVIEW_PENDING / FOOT_CONTACT_NOT_VERIFIED / USER_VISUAL_REVIEW_PENDING`**.
