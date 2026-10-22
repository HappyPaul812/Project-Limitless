| 범위 | 판정 | 한 줄 사유 |
|---|---|---|
| ZIP·PNG·Strip/Atlas·CSV·Unity | **기술 PASS** | 신고 SHA 일치, CRC/Manifest26/SUMS27·6셀/CSV6 정상, v5 정상5프레임 바이트 보존·Frame03만6275픽셀 변경 |
| Frame03 및 연속 모션 | **미술 PASS 권고** | v5의 분리된 흰 쐐기 제거·몸통에서 다리 실루엣 연결 개선, 02→03→04와05→00에서 뚜렷한 새 단절/발방향 급반전 미관찰 |
| 대표 최종 승인·제품 적용 | **승인 대기 / 미적용** | Codex 미술 권고이며 사용자 최종 승인이 아님. 제품쓰기·Push·병합0 |

# Fox Run6 v7 Frame03 앞다리 국소 복원 격리 QA

[대표 확인용 실제 모션·비교 자료](Evidence/FoxV7ForelegQA20261022/index.html) · [Frame03 동일4배 비교](Evidence/FoxV7ForelegQA20261022/ActualUnity_Frame03_v5_v7_Focus4x.png) · [02→03→04 실제 캡처](Evidence/FoxV7ForelegQA20261022/Sequence_02_03_04.png)

## 기준과 GitHub 확인

