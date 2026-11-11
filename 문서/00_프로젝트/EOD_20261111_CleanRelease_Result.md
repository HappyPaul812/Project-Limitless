# 2026-11-11 Clean Release 후보 — 이력 분리 PASS / Unity 기술 HOLD

**CLEAN_RELEASE_CANDIDATE_LOCAL_COMMITTED / OBJECT_SCOPE_PASS / UNITY_COMPILE_FAIL / RELEASE_READINESS_HOLD / ORIGINAL_LOCAL_MAIN_PRESERVED / REMOTE_NOT_PUSHED**

## 실제 기준과 이력

- 원본 local main은 `d360813667d2ee47515067b8dd07c1a599efec15`로 시작·종료 동일합니다. ref 이동·원본 파일 적용·원본 직접 commit·이력 삭제/재작성은 없습니다.
- 최신 fetch 및 서버 main은 `bfd281545632877325f5d21e04cefb924042c465`입니다.
- 후보 `codex/eod-clean-main-candidate-20261111`는 이 원격 SHA에서 새로 시작했습니다. 제품 스냅샷 커밋은 `26dfd0b1dda60bb73315a9af59830d67fc470d2c`이며 부모는 정확히 위 원격 main 하나입니다. 이 보고/인계를 추가한 최종 후보 SHA는 브랜치 HEAD 및 Issue #85 완료 댓글에 기록합니다. 원격에는 아직 없습니다.
- 기존 제품 이력 67개와 EOD 문서 커밋 1개, 합계 68개의 local-only commit을 merge/cherry-pick하지 않았습니다. 기존 local main 및 과거 커밋은 그대로 보존합니다.
- 최신 기획 [Clean Release 지시](https://github.com/HappyPaul812/Project-Limitless/issues/85#issuecomment-6793878896), `origin/docs/planning` 최신 Clean Release 절만 선별했습니다. 다른 docs 브랜치 전체 merge는 없습니다.

## 명시적 허용목록과 동등성

[파일별 허용목록·원본 blob/SHA256](EOD_20261111_CleanRelease_Allowlist.md).

- 승인 최종 제품 **235파일**을 local main의 Git blob에서 직접 가져왔으며 전부 SHA256 동일합니다. 작업 폴더의 dirty 파일을 입력으로 사용하지 않았습니다.
- 승인된 PilotBee 제품 제외 **21경로 삭제**를 재현했습니다. 원본 ThirdParty 폴더를 새로 수정한 것이 아니라 승인된 최종 local main 상태를 새 후보에만 재현한 것입니다. 외부 원본 및 로컬 보관본은 새 배포 이력에 복사하지 않았습니다. 기존 서버 과거 이력의 객체를 지웠다는 의미는 아닙니다.
- 공식 보고/결정/인계 **41 Markdown**을 선별했습니다. 그중 PLANNING_LATEST는 최신 Clean Release 지시를 선별 추가했고 CODEX_LATEST/CURRENT_STATUS는 후보 결과를 추가했습니다. 이 3개는 의도적인 문서 갱신 예외입니다. 나머지 선별 문서는 원본 blob 그대로입니다. 여기에 본 보고/허용목록 2개를 추가합니다.
- 전체 Unity tree도 현재 local main과 비교했습니다. 생성된 `Main24F2ContentBuilder`, `Main24F2TraversalQA`, `Main24FirstPlayableQA` 및 각각의 meta **6파일만 명시 제외**, 나머지 승인 제품 누락·의도치 않은 차이 0입니다. 이름이 Candidate/Pilot인 기존 승인 런타임 코드는 이름만으로 삭제하지 않았습니다.
- F2 Scene이 참조하는 기존 placeholder PNG/meta는 승인 B352의 기존 환경 보존 계약에 필요한 기반입니다. 임시로 만든 새 아트가 아니며 Scene 참조를 훼손하지 않도록 보존했습니다. B352는 기존 Renderer4개만 실행 중 숨기는 계약 그대로입니다.
- NPC4·Ch3BG2·F2B352·한기 코어/정식 Owl·독침벌/거미/맹독뱀·물약·Main11 음성 및 기존 환경/펫/오디오를 유지했습니다. 이미 서버에 있는 제품도 보존했으며 재제작/과거 대규모 QA 반복은 없습니다.
- 미승인 동상/동결괴사 실전 처리·정화Ⅱ/어묵탕/화로·경제·Discord·C SnowEdge·OGA20·VFX 배치 실험을 추가하지 않았습니다. 승인 냉기 원본6종 보존과 미승인 기능 활성화는 별개입니다.

## Git 객체·외부 자산 검사

첫 스냅샷 신규 객체 **322(커밋1/tree60/blob261)**, blob 23,350,559bytes를 전체 조사했습니다. 보고 commit 추가 후의 전체 신규 객체도 동일 검사기로 다시 확인합니다. 검사기/원시 로그/개인 보호 목록은 Temp에만 두고 커밋하지 않습니다.

- origin/main 조상 관계 정상, 기존 local-only commit 교집합 0, 허용목록 밖 diff 0, 허용 파일 누락 0.
- 선별 제외 파일 blob과 신규 도달 가능 blob의 교집합 0. 동일한 허용 meta로 인한 예외도 0. 신규 커밋의 부모/tree/blob 모두 확인하므로 HEAD에서만 파일을 숨기는 방식이 아닙니다.
- 알려진 GitHub/OpenAI/Google/AWS 키·private-key header 패턴 검출 0, 50MiB 이상 blob 0, 새 LFS pointer 0. 이는 모든 형태의 비밀/개인정보나 법적 권리를 보증하는 선언은 아닙니다.
- 새로 포함하는 아트는 승인된 기존 원본의 바이트만 사용합니다. 미채택 외부 PilotBee 아트/ZIP/라이선스 미결 자료를 새 이력에 넣지 않았습니다. 기존 제3자 자산·라이선스는 유지했고 새로운 라이선스 취득/권리 보증은 하지 않았습니다.
- GitHub read-only 확인: public repo, push/admin 권한 true, main protected=false, required contexts 없음, rulesets=[]입니다. 실제 Push나 CI 실행은 없으며 향후 정책/remote 재확인이 필요합니다.
- 기존 승인 Unity meta와 역사 문서의 `git diff --check` 공백 경고는 그대로 기록했습니다. 제품 SHA를 맞추기 위해 공백 정리도 하지 않았습니다. 새 보고 문서의 공백 검사는 별도로 수행합니다.

## Unity6000.6.5f1 최소 Import·컴파일 — FAIL / 후속 범위 HOLD

과거 81/99/68/46 검사는 재실행하지 않았습니다. 새로 만든 정확한 후보 프로젝트만 백그라운드 `unity run`으로 Import/컴파일했습니다. 원본 Editor의 Play/Scene/선택/포커스 조작은 없습니다.

1. 첫 sandbox 실행은 라이선싱 시스템 정보 접근 거부/연결 실패로 컴파일에 도달하지 못했습니다. 이번 batch PID/부모/경로만 확인 후 해당 격리 프로세스를 종료했고, 권한을 보완한 같은 후보 batch를 1회 실행했습니다. 원본 Editor/공유 LicensingClient를 종료하지 않았습니다.
2. 권한 보완 실행은 라이선스 및 패키지 해석 후 **컴파일 오류로 exit1**입니다. 커밋된 `com.unity.2d.animation` **15.1.0** Editor 코드의 `Button.SetChecked/IsChecked` **CS1061 6개 위치**입니다. 위치는 ButtonStripField(129), Toolbar(40/45/55), VisibilityTool(157/158). 프로젝트 게임 코드의 PASS 또는 전체 Import 완료로 보고하지 않습니다.
3. Packages/manifest와 lock은 원격/현재 커밋 상태를 그대로 사용했습니다. 원본의 미커밋 Packages 설정을 자동 채택하지 않았습니다. 자동 해석된 lock 변경1개는 private 검증 자료로 보존한 뒤 **후보 작업 폴더만 승인된 커밋 blob으로 되돌렸고**, 배포 commit에 포함하지 않았습니다. package-cache 직접 패치나 미승인 버전 변경은 없습니다.
4. 기존 원격 main부터 있던 GUID 중복: GoldenArrow00와 HealerMagicalProjectile00~04의 meta **6개가 같은 GUID**를 사용합니다. 신규 승인 meta가 만든 중복은 0이지만 기존 중복이 해소된 것은 아닙니다. 원본에는 관련 meta 미커밋 변경이 있으므로 자동 채택하지 않았습니다. 배포용 GUID 정리도 별도 검토/승인 대상입니다.
5. 정적 F2 Scene 참조 검사에서 처음 Assets에 없다고 나온 `a97c105638bdf8b4a8650670310a4cd3`는 실제 해석된 URP package의 `Runtime/Materials/Sprite-Lit-Default.mat.meta`에서 확인했습니다. Missing asset로 확정하지 않습니다. 새 meta/대상 파일 쌍은 전부 있습니다.

따라서 **Git 이력 분리·파일 허용 범위 검증은 PASS지만 Unity 배포 준비는 HOLD**입니다. 후속 최소 후보 범위는 `Packages/manifest.json`·`Packages/packages-lock.json`의 6000.6.5f1 호환 패키지 검토와 기존 투사체 meta의 GUID 충돌 처리입니다. 사용자 미커밋 내용을 그대로 가져오는 승인으로 해석하지 않으며 이번에는 수정하지 않습니다. 오류 해결 전 RELEASE_READY/UNITY_PASS를 선언하지 않습니다.

## 원본·사용자 보호 및 다음 승인 경계

원본 보호 대상8322파일 SHA 모두 동일, 원본 status의 tracked302/untracked550 경로/상태 동일, Save/UserData/UserSettings와 사용자 작업 보존을 재확인했습니다. 기존 EOD의 실제 백업을 보존하며 새 비공개 보호 snapshot으로 재대조했습니다. 원본 main은 d3608136 그대로, 원격 main도 bfd28154 그대로입니다. 원본 Editor는 기존 Bootstrap/PlayOFF/선택null/비포커스/compileidle로 유지했고 이미 존재하던 external_changes_dirty=true를 임의 Refresh하지 않았습니다.

- **이력 구조만으로는 origin/main → 후보의 정상 FF Push가 가능합니다.** 그러나 이번 Push 금지와 Unity 기술 HOLD 때문에 실제 전송하지 않았고 즉시 Push 가능/승인 완료로 보고하지 않습니다. 향후 호환성/GUID 처리 승인·검증과 remote/권한/정책 재확인 및 Push 별도 지시가 필요합니다.
- 기존 local main은 후보의 조상이 아니므로 **local main → 후보 FF 이동은 불가능**합니다. 기존 이력/dirty/backup 보존과 별도 사용자 승인 아래 ref 정렬 방법 및 compare-and-swap을 결정해야 합니다. 이번 ref 이동/이력 재작성은 0입니다.
- Owl 정식 전투의 대표 실플레이는 `USER_REALPLAY_PENDING`입니다. 서릿길마을 북문→F2 Owl(5.7,-2) E/F/클릭→예고 확인→유효타격/생존시 한기2회 차단→복귀. 동상/동결괴사/정화Ⅱ 등은 여전히 이번 범위 밖입니다.

후보 검증 결과를 Issue85와 후보 CODEX_LATEST/CURRENT_STATUS에 기록하고 종료합니다. 원본 CODEX_LATEST는 사용자 원본 불변 조건에 따라 이번에 갱신하지 않습니다. 추가 게임 작업 자동 시작 없음.
