# Chapter2 전투 배경 읽기 전용 감사·신규 ZIP 단회 확인·VFX 최소 제안 / 2026-11-04

## 이번 결론
- 신규ZIP2개 실제F: 전체SHA256을 각각1회 계산하여 **최신 총괄 기획 보고와 모두 정확히 일치**했습니다. 아트/애니메이션 최종 승인을 새로 부여한 것은 아닙니다.
- 로컬 Chapter2 지역의 배경은 **Field_04~10의 기본 색상 전장 패널**과 **Field_11의 심부 지면PNG 재사용**으로 구분됩니다. Chapter2 전용 전투 일러스트 세트/카탈로그 연결은 발견되지 않았습니다. Chapter2 도입 Main12가 기존 Field_03에서 전투한다면 Chapter1 Forest 매핑이 적용되며, 스토리Chapter번호로 배경을 선택하는 구조가 아닙니다.
- #89의99PASS·49PNG·VISUAL_PARTIAL은 기존결과를 그대로 참조합니다. 신규Unity실행/QA/캡처/제품수정0, 제안만 기록합니다.

## 근거와 SHA
최신기획 댓글 [#91](https://github.com/HappyPaul812/Project-Limitless/issues/91#issuecomment-6622460670), [#90](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6622459940), [#89](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6622459221), 총괄검수 정본 `20261104_VFX99_BattleBG2_SpiderAttack8_Review.md`(docs/planning 1795b6b526f432dc6ddb0b2a121a20971b4ba9ba)을 읽었습니다.

| 실제 ZIP | 전체 SHA256 실측 | 최신보고 대조 |
|---|---|---|
| F:/Downloads/Limitless_Chapter3_BattleBackground_2Pilot_20261104.zip | a8c02e9ab6189a6feb074d3f83c8cd6a8bce61ff1b8de110e7c8216934468eb9 | MATCH |
| F:/Downloads/LIMITLESS_SPIDER_ATTACK8_ISSUE90_20261104.zip | e612a88a6d4c6b4422cc025c0d32fb34e4ffc2f40337ffa909a1885cfae34041 | MATCH |

#90 아트납품댓글6622194831은 끝의1이 빠진63자 SHA를 기재했으나 최신기획댓글6622459940/기획정본은64자이며 실측과 일치합니다. 과거댓글을 임의수정하지 않고 이 차이를 명시합니다. CRC·SUMS·Manifest·GIF/프레임 검수는 반복하지 않았고 새 ZIP을 풀거나 PNG를 변경하지 않았습니다. #91 신규2배경 및 #90 신규중간5장은 **USER_ART_REVIEW_PENDING**, 기존Spider승인3키포즈/v5/Idle8/Walk8은 승인상태를 보호합니다.

## 로컬 감사 범위
실제 `Unity/Client/Assets`를 --no-ignore 파일목록으로 조사했습니다. PNG949개 파일목록 중 배경명/Background폴더/Main20지면 해당8PNG의 크기·GUID·바이트를 확인하고, Scene39개·Prefab13개 및 .asset의 해당GUID 참조와 런타임 C#을 읽었습니다. 이 수치는 테스트PASS가 아닌 조사범위이며 Assets외 개인Save/UserData를 스캔하지 않았습니다. 현재 로컬의 미커밋 버전도 읽되 수정/스테이징하지 않았습니다. 별도Temp/worktree/원격브랜치에 있는 미적용후보는 현재제품의배경으로 합산하지 않았습니다.

## 배경 선택·Scene/Prefab/Catalog/GUID 추적
1. `BattleSceneFlow.cs:34`가 Spawn.SceneName을 `BattleEncounterContext.FieldSceneName`으로 전달합니다. Battle Scene 로드 때 `BattleSceneFlow.cs:167~168`에서 **BattleSystem/BattleSceneController를 런타임 생성**합니다. 실제 Battle.unity에는 카메라/GlobalLight만 있고 해당Controller의 직렬화GUID참조0입니다. Prefab13개에서도 이Controller 및 아래배경PNG의 직접참조는 발견되지 않았습니다. 참조0을 실행누락으로 오해하지 않습니다.
2. `BattleSceneController.cs:479~481`은 `Resources.Load<BattleBackgroundCatalog>("BattleBackgrounds/Chapter1")`만 로드합니다. 실제catalog는 `_Project/Resources/BattleBackgrounds/Chapter1.asset` 한개, GUID **a5907f030508c8242912645d8b177ecb**. catalog클래스GUID **54ad0baaa3dda8a43a3d0b9976da4a0e**도 해당asset하나에 연결됐습니다. catalog자체는Resources문자열로드이므로Scene/Prefab직접참조가필수는아닙니다.
3. `BattleBackgroundCatalog.Find()` 우선순위는 Main11파수꾼 EncounterId→전용BossPNG, originScene=Field_11_DeepCore→심부지면Resources로드, scenes배열매핑, 나머지null입니다. 따라서 Chapter번호로 다르게로드하지 않습니다.
4. 현재카탈로그매핑은 Field_01→Grassland, Field_02/03→Forest, Dungeon_01/B2→SilentCatacombs, 파수꾼→SilentCatacombsBoss로 **고유4PNG**입니다. Ch1실제PNG는6개지만 ToxicSwamp/AncientUndergroundSanctuary는 조사한Scene/Prefab/asset에서참조0이므로 보유와활성연결을분리합니다.
5. backdrop이있으면 `BattleSceneController.cs:482~494`가 NightSky/DistantField/BattleGround/GoldenHorizon을끄고 전체Canvas Background에Sprite를원비율유지로표시하며 전장패널alpha를.12로둡니다. backdrop=null이면 **실제배경PNG없이** 위단색패널·황금수평선을유지합니다. 엄밀히텍스처그라데이션도아니며 코드로만든기본기하전장입니다.

| Chapter2 관련 originScene | 현재 코드/자산의 선택 결과 | 구분 |
|---|---|---|
| Field_03 (Main12 도입의 기존지역) | Ch1 BattleBG_Forest | 기존Ch1배경재사용, Ch2전용아님 |
| Field_04_WesternBorder | null→NightSky/DistantField/BattleGround/GoldenHorizon | 기본전장대체표시 |
| Field_05_WesternOutskirts | 동일 | 기본전장대체표시 |
| Field_06_ScorchedTrail | 동일 | 기본전장대체표시 |
| Field_07_AshenReach | 동일 | 기본전장대체표시 |
| Field_08_RedRift | 동일 | 기본전장대체표시 |
| Field_09_ObsidianScar | 동일 | 기본전장대체표시 |
| Field_10_BurningPulse (Main19/21) | 동일 | 기본전장대체표시 |
| Field_11_DeepCore (Main20 지역) | Resources/Main20/Environment/DeepCore_Ground_Base | 지면PNG의명시적전투배경재사용 |

위는 실제로컬코드·파일에따른선택경로입니다. 이번에 각전투를실행/현재Editor화면을확인한것은아니므로 새RuntimePASS/실제화면승인으로쓰지않습니다. originScene이정상전달되고Resources로드가성공한다는조건이며 각예외EncounterId의우선순위는위코드를따릅니다.

### 혼동하기 쉬운 두 PNG
- **DeepCore_Ground_Base.png**:512x512 RGBA, GUID **246ee89cea2cde74e9435490b8c657d8**, TextureImporter Sprite mode1/PPU128. 실제직접그림을보았으며 어두운암석균열지면텍스처입니다. Field_11_DeepCore.unity:295의SpriteRenderer(정렬-20)에GUID직접참조1개, 전투에서는Catalog.cs:21의Resources경로로추가재사용합니다. 별도완성전투일러스트를찾았다는뜻이아닙니다.
- **BurningPulse_DistantFlameSilhouette.png**:1024x512 RGBA, GUID **7c70f0eb774f7b0438b019f2f85aaf1b**. Field_10_BurningPulse.unity:1682의월드SpriteRenderer직접참조1개(직렬화기본enabled0), Main19ArtContentBuilder.cs:64의DistantSilhouette연출경로입니다. 전투Catalog/Scene/Prefab배경연결이아니므로 `Background/`폴더이름만으로전투배경이라세면안됩니다. 실제퀘스트연출노출여부를이번에PlayMode재검증하지않았습니다.
- 위두PNG/catalog/script의GUID정의는각meta한경로에만있음을표적검색했습니다. Scene직접참조와Resources문자열참조를분리한전체목록은ReferenceAudit.json에있습니다.

## 중복 제작 판단
현재로컬에서Field_04~10의전용전투일러스트세트를발견하지못했으므로 'Chapter2배경전체완료'나'새Ch2배경제작은중복'이라고판정할근거는없습니다. 반대로필드지면/배경폴더PNG가존재한다고전투일러스트적용완료로쓰지도않습니다. Main20은이미지면재사용이명시연결되어있어무단교체/중복작화대상으로삼지않습니다. 필요한Ch2신규배경범위와기존4종재사용여부는별도미술결정이며이번감사는신규제작발주가아닙니다. #91 C3신규2장은이번SHA일치만으로최종미술승인/게임적용승격0, Ch1정본과동배율화풍비교가여전히남아있습니다.

## #89 최소 표시 조정 후보 — 구현/실행하지 않음
기존9c799745의실제캡처와최신기획지시만근거로제안합니다. 승인3PNG/색상/알파바이트를보존하며정식규격확정이아닙니다.

| 후보 | 최소 변경 제안 | 보호/남은 판단 |
|---|---|---|
| A·40px 위치 | 화면상 발/바퀴 하단과 VFX 상단 사이 약8px 간격을 두는 **바닥쪽분리**.40px전체캔버스크기는우선유지.별도상태명/서로다른아이콘/턴·중첩표시는유지 | 바퀴청색장식과의겹침을먼저줄임.얼굴·HP·스킬버튼이있는쪽으로옮기지않음.필드하단/다른캐릭터충돌은향후소량비교에서확인필요 |
| B·128px 레이어/위치 | 캐릭터본체보다뒤에VFX를그리고, 현재시험점보다화면상약24px아래로이동하는1안.128px·원본알파는우선유지 | 하체차폐를줄이는비파괴후보.임의전체반투명화보다레이어/위치를먼저비교.발밑결정·경계가보이는지미검증 |
| C·HUD만 | 960폭에서는상태배지를2행/별도상태그룹으로분리하고높이를내용에맞춤.1680은실제HUD하단보다전장상단을약8px아래에두는경계안.상태위험문구의독립영역확보 | HP/MP·2x3스킬/선택강조유지.원인은기존레이아웃과VFX를분리.정확행높이/글자크기는사용자판단후고정 |

서로다른아이콘은접근성원칙이며이번에새아이콘을제작하거나임시VFX아이콘을정식승인하지않습니다. 상태별1/3·2/3은별도UI이며지속3/3은없습니다. 위8/24px은다음격리소량비교를위한가정값일뿐입니다. 현재 **LAYOUT_PROPOSAL_ONLY / NOT_IMPLEMENTED / NOT_RUN / VISUAL_PARTIAL**. 이번에Unity99PASS/12조건/49PNG재생성·전체QA반복0. 사용자선택이후에도전체회귀가아닌표적2~3안비교범위로제한해야합니다.

## 증거·보호·Git
- [로컬PNG/GUID/Scene·Prefab·Catalog참조감사](Evidence/Chapter2BackgroundAudit20261104/ReferenceAudit.json), [코드행근거](Evidence/Chapter2BackgroundAudit20261104/SourceExcerpts.txt), [ZIP단회결과](Evidence/Chapter2BackgroundAudit20261104/ZipSingleHashResults.json).
- 브랜치 `codex/chapter2-battle-background-audit-20261104`, 직전관련commit9c799745755232e0f3b5ef369547ff897cd1547e. 이번실제commitSHA는각Issue결과댓글에기록합니다. 문서·정적조사증거만LOCAL_COMMITTED/REMOTE_NOT_PUSHED.
- Unity6000.6.5f1/3ff58d469c8a 버전파일읽기확인·변경0. 개인Save/UserData접근0, 원본Editor조작/포커스전환/Play0, 원본PNG/Scene/Prefab/Catalog/meta/스크립트수정0, 사용자미커밋파일수정/스테이징0, main직접commit/Push/병합0. rootmain634a91d3e4174c87761c0d377cfa5a695ca79e7f 유지.
