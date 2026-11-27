# Issue #124 · GuardianVFX2 원화 검수 · 2026-11-27

**종합 PARTIAL. 철벽 PASS, 수호의 맹세 PARTIAL.** 기준 HEAD는 `9b0f3e94cceac43542430f059dd395b096cb0757`입니다.

[기획 정본](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7215549625) · [원화 납품](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7215314368)

## 파일 검증

- `F:\Downloads\LIMITLESS_Issue124_GuardianVFX2_Original_20261127.zip` SHA256 `f0068ac50e6868e619af83bbe7c47c0bda893f04f7819f90f02192b1a78c2400`.
- ZIP CRC 46/46, SHA256SUMS 45/45, Manifest 크기·SHA256 44/44 일치. 96×96 RGBA 27/27 개별 PNG·시트·APNG 픽셀 일치. APNG 지연은 각각 50ms·45ms이고 두 파일 모두 1회 재생. 수호의 맹세 시트 미사용 8셀 완전 투명.
- 철벽 시트 SHA256 `b83c79884917a065f76152df613be911ccc8686c2d32bbe77b7974be3af0c01a`. 수호의 맹세 시트 SHA256 `a1593cd43d9d41f092192f9ddb52425b1ba09a0a36a4d398e444596f9dafda04`.

## Unity 6000.6.5f1 격리 Battle

실제 Battle 씬·프레젠터에 원본 EarthRupture/FrostNova와 메모리에서 슬라이스한 납품 후보를 각각 주입했습니다. 휠체어 수호자와 동료 태온·미엘, 상태 및 명령 HUD가 있는 1280×720 화면에서 승인 설원 배경과 같은 배경의 어두운 틴트 버전을 비교했습니다. 프레임 간격은 120Hz 샘플링으로 확인했습니다. 배치 종료 0, 대상 검증 19 PASS. Editor Search 인덱스에서 예외 1건이 있었으며 Battle 검수는 정상 종료했습니다.

| 대상 | 판정 | 실제 Battle 결과 |
|---|---|---|
| 철벽 | PASS | 158×158 Rect, 20장, 20FPS, 상태 적용 index 9에서 1회. 발밑의 낮은 불빛이 위로 차오르며 장벽 형태가 분명해집니다. index 4~9에서 휠체어 중앙과 장벽이 잠시 겹치지만 머리와 외곽 실루엣, 동료와 HUD는 식별됩니다. |
| 수호의 맹세 | PARTIAL | 460×250 Rect, 슬롯 4~10의 7장, 45ms 간격, 상태 적용 index 1(시트 슬롯 5)에서 1회. 기존 비율 유지 설정 때문에 정사각 Sprite가 약 250×250 안에 맞춰집니다. 노란 파동은 어두운 배경에서 읽히지만 밝은 설원에서 약하고, 시전자 왼쪽에 집중되어 우측 미엘을 포함한 아군 보호로 읽기 어렵습니다. 휠체어·동료·HUD의 주요 정보는 식별됩니다. 기존 상태 런타임이 살아 있는 동료 두 명의 직접 피해를 각각 가로채는 것을 확인했습니다. |

수호의 맹세는 보호 대상이 화면에서 읽히도록 배치와 명암을 보완한 뒤 표적 비교가 필요합니다. 후보는 격리 프레젠터 주입으로 검수했습니다. 도발은 기존 코드 VFX 경로를 확인했습니다. FrostNova, Unity 제품 파일, 기능·수치·타이밍, Save/UserData, main 및 #85/#131/#132를 보호했습니다. 본편 적용·병합·Push 없이 기존 PASS를 재실행하지 않았습니다.

## 실제 화면

- [철벽 밝은 배경](../../Evidence/Issue124_GuardianVFX2_20261127/Iron_Light_comparison.png) · [어두운 배경](../../Evidence/Issue124_GuardianVFX2_20261127/Iron_Dark_comparison.png) · [HUD 포함 원본 크기](../../Evidence/Issue124_GuardianVFX2_20261127/Light_Candidate_Iron_9.png)
- [수호의 맹세 밝은 배경](../../Evidence/Issue124_GuardianVFX2_20261127/Cover_Light_comparison.png) · [어두운 배경](../../Evidence/Issue124_GuardianVFX2_20261127/Cover_Dark_comparison.png) · [HUD 포함 원본 크기](../../Evidence/Issue124_GuardianVFX2_20261127/Light_Candidate_Cover_3.png)

비교판은 동일 프레임을 1:1로 잘라 나란히 놓았습니다. [파일 검증 로그](../../Evidence/Issue124_GuardianVFX2_20261127/files.txt) · [Unity 표적 로그](../../Evidence/Issue124_GuardianVFX2_20261127/unity-summary.txt)
