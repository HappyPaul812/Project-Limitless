# LIMITLESS / Chapter 1 리뉴얼 / 2026-11-17 / Issue #104 + #105 새 NPC 정지 원화 2종 납품 보고

## 납품 요약
- 담당: `docs/art-renewal`. 참조: [Issue #104](https://github.com/HappyPaul812/Project-Limitless/issues/104), [Issue #105](https://github.com/HappyPaul812/Project-Limitless/issues/105), `WORLD_ART_RENEWAL_LATEST.md`, `Renewal_StarterVillage_NPC_Visual_Diversity_Rule_20261117.md`.
- **신규 오리지널 아트 마스터 정확히 2종**: OGA19 주민4 + OGA20 남문경비. 기존 OGA20 태온 닮은 시안은 **계속 HOLD**; 이번 새 디자인을 자동으로 승인한 것으로 보지 않음.
- **중복 조사:** 최신 #104·#105·#60·담당 인계 및 접근 가능한 ChatGPT Library 납품 폴더에서 OGA19 독립 승인 원화 또는 태온과 구분되어 승인된 OGA20 새 정본은 확인하지 못함. **사용자 Windows F: 전수 조사 및 부재 증명은 아님.**

## 신규 PNG/권한
| NPC | StableID | ZIP 내부 PNG 경로 | 파일 SHA256 | 크기 |
|---|---|---|---|---:|
| OGA19 주민4 | `starter-village-resident-04` | `Candidates_512/OGA19_Resident04_Original_512_RGBA.png` | `d0ec6f5712ea9d7d6cada6335742986613b2d4149b1f4b75c662b0a3e22a1028` | 161540 bytes |
| OGA20 남문경비 | `starter-village-gate-guard` | `Candidates_512/OGA20_GateGuard_Distinct_512_RGBA.png` | `19efaea3859246a41d9bd3cb1880744f3b0802b9c51ec22f0937a6794e201ef9` | 165861 bytes |

- 두 PNG 모두 **512×512 투명 RGBA 정지 1포즈**, 알파 유효 bbox OGA19 `(153,53,358,473)`, OGA20 `(134,53,377,473)`. 캔버스 4면 알파 접촉0, 발 시각 접지 y473, 비균일 리사이즈 없음. 1254×1254 RGBA 별도 신규 이미지 생성 원화를 비율 유지 축소한 결과이며 SourceDeclaration에 파일 SHA 기록.
- OGA19: 넓은 얼굴/도톰한 턱, 상대적으로 올라간 좁은 눈매, 옆가르마 짙은 적갈색 웨이브, 넓은 어깨·단단한 실루엣, 청회색 바깥옷/분홍색 허리천·일상용 가방. **기존 OGA07(두건/부드러운 눈), OGA13(얌전한 눈·숄/천), OGA11(모자/채소 바구니)과 얼굴·실루엣·색면·소품에서 차별화**. 소품은 생활감 표현이며 새로운 직업·대사·퀘스트 설정이 아님.
- OGA20: 각진 얼굴·굵은 눈썹, 짧은 흑갈색 머리·수염, 크고 단단한 체형, 남색 천+강철색/가죽 갈색 방어구·장병기/성문 상징. OGA10(젊고 부드러운 얼굴·모험가 조끼), OGA11(생활 조끼/바구니), OGA19(일반 주민)과 다르게 **경계 근무 역할 실루엣**. 직업/스킬/퀘스트를 새로 추가하지 않음.
- **중요: 공식 태온 초상화 또는 정식 스프라이트의 검증된 픽셀 원본을 이번 실행환경에서 확보하지 못했음. `TAEON_VISUAL_NOT_VERIFIED`**. 따라서 "태온과 실제 정면 픽셀 비교 합격"으로 보고하거나 임의 생성 이미지를 태온이라고 표시하지 않음. 기존 태온 닮은 OGA20 구형 후보의 HOLD는 유지. 사용자 시각 검토 시 실제 태온 원본이 확보되면 Codex 읽기 전용 비교를 요청.

## 실제 원본 비교/검증
- 참조 승인 원화 **OGA02/03/06/07/09/10/11/13/16/17 10종**은 채팅·Library에서 확인된 실물 PNG를 읽기 전용으로 사용하고 각 SHA256을 `Issue104_105_QA.json`에 기록. 기존 승인 원화 바이트 변경0. OGA19·20과 섞은 타인 검색 이미지0.
- `Review_OFFLINE/Issue104_105_12NPC_512_ActualScale_OFFLINE.png`는 승인10+신규2의 원본 512×512를 그대로 같은 캔버스 배치. SHA256 **`8f20b841eccde923a6f169de31fee28cb9738eaf420584256b2eb4fdceb8ded1`**.
- `Review_OFFLINE/Issue104_105_12NPC_128_40_ActualScale_OFFLINE.png`는 각 해당 원본에서 실제 128·40px LANCZOS 축소한 비교판. SHA256 **`dc8f599c5427359cd0e831958d9c9ef8128956e4ee8a41790b24396ee1da1756`**.
- `Review_OFFLINE/Issue104_105_Distinction_256_Preview_OFFLINE.png`는 OGA07·OGA10·OGA11·OGA13·OGA19·OGA20 실제 PNG의 256px 미리보기. SHA256 **`399e33ae8cb738426499a88db867a093c95aeab980cc61eb1cb33dfb37090cb0`**. 전부 **오프라인 보드**, 실제 Unity GameView 아님.
- `Previews_128/`과 `Previews_40/`에 새 후보 2인별 실제 축소 PNG 총4.
- SourceDeclaration/README/Issue104_105_QA.json/Manifest.json/SHA256SUMS.txt 포함.

## 실물 압축 및 사용자 다운로드
- 전체 ZIP: `/mnt/data/Limitless_NPC_104_105.zip`, **2166760 bytes**, SHA256 **`807f7d6bb3c29f8846c08d5c7e985244b009ebc6b1bea4a1181c641ed3d9351e`**. **14 entries**, ZIP CRC `testzip() is None` PASS, Manifest가 포함한 12 payload의 bytes/SHA256 PASS. Manifest/SUMS 자체는 순환참조를 피하도록 payload 목록에서 제외하고 SHA256SUMS에 Manifest 단독 SHA 포함.
- 다운로드 문제 대비 **2 PNG 전용 ZIP**: `/mnt/data/Limitless_NPC_104_105_2PNG.zip`, **325672 bytes**, SHA256 **`df48347008a808fbe787d362f301724d837337a4f5314faec4c8f97621876018`**, 2 entries, ZIP CRC PASS.
- 상기 경로는 **ChatGPT 실행환경 경로**. 사용자의 `F:\Downloads` 자동 복사·로컬 Unity import·GitHub 바이너리 Push가 아님. 과거 다운로드가 막혔으므로 Library에도 별도로 실물 저장 시도.

## 보호 및 다음 승인
- 원화 제작만 진행. 공식 승인 NPC 10종·기존 OGA20 시안·Scene·Prefab·Resources·GUID/meta·Nameplate/Collider·StableID/퀘스트/상호작용·Save/UserData·Discord·기존 Chapter1·2 몬스터/펫/환경·원본 Unity6000.6.5f1 변경0.
- OGA20 기존 `safezone-catacomb-keeper`, `arbel-portal-keeper` 등 같은 Sprite 재사용 지점은 **추후 승인 후 Codex의 별도 런타임 영향 검토 필요**. Issue #106 이름표 얼굴 가림은 아트 제작과 분리. Issue #85 HOLD 유지, 새 대형 QA 복제0, main 직접 Commit/Push/Merge0.
- **`OGA19_NEW_1POSE / OGA20_NEW_DISTINCT_1POSE / FILE_TECH_PASS / TAEON_VISUAL_NOT_VERIFIED / USER_ART_REVIEW_PENDING / NOT_UNITY_APPLIED`**.
