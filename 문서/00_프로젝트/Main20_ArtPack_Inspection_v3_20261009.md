# Main20 Art Pack v3 최종 기술 검수 — 2026-10-09

**READY_FOR_USER_ART_REVIEW**. v2의 마지막 FAIL인 절벽 가이드선 제거는 **PASS**. 게임용 PNG17개 무결성 PASS, 절벽을 제외한16개는 v2와 원본 byte 동일하여 Boss/공용3행/Phase2 Overlay/VFX의 기존 기술 PASS를 유지한다. 필수 추가 아트 재제작 요구0. 사용자 미술 승인과 Overlay 최종 채택은 대기이며 실제 Unity Import·Main20 구현은 미착수다.

## 1. 시작 맥락·검사 범위

LOCAL HEAD `e75e7b73919dc12e4f663e90dc5b5048d2ab3992`. 최근5 commit e75e7b7 /cad75de /14327fd /7a78aa8 /f8ca63a. staged0, 기존 tracked 수정75·미추적 top-level25·전체 status562행을 기록했다. 원격 조회·Push0.

git→AGENTS→PROJECT_CONTEXT→CURRENT_STATUS→CODEX_WORKFLOW→[Art Handoff](Main20_Art_Handoff.md)→[v1 검수](Main20_ArtPack_Inspection_20261009.md)→[v2 검수](Main20_ArtPack_Inspection_v2_20261009.md)→[Design QA](Main20_Design_QA.md)→[Story 정본](../03_스토리/Chapter2_Main20_심부의_거신.md)을 확인했다. v1/v2에서 완료한 전체 미술 비교를 반복하지 않고 v2→v3 해시 변경과 절벽에 집중했다.

ZIP은 읽기 전용 메모리 조회. 내부 모든 파일 SHA256과 ZIP CRC, PNG signature/IHDR/Pillow verify 후 재열기·전체 decode, 게임PNG17의 해상도/RGBA/Alpha와 Manifest 일치를 확인했다. Boss16셀 기하/경계·Overlay 본체 외부 픽셀과 KO alpha도 간단한 회귀 수치 검사로 확인했다. 절벽 원본·체커 배경·실제 Field11 Ground 합성·납품 QA 전후 이미지를 비대화형으로 조회했다. Unity Import/실행/코드 변경/화면 포커스 조작0.

## 2. ZIP 및 v2 대비 파일 변경

| 원본 ZIP | bytes | SHA256 | 구성 |
|---|---:|---|---|
| `F:\Downloads\Limitless_Main20_DeepCore_VeinfireColossus_Art_Pack_v2.zip` |5,099,833|`5be1678d6be207e1dc81414b5392bd00d429bc1af580d78f185863eb0c40fef4`|파일23 /PNG21 /text2|
| `F:\Downloads\Limitless_Main20_DeepCore_VeinfireColossus_Art_Pack_v3.zip` |5,702,855|`eed33ea29d31788ee30d38750bc186fe6178d658c9c10881bb02a6355a7da60f`|파일25 /PNG22 /text3|

v3 PNG22=**게임RGBA17(Boss2/VFX7/환경8)+QA RGB5**. text3=Manifest/v2 Review Notes/v3 Review Notes. CRC 오류0·PNG 손상0·중복 경로0·위험 경로0. 게임 Ground/Arena는 의도한 불투명RGBA이고 나머지15개는 실제 투명 영역이 있다. QA RGB는 배경/라벨이 합성된 검토용이며 게임 Sprite가 아니다.

- 실제 수정 게임PNG: **Environment/DeepCore_Cliff_Boundary.png 1개**.
- 미수정 게임PNG16개: 원본 SHA256과 byte 모두 v2 동일. 의도하지 않은 게임PNG 변경0.
- Manifest 수정1: 버전/검수 상태 v3, Cliff Purpose·이름 대응 설명·가이드 제거 구간/대상 한정과 신규QA 안내 추가. Phase2/Static VFX/hot-cooled 계약은 유지.
- v2 QA PNG4와 v2 Review Notes는 그대로 보존. 기존 QA 이미지 교체0.
- 추가2: `QA/Main20_Cliff_Boundary_BeforeAfter.png`, `QA/Main20_v3_Review_Notes.md`(QA만 추가).
- 삭제0 /이름 변경0 /게임 역할 추가0. v2 Review Notes의 'Cliff byte 그대로'는 v1→v2 이력 설명이며 v3 최신 계약과 구분한다.

