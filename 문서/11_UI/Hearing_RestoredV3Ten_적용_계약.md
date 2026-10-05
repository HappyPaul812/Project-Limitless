# Hearing v3 복원10종 적용 계획

입력 `F:/Downloads/Limitless_Hearing_Path_10_Restored_v3.zip`. LOCAL 정본·문서화→구현→검증. 현재READY30/BLOCKED20, 기대40/10은 실제검수 전 미확정. Hearing Fighter/Guardian/Healer/Mage/Sharpshooter(Marksman)×Male/Female만10종 대상. Mobility 포함 다른40종 PNG/meta/Entry/QA/기존240Clip·READY30·사용자94항목(410Git 파일) 보호.

PNG10/Male5/Female5/Job5×Gender2·중복0/누락0·512×512 RGBA/4×4/16Frame/Cell128 확인. 원본바이트만교체하며 이미지수정/리사이즈/crop/픽셀이동/삭제/생성 없음. 기존10 AppearanceID/160SpriteID/meta/Import/PPU/Pivot/Filter/Compression·CatalogEntry/성별/Path/Job/SaveMapping 유지. 전체Catalog 재생성 금지, 통과한 대상Entry에만8Clip 연결.

새160Frame 전체 머리/귀장치/발/무기/망토 crop·조각/인접셀침범·CharacterSwap/방향/Empty/Alpha/Scale 검사. 파일명이복원본이라는 이유로READY 처리하지 않으며 BLOCKED시0-basedFrame/좌표/파츠/정확사유 기록. 이전사유는비교자료일뿐 새판정에복사하지 않는다.

10Mapping/JobPreview 및 대표Male/Female CharacterCreationPreview/World/BattleLeftIdle/Save→Continue·구버전Save/기본fallback를 격리PlayUnfocused로검증. cleanBootstrap/설정복원·컴파일/Console조회·백그라운드·포커스전환 없음. 실제걷기/물리입력 미검증. 지정변경만stage/commit, ZIP/unrelated/GitHubPush 금지.

이전QA:
- Hearing_Fighter_Female.png: 이전4/9 발밑선 해결,8은축소됐지만[60,121,65,122)5px 검은선잔존. 새Frame6 우측망토옆[124,110,125,113)3px 검은조각,10 머리/망토옆[23,74,24,75)1px 분리점. 머리/귀장치/검/방향정상.
- Hearing_Fighter_Male.png: 이전Left 큰발조각과Up13/15 머리위선 해소. Left4-6 발밑2-3px선 및5 우측[123,108,125,112)5px 조각잔존. 새Down1 우측[120,107,125,118)34px 빨간/검은검파츠, Right8-11 발아래[60,119,71,122)26px+[56,121,58,122)2px/[57,119,67,122)25px/[57,122,68,125)25px/[60,122,71,125)27px 살색/검은분리파츠.
- Hearing_Guardian_Female.png: 이전Left4/5 조각해소. Down1/2 우측[122,109,124,116)10px/[123,108,125,116)13px,Left6[123,102,125,107)6px 검은파츠잔존. Right9 좌측[3,101,7,113)35px 검끝조각과발밑[59,118,74,121)32px. 새Up14 좌측[6,85,10,97)34px 검파츠.
- Hearing_Guardian_Male.png: 이전Down1 조각해소. Right8 우측[123,81,125,87)10px 검은/금색절단파츠잔존. 새Right10 우측[124,82,125,85)3px 조각. 귀장치/머리/방향정상.
- Hearing_Healer_Female.png: 이전큰발조각과Up13 상단선해소. Frame0 우측[122,36,125,52)37px 지팡이잘린파츠,4[121,38,125,53)38px 및머리위[66,7,71,8)5px 검은선,7 좌측[3,63,7,79)43px 잘린장식. Right9/10/11 좌측지팡이파츠[3,34,7,52)47px/[3,33,10,54)91px/[3,29,13,55)184px. 머리위선9[39,3,47,5)13px/11[46,5,53,7)11px/Up15[87,3,93,5)9px. Up15좌측[5,28,8,39)24px조각. 정상음파효과자체는실패로세지않음.
- Hearing_Healer_Male.png: 이전0 우측큰파츠해소. Right9 좌측[3,42,5,49)12px,11 좌측[4,33,10,50)58px 잘린빨간/금색지팡이파츠잔존. Left5 좌측[3,93,4,101)8px 검은선. 발아래Frame8[59,120,62,121)3px,10[57,122,61,123)4px+[50,122,53,123)3px+1px. 1 우측3px/3,5에1px 잔여점. 귀장치/방향정상, 정상지팡이발광과구분.
- Hearing_Mage_Female.png: 이전Frame1 큰장식조각축소. 새Down2 좌측[3,68,9,106)142px 금색/빨간매달린장식파츠,Left6 좌측[3,64,9,102)140px 동일이웃장식. Left5[3,76,7,94)43px+[3,68,5,75)11px 잔존. Right9[4,28,6,41)21px,10[3,30,5,43)22px 좌측지팡이/장식절단조각,10우측9+3px. 0/1/3/7/14 작은잔여점. 정식귀장치보존/방향정상.
- Hearing_Mage_Male.png: Right9 왼쪽[3,45,6,59)33px 빨간/흰지팡이원잘린파츠잔존. Right10[3,51,4,55)4px와11[3,51,4,54)3px잔여선,10[33,65,34,66)1px 및Up14[83,48,84,49)1px분리점. 다른행귀장치/머리/방향정상.
- Hearing_Marksman_Female.png: 이전3/13/15 셀옆큰파츠해소. Left4/6/7 발아래[70,122,79,124)14px/[67,123,73,124)6px/[69,123,74,124)5px잔존. 새Right8/9 발조각[51,117,71,122)58px/[55,119,73,122)34px. Up12/14 포니테일상단이셀y=3부근에서넓은수평단면으로잘림(셀여백은있으나원래머리끝윤곽불완전). 0/9/11 작은분리점. 귀장치보존/방향정상.
- Hearing_Marksman_Male.png: 이전Right8-11 머리위선/Left5 큰셀옆파츠해소. 새Up13 머리위[44,3,52,4)8px+[78,3,82,4)4px검은선,Up15[80,3,84,4)4px검은선잔존. Left4발아래1px/5발아래2+2px/7발아래3+2px분리점. 귀장치/방향정상.

실행완료: Hearing10종 READY/전체40·10. 기존Marksman명명과ID에Sharpshooter원본대응, 대상Entry80Clip만연결. 다른40종/모든C#보존·88PASS/0FAIL. 최소추가수정리스트없음, 다음Mobility10종ZIP수령후계획수립준비. [최신QA](Hearing_RestoredV3Ten_QA.md).
