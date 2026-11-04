# Issue #89 승인 냉기 VFX v2 격리 시각 Delta QA / 2026-11-04

## 결과
**UNITY_VISUAL_DELTA_EXECUTED / 12_CONDITIONS_CAPTURED / AUTOMATED_99_PASS_0_FAIL / VISUAL_PARTIAL / NOT_ORIGINAL_APPLIED**.

최신 [기획 실행 지시](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6621778475)를 확인하고 Unity **6000.6.5f1 (3ff58d469c8a)** Batch Mode의 실제 Play Mode·Camera/Canvas RenderTexture로 실행했습니다. 엔진 최종 종료코드0, 컴파일 오류0·새 Runtime 오류0. 기존 UnityEditor.Search.SearchDatabase 시작 예외1은 환경 문제로 별도 기록했습니다. EnvironmentErrors.txt는 재시도 누적 기록이며 최종1회 판정은 최종 로그를 기준으로 했습니다. 기존 #88 63PASS와 #84/#85 등의 전수 QA는 호출하지 않았습니다.

## 입력과 QA 가정
승인 ZIP은 `F:/Downloads/Limitless_Ch3_ColdVFX_Issue89_MinimalFollowup_20261104.zip`, 이전 단회 검증 SHA `9c07f5ff782c6e272796c4f003b816a39806ab35248c2353884325ac3c24a1eb`를 재사용했고 ZIP 전체 SHA/CRC/Manifest 전수 재검사는 하지 않았습니다. 승인 v2 PNG 3장만 격리 폴더에 그대로 추출했으며 ApprovedInputProtection.json의 개별 SHA·원본 ZIP 엔트리 바이트 일치3/3을 확인했습니다. 승인아트 수정/재생성0.

기존 격리 프로젝트 `Temp/Nov04F2Smoke20261104/QA/Client`와 #88의 Battle 진입·합성 Save 가드를 재사용했습니다. 새 실행기는 기존 상태 로직 테스트를 호출하지 않습니다. 1인 휠체어 캐릭터에 세 상태를 각각 렌더하여 파티를 새로 만들지 않았습니다. 실제 Battle HUD/상태배지 생성기, 실제 2행3열 스킬 메뉴(시험용 여섯 스킬 스냅샷)와 선택 강조를 사용했습니다. 스킬 실행/피해/저장복원 테스트0.

- 40/128은 투명 PNG **전체 512캔버스의 화면 투영 크기**입니다. 가시알파 덩어리가 항상40/128인 뜻이 아닙니다. 캔버스 배율로 나누어 Image 크기를 맞추고 실제 화면 rect를48회 측정했습니다.
- PNG Sprite.Create PPU512·중앙 pivot, UI Image scale1·Bilinear는 **QA 가정**입니다. 공식 VFX PPU/Pivot/WorldScale 확정이 아닙니다.
- 밝은 배경은 기존 격리 F2의 카메라를 실제 렌더한 `F2_Source_Camera.png`를 Battle 패널에 표시한 시험입니다. 배경 촬영 중 월드 Canvas/플레이어 Renderer만 잠깐 숨겼다가 복원했습니다. F2에 VFX를 영구 배치한 것과 다릅니다. 어두운 배경은 기존 Battle NightSky/DistantField/BattleGround입니다.
- VFX는 실제 캐릭터 SpriteImage 기준 아래38 UI 단위의 시험 위치에 겹쳤습니다. 별도 상태명/턴/중첩 안내를 QA용 Text로 병행했습니다. 실제 상단 HUD 아이콘 연결에는 승인 정지 VFX를 임시 사용했습니다. **최종 전용 아이콘·배치 승인이나 실제 효과 생명주기 구현이 아닙니다.**
- 한기2턴·동상2턴·동결괴사1/3,2/3 및 다음 중첩 즉시 KO를 합성 상태로 표시했습니다. 지속3/3은 만들지 않았습니다. 동결괴사1/3·2/3은 같은 승인 대표 정지 그림을 사용하고 숫자 UI로 구분했습니다.

## 화면 조건과 판정
| 화면 | 밝은설원40 | 밝은설원128 | 어두운전장40 | 어두운전장128 |
|---|---|---|---|---|
| 960x720 | 캡처완료/시각PARTIAL | 캡처완료/시각PARTIAL | 캡처완료/시각PARTIAL | 캡처완료/시각PARTIAL |
| 1280x720 | 캡처완료/시각PARTIAL | 캡처완료/시각PARTIAL | 캡처완료/시각PARTIAL | 캡처완료/시각PARTIAL |
| 1680x720 | 캡처완료/시각PARTIAL | 캡처완료/시각PARTIAL | 캡처완료/시각PARTIAL | 캡처완료/시각PARTIAL |

한기: 128px은 설원에서도 바닥 고리/눈송이·연무가 보이고 어두운 전장에서는 더 선명합니다. 40px은 고리가 휠체어 바퀴의 청색 장식과 겹쳐 상태 자체의 실루엣 인지가 약합니다. 흰색 고리 일부는 보이나 VFX 단독 가독성 PASS로 판정하지 않습니다.

동상/동결괴사: 128px은 밝은 결정군집과 검푸른 큰 쐐기가 구분됩니다. 40px은 둘 다 바퀴 주변 작은 파란 결정덩어리가 되어 형태만으로 안정적으로 구분하기 어렵습니다. 별도 상태명과 아이콘·숫자 UI가 필요합니다. 승인 그림의 재작화가 아니라 **부착 위치/표시 크기/전용 아이콘**을 먼저 검토할 근거입니다.

