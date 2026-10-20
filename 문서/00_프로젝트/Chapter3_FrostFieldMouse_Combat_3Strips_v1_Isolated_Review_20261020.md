# Chapter3 서리들쥐 A 전투 3Strip v1 격리 검수

2026-10-20 — **종합: 수정 권고 / 사용자 미술 승인 보류 권고.** 실제 재생은 정상이지만 승인 Idle과 연결되는 외형·크기가 충분히 일치하지 않습니다. 아트 보정·정식 몬스터 등록은 하지 않았습니다.

| 항목 | 판정 | 근거 |
|---|---|---|
| ZIP·PNG 기술 형식 | PASS | CRC·실제 SHA·SHA목록 일치, 3×2048×512 RGBA, 가로4셀512 |
| Attack 모션 | FAIL | 할퀴기는 식별됨. Attack00이 승인 Idle00과 다르고 작아짐. 최대 전방 타격은01로 계약상 타격02보다 빠름 |
| Hit 모션 | FAIL | 움찔→낮아짐→회복은 읽힘. Hit03이 승인 Idle00과 다르고 작아 Idle 복귀 때 확대 점프 |
| Defeat 모션 | HOLD | 쓰러짐·03고정 자체는 PASS. 시작 체격 및 전 모션의 색상 정합 보완 후 재확인 |
| 승인 Idle 색상·스타일 일관성 | FAIL | 갈색/짙은 털 비중 증가와 작아진 준비·복귀 자세가 겹쳐 전환 시 연속성이 깨짐 |
| 실제 Unity 재생·상태 전환 | PASS | **36 PASS / 0 FAIL**, 실제 Import·공용 재생기·복귀·03고정·좌우반전 |

## 정본·범위

