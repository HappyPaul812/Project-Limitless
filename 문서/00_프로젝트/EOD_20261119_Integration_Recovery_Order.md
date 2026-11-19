**WORK_ORDER_ONLY / LOCAL_INTEGRATION_AND_RECOVERY_PUSH_PENDING**

[마감 작업 이슈](https://github.com/HappyPaul812/Project-Limitless/issues/121)

# 2026-11-18~19 마감 — 승인분 통합·로컬 동기화·PC 고장 대비 원격 복구

대표 요청: 이틀간 모든 작업 합치고 로컬 동기화 및 푸시. 신규 개발 금지. 기존 11/17 복구 ref backup/2026-11-17-safe-checkpoint (ba59838bf923832362f3b1fccf4897864cc678fa)를 보존한 채 최신 증분 복구 ref backup/2026-11-19-safe-checkpoint 생성 목표. 실제 PC의 git log/status/refs/작업 폴더를 우선하여 아래 알려진 기록은 누락 방지 최소 목록으로 사용한다.

## 통합할 승인 범위
- docs/planning 11/18~19 최신 정본: 북부 몬스터7/펫4/전리품6, 스킬25·상점·냉기 해제 아이템, Main22~29 및 서브6 승인, 세부 대사·보상 기준(메인1160EXP/480T, 서브690EXP/255T), Ch4 폴 고향/장비·제작 방향, 정령계 연출, 모든 승인/인계/밸런스 QA. docs/art-new 및 docs/art-renewal 최신 납품/승인/검증 문서 및 필요한 승인 PNG 등 실제 제품 경로. TTS/BGM/SFX/영상 브랜치의 실제 변경 유무 확인 후 선별, 기존 정상 자산·문서 유지. 원격 main의 오래된 버전/문서로 덮어쓰지 않음.
- 펫 분양·관리 독립 원화 및 전리품 적용 준비: 제품44d4ab5449131ca51876791f5ca3879aaaaf7a5d, 보고27c2c104cecfaea54ea913db878c3ddf32aedb09. 전리품은 정식 Item/Drop 연결0/6. 승인 PNG 6개 복구에 포함(제품 미적용과 별도 staging/승인 자산 구분).
- 아르벨 주민·포털 관리인: 제품31a72cf42a890a7c43e32005ba583172a5733256, 보고8698f594021ac7ed6849e47639bcc9a2abbf1a52. NPC 종료 실제 50PASS/0FAIL/오류0 보고d14f5907235b462df5bbe7c92c665c43ad8bff50. 공용8 유지, 포털 비활성, 기존 이름표 겹침은 기존 이슈.
- 불씨망령 승인 통합 시트: 제품37248c3b4345c7152db2ad2afad2aaf743294369/보고921a5e30a817a02fe065fba7be827a8eda84fa3a, Field07 실제30PASS. HP0 몬스터 대기 Sprite 복원 건너뜀 수정 제품4b423a70585cfed7b4fbf1697bf61da7476ccd4b/보고df77effd747a6a545ef48247807a071ad9b9a5da 실제3종75PASS/0FAIL/오류0.
- 눈보라 정령 원화/Idle8 v2/Attack4/Hit4/Defeat4 모두 미술 승인. Idle 제품32297d6ad37a1be7f31f94e98b57dcfb91a5886e 및 최종 제품9324b65849a25a4322052d217a7baa087a2b01f0/보고67c9c3c7d797c422c9af7802ec5e776920a301ca. 격리 임포트 및 수동 검토 Clip/Prefab/Animator PASS. 정식 MonsterDefinition/조우/F4·F5 미등록, 실제 전투/동상 미검증. 등록이나 게임 기획 변경 금지.
- 설원족제비/얼음갑충 전체 승인 아트 및 아직 미적용인 승인 원화·자료 등 복구 가능성 확인. 미등록·미적용 구분 유지. 승인 최종 원본이 Downloads/Temp/Library에만 있으면 허용된 비개인 승인 파일을 안전한 복구 경로에 별도 포함·해시 기록(불필요한 중복/대용량 중간 후보 제외). Library 저장만으로 전부 오프사이트 보장하지 않음. 영상/음성 원본·미커밋 작업 등 비Git 자료는 승인된 별도 비공개 외부 저장소가 실제 확인되면 암호화·검증, 없으면 INCOMPLETE로 보고. 개인 Save/인증/키/토큰/Discord 설정/Git 전체 bundle/미분류 dirty를 GitHub로 전송 금지.
- #118 PARTIAL: 로컬 C# 성장 모델 및 36조건 모형, 실제 보스 미검증. EXP +11000 제안 미승인, 승인 수치 유지. #85 HOLD 유지. 기존 Unity6000.6.5f1 유지.

## 안전 통합 절차
1. 원본 실제 path/HEAD/branch/Unity ProjectVersion/dirty/untracked/worktree/refs·가용 디스크 및 이전 안전 백업 상태 확인. 기존 파일 해시와 필요한 증분 백업/refs bundle(로컬만) 생성 및 검증. 이전 마감 문서 절차 재사용; reset --hard/clean/force push/무단 stash/삭제/대형 복제 금지.
2. 명시 승인 경로/커밋을 실제 포함 여부 대조하고 의존 관계 확인. 별도 codex/eod-safe-integration-20261119에서 선별 통합. 겹치는 Catalog/Animator/전투 코드 및 *.meta/GUID/BuildSettings/인계는 의미 결합; 오래된 본문으로 최신 승인 상태 되돌리지 않음. 동일 변경 중복 적용하지 말 것. 로컬 원본 작업 트리의 사용자 수정 및 기존 dirty/untracked 보호. 허용 경로만 안전하게 반영·FF-only 동기화, 실패/충돌 미해결 시 HOLD하고 원본 보호. Unity 버전 자동 변경/패키지 재설치 금지.
3. 복구 브랜치는 이전 감사된 clean 복구 계보 기준으로 최신 승인 제품·문서·허용된 승인 아트를 선별 수록. 모든 전송 예정 객체/경로/대용량/LFS 실제 파일/민감정보/인증 자료 검사 후 origin에 push. 새 ref는 기존 backup 브랜치 덮어쓰지 않는다. 원격 main은 기존 규칙 및 안전성(정상 ancestry, 승인된 변경만, 깨끗한 push 가능) 검증된 경우에만 정상 FF 반영 가능; 불가하면 기존 main 유지하고 검증된 복구 branch push를 우선, 사유 명시. 직접 main commit·force 금지. 대체 private 목적지 확인 없으면 비Git offsite 미완료.
4. 실제 GitHub ls-remote/ref SHA 및 로컬과 무관한 fresh bare fetch로 확인, 승인 제품/코드/PNG/meta/핵심 기획과 승인 미적용 아트의 SHA/blob 일치, 전송 LFS 사용 시 실제 객체 수신 확인. Unity 6000.6.5f1 및 컴파일/핵심 참조 최소 확인, 기존 격리 QA 재사용(전수 반복 X). #85 HOLD·Save·UserData·Discord 보호. 작업 끝나면 새 개발 자동 착수 금지.
5. 실제 결과를 CURRENT_STATUS/CODEX_LATEST/마감 보고와 이슈에 남긴 뒤 최종 마감 기록 자체도 복구 ref에 반영·재검증. 통합/로컬 HEAD/원격 backup ref+SHA/main 상태/누락·충돌·미적용·검증·비Git 외부백업 여부·복원 절차·다음 시작 위치 보고. 이슈에 실제 동기화 및 push 완료와 실패를 구분. 원격 수신 검증하기 전 성공 선언 금지.