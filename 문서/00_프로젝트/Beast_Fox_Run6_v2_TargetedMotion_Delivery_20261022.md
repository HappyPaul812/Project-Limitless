# LIMITLESS / Fox Run6 v2 — 모션 미술 보완 파일럿 실 PNG·ZIP 납품 (2026-10-22)

## 상황과 승인 게이트
- 바울 대표: 승인 Wolf Run6 v5 실제 적용 완료, 절대 수정 금지. Codex Bear/Fox v1 기술 PASS / 모션 미술 HOLD. 이 작업은 **Fox Run6 v2 단독 후보**.
- `Fox` 기존 v1/ThirdParty/Game/Save/UserData/Unity Scene/GUID/.meta/적용된 Wolf와 GitHub `main` 변경0. `docs/art-renewal` 문서만 작업.
- 정확 상태 **`V2_REAL_PNG6_ZIP_DELIVERED / OFFLINE_ASSET_TECH_PASS / CODEx_UNITY_V2_NOT_TESTED / USER_MOTION_ART_APPROVAL_PENDING / MAIN_UNCHANGED`**.

## 실제 작업 방식과 미술 소스
- 기존 v1은 작은 콘셉트 보드 포즈를 512px로 확대했기에 원본 세부 품질과 발·꼬리 연결이 부족. 이번 v2에는 **이번 턴 별도 새로 생성한 6개의 포즈가 각각 분리된 고해상도 원화 시트**(`Source/Fox_IndependentPose_ArtSheet.png`, 원천 파일 `여우_달리기_애니메이션_스프라이트_시트.png`, 1774×887)를 사용, 총 3×2 다른 포즈를 추출.
- 이전 v1의 단순 픽셀 이동/scale 변화 6개를 재사용한 것이 아니라 각각 다른 팔다리 포즈를 가진 신규 시트 기반. 연결 컴포넌트로 배경의 독립 프린지·잔여 조각을 정리하고 동일 512×384 RGBA/좌향 캔버스에 배치, 12FPS 후보 구성.
- **투명하고 추적 가능한 후보이지 전신을 각 512px 해상도로 신규 수작업 페인팅했다고 주장하지 않음.** 정식 별도 승인 Bear/Fox 단일 KeyArt를 GitHub main에서 찾지 못했고, 기존 ScratchIO 제3자 Run은 기능·동작 규격 참고만 하였으며 그 픽셀은 사용하지 않음.
- **집중 보완:** v1의 앞다리 구분 부족·꼬리 기준점/곡률 부조화. 새로운 각기 다른 6포즈에서 앞지 다리 위치/구름을 분리하고 길고 흰 꼬리 끝과 귀·머리·몸통 비율을 통일하는 방향. 05→00 바닥 실제 투명 bbox 최저점 325→326px.
- 여섯 포즈별 모션 의도: 00: 앞발 교대 착지; 01: 반대쪽 앞발 전진·후지 추진; 02: 공중 신장·전후지 분리; 03: 공중 이동·몸길이 유지; 04: 앞발 착지·뒷발 회복; 05: 후지 밀어내기·꼬리 반동·00 연결.
- 입력 시트의 물리적 frame cell은 대략 591×443 (원본 시트 1774×887), 따라서 **v1 포즈 약120px를 확대 재사용한 결과가 아님**. 512 캔버스 내 각 개체 실루엣은 437~447px에 정렬; 뼈 길이와 실제 Unity 렌더 등신/PPU는 Codex의 별도 시각 QA가 필요.

## 실제 납품 경로와 아카이브 SHA
- 파일명 `Limitless_Beast_Fox_Run6_v2.zip` — **4,166,008바이트 / SHA256 `22e780627a96a87b2d1ce06f746248b0b9158cc59c834b8c6d4b2c6f9661c6ea` / 총36개 엔트리**.
- 이 ChatGPT 대화의 실제 아카이브: `sandbox:/mnt/data/Limitless_Beast_Fox_Run6_v2.zip`. **GitHub에는 ZIP 바이너리 미업로드, 대표 F: 로컬 자동복사 없음.** `/mnt/data/Limitless_Beast_Fox_Run6_v2_ReleaseQA.json`는 ZIP 외부 최종 릴리스 해시 검증 JSON.
- `Assets/Poses/Fox_Run_00_v2.png`~`05_v2.png`: 6개의 각각 독립 투명 PNG 512×384 RGBA. `Assets/Fox_Run6_v2_Strip_3072x384.png`와 `Assets/Fox_Run6_v2_Atlas3x2_1536x768.png`: 픽셀 그대로 결합.
- `Preview/Fox_Run6_v2_12FPS_128px.gif`, `...12FPS_Transparent.apng`(6frames 500ms), `128px_ContactSheet.png`, `40px_ContactSheet.png`, 각 128/40 PNG, `Loop_05_to_00.png`, `Run_v1_vs_v2_128px.png`, 기존 v1 실제 GIF 참고본.
- `QA/Frame_Gait_And_Contact.csv`, `QA/Offline_Validation.json`, `Frame_Descriptions_KO.md`, `Source/Provenance.md`, `Manifest.json`, `SHA256SUMS.txt`, `README.md`.

## 실제 자체 검증 및 한계
- PNG 해독6/6, RGBA·512×384, 6프레임 해시 서로 다름, 사방 3px 프레임 경계 Alpha 누출0.
- 가로 Strip 6개 Rect·Atlas 6개 Rect는 각각 개별 frame과 RGBA 픽셀 동일6/6.
- GIF 6프레임 duration 80+90+80+90+80+80=500ms, APNG 투명 6프레임 83+83+84+83+83+84=500ms. 좌향 Run.
- ZIP 표준 `testzip()==None` CRC PASS, 엔트리36개 전부 외부 작업 파일 바이트 및 SHA256 일치, Manifest/ SHA256SUMS 생성. ZIP 최종 바이트의 실제 SHA를 생성 종료 후 기록했으며 원본을 재압축·덮어쓰기하지 않음.
- **OFFLINE_ASSET_TECH_PASS≠ART_PASS**. 05→00 실제 도약과 지면 접지, 털 윤곽·관절/귀·꼬리 형태, 128/40px 게임 HUD 판독, 실제 Unity GameView 크기·PPU는 Codex 변경분 격리 모션 QA와 바울 대표 최종 승인 필수. 제작자 자체 미술 100% 합격 주장 없음.
- Wolf v5 정식 펫/다른 야수/몬스터/Field09/Scene/Save, Git main 직접 commit·push·merge0. `Fox` 미술 최종 승인 전 공식 Unity 적용 금지.
