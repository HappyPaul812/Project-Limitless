# Issue #81 후속 — 자연형 그루터기 + Spider/Bee 실제 델타 검수 / 2026-11-03

**LOCAL_ART_DELTA_REVIEW_DONE / USER_VISUAL_REVIEW_PENDING / NOT_UNITY_APPLIED / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**

## 판정과 단일 비교판

`F:/study/codex/Project-Limitless/Temp/Issue81ArtDeltaFollowup20261103/Review/index.html`

모든 이미지·재생 프레임을 포함한 단일 독립 OFFLINE HTML입니다. 외부 서버 없이 열리며 **Unity/GameView가 아닙니다**. 밝음/어둠/실제 Field03 바닥 전환·512/128/40·앞다리4배 확대·이전/신규 순서 재생/정지/앞뒤이동·파일명/SHA를 제공합니다.

HTML SHA256: `83b32736108df60d66e401da7270e4ae8556ee5ffc27d74d95515e2fa0d5a6ef`.

|대상|이번 판정|실제 개선·남은 이유|
|---|---|---|
|F2 NaturalBroken v4|**KEEP_CANDIDATE**|원판형 스툴/곧은 지주가 불규칙 부러진 가지·수피 단면으로 바뀌어 기존 HOLD 사유 감소. 40px 자연형 윤곽 유지|
|Organic Spider v4|**REVISE / ALPHA_FINAL_HOLD**|RGB 보존·alpha491감소는 사실. 밑띠 일부는 지웠으나 하단 회백색 띠와 ROI 끝의 직선 절단 경계 잔존. ALPHA_FIX_OK_CANDIDATE 아님|
|Bee 새5 PNG|**REVISE / MOTION_REVISE_CONTINUE / FULL_COMBAT_READY_NO**|5/5 새 SHA·새 RGBA 픽셀 확인. 그러나 우향 머리/좌향 독침 불일치·관절/회수 급전환 미해결. MOTION_REVISE_OK_CANDIDATE 아님|

대표 최종 미술 승인 대상으로는 **새 자연형 그루터기1개**를 권고합니다. 기존 Sign/Fence FULLREDRAW KEEP_CANDIDATE 상태는 그대로 유지합니다. KEEP은 최종 승인이나 게임 적용 승인이 아닙니다. Collider/휠체어 통행/PPU/Pivot/실전 배율 모두 **NOT_VERIFIED**입니다.

## ZIP 실물 무결성

- `Limitless_F2_Stump_Natural_Issue81_20261103.zip`: **820,391 bytes / 6 entries**, SHA256 `890013d4885319e1dc8a5a7f4ac2771c485b7ed2664617882cf535e6b48b70b5`. CRC PASS·중복경로0·zip slip 경로 이상0. Manifest **3/3 SHA/bytes PASS**, SUMS **5/5 PASS**.
- `LIMITLESS_ISSUE81_ART_DELTA.zip`: **2,444,044 bytes / 35 entries**, SHA256 `d3ac1c2137a150e0b883e8fe3b236e71575915758620949ee8c84d8ceb1c1955`. CRC PASS·중복경로0·zip slip 경로 이상0. Manifest **33/33 SHA/bytes PASS**, SUMS **34/34 PASS**.

두 ZIP의 SHA/용량/항목 수는 각각 docs/art-new·docs/art-renewal 최신 납품 보고와 실제 일치합니다. 이번 두 ZIP에는 Manifest/SUMS가 모두 있습니다. 이전 개별 Bee/Spider ZIP의 미동봉 상태와 혼동하지 않습니다.

F2 Manifest의 `source_image_sha256` 큰 생성 원화는 ZIP에 없으므로 **그 큰 원화 자체는 NOT_SUPPLIED/NOT_VERIFIED**입니다. created_files에 기록된 PNG3개의 SHA/bytes와 SUMS5개를 검사한 범위만 PASS입니다. 참조 이전 소스·KEEP 후보 SHA는 실제 기존 로컬 파일과 대조했습니다.

