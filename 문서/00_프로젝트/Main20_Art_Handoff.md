# Main20 Art Handoff — Field11 / 열맥 거신 / Boss VFX

정본: [Main20 심부의 거신](../03_스토리/Chapter2_Main20_심부의_거신.md). **ART_PENDING / USER_ART_REVIEW_REQUIRED**. 구현 전 아트 담당 세션에 전달할 제작 사양이며 이번 PNG/Sprite 생성·기존 원본 수정·Unity Import0이다.

## 공통 방향과 납품

검게 굳은 암석과 굵은 붉은 열맥, 압력으로 갈라진 대지, 거신 이동 흔적과 중앙 열린 전투 공간. 지표/심부 대지이며 용암 바다·화산 내부·지옥·악마 성·닫힌 Dungeon 방은 만들지 않는다. Main19보다 열기가 강하되 플레이어·조사 지점·통행 가능 영역을 읽을 수 있게 한다. 환경 규모·좌표·콜라이더는 구현 전 확정이며 배경 그림에 UI/텍스트/그리드/좌표/가이드선을 굽지 않는다.

PNG 원본과 파일별 Manifest（용도·크기·셀/프레임 수·순서·반복 여부·pivot 기준·권장 배치·hot/cooled 대응）를 납품한다. Sprite/VFX는 RGBA 투명 배경·프레임 간 동일 캔버스·발밑/중심 정렬·잘림/이웃 셀 침범0. 기존 수동 수정 Asset/ThirdParty 원본은 보존한다. 이 문서의 파일명은 새 납품 계약이며 기존 파일을 덮어쓰지 않는다.

## Field11 필요 자산

| 새 파일명 | 필요 사양 / 용도 |
| --- | --- |
| DeepCore_Ground_Base.png | 반복 가능한 검은 암석 지면 Base, 경계 seam 최소화 |
| DeepCore_Rift_01.png / DeepCore_Rift_02.png | 깊고 굵은 붉은 열맥 균열 최소2종, 투명 장식·통로와 구분 |
| DeepCore_Arena_PressureMark.png | Boss 열린 지대 지면/압흔, 일반 지면과 자연 연결 |
| DeepCore_Cliff_Boundary.png | 깊은 틈·절벽·외곽 Boundary 표현, 실제 Collider는 별도 |
| DeepCore_CollapsedCore.png | 승리 후 검게 굳어 무너진 잔해 Prop |
| DeepCore_HeatRecession.png | 동일 지면/균열과 정렬되는 cooled 표현, 잔여 약한 진동은 남음 |

환경 장식은 크기 가변이며 구체 픽셀 크기·타일 단위·Bounds는 구현 전 합의한다. hot/cooled는 같은 배치·발밑·통행 폭을 유지한다. 색만 바꾸지 않고 열빛 면적·열기 표현 감소·붕괴 잔해로 차이를 보여 준다. 정적 이미지를 기본으로 하고 환경 열감/맥동이 필요하면 별도 짧은 VFX를 사용한다. Main18/19 원본 일부 재사용은 가능하지만 이번 제작으로 원본을 편집하지 않는다.

## Boss 고유 디자인과 Main19 연결

열맥 거신 / Veinfire Colossus / `veinfire_colossus`. 검은 암석질 거체 내부 붉은 열맥, 넓은 어깨의 거대한 준인간형. 사람이 아니고 얼굴은 검은 머리 형태와 균열/열빛 정도. 흔한 용암 골렘·화염 악마·뿔·날개·Devil·인간 얼굴·정교한 갑옷 기사·거대한 검·특정 게임 Boss 복제 금지.

참고 원본: `Unity/Client/Assets/_Project/Resources/Main19/Background/BurningPulse_DistantFlameSilhouette.png`. 연결할 것은 체격·어깨 너비·준인간형 비율·규모감이다. 해당 픽셀을 확대/복사하거나 Boss Sheet로 사용하지 않는다. 새 고유 디자인을 제작한다.

## Boss Sprite Sheet — 기존 4×4를 우선

기술 근거: `MonsterSpriteSheetAnimation`의 ExplicitIdle/Attack/Hit/Defeat 배열 및 `Main18ContentBuilder`의 314px 셀 선례. **1256×1256 RGBA / 셀314×314 / 4열×4행 / 총16프레임**은 기본 동작에 안전한 구조다. 위에서 아래 행, 각 행 왼쪽→오른쪽 순서:

| 행 | 프레임 | 내용 / 기존 Runtime 연결 |
| --- | --- | --- |
| 1 | 00~03 | Idle4, 자연스럽게 반복 |
| 2 | 04~07 | Basic / Strike4, Attack 배열, 한 번 재생 |
| 3 | 08~11 | Skill / Core Action4, Hit 배열을 PlaySkill과 PlayHit가 공유 |
| 4 | 12~15 | KO / Collapse4, 한 번 재생 후 마지막 프레임 유지 |

