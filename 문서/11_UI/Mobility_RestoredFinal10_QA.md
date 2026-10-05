# Mobility 최종 복원10종 QA

입력 `F:\Downloads\Limitless_Mobility_Path_10_Restored_Final.zip`, SHA256 `55787cb4832542b6e782db5da7eeba91b375c6be4eecbd80b6898036dccb2ef8`. 문서화→원본10PNG 교체→검증. PNG10/Male5/Female5/Job5×Gender2/512×512RGBA/4×4/16Frame/Cell128, 중복0/누락0, 이미지수정0.

2026-10-05 Mobility 최종10종: READY1/BLOCKED_ART9, 전체READY41/BLOCKED9. 다른40종 보존. [최신 QA](Mobility_RestoredFinal10_QA.md).

|Path|Job|Gender|판정|남은0-basedFrame|사유/다음 이미지 세션 최소수정 리스트|
|---|---|---|---|---|---|
|Mobility|Fighter|Female|BLOCKED_ART|1,5,6,9,11,13-15|셀 옆 잘린 검 파츠/Up13-15 좌측 분리 검끝. Frame1 우측 미세 잔여점. F1: [[125, 115, 126, 117]]; F5: [[122, 60, 126, 65]]; F6: [[124, 58, 126, 62]]; F9: [[2, 54, 7, 61]]; F11: [[2, 56, 6, 62]]; F13: [[3, 81, 10, 95]]; F14: [[4, 88, 10, 99]]; F15: [[5, 85, 12, 99]]|
|Mobility|Fighter|Male|BLOCKED_ART|1,5-7,9-11,14,15|Left5-7/Right9-11/Up14-15 셀 좌측 잘린 검/장비 파츠. Frame1 좌측 잔여점. F1: [[2, 77, 3, 80]]; F5: [[2, 62, 6, 79]]; F6: [[2, 66, 6, 80]]; F7: [[2, 63, 7, 81]]; F9: [[2, 45, 6, 53]]; F10: [[2, 44, 6, 53]]; F11: [[2, 45, 7, 55]]; F14: [[4, 28, 9, 43]]; F15: [[2, 28, 6, 40]]|
|Mobility|Guardian|Female|BLOCKED_ART|0|Down0 바퀴 아래 6px 분리 잔여선, Alpha117. F0: [[67, 125, 73, 126]]|
|Mobility|Guardian|Male|BLOCKED_ART|6-11|Left6/7 좌측 잘린 장비 파츠, Right8-11 바퀴 아래 갈색/검은 분리선. F6: [[2, 64, 6, 82]]; F7: [[2, 69, 4, 83]]; F8: [[59, 123, 71, 126]]; F9: [[60, 123, 74, 126]]; F10: [[62, 123, 73, 126]]; F11: [[60, 123, 72, 126]]|
|Mobility|Healer|Female|READY|-|머리/지팡이/휠체어 복원. 잘림/가시분리파츠 없음. Frame9-11 저Alpha18-49 머리 가장자리 성분은 부드러운 윤곽으로 기록. F9: [[60, 2, 67, 3]]; F10: [[62, 3, 69, 5]]; F11: [[62, 2, 65, 3]]|
|Mobility|Healer|Male|BLOCKED_ART|4,15|Left4 우측3px 지팡이 잔여파츠 Alpha191. Up15 좌측16px 금색 장비 파츠 Alpha250. F4: [[125, 32, 126, 35]]; F6: [[2, 71, 3, 73]]; F15: [[5, 56, 8, 64]]|
|Mobility|Mage|Female|BLOCKED_ART|0,8-10|Down0 및 Right8-10 바퀴 아래 12/25/19/11px 갈색/검은 분리선 Alpha225-255. F0: [[63, 124, 74, 126]]; F8: [[55, 121, 69, 124]]; F9: [[53, 122, 66, 124]]; F10: [[57, 122, 68, 123]]; F11: [[60, 123, 62, 124]]|
|Mobility|Mage|Male|BLOCKED_ART|5-7|Left5-7 좌측 잘린 망토/장비 파츠 Alpha243-253. Frame6 추가 잔여선 Alpha112. F5: [[2, 78, 4, 82], [43, 5, 44, 6]]; F6: [[2, 77, 5, 85], [2, 71, 3, 74]]; F7: [[2, 77, 4, 82]]; F10: [[62, 124, 63, 125]]|
|Mobility|Sharpshooter|Female|BLOCKED_ART|0|Down0 바퀴 아래 7px 검은/갈색 분리선 Alpha195. F0: [[68, 125, 75, 126]]; F3: [[71, 125, 72, 126]]|
|Mobility|Sharpshooter|Male|BLOCKED_ART|0-3|Down0-3 바퀴/발 아래5/8/5/5px 분리 조각 Alpha198-240. F0: [[56, 123, 60, 125], [65, 124, 66, 125]]; F1: [[56, 124, 61, 126], [65, 125, 67, 126]]; F2: [[61, 124, 64, 126]]; F3: [[54, 124, 57, 126]]|

좌표는 셀좌상단 bbox[x0,y0,x1,y1). Alpha>10/8방향 연결성분과 시각검수로 판정. 모든160Frame 머리/캐릭터/휠체어/바퀴/무기/장비/방향/Scale 검사. Empty0/CharacterSwap0/방향오류0/심각Scale문제0. 잘린 머리/바퀴 개선, 셀내 잔여파츠9종은 BLOCKED 유지. 바퀴 중심의 문양/푸른 발광은 같은 차체/바퀴의 효과로 구분하며 디자인 교체로 세지 않음. 정상 낮은Alpha 윤곽과 가시적 분리 조각을 구분. Runtime검증완료.

백그라운드 격리 PlayUnfocused 88PASS/0FAIL. Mobility10 자동Mapping/JobPreview 통과. 대표Male Fighter는 BLOCKED 상태의 기본성별fallback, Female Healer는 복원Frame0 Preview/World/Continue 및Frame4 BattleLeftIdle 직접표시를 확인했다. 실제BattleScene 전달/Save→BootstrapContinue/구버전ID 재계산/PathJob 없는 옛Save fallback 유지. BLOCKED9종은 원본을 등록했으나 실행에서는 기존fallback 정책을 유지한다. 기존 Runtime 로그의 Hearing 라벨은 예전 문구이며 실제 필터/대표Path는 path.mobility이다.

컴파일Error0, 최종ConsoleError0/Warning0. 컴파일시 기존CS0618 2건 및MCP재연결경고1건 관찰/기록, 수정범위 밖이라 변경하지 않음. 포커스전환없음, cleanBootstrap/EditMode/Save/Settings/Play설정 복원. 실제걷기/물리입력 미검증.

보존검사 PASS: ZIP/원본10PNG bytes 동일, 모든meta/160SpriteID/Import 동일, 다른40PNG/Entry/QA 및기존320Clip 동일. READY HealerFemale 기존Entry에만8Clip 추가. Assets/Settings/UserData 변경은10PNG+Catalog+Inventory+검증helper13파일. 게임기능C# 변경없음, QA helper의 대상Path만 변경. 기존사용자 변경411Git항목 보존. [세부QA](Mobility_RestoredFinal10_QA.json)·[Runtime결과](Mobility_RestoredFinal10_Runtime_Results.txt). GitHubPush없음.
