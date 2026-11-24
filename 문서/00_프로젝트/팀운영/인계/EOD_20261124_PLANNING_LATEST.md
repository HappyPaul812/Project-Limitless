# PLANNING · 2026-11-24 마감 발췌

[원격 인계 기준](https://github.com/HappyPaul812/Project-Limitless/commit/bad055b51cccaae70c5fc973e4ccfa740cec8fb0)

# PLANNING / 2026-11-24 — #131 통합 후보 실행 PASS / 원본 동기화 HOLD

- [Codex #131 최종 결과](https://github.com/HappyPaul812/Project-Limitless/issues/131#issuecomment-7123671090) 직접 확인. 통합 브랜치 `codex/integration-20261124-main25-safe`, 기준 `65cceb2d6c3283d18abec6e27d8a6f1296411f17`, 통합 제품 `5399c89b2be41c2c7ea0a0aedb36b16eb70f3120`, 보고 `9d155ca635143b217a9cdd68a0c3e8be54106cb4`. #85 승인7경로, #124 전투 버튼/Ch1 잡템4, #130 Main24/25 및 음성19 포함. Unity6000.6.5f1 새 Library 클린 Import/C# 컴파일 성공, 실제 Bootstrap 합성 슬롯→Main24/25 완료→Continue 및 대표 UI/음성/BGM **137 PASS / 0 FAIL**. 기존 223/27 전수 재실행0.
- **원본 FF HOLD:** 사용자 미커밋 8경로 겹침. 치유사 .meta 5개는 정규화 텍스트 동일, manifest.json/packages-lock.json/BuildSettings 관련 파일은 후보와 다름. 클린 Import 직후 기존 UnityEditor.Search.SearchInit.IndexationOnStartup ArgumentOutOfRangeException 재현. 게임 실행 후보 PASS와 Editor 오류 해결은 별개. #85 OPEN/HOLD. 원본 main 기준 SHA 그대로, 미커밋303/미추적550/보호23 전후 보존, Save/UserData/Discord/Editor 원본 보호, 원격 Push0.
- 대표가 전달한 한도 화면 기준 5시간 8% 남음, 주간60% 남음, 크레딧0, 초기화까지 2시간(캡처 당시). 기획 권고: 지금 추가 Codex 작업을 발주하지 않고 현재 후보/보고 커밋 보존. 다음 승인/작업 시 8경로의 원본 변경 의도·바이트·충돌을 읽기 전용 비교, 특히 실제 차이3경로만 별도 안전 병합안 검토. 사용자 변경 삭제/강제 FF/원본 설정 변경/Library 삭제/기존 전수 QA 금지. Search 예외 관련 원본 적용 게이트는 별도 기획 결정 전 유지. 아직 로컬 main 동기화 또는 원격 복구 완료 아님.

---

# PLANNING / 2026-11-24 — 안전 통합 및 로컬 동기화 승인 (5시간 잔여24%)

- 대표 요청 “지금 안전한 통합해 5시간 한도가 24%라 안되나?” → [Issue #131 안전 통합 발주](https://github.com/HappyPaul812/Project-Limitless/issues/131). #85 검증 제품7파일, #124 전투 UI/Ch1 잡템4, #130 Main24·25 및 승인 음성19개를 최신 실제 local main 기반 별도 통합 worktree에서 선별 결합. 정확한 한도 사용량은 미확인, 24% 내 완료 보장 없음.
- Unity6000.6.5f1 고정, 통합 후보 커밋 우선, 클린 Import/컴파일 및 최소 통합 스모크 1회. 기존 223/27 PASS 등 전수 반복 금지. #85 Search 예외 재현 또는 제품 검증 FAIL/충돌/한도 부족 시 후보 보존 후 원본 반영 HOLD.
- 안전 검증 PASS 및 원본 보호/충돌0 확인 시에만 local main FF-only/no-autostash 동기화 승인. 사용자 미커밋/미추적·Save/UserData·Discord/키·11/23 복구 ref 보존. 원격 Push 별도 승인, Main26 이후 신규 작업 금지. 실제 실행 및 결과 대기, 통합 완료로 해석 금지.

---

# PLANNING / 2026-11-24 — Main25 승인 음성9 연결 완료 / 안전 통합 게이트

- [#130](https://github.com/HappyPaul812/Project-Limitless/issues/130#issuecomment-7123300696) 및 [#125](https://github.com/HappyPaul812/Project-Limitless/issues/125#issuecomment-7123298974) Codex 최종 보고 직접 확인. Main25 기존 Runtime ID 9개에 승인 WAV9 연결, 원문/화자/Voice/SHA256/PCM24kHz16bit모노 일치. 실제 F3 표식1→표식2→갑충전 직전→정리 대화 Clip9/9 및 순서, 마지막 플레이어 무음 자막 유지. Unity6000.6.5f1 호환 QA 신규 27PASS/0FAIL, 기존 223PASS 재실행0. Main24 WAV10 및 기존 승인 음성 보존, 재합성/API0.
- 브랜치 `codex/ch3-main24-main25-playable-20261124`, 제품 `751c8458b203b55ad71355b5f7d267bd4bc7c4fd`, 보고 `2abf6dfe4002953c414f4ae3e782712df5b1583e`. MAIN24_COMPLETE / MAIN25_COMPLETE / AUDIO_CONNECTED_19_OF_19 / LOCAL_COMMITTED / MAIN_NOT_APPLIED / REMOTE_NOT_PUSHED.
- 다음 제안: #130 구현·음성 및 #124 전투 UI/Ch1 잡템4(제품 549549a1ff197dc74686c7042c50122b661aa0f2, 보고 7d14e4fa94c6b692cec5538c3c0049f23d05aa77)와 #85 검증 복구7경로(제품 6fc46b429159026adf99da686b0b87e909238e19, 보고 c5d9ed202a445ce3ef68c72f0b54cc47c1719b9c)의 관계·충돌·실제 로컬 HEAD를 확인해 별도 통합 후보에서 클린 Import/컴파일 및 최소 실플레이·합성 Save 검증. 이전 223/27 전수 반복 금지. #85 Search 오류의 미해결 상태를 자동 PASS로 바꾸지 않는다. 실제 출력 장치 청취 미검증 유지.
- 원본 main 반영·원격 Push는 대표 별도 승인 후. 기존 11/23 복구 ref, 사용자 미커밋/미추적, Save/UserData/Discord/키 보호. Unity6000.6.5f1 고정. Main26 이후 신규 구현 별도. 이 인계는 통합 완료 또는 실제 Codex 실행을 뜻하지 않음.

---

# PLANNING / 2026-11-24 — Main24·25 정식 플레이 구현 완료 / Main25 음성 승인 누락 정정

- [#130 Codex 실적](https://github.com/HappyPaul812/Project-Limitless/issues/130#issuecomment-7123142095): MAIN24 COMPLETE / MAIN25 COMPLETE, Unity6000.6.5f1 호환 QA 223 PASS/0 FAIL. Bootstrap 합성 Main23 완료 슬롯에서 보급·올빼미 한기·패배/안전복귀·재도전·F3 북진·갑충 갑각·Main25 완료·Save/Continue·도보/휠체어·단회 보상 검증. 실제 원본 클린 Import/Editor Search/출력 장치 청취 미검증.
- 브랜치 codex/ch3-main24-main25-playable-20261124; Main24 `939f0d8239443155f8724bb2a33c508ffd2bed62`, Main25 `c1e7dbf1e2b2e2047fdcd1c2397e817fdcd90594`, Main24 음성 `2dc3ef008d38a8c28d2d1f850189c307d397a4c9`, 통합 보완 `e85835235b7ff8aa558a4e15568387bf5234f445`, 보고 `e16557881daea9843dfada6f7812e407be048f79`. LOCAL_COMMITTED / REMOTE_NOT_PUSHED / MAIN_NOT_APPLIED.
- Main24 WAV10/10 실제 대사 연결 PASS. Main25 9개 로컬 WAV는 #130에서 AUDIO_HOLD. 하지만 당일 10:01 KST경 TTS 담당 세션의 Main25 9개 실행 직후 대표 발언 “다 뽑았고 들었어 좋아.” 및 해당 세션의 9/9 제작·청취 승인 기록 확인. [#125 승인 인계 정정](https://github.com/HappyPaul812/Project-Limitless/issues/125#issuecomment-7123232724), [#130 기존 WAV9 연결 발주](https://github.com/HappyPaul812/Project-Limitless/issues/130#issuecomment-7123233472). 실제 파일 SHA/화자/문장/Runtime ID 일치한 것만 연결, 재합성/API 호출0, 기존181개 보호. 223 PASS 반복 금지, Main25 음성 표적 QA만. 실제 연결 결과 대기.
- #85 호환7경로는 QA 복제본에서만 사용, 원본 #85 HOLD 유지. #124 승인 자산/개인 Save/UserData/Discord/원본 main 보호. Main26 이후 신규 구현 및 main 병합/Push 금지. 향후 대표 승인으로 안전 통합 별도.

---

# PLANNING / 2026-11-24 — Main24·25 정식 플레이 구현 착수 승인

- 대표 “메인25까지 구현 가능한거지?” → 기획 Main24 완료→F3 구현→Main25 완료→연속 플레이 검증 제안에 “그래 고~!” 승인. [신규 Issue #130 실제 구현 발주](https://github.com/HappyPaul812/Project-Limitless/issues/130). Unity6000.6.5f1 별도 worktree, 원본 main/원격 Push 금지.
- 11/23 복구 정본의 Main24 QuestDefinition은 F2 안전 왕복 후보(완료/보상 없음), Main25 정식 Scene/Quest 신규 구현 필요. Main24 보급·서리올빼미 단독 한기 전투·완료, F3 승인 원화3 및 등록 BGM 연결·마력 표식2 조사·얼음갑충 단독 갑각 방어 전투·Main25 완료 목표. 남쪽 입구→북쪽 출구, 실제 도보/휠체어·Save·단회 지급/보상·연속 플레이 검증. 미확정 계약은 HOLD, Main26 이후 구현 금지.
- Main24 TTS Stage10 승인/로컬 해시 일치 보고, Runtime 0/10; Main25 Stage9 준비 기록, 실제 WAV/청취 승인 여부 재확인. 증거 일치하는 음성만 연결, 나머지는 텍스트 진행 및 AUDIO_HOLD; 신규 API 호출/재합성 금지. #85 후보 및 기존 승인 자산 보호.
- [#124 Chapter1 잡템4 격리 적용 마감](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7122478787): 제품 `549549a1ff197dc74686c7042c50122b661aa0f2`, 보고 `7d14e4fa94c6b692cec5538c3c0049f23d05aa77`, 호환 QA 10PASS/0FAIL, White Tint 개선·기존 ID/수량/드롭률/판매가 유지. 제품 클린 Import/전체 Save 미검증, main 미반영. 기본 명령 버튼6(아이템 기존 유지) 완료 보호.
- #123 결빙 배경 채택/화로 실물 비교 PARTIAL은 기존 발주 유지; Main25 구현의 선행 조건 아님. 실제 Codex 실행 결과 대기, 목표 완료 선언 금지.

---

# PLANNING / 2026-11-24 — F5 균열 원화·도망 v2 승인 및 보스 구역/취소 버튼 후속

- Codex 실제 표적 검수 보고 `8fe6e0eab4b16e2f43c37d40969cdb051e285d8c`, [#123](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7121034099), [#124](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7121035060). F5 Edge 파일·개념 배치 PASS, F3 참조 실물2종 당시 PARTIAL. 기획에서 기존 11/23 Library ZIP CRC 정상, GroundTransition SHA `d63c8d5ae60921dc7007caf839b720d68dda8078f273b86f1404f6121475a4be`, LowFrostRock SHA `ddbba749c8f60f3b207769c35883aa844e80e782e8dc512077608d2568f07bb1` 승인값 일치 직접 확인. 사용자 PC의 실물 재검증을 뜻하지 않음.
- 기존 대표의 Codex 이상 없으면 승인 방침에 따라 F5 Edge 원화 채택 및 Flee v2 최종 미술 승인. 실제 F5 Import/Scene/Collider/4.5WU 통행과 도보·휠체어 실주행 HOLD. F5 추가 암반 P2 보류. 도망 v2 파일/포털 구분/Unity 기능 PASS, 보스전 도망 제한 미실행.
- [#123 승인·후속 발주](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7121162958): 마지막 온기의 화로(전장 가칭) 환경 구상, 기존 화로/보스/환경 자산 재사용 우선, 신규 후보 최대2종 제안만. 남쪽 F5 입구→북쪽 보스 구역 세로 주동선·북쪽 화살표·목표 표시. 신규 PNG 양산/Scene/기존 원화 변경 금지.
- [#124 승인·후속 발주](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7121163563): Codex 도망 v2 command.flee 격리 적용, 기존 승인 자산·#85 후보 보호. 리뉴얼은 command.cancel 기본 취소/뒤로가기 128 RGBA 1종 제작. Unity6000.6.5f1 유지, 기존 PASS 반복 최소화, main 병합/Push 금지.
- 기존 스킬 적용 제품 `5deda99eeeb5bb817a22dbee426f5d7d600704ed` / 보고 `1f086de2593991f76ff0d47dd37cc167702a2bdf` 보존. 제품 클린 Import/Save 미검증. 각 담당 실제 실행 결과 대기.

---

# PLANNING / 2026-11-24 — F4 원화·스킬 버튼 조건부 최종 승인 및 다음 발주

- 대표 조건부 결정 “덱스씨가 별 문제 없다고 하면 나도 승인.” Codex 검수 보고 `a4de990615563214fc279936c50e3c1f88d6f2d6`, [#123 F4 검수](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7119159890), [#124 스킬 검수](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7119160974) 확인.
- F4 백색 침엽수·방향성 풍설 오버레이 원화 채택 승인, 파일/Import PASS, 시험 배치 남→북 방향·4.5WU bounds 침범0. **배치 PARTIAL/실제 F3~F5 씬·Collider·도보·휠체어 실주행 HOLD**, 시험 배치를 실제 제품 검증으로 오인 금지. F5 동토의 균열 환경 구상 1장·신규 파일럿 후보 최대2종 제안 발주 [#123](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7119304884). 앞으로 Ch3 환경 개념도 **남쪽 입구→북쪽 출구/북쪽 화살표/세로 주동선** 명시.
- 기본 스킬 버튼 최종 미술 승인, 실제 30/40px 및 메뉴 PASS. 30px 장식 흐림은 수용. Codex command.skill 적용 발주 및 리뉴얼 다음 command.flee 도망 버튼 1종 제작 발주 [#124](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7119305405). 기존 방어 적용 제품 `1d8ec56059458b18a3344eb1753c9f63fc041ae2`, 보고 `97bcc020aba378b5d57056e108f1a9efe2372df0` 보존. 실제 제품 클린 Import/Save 미검증.
- Unity6000.6.5f1/#85 HOLD, 기존 승인 원화/원본 main/Save/UserData/Discord 보호. 승인 원화 재작화 금지, 새 적용 결과 및 부서 납품 대기.

---

# PLANNING / 2026-11-24 — F3 환경 최종 미술 승인 · 방어 버튼 승인 · F4/스킬 후속 발주

- 대표 직접 결정 “나도 승인” → [#123 F3 환경 3종 최종 미술 승인](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7118370238), [#124 방어 버튼 최종 미술 승인](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7118370936). F3 GroundTransition·LowFrostRock 유지, FlushStone v2 SHA256 `e4d6af0f3aa854eeeef591091312ae593e2c928ed31123f4ca83cfe049b704b3` 채택. v1 보존. DefendButton PNG SHA256 `026574c506cf03db37eca4b57a24ba74251db4d6e48f5c868cf421ca69c43413` 채택. 재작화 금지. 실제 F3 Scene/통행·v2 Import 및 방어 버튼 적용은 미검증.
- #124 기존 승인 독침·젤·공격 3종 격리 적용: 제품 `39fa4576e1ae984c09684ac14f56e48f1d541ca1`, 보고 `f96b732c78a82159b84d3c4acf24a757e39fa172`. 호환 QA 복제본 실제 UI/단회 타격 PASS, 30px 침끝 세부 PARTIAL 수용 권고, 클린 Import/Save 왕복 미실행, 원본 main 미통합.
- Codex 다음: 위 3종 보존 후 방어 command.defend 경로만 신규 PNG에 연결, 50% 방어 감소 기능/버튼 표시 최소 검증. 기존 #85 후보 보호 및 원본 main 병합/Push 금지.
- 메인 신규 아트 다음: F4 백색 침엽수 1종 + 방향성 풍설 지면 오버레이 1종 파일럿. 기존 자산 재사용 우선, 통행로 확보, 기존 승인 F2/F3 보존. 신규 맵 양산/Unity 금지.
- 리뉴얼 다음: 공용 스킬 버튼(command.skill, Kenney star) 1종, 공격·방어 화풍 통일, 128 RGBA 및 30/40px 실축소 비교. 기존 원화/코드 보호. 각 부서 발주 기록 완료, 실제 실행/납품 대기.

---

# PLANNING / 2026-11-24 — 독침벌 침·초원 슬라임 젤 최종 미술 승인 및 공격 버튼 포함 3종 적용

- 대표 직접 승인 “내가 승인 안했어? 미안..;; 승인..;” → 독침벌 Organic v2 원본과 초원 슬라임 젤 원본 `USER_ART_APPROVED`. White Tint 권고 채택, TipOnly 59px 후보 미채택 보존. 기존 원화 재제작 금지.
- [#124 승인 및 Codex 적용 지시](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7117133033): 승인 전리품2 + 기존 승인 공격 버튼1만 별도 `codex/issue124-loot2-attack-icon-20261124`에 연결. 실제 PNG SHA 확인, ItemDefinition icon/White Tint와 command.attack 경로만 변경, 실제 Unity 6000.6.5f1 UI 최소 검증. 기존 itemId/드롭/Save/외부 원본·기존 GUID/meta 유지, 방어 버튼 제작 중으로 제외.
- `ISOLATED_APPLICATION_AUTHORIZED / CODEX_EXECUTION_PENDING / MAIN_NOT_APPLIED`. #85 수정 후보 및 기존 원본 main·개인자료 보호, 패키지 임의 변경/병합·원격 Push 금지. 실제 적용 결과 및 제품/보고 SHA 별도 보고 필요.

---

# PLANNING / 2026-11-24 — 기본 공격 버튼 대표 최종 미술 승인 · 방어 버튼 후속 제작

- 대표 직접 결정 “나도 봤는데 승인해도 돼.” → [#124 공격 버튼 최종 미술 승인](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7116923432). `AttackButton_Original_128_RGBA.png` SHA256 `cf775727e27be825809a6b3d9bd04b227abe3408055873f70616cc8a9ca19a32`, `USER_ART_APPROVED / UNITY_NOT_APPLIED`. 원본 보존, 재제작 금지.
- [기본 방어 버튼 다음 발주](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7116871218): 기존 Kenney shield 대체 오리지널 방패 1종, 공격 버튼과 화풍 통일, 철벽/수호의 맹세/가이아 웰과 구분, 128 RGBA·30/40px 비교·ZIP/Manifest/SHA 제출. 미납품·미승인.
- 공격 버튼의 실제 Unity 적용·기존 Kenney 대조·GameView QA는 별도 단계. #85 HOLD 및 main 보호. F3 또는 다른 리뉴얼 후보 승인을 의미하지 않음.

---

# PLANNING / 2026-11-24 — 바울 대표 #124 공격 버튼·#123 F3/F4 ZIP 실물 기획 검수

- 바울 대표 로컬 납품 파일명 2개 전달. 기획에서 ChatGPT Library의 동일명 ZIP 실물 열람 및 Manifest·SHA256SUMS·CRC 검증 완료. [기획 실물 검수 정본](../../20261124_Issue123_Issue124_Art_Delivery_Review.md) 커밋 `a5ac0ff3f6d1e7417224b5e789ddab68915ff359`.
- **#124 AttackButton** `LIMITLESS_Issue124_AttackButton_Original_20261124.zip` 1,659,563B SHA `14aeea5420912bbfc4c996f108a96db36058f34433324e02321c5de32b1ba9c7` CRC10/Manifest8/SUMS9 PASS. 128×128투명 아이콘(검 은색·손잡이 금색·붉은 참격) 실제 30/40px 오프라인 다크/라이트 검토 `PLANNING_ART_PASS / REDRAW_NOT_NEEDED`; 실제 Kenney 원본 1:1 및 GameView `NOT_VERIFIED`. 대표 최종 미술 승인·Unity 제품 교체 HOLD. [#124 검수 기록](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7116768391).
- **#123 F3→F4** `LIMITLESS_Issue123_F3_FinalArtReview_F4Concept_20261124.zip` 1,113,445B SHA `191d9f52c6c4baba35cb1cd276ffa98573b9c64875baa5db1463320438520098` CRC8/Manifest6/SUMS7 PASS. 기존 11/23 F3 PNG3 원본 SHA 동일 확인, 원본수정0. GroundTransition(단일 오버레이), LowFrostRock(길 밖) **기획 시각 PASS**. FlushStone EdgeMarker 중앙 청색은 물웅덩이/위험 오독 우려로 **색상만 저채도 회청색 국소 v2 옵션 후보** 요청, v1 원본 보존/30·40px 비교/대표 별도 채택. F4 백색침엽림 풍설·수목 그늘·중앙 통행로 팔레트는 **기획 CONCEPT PASS**이며 신규 게임용 PNG/맵 승인 아님. [#123 검수·후속 기록](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7116769430). F3 Scene/4.5WU 실제 도보·휠체어/Collider/Sorting/GameView NOT_VERIFIED, 최종 대표 미술 승인·Unity 적용 HOLD.
- 두 ZIP은 ChatGPT Library 동일명 자료를 확인한 것이며 사용자 PC F:의 바이너리 직접 실측은 아님. Codex #85 수정후보·Unity6000.6.5f1·기존 11/23 복구/원본 main, Save/UserData·Discord·기존 승인 PNG/WAV/meta/GUID 보호. 새 Codex QA·Unity 작업 발주하지 않음.

---

# PLANNING / 2026-11-24 — Issue #85 패키지·GUID 검증 수정 7파일 로컬 커밋 보존 완료

- 바울 대표가 Codex 최종 보존 결과 전달. [Issue #85 Codex 실적 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/85#issuecomment-7116392287) 직접 확인.
- 격리 브랜치 `codex/issue85-unity60006-package-guid-repair-20261124` **제품 commit `6fc46b429159026adf99da686b0b87e909238e19`**, 보고 commit `c5d9ed202a445ce3ef68c72f0b54cc47c1719b9c`. **LOCAL_COMMITTED / REMOTE_NOT_PUSHED / LOCAL_MAIN_NOT_APPLIED**.
- 제품 변경 정확히 **7경로**: `Unity/Client/Packages/manifest.json`, `Unity/Client/Packages/packages-lock.json`, `Assets/_Project/Resources/BattleProjectiles/HealerMagicalProjectile/healer_magical_projectile_00..04.png.meta` 5개. 승인 PNG/개인 설정/자동 Import 생성물 미포함. `GoldenArrow/golden_arrow_00.png.meta` 기존 GUID 잠금 유지, Healer5는 원본에서 실존하던 고유 GUID 재사용. 16개 보호 파일 SHA/lock 변경20항목·diff 체크 통과.
- **검증 PASS (선행 증거 재사용):** Unity6000.6.5f1 UPM Animation16.0.1/PSDImporter15.0.1/Tooling4.0.1 Resolve·C#오류0·Tooling Import예외0/UI33/33, 전체 Asset GUID 중복0, GoldenArrow8/Healer5 실제 Sprite 로더·공격 프레임, 빙각 KO/단회 피해·F2 BGM Intro→Loop 표적. 새 QA 강행·새 Unity 재실행 없음.
- **잔여 HOLD:** `UnityEditor.Search.SearchInit.IndexationOnStartup` 예외가 즉시 Bootstrap Play에서는 1건, 5초 지연 Play에서는 0건. 두 테스트 시작화면 업데이트는 계속 됨. 정확한 원인/실제 일반 Editor Play·Standalone·시작화면 이후 게임 흐름 미검증. 캐시·실행순서 완전통제 없음. **ISSUE85_OPEN / SEARCH_EDITOR_FOLLOWUP / GENERAL_PRODUCT_QA_NOT_VERIFIED**. C# 컴파일 오류가 잔존한다고 쓰지 않는다.
- **차기 게이트 제안:** 패키지·GUID 수정을 반복하지 말고 대표의 실제 게임 사용 또는 향후 단 1회 범위 제한된 정상 Play 검증으로 일반 플레이 영향 구분, 결과 보고 후 **별도 사용자 승인 하에서만** 로컬 main 적용·11/23 복구 체크포인트 후속 새 안전 백업/원격 Push 결정. 검색 인덱싱 옵션/Library 삭제/임의 패키지 업데이트로 오류를 감추지 말 것.
- 원본 `main` `65cceb2d6c3283d18abec6e27d8a6f1296411f17`, Git 상태853 경로, 기존 11/23 복구 ref 및 승인 자산/Save/UserData·Discord·기밀 불변, `main` 병합·원격 Push/Issue 닫기 없음.

---

# PLANNING / 2026-11-24 — #85 Bootstrap Search 조건부 예외, 기술 복구7파일 안전 커밋 우선

- 바울 대표가 Codex 11/24 Unity 6000.6.5f1 격리 Bootstrap 2회 결과를 전달. [원문](https://github.com/HappyPaul812/Project-Limitless/issues/85#issuecomment-7116282657): 즉시 Play에서 `UnityEditor.Search.SearchInit.IndexationOnStartup` `ArgumentOutOfRangeException` 1건 발생하나 시작 메뉴는 6초 진행, Play 요청을 5초 지연하면 예외0·시작 메뉴 6초 진행. 모두 `IndexOnStartup=True`; 캐시/실행순서 통제 미완, 일반 인터랙티브 Editor·Standalone·타 화면 검증 미실행. Editor Search 초기화 타이밍 관련 **가능성만**, 원인·사용자 실제 게임 무해 확정 금지.
- 유지된 기술 PASS: `Animation16.0.1 / PSDImporter15.0.1 / Tooling4.0.1` Unity UPM, C#0·Import0·ToolingUI33/33, Healer5 고유GUID+GoldenArrow 원GUID·중복0, GoldenArrow8/Healer5 런타임·빙각 KO·F2 BGM 최소 회귀.
- [**기획 최종 지시: Codex 제품 후보 안전 보존·Search 분리**](https://github.com/HappyPaul812/Project-Limitless/issues/85#issuecomment-7116338215). 현재 격리 worktree 미커밋 `Unity/Client/Packages/manifest.json` + `packages-lock.json` + `HealerMagicalProjectile/*00..04.png.meta` **7경로만** 별도 제품 커밋, 관련 결과/인계 문서 별도 보고 커밋하도록 지시. 이미 PASS한 Play/Import 반복 금지, 원본 main `65cceb2d6c3283d18abec6e27d8a6f1296411f17`, 11/23 원격 복구 SHA·사용자 미커밋/미추적·Save/UserData·Discord/Unity6000.6.5f1 유지. 이 지시로 로컬 실제 커밋 실행됐다고 선언하지 않는다.
- Search 예외는 **EDITOR_SEARCH_FOLLOWUP**, 일반 Editor/실게임/Standalone 미검증. `#85 OPEN/HOLD` 유지, 대표의 제품 적용 승인 전 main FF·원격 Push·Issue Close 금지. 임의 패키지 버전업/Editor prefs/검색 DB 캐시 삭제 및 신규 회귀전수 금지.

---

# PLANNING / 2026-11-24 — 바울 대표 SFX 중단 · 영상 월간 1회 / #85 기술 PASS와 Search 에러 HOLD 분리

- **바울 대표 운영정책 변경:** “sfx는 포기 상태, 영상은 한달에 한번만 하면 돼... ^^”. 이전 11/24 SFX #33 제작 지시는 **취소**, #33 `closed/not_planned` [공식 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/33#issuecomment-7116164370). 신규 SFX 제작·지시·도구·비용 투입0, 과거 자산 삭제0. 개발기록 영상은 **월1회**, 11/16 Raw 촬영/QA 존재하므로 이번 11/24 추가 비교편집 발주 **보류**, 12월 월간 작업에 대표 승인 따라 반영 [#98](https://github.com/HappyPaul812/Project-Limitless/issues/98#issuecomment-7116165098). 11월 영상 Final 업로드/승인을 추정하지 않음.
- [병렬 발주 문서 정정](../../20261124_Parallel_Department_Orders_While_Issue85_Repair.md) `docs/planning` 커밋 `cd512078667175e251c77b8f2aff8b7c3a690f06`. 현재 실제 병렬 추진 부서는 **메인 신규 아트 #123, 리뉴얼 #124, TTS #125, BGM #129** 4개만. Codex와 Unity 파일 충돌없음.
- **#85 Codex 11/24 실제 결과** [원문](https://github.com/HappyPaul812/Project-Limitless/issues/85#issuecomment-7115971873): Unity 6000.6.5f1, UPM Animation16.0.1/PSD15.0.1/Tooling4.0.1 `PACKAGE_RESOLVE_PASS`; 클린 C#오류0/Tooling Import예외0/UI33/33, Assets meta GUID중복0/GoldenArrow8·Healer5 실사용 PASS, 빙각 단타·KO/F2 BGM Intro→Loop 표적 PASS. **전체 Unity QA는 Editor Search `SearchInit.IndexationOnStartup` `ArgumentOutOfRangeException` 1건으로 FAIL / #85 HOLD**. 예외가 사용자 일반 Play에 영향을 주는지는 아직 미확정. 후보 작업 격리 worktree에 **미커밋으로 보존**(제품·보고 commit SHA없음), 로컬 main `65cceb2d6c3283d18abec6e27d8a6f1296411f17` 원본 Git상태853 유지/Push0.
- **기획 판단:** 패키지·컴파일/Import/GUID 해결 성공은 정확하게 기록하면서 Search Editor error **독립 재현·범위 제한**만 [#85 최소 후속 지시](https://github.com/HappyPaul812/Project-Limitless/issues/85#issuecomment-7116165847). 공식 Unity Search 환경설정에 시작시 인덱싱 옵션 존재; 오류 해결 확정 아님, 격리 환경 1~2회 비교만 허용, 원본 Library/Editor prefs/Packages 캐시 변경·대규모 QA 금지. 전부 PASS·대표 적용 승인 전 원본 main 반영/원격 Push/Issue 닫기 금지.
- 현재 모든 역할 문서화는 기획 `docs/planning` 기록만이며 타 담당 실제 실행은 별도. Unity6000.6.5f1, 11/23 복구 ref, Save/UserData·Discord/개인 API·기존 승인 PNG/WAV·미커밋303/미추적550 유지.

---

# PLANNING / 2026-11-24 — Codex #85 복구 중 타 담당 6부서 병렬 지시

- 바울 대표 직접 지시: “코덱스는 열심히 하고 있고, 딴 담당자들 일 하자.” → #85는 Codex 단독 복구 유지, 기획이 신규아트·리뉴얼·TTS·BGM·SFX·개발기록영상 작업 분리.
- [**기획 실행 정본**](../../20261124_Parallel_Department_Orders_While_Issue85_Repair.md) `docs/planning` commit `d1672ee3bee5856bc6078d8024b6dc7a2e02fdfe`. GitHub 실제 발주: [#123 F3 최종 보드·F4 개념](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7115938974), [#124 Kenney 공격버튼 신규 오리지널 PNG 1파일럿](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7115939762), [#125 Main24 Stage10 실물 인계·Main25 9건 무호출 사전큐](https://github.com/HappyPaul812/Project-Limitless/issues/125#issuecomment-7115940550), [#129 F4 BGM1 브리프·Gemini 프롬프트](https://github.com/HappyPaul812/Project-Limitless/issues/129), [#33 북풍/걸음/온기 SFX 소량 파일럿](https://github.com/HappyPaul812/Project-Limitless/issues/33#issuecomment-7115941309), [#98 11/16 Raw 개발기록 비교편집 후보](https://github.com/HappyPaul812/Project-Limitless/issues/98#issuecomment-7115942073).
- #123 기존 F3 원본3와 #126 빙각 5모션, #124 Trait5·Stinger/Gel, 기존 승인 BGM4/WAV8·TTS Stage10 및 기타 공식 Monster·NPC·맵 전부 보호. 신규 art1만 파일럿, F4 신규 영상/맵 PNG 양산 금지. TTS 신규 API0, BGM F4 프롬프트/브리프만, SFX 사용자 청취 승인 전 Unity0, video YouTube 공개0.
- **이번에는 Unity/Package/GUID/meta/원본 main/원격 Push 작업을 어느 부서에도 배정하지 않음**, #85 Codex 격리 복구와 작업 파일 비충돌. 기준 복구 ref `backup/2026-11-23-safe-checkpoint` `65cceb2d6c3283d18abec6e27d8a6f1296411f17` 보존. 각 부서가 실제 사용자 ChatGPT 역할 세션에서 작업을 시작한 것으로 자동 해석하지 않는다. 사용자에게 복붙 지시문 제공. 결과 브랜치/커밋/SHA/검증/사용자승인·Unity 적용 여부 별도 보고.

---

# PLANNING / 2026-11-24 — Issue #85 2D 패키지 CS1061 · Tooling 경로 · 투사체 GUID 격리 복구 승인

- 바울 대표 “기존 Animation 15.1.0 컴파일 오류로 #85 HOLD는 미해결 건 해결 할 수 있니? 니가 판단해보고 할 수 있겠으면 코덱스 시켜.” → 기획 검토 결과 **수정 가능성 높음**, Codex **격리 제품수정 파일럿**을 명시 발주. [정식 지시](../../20261124_Issue85_Animation_PSD_Tooling_GUID_IsolatedRepair_Order.md) 커밋 `ff35e095f950a201a5f781960dee74196b807dc6`, [Issue #85 업무 지시](https://github.com/HappyPaul812/Project-Limitless/issues/85#issuecomment-7115654109).
- 근거: 11/23 복구 `65cceb2d6c3283d18abec6e27d8a6f1296411f17`의 Unity6000.6.5f1 + Git `manifest/lock` Animation15.1.0/PSD14.0.3/Tooling3.0.1. 공식 Unity6000.6 권장 패키지 Animation16.0.1/PSD15.0.1/Tooling4.0.1. 11/12 격리 시험에서는 첫 두 업데이트로 C# CS1061 7개 소멸, Tooling3 USS/UXML 경로 예외8/12. 11/17 실 사용자 미커밋 working tree에 3업데이트·GoldenArrow+Healer GUID6고유 이미 있었으나 전체 UPM lock13/27차이·짧은 경로 영향과 Runtime 검증 미완료. 자동 복제/재설치 금지.
- Codex는 오늘 최신로컬 안전 베이스 및 원본 미커밋303/미추적550/Save/UserData·Discord/키/TTS/BGM/승인아트/11-23 복구 ref 보호 후 짧은 경로의 별도 `codex/issue85-unity60006-package-guid-repair-20261124`에서만 **패키지 manifest/lock 3종 UPM 정합 해결 + GoldenArrow 정본 GUID 보존·Healer5 meta 충돌 수정**. 실제 clean checkout Unity6000.6.5f1 Batch C# 컴파일0·Tooling Import예외0·GUID0·GoldenArrow8/Healer5 Resources runtime PASS 및 최소 빙각/BGM 회귀 증거 요구. 패키지 추가 드리프트·GUID 참조 이상·검증오류 시 STOP/HOLD.
- **`ISOLATED_REPAIR_ORDER_POSTED / CODEX_NOT_YET_RUN / ISSUE85_HOLD`**. 대표 원본 main 동기화·원격 Push/이슈 닫기는 **격리 전체 PASS 후 별도 기획 판단/승인**. 11/23 제품 11PASS/0FAIL 및 기존 복구원격 SHA 유지, 사용자 PC 직접 실행했다는 주장 금지.

---