이번 새 PNG/GIF는 모두 decode 가능. 새 게임 후보7 PNG(F2 320×384, Spider512², Bee Idle238×215×2/Attack315×253×3)는 RGBA·alpha0~255이고 캔버스 경계의 가시 alpha 접촉0입니다. 128/40 보조 PNG도 decode/크기/alpha를 기록했습니다. GIF는 P 모드 불투명 프리뷰로서 투명 게임 Sprite가 아닙니다. RGB 비교판을 RGBA 후보와 구분했습니다. SHA가 같은 Old5_KEEP/Spider v3는 이전 QA를 재사용하고 차이 비교·표시를 위한 로드만 수행했습니다. 기존20·전체승인아트·게임QA를 반복하지 않았습니다.

## F2 — 실제 새 그림과 HOLD 해소 여부

- NaturalBroken v4는 기존 v3와 RGBA **46,676pixel**(RGB46,382/alpha46,213), 기존 FULLREDRAW와 RGBA **47,315pixel**(RGB46,996/alpha29,489)이 다릅니다. 같은320×384 캔버스 안에서 넓은 그림 영역이 바뀌어 단순 재인코딩·재포장이 아닙니다. 국소 수정이라고 주장하지 않으며 독립 자연형 시안입니다.
- 작은 원판/대칭 테두리/곧은 받침대가 없어지고, 비대칭으로 부러진 나무·갈라진 수피·굽은 잔가지가 생겼습니다. 기존 '가구 같다'는 HOLD 사유가 눈에 띄게 줄어 **KEEP_CANDIDATE**입니다.
- 청백색 눈·갈색 수피·낮은 주변 식생은 기존 F2 표지판/울타리 대안과 함께 봤을 때 큰 색감/재질 이질감이 없습니다. 이는 OFFLINE 외형 의견이며 실제 Scene 조명·Sorting·크기 적합성 판정은 아닙니다.
- 너비128px와40px로 원종횡비·투명 여백을 보존했습니다(320×384→128×154/40×48). 40px에서 나무결·작은 잔가지는 합쳐지지만 큰 그루터기+옆 부러진 가지의 자연형 실루엣은 읽힙니다. 원판형 가구로 되돌아 보이지 않아 핵심 개선은 유지됩니다. 화면비/게임 PPU 검증과 다릅니다.
- 기존 Sign FULLREDRAW `864e964e...`, Fence FULLREDRAW `49c2969b...`의 원본·KEEP_CANDIDATE는 변경0. 기존 두 그루터기 HOLD 자료도 그대로 보존했습니다.

## Spider — RGB 보존은 PASS, 알파 마감은 계속 HOLD

- 이번 v3 reference는 기존 `52d2e5fb...` 원본과 바이트 일치. v4는 새 SHA `c9eedca2...`로 실제 다른 파일입니다.
- RGB262,144픽셀 변경0, alpha **491pixel 감소/증가0**, 실제 변경 bbox `[41,306,96,354)`입니다. 담당의 ROI x35..95/y304..353 안에 한정됩니다. 알파만 수정한 사실과 기승인 유기형 디자인/RGB 보존을 확인했습니다.
- 밝음/어둠/실제 Field03 지면 PNG 반복배경과 원본512·동일영역 최근접4배를 비교했습니다. 중간 밑띠는 더 줄었지만 **아래쪽 회백색 띠가 남고 수정 ROI 끝에서 수평/수직 절단 단차가 보입니다**. 다리 하단 털 경계도 이 지점에서 갑자기 끊겨 보이므로 자연스러운 최종 알파로 판단하지 않습니다.
- 512/128/40에서 기존 몸통·다리 전체 실루엣은 유지되며 40px에서는 문제 경계가 작아져 덜 보입니다. 축소에서 안 보인다는 이유로512 경계 잔상을 해결 처리하지 않았습니다. **REVISE / ALPHA_FINAL_HOLD**, `ALPHA_FIX_OK_CANDIDATE`로 올리지 않습니다.
- 디자인 방향 자체는 재심사하지 않았습니다. 다음 보완은 RGB/몸체를 지키면서 잔여 밑띠와 잘린 ROI 경계만 자연스럽게 마감하는 범위입니다. 새32프레임·Unity교체0. 메카닉 Spider v3 Chapter4 ARCHIVE_ONLY 원본 SHA 보존.

