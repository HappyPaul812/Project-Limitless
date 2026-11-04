# LIMITLESS / 2026-11-04 / 냉기 상태 아이콘·Chapter3 전투 배경 v2 최소 후속 기획

**담당:** 총괄 기획 / `docs/planning`. 바울 대표 전달: #90 사용자 Attack8 최종 미술승인, #91 `ch3_bg_v2_easy.zip` 검수, #89 B배치/아이콘 40px 실화면 재검토. `main`·Unity 원본·Save 변경 없음.

## 현재 결정과 근거
- [#90](https://github.com/HappyPaul812/Project-Limitless/issues/90): 승인 Attack8 **8/8**, 승인 SHA 자산 동결; 격리 Unity6000.6.5f1 16PASS/0FAIL. Shoot8·본편 Unity 반영/타격접점 별도. **새 리뉴얼 아트 작업 발주하지 않음**.
- [#89](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6624594309): B안(40px 8px간격, 128px 본체 뒤/아래24px, 960 HUD두행, 1680 HUD경계) **격리 50PASS / 적용전 후보**. 상태명·턴·중첩·위험 경고로 정지 캡처 식별 가능. 밝은 설원 단독 연무는 약하고 기존 임시 VFX아이콘 HUD14UI≈12.12화면px에서 3종 식별 어려움. `OFFICIAL_ICON_PENDING / VISUAL_PARTIAL / ORIGINAL_NOT_APPLIED`.
- [#91](https://github.com/HappyPaul812/Project-Limitless/issues/91#issuecomment-6624731902): `ch3_bg_v2_easy.zip` **CRC PASS·SUMS4/4**, 원본 그림 Snowroad/F2 v2 SHA가 정식24항목 ZIP 보고 PNG와 **일치**. Easy ZIP에는 Manifest·Ch1/v2직접비교판 없음(누락을 통과로 쓰지 않음). 동봉 전후판과 적왼쪽·아군오른쪽 **오프라인 목업**은 실 BattleUI/Unity 근거 아님. `USER_VISUAL_REVIEW_PENDING / NOT_UNITY_APPLIED`.

## 다음 작업 — 새 그림 중복 제작 금지
### #89 메인 신규 아트: 정식 냉기 상태 아이콘 3종 소량 파일럿
- **한기:** 작은 눈송이/바람형, **동상:** 인체에 붙은 갈라진 얼음 결정형, **동결괴사:** 검푸른 큰 균열/쐐기형. 3종의 **형태**가 달라야 하며 색만 바꾸지 말 것.
- 실제 HUD가 **14UI(960×720에서 약12px)**로 보이는 점을 참고해 12/14/20/40px 축소판과 설원/어두운 배경 동배율 비교. 최종 원본 후보는 각1 독립 RGBA PNG(파일해상도/PPU는 기존 UI Sprite 계약을 읽어 권고, 임의 공식 고정 금지).
- 상태명·턴·1/3·2/3과 위험문구를 그림에 굽지 않음. 동결괴사 지속3/3 없음. 접근성/점멸·가림 금지. 기존 승인 냉기 VFX3 PNG·B배치 코드·Owl원화 변경0.
- 아트 사용자 최종 시각 승인 전 새 아이콘 본편 적용 금지. `docs/art-new` 보고·최신 인계·#89 댓글에 SHA·실물 비교판/결과.

### #91 Codex: Chapter1 실제 배경 vs Chapter3 v2 1:1 증거 보충
- 이미 F:에 있는 Ch1 Forest/Grassland/Catacombs 3PNG, `ch3_bg_v2_easy.zip`의 **동일 바이트 v2 Snowroad/F2 2PNG**, 원본 1672×941 동일 크기를 재사용하여 **실제 Ch1-v2 1:1 화풍 비교판 1개**만 작성. Chapter1-v1 비교판을 v2 검증으로 재사용했다고 주장 금지.
- 기존 Easy ZIP은 재포장·재해시·재제작하지 않음. Manifest 없는 간편 ZIP과 **정식 납품 ZIP의 보고된 Manifest/CRC 자료를 구분**. 사용자가 최종 화면을 평가할 수 있도록 공식 적왼쪽/아군오른쪽의 실제 전투 UI/실 Sprite 참조와 합성 목업을 엄격히 구별.
- Codex는 기존 Unity 실화면이 있으면 **읽기 전용 재사용**; 새 960/1280/1680 게임 QA·원본 적용은 *미술 승인 이후* 별도. v2 추가 재작화 지시X.
- 산출: 비교 PNG의 실제 위치·SHA, 원본 5PNG SHA 보호, Ch1-v2 스타일 차이/아군 후열 x=.86과 오른쪽 수간 가독성 판단, #91 코멘트. 이후 바울 대표 시각 승인 시 격리 DeltaQA 가능.

## 보호·보고
Unity **6000.6.5f1**, 기존 Owl, Spider Attack8 승인8/8, 냉기 VFX3, Ch3 배경 v1/v2, Ch1 공식배경·개인 Save/UserData·사용자 미커밋 원본 보호. `main` 직접 commit, 원격 Push/merge 없음. Issue 지시 게시만으로 다른 PC Codex나 아트 담당 세션이 자동 시작되지는 않음.
