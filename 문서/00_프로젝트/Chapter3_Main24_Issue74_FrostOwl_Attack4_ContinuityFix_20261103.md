# LIMITLESS / 메인 신규 아트 / Frost Owl v3 Attack4 디자인 연속성 보완 — 2026-11-03

**담당 브랜치:** `docs/art-new`  
**작업 근거:** [Issue #74 Codex 통합 실물 QA](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6594778265), [#74 기획 최종 표적 지시](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6594916733).  
**판정:** `ATTACK4_REVISED_CANDIDATE / OFFLINE_FILE_QA_PASS / V3_STATIC_AND_IDLE8_PROTECTED / USER_FINAL_MOTION_ART_REVIEW_PENDING / UNITY_ANIMATION_NOT_VERIFIED / NOT_UNITY_APPLIED`.

## 1. 최종 납품 실물

- **ZIP:** `Limitless_FrostOwl_Attack4_Continuity_20261103.zip`
- **크기:** **4,623,041 bytes**, **22 entries**, ZIP CRC `testzip PASS`; `SHA256SUMS.txt` **21/21 대조 PASS**.
- **ZIP SHA256:** `dd50da39530380047c8d4a6dd728d624ff828fc2dde089abc708be0558bc6856`.
- ChatGPT 파일 라이브러리 **`/LIMITLESS/Main24_Issue74_Owl_Continuity_20261103/`에 ZIP과 한 장 전후 비교 PNG 각각 업로드 성공**. 기존 다운로드 링크 오류 대응. 사용자의 Windows `F:\Downloads` 직접복사/로컬 해시 확인은 미실시. GitHub에 PNG/GIF/ZIP 바이너리 업로드0. GitHub에는 보고서/최신 인계 문서만 커밋.
- **수정된 Attack PNG 네 장만 `Candidate/Attack`**에 수록(기존 Idle8 원본 PNG는 다시 수록·재생산하지 않음). 모두 **512×512 독립 투명 RGBA**:

| 변경 후보 | 정확한 SHA256 |
|---|---|
| `Candidate/Attack/FrostOwl_v3_Attack_00_512_RGBA_CANDIDATE.png` | `b6029c30bf76e8b3f32843184f812c7ca6254f07b79214eca4069cd42a8fe547` |
| `Candidate/Attack/FrostOwl_v3_Attack_01_512_RGBA_CANDIDATE.png` | `a98aab1c6a14caae1ed255882e90ec7fc3f1bdfa28693409d307a1cc1fed7142` |
| `Candidate/Attack/FrostOwl_v3_Attack_02_512_RGBA_CANDIDATE.png` | `a7273d0d3c0b20d2a005b90f158906aa9760ef551dbba11aa3e80c5e692b9851` |
| `Candidate/Attack/FrostOwl_v3_Attack_03_512_RGBA_CANDIDATE.png` | `81a31cf37cb834799ab5c9428da612ab13a5a496025805685daefed7b5d3c467` |

- **시트:** `Candidate/Sheet/FrostOwl_v3_Attack4_8FPS_2048x512_RGBA_CANDIDATE.png` **2048×512 / 4×1**; 실제 각 PNG와 시트 셀 RGBA 배열 **4/4 exact 동일**. SHA `c47dd4b64e659cc5a02d170491fac6d36adfb8f0618cc043178f317da1ffecfb`.
- **8FPS 실제 GIF:** `Preview/FrostOwl_v3_Attack4_8FPS_CANDIDATE.gif`, 4프레임 순서00→01→02→03, frame duration **120/130/120/130ms**, 4프레임500ms, 평균8FPS. SHA `f780cc56beb53f3eb6a0628f295dda9d26ff9d645b07e0cbdc4a5ff9e4458d87`. Preview GIF는 배경 합성; 투명 게임용은 4 PNG.
- **비교판:** `Preview/Attack4_BeforeAfter_IdleContinuity_128_40_OFFLINE.png`는 전기 Attack4 4장 vs 수정 Attack4 4장 + 각 **128px / 40px 실 PNG LANCZOS 축소** 한 장. `Preview/Attack03_to_Idle00_Continuity_OFFLINE.png`는 Attack02→Attack03→승인 Idle00 복귀 비교. 모두 OFFLINE (Unity GameView 아님).
- **독립 미승인 VFX:** `VFX_UNAPPROVED_HOLD/FrostOwl_IceArc_Detached_VFX_HOLD_512_RGBA.png` (512×512 RGBA), SHA `46215c1cf7fd3fa3f94af92f4dd457436cf70711ca95b1bed86dc527f49d761c`. Attack01 본체에는 **추가 생성 냉기 파티클/궤적을 포함하지 않으며**, 이 미승인 아트는 Spawn/Skill VFX 또는 게임효과로 적용하지 않는다. 승인된 정지 v3 원화에 원래 포함된 작은 냉기 디테일은 그 원화의 일부로 보호한다.

## 2. 이번 실제 디자인 보완 범위·정확한 한계

- 기존 2026-11-03 이전 `Limitless_FrostOwl_Attack4_Review_20261103.zip` SHA `5ba6ecffad2e3ccf5a07c0fda00041073f1db6938a439fdeea170eae9abf250b` 원본 바이트 보호. 이전 Attack4 대비 실제 RGBA 변화 픽셀 수: **00 127,837 / 01 134,605 / 02 101,254 / 03 119,068**. 네 프레임 모두 변경파일로 납품하며 기존 정상 **Idle8 프레임에 대한 변경은0**.
- **개선된 준비→발동→후속→복귀 포즈의 날개 폭 대비를 유지**하되, 00~02는 새 모션 그림에서 v3 승인 원화 **얼굴 중앙·머리깃 영역의 실제 RGB/명암을 원본 참조 마스크로 복원**하여 게임 내 동일 인물 인식을 높이는 후보를 제작했다. 외부 날개·몸통·꼬리의 새 포즈는 이종 그림을 합성한 **미술 검토 후보**로서 v3 모든 픽셀이 동일하다는 허위 주장은 금지. Codex #74의 디자인 연속성 이슈가 **최종 해결되었다는 판정은 바울 대표 시각 승인 전 HOLD**.
- **Attack03은 승인 v3 정지 원화이자 기존 Idle00 파일과 PNG 바이트 자체가 동일**, SHA `81a31cf37cb834799ab5c9428da612ab13a5a496025805685daefed7b5d3c467`. 이로써 `Attack03→Idle00` 복귀 시 프레임과 프레임 사이 픽셀 불연속은 **정확히0**. Attack02→03의 자연스러움은 별도 사용자 미술 검토 필요.
- 원화 v3 및 Idle8 원본 파일은 읽기 전용 참조: `Limitless_FrostOwl_v3_TargetedFrames_20261029.zip` 내부 Idle8 SHA8개를 `Manifest.json`과 `QA/DeltaQA.json`에 기록(Idle00 exact SHA 검증). 실제 Idle8 변형·재작성0. 기존 Clip-safe Idle02 기술 PASS를 그대로 보호.
- 00~03 PNG들의 `alpha>8` 상·하·좌·우 접촉 **모두0**, 512² RGBA decode PASS; 시트 픽셀 일치4/4, 8FPS GIF4/4·총500ms, `SHA256SUMS` 21/21 및 ZIP CRC PASS. 따라서 **OFFLINE 파일 기술 검사 PASS**까지만 확인.
- `PPU512 / Pivot(0.5,0.5) / Scale 1.0 / FPS8`은 `Manifest.json`에 기록한 **Unity Import 목표 계약**; 실제 Unity `.meta` 새로 만들거나 전투에 적용하지 않음. Animator Transition/프레임 정렬·512·128·40 실제 GameView·16:9/4:3/21:9·휠체어/전투/상태이상 구현 기술 검증은 `NOT_RUN`.
- 냉기 VFX 분리 파일은 **미승인 이펙트 후보 보관만**. 새 몬스터 전투 Hit/Defeat/Spawn·SkillVFX/MonsterDefinition/Scene/Prefab/Save/UserData/BuildSettings·Quest 및 원본 Unity 변경0.
- Main24 #83 90PASS/23PASS, 기존 정적 v3/Idle 기술검수, 승인 F2 자연형 Stump 및 미승인 Sign/Fence KEEP_CANDIDATE, F1/Main23 환경43+승인지면9·공식 몬스터/펫·Chapter1·2 리뉴얼 Bee/Spider 보호, 기존 대규모 QA 재실행0. `main` 직접 commit·Push·Merge 금지.

## 3. 다음 승인 게이트

**바울 대표가 검토할 실제 항목:** (1) Attack00→01→02의 날갯짓 대비와 v3 얼굴·몸통·깃의 일관성, (2) Attack02→03 복귀, (3) 128/40 실축소에서 Idle과 공격 구분, (4) 분리한 VFX는 **이번 승인 범위 밖으로 HOLD**.

상태 `ATTACK_MOTION_ART_REVIEW_PENDING / DESIGN_CONTINUITY_USER_REVIEW_PENDING / VFX_UNAPPROVED_HOLD / UNITY_NOT_VERIFIED / NOT_UNITY_APPLIED`. 최종 모션 미술 승인은 사용자 직접 검토 후; 이후 Codex **격리 Unity 8FPS/PPU512/중앙Pivot/Clip·UI 최소 DeltaQA**만 실행, 정적·#83 검증 전수 반복 없이 사용자가 별도 본게임 적용 승인하기 전 정식 Sprite 교체/전투·main 병합 금지.
