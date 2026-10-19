# LIMITLESS / Field09 v1 시각 FAIL + 독사·늑대 고품질 원화 실제 납품 우선 지시 (2026-10-19)

- 작성: 기획 `docs/planning`, 실행 담당: 리뉴얼 `docs/art-renewal`, Unity 검수: Codex.
- 바울 대표: "코덱스는 잘했고 리뉴얼 담당은 논다? 리뉴얼 보고서 봐봐."
- 증거: 바울 대표 Codex 화면 보고(2026-10-19), 리뉴얼 `Monster_Wolf_Chapter2_Quality_Gate_Correction_20261019.md`, `WORLD_ART_RENEWAL_LATEST.md`, Field09 v1 아트 납품 기록.
- **정확한 상태:** `FIELD09_ZIP_REPACK_PASS / FIELD09_ISOLATED_TECH_QA_40_PASS_0_FAIL / MAIN18_DELTA_92_PASS_0_FAIL / FIELD09_USER_VISIBLE_ART_VISUAL_FAIL / FIELD09_ORIGINAL_APPLY_BLOCKED / VENOMSNAKE_V2_ART_HOLD / WOLF_V2_ART_HOLD / OFFICIAL_CHAPTER2_COMPARE_NOT_VERIFIED / KEYART_2_PNG_NOT_DELIVERED`.
- 본 문서 자체는 PNG/ZIP 생성 또는 Unity 원본 편집·Push가 아니다. 기존 공식 PNG/meta/GUID/Save/UserData 및 Main09·10 음성/Field06·07 적용 승인 자료 보존.

## 1. 실제 상태 확정 — 리뉴얼이 먼저 제작해야 함

최신 `docs/art-renewal` 커밋 `3f3002cca53c080bfe7b0f023a3219b5dddb4332`과 `b3dd5cb721d46`는 **정정·인계 문서**. 직전 리뉴얼 담당이 만든 "LIMITLESS Monster Concept Art" 장식형 비교판은 실제 Chapter2 공식 5종 PNG/동일 월드크기 실제 비교가 **아니었음**을 본인이 정정했다. 현재 대표가 요구한 **독사 독립 고품질 Key Art 1장 / 늑대 독립 고품질 Key Art 1장 / 실물 공식 5종 기준 비교**는 아직 정식 납품되지 않았다. "분석·문서화 완료"를 "아트 완성"으로 주장 금지.

## 2. 작업 A: Field09 실제 화면 FAIL, 미술 담당이 픽셀 재수정

Codex `codex/field09-minimal-fix-unity-integration-20261019` 보고:
- 기존 손상 ZIP 재포장 성공(내부 PNG8과 문서3개 byte 동일), 정상 ZIP 새 SHA256 `6d3d866e75c696d451919d80789a89f12b429ac72f22d8e3f245a91bfc43a012`. Python/Windows Explorer/.NET 실제 해제·CRC·Manifest PASS.
- 격리 Unity 기술 `40PASS/0FAIL`, Main18 delta `92PASS/0FAIL`; **시각 FAIL: 지면 반복띠 회색6개 및 소품 6종 하얀 가이드선 잔존**. **원본 적용0**. ZIP만 다시 포장하는 것은 이미 해결했으므로 반복 재포장만 해서는 해결 불가.
- **리뉴얼 아트 담당 실행**: Codex의 격리 Unity Before/After 및 Visual FAIL 위치/이미지/좌표를 우선 읽어 오브젝트별 결함을 매핑한다. `ObsidianScar_Ground_Base.png`와 `ObsidianScar_Ground_Cracked_01.png`의 Tiled 반복/두 불투명 Material Overlay 교차부를 *실제 게임 화면에서* 이음이 없도록 픽셀/색/알파 최소 정정. `ObsidianScar_Cliff_Blocker_01.png`, `ObsidianScar_Fissure_Edge.png`, `ObsidianScar_Fissure_Glow.png`, `ObsidianScar_HeatVent_01.png`, `ObsidianScar_Obsidian_Slab_01.png`, `ObsidianScar_Obsidian_Spire_01.png`의 **흰 가이드선 실제 잔존 픽셀만** 제거. 기존 v1 자동 저alpha 추출에만 의존하지 말고 화면에서 보이는 선에 맞춰 RGBA, 항목별 bbox/변경픽셀, 반복/크랙 Overlay 의도된 디자인 보존 점검.
- 수정이 필요한 8개 PNG만 **별도 `Limitless_Field09_ObsidianScar_Minimal_Fix_v2.zip`** 납품. 원본 정식 PNG10과 이전 v1 후보, 양쪽 메타/GUID 보존. PNG9~10(AshPatch, 미사용 GroundCracked02) 수정 금지. 색감 전체 변경·새 지도 제작·가짜 Ground 덮개/Scene collider 임의 조정 금지.
- 정확한 PNG bytes와 Manifest SHA/CRC 확인, `Changed_Pixels.csv` 및 Before/After 실제 지표, 기존 QA vs 예상 게이트, Codex Import/Unity deltaQA 인계. **표준 Python zipfile로 ZIP 자체를 실제 testzip/해제·CRC 확인**해 이전 v1 ZIP 헤더 사고 반복 금지. Unity의 *실제 최종 합격*은 Codex 담당이며 합성 Preview를 실제 Unity PASS로 표시 금지.
- 원본 Unity 적용은 Codex가 v2 SHA·GUID 안전성·가시 잔여선0·과열/퀘스트/Save 국소QA 합격 후만, 원본 사용자 Editor/Save 보호; 재적용 전 기존 `Main18 500PASS` 전체 재반복 금지.

