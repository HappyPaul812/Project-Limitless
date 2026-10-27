# Issue66 Worker11 관리자 결과 — 역할·독점 실행 HOLD

**11/11 동일성·실제 소유 SID 확인, Group SID 조회 성공. 실제 AI 작업 및 독점 실행은 UNKNOWN/HOLD.** 이번은 저장된 결과의 판독·문서화만 수행했습니다. 코드/패키지/운영 변경·새 진단 재실행0.

원본 `F:\study\codex\Project-Limitless\Temp\WorkerProcessGroupInvestigation20261027\f1f6f3991b5e4a78bffef16aac347c98.json`, SHA256 `fdcf6351873fe5eeb999381f0b5c9ccab96c5abd84fcbe8ec94ededeb74c6200`. 2026-10-27 **13:34:02 KST**, admin=true. Evidence/WorkerInvestigation20261027/에 원본 bytes, AdminProcessMatrix_f1f6f399, AdminInterpretation_f1f6f399를 보존했습니다. 기준 코드07ef386bd150af8b2f81ae77edd35722e3870dc1 / 인계5d45297ecf649a8dfc1420cb2c5c240ebe694d44, branch codex/discord-usage-diagnostics-20261026.

## 1. 프로세스11개 판독

모두 원본 WMI 마이크로초 정밀도에서 생성시각 MATCH, 실제 owner `PAUL-PC\LIMITLESSRelaySvc`/SID1007, Session0, Windows Security LogonType4 **BATCH**, 기본토큰 조회 성공입니다. 실제 생성시각100ns의 마지막 자릿수 차이를 임의 초 단위 오차로 허용한 것이 아니라 원본 정밀도에 맞춰 비교했습니다. 이번11개에 대해서는 실제 Worker 소유가 확인됐습니다.

| PID | 이미지·역할 근거 | 서명 | Job 포함 | 부모 PID |
|---|---|---|---|---|
| 6356 | python.exe / 일반 런타임, 실제 작업내용 UNKNOWN | cache 정책 유효 | false | 41880 |
| 7432 | 이름 정제된 동일 파일 A / 역할 UNKNOWN | cache 정책 유효 | false | 18828 |
| 7936 | 이미지 조사 미완료 / UNKNOWN | UNKNOWN | false | 7432 |
| 18796 | 이미지 조사 미완료 / UNKNOWN | UNKNOWN | true | 6356 |
| 23564 | 이미지 조사 미완료 / UNKNOWN | UNKNOWN | false | 15004 |
| 28788 | python.exe / 일반 런타임, 실제 작업내용 UNKNOWN | cache 정책 유효 | false | 15004 |
| 31460 | 이미지 조사 미완료 / UNKNOWN | UNKNOWN | true | 34416 |
| 34416 | python.exe / 일반 런타임, 실제 작업내용 UNKNOWN | cache 정책 유효 | false | 7432 |
| 36008 | 이미지 조사 미완료 / UNKNOWN | UNKNOWN | true | 28788 |
| 41880 | 이름 정제된 동일 파일 A / 역할 UNKNOWN | cache 정책 유효 | false | 36008 |
| 47064 | 이미지 조사 미완료 / UNKNOWN | UNKNOWN | false | 41880 |

Python3개는 SHA `2f2adeda344aa5bf4189778a4e0216a95fe3ba3050b96e7014c52d079dd562d6` 및 file ID가 같고, 파일 A 두 개는 SHA `bfb46af89433268872ddb43d1ca7a3f433452ee91ed356a9786940f90118e285` 및 file ID가 같습니다. A의 실제 이름은 도구가 정제했으므로 해시로 이름을 추측하지 않습니다. 유효 서명5개는 cached file-handle WinVerifyTrust code0이며 최신 온라인 폐기 검증/전체 catalog 검색/안전한 작업 내용의 증명이 아닙니다.

이미지 미완료6개는 7936/18796/23564/31460/36008/47064입니다. IMAGE_HANDLE_SHA_SIGNATURE 단계의 InvalidOperationException/HRESULT -2146233079, 바깥 MethodInvocationException -2146233087. 원본은 어느 검증 조건인지 기록하지 않았습니다. hardlink/reparse/크기/경로 중 특정 원인으로 채우거나 접근 거부·무서명·Windows 정상 파일로 단정하지 않습니다. 해당 가드를 완화하지 않았습니다.

로그온 LUID는 2개: c4a134d4의4개(7432/7936/31460/34416), c4a1e55b의7개. 두 로그온 영역이 각각 Bot/Art라고 지정할 근거는 없습니다. 10개 부모 연결은 생성 순서 호환일 뿐 런처 실행의 완전한 증명이 아니고, 7432→18828은 UNKNOWN입니다. 직접 자식 추가0도 해당 수집 범위의 결과이며 전체 Worker 계정의 모든 새 실행을 감시한 것이 아닙니다.

3개는 어떤 Job에 속하지만 특정 V2/기존 Job 이름·정책과의 연결은 미확인입니다. 나머지의 in_any_job=false도 AI가 실행되지 않는다는 뜻은 아닙니다. Codex 이름 지표 배열은 빈 상태이나 이미지6개 미완료와 이름 정제2개가 있어 CLI·AI의 부재 증명이 아닙니다. 민감한 명령줄·환경·실행 요청은 수집하지 않아 **실제 모델 호출 여부는 미확정**입니다.

## 2. GroupPrincipal 조회 결과

