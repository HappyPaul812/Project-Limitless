# LIMITLESS — Field09 Phase2 Pilot v1 실화면 시각 FAIL / Cliff·Fissure 2종 표적 보완 지시 (2026-10-20)

- 작성 담당: 기획 `docs/planning`. 업무 이슈: [#38](https://github.com/HappyPaul812/Project-Limitless/issues/38). 실제 아트 재작업 담당: `docs/art-renewal`.
- 근거: 바울 대표가 전달한 Codex 검수 스크린샷/보고. **ZIP·PNG 파일/기술 PASS, 시각 FAIL**. 본 기획 세션은 실제 Windows F: ZIP나 Codex GameView를 자체 실행·독립 검증한 것이 아니다.
- Codex 보고 브랜치 `codex/field09-phase2-pilot-qa-20261020`; QA commit `f2ad1781`, 인계 commit `89319b10`. 격리 Unity Delta30 PASS/0 FAIL 및 3 화면비 검증 완료; **실제 게임 미술은 아래 2개 이유로 FAIL**. 원본 3,546파일, Save/UserData, 기존 승인3종 SHA 보호. 기존 전체 17/92PASS 재사용, official Scene 적용·Push·main 병합 없음.
- v1 파일: `F:\Downloads\Limitless_Field09_Redesign_Phase2_Pilot_v1.zip`, ZIP SHA256 `f12b056924665eeb593d084d5a5b73ba67a1a3a335d2bd9f882a547e5b8a7ff6`. Art 파일 출처: `docs/art-renewal/문서/00_프로젝트/Field09_Redesign_Phase2_Pilot_v1_ActualArt_Delivery_20261020.md`; 실제 png/zip은 GitHub에 바이너리 등록한 것이 아닌 리뉴얼 담당 ChatGPT 납품 후 사용자 F: 로컬 복사.

## 결과: 수정 대상 **정확히 두 PNG**

1. **Cliff** `LL_Field09_Phase2_Cliff_Blocker_v1.png` (기존 SHA `2e7a863547688773dbb3d6672623e0958fbab01c8241d2665d8739a0777a231c`): 직선 가이드 자체는 제거됐지만 소품 하단에 **회백색·홍흑색 매트/바탕의 잔여 픽셀**이 보이고 지면 접지가 어색함. 새 원화의 외곽 아래쪽과 실제 지면을 비교하여 부적절한 투명도/단색 판을 제거, 바위 발치·그림자의 alpha/암부/접지 마감을 자연스럽게 재작업. 임의 검은 사각 그림자·흰색 주변 blur·일괄 알파 임계값으로 미봉하지 말 것.
2. **Fissure** `LL_Field09_Phase2_Fissure_Edge_v1.png` (기존 SHA `7c892de74b1f3327b37f4fb3ff7673954d64542498b32f6ab76826e6ca53b1d0`): 소품 외곽 매트가 잔존하고 **현행 `ObsidianScar_Glow`와 형상이 맞지 않아 붉은 띠가 노출**됨. `ObsidianScar_Fissure`(original position -5,3.8, scale .525/.625, order -8) 위 `ObsidianScar_Glow`(same position/scale, order -7)를 실제 겹친 상태로 봐야 함. **기존 Glow 파일, Renderer 설정/위치, 과열·조사 기능은 변경하지 말고, 우선 Fissure Edge 쪽 새 Alpha·형상·붉은 균열의 넓이/끝맺음**을 현행 Glow와 정합시킬 것. Glow만 감추도록 불투명 판을 덧씌워 과열 피드백을 가리는 수정 불합격. 기존 Glow 수정 없이는 해결 불가능하면 결합형 교체가 필요하다는 정확한 근거·제안만 별도 보고, 사용자 승인 없는 Glow 재작업 금지.

## 보존·검증 계약

- 바울 대표가 승인한 Sample3 3종의 실 PNG 및 SHA 불변: `Base Ground=1bd8a614988ee182640a71b9771646c3009545140e0522a3290ee3b0e3138203`; `Cracked Overlay v2=0147e6aff58e5304fd71e7474a9c018958a4234ec4c2dc2cf7382a905a7be9b7`; `Obsidian Slab=dfa3abb2c6c86abc3f94baa213428038a1c15b4c963e76fef69491a6deb09692`. AshPatch KEEP 후보, Glow/HeatVent/Spire 후속 HOLD, Cracked02 미사용 HOLD. Chapter3 신규 아트/BGM과 업무 섞지 않기.
- 기존 Field09 Scene/Transform/Sorting/Collider·Main18 퀘스트·과열·지형 이동/몬스터·Field08·Save/UserData·Git main/기존 공식 PNG/meta/GUID 절대 수정 금지.
- 아트 담당은 **v2 보완 실 PNG2**만 제작→새 ZIP `Limitless_Field09_Redesign_Phase2_Pilot_Fix_v2.zip`(작명 제안), `Manifest` 원본/새 SHA, 비교 자료, 불량 매트 픽셀 위치와 변경 요약, 실제 오프라인 Cliff 접지/Fissure와 기존 Glow 겹침 Preview, ZIP CRC/testzip/디코딩 검증 증거를 포함한다. **오프라인 Preview를 Unity 실제 GameView라고 주장 금지.** GitHub 담당 브랜치 `docs/art-renewal` 문서+실제 바이너리 접근 가능한 경로를 보고. 납품 상태와 사용자 승인 상태 분리.
- Codex에 필요한 경우 실패 GameView의 관련 객체 영역과 정확 alpha/bbox 및 현행 Glow 참조 **읽기 전용으로만** 제공받되, Codex 기존 전체 QA/원본 Editor의 재실행은 금지. 리뉴얼 담당이 의도와 다른 이미지를 추측만으로 재제작하지 않게 참조.
- 후속 Codex는 실제 v2 ZIP이 나온 뒤 **오직 변경 Cliff/Fissure 2종의 3화면비 GameView visual Delta와 Glow overlay**, 기능 시각마커를 검수. 기존 30PASS/17PASS/92PASS 기술 QA 전수 재실행 금지. 미술 결과 0FAIL이어도 바울 대표 새 2종 미술 승인 전 다음 전체 환경 양산·원본 본게임 적용·main 병합 금지.

**정확 상태:** `PHASE2_PILOT_V1_TECH_PASS_30_0 / CLIFF_VISUAL_FAIL / FISSURE_VISUAL_FAIL / ART_FIX_V2_REQUESTED / USER_ART_APPROVAL_PENDING / ORIGINAL_SCENE_NOT_APPLIED`.
