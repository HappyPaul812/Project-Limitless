# Issue #93 — 숲거미·Main11 동행 대사·던전 자유 귀환

상태: **수정 완료 / LOCAL_MAIN_APPLIED / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**. 기준 local main `674e6b0f4a613ad7caaa0ee3a94f3f5f0b3e592d`, Unity `6000.6.5f1 (3ff58d469c8a)`.
브랜치: `codex/fix-20261105-dungeon-spider-dialogue`. 원격 Push/PR 병합은 하지 않는다.

## 실제 원인과 수정

1. **숲거미**: 로컬 `03_ForestSpider.asset`의 ExplicitAttackFrames는 승인 Attack8이지만 `shootFrameResourcePath`에 구 `MonsterAnimations/Spider/Shoot`가 남아 있었다. 실제 BattleSceneController의 `PlayBasicAttack`은 승인 `00.png`, `PlayMonsterDirectAreaAttack`은 `PlayShoot`를 통해 구 `spiderS_000.png`를 표시했다. Shoot 경로를 비워 기존 PlayAttack fallback으로 승인 Attack8을 공통 사용한다. Shoot8을 승인·제작한 것으로 취급하지 않는다. 피해·대상·효과·쿨타임·Battle Controller 코드는 변경하지 않았다.
2. **Main11**: 조사 대사와 보스 후 대사가 실제 Active Party를 검사하지 않았다. 슬롯02·03 복제본은 모두 조사 index2, 태온·미엘 동행이므로 폴의 `main11_paul_002`가 비동행 발언이었다. 미엘의 index1 `main11_miel_002`는 실제 동행 중인 정상 대사다. 실제 동행자는 기존 원문·TTS ID를 유지하고 부재 인물만 의미에 대응하는 무음 지문으로 바꾼다. 폴에게 직접 말을 거는 미엘의 질문도 폴 부재 시 지문으로 처리한다. 기존 동료 원문·ID 28/28 동일. 현재 목표·거리·대화 완료 후 순차 진행을 유지하고 `CanBeginInteractionThisFrame` 검사를 추가했다.
3. **던전 귀환**: B1/B2의 기존 남쪽 출구는 `(0,-11.9)`의 보이지 않는 자동 Trigger였고 안내·명시적 상호작용·확인창이 없었다. 두 Scene 모두 Bounds와 출구/Spawn이 존재했으며, 물리 경로 자체가 막혔다고 확정하지 않는다. 기존 남쪽 Trigger만 Runtime 비활성화하고 입구 안쪽 `(0,-10.6)`에 안내 버튼과 확인창을 설치했다. B1→Field_03/`Spawn_From_Dungeon01`, B2→B1/`Spawn_From_B2`. E/F/A는 선택된 확인·취소 버튼을 사용하고 마우스 Button도 제공한다. 모달·같은 프레임 재입력과 입장 직후 자동 재전환을 차단한다. 귀환은 기존 SceneTransitionService와 Spawn의 정상 저장만 사용하며 퀘스트·몬스터·인벤토리를 직접 갱신하지 않는다.

## 수정 파일

- `Unity/Client/Assets/_Project/Resources/MonsterDefinitions/03_ForestSpider.asset`
- `Unity/Client/Assets/_Project/Scripts/Monster/MonsterSpriteSheetAnimation.cs`: Shoot fallback 및 이전 Hit/Skill 상태 정리.
- `Unity/Client/Assets/_Project/Scripts/World/MainQuest11DungeonFlow.cs`: 동행 대사·조사 입력 차단·출구 설치.
- `Unity/Client/Assets/_Project/Scripts/World/DungeonExitInteraction.cs`와 신규 meta: 명시적 귀환 안내/선택.
- `Unity/Client/Assets/_Project/Scripts/UI/DialoguePresenter.cs`: 확인창을 여는 입력이 같은 프레임에 확인·취소까지 실행되지 않게 차단.
- `문서/03_스토리/Main11_침묵의_지하묘지.md`: 현재 동행·귀환 동작 반영.

## 실제 Sprite 출처

| 경로 | Sprite / GUID / 원본 |
|---|---|
| 수정 전 일반 공격 | `00` / `b9f82a5d87642766fb00d2de0b6a43a4` / `Assets/_Project/Resources/Approved20261104/Spider/Attack/00.png` |
| 수정 전 독액 분사 | `spiderS_000` / `cb000000000000000000000000000124` / `Assets/_Project/Resources/MonsterAnimations/Spider/Shoot/spiderS_000.png` |
| 수정 후 두 공격 | 승인 Attack8 공통. 전체 프레임 이름·GUID·경로는 로컬 `Temp/Issue93/Evidence/extra/sprite.txt` 참조. |

## 검증 경계와 증거