Task 관련96행은 **GROUP principal92행 + Worker user principal4행**입니다. 'Worker 작업96개 실행 중'을 뜻하지 않습니다. principal 조회 오류0, task detail 오류0으로 새 도구의 OS SID 조회는 성공했습니다.

| 실제 GroupId | OS SID·범주 | 관측된 Worker 기본토큰 소속 | 관련행 / Enabled / State4 |
|---|---|---|---|
| Everyone | S-1-1-0 / WellKnownGroup | 활성 그룹 있음 | 2 / 1 / 0 |
| Authenticated Users | S-1-5-11 / WellKnownGroup | 활성 그룹 있음 | 5 / 3 / 0 |
| Users | S-1-5-32-545 / BuiltinAlias | 활성 그룹 있음 | 21 / 19 / 2 |
| Administrators | S-1-5-32-544 / BuiltinAlias | 관측 토큰에 없음 | 5 / 3 / 0 |
| INTERACTIVE | S-1-5-4 / WellKnownGroup | 관측 토큰에 없음 | 59 / 55 / 1 |

이제 실제 INTERACTIVE/SID S-1-5-4를 확인했지만, 옛 GroupNotFound 원본에는 GroupId·Task ID가 없으므로 바로 그 과거 실패 객체였다고 연결하지 않습니다. 새 도구에서 local SAM에 한정하지 않은 조회가 성공했다는 것이 확인 범위입니다. r11 내부의 기존 resolver를 교체한 것은 아닙니다.

**Task Scheduler type4=GROUP**, **Windows Security type4=BATCH**. 이번 Worker11은 후자이고 GROUP Task92는 전자입니다. 두 문맥을 같은 의미로 취급하지 않습니다. Users 관련 State4 두 행/GetInstances2 및 INTERACTIVE State4 한 행/GetInstances0의 차이는 snapshot 자료 그대로 보존합니다. GROUP Task의 실행은 다른 사용자 세션일 수도 있으므로 그 실행을 Worker의 AI 작업으로 지정하지 않습니다. 관측 그룹 포함은 미래 세션/Task 실행 보장도 아니며, 관측 부재는 미래 멤버십 제외 증명도 아닙니다.

직접 Worker user principal4개는 전부 Enabled/Ready(State3)/조회된 instance0입니다. 이 중 LIMITLESS-Bot-OnBoot·LIMITLESS-Art-OnBoot는 Task type1=PASSWORD/동작 종류pwsh.exe, 나머지2개는 type3=INTERACTIVE_TOKEN/실행파일명 정제 상태입니다. Task가 Ready라는 사실로 기존에 분리 실행된 프로세스의 종료를 증명하지 않습니다. 등록 설정·트리거·실행·재시작 변경0. Task EnginePID와 action PID·Job 연결을 동일시하지 않았습니다.

## 3. HOLD와 다음 조건

현재 상태는 **HOLD_ROLE_AND_EXCLUSIVITY_UNKNOWN**. 기존 '접근 거부로11개 전부 미확인'은 이번 관리자 자료로 갱신됐으나, 이미지6개/파일A 역할2개/Python 작업내용3개 및 실제 실행 충돌은 여전히 미확정입니다. lease와 r9/r11 consume·r11 approval 메타데이터 ABSENT는 알려진 V2 경로의 관측일 뿐 전역 독점 실행 보장이 아닙니다.

r11의 외부 프로세스0 정책·공유 Worker·실제 설치 차단을 완화하지 않았습니다. 조회 성공을 설치 승인으로 바꾸지 않습니다. 새 진단 코드는 frozen r11에 포함하지 않았습니다. 정상 서명/이름만으로 프로세스를 제외하지 않습니다.

같은 전체 진단을 재실행할 필요는 없습니다. 추가 코드 개선을 한다면 **실패한6개의 이미지 가드 상수 tag와 정제된 파일 종류만 남기는 좁은 진단**, 파일 A의 허용된 제품/서명 식별 정보, 작업·승인·lease의 비밀 없는 실행 식별 계약을 별도 범위로 검토합니다. 원시 예외문/명령줄을 출력하거나 가드를 무조건 제거하지 않습니다. 실제독점 보장을 위해 기존 모든 launcher를 바꿔야 한다면 이번 금지 범위에 해당하므로, 이전 보고의 독립 Worker SID·빈 profile FAKE_ONLY/별도 VM 설계를 우선 검토할 수 있습니다. 이번에는 구현·설치하지 않았습니다.

원본·인계만 갱신, 새 테스트 재실행0(기존56PASS는 코드 불변의 과거 근거). r11 Installer/Manifest/ZIP·원본 코드는 변경0, 실제 staging·계정/ACL/Task·설치·IPC/FakeWorker/AI/usage/Discord·승인파일·Push/main병합·PC 재시작0. V1 REFERENCE_VERIFICATION_HOLD/MANUAL/A·Worker/인증/CODEX_HOME/OnBoot2/봇아트/운영SQLite·Unity·Save 보존.

LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 관리자 결과 판독 후 HOLD로 중지합니다.

마감: 관리자판독·증거 로컬커밋d2102bb9eca1d0e0826cbcb6b3ae4f8f35deb3e3. 실제 Issue66 보고 https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6433935615 . 실제11개동일성·owner/GroupSID조회성공, 역할·AI·독점HOLD. 코드/r11/운영불변, LOCAL_COMMITTED / REMOTE_NOT_PUSHED.
