# LIMITLESS — 2026-10-21 서릿길 마을 GameArt v3·Wolf Run6 v4 합동 QA: 기술 PASS / 미술 FAIL / 표적보완

## 실증 근거와 승인경계

바울 대표 제공 Codex 검수 화면. `codex/qa-frostway-v3-wolf-run6-v4-20261021`, 최종 QA 커밋 `695f6f74e63996bb9d0ba0561dc6518f1e7af049` (사용자 보고).

**A 서릿길 마을 v3** 실제 사용자 ZIP SHA256 `2a74677d5f62d7e13e64ea85ba62e12175be0be0e0d19864c23233f1bd84e79a` = `docs/art-new` 납품 보고 동일, CRC/Manifest/PNG43 기술 PASS. 격리 Unity 16:9·4:3·21:9 **실제 카메라/GameView 조립·통행 검증** 완료(도보·휠체어 비교는 0.5WU legacy collider 조건), 단 **마을 전체 Scene·퀘스트·서비스 실제 구현 완료 아님**. 미술 **FAIL**: 이웃 원화 잘림/찌꺼기 조각들이 **`Frostway_SkillShop_Building.png` x=12..29/y=196..376**, **`Frostway_Roof_Gable.png` x=100..144/y=183..207**에 잔류. 타일 4종 픽셀 경계 기술 PASS이나 반복무늬 시각 **HOLD**. 미술 최종승인/본게임 적용 금지.

**B Wolf Run6 v4** 실제 ZIP SHA256 `3ab0b57d2389b55ed33f73c1d3d13862b9b595245e196d1f3a4c98f090b8a412` = `docs/art-renewal` 납품 보고 동일, CRC/Manifest/6 Pose·Strip·Atlas 기술 PASS. Run00~03 PNG SHA 보존, 04/05 새 변경량 확인. **격리 Unity 실제 12FPS PlayMode** 재생됨. 다만 `Wolf_Run_05_v4.png`의 **x=315..338/y=235..280** 후방 다리 윤곽이 직각처럼 잘리고 부자연스러움 → **미술 FAIL**, 대표 Run 모션 최종승인 아직 없음. 공식 기존 Wolf 컴패니언은 교체하지 않았음.

두 ZIP 이번 배포는 **ZIP 바이트 동일성 PASS**: 과거 v2/v3 SHA mismatch를 이번 새 납품에 재적용하거나 추가 재압축으로 문제 만들지 말 것. 사용자 PC `F:\Downloads\` 새 파일 수정/이동/삭제 금지. 사용자 원본 4,351파일 SHA·Git·Editor·Save/UserData 보호, main 커밋/Push/병합 없음.

## 새 제작지시 A — `docs/art-new` 서릿길 마을 v4

1. **단 두 자산의 이웃 원화 잔여 제거**: `Frostway_SkillShop_Building.png` 위 픽셀 사각형, `Frostway_Roof_Gable.png` 위 픽셀 사각형을 확대 확인 후 건물 실제 외곽·지붕·문·명암·안티앨리어싱과 구분하여 외부 누출 조각만 Alpha 복구. 통째로 무조건 투명 사각형 마스킹하여 정상 지붕/벽을 깎는 임시방편 금지. 픽셀 범위는 **Codex 관찰 위치**, 전체 마스크 범위 강제가 아님.
2. 기존 GameArt v3 **43 PNG 중 위 2종을 제외한 41개는 바이트 SHA 불변**. 성공한 Tile edge/세 개 화면비/타 독립 sprite·기존 F1 승인6 및 v2 콘셉트11 모두 그대로 KEEP.
3. 별도 **반복무늬 HOLD**는 근거 자료만 추려 `Tiles/` 실제 4개 지면의 3×3 동일 배율 접합 보드에서 패턴이 뚜렷한 파일/좌표를 pinpoint하라. 확실한 파일/문양은 **변경 허가 전 user 미술 결정** 또는 엄격히 좁힌 v4 지면 최소 질감교정으로 별도 구분; 이 검수만으로 모든 타일을 임의 재그리거나 tile 시각PASS 주장 금지. 만약 패턴이 게임화의 핵심 장애이면 고립된 일부 패턴 변경만 제안하고 original tile 패턴/색·edge cell seam 유지.
4. 신규 `Limitless_Chapter3_FrostwayVillage_Environment_GameArt_TargetedFix_v4.zip` 실물 ZIP; 예전43 PNG 배열/경로/크기/Manifest/QA3화면비는 유지, 수정2 PNG 각각 before/after 2x alpha + screen-scale 비교/CRC/per-PNG SHA/ZIP actual SHA, 타일 반복패턴 HOLD 목록. Unity Scene/Main23 Quest/상점 시스템·main 직접 적용 금지. 정식 사용자 미술승인 및 Codex 게임용 v4 DeltaQA 별도.

## 새 제작지시 B — `docs/art-renewal` Wolf Run6 v5

1. 기존 v4 Pose `00..04` PNG **SHA 그대로**, 복원한 03/04 꼬리·04 뒷다리와 승인 Wolf v5 정적 KeyArt 절대 수정 금지.
2. **05 프레임만** 고쳐 실제 뒷다리의 관절부터 발끝까지 해부학·발 접지 위치/무릎·발목 곡선이 매끈하게 이어지게 한다. 표적은 `Wolf_Run_05_v4.png` x315..338/y235..280 부근 **잘린 직각형 윤곽**. 단순 알파로 잘라 제거하거나 다른 프레임 조각을 적당히 붙여 자연스러운 관절이 아닌 경우 불합격. 필요하면 해당 포즈의 뒷다리 부분만 **실제로 새로 그리기**, 05 머리·몸/털/길이·앞다리·꼬리 및 전체 기존 스타일 보호.
3. v4 05→00 간 접지/루프 연결은 새05로 12FPS 6프레임/0.5sec에서 부드럽게 보일 것. 프레임04/이전 승인 꼬리·중간 루프가 악화되면 PASS로 조작하지 말고 명시. 6poses 512×384, LEFT, Strip3072×384/Atlas1536×768, 128/40px preview GIF/APNG, Foot anchor/alpha/변경좌표 & frame05 v4-v5 2x/12FPS 움직이는 실제 아트 미리보기, SHA/CRC/Manifest/ZIP byte SHA.
4. 신규 `Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtRework_v5.zip`. **고해상 승인 KeyArt v5**(정지 원화)와 **Wolf Run6 v5**(새 동작 후보)를 구분, 'Wolf v5' 한 단어로 혼동하지 않음. 기존 런타임 Wolf_Guid/BeastCompanion/Player Scene 교체·Bear/Fox 제작·main 직접 Push/merge 금지. 실제 격리 Unity Run6 v5 DeltaQA와 대표 모션 승인 후만 정식 연결.

## 다음 실제 검수

두 신규 ZIP 실제 납품 시 Codex **단일 격리 델타QA**: 전체 본편 QA 재실행 없음. A village 2 modified SHA, 41 KEEP SHA, Sprite clipping 수정/타일 패턴별 HOLD, 3화면비 짧은 재확인; B Wolf 05 수정/00..04 byte identity, SHA, 실제 12FPS 접지/05→00 loop. 종료 보고는 file TECH vs ART vs Unity-Runtime vs user final decision 구분.

기존 Main22 Unity 격리 136PASS, F1 64PASS, Mouse Battle40PASS 및 Main23 기술 프리플라이트는 다시 진행하지 않음. Main24 skill6 슬롯 persistence Issue #46, 아이콘/잡템/소모템 누락은 별도 내부 backlog.
