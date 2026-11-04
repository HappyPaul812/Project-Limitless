# Issue #92 실제 Chapter1 공식3종·Chapter2 신규2종 화풍 비교

**CH1_CH2_ACTUAL_PIXEL_STYLE_COMPARISON_DELIVERED / TWO_CANDIDATES_RECOMMENDED_FOR_USER_REVIEW / USER_VISUAL_APPROVAL_PENDING / UNITY_NOT_TESTED / NOT_UNITY_APPLIED**

최신기획6627089239/4bdde36e·납품13c872a54c049672085bf385256dea4a9e985b91를확인했습니다. 기존문자기준판과달리실제Ch1 Forest/Grassland/SilentCatacombs PNG3장과이번Ch2Ashen/Obsidian2장을직접열었습니다. 각1672×941을1:1로배치하고지면/원경256px동일좌표크롭으로질감/명암/거리차이만비교했습니다. 실제전투배치QA는하지않았습니다.

[실제5장동일배율화풍판](Evidence/Issue92Followup20261104/92_Ch1_Ch2_Actual_1to1_Style.png)·[원본패널화소5/5일치근거](Evidence/Issue92Followup20261104/PanelPixelEvidence.json). 원본패널리사이즈/색보정/샤프닝0. 이것은검수문서이미지이며새배경원화가아닙니다.

## 관찰과 추천

- **공통화풍:**Ch1의불규칙작은색면·재질별명암·거리별세부감소가Ch2의재/암반/층상절벽표현에도보입니다. Ch1-vsCh3의큰단순도형과는달리이번Ch2는세부질감이풍부한회화적방향으로읽힙니다. 단,도트논리격자가같다는측정결과는아닙니다.
- **Forest/Grassland 대비:**Ch1은잎/이끼·풀/흙/돌의작은유기적변화와풍부한녹색계열빛이주도합니다. Ch2는회보라저채도·넓은가로암반띠와균열이더크고반복됩니다. 소재/지역차이를무조건화풍결함으로처리하지않습니다.
- **Catacombs 대비:**석재의불규칙모서리·국소반사광·층별거리명암이유사한재질기준입니다. Ch2는성벽/계단의건축구획대신자연암반면과균열,안개로공간을나눕니다.
- **AshenWasteland:**낮은대비평지와중경바위·원경불길/연무가거리순서를만듭니다. 넓은평지의작은파편/가로붓결반복은Ch1지면보다균일하게보일수있습니다. 검토영역[200,400,1470,900].
- **ObsidianRift:**가장자리의크고각진검은절벽과짙은중앙암반,절제된보라반사/균열광이주도합니다. Catacombs보다빛이적고큰가로암반면반복이강합니다. 검토영역[240,400,1480,900]. 어둡다는것만으로품질불합격이라고하지않습니다.

**두후보유지·바울대표최종미술검토추천**입니다. 큰화풍이질감은제한적이지만화풍동등PASS/상업출시승인을확정하지않습니다. 이번근거만으로전면재작화/추가필드양산요구0.

원본좌상단기준·우/하단제외. 동봉BattleUI도형목업은실Unity/실Sprite가아닙니다. Obsidian에서어두운캐릭터/상태·VFX/휠체어대비는미술승인후최소실행항목으로남기고이번화풍비교에서가독성PASS를주지않습니다. Field11DeepCore기존fallback/Ch1공식·Ch3후보는보호하고카탈로그/Scene에연결하지않았습니다.

비교판실제SHA256 **707cd075cfda568edd228e8945082ad8ec097e160f7f04b0e3d1994595bdae03**,파일경로 `F:\study\codex\Project-Limitless\Temp\Issues638992Followup20261104\92_Ch1_Ch2_Actual_1to1_Style.png`.


## 실제 F: ZIP·보호·보고 경계

`F:\Downloads\LIMITLESS_ISSUE92_CH2_BATTLEBG_2PILOT_20261104.zip` 10,175,488bytes. 전체SHA256 **e439cd32bda173063ee05fd3cb3fd8a099dc4f3a6a67b805150b2777ba6b7ff0**,기존납품보고와정확일치. 이번전체ZIP해시는**1회만**계산했습니다. CRC/Manifest/SUMS/기존QA 전수대조는반복하지않고기존납품·기획검수기록을재사용했습니다. 선택이미지를읽어비교한것은새전체QA가아닙니다. [실측기록](Evidence/Issue92Followup20261104/ActualWholeZipHash.json).

Unity6000.6.5f1고정·이번Unity실행/Import/PlayMode/빌드0. 개인Save/UserData·원본Editor/Unity/Scene/Prefab/Catalog/GUID·사용자미커밋·승인아트수정0. 원본main634a91d3e4174c87761c0d377cfa5a695ca79e7f·tracked dirty308보호. main직접commit/Push/병합0. 기존153/15/99/50QA재실행·새PASS추가0. USER_VISUAL_REVIEW_PENDING / NOT_UNITY_APPLIED.

브랜치 codex/issues63-89-92-followup-20261104. CODEX_LATEST/CURRENT_STATUS갱신. 이이슈관련실제commit은각Issue결과댓글참조. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 비교판/근거는로컬보존,원격에는실제댓글/경로/SHA만기록합니다.
