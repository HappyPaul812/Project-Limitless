# Issue #63 지시 정정 — PilotBee 최종 미사용·빌드 제외·Prepare/Idle 출처 확인

**PILOTBEE_NOT_FOR_FINAL / LICENSE_VERSION_INVESTIGATION_STOPPED / CURRENT_BUILD_EXCLUSION_NOT_COMPLETE / V3_ART_APPROVAL_PRESERVED / NOT_UNITY_APPLIED**

바울 대표의 최신 명시 지시를 적용합니다. PilotBee CC BY3.0/4.0 충돌 해결·저작자 확인·상업 사용 조건 조사는 **중단**했습니다. 이전 조사는 과거 근거로 보존하되, 신규 독침벌 개발의 선행조건으로 라이선스 버전 해결을 요구하지 않습니다. 이전 보고318e14bf의 적용조건①은 **PilotBee 및 파생물의 출시 구성 제외 확인**으로 대체합니다. 외부 출처 연락0·새 라이선스 조사0.

## 현재 종료 판정

아직 '최종 빌드에 외부 원본이 없음'으로 종료할 수 없습니다. 현재 원본 프로젝트의 아래2PNG가 Resources 안에 그대로 남고 02_VenomBee.asset이 GUID로 직접 참조합니다.
- Assets/_Project/Resources/MonsterValidation/PilotBee/pilot_bee_idle.png,GUID24000000000000000000000000000004
- Assets/_Project/Resources/MonsterValidation/PilotBee/pilot_bee_attack.png,GUID24000000000000000000000000000005

BattleSceneController.cs의 Resources.LoadAll<MonsterDefinition>("MonsterDefinitions") 경로에도 해당 정의가 포함됩니다. 이름이Validation이거나검증Scene이BuildSettings에서빠졌다는이유로제외됐다고할수없습니다. [Unity6 공식Resources문서](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Resources.html)는 Resources내자산이빌드에포함됨을설명합니다. 현재 일반Player빌드의포함대상이라는 **정적확인**이며,최종배포용Player파일/BuildReport를이번에생성하거나열어본것은아닙니다. 따라서최종산출물제외PASS를주지않습니다.

원본적용별도승인이유지되므로이번에삭제·이동·GUID/MonsterDefinition/BuildSettings교체를하지않았습니다. 필요한향후범위는출시용정의의신규아트전환및외부원본/파생복사본의Resources·직접참조·빌드파이프라인잔존제거와최종BuildReport확인입니다. 단순히사용하지않는것과빌드에바이트가없는것은다릅니다. ThirdParty원본은승인없이파괴하지않고출시Assets밖보관을검토할수있지만이번이동0.

## 신규 연결용 Prepare·Idle 3개 출처

현재격리15PASS가사용한3개는 **외부원본시트의직접셀복사와일치하지않습니다**. Prepare를외부Attack10셀,Idle2를외부Idle10셀각각과동일규격RGBA화소로대조해직접일치셀0을확인했습니다. 실제이미지도구PilotBee만화벌과다른새독침벌외형입니다. 다만'원본셀과다름'만으로모든파생가능성의부재를증명하지는않습니다.

실파일SHA와납품연결:
- Prepare e337921236b7b947f92bbb5d88824bc6729d6668a050a85fc2f519ff4df26212는11/03납품 **Attack_Prepare_REPAINT_v2_315x253.png**와바이트동일. 해당README는 **new source artwork**로새공격3포즈를만들었다고명시합니다.
- IdleFolded d17114176c13c2911cd533a0e7a363a1f4ae2ca110b540bcec5d63a2aa941605,IdleSpread d35f4583c0fa119396a40a6637f48b36b677fe00327640c655d86bde3dd47820는11/03 **New5**의Idle_Folded/Spread와각각바이트동일. 그납품README는 **10/29기존5포즈를재활용하지않고11/03새생성시트에서5PNG를분리**했다고명시합니다. QA에는생성시트SHA2ee3e66c0bb5a65472c1b70b65770a3589b567a873ad95a4b2e0076787c5d291도기록되어있습니다.

이후패키지의KEEP/UNCHANGED는'이미납품된신규포즈를보존'한뜻으로추적되며,그단어만으로구외부PilotBee라고분류할수없습니다. **신규생성·보완제작이력과기존외부원본의직접픽셀재사용아님은확인**했습니다. 다만원생성시트/Prepare생성마스터가이패키지에없어제작입력전체를직접재현한것은아닙니다. 실제소스없는부분까지완전독립성을단정하지않습니다.

## 기획 기록과의 차이

[최신기획6625568292](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6625568292)는3개를'원본PilotBee계열'로파악하여신규3포즈를발주했습니다. 이번실파일계보/납품README는그분류와충돌합니다. **기획을임의변경하거나3포즈발주를취소하지않고이근거를보고**합니다. 파일명KEEP만근거로무조건외부원본이라거나반대로독립권리가증명됐다고쓰지않습니다. 최종선택전에리뉴얼제작출처와이3개SHA의분류를일치시키는것이필요합니다. 이번추가이미지제작·기존그림수정0.

## 유지·다음범위

승인Sting28500201…·Recover69d0944b… **USER_ART_APPROVED/BYTE_KEEP** 유지,기존격리15PASS/후보A153PASS보존·재실행0. 신규전투연결작업은진행가능하되외부원본을사용하지않는최종구성을명시하고별도원본적용승인을유지합니다. 이번에미술승인을취소하거나동일포즈의재작화를강제하지않습니다.

**현재남은조건은라이선스충돌해결이아니라출시자산배제와최종구성출처확인**입니다. 신규3포즈납품/최종구성확정후외부바이트가최종빌드에없다는표적확인을하면옛라이선스문제는과거이력으로만남기고종료할수있습니다. 현재Resources잔존이확인되어그종료조건은아직미충족입니다. 원본Player출력과격리QA전용출력을혼동하지않습니다.

[포즈셀대조](Evidence/Issue63PilotBeeExclusion20261104/PrepareIdleProvenance.json)·[실파일계보](Evidence/Issue63PilotBeeExclusion20261104/NewArtProvenanceChain.json)·[Resources잔존](Evidence/Issue63PilotBeeExclusion20261104/BuildExclusionReadOnly.json)·[New5제작선언](Evidence/Issue63PilotBeeExclusion20261104/11Nov03_New5_Source_Readme.txt)·[Preparev2제작선언](Evidence/Issue63PilotBeeExclusion20261104/AttackPrepareV2_Source_Readme.md).

Unity6000.6.5f1유지·새Unity실행/PlayMode/빌드0·개인Save접근0·원본Unity/Editor/승인아트/main/사용자미커밋수정0. 원본main634a91d3e4174c87761c0d377cfa5a695ca79e7f·tracked dirty308유지. Push/병합0.

브랜치 codex/issue63-pilotbee-exclusion-audit-20261104,직전318e14bfe35260ec4ad87f0d716a1ef13d3e8fff. CODEX_LATEST/CURRENT_STATUS갱신·LOCAL_COMMITTED/REMOTE_NOT_PUSHED. 실제commit은Issue63결과댓글기록. 과거권리조사원문은삭제/덮어쓰기하지않고이번결정으로대체상태를명확히합니다.
