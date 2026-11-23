# LIMITLESS / 2026-11-24 / Issue #85 기술 복구 실작업 명령

- 사용자 직접 요청: **“기존 Animation 15.1.0 컴파일 오류로 #85 HOLD는 미해결 건 해결 할 수 있니? 니가 판단해보고 할 수 있겠으면 코덱스 시켜.”**
- 총괄 기획 기술판정: **격리 복구 시도 GO / 원본 제품 자동 적용 NO**. 과거 2026-11-12에 Animation16.0.1 + PSDImporter15.0.1으로 C# CS1061 7건 0건 확인, 다만 Tooling3.0.1 USS/UXML 경로 예외·Projectile GUID 중복으로 #85 전체 HOLD. 11/17 사용자 원본 working tree에 16.0.1/15.0.1/4.0.1 및 고유 GUID6개가 **미커밋으로 이미 존재**, 이 자료를 삭제·중복 덮어쓰기하면 안 됨.
- 기준 2026-11-23 EOD 원격 복구 브랜치 `backup/2026-11-23-safe-checkpoint` **`65cceb2d6c3283d18abec6e27d8a6f1296411f17`**, 실제 로컬 `main`도 Codex #128에 동일 SHA로 보고됨. 원격 `main` `bfd281545632877325f5d21e04cefb924042c465` **구버전**. 실제 현장 `git rev-parse HEAD`/status/refs 확인을 우선하고 과거 SHA 강제 checkout/reset 금지.
- **Unity Editor 6000.6.5f1 (3ff58d469c8a) 고정.** 프로젝트 `Unity/Client/ProjectSettings/ProjectVersion.txt`은 현재 6000.6.5f1. 패키지 정본은 6000.6 공식 릴리스 `Animation 16.0.1 / PSDImporter 15.0.1 / Tooling 4.0.1`.
- Git 체크포인트의 `Unity/Client/Packages/manifest.json`은 실제 `15.1.0 / 14.0.3 / 3.0.1`, lock도 대응. [2026-11-12 2차](https://github.com/HappyPaul812/Project-Limitless/issues/85#issuecomment-6816270961) C# error 0, Tooling3 Import DirectoryNotFoundException 8파일/12회; [2026-11-17 기존 수정 진단](https://github.com/HappyPaul812/Project-Limitless/issues/85#issuecomment-6931267613)에서 Tooling3 경로263~274자 일반 읽기 실패·확장경로8/8 성공, 원본 Tooling4 경로218~229자 일반 읽기8/8 성공 확인. Tooling4 단독 전체 Import PASS라는 결론은 아직 **NOT_VERIFIED**.
- **기존 EOD #128 보존:** 본편의 빙각 수호체 PNG5/KO, Chapter3 BGM4 WAV8, 전리품6/100% 드롭, 서리들쥐 정의1/6 및 새로운 GUID/meta. 기존 QA 11 Play PASS·10 참조PASS, 원본 미커밋303경로/미추적550파일과 비Git TTS10 WAV·ZIP·개인 Save/UserData·Discord·API키 안전보호. 기존 `backup/2026-11-19-safe-checkpoint`/11-23와 remote main 불변.

## 실행 — 한 번의 표적 복구, 통과 전 제품 반영 금지

1. **현장 비교·분리**: 기존 프로젝트 `git status --porcelain=v2 --untracked-files=all`, `git worktree list`, 복구 ref·원본 로컬 main·원본의 현재 `manifest/lock/Healer5 meta` 실물을 **읽기 전용** 조회. 기존 미커밋 파일의 SHA/diff, 기존 고유 GUID가 어디서 왔는지 이력/크기 비교, UPM resolves, Tooling4 파일 존재를 추측 없이 확인. 11/17에 보고된 사용자 미커밋13/lock27 차이를 통째로 복사하거나, 제안된 이전 GUID5로 현재 고유 GUID5를 덮어쓰지 말 것.
2. **격리 작업**: 실제 최신 로컬 main 안전 SHA에서 고유 worktree `codex/issue85-unity60006-package-guid-repair-20261124` 생성. 기존 업무worktree 충돌 방지, 가능한 짧은 실제 파일경로 (예: **F: 드라이브 내 짧은 격리 경로**) 사용. 대규모 프로젝트 복제/원본 Unity Editor 전경/원본 Cache 수정 금지. 필요한 별도 격리 Library가 이미 있다면 재사용 가능성 확인, 없으면 최소 배치에서 생성하되 원본 사용자 프로젝트 Library 삭제 금지.
3. **범위가 명확한 패키지 복구**: 복구 브랜치에 **`Unity/Client/Packages/manifest.json`의 아래 세 개 직접 의존성만** Unity6000.6.5f1 호환으로 적용/해결한다:
   - `com.unity.2d.animation` `15.1.0` → `16.0.1`;
   - `com.unity.2d.psdimporter` `14.0.3` → `15.0.1`;
   - `com.unity.2d.tooling` `3.0.1` → `4.0.1`.
   반드시 실제 Unity Package Manager resolve로 `packages-lock.json` 생성·정합성 검증(수작업 Lock 임의 작성 금지). 앞서 확인한 **기준 lock 17항목 불일치**와 이번 실제 의존성의 증분을 명확히 분리. 기존 URP·MCP·InputSystem·기타 직접 의존성 버전을 임의로 올리지 말 것. 추가 package drift, registry 오류, vendor UPM 불일치가 필요하면 변경 목록을 보고하고 **수정 확장 HOLD**. PackageCache 내부 `Button.SetChecked/IsChecked`를 직접 코드패치하거나 설치경로/Editor 버전 변경 금지.
4. **GUID 충돌 복구**: Git 커밋된 `Assets/_Project/Resources/BattleProjectiles/` 하위 6개 PNG meta는 GoldenArrow/Healer5가 GUID `ea7264dba5af46cda070ad3e3aa3eac3` 하나를 공유. 기존 GoldenArrow 원GUID는 반드시 유지. Healer5 meta의 최상위 `guid:` 5줄만 각각 **고유하고 다른 Asset 전체와 중복없는 값**으로 격리 작업. **11/17 원본 미커밋 Healer5의 고유GUID 5개가 실제 존재·중복없음·안전함을 확인했다면 해당 값을 재사용**, 과거 계획 GUID 5개로 재치환하지 말 것. 어떠한 직접 GUID 참조가 발견되면 영향 그래프 수집 후 변경/해당대상 HOLD. PNG·import setting·Sprite ID/localFileID·PPU32·Pivot·Scene·Prefab·Scripts·Resources path·GoldenArrow meta 변경 금지. 새 GUID는 작업기록에 path별 명시.
5. **기술 검증을 구분**: Unity6000.6.5f1 격리 **클린 Git 체크아웃(원본 미커밋의 도움 없이)**으로 최소 Batch Import+**C# compiler errors0**+**Console Import/USS/UXML Tooling exception0**+Dependency lock 안정/재 Import drift0 확인. UPM 실제 resolved 16/15/4 기록. Win32 경로 길이와 Plugin/Coplan/MCP 관련 신규 문제를 별도 식별. 과거 원본 Tooling4에서 8/8 경로 읽을 수 있었다는 건 성공 가능성 근거이지 PASS 자동 증명 아님. 오류 발생 시 로그 원문/stack 보존 후 중단, 다른 패키지 무단 업그레이드 금지.
6. **실사용 투사체 QA**: Unity 실제 `Resources.LoadAll<Sprite>` GoldenArrow8 + Healer5 **개수/순서/null0/렌더/PPU/Pivot** 및 공격 시 예상 sprite 표시; GUID duplicate0, Import auto regenerate GUID0, 기존 에셋 참조/승인 PNG/WAV SHA unchanged. 표적 확인한 후 **최소 회귀** #126 KO 마지막 셀/단회 타격, BGM Intro→Loop, 기존 F2 Owl 및 전투/로드 주요 경로에만 점검. 과거11PASS/10PASS 및 81PASS 전수 QA 반복하지 않는다. Unity 회귀가 안전하지 않으면 범위·이유를 제한하여 `PARTIAL`로 보고.
7. **원본과 배포 게이트**: 정해진 파일(패키지 manifest/lock, Healer5 meta)만 제품 커밋으로 격리 브랜치에 남긴다. 보고 Markdown·`CODEX_LATEST.md`는 별도 커밋. `main`/원본 사용자 dirty 파일·Scene/Prefab·Save/UserData/Discord·키/음성/지시없는 아트 변경 금지. **격리 기술 PASS만으로 로컬 main 동기화/원격 복구/원격 main Push 하지 말 것**. 바울 대표의 본편 반영 승인과 파일충돌 여부 판단은 Codex 결과를 보고 기획이 후속 결정한다.
8. **완료 상태**: `PACKAGE_RESOLVE_PASS/FAIL`, `CLEAN_CSHARP_PASS/FAIL`, `TOOLING_IMPORT_PASS/FAIL`, `GUID_UNIQUE_PASS/FAIL`, `GOLDENARROW8_HEALER5_RUNTIME_PASS/PARTIAL/NOT_RUN`, `REGRESSION_PASS/PARTIAL`, `MAIN_UNCHANGED`, `PRODUCT_APPLY_HOLD`를 정확히 구분. 기존 #85 전체 HOLD가 풀리는 것은 ***모든 기술게이트가 PASS* 후 명시적 승인 시점**, 이전까지 닫지 않는다. 오류 재현과 PASS를 동시에 한 것처럼 보고 금지.
9. GitHub [Issue #85](https://github.com/HappyPaul812/Project-Limitless/issues/85) 결과 댓글 및 `문서/00_프로젝트/Issue85_Unity60006_PackageGuid_Repair_20261124.md`, `CODEX_LATEST.md`에 **작업브랜치/제품SHA/보고SHA/실제 파일별 diff/manifest+lock versions/원래 사용자 dirty 보호/QA 로그/남은 HOLD/대표 승인 필요 여부**를 기록. GitHub 원격 Push 금지, **실제 Codex 로컬 작업을 수행하지 않고 댓글로 성공 선언 금지**.

## 판단
- `ROOT_CAUSE_LIKELY_EDITOR_PACKAGE_VERSION_MISMATCH / ISOLATED_REPAIR_APPROVED / NOT_YET_VERIFIED`.
- GUI 사용/본편 `main`에 직접 수정/원본 Editor 강제종료/ProjectVersion 업·다운그레이드/전체 Unity Library 제거/기존 완료QA 반복 금지.
- 이 지시는 기획이 과거 작업기록과 공식 Unity 6000.6 패키지 안내를 확인해 **Codex 격리 실제 수정을 요청하는 것**이며, 기획 세션은 PC에서 고쳤다고 주장하지 않는다.
