# LIMITLESS / 리뉴얼 아트 / 2026-11-16 / Issue #101 OGA10·OGA07 오리지널 NPC 정지 원화 2종 납품

## 계약·선행 조사
- 발주 [Issue #101](https://github.com/HappyPaul812/Project-Limitless/issues/101), [기획 정본](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/20261116_Renewal_OGA10_OGA07_NPC_TwoPilot_Order.md). 담당 `docs/art-renewal`.
- GitHub Issue #101 댓글(제작 전 0), `WORLD_ART_RENEWAL_LATEST.md`, #57/#95/#99 및 ChatGPT Library `/LIMITLESS_Art_Delivery` 폴더 목록·키워드 조사에서 OGA10/07의 기승인 오리지널 원화/납품을 확인하지 못했습니다. **사용자 Windows F: 전체 전수 조사 및 실제 게임 시각 비교를 완료했다는 의미가 아닙니다.** 기존 구형 OGA10/07 게임 자산은 재작성하지 않았습니다.
- 기승인 OGA02/03/06/09/16/17 원본 실물 6장을 읽기 전용 비교에 사용했고, 기존 6개 PNG 변경·재인코딩·재작화 0. #99 OGA02/17 사용자 최종 실플레이 수용은 별도 대기.

## 실제 산출물
신규 마스터 **정확히 2개**, 각 **512×512 RGBA 투명, 1포즈 정지**입니다.

| NPC | StableID | 파일 | SHA256 | 크기(bytes) | alpha>8 bbox |
| --- | --- | --- | --- | ---: | --- |
| OGA10 파티 관리인 | `starter-village-party-manager` | `Candidates_512/OGA10_PartyManager_Original_512_RGBA.png` | `7a53cbb71544b69e996a06c7b576c9498d0565cc701a9123a6ad9485cd1d18a1` | 146037 | (140,58,371,470) |
| OGA07 시작마을 주민1 | `starter-village-resident-01` | `Candidates_512/OGA07_Resident01_Original_512_RGBA.png` | `940aa98590333b1423ed5f49101c9cf5a146a0079c8e354fc33e0ce943dff798` | 143641 | (165,58,346,470) |

- OGA10은 초록 조끼·붉은 스카프·세 사람 모양 파티 표식·환영 손짓으로 파티 담당 기능을 시각화하며, 은행 안내인 OGA16의 정장/안경/장부 및 주민대표 OGA09의 장외투/지팡이를 반복하지 않습니다.
- OGA07은 수수한 생활복·머릿수건·천 조각/소형 파우치로 일반 주민의 일상성을 표현합니다. 특정 신규 직업·퀘스트·대사·성별 캐논은 확정하지 않습니다.
- **정확한 제작 방식:** 신규 이미지 생성 도구의 투명한 2인 시안에서 두 인물을 각각 독립 분리, 원래 종횡비를 보존하면서 최종 512 정지 스프라이트를 제작. 이때 승인 기존6종의 픽셀을 복사·변형하지 않았습니다. `SourceDeclaration.md`에 고지.
- 실제 128px·40px 축소(Pillow LANCZOS), 승인 NPC6와 신규2의 512 동규격 원화 비교판 및 실제128/40 비교판 각1. **전부 OFFLINE**, 실제 Unity GameView/Importer·월드 PPU 측정 아님.
- alpha>8 캔버스 외곽 접촉 **각 0**. 40px에서 OGA10의 집단 표식 세부는 읽기 어려울 수 있어 역할 표식/화풍 **대표 육안 심사 필요**. 기존 6종보다 신규 후보의 눈·명암이 세밀하여 **화풍 완전 일치 PASS로 선언하지 않고 `ART_STYLE_REVIEW_REQUIRED` 유지**. 파일·RGBA·크기·checksum 기술QA와 시각미술 승인을 분리합니다.

## 납품·무결성
- ZIP: `LIMITLESS_ISSUE101_OGA10_OGA07_NPC_ORIGINAL_ART_PILOT_20261116.zip`
- 생성 컨테이너 실제 경로: `/mnt/data/LIMITLESS_ISSUE101_OGA10_OGA07_NPC_ORIGINAL_ART_PILOT_20261116.zip`; **사용자 PC의 F: 경로에 자동 복사하지 않았습니다.**
- ZIP 크기 **1,188,626 bytes**, SHA256 **`0ec80e1fa546f1ce9cde560bea34f07b3359a0678565d72daec3f711bc232ca5`**, **13 entries / ZIP CRC PASS / Manifest 참조 11파일 바이트·SHA PASS**.
- 비교판: `Review_OFFLINE/Issue101_8NPC_512_Style_OFFLINE.png` SHA256 `0fae31e460aea006d2ff0d18a1f4f54ee9ad88b4ad5cd6648dc705c1333caa10`; `Review_OFFLINE/Issue101_8NPC_128_40_ActualScale_OFFLINE.png` SHA256 `4ad913c49ba89deafebb2572a95b3096ee53146b8f0146a561f79237cb88bcef`.
- 내부 `Candidates_512` 2 PNG, `Previews_128` 2 PNG, `Previews_40` 2 PNG, `Review_OFFLINE` 2 PNG, `Issue101_SourceDeclaration.md`, `Issue101_README.md`, `Issue101_QA_Results.json`, `Manifest.json`, `SHA256SUMS.txt`.
- ZIP/PNG **GitHub 바이너리 Push 0**; GitHub에는 본 보고서+인계/이슈 댓글만 기록. 다운로드/Unity 적용은 사용자가 미술 승인한 뒤 별도 게이트.

## 보호·미검증·다음 작업
- 기승인 OGA02/03/06/09/16/17 BYTE_KEEP / 로컬 게임 적용 완료 유지. OGA20 남문경비 태온 유사성 HOLD 유지. OGA11/13/19 등 기타 주민 신규 제작0.
- NPC 초상화·애니메이션, 파티 기능/대사, Role·StableID·Scene·Prefab·Resources·GUID/meta·Nameplate·Collider·Sorting·Quest, Save/UserData, Discord 자동화·파일, 기존 공식 몬스터/펫/환경, Unity 원본/Editor/포커스 모두 **이번 작업에서 변경0**. Unity 기준 **6000.6.5f1** 고정. 새 대형 QA Worktree 복제0.
- **`NEW_2_STILL_ART_PILOTS_DELIVERED / PNG_AND_ZIP_FILE_QA_PASS / ART_STYLE_REVIEW_REQUIRED / USER_ART_REVIEW_PENDING / NOT_UNITY_APPLIED`.**
- 바울 대표가 시각 승인하면 그 뒤에 Codex는 기존32px/PPU28에서 올바른 월드 스케일/피벗/발접지/이름표를 최소 DeltaQA로 검토. **512 마스터 PPU28 직접 Import 금지**.
- main 직접 Commit/Push/Merge 0. 이번 원격 문서만 `docs/art-renewal` 작업 브랜치에서 관리.