### 변경 파일 SHA256

| 파일 | v2 SHA256 | v3 SHA256 |
|---|---|---|
|Environment/DeepCore_Cliff_Boundary.png|`e020a8681401005c8664fc12a5afdc7d61a1725a6c2bd8f5aad6178fa14a5c9c`|`143adabf22c326af766615f702eb37fe06fbafc36310c5f7de9e0e04cc025a17`|
|Manifest.txt|`ec87bf03cb90d2209333938218d1cb837f4a1cb4ca4300ccd400f287a85b052c`|`9b51773cfd071f759cd4c00fffb715ecdb7418210da1f93180a3f934ae841943`|

### 전체 내부 파일 비교

SAME은 아래 v3 SHA256이 v2와 동일함을 뜻한다. ADDED는 v2에 없는 QA 파일이다. 기존 파일의 해상도·형식도 함께 대조했다.

| 상대 경로 | v2 대비 | 형식 / 크기 | v3 SHA256 |
|---|---|---|---|
|Boss/Veinfire_Colossus_Phase2_Overlay.png|SAME|PNG RGBA8 1256×1256 /266,722bytes|`f566f96662d156ed76495a10aa62512530110a7fe5c684a042799fb0e9da28f6`|
|Boss/Veinfire_Colossus_Sprite_Sheet.png|SAME|PNG RGBA8 1256×1256 /540,211bytes|`bc8a290bd0742a9043e9b431a6faec772c5698e57c96aac6e9c7f7e36a8f5f6f`|
|Environment/DeepCore_Arena_PressureMark.png|SAME|PNG RGBA8 512×512 /457,445bytes|`3b5cc0a7cbc820045c7d67827f336d54cd6579b15b7970c18f2c6601e8dba1a7`|
|Environment/DeepCore_Cliff_Boundary.png|MODIFIED|PNG RGBA8 512×512 /222,645bytes|`143adabf22c326af766615f702eb37fe06fbafc36310c5f7de9e0e04cc025a17`|
|Environment/DeepCore_CollapsedCore.png|SAME|PNG RGBA8 512×512 /33,594bytes|`a06c5ec34ae1771374b4a8c02b24d99531c4749d9125fb79b4261b6d5fe6b99a`|
|Environment/DeepCore_ColossusTrace.png|SAME|PNG RGBA8 512×512 /200,590bytes|`f463f7a33ff6370b8d34af76c6d446771fabcc4c44b6412b4002f98d6a1b3b48`|
|Environment/DeepCore_Ground_Base.png|SAME|PNG RGBA8 512×512 /455,477bytes|`54e8fb28dff8877ed635177360d36ae189843f1a6d33a0e3623285ea0ac1cd78`|
|Environment/DeepCore_HeatRecession.png|SAME|PNG RGBA8 512×512 /23,353bytes|`518280749821145dd2cb7a1fb47dc3681ec15cffc356789359f34b84a9b901d1`|
|Environment/DeepCore_Rift_01.png|SAME|PNG RGBA8 512×512 /29,675bytes|`bd2f8820cad8979ba4054aea96338d7d58a14ed2b5253d2a837f4f2df6291b61`|
|Environment/DeepCore_Rift_02.png|SAME|PNG RGBA8 512×512 /29,504bytes|`948a3ef40f6ae3b9f51c082635e45553ed607a4020afce3c173922fdcad411e8`|
|Manifest.txt|MODIFIED|UTF-8 text /8,461bytes|`9b51773cfd071f759cd4c00fffb715ecdb7418210da1f93180a3f934ae841943`|
|QA/Main20_Boss_Row3_BeforeAfter.png|SAME|PNG RGB8 1256×712 /247,683bytes|`6870f8cffc4477b4aba107bb64fe1f43f70ecd8668c6d6f5a60498aa34a87df3`|
|QA/Main20_Environment_BeforeAfter.png|SAME|PNG RGB8 2080×565 /864,941bytes|`b42aeb9f0c3847f5798c277d37bca2fc997a9b16747a935f5f75f737a6388e8b`|
|QA/Main20_Ground_Tiling_BeforeAfter.png|SAME|PNG RGB8 2048×1024 /1,034,521bytes|`b6e72d24aea2ad8d80bde6c8f6c53b4fb3700ec5224b1420b6297519ae4ae90d`|
|QA/Main20_Phase2_Composite_16Frames.png|SAME|PNG RGB8 1256×1256 /567,744bytes|`659ec47d19dff0102bf1c19e04f5e22d54845236b616f9919719b34945efd639`|
|QA/Main20_v2_Review_Notes.md|SAME|UTF-8 text /2,597bytes|`b0421b1f0fc3397bd95cc717cf0784669f038a4309933bedafe80e460a31c074`|
|VFX/Veinfire_CoreCondensation_Telegraph.png|SAME|PNG RGBA8 512×512 /19,874bytes|`67b91c92b3ed0cdaa1fa90985076a50dfa297512d30826bef113480041a13a90`|
|VFX/Veinfire_CoreEruption_VFX.png|SAME|PNG RGBA8 512×512 /49,117bytes|`f163c75daeab7b9f54b077223c29376a9a646326fc2196a468bfb0cd504d6ab9`|
|VFX/Veinfire_CoreResonance_Telegraph.png|SAME|PNG RGBA8 512×512 /15,032bytes|`ac41ef8a18c3b1ee1c1bca5ee53bdf94e2cb0910749c1298b26aa8d4df66fc54`|
|VFX/Veinfire_CoreWave_VFX.png|SAME|PNG RGBA8 512×512 /16,522bytes|`373a8062c16ce899f32642fbe3388bd9a7ef86d7f3b4e0427a05ccaef085a463`|
|VFX/Veinfire_HeatPressureInjection_VFX.png|SAME|PNG RGBA8 512×512 /16,816bytes|`e6e59ccc4bd2412796535757c928261b34f938b30439681125c4c50df601fefc`|
|VFX/Veinfire_HeatWave_VFX.png|SAME|PNG RGBA8 512×512 /50,529bytes|`2a8eb0379b6e96e56cccdc2ba5ec0298a014fb8539c3ed9b5a666d2d49d51b2c`|
|VFX/Veinfire_MoltenStrike_VFX.png|SAME|PNG RGBA8 512×512 /26,072bytes|`bc4bd18f45d9aba93f3ace5ce823e1db7c9b6220a95b03dacdb4eef985fc1de7`|
|QA/Main20_Cliff_Boundary_BeforeAfter.png|ADDED|PNG RGB8 1100×1190 /621,548bytes|`5cb14dfe436d84dbe99a4dffb12e316b1161d6746c497cd7645632f85cc0074c`|
|QA/Main20_v3_Review_Notes.md|ADDED|UTF-8 text /1,091bytes|`ba37b9aeeb4327c74a94b49153dcfd44bb3e0d19665ced73fc6cda6291116954`|

