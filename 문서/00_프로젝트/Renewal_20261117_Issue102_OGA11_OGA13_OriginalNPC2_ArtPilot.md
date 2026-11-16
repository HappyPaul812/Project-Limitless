# LIMITLESS / Chapter1 / Issue #102 — OGA11 주민2·OGA13 주민3 오리지널 정지 원화 2종 제작 보고

- **일자:** 2026-11-17
- **담당 브랜치:** `docs/art-renewal`
- **발주:** [Issue #102](https://github.com/HappyPaul812/Project-Limitless/issues/102)
- **결과:** `TWO_STILL_ART_MASTERS_DELIVERED / FILE_QA_PASS / USER_ART_REVIEW_PENDING / NOT_UNITY_APPLIED`

## 중복 제작 방지 및 정본 확인
- GitHub Issue #102, `WORLD_ART_RENEWAL_LATEST.md`, GitHub OGA11/OGA13 코드 검색과 접근 가능한 ChatGPT Library `/LIMITLESS_Art_Delivery` 납품 목록에서 이미 승인·제작 완료된 OGA11/13 오리지널 원화를 확인하지 못해 신규 **2종만** 제작.
- 이 판단은 **현재 확인 가능한 GitHub·Library 기준**입니다. 사용자 Windows F: 로컬 디스크 전수 조사를 했다는 의미가 아닙니다. Unity 내 구형 OGA11·13 실사용 Sprite와 새로운 고유 원화는 구분합니다.
- 기존 승인 **OGA02/03/06/07/09/10/16/17 총8종**의 실제 512 PNG를 비교판 제작에 읽기 전용 사용했고, 이 8종의 원본 SHA256 일치 여부를 모두 확인했습니다. **기존 원화 재저장·수정·재작업 0**. 이전 #101의 잘못된 타인 이미지 표시 문제를 방지하기 위해 비교판은 **실제 원화 파일에서만 생성**, 이미지 검색·무관 생성 이미지 혼입 0.

## 새 원화 2종
| NPC | StableID | 원화 경로 (ZIP 내부) | 바이트 | SHA256 |
| --- | --- | --- | ---: | --- |
| OGA11 시작마을 주민2 | `starter-village-resident-02` | `Candidates_512/OGA11_Resident02_Original_512_RGBA.png` | 144955 | `5fde2e4cfbcdca2839f7da1071dbbbc4b7f239ee85ab7f7b9985be48a5cb91f9` |
| OGA13 시작마을 주민3 | `starter-village-resident-03` | `Candidates_512/OGA13_Resident03_Original_512_RGBA.png` | 145915 | `0b2b5a1a19965eeff4bbdccff0072d25c7881e6695bdc32b146542311be7f849` |

- **OGA11:** 소박한 모자·조끼·바지·채소 바구니, 마을 생활감. 신규 직업·퀘스트·스토리 설정 없음. Resources 기존 이름: `VillageNpcSprites/Eldiran/OGA11_Resident02`.
- **OGA13:** 차분한 숄·앞치마와 생활천·작은 바구니, OGA11과 다른 색·실루엣. 신규 직업·퀘스트·스토리 설정 없음. Resources 기존 이름: `VillageNpcSprites/Eldiran/OGA13_Resident03`.
- 두 시안 모두 이미지 생성으로 새로 만든 원화를 각기 **비균등 변형 없는 동일 축척**으로 리샘플하여 **512×512 투명 RGBA 한 장씩** 생성. 원본은 1254×1254 RGBA 이미지 각1개. 출력은 보이는 높이 약410px·발 접지 y470 정렬. Alpha>8 실제 bbox OGA11 `[149,61,361,470]`, OGA13 `[169,59,342,470]`; 투명 캔버스 4변 접촉 0.
- 기존 승인 8명과의 화풍/안면/명암 **최종 일치 여부는 사용자 미술 승인 대기**. 신규 원화의 눈·의상 명암이 기존 OGA02/03/09 등 단순 표현보다 세밀한 부분이 있어 **ART_STYLE_REVIEW_REQUIRED**. 40px 세부 소품 가독성은 실제 축소 비교판에 표시. 승인 완료로 쓰지 않음.

## 정확한 원본 기반 비교판과 QA
- `Review_OFFLINE/Issue102_10NPC_512_Master_ActualScale_OFFLINE.png`: **실제 512 마스터 10종**(승인8+신규2)을 각각 512px 크기 그대로 배치한 비교판. SHA256 `115f94686e551def405a59919b86a308fbc275f6bde441d21587120e32d791f9`.
- `Review_OFFLINE/Issue102_10NPC_128_40_ActualScale_OFFLINE.png`: 해당 **실물 512 원본 각각을 LANCZOS로 실제 128px·40px 축소**한 비교판. 작은 캐릭터를 가짜 확대하거나 다른 이미지를 대용하지 않음. SHA256 `915a578eba42cb3141c4312d47cf16486033fd769e45e47eb46b6a96bdaee709`.
- 신규 후보 각각의 개별 128px·40px PNG 총4 및 `README.md`·`SourceDeclaration.md`·`Issue102_QA.json`·`Manifest.json`·`SHA256SUMS.txt`.
- ZIP: **`/mnt/data/Limitless_NPC_102.zip`**, **1,436,792 bytes**, SHA256 **`c6e2268f817df91447f462ad7b7f30de3710fcc2abb168556884f7136c285b95`**, 13 entries, **CRC PASS, Manifest 11/11 bytes/SHA256 PASS**. 짧은 파일명 PNG 전용 ZIP: `/mnt/data/Limitless_NPC_102_2PNG.zip`, 289,061 bytes, SHA256 `99446dbd018c5b82eb4b20d4ecfd7e8f5ce90d87663b6294d2ba4928e659d650`, 2 entries, CRC PASS.
- 두 ZIP은 ChatGPT 실행 환경에서 생성·검사했으며 사용자 PC F:에 자동 복사하거나 GitHub에 바이너리 게시하지 않았습니다. 오프라인 비교는 Unity GameView 검증이 아닙니다.

## 안전 범위
- **OGA02/03/06/07/09/10/16/17 승인 원화8 / 로컬 제품 적용 내용 변경0**, **OGA20 남문경비 태온 유사성 HOLD**, OGA19 등 타 주민 변경0.
- 초상화·애니메이션·추가 주민·기존 StableID/대사/상호작용/Quest/Scene/Prefab/Nameplate/Collider/Sorting/GUID/meta/Save/UserData/Discord/원본 Unity 변경0. Unity **6000.6.5f1** 유지, Issue #85 HOLD, 새 대형 Unity QA 복제0.
- 대표 시각 승인 전 **`NOT_UNITY_APPLIED`**. 승인 시 Codex가 기존 32px/PPU28와 월드 높이/발접지/이름표에 맞는 최소 델타 검증을 별도로 수행하며 512마스터를 PPU28로 바로 Import 금지.
- `main` 직접 Commit/Push/Merge 0. 담당 브랜치의 문서·인계 및 본 Issue 결과 댓글만 변경.
