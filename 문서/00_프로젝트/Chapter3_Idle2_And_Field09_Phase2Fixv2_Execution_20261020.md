# LIMITLESS — 2026-10-20 Chapter3 Idle2 본제작 + Field09 Phase2 수정 v2 후속검수

- 기획 담당 브랜치: `docs/planning`.
- 바울 대표가 제공한 **Codex Chapter3 Idle 제작 기술 계약 스크린샷**과 **Field09 Phase2 리뉴얼 ZIP 로컬 경로**, 최신 `docs/art-new` 기술 선행조사·`docs/art-renewal` 수정 납품 문서 기반 실행 인계.
- 타 담당 세션에 메시지가 자동 전송된 것으로 주장하지 않는다. 아래 작업은 각각 담당 세션으로 1회만 전달, 이미 해당 작업을 시작했다면 중복 지시하지 않는다.

## A. Chapter3 신규 몬스터/보스 Idle 본제작 계약

- **바울 대표 1포즈 v2 승인**: 서리들쥐 A `Sprites/FrostFieldMouse_A_1Pose_Rework_v2.png` SHA256 `0516914ddad36af5854065d66d01b9664329aa89eea8b394480eed6af92e32c2`, 설빙의 폭군 B `Sprites/FrostTyrant_B_1Pose_Rework_v2.png` SHA256 `f1a3c5f91a5fef69cd881e27b578c32253d0124d5d8628a8ca748f7479333830`. 원본 ZIP `F:\Downloads\Limitless_Chapter3_ApprovedAB_1Pose_ArtRework_v2.zip`, SHA256 `0b1a3711bed9616b828c83e51070f35ed161e2d8a4d93de9cb949d318dc46919`.
- **새 Codex 최종 로컬 Idle 기술 계약**: 브랜치 `codex/chapter3-ab-idle-contract-20261020`, 계약 commit `b1793f85`, 최종 인계 `cfed20a8`. 사용자 스크린샷 보고 static technical PASS. 1포즈 정본2 SHA 동일, 로컬 승인 원본3,546 / Save / UserData / Editor 보호. Unity Import/Animation 제작/Push/main 병합0.
- **최종 계약이 과거 art-new 사전조사보다 우선**: `docs/art-new/문서/00_프로젝트/Chapter3_ApprovedAB_Idle_Animation_Technical_Preflight_20261020.md`는 Codex 계약 이전에 **314px 정규 Chapter2 셀을 검토한 PRE-FLIGHT**일 뿐이다. 그 문서의 `1256×314`, `PPU314`를 현재 최종 계약과 혼용해선 안 됨.
- **두 캐릭터 모두**: Idle 전용 1행 4프레임, Sheet **2048×512** (4×1), Cell **512×512**. Sprite Import **PPU512**, **Pivot bottom-center (0.5,0)**, 오른쪽 우향 1방향, **8 FPS**, 4프레임 반복 **0.5초**. 원본 프레임 512화폭은 4프레임 각각 좌→우 00,01,02,03. 애니메이션에서 **발 기준선/움직이지 않는 접지 Anchor**를 유지하고 루프 03→00 끊김을 피할 것. 엔진의 명시적 Sprite Frame 재생 재사용: 새 Animator/Monster/Skill 코드 구현 자동 불허.
- 크기 계약: 서리들쥐 `VisualScale=1`, 실제 전투 체감 **약 102×80 UI px**; 폭군 `VisualScale=3`, 실제 체감 **약 307×305 UI px**. 위 값은 **투명 알파 실제 실효 크기 추정**이며 프레임 캔버스 512 전체 UI rect를 의미하지 않음. Codex가 승인 원화의 alpha bbox/Aspect에서 산출한 계약에 맞게 새 4프레임 전체에서 체형과 크기 안정성을 지킨다. 임의 314px 강제 축소/PPU314 및 큰 실루엣 잘림 금지.
- 신규 아트 담당 `docs/art-new` 첫 제작: 서리들쥐 A Idle sheet1, 폭군 B Idle sheet1의 **실제 투명 PNG 2개**, 필요하면 프레임별 PNG4장씩 별도, 저용량 루프 비교 미리보기/Frame Timeline/Manifest 각 Sprite SHA/ZIP CRC/testzip/무결성 검사. 승인 1포즈 원본의 디자인·색감/해부학·꼬리·뿔/발톱 품질을 유지하되 **숨/배·어깨·털/수염 등 소폭 움직임** 구현. 단순 4장 동일 복제는 실제 애니메이션 납품 불합격, 전체 프레임 뒤집기·위아래 통째 이동/신체 잘림·지면 미끄럼 금지.
- 근원 PNG 원본은 바이트 변경하지 않고, v1 예전 마스코트·어설픈 고정 실루엣으로 회귀하지 말 것. F1/마을 환경10종/Chapter2 리뉴얼 영역 보호. **Attack/Hit/Defeat, 다방향, 다른 몬스터5종, 신규 전투/Unity Implementation** 전부 이번 발주 아님.
- 실제 시트 파일 납품→Codex **그 시트2만** 독립 Unity Import/단순 반복 시각 QA→바울 대표 모션 미술 승인 순서. 원본 main/Unity 정식 적용/Push는 별도 승인.

