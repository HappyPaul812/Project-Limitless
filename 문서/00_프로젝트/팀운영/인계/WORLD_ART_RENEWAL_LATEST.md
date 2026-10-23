## 2026-10-23 / Issue #63 — VenomBee 오리지널 원화 1포즈 후보

- 보고서 `../../Renewal_VenomBee_OriginalKeyArt_Pilot_20261023.md` commit `3840193957f8f83e67f820bd4065ee4519e00c50`. 기존 MonsterDefinition Idle GUID ...04, Attack ...05, 10FPS·각10프레임 확인. GUID에 매핑된 물리 PNG/라이선스 미확정 `RIGHTS_SOURCE_PENDING`.
- 신규 512×512 RGBA 1포즈 + 128/40 미리보기, 원화 보드, Manifest/SHA/QA ZIP `Limitless_Chapter1_VenomBee_OriginalKeyArt_Pilot_v1_20261023.zip`, SHA256 `2e65662d68c77cfb90d7e39d9d40c2cd2816ee33882debe8a8e93ace782bd478`, CRC PASS. 자동 마스킹 가장자리·실 GameView 미검증. 실제 기존 게임 PNG와의 정밀 비교 불가. `USER_REVIEW_PENDING / NOT_UNITY_APPLIED`.
- **기존 54행 감사·OGA03/09/20 세 NPC 시안·Issue #60 숲거미 출처 HOLD** 이력 보존. 승인 Wolf/Bear/Fox 및 모든 공식 몬스터/보스, 독/드롭/Spawn/Unity/Save/Main 무변경. Idle10/Attack10 전체20 제작0.

# WORLD_ART_RENEWAL 최신 인계 — 2026-10-23

## 2026-10-23 / Issue #57 — Chapter1·2 비고유 자산 사용·보관 정적감사 및 Eldiran NPC 2종 시안 (최신)

- 기준 main 47ad2eecb40135b2ee28e4947d642ebdbf0bf68a; Issue #57 및 10/23 기획 계약. 승인 Wolf/Bear/Fox Run6, Ch2 몬스터·보스, 공식 초상화·환경 모두 보호·변경0.
- Eldiran NPC **12 StableID**는 StarterVillageHubInstaller에서 생성하고 VillageNpcAppearanceCatalog.Resources.Load<Sprite> 경로에 12/12 매핑; 실제 PNG meta GUID 12/12, PPU28. 게임 Unity 실제 GameView 렌더12/12는 아직 검증하지 않아 Codex 읽기전용 QA 필요.
- MonsterDefinitions 18개 + Field01/02 독침벌/거미/뱀 실제 Spawn YAML 참조 확인. 독침벌/숲거미/맹독뱀 아트 출처·최종 시각 미확정; Ch2 11~18 KEEP. PilotBee README상 미연결, CC BY3.0/4.0 충돌. PVFX Foundry 8 Resources 그룹(7 그룹 연동 문서·코드, FrostNova 호출 불명), Kenney BattleIcons 14+7 런타임 매핑, Game-icons.net Path10+Material4는 실제 화면 확인 전; Xariami license unknown HOLD.
- 54행 로컬 CSV NonOriginal_Asset_Runtime_Audit_20261023.csv + 한국어 보고서 + **오리지널 디자인 시안 2종만**(잡화상인 starter-village-general-shop, 주민대표 starter-village-main-guide) 실제 PNG512/SVG/128·40 미리보기 납품. 캐논/Unity 규격 승인은 별도이며 교체0.
- ZIP 실제 납품: Limitless_Chapter12_NonOriginal_Audit_NPCPilot2_20261023.zip SHA256 4b9e08538f8669cc00c2a8a9f73db5141f8377fe5a94cf69a9d137e38847b4dd CRC PASS; NPC ZIP Limitless_Eldiran_NPC_Pilot2_Concepts_20261023.zip SHA256 7162600a51fddffc8a51087c8d1bbb1f8b14c928250ade120c197d23677fb0b3 CRC PASS. Both in ChatGPT sandbox; ZIP/PNG 바이너리 GitHub 업로드·대표 F: 자동복사 아님.
- 문서 정본: ../../Renewal_NonOriginal_ActiveVsStored_Audit_NPCPilot2_20261023.md (최초 commit 2ea29d6a82b3722ba1822bb3d3d848826c25605e).
- 후속: Codex read-only Scene/Prefab Sprite GUID/MonsterSpawn/Renderer/UI icon/Resources dynamic usage + GameView 실사, ACTIVE_RENDER_CONFIRMED/STORED_ONLY/UNKNOWN 분류. 바울 대표 NPC 2종 스타일 승인 후에만 1포즈 게임용 규격 Pilot; 12종 전체 제작 금지. Chapter3 Main23 병렬 실행 방해·main 병합 금지.
- 상태 STATIC_REPO_AUDIT_COMPLETE / LOCAL_UNITY_READ_ONLY_AUDIT_PENDING / TWO_NPC_DESIGN_CANDIDATES_CREATED / USER_ART_APPROVAL_PENDING / MAIN_UNCHANGED.


## 2026-10-22 Fox Run6 v6 미술 FAIL → v7 Frame03 앞다리 국소 복원 후보 (최신)
- 바울 대표 직접 피드백: v6 오른쪽 앞발 이상. v6는 Frame03 결함 국소수정 주문 위반으로 전신 포즈 교체, 기존 v5 대비 60,629 픽셀 변경하여 동작·앞발 방향이 달라진 **미술 FAIL**. ZIP·CRC PASS는 미술 PASS 아님. v6 정식 사용 금지.
- v7 기준은 Fox v5 ManifestFix 6개 원본 PNG. Frame03만 기존 v3에서 **원래 그려진 정상적인 연속 앞다리/관절/발끝 페인팅**으로 국소 복원, 잘려 떠있는 흰 사각형/쐐기·분리된 발 제거. 변경 6,275 RGBA 픽셀 (x159..255,y185..294). Frame00/01/02/04/05 PNG 압축바이트 해시 **5/5 그대로**; 승인 Wolf·Bear v4·게임 원본·main/Unity/Save 변경0.
- 실제 사용자 다운로드 **Limitless_Beast_Fox_Run6_v7_Frame03_ForelegFix.zip** 2,990,546바이트, SHA256 `de6139858b2f5de95c9e8256a350f0e45744f15e404efb00161688c88e1c0030`. 6PNG 512x384, strip3072x384, atlas1536x768, 12FPS GIF/APNG, 128/40 contact, frame03 old/new zoom, v6 rejection comparison, 02→03→04/05→00→01, changed coord CSV, bbox script, Manifest self-hash excluded, SHA256SUMS external. ZIP CRC PASS, Manifest 26/26 PASS, SHA256SUMS 27/27 PASS, frames exact6/6, bbox6/6 PASS. ZIP is in chat sandbox, not automatically uploaded to Github or F:.
- [Fox v7 local foreleg repair report](../../Fox_Run6_v6_VisualReject_v7_Frame03_LocalForeleg_Repair_20261022.md), initial commit `dfd714c7428c13592e22d1332dfcfbfe6b1e8a95`. Accurate state **V7_REAL_FILE_DELIVERED / OFFLINE_FILE_QA_PASS / RUN_MOTION_ART_APPROVAL_PENDING / CODEX_ISOLATED_UNITY_V7_NOT_RUN / MAIN_UNCHANGED**. Do not call the candidate approved; restore physical leg continuity is not proof of 12FPS gait acceptance.


## 2026-10-22 Fox Run6 v5 ManifestFix ZIP 무결성 정상화 — 최신
- Codex v5: CRC PASS, SHA256SUMS 38/38 PASS, Manifest 37/38 FAIL. 기존 Manifest가 자기 자신(기록 9,184bytes / 실제 9,345bytes)을 해시 목록에 넣어 SHA·CRC·크기 순환 불일치. 일반 37개 항목은 기존 해시 검사 PASS.
- 실제 새 아카이브 Limitless_Beast_Fox_Run6_v5_ManifestFix.zip 3,131,729 bytes / SHA256 `b0968d4358c916057bd23734d426f6fa0e6548a0ad8880400e4d061413f2ae29` / 39 entries; 원본 v5 ZIP SHA `5f6da110fe64f280528d9768bc9154d4175c0724f8541d72aa9ebbecf1cd44df` 보존.
- 변경된 내부 파일 정확히 Manifest.json, SHA256SUMS.txt만 2개. 새 Manifest에서는 자기 자신과 SHA256SUMS를 제외한 37개 항목의 SHA/bytes/CRC32만 검사. SHA256SUMS에는 나머지 37 + 최종 Manifest 38항목 기록, 자기 자신은 제외.
- **실물 재검증:** ZIP CRC PASS, Manifest 37/37 PASS, SHA256SUMS 38/38 PASS, 원본 대비 나머지 37/37 바이너리 파일 byte-for-byte 동일. 그중 이미지 31/31(독립 포즈 PNG6/6) SHA·바이트 변경0, CSV ↔ 실제 Alpha bbox 6/6 동일. 승인 Wolf/Bear v4 및 본게임(main) 변경0.
- [Fox v5 ManifestFix 공식 납품 문서](../../Fox_Run6_v5_ManifestFix_Integrity_Delivery_20261022.md); 최초 문서 커밋 `cf9e7485847a84e31299081985a432da77c4c5a0`.
- 파일은 이번 ChatGPT 대화 sandbox 다운로드로만 납품하고 ZIP 자체 GitHub 업로드/대표 F: 자동 복사는 하지 않음. Codex는 이 ZIP SHA부터 대조한 후 Frame03 밝은 접합부/02→03→04 실제 12FPS Unity 미술 QA 재개. 미술 사용자 최종 승인 대기, 정식 게임 적용 금지.
- STATUS: FOX_V5_MANIFESTFIX_ACTUAL_ZIP_DELIVERED / MANIFEST37_PASS / SUMS38_PASS / IMAGE31_BYTE_IDENTICAL / CODEX_ART_DELTA_PENDING / USER_ART_APPROVAL_PENDING / MAIN_UNCHANGED.


