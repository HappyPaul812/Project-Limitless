# Player Sprite 자동 매핑·저장 계약

2026-10-03 사용자 설계 정정. Player는 Appearance를 직접 선택하지 않는다. 정본 선택은 Gender / Name → Path → Job → Confirm이며, GenderStableId + PathStableId + JobStableId로 대응 Sprite Definition과 기존 Appearance Stable ID를 자동 결정한다. 배열 순번이나 UI 순번은 조회 키가 아니다.

## 50조합과 Stable ID

Male/Female × Vision/Hearing/Intellectual/Mobility/EmotionalScar × Fighter/Guardian/Healer/Mage/Sharpshooter = 50조합이다. Asset 메타데이터의 Marksman은 Job ID `sharpshooter`, Physical은 `path.mobility`, Visual은 `path.vision`, Heartscar는 `path.emotional-scar`에 대응한다. README/CSV/Inventory를 근거로 연결하며 그림으로 추론하지 않는다. 기존 Appearance ID, PNG50개, Sprite800개, Clip과 공용 Animator 구조를 유지하고 재Import·재Slice하지 않는다.

Path Theme는 실제 Player Path의 시각 디자인이다. 모든 Path×Job 조합을 허용하며 추천 직업은 안내다. Story/Quest/PathSymbol은 Player Path Data의 Stable ID를 직접 사용하고 Sprite에서 Path를 역추론하지 않는다.

## UI와 Preview

전체/테마 필터, 외형 카드·목록, 이전/다음 외형, 수동 Appearance 선택·확정·번호 표시를 제거한다. Preview는선택 결과를 보여주는 `캐릭터 미리보기`다. Gender/Path/Job 변경에 따라 자동 갱신한다. Path/Job 미선택 시 기존 성별 기본 Sprite를 사용하며 임의 조합을 자동 선택하지 않는다.

## Runtime·검수 상태

Intellectual4/Vision5 최신 수정판 적용 후에도 해당9종은 미술/방향 Blocked이며 전체 Ready21/Blocked29를 유지한다. 기존 ID/자동 매핑/Save와 다른41종 상태는 보존한다. [9종 QA](Intellectual_Vision_Fixed9_QA.md).

매핑 정확성과 Art Ready는 분리한다. 현재 50조합 중 Runtime Ready21 / Art Blocked29이며 정확한 프레임·사유는 [QA](Player_Appearance_Foreground_QA.md)를 따른다. Mobility10종은 제공 수정판 교체 후에도 분리 조각·머리 절단으로 BLOCKED_ART이며 [수정판 QA](Mobility_Player_Sprite_Fixed_QA.md)에 기록한다. 다른40종 판정은 유지한다. Blocked도 고유 Definition/ID를 조회한다. 실행에는 기존 Male/Female 기본 Sprite를 **임시 fallback**으로 쓰고 Preview에 이를 표시한다. 다른 Path/Job의 Sprite 또는 공통 Mobility Sprite로 대체하지 않는다. 기술 QA 필드 `readyForSelection`은 직렬화 호환 때문에 유지하되 RuntimeReady로 읽으며 선택을 제한하지 않는다.

World는 대응 Ready Definition의 Clip을 공용 성별 Controller에 Override한다. Battle은 같은 World Controller의 Left Idle을 사용하는 기존 전달 구조를 재사용한다. 50개 Animator Controller를 만들지 않는다.

## Save·Continue

Gender/Path/Job/Name이 정본이고 AppearanceId는 자동 계산 결과를 저장하는 호환 필드다. Continue는 정본 조합으로 재조회한다. 이전 수동 AppearanceId가 누락·invalid·blocked·다른 조합이어도 정본 조합을 우선한다. Path/Job이 없는 오래된 Save는 기본 성별 Sprite로 fallback한다. Save Version1 및 기존 슬롯은 유지하고 삭제·초기화하지 않는다.

## 검증

50개 조합의 Combination / Appearance ID / Sprite Definition / Runtime Ready / Validation Status Matrix, Duplicate0 / Missing0, 남25·여25·Path별10·Job별10을 검사한다. 대표 5Path의 Preview·생성·World·Save→Continue·Battle, 구버전 및 불일치 ID 호환, Story Path 판정, Compile/Console을 백그라운드 우선으로 검사한다. 화면 포커스 전환은 이번 작업에 별도 승인 없이 실행하지 않는다.
