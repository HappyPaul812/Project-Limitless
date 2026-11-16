> 2026-11-16 EOD 선별 인계: 출처 `origin/docs/planning`, Git blob `8bf5d02417b4823ba80a322ef6650b662173a77b`. 이하 보고의 승인/미적용/PENDING 표시는 작성 당시 상태입니다. 최신 결재는 EOD 보고 우선: Stoat KeyPose·Idle4·Attack4v2·Hit4 미술 승인/Unity미적용, NPC02/17·10/07 로컬적용, Issue85 Import/GUID HOLD. NPC 손 한정 지시는 기획 해석 오류였으며 최신 정정대로 인계합니다. 기존 제품·미완료 자산을 새로 적용하지 않습니다.

# LIMITLESS / 2026-11-16 / Chapter1 시작마을 오리지널 NPC 다음 2종 — OGA10 파티 관리인 · OGA07 주민1

- **지시자:** 바울 대표: "리뉴얼 다음 일 시켜줘.."
- **담당:** Chapter1·2 리뉴얼 아트 (브랜치 `docs/art-renewal`)
- **범위:** 기존 NPC 12종 중 아직 원화 완료 확인되지 않은 OGA10·OGA07 **1포즈씩, 총 2장**, 승인 후 실제 게임 반영 여부 별도 판단
- **상태:** `PILOT_2_REQUESTED / ART_NOT_DELIVERED / USER_ART_APPROVAL_PENDING / NOT_UNITY_APPLIED`

## 1. 기존 완료와 정확한 현황

