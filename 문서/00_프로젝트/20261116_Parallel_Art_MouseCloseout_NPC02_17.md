# LIMITLESS / 2026-11-16 / Chapter3 서리들쥐 마감 + Chapter1 NPC 2종 병렬 발주

- **지시:** 바울 대표 “메인 아트는 들쥐? 완성을, 리뉴얼은 계속 npc 작업 맡겨줘.”
- **분담:** Chapter3 이후 신규 아트 `docs/art-new`, Chapter1·2 기존 비고유 자산 리뉴얼 `docs/art-renewal`.
- **방향:** 기존 승인 작업 재제작 금지, 신규 2종 소량 원화, Unity 컴파일/Import HOLD(이전 #85)와 대용량 QA 디스크 정리 규정 보호. Unity **6000.6.5f1** 고정.
- **유의:** GitHub `main`의 시점이 로컬 승인 적용 이력보다 과거일 수 있으므로 최신 작업 Issue/담당 인계/로컬 실제 결과가 우선. GitHub 이슈 발주는 외부 ChatGPT 담당 세션을 자동 실행하지 않는다.

## 1. 메인 아트 — 서리들쥐 FrostFieldMouse A 최종 마감

참고: [Chapter3 FrostFieldMouse 전투 Fix v2 승인본 실제 연결/40PASS](../../문서/00_프로젝트/Chapter3_FrostFieldMouse_Fix_v2_Official_Apply_QA_20261021.md), [Issue #30](https://github.com/HappyPaul812/Project-Limitless/issues/30), [Main22 첫 플레이 #44](https://github.com/HappyPaul812/Project-Limitless/issues/44).

**이미 완료된 자산(변경 금지):**
- 1포즈 `FrostFieldMouse_A_PixelArt_1Pose` / 사용자 승인.
- `FrostFieldMouse_A_Idle_4Frames.png` **SHA256** `5afebfa1f95ca5673162513fdea63b025546aa836550c0ceb17f0334659988f2`.
- `FrostFieldMouse_A_Attack_4Frames.png` **SHA256** `c83e35b5aebbce908b6025d5a071fafc463cfe301763f68806ecddb8bd4f576b`.
- `FrostFieldMouse_A_Hit_4Frames.png` **SHA256** `695ed7db1f794c3f71df1b058eb5e91f01de7ecb125369264f3624b36c9fd3c2`.
- `FrostFieldMouse_A_Defeat_4Frames.png` **SHA256** `3f6a52039b766c57a5367c7145e0234b68ac0a552940de1e88ffe8f2404f3395`.

기술 정본: 2048×512 RGBA / 가로4셀 각512 / 8FPS / PPU512 / Pivot(0.5,0) / 우향 / VisualScale1. #30의 기존 미술·전투 재검증 작업을 반복하지 않는다. 2026-10-21 Unity 외형연결 40PASS·원본 임포트 보고는 당시 기록의 실제 성과. 이후 Main22 실제 필드·전투 반영은 최신 로컬 검증 없이는 미완료/완료 단정 금지.

**이번 신규아트 담당 마감 범위:**
1. 현재 실제 보존된 승인 4모션 및 1포즈/관련 preview/Manifest/sha와 게임용 Sprite 지원 범위를 목록화. 누락 파일만 정확하게 기록, 기존 승인 이미지를 재그리거나 신규로 양산하지 않는다.
2. Main22/F1에서 쓰는 `fieldSprite=Idle00` 외형, 별도 필드 이동 애니의 필요 여부를 기획/코드 정본으로 구분. 기획상 미요구 모션을 임의 추가하지 않음.
3. 전투 외형 **ART_COMPLETE** 여부를 책임 있게 명시하고, 게임의 정식 `monsterId`, Spawn, EXP/Drop, Quest 및 실제 실플레이 검증 상태와 별개로 기록한다. ID/전투 스탯/보상은 기획·Codex 승인 전 임의 확정 금지.
4. 정말 빠진 게임용 Sprite가 문서상 명확할 때만 **정확한 1종**의 표적 제작 계획을 제시하고 먼저 승인 요청. 현재 명시된 4동작은 새 제작0.
5. `docs/art-new` 보고서·`SPRITE_ART_LATEST.md`와 기존 Issue #30 결과 기록. 실제 원본 파일 경로/SHA/APPROVED/QA/미검증 구별. PNG 바이너리가 GitHub에 없으면 실제 납품·로컬 적용을 허위 보고하지 않음.

## 2. 리뉴얼 — OGA02 훈련장 안내인 + OGA17 장비상인 (신규 2종)

정본: [#57 비고유 실사용 감사](https://github.com/HappyPaul812/Project-Limitless/issues/57), `docs/art-renewal/문서/00_프로젝트/Renewal_NonOriginal_ActiveVsStored_Audit_NPCPilot2_20261023.md`. 기존 NPC12의 StableID/Resources 코드 참조 확인.

**새 후보 2종**
- OGA02 `starter-village-training-guide` / 기존 `VillageNpcSprites/Eldiran/OGA02_TrainingGuide`. 플레이어가 초반 조작·전투를 배우는 안내인으로 식별되는 독창적인 편안한 복장, 준비/안내 동작을 느낄 수 있는 **정지 1포즈**.
- OGA17 `starter-village-equipment-shop` / 기존 `VillageNpcSprites/Eldiran/OGA17_EquipmentShop`. 장비를 안내·판매하는 상인으로 역할을 분명하게 구별하는 고유한 앞치마/가죽 공구 주머니 등. 이미 승인된 OGA03 잡화상인과 시각적으로 **명확히 달라야 함**.

먼저 해당 2종의 이전 별도 제작/승인 사실을 최신 Issue/문서/실물로 조회한다. 기존 오리지널이 있다면 중복 제작하지 않고 보고. 미제작 확인 후 **각 1포즈 512×512 투명 RGBA 마스터 1개씩(총 2개)**, 128/40px 실축소와 마을 기존 승인 OGA03/06/09/16 네 명의 동배율 스타일 비교판 제작. 게임용 최종 PPU·Pivot/크기 변환은 실제 Scene/코드 기준 표준 검증 전 `SPEC_PENDING`, 원본 512 마스터 직접 Scene Import 금지.

**중복/보호**
- OGA03/09 [#57 사용자 미술승인·로컬 반영·실플레이 완료](https://github.com/HappyPaul812/Project-Limitless/issues/57#issuecomment-6791260745); OGA06/16 [#95 완료](https://github.com/HappyPaul812/Project-Limitless/issues/95#issuecomment-6786869689): **기존 PNG·meta·Unity Mapping 변경0**.
- OGA20 경비는 태온 유사성 미해결 HOLD; 나머지 주민 OGA07/11/13/19 및 파티 OGA10은 이번 일괄 제작 금지.
- **NPC 초상화·애니메이션 제작 금지**. 기존 상점·튜토리얼·퀘스트·E/마우스/패드 상호작용·Nameplate·Collider·Save/GUID 보존.
- 독립 원본 아트, 저작권/출처 기록, SHA256SUMS·Manifest·CRC·디자인 비교판/미술 확인용 ZIP. 기존 리뉴얼 환경·펫·몬스터/상태 VFX/Discord 작업 건드리지 않음.
- 미술 승인 이전 Unity 원본 적용·임의 Sprite 교체 금지. 문서화는 `docs/art-renewal`, 인계 갱신 및 GitHub Issue 보고.

## 3. 공통 실행/검증·자원 보호

- 담당별 독립 진행. 실제 이미지 제작은 각 아트 담당에게, Unity 로컬 적용은 Codex에게 별도 요청. 기획이 GitHub 댓글로 발주했다고 이미지가 이미 만들어진 것은 아님.
- 승인 1포즈/모션/씬, 원본 Unity/Save/UserData, Discord 봇·자동화, Git worktree/미Push 이력 보호.
- 본편 및 Package/Import HOLD(기존 Issue #85) 관련 수정·Unity6000.6.5f1 버전 변경 금지.
- 11/12 [디스크 QA 운영 규칙](팀운영/LIMITLESS_QA_DISK_STORAGE_POLICY_20261112.md) 준수: 대규모 새 격리 작업복제 금지, 기존 검증 재사용, 사용자 화면 포커스 강제 전환 금지.
- 완료 보고: 해당 아트 담당 브랜치 / commit SHA / 실제 파일 SHA / Preview / 미술 승인·Unity QA·원본 적용 각각의 상태 / 사용자 승인 필요 여부. `main` 직접 commit/원격 Push·Merge 금지.

**지시 상태:** `ART_TASKS_ASSIGNED / MOUSE_EXISTING_ART_KEEP / NPC02_17_TWO_PILOTS_PENDING / NO_UNITY_OR_MAIN_CHANGE`.
