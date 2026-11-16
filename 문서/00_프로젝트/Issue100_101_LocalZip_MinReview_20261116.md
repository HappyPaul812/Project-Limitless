> 2026-11-16 EOD 선별 인계: 출처 `codex/art100-101-min-review-20261116`, Git blob `90e6c849e24f9ced0aedf4d7f10808daa93715f0`. 이하 보고의 승인/미적용/PENDING 표시는 작성 당시 상태입니다. 최신 결재는 EOD 보고 우선: Stoat KeyPose·Idle4·Attack4v2·Hit4 미술 승인/Unity미적용, NPC02/17·10/07 로컬적용, Issue85 Import/GUID HOLD. NPC 손 한정 지시는 기획 해석 오류였으며 최신 정정대로 인계합니다. 기존 제품·미완료 자산을 새로 적용하지 않습니다.

# Issue #100·#101 로컬 아트 ZIP 최소 검수 — 2026-11-16

**OFFLINE_FILE_TECH_PASS / ART_REVIEW_PENDING / UNITY_NOT_RUN / GAME_NOT_APPLIED**

## 정본 대조와 무결성

[Issue100 최신 납품](https://github.com/HappyPaul812/Project-Limitless/issues/100#issuecomment-6906336530) 및 [정식 보고](https://github.com/HappyPaul812/Project-Limitless/blob/docs/art-new/문서/00_프로젝트/Chapter3_SnowStoat_M2_Idle4_ArtPilot_20261116.md), [Issue101 최신 납품](https://github.com/HappyPaul812/Project-Limitless/issues/101#issuecomment-6909823447) 및 [정식 보고](https://github.com/HappyPaul812/Project-Limitless/blob/docs/art-renewal/문서/00_프로젝트/Renewal_20261116_Issue101_OGA10_OGA07_OriginalNPC2_ArtPilot.md)를 조회했습니다.

| 로컬 ZIP (F:/Downloads/) | 바이트 / entries | ZIP SHA256 | CRC / Manifest / SHA256SUMS |
|---|---|---|---|
| Limitless_SnowStoat_Idle4_REDELIVERY.zip |904015 /12|fa850041d0ed88d30a9e23a7b60e4345f22e2c12b4d01875042fb755970b734d|PASS /10/10 /11/11|
| Limitless_NPC_101.zip |1188626 /13|0ec80e1fa546f1ce9cde560bea34f07b3359a0678565d72daec3f711bc232ca5|PASS /11/11 /12/12|

로컬 파일명은 납품명과 다르지만 **ZIP 전체 SHA256과 크기가 정식 보고와 동일**합니다. 내부 목록 중복/상위경로 없음, Manifest 모든 참조의 크기·SHA 일치, SHA256SUMS가 자기 자신을 제외한 전체 파일을 포함합니다. Issue100 Manifest 안의 zip_crc="pending"은 납품시 정적 문구이며 실제 testzip 결과는 PASS입니다. 원본 문서는 수정하지 않았습니다.

## 설원족제비 Idle4

- Sheet **2048×512 RGBA /512²×4셀**, 개별4 PNG와 셀 RGBA 픽셀 일치, 서로 다른4프레임, 셀 외곽 비투명픽셀0.
- Sheet SHA256 `ee6951b0333ba6586b4009ccf51cfd3a397d69f839552fe88a709f9c34bf0633` 일치.
- Frame00 파일 SHA256 `fd25ef50528a5f630e44382e3fc40a3df607a267176f758706962a2cbc41e778` = GitHub에 기록된 승인 원화 SHA. 승인 원화와 파일 바이트 동일성이 확인되어 셀00 픽셀 동일성도 확인했습니다.
- Frame01/02/03의00대비 변경픽셀41001/39943/33451, Manifest 수치 일치. 발접지 y440~470 RGBA 변경0픽셀, alpha>8 하단 y455 고정. 발 미끄러짐/수직이동 없음.
- 4프레임을 기존 검토판으로 육안 비교: 몸-꼬리 분리/절단 없음. alpha>8 꼬리 좌측 x<180은 모든 프레임에서 몸통 주 연결영역에 포함. 순차 00→01→02→03→00 꼬리 실루엣 최대 최근접 변위 약2.24/4.47/3.61/1.41px(512 기준), 끝→처음 급격한 위치점프 증거 없음. 이는 형태 연속성 검사이며 움직임의 미술적 자연스러움 최종 승인을 대신하지 않습니다.
- 기존 GIF는4프레임, 실제 지연 각120ms, 총480ms(약8.33FPS). 보고서의8FPS/0.5초는 GIF10ms 단위 반올림에 따른 근사치입니다. 게임 Animator 검증 아님.
- **기술 PASS. 승인된 것은 기존1포즈이며 Idle4 신규 미술 승인은 별도로 필요합니다.** 호흡/꼬리 리듬과 실제 게임 표시 크기는 이번 오프라인 최소 검수의 승인 범위 밖입니다.

## OGA10·OGA07

- 원본 각512×512 RGBA, alpha 최소0/최대255, 네 변 모든 픽셀 alpha0. 불투명 배경/캔버스 접촉 잘림 없음.
- OGA10 SHA256 `7a53cbb71544b69e996a06c7b576c9498d0565cc701a9123a6ad9485cd1d18a1`, OGA07 `940aa98590333b1423ed5f49101c9cf5a146a0079c8e354fc33e0ce943dff798`: 정본과 일치.
- 128/40 프리뷰 각4개는 정확한 크기이며 원본의 LANCZOS 축소와 RGBA 픽셀 일치. 512 캔버스 기준 축소이며 가시 인물 높이는40px 캔버스 안 약32px입니다. Unity 월드 크기 검증과 다릅니다.
- 기존 납품512 비교판과128/40 비교판 **그대로 재사용**. 비교판 승인6종의 완전불투명 픽셀은 실제 로컬 승인 OGA02/03/06/09/16/17 원본과 모두 일치함을 확인했습니다. 새 검토판 생성/PNG 재인코딩 없음.
- 육안 의견: 두 신규 후보는 기존6종과 따뜻한 색·짙은 외곽선·작은 전신 비율을 공유합니다. 다만 기존은 단순한 닫힌 눈/큰 색면/굵은 선이 주를 이루고, 신규는 큰 열린 눈·머리카락/옷감 질감·다단계 명암이 더 세밀합니다. **화풍 완전 일치로 판정하지 않습니다.**
-128px: OGA10 초록 조끼/붉은 스카프/환영 손짓, OGA07 머릿수건/생활복/천 소품이 구별됩니다.40px: 색과 실루엣은 구분되나 OGA10 세 사람 표식/매달린 소품과 OGA07 천 무늬/파우치 세부는 읽기 어렵습니다. 작은 역할 표식의 명료함은 승인 판단 항목입니다.
- **파일 기술 PASS / ART_STYLE_REVIEW_REQUIRED / 두 원화 미술 승인 필요.** 게임 적용·PPU/Pivot·GameView QA는 실행하지 않았습니다.

## 증거와 보호

`Temp/Art100101Review20261116/audit.json`, `visual-evidence.json`, 원본 ZIP에서 바이트 그대로 추출한 기존 비교판/프레임. ZIP 원본과 PNG 자체 수정0. 검사 코드/증거는 Temp에만 보관합니다.

Unity 실행/Editor 명령/프로젝트 복제/게임 적용0. 기존 승인 아트/Save/Discord 읽기 외 쓰기0(승인6종 비교용 이미지 읽기만 수행, Save/Discord 접근0). 원본 main 사용자 상태 바이트 일치 확인. 별도 `codex/art100-101-min-review-20261116` 문서 sparse worktree에 보고와 CODEX_LATEST만 로컬 기록, main 반영/원격 Push/Merge 없음.
