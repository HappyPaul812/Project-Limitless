# LIMITLESS / 2026-11-24 / 신규 아트 #123·리뉴얼 #124 ZIP 실물 기획 검수

## 입력과 실제 검수 출처

바울 대표가 Windows 로컬 파일명으로 납품 통지:
- `F:\Downloads\LIMITLESS_Issue124_AttackButton_Original_20261124.zip`
- `F:\Downloads\LIMITLESS_Issue123_F3_FinalArtReview_F4Concept_20261124.zip`

기획은 대표 PC의 F:를 직접 탐색할 수 없으므로 **ChatGPT Library에서 이름이 완전히 같은 ZIP 실물**을 찾아 컨테이너로 복원, 내부 PNG·문서·Manifest/SHA256SUMS·CRC 직접 점검. 원본 Windows 파일 경로의 실제 바이트와 동일하다는 주장은 **해당 PC 실측 전까지 보류**, 로컬에서 보관된 동일명 아카이브 내용을 근거로 판단.

### #124 공격 버튼
- ZIP `LIMITLESS_Issue124_AttackButton_Original_20261124.zip` **1,659,563 bytes**, SHA256 `14aeea5420912bbfc4c996f108a96db36058f34433324e02321c5de32b1ba9c7`. ZIP CRC 10/10 PASS; Manifest **8/8** 파일 크기+SHA256 PASS; SHA256SUMS **9/9** PASS.
- 게임 후보 PNG `Art/AttackButton_Original_128_RGBA.png` 128×128 RGBA SHA256 `cf775727e27be825809a6b3d9bd04b227abe3408055873f70616cc8a9ca19a32`; (0,0) alpha0, 실제 컬러/알파. Master512 PNG는 아트 원천/검수 자료이고 직접 게임 임포트 후보 아님.
- `Review/Attack_128_40_30_Actual_UI_Comparison.png`와 별도 30/40 픽셀·확대 이미지를 실제 열람. 은색 검날·금색 손잡이·붉은 참격으로 일반 공격 기능을 30/40에서도 구분할 수 있음; 어두운/밝은 오프라인 UI 배경 모두 인지 가능. 작은 30px 붉은 참격이 다소 강조되나 검 실루엣을 해치지 않음.
- **기획 미술 방향 `PLANNING_VISUAL_PASS / REDRAW_NOT_NEEDED`**. 단 외부 Kenney 실물 원본 픽셀 1:1 비교는 제출 자료상 `NOT_VERIFIED`; 정식 Unity 6000.6.5f1 전투 버튼 GameView와 UI 상태(hover/disabled)/색대비 검증은 `NOT_RUN`. 바울 대표 **최종 미술 승인 전까지 `USER_ART_APPROVAL_PENDING / PRODUCT_INTEGRATION_HOLD`**. 기존 BattleUiIconCatalog, 다른 20개 아이콘, Asset GUID/meta, 공격 코드·SFX/장면 변경 금지. 1종 신규 파일럿만 검토한 것으로 나머지21종 완료 주장 금지.
- 담당 제출 [#124](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7116509284) 실제 `docs/art-renewal` 문서SHA `5f95aad00810b817eae70bdb0cf7ab59247973fa`, 인계 `381bb2732dcfddc497c0cbe09deea8cdd83aba54`.

### #123 F3 원본 동배율 비교 + F4 기획
- ZIP `LIMITLESS_Issue123_F3_FinalArtReview_F4Concept_20261124.zip` **1,113,445 bytes**, SHA256 `191d9f52c6c4baba35cb1cd276ffa98573b9c64875baa5db1463320438520098`. CRC 8/8 PASS, Manifest payload **6/6** 파일 크기+SHA256 PASS, SHA256SUMS **7/7** PASS.
- 실제 PNG 3장 검토: `F3_OriginalPixels_1to1_F2F3_FinalArtReview.png`, `F3_AccessibleRoute_OfflinePlacement_NotValidated.png`, `F4_WhiteConifer_BlizzardPalette_ConceptOnly.png`. F2 승인된 그루터기 v4와 **미승인** 울타리/표지 비교 후보의 구분이 명확함.
- 기존 11/23 원본 ZIP `LIMITLESS_Issue123_F3_SnowPass_3Asset_ArtPilot_20261123.zip`을 별도로 Library 실물 회수해 PNG 3종의 원본 SHA256 확인: GroundTransition `d63c8d5ae60921dc7007caf839b720d68dda8078f273b86f1404f6121475a4be`, LowFrostRock `ddbba749c8f60f3b207769c35883aa844e80e782e8dc512077608d2568f07bb1`, FlushStone `ccb99fa51dec1c33d07eaca3fd0fede994b65f0fd5776c98cda690b197ffedc0`. 재제작 없었음.
- **미술 방향 결론 (최종 대표 승인 아님)**:
  - SnowRock GroundTransition: `PLANNING_ART_PASS` **단일 겹침 오버레이 전용**, 타일 반복 베이스로 승인 불가.
  - LowFrostRock Outcrop: `PLANNING_ART_PASS`, 주 통행로 외측의 시각 배경 암반으로 적합; 실제 Collider/Sorting·접지 검증 별도.
  - FlushStone EdgeMarker: `PLANNING_ART_TARGETED_TINT_REVIEW`. 양끝 낮은 돌과 통행 경계 컨셉은 좋지만 푸른 중앙부가 물웅덩이/위험 바닥처럼 오독될 가능성. **돌·눈 외형과 픽셀 크기·배치는 보존하고, 중앙 푸른 부위만 더 낮은 채도 회청색(얼음/그늘색) 후보로 국소 수정한 v2 1개 비교판** 제안. 기존 v1 PNG 바이트 보존. 대표가 기존 색을 선호한다면 기존 v1 유지 가능; 자동 승격 금지. 최소 30/40px 실제 축소, v1/v2 동배율, 다크/라이트 배경 비교와 투명 RGBA·Manifest/SHA 제출. 반복 타일/Collider 구현 금지.
- 오프라인 배치는 중앙 통행로를 비우고 암반 바깥/시각 경계 낮게 배치했다는 **디자인 개념만** 검증, 실제 월드 통행폭 `4.5WU`, 도보·휠체어 collision/Sorting/PPU/Pivot, 본편 F3 Scene/GameView **NOT_VERIFIED**. 기존 Unity 격리 Import3/3 PASS는 과거 별도 결과로만 인용.
- F4 백색 침엽림 기획 `PLANNING_CONCEPT_DIRECTION_PASS`: F3 밝은 고갯길→F4 깊은 회청 수목 그림자·흰 침엽수·방향성 바람, 눈길 대비 유지, F5 균열 보스 분위기 조기 재사용 금지. 기획 Palette `#DCEAF0/#B9CFDC/#91AABB/#4A6A81/#D8C8A6`은 **후보**, 확정 게임용 팔레트가 아님. F4 새 PNG·타일·VFX·Scene/Unity0, 실제 파일럿 제작에는 대표 별도 지시 필요.
- 기존 담당 납품 [#123](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7116170454) `docs/art-new` 문서 `3dd61fa921647b7c6f77dcbaf59e9c3b09505c18`, 인계 `3c1d352a9873ea77fa69b0c302484c4547f840f4`.

## 승인 및 실행 게이트

- `ZIP_QA_PASS_BOTH / ATTACK_PLANNING_ART_PASS / F3_2_PLANNING_PASS_1_TARGETED_VISUAL_REVIEW / F4_CONCEPT_ONLY`.
- 바울 대표 **직접 최종 미술 승인 명시 전** 신규 공격 버튼 Unity 실제 교체, F3 후보 3종 제품 공식 승인/Scene 연결·`main` 병합·Push 금지.
- #85 Codex 패키지·GUID 오류 격리 후보 및 `Unity 6000.6.5f1`, Save/UserData·Discord·승인 자산 보호. Codex에 임의 새 작업 발주 금지. F3 FlushStone 국소 수정은 **메인 신규 아트 담당**에게 별도 보고/판정을 요청하는 것이 적합하며, 수정은 별도 파일·v1 보존 조건.