## B. Field09 Phase2 Pilot Fix v2: Codex 두 소품 격리 실제 GameView 재검수

- 로컬 ZIP `F:\Downloads\Limitless_Field09_Redesign_Phase2_Pilot_Fix_v2.zip` 리뉴얼 납품 보고 GitHub `docs/art-renewal/문서/00_프로젝트/Field09_Phase2_Pilot_v1_VisualFail_CliffFissure_ArtFix_v2_Delivery_20261020.md`. 리뉴얼 담당 `docs/art-renewal` 커밋 `6fb4701d`, 최신 인계 `ef2ada26`.
- 납품 ZIP **13 entries / 3,602,303 bytes / SHA256 `0e4b7bfb3784b5b97a4a719c669f9564f502d67e88c2096d32475fa69a6876c8`** (아트 담당 독립 오프라인 PASS 보고, Codex의 로컬 ZIP 재검증 전).
- 후보 `LL_Field09_Phase2_Cliff_Blocker_v2.png` SHA256 `ad995f3cc4914759d6c88fa47c80227cb1a7c5f1a9a549679e06472402fee9f1` (v1→v2 변경 13,918px, 하단 회백/홍흑 매트 문제), `LL_Field09_Phase2_Fissure_Edge_v2.png` SHA256 `c3dc0c131c11fef72e4d210a1a43528f9882d8c50486e8da0bc15b6984ea145a` (14,574px 변경, 외곽 매트·Glow 붉은 띠 문제).
- **중요 미해결 기술 가능성**: 현재 `ObsidianScar_Glow`의 sortingOrder **-7**은 `ObsidianScar_Fissure` **-8**보다 앞에서 그려짐. 아트 담당이 Alpha 매트만 제거해도 기존 Glow의 붉은 형태가 외곽에 드러날 수 있음. 정상 시각이 아니면 **픽셀 재수정만 반복하지 말고 SpriteRenderer Sorting/Mask/Material/Glow 결합 자체 문제**인지 직접 검사해 보고. 기존 Glow 이미지를 임의 수정하거나 임시 불투명판으로 가리는 금지.
- Codex의 안전한 격리 worktree에서 신규 PNG2를 Import하되 기존 Base/CrackedOverlay v2/Slab 승인이 유지된 상태, Glow 파일/Importer/Sorting·Collider·퀘스트·과열/Save/UserData/원본 Editor 불변. 원래 `ObsidianScar_Cliff` (-9,0) 및 `ObsidianScar_Fissure` (-5,3.8) 2개 Renderer만 시트 후보 교체, **동일 3 GameView 화면비 전/후 시각 Delta**. Cliff 하단의 매트/접지감0, Fissure 외곽 잔재0, **현행 Glow + Edge 결합 붉은 띠0**, Main18 과열/조사 가독성 유지.
- 기존 QA `Phase2 v1 30PASS`, `Sample3 17PASS`, Main18 기능 `92PASS` 전수 재실행 금지. 새 후보만 검사·위험 위치 bounding box/스크린샷/기술 또는 아트 원인 분리 보고.
- 완료 판정: `PNG_ZIP_TECH_PASS`와 `UNITY_GAMEVIEW_VISUAL_PASS`를 별도 표기. Glow 돌출이 남으면 해당 항목 **FAIL**, 왜 Edge 재드로잉만으로 해결 불가한지 정확 근거/대안(Glow 수정 허가 필요? Sorting/Mask 해법?) 보고하고 사용자 승인 받기 전 원본 변경 금지.
- 최종 시각 PASS되어도 바울 대표 Phase2 2종 미술 승인·나머지 소품 제작 승인과 원본 Unity 정식 적용/main 병합은 별개.

## 담당별 지금 실행/보류

- **지금 병렬 허용**: `docs/art-new`의 Chapter3 Idle sheet2 제작; Codex의 Field09 Phase2 v2 격리 QA. 두 작업의 자산·worktree/Branch/Commit/SHA 보고 구분.
- **대기**: Field09 `docs/art-renewal` 신규 원화 추가 제작(코덱스 결과 전까지), BGM 신규 곡 추가 제작, TTS/SFX/영상, Chapter3 Attack/Hit/Defeat, Chapter3 Main22~29 Unity 구현·전체 메인 병합. 기존 BGM 두 데모곡 승인 여부가 이미 바울 대표 구두 긍정이 있었으나 Loop 재편집 게이트 미완료라면 BGM 담당 기존 작업 계약은 보호.
