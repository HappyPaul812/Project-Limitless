# 2026-11-16 복구 범위와 비Git 누락 목록

공개 GitHub에는 개인경로/Save/인증값/Discord 설정/전체 private manifest를 올리지 않습니다. 전체 복구 목록은 원본 PC의 `Temp/EOD20261116/` private JSON에만 있습니다.

| 자료 | 보존/목록 | PC 밖 복구 여부 |
|---|---|---|
| 승인 제품 Git 체크포인트 | backup/2026-11-16-safe-checkpoint,9d84b81934a7cff7dcf4bf5083de3f1f45ea7201 | 원격 SHA 재조회·빈 bare Git 원격 fetch·NPC9파일/README SHA 검증 완료. 전체 실제 실행은 Issue85 HOLD |
| 전체 로컬 Git refs/미Push 이력 | refs-before.json, branches-unpushed-private.json.210개 local branch 중171개에 당시 어떤 origin ref에도 없는commit 존재. 전체 로컬 bundle은 별도 로컬 보존 | 일부 공개불가 이력 포함. 체크포인트 밖171개 전체가 오프사이트로 보존된 것은 아님 |
| 원본 미커밋/미추적 | tracked302/untracked550. protected8342파일/1221219326bytes, RestorePlan-private.json | 기존F:백업8315 재사용+새/변경27파일2260357bytes SHA 대조. F: 내부이므로 오프사이트 아님 |
| 다른 worktree | 총160개 HEAD/branch목록.81개status확인,76개6초timeout,3개기존경로사용불가 | 최근3작업worktree는확인. 오래된76개untracked전수확인완료 주장은안함. 현재ref/작업파일수정0 |
| 아트ZIP/PNG·영상·다운로드/TTS | Downloads관련1479파일/1256003088bytes 경로·크기·SHA목록 | 비공개외부저장소 확인못함. 체크포인트에F: ZIP/MP4 추가0. 일부승인제품PNG는Git에포함되나원본ZIP/미적용Stoat는별도자료 |
| Stoat KeyPose/Idle4/Attack4v2/Hit4 | 모두미술승인/Unity미적용. Attack메타수정ZIP/HitZIP 실제SHA·CRC·Manifest9/10 확인 | Git에아트시트추가0. Library담당납품이있지만이번PC의전체외부백업/복원확인을대체하지않음 |
| TTS·Save/UserData/UserSettings | 원본보호목록에오디오/TTS1500파일253662349bytes, Save설정관련39파일944139bytes(분류중복가능) | Git승인TTS와비Git소스/설정구분. 사용자Save/Settings 공개업로드0. 외부개인자료전수복구확인미실행 |
| Discord봇/설정/자동화/인증 | 현재프로젝트관련목록/보호와Git외부경로탐색범위기록. 실제봇폴더/계정/키전체위치는확정못함 | 인증값공개/새암호생성/임의클라우드계정생성0. 외부별도Discord자료전체복구미확인 |

**OFFSITE_ASSET_BACKUP_INCOMPLETE**. 확인된사용가능 비공개외부저장소/암호키가없어 민감자료외부암호화전송/다른물리PC복원검증은수행하지않았습니다. 다른디스크문자나OneDrive폴더만으로안전한오프사이트라고추정하지않습니다. 동일F:의기존백업과로컬bundle은PC전체고장을보호하지못합니다.

전체로컬bundle은공개전송금지: 별도공개감사에서제외한로컬이력도포함될수있으므로검증된private/암호화외부저장소를확보한뒤후속보관해야합니다. 원본/기존백업을삭제하거나원격main에강제정렬하지마세요.
