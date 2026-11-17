# Issue #85 기존 수정 진단·외부 백업 재개 — 2026-11-17

**OFFSITE_BACKUP_BLOCKED_NO_DESTINATION / STATIC_CHECK_PASS / TARGETED_UNITY_QA_NOT_RUN / RELEASE_HOLD / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**

## 근거와 외부 백업

[최신 지시](https://github.com/HappyPaul812/Project-Limitless/issues/85#issuecomment-6931010623), [11/16 EOD](EOD_20261116_SafeIntegration_Recovery.md), CODEX_LATEST를 확인했습니다. 최신 지시 C는 A/B 안전 조건을 모두 만족할 때만 기존 격리 QA 1회를 허용합니다.

현재 연결 장치는 내부 NVMe·SATA 2개, C/D/E/F는 Fixed입니다. 연결된 외장/네트워크 드라이브와 승인된 비공개 외부 목적지는 확인되지 않았습니다. 기존 OneDrive 경로의 존재는 프로젝트 민감자료 업로드 승인이나 동기화·암호화·복원 보증이 아니므로 사용하지 않았습니다. 새 계정/암호 생성·임의 업로드·백업 삭제·외부 복사0. **PC 밖 암호화 백업과 실제 복원 시험은 BLOCKED/NOT_RUN**입니다.

어제 AllLocalRefs.bundle과 8342 보호파일 RestorePlan을 재사용했습니다. 최종 SHA 재검사 수치와 F: 실측은 private `Temp/Issue85Resume20261117/Final.json`에 기록합니다. F: 내 사본을 오프사이트 백업 성공으로 표시하지 않습니다. Downloads1479와 76개 오래된 worktree 전체 미커밋 보존 범위는 여전히 NOT_VERIFIED입니다. 민감 파일의 내용/개인 경로 목록은 커밋·GitHub 댓글에 넣지 않습니다.

## 현재 Git과 원본 작업 트리

원본 main/HEAD `52f84e643d5d860665eadfe1614a30c745209f00` 유지. Packages와 meta 수정0, 미커밋 변경의 작성자·실행 경위는 Git만으로 확정할 수 없습니다.

| 항목 | 커밋 | 실제 작업 트리 |
|---|---|---|
| Animation |15.1.0|16.0.1|
| PSD Importer |14.0.3|15.0.1|
| 2D Tooling |3.0.1|4.0.1|
| 대상 투사체6meta |같은 GUID1개|고유 GUID6개|

manifest SHA256 `5503b0fe8e1f9e58b2153e397ee73d2541bc698e1311a94b351c77f023f636fc`, lock SHA256 `4bece7348b1d945916d1c940e2fbce5f24bc87f577fc00c611b41aede2e67de8`. **전체 차이는 세 패키지만이 아닙니다.** manifest 변경/추가 13개, lock 변경/추가 27개이며 Aseprite/SpriteShape/Tilemap Extras/AI Inference/tetgen 등 기존 차이도 존재합니다. 삼중 패키지 수정으로만 간주해 커밋하지 않았습니다. 상세 diff는 private diagnostic.json, 원본 `git diff`에 보존합니다.

GoldenArrow 기존 GUID 유지, Healer5 현재 값은 과거 계획의 제안값과 다릅니다. 현재 값은 실제 Assets meta 전체 조사에서 충돌0이며 제안값으로 다시 치환하지 않습니다.

| meta | 현재 GUID |
|---|---|
| golden_arrow_00.png.meta | ea7264dba5af46cda070ad3e3aa3eac3 |
| healer_magical_projectile_00.png.meta | ce8920d06c62aaa419f1180d535d493d |
| healer_magical_projectile_01.png.meta | 6a8353c59be3232499a57e261c3f5749 |
| healer_magical_projectile_02.png.meta | cd4a6917c5b6b644faeb282bd41ec2b8 |
| healer_magical_projectile_03.png.meta | 0a9dd0f05e4d9a24492b71659c4df5a9 |
| healer_magical_projectile_04.png.meta | 0d542898cc24c8848b6d91b06b1c4de0 |

원본 디스크 GoldenArrow PNG8 / Healer PNG5 및 대응 meta 확인. 계획의 보호 PNG6 SHA와 일치 여부는 최종 receipt에 기록합니다. BattleSceneController.cs:2885의 공용 로더는 `Resources.LoadAll<Sprite>(resourcesPath).OrderBy(sprite => sprite.name).ToArray()`이며 사용 경로는 BattleProjectiles/GoldenArrow·HealerMagicalProjectile입니다. 정적 파일 개수는 실제 Unity Sprite 로드 성공을 뜻하지 않습니다.

## USS/UXML와 검증 경계

현재 원본 Tooling4.0.1에서 과거 오류 대상8파일을 모두 열어 SHA를 읽었습니다. **기존 격리 Tooling3.0.1 캐시는 열거에는 나타나지만 일반 경로로8파일 읽기가 모두 실패합니다.** 263~274자 경로에서 Win32 확장 경로로 읽은 결과는 private tooling-path-check.json에 기록했습니다. 현재 Tooling4 대상은218~229자로 읽기 성공입니다. 긴 경로의 영향 여부와 패키지 버전 효과를 분리해야 하며 Unity 재시험 없이 해소를 확정하지 않습니다. 캐시 수정·삭제·재생성0.

ProjectVersion은6000.6.5f1입니다. 현재 Win32 프로세스 조회에서 Unity.exe는 없었고 CLI도 연결 Editor를 찾지 못했습니다(샌드박스 단독 결과로 종료를 단정하지 않고 별도 읽기 전용 조회). CLI1.0.0 업데이트 확인은 CDN DNS ENOTFOUND로 실패, 업데이트/패키지 설치0. 연결된 Unity MCP 도구도 이번 세션에 노출되지 않았습니다. 현재 live Console0/compile idle은 확인하지 못했으며 어제 상태를 오늘 상태로 재사용하지 않습니다. 참고 skill reference는 unity-cli/references/integration-advanced.md입니다.

기존 PSD QA Library는 존재하고 그대로 보존했습니다. 외부 백업 안전조건 A가 충족되지 않아 기존 격리 환경에 현재 Packages/meta를 복사하거나 Unity를 실행하지 않았습니다. 대형 프로젝트 신규 복제·전체 QA 반복0.

| 검증 | 이번 결과 |
|---|---|
| 현재 Tooling 오류대상8파일 읽기 |정적 PASS|
| 대상 GUID6 고유/Assets 전체 중복 |정적 PASS/중복0|
| Unity6000.6.5f1 Import·컴파일 |NOT_RUN/HOLD|
| Tooling 예외 재현/해소 |NOT_RUN/HOLD|
| GoldenArrow8·Healer5 실제 Resources 로드 |NOT_RUN/HOLD|
| 새 Unity Console 오류/런타임 MissingSprite |NOT_VERIFIED|

## 보호·다음 단계

원본 Unity/Save/UserData/Discord/승인 아트/기존 dirty·미추적은 수정·삭제하지 않았습니다. 원본 Editor/Scene/Play/선택/OS 포커스 조작0. reset/clean/stash/force/원격 Push/제품 커밋0. 기존 소형 문서 worktree만 재사용해 `codex/issue85-existing-fix-audit-20261117`에 보고서3개만 로컬 커밋합니다. 원본 main 이동0. root CURRENT_STATUS/CODEX_LATEST는 같은 보고를 상단에 반영하되 원본 미커밋 변경으로 남깁니다.

다음은 승인된 PC 밖 비공개 저장 목적지와 암호화 복구 수단 확보입니다. 그 후 기존 QA 재사용 안전성/실제 전체 패키지 차이를 확인하고 승인 범위의 표적 Import·Sprite 로드1회를 결정합니다. 이미 고유한 GUID를 다시 고치지 않습니다. 검증 전 제품 커밋/본편 적용/main Push는 HOLD입니다. 최종 실제 commit SHA는 Issue85 댓글과 private Final.json에 기록합니다.


## 최종 실측·보호 확인

- 기존 백업 **8342/8342 SHA 일치**, AllLocalRefs.bundle SHA256 `4b107893697de5499dc61d2c70f27ae0e8c0460e33e7a93595633f3ca1dcc615`·1478789320bytes·`git bundle verify` exit0. PNG6은 과거 보호 SHA와 모두 일치. 외부 복원 시험은 미실행입니다.
- 원본 기존 비보고 git status는 시작값과 byte-exact 동일. 이번 보호목록 재읽기 이후 8342파일 SHA 변경0(보고문서는 재읽기 전 갱신). 어제 스냅샷과는 EOD허용 문서5개 외 EditorSettings/ProjectSettings/HierarchyWindow/Layout4개 차이가 관찰됐습니다. 변경 경위는 미확정이며 덮어쓰거나 복원하지 않았습니다. 이번 검사는 Unity를 실행하지 않았습니다.
- 기존 Tooling3 대상8파일은 일반경로 실패/Win32 확장경로 읽기 **8/8 성공**. 파일 누락보다는 긴 경로 접근 문제를 지지하나 Unity에서 재검증하지 않았습니다.
- F: 실측 시작 **321534255104bytes**, 현재 **321531150336bytes**. 다른 프로세스도 사용할 수 있는 시점별 실측입니다.
- 마지막 관련 문서 commit `e0ac9a1f16aeb9f44c32b2f6ed4aab48089ed702`; 이 최종 수치 기록 후의 최종 commit은 private Final.json/Issue85 댓글을 기준으로 합니다. 원본main52f84e64 유지, LOCAL_COMMITTED / REMOTE_NOT_PUSHED.
