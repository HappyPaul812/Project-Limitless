# Issue #86 완료 작업 통합·원본 로컬 동기화

> **최신 후속: F2 등록 HOLD 해소 / ORIGINAL_LOCAL_SYNC_DONE.** 바울 대표 후속 승인으로 기존25/app-ui를 보존한 F2 단일등록을 원본에 반영했습니다. 제품 `35b87d297392ba7d0e02c6657a94741c875abc10`, 신규표적26PASS/0FAIL. [최종 등록 보고](F2_BUILDSETTINGS_FINAL_SYNC_20261103.md). 아래는 등록 전 당시 통합 기록이며 HOLD 표기는 역사 상태입니다.

2026-11-03. **STAGING_INTEGRATION_PASS / APPROVED_FILES_LOCAL_SYNC_DONE / ORIGINAL_LOCAL_SYNC_HOLD(F2 BuildSettings 등록) / UNITY_VERSION_6000_6_5F1_CONFIRMED / REMOTE_PUSH_NO**.

## 실제 완료 범위와 남은 한 파일

승인 코드·자산을 별도 `codex/local-sync-approved-20261103` worktree에서 모으고 **신규 통합 경계 Unity44 PASS/0 FAIL** 후 원본 `main`을 `ab47b931fec92976170f9f50aabde1822d78bb90`→**`ac12abb0645f06fe84d4ae246b584a19d29ebba5`**로 실제 fast-forward했습니다. 원본에서 새 커밋을 만들거나 원격 Push/PR/병합하지 않았습니다. 후속 문서 커밋도 동일 안전 조건에서 ff-only 반영하며 최종 HEAD는 영수증/최종 보고에 기록합니다.

**전체 원본 플레이 동기화는 HOLD입니다.** 사용자 `EditorBuildSettings.asset`의 app-ui 설정과 기존25 Scene을 바이트 그대로 지켰습니다. 이 파일에 F2 후보 Scene을 추가하는26 Scene 제안은 **QA 복제본에만** 사용했습니다. 따라서 원본 프로젝트에서 북문→F2 로드까지 이미 작동한다고 주장하지 않습니다. 원본에 기능 코드·F2 Scene·승인 자산은 들어갔지만, 실제 진입을 위한 F2 Scene 등록은 아직 미반영입니다. 원본 개인 Save/사용자 Scene을 시험에 쓰거나 자동 교체하지 않았습니다.

## 버전 절대 고정

- 실제 설치: `E:/Program Files/Unity/Hub/Editor/6000.6.5f1/Editor/Unity.exe`
- ProductVersion **`6000.6.5f1_3ff58d469c8a`**, FileVersion6000.6.5.4191629.
- 실제 QA 로그: `Version is '6000.6.5f1 (3ff58d469c8a) revision 4191629'`.
- #79 정렬 후보 `5f619bc002a2b0725de65d154e377064884b5f43`의 정확한2줄을 staging에서 반영했습니다.
- 원본은 시작 시 Git HEAD에는 옛5.7이 있었지만 **사용자 working file은 이미 정확한6.5/revision**이었습니다. 승인 후보와 일치함을 증명한 뒤 이 버전 파일만 정상 `git add`하여 ff-only 선행조건을 충족했습니다. 파일 내용·바이트는 변경하지 않았습니다. 나머지 사용자 변경을 add/commit/stash하지 않았습니다.
- 현재 원본 **HEAD의 버전과 실제 파일 모두**6.5/revision 일치. 구버전 설치·다운그레이드0, Packages/공식BuildSettings 재생성0.

## 선행관계와 통합 커밋

| 항목 | 근거 커밋/상태 | 처리 |
|---|---|---|
| #46 6슬롯 Save | `356f9226b8ed83120f828709045d50a5041055a7` | 이미 원본 포함·중복 cherry-pick0 |
| #73 완료 표현 | `27f7d1db9c3510fd39ca8bc34f09d8b8cb20e9f1` | 이미 원본 포함·중복0 |
| #80 Main24 첫 구간 | `70e363bea43d72c7e68abb089952e2d6f6a77a8b` / 최종fd869f60 | 순차 이력 그대로 통합, 구매0 |
| #83 F2 첫 왕복 | `e32d814f7f0dee8622cf386541eab94bf1fc1d69` / 최종24232ccd | Scene/Portal/진행 코드 통합, 공식 BuildSettings 연결만 HOLD |
| #85 Owl 기본 전투 | `61dce08b271bd2e801205a711a8af7d54400c84f` / 최종47f479cc | 기존 UI Image 재생/피해/승패·귀환 코드·승인12프레임 통합 |
| Spider 승인 원화/Walk8·버전 | `ac12abb0645f06fe84d4ae246b584a19d29ebba5` | 승인 참조PNG9/meta 등록·버전 정렬 |

