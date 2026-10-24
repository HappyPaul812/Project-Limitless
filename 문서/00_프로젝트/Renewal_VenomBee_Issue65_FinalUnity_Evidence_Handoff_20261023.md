# LIMITLESS / 리뉴얼 아트 — 독침벌 Issue #63 / Codex #65 최신 실제 비교판 검수 인계 (2026-10-23)

## 결론과 증거 단계

**판정:** `TECH_PASS_SINGLE_POSE / ART_HOLD_REPLACEMENT / USER_ART_REVIEW_PENDING / MOTION_NOT_STARTED / RIGHTS_VERSION_CONFLICT_OPEN / NOT_PRODUCTION_APPLIED`.

이번 작업은 **Codex의 최신 #65 결과를 정본에 반영하는 문서 인계**다. 새 벌 PNG·Idle/Attack 키포즈·전체 20프레임을 **제작하지 않았고**, 기존 v1 ZIP, 게임 원본, 사용자 Save, 승인된 자산, main을 변경하지 않았다.

**직접 확인한 근거:** GitHub [Issue #63 Codex 최종 인계](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6358735741), [Issue #65 Codex 최신 MinimalDelta 보고](https://github.com/HappyPaul812/Project-Limitless/issues/65#issuecomment-6358720003) 및 [#65 최초 비교판 상세 보고](https://github.com/HappyPaul812/Project-Limitless/issues/65#issuecomment-6358407741). **GitHub 텍스트 및 기재 SHA/경로를 확인했으며 Codex 로컬 Windows ZIP 이미지 바이너리를 이 세션에서 직접 열거나 해시 계산한 것은 아니다.** 수치/검증은 **Codex가 직접 보고한 결과**이다. `LOCAL_COMMITTED / REMOTE_NOT_PUSHED / REMOTE_IMAGE_UNAVAILABLE` 유지.

## A. 실제 Unity 비교판 인계 — 새 테스트가 아닌 #64 증거 재사용

- 최신 바울 대표 로컬 리뷰 패키지: `F:\Downloads\Limitless_MinimalDelta65_Final_Unity_20261023_17db915d.zip` — Codex 보고 **47,639,471 bytes / 44 entries / CRC PASS**, SHA256 **`17db915d60dab9dcf0de521d0ac4e37e8cf9a7c0a8876461dd2aa6e944c333fd`**. 압축 해제 → `index.html` → **독침벌**.
- **재사용 실제판**: `A_REUSED_A64_Bee_Ch2_Original_UnityBoard_UNAPPROVED.png` (원본 `A64_Bee_Ch2_Original_UnityBoard_UNAPPROVED.png`) / SHA256 **`0c228ce8d9f679b6b6f88564f038e7eb2371234ce98d32d73cd3983d46b77777`**. #64 실제 **Unity 검토 Canvas**를 바이트 동일하게 재사용했다고 Codex가 보고함. 기존 Pilot Bee Idle/Attack / 신규 v1 좌향1포즈 / Chapter2 공식 몬스터 **11~15 다섯 종**을 40px·128px 공통 Image 상자 및 전투 표시 크기 계약으로 비교한 자료.
- **중요:** 이 보드는 정식 제품 Battle에 신규 독침벌을 적용한 화면이 아니다. 로컬 ZIP·PNG는 GitHub에 공개/업로드되지 않았으며 로컬 Codex 커밋은 원격에 Push하지 않았다. **리뉴얼 담당이 실제 이미지 내용을 독립 시각 확인했다고 주장하지 않는다.**
- Codex 관찰 요약: **128px에서는 갑각·날개·침을 식별**할 수 있으나 **40px에서는 앞다리·침·날개맥의 개별 식별력이 약함**. 512 Master 경계 alpha0, 축소 40/128px 미리보기의 경계 접촉·다색 배경(alpha) 검토 필요. 이는 사용자 최종 미술 합격 판정이 아니다.
- 최신 #65의 신규 **23 PASS**는 **오르나 인물높이/휠체어 등의 Main23 Delta QA**이며 **독침벌에 대한 새 PASS가 아니다**. 기존 148/136/41/15 및 직전33/8 전수재실행0. 독침벌 새 실행·모션 제작·QA PASS 추가0.

## B. 기존 외부 원본 및 라이선스

- 현행 `venom_bee` Idle/Attack Sprite GUID: `24000000000000000000000000000004` / `24000000000000000000000000000005`; Codex #64 실제 참조 경로는 `Resources/MonsterValidation/PilotBee/pilot_bee_idle.png`, `Resources/MonsterValidation/PilotBee/pilot_bee_attack.png`. Codex는 현행 PNG와 `ThirdParty/2DPIXX/PilotBee/Original/` 원본 PNG가 바이트 동일하다고 검증. 이전 README의 '비연결'은 현재 연결 상태 정본이 아니다.
- 제작자: **Jana Ochse / 2D!PIXX**; [OpenGameArt 배포 페이지](https://opengameart.org/content/pilotbeegamecharacter)는 **CC BY 3.0**, 패키지 `Original/LICENSE.TXT`는 **CC BY 4.0**. 버전 충돌 **미해결**. 원본·LICENSE·제작자 출처·기존 크레딧 후보 보호. 양자 중 하나를 확정했다고 기록하지 않음.
- 기존 [출처·권리·모션 조건부 계약](Renewal_VenomBee_Codex64_65_Rights_Comparison_And_MotionContract_20261023.md) 내용은 **그대로 보존**한다.

## C. 모션 제작 게이트 — 아직 발효 금지

- 기존 게임 계약: 우향(`sourceFacesRight=1`), `Idle 10프레임`(238×215 셀), `Attack 10프레임`(315×253 셀), **10FPS**, `visualScale=0.85`. 신규 원화는 **좌향 512×512 RGBA 단일 포즈**라 즉시 대체 **불가 / ART_HOLD**.
- 바울 대표가 **위 실제 비교판을 열어 보고 원화 디자인을 명시적으로 승인**하기 전에는 새로운 Idle/Attack **키포즈도 제작하지 않고 20프레임 양산도 금지**. 승인이 발생하더라도 기존 조건부 계약에 따라 소수 독립 키포즈 → 격리 방향·40/128px·알파/스케일 Delta QA → 사용자 중간 승인 → 20프레임 시트 제작·10FPS 모션 검증의 별도 발주를 받아야 한다.
- 검수 대상(향후): 40px 핵심 실루엣(독침·다리·날개), 좌우 방향 및 공격 진행/복귀, alpha 접촉·경계, PPU/Pivot/Frame Center, 공식 Ch2 동배율 명암·질감 통일, Idle09→00 및 Attack 연속성. **이번 단계 신규 PNG0·모션0·Unity 적용0**.

## D. 변경·보호·다음 판단

- 기존 [v1 보고](Renewal_VenomBee_OriginalKeyArt_Pilot_20261023.md) 및 배포 ZIP `Limitless_Chapter1_VenomBee_OriginalKeyArt_Pilot_v1_20261023.zip` (**2,291,579 bytes / SHA256 `2e65662d68c77cfb90d7e39d9d40c2cd2816ee33882debe8a8e93ace782bd478`**) **수정·재생성0**.
- 현 MonsterDefinition·Sprite GUID/.meta·독 DoT·드롭·AI·Field01 Spawn·전투 처리, 공식 Ch2 몬스터/보스, Wolf·Bear·Fox, NPC OGA03/09/20 시안, 숲거미 HOLD, 기존54행 감사, Save/UserData/원본 Unity **변경0**. Codex #65도 로컬 원본 28,180개 파일 SHA 변경0이라 보고.
- 바울 대표에게 남은 결정: **#65 실제 Unity 비교판 보고 v1 디자인 승인 / 표적 보완 / 반려 중 선택**. 어느 선택도 아직 확정됐다고 기록하지 않는다.
- Codex #65 최신 로컬 브랜치: `codex/art-minimaldelta-final-review-20261023`, 검수 `bc357232b16f45cab541ee496694049950d7fdf0`, 보고 `263283823186ab99abddce45b97b2b3be7e31a82` (모두 **LOCAL_COMMITTED / REMOTE_NOT_PUSHED**). 이 문서 저장은 **`docs/art-renewal`만**, `main` 병합/Push0.
