# LIMITLESS / 2026-11-04 / 냉기 VFX 99PASS·Chapter3 전투 배경2종·숲거미 Attack8 총괄 기획 검수

**작성:** 총괄 기획 (`docs/planning`)  
**접수:** 바울 대표 2026-11-04 스크린샷(코덱스 12조건·49Unity 화면·자동99PASS·시각 PARTIAL)과 로컬 ZIP 경로2건.
**기준:** [냉기 VFX #89 실제 Unity 보고](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6622135223), [Spider Attack8 신규5 납품 #90](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6622194831), [BattleBG #91](https://github.com/HappyPaul812/Project-Limitless/issues/91), 아트 담당 `docs/art-new` 정식 문서.

## 1. 격리 냉기 VFX QA — 숫자와 시각 판정 분리
- **Unity 6000.6.5f1 / 실제 12조건·49 PNG·자동99PASS/0FAIL**, 원본 Unity/Save/기존 승인 아트·main 변경0. Codex 격리 브랜치 `codex/issue89-v2-unity-visual-delta-20261104`, commit `9c799745755232e0f3b5ef369547ff897cd1547e` (로컬 only).
- **기획 판정 `TECH_PASS / VISUAL_PARTIAL / ART_APPROVED / NOT_ORIGINAL_APPLIED`**. 자동PASS는 크기/캡처 검증이며 최종 시각/접근성 PASS가 아니다.
- **40px:** 휠체어 바퀴의 청색 장식+VFX 한기 원형 고리가 섞이며 동상/동결괴사도 비슷한 파란 덩어리. 색만으로 해결하지 말 것. 실제 상태명/서로 다른 아이콘/2턴·1/3·2/3·위험 문구를 별도 명확 표시하고 작은 상태 VFX의 **위치/점유를 줄이는 배치 제안**만.
- **128px:** 동상/동결괴사가 하체와 휠체어를 크게 가리는 문제. **승인 원본 PNG 바이트 보존**, 렌더 위치·크기/마스크/투명도 등 **런타임 표시 파라미터 실험**만 다음 게이트로 권고. 다른 플레이어 이동형태/기기/다인파티 미시험.
- **HUD:** 960에서 상단 배지 밀집; 1680 전장 상단과 기존 HUD 중첩 이슈는 기존 문제와 신규 VFX 결함 구분. 스킬 2×3/HP·MP/선택 외곽선/얼굴은 기본 시험에서 보호.
- **다음 게이트:** 1회 소량 **2~3개 표시 레이아웃 실험**을 계획(예: 40px 작은 원샷 위치, 128px 하체에서 가장자리로 옮김, 상단 배지는 960폭에서 줄바꿈 또는 그룹분리). 실제 적용과 정식 PPU/Scale·아이콘 이미지/상태 해제 로직은 별도 승인. 기존12조건/99PASS 전수 반복 금지.

## 2. Ch3 전투 배경 2종 — 실제 Library ZIP 검사
- Windows 사용자 경로: `F:\Downloads\Limitless_Chapter3_BattleBackground_2Pilot_20261104.zip`. **실제 기획 검사 바이트는 ChatGPT Library 사본**; Windows F: 해시는 `NOT_VERIFIED`.
- **전체 ZIP SHA256 `a8c02e9ab6189a6feb074d3f83c8cd6a8bce61ff1b8de110e7c8216934468eb9`**, 1,600,528 bytes, 12 entries, ZIP CRC PASS, SHA256SUMS11/11, Manifest9/9 bytes+SHA PASS.
- 두 신규 **1672×941 RGB 불투명**: `C3_BattleBG_FrostwaySnowroad_v1_CANDIDATE_1672x941.png` SHA `a5f3bb960b5c5e6fe4140cb2de64e726f4bc7fabb8674f895789fed128adcd25` / `C3_BattleBG_FirstSnowForest_F2_v1_CANDIDATE_1672x941.png` SHA `93388ee52fd75da4a82df0b68e03b2083167a25f339d97b41dc71c807125282b`. 후보들은 별도 플레이어·UI를 굽지 않은 순수 배경. 2000×870 비교/960×540 UI 대체 목업은 **OFFLINE**, 실 Unity 화면·정식 UI/캐릭터 아님.
- 직접 본 배경 2장과 비교판: 설원=열린 설산/눈길, F2=숲 테두리/첫눈 길. 전투 슬롯 중앙/하단이 비교적 비어 있음. **도형·절차적 픽셀 표현이 단순해 기존 상세 몬스터 및 Ch1 전투 배경과의 화풍 일치 미검증.** 화풍 일치라며 최종 미술승인·적용 불가. 디자인 구조는 적절하므로 **`CONCEPT_OK / USER_VISUAL_REVIEW_PENDING / UNITY_NOT_VERIFIED`**.
- 기존 Ch1·Ch2 사전 자산 감사(납품 ZIP Docs): **Ch1 실제 원격6 PNG**; 카탈로그 참조 고유4종 (Grassland/Forest/Catacombs/Boss). ToxicSwamp/AncientSanctuary는 보유하지만 해당 카탈로그 연결 미확인. **Ch2 전용 BattleBG 폴더·카탈로그는 원격 main 미발견**; `BattleBackgroundCatalog.Find()`가 Main20 DeepCore_Ground_Base 지면을 재사용하며 기타 상황은 기본 그라데이션/지면 전장 경로를 탈 수 있음. 사용자 F: 로컬에 다른 Ch2 전용 BG가 있는지는 `NOT_VERIFIED`; 과거 “Chapter1·2 다 있음” 단정 정정.
- `docs/art-new` 제작 commit `580e55d9ebad719a56bbc15da0215b3224b4cea2`, [정식 납품/감사](https://github.com/HappyPaul812/Project-Limitless/blob/docs/art-new/문서/00_프로젝트/Chapter3_BattleBG_InitialPilot_Audit_20261104.md).
- 후속: **#91**에서 기존 Ch1 실제 BattleBG PNG와 동일 축척 화풍 비교판 1장, Ch2 PC 원본 Scene/Catalog/GUID read-only 감사, 새 두 PNG 원본 보존. 바울 대표 최종 배경 미술 승인 이후 격리 Unity6000.6.5f1 전장 HUD·Party3·Owl·3해상도/비율 최소 시각 검증. 원본 적용 자동 승인X. Chapter2 전용 배경 새로 발주하는 결정도 감사 이후 별도.

## 3. 숲거미 Attack8 8장 — 실제 Library ZIP 검사
- 사용자 Windows 경로 `F:\Downloads\LIMITLESS_SPIDER_ATTACK8_ISSUE90_20261104.zip`. 검사 바이트는 ChatGPT Library 사본. Windows F: SHA 미검증.
- ZIP SHA256 `e612a88a6d4c6b4422cc025c0d32fb34e4ffc2f40337ffa909a1885cfae34041`, 5,922,807 bytes, 23 entries, CRC PASS, SHA256SUMS21/21, Manifest21/21 PASS.
- **실제 512² 투명 RGBA 8개** PNG, 4×2 시트2048×1024; 40·128px GIF **8장×100ms=10FPS**, 고유 SHA8/8, 이미지 네 변 알파 접촉0.
- 승인 Prepare00=slot00 SHA`439ef26e4528e9f57877b471f602f9cfaa2f7bcd2a86b2fce991ec2e17ae5bf2` / Contact01v2=slot03 SHA`0d0e24c2fae579f2f944255fe3bd3841216299b14f439cc3612906269020f697` / Recover02v2=slot06 SHA`0501a452467a07f34688abdfff5ea5ef49b0dfd93e1c99bdc368229986d44096` **원본 바이트 exact KEEP**.
- 신규 01Windup·02Lunge·04Recoil·05Return·07IdleBridge는 **광학흐름 기반 보간 파생 5장**. 파일 생성/차이 및 10FPS 재생은 기술 PASS, 하지만 독립 8다리 각 관절의 수작업 동작·발 접지 고정·타깃 접촉·실 10FPS Unity Clip은 미검증. **8PNG TECH_PASS ≠ MOTION_ART_FINAL_PASS**. 신규 5장에 사용자 미술 승인 필요.
- 8프레임 전체 이미지 직접 검토: 기본 우향 모델·앞다리 공격/복귀 흐름은 일관되나 2·4프레임 이후 몸통/다리 길이/방향의 보간 흔적과 시퀀스의 지지발 미끄러짐은 실제 재생 판정 필요. 물리 접지 PASS 선언 금지.
- `docs/art-renewal` 보고 commit `d22bec7e19b0cd986ba46484ba18c7fa111c97c1` / 증분 인계 commit `48efcf1052502162797d8568dffc41ecf047d99b`.
- **기획 판정 `ATTACK8_OFFLINE_FILE_TECH_PASS / NEW_5_USER_ART_REVIEW_PENDING / TARGET_CONTACT_NOT_VERIFIED / NOT_UNITY_APPLIED`**. 기존 승인 3장·Idle8/Walk8 절대 재작화 금지, Shoot8 및 Bee v3 RIGHTS_HOLD 유지. 신규 5장만 **10FPS 실제 애니메이션 연속·40px/128px·관절·발 위치·표적접촉**의 표적 검증 후 사용자 최종 미술 판정.

## 4. 협업 지시 경계 및 공통 보호
- 요청(기획 기록 및 이슈 글)은 실제 Codex/아트 담당 창을 자동 실행하지 않는다. 별도 담당 대화에 Issue 링크 또는 지시문 전달 필요.
- **중복 QA 금지:** #89의99PASS, #88 63PASS, #84 37PASS, #85 81PASS, #87 36/25PASS 등 재실행하지 않는다. 수정사항 발생 시만 최소 델타.
- **보호:** Unity 6000.6.5f1 원본, 개인 Save/UserData, 승인 Owl·F2/Frostway 환경·거미v5/Idle8/Walk8/Attack3, Chapter1 배경6/Ch2 Main20지면, 원본 Scene/Prefab/.meta/BuildSettings, main 직접 커밋·원격 Push·merge 금지. 작업 브랜치 기록 의무.