## 2026-10-22 Bear/Fox Run6 v4 연속 모션 보완 — 실제 독립 ZIP 납품 (최신)
- Codex v3 Bear/Fox PNG/metadata tech PASS, gait art HOLD. Bear 02→03→04→05 mass/head/foot transition; Fox 01→02→03→04 forepaw release/recovery/landing. Already corrected bbox CSV convention kept exactly, no unnecessary metadata refix.
- Bear v4 6×512×384 PNG/3072×384 Strip/1536×768 Atlas/6-frame 500ms GIF/APNG/128·40 previews, target motion v3-v4 comparison, Manifest/SHA256SUMS/alpha bbox QA. v3 00,01,02,05 PNG compressed bytes unchanged; 03 controlled descent/recovery, 04 shoulder-height continuity with planted paws. ZIP **Limitless_Beast_Bear_Run6_v4.zip**, **5,130,611 bytes**, **SHA256 4a9fedd54d58999def05b92839a1f00bc1d001375746e572f5d88bcade44977a**, 35 entries. [실제 납품 보고](../../Beast_Bear_Run6_v4_Continuity_Motion_Delivery_20261022.md); doc commit `37b30a6dbbd457445c2989ddcb3d321b06de59cc`.
- Fox v4 6×512×384 PNG/Strip/Atlas/12FPS GIF/APNG/128·40 previews. v3 00,05 PNG compressed bytes unchanged. 01/04 head/chest continuity, 02 near forepaw lift, 03 far foreleg bending from existing painted source and near paw recovery. Tail silhouette remains consistent. ZIP **Limitless_Beast_Fox_Run6_v4.zip**, **3,698,704 bytes**, **SHA256 9c46764df7f88194b1d15663e4df34ecdb24367f7e65eed298dfc089221b5827**, 35 entries. [실제 납품 보고](../../Beast_Fox_Run6_v4_Continuity_Motion_Delivery_20261022.md); doc commit `5a809599f54354b31911fdcf5030b41b7f14a69d`.
- **Actual files in ChatGPT sandbox** (not copied to representative's F:, not binary-uploaded to GitHub): `/mnt/data/Limitless_Beast_Bear_Run6_v4.zip`, `/mnt/data/Limitless_Beast_Fox_Run6_v4.zip`.
- Each: Pillow PNG6/6, RGB+Alpha Strip/Atlas per-frame 6/6, 3px alpha border leak0, unique6, GIF/APNG duration500ms/6frames, Python recompute alpha bbox 6/6 from final pixels, ZIP CRC testzip PASS, all Manifest per-file SHA/readback PASS. Combined bbox12/12 PASS. ZIP SHA readback from final immutable archive. QA indicates asset integrity only; NOT 12FPS Unity art PASS.
- **Important provenance**: v4 local pose deformation and painted-source far-foreleg rearticulation, not newly hand-painted all six original animals; 6-motion aesthetic PASS subject to Codex and user. User-approved Wolf v5 and main/Unity/Scene/Save/monsters untouched. Current `BEAR_FOX_V4_FILE_QA_PASS / CODEX_ISOLATED_UNITY_V4_PENDING / EACH_USER_MOTION_ART_APPROVAL_PENDING / NO_PRODUCTION_APPLY`.


## 2026-10-22 Bear/Fox Run6 v3 실 PNG·ZIP 및 bbox CSV 12행 전체 수정 (현재 최신)
- User reports Codex v2 Unity technical PASS / art HOLD. Bear issue 03→04 landing and realistic fore/hind foot support; Fox issue 02→03 far foreleg visibility/recovery. Bear v2 05→00 improved head-height and Fox v2 tail preserved.
- Bear actual deliverable Limitless_Beast_Bear_Run6_v3.zip; 5,554,445 bytes; SHA256 adc3f36a65c9bc093cf2c1b3e68b7da3abe5d68c4edac461096fb1611f4a0a35. Changed independent art poses 03/04; exact v2 PNG compressed bytes preserved 00/01/02/05. Six RGBA 512x384, strip3072x384, atlas1536x768, 12FPS GIF/APNG, 128px/40px, frame/transition comparisons, CSV/meta/Manifest/SHA256SUMS. Documentation commit 83944ecbe22b5181e8886f2c49bd62cf71b356bf.
- Fox actual deliverable Limitless_Beast_Fox_Run6_v3.zip; 4,138,420 bytes; SHA256 d4727d0e5fedbb2db7d224f0684fc6c80c78221aa120da3bef356ab43b800696. Local painted-source far leg cutout from v2 Frame01 added beneath unchanged torso on frames02/03; v2 compressed bytes of 00/01/04/05 preserved. Remaining knee/shoulder movement requires Codex art QA. Documentation commit 26ee05c2535b5460caa9666d95ba87e3974bb2f9.
- Both ZIPs are tangible user-download files in this conversation sandbox, NOT uploaded as GitHub binaries or directly copied to F:. 24 entries each; zip CRC PASS, manifest SHA readback, PNG decode 6/6, strip/atlas per frame pixel equality 6/6, 3px no alpha bleed, 12FPS 6 frames per 500ms cycle.
- Important QA fix: Both QA/Frame_Gait_And_Contact.csv have actual PNG A>0 bbox values in order bbox_min_x,bbox_min_y,bbox_max_x_exclusive,bbox_max_y_exclusive, 0-based top-left x-right/y-down. All 12 rows checked by included Python QA/verify_bbox.py. Unchanged frames NOT reworked to fix metadata.
- Status BEAR_V3_FILE_QA_PASS / FOX_V3_FILE_QA_PASS / BBOX_CSV_12_OF_12_PASS / REAL_UNITY_12FPS_V3_NOT_RUN / USER_ART_APPROVAL_PENDING / WOLF_APPROVED_UNTOUCHED / MAIN_UNCHANGED. Genuine gait/weight and paw timing cannot be established by ZIP integrity alone. Codex delta-only Unity visual QA for each before user final acceptance; no auto game replacement.
- Documents: [Bear v3](../../Beast_Bear_Run6_v3_TargetedArt_BBoxFix_Delivery_20261022.md); [Fox v3](../../Beast_Fox_Run6_v3_TargetedArt_BBoxFix_Delivery_20261022.md).


## 2026-10-22 Bear/Fox Run6 v1 Codex 기술 PASS·미술 HOLD → 독립 v2 실제 PNG/ZIP 납품 (최신)

- 바울 대표: Wolf Run6 v5 사용자 최종 미술 승인/실게임 반영 완료이므로 **Wolf 정본 재제작 및 변경 금지**. Bear와 Fox 각 v1이 격리 Unity 기술 PASS/미술 HOLD. Bear Frame05→00 머리/접지, Fox 앞발 구분/꼬리 일관성을 집중 보완한 **동물별 v2 독립 아트 후보** 제작.
- 신규 1774×887 3×2 포즈 시트에서 **실제 서로 다른 동물 자세6을 각각 분리**하고 세부 실루엣 정돈, 512×384 RGBA PNG6, 3072×384 Strip, 1536×768 Atlas, 12FPS GIF/APNG 500ms/6, 128px·40px·v1 대비·05→00 미리보기 및 QA 생성. 기존 v1의 작은 그림 확대를 다시 재사용한 게 아니라 신규 원화 시트 이용. **전 프레임 512px 단위 완전 수작업 드로잉이 아님**, 미술 PASS 아직 미정.
- **Bear** `Limitless_Beast_Bear_Run6_v2.zip`: **5,784,917 bytes / SHA256 `3d74103e01fcd9f17127afcecf52eaa9f578af96f8fa23a197a1237681c3c147` / 36 entries**. Frame05→00 바닥 bbox 최하단 325→326px, 머리/가슴 무게감 새 6포즈. `docs/art-renewal/문서/00_프로젝트/Beast_Bear_Run6_v2_TargetedMotion_Delivery_20261022.md`, 첫 문서 commit `5a6a451de1205b82ba81ff953d6ff4748c015722`.
- **Fox** `Limitless_Beast_Fox_Run6_v2.zip`: **4,166,008 bytes / SHA256 `22e780627a96a87b2d1ce06f746248b0b9158cc59c834b8c6d4b2c6f9661c6ea` / 36 entries**. 앞다리 교차 자세와 꼬리 기준 윤곽을 유지한 신규 6포즈, 바닥 bbox 마지막05→첫00 325→326px. `docs/art-renewal/문서/00_프로젝트/Beast_Fox_Run6_v2_TargetedMotion_Delivery_20261022.md`, 첫 문서 commit `b1bffed559f871da2723a780a8a992df063d9072`.
- 실제 ZIP/개별 PNG는 해당 ChatGPT 대화 Sandbox 다운로드로만 납품. GitHub 브랜치에는 문서/인계만 저장, 바울 대표 F: 로컬 파일 직접 사용·복사0. Bear/Fox 별도 GitHub 승인 KeyArt 정본은 main tree에서 확인 못하고 제3자 ScratchIO 임시 Sprite 두 개만 존재 → 새 두 v2 원화는 자체 제작 6포즈 시트 출처, 기존 제3자 픽셀 복제 아님.
- 검사: 각 ZIP 6 PNG 재해독, 사방 프레임 Alpha 침범0, Strip/Atlas 6cell RGBA 바이트 동일6/6, GIF/APNG 총0.5sec/12FPS, ZIP testzip CRC/모든 entry SHA PASS. 단 이것은 `OFFLINE_ASSET_TECH_PASS / CODEX_V2_UNITY_QA_PENDING / BEAR_FOX_INDEPENDENT_USER_MOTION_APPROVAL_PENDING / OFFICIAL_WOLF_AND_MAIN_UNCHANGED`. Codex 격리 GameView 05→00·자세/접지 및 바울 대표 종별 승인 필요. 승인 이전 Bear/Fox 정식 Unity 적용 금지, 기존 승인 Wolf 등 자산/게임/Save/main 불변.



## 2026-10-22 승인 Wolf Run6 v5 다음 단계 — Bear/Fox 각각 Run6 v1 실제 PNG+ZIP 납품 (현재 최신)

- 바울 대표 직접 최신 결정: **Wolf Run6 v5 최종 미술승인/실제 게임 적용/main 통합·로컬동기화 완료, 기존 Wolf 변경 금지**. 이번 발주는 **Bear → Fox 독립 Run6 v1 파일럿 2종**. Codex Unity 실제 Bear/Fox 검증/본게임 반영은 아직 아님.
- GitHub main 전체 tree 7088 항목에서 Bear/Fox 각각 공식 승인 고해상도 정적 PNG는 발견하지 못하고 `Resources/CompanionAssaultValidation/{Bear,Fox}_Run.png` 제3자 ScratchIO 임시 후보만 확인. 제3자 그림 픽셀 트레이스/리컬러0. 신규 동물 원화는 ChatGPT 이미지 생성 원본 6포즈 보드에서 서로 다른 6포즈를 개별 분리·투명 처리한 후보이며 **보드 원본 각 포즈의 정보량은 약120px, 512px는 확대 후보**. '승인 정적 Bear/Fox 원화' 혹은 512px 수작업 순수 원화라고 주장 금지.
- **Bear ZIP** `Limitless_Beast_Bear_Run6_v1.zip` 실제 채팅 Sandbox 파일, **5,725,723 bytes / 37 entries / SHA256 `1a29475207c0f99db87726b637d301e5010bf5ef6790e2415ad679f67ffd7f19`**. 512×384 투명 좌향 PNG 6개, 3072×384 Strip/1536×768 3×2 Atlas, 12FPS/0.5초 GIF/APNG, 128px·40px 콘택트 시트/개별/05→00, Manifest/SHA256SUMS/오프라인 QA/프레임 설명/원본·분리 코드. **CRC/PNG6/Strip·Atlas6 동등/프레임 테두리 Alpha0 PASS**. 보고서 [Bear v1](../../Beast_Bear_Run6_v1_ArtPilot_Delivery_20261022.md) Commit `cd9a666fbc549c9deb97a494e1f1ce68c44c681f`.
- **Fox ZIP** `Limitless_Beast_Fox_Run6_v1.zip` 실제 채팅 Sandbox 파일, **4,860,377 bytes / 37 entries / SHA256 `6d352219505497a261ffe2c1d687cece28bdf4859003d16fce88334d9a15f43d`**, 위와 같은 독립 6 PNG/Strip/Atlas/12FPS/128·40/검증 구성, **CRC/PNG6/셀 동등/Alpha0 PASS**. 보고서 [Fox v1](../../Beast_Fox_Run6_v1_ArtPilot_Delivery_20261022.md) Commit `fc0bc2afac2a6751ca4e055dbc42b29b569b4cb4`.
- 현재 정확 상태 `BEAR_V1_PNG6_ZIP_DELIVERED / FOX_V1_PNG6_ZIP_DELIVERED / BOTH_OFFLINE_FILE_QA_PASS / BOTH_USER_MOTION_STYLE_APPROVAL_PENDING / BOTH_NOT_UNITY_APPLIED / MAIN_UNCHANGED`. 아트 위험: 프레임 사이 접지/05→00 모션 자연스러움 **아직 미술 검토 필요**, 확대 원화 detail은 생성 원본의 소형 포즈 한계 있음. 128px 검토는 실제 Unity 동일 WorldSize/GameView가 아님.
- 다음: 바울 대표 Bear/Fox **각각 별도 미술/모션 승인** 후 필요한 포즈만 보완; 그 후 Codex 격리 실제 PPU/게임 런타임/40px UI 12FPS QA. 기존 승인 Wolf Run6 v5, Field09 합격6, 몬스터, Unity Editor·Save·UserData, main 직접 commit/Push/merge 보호. GitHub에는 문서만 커밋; PNG/ZIP 바이너리는 채팅 Sandbox에서 납품(바울 대표 F: 자동 복사0).



## 2026-10-21 Wolf Run6 v4 기술 PASS·Unity 12FPS PASS / Frame05 미술 FAIL → v5 국소 보완 실제 납품 (현재 최신)

- 바울 대표 확정: 기존 Frame00~04/복원 03·04 꼬리/승인 Wolf 단일 v5 KeyArt **변경금지**. 실패 한 곳: `Wolf_Run_05_v4.png` 뒷다리 x315~338 / y235~280의 직각 윤곽. 다른 늑대 그림 이미지 생성기 결과는 **폐기·사용0**.
- 새 `Wolf_Run_05_v5.png` 원본 v4 실제 PNG에서 **174 RGBA픽셀만**, bbox **x318..333 / y245..280**, 무릎-정강이 윤곽 곡선형 재구성 및 해당 복원 영역의 기존 다리 근접 색 보간. Frame00~04 5PNG **파일 바이트·SHA 완전 동일**. 독립 신규 늑대 전체 원화로 교체한 게 아니며 승인 KeyArt SHA `b770c73c36b9035a2bb0e70b8c126090b5fa248d27db542af077094d12ee2ea1` 직접 재대조 PASS.
- 실제 ZIP **`Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtRework_v5.zip`**: **2,663,380bytes / SHA256 `16ba7923771c98f05b0f041b5c5a63857cdecfcdfd2e1f7768d9f24d1af0e351`**. 6 독립 PNG 512×384, Strip 3072×384, 3×2 Atlas 1536×768, 12FPS 0.5초 GIF/APNG, 128px·40px 미리보기, Frame05 5x 확대 v4↔v5, 루프 05→00 비교, 변경 RGBA 정확174행 CSV, Manifest per-file SHA, QA. ZIP CRC/readback·PNG Decode6·Strip/Atlas 6/6·00~04 원본 byte5/5 PASS.
- [v5 수정·미술 보류·SHA 정본](../../Wolf_Run6_ArtRework_v5_Frame05_Hindleg_Delivery_20261021.md), 최초 문서 commit `1dd00402ece7cd6cfcd77c92e83d556c6cdc5b25`. ZIP/PNG 실제 파일은 본 ChatGPT 샌드박스 다운로드 제공, GitHub 바이너리와 F:에 자동저장0. 원본 Unity/main 변경0.
- 정확 상태 `WOLF_RUN6_V5_ACTUAL_PNG_ZIP_DELIVERED / OFFLINE_TECH_QA_PASS / FRAME05_MOTION_ISOLATED_UNITY_QA_PENDING / USER_RUN_ART_APPROVAL_PENDING / NO_RUNTIME_APPLY`. Codex는 **05만 격리 12FPS 실제 Motion/05→00 DeltaQA**, 대표 승인 전 나머지 모든 아트/정식 펫 교체/전체 QA 반복 금지.



## 2026-10-21 Wolf Run6 v3 기술/Unity PASS · 미술 HOLD → v4 모션보완 실제 PNG ZIP (최신)
- [Issue #43](https://github.com/HappyPaul812/Project-Limitless/issues/43) 최신 `#issuecomment-6310494103`, `docs/planning/문서/00_프로젝트/20261021_Village_WolfRun_JointQA_ShaMismatch_TargetedProduction.md` 확인. v3는 격리 Unity 실제 12FPS PASS이나 Frame04 어색한 후지관절/발접지·중간 몸체비율·05→00 Loop 미술 HOLD.
- v3 ZIP 해시불일치: **초기 Codex 수령 f7eb4122… (보고상 2,788,009 bytes) vs 이 세션 실제 최종 파일 171b954d… (2,788,020 bytes)**, 최종 GIF 타이밍/인계 갱신 단계에서 서로 다른 ZIP 릴리스 존재. 11byte 차이; **초기 ZIP 바이너리가 현 컨테이너에 없으므로 어떤 ZIP 엔트리가 다른지 세부 원인은 추가 read-only 대조 필요**, 동일 바이너리라고 주장하지 않음.
- v4 실제 `Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtRework_v4.zip`, **3,024,517 bytes / 34 files / SHA256 `3ab0b57d2389b55ed33f73c1d3d13862b9b595245e196d1f3a4c98f090b8a412`**. 512×384 RGBA PNG 6장, 3072×384 Strip, 1536×768 Atlas, 12FPS GIF/APNG, 128px/40px, v3↔v4 동시 움직임, Frame04 후지·Frame05 전지의 2x Before/After, 05→00 정적 전후, Manifest/per-file SHA/foot·bbox CSV/QA.
- v3 **00~03 PNG 파일 바이트 동일**. 04의 부자연스럽게 위로 꺾인 뒷발을 Frame01에 실제로 칠해진 관절·털·발끝을 활용해 굽힌 새 독립 다리 위치로 재구성(4,368 RGBA픽셀 변경)하고 승인된 꼬리 영역(346,88..485,164) pixel 동일. 05는 기존 뻣뻣한 앞다리를 들어 올린 전완 굽힘 자세로 재배치(7,542픽셀 변경), 05→00 루프 개선 후보. **이전 원화 조각을 회전/재배치한 포즈 구성이지 전체6 신작을 독립 손그림으로 그렸다는 주장 금지**.
- 승인 Wolf v5 KeyArt `b770c73c36b9035a2bb0e70b8c126090b5fa248d27db542af077094d12ee2ea1` 실제 SHA 재검사/변경0. ZIP stdlib CRC PASS/PNG6 Decode/Strip·Atlas 6칸 픽셀동일/3px 외곽알파0. 모션은 **Codex 격리 실제 Unity v4 재검수/바울 대표 모션 최종승인 별개**, PPU/본게임 Wolf Run 교체 금지.
- [납품·해시·QA 보고](../../Wolf_Run6_ArtRework_v4_RealPNGs_DeltaQA_20261021.md), 최초 보고 커밋 `e6b80f84aecda57ebcc29c3b77ce739e4766e979`. 새 PNG/ZIP은 이번 채팅 sandbox 파일, GitHub 바이너리/F: 자동전달 아님. 다른 챕터1·2/Chapter3 asset, Bear/Fox/Snake, Scene/Save/UserData/main 변경0.
- 현재 `WOLF_RUN6_V4_DELIVERED / OFFLINE_TECH_QA_PASS / MOTION_VISUAL_ART_PENDING / CODEX_ISOLATED_UNITY_V4_PENDING / USER_APPROVAL_PENDING`.



## 2026-10-21 Wolf Run6 v2 기술 PASS / 미술 FAIL → v3 실제 6PNG+Strip/Atlas/12FPS 재작업 (현재 최신)

- 근거 Issue #43 최신 Codex 댓글 및 `docs/planning/문서/00_프로젝트/20261021_Main22_ApplyReady_WolfRun6_ArtFail_NextActions.md`. 이전 v2는 프레임03·04 꼬리 절단, 03 뒷발 절단, 05→00 접지·몸통 비율 미술 FAIL, 형식 PASS/실제 Unity Import 미실행.
- **현재 v3 ZIP 실물** `Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtRework_v3.zip`, **2,788,020bytes/36entries/SHA256 `171b954ddeef2b37d1bfaeb197b8a9f028293494457b66664c1d83509b87b5e0`**. Frame 00~05 각 512×384 RGBA, Strip 3072×384, 3×2Atlas 1536×768, 실제 GIF 80/80/90/80/80/90ms 총 500ms=12FPS 및 APNG, 128px·40px 아이콘/실 프레임 v2↔v3 동시 GIF, 꼬리03/04 2x 비교, 발 접지/몸통 시각비례 추정 CSV·JSON, Manifest per-file SHA, CRC 재검증 PASS. ZIP은 채팅 Sandbox 파일, GitHub 바이너리/사용자 F: 자동저장 없음.
- **정직한 변경 범위**: 00/01/02/05 미술 원본 v2 그대로, **03 꼬리+후방 잘린 발 재구성**, **04 꼬리 스텁→연속 꼬리 재구성**. 같은 늑대 05의 실제 털 페인팅을 자세별 방향·접지에 맞게 재배치/복원. 6프레임을 단일 정적 PNG 여섯 번 위치이동한 것이 아님. 다만 v2 콤포지트의 근본적인 체형/움직임은 **Run6 미술 승인 전까지 QA 대상**, 완전 독립 신작6포즈로 허위 보고 금지.
- 승인 원본 `Wolf_KeyArt_v3_AlphaFix_v5.png` SHA256 `b770c73c36b9035a2bb0e70b8c126090b5fa248d27db542af077094d12ee2ea1` 실제 재확인, 원본 변경0. v2 ZIP SHA256 `c69ca17e8ebe2b8591c4dc77ebc6531b60dbe8a44ee3475d39d4cc6159f8f2ad` 재확인 변경0.
- [Run v3 제작·실물 파일 명세 및 미술 보류사항](../../Wolf_Run6_ApprovedV5_ArtRework_v3_Delivery_20261021.md), 문서 최종 커밋 `b6800d7a5ecd84c9c26537e3aa636c621eef50b7`. 현재 `WOLF_RUN_V3_6PNG_AND_ZIP_DELIVERED / OFFLINE_FILE_TECH_PASS / 12FPS_PREVIEW_READY / USER_RUN_MOTION_ART_APPROVAL_PENDING / CODEX_RUNTIME_NOT_RUN / MAIN_UNCHANGED`. Codex는 새 ZIP만 격리 Delta 확인, 전체 타 작업 재실행·원본 Unity 변경 금지. Bear/Fox/뱀 모션과 환경 신규 제작 없음.



## 2026-10-21 Wolf Run6 실제 Import 후보: 6 PNG + Strip + 12 FPS / 시각 승인 전 (최신)
- 사용자 질의 "이걸 게임에 어떻게 넣니?"에 대한 조치: 일렬 합성 아트는 그대로 Unity 사용 불가 → 이번 이미지 소스에서 6포즈를 **실제 개별 동일 캔버스 512×384 RGBA PNG**로 추출하고 독립 strip/atlas/12FPS GIF/APNG/128·40px QA 생성.
- 실제 ZIP `Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtPilot_v2_CANDIDATE.zip`: 2,482,392 bytes, 29 entries, SHA256 `c69ca17e8ebe2b8591c4dc77ebc6531b60dbe8a44ee3475d39d4cc6159f8f2ad`. Strip 3072×384/6 cells, 3×2 Atlas 1536×768도 있음. PNG Frame→Strip→Atlas 픽셀 대조6/6, Alpha frame boundary0, frame SHA 중복0, ZIP CRC PASS.
- 승인 v5 정적 1포즈 SHA256 `b770c73c36b9035a2bb0e70b8c126090b5fa248d27db542af077094d12ee2ea1` **확인·보호**. 이전 v1/v2 런 재활용/게임 코드 변경/Unity 원본 적용 없음. 기존 원본 v3/v4/v5 변경0.
- **반드시 구분:** 이 작업은 *그림 6마리 합성 소스를 분리한 import 후보*로, 4→5/5→6 경계의 원화 겹침·모션/알파 프린지·05→00 루프는 실제 미술 심사 필요. **ART_NOT_YET_APPROVED / UNITY_NOT_APPLIED**. 유저가 ZIP 내려받아 승인한 뒤 Codex 새GUID 격리QA; 이 ZIP만 있으면 Unity에 넣을 수 있는 구조지만 즉시 공식 반영하지 않음.
- [완료 내역과 Codex 기술 지시](../../Wolf_Run6_ArtPilot_Composite_ImportCandidate_20261021.md), 문서 최초 커밋 `768f77f3e2d68386311e8badd3eb0c58806d5e50`.
  


## 2026-10-21 Wolf v4 미술 HOLD → 세 영역만 AlphaFix v5 실제 PNG/ZIP 납품 (최신)

- Issue #43 최신 Codex 및 `docs/planning/문서/00_프로젝트/20261021_Wolfv4_F1v2_QA_TargetedFix_Orders.md` 직접 확인: Wolf v4 기술 PASS, 미술 HOLD(가슴 **위쪽 회색 배경조각**, 다리 미세 배경잔여, 앞발 밑 바닥조각). **v3/v4 원본 보호, v5 별도 후보만**, RGB·생물 실털/발톱·크기·포즈 수정 금지.
- 실제 원본 SHA 재검증: v3 `30b2d464fe2c456c0fa7d82f1fad51f64065b6e8379ea174548deec7d1b91b4e`; v4 `62f72e97525f0266cbb6132da8b21571594f7c0deb3b1e1749cce749c9bf1c0a`; Snake v3 원본 `7001d199b279cb8e541fc603bd62351ab460d3b44da91bc91a2d65dc099f8fd0` 보존/Codex 정적 PASS **권고**, 대표 최종 승인 아님.
- 새 **`Wolf_KeyArt_v3_AlphaFix_v5.png`** 실제 파일 952×1306 RGBA SHA256 **`b770c73c36b9035a2bb0e70b8c126090b5fa248d27db542af077094d12ee2ea1`**. 원본 v4 기반 국소 alpha 감소 2,174px (**가슴 900, 다리 237, 앞발 1,037**). 전체 RGB 변경 0, Alpha 증가 0, 변경 영역 외 변화 0. v3/v4 원본 PNG 덮어쓰기0.
- 실제 ZIP **`Limitless_Wolf_KeyArt_v3_TargetedAlphaFix_v5.zip`** 2,214,467 bytes/13 entries/SHA256 **`920459eb2ef4e160461b2b6425a9b183da633cd14411655a4d5affdbaf3d8bc7`**. 이번 대화 Sandbox 다운로드만 제공, GitHub ZIP 바이너리·사용자 F: 자동저장 아님. Assets/ v5 단일 PNG, Manifest old/new SHA, QA/Changed_Alpha_Coordinates_v4_to_v5.csv(2,174 행), 2배 가슴/다리/앞발 v4↔v5 실제 이미지, 128px BeforeAfter(2배경), Alpha 변경 오버레이/오프라인 QA. ZIP `testzip` CRC PASS, PNG 디코드/해시 readback PASS.
- [v5 실제 납품·픽셀·검증 기록](../../Wolf_v4_HOLD_TargetedAlphaFix_v5_ActualDelivery_20261021.md), 최초 commit `56e901f271e63c70ba6bd745f3f60bd57ff061fd`. **Codex 실제 GameView 아님 / User 미술 승인 대기**. Wolf Run·Bear·Fox·Snake 모션/원본 Unity/Field09 5+B/Save/MonsterDefs/main 변경 없음. `CODEX_WOLF_V5_DELTA_ART_QA_PENDING / USER_ART_APPROVAL_PENDING / MONSTER_ANIMATION_HOLD`.
  


## 2026-10-21 Snake v3 PASS 권고 보존 · Wolf v4 한정 알파 보정 실제 PNG/ZIP 납품 (현재 최신)

- Issue #43 최신 Codex: **Snake v3 정적1포즈 미술 PASS 권고(대표 최종승인 별개), Wolf v3 미술 FAIL**. Codex 비교는 실제 Unity GameView 아닌 오프라인 합성. 이 세션에서 Snake 그림 변경0. 정본 `VenomSnake_KeyArt_v3.png` SHA256 `7001d199b279cb8e541fc603bd62351ab460d3b44da91bc91a2d65dc099f8fd0` 실제 원본 재대조 PASS. 독사 x=0 알파 접촉25px·작은 알파 군집548건 기술 경고 기록 유지.
- Wolf v3 실제 PNG 원본 `30b2d464fe2c456c0fa7d82f1fad51f64065b6e8379ea174548deec7d1b91b4e` **보존**, 새 `Wolf_KeyArt_v3_AlphaFix_v4.png` SHA256 `62f72e97525f0266cbb6132da8b21571594f7c0deb3b1e1749cce749c9bf1c0a` (952×1306 RGBA). 회색 가슴 매트·다리 근처 배경 조각만 국소 알파 12,088px 감소/제거, 원본대비 RGB 픽셀 변경0, 새로운 늑대 재디자인0. 원본 PNG도 ZIP에 별도 보존.
- 실제 ZIP `Limitless_Wolf_KeyArt_v3_TargetedAlphaFix_v4.zip` 2,466,565 bytes / 14 entries / SHA256 `e241535f1c505bf50f4de09317e74dbc3780be722139ff0baa02c908223765c6`. 이번 ChatGPT Sandbox 파일로 실물 납품. Chest/Rear/Leg 전후 체커보드 비교·128px 전후·Alpha 변경 지도·Manifest/픽셀별 CSV/QA 제공, ZIP testzip CRC/PNG 재Decode/SHA PASS. **게임 실제 시각 최종 통과 아님**, 털 가장자리 대표 미술검수 필요. ZIP GitHub 바이너리나 사용자 F: 자동저장으로 주장 금지.
- [실제 납품 기록](../../MonsterWolf_v3_SnakePass_WolfAlphaFix_v4_ActualDelivery_20261021.md) 최초 문서 Commit `ca455df97a405d5818d63a55f49f8b6ccd2af2ff`. `docs/art-renewal`만 커밋, 원본 공식 Field09 승인5+B/기존 18 몬스터/게임 시스템/main 변경0. Codex 별도 Chapter3 검증 방해 금지, Snake 모션/Wolf Run/Bear/Fox 제작 및 Unity 정식 적용은 사용자 승인 전 HOLD.
- 상태 `SNAKE_V3_PASS_RECOMMENDED_USER_PENDING / WOLF_V4_ACTUAL_FIX_DELIVERED / WOLF_V4_OFFLINE_QA_PASS / WOLF_V4_USER_ART_PENDING / NO_ANIMATIONS / MAIN_UNCHANGED`.



## 2026-10-21 Chapter1·2 맵 핵심 마감 관리 → 맹독뱀·늑대 기존 v3 단일 원화 실물 재검사 (최신)

- 기획 정본 `docs/planning/문서/00_프로젝트/Renewal_20261021_Map_Closeout_And_MonsterWolf_V3_Resume.md`, 발주 `Chapter1_2_Monsters_And_Beasts_Original_Art_Renewal_Assignment_20261019.md`, Issue #43/#38 확인. **Field09 승인 5+B Glow PR41 official main 반영됨; 기존 합격본 재제작 금지.** 잔여 HeatVent/Spire·Field10 예술 승인 backlog만. Codex 현재 Chapter3 FrostFieldMouse 전투 Fix v2 작업 중 → 중복 요청 금지.
- 이전 세션 자체 v3 PNG **실물 2개 이번 컨테이너에서 발견**, 이전 기록 SHA256 2/2 정확히 일치, **신규 재제작 없음**:
  - `VenomSnake_KeyArt_v3.png` 1254×1254 RGBA SHA256 `7001d199b279cb8e541fc603bd62351ab460d3b44da91a2d65dc099f8fd0`
  - `Wolf_KeyArt_v3.png` 952×1306 RGBA SHA256 `30b2d464fe2c456c0fa7d82f1fad51f64065b6e8379ea174548deec7d1b91b4e`.
- 실제 새 **검토 패키지** `Limitless_MonsterWolf_KeyArt_v3_Review_20261021.zip` **3,468,949 bytes / 13 entries / SHA256 `a5189bdb542988da69581135f137b39c98b3c5434272cb5020d13310a98fdae8`**. 원본바이트 동일 PNG2 + 개별 128px RGBA 미리보기2 + 밝기배경 테스트4 + 두 종 128px 동일 슬롯 이미지1 + 기존 Chapter2 공식5종 GitHub raw PNG와 비교하는 온라인 HTML + Manifest/QA/README. ZIP testzip CRC PASS.
- 실제 Unity `.meta` 확인: 공식 Ch2 5종은 모두 314×314 sprite rect, PPU314, Pivot center → 프레임당 1 world-unit. 하지만 GitHub raw 큰 PNG5가 현재 로컬로 내려오지 않음. HTML은 공식 실제 GitHub PNG URL을 참조하며 **네트워크 필요**, 동일 GameView RenderScale로 검증한 화면 아님. v3 PPU/WorldSize는 **미결**. 공식 5와 동배율 실제 Unity 비교 `NOT_VERIFIED`.
- 원화 미술 QA: 독사 x=0에 alpha25픽셀 접촉, low-alpha 고립 군집 다수(16px 미만 548개) → 직수입 전 아트 정리 필요 가능성. 늑대 털·발 끝 배경제거 매트 잔여 위험. 모두 **USER_ART_REVIEW_PENDING**, 전체 애니/Run/Bear/Fox 작업0.
- [실제 PNG2 원본·128px·공식5 비교 링크·QA 납품 보고](../../MonsterWolf_v3_ActualFile_Review_128px_And_Chapter2_Reference_20261021.md); 첫 문서 commit `8ee584f26e01fc3203b43f8b0c574f4cbca5fcd6`. 아트 ZIP/PNG는 채팅 Sandbox에서 제공; 브랜치에는 보고서/인계만(바이너리 GitHub 저장0, 사용자 F: 자동 복사0). 정식 장면/몬스터정의/메인 변경0.
- 정확 상태 `MAP_CORE_CLOSEOUT_SUBSTANTIAL / FIELD09_REMAINING_PROPS_BACKLOG / FIELD10_USER_ART_REVIEW_PENDING / V3_SOURCE_2PNG_FOUND_SHA_PASS / V3_128PX_REVIEW_DELIVERED / OFFICIAL_SAME_GAME_SCALE_NOT_VERIFIED / USER_V3_APPROVAL_PENDING / NO_ANIMATION_OR_PET_EXPANSION`.



## 2026-10-20 Field09 Phase2 Cliff/Fissure 실제 PNG 2종 v2 보완 납품 (현재 최신)

- 바울 대표와 Codex 검수 Phase2 Pilot v1: **기술 30PASS / 미술 Visual FAIL** (Cliff 발치 밝은 매트, Fissure 외곽 매트와 기존 Glow 붉은 띠 노출). 기획 정본 `docs/planning/문서/00_프로젝트/Field09_Phase2_Pilot_v1_VisualFail_CliffFissure_TargetedArtFix_20261020.md`, Issue #38 최신 댓글 확인.
- 기존 v1 실제 PNG에서 **두 후보만 국소 수정**: `LL_Field09_Phase2_Cliff_Blocker_v2.png` SHA256 `ad995f3cc4914759d6c88fa47c80227cb1a7c5f1a9a549679e06472402fee9f1` (13,918px 변경), `LL_Field09_Phase2_Fissure_Edge_v2.png` SHA256 `c3dc0c131c11fef72e4d210a1a43528f9882d8c50486e8da0bc15b6984ea145a` (14,574px 변경). Canvas512² RGBA, PPU128/Pivot center 제안, Collider/Sorting/Scene 원본 불변.
- 실제 ZIP `Limitless_Field09_Redesign_Phase2_Pilot_Fix_v2.zip` **3,602,303bytes / SHA256 `0e4b7bfb3784b5b97a4a719c669f9564f502d67e88c2096d32475fa69a6876c8`**, 13 entries. Assets 신규 PNG2만, Manifest 원본/수정 SHA, 픽셀별 변경 CSV 28,492개, 승인 Base+새 소품 전후/접지 확대 PNG, 온라인 원본 Glow 정확 겹침 HTML, QA JSON, 재현 스크립트. 표준 ZIP 재해제/CRC testzip/Pillow PNG Decode/SHA **오프라인 PASS**. ZIP과 PNG는 이번 채팅 sandbox에 실제 첨부. **GitHub repo에 바이너리 넣거나 바울 대표 F: 자동 복사하였다고 주장하지 않음**.
- 승인 Base Ground v1 SHA `1bd8a614988ee182640a71b9771646c3009545140e0522a3290ee3b0e3138203` / Cracked Overlay v2 SHA `0147e6aff58e5304fd71e7474a9c018958a4234ec4c2dc2cf7382a905a7be9b7` / Obsidian Slab v1 SHA `dfa3abb2c6c86abc3f94baa213428038a1c15b4c963e76fef69491a6deb09692` **실제 파일 재대조 3PASS / 수정0**. 공식 Fissure Glow PNG/meta/GUID 원본 그대로. Glow sorting -7가 Edge sorting -8보다 앞이므로 아트 외곽 정리만으로 Glow 붉은 띠가 전체적으로 사라진다는 주장은 **금지**. 독립 Unity 실제 Glow 결합 검증 필수, 지속 시 Renderer/Material/Mask 기술문제 분리해 추가 승인 받음. `OnlineCompare.html`은 GitHub 공식 Glow를 URL에서 읽어와 동일 위치 위아래로 겹침; 오프라인 PNG는 Glow 실제 포함하지 않고 GameView도 아님.
- 보고서 [Field09 Phase2 Cliff/Fissure 수정 v2](../../Field09_Phase2_Pilot_v1_VisualFail_CliffFissure_ArtFix_v2_Delivery_20261020.md) 최초 commit `6fb4701d9749b28d7735991a3b68721c713f3a14`. Codex 재현용 `Art_Tools/Field09_Phase2_CliffFissure_Fix_v2.py` commit `7e92e05f1a8cd27b17c60a6f5e4b412f5f4937fb`. 상태 `FIX_PNG2_DELIVERED / OFFLINE_QA_PASS / GLOW_RENDERMASK_RISK / CODEX_UNITY_DELTA_PENDING / USER_VISUAL_APPROVAL_PENDING / MAIN_UNCHANGED`.
- Codex 다음은 기존 PASS30/17/92 **재실행 금지**. Cliff 접지/Edge+Glow 결합/3 GameView, 기존 Main18 시각 정보만 델타 검증. 바울 대표 최종 미술 승인 전 다른 환경 소품 양산/공식 원본 적용/메인 병합 금지.



## 2026-10-20 Field09 Phase2 신규 소품 2종 실제 납품 — 최신 결정/승인 상태

- [Issue #38](https://github.com/HappyPaul812/Project-Limitless/issues/38) 및 기획 `docs/planning/문서/00_프로젝트/Field09_Redesign_Sample3_UserVisualApproval_And_Phase2_20261020.md`: **승인된 Base Ground v1/Cracked Overlay v2/Slab v1 절대 수정 금지**. 실제 SHA256 재확인 3/3 PASS (`1bd8a614988ee182640a71b9771646c3009545140e0522a3290ee3b0e3138203`, `0147e6aff58e5304fd71e7474a9c018958a4234ec4c2dc2cf7382a905a7be9b7`, `dfa3abb2c6c86abc3f94baa213428038a1c15b4c963e76fef69491a6deb09692`).
- 실제 Scene `Unity/Client/Assets/_Project/Scenes/Field_09_ObsidianScar.unity` 및 공식 원본 PNG6종 육안 확인 후 분류: **AshPatch KEEP 후보**, **Cliff Blocker와 Fissure Edge REDESIGN 이번 Pilot**, FissureGlow·HeatVent·Spire **REDESIGN 필요/HOLD 다음 묶음**. 분류/Renderer 실제 position/scale/sorting CSV는 ZIP에 포함. Cracked02 미사용 HOLD.
- 신규 `512x512 RGBA` PNG 실제 제작2: `LL_Field09_Phase2_Cliff_Blocker_v1.png` SHA256 `2e7a863547688773dbb3d6672623e0958fbab01c8241d2665d8739a0777a231c`, `LL_Field09_Phase2_Fissure_Edge_v1.png` SHA256 `7c892de74b1f3327b37f4fb3ff7673954d64542498b32f6ab76826e6ca53b1d0`. 새 원화에서 개별 원본 투명 PNG2로 추출. 합격3종 수정0.
- ZIP **`Limitless_Field09_Redesign_Phase2_Pilot_v1.zip`** (실제 ChatGPT sandbox 파일, 1,672,537 bytes, 9 entries, SHA256 **`f12b056924665eeb593d084d5a5b73ba67a1a3a335d2bd9f882a547e5b8a7ff6`**). ZIP CRC/testzip PASS, PNG2 decode/RGBA/알파/가이드선/고립픽셀 오프라인 PASS, Manifest/승인3종 해시 PASS. 승인3과 시각비교/오프라인 합성 Preview + Role/Renderer CSV 포함. **실제 Unity 시각 PASS 아님; 외곽 매트/겹침 검증 필요**.
- **GitHub에는 보고서만 커밋**(현재 대화 ZIP 바이너리 GitHub 업로드0/사용자 F: 자동저장0). 보고서 [Field09 Phase2 Pilot v1](../../Field09_Redesign_Phase2_Pilot_v1_ActualArt_Delivery_20261020.md), 최초 문서 Commit `f5cdd9821136b7582558af6af1a08d973d90b3c5`.
- Codex만 격리 Import/실제 GameView·기존 Glow/퀘스트/과열 델타QA; 기존 승인3 QA17/92PASS 재검증0. 대표 미술 승인 전 다음 묶음 제작/공식 Scene 적용/Push/main 병합 금지. 현재 `PHASE2_PNG2_DELIVERED / OFFLINE_QA_PASS / CODEX_UNITY_QA_PENDING / USER_ART_APPROVAL_PENDING / MAIN_UNCHANGED`.



## 2026-10-20 Issue #37 Field09 Cracked Overlay 1종만 보완한 v2 실제 PNG/ZIP 납품 (현재 최신)

- **Codex 격리 QA**: Base Ground Visual PASS, Slab Visual PASS — **둘 다 수정0 / SHA v1 그대로**. 유일 FAIL: Cracked Overlay의 오른쪽 x≈-1.5 밝은 균열 직선 절단.
- 기존 v1 Overlay PNG만 변경. `512×512 RGBA / PPU128 / Pivot 중앙`, Tile x≈128 인접 3개 균열 끝의 Alpha만 3,705픽셀(x100..155) 국소 조정, RGB 전체 원본과 완전 동일. Old SHA256 `56d8e0ea9bf3ca2167227362b87bb219a21d4470f317456f36eb8aba99caedeb`, New SHA256 `0147e6aff58e5304fd71e7474a9c018958a4234ec4c2dc2cf7382a905a7be9b7`.
- 신규 ZIP `Limitless_Field09_Redesign_Sample3_OverlayFix_v2.zip` **3,338,697 bytes / 11 entries / SHA256 `ec831166a753eef179da8fc283faf1fe81d1880941bce10c8d93225269cd418c`**. 실제 Sandbox ZIP/PNG 다운로드로 납품; Manifest/수정픽셀3705행/오프라인 Base 21×15 + Overlay 9×15 전후 및 접합 확대컷/QA/재현 스크립트 포함. ZIP `testzip`=None, 새 PNG 딥 검사 PASS. **Unity 실제시각 재검수 대기**.
- GitHub `docs/art-renewal`에 실제 재현용 Python 소스 `Art_Tools/Field09_Issue37_OverlayFix_v2.py` (Commit `b9dbd5e66524eb249a86e4d7cc421c12656b60dc`) 업로드. Codex는 바울 대표 F: 기존 Sample3 v1 ZIP에서 오버레이 원본1개만 읽기전용으로 꺼내 같은 아트를 자동 재생성·SHA 대조 가능. 사용자 원본·합격2종·main 변경 금지.
- [Issue37 새 v2 아트 납품 보고서](../../Field09_Redesign_Sample3_OverlayFix_v2_Issue37_Delivery_20261020.md), 보고서 commit `a50dcd86abca567906c2c3aada919af53b41bb76`. **다음 담당 Codex**: 동일 x≈-1.5 및 3개 GameView 시각 Delta QA만; 기존 31PASS/0FAIL·Main18 PASS 전체 재검사 하지 않음. 진짜 Unity에서 수직 경계 0FAIL, 바울 대표 미술승인, 별도 공식 적용승인 전 후속 Field09 아트/병합 금지.
- **정확 상태**: `ISSUE37_OVERLAY_V2_PNG_AND_ZIP_DELIVERED / BASE_SLAB_PASS_UNCHANGED / OFFLINE_QA_PASS / CODEX_UNITY_DELTA_QA_PENDING / USER_ART_APPROVAL_PENDING / MAIN_UNCHANGED`.



## 2026-10-20 Field09 새 환경 3종 기술계약 대응 — 실제 신규 PNG3 납품 (최신)

- 기획 정본: docs/planning/문서/00_프로젝트/Field09_Art_Redesign_Sample3_Technical_Contract_20261020.md 직접 확인. 기존 부분 수정 중단, 맵 구조/Collider/Main18/과열/몬스터/세린/전투/Save/Continue 보호.
- PNG3 전부 512×512 RGBA / PPU128 / 중앙 Pivot 제안. Ground Base (0,0) Tiled 21×15 — SHA256 1bd8a614988ee182640a71b9771646c3009545140e0522a3290ee3b0e3138203. Cracked Overlay (-6,0) Tiled 9×15 — SHA256 56d8e0ea9bf3ca2167227362b87bb219a21d4470f317456f36eb8aba99caedeb. Obsidian Slab (5.2,1) 월드 약 2×2 — SHA256 dfa3abb2c6c86abc3f94baa213428038a1c15b4c963e76fef69491a6deb09692.
- 신규 ZIP `Limitless_Field09_Redesign_Sample3_v1.zip`: 4,207,395 bytes / 14 entries / SHA256 4ca6ea65f6315b0e35011a902d520549ba7d944145e6ec0b05962ff945f893b4. 실제 ChatGPT 대화 파일 다운로드용. GitHub 바이너리 업로드나 F: 자동 저장 아님. 원본 공식 PNG/meta/GUID/Scene 및 main 불변.
- PNG 3/3 Decode, 지면 2종 양축 RGBA edge exact equality, ZIP `zipfile.testzip()==None`, ZIP 내 PNG SHA 대조 PASS. 3×3 반복·정확한 WorldSize 위치 모식도 포함. 다만 실제 GameView 아님; 반복 주기성/Overlay 끝단 이종재질 연결/Slab 접지 등 Unity 시각QA 대기.
- 상태: NEW_ART_PNG3_DELIVERED / ZIP_OFFLINE_QA_PASS / USER_ART_REVIEW_PENDING / CODEX_UNITY_VISUAL_QA_PENDING / REMAINING_ENV_HOLD.
- [상세 납품 문서](../../Field09_Redesign_Sample3_v1_Art_Delivery_20261020.md), 최초 문서 커밋 26edd65c25359f208170eaadf45ee9feca3178aa. 바울 대표 미술승인과 Codex 시각 QA 통과 전 나머지 자산 제작·정식 반영 금지.


## 2026-10-20 Field09 부분수정 중단 → 환경 신규제작 3종 샘플 실제 납품 (최신/최우선)

- **바울 대표 최신 결정:** Field09 v1/v2 최소 보완은 중단·역사보존. **맵 구조·Main18 조사/과열/퀘스트·충돌/몬스터/세린/전투/Save/Continue** 전부 보호. 신규 환경 미술만 다시 제작하며 전체 환경 아트 자동 발주가 아니라 **1차 샘플 3종 승인 후 다음 진행**.
- 실제 새 PNG3: `LL_Field09_Ground_Basalt_Base_Sample_v1.png` (**512² RGBA, SHA256 e41b9ee7b4f197dcd77319130b7cfc79f7da38b8f98afdc0c71cc78fa0b78a83**), `LL_Field09_Ground_Fissure_Overlay_Sample_v1.png` (**512² RGBA alpha0~180, SHA256 c29209c89701bf0a93094e22ec7bb07e78a0db507c87c04f37867a3284aebbf2**), `LL_Field09_Obsidian_Spire_Sample_v1.png` (**512² RGBA 투명배경, SHA256 529db3c18f8d15adf7bc1217e88d27838de9e5019ac7734a3530c582ba13e2af**).
- 실제 ZIP `Limitless_Field09_Environment_NewArt_Samples_v1.zip` **4,411,937 bytes / SHA256 af1475a04649b8033a5f8266ef670cdae0e998d0b0429266461f0ae18bdc75e8 / 11 entries** ChatGPT 파일 링크로 납품. PNG Decode3/3, 독립 ZIP testzip CRC PASS, PNG3 SHA 대조 PASS, 지면2종 외곽 좌우·상하 RGBA seam mismatch0. 미리보기/원화→PNG 비교/기존 공식 GitHub PNG와 나란히 보는 온라인 HTML 비교 수록. **실제 Unity 전후캡처·실행 PASS 아님**. 현재 GitHub 브랜치에는 **명세문서와 해시만**, ZIP 바이너리는 대화 첨부 파일이고 바울 대표 F: 자동 저장 아님.
- **주의:** 신규 Cracked Overlay는 투명 RGBA이므로 현재 불투명 Tiled Overlay와 동일 Render 설정임을 보장하지 못함. 기존 공식8·Field08 PNG·meta/GUID·Scene 불변. 실제 3×3 반복에서 큰 암석 무늬의 주기성, 소품 마스크 테두리, Field08 실제 접합/게임크기 화면은 **미술 검토 및 Codex 통합검증 필요**.
- 상태 `NEWART_SAMPLE3_DELIVERED / OFFLINE_FILE_QA_PASS / USER_SAMPLE_STYLE_REVIEW_PENDING / UNITY_NOT_APPLIED / NO_FULL_ART_AUTHORIZATION`.
- [새 3종 샘플 실제 납품·SHA·QA·온라인 비교 보고](../../Field09_Environment_NewArt_3Samples_v1_Delivery_20261020.md), 보고서 Commit `2278f9a95cad1b6b167de1d1fbcd409e4f180c29`. 이전 v1/v2 부분 보완 납품·미검증 기록은 이제 **이전 방식의 역사 기록**, 신규 환경 샘플이 최신 지시를 대체. 사용자 승인 후만 후속 제작 및 Codex Unity 통합.


## 2026-10-20 Field09 v2 실제 8 PNG 수정 ZIP 납품 (최신)

- **Issue #29** 확인 후 최신 공식 `main 12c69c72`의 `Resources/Main18/Environment`에서 **원본 공식 PNG8 바이너리 직접 취득·RGBA 디코드**하여 수정 PNG8 실제 제작. v1 단순 재포장 아님. 공식10 PNG·meta·GUID/원본 Unity 변경0.
- `Art_Deliveries/Limitless_Field09_ObsidianScar_Minimal_Fix_v2.zip` **2,659,837 bytes**, `SHA256 30f102c223aa127a7b6a290ef9fa503354cb9e0104faac03e6c395c2b247b8b8`, **Git blob** `380cc2c0f588f0a6d562e8250cfe223bf229cd9c`. ZIP 납품 commit `4babb98b1c90bbd001dda1cff4be46097d02cfc7`. 업로드 이후 GitHub size/sha 재조회 일치.
- 실제 수정: Ground2 **135085 / 139085px**, 외곽88px 질감 블렌딩·상하좌우 RGBA 반복 mismatch0. Guide6 **3120 / 2457 / 3035 / 2258 / 2297 / 2905px** 원본 공식 안내선 근처 밝은 무채색 경계픽셀 국소 제거. 모든 수정 PNG8 RGBA512x512 재Decode/PixelRoundtrip PASS, 원본PNG CRC PASS. ZIP `Fixed_Candidates/PNG8 + Preview/Before_After_8.png + Manifest + Changed_Pixels.csv + VisualFail_Cases.csv + README + QA` 총14 entries 표준 STORED 작성. **GitHub Connector ZIP readback이 크기 제한으로 빈 값을 반환하여 Python `zipfile.testzip()` 독립 검증은 아직 없음**; Codex 최초 게이트로 지정.
- **중요 구분:** `ART_V2_8PNG_DELIVERED / GITHUB_BLOB_VERIFIED / OFFLINE_PNG_QA_PASS / USER_ART_REVIEW_PENDING / ZIP_INDEPENDENT_TEST_PENDING / UNITY_VISUAL_QA_PENDING / OFFICIAL_UNITY_NOT_APPLIED`. 이전 실제 화면 반복회색띠6·가이드잔여6종 **Runtime에서 해결됐다는 주장 금지**. 같은 카메라 v1 FAIL→v2 Delta QA로 0FAIL 입증 후 대표 미술 승인·별도 공식 Unity 적용.
- [v2 실제 아트 PNG8 납품·해시·픽셀수·Codex 검수 게이트](../../Field09_ObsidianScar_Minimal_Fix_v2_Real_PNG_Delivery_20261020.md), 문서 commit `5b84285d3ef462cab1e7a4fc4a99360d02239d6e`. v1 손상·재포장 및 v1 Unity Visual FAIL 기록 보존; main 직접 병합/Push 없음.


## 2026-10-19 v3 독사·Wolf PNG2 실제 파일 납품, Field09 소스 접근 보류 (최신)

- 독사 `VenomSnake_KeyArt_v3.png` 1254×1254 RGBA / SHA256 `7001d199b279cb8e541fc603bd62351ab460d3b44da91bc91a2d65dc099f8fd0`; 늑대 `Wolf_KeyArt_v3.png` 952×1306 RGBA / SHA256 `30b2d464fe2c456c0fa7d82f1fad51f64065b6e8379ea174548deec7d1b91b4e`. 두 개 모두 현재 세션에서 **실제 별도 PNG 파일로 생성**하고 디코드/투명도/해시 확인; GitHub 바이너리 및 대표 F: 디스크 자동 저장0. 미술 승인, 공식 몬스터5 원본 PNG 동배율 비교, Unity 적용 미완료.
- Field09 v1 재포장/격리 기술 검사 PASS 기록과 실제 화면 아트 FAIL(지면 반복띠6/가이드6종) 구분. 기존 8개 공식 PNG와 원본 스크린샷이 현재 이 세션 컨테이너에 없어서 픽셀 원본 보존을 검증하는 Field09 v2 실제 수정8PNG/ZIP **제작·납품0**. 공식 원본/실물 화면 확보 이후 국소 수정 필요. 재포장만 반복/가짜 합성 비교 금지.
- `KEYART_V3_2PNG_DELIVERED / USER_STYLE_PENDING / REAL_OFFICIAL_COMPARISON_BLOCKED / FIELD09_V2_NOT_DELIVERED / ANIMATIONS_HOLD / UNITY_UNCHANGED`. [상세 작업·한계·SHA](../../MonsterWolf_v3_KeyArt_Actual_PNG_And_Field09_Blocker_20261019.md), 최초 문서 commit `2f38e4096f2ec3228284aba9ad97d1bf7cee93e1`.


## 2026-10-19 Chapter2 품질 게이트 및 잘못된 이미지 비교 정정 (최신)

- 새 기획 정본 `docs/planning/문서/00_프로젝트/Field09_Zip_Header_Repair_And_MonsterWolf_Chapter2_Quality_Gate_20261019.md` 직접 열람. 바울 대표는 **VenomSnake v2·Wolf v2를 미술 품질 보류**하고 Chapter2 공식 몬스터 5종(11~15)의 명암·형태·질감·액션 완성도를 기준으로 재설정. **32×32 확대만 하는 방식 금지**.
- 직전 세션이 생성한 뱀/늑대 2장의 레이아웃형 'Concept Art' 하단 비교는 **공식 5종 원본의 실제 PNG·실게임 렌더 배율 비교가 아님**. 부정확한 비교 이미지이므로 **미술 승인/실게임 사이즈 증거로 사용 금지**. 비교 UI 등 홍보용 장식과 게임 납품용 원화 구분 필수.
- 상태: `V1_REJECTED / V2_USER_QUALITY_HOLD / CHAPTER2_QUALITY_TARGET_CONFIRMED / REAL_COMPARISON_UNVERIFIED / V3_KEYART_REVIEW_PENDING / NO_FULL_ANIMATION / NO_BEAR_FOX / UNITY_UNCHANGED`.
- 다음: 기존 공식 Sprite 5종의 실물 고유 프레임·메타와 PPU·게임 World Size를 확인하고, **뱀/늑대 독립 고해상도 단일 포즈 원화 1장씩**, 정확한 실물 5종과 동일 스케일 비교판 제출. **바울 대표 스타일 승인 전** 새 20/6프레임, 추가 Bear/Fox 그림 제작 금지.
- [상세 정정 기록](../../Monster_Wolf_Chapter2_Quality_Gate_Correction_20261019.md), 문서 commit `3f3002cca53c080bfe7b0f023a3219b5dddb4332`. 기존 손상 Field09 ZIP 재포장은 Codex 별도 기술 작업.


## 2026-10-19 맹독뱀·Wolf v1 반려 → v2 실제 PNG 파일럿 신규 납품 (최신)

- **바울 대표 확정:** `Limitless_Monster_Renewal_Pilot_VenomSnake_v1.zip`은 '뱀이 무섭지 않음', `Limitless_Beast_Companions_Wolf_v1.zip`은 '보노보노에 나오는 강아지처럼 만화적임'으로 **둘 다 v1 반려**. 최종 미술 승인 이력으로 쓰지 않음.
- v2 아트 별도 제작 완료(채팅 ZIP 실제 납품, GitHub 바이너리·대표 PC F: 자동 업로드 아님). **뱀** `Limitless_Monster_Renewal_Pilot_VenomSnake_v2.zip` 82,330 bytes / SHA256 `b4d2c023c312893bb5035620bcfa049131b25b9a471920e4da16c60481ef4e38`, PNG26 / 5동작×4프레임, 시트5장128×32 RGBA. 날카로운 머리·독니·열린 입·S자 긴장/공격성 강화. **Wolf** `Limitless_Beast_Companions_Wolf_v2.zip` 47,659 bytes / SHA256 `cbcb62a766ae044bf4b1ca549eeb16027f91938aab5044eb7d701a574ef106cd`, PNG12 / Run384×40 6프레임12fps / IconFrame04 crop / 40px 확인 이미지. 길고 좁은 주둥이·선 귀·탄탄한 다리·현실적인 판타지 야수 체형 강조.
- 두 ZIP CRC/전체PNG decode/SHA/프레임 시트 대조/프레임 경계Alpha0·프레임 중복0 **오프라인 PASS**. **최종 상태 `V1_REJECTED / V2_ART_DELIVERED / V2_OFFLINE_QA_PASS / USER_V2_ART_APPROVAL_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`**.
- Bear/Fox 신작은 Wolf v2 미술 승인 전 제작 **HOLD**. 몬스터18·펫3 기존 애니메이션/권리/공식 아트/GUID/meta/Scene/Quest/Save/펫패시브 보호. 승인 이후에만 Codex 격리 worktree 신규 Import 및 실제 Runtime/접근성 회귀.
- [v1 반려 사유·v2 실제 자산 규격·납품 SHA·Codex 후속 정본](../../Monster_Beast_Pilot_v1_Rejection_v2_Delivery_20261019.md), 담당 `docs/art-renewal`; 신규 문서 최초 커밋 `213457cd1547254969bd71cbe29bf02a3cb31ca6`. **이전의 v1 납품·미술 승인 대기 표기는 모두 역사 기록**입니다.


## 2026-10-19 Field09 공식 아트 8종 국소 최소 보완 — 실제 ZIP GitHub 납품

- **바울 대표 작업 범위 승인:** 기존 Main18 공식 환경 PNG 8종의 실제 지면 경계/흰 가이드선만 국소 수정. 디자인/역할/원본 경로/색감 보호, 전체 재디자인 금지. `AshPatch_01`·`Ground_Cracked_02` 완전 보호. 이전 Field09 기존 아트 감사의 '0종 확정'은 **이후 사용자 실제 결함 보완 발주 전에 작성한 역사 기록**이며, 현재 이 8종 국소 작업이 승인됨.
- 원본은 `main` `675988c9f2c5c507d9a696b0378be27c0bd3309b`, `Resources/Main18/Environment/ObsidianScar_*.png` 8개 그대로 보존. **별도 ZIP으로만** 픽셀 변경한 8종 후보를 제작하고 `docs/art-renewal` 브랜치에 GitHub 바이너리 납품 **실제 업로드/목록 크기 검증**. ZIP은 바울 대표 로컬 `F:`에 자동 생성·저장한 것이 아님.
- 파일: [`Art_Deliveries/Limitless_Field09_ObsidianScar_Minimal_Fix_v1.zip`](https://github.com/HappyPaul812/Project-Limitless/blob/docs/art-renewal/Art_Deliveries/Limitless_Field09_ObsidianScar_Minimal_Fix_v1.zip); **2,995,422 bytes, 13 entries**(실사용 수정 PNG8 + Manifest + QA표2 + README + Codex HOLD), **SHA256 `7dfa2417a8135e29e3e72c7d7921d3ade76fbedfdb376f764c09b02892296139`**, GitBlob `34bce32d0d91b9ef8167308383355590d3834f9f`, 최초 바이너리 납품 commit `434f506ff20b9b6cf99ab9937616a76803467ae0`.
- **실제 변경 범위:** Ground Base **21,919px** / Ground Cracked01 **22,859px**, 모든 변경이 12px 외곽 대역 안이며 RGBA 좌우·상하 seam mismatch**0**. Cliff Blocker **2,363px**, Fissure Edge **895px**, Fissure Glow **2,785px**, HeatVent **873px**, Obsidian Slab **2,030px**, Obsidian Spire **1,169px** — 기준 원본의 밝은 무채색 낮은 alpha 가이드계열 바깥 프레임 픽셀만 선별 제거. 원본/후보 각 SHA·바이트·BBox `QA/Original_Source_SHA256_and_Delta.csv` 및 `Manifest.json`에 모두 기록.
- 오프라인 QA: 원본 각 PNG와 수정 PNG8 RGBA512×512 decode 및 픽셀 roundtrip **8 PASS**, 지면2종 edge0·중심부 변경0, 가이드6종 alpha 제거 픽셀 각각 존재 확인. ZIP 각 Entry local/central CRC를 작성하여 업로드한 GitHub Blob SHA/크기 확인. **GitHub 바이너리 커넥터는 >1MB ZIP 본문 다운로드를 비워 돌려주므로 이번 세션에서 ZIP를 Python zipfile로 별도 재다운로드 CRC 실측한 것은 아니며**, Codex가 추후 바이너리를 직접 받아 `testzip`/Manifest 재확인 필요.
- **품질 보류:** 지면의 `GroundCrackedWest` Tiled Overlay와 다른 Ground Material 간 **이종 접합부가 완벽한지 아직 실제 Unity 1280/1600/1920 GameView 미검증**. 가이드 자동 삭제가 Cliff 실루엣 일부 픽셀과 겹치는지 실제 장면 확인 필요. 본 패키지는 아트 최소 수리 **후보**이고 바울 대표 최종 수정본 미술 승인과 공식 Unity 적용/Runtime PASS와 다름.
- 상태 `DELTA_ART8_DELIVERED / OFFLINE_QA_PASS / USER_DELTA_VISUAL_REVIEW_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`. Codex 작업은 바울 대표 승인 후에만 **별도 `codex/*` worktree**에서 기본 원본SHA 대조→Importer GUID/meta·Scene Sprite 참조 보호→지면 접합/기존 가이드선 흔적·Collider/Quest/과열/세린/Save 회귀. `main` 직접 Commit/Push/병합 금지.
- [Field09 정식 납품/기술 QA 문서](../../Field09_ObsidianScar_OfficialArt_MinimalFix_v1_Delivery_20261019.md), 최초 문서 commit `935c53686e5a464e483221f36480f0bd958f5e8f`.

## 2026-10-19 환경 신규제작 일시 마감 / 몬스터18·기본 펫3 감사 및 두 파일럿 납품

- 바울 대표 최신 지시: 환경 지도 **신규 아트 제작 일시 마감**, 미해결 환경 기술/승인 게이트는 역사대로 독립 유지. Chapter1·2 **등록 몬스터18 + 기본 펫 Wolf/Bear/Fox3** 소스 감사 및 **非공식 몬스터1 + Wolf1** 오리지널 파일럿 신규 PNG 제작 승인. Phase2 Bear/Fox 그림은 Wolf 미술 승인 전 HOLD. 기준 `main` **`675988c9f2c5c507d9a696b0378be27c0bd3309b`**, 기획 `docs/planning/문서/00_프로젝트/Chapter1_2_Monsters_And_Beasts_Original_Art_Renewal_Assignment_20261019.md`.
- 실제 최신 `Resources/MonsterDefinitions` 01~18 **18개 YAML 전수 검토**. `11~15` Chapter2 보호된 공식 몬스터5종, `18` Main20 공식 보스 v3, `16~17` Main18 등록 시트(최종 사람 미술 승인 필요)는 **무단 재제작 금지**. `05~10`도 이미 개별 전용 시트·프레임(보스 포함)이 있어 저작권·승인 근거 확인 전 KEEP/HOLD. 기존 ThirdParty 후보 `02 venom_bee`(2D!PIXX CC BY 표기3.0 vs 원본4.0 확인 필요), `03 forest_spider`(ethanf30games CC0), **`04 venom_snake`(XenosNS CC0)** 중 실제 32×32/4프레임/8fps 안정 contract 가진 `venom_snake`를 첫 신작 대상으로 선정.
- **A 몬스터 파일럿 실제 ZIP:** `Limitless_Monster_Renewal_Pilot_VenomSnake_v1.zip` **81,582 bytes / 35파일 / PNG26개**, SHA256 **`8eb8d512b1ec1249f18f4770616e988acbee449dfc74a63678eb5916fb37d303`**. 새로운 독청록 뱀 실루엣 **Idle/Walk/Attack/Hit/Defeat 각4 = 20개 32×32 프레임** → 128×32 시트5 + 개별 PNG20 + Contact PNG1 + 애니메이션 GIF. `Monster18_Source_Rights_Audit.csv` 18행, `Source_Mapping.csv`, `Animation_Contract.csv`, Manifest SHA, QA/권리/통합 인계 포함. Idle/Attack 기존 C# sheet slicing과 호환; Walk/Hit/Defeat는 신규 Sprite12개를 승인 후 Codex가 Explicit 프레임 배열에 별도 연결해야 실제 실행됨. 기존 CC0 원본/몬스터ID/독/AI/Quest/Save 변경0. 상세 [VenomSnake 납품·18종 감사](../../Monster_VenomSnake_Original_Art_Pilot_v1_Delivery_20261019.md), 최초 기록 커밋 `713da1bfc92daf5056608b0e017ea77d38b90187`.
- **B Wolf 펫 파일럿 실제 ZIP:** `Limitless_Beast_Companions_Wolf_v1.zip` **55,181 bytes / 20파일 / PNG10개**, SHA256 **`d6c490df6a458c95ad55e2f0eb1b902c4db5768aee529184a335545b00bdb56b`**. 새 픽셀 Wolf LEFT Run 시트 **384×40 = 6×(64×40), 12FPS**, 6개 Frame PNG 및 기존 아이콘 `IconFrameIndex4` 동일 픽셀 Crop, ContactSheet/40px Preview/ GIF. 현재 검증용 ScratchIO CC0 원본 `Resources/CompanionAssaultValidation/Wolf_Run.png` **실제 코드 실사용**; 옛 README의 미연결 표기는 낡았음. 신규 코드를 자동 변경하지 않고, 미술 승인 후 Codex만 새 Resources 경로+RunResourcePath 교체. 현재 기본 Wolf Direct+5%, Bear HP+10%, Fox AGI+2, Beast 슬롯0/턴0/HP0, 세린 기본 Fox·비사수 주인공+세린 동행 시 펫 접근·몬스터펫100Talent 규칙 보호. 상세 [Wolf 납품·3종 계약](../../Beast_Wolf_Original_Run_Pilot_v1_Delivery_20261019.md), 최초 기록 커밋 `9ca1628cd46005ffd087dde14e48914a37c1c70c`.
- **Bear/Fox 미술 제작0, 규격만 확인:** 현재 Bear **320×33 = 5×(64×33)**, Fox **384×36 = 6×(64×36)**, 둘 다 12fps/LEFT/하단중앙 Pivot, Catalog 기존 IconFrameIndex0. Wolf 프레임 수를 억지로 강제할 수 없음. Wolf 미술 승인 후 2종 새 실루엣 별도 발주·제작.
- **오프라인 QA:** A ZIP CRC PASS, PNG26 decode/Manifest SHA PASS, 20개 프레임 비어있음0, 사방 경계 비투명0, 동일프레임 중복0, 분리 PNG 시트와 byte일치. B ZIP CRC PASS, PNG10 Decode/Manifest SHA PASS, Run6·Icon index4 픽셀 정확·프레임 edge0·반복 유니크6. 기존 ThirdParty PNG 복제/수정0. **두 파일럿 모두 `USER_ART_APPROVAL_PENDING / UNITY_NOT_APPLIED / UNITY_RUNTIME_NOT_VERIFIED`.**
- 새로운 png는 ChatGPT 다운로드 ZIP로 제공됨; 바울 대표 로컬 `F:`나 GitHub 본체 자산에 직접 저장한 것 **아님**. 몬스터 공식 승인/기존 TTS/Unity Scene·Animator·GUI/Quest/Save/아이템/Gold/GUID/meta 등 무단 변경0. GitHub `docs/art-renewal` 문서 커밋만; `main` 직접 Commit/Push·병합·강제 Push 없음.
- **다음 게이트:** 바울 대표가 ① VenomSnake 미술 시안 5동작 ② Wolf Run·40px 아이콘을 **각각 승인/수정 지시** → Codex가 이후 별도 `codex/*` worktree에서 신규 Import·Sprite GUID·프레임 배열 및 사수 '동료의 습격'·펫 시설/세린/Fox·MonsterPet·Save/16:9·4:3 QA → 필요 범위만 승인 후 반영. Bear/Fox 제작은 Wolf 파일럿 승인 이후. 현재 미술 승인도 Runtime 승인도 없음.

## 2026-10-19 Field08 공식 아트 유지 종료 및 Field09 ObsidianScar 기존 아트 감사

- **바울 대표 확정:** `Field_08_RedRift` 공식 Main17 환경 아트를 **그대로 유지하고 추가 PNG/ZIP 제작 생략 승인**. **상태 `CLOSED_EXISTING_OFFICIAL_ART_PROTECTED`**. Field07 실제 적용 후 Field07↔08 색감·경계 문제 *입증 시* 별도 최소 보완 제안만 허용; 승인 자산 무단 교체 금지. 기존 Field08 감사 [정본](../../Field08_RedRift_ExistingOfficialArt_Audit_20261019.md) 유지.
- 다음 `Field_09_ObsidianScar`은 최신 `main` **`675988c9f2c5c507d9a696b0378be27c0bd3309b`** 기준 읽기 전용 조사. 실제 Field09 Scene `f02ef9f2eccba4a32cc9afb7e819a3f9536816fc` **GameObject18, SpriteRenderer9(모두 표시), MeshRenderer1(텍스트), BoxCollider2D6**. Sprite9는 전부 Main18 전용 `Resources/Main18/Environment` 등록 PNG와 **GUID/fileID21300000 일치**, 따라서 활성 무료/임시 환경 Sprite 없음. 공식 환경 PNG 총10, 사용9, `ObsidianScar_Ground_Cracked_02.png` 1개 등록·미사용.
- 현행 지면 구조 `ObsidianScar_Ground_Base.png` **Tiled 21×15**, 위에 `ObsidianScar_Ground_Cracked_01.png` **Tiled 9×15, 위치(-6,0), Alpha255 불투명**. 기존 Main18 기술 QA는 `PASS_WITH_NOTE17/FAIL_BLOCKING0`, **`USER_ART_REVIEW_REQUIRED17`**. *미술 승인 완료라고 절대 해석 금지*.
- Main18 `main_18_black_heat` 목표13, 동쪽 Field08 왕복·서쪽 폐쇄 안내와 Field10 미래 연결 정의, 과열0~2(3차 중첩 HP8% 피해 후0), 냉각약/몬스터2/세린·접근성·Save 보호. 이전 `Main18_Final_QA` **Runtime 고유500PASS/0FAIL**은 과거 Codex 검수로 재사용하며 이 세션은 실행0.
- **확정 교체 필수 PNG 0종.** 단 조건부 가시성 검수 항목: (1) **Ground_Base + Ground_Cracked_01**의 반복·서쪽 오버레이 경계, (2) Main18 아트 QA에서 원본 흰 가이드 형태 선/테두리 문제가 기록된 **Cliff_Blocker/Fissure_Edge/Fissure_Glow/HeatVent/Obsidian_Slab/Obsidian_Spire 6종**. 실제 HUD 포함 화면에서 가이드·seam이 식별되고 Import/Tint/기존 배치로 해결 불가능한 개별 범위만 **바울 대표 별도 미술 발주 후** 픽셀 최소 보완 검토. `AshPatch` 및 미사용 `Cracked02` 임의 재제작·등록·활성화 금지.
- 원본 QA의 1280/1600/1920 캡처 파일 **기록 경로만 확인**, GitHub 바이너리 픽셀 직접 열람·새 Unity RenderTexture 캡처 검증은 하지 않았음. 결함을 실제 현재 게임에서 확인했다는 표현 금지. `main` Scene·.meta/GUID·Art·Main18/Heat/Quest/Collider/Save/LocalDirty 및 PNG/ZIP 변경0.
- **상태 `FIELD09_SOURCE_AUDIT_DONE / CONFIRMED_MUST_REPLACE_ENV_PNG_0 / VISUAL_DEFECT_REVIEW_REQUIRED / NO_NEW_PNG_AUTHORIZED / UNITY_UNCHANGED`**. [Field09 상세 감사 및 최소 보완 게이트](../../Field09_ObsidianScar_ExistingArt_MinimalReplacement_Audit_20261019.md), 문서 첫 기록 커밋 **`48f3a44ddba08603fe5741f427dc6b93078ae450`**. 브랜치 `docs/art-renewal`, `main` 직접 Commit·Push·병합 없음.
- **다음:** 바울 대표 필요시 Field09 실제 게임 화면 국소 검수 범위 확정 → 결함 확정된 PNG만 개별 아트 보완 범위 승인 → 그 뒤 실제 제작. 보완 승인 전 신규 PNG 생성 금지. Field10 이후는 별도 지시 필요.

## 2026-10-19 Field08 RedRift 기존 공식 아트 감사 — 새 PNG 제작 확정 0종

- **사용자 최신 결정:** Field06 v1.1 및 Field07 v1의 **미술 승인 완료**. 이전 섹션의 이들 `USER_ART_REVIEW_PENDING`은 제작 당시 상태 이력. 다만 현재 `main`의 원본 Unity/Runtime 공식 적용 완료까지 포함하는 승인은 아니므로 별도 적용·검증 여부는 각각 최신 Codex 작업으로 확인.
- 기준 `main` `675988c9f2c5c507d9a696b0378be27c0bd3309b`, GitHub `Field_08_RedRift.unity` blob `6100dde3685e26c85b06fd2d4f06995fb6d582ee` 원문 YAML 전체 직접 감사. 저장 GameObject **428 / SpriteRenderer375(표시7·비표시368) / MeshRenderer45(표시30·비표시15)**. 표시 중 **37 Renderer는 공식 Main17 환경 PNG와 연결**되어 있음.
- 표시되는 7 SpriteRenderer: Obsidian `RockWest`/`RockNorth` 2, CanyonLip2, DeadShrub3. 표시되는 Mesh30: Ground1, DeepCrack1, Glow1, Ash24, Obsidian3. 기존 Grass/AshSoil315, 남북 Road30, BrokenTrack10, Tree5는 GameObject 비활성; AshCrack8 및 옛 Branch6/Track8은 Renderer.enabled=0. **비표시 임시 Sprite를 전부 신규 아트 작업으로 오인하지 않는다.**
- 정본 원본 아트 `_Project/Art/Environment/Chapter2/RedRift/` **PNG9개 등록/7종 실제 적용/CrackedGround2종 의도적 미사용**. 원본 1254px, 기등록 meta/GUID·Materials `Ground/Fissure/Glow/Ash/Obsidian` 보호. 원본 ZIP SHA `a238bc2eca0450b95fb5a61cd8cc655018bff82e54096fa91cfb2f7f424e19fb`은 기존 공식 감사 보고 값. 로컬 ZIP 직접 열람하지 않음.
- `Main17_Field08_Art_Applied.md`, `Main17_ArtAudio_QA.md` 등 기존 **270 고유 PASS/0FAIL**, DeepCrack 충돌과 북남 우회, 3해상도 HUD 포함 가독성·seam PASS를 재사용 **(새 QA로 재실행하지 않음)**. 원본 Material/Sprite 연결 현재 YAML은 직접 확인.
- Field07 승인 v1은 **최신 main Field07 Scene blob이 과거 `2f267575bfbd1d7802b58c8653c2b33d7a84d6ac`와 동일**하여, 승인만으로 실제 렌더 적용 완료 단정 금지. 조건부 후보 1개: **Field07 서쪽(-8,0) ↔ Field08 동쪽(+7,0) 왕복 진입 색감/타일 경계**를 승인된 Field07이 실제 설치된 뒤 Codex가 동일 카메라로 재검토. 눈에 띄는 결함이 *입증되는 경우에만* 새 경계 Decal 또는 Material Tint를 사용자 별도 미술 범위 승인 뒤 검토. 현재 신규 PNG 정당화 증거0.
- Field08 `Field08_WestToField09.asset` 전환 정의도 존재하므로 Main17 당시 서쪽 차단 설명으로 현행 연결을 임의 삭제/신설하면 안 됨. Quest/Player/세린/Collider/Spawn/Save/Official Art 보호.
- **최종 상태:** `EXISTING_OFFICIAL_ART_AUDIT_COMPLETE / CONFIRMED_NEW_PNG_REQUIRED_0 / CONDITIONAL_VISUAL_BRIDGE_CHECK_PENDING / NO_NEW_ASSET_AUTHORIZED / UNITY_UNCHANGED`. 제작 PNG 0, ZIP 0, GitHub 문서+인계만 담당 `docs/art-renewal` 커밋. 최신 상세 [Field08 공식 아트 감사 정본](../../Field08_RedRift_ExistingOfficialArt_Audit_20261019.md), 최초 감사 커밋 `f3315356f9d8ba54e192fa8acdd4cdedc8d9d3d6`.
- 다음: 바울 대표 감사 결론 승인 → 필요 시 Field07/08 동일 카메라 격리 연결 화면만 확인 → 실제 문제와 별도 범위 승인 전까지 **공식 PNG 재제작·덮어쓰기 및 Field08 신규 제작 금지**. 다음 Field09 오리지널화도 별도 사용자 기획 발주 후 진행.

## 2026-10-19 Field06 v1.1 기술 보완 및 Field07 v1 신규 아트 — 두 ZIP 납품

**최신 정본 우선 (2026-10-19 오후 기준):** 기획 `docs/planning`의 `Field06_Art_v1_UserVisualApproval_TechnicalFix_20261019.md`, `Chapter2_Field07_AshenReach_Renewal_Art_Assignment_20261019.md`에 의해 **Field06 v1 미술 방향은 바울 대표 승인**, 단 **기술 수정/Unity 정식 적용/PR·main 병합은 미승인·미완료**. 기존 역사적 Field06 `USER_ART_REVIEW_PENDING`을 오늘 현재 상태로 오인하지 않을 것. Field04/Arbel/Field05는 승인 적용 정본 유지.

### 작업 A — Field06 v1.1 기술 최소 보완

- 제작 ZIP: `Limitless_Field06_ScorchedTrail_Renewal_Art_v1_1.zip`, **1,399,079 bytes / 41파일 / PNG29(실사용22 + Preview/QA7)**, SHA256 **`d8551df7315495dcc6e4556ec33f30b711af8aeb85dc7b0a1c3612a5a6197f42`**. v1 ZIP `d01f797dd5566c33c6bf917a397c2032f5fc44fcf31a53dc328807e912a23b70` 보호·무변경.
- v1 실사용 PNG22 중 **Road Upper/Lower/WestEnd Upper/WestEnd Lower 4개만 v1.1 기술 수정**. 기존 그림의 상하 위상과 실제 y=+0.5/-0.5 행이 반대로 읽히는 배치 오류를 교정하고, 중앙 경계 Alpha0 자리에 불투명한 동일 RGBA 연결 한 줄 부여. 기존 Ground3/Props7/선택 Runtime6/Decal2 **실사용 PNG18개 모두 v1 SHA/바이트 동일**. 새 Road4는 128×128 RGBA PPU128 Center/Point NoMip.
- `Road_EW_Placement_Proposals.csv` A30/B42 72행은 새로운 v1.1 파일명으로 갱신했으나 **A/B 사용자 최종 결정 없음**. 기존 `Renderer_Mapping_363.csv` 원본 source/coord 보존, **Road30은 `LAYOUT_GATE_REQUIRES_A30_OR_B42` 명시**. 원본 Scene을 Sprite-only 교체로 처리하지 말 것.
- QA: 중앙 불투명 연결 128px(직선 Upper/Lower), WestEnd 동일 중앙 결합, 직선 반복·End→직선·UpperLower **6조건 seam RGBA mismatch0**; ZIP CRC/PNG29 해독/Manifest22 SHA PASS. Props 7 새 후보 Alpha 접지 수치표만 제공하고 기존 소품 PNG는 전혀 수정하지 않음. Codex가 원본 Sprite.rect/본체 Alpha/Collider 차이를 실제 측정한 후 개별 문제 확정 필요.
- **Runtime Grass 49개 비활성화 및 Unity Sorting/실제 빈틈 문제는 Codex 영역.** 717 PASS/0 FAIL은 Codex v1 이전 격리 QA이므로 v1.1 재검증으로 승격 금지. 현재 `USER_V1_VISUAL_DIRECTION_APPROVED / ART_V1_1_OFFLINE_QA_PASS / ROAD_A_B_UNDECIDED / OFFICIAL_UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`.
- [납품·기술 QA 정본](../../Field06_ScorchedTrail_Renewal_Art_v1_1_TechFix_Delivery_20261019.md), 문서 첫 커밋 `c189e8a784d33c979b58b4800e5993740ca53e86`.

### 작업 B — Field07 AshenReach v1 오리지널 신규 미술

- 실제 GitHub `Field_07_AshenReach.unity` blob `2f267575bfbd1d7802b58c8653c2b33d7a84d6ac`: **정적 SpriteRenderer372 = AshSoil315/Road30/AshCrack8/BrokenTrack10/CanyonLip2/Tree5/Rock2**, 기존 Road30은 남북형. Runtime `Chapter2Main16Flow.CreateAshMarks()`는 첫 `AshSoil_` Sprite를 선택해 **WindlessAsh7개** 생성하므로 새 Ground 적용 시 재 입자의 불투명 사각형 표현·가독성 변화를 꼭 Codex가 별도 테스트해야 함.
- v1 ZIP **`Limitless_Field07_AshenReach_Renewal_Art_v1.zip`**, **477,759 bytes, 27파일, PNG18(실사용15+Preview/QA3)**, SHA256 **`5d398295b6f98491a294628a18763b6d72e0dedfdf5f691ccd2e8d0539150403`**. 새 후보 PNG15=Ground3/Road4/Props5/Decal2/별도 Runtime 선택1. 본체/세린/불씨망령/공식 자산·이전 Field 아트 재사용/덮어쓰기0.
- `Renderer_Mapping_372.csv` 이름 고유372·GUID/fileID/XY/Sorting/Flip/target, `Road_EW_A_B_Proposals.csv` A30/B42 후보72행, `Runtime_WindlessAsh_7_Audit.csv`, Manifest15 SHA 및 Alpha footprint·PPU/Pivot·QA, Contact Sheet·실제 PNG 합성 Preview(실 Unity 캡처 아님) 포함. **오프라인 Ground18 + Road6 =24 seam RGBA mismatch0**, ZIP CRC/PNG18 Decode/SHA PASS.
- Main16 12목표/세린(4,-2), 조사·목격(7,0)(4,0)(1,-2)(-1,1)(-3,0)(-3,-1)(-8,0), Path Hearing/공식 Ember Wraith·아르벨왕복/Save/도보·휠체어 모두 보호, **실제 Unity 검증 전**.
- **상태:** `ART_DELIVERED / OFFLINE_QA_PASS / USER_ART_REVIEW_PENDING / ROAD_LAYOUT_UNDECIDED / RUNTIME_ASH7_HOLD / UNITY_NOT_APPLIED`; A30/B42 사용자 승인 전 Scene 이동/추가 Renderer12 생성 금지. Runtime WindlessAsh 선택1도 별도 코드 승인 전 자동 적용0.
- [Field07 납품·검수 정본](../../Field07_AshenReach_Renewal_Art_v1_Delivery_20261019.md), 문서 첫 커밋 `670173ee19d422992086f4dd11644ad4b1b32136`.

**둘 다** 이 ChatGPT 세션에서 생성해 다운로드로 전달. 바울 대표 PC F: 드라이브에 자동 저장했다고 말하지 않음. GitHub 문서 커밋은 `docs/art-renewal` 전용이며 main/Unity 원본/GUID/meta/Save/사용자 미커밋/Monsters/Chapter3 변경 없음. 다음: Field06은 Codex 소규모 Delta QA, Field07은 바울 대표 시안 미술 승인 후 Codex 격리 통합. Field07 미술 검토 전에 Field08 공식 자산 중복 제작 착수 금지.

## 2026-10-19 Field06 ScorchedTrail 환경 리뉴얼 v1 제작·ZIP 납품

- **최신 확인 기준:** `main` `675988c9f2c5c507d9a696b0378be27c0bd3309b`; 기획 발주 `docs/planning/문서/00_프로젝트/Chapter2_Field06_ScorchedTrail_Renewal_Art_Assignment_20261019.md`. 현행 Field04 승인42길/Arbel 공식23종/Field05 공식 B42는 모두 **기존 공식 적용 자산**, 재작업 금지. 이전 본 인계에서 Field05 A/B 미선정이라고 쓴 부분은 2026-10-19 그때의 **역사 기록**이며 최신 main Field05 B42 공식 적용 상태가 우선.
- 실제 GitHub Field06 Scene(원본 blob `01c31f42fb1ff477f496193f14ca9c1e349ad7fc`) **static SpriteRenderer363 = Grass315 + Road30 + Props18** 분석. 21×15 지면/원본 Ground Source ID 104·105·106개, Road 기존 15행×2열 **남북형**. `Chapter2Main15Flow.BuildEnvironment`는 **Runtime MeshRenderer Quad6** 추가하며 x<2 Grass tint/x<-3 짝수 Y 비활성화 처리 → 정적 렌더러와 분리. Main15 입장 (7,0), 조사 5곳·세린(-6,1), Encounter4 원본 위치 및 Quest12·기능 보호.
- 실제 제작 다운로드 **`Limitless_Field06_ScorchedTrail_Renewal_Art_v1.zip`**, SHA256 **`d01f797dd5566c33c6bf917a397c2032f5fc44fcf31a53dc328807e912a23b70`**, 크기 **1,362,496 bytes**, **38파일/PNG28**(새 실사용 후보22 + Preview/QA6). **새 PNG22종**: 정적 후보 **14**(Ground3/Road4/Props7), 별도 사용자 개별 승인 선택 **8**(Main15 Runtime Quad6용 후보·새 Decal2). 기존 Field04/Arbel/Field05/세린/몬스터 Sprite 복사·재제작·덮어쓰기0.
- **A30 / B42 동서 Road 배치는 둘 다 제안만**, 기존 남북 Road30의 위치 이동이 필요하므로 **Sprite-only 적용 금지**. A30 기존 30위치 재배치 x=-4..10/y±0.5, B42는 12 Renderer 추가 x=-10..10/y±0.5; 서쪽 흙길 끝은 **출입구·Portal 아님**. `Road_EW_Placement_Proposals.csv` 72행 제공, 어떤 안도 확정 선택하지 않음.
- 오프라인 QA: 대응표 static 363행·고유 이름363, Ground3× 양방향 + Road 중앙/반복/WestEnd/Alpha **27조건 RGBA 불일치0**, ZIP CRC PASS, PNG28 해독 PASS, Manifest 신규 PNG22 SHA PASS. 실물 Unity Import·Runtime/도보·휠체어/Monster/Quest15/Save·Main15 Runtime Grass 빈틈·원본 Sprite foot/Cam QA는 **NOT_VERIFIED**.
- 출처/권리 및 Codex 읽기전용 감사 프롬프트, `Runtime_Patch_Audit_6.csv`, `Renderer_Mapping_363.csv`, ContactSheet/Scene좌표 합성 비교 Preview 제공. 사용자 F드라이브·Unity 원본·Git main 수정0, Codex는 Main09 음성작업 우선이므로 **실제 Codex 조사 수행을 주장하지 않음**.
- [Field06 v1 납품 정본](../../Field06_ScorchedTrail_Renewal_Art_v1_Delivery_20261019.md), 최초 docs commit `532c8c0af196192a0e9caa051afff4fcc817c5ff`, 작업 브랜치 `docs/art-renewal`.
- **현재 상태 `ART_DELIVERED / OFFLINE_QA_PASS / USER_ART_REVIEW_PENDING / UNITY_NOT_APPLIED / UNITY_RUNTIME_NOT_VERIFIED`**. 다음: 바울 대표 실 PNG/미리보기 미술 승인 → Road A/B·선택8 개별 여부 확정 → Codex 격리 Scene·접근성·Main15·Arbel왕복 QA → 별도 승인 후 Unity 적용. 다음 차례 Field07 환경은 Field06 미술 방향 확인 후 착수, Chapter3 아트는 `docs/art-new`.

## 2026-10-19 Field05 동서 Road v2 최소 아트 보완 납품 — A/B 미선정

- 발주: [`docs/planning`의 동서 진행로 정본](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/Chapter2_Field05_EastWestRoad_VisualCorrection_20261019.md), 기획 SHA `b614a5c6d6a1e51316c0ed22e03cc34b517982c5` 및 사용자 전달 Codex 실제 Unity6000.6.5f1 격리 비교/PNG 인계. 리뉴얼 담당은 Codex 로컬 `F:/.../Road_PNG_Handoff.md`를 직접 읽었다고 주장하지 않고 전달받은 전문으로 작업.
- **신규 최소 Road4종 실제 제작**: `LL_C2_F05_Road_EW_Upper_128_v2.png`, `...Lower_128_v2.png`, `...WestEnd_Upper_128_v2.png`, `...WestEnd_Lower_128_v2.png`. PPU128/Center/128×128 RGBA, Z0/Flipfalse. 북·남 외곽 투명 흙 경계/서쪽 끝 흙으로 자연 소멸. 새 서쪽 Exit/Portal/화살표 아트0.
- 채팅 다운로드 실제 ZIP **`Limitless_Field05_EastWest_Road_v2.zip`**, 589,850 bytes, 14파일/PNG8(실사용4+합성Preview4). **SHA256 `3a0d3b91cd87a5362ebbe435c1e5acd06ffc74354eafaa41f02b4f181d27c3d3`**. v1 ZIP은 **`a6646a676a619bc149798fe0f3909401b9c3b5a75f253cf6a1e61e21661b1258`** 그대로 보존, Ground3/Props7/선택7·Field04/Arbel 승인 파일 재제작0.
- 오프라인 RGBA 연결 seam 검증: 직선 Upper/Lower 좌우 반복, Upper-Lower, End→직선, End Upper-Lower 각 mismatch **0**, 북·남/서쪽 외곽 Alpha0. PNG4 hash/Decode/ZIP CRC PASS. **실제 Unity RenderTexture/접근성/물리/Save·Quest14/몬스터/Arbel 귀환 Runtime 미검증.**
- 두 배치 모두 별도 승인 대기: **A 30기존 Renderer(x=-4..10,y±0.5)** / **B42(신규12추가, x=-10..10,y±0.5)**. 두 경우 서쪽 최소 두 타일은 Endcap. B 끝은 지도 경계이지 새 서쪽 출구 아님. `Road_Placement_A_B.csv` 72행 및 Codex 적용 인계 ZIP 포함.
- **현재 `ART_DELIVERED / OFFLINE_SEAM_QA_PASS / A_B_UNDECIDED / USER_FINAL_ART_APPROVAL_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`.** Codex A/B 기능 검수는 사용자 인계 시 아직 진행 중. 여기서 A/B 최종 선택·원본 Scene 반영·새 GUID/Renderer 추가 승인으로 해석하지 않음.
- 다음: 바울 대표 4개 PNG 및 반복/WestEnd 미술 검토 + A/B 최종 선택 → 승인안만 Codex 격리 worktree 실제 Unity Road·가독성·휠체어·Main14/Save 검증 → 별도 사용자 정식 적용 승인. 기존 Field03 Rock B·선택 소품7·Field06 범위는 독립 보류.
- [Field05 Road v2 납품 문서](../../Field05_EastWest_Road_v2_Art_Delivery_20261019.md), 최초 기록 SHA `8463ef25be012a792ab67c29fb277647e34ad771`, 담당 `docs/art-renewal`. `main` 직접 Commit/Push/병합 금지.

## 2026-10-19 최신 main 우선 갱신 및 Field05 v1 아트 납품

**상태 기준시점 2026-10-19 / 기준 `main` `c263d9e36fc0570b16130d9c011abf3965def5f7`.** 현재 `main`에서는 **Field04 v2 공식 42도로**와 **Arbel v3 기본23종·건물 Depth/가림 보완**이 승인·적용 및 검증된 상태입니다. 이 문서 10월13~14일의 `USER_ART_REVIEW_PENDING` / `UNITY_NOT_APPLIED`는 **그 당시의 역사 기록**으로만 읽으세요. Arbel 선택 장식11개는 여전히 미배치, 기존 Portal/나무 겹침 OPEN. Field03 Rock B 미술 승인은 Field05 범위와 무관하게 별도 유지.

### Field05 실제 납품 (리뉴얼 담당)

- 최신 기획 `docs/planning` 발주서 `Chapter2_Field05_Renewal_Art_Assignment_20261019.md` 조회 후 Source Audit. GitHub `Field_05_WesternOutskirts.unity` **직렬화 SpriteRenderer363 = Grass315 + Road30 + Props18**, 실제 좌표·GUID/local fileID 대조. Grass 원본 Sprite 분포 104/105/106참조, 기존 Road는 남북 계단형; 포지션 이동0.
- 메인14 보호좌표: 메마른 수로 **(5,2)**, 감시초소 **(-2,2)**, 강한 진동 **(-6,-2)**, 세린 **(1,0)**. 몬스터 두 종 각각 **(3,-3)** / **(-4,3)**, 리스폰35초, 동쪽 Arbel Spawn **(8,0)** / Trigger **(10.7,0)**.
- 실제 제작 `Limitless_Field05_Renewal_Art_v1.zip` **3,275,850 bytes / 35파일 / PNG26**. PNG **19종 신규 환경 후보** = 기존 Renderer에 바로 대응하는 12종(Ground3/Road2/Props7) + 현재 Scene에 없는 **별도 승인 필요 선택7종**(수로·낡은 감시초소·진동 등). Preview/QA PNG 7. ZIP SHA256 **`a6646a676a619bc149798fe0f3909401b9c3b5a75f253cf6a1e61e21661b1258`**.
- `Renderer_Mapping_363.csv` 363/363 (GameObject 이름·XY·원본 GUID/fileID·Sorting·신품경로), `Manifest.json` PNG19 SHA, `QA/Alpha_Grounding_Audit.csv`, `QA/Offline_Validation_v1.json`, `Codex_ReadOnly_Source_Audit_Prompt.md`, `README.md`, `Rights_and_Provenance.md`, ContactSheet 및 비교(기존 Scene 역할 **모식도**, 실제 Unity 캡처 아님) 포함.
- 오프라인 ZIP CRC PASS, 전체 PNG26 decode, PNG19 Manifest SHA 일치, Ground3·Road2 좌우/상하 최외곽 RGBA seam 불일치0. **Original Sprite alpha foot, new Unity Import, 실제 가독성·휠체어·Monster/Quest14/Save·Runtime는 NOT_VERIFIED**. 특히 RockNorth의 기존 Sprite 85×54/PPU100 Pivot/접지는 Codex 실물 검수 필수.
- 이 ChatGPT 세션의 다운로드 링크로 제공하며 바울 대표 F드라이브 직접 기록/Unity 적용은 하지 않았습니다. Field04·Arbel 승인 PNG 재작업0, 정식 Scene·Save·기존 GUID/meta·NPC/몬스터·TTS/BGM·ThirdParty 원본수정0.
- **현재 상태:** `ART_DELIVERED / OFFLINE_QA_PASS / USER_ART_REVIEW_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`. 다음: 바울 대표 전체 Scene 합성 및 개별 PNG 미술 승인(직접교체12/선택7 범위 별도 결정) → Codex 읽기 전용 실물 대조 → 승인 후 feature worktree에서 Import/Scene·기능 회귀. 다음 환경 Field06은 Field05 미술 방향 검토 후 착수.
- [Field05 v1 납품 정본](../../Field05_Renewal_Art_v1_Delivery_20261019.md). 납품 문서 최초 커밋 `236bb3c2a4478f9098f7cbf5634ab8e290010541`; 담당 `docs/art-renewal`. main 직접 커밋/Push/병합 금지.

---

**최종 사용자 결정(2026-10-12): 리뉴얼 담당은 Chapter 1·2 기존 아트 교체(환경·몬스터·보스·펫·NPC·VFX)만 전담합니다. 기존 스프라이트 담당은 Chapter 3 이후 모든 신규 아트(환경·필드·던전·몬스터·보스·펫·NPC·캐릭터·VFX·UI 필요 이미지)를 총괄합니다. 기존 공식 승인 자산은 별도 승인 없이 교체하지 않습니다. Codex는 구현·Unity 통합·QA 전담입니다.**

Chapter2 Field04 실제 아트 ZIP 제작부터 시작. 환경 순서 Field04 → Arbel → Field05~Field11. 환경 완료 후 Chapter1·2 기존 몬스터→펫→NPC→VFX. 실제 공식 승인 몬스터 시트·캐릭터·보스는 임의 교체하지 않습니다. B1 문 상태 표시는 Codex 보완 보고, Field03 Rock B는 **ART_FIX_PENDING**. 최신 로드맵 `../../LIMITLESS_Chapter2_Original_Art_Roadmap_20261012.md`·전체결정 `../../LIMITLESS_20261012_전체대화_결정_인계.md` 참고. 사용자 승인 뒤 Codex Unity 통합. 세션 여유가 줄면 사용자에게 사전 보고·후임 인계.


## 2026-10-13 신규 리뉴얼 담당 인수인계 중

사용자 보고: Chapter 1·2 기존 아트 **리뉴얼 담당 신입 세션이 교육 중**입니다. 교육 완료·실제 신규 납품·Unity 통합 완료로 기록하지 않습니다. 위 역할 경계와 기존 리뉴얼 로드맵을 우선 학습시키고, 진행 중인 Field03 Rock B v2·Field04 결과는 새 담당이 최신 검수/승인 상황을 조회한 뒤 인계받도록 합니다. 이전 담당의 결과물을 중복 제작하거나 신규 Chapter3 아트를 담당하지 않습니다.

## 2026-10-13 Field04 v2 동서 길 보완 — 아트 납품, 사용자 승인 대기

- 담당 브랜치: `docs/art-renewal`. 납품·기술 보류 정본: [Field04 v2](../../Field04_Art_v2_EastWestRoad_Delivery_20261013.md). 기존 v1/Unity 원본·Scene·GUID/meta·Save·공식 세린/몬스터를 변경하지 않았음.
- 실제 v2 ZIP `Limitless_Field04_Renewal_Art_v2.zip`을 ChatGPT 파일로 생성하여 사용자 다운로드용으로 제공. SHA256 `178ad6b235c65848cce48f4e2cfe86ff77c36857a12663237f7d7afa561f969d`; 파일68개/PNG60개. 사용자 F: 저장 확인은 아직 없음.
- 기존 Road30 오브젝트별 서→동 15열×2행 후보(30개 독립 PNG와 배치표) 및 추가 양끝 연결용 12PNG(별도 승인 필요) 제공. Scene 자체 회전/원본 전면 교체하지 않음. v1 합성 대비 Preview는 실제 Unity RenderTexture 아님.
- Grass `tiles_grass_6_0` 라벨 정정, 실제 Medium3/Big2 나무 역할 정정, Field04 RockNorth 접지 후보만 신규 제작. **Field03 Rock B v2와 혼동 금지**.
- 상태: `ART_DELIVERED / USER_ART_REVIEW_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`. 다음: 사용자 미술 승인 → Codex 전용 worktree에서 선별 연결·접지/휠체어/세린 Main12~13/Save QA. 기존 서쪽 Spawn-TreeWest 겹침, 북쪽 경계 이슈 유지.
- 저장소 협업: main 직접 커밋/강제 Push 금지. 담당별 문서는 `docs/art-renewal`; 사용자 승인 후 별도 main 병합.

## 2026-10-14 Arbel 리뉴얼 v1 — 실제 Sprite PNG34종 ZIP 납품

- GitHub main 최신 `bddca540d2fdd9e93f642f8d0f387b34730bd2d8`, PR10 병합 및 5씬 Sprite복구1747/11PNG/123Sprite·23BuildScene PASS를 기준으로 복구 원본 자산 모두 보호. 과거 PR10 Blocked를 최신 상태로 오인하지 않음.
- 현행 Arbel 직렬화 SpriteRenderer **284(Grass247/Path16/건물8/기타13)**, Runtime295는 별개. GUID+fileID·이름·좌표를 근거로 정확히 284행 Renderer 대응표 제작. 실제 서비스 NPC는 대부분 Runtime 생성.
- 새 납품: `Limitless_Arbel_Renewal_Art_v1.zip` 파일47/PNG40 중 **신규 환경34**(기존 렌더러 치환후보23 + 별도 장식11). SHA256 `ec707452169aa99ec297a7ac23a14928d4a0123b4b7b051d17ddcb282953d626`. ChatGPT 다운로드용이며 사용자 F드라이브 저장·청취/미술 검수는 별도.
- 미술 콘셉트: Field04 v2와 연속되는 건조 올리브·황토 흙길, 따뜻한 목재·지붕/석재·중앙 광장/생활 소품. 정본 고정된 두 건물 및 기존 Path16/Grass247로 정적 합성 Preview 제작. Preview는 실제 Unity RenderTexture 아님.
- 현행 PortalAnchor(-7,-4)와 기존 TreeSouthWest(-7,-4.2) 중첩 확인. Portal 장식 추가는 **HOLD_COLLISION_WITH_EXISTING_TREE**로 등록하고 미리보기에서도 제외. 두 지붕 384×192 확장으로 NPC/Camera·접지 QA 필요.
- 아트 결과물 내부 `Manifest.json`, `Renderer_Mapping_284.csv`, `Optional_Service_Placement.csv`, `README.md`, `QA/Offline_Validation_v1.json`, 출처·상업 사용 선언, Codex 인계, Preview/Contact Sheet 및 비교 모식도 포함.
- **상태:** `ART_DELIVERED / USER_ART_REVIEW_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`. 오프라인 PNG/CRC/SHA·284행 대응과 Ground 외곽 검사는 PASS이나 실제 Unity Import/직접 플레이/휠체어·Quest·Portal·마차/23Scene회귀는 검증 전.
- 문서 기록: [Arbel v1 납품](../../Arbel_Renewal_Art_v1_Delivery_20261014.md), 최초 Docs Commit `e84acba692f8e211bd2f28cef0de7f27a1ca82f6`. 담당 `docs/art-renewal`, main 무변경·강제 Push 없음.
- 다음 순서: 바울 대표 미술 승인(교체23종, 장식11종 각각 선택) → Codex의 로컬 원본 비교/선별Import·Scene 적용·복구 자산 보호·Runtime 검증 → 사용자 최종 화면 확인. 기존 Field04 공식42, Chapter3 신규 아트는 타 담당 영역으로 보호.

## 2026-10-14 Arbel v2 최소 수정 납품 — 사용자 최종 미술 승인 대기

- 기획 `docs/planning`의 `Arbel_Renewal_v1_MinorFix_QA_Handoff_20261014.md` 검토 후 **v1 전체 재제작이 아닌 Road2 + 북서 건물1 + Props9 접지 정보**만 보완.
- 실제 v2 ZIP `Limitless_Arbel_Renewal_Art_v2.zip`, SHA256 `17625d35c93599bdd74a4b8280c0416240c402be279815c8966199410bca97b7` (1,509,138 bytes, 파일55개/PNG43개 중 실사용34·Preview8·QA1). v1 ZIP SHA `ec707452169aa99ec297a7ac23a14928d4a0123b4b7b051d17ddcb282953d626` 불변.
- 사용 PNG34 가운데 **변경3, 동일 바이트31**: Road Left10pixel / Right4pixel 변경해 Left→Right·상하 RGBA seam mismatch 0. 북서 `Roof_Respite`의 실제 치유 위치와 무관한 십자205pixel만 동일 메달 배경으로 바꾸고 외형/384×192 HOLD 보호.
- Props9종의 원본 회복 Sprite.rect px와 신품 canvas/alpha foot를 `QA/Props_9_Grounding_Audit_v2.csv`로 보완. Props PNG 변경0; 원본 PNG의 실제 alpha foot 및 Unity 직접 접지는 `NOT_VERIFIED`. 부정확한 이미지 전면교체보다 현장 재측정 우선.
- `Renderer_Mapping_284.csv`: 이름/원본GUID+fileID/Transform/Sorting/PPU 불변, PNG target 필드 17참조만 새이름으로 정정. 직접 후보23 / 선택장식11 유지. PortalAnchor와 Tree 충돌/대형 지붕 런타임 가림 HOLD, 공식 캐릭터/NPC/Save/Quest/GUID/meta/복구 자산 변경0.
- 정본 기록: [Arbel v2 납품 및 QA](../../Arbel_Renewal_Art_v2_MinorFix_Delivery_20261014.md). 아트 문서 최초 commit `5d975a65cef117061fee9c3b6c89247ec18301aa`, 브랜치 `docs/art-renewal`.
- 현 단계 `ART_DELIVERED / OFFLINE_QA_PASS / USER_ART_REVIEW_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`. 사용자 미술 검토→Codex 읽기전용 소품 원본 Alpha 접지/scene 감사→미술 승인 후에만 Unity 최소 적용 및 QA. 기존 v1 인계는 이력으로 남기며 v2 미술 승인으로 자동 승격하지 않습니다.

## 2026-10-14 Arbel v3 소품 rect 기록 정정 — Codex 인계 접수

- **현재 최신 Arbel 후보는 v3**. 바울 대표가 Codex의 완료 인계/검증 근거를 전달했고, 리뉴얼 담당 세션은 **로컬 F: ZIP을 직접 열거나 검사하지 않았습니다**. v2는 역사 자료이며 8종의 `source_sprite_rect` 수치가 최신 정본이 아닙니다.
- 사용자 로컬 ZIP: `F:\Downloads\Limitless_Arbel_Renewal_Art_v3.zip`; 보고 SHA256 **`84e9707df7f0409e0238781f0860499dd4344b79ec88b49964a7ce7d8e10633b`**, **1,521,440 bytes / 60파일 / PNG43** (실사용34·Preview8·QA1). v2 SHA `17625d35c93599bdd74a4b8280c0416240c402be279815c8966199410bca97b7`·v1 SHA `ec707452169aa99ec297a7ac23a14928d4a0123b4b7b051d17ddcb282953d626` 보호.
- v2 대비 **PNG43/43 전부 byte·SHA 동일**. 변경은 문서/CSV·JSON뿐이고 새 PNG 재작업0. **직접 교체 후보23/선택11** 및 Renderer284/284, 이름·좌표·GUID/fileID·PPU/Pivot/Sorting/target 경로 불변.
- `fileID21300000` Single Sprite 실체 확인 후 8종 원본 `source_sprite_rect`만 정정: Barrel/Bush_Dry/Bush_Olive/Campfire/Chest/DryMossRock 모두 **128×128**; OldTree_Big **255×256**, OldTree_Medium **128×156**. Fence **128×128 불변**. 기존 meta의 Multiple slice rect를 Single Sprite rect로 혼용 금지.
- 원본/후보 Alpha>0 하단은 그림자 포함 **오프라인 계측**이며 **본체·Collider 물리 접지 PASS가 아님**. v3 CSV `QA/Props_9_Grounding_Audit_v3.csv`·`QA/Props_8_Rect_BeforeAfter_v3.csv`·Manifest를 정본으로 사용. **v2 Props9 Preview의 숫자 자막은 의도적으로 보존한 구 버전 수치이므로 재사용 금지**.
- Codex 보고 QA: `main` 대응 **2455 PASS**, 패키지 **191 PASS/FAIL0**, rect 정정 오류0, Road RGB/Alpha Seam0, ZIP CRC·Manifest·PNGDecode PASS, 기존 원본5,221개 SHA 및 dirty·HEAD 불변, Unity/Play/포커스/원본 PNG·GUID/meta/Scene/Quest/Save 변경0, Codex commit·Push·main병합0.
- **HOLD 유지:** Roof2 NPC·출입구·Camera 가림, Props9 실제 본체 접지, Optional11 개별 미술·배치 승인, Portal(-7,-4)/TreeSouthWest(-7,-4.2) 겹침. **미술 승인·Unity 적용·Runtime 승인 없음**.
- 정본 문서: [Arbel v3 Codex 문서 정정 인계](../../Arbel_Renewal_Art_v3_Metadata_Correction_Handoff_20261014.md). Codex 로컬 상세보고: `F:\study\codex\Project-Limitless\Temp\ArbelV3Metadata20261014\Arbel_v3_Metadata_Correction_Report_20261014.md` (이 세션 미열람). 문서 담당 브랜치 `docs/art-renewal`, main 직접 커밋/강제 Push/병합 금지.
- **현재 상태:** `CODEX_METADATA_QA_PASS / USER_ART_FINAL_APPROVAL_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_APPROVED`. 다음: 바울 대표 v3 미술 승인 범위 결정 → Codex가 원본 물리 접지·Roof·Portal/선택장식 HOLD 재검토 → 승인 범위만 Unity 적용 후 Runtime QA.
