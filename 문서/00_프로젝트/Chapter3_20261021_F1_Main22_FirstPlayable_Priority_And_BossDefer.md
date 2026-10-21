# LIMITLESS — 2026-10-21 Chapter3 실제 플레이 우선순위: 서리들쥐 → F1 → Main22, 보스 후순위

- 작성 담당: `docs/planning`. 사용자 최신 발언: “서리폭군 맞니? 나도 헷갈리는데 너까지 헷갈리면 안된다... ㅋㅋㅋ 하여튼, 폭군은 보스니까 안급하니 당장 해야 할거 하자.”
- **공식 표기 정정**: 첫 일반 몬스터 `서리들쥐` (`FrostFieldMouse`). Chapter3 후반 보스 `설빙의 폭군` (`FrostTyrant`). `서리폭군`은 비정식 오기이며 신규 코드·UI·문서명으로 사용하지 않음.
- 우선순위: `FIRST_PLAYABLE_MAIN22` — 메인 전투 1종(서리들쥐) → 북부 첫 필드 F1 미술·기술 확정 → Chapter2 Main21→StarterVillage 북문→F1 Main22 5목표→서리들쥐 지정전투→F1 쉼터 완료. Main23 서릿길 마을·Main24 F2가 후속. 설빙의 폭군 보스 공격·피격·사망·화로/보스전 맵은 **당장 발주하지 않음**.

## 이미 완료·중복 발주 금지

- 서리들쥐 A Attack/Hit/Defeat `Limitless_Chapter3_FrostFieldMouse_Combat_3Strips_Fix_v2.zip`은 바울 대표가 **“쥐 괜찮아.. 승인”**으로 사용자 미술 최종 승인. Codex 변경분 Unity **23PASS/0FAIL**, Attack00·Hit03=승인 Idle00 픽셀동일, 타격 Attack02·Defeat03 장시간 hold 정상. Codex branch `codex/chapter3-mouse-combat-v2-review-20261021`, QA 최종 commit `757d6a759a3ecd53d4a4e14d03324c20dd3b7897`. 승인 Idle/1포즈 바이트 불변, 정식 실게임 Combat/MonsterDefinition 연결은 **이전 대화에서 Codex용 별도 지시문만 전달됐고 실제 완료 보고 없음**. 동일 기능 구현을 다른 중복 브랜치에 발주하거나 구현 완료 주장 금지.
- 보스 설빙의 폭군 B는 1포즈와 Idle만 승인/참조 보관. 후반 보스 애니메이션 전체 제작/정식 Unity 연결0; 급한 순서에 넣지 않음.
- `main` 최종 확인 2026-10-21 오전 `e4e9891f6f01ea95cada9e63b1f09946d94d6041` (EOD PR41/42). Chapter3 PNG4 참조는 Unity Assets 외부이고 Runtime 연결 없음. 다른 Codex 작업 브랜치 완료 여부는 실행 전 다시 확인해야 함.

## 이번 **즉시 병렬** 업무

### 메인 아트 `docs/art-new`: F1 6종 **실물 미술 비교/사용자 승인 게이트** (새 전체 제작보다 먼저)

