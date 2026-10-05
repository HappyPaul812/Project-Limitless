# Hearing v2 복원10종 적용 계획

입력 `F:/Downloads/Limitless_Hearing_Path_10_Restored_v2.zip`. LOCAL 정본·문서화→구현→검증. 현재READY30/BLOCKED20, 기대40/10은 실제검수 전 미확정. Hearing Fighter/Guardian/Healer/Mage/Sharpshooter(Marksman)×Male/Female만10종 대상. Mobility 포함 다른40종 PNG/meta/Entry/QA/기존240Clip·READY30·사용자94항목(410Git 파일) 보호.

PNG10/Male5/Female5/Job5×Gender2·중복0/누락0·512×512 RGBA/4×4/16Frame/Cell128 확인. 원본바이트만교체하며 이미지수정/리사이즈/crop/픽셀이동/삭제/생성 없음. 기존10 AppearanceID/160SpriteID/meta/Import/PPU/Pivot/Filter/Compression·CatalogEntry/성별/Path/Job/SaveMapping 유지. 전체Catalog 재생성 금지, 통과한 대상Entry에만8Clip 연결.

새160Frame 전체 머리/귀장치/발/무기/망토 crop·조각/인접셀침범·CharacterSwap/방향/Empty/Alpha/Scale 검사. 파일명이복원본이라는 이유로READY 처리하지 않으며 BLOCKED시0-basedFrame/좌표/파츠/정확사유 기록. 이전사유는비교자료일뿐 새판정에복사하지 않는다.

10Mapping/JobPreview 및 대표Male/Female CharacterCreationPreview/World/BattleLeftIdle/Save→Continue·구버전Save/기본fallback를 격리PlayUnfocused로검증. cleanBootstrap/설정복원·컴파일/Console조회·백그라운드·포커스전환 없음. 실제걷기/물리입력 미검증. 지정변경만stage/commit, ZIP/unrelated/GitHubPush 금지.

이전QA:
- Hearing_Fighter_Female.png: 발 아래 검은 외딴 선. Frame4 bbox[62,124,69,125),8[60,121,70,123),9[60,121,66,123)+1px,10[60,122,65,123). 본체와 분리된7/16/11/5px 잔여 조각.
- Hearing_Fighter_Male.png: Left4-7 발 아래 분리된 살색/검은 파츠84/68/76/68px, bbox 각각[65,121,89,126),[66,122,89,126),[62,122,86,126),[65,122,88,126). Up13/15 머리 위 검은 수평선9/8px bbox[77,2,86,3),[76,2,84,3).
- Hearing_Guardian_Female.png: Down1/2 및 Left4-6 오른쪽 셀 가장자리의 분리된 검은/금색 잘린 파츠. Frame1[123,111,126,122)24px,2[123,116,125,124)14px,5[123,104,126,112)19px. Right9 왼쪽[2,102,6,114)35px 검 파츠와 발 아래[56,118,69,121)26px 분리 조각.
- Hearing_Guardian_Male.png: Frame1 우측 아래 검은 세로 조각[125,114,126,122)8px, Frame8 우측[124,81,126,88)11px 검은/금색 외딴 파츠. 셀 여백 안으로 옮겨져도 본체와 분리된 절단 조각이 남음.
- Hearing_Healer_Female.png: Left4-7 발 아래 살색/검은 발 파츠60/48/43/47px 분리. Frame4[58,121,78,126),5[61,122,77,126),6[58,122,73,126),7[62,122,77,126). Right10 왼쪽[2,31,7,49)68px 지팡이 효과/테두리 절단 조각. Up13/15 머리 위 검은 조각[83,2,86,3)3px/[82,2,90,4)13px. 정상 지팡이 주위의 의도된 음파 발광 자체는 문제로 세지 않음.
- Hearing_Healer_Male.png: Frame0/1/5 우측 셀 가장자리 지팡이 발광/테두리 잘린 분리 파츠[123,42,126,53)26px/[121,34,126,48)48px/[123,46,126,58)27px. Right9 좌측[2,42,4,49)11px 및 우측[124,89,126,97)12px, Right11 좌측[2,31,9,51)92px 금색/빨간 지팡이 파츠가 본체와 떨어져 절단된 모양으로 남음. 정상 음파 효과와 구분.
- Hearing_Mage_Female.png: 셀 좌우에 이웃 지팡이/장식 절단 조각 잔존. 대표Frame1 왼쪽[2,67,7,95)80px+[2,98,6,107)29px, Left5 왼쪽[2,60,10,94)197px 빨간/금색 지팡이·매달린 장식 조각, Left6[2,77,8,97)77px, Right9 왼쪽[2,23,8,42)62px 빨간 장식 절단 조각. 머리/귀장치는 개선됐으나 이웃 파츠 제거 미완료.
- Hearing_Mage_Male.png: Right9/10 왼쪽 가장자리[2,43,7,60)54px/[2,47,5,58)27px 빨간/흰 지팡이 원 테두리 절단 조각이 본체와 분리됨. 정상 음파 발광으로 볼 수 없는 잘린 곡선 파츠.
- Hearing_Marksman_Female.png: Frame3 왼쪽[2,67,6,82)43px 머리/파츠 조각. Left4/6 및 Right11 발 아래 살색 발 조각[69,122,86,126)43px/[64,123,77,126)28px/[51,120,73,125)79px. Up15 왼쪽[2,74,7,88)55px 잘린 파츠. Frame5/7/13에도 작은 잔여 조각.
- Hearing_Marksman_Male.png: Left5 왼쪽[2,84,5,91)16px 잘린 파츠. Right8-11 머리 위 분리된 검은 수평선/머리 조각7/18/7/17px: [48,6,55,7),[33,6,43,8),[36,2,43,3),[39,2,49,4). Frame7/15 머리 위에도2/3px 잔여 조각.