## 3. 작업 B: 맹독뱀 + Wolf 실제 고품질 2개 원화 제작

- 바울 대표는 v2의 코드 생성 도트 도형 방식에 **미술 품질 불만**을 제기. 이미 확보된 `11_SootHound`, `12_HeatwindHawk`, `13_FissureLizard`, `14_EmberBeetle`, `15_EmberWraith` 공식 PNG/meta/단일 프레임 비주얼을 *실물 그대로 참고* (복제 금지), 실제 World Size와 카메라 Scale을 비교 가능한 기준으로 사용한다. 기존 실물 소스에 접근 못 했으면 추측 비교 보드 생성하지 말고 Codex/공식 GitHub에서 정확한 자료 확보.
- 결과물 **두 장**: (1) 위협적인 독사 `VenomSnake_KeyArt_v3.png` 1장 (독니·날카로운 머리·독비늘·빛/그림자·몸통 S자 긴장), (2) 늑대 `Wolf_KeyArt_v3.png` 1장 (실제 야수 비례·긴 주둥이·근육·털의 명암·사수 동료감). **각각 완성도 있는 높은 해상도 신규 원화**이며 32×32 및 64×40 v2 nearest upscale/벡터 도형 단순 확대 불허. 기존 제3자 원본/공식몬스터 픽셀·그림 복사 금지.
- 반드시 **독립 PNG2 원본**을 제출하고, 실제 공식5종 단일프레임과 배율이 정합된 비교판/게임 표시 크기 mock-up을 추가할 수 있으나 비교 원본과 Scale 인수가 입증되지 않는 장식판을 '공식 비교'라 부르지 않는다. 원화와 프레임 시트/게임 장착이 서로 다르므로 **기술 적용용 Sprite READY**라고 표시하지 않는다.
- 바울 대표가 두 그림을 확인해 스타일을 승인하기 전 VenomSnake 전체 20프레임/Wolf Run6·Bear/Fox 신규 제작·Codex Unity Import 금지. 실제 KeyArt 2장이 준비되기 전 장황한 기준 재설정 문서 추가만 제출하지 말 것.

## 4. 납품·역할

- 리뉴얼 담당은 **우선 실제 산출물** Field09 v2 보완 PNG8 ZIP과 VenomSnake KeyArt 한 장/Wolf KeyArt 한 장을 납품. Codex가 제공해야 할 Field09 좌표/시각 증거는 현재 저장된 Codex 리포트/이미지 참조; 막히면 딱 그 자료만 요청하고 독사·늑대 병렬 제작은 멈추지 않는다.
- `docs/art-renewal`에 브랜치/커밋/ZIP SHA, PNG2 실파일, Field09 변경픽셀·비포애프터, 실제 QA 범위 및 사용자 미술 승인 필요 여부를 기록. **최종 보고는 '문서화만 완료'와 '실제 PNG 납품' 구분**.
- Codex는 Field09 v1 ZIP 복구/기술 QA 성공 기록 보존, 새 PNG v2 미술 수정 자체는 담당하지 않음. 원본 Unity 적용 / GitHub Push/main merge 권한 별도. 몬스터·펫은 v2 미술 HOLD 유지, 정식 애니메이션/기능 변경0.
