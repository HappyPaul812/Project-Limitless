# Main24 스킬상점 / Issue #70 기획 검토 — 2026-10-28

상태: **PLANNING_REVIEWED / USER_REVIEW_PENDING / IMPLEMENTATION_NOT_AUTHORIZED**

## 출처와 판정
- Codex Issue #70 완료 보고: https://github.com/HappyPaul812/Project-Limitless/issues/70#issuecomment-6468962827
- Codex 로컬 설계 브랜치 `codex/main24-skill-shop-contract-20261028`, 로컬 SHA `0d4c50b819ae16c626fc06eade1f83d62e8985b1`, `LOCAL_COMMITTED / REMOTE_NOT_PUSHED`. GitHub 원격에서 로컬 문서·Diff 본문을 독립 확인했다고 주장하지 않음.
- 선행 #46 Save 로컬 구현 `3380fe9d3fb98cd7547aa9c517197e41cb977745`: 격리 Unity 31 PASS, 종료/Continue 12 PASS 포함. 다만 **유료 신규·강화 소유권 영속성까지 완성된 것이 아님**.
- Issue #70의 QA 매트릭스 20건은 **설계 작성 / 미실행**. Unity/원본 Save·TTS·Scene 변경 없음. Issue #46/#70는 제품 main 통합 또는 판매 구현 완료로 닫지 않음.

## 발견된 출시 차단 조건
1. 기존 SkillState SchemaVersion1은 CharacterId+JobId / OwnedSkillIds / EquippedSlots. Import가 기본 카탈로그만 재생성하여 아직 승인되지 않은 추가·강화 스킬의 구매 영속성 경로는 없음.
2. `GameSaveService`의 현재 `.tmp → File.Copy(overwrite) → 삭제`는 결제의 원자적 커밋과 정전 내구성 보장이 아님. 기존 일반 Shop 성공 후 별도 저장도 안전한 선례로 취급하지 않음.
3. 요구 설계: 승인 상품 및 구매자/직업/선행/잔액/중복/동시성 검증 → Currency+Ownership+Replacement+Slots+거래ID 후보 통합 → 검증된 공용 저장 Writer 경계에서 확정 → 파일 확정 확인 후 메모리/UI 반영. 불확실한 쓰기결과는 추정 환불·중복재시도 없이 HOLD 및 재조사. 지원하지 않는 파일시스템/다중 Writer는 안전 계약 검증까지 HOLD.
4. 기존 기본3/직업 무료, 이후 신규/강화 탈렌트 개별구매, 신규 자동장착0, 강화 슬롯승계, 전투 밖 무료6칸 장착은 기확정 KEEP.

## 기획 권고 — 아직 바울 대표 최종 승인이 아님
| 분류 | 권고 후보 (탈렌트) | 상태 |
| --- | ---: | --- |
| 일반 신규스킬 | 40 | USER_REVIEW_PENDING |
| 기존스킬 강화 1단계 | 30 | USER_REVIEW_PENDING |
| 온기의 불꽃 (마도사 신규) | 20 | USER_REVIEW_PENDING |
| 부활 (치유사 신규) | 50 | USER_REVIEW_PENDING |

근거: Codex 정적 추산 Main13~21 관련 퀘스트 보상 470, 임의 납품4종 111, Chapter2 일반 몬스터11~14/감시자22/거신40(확률 잡템 제외). **퀘스트·납품 총액은 동일 세이브 실제 보유 잔고가 아님**. 소모품20~40/기존 펫100과 경합. 예상 최초 상점 도달 시 잔액 분포 및 구매후 포션·펫 자금 잔여 검증 전 정확 가격 확정 금지.

기획 추천 정책(대표 검토 필요):
- 구매 소유권은 현행 **캐릭터+직업+Save슬롯 별개**로 유지, 비활성 동료·비현재 직업 기록 보존. 계정 전체 공유로 암묵적 확대하지 않음.
- 추가·강화 스킬 ID 및 계보·해금·실제 판매목록은 기존 5직업 상세초안과 충돌 검사를 거쳐 명시 확정. 상품 ID 발명·효과 임의구현 금지.
- 임의 환불을 거래 복구 수단으로 사용하지 않음. 향후 콘텐츠 삭제·구버전 Writer 실행 시 데이터를 침묵 속에서 잃지 않게, 미인식 ID 격리·명시 안내 및 미래 스키마에 대한 구버전 Writer 저장 차단 여부를 따로 검증.
- Main24 첫 서리올빼미 한기(2턴 행동불가)는 구매 불필요. 북문~F2 진입 과정에서 **무료 비상 한기 대응 도구(예: 따끈 어묵탕) 2개**가 자동 확보되고, 실패/재전투에서 부족분을 다시 받을 수 있도록 하는 방안을 우선 제안. 마레 상점 대화/구매/특정 직업 편성은 필수 조건으로 삼지 않음. 정확 수량·대상·재지급·한기 타이밍은 기술/전투 QA 전 미확정.
- 초기 Main24 구매처 루덴(가칭), 마레 보급, Chapter3 완료 시 전 안전거점 상점 확대 기존 스토리 계약 KEEP. 이름/위치/공식 NPC ID 최종 구현 명세 TBD.

## 다음 실행 게이트
1. 바울 대표가 가격표와 소유권/환불·구버전 호환, 무자금 대응 보급 정책을 승인 또는 수정.
2. Codex가 승인된 범위에서 **격리 Unity/합성 Save**만 사용하여 공용 거래 Writer·유료 소유권 복원 안전 구현. 초기에는 승인 정의가 없는 스킬을 임의로 생성하지 않고 인터페이스/거래 전후 불변식 중심으로 최소 델타. 20개 설계 시나리오의 실제 PASS/FAIL을 새 검증 근거로 기록.
3. 새 게임/기존 Save/구버전/동시 저장/중복구매/실패 및 별도 프로세스 Continue 검증 후에만 제품 PR·원본 로컬 반영 검토. 이전 #46 31 PASS, Main23/TTS162 전수 재실행 금지.
4. `main` 직접 commit/Push, 사용자 개인 Save·Editor 포커스, Discord #66, 미승인 아트·BGM/SFX 변경 금지.

현재 **Issue #70 완료는 설계 완료**라는 뜻이며, 유료 스킬 상점 또는 원자 결제 기능의 실제 구현 완료가 아니다.
