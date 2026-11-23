# LIMITLESS / 리뉴얼 아트 / Issue #124 후속 — 길 Trait 4종 오리지널 정지 아이콘 제작 납품

> 2026-11-23 / 담당 브랜치 `docs/art-renewal` / **ART_CANDIDATE_TECH_PASS · USER_ART_APPROVAL_PENDING · NOT_UNITY_APPLIED**

## 발주 및 기존 승인 자산 보호

- 바울 대표 최신 결정: **시각 Trait v2 30px 기획 검수 통과**, v2 PNG **BYTE_KEEP**; 남은 청각·지적·지체·마음의 상처 Trait 4종 128×128 투명 RGBA 독립 아이콘 신규 제작. 기존 공식 Path Symbol 5종, Unity 기존 Trait4·GUID·PathDefinition·Scene·meta·원본 리소스는 변경하지 않는다.
- 확인 출처: [Issue #124](https://github.com/HappyPaul812/Project-Limitless/issues/124) 및 최신 해당 댓글, `WORLD_ART_RENEWAL_LATEST.md`, 기존 `Renewal_20261123_Issue124_VisionTrait_v2_GrassSlimeGel_Pilot_Delivery.md`. Codex #124 실물 비교 당시 시각 v2의 **40/30 PASS, 26/24 PARTIAL**을 확인. 본 새로운 발주는 30px 통과 판정을 수용한 것으로, 실제 Unity 게임 적용/대표 최종 미술 승인까지 주장하지 않는다.
- 기존 승인 기준 `/mnt/data/LIMITLESS_Issue124_TraitVision_v2_GrassSlimeGel_20261123/PNG_Originals/VisionTrait_v2_128_RGBA.png` **SHA256 `6b4fcdc9897d712b2490dc14dbe8ea59abc30876738aa2f7443e5effc55aacb1`** 원본 재사용·SHA 확인, 변경0.

## 신규 게임 임포트 후보 PNG 4종 (모두 128×128 RGBA 투명)

| 역할 | PNG 경로 (ZIP 내부 `PNG_128/`) | SHA256 | 원래 외부 Trait 아이콘 및 GUID |
|---|---|---|---|
| 청각 | `Hearing_Trait_Original_128_RGBA.png` | `fb429502cf62d063887e196ed878d402f0b575d0658a9e9de864440cfeb949bb` | `sound-waves.png`, `e54a40b325884dfe94b051148ef04d14` |
| 지적 | `Intellectual_Trait_Original_128_RGBA.png` | `9bbc023310f8210d6f7d7e17e2e089edb7e0352f4065dd76a2732b148106bb71` | `mesh-network.png`, `b5ddb75c1df7465fb0494dfd5c76a6e7` |
| 지체 | `Mobility_Trait_Original_128_RGBA.png` | `740f5c832799a471c6d83ede60b1b094756d7d5456d6a19e85cf930acec9709e` | `cog.png`, `765c9dee6e494a2cbc30f76d637b5066` |
| 마음의 상처 | `EmotionalScar_Trait_Original_128_RGBA.png` | `9a4801e883eee89305c6835bce3038da9992832b6a44aa74e644cf23204285c5` | `heart-shield.png`, `ecc84d2dd627400cb76d063c617fff46` |

- 시각 v2의 흰색/얼음 청색/짙은 남색 윤곽선·4방향 마름모·분절된 링·명암을 참고하되, **서로 다른 중심 문양** 사용: 청각=귀/소리 파동, 지적=연결된 사고 결절, 지체=전방 이동하는 바퀴/궤적, 마음의 상처=균열과 회복을 잇는 봉합된 마음. 새로운 퀘스트·직업·특정 장애 원인/신상 추가 없음.
- **제작 선언**: AI에서 2×2 신규 일러스트 콘셉트 시트를 생성한 후 각 셀을 독립적으로 추출, RGB 시트에 실제로 인쇄되어 있던 체크무늬 배경을 분리하고 **진짜 RGBA 투명**으로 변경; 고립 외곽 잔여를 제거·512 투명 마스터 4장·128×128 후보 4장 및 40/30 실축소 각 4장 생성. 체크 배경이 인쇄된 생성 RGB 원본은 납품 후보가 아님. 기존 PathSymbol·외부 아이콘·공식 캐릭터 이미지 픽셀 재사용0. `Docs/SourceDeclaration.md`에 재료·과정 기재.
- **공식 Path Symbol과 혼동 방지**: 기존 `PathDefinitions/*.asset:pathSymbol`과 `traitIcon`은 별개이며 이미 확인된 GUID 매핑을 보존. 현재 이번 환경에는 공식 Symbol5의 검증된 원본 이미지 바이트가 없어 **신규4 vs Symbol5의 직접 동일배율 이미지 픽셀 비교는 미실시**. Codex의 기존 실물 조사 참조를 넘어서 `PIXEL_UNIQUENESS_PASS` 주장 금지.
- Game-icons.net 원본은 CC BY 3.0 (청각 Skoll, 지적/마음 Delapouite, 지체 Lorc)이며 현재 게임에 원본이 남은 동안 기존 크레딧·저작권 표시 유지. 기존 원본 삭제나 원본 GUID 재사용 금지.

## 실제 오프라인 PNG 검수

- **검수 보드:** `Review/Trait4_vs_Vision_v2_128_40_30_Comparison.png` — 기존 Vision v2 실제128 PNG와 신규 4 PNG를 같은 128 / 40 / 30 실제크기에서 나란히 배치(30px 확대 사례 별도). `Review/Trait4_40_30_DarkLight_Readability.png` — 네 원화의 40/30 실제 크기 어두운·밝은 UI 모의 배경 비교. `Review/{Hearing,Intellectual,Mobility,EmotionalScar}_Trait_{40,30}px.png` 개별 실제 축소8장. **모두 오프라인 실제 픽셀**, Unity GameView 아님.
- **기술 검증 PASS**: PNG 4장 모두 `128×128 RGBA`, 알파 채널 0~255 포함, 4개 구석 알파0, bbox는 x=6..122 내, 128 마스터와 40/30 축소를 소스에서 생성. 시각 v2 원본 SHA `6b4fcdc...` 전후 동일. 오프라인 보드에 작은 크기 후보들이 렌더됨. 다만 **30px 판독의 실제 플레이 화면 PASS/개별 미술 승인 아님** (디자인 최종 검토 필요).
- **전체 ZIP:** `/mnt/data/LIMITLESS_Issue124_Trait4_OriginalArt_20261123.zip`, **1,176,825 bytes**, SHA256 **`0fa4db686a5886af004bddb0f4df15ed4557be8335019bef5a187c9e125de1c0`**, **23파일, ZIP CRC PASS / Manifest payload 21/21 SHA256+bytes PASS**. `PNG_128`, `Masters_512`, `Review`, `Docs/{README,QA,SourceDeclaration,Manifest,SHA256SUMS}` 포함.
- **4 PNG 간편 ZIP:** `/mnt/data/LIMITLESS_Issue124_Trait4_128PNG_ONLY_20261123.zip`, 85,152 bytes, SHA256 **`ed3772372fbcb49cff6866ed85723691b69fcadb9b23210d86f5e37e319a7789`**, 4개 PNG, ZIP CRC PASS.
- **ChatGPT Library 중복 보관:** `/LIMITLESS_Art_Delivery/Issue124_Trait4_OriginalIcons_20261123/` (전체 ZIP1, 간편 ZIP1, PNG4, 검수 보드2, **총8파일 업로드 PASS**). 사용자 Windows `F:\Downloads` 자동복사·로컬 Unity 임포트·GitHub PNG 바이너리 업로드는 하지 않았음.

## 승인 게이트/향후 업무

- `4_NEW_TRAIT_PNG_TECH_PASS / OFFLINE_SIZE_COMPARE_READY / PATHSYMBOL_PIXEL_SIDE_BY_SIDE_NOT_VERIFIED / USER_ART_REVIEW_PENDING / NOT_UNITY_APPLIED`.
- 대표 미술 승인 후 Codex에 기존 공식 PathSymbol5·외부 Trait5 실제 로컬 원화와 30px UI 판독·시각 v2 화풍/자산 중복 비교 요청. 그 다음 별도 제품 적용 승인 시에만 **기존 GUID/meta 유지, 새 Sprite 독립 import 후 청각/지적/지체/마음 각 PathDefinition.traitIcon만 최소 변경**.
- Unity **6000.6.5f1**, Issue **#85 HOLD**, 기존 PathSymbol5·Vision v2·전리품 GrassSlimeGel 및 외부 자산 원본, NPC·몬스터·펫·환경·BG·물약·기타 승인 자산, Scene/Prefab/PathDefinitions/GUID/meta/Save/UserData/Discord 전부 변경0. `main` 직접 Commit/Push/Merge 금지.
