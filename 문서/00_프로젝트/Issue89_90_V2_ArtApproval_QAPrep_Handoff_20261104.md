# Issue #89·#90 v2 사용자 미술 승인 확정·QA 준비·제작 인계 / 2026-11-04

## 승인 근거와 단회 SHA 게이트
사용자 이번 지시와 최신 기획 승인 댓글 [#89](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6621363277), [#90](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6621363801), 기획 정본 `20261104_Issue89_90_ConditionalUserArtApproval.md`(docs/planning 72205da045070852320fadb4212154297615b84a)을 확인했습니다.

PowerShell Get-FileHash로 실제 F: 두 ZIP의 전체 SHA256을 각각 정확히 1회 계산했습니다. ZIP 열기·CRC·Manifest·내부 PNG 해시·이미지 재검토는 이번에 하지 않았습니다.

| Issue / 실제 경로 | 실측 SHA256 = 승인 정본 | 확정 상태 |
|---|---|---|
| #89 F:/Downloads/Limitless_Ch3_ColdVFX_Issue89_MinimalFollowup_20261104.zip | 9c07f5ff782c6e272796c4f003b816a39806ab35248c2353884325ac3c24a1eb | USER_ART_APPROVED_SHA_MATCH / UNITY_NOT_APPLIED |
| #90 F:/Downloads/LIMITLESS_SPIDER_ATTACK3_TARGETED_ISSUE90_v2.zip | 804f8ebf85dbe7f7301cbfdecc6e85adc4e7d8888e23c3ee132bfca95854499b | ATTACK3_KEYPOSE_USER_ART_APPROVED_SHA_MATCH / UNITY_NOT_APPLIED |

#89는 한기·동상·동결괴사 v2 정지 RGBA 3장, #90은 기존 Prepare00 KEEP 및 v2 Contact01·Recover02만 미술 승인입니다. 이전 미술 승인 대기 기록은 이 범위에서 해소됩니다. 실게임 QA/전체 모션/원본 적용 완료로 승격하지 않습니다.

## #89 격리 시각 Delta QA 준비서
상태 QA_PREPARATION_READY / UNITY_DELTA_QA_NOT_RUN. 이번 요청은 준비이므로 Unity 실행·새 프로젝트 생성·Import·Runtime 수정은 하지 않았습니다.

- 다음 실행 환경은 Unity **6000.6.5f1 (3ff58d469c8a)** 별도 격리 프로젝트입니다. 원본 Scene/Packages/BuildSettings/UserData를 공유·변경하지 않습니다. 개인 Save 대신 합성 캐릭터/상태 표시만 사용합니다.
- 승인 v2 정지 PNG 3장은 원본 그대로 격리 사본으로만 가져옵니다. 재작화·애니/Particle 양산0. PPU/Pivot/WorldScale은 미확정이며 테스트용 값과 공식 계약을 구분합니다. 판정 기준은 실제 화면 투영40/128px입니다.
- 최소 캡처 매트릭스는 960x720,1280x720,1680x720 × 밝은 F2 설원/어두운 전장 × 40/128px = **12개 화면 조건**입니다. 가능하면 한 화면에 세 상태를 같은 배율로 배치하여 반복을 줄입니다. UI/카메라 실제 렌더 PNG와 설정을 저장하고 OFFLINE 합성과 구분합니다.
- 각 조건에서 한기 바닥고리·연무가 설원에서 보이는지, 동상 결정 군집과 동결괴사 균열/쐐기의 형태가 구분되는지 확인합니다. 색에만 의존하지 않고 별도 상태명·아이콘·한기2턴·동결괴사1/3,2/3 및 위험 안내를 합성 데이터로 표시합니다. 한기/동상/동결괴사는 독립이며 지속3/3 상태를 만들지 않습니다.
- 실제 Battle HUD와 캐릭터를 포함해 얼굴/HP/MP·상태명·아이콘·턴/중첩·메뉴 차폐를 점검합니다. 합성 상태 표시를 실제 상태/피해/해제 로직 연결 PASS로 보고하지 않습니다. 세로 화면 점유·가독성 문제가 있으면 QA fixture 배치 문제와 승인 아트 문제를 분리해 보고합니다.
- 백그라운드 Batch/RenderTexture 캡처를 우선하고 원본 Editor/OS 포커스를 변경하지 않습니다. 실행 후 표적 compile/새 Runtime 오류만 확인하며 기존 #84/#85/#87/#88 전수QA·Stage1 63PASS 재실행은 제외합니다.
- 정식 상태·해제/화로 연결, 새 수치, 원본 제품 적용은 별도 게이트입니다. 실제 QA 결과가 나오기 전 UNITY_NOT_TESTED를 유지합니다.

## #90 리뉴얼 담당 제작 인계
수신: 리뉴얼 아트 담당, docs/art-renewal. 사용자 승인과 SHA 게이트가 해소되어 **미제작 중간5프레임만 새 후보로 제작 가능**합니다. 본 Issue 댓글을 인계 채널로 사용하며 다른 채팅/담당이 이미 수신·착수했다고 주장하지 않습니다.

- 동결 기준: 기존 Prepare00 + 승인 v2 Contact01·Recover02 3개 키포즈를 그대로 보존합니다. 승인 v5/Idle8/Walk8도 재작화하지 않습니다. 신규5장과 동결3장의 파일을 분리하고 Attack8 슬롯/순서를 납품 자료에 명시합니다. 기존 키포즈 번호00/01/02를 최종8프레임의 인접 슬롯으로 단정하지 않습니다.
- 아래는 아트 납품 보고에 기록된 보호용 개별 SHA이며 이번에 다시 계산하지 않았습니다: Prepare00 `439ef26e4528e9f57877b471f602f9cfaa2f7bcd2a86b2fce991ec2e17ae5bf2`, Contact01v2 `0d0e24c2fae579f2f944255fe3bd3841216299b14f439cc3612906269020f697`, Recover02v2 `0501a452467a07f34688abdfff5ea5ef49b0dfd93e1c99bdc368229986d44096`.
- 기존 우향/10FPS/PPU256/scale0.7 계약을 유지합니다. Prepare→Contact→Recover를 잇는 두부·복부 기준점, 같은 다리의 어깨→관절→발끝, 지지발의 연결을 우선합니다. 알파 무게중심 숫자만 억지로 맞추지 않습니다.
- 납품은 신규 중간5 PNG, 승인3키포즈 KEEP 근거, 전체8슬롯 순서·10FPS 미리보기,512/128/40 비교 및 관절/접지·미해결 항목, 실제 ZIP SHA/Manifest/CRC 기록입니다. 제작 완료 전 ATTACK8_5_NOT_PRODUCED를 유지합니다. 새5장의 최종 미술 승인과 실제 전체10FPS 연속성·타깃 접점·발 고정/미끄러짐·Unity Runtime 검증은 남아 있습니다.
- Shoot8 및 본편 적용은 HOLD. Bee v3 RIGHTS_HOLD, Owl/기존 공식아트/Save/UserData/원본 Unity 보호. docs/art-renewal 보고서·증분 인계·#90 납품 댓글로 이어갑니다. main 직접 커밋·Push·병합 금지.

## 로컬 기록과 보호
기존 격리 문서 브랜치 codex/issue89-90-local-zip-review-20261104에서 보고·CURRENT_STATUS·CODEX_LATEST만 갱신합니다. 직전 관련 commit fd70f8e8e7c009200f7e09c61d4a0d5be5ddd5bf. 이번 commit은 Issue 결과 댓글에 기록합니다. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 원본 main 634a91d3e4174c87761c0d377cfa5a695ca79e7f 유지, 사용자 미커밋/승인아트 수정0·Save 열람0·Unity 실행0. 버전파일은 6000.6.5f1 확인만 했습니다.
