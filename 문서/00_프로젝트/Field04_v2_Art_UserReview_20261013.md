# Field04 v2 리뉴얼 — 바울 대표 미술 검토 의견 (2026-10-13)

**상태(최신): USER_ART_APPROVED / ROAD42_APPROVED / UNITY_OFFICIAL_INTEGRATION_PENDING / FINAL_RUNTIME_QA_PENDING.**

## 사용자 직접 의견
2026-10-13 바울 대표: “아, 그리고 리뉴얼 확인함. 좋구만.”

현재 이어진 대화 문맥상 Chapter2 `Field_04_WesternBorder` 동서 진행로 중심의 리뉴얼 v2에 대한 **미술 인상 승인**으로 기록한다. 다른 리뉴얼 산출물에 대한 의견이었다면 정정할 것. 이 긍정적 미술 평가는 기술 QA나 Unity 정식 반영을 뜻하지 않는다.

## 2026-10-13 최종 승인 — Road42 (최신 결정)

바울 대표: **“F:\\Downloads\\Limitless_Field04_Renewal_Art_v2.zip 이거 봤는데, 이게 42개 연장안이면 확정해.”**

- 이전 조건부 검토 기록과 달리 **이번 최신 지시로 Road42(기존 동서 도로30개 + 양쪽 연결 연장12개)를 최종 확정**한다. ZIP은 42개를 한 번에 배치하는 단일 Unity Scene이 아니라 **Road30 + Optional Endpoint12 별도 PNG/Mapping을 함께 납품한 아트 패키지**이다. Codex의 Field04 v2 QA 및 아트 납품 정본에서 이를 확인했다.
- 승인 입력 파일: `F:/Downloads/Limitless_Field04_Renewal_Art_v2.zip`, 납품/검수 SHA256 `178ad6b235c65848cce48f4e2cfe86ff77c36857a12663237f7d7afa561f969d`. 새 통합 전 Codex는 로컬 ZIP의 SHA를 다시 확인할 것.
- 기존 Road30의 배치는 x=-7..7, y=+0.5/-0.5; 신규 끝단12는 x=-10,-9,-8,+8,+9,+10의 두 행. 기존 Scene 전체/도로 PNG의 90도 회전 금지. 추가 12 Renderer는 원안처럼 Collider 없는 시각 표시만 허용. Optional Story3는 승인 범위 밖으로 미배치.
- 승인 범위: Field04 v2 환경 외형·나무 Big/Medium 대응·RockNorth 시각 후보 및 **도로42 시각 배치 정식 적용 준비 허가**. 기존 Field03 Rock B 보완과 구별, 다른 지역/아트 자동 승인 금지.
- **Unity 원본 정식 반영은 아직 미진행**. Codex가 최신 main과 기존 사용자 변경을 보호한 별도 `codex/` worktree에서 통합 후보를 만들고, 42안으로 Main12~13·Field03↔Field04↔Arbel↔Field05·휠체어·실제 출입 Trigger·Save/Continue·전투·리스폰·Camera/Collider·오류회귀를 검증한 후 별도로 보고할 것.
- 기존 **서쪽 Spawn(-8,0)와 TreeWest 겹침 및 북쪽 경계 틈**은 이번 미술 승인으로 해결되지 않았다. 기능 수정은 별도 범위 검토·백업·승인 필요. 현행 42안 독립 전체 스토리/Save 회귀 PASS도 아직 없음.
- 기존 승인 PNG, Scene, Quest, NPC, GUID/meta, Save, TTS/BGM, dirty checkout·원본 Unity Editor 포커스 보존. **AI의 main 직접 커밋·Push/병합 금지**, 완료 보고에 브랜치/커밋/QA/PARTIAL/사용자 확인사항 분리.
- 리뉴얼 담당은 Field04 v2 중복 제작하지 않고 **Arbel 환경 리뉴얼** 조사·제작 준비로 이동 가능. Arbel 아트의 독립 최종 승인·Codex 통합은 후속 단계.

## 이전 검토 시점의 변경/승인 경계 (최신 Road42 승인이 대체)
- 검토 대상: `Limitless_Field04_Renewal_Art_v2.zip`, 기존 Codex 보고 `Field04_v2_Road_Candidate_QA_20261013.md`, 아트 담당 납품 `Field04_Art_v2_EastWestRoad_Delivery_20261013.md`.
- 환경 톤·나무·바위·동서 진행로의 전반적인 미술 방향: **USER_ART_LOOK_APPROVED**.
- Unity 후보는 **기본 Road30안**과 양쪽 출입구까지 연결한 **Road42안(30+Optional12)** 두 가지이며, **사용자는 이번 발언에서 어느 배치안을 확인했는지 명시하지 않았다**. 따라서 최종 구현 버전 `ROAD_LAYOUT_OPTION_PENDING`. 기획상 추천은 서쪽 Arbel·동쪽 Field03 연결이 시각적으로 자연스러운 42안이지만 승인으로 간주하지 않는다.
- 별도 미해결: 기존 서쪽 Spawn과 TreeWest 간 겹침, 북쪽 Collider 틈, Search 예외 등. 아트 승인으로 QA PASS 또는 기능 수정 승인 처리하지 않음.
- 현재 공식 Scene·Unity 원본에 v2 정식 적용됐다는 근거 없음. Codex는 사용자 최종 Road30/42 선택 뒤 적용 및 회귀 QA, main 병합/Push는 별도 승인. 리뉴얼 담당은 기존 제작 v2 중복 재제작 금지, 다음 Arbel 리뉴얼 조사·기획 선행 가능.
