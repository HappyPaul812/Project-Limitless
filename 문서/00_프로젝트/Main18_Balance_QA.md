# Main18 Balance QA

승인 수치 변경 0. 갑충 Lv13 HP250 Attack24 Agility8 EXP50 Talent14, 감시자 Lv14 HP560 Attack28 Agility11 EXP85 Talent22. 프로젝트는 기본 Attack10의 배율 데이터를 사용하므로 asset의 240/280%가 실제 Attack24/28을 만든다. Loot 없음·분양 미등록·IsBoss=false·Flee 가능. 보스 및 미래 Content 추가 0.

5 Job × 지정 Monster2 = **실제 Battle10/10 정상 승리**. 격리 Save, 기존 성장식과 저장 파티·편성, 기본 공격 버튼과 실제 대상 선택을 이용했다. 새 Skill이나 피해 치트로 승리를 만들지 않았다. 아래 HP는 한 대표 실행 결과이고 치유사/수호자 수치가 무작위 Turn tie와 일반 대상 선택을 포함하므로 모든 편성의 난이도 보장이 아니다.

| Job | Monster | 검증 조건 | 종료 HP Player,Serin,Miel | 결과 |
|---|---|---|---|---|
| guardian | beetle | Lv14 / 세린·미엘 | 125,173,169 | 정상 승리 |
| guardian | watcher | Lv14 / 세린·미엘 | 61,157,153 | 정상 승리 |
| healer | beetle | Lv14 / 세린·미엘 | 95,173,169 | 정상 승리 |
| healer | watcher | Lv14 / 세린·미엘 | 33,157,153 | 정상 승리 |
| sharpshooter | beetle | Lv14 / 세린·미엘 | 145,173,169 | 정상 승리 |
| sharpshooter | watcher | Lv14 / 세린·미엘 | 95,157,153 | 정상 승리 |
| fighter | beetle | Lv14 / 세린·미엘 | 158,173,169 | 정상 승리 |
| fighter | watcher | Lv14 / 세린·미엘 | 108,157,153 | 정상 승리 |
| mage | beetle | Lv14 / 세린·미엘 | 132,173,169 | 정상 승리 |
| mage | watcher | Lv14 / 세린·미엘 | 82,157,153 | 정상 승리 |

주요 통합 경로에서는 두 지정 전투 모두 도망·전멸·복귀·재도전·정상 승리와 중복 EndBattle 보상0을 확인했다. 지정 Tutorial/일반 Beetle은 별도 stable Encounter ID로 구분한다. 일반3종 Respawn은 기존35초 정책·Spawn 서비스를 사용한다.

AI 순서는 각12행 검사: Beetle80%+열→기본→기본, Watcher85%+열→105%+열→55%전체(열0)→85%+열. 실제 Controller에서 최고 과열의 후열 대상→Burst, 도발 우선, 광역의 열 변화0을 검사했다. 동률 선택은 기존 ChooseEnemyTarget을 그대로 사용한다.

최대HP1/12/13/50/99/100/101/250/560/1000000 × 보호없음/방어/철벽/가이아/수호 = 50조건, 0→1→2→ceil8%→0 및 보호 예산 미소비를 검사했다. 실제 정화/네 해제약은 과열을 유지하고 냉각약은 침묵 중 과열만 제거한다. 무열 대상에 대한 실제 Controller 입력은 수량·현재 Actor·Turn Queue 불변을 확인했다.

5 Path 관찰은 각 Player 무음·공통 세린·동일 Quest 결과를 검사했다. 피해 자체는 Path/펫 Direct/DoT API에 진입하지 않는 독립 HP 경계다. 저장 파티 변경·임시 편성 추가 0, 기존 전열/후열/도발 규칙을 우선한다. 이번에는 기존 전체 50조합 회귀를 무조건 재실행하지 않았다.

[Balance Runtime](Main18_Balance_Runtime_Results.txt) · [통합 Runtime](Main18_Runtime_Results.txt). 사용자 난이도 체감과 실물 키보드/마우스/게임패드 조작은 별도 확인이다.