- 기획·리뉴얼 감사 [#57](https://github.com/HappyPaul812/Project-Limitless/issues/57), [#95](https://github.com/HappyPaul812/Project-Limitless/issues/95), [#99](https://github.com/HappyPaul812/Project-Limitless/issues/99)와 [비고유 원본 실사용 감사](https://github.com/HappyPaul812/Project-Limitless/blob/docs/art-renewal/문서/00_프로젝트/Renewal_NonOriginal_ActiveVsStored_Audit_NPCPilot2_20261023.md) 확인.
- 원화 승인·로컬 게임 적용까지 완료된 6종: **OGA03 잡화상인, OGA09 주민대표, OGA06 치유사, OGA16 은행안내인, OGA02 훈련장 안내인, OGA17 장비상인**. 이 6종은 SHA·Sprite GUID·PPU/Pivot·매핑·StableID **그대로 보호**. #99 OGA02/17은 2026-11-16 [Codex 로컬 적용·62/62 Delta PASS](https://github.com/HappyPaul812/Project-Limitless/issues/99#issuecomment-6906678804), 사용자 **최신 실플레이 수용만 PENDING**. 완료를 기다리면서 이번 독립 아트 제작은 병렬 진행 가능.
- **OGA20 남문경비**는 태온과 외형 유사성으로 `HOLD`. 사용자 승인 없이 재작업하지 않는다.
- 나머지 OGA11·OGA13·OGA19 일반 주민은 아직 신규 원화 완료 증거 없으며 이번 두 종과 동시 제작 금지. 최신 로컬 시안 발견 시 재제작 전에 보고.
- **기존 훈련장 별도 훈련 기능과 장비상점 구매·판매는 미구현이던 기능**으로 이번 NPC 원화 업무와 무관. NPC10 파티관리 기존 기능도 새 기능으로 확대하지 않는다.

## 2. 이번 2종의 캐논·역할·표시 계약

| NPC | 기존 Stable ID | 기존 Resources 경로 | 시각 방향 |
|---|---|---|---|
| OGA10 파티 관리인 | `starter-village-party-manager` | `VillageNpcSprites/Eldiran/OGA10_PartyManager` | 동료·파티 구성 담당자임을 한눈에 알 수 있는 정돈된 여행복/작은 파티 표식 또는 명부. 은행안내인 OGA16의 안경·장부·남색 정장 및 주민대표 OGA09 지팡이·긴 코트와 **구별** |
| OGA07 시작마을 주민1 | `starter-village-resident-01` | `VillageNpcSprites/Eldiran/OGA07_Resident01` | 따뜻한 시작마을의 평범한 거주민. 활동하기 편한 소박한 생활복과 작은 생활 소품. 새 직업·퀘스트 역할·신상 캐논을 임의로 부여하지 않음 |

- 해당 OGA10·OGA07의 과거 제작/승인 원화가 있는지 최신 리뉴얼 기록·해당 아트 Library 납품 목록·GitHub Issue부터 확인. **이미 승인 원화가 있으면 신규 출력0, 기존 원본 근거와 SHA 보고**. 사용자 Windows F: 전체 부재 조사까지 했다는 허위 주장 금지.
- **마스터 각1개:** **512×512 RGBA 투명 정지 1포즈**, 총 신규 2. 기존 6종과 굵기·명암·질감·얼굴 크기·따뜻한 생활감 스타일 일치. 기존 32px/PPU28 씬과 비교하되 **512마스터를 PPU28로 직접 Import 금지**. 허용 실제 게임 높이1.142857WU는 후속 Codex 계약에서 개별 PPU/Pivot 산정.
- **실물 비교:** 승인 NPC6 중 적어도 유사 가능성이 높은 OGA09 주민대표, OGA16 은행안내, OGA02 훈련장, OGA17 장비상인 실물 원화와 1:1 화풍 비교. **512 원화, 128px, 40px 실제 축소**를 함께 보고, 두 NPC 서로 및 기존 NPC 실루엣/정체성 차별화. 평범한 주민은 의복·사소한 도구가 40px에서 판별되지 않더라도 인물 실루엣 구분이 우선.
- 게임의 NPC는 **대사 초상화 없음·정지 스탠딩**. 애니메이션/다방향/추가 주민 3종·NPC12 일괄 양산 금지.
- **납품:** PNG2 + 실물 참고 비교판 + 128/40 축소판 + 출처/SourceDeclaration/Manifest/SHA256SUMS/ZIP CRC·정확 해시 보고. 오프라인 합성은 실제 Unity GameView로 표기하지 말 것.

## 3. 적용/보호·후임 인계

- 이번은 오리지널 **미술 파일럿 2종 발주**. 사용자 원화 시각 승인 전 Unity/Scene/Prefab/Resource/NPC Mapping 적용 금지.
- 승인 후 Codex가 기존 #57/#95/#99의 **소수 제품 파일/Delta QA** 방식 재사용. 기존 원본 PNG/meta 삭제·변경 금지, StableID/역할/Quest/E·패드·마우스/이름표/Collider/Sorting/Save/UserData 모두 그대로 보호.
- Chapter3 신규 아트(설원족제비), 기존 Ch1/2 승인 환경/몬스터/펫, 기존 NPC6, OGA20, Discord 봇·자동화·설정, 개인 파일 전면 보호.
- Unity **6000.6.5f1** 고정. Issue #85 Import/Compile·원격 Clean Release Push HOLD, GitHub main 직접 커밋·Push·merge 금지. **대형 QA Worktree 복제/불필요한 백업/캐시 증식 금지**. 11/12 [디스크 정책](팀운영/LIMITLESS_QA_DISK_STORAGE_POLICY_20261112.md) 준수. 실제 제작은 아트 담당 세션, 로컬 게임 검증/적용은 Codex.
- **`docs/art-renewal` 제작 보고서·`WORLD_ART_RENEWAL_LATEST.md` 갱신·해당 Issue 결과 댓글.** 파일 SHA/브랜치/커밋, 사용자 미술 승인 필요 여부·게임 적용 미완료 구분.

이 문서의 등록은 **이미 NPC 원화를 제작하거나 로컬 Unity를 수정했다는 의미가 아님**.
