# Issue #63 PilotBee 외부 원본·신규 VenomBee v3 권리 조사 — 2026-11-04

**WEB_CC_BY_3_CONFIRMED / LOCAL_LICENSE_BY_4_CONFIRMED / RIGHTS_HOLD_UNRESOLVED**

이번 조사는 출처·조건을 대조한 기록이며 저작권자의 별도 허락이나 권리 보증을 확보한 것은 아닙니다. 누구에게도 메시지를 보내지 않았습니다.

## 서로 다른 근거와 충돌

1. [OpenGameArt Pilot_Bee_Game_Character](https://opengameart.org/content/pilotbeegamecharacter): 현재 조회한 페이지는 author 2DPIXX, CC-BY 3.0, Idle/Attack 두 시트 ZIP을 표시합니다. 3.0 표기를 실제 웹에서 다시 확인했습니다.
2. 보존 원본 `Unity/Client/Assets/ThirdParty/2DPIXX/PilotBee/Original/pilot_bee.zip` 내부 LICENSE.TXT 및 옆의 추출 LICENSE.TXT는 바이트 동일하며 **Creative Commons Attribution4.0**,author **Jana Ochse / 2D!PIXX / www.2dpixx.de**를 적고 있습니다. ZIP/README/라이선스 원문은 수정하지 않았습니다.
3. 위 두 표기는 3.0 대4.0의 버전 충돌입니다. 웹이 틀렸다거나 ZIP이 우선한다고 임의 선택하지 않았습니다. 자동4.0업그레이드/명시적듀얼라이선스라고 선언하지 않았습니다. 자료만으로 어떤 의도로 두버전을 병기했는지는 확정할 수 없습니다.

## 두 버전의 상업 사용·표시 조건

[CC BY3.0 공식요약](https://creativecommons.org/licenses/by/3.0/)과 [법문4(a)/4(b)](https://creativecommons.org/licenses/by/3.0/legalcode),[CC BY4.0 공식요약](https://creativecommons.org/licenses/by/4.0/)과 [법문2/3](https://creativecommons.org/licenses/by/4.0/legalcode)을 확인했습니다. **두 BY 버전 모두 조건을 지키는 상업적 복제·배포·수정 이용을 허용**합니다. '상업사용 금지'나'구매해야만 사용가능'이라는 결론은 근거가 없습니다. 이는 이파일의올바른버전/권리자범위를확정했다는의미는아닙니다.

공통 실무 조건은 창작자와 출처, 적용 라이선스 링크, 제공된 관련 고지 보존, 수정시 변경 표시, 저작자 보증/후원을 암시하지 않는 것입니다. BY3.0은 제공된 작품명도 표시해야 하며 파생물에 원본 사용을 식별합니다. BY4.0은 수정 사실과 이전 수정 표기를 유지하고 제공된 저작권·라이선스·보증부인고지 및 실현가능한 원본URI를 보존합니다. 두버전 모두 허용권을 제한하는 추가법적조건/기술적조치를 걸지 않는 조건이 있습니다. 배포시 크레딧·동봉라이선스·수정내역과 자산에 적용되는 제한을 함께 검토해야 합니다.

버전확인 후 쓸 **표시안**(지금제품에적용한크레딧아님):

- 제목: Pilot_Bee_Game_Character
- 제작자: Jana Ochse / 2D!PIXX (OpenGameArt account:2DPIXX)
- 출처: https://opengameart.org/content/pilotbeegamecharacter
- 라이선스: **확인된버전만** CC BY3.0 (https://creativecommons.org/licenses/by/3.0/) 또는 CC BY4.0 (https://creativecommons.org/licenses/by/4.0/)
- 변경사항: 실제사용한경우에한해 런타임프레임분할·크기/방향표시조정·실제파생이미지수정내역을 구분기록. 원본파일복사만했다면그림을재작화했다고쓰지않습니다.
- 원본LICENSE와제공된고지를함께보존. 제작자가LIMITLESS를보증한다고표현하지않습니다.

제품판에 'CC BY3.0/4.0 둘중아무거나'라는문구를임의확정하지않습니다. 가장명확한해결은 권리자에게 이원본ZIP/해시가 어느버전또는선택가능한듀얼버전으로허락되는지,원하는표시를서면확인하는것입니다. 연락은이번승인범위에없어발송하지않았습니다. 회신이나명확한공식정정자료확보전현재프로젝트의 RIGHTS_HOLD를유지합니다.

## 현재 활성 외부 자산과 문서 충돌

현재02_VenomBee.asset은 Idle GUID24000000000000000000000000000004,Attack GUID24000000000000000000000000000005를참조합니다. 두GUID는 `_Project/Resources/MonsterValidation/PilotBee/pilot_bee_idle.png`·`pilot_bee_attack.png`의meta와일치합니다. 이복사본들은ThirdParty원본과각각바이트동일합니다.
- Idle SHA febcca0316167d50d4638216d9534ed2974de01c62e1c0c2daaba849726b6c63
- Attack SHA 0e8886c56d7670fb317d4cada41a03e4684f9cf97adf908f2b860740630c018e

README의'실제Field/Battle에서사용하지않음'과현행MonsterDefinition참조가충돌합니다. 원본파일을고쳐맞추지않고충돌을보고합니다. 폴더명이Validation이거나신규v3가승인됐다고기존외부원본의사용권검토가사라지지않습니다.

## 신규 제작물과 사용 범위 분리

|구분|근거/현재사용범위|권리경계|
|---|---|---|
|기존외부PilotBee Idle10/Attack10|Jana Ochse/2D!PIXX,웹BY3·패키지BY4,현행정의GUID참조|상업허용조건과출처표시필요·적용버전미확정RIGHTS_HOLD|
|신규Sting/Recover v3|담당676ec9d440c6d3e59b9285ec3d3672113128dfb0의별도생성원화/크롭·균일축소선언,바울대표미술승인6624979503|신규2PNG로승인된바이트보호·격리기술검증에사용. 별도독립권리/비침해법적보증·독점소유·자동CC라이선스취득을주장하지않음|
|기존Prepare/정상Idle2 참조|기존승인/유지PNG를변경없이재사용|미술KEEP와원본출처·사용권보증을혼동하지않음. 신규2장승인으로나머지포즈/외부원본권리문제가일괄해결되지않음|
|이번공격10슬롯QA시트|Prepare2/Sting2/Recover6 반복배치·기존Idle2연결|승인PNG의기술적조립사본. 독립10장신규작화/원본게임대체완료아님. 공개상용배포허가로승격하지않음|

신규v3담당선언: 생성원화1399×1124 2장을독립생성하고alpha정리/Crop/비율유지축소/315×253정렬했다는내용입니다. 기존PilotBee단순트레이스/색변경이아니라는제출자선언을확인했지만,원생성파일은이전ZIP에도미동봉이었고현재우리가법적독립성을감정한것은아닙니다. 새SHA는파일동일성증거이지권리증명서가아닙니다.

출처정본: [리뉴얼v3제작보고](https://github.com/HappyPaul812/Project-Limitless/blob/676ec9d440c6d3e59b9285ec3d3672113128dfb0/문서/00_프로젝트/Renewal_20261103_VenomBee_StingRecover_Continuity_v3_OfflineReview.md),[최종승인](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6624979503).

## 남은 원본 적용 조건

- PilotBee적용버전/권리자·표시정책확인 및 실제배포크레딧/고지정리.
- 새Sting/Recover·기존Prepare/Idle의출처범위와사용권근거를승인자산대장에유지. 신규아트와외부원본의독립성을선언문만으로무조건확정하지않음.
- 승인된v3의기술연결결과를바탕으로원본적용별도승인. 원본외부폴더/현행정의/GUID를이번검증에서교체하지않음.
- 실제침끝/상대Sprite접촉및게임판정은별도표적기술조건. 콜백시간통과로해소하지않음.