최신 [리뉴얼 납품 보고](https://github.com/HappyPaul812/Project-Limitless/blob/docs/art-renewal/문서/00_프로젝트/Fox_Run6_v6_VisualReject_v7_Frame03_LocalForeleg_Repair_20261022.md), [리뉴얼 인계](https://github.com/HappyPaul812/Project-Limitless/blob/docs/art-renewal/문서/00_프로젝트/팀운영/인계/WORLD_ART_RENEWAL_LATEST.md), [기획 인계](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/팀운영/인계/PLANNING_LATEST.md), [Issue #50의 v7 접수 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/50#issuecomment-6341903480)을 읽기 전용 확인했습니다. 조회한 본문·발췌는 증거 폴더에 보존했습니다.

**정상 비교 기준은 v5 ManifestFix**입니다. v6는 사용자 미술 반려본·게임 사용 금지이며 정상 포즈나 승인 기준으로 쓰지 않았습니다. v7 납품 내 v6 비교 Preview도 본 미술 판정의 정상 기준으로 사용하지 않았습니다. v6 반려를 v7에 자동 전가하지 않고 별도 판정했습니다.

## 실제 ZIP 무결성 — PASS

- 파일: `F:\Downloads\Limitless_Beast_Fox_Run6_v7_Frame03_ForelegFix.zip`
- 실제 크기 **2,990,546bytes / 28항목**, 실제 SHA256 **`de6139858b2f5de95c9e8256a350f0e45744f15e404efb00161688c88e1c0030`** — 사용자/납품 신고와 일치.
- ZIP CRC PASS, Manifest 해시·바이트·CRC **26/26 PASS**, SHA256SUMS **27/27 PASS**. Manifest 자신/SUMS 자기 참조 제외, 외부 SUMS가 최종 Manifest 포함.
- 재검수 종료 시 ZIP SHA 재독 확인·동일. [PackageQA](Evidence/FoxV7ForelegQA20261022/PackageQA.json) / [원문 Manifest](Evidence/FoxV7ForelegQA20261022/Manifest.json) / [SUMS](Evidence/FoxV7ForelegQA20261022/SHA256SUMS.txt).

## v5 기준 변경 범위 — PASS

v5 ZIP은 현재 Downloads에 없지만 **이전 실제 검수 때 추출해 보존한 원본 PNG6**을 직접 읽었습니다. 먼저 그6개를 v5 ManifestFix의 검증된 Manifest SHA와 재대조한 뒤 v7 PNG와 직접 바이트/픽셀 대조했습니다. 단순 제작자 주장이나 v6 포즈를 기준으로 삼은 검사가 아닙니다.

| 프레임 | v5 대비 결과 |
|---|---|
| 00·01·02·04·05 | **압축 PNG 바이트 동일5/5**, 변경 RGBA0 |
| 03 | **6,275 RGBA픽셀 변경**, 변경 bbox **[159,185,256,295)** |

Frame03 변경 사각형 밖 **모든 RGBA 동일**을 확인했습니다. 머리·귀·꼬리 및 사각형 밖 몸통/다른 위치에 불필요 변경은 없습니다. 국소 가슴 아래 앞다리 복원 범위를 전신 재제작으로 확대하지 않았습니다. v6의 60,629픽셀 전신 교체 패턴이 재도입된 것이 아니라는 것은 v5 대비 실제6275개 국소 변경으로 구분합니다.

동봉 `Frame03_Changed_RGBA.csv`의 **6275행 모두** x/y 중복 없음, 실제 변경 좌표 집합과 완전 동일, 각 v5/v7 RGBA 채널 값까지 일치했습니다. [FrameQA](Evidence/FoxV7ForelegQA20261022/FrameQA.json) / [CSV 전행 검증](Evidence/FoxV7ForelegQA20261022/ChangedCSV_Verified.json) / [원문 변경 CSV](Evidence/FoxV7ForelegQA20261022/Frame03_Changed_RGBA.csv).

## PNG·Strip/Atlas·메타데이터 — PASS

- PNG6 **512×384 RGBA**, 프레임 외곽3px alpha 침범0.
- Strip **3072×384**, Atlas **1536×768**, 각셀과 개별 PNG **원시 RGBA6/6 정확 일치**. v5에서 남았던 완전투명RGB 차이도 이번 v7은 없습니다.
- bbox CSV **6/6 PASS**. 헤더/열 순서 `frame,bbox_min_x,bbox_min_y,bbox_max_x_exclusive,bbox_max_y_exclusive`, PNG alpha>0·좌상단/x우/y하·max-exclusive 계약, 점유 픽셀/최하단 y/변경 픽셀 수를 재계산했습니다.
- 납품 GIF128·40 및 APNG 각각6프레임·**총500ms** 확인. GIF는10ms 단위 혼합으로500ms를 구성합니다. v5의480ms Preview 문제를 v7에 자동 승계하지 않았습니다.
- 격리 Sprite Import 설정 **14/14 PASS**: PPU128, Point, mip없음, 압축없음, 읽기 가능, 하단중앙 Pivot, 기본 플랫폼 최대4096. [ImporterQA](Evidence/FoxV7ForelegQA20261022/ImporterQA.json).

## 실제 격리 Unity Play Mode

Unity **6000.6.5f1 / Direct3D12**, 사용자 원본과 별도 scratch의 숨김 Batch 실제 Play Mode입니다. 원본 Editor의 Scene/선택/Play/Refresh·OS 포커스/키보드/마우스 조작은0입니다.

- 재생 설정 **12FPS**, 실시간·engine frame 증가를 기록한 **24샘플 / 4루프**.
- Engine frame **3→212**, 평균 간격 **83.6918ms**, 실측 평균 **11.9486FPS**. 루프 실측 **0.508240 / 0.499500 / 0.502025초**. 렌더 프레임 경계의 편차를 포함하며 벽시계12.000FPS라고 확대하지 않습니다.
- 카메라·배율·PPU 동일. 512px canvas를 .25배=**128px**, .078125배=**40px**로 표시했습니다. 좌열v5좌향/중앙v7좌향/우열v7우향 flip, 위128px/아래40px입니다.
- 실제 Camera→RenderTexture PNG **24장**. GIF·확대 진단판은 이 캡처에서 파생됐으며 **Editor GameView UI 캡처가 아닙니다.** GIF 프레임 시간은 실측을10ms 단위 반올림한 참고이며 엔진 JSON이 원 근거입니다.
- C# compile 오류0·NullReference0·MissingReference0·완료 마커 확인. 기존 **Editor SearchDatabase 초기화 예외1 OPEN**이므로 Console 전체 무오류로 표현하지 않습니다.

| 비교 자료 | 링크 |
|---|---|
| 실제12FPS 설정 모션 | [양방향128px/40px GIF](Evidence/FoxV7ForelegQA20261022/Fox_ActualUnityMotion.gif) |
| v5↔v7 Frame03 동일 배율 | [실제 RT4배](Evidence/FoxV7ForelegQA20261022/ActualUnity_Frame03_v5_v7_Focus4x.png) |
| 02→03→04 | [원배율128px·40px](Evidence/FoxV7ForelegQA20261022/Sequence_02_03_04.png) · [v7 실제3배](Evidence/FoxV7ForelegQA20261022/ActualUnity_02_03_04_Focus3x.png) |
| 05→00→01 | [루프 연결](Evidence/FoxV7ForelegQA20261022/Loop_05_00_01.png) |
| 실제 프레임/시간 | [UnityQA 원기록](Evidence/FoxV7ForelegQA20261022/UnityQA.json) · [요약](Evidence/FoxV7ForelegQA20261022/UnitySummary.json) |

## 미술 판정 — PASS 권고 / 사용자 최종 승인 대기

**기술 PASS와 별도로 실제 v5↔v7 및02→03→04 화면을 보고 판단했습니다.** v5의 접힌 앞다리에 떠 있던 흰 쐐기형 조각과 분리 발 인상은 제거됐습니다. v7은 가슴 아래에서 오른쪽/회수쪽 앞다리·발목·발끝까지 실루엣이 이어져 별도 조각처럼 보이던 문제가 개선됐습니다.

요청한128px/40px·좌우 방향·12FPS 설정의 연속 자료에서 **발 방향의 뚜렷한 급반전, 새 관절 단절·분리 실루엣, 해당 다리만 따로 뜨는 새 결함은 관찰하지 못했습니다.** 02→03→04가 이웃 자세와 연결되고, 정상5프레임을 보존하여 머리/귀/몸통/꼬리의 기존 성과도 유지됩니다. 05→00은 기존 프레임 그대로이며 새로 튀는 루프 문제를 뚜렷하게 관찰하지 않았습니다.

**확대 시 앞다리 상단의 밝은 털 띠는 남습니다.** 이는 실제 비교 자료에 그대로 보이며, 현재 표시 크기에서는 기존의 떠 있는 삼각 쐐기/분리 조각과는 다르게 읽혔습니다. 이 세부 표현의 최종 수용 여부는 대표의 미술 판단으로 남깁니다. 결함이 전혀 없거나 해부학/본게임 접지가 완벽하다는 보장은 아닙니다.

따라서 **FOX_V7_ART_PASS_RECOMMENDED / USER_FINAL_MOTION_APPROVAL_PENDING / GAME_NOT_APPLIED**입니다. 이 검수는 제자리 달리기 모션과 국소 복원 비교이며 본게임 이동속도에 대한 접지·발 미끄러짐 전체 검증이나 전투/시설/Save 통합 승인이 아닙니다. **대표 명시 최종 승인 전 제품 적용 금지**를 유지합니다.

## 원본 보호·Git

원본 Unity 관련 **4,402파일 SHA 동일**, 사용자 dirty/untracked **862파일 SHA 동일**, 시작 시 보존한 **승인 Village v10 후보9 PNG SHA 모두 동일**, v5 기준PNG6도 그대로입니다. 원본 `main` HEAD **`510c021b4159167e211be60ca90e1cb7328bafe9`**, status/index 바이트 동일. Wolf/Bear 자산은 원본 보호 검사에만 포함하며 기존154/전체 QA를 재실행하지 않았습니다.

제품쓰기·Save/UserData쓰기·원본 Editor/포커스 조작0, Bear/Wolf/Main22/전체전투·마을 승인9종 재검수0, Push/PR/main직접commit/merge0. 별도 브랜치 **`codex/qa-fox-v7-foreleg-20261022`**에 문서·증거만 로컬 커밋합니다. 원본 CURRENT_STATUS는 건드리지 않고 QA worktree 상태 문서만 갱신합니다. [보호 증거](Evidence/FoxV7ForelegQA20261022/OriginalProtection.json).

다음은 대표가 위 실제 모션과 Frame03 전후를 확인하고 Fox v7을 최종 승인하거나 잔여 밝은 털띠/발 표현을 표적으로 피드백하는 단계입니다. 후속 게임 적용과 GitHub 병합은 이번 작업에 포함하지 않습니다.

실제 QA 증거·보고 commit: `46c2ac837bc8a110c6e4f4607cabcfcb8d3b7705`. 상태 문서 후속 커밋은 최종 응답을 따릅니다.
