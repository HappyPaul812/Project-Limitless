# LIMITLESS / Chapter1·2 NPC 리뉴얼 잔여 목록 / 2026-11-19

## 대표 확정
- #117 펫 관리 담당 최종 미술 승인. 기존 512 PNG SHA256 62a771d90ccf27bc9754779c900178d22740dee041b66b1ec74172b00df285b9 BYTE_KEEP. #113 분양 승인도 유지. 재작화·반복 검수 금지.
- NPC 리뉴얼 전체 완료 확인 전 **불씨망령 HOLD_UNTIL_NPC_RENEWAL_COMPLETE**. 기존 불씨망령 원화/모션 보호. 이번 제작/Unity 변경 없음.

## 근거 및 확인 범위
- [11/17 EOD #114](https://github.com/HappyPaul812/Project-Limitless/issues/114#issuecomment-6946090923), 원격 복구 checkpoint ba59838bf923832362f3b1fccf4897864cc678fa의 VillageNpcAppearanceCatalog.cs, 실제 PNG 경로, #110/#111 상세 보고 및 #107/#108/#109 적용 기록, #113/#117 최신 승인 기록, docs/planning 최신 인계, 접근 가능한 Library 검색을 대조.
- 원격 main은 과거 코드이므로 로컬 적용 판정은 복구본 및 Codex 결과를 우선시함. 복구 트리 7909항목, truncated=false. 사용자 PC 전체 및 모든 동적 분기 직접 재실행 감사 아님.
- 완료: 시작마을 12종(#57/95/99/101/102/104/105), Main04/07 여행자 2종(#107/108), 레온1종(#109), 총 **15종 사용자 미술 승인 및 실제 로컬 적용 보고 확인**. 새 OGA20은 태온 직접 비교 PASS/3지역 적용, 구형 유사 시안만 HOLD. 기존 승인품 재제작0. 15종의 최신 사용자 실플레이 수용 여부는 별도.
- #113/#117 펫 분양·관리 전용 원화 **2종 미술 승인·기술 PASS, Unity 미적용**. 현재 적용된 것은 각각 승인 OGA17/OGA16 공유 이미지. 적용은 별도 Codex 발주. Main21 후 시작마을/카타콤 동적 역할 fallback과 범위 구분. Chapter3 Frostway 별도 담당이나 공유 fallback 영향 확인 필요.
- [#111](https://github.com/HappyPaul812/Project-Limitless/issues/111#issuecomment-6941300553) 3지역×BASE/Main21완료 NPC56기록 외부 legacy 렌더0. 이는 전체 Chapter1·2 모든 Scene/퀘스트/동적 조건의 전수 증명이 아님.

## 미제작 독립 외형 후보만 (승인 원화 재사용 중)
기존 원화는 이미 오리지널로서 기술적으로 사용 가능. 아래 9종의 **별도 지역 인물 독립 외형 제작은 제안 범위**이며 기획/대표가 공용 사용을 허용하는 경우 신규 제작 의무 없음. 확인 가능한 복구 자산 경로, 기존 보고 및 Library 검색에서 별도 승인 원화 미발견, PC/모든 브랜치 부재 확정 아님.

| 지역 | 대상 | stable ID | 현재 승인 공유 외형 |
|---|---|---|---|
| Ch1 Field_03 카타콤 입구 | 보급 상인 | safezone-catacomb-supply | OGA03 |
| Ch1 Field_03 카타콤 입구 | 파티 안내인 | safezone-catacomb-party | OGA10 |
| Ch1 Field_03 카타콤 입구 | 치유사 | safezone-catacomb-healer | OGA06 |
| Ch1 Field_03 카타콤 입구 | 감시인 | safezone-catacomb-keeper | OGA20 |
| Ch2 Arbel | 잡화상인 | arbel-shop | OGA03 |
| Ch2 Arbel | 치유사 | arbel-healer | OGA06 |
| Ch2 Arbel | 파티 안내인 | arbel-party-guide | OGA10 |
| Ch2 Arbel | 서쪽 정보 주민 | arbel-resident-west | OGA11 |
| Ch2 Arbel | 포털 관리인 | arbel-portal-keeper | OGA20 |

- 기존 #110/#111 역할 재연결 완료 건을 외부 PNG 잔여로 잘못 분류하지 않음. 레온·공식 주요 캐릭터 보호. Main14~21의 추가 미제작 일반 퀘스트 인물은 기존 기획 정본 조사에서 명확히 발견되지 않음.
- 별도 1개 미결: Arbel(1.5,-1.5) NO_ROLE:VillageNpc, 단색 런타임 1×1 PlaceholderVisual, StableID/역할 없음, 기존 임시 인사 대사. 외부PNG 아님. 유지/제거/정식 주민화 기획 결정 전 제작·삭제·새 ID 부여 금지.
- 결론: **당장 의무 신규 제작 확정 0 / 독립 외형 후보9 / 임시 주민 처리 미결1 / 승인 후 적용 대기2**. 독립 외형 전체 제작 선택 시 9종, 임시 주민 정식화 선택 시 최대10종. 지역마다 새 펫 분양/관리 담당을 반복 제작할 필요는 현재 계약에 없으며 전역 공통 역할 재사용 허용 범위 별도 확인.
- 다음: 기존 감사 재사용, 필요한 미확인 참조만 Codex 읽기전용 보완 감사. 사용자 승인 후에만 개별 발주·PNG 제작/Unity 적용. 완료 기준은 범위 확정 및 승인/실제 적용/임시 주민 결정/잔여 실사용 감사. 불씨망령은 그 전까지 HOLD. Unity6000.6.5f1, #85 HOLD, Save/UserData/Discord 및 main 보호. docs/art-renewal 문서화만, main 직접 commit/Push/Merge0.