## Bee — 새 수정 확인과 모션 완성 판정을 분리

- Old5_KEEP5개는 이전 문제 포즈와 SHA5/5 일치. New5의 실제 크기 PNG5개는 모두 이전 SHA·디코딩 RGBA 픽셀이 다릅니다. 비교판만 추가하거나 같은 이미지를 PNG 재인코딩한 납품이 아닙니다. 실제 그림 세부·알파·자세/배치 수정은 확인하지만 생성/수작업 과정 자체는 파일만으로 독립 입증하지 않습니다.

|포즈|RGB 변경|alpha 변경|RGBA union 변경|
|---|---:|---:|---:|
|Idle_Folded|17,609|18,342|18,383|
|Idle_Spread|19,581|20,124|20,144|
|Attack_Prepare|29,727|30,682|30,730|
|Attack_Sting|23,579|24,523|24,559|
|Attack_Recover|31,487|32,388|32,416|

- 접힘/펼침의 날개 각도와 세부, Prepare의 앞다리 자세, Recover의 몸/다리 외형이 바뀌었습니다. 그러나 **머리는 우향, Sting의 뒤쪽 독침은 좌향**으로 길게 뻗는 충돌이 여전히 명확합니다. 40px에서도 복부/침 변화는 읽히지만 실제 타격 방향은 혼동됩니다.
- 날개 뿌리 회전의 중간 단계·다리 관절 대응·복부 전진/회수 연결은2/3키포즈만으로 자연스러운 연속동작을 입증하지 못합니다. Sting→Recover는 형태/다리 배치가 갑자기 바뀌고, 같은 실루엣 축을 따라 부드럽게 회수하는 완성 애니메이션으로 보기 어렵습니다.
- 동봉 GIF 새 SHA2개 확인. Idle **2프레임/100+100ms**, Attack **3프레임/100+100+100ms**, 각각 **10FPS·loop0**입니다. 팔레트/배경 차이로 raw RGBA 거리만 믿지 않고 같은 배경 합성 RGB 거리와 원본 프레임으로 **Folded→Spread / Prepare→Sting→Recover** 순서를 확인했습니다. 자체 PNG 순서 제어와 원본 GIF를 비교판에서 별도로 표시했습니다.
- 판정: **NEW_ART_DELTA_CONFIRMED**이지만 **REVISE / MOTION_REVISE_CONTINUE / FULL_COMBAT_READY_NO**. `MOTION_REVISE_OK_CANDIDATE`나 전투 교체 완료가 아닙니다. 이번 ZIP에 전체20프레임·새 원화 마스터 시트는 없고 기존20은 이전 자료 그대로입니다. 전체20 재양산을 수행하거나 승인하지 않았습니다.
- PilotBee CC BY3.0/4.0 출처 충돌은 **RIGHTS_HOLD** 계속 유지합니다.

## 최종 후보 정확한 파일명·SHA

- `F2/Candidate/F2_SnowStumpPerch_NaturalBroken_v4_CANDIDATE_320x384_RGBA.png`: `22f2f5816b3fd2ffe45234ec807bb3d81a83f87440eb8eb1f338e0dff33404a8`
- `Renewal/Spider/ForestSpider_Organic_v4_ALPHA_EDGE_CANDIDATE_512.png`: `c9eedca2634baaae41b53af26f5c26e18b1abe3be7fe15fe35a717d0a7e8378d`
- `Renewal/Bee/New5/Idle_Folded_238x215.png`: `d17114176c13c2911cd533a0e7a363a1f4ae2ca110b540bcec5d63a2aa941605`
- `Renewal/Bee/New5/Idle_Spread_238x215.png`: `d35f4583c0fa119396a40a6637f48b36b677fe00327640c655d86bde3dd47820`
- `Renewal/Bee/New5/Attack_Prepare_315x253.png`: `50c8047125e86bced17073fead14adaaa35545dea19beb9306f1c267481ea7e8`
- `Renewal/Bee/New5/Attack_Sting_315x253.png`: `17bd20da372f1453ec903228a0394148bf1a0000c3d6d714d0b2404c31863624`
- `Renewal/Bee/New5/Attack_Recover_315x253.png`: `9002a76ffa2b35b8aa34c35178e3cae4967992878e2df2b72db3e96e068d18be`
- `Renewal/Bee/Attack_NEW_KEYPOSES_ONLY_10fps.gif`: `7977e3bdb13727b17b78ac04cecebe92aa2ba2d167c774540c51ab3c5e1f718c`
- `Renewal/Bee/Idle_NEW_KEYPOSES_ONLY_10fps.gif`: `891be7530c5f05b1d04de3b8a702e99f8fda290bfae7d023ecdf17baffad143b`