## 3. 절벽 가이드선 집중 검사

**PASS**: 실제 PNG에서 가이드선과 미세 alpha 꼬리가 제거됐고, 원래 암석 외형은 유지됐다. 단순히 QA 미리보기에서 선을 숨긴 결과가 아니다.

512×512 RGBA8 유지. 변경pixel 6,100, 변경bbox [16, 53, 496, 458] (우하단 exclusive). 변경한 모든 pixel은RGBA(0,0,0,0)으로 제거됐으며 새 alpha>0 pixel0, 나머지256,044 pixel은 v2와 그대로 동일하다. 캔버스 이동/확대/축소 없음. 전체alpha bbox [16, 53, 496, 458]→[21, 62, 496, 455]는 가이드 제거에 따른 외곽 축소이지 Sprite 캔버스나 pivot 변경이 아니다.

| 대상 구간(좌상단 기준) | v2 alpha>0 pixel | v3 alpha>0 pixel | 판정 |
|---|---:|---:|---|
|상단 y53~59,x16~495|1,179|0|PASS|
|왼쪽 x16~20,y53~457|2,021|0|PASS|
|하단 y455~457,x16~495|1,440|0|PASS|

구간별 수치는 교차 구간이 겹치므로 합산하지 않는다. 기존문제보다 조금 넓은 주변 구간까지 실제투명0으로 검사했다.

