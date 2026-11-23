# LIMITLESS / Chapter 3 F5 빙각 수호체(Rime Sentinel) 1포즈 파일럿 — 2026-11-23

**정본:** [Issue #126](https://github.com/HappyPaul812/Project-Limitless/issues/126), [북부 7종 전투 밸런스](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/08_몬스터/Chapter3_북부몬스터7종_전투밸런스_20261118.md), [10/14 북부 몬스터 상세기획](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/08_몬스터/Chapter3_설빙의폭군_온기의화로_북부몬스터_상세기획_20261014.md)  
**담당 브랜치:** `docs/art-new`  
**현재 상태:** `MONSTER_EXISTENCE_APPROVED / KEYPOSE_ART_CANDIDATE_DELIVERED / USER_KEYPOSE_ART_REVIEW_PENDING / OFFLINE_FILE_QA_PASS / BOSS_SAME_SCALE_COMPARISON_PENDING / UNITY_NOT_APPLIED`

## 1. 발주와 승인 구분
- 대표의 "빙각 수호체 승인"은 **F5 1회 등장 전열 엘리트(중간 보스)의 기획상 존재 승인**. 이번 신규 KeyPose 디자인 자체의 미술 승인과는 다름.
- 고정 기획: F5 단회 엘리트/전열, Lv21·HP820·공격40·민첩9·EXP125·탈렌트32. 얼음 주먹110% → 팔 들어올리기(피해0·대상예고) → 균열 강타180% → 반복. 동상·동결괴사 신규 부여 없음. 도망 가능, 반복 리스폰 없음. '마력차단 결정' 단회100% 기획, 인게임 Item/Drop/Monster/Spawn 구현은 담당 외, 미수행.
- M4 얼음갑충(갑각/낮은 곤충), M5 눈보라 정령(비인간형 회오리) 및 마지막 보스 설빙의 폭군(야수형)과는 다른 **두 발·두 팔의 얼음 암석 인공 수호체**. 큰 주먹과 들어올릴 수 있는 전방 팔 실루엣을 우선. 화로/5개 길 상징/동결괴사 VFX 표현을 새로 만들지 않음.

## 2. 실제 납품: 게임 후보 KeyPose 한 장
- **`Candidate/RimeSentinel_M6_KeyPose_512_RGBA_CANDIDATE.png`**: 투명 RGBA 512×512 **1포즈만**, 320,670 bytes. SHA256 **`c34ea53a95aa6a1fdba4f5e355715fbc25797d6d51e4d544cb9e4dab52f7073c`**. 가시 알파 bbox `(31,65)-(481,490)` (반열림), 4변 최외곽 alpha0.
- 원본: 이번 대화에서 생성된 `빙결_거대_골렘_수호자.png` (1254×1254 RGBA), SHA256 **`fb031eec459d864995fc31b30f5c17569906bebe9bae896a26b2ce385048463b`**. 이전 생성된 `서리_파수꾼_얼음_골렘.png`는 **별도 디자인 시안**으로 혼동하지 않으며 이번 정식 신규 후보로 선택하지 않음.
- 선택한 최신 시안의 본체 영역을 추출하여 저알파 고립 잡음을 정리하고, **원비율 유지 premultiplied-alpha 다운스케일**, 512 RGBA 캔버스 하단 기준 정렬. 이는 새 후보 파일 생성이며 승인된 기존 공식 스프라이트 PNG를 변경/복제한 작업이 아님.
- 디자인: 청백색 각진 얼음 갑주, 짙은 암석 분절·무거운 주먹, 양발 지지, 절제한 금빛 내부 균열, 청색 눈. 전열 타깃 인식과 예고 팔 모션의 출발 자세로 의도. 다만 40px 실축소에서 가시 밀도가 높고 최종 보스급 과장 가능성은 **사용자 시각 판정 대상**. 후속 표적 수정 또는 승인을 받기 전 애니메이션 개발 금지.

## 3. 실제 비교자료와 QA 한계
- `Preview/RimeSentinel_Actual_128_RGBA.png` (128²), `Preview/RimeSentinel_Actual_40_RGBA.png` (40²): **최종 후보 PNG 512에서 직접 리샘플링한 실제 RGBA 파일**, 새 그림 생성 아님.
- `Preview/RimeSentinel_512_128_40_ActualReadability_OFFLINE.png`: 원본 512·실제 128·40px 축소 시야 확인. 일부 큰 화면 미리보기 확대는 픽셀 nearest로 표시. 체크무늬 배경은 보드에만 있으며 원본 PNG는 완전 투명.
- `Preview/RimeSentinel_vs_M4_M5_Actual512Canvas_OFFLINE.png`: 기존 승인 M4 Ice Beetle 및 M5 Blizzard Wisp의 실제 512 RGBA PNG를 읽기 전용 비교한 자료. **같은 512px 원본 캔버스에 놓아 표시**했으나 Unity 내 동일 PPU·world scale·Sorting/일반·엘리트 상대크기와는 다름.
- **공식 설빙의 폭군 또는 Ch2 중간 보스 실물 원화가 현재 작업 컨테이너에 없어** 그것들과의 공식 동배율 직접 시각 비교는 `NOT_VERIFIED`. 추정 합성으로 실제 비교 PASS 주장 금지. 차후 Codex가 공식 원본 실제 참조를 가져와 40px 선 판독을 추가 점검.
- 실제 512 PNG 디코드 RGBA, 원형 비율, alpha 바깥0, 128/40 PNG, ZIP CRC, Manifest/SUMS 자체참조 없는 해시 전부 확인. **OFFLINE 아트 QA에 한정**. Unity import/PPU/Pivot/Animator/Collider/전투 가시 크기 검증 `NOT_RUN`.

## 4. 파일 무결성과 납품
- ZIP **`LIMITLESS_Issue126_RimeSentinel_KeyPose_20261123.zip`**, **1,158,884 bytes / 9 entries**, SHA256 **`f2098a5e1affc13e3708eebf5d30f5c8e4fd9bdcfb0f72d41059e9178d34a46e`**.
- CRC `testzip() is None` **PASS**, Manifest payload **7/7 크기+SHA** PASS, SHA256SUMS payload7+Manifest1 **8/8 PASS**, 자기참조·순환 해시 참조 없음.
- `Docs/SourceDeclaration_And_QA.md`, `QA/art_specs.json`, `Manifest.json`, `SHA256SUMS.txt` 동봉.
- ChatGPT Library `/LIMITLESS/Issue126_RimeSentinel_KeyPose_20261123/`에 ZIP, 신규 512 PNG, 비교판2 **4파일 업로드 성공**. 대화에서 실제 ZIP/PNG 다운로드도 제공. Windows `F:\Downloads` 자동 복사와 GitHub 원격 이미지 바이너리 업로드 없음.

## 5. 승인 게이트와 보호
**대표 미술 승인 필요: YES (이번 1포즈만).** F5 엘리트 기획 승인은 인정하되 **KeyPose USER_APPROVED 표시 금지**. 후속 Idle/Attack/Hit/Defeat, MonsterDefinition/Spawn/Loot/Save/게임 수치 변경, Unity 적용/격리 QA 실행 모두 미수행. 특히 Codex [#122](https://github.com/HappyPaul812/Project-Limitless/issues/122) 별도 진행 작업과 충돌하지 않음.
기존 F1/F2/F3 #123 후보 및 승인 M1~M5/BattleBG/최종보스 등 아트 원본 변경0, Unity 6000.6.5f1/#85 HOLD/Save/UserData/Discord 보호. 메인 직접 commit·merge·원격 Push 없음. `docs/art-new`에는 **보고 문서 및 인계만** 기록.