파일 `Veinfire_Colossus_Sprite_Sheet.png`. 발밑 중심 pivot（0.5,0）, 원본 오른쪽 지향, 314 PPU·Point·무압축·8FPS 선례를 적용할 수 있다. 거대한 규모는 새 캔버스 확대 대신 이후 Battle VisualSize/Scale·프레이밍에서 검증한다. Boss의 팔·열핵·KO 파편이 셀을 넘지 않게 여백을 확보한다. 전장 UI/아군/상태표시를 가리지 않는 실제 크기는 구현 QA에서 결정한다.

**3행은 피격과 스킬을 함께 재생한다.** 따라서 피격만으로 공격이 발동한 것처럼 보이는 완성형 방출·대상 타격은 이 행에 넣지 않는다. 중심부 긴장/열핵 반응 같은 공통 자세를 만들고 특정 Skill 효과는 별도 VFX로 분리한다. Walk·수십 프레임의 새 전용 Animator는 요구하지 않는다.

Phase2: `Veinfire_Colossus_Phase2_Sprite_Sheet.png`를 **같은1256/314/4×4/발밑·프레임 대응**으로 납품하는 안을 우선한다. 몸체 균열과 노출된 열핵을 표현하고 HP 회복/부활 디자인으로 보이지 않게 한다. Phase2 배열 교체는 아직 Runtime에 없으므로 소규모 연결 구현이 필요하다. 대안은 기본 Sheet + 고정 열핵 노출 overlay이며 제작 전에 하나를 선택한다. 한 Sheet에 임의 추가 행을 넣어 기존16프레임 규격을 깨지 않는다. 별도 Phase2 Sheet 선택 시 기본16+대응16이며 Skill7개별 추가 동작 시트는 불필요하다.

## Boss Skill VFX — 7종

투명 PNG/Sprite Sheet, **4프레임 이하의 가로 strip**을 기본으로 한다. 권장 셀128×128（단일/예고）또는256×256（광역）, 4프레임이면512×128 / 1024×256. 셀 크기·anchor·순서·peak frame·한 번/loop를 Manifest에 적는다. 환경 영구 부착과 Battle UI 효과는 분리한다. VFX 파일만으로 현재 자동 연결되는 것은 아니며 Presenter용 명시 매핑이 필요하다.

| 파일명 | 의미 / 재생 계약 |
| --- | --- |
| Veinfire_HeatPressureInjection_VFX.png | 단일 대상 압력 주입, 직접 타격 뒤 공용 과열 pulse; 과열 자체 효과를 새로 만들지 않음 |
| Veinfire_MoltenStrike_VFX.png | 단일 강타/열흔, 생존 대상 Burn은 기존 공용 표시 |
| Veinfire_CoreCondensation_Telegraph.png | Boss 중심 느린 응축, 다음 Core Wave 예고; 피해 없음 |
| Veinfire_CoreWave_VFX.png | 생존 아군 전체 직접 파동, 과열/Burn 부여를 연상시키는 잔류 상태 없음 |
| Veinfire_CoreResonance_Telegraph.png | 응축과 구분되는 공명, 전체 직접 피해+과열1 예고; 피해 없음 |
| Veinfire_CoreEruption_VFX.png | 전체 분출, 직접 피해 후 생존 대상 공용 과열 처리 |
| Veinfire_HeatWave_VFX.png | 약한 넓은 열파, 직접 피해만; Burn/과열 잔류 없음 |

예고 PNG는 느린 짧은 애니메이션 뒤 안정 프레임 유지로도 충분하다. 예고 상태가 다음 Boss 행동까지 유지되는 것은 HUD/Runtime 책임이며 VFX 반복 속도나 음성만으로 전달하지 않는다. 빠른 Flash·흰 Strobe·강한 반복 Shake 금지. KO는 본체4행과 Field 잔해 Prop으로 충족하고 추가 Collapse VFX는 선택 사양이며 필수 수십 프레임을 요구하지 않는다.

## 납품 후 확인과 다음 단계

셀 경계/Alpha/행 순서/발밑 정렬·Idle loop·KO 마지막 유지·피격/Skill 공유·Phase2 정렬·7종 VFX 식별·예고 텍스트 가독성·hot/cooled 정렬·실루엣 연결을 검사한다. 기술 적합성과 사용자 미술 승인을 구분한다. 이번 납품/생성0. **다음 단계는 이 Handoff를 아트 담당 세션에 전달하여 정식 Boss / Field11 / VFX를 제작하는 것**이다.