원본 ab47b931의 후손인47f479cc까지 승인 기능 이력이 연속되어 **staging ff-only**로 통합했습니다. Unity 제품 변경은 #80/#83/#85 세 커밋이며 Bee 타이밍·Writer 복구 코드는 포함되지 않았음을 확인했습니다. 코드 충돌0, 신규GUID 충돌0, Quest ID 중복0, 신규 Main24 Quest asset1, F2 Scene GUID/외부Sprite-Lit material/builtin 참조 해석 정상. 기존 투사체6meta의 GUID 중복1그룹은 과거 baseline으로 별도 기록했고 임의 수정하지 않았습니다.

## 승인 아트와 제품 활성화 구분

- Owl 승인 ZIP SHA `d6648b7451dc2184c2187c0b1c2f8e0f52fc800ac6975753d0f6d38f6fbf9192`의 Idle8/Attack4,8FPS/PPU512/중앙Pivot 데이터 유지. 기존 #85 후보 Definition/Encounter/표기를 그대로 통합했으며 **CANDIDATE ID·HP60 등 차용 수치를 정식 기획으로 승격하지 않았습니다**. EXP/탈렌트/아이템 보상0·한기/VFX 없음.
- NaturalBroken Stump v4 SHA `22f2f5816b3fd2ffe45234ec807bb3d81a83f87440eb8eb1f338e0dff33404a8` 유지.
- Organic Spider v5 SHA `c600cd2264f6a0323771288666d16b49aa213ad938036d0746ed6f7f1d50adb9`, 승인 Walk8은 #84 실물 SHA와 대조해 참조 등록했습니다. 위치 `Assets/_Project/Art/ApprovedReferences/ForestSpiderOrganicV5/`, PPU256/중앙Pivot,9Sprite 정상 로딩. **기존 forest_spider Definition/전투 스프라이트는 교체0**입니다. Attack/Shoot 미제작 및 발접지 정량 미검증을 완성으로 포장하지 않았습니다.
- 승인 신규PNG22(Spider9/Owl12/Stump1)와 기존 임시PNG3의 원본 대비SHA 동일. 기존 승인 환경·몬스터·펫·TTS·사용자Sprite 및 meta는 비승인 변경 없이 보존했습니다.
- F2 재사용68PASS의 Tools/시각 선택은 적용하지 않았습니다. 기존 #83 placeholder 환경/임시 검증 표기를 유지합니다. Sign/Fence·냉기VFX 미적용.

## 백업·충돌 판단·원본 보존

- 원본 시작: main ab47b931, 상세status858 / 기본344, index 미스테이징.
- 비파괴 백업 **847 파일**, SHA 대조 불일치0. 개인Save/UserData 등 제외11개는 **경로 메타데이터만** 분류하고 파일 열람/해시/복사0.
- 백업: `F:/study/codex/Project-Limitless/Temp/LocalSync20261103/Backup/UserChanges/`
- 상세 Manifest: `F:/study/codex/Project-Limitless/Temp/LocalSync20261103/BackupManifest.json`
- 원본 index 사전 보존: `Temp/LocalSync20261103/Backup/OriginalIndexBeforeSync`.
- 반영 전 기존 tracked7092·사용자847파일 해시 동일. 전체 통합 경로와 사용자 변경의 교집합은 **승인값과 정확히 같은 ProjectVersion 한 개**뿐이었습니다.
- 반영 후 사용자847 바이트 동일, 비대상 tracked 변경0. 승인 이력에 포함된 CURRENT_STATUS/CODEX/아트 인계4문서는 정상 갱신으로 따로 기록했습니다.
- 상세status **858→857**, 차이는 승인 버전 변경1개가 정상 commit 이력에 포함된 것뿐입니다. 다른 status 항목의 추가·삭제0, index staged0. 기존 사용자 Bootstrap/CharacterCreation Scene·Packages·app-ui/BuildSettings·GUID/meta와 신규 미커밋 파일 보존.
- 기존148 branch refs 그대로. Writer1/Writer2 #70/#77/#78 후보도 branch/커밋/작업폴더를 삭제·이동·활성화하지 않았습니다. 개인 데이터 결합0.
- `reset --hard`/clean/강제 checkout/ours-theirs/stash/원본 index 저수준 우회/원격 작업0.