| 기존 증거 pixel(x,y) | v2 RGBA | v3 RGBA |
|---|---|---|
|(20, 53)|[85, 85, 88, 255]|[0, 0, 0, 0]|
|(40, 54)|[71, 72, 73, 199]|[0, 0, 0, 0]|
|(17, 100)|[15, 18, 19, 255]|[0, 0, 0, 0]|
|(200, 457)|[70, 72, 73, 255]|[0, 0, 0, 0]|

| 확인 항목 | 판정 | 근거 / 한계 |
|---|---|---|
| 회색 사각 가이드선 제거 | PASS | 문제 구간 실제 alpha0, 원본·체커·지면 합성 모두 테두리 없음 |
| 암석 외형 유지 | PASS(정적) | 남은 pixel 전부 동일, 뾰족한 암석·바닥 파편 형태 유지·명백한 직선 잘림/구멍 없음 |
| 투명 배경 보존 | PASS | RGBA decode, 삭제pixel 투명화·새 불투명 배경 생성0 |
| 검은 지면 합성 | PASS(정적) | 실제 Ground_Base에 alpha합성, 사각 테두리 없음 |
| 절벽/통행 불가 시각 역할 | PASS(아트 역할) | 암석벽·깊은 틈 유지. 실제 이동영역·Collider·Sorting·화면 가독성은 PARTIAL/미검증 |

독립 생성한 합성에서 확인했고 납품 `QA/Main20_Cliff_Boundary_BeforeAfter.png`도 일치하는 전후 변화를 보여 준다. 전역 미술 승인이나 실제 Unity 렌더링 PASS로 확대하지 않는다.

## 4. 기존 PASS 유지 및 승인 대기

| 항목 | 기술 판정 | 이번 근거 / 남은 사용자 검토 |
|---|---|---|
| Boss1256/314/4×4 | PASS 유지 | 기본/Overlay 해시 동일·1256×1256 RGBA 확인·16셀 내용/기본 셀경계0 유지 |
| Skill/Hit 공용3행 | PASS 유지 | 기본 시트 byte 동일, v2의08~11 중립 반응·10/11 방출 제거 그대로. 실제8FPS 자연스러움 승인 대기 |
| Phase2 Overlay16프레임 정렬 | PASS 유지 | byte 동일, Overlay가 본체 투명부에 나타나는pixel0·동일16셀. 사용자 최종 채택은 대기 |
| KO Overlay 소거 | PASS 유지 |00~11 maxalpha129→12=47→13=21→14/15=0. 기본KO13~14 상체/열핵 재확대 자연스러움은 PARTIAL 유지 |
| VFX7 | PASS 유지 | 모두byte동일·512RGBA static1프레임·역할명 유지. 응축/공명 피해0 및 파동/분출 매핑 보존. 실제밝기·점멸·HUD 가독성 미검증 |
| Ground/Arena | 기술 무결성 PASS | byte동일. 반복띠·Arena 연결 자연스러움은 PARTIAL 유지·무단 미술PASS0 |
| Rift/잔해/cooled | 기술 무결성 PASS | byte동일. HeatRecession은Rift01 교체용, Rift02/전체Field cooled판 아님 |
| 추가Trace | PASS 유지 | byte동일·목표3 시각 단서. 실제Field 식별은 사용자 검토 |
| Handoff16역할 | 기술 대응 PASS | 정본 이름15/16+명시Overlay대안1, 추가Trace1 보존. 정본 완성Phase2시트를 납품했다고 주장하지 않음 |

