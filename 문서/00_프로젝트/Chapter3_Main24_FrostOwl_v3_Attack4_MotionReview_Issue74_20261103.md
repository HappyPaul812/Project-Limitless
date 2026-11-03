# LIMITLESS / Main24 Frost Owl v3 / Issue #74 — Attack4 실동작 재작화 후보 납품 (2026-11-03)

**브랜치:** `docs/art-new`  
**관련:** [#74 최신 발주](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6593592716) · [#83 F2 격리 이동](https://github.com/HappyPaul812/Project-Limitless/issues/83).  
**현재 상태:** `ATTACK4_POSE_ART_CANDIDATE / OFFLINE_FILE_QA_PASS / STATIC_V3_APPROVED_AND_UNCHANGED / IDLE8_SOURCE_PROTECTED / USER_MOTION_ART_REVIEW_PENDING / UNITY_ANIMATION_NOT_VERIFIED / NOT_UNITY_APPLIED`.

## 제작·실물 전달

- 납품 ZIP **`Limitless_FrostOwl_Attack4_Review_20261103.zip`**, **3,153,329 bytes**, ZIP SHA256 **`5ba6ecffad2e3ccf5a07c0fda00041073f1db6938a439fdeea170eae9abf250b`**.
- 총 **18 entries**, ZIP CRC `testzip PASS`, 내부 `SHA256SUMS.txt` **17/17 PASS**.
- **실제 ChatGPT 라이브러리 등록 성공**: `/LIMITLESS/Main24_Issue74_Owl_Attack4_20261103/Limitless_FrostOwl_Attack4_Review_20261103.zip`. 사용자 Windows `F:\Downloads` 실물 복사 여부 확인 전. 원격 GitHub에는 문서만 커밋하며 PNG/GIF/ZIP 바이너리 업로드하지 않는다.
- 후보 **Attack PNG4장만**, 각각 512×512 RGBA 투명. 보존된 직전 `Limitless_FrostOwl_v3_TargetedFrames_20261029.zip` SHA **`d42d4e7e24607e759dfc8de04b807ec8e6a04fc117abbb2a4aa87b27be0b5c1d`** 비교 사용. 4개 새로운 포즈: Prepare(날개 모으고 몸 수축), Release(날개 큰 펼침과 냉기 동작), Follow-through(날개 내려 발동후), Recovery(복귀).
- 실제 후보 폴더 `Candidate/Attack/`:
  - `FrostOwl_v3_Attack_00_512_RGBA_CANDIDATE.png` SHA **`f6274c6c1bb343d9650d8b5537eabc84690c07bb90cf3af6b1d61846e2a592c6`**.
  - `FrostOwl_v3_Attack_01_512_RGBA_CANDIDATE.png` SHA **`1915cfd62c4592711d8ffc29786d463d5709df5fda787401c96f0e1d48ca7cd5`**.
  - `FrostOwl_v3_Attack_02_512_RGBA_CANDIDATE.png` SHA **`79e238611a387ade3f245d7eacacfadd16eaba63abd9fb789d9f5ac1be0ad2b7`**.
  - `FrostOwl_v3_Attack_03_512_RGBA_CANDIDATE.png` SHA **`187829c7237cda0bc85802069d3dd5d5bf1ad98266044143ae1756cace96005a`**.
- `Candidate/FrostOwl_v3_Attack4_8FPS_Sheet_2048x512_RGBA.png` **2048×512**, 4×1 동일 512 픽셀 셀의 재조합.
- `Preview/FrostOwl_Attack4_8FPS_REVIEW.gif` **4프레임**, 120/130/120/130ms, 한 주기500ms = 평균8FPS; GIF는 어두운 배경이 입혀진 미술검토용이지 Unity 투명 스프라이트 아님.
- `Preview/Attack4_BeforeAfter_OnePage_512_128_40_OFFLINE.png` **1800×1190 RGB**: 바로 앞 Attack4 원본과 신규 4자세를 같은 셀 배율로 비교하고 실 파일에서 파생한 128×128·40×40 축소를 함께 표시. `Preview/`에 각 프레임의 128px, 40px 투명 축소 PNG8장, `Manifest.json` 원본8Idle SHA·기존Attack4 SHA·현재PNG SHA와 import contract·치수/clip QA, `README_KR.md` 수록.

## 엄격한 기술·미술 판정

- **원본 보호**: 바울 대표가 승인한 정적 FrostOwl v3 / 기존 Idle00 SHA256 **`81a31cf37cb834799ab5c9428da612ab13a5a496025805685daefed7b5d3c467`** 동일. 기존 Idle8의 파일 바이트는 새 아카이브로 재구성하지 않고 **직전 파일 SHA8개만 참조**. Idle/정지 PNG/기존 Attack4 원본에 직접 수정0.
- **모션**: 새 Attack4는 단순한 기존 PNG 전체 위치 이동이 아닌 **새로 그린 서로 다른 날개·몸통·꼬리 포즈**를 1254×1254 투명 원화에서 512×512 규격의 독립 프레임으로 출력. PNG 규격 정합화를 위해 새 원화만 **480×480 출력 + 사방16px 여백**; 승인 v3 정지원화와 Idle8의 크기/캔버스/소스 바이트를 축소하거나 바꾸지 않았다.
- **셀 경계 검사**: 프레임 4개 모두 RGBA PNG decode / 512×512 PASS, `alpha > 8` 기준 상하좌우 접촉 **0**. 정확한 bbox/frame SHA와 편집 제한은 Manifest 참조. `PPU512 / Pivot(0.5,0.5) / Scale1.0`은 **Unity Import 계약 기록만**, 실제 `.meta` 생성·Scene/Runtime Unity 검증 실행0.
- **중요한 시각 HOLD**: 새 포즈는 기존 v3에 비해 **깃털 밀도·얼굴의 세부 질감·부가 냉기 궤적 표현이 다르므로**, v3 공식 고유 디자인 픽셀이 엄밀히 보존된 동일 스타일 애니메이션이라고 주장할 수 없다. 신규 이미지에 **통합 냉기 광선·입자**가 포함되므로 `VFX_SCOPE_ART_REVIEW`: 별도 Hit·Defeat·Spawn/Skill VFX 파일을 생성한 것은 아니나, 바울 대표가 **정지원화 연속성과 냉기 시각효과의 허용범위**를 확인해야 한다. `ANIM_ART_PASS` 미선언.
- **공식 Unity 배포/전투 적용 금지**: Codex #83은 별도 격리 북문↔F2 왕복 구현에 관한 건으로 Owl 첫 한기 전투와 분리. Scene/Prefab/MonsterDefinition/Skill VFX/Save/UserData/BuildSettings/Packages, 원본 승인 F2 Stump·KEEP_CANDIDATE Sign/Fence 등 변경0. `main` 직접 Commit·Push·Merge0.
- **다음**: 바울 대표의 동일 캐릭터 화풍·4포즈 Attack 대비·40px 축소판 **미술 승인/반려** → 승인 시에만 Codex 격리 Unity6000.6.5f1 8FPS/clip/PPU512 Pivot중앙/Scale1/전투 UI·40px 3비율 최소 DeltaQA → 별도 본편 적용 승인. 기존 통과한 Main24 81PASS 및 정적 Owl/Idle QA 전수 반복 금지.

**미술 승인 필요 여부:** **필요, 아직 대기**. `USER_MOTION_ART_REVIEW_PENDING / NOT_UNITY_APPLIED`.
