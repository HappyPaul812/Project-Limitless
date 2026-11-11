# Issue #86 F2 Scene 단일 등록 — 마지막 HOLD 해결

**F2_BUILDSETTINGS_SCENE_REGISTERED_PASS / ORIGINAL_LOCAL_SYNC_DONE / UNITY_VERSION_6000_6_5F1_CONFIRMED / REMOTE_PUSH_NO**.

2026-11-03 바울 대표의 후속 승인으로 F2 항목 하나를 원본에 실제 반영했습니다. 기존 승인 코드·아트를 다시 통합하지 않았습니다. 제품 커밋 **`35b87d297392ba7d0e02c6657a94741c875abc10`**, branch **`codex/f2-buildsettings-final-sync-20261103`**. 원본main은8db12b57에서 이 커밋으로 정상ff-only 했습니다. 후속 문서/영수증도 별도 브랜치 커밋 후 ff-only 반영하며 최종HEAD는 영수증/최종증명에 기록합니다.

## 정확히 바뀐 것

`Unity/Client/ProjectSettings/EditorBuildSettings.asset`의 기존25 Scene 뒤에 아래 **3줄만** 추가했습니다.

```yaml
  - enabled: 1
    path: Assets/_Project/Scenes/World_NorthF2_CANDIDATE.unity
    guid: bcd0e6f38e69dc141b741fe5247855cf
```

실제 기존 `.unity`·`.meta`·Main24F2Traversal.SceneId와 일치합니다. 새 Scene 제작/이름변경/GUID변경0, 중복 등록0, 기존25의 path/GUID/order/enabled 모두 동일. 사용자 app-ui 및 input 설정을 포함한 `m_configObjects` 이하 **전체 바이트 동일**, 다른 설정/주석/순서/줄바꿈 변경0.

원본 working BuildSettings SHA256:

- 이전25Scene+사용자설정: `ec6529bc0f343fedb06cd08c99f7c5be475b0d3d061ef3e24379cbf02671245a`
- 이후26Scene+동일설정: `404a655f53ce5122775944db21d3b4d2212d65fc53390f0c65b493bdf50bf9f4`

[25/26 항목·SHA·설정 Manifest](Evidence/F2BuildSettingsFinal86_20261103/SceneAndSettingsManifest.json).

## 사용자 미커밋 변경을 유지한 반영 절차

별도 worktree의 제품 커밋에는 **F2 3줄만** 들어갑니다. 사용자 app-ui 변경을 커밋에 섞지 않았습니다.

1. 원본의847 보호파일·HEAD/status/index·설정SHA를 기록하고 기존 BuildSettings/index를 Temp에 별도 보존했습니다. 개인Save/UserData는 읽지 않았습니다.
2. app-ui 변경이 있는 동일 구성의 별도 Git fixture에서 작업파일 patch→승인F2 hunk만 `git apply --cached`→정상ff-only 절차를 먼저 검증했습니다.
3. 기본 Git 줄바꿈 정규화가 원본 LF를 바꿀 수 있음을 fixture에서 발견해, 작업파일 patch 명령에만 `-c core.autocrlf=false`를 사용했습니다. **사용자 Git 설정을 수정하지 않았습니다.**
4. Unity26 PASS·QA BuildSettings 자동변경0·원본충돌0 확인 후 원본에도 같은 최소hunk만 적용/선택적staging/ff-only 했습니다. 전체 파일 덮어쓰기·main 직접commit·강제checkout·reset/clean/stash·저수준index우회0.

[전체 Git 복제 시험](Evidence/F2BuildSettingsFinal86_20261103/GitFullPreflight.json) · [원본 실제 반영/보호 증명](Evidence/F2BuildSettingsFinal86_20261103/AppliedProof.json).

기존 사용자파일847개 중 **846개 바이트 동일**, BuildSettings만 승인3줄 변경이며 이를 제거하면 기존 전체파일과 정확히 같습니다. app-ui 사용자 변경은 여전히 미커밋이며 변경hunk의 실제추가/삭제내용도 동일합니다. status343/상세857 유지·indexstaged0. ProjectVersion·Packages SHA 불변, Scene/Prefab/아트/몬스터데이터/기존게임코드 변경0.

## 표적 Unity QA — 신규26 PASS /0 FAIL

실제 설치된 `E:/Program Files/Unity/Hub/Editor/6000.6.5f1/Editor/Unity.exe`, revision **3ff58d469c8a**를 숨김Batch로 실행했습니다. ProjectVersion 정렬/수정·구버전 설치/다운그레이드0.

- Editor에서26개/중복0/26번째 활성F2/실제GUID 확인.
- 기존 북문 상호작용→F2 실제 Scene 로드.
- 합성F2 Save→실제Restore/Continue→등록F2 로드.
- F2 기존 출구 상호작용→마을 복귀→북문으로 F2 재진입.
- 기존Owl 전투 승리→등록F2 복귀, 합성HP1 패배→기존마을안전거점 복귀.
- 경제/보상/한기/정식몬스터기획/Main24완료 등의 변화 없음, 최종컴파일오류0·게임Runtime오류0.
- **Unity 초기import/PlayMode/종료 전후 QA BuildSettings SHA가 위26Scene 후보와 정확히 동일**. 자동 재직렬화 변경0.

[26개 실제 체크](Evidence/F2BuildSettingsFinal86_20261103/checks.txt) · [Editor에서 읽은 GUID](Evidence/F2BuildSettingsFinal86_20261103/EditorPreflight.txt) · [오류기록](Evidence/F2BuildSettingsFinal86_20261103/Errors.txt).

기존44/81/90·저장 전수QA는 호출하지 않았습니다. 실제 데이터는 새 합성Save/Settings 디렉터리에만 저장했습니다. 원본Editor를 실행하거나 사용자포커스를 전환하지 않았습니다.

## 검증 범위와 원본 동기화 상태

동일한 원본 구성·BuildSettings를 사용한 **격리 실제 Unity 이동 검증 PASS**, 원본 반영 후 목록/GUID/설정/바이트 일치까지 확인했습니다. **원본Editor 자체의 손플레이는 NOT_VERIFIED**입니다. 이 한계는 등록HOLD가 남았다는 뜻이 아니며, 승인한 파일 동기화는 완료됐습니다.

실제RT3장 OFFLINE 열람판: `F:/study/codex/Project-Limitless/Temp/F2BuildSettingsFinal20261103/Review/index.html`. HTML은 OFFLINE이며 내용은 실제CameraRT/OS GameView 아님. 경로/해시는 [QA요약](Evidence/F2BuildSettingsFinal86_20261103/QASummary.json).

원본 사전백업/보호 Manifest:
`F:/study/codex/Project-Limitless/Temp/F2BuildSettingsFinal20261103/ProtectionBefore.json`
기존25Scene 원본: 같은 폴더의 `OriginalBuildSettings.asset`, index: `OriginalIndexBefore`. 앞선 사용자847파일 비파괴백업은 `Temp/LocalSync20261103/Backup/UserChanges/`에 그대로 보존했습니다.

정식한기·수치/보상·미승인환경·독침벌·Writer복구 작업은 전혀 건드리지 않았습니다. Push/원격PR/병합0. 마지막 F2 등록 HOLD만 해소했습니다.
