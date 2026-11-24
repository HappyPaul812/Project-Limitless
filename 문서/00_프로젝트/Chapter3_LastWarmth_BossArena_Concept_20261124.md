# LIMITLESS / Issue #123 / 마지막 온기의 화로 최종 보스 구역 환경 구상 — 2026-11-24

[최신 발주 및 F5 채택](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7121162958), [Codex F5 검수](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7121034099), [설빙의 폭군·온기의 화로 상세 기획](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/08_몬스터/Chapter3_설빙의폭군_온기의화로_북부몬스터_상세기획_20261014.md), [11/18 전투 통합안](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/08_몬스터/Chapter3_북부몬스터7종_전투밸런스_20261118.md), [화로 전투 계약](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/10_전투/Chapter3_냉기_보스기믹_20261012.md) 확인. F5 FrozenRift_Edge 원화 채택 승인 KEEP, 실제 Import/Sorting/Collider/정식 Scene/실주행 HOLD. P2 추가 암반 계속 보류.

## 구상
남쪽 F5 입구 → 연속된 밝은 평탄 통행로 → 북쪽 설빙의 폭군 단독 대치 목표. 북쪽 화살표와 입구/목표 표시, 후속 출구 TBD. 4.5 WU 목표(실측 아님). 짙은 회청 결빙 #24394F, 서리 #8DA7BD, 희미한 화로 금빛 #CDB784. 승리 후 눈발 감소·설면 #D7E3E8·온기 #EDC783 회복. 새 타이머/강제 이동/전투 기믹 없음.

탐험 동선과 전투 화면 별도. 화로는 전투 화면에서 명령으로 사용하는 상호작용 대상이며 자유 이동 시설 아님. 아군 3인/보스/화로/상태 안내/명령 영역 가림 방지 도식. 행동 가능한 아군 자기 차례 화로 사용 → 살아 있는 아군 1명 선택 → 동결괴사 1/2중첩 전체 제거, 행동1회 소비. 아이템/탈렌트0·회복/부활0·횟수 제한0. 취소/부적격 대상 비용0. 행동불가 사용자는 사용 불가, 살아 있는 행동불가 대상은 다른 동료가 지원 가능. 3중첩 전 유효 대응 행동 최소1회 보장. 실제 입력/턴큐/접근성 NOT_VERIFIED.

## 재사용 감사
Library 실물 F3~F5 PNG 6종 회수, 승인 SHA와 일치, 작업 전후 불변. F3 GroundTransition d63c8d5ae60921dc7007caf839b720d68dda8078f273b86f1404f6121475a4be, LowFrostRock ddbba749c8f60f3b207769c35883aa844e80e782e8dc512077608d2568f07bb1, FlushStone v2 e4d6af0f3aa854eeeef591091312ae593e2c928ed31123f4ca83cfe049b704b3, F4 WhiteConifer 398b07335d84b0702a54e1bfae109bce75c60679f8e6a6fda6ccad6cd4386fad, DirectionalSnowdrift 1ae4ae973495d7d5c09efa3c7b6604233e6d4e66e74ca4d2d0e275e3bef630b1, F5 Edge 29d2eff4659aa3c4aad0b63cf8a7ae661409d111488f8e625042f1b6339dbd7c. 기존 Codex F3 로컬 참조 PARTIAL 기록은 유지. 이번 Library 해시 대조는 사용자 PC 재검증 아님.

Issue #30 [보스 1포즈 제작](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-6282576507) 및 [Idle 검수](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-6285579775) 확인. 기록상 FrostTyrant_B_1Pose_Rework_v2.png SHA f1a3c5f91a5fef69cd881e27b578c32253d0124d5d8628a8ca748f7479333830, FrostTyrant_B_Idle_4Frames.png SHA 9d9b8d05be1813c013a5d784d749ce2d8ef751541ac36ce90712f3255cb53f43. 이번 검색 범위 내 보스/기제작 화로 원본 PNG 실물 미확보. 존재하지 않는다는 판정 아님. 구상도 내 명시적 자리표시만 사용, 신규 보스/화로 원화 제작0. 원본 재사용 우선, 후속 파일럿 전 기존 실물 확인 필요.

## 신규 후보 최대 2종 — 제안만
P1 Boss_EncasingIce_Backdrop 512×384 RGBA 제안: 거대 결빙 외곽 벽/빙층, 기존 암반과 F5 균열로 부족할 경우만 발주. P2 Hearth_WarmthGround_Overlay 256×256 RGBA 제안: 기존 화로 주변 낮은 불투명도 금빛 지면 오버레이, 기존 광원으로 충분하면 생략. 화로 본체 재제작/복제 금지. 제안 규격이며 PPU/Pivot/Sorting/실제 좌표 미확정. 신규 게임용 PNG0.

## 납품 및 파일 검증
LastWarmth_Northbound_Concept_OFFLINE.png 1680×1400, SHA256 c4c37da6c8d868dfec16df242cd1c6cfb6f5c1dea3ac0adc9191e24a4447f624. 기존 승인 6종 실물 축소 합성 및 북진 배치, 13개 배치 사각형과 도식상 통행로 다각형 교차0. 실제 4.5WU·Scene·Collider·도보/휠체어 실주행·전투 UI·턴큐 미검증. LastWarmth_Environment_Proposal.md, QA.json, Manifest.json, SHA256SUMS.txt.
ZIP LIMITLESS_Issue123_LastWarmth_Concept_20261124.zip 502657bytes/5entries, SHA256 2ad2122e697b92a4888963aa9878ab247b89c1ff5d48b8d8d5d0f7383a4900bd. CRC PASS, Manifest3/3, SHA256SUMS4/4 PASS, 순환 참조0.

**BOSS_ARENA_CONCEPT_ONLY / CONCEPT_REVIEW_PENDING / EXISTING_BOSS_HEARTH_BYTES_NOT_RECOVERED / UNITY_NOT_APPLIED**. 기존 승인 원화 수정0, 신규 맵/Scene/스프라이트 양산0, Unity6000.6.5f1 실행/적용0. #85 HOLD/main/Save/UserData/Discord 보호. docs/art-new 문서·최신 인계 및 #123 결과 보고만, main 직접 commit/Push/Merge0.
