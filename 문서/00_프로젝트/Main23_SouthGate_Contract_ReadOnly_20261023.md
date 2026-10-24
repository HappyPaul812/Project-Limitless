# Main23 남문 위치 계약 최종 확인 — 2026-10-23

**SOUTHGATE_PROPOSAL_ONLY / NEW_ART_REQUIRED_0 / ORIGINAL_SCENE_UNCHANGED**

[#58 최신 기획](https://github.com/HappyPaul812/Project-Limitless/issues/58#issuecomment-6359023173)·[#65 최신 기획](https://github.com/HappyPaul812/Project-Limitless/issues/65#issuecomment-6359021033)을 기준으로 기존 격리 자료 `dd5c0f4600857d14aa647d0dda6ab4318bad7a52`의 Scene·GapList·생성기·Quest·런타임 소스만 대조했습니다. **현재 Main23 남문 기능·동선 계약에는 별도 주택이나 신규 출입문이 필요하지 않습니다. 기존 시안·표지·상호작용을 유지하면 충분하며 신규 PNG 발주0을 권고합니다.** 기존 미승인 아트 전체의 최종 미술 승인이라는 뜻은 아닙니다.

|구분|실제 근거·좌표|판정|
|---|---|---|
|북서 Residence|Scene GameObject1237114990 / Transform1237114993 `(-12.5,7.2)`; GapList4행; 생성기78행. 기존 본체502×374/PPU128/pivot(.5,0)/scale1, 3.922×2.922WU; Collider3.1×1.6, offset(0,1.35)|기존 집·본체 내장 문·지붕 KEEP. 남문 집으로 이동·재해석하지 않음|
|서쪽 모듈 접합 연구|Scene GameObject438392462 / Transform438392465 `(-13,1)`; 생성기79~85행. Wall_Timber·Roof_Gable·Door_StoneArch 기존3종이 **이미 격리 시범 배치**됨. 공유 Collider2.2×1.7/offset(0,1.25)|장식·접합 연구이며 필수 남문 건물 아님. PROPOSAL_ONLY는 시안이 없다는 뜻이 아니라 **새 교체 아트의 필요성/발주가 제안**이라는 뜻|
|GapList 새 주택 제안|47행 `PROPOSED/Frostway_Residence_FrontDoorRoof_Joined.png`, `MISSING / NEW_PILOT_PROPOSAL`, UsedInIsolatedScene=NO, anchor(-13,1), 512²/PPU128/4×4WU **제안 예산**|파일 부재가 곧 퀘스트 결함은 아님. 필수 공백으로 확정할 근거 없음. CSV 원문은 수정하지 않고 필요성 분류만 정정|
|실제 남문|`Chapter3Main23Flow.cs:28`의 SouthGate `(0,-10)`, 기존 표지 `(-2.8,-8.6)`, Scene 도착 Spawn `(0,-8)`|선택→SceneTransitionService→F1 ReturnSpawn. 건물 문 Sprite/주택 참조를 요구하지 않음|
|목표·주민·안전거점|Quest3 frostway_main23_arrival: 마을 진입 후 Spawn처리 종료로 통지(Flow36~37행). Quest4 주민 `(-3,-4)` 증언 종료, Quest5 거점 `(0,0)` 등록/안전 Spawn `(0,2)`|모듈 주택·Residence·새 출입문과 연결된 목표/대화/출입구 없음|

**경계·동선:** Scene Bounds 중심(0,0)/크기40×28로 남쪽 맵 경계는 y=-14입니다. 현재 남문 상호작용 y=-10은 그 안쪽이며 경계 Collider를 물리적으로 통과하는 건물 문이 아닙니다. (-13,1) Collider 월드 범위는 x[-14.1,-11.9], y[1.4,3.1]로 중앙 남북3WU 통로 예산 x[-1.5,1.5] 밖입니다. 기존 연구 모듈을 남문에 옮기거나 건물을 추가할 이유가 없습니다. 실제 통행은 기존 근거를 보존하며 이번 소스 대조를 새 통행 PASS로 세지 않습니다. 동쪽7×7 예약공간·북문 F2 미개통·남문 F1 왕복 변경0.

**최소 권고:** 현재 남문을 위해 추가 제작할 부족 자산은 **0개**입니다. 재사용 대상은 기존 Residence 본체, 모듈3종과 SouthGate_ArrowSign이며 미술 승인 상태는 그대로입니다. 모듈 접합 품질/기존 OPEN16은 해결 선언하지 않습니다. 새 EntryDoor/RoofGable/RoofTrim은 미적용 후보 보존, 북서 A/B를 남문 완성 증거로 사용하지 않습니다. 새 문짝·문틀 제작도 미발주. 여관은 본체 내장 간판1개 KEEP, standalone 미적용. 오르나는 **visible80px / canvas약84.63px / PPU512 / scale1.175390** 기획 기본안만 유지하며 원본 적용·최종 외형 승인은 하지 않습니다.

**검증·보호:** 이번 Unity/Play/새 캡처/추가 전수QA 실행0, 신규 PASS 수0. 최신23 PASS/FAIL0와 이전148/136/41/15/33/8은 기존 결과로만 보존합니다. 원본 Unity·Save/UserData·승인지면9·PNG/GUID/meta·Scene·코드·기존 worktree 변경0, Push/main병합0. 대조5파일의 [원본 지문](Main23_SouthGate_Contract_SourceSHA_20261023.json)을 보존했습니다. 출처: Unity/Client/Assets/_Project 아래 Scenes/World_FrostwayVillage.unity, Scripts/World/Chapter3Main23Flow.cs, Scripts/Editor/Main23PresentationBuilder.cs, Resources/QuestDefinitions/Main23_WarmLand.asset 및 문서/00_프로젝트/Main23_Frostway_VisualGapList.csv.

보고 전용 새 브랜치 `codex/main23-southgate-contract-readonly-20261023`, worktree `F:/study/codex/Project-Limitless/Temp/SouthGateContract58/Worktree`. **LOCAL_COMMITTED / REMOTE_NOT_PUSHED**. #58 댓글에 근거·실제 로컬 커밋 보고. 신규 필수 건물·아트 발주 없음.