- 실제 숨김 batch Unity Play Mode, Camera.Render→RenderTexture 캡처, Input System 가상 장치 및 UI Button 콜백으로 검증한다. 사용자 창/OS 포커스/GameView 활성화나 Computer Use는 사용하지 않았다.
- 원본 슬롯02·03은 `Temp/Issue93/Private/`에 바이트 복제했다. 각 실행에서 하위 별도 Save 폴더에 다시 복사하며 원본 UserData를 사용하지 않았다. 개인 Save 본문은 Git/GitHub에 포함하지 않는다.
- 저장소의 구 Packages 조합은 6000.6.5f1에서 `SetChecked` API 컴파일 오류가 있었다. 사용자 실제 로컬 미커밋 `manifest.json`·`packages-lock.json`을 검증 worktree에만 복사했다. 패키지 갱신을 제품 커밋으로 채택하거나 원본 파일을 덮지 않았다.
- QA 실행기의 참가자 조회·동일 Scene 로드 대기·비포커스 가상 키보드 활성화 오류는 검증 코드를 보완해 재실행했다. 초기 실패를 제품 PASS로 집계하지 않는다. 새 worktree의 지연 define 재컴파일은 batch 실행 중 reload 지연으로 처리했다.
- 기존 전체 QA는 재실행하지 않았다. 실제 이전 결과 60/52/16 등을 이번 검사에 합산하지 않는다.
- 기존 Unity Editor Search의 `ArgumentOutOfRangeException`은 수정 전에도 나타나는 별도 Editor 내부 문제다. 프로젝트 C# 컴파일 오류와 구분하며 Console 전체 무오류를 주장하지 않는다.

최종 검증은 기본 Delta **121PASS/0FAIL**, 보완 **37PASS/0FAIL**, 마지막 입력 **15PASS/0FAIL**이다. 중복 항목이 있으므로 합산하지 않는다. 정적 검사7PASS/0FAIL과 git diff --check도 통과했다. 마지막 보완에서 패드 A 확인을 명시적으로 선택 버튼에 연결했고, 그 변경 후 A 확인과 E/F 취소·확인·동일 프레임 중복 차단을 재검증했다.

실제 독액 분사의 피해 훅이 생존 아군 각각20 피해를 적용함을 관찰했다. 보스 처치·석판 보유 상태는 복제 데이터에서 목표를 구성한 별도 fixture로 검증했으며, 기존 보스 전투 전체를 다시 실행한 것은 아니다. 몬스터 재생성 대기, 보스 장벽 개방, 퀘스트 미완료, 석판1개, Save/Continue·재입장 유지가 통과했다.

구현 커밋 **`6f094a4095e9298d20e879d842f29b02bca6587a`**을 검증 후 원본 local main에 FF 반영했다. 변경 경로와 사용자 dirty/untracked 겹침0, 전후 보호942파일 SHA변경0, 원본Save02·03 동일. 승인Spider24 PNG가 포함된 Approved20261104 PNG29도 변경0. main 직접 커밋·원격 Push/병합0. 자동 승인 검토가 처음에는 FF를 병합 금지로 해석했으나, 사용자 명시적 조건부 로컬 반영 권한과 실제 안전 게이트 근거를 제시한 재검토 후 승인·실행됐다.

구현 범위의 자동 Delta 검증이 통과했다. 실제 물리 키보드/패드/마우스를 사용한 장시간 플레이·가독성은 가상 입력/Camera.Render 검증과 별도로 사용자 확인 사항이다. Shoot8은 계속 미승인이다.

## 게임 렌더 전후 캡처 (로컬 전용)

- [수정 전 독액 분사](../../Temp/Issue93/Evidence/before/PlayMonsterDirectAreaAttack.png) / [수정 후](../../Temp/Issue93/Evidence/after/PlayMonsterDirectAreaAttack.png)
- [수정 전 폴 대사](../../Temp/Issue93/Evidence/before/slot2-step2.png) / [수정 후 조사 지문](../../Temp/Issue93/Evidence/after/slot2-step2.png)
- [수정 전 B1 입구](../../Temp/Issue93/Evidence/before/slot2-B1-entrance.png) / [수정 후 귀환 안내](../../Temp/Issue93/Evidence/after/slot2-B1-exit-visible.png)
- [B1 확인창](../../Temp/Issue93/Evidence/after/slot2-exit-confirm.png) / [B2 확인창](../../Temp/Issue93/Evidence/after/slot2-B2-return-confirm.png)

캡처는 실제 Unity 게임 렌더이며 오프라인 합성 그림이 아니다. 외부 원격 바이너리 업로드는 수행하지 않는다. 슬롯별 검사와 원본 보호의 비공개 근거는 `Temp/Issue93/Private/`, 공개 가능한 결과만 `Evidence/Issue93_20261105/`에 선별한다.


원본 main 반영 후에도 백그라운드 AssetDatabase refresh·컴파일을 완료했다. 실제 Application.dataPath가 원본 Client임을 확인하고 신규 DungeonExitInteraction 타입 로드, compiling=false, C# Console 오류0을 확인했다. 원래 열려 있던 외부 worktree Bootstrap Scene은 그대로이며 dirty=false·Play=false를 보존했다.
