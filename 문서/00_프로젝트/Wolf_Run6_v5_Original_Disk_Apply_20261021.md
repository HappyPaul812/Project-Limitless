# 승인 Wolf Run6 v5 원본 디스크 반영 완료

## 결과

**ORIGINAL_DISK_APPLIED / MINIMAL_IMPORT_DELTA_PASS / REMOTE_NOT_PUSHED / MAIN_NOT_MERGED**.

사용자 최신 지시에 따라 기존 승인 커밋 `ab1b47ff9d46f4ed5883b85b73e87ca2036c83e6`의 제품6파일만 `F:/study/codex/Project-Limitless/Unity/Client/` 원본 디스크에 반영했습니다. 새 애니메이션 제작·구현 재작성·106회귀 재실행은 하지 않았습니다. 제품은 기존 `eb0e932f0914727ce7205a8374e15122b2fcb0d7` 그대로입니다. 작업 브랜치는 `codex/wolf-run6-v5-apply-20261021`이며 신규 변경은 적용 보고·증거·현재 상태뿐입니다.

## 사전 검사·백업·읽기 검증

- 승인 ZIP SHA readback `16ba7923771c98f05b0f041b5c5a63857cdecfcdfd2e1f7768d9f24d1af0e351` 일치.
- 최신 GitHub main 및 원본 HEAD=`e4e9891f6f01ea95cada9e63b1f09946d94d6041`. 제품 변경의 기준 main과 동일하여 통합 대상6파일의 차이·사용자 수정 충돌0.
- 기존 C#4파일에 사용자 미커밋 내용 변경0, 신규 PNG/meta2경로는 원본에 없음을 확인했습니다. 각 파일의 실제 사전 SHA를 기록하고 기존4파일을 복사·해시 검증한 다음 반영 직전 다시 읽었습니다.
- 백업: `F:/study/codex/Project-Limitless/Temp/WolfOriginalApply20261021/Backup/` (원본 상대 경로 유지). [백업 manifest](Evidence/WolfOriginalApply20261021/BackupManifest.json)에 사전 존재 여부·SHA·승인 SHA를 보존했습니다. 복구 시 기존4파일은 이 백업을 사용하고 신규2경로는 적용 전에 없었다는 manifest를 기준으로 처리합니다. 복구 실행은 하지 않았습니다.
- 적용 전 보호6743파일 중 승인 C#4개만 변경, 신규 PNG/meta2개만 추가. 기존 사용자 dirty/untracked 내용·Save/UserData·Settings·기존 아트·GUID/meta·Animator/Controller는 범위 외 SHA 변경0. root HEAD/index 불변이며 원본 main 커밋0입니다.

## 실제 적용6파일·최종 SHA256

`Unity/Client/Assets/_Project/` 기준입니다. 모든 값은 복사 후 원본 디스크를 다시 읽은 결과이며 승인 브랜치와6/6 동일합니다.

| 파일 | SHA256 |
|---|---|
| `Resources/CompanionAssaultValidation/Wolf_Run6_v5.png` | `96f09da273c0e438b37ede80a6673c797b7623ed78b8e6e93de77eddb6aa649c` |
| `Resources/CompanionAssaultValidation/Wolf_Run6_v5.png.meta` | `159616b36caf4ca3d28104fc6a85c5b1165347515589194ad008f3e3ab569ec4` |
| `Scripts/Battle/BattleActionPresenter.cs` | `0f88b5fad89f6c10fb83451aa00b914dcefe759a1615a87be148aaee1c988358` |
| `Scripts/Battle/BattleUiIconCatalog.cs` | `c84884358a73afef8e6f13c67a8721457f94521a5f68aff65a977600754bfe34` |
| `Scripts/Battle/BeastCompanionSystem.cs` | `4ef77740a27a171d2ac92d1e13e3a7d7a7147d9314a0910ef2696c3a36066987` |
| `Scripts/UI/PetFacilityPresenter.cs` | `f53704395275bb4d74bae8c5f7eaffd682d4624592308eb6e102fcd413900f1b` |

새 Run GUID만 추가하고 기존 Wolf 아이콘 시트/meta/GUID는 보존했습니다. Catalog는 새3072×384 Run을512셀/12f/0.25배로 참조하고, 아이콘은 원래64셀 시트를 계속 사용합니다. Presenter 실행 로직·좌우 반전·속도570은 그대로입니다. 정적 KeyArt/Idle/Bear/Fox/독사/마을/Main23 수정0.

## 신규 확인과 미검증 구분

기존 실제 Unity Play Mode **106 PASS / 0 FAIL을 재사용**했습니다. 이번에는 원본에 반영한6파일을 readback하여 동일 SHA의 복사본만 기존 scratch Unity에 연결하고 **컴파일·새 시트 강제 Import·Resources 참조·3072×384 유지·Readable** 최소 확인만 실행했습니다. Unity6000.6.5f1, 종료코드0, `WOLF_IMPORT_DELTA_COMPLETE`, 컴파일 오류0. Play Mode 반복0입니다.

이 검사는 **원본 Editor Import가 아닙니다**. 사용자 원본 Editor에 Refresh/Play/Scene/포커스 명령0이며 원본 프로젝트 Lockfile은 확인 시 없었습니다. 원본 Editor 자연 Import 및 실제 Battle 입력·손플레이는 `NOT_VERIFIED`로 남깁니다. 기존 Search 환경 OPEN은 이 작업에서 수정하지 않았습니다. scratch 패키지 환경 차이는 기존106 QA 보고의 한계를 그대로 유지합니다.

## GitHub 통합 상태와 다음 단계

GitHub 최신 main은 제품 기준 e4e9891f와 동일합니다. 원격 Wolf 브랜치 검색0·관련 head PR검색0을 확인했습니다. 로컬 브랜치·제품6파일·검증·[PR 본문 초안](Evidence/WolfOriginalApply20261021/PR_DRAFT.md)은 준비됐습니다. 실제 Push/PR 생성/병합0이며 별도 사용자 지시가 필요합니다. 원본 디스크 적용 완료를 main 병합 완료나 원본 실손플레이 완료로 해석하지 않습니다. 이미 받은 미술 승인은 다시 요청하지 않습니다.

[원본 적용 증거](Evidence/WolfOriginalApply20261021/OriginalApplyResult.json) · [최소 Import 결과](Evidence/WolfOriginalApply20261021/ImportDelta.json) · [기존106 QA](Wolf_Run6_v5_Product_Apply_QA_20261021.md).
