# Issue112 — M5 Blizzard Wisp 1포즈 오프라인 기술·가독성 검수

**ART_CANDIDATE / OFFLINE_FILE_QA_PASS / USER_ART_REVIEW_PENDING / UNITY_NOT_APPLIED**

## 최신 정본과 기존 승인 원화 확인

AGENTS·COMMON_RULES·PROJECT_CONTEXT·CURRENT_STATUS, 원격 docs/art-new SPRITE_ART_LATEST(a9128414…) 및 docs/planning PLANNING_LATEST와 Issue112 발주/납품 댓글6941786647을 확인했습니다. M5는 F4~F5 후열 일반 자연 정령이며, 이번 범위는 우향512RGBA 정지1포즈 검수뿐입니다. 기존 M4 5종 승인 및 #103 M4 등록계약 부재 HOLD 상태를 바꾸지 않습니다.

기존 승인 M5 원화는 Issue112·인계 및 현재 원본Assets 파일명 검색 범위에서 미발견, 현재 ZIP은 미승인 후보입니다. Library `/LIMITLESS/Issue112_BlizzardWisp_OnePose/`의 ZIP·PNG·비교판2 저장 성공은 아트 담당 납품 기록에서 확인했습니다. 이번 Codex 도구에는 Library 직접 조회/다운로드 기능이 없어 Library 원격 실물 재열람은 NOT_VERIFIED입니다. Windows 실제 전달 ZIP과 납품 SHA를 독립 대조했으며 기존 승인을 추정하지 않습니다. 전체 사용자PC·모든Git브랜치 전수조사 아님.

## 파일 기술 판정 — PASS

- 입력 `F:/Downloads/BlizzardWisp_Issue112_OnePose.zip`: **798,663bytes / 8파일**, SHA256 `53b21965bd0513a42bb04ebefd78572554a7e9803106e56f8573b36da7150ede`.
- CRC PASS, Manifest payload6/6 bytes+SHA PASS, SHA256SUMS payload6+Manifest1=7/7 PASS. 전체커버리지·자기/순환참조 없음·중복/탈출경로 없음.
- `Candidate/BlizzardWisp_M5_KeyPose_512_RGBA_CANDIDATE.png`: **512×512 투명 RGBA / 217,196bytes / 정지1셀**, SHA256 `0c72c3f8fc80c9e74bf648236a0ff1746ef344ef71f03b4c005c38db59915586`.
- alpha>0 bbox(36,8)-(475,464), 외곽alpha0, 완전투명171,041/262,144픽셀(약65.25%). 캔버스 배경은 실제 투명이며 제출 비교판 배경과 구분했습니다. 단일PNG의 정지1포즈, 별도모션 없음.
- 128×128 RGBA **25,188bytes**, SHA256 `9e7051286081decff07449a583781dd8e107c765b9d20bca6fdfc5df0ad12096`.
- 40×40 RGBA **3,697bytes**, SHA256 `7246ab80a1a54d0b9f50e385742036ab145b80041507e205a44abf4b205fdc79`.
- 두 축소본 모두 실제512원본 LANCZOS 축소와 전RGBA 픽셀 동일.
- 파일구성: PNG5(원화1+축소2+검토판2), SourceDeclaration1, Manifest1, SUMS1. 원본512/128/40 검토판356,445bytes, SHA9df2f6739426e8d3f1db3ee97605f195f78135fa0477a0a5d43695d4a610e622. 북부M2/M4/M5검토판198,114bytes, SHA800144b11ab4f0f594207580ddb6bc9363500696bcdcf14330e3458d340b3f8d.

## 실제 미술·가독성 의견

| 기준 | 판정/의견 |
|---|---|
| 비인간형 자연 정령 | PASS 의견. 은백색 눈/바람 띠와 어두운 청색 핵. 눈·코·입/팔다리·유령 얼굴로 읽히는 구조 없음 |
| 우향 | PASS 의견. 오른쪽으로 열린 비대칭 흐름/결정으로 방향감 있음. 생물의 얼굴 방향처럼 강하지 않은 추상적 우향이므로 최종 미술 수용은 별도 |
| 512/128 | PASS 의견. 띠의 겹침과 중심핵이 명확. 화면 배경이나 별도 대형VFX 없음 |
| 순백 설원/40px | PASS 의견. 직접 순백(#fff)·어두운(#23313e) 합성의 실제40px에서 어두운 윤곽·파란 핵 식별. 작은 분리 결정은40px에서 부수적인 점으로 축약되지만 주 실루엣을 잃지 않음 |
| 발광/이펙트 | HOLD급 결함 미발견. 핵의 파란 하이라이트는 강하지만 넓은 화면 발광/배경은 없음. 다만 국소 발광 강도는 사용자 미술 검토 사항 |
| M1~M4 화풍 | 대체로 호환 의견. M2/3/4의 청백색·명암 덩어리·짙은 윤곽과 이어짐. M1의 더 세밀한 털 표현과 차이는 있으나 추상 자연 정령의 재질 차이 범위로 보임. 통일성 최종승인 아님 |

납품 M2/M4/M5 비교판에 더해 현재 원본 M1 Idle 첫셀·M3 Owl Idle00, 승인 M2 Idle00 및 M4 KeyPose의 실물을 읽어 M1~M5 순백128/40·어두운40 비교판을 만들었습니다. M2SHAfd25ef50…/M4SHAb7ff0592…는 납품 출처선언의 승인 SHA와 일치합니다. M1/M3는 현재 승인 제품 파일을 읽기 전용 사용했습니다. 다섯 참조의 실제 경로·SHA는 reference-comparison.json에 보관했습니다. 같은캔버스 비교이며 실제 월드체급·PPU/런타임을 검증한 것이 아닙니다.

## 바울 대표 확인 자료

- 납품 512/128/40 실물: `Temp/Issue112WispReview20261117/Delivery/Preview/BlizzardWisp_M5_512_128_40_Review_OFFLINE.png`.
- 북부M1~M5 추가 비교: `Temp/Issue112WispReview20261117/M1_M5_White40_Comparison.png`.
- 최종 의견: **기술 PASS / 미술 KEEP 검토 권고 / USER_ART_REVIEW_PENDING**. HOLD 강제 사유는 발견되지 않았습니다. 우향의 추상성·핵 하이라이트 강도를 확인 후 1포즈 미술 승인 여부를 결정하시면 됩니다. 추가모션/게임적용은 이번 검수로 승인하지 않습니다.

## 보호와 기록

Unity 실행·Import·적용0, Spawn/스탯/전투/펫 등록0, 추가모션/이미지 생성0. 추가 비교판은 기존PNG를 읽어 축소·배경 합성만 했으며 승인 원화 바이트 변경0. PNG/meta/GUID·Scene/Prefab 변경0, Save/UserData/Discord/#85 HOLD·기존원본/미커밋상태 보호.

로컬 제품commit없음, 문서commit만. `codex/issue112-wisp-offline-20261117`의 본 보고서와 SPRITE_ART_LATEST에 docs/art-new 업무 검수로 기록합니다. 원격 docs/art-new/main에는 Push하지 않았습니다. 원본 root main HEAD52f84e643d5d860665eadfe1614a30c745209f00 유지. 실제 문서SHA·8356보호대조 결과는 Final.json/Issue112댓글 참조.

증거 `Temp/Issue112WispReview20261117/audit.json`, `reference-comparison.json`, `Final.json`. 원화·ZIP·보호목록 등 비공개 로컬 바이너리 업로드0. Library 직접열람·Unity 화면·실제맵설원 가독성은 미검증이며 오프라인 단색 배경 실물 검수와 혼동하지 않습니다.
