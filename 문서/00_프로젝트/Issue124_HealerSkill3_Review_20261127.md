# Issue #124 치유사 기본 스킬 3종 원화 표적 검수

2026-11-27 · `codex/issue124-healer-skill3-review-20261127` · 기준 `6db375fb21ae4bfff1f9f2a1bf45a59b5cb4fe0d`

**종합 PARTIAL · 본편 반영 HOLD.** 40px에서는 세 그림의 주요 구성을 구분할 수 있습니다. 30px 아래에서는 치유의 빛과 정화의 공통 금빛 별·테두리가 내부 단서를 압도합니다.

[11/24 기획 발주](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7129561043) 및 [11/27 리뉴얼 납품](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7205176180)을 대조했습니다. 이번 후보는 버튼 전용입니다.

## 파일 검증 PASS

- `F:/Downloads/LIMITLESS_Issue124_HealerSkill3_Original_20261127.zip` SHA256 `1f7bbfb01efbee2e0c7f37d2c12c6058c70fe581dbb56b843a6b6cf1976f4fe5`가 납품값과 일치합니다.
- ZIP 항목 CRC 25/25, PNG 21장 청크 CRC 72/72, Manifest 경로·크기·SHA256 23/23, SHA256SUMS 경로·SHA256 24/24. 후보 3장 모두 128×128, 8비트 RGBA, 알파 범위 0–255입니다.
- 기본 공격·방어·스킬, 도발 v1·철벽 v2·수호의 맹세 v2 비교본은 승인 자산과 바이트 6/6 일치합니다.

| 후보 | PNG SHA256 |
| --- | --- |
| 치유의 빛 | `f7b40475ac18794c72803f5601830b9463c176990b29fe61585499ecfe94f28d` |
| 회복의 파동 | `fecfc5b86cd8d54c91bf5ac7fb2b13ee7dc604a0afe36d9ba5708925803b3593` |
| 정화 | `a86dfcf1dd75a725ae679b1b1ed6b3785568a10218d5776348eeebc21c25d6f4` |

## 화풍과 식별성

- **PASS · 화풍.** 금테, 은색 면, 밝은 하이라이트와 유선형 효과가 승인 기본 명령·수호자와 맞습니다. 치유사 세 후보는 상단 별과 하단 프레임을 모두 공유해 그룹 내부의 윤곽 차이가 약합니다.
- **PASS · 40px 버튼.** 치유의 빛은 한 인물에 집중되는 빛, 회복의 파동은 세 인물과 가로로 넓은 원, 정화는 보라색 문양의 파편을 확인할 수 있습니다.
- **PARTIAL · 30px 버튼.** 회복의 파동은 넓은 원으로 구분됩니다. 치유의 빛과 정화의 내부 도상이 작아 정화의 상태 해제 의미는 라벨이 보완합니다.
- **PARTIAL · 24/20px 소형 표시.** 회복의 파동은 폭으로 세 후보 중 가장 쉽게 구별됩니다. 치유의 빛과 정화는 세로 실루엣과 상단 별이 유사합니다. 특히 20px 회색조에서 두 후보의 구분이 약하고 밝은 배경에서는 내부 대비도 낮습니다. 세 인물이나 부서지는 문양을 아이콘 단독으로 안정적으로 읽기 어렵습니다.
- **PASS · 기존 표시 자산과 구분.** 실제 경로의 Kenney `suit_hearts`, RadiantHeal `radiant_heal_07`, SpectralBloom 시트 96×96 index 5를 대조했습니다. 하트 단독 윤곽, 연한 고리·입자, 청록색 발광 문양은 각각 후보의 금속 프레임·인물·보라색 파편과 구별됩니다. 비교 범위는 현재 버튼이 참조하는 파일과 프레임입니다.

## Unity와 증거

**PASS · Unity 6000.6.5f1 격리 배치.** 재사용 QA 복제본 `Temp/q124`의 실제 Battle 씬에서 기존 `MakeSkillMenuButton`이 만든 버튼과 라벨을 사용했습니다. 기존 Sprite 3종을 확인한 뒤 실행 중 Image의 Sprite만 교체했습니다. 후보 3종 Sprite import 128×128, 표시와 Play Mode 캡처 40/30/24/20px, 총 20 PASS, 종료 코드 0. 24/20px는 기존 버튼의 아이콘 영역을 12 유닛으로 줄여 검수한 소형 UI 모사입니다. 실제 상태 HUD 장착 및 스킬 발동은 검수 범위 밖입니다. 격리 임시 파일을 원복했습니다.

- `Temp/i124-healer-qa/Unity_Actual_Button_Comparison.png` · Unity 캡처에서 버튼 행을 확대 없이 잘라 비교했습니다. SHA256 `5404321758a466d41e595df53f5353810ae8518955abb62c522871537139dc35`.
- 같은 폴더에 `Unity_Baseline_30px.png`, `Unity_Candidates_{40,30,24,20}px.png`, `Unity_ApprovedCommands.png` 전체 화면과 `unity-final.log`를 보존했습니다.

`Temp/i124-healer-qa/Actual_Pixel_Comparison.png`는 실제 PNG의 128/40/30/24/20px 비교판입니다. 20px 밝은 배경·회색조와 기존 버튼 3종, 승인 비교본 6종을 함께 표시했습니다. SHA256 `8dc7932b2ae62a3be896a31243a5180062f662babf27ab617959fd8ba4269354`. 무결성 로그는 같은 폴더의 `file-qa.txt`에 보존했습니다.

본편 미반영. 승인 자산 및 #85·#131 후보 보호. 원본 main `65cceb2d6c3283d18abec6e27d8a6f1296411f17`. 기존 전수 QA 미실행, main 병합·원격 Push 없음. 보고 커밋만 별도 브랜치에 기록합니다.
