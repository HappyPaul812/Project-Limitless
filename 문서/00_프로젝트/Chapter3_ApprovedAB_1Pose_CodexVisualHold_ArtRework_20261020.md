# LIMITLESS — Chapter3 서리들쥐 A·설빙의 폭군 B 픽셀아트 1포즈 검수 HOLD / 미술 재작업 지시 (2026-10-20)

- 담당 기획: docs/planning. 신규 아트: docs/art-new, GitHub Issue #30.
- 최신 Codex 사용자 전달 보고: **ZIP·PNG 구조 PASS, 시각 완성도 '수정 권고·합격 보류'**. 검수 커밋 `d2893591`. 보고 이미지: Chapter2 공식 몬스터 5종/보스와 128px에서 동일 게임 UI 배율 비교, 실제 Unity 배치가 아니라 정적 Code/Sprite 규격 자료.
- 상태: `CONCEPT_A_B_USER_SELECTED / PIXELART_1POSE_V1_TECH_PASS / PIXELART_1POSE_V1_ART_HOLD / REDRAW_2_POSES_REQUESTED / USER_NEW_ART_APPROVAL_PENDING / UNITY_NOT_APPLIED / ANIMATION_NOT_AUTHORIZED`.
- 본 문서는 **기존 바울 대표의 서리들쥐 고해상도 A안 / 설빙의 폭군 고해상도 B안 콘셉트 선택을 유지**하며 1포즈 픽셀 마감 방식만 개선한다. 사용자가 이번 새 픽셀아트를 승인했다고 주장하지 않음.

## 독립 시각 QA 결과

- `Sprites/FrostFieldMouse_A_PixelArt_1Pose.png`: 512 RGBA ZIP PASS지만 실제 정보량/원본 형태는 **75×49픽셀**을 최근접 3배 확대. 128px 표시에서 **크고 둥근 몸통·머리/완구·마스코트 인상**, 앞니·긴 꼬리·발톱·야생성이 충분히 식별되지 않음. Chapter2 공식 몬스터 5종에 비해 동물체형·털·그림자 정보 부족.
- `Sprites/FrostTyrant_B_PixelArt_1Pose.png`: 512 RGBA ZIP PASS지만 실제 정보량/원본 형태는 **142×121픽셀**을 3배 확대. 뿔/두개골 윤곽 식별, 하지만 흉곽·앞발/발톱 질감이 뭉치고 Chapter2 공식 보스보다 무게/크기·위협감·거대한 냉기 존재 표현 약함.
- Codex ZIP/Manifest/PNG 검사 PASS는 **그래픽 완성도/미술 승인과 독립**. 512 캔버스 크기만 채우거나 75/142 Source를 더 크게 확대해도 질감·해부학·실루엣 정보가 증가하지 않음. 동일 게임 UI/표시크기의 소수 예시와 비교해 조정.

## 메인 아트 담당 재작업 — 1포즈 단 2개

1. **원본 고해상도 A/B의 형태/분위기를 기준**으로, 대상 실루엣을 실제 게임 내 표시 크기를 먼저 정한 후 **그 크기에 맞는 정보량의 오리지널 픽셀 디테일을 새로 그린다**. 기존 75×49/142×121 소스 단순 Nearest 3배·색상수 56개 강제 축소/자동 도형화/문자 배경 크롭만으로 재납품 금지. 필요한 해상도는 Codex의 실제 Sprite PPU/Canvas와 기존 공식 Ch2 몬스터·보스 표시 높이 기준의 **알파 실효 크기·Pixel Density**에 맞춰 선택. 무조건 원화 314px/512px 강제하지 말 것.
2. **서리들쥐 A**: 약한 첫 몬스터 규모를 유지하면서 생존형 긴몸·작은 귀 비율·뾰족한 주둥이, 발톱/앞니·거친 털끝·길고 긴장된 꼬리·눈빛을 명료하게. 128px에서 실루엣만 봐도 야생 설치류; 과도한 귀 원·완구형 몸통·의인화 금지. 배경 투명.
3. **설빙의 폭군 B**: 웬디고 모티브/두개골 실루엣은 유지하되 무거운 흉곽·긴 팔·발톱/얼음 갑각·상체의 압박감, 발 아래 무게·냉기 조명을 높임. 붉은 악마/귀여운 마스코트가 아니라 북부의 위협적 최종보스로. 게임 화면에서 뿔과 어깨/손의 윤곽이 분리되게. 폭발적 FX로 어설픈 실루엣을 감추지 않음.
4. **실제 독립 PNG 2장만** 그린 후 `Preview`로 (A) 1:1 실효 알파 영역, (B) 화면 128px, (C) **Chapter2 승인 공식 몬스터/보스와 동일 게임 표시 배율 비교**를 반드시 함께 제시. 샘플 2장/미리보기/Manifest SHA/ZIP testzip/QA를 별도 **v2**로 납품. 편집 소스가 고해상도 콘셉트면 기존 캐릭터를 그대로 단순 resize하지 않았다는 새 그리기 단계/픽셀 디테일 변화 증거도 기록.
5. 정식 A/B **디자인 방향은 보호**. 다른 5종 몬스터/보스 애니메이션 전 행/기존 캐릭터 아트/F1 마을 10종 환경 자산/리뉴얼 Field09 자산은 건드리지 않음.
6. `docs/art-new`에서 실제 PNG·ZIP을 사용자와 Codex가 접근 가능한 경로로 인계, 브랜치/커밋·SHA/시각 자료/승인 필요 여부 기록. ZIP CRC PASS만으로 완료·사용자 승인 선언 금지.

## 검수·승인 순서

메인 아트 담당 v2 1포즈 두 PNG 실납품 → Codex는 **새 PNG만** 기존 공식 비교판 배율로 최소 변경분 QA (이전 ZIP 기본 검사 재실행 최소화, Unity 원본·Save·UserData 보호) → 바울 대표가 두 1포즈의 **미술 합격/반려 직접 판단** → 합격 후에만 Animator/방향별 Sprite/Attack·Hit·KO 제작 발주. Codex 새 Unity Scene/몬스터 구현·Push/main 병합 금지. 메인 아트 Issue #30 댓글로 결과 및 계약 인계.

## 자료
- `docs/art-new/문서/00_프로젝트/Chapter3_ApprovedAB_PixelArt_1Pose_v1_QA_20261020.md`
- `docs/planning/문서/00_프로젝트/Chapter3_SelectedAB_PixelArt_1Pose_VisualReview_Gate_20261020.md`
- Codex 정적 동일 게임UI 배율 스크린샷(바울 대표 공유)
