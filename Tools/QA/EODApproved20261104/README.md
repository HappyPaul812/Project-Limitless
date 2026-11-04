# 2026-11-04 마감용 격리 QA 도구

현재 원본/통합제품 Assets에 자동 설치하지 않습니다. 실제 실행본은 Temp/EODApproved20261104/QA/Client입니다. 명시된 격리 경로와 Unity6000.6.5f1 guard가 있습니다. 원본 Editor foreground 조작 없이 Unity.com -batchmode -projectPath [격리경로] -executeMethod EodApprovedBatch.Run / RunVisual, 빌드는 EodApprovedBuild.Run을 사용했습니다. 비동기 PlayMode이므로 -quit를 붙이지 않습니다. 재실행은 자동 지시가 아닙니다. 이번 마감에서는 추가 QA를 시작하지 않습니다.

제품 Assets를 복사하고 QA에만 런타임 자동초기화 attribute 제거·Editor 자동화 제외·기존 최소QA Packages/Settings를 사용했습니다. UserData/Save는 복사하지 않고 합성 저장 경로를 지정했습니다. 새 정의 배열은 실제 Unity SerializedObject로 저장했습니다. 최종 통합 정의는 그 결과와 같은 GUID 참조입니다. 최종 소스에는 Run/RunVisual의 세션 플래그를 명시 분리했습니다.

초기 수동 YAML 배열0·환경 Search 예외·합성 Sprite 누락·대상 첫 아군 선택·QA static SessionState 초기화 실패는 실제제품 문제와 구분한 뒤 교정했습니다. 첫 모션 Trace는 태온을 표적으로 하는 실제 전투 실행이며 최종 player-ID 시각 캡처와 혼동하지 않습니다. 초기 공격/연결검사60PASS와 후속52PASS에는 중복검사가 있습니다. 출시 검증/전체 QA의 대체물이 아닙니다.