정본16개 역할과 Quest9의 상세 매핑은 [v2 표](Main20_ArtPack_Inspection_v2_20261009.md#4-정본16개-대응)와 [Objective9](Main20_ArtPack_Inspection_v2_20261009.md#8-main20-objective9-아트-대응)를 유지한다. 이번 변경은 절벽역할의FAIL→PASS이며 새이름/역할/목표ID를 추가하지 않는다. Overlay 대안은 사용자 요청대로 기술PASS를 유지하고 채택승인만 별도 남기므로 NEEDS_HANDOFF_MAPPING을 최종 판정으로 요구하지 않는다.

## 5. 구현 전 기술 계약 — 권장안이며 미확정

아래는 [Story 정본](../03_스토리/Chapter2_Main20_심부의_거신.md)과 현재 공용 코드의 경계를 고려한 **제안**이다. 이 문서 작성으로 게임 디자인을 확정하거나 구현하지 않는다. Phase 행동경계·용융강타 분류·EXP정책·Overlay채택은 사용자 결정이 필요하다.

| 계약 | 권장안 / 이유 | 승인·후속 검증 |
|---|---|---|
| HP50%/기존예고/Phase2첫행동 | HP600이하 첫진입 즉시Phase표현1회, Party/HP/상태/턴큐 유지. 이미 응축이 예고한CoreWave가 남으면 **다음Boss행동에서 원래80%파동을1회 수행**한 뒤 Phase2의열압주입부터5행동순환 시작. 대기예고가 없으면 다음Boss행동을열압주입으로 시작. 예고를분출로바꾸거나 추가전환턴을소비하지 않는 안 권장 | 미확정. Phase2에Phase1파동이1회 이행되는 명시예외 승인 필요. 다중타격/DoT/600경계/예고직후전환/전환직후KO 검사. KO이면 대기공격 미실행·예고 정리 |
| 용융강타속성/사거리/대상 | 기존 `TargetRangeType.MeleePhysical` 후보 권장. 단일125%·Taunt우선, 그 외 기존 근접 대상resolver(생존전열+같은열의보호전열이없는후열)·기존 안정선택 재사용. 피해후 생존대상 공용Burn, 과열추가0. 후열무시관통/방어무시/새마법효과 없음 | **분류 미확정**. Manifest의melee는아트설명이라승인근거아님. 기존PlayBasicAttack/공용직접피해 경로 사용 권장; 공용resolver는수정하지않음 |
| 기본EXP140/레벨차 | `BaseExperience=140`, 기존 `ExperienceProgression.MonsterExperience` 배율 적용 권장. BossLv15에서 Player≤8=210/9~11=175/12~18=140/19~21=70/≥22=0. QuestEXP100/Talent80과BossTalent40은별도1회 | 모든레벨최종140고정 의도가 있으면 별도결정. 이번 공용EXP 변경0. 승리1회/패배0/완료중복0·일반Loot0 검증 |
| Phase2 Overlay동기화 | 본체와같은16Rect/PPU314/pivot(0.5,0), 동일frame index/scale/flip로별도Image또는Renderer갱신. 본체ShowFrame 경로를같이관측하는소규모연결 권장, 독립타이머 금지. KO14/15투명프레임유지 | 사용자채택승인 대기. 전체1256Overlay를314셀위에얹지않음. Idle/Attack/Hit/Skill/KO 및Phase진입동기화 QA |
| Field10↔Field11 | Main19완료Gate 뒤 기존서쪽출구를새Field11왕복Exit로연결. 별도Entry/ReturnSpawn, Trigger비중첩·WorldBounds/viewportCamera/실제Exit만Collider통과·BuildSettings/Connection/Navigation등록 | Geometry/좌표/실제Boss크기는구현전결정. 잠금전/완료후/역방향/Continue위치 QA. 기존Field10/Main19Scene작업보호 |
| Quest9/Save·Continue | 정본ID9개와공용ReachLocation/Interact/DefeatEncounter 사용, 목표5대화완료→6·승리만6진행·패배6유지/재도전. 진행상태로Boss/잔해/cooled/일회대화표현복원·완료ID로보상중복차단. Version1/5슬롯 유지 | 9단계별Save/Continue·승리복귀·재도전·완료후재로드·Side9병행·인벤토리/Party/Formation/Beast 회귀. 전투중Phase세이브기능으로확대하지않음 |
| The Weight of Crowns | [Audio Handoff](Main20_Audio_Handoff.md)의원본을재확인하고전용Encounter `field11_main20_veinfire_colossus`→Crowns를FindBattle의isBoss null분기보다앞에매핑. Field11/복귀→Paths of Cracked Earth. 기존1AudioSource·Mixer·fade/loop재사용 | USER_LISTENING_REQUIRED 유지. 원본MP3복사/Import는별도구현. Phase별곡교체/모든Boss강제Crowns/새Source 없음. 진입·결과·패배·복귀·Continue 청취 QA |

근거: `MonsterSpriteSheetAnimation.ShowFrame`은현재본체만갱신하고Phase2동기화API는미구현. `BattleCore.TargetResolver`는단일Taunt가사거리보다우선하며근접후열의보호전열존재를확인한다. `ExperienceProgression`은기존레벨차배율과정수반올림, `BgmSceneCatalog.FindBattle`은현재일반Boss에null을반환한다. 기존파수꾼Runtime을Main20의50%즉시전환으로임의변경하지 않는다.

## 6. 승인 후 실제 구현 순서·체크리스트

**아트 기술 준비 완료. 실제 구현 착수에는 사용자 미술/Overlay 승인, 핵심 미결 계약 결정과 별도 구현 지시가 남았다.** 아래 순서로 후속 작업을 진행할 수 있다.

- [ ] 사용자 미술 검토: 절벽·Ground/Arena/Trace 배치, Boss고유외형/Main19연결·Idle/공용3행/KO 흐름·Overlay강화·VFX구분. 현재 승인 완료0.
- [ ] 위 Phase행동경계·용융강타분류·EXP배율·Overlay채택을 승인된 계약으로 정본에 기록. 음악청취상태도 확인.
- [ ] 시작Git/Save/자산 보호 스냅샷, 원본v3 ZIP SHA 재확인 후 신규경로·GUID/meta로Import. 기존PNG/meta 덮어쓰기0.
- [ ] Boss/Overlay: Multiple·1256원본유지(MaxTextureSize≥2048/NPOTNone)·Point·무압축·mipmapOff·PPU314·발밑pivot. Rect=(col×314,(3-row)×314,314,314), 행별4프레임/기존8FPS선례. VFX7은Single·center/RaycastOff/static512,4분할금지.
- [ ] Field11/왕복연결/Bounds/Sorting/통행·조사Site 등록. 환경PPU128선례,Ground/균열center·Cliff/잔해발밑·hot/cooled동일transform. Default sortingOrder선례를사용하고실제Player가림QA 후결정.
- [ ] BossDefinition·Encounter(IsBoss/Flee불가/Loot0)·4/5고정패턴·즉시50%1회·상태보호·예고HUD/VFX/Overlay동기화 연결. 반응속도조건/빠른Flash/강한반복Shake 없음.
- [ ] Quest9·대화·목표별표현/Save·Retry·보상1회·Crowns/Paths BGM 연결. Main20 TTS는기존Main18→19 우선순위와quota정책을보호하며이번자동제작0.
- [ ] 격리백그라운드Compile/Runtime: 목표9·600경계·대기예고·Taunt/근접대상·Guard/공용Burn/과열·KO/패배재도전·보상중복·9단계Continue·왕복Bounds·BGM·Side9/잡템7 회귀.
- [ ] 사용자 실제UI/음악/난이도·미술 확인. 새문제가실제로확인된경우에만추가아트범위를검토하며지금필수재제작요구0.

## 7. 판정·보호 검증

**READY_FOR_USER_ART_REVIEW / ART_TECHNICAL_PASS / USER_ART_REVIEW_PENDING / UNITY_IMPORT_PENDING / IMPLEMENTATION_PENDING**. 마지막기술FAIL은해결했으며기존미술PARTIAL을PASS로바꾸지않았다. Phase2완성시트/추가VFX/새Rift02cooled판을불필요하게요구하지않음. 이후실제배치에추가냉각판이필요한지는현재미확정이다.

변경은이보고서와CURRENT_STATUS 두문서만. 보호4304기존파일(Git관리/미추적및사용자Save/TTS)의SHA256과v2/v3 ZIP SHA256 대조, 기존CURRENT_STATUS외변경0을확인한다. 이전v1/v2보고서·Handoff·DesignQA·Story·Side9·잡템7·기존PNG/meta/Save/Audio는보존. 기존562Git상태행보존·staged타작업0·reset/revert0·Push0. 문서링크·파일25/PNG22/게임17/Manifest17·목표9대응및git diff --check검증. 마지막관련commit은이문서를포함한 `Docs: Main20 아트 v3 최종 기술 검수와 구현 준비 기록` 참조.