[Issue30 최신 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-6286950076), [기획 검수 게이트](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/Chapter3_FrostFieldMouse_Combat_3Strips_v1_Delivery_VisualTechnical_QA_Gate_20261020.md)(blob `caeff85a6251bad1d4544306565b67df015938fd`), [아트 납품 보고](https://github.com/HappyPaul812/Project-Limitless/blob/docs/art-new/문서/00_프로젝트/Chapter3_FrostFieldMouse_Combat_3Strips_v1_ArtDelivery_QA_20261020.md)(blob `0d813b44c655a63cac8c8a53b7c25f482e2fa73a`) 및 확정 계약 `ad9e4b46`을 대조했습니다. AGENTS·PROJECT_CONTEXT·CURRENT_STATUS·CODEX_WORKFLOW·팀 공통/Codex 규칙·인계를 확인했습니다.

브랜치 `codex/chapter3-mouse-combat-v1-review-20261020`, 시작 `1317807190b0bcfd9227870b4764b0fb2a495b80`. 관리형 worktree의 제품 Unity 파일 변경 없이 문서·검수 근거만 저장했습니다. 기존 Idle14PASS 재사용, 전체 회귀 검사 재실행0입니다.

## ZIP·PNG — 형식 PASS

`F:\Downloads\Limitless_Chapter3_FrostFieldMouse_Combat_3Strips_v1.zip`

| 파일 | SHA256 |
|---|---|
| ZIP | `24ef8dddf76fb5843e8e7d3fac05942f1450c9f39cf366b2422a0ab82cba6741` |
| FrostFieldMouse_A_Attack_4Frames.png | `08ed9ffb3d2807b0e1a3b27450f8f3bf1ff3e56f5f33e8b6607b2983db4e402e` |
| FrostFieldMouse_A_Hit_4Frames.png | `37c51117ae18f2fed2bda5a8cef6af3099812b5e0979e1368634172937568f1b` |
| FrostFieldMouse_A_Defeat_4Frames.png | `bda2809d177d03088e85389ecea23815b92962d61c36cc68967cd0015c96dfcc` |

8,688,417bytes / 28파일. 전 파일 CRC/testzip PASS, SHA256SUMS 27항목 일치. PNG3·개별 QA프레임12·비교판·APNG/GIF·Manifest.md·Offline_QA.json·SHA256SUMS가 있습니다. 별도 README/Manifest.json은 없으며 Manifest.md가 제작·Import·한계를 설명합니다. 최신 납품 게이트와 실제 파일명은 일치하나, 최초 계약의 PNG 말미 `_v1` 및 ZIP명과 다릅니다. 이는 미술 결함과 분리해 기록합니다.

**형식 PASS는 계약상 동일해야 하는 끝점 셀과 미술까지 PASS라는 뜻이 아닙니다.**

## 셀·알파·접지

- 전12셀512×512, 각 모션 내 프레임 RGBA 해시가 모두 달라 복제 정지화면이 아닙니다. 셀 최외곽 알파0, 셀 침범·꼬리/귀/발톱의 경계 잘림 없음.
- alpha>16의 6px 이하 작은 성분은0~4개/셀이나 최대alpha23/255인 매우 옅은 외곽 성분입니다. 128px에서 독립한 불투명 잡픽셀·사각 테두리로 보이지 않아 이것만으로 FAIL 처리하지 않았습니다. 좌표는 DetailAudit.json에 기록했습니다.
- 임의 알파 bbox 하단은 전12셀y491, alpha>16 기준489~490입니다. 납품 ground_y490은 좌표 규약을 포함한1px 외곽 차이가 있고112px 표시에서 약0.22px입니다. 큰 수직 접지 드리프트·배경 매트는 없습니다.
- **하단 일치만으로 발 위치·체격의 일치를 보장하지 않습니다.** 루트 Transform이 고정되어도 그림 안의 머리·등·발 위치가 바뀝니다. 지지 발의 완전 고정이나 미끄럼0을 bbox 수치만으로 PASS 처리하지 않았습니다.

## 승인 Idle과 동일 표시 배율 비교

프레임별 bbox 맞춤 확대 없이 셀512→112px(전투 Image136×112/preserveAspect/Scale1), 별도512→128px로 동일하게 표시했습니다.

| 프레임 | alpha>16 bbox | 112px 셀에서 가시 폭×높이 |
|---|---|---|
| 승인 Idle00 | (22,124)–(490,490) | 102.4×80.1px |
| Attack00 | (63,189)–(448,489) | 84.2×65.6px |
| Attack03 | (63,203)–(449,489) | 84.4×62.6px |
| Hit03 | (66,197)–(445,489) | 82.9×63.9px |
| Defeat00 | (54,188)–(457,489) | 88.2×65.8px |
| Defeat03 | (47,312)–(464,489) | 91.2×38.7px |

Attack00과 Hit03은 계약상 승인 Idle00의 RGBA 복사 셀이나 실제로는 둘 다 불일치합니다. 같은 준비/복귀 자세 비교에서 폭 약18~19%, 높이 약18~20%가 줄었습니다. 실제 GameView에서도 새 모션으로 작아졌다가 Idle로 커지는 전환이 보입니다. Defeat 최종 높이 감소는 쓰러짐이므로 결함으로 세지 않고, 시작00의 체격 연결을 확인 대상으로 삼았습니다.

승인 Idle에도 갈색 그림자가 있습니다. 새3종이 완전히 갈색 단색으로 바뀐 것은 아닙니다. 다만 새 얼굴·등·몸통에서 차가운 밝은 털과 짙은 털의 분포가 달라지고 축소가 겹쳐, 전환 시 같은 개체의 연속 동작으로 읽히기 어렵습니다. alpha>128 중 R−B>20/R>G 픽셀 비중은 Idle00 약49.2%, 새 모션 약53.2~60.9%입니다. 이 값은 털만 분리한 측정이 아니며 포즈·피부 면적에도 좌우되므로 보조 자료로만 사용했습니다. 판정 주근거는 동일 크기 비교판과 실제 전환입니다.

128px에서도 긴 꼬리·귀·앞니·발톱·야생성은 남아 마스코트로 전면 회귀한 것으로 보지는 않습니다. 다만 작아진 Attack/Hit의 얼굴과 발톱은 Idle보다 정보가 뭉칩니다. Attack02는 앞발과 얼굴이 겹쳐01의 전방 타격보다 공격 순간이 약하게 읽힙니다.

## 정확한 수정 대상 — 제안만, 아트 수정0

| PNG·프레임 | 수정 권고 |
|---|---|
| Attack00 | 승인 Idle00 셀을 RGBA 그대로 재사용. 축소·색 변경 금지 |
| Attack01·02·03 | 01준비→02전방 앞발 타격→03회수로 연결. 현재01이 최대 전방 타격이고02가 반동이라 계약상 명목0.25초의 타격02와 어긋남. 머리/몸통/발/꼬리의 원화 배율을 승인A에 맞추고03→Idle의 확대 점프 제거 |
| Hit00·01·02 | 피격 구분은 유지하되 승인A와 같은 머리/몸통 배율과 냉회백색 털·음영 분포로 연결. 세운 몸에서 급격히 작은 몸으로 바뀌는 인상 완화 |
| Hit03 | 승인 Idle00 셀을 RGBA 그대로 재사용. 현재의 작은 별도 복귀 그림 교체 |
| Defeat00 | Idle에서 진입할 때 머리/몸통 체격 및 냉회백 털·음영 일치 |
| Defeat01·02·03 | 쓰러짐 실루엣과03유지는 보존하고 같은 개체의 머리/몸통/꼬리·털색으로 연결. 최종 몸을 투명 소멸시키지 않음 |

일괄 확대만 하면 꼬리·발톱이 셀에 걸릴 수 있으므로 승인 Idle 재작업 대신 신규3Strip의 해당 셀만 아트 담당이 보완하는 방식을 권고합니다. 아트 담당에게 외부 메시지나 제작 시작 지시는 보내지 않았습니다.

## 실제 격리 Unity — 36 PASS / 0 FAIL

기존 숨김 Batch Client `Temp/Field05Review20261019/Client` QA 폴더에 새PNG3 및 승인Idle의 동일 바이트 복사본을 사용했습니다. Editor의 ISpriteEditorDataProvider 지원 권한을 확인한 뒤 Multiple 분할/PPU512/개별Pivot(256,0)/Point/Clamp/무압축 Import를 수행했습니다.

메모리 전용 Definition의 Explicit 배열에 연결하고 기존 MonsterSpriteSheetAnimation을 그대로 재생했습니다. 정식 MonsterDefinition Asset·Scene·Animator·Quest·전투 코드 변경0입니다.

- 실제 GameView1280×720. 전투UI와 같은 Image 크기, 128px 보조 칸, X반전 칸을 둔 **QA Canvas** 비교입니다. 실제 Battle Scene 정식 등록 결과가 아닙니다.
- 각00→01→02→03을 관측했습니다. Attack/Hit의 호출부터 Idle 복귀 관측은 약0.493/0.496초, 공통 설정8FPS입니다. 최초 Update와 Editor 일정 때문에 관측 간격이 정확히125ms가 되지는 않습니다. 실측 프레임 TSV와 RuntimeTiming.json을 제공합니다.
- Defeat03 도달 후1.25초 이상 유지·Idle 복귀 없음, 동일 프레임 좌우반전, 루트 위치/Scale 불변, QA Collider 추가 없음, Missing Sprite 없음 확인.
- 실제 GameView16장 및 모션별 **캡처 프레임 재구성 APNG** 저장. APNG는125ms 간격으로 재구성한 미리보기이며 연속 실시간 녹화가 아닙니다. 실측 시각은 TSV를 기준으로 합니다.
- 첫 검수 하네스에서 Object 이름 충돌 컴파일 오류가 있어 UnityEngine.Object로 명시하고 재실행했습니다. 제품 코드 오류와 분리했습니다. 최종 실행 신규 제품 오류0. 기존 UnityEditor.Search 및 SpeedTree shadergraph 진단은 원문 로그에 보존했습니다.
- 실제 BattleActionPresenter 피해 이벤트 동기·승리 전환·연속Hit→Attack은 이번3시퀀스 검수 범위 밖입니다. 소스 계약 대비 타격 포즈 차이를 지적한 것이며 실전 동기까지 PASS한 것은 아닙니다.

## 보호·다음 단계

원본 Assets/_Project·UserData·Settings·Packages3,546파일 SHA와 원본Git HEAD/status 전후 동일. 승인1포즈·Idle ZIP/PNG 불변. 원본 Editor Bootstrap/PlayOFF/비포커스 유지. 검수 브랜치 제품Unity 변경0, Push/main 병합0입니다.

**현v1 미술 승인은 보류하고, 신규3Strip 보완본 수령 후 변경 셀·Idle 전환만 재검수하는 것을 권고합니다.** 사용자 색감·비율 승인과 본게임 등록 승인은 별도로 기록해야 합니다.

[움직이는 비교·128px·GameView](Evidence/MouseCombatV1_20261020/index.html) · [끝점 비교판](Evidence/MouseCombatV1_20261020/ContractEndpoints.png) · [파일 검증](Evidence/MouseCombatV1_20261020/Validation.json) · [Unity 검사](Evidence/MouseCombatV1_20261020/Runtime/checks.txt) · [원본 보호](Evidence/MouseCombatV1_20261020/Protection.json)
