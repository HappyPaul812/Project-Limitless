# Mobility 최종 복원10종 적용 계획

입력 `F:/Downloads/Limitless_Mobility_Path_10_Restored_Final.zip`. LOCAL 정본·문서화→구현→검증. 현재READY40/BLOCKED10, 기대50/0은 실제검수 전 미확정. Mobility Fighter/Guardian/Healer/Mage/Sharpshooter(Marksman)×Male/Female만10종 대상. Hearing 포함 다른40종 PNG/meta/Entry/QA/기존320Clip·READY40·사용자94항목(411Git 항목) 보호.

PNG10/Male5/Female5/Job5×Gender2·중복0/누락0·512×512 RGBA/4×4/16Frame/Cell128 확인. 원본바이트만교체하며 이미지수정/리사이즈/crop/픽셀이동/삭제/생성 없음. 기존10 AppearanceID/160SpriteID/meta/Import/PPU/Pivot/Filter/Compression·CatalogEntry/성별/Path/Job/SaveMapping 유지. 전체Catalog 재생성 금지, 통과한 대상Entry에만8Clip 연결.

새160Frame 전체 머리/휠체어/바퀴/발/무기/망토 crop·조각/인접셀침범·CharacterSwap/WheelchairDesignSwap/방향/Empty/Alpha/Scale 검사. 파일명이복원본이라는 이유로READY 처리하지 않으며 BLOCKED시0-basedFrame/좌표/파츠/정확사유 기록. 이전사유는비교자료일뿐 새판정에복사하지 않는다.

10Mapping/JobPreview 및 대표Male/Female CharacterCreationPreview/World/BattleLeftIdle/Save→Continue·구버전Save/기본fallback를 격리PlayUnfocused로검증. cleanBootstrap/설정복원·컴파일/Console조회·백그라운드·포커스전환 없음. 실제걷기/물리입력 미검증. 지정변경만stage/commit, ZIP/unrelated/GitHubPush 금지.

이전QA:
- Physical_Fighter_Female.png: Left/Right 발 아래 다른 행의 머리 조각, Right 및 Up 머리 상단 수평 절단. 일부 검 끝 조각이 셀 옆에 분리됨.
- Physical_Fighter_Male.png: Left/Right 발 아래 머리·무기 조각과 옆 셀의 검/망토 조각. Up 머리 상단 수평 절단.
- Physical_Guardian_Female.png: Left 머리 위 분리된 바퀴 조각, Right 발 아래 다음 행 머리 조각. Up 머리 상단 수평 절단.
- Physical_Guardian_Male.png: Right 발 아래 다음 행 머리 조각, Up 머리 상단 수평 절단.
- Physical_Healer_Female.png: Left/Right 발 아래 다른 행의 금발 머리 조각. Up 머리 상단 수평 절단.
- Physical_Healer_Male.png: Left 머리 위 바퀴 조각, Right 발 아래 머리·장비 조각. Up 머리 상단 수평 절단.
- Physical_Mage_Female.png: Left/Right 발 아래 다른 행의 머리 조각. Up 머리 상단 수평 절단.
- Physical_Mage_Male.png: Left 머리 위 바퀴 조각, Right 발 아래 다음 행 머리 조각. Up 머리 상단 수평 절단.
- Physical_Marksman_Female.png: Left/Right 발 아래 다른 행의 금발 머리 조각. Up 머리 상단 수평 절단.
- Physical_Marksman_Male.png: Down 발 아래 잔여 조각, Left/Right 발 아래 다음 행 머리 조각. Up 머리 상단 수평 절단.
