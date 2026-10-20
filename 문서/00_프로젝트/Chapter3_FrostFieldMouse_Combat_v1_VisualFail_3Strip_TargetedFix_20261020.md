# LIMITLESS — Chapter3 서리들쥐 A 전투 3 Strip v1 격리 QA 미술 FAIL·표적 보완 (2026-10-20)

- 담당: 기획 docs/planning. 실제 PNG 재작업: docs/art-new. GitHub Issue #30.
- 바울 대표가 2026-10-20 전달한 Codex 동일 전투 표시 배율 비교(Idle00/Attack02/Hit01/Defeat03 각 112px) 및 격리 Unity QA 보고에 근거. 이 기획 세션은 로컬 PNG ZIP 원본 및 Codex 실제 애니메이션을 독립 실행해 본 것이 아님.
- Codex branch: codex/chapter3-mouse-combat-v1-review-20261020. QA commit da55fe6e / 인계 036037e8. 원본 ZIP SHA256: 24ef8dddf76fb5843e8e7d3fac05942f1450c9f39cf366b2422a0ab82cba6741.
- 격리 Unity 모션 상태전환/Import 기술 **36 PASS / 0 FAIL**, ZIP/PNG 규격 PASS. **시각 판정 Attack FAIL / Hit FAIL / Defeat HOLD / 승인 Idle과 색상·스타일 일관성 FAIL**. 원본 승인 1포즈·Idle/Save·UserData·Editor 불변, 원본 게임 적용·Push·main 병합0.
- 상태: COMBAT_V1_FILE_TECH_PASS / ISOLATED_UNITY_STATE_QA_PASS36 / ATTACK_ART_FAIL / HIT_ART_FAIL / DEFEAT_ART_HOLD / IDLE_CONTINUITY_ART_FAIL / COMBAT_ART_FIX_REQUIRED / USER_APPROVAL_PENDING.

## 결함 — 변경해야 할 정확한 위치

1. **Attack 프레임 00 / Hit 프레임 03**: 모션 시작·복귀는 **승인 Idle00과 같은 형태/크기/기준선**이어야 하나 현재 가시 크기가 약 **18~20% 작아** Unity 전환 시 캐릭터가 갑자기 작아졌다가 다시 확대되는 점프가 보임. **기존 Idle00 프레임 RGBA를 경계 프레임 기준선으로 삼아 동일 위치·알파 bbox·색상·발 접지 Anchor로 수정**; 필요시 해당 경계 프레임을 승인 Idle00에서 그대로 사용(본격 Attack 01~02/Hit 00~02 자체 포즈는 보존하면서 경계값 보간). 프레임 셀 512×512 / PPU512/하단중앙 변경 금지. 전체 Image를 임의 20% 키워 여러 포즈의 모션을 왜곡하는 '무조건 확대'는 금지.
2. **Attack 프레임 01·02 타격 시점**: 가장 강한 할퀴기가 **01에 위치**해 기술 계약의 **타격 프레임 02**와 맞지 않음. 00 준비→01 몸과 앞발 치켜들기→**02 최대 전방 할퀴기(명확한 임팩트)**→03 반동·Idle 전이 순으로 타이밍/실루엣 수정. 코덱스의 실제 타격 이벤트 호출/프레임 매핑을 임의로 바꾸지 말고 원화 프레임 02에 맞춘다.
3. **Attack/Hit/Defeat 세 시트 색상·체형**: 신규 전투 원화에서 **갈색 및 비중화된 털색**이 두드러지며 기존 공식 Idle은 **차가운 회백색 털**. 털 팔레트/명암·눈·귀·앞니·긴 꼬리·발톱·가시 크기/체형을 승인 Idle 기준으로 통일. 기존 동작의 물리적 자세 변화는 보존하고 무조건 전장 일괄 색 필터만 씌워 명암·눈·코를 훼손하지 않도록 한다.
4. **Defeat**: 03의 바닥에 누운 사망 고정 자세·03 hold 기능 자체는 **정상**으로 확인; 이 포즈를 다시 그릴 필요는 없으며 **색/외형·크기 연속성만** 조정. 00~02의 쓰러짐 단계가 정상인지 새 미리보기로 확인하되 03 final pose 유지.

## 재제작 범위 및 실제 납품

- **수정 대상은 Attack, Hit, Defeat 3개 Strip 내 지정 결함 프레임·팔레트**. 기존 승인 Mouse Idle `Sprites/FrostFieldMouse_A_Idle_4Frames.png` SHA256 `5afebfa1f95ca5673162513fdea63b025546aa836550c0ceb17f0334659988f2`/승인 1포즈 `0516914ddad36af5854065d66d01b9664329aa89eea8b394480eed6af92e32c2` **바이트 변경 불허**.
- Strip 3장 게임용 2048×512 RGBA (4×512 셀), PPU512 Pivot bottom-center(0.5,0), VisualScale1, 우향1, 8FPS/프레임125ms. Attack/Hit→Idle, Defeat03 Hold. 기존 프레임 수/Import 좌표/명칭 불변.
- **제작 작업을 실제로 해야 한다**: Photoshop/픽셀아트 등 원본 편집 방식은 담당 자율, 원본 실 PNG3와 승인 Idle PNG를 직접 열어 대응시킬 것. 조잡한 이미지 전체 확대·단순 색 보정·모션 전체 폐기는 금지.
- 새 ZIP `Limitless_Chapter3_FrostFieldMouse_Combat_3Strips_Fix_v2.zip` 및 실 PNG3, v1/v2 프레임별 변경 요약과 정확 SHA, Idle00/Attack00/Hit03 알파 bbox·가시 크기 비교 숫자, Attack02 타격 순간/Defeat03 자세 확대 컨택트 시트, Idle→Attack/Hit/Defeat 112px 또는 실제 게임 표시 배율의 시각 비교/8FPS APNG, ZIP CRC/PNG/testzip/셀 경계/발 접지 점검. 실제 사용자 PC ZIP 전달과 docs/art-new 보고 브랜치/커밋을 분리.
- 아트 담당은 v2를 **완성품·Unity 승인**으로 단정하지 않고 `ART_FIX_V2_DELIVERED / CODEX_DELTA_QA_PENDING / USER_COMBAT_ART_APPROVAL_PENDING`으로 보고.
- 이미 격리 **36 PASS**한 기술 상태전환/기능 테스트를 전수 반복하지 않는다. 새 PNG가 오면 Codex는 기술 규격 변경이 없는지 SHA/PNG3 검사 + **시각 핵심: Idle00 ↔ Attack00/Hit03 크기 점프0, Attack02 타격 프레임, 냉회백 털색·서리들쥐 정체성, Defeat03 HOLD**만 8FPS 같은 UI 배율에서 짧게 Delta QA하고 사용자에게 4컷 미리보기 제시. 필요하면 변경분 코드 경로만 최소 검사.
- **금지:** 사용자가 승인한 1포즈·Idle·설빙의 폭군 Idle, F1/서릿길 마을 PNG/Chapter1·2/Field09 리뉴얼 자료·MonsterDefinition/Quest/Scene/Save/UserData/원본 Editor 변경; 새 F1 몬스터 정식 코드 구현; Push 및 main 병합.