BuildSettings는 기능과 별개로 **해당 파일만 HOLD**한 것이며 코드 병합이 불가능했다는 뜻이 아닙니다. QA 제안은 사용자 app-ui와 기존25 Scene의 순서/GUID를 유지하고 `World_NorthF2_CANDIDATE` 한 개만 추가했습니다. 후보/테스트 Scene 등록의 공식 반영을 자동 승인하지 않는 #86 계약에 따라 원본은 바꾸지 않았습니다. 다음 최소 작업은 이 **한 항목의 공식 등록 범위 검토**입니다. 다른 Scene/BuildSettings 전체 교체나 기존44/90/81 재검증은 필요하지 않습니다.

## 신규 최소 통합 QA

Unity6000.6.5f1 **44 PASS/0 FAIL**, 컴파일/Runtime 오류0(초기 QA 보조 코드 속성 선언 오류는 수정 후 최종 재실행).

- 합성 Main23 마지막 거점 완료→Main24 가용→스킬 안내 진행.
- 실제6슬롯 UI,6번 슬롯 장착·저장, ‘완료’ 실제 대화 버튼.
- 구매 없이 안내→북문→F2 실제 로드/Collider 경로→관찰→마을 복귀 진행.
- 기존 Battle UI에 승인 Owl 프레임 로드, 실제 방어/피해1회/턴 복귀·공격 승리→F2·패배→마을 안전거점.
- 기존 Save 서비스의 합성 슬롯 Continue로6번 슬롯·Main24 진행 보존.
- 보상/구매/Main24 완료·Main25 해금 없음, Spider 승인 참조9 정상 로딩.

**원본에서 QA하지 않았습니다.** staging 승인 Assets에 비파괴 백업의 사용자 Assets 변경과 원본의 현재Packages/ProjectSettings를 복제한 환경에서 실행했습니다. F2 BuildSettings 연결26Scene은 이 복제본에만 반영했습니다. 테스트 코드·증거와 합성 Save는 별도 QA 경로에만 존재합니다. 기존153/81/90/68/37/저장85·72·72/과거Main23 전수 재실행0.

[44개 실제 체크](Evidence/LocalSync86_20261103/checks.txt) · [QA 요약](Evidence/LocalSync86_20261103/QASummary.json) · [정적 통합 검사](Evidence/LocalSync86_20261103/StaticIntegrationQA.json) · [참조 해석/기존GUID중복](Evidence/LocalSync86_20261103/ResolvedSceneExternalReferences.json).

실제 Unity RT5장 및 단일 OFFLINE 열람판:
`F:/study/codex/Project-Limitless/Temp/LocalSync20261103/Review/index.html`
SHA `57a1b11b8359a25fe7bd1d8ae560193c83243a4995c8229b863450cce02e6e90`.
내용은 실제 Camera RT이며 HTML은 OFFLINE·OS GameView 아님. 사람 OS입력·원본 Editor 손플레이는 NOT_VERIFIED입니다.

## 별도 보존/HOLD

| 대상 | 현재 보존 위치/상태 |
|---|---|
| Bee v3 | `codex/bee-sting-recover-v3-delta-review-20261103` ec56fa63, ART_OK_CANDIDATE/미술승인대기·09프리뷰정리/접촉미검증/RIGHTS_HOLD |
| Bee 타이밍A | `codex/bee-attack-timing-isolated-20261103`4c6d5c53,153PASS 보존·본편제외 |
| F2 환경재사용 | `codex/f2-environment-reuse-delta-20261103`28e9fae7,68PASS/USER_ART_REVIEW_PENDING·제품미적용 |
| Writer fork/복구 | issue70-writer-fork-safety a6f2b0d, recovery-confirmed fed9248f, production-recovery-gate fd657384 등 기존 refs148 보존·운영 root/계보/복구 UI Bootstrap 활성화0 |
| 몬스터·진행 미완 | Owl 한기/VFX·정식Monster ID/밸런스/보상·Spider Attack8/Shoot8·Main24완료/Main25·상품구매 모두 HOLD |
| 환경 미승인 | F2 Sign/Fence 대안·재사용최종외형 미승인·Ch4 메카닉거미 ARCHIVE_ONLY |

최종 기술 상태와 보류를 혼동하지 않습니다. 승인 파일의 원본 반영은 수행했으며, **원본 F2 진입 설정을 포함한 전체 플레이 동기화는 HOLD**입니다. Push/원격PR/병합은0입니다.
