# LIMITLESS / 2026-11-04 / 메인 신규 아트·Chapter1·2 리뉴얼 병렬 작업 발주

**발주 근거:** 바울 대표 “메인 리뉴얼 일 시켜줘.” / 총괄 기획 / 브랜치 `docs/planning`.

## 대상 및 범위
| 담당 | GitHub 발주 이슈 | 시작 상태 | 이번 산출물 | 다음 승인 |
|---|---|---|---|---|
| 메인 신규 아트 (`docs/art-new`) | [#89](https://github.com/HappyPaul812/Project-Limitless/issues/89) | Chapter3 냉기 VFX 기획 [#88](https://github.com/HappyPaul812/Project-Limitless/issues/88) 문서화 완료·아트0 | 한기·동상·동결괴사 각 **대표 VFX 정지 이미지1개, 총3개의 독립 RGBA PNG** + 128/40 동배율 비교판, 오프라인 레이어·연출 설명 | 사용자 **3종 시각 언어 승인** 전 애니메이션 전체 양산/Unity 적용 금지 |
| Ch1·2 리뉴얼 (`docs/art-renewal`) | [#90](https://github.com/HappyPaul812/Project-Limitless/issues/90) | ForestSpider Organic v5 정적·Idle8 원본 보호, Walk8 **USER_ART_APPROVED**/10FPS Clip PASS, Attack8/Shoot8 미제작 | 유기형 숲거미 **Attack 준비·타격·회수 각 핵심1포즈 총3PNG** + 기존 승인 모델/40px 비교판; 8슬롯 지도는 미제작5슬롯 명시 | 사용자 미술 승인 후에만 나머지 Attack8·Shoot8 발주 |

### 기존 승인/보류 보호
- Owl v3 Idle8·Attack4 사용자 미술 승인 및 #84 37PASS/#85 81PASS, #87 원본 Owl 표식 파일 적용 보고 보존. 미승인 한기 런타임/정식 밸런스·보상·Main24 완료/원본 F2 환경 적용 구분.
- F2 재사용 아트 비교판은 바울 대표가 “어제 봤는데 괜찮고”라고 시각 수용했으므로 기존 #75의 시각판단 대기 기록보다 최신 의사 우선, 다만 원본 F2 Scene 배치·Unity 적용은 별도 QA.
- Organic Spider Walk8 승인 ZIP SHA256 `771bd1813608aa3e350c6124ab0d43293c6d2988b1833014da64484d5272c8da`; 정지원화 v5 SHA256 `c600cd2264f6a0323771288666d16b49aa213ad938036d0746ed6f7f1d50adb9`. 기존 표식/Idle/Walk를 다시 제작하지 않음. Walk8 발별 접지 물리계측 `WALK_CONTACT_HOLD` 유지.
- Bee StingRecover v3 ZIP SHA256 `1169aca1c46f4322b85e7247b33a6cbc1fb684e01e0f04fb2ea7f9e9c0194ff0`은 #63 `ART_OK_CANDIDATE / USER_VISUAL_REVIEW_PENDING / RIGHTS_HOLD / TARGET_CONTACT_NOT_VERIFIED / NOT_UNITY_APPLIED`. 이번 #90에서 Bee 재양산·본편 교체 금지.
- Chapter2 공식 몬스터/보스·펫3종, F2 승인 Stump/지면, 기존 스토리·퀘스트·Save/UserData, Unity **6000.6.5f1**/프로젝트 원본 변경0. Main 직접커밋/Push/병합 및 세션 자동실행 없음.

## 실행·보고 계약
1. 담당자는 `START_HERE → COMMON_RULES → 담당문서 → 최신 자신 인계 → 새 이슈` 순서로 확인. 같은 자산 중복 작업 금지.
2. 각자 담당 `docs/art-new` 또는 `docs/art-renewal` 브랜치에 납품 기술보고 Markdown commit, `SPRITE_ART_LATEST.md` / `WORLD_ART_RENEWAL_LATEST.md` 인계 갱신(큰 파일은 증분 인계 + 미갱신 명시), 해당 새 이슈에 SHA·실제 PNG/ZIP·CRC/Manifest·사용자 승인 필요 여부 남김.
3. 각 PNG는 **아트 후보**이며 대표 미술 승인·Codex 격리 Unity Delta QA·제품 원본 적용·main 병합은 별개 게이트. 이미 통과한 QA 전수 반복 금지.
4. 본 기획 세션의 GitHub 이슈 발주는 **각 담당 대화창을 자동으로 실행시키지 않는다**. 바울 대표가 담당 대화에 각 발주 이슈/지시문을 전달하면 실제 제작 시작 가능. Discord AI 자동화/무인 호출은 기존 HOLD 유지.

**판정:** `ISSUE_89_90_CREATED / ART_PRODUCTION_NOT_YET_VERIFIED / USER_ART_REVIEW_GATE`.
