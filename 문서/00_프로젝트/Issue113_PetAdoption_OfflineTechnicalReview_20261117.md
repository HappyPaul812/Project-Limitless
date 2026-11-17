# Issue113 — 펫 분양 담당 후보 오프라인 기술 검수

**ART_CANDIDATE / OFFLINE_FILE_QA_PASS / USER_ART_REVIEW_PENDING / UNITY_NOT_APPLIED**

## 최신 근거와 원본 상태

Issue113 납품 댓글6942788970 및 최신 원격 docs/art-renewal WORLD_ART_RENEWAL_LATEST(221ec3dc…)와 docs/planning PLANNING_LATEST를 확인했습니다. 원격 납품 기록의 ‘현재 OGA17_EquipmentShop/OGA16_Bank’는 원격 main의 이전 경로입니다. 현재 로컬은 #111 완료 후 **arbel-pet-adoption=OGA17_EquipmentShop_Original_Approved**, **arbel-pet-management=OGA16_Bank_Original_Approved**이며 이를 되돌리지 않았습니다. 레온도 후보가 아닌 이미 승인·원본 적용 상태입니다.

기승인 OGA17·OGA16 실물은 원본 Resources에 존재하며 SHA 대조했습니다. 분양 담당의 독립 기승인 제품 PNG는 현재 원본 Assets 파일명·기존 인계/Issue 범위에서 발견되지 않았습니다. 후보 #113은 승인 전이며 정식 적용 원본으로 취급하지 않습니다. 사용자 PC 모든 경로·모든 Git 브랜치 전수조사라는 뜻은 아닙니다.

## 실제 ZIP/파일 검수

- `F:/Downloads/Limitless_PetAdoption_113.zip`: **722,753bytes / 9파일**, SHA256 `b99c853fb4c92f8f8f1a7b6ef10c6c0b83a18d2759f565b5387e20d309bdc307`.
- CRC PASS, Manifest payload7/7 bytes+SHA PASS, SHA256SUMS8/8 PASS, 파일 커버리지·비순환·ZIP 경로 안전성 PASS.
- `Candidates_512/Arbel_PetAdoption.png`: **134,346bytes / 512×512 RGBA / 투명 정지1포즈**, SHA256 `d64c342bd4bd8953bafce1feb8949d3cb5ce1a4b67ecf88cda3a6757beb4e47d`.
- alpha0 픽셀211,984/262,144(약80.87%), alpha>8 bbox(155,59)-(357,471), 외곽 alpha접촉0. 한512셀, 시트·애니메이션 아님.
- `Previews/Arbel_PetAdoption_128px.png`: 128×128 RGBA, 12,597bytes, SHAd1639f400205a421293d052e60fc927b5cd7f7920225c34b329357477cea626a.
- `Previews/Arbel_PetAdoption_40px.png`: 40×40 RGBA, 2,067bytes, SHAb1a45b9a6fb7724cad1713451ef031b844abe7ebdbdf21d42b2cc86d508d981d.
- 128/40 두 축소본은 실제512PNG의 LANCZOS 축소와 전 RGBA 픽셀 일치.
- `Review_OFFLINE/Issue113_Actual_512_128_40.png`: 2690×880 RGB, 577,396bytes, SHA256 `8e7c89198f096844131c1558282ab216f7ce63a9afa26e1ccc189c1ae860b53a`. 기존실물 비교판 존재·시각 확인. QA.json/README/SourceDeclaration/Manifest/SUMS 포함.

## 기술 호환성과 미술 검토 의견

원본 OGA17은 PPU358.75/Pivot(255.5,42), alpha>8 가시410px(약1.143WU), OGA16은 PPU320.25/Pivot(269,43)·가시366px(약1.143WU). 후보 가시412px를 같은 기준8/7WU에 맞추면 **가안 PPU360.5 / Pivot(256,41)px=(.5,.080078125)**로 접지·체급을 맞출 수 있습니다. 이는 파일 수학 검토용이며 Unity import 설정·최종 승인 수치가 아닙니다. 실제 발·그림자/방향/라벨/Collider/역할UI 호환성은 미술 승인 후 최소 Delta QA가 필요합니다.

512/128에서는 묶은 머리·리본·환영 손짓·올리브/크림 복장으로 OGA17의 고글/공구·앞치마 및 OGA16의 안경/푸른 코트·장부와 구별됩니다. 40px에서도 올리브 실루엣과 한쪽 팔 자세로 대체로 구별되지만, 빗·작은 주머니 같은 돌봄 소품의 뜻은 읽기 어렵습니다. 기존 평면·굵은 윤곽 NPC에 비해 채색 명암과 의상 소품이 세밀하고, 승인 레온 쪽 표현에 더 가깝습니다. 기술 결함은 발견되지 않았으나 이 차이를 수용할지는 바울 대표 미술 승인 사항입니다. 새 동물/신상/기능을 확정하지 않았습니다.

## 실물 비교 자료·확인 절차

1. 납품512/128/40 원본비교판: `Temp/Issue103Defeat113Review20261117/Pet113/Review_OFFLINE/Issue113_Actual_512_128_40.png`.
2. 현재 원본의 승인 OGA17·OGA16·레온과 후보를 직접 읽어 만든 추가 비교판: `Temp/Issue103Defeat113Review20261117/Pet113_ActualComparison.png`. 첫 행은512캔버스를256화면으로 표시, 다음128/40 실제크기, 맨아래는 같은가시높이 기준입니다. 기존 원화 바이트 수정0. **오프라인 합성 비교판이며 Unity 화면이 아닙니다.**

지금 확인은 위 이미지 열람입니다. 게임 속 현재 분양 담당은 Arbel(5,-3)에서 승인 OGA17을 계속 사용합니다. #113 모습을 게임에서 확인할 수 있다고 안내하지 않습니다. 승인 후 arbel-pet-adoption 및 공통PetAdoption fallback 영향/화면/라벨/업무UI를 별도 검증해야 합니다.

## 적용 금지 준수·기록

후보는 이번 Temp에만 추출. **원본/격리 Unity Import0, 신규PNG/meta/GUID0, 카탈로그·Scene·Prefab·런타임 적용0**, 제품 커밋없음. 본 작업을 #111 외형 참조 정리의 재작업으로 취급하지 않습니다. Placeholder/레온/Main04/07/NPC12/아르벨 승인 NPC·Save/UserData/Discord·#85 HOLD와 기존 dirty/untracked 보호.

제품 변경0. 보고서와 WORLD_ART_RENEWAL_LATEST만 본 이슈의 문서 변경입니다. 브랜치 `codex/issue103-defeat113-review-20261117`; 문서SHA는 Final.json 및 Issue113 댓글 참조. 원본rootmain HEAD52f84e643d5d860665eadfe1614a30c745209f00 유지, main 직접commit/Push/Merge0. 바이너리/보호목록 원격 업로드0.

공통 원본 Editor는 Unity6000.6.5f1·compileFalse·오류0/경고42·Bootstrap/PlayFalse/dirtyFalse 유지. #113 자체는 오프라인만 수행했으므로 후보 Unity compile/Console/게임 접지 PASS를 주장하지 않습니다. 실제 증거: Temp/Issue103Defeat113Review20261117/audit.json, pet-compatibility.json, Final.json.