## 근거·보존·인계

- 로컬 `Temp/Issue81ArtDeltaFollowup20261103/Audit.json`: ZIP/Manifest/SUMS/PNG spec·실제 차이 수치.
- `InputSHA256.json`, `ProtectionAfter.json`: 입력/이전 소스 SHA와 원본 Git 보호.
- `GIFOrder.json`: 실제 GIF 순서/100ms 및 PNG 대응.
- `Review/BrowserQA.json`: 임베디드 이미지 decode·외부 이미지 의존성0·4순서2/2/3/3·앞뒤 버튼·4배경·가로폭 **18PASS/FAIL0**. 초기17PASS/1FAIL은 이전 도구의 SHA구역ID `hash`→이번`sha` 변경을 시험 선택자에 반영하지 않은 오류이며 수정 후 재검증. 제품/이미지 오류가 아닙니다.
- 검사 도구 첫 numpy 실행은 로컬 `inspect.py` 파일명이 Python 표준라이브러리를 가린 오류로 실패했습니다. 해당 시험 스크립트명을 바꾼 뒤 실행했고 소스 아트는 수정하지 않았습니다. 초기 실패도 보존했습니다.
- 원본 PNG·비교 확대를 직접 검토했습니다. HTML은 새 별도 경로이며 이전3ZIP 비교판도 덮어쓰지 않았습니다. 실 Field03 바닥을 OFFLINE 반복 합성하며 Scene/조명 재현이라고 주장하지 않습니다.

입력48파일 SHA 변화0. 원본main `ab47b931fec92976170f9f50aabde1822d78bb90`/status344항목 지문 불변. Main24 완료 HEAD `fd869f60c9fde438a3dd97885cb6f447efc4454b` 보존,81/14PASS·저장85/72/72·Main23 재실행0. Unity실행/버전/Scene·Prefab·BuildSettings·Packages·Quest·개인Save/UserData열람·복사·해시/Discord#66/foreground 변경0.

브랜치 `codex/issue81-art-delta-review-20261103`, 출발점 `6ea756d490dabf4b2c1a3fde1539bdb0d60b38ae`. 이번 delta는 **본 보고서와 CODEX_LATEST Markdown2개만** 로컬 커밋합니다. HTML/PNG/GIF/JSON/도구는 Temp에만 보존하고 원격 Push하지 않습니다. 게임 적용·main 직접commit/Push/merge0.

대표 최종 판단은 새 자연형 그루터기1개의 미술 승인 여부입니다. Spider 밑띠·ROI경계와 Bee 공격축/관절/회수는 최소 보완 후 다시 비교할 항목이며, 승인 전 양산/게임교체로 확대하지 않습니다.

## GitHub 보고 영수증

검수/인계 commit `cae0d8efebbfbf12bec51e15dd7ea8746f03bacc` 기준 실제 GitHub 보고 [#81](https://github.com/HappyPaul812/Project-Limitless/issues/81#issuecomment-6591984789) · [#75](https://github.com/HappyPaul812/Project-Limitless/issues/75#issuecomment-6591993553) · [#60](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6591994340) · [#63](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6591995213) 등록 성공. 원격에는 댓글만 게시했고 파일Push/게임적용0. 본 후속 commit은 문서 영수증만 기록합니다.