가림: 12조건의 기본 화면에서 VFX가 얼굴·상단HP/상태행·하단2x3스킬/선택강조·돌아가기 버튼을 덮지 않는 것을 실제 캡처로 확인했습니다. 다만128px은 동상/동결괴사가 휠체어·하체를 크게 가려 캐릭터 전체 가림없음 PASS는 불가합니다.40px은 작은 크기 자체보다 바퀴와의 겹침이 식별을 방해합니다. 상단 상태배지는 실제 생성됐지만 특히960에서 글자가 매우 작고 인접 상태가 밀집하여 접근성 가독성은 PARTIAL입니다.1680에서는 기존 상단HUD 패널과 전장 상단 구역이 겹치는 배치도 보이며 VFX에 의한 새 겹침으로 오인하지 않습니다.

별도 초기 팝업 캡처에서는 기존 스킬 상세 팝업이 전장 중앙과 VFX 일부를 덮었습니다. 이후 기본 시각 캡처는 상세 팝업만 닫고 메뉴/선택 강조는 유지했습니다. 팝업 가림을 승인 VFX 파일 결함으로 단정하지 않으며 원본 UI 코드는 수정하지 않았습니다. 대표 팝업 캡처는 원본 workspace `Temp/Issue89V2VisualDelta20261104/PopupOcclusionAttempt/`에 보존했습니다.

## 검사 수와 실제 증거
최종 자동99PASS/0FAIL은 진입준비2·실제상단HUD라벨1·48캡처 각각의 투영크기/출력크기96입니다. **미술/접근성99개 통과를 뜻하지 않습니다.** 시각12조건은PARTIAL이며 새 제품 Runtime 오류0입니다. 초기 실행기 resolver형식오류2와1인파티 가정 오류는 수정 후 재실행했고 초기 로그/Attempt3를 Temp에 보존했습니다. 제품 오류로 집계하지 않습니다.

실제 Unity PNG는 상태3종×12조건36장 + 동결괴사1/3 보충12장 + F2소스1장 = **49장**입니다.48장의 전장 캡처 모두 실제 Unity RenderTexture 결과이며 픽셀을 사후 합성하지 않았습니다. NativeCrops960/1280/1680은 이 실캡처를1:1로 잘라 나란히 놓은 **파생 비교판**입니다. 실제 원본PNG와 구분합니다.

- 증거 폴더: [Evidence/Issue89V2VisualDelta20261104](Evidence/Issue89V2VisualDelta20261104/)
- [960 실캡처](Evidence/Issue89V2VisualDelta20261104/Snow_960x720_40px_Chill.png), [1280·128px 동결괴사](Evidence/Issue89V2VisualDelta20261104/Dark_1280x720_128px_Necrosis.png), [1680 실캡처](Evidence/Issue89V2VisualDelta20261104/Dark_1680x720_40px_Chill.png)
- [49장/측정/검사 SHA·bytes](Evidence/Issue89V2VisualDelta20261104/CaptureManifest.json), [검사결과](Evidence/Issue89V2VisualDelta20261104/Checks.txt), [최종 Unity 로그·로컬IP/기기ID 비식별 및 행끝공백 정리본](Evidence/Issue89V2VisualDelta20261104/unity-final-verified.log)
- 로컬 전체 열람판: `F:/study/codex/Project-Limitless/Temp/Issue89V2VisualDelta20261104/index.html`.
- 실행 코드: Tools/Issue89V2VisualDelta/Cold89VisualBatch.cs, Cold89VisualProbe.cs. 실제 QA프로젝트의 생성/추출 경로만 수정했고 원본Assets에 검증코드를 넣지 않았습니다.

## 보호와 다음 판단
원본 main HEAD `634a91d3e4174c87761c0d377cfa5a695ca79e7f` 유지, tracked미커밋항목수308→308입니다(개별파일 전체바이트불변 검사를 대신하는 수치 아님). 이번 쓰기는 Temp 격리 fixture와 별도 문서 worktree에만 수행했습니다. 개인 Save/UserData 열람/복사/해시/수정0, 사용자 원본 Editor Scene/포커스/PlayMode 변경0, 기존 승인 Owl/Spider/환경 PNG 재작화0. 원본 게임 적용·Packages/ProjectVersion 변경·상태/해제/화로 정식연결·애니/Particle양산0. 원본 상태 조회만 읽었으며 사용자 화면 조작0.

다음은 승인 그림을 보존한 채 VFX의 휠체어/캐릭터 부착 위치·128px 점유·40px 별도 상태 아이콘/상단HUD의 최소 가독성 정책에 대한 사용자 시각 판단입니다. 이번 결과로 재작화를 자동 발주하거나 기승인 미술을 취소하지 않습니다. 다른 외형·전체 파티·실제 아이콘·동적VFX 생명주기/광과민/성능은 이번 정지 시각QA 범위 밖입니다.

브랜치 `codex/issue89-v2-unity-visual-delta-20261104`, 직전문서 `a4cd85af`. 최종commit SHA는Issue#89결과댓글에 기록합니다. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 캡처는 로컬 브랜치에 보존하며 원격 PNG업로드/Push/PR/merge를 수행하지 않습니다. Issue댓글에는 실제 파일 경로·SHA·결과를 보고하고 GitHub에 캡처가 첨부됐다고 주장하지 않습니다.