- 기존 `Limitless_Chapter3_ArtNew_F1_Frostway_Pilot_v1.zip` 중 **F1 PNG6만** 표적. 나머지 서릿길마을 PNG4/이전 반려 몬스터 KeyArt2/보스 B는 이번 게이트 제외. ZIP은 과거 해당 세션 대화 첨부였고 바울 대표가 Downloads 폴더를 정리했으니 이전 `F:\Downloads\...` 위치 있다고 단정 금지. 가능하면 기존 아트 세션 실제 원본을 활용, 재제작/재생성은 보류.
- 대상: `NorthF1_Ground_Earth_Base`, `NorthF1_Ground_ThinFrost_Base`, `NorthF1_EarthToFrost_Transition_Overlay`, `NorthF1_Path_WornTrail_Overlay`, `NorthF1_Prop_FrostShrub_01`, `NorthF1_Prop_Waymarker_01`.
- 현재 파일 QA는 256×256 RGBA/CRC/2×2 타일 경계 일부 PASS 보고. **미술·실사용 Unity 비율/PPU·바닥-길 전이/가독성·휠체어 경로 검사 미실시, 미술 승인 없음**. Pilllow 기반 원화가 기존 Chapter2 공식 맵 대비 지나치게 단순·도식적인지 동배율 128px·하나 이상의 F1 코너 비주얼 보드로 비교. 필요시 같은 게임 화면 비율의 **합성 시안**은 '오프라인 Preview, 실제 Unity GameView 아님' 표기. 실제 기존 공식 지형 원본을 현재 세션에서 가져올 수 없으면 추정 비교 대신 source 접근제약 보고.
- 대표님께 F1 **6종 미술 PASS/HOLD/FAIL 결정 자료**를 먼저 제출. 승인되지 않은 PNG의 대규모 신규 지역 추가/서릿길 마을 NPC7·F2 몬스터·보스 시트 착수 금지. F1 화면에 얇은 서리·갈색 흙·한눈에 읽히는 단단한 귀환 길, 과도한 하얀 눈밭 X.

### Codex `codex/*`: **서리들쥐 승인 연결이 실제 완료됐는지 우선 확인**, 끝난 뒤 Main22 첫 플레이 기술·ID 갭 봉합

- `Issue #35` 읽기 전용 기획·Scene 기술 preflight와 `Main22_북풍의징후_상세기획`을 참조해 현재 실제 StarterVillage 북쪽 문/Spawn/F1 Scene 존재 및 SceneTransition/Save/Quest nextMainQuestId 빈 값, 입·퇴장/휠체어 통행, F1 월드 bounds/PPU/pivot/source와 `MonsterDefinition` 실제 연결 id 충돌 확인. 이미 관련 기술 조사 문서가 있다면 **중복 재조사 금지**.
- 최신 `Chapter3_Main22_북풍의징후_상세기획_v1_20261014.md`: 5목표(마을 북문→F1 진입, 얇은 서리 조사, 남하 동물 흔적2, 서리들쥐 한 마리 지정전투, 쉼터 정리). Chapter2 Main21 마지막13목표·펫 NPC 이용은 강제 구매 없이 유지. F1 한기/동상 아직 도입 X. 하렌 4인 전투·설빙 보스 스폰 X.
- 이번 지시는 **서리들쥐 승인 연결 작업의 중복 진행 금지**, 그 완료 보고 이후 Main22의 최소 구현 전제/ID 후보 충돌표·Scene/Pivot 기술 갭을 먼저 갖추자는 방향. 사용자가 별도로 실제 Main22 Unity 구현과 F1 미술을 승인하기 전까지 새 Scene/Quest/Script 강제 생성·PR main병합 안 함.
- Main22 F1 실제 제작/게임 적용은 사용자 승인·기술계약 lock 후 작은 vertical slice 단위로 시행한다. 원본 dirty/Save/UserData·원래 Editor/Play 보호.

## 후속 제작 게이트

1. 서리들쥐 전투 연결 안전QA 보고, F1 미술6 종 실물시안 사용자 최종 시각판정, Main22 Scene/ID/경로 gap 종결.
2. F1 승인 PNG를 Codex 계약에 맞춰 game-ready 제작/Import 후 `Main22` 한 챕터만 격리 구현·진행/전투/Save/복귀 테스트, 사용자 플레이 검수.
3. Main23 마을4PNG/기능/안전지대·NPC 서비스, Main24 첫눈/한기 및 마을 스킬상점, 설빙의 폭군 B 최종 전투 애니메이션은 해당 보스 단계 후반.
4. 현재 다른 담당 `docs/art-renewal`의 Chapter1·2 맹독뱀/Wolf v3과 업무 구분, TTS/SFX/영상 보류·BGM Loop 미완료 보호.

- 변경 기록은 `docs/planning`, art 제작은 `docs/art-new`, 기술은 `codex/*`. main 직접 commit/Push/merge 금지. 사용자 원본 로컬 Downloads 폴더 위치 변경을 임의 추측하지 않음.
