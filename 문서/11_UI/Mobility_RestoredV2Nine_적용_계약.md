# Mobility 최종 복원9종 적용 계획

입력 `F:/Downloads/Limitless_Mobility_Path_9_Restored_v2_redownload.zip`. LOCAL 정본·문서화→구현→검증. 현재READY41/BLOCKED9, 기대50/0은 실제검수 전 미확정. Mobility Fighter/Guardian/Healer/Mage/Sharpshooter(Marksman)×Male/Female만9종 대상. Mobility 포함 다른41종 PNG/meta/Entry/QA/기존328Clip·READY41·사용자94항목(410Git 파일) 보호.

PNG9/Male5/Female4/Job5×Gender2·중복0/누락0·512×512 RGBA/4×4/16Frame/Cell128 확인. 원본바이트만교체하며 이미지수정/리사이즈/crop/픽셀이동/삭제/생성 없음. 기존9 AppearanceID/144SpriteID/meta/Import/PPU/Pivot/Filter/Compression·CatalogEntry/성별/Path/Job/SaveMapping 유지. 전체Catalog 재생성 금지, 통과한 대상Entry에만8Clip 연결.

새144Frame 전체 머리/귀장치/발/무기/망토 crop·조각/인접셀침범·CharacterSwap/방향/Empty/Alpha/Scale 검사. 파일명이복원본이라는 이유로READY 처리하지 않으며 BLOCKED시0-basedFrame/좌표/파츠/정확사유 기록. 이전사유는비교자료일뿐 새판정에복사하지 않는다.

9Mapping/JobPreview 및 대표Male/Female CharacterCreationPreview/World/BattleLeftIdle/Save→Continue·구버전Save/기본fallback를 격리PlayUnfocused로검증. cleanBootstrap/설정복원·컴파일/Console조회·백그라운드·포커스전환 없음. 실제걷기/물리입력 미검증. 지정변경만stage/commit, ZIP/unrelated/GitHubPush 금지.

이전QA:
- Physical_Fighter_Female.png: 셀 옆 잘린 검 파츠/Up13-15 좌측 분리 검끝. Frame1 우측 미세 잔여점.
- Physical_Fighter_Male.png: Left5-7/Right9-11/Up14-15 셀 좌측 잘린 검/장비 파츠. Frame1 좌측 잔여점.
- Physical_Guardian_Female.png: Down0 바퀴 아래 6px 분리 잔여선, Alpha117.
- Physical_Guardian_Male.png: Left6/7 좌측 잘린 장비 파츠, Right8-11 바퀴 아래 갈색/검은 분리선.
- Physical_Healer_Male.png: Left4 우측3px 지팡이 잔여파츠 Alpha191. Up15 좌측16px 금색 장비 파츠 Alpha250.
- Physical_Mage_Female.png: Down0 및 Right8-10 바퀴 아래 12/25/19/11px 갈색/검은 분리선 Alpha225-255.
- Physical_Mage_Male.png: Left5-7 좌측 잘린 망토/장비 파츠 Alpha243-253. Frame6 추가 잔여선 Alpha112.
- Physical_Marksman_Female.png: Down0 바퀴 아래 7px 검은/갈색 분리선 Alpha195.
- Physical_Marksman_Male.png: Down0-3 바퀴/발 아래5/8/5/5px 분리 조각 Alpha198-240.
