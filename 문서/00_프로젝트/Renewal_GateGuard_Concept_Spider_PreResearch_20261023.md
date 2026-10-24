# 2026-10-23 LIMITLESS 리뉴얼 — OGA20 남문 경비 후보 및 숲거미 사전조사

## 범위·보호
Issue #60, #57 후속. 종전 54행 정적 감사와 OGA03 잡화상·OGA09 주민대표 시안 그대로 보존. 기존 StableID `starter-village-gate-guard`, OGA20 GUID `a1200000000000000000000000000008`, NPC Role/Quest/Interact/E키/Collider/Sorting, Unity Scene/Sprite/Save, Wolf·Bear·Fox, Chapter2 공식 아트 불변. main 직접 변경 없음.

## 남문 경비 시안 1종 — 미술 후보
`Eldiran_GateGuard_Concept_512.png` 512×512 RGBA, 128×128 및 40×40 PNG 프리뷰; 판금 어깨갑옷·남청색 직물·창과 깃발, 경비의 역할이 보이는 실루엣. AI 생성 다중 뷰 보드에서 정면 1종을 분리·배경 제거·리사이즈한 **콘셉트 후보**. 512×512는 캔버스 크기이지 512px 네이티브 정밀 드로잉이라는 뜻이 아님. 배경제거 가장자리·투명도 및 기존 게임 PPU28과 실제 통일감은 추가 미술/Unity 검증 필요.

비교 보드 `NPC_3_Concept_Comparison_Limited.png`는 NPC 3종의 이름·역할·새 후보만 표시. **이 세션에는 과거 OGA03·OGA09 후보 원본 바이너리가 없어 그 둘은 명시적 placeholder**로 남겼으며 실제 세 캐릭터 동시 미술 비교 PASS로 주장하지 않음. 원본 두 시안은 이전 ZIP `Limitless_Eldiran_NPC_Pilot2_Concepts_20261023.zip`(SHA256 `7162600a51fddffc8a51087c8d1bbb1f8b14c928250ade120c197d23677fb0b3`)에서 Codex가 읽기전용 결합 가능.

이번 ZIP `Limitless_Eldiran_GateGuard_Concept_SpiderResearch_20261023.zip` / **318790 bytes** / SHA256 `692d1a72e12f59e61373b880f3ebf1afd8fec909b2659563b35b370eb63c3cab`; 512/128/40 PNG + 제한 비교보드 + Spider 보고서 + README + Manifest. ZIP CRC 검사 통과. 사용자 로컬 F: 자동 복사 및 게임 적용 없음. `USER_ART_APPROVAL_PENDING`.

## forest_spider 사전조사 — 제작 보류
기존 [54행 정적 감사](Renewal_NonOriginal_ActiveVsStored_Audit_NPCPilot2_20261023.md): MonsterDefinition 03 `forest_spider`, Field02·Field03 Spawn에서 정의 참조; `Resources/MonsterAnimations/Spider/{Idle,Walk,Attack,Shoot}` 존재 보고. 이것은 **정적 데이터/파일 존재 확인**이며 게임 프레임·실제 GameView 애니메이션 검증은 아님. 현재 사용 중인 실제 Sprite GUID/AnimatorClip/FPS/프레임 수·크기, 해당 아트의 제3자 출처·license, 별도 공식 미술 최종 승인 기록은 **미확정**.

**결정: `HOLD_EVIDENCE` / 숲거미 PNG 신규 제작 0**. Codex 읽기전용으로 Spawn→MonsterDefinition→Animation/Sprite와 ThirdParty 원본·LICENSE 및 과거 사용자 승인 기록 추적. 이후 승인된 경우에만 기존 계약 해상도·PPU·Pivot·FPS를 유지해 대기 Idle 4~6 / 이동 Walk 6~8 / 공격 Attack 6~8 / Shoot 6~8 프레임을 **권고안(미확정)**으로 제안. Hit/Die는 현재 확인된 4동작의 일부가 아니므로 임의 추가 금지.

## 브랜치
`docs/art-renewal` 문서 기록. 이번 작업으로 공식 자산·기존 54행 CSV를 덮어쓰지 않음. PNG/ZIP 파일은 GitHub에 업로드하지 않고 채팅 첨부로 전달. 이 문서 커밋은 GitHub 작성 결과로 별도 보고.
