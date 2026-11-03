# LIMITLESS / 2026-11-03 / VenomBee Sting·Recover 최종 국소 보완 v3 (Issue #63)

**담당 브랜치:** `docs/art-renewal`. **성격:** 실제 변경된 신규 PNG 2장 + 오프라인 검토용 GIF/비교판. **바울 대표 미술 승인 대기 · Unity/본편 변경 없음**.

## 1. 최신 정본 및 최소 변경 계약

- [Issue #63 / Codex Sting·Recover 새 원화 1차 검수](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6601942012) 및 [기획 표적 수정 지시(사용량 8% 보존)](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6602162870), `WORLD_ART_RENEWAL_LATEST.md` 확인.
- 기존 `Prepare`·정상 `Idle2`·원본 `Idle10+Attack10` 20프레임 **수정/재제작하지 않음**. 이번에는 **Sting 1 PNG + Recover 1 PNG**만 신규 전투 키포즈로 제작.
- 기존 후보 A 계약: 왼쪽 적→오른쪽 아군 +X, Sting index02/피해 명목 0.25초, Recover index04~08·index09 안정, Attack10@10FPS=1초. **격리 Unity 153 PASS는 타이밍 후보에 대한 결과이며 본게임 적용 승인·실제 침끝 접촉 증거가 아니다**.

## 2. 실제 이미지 산출 / 정적 SHA QA

- **ZIP 파일:** `LIMITLESS_BEE_STING_RECOVER_CONTINUITY_V3.zip`
- **ZIP SHA256:** `1169aca1c46f4322b85e7247b33a6cbc1fb684e01e0f04fb2ea7f9e9c0194ff0`
- **실제 크기:** 698,815 bytes / **ZIP entries 10 / CRC PASS / Manifest 8/8 SHA PASS**.
- 새 PNG들은 둘 다 **315×253 RGBA**, 4개 테두리 가시(alpha>8) 픽셀 접촉0. 기존 카메라/PPU/타격 위치를 검증한 것은 아님.

| 포즈 | 기존 오프라인 후보 SHA256 | 신규 v3 SHA256 | 실제 RGBA 변경 픽셀 |
|---|---|---|---:|
| Sting | `a10fefc6db6ff2289d298ab8b3def2263283eb163d946e7b2cd7641428485de8` | `2850020146718f30ac320cd6bd0c42dcf31332ed2820a718d06bcc76949a2d67` | 35,270 |
| Recover | `c5792c1c7c5f1b3be788dea96b013865f7f42162ca67e7d85d09add65bf9bb25` | `69d0944b3565fe2e4cc92034e51df2006847103a366db18d21221561fa5a19bb` | 39,436 |

- 신규 포즈 출처: 대화 내 **2026-11-03에 생성한 서로 다른 1399×1124 RGBA Sting/Recover 아트 2장**. Sting source SHA `bafd130745364876c66ef46e140f957113cd8018bb790615e6a97e469b517136`, Recover source SHA `3c204988eb319cdc467f8302782dc4d3f9e0ca0f1ff77c3cfe86f73fd5840c36`. 투명 영역의 고립 점 제거, Crop 및 **종횡비 유지** 축소·315×253 중앙 정렬. **기존 활성 PilotBee 이미지를 트레이싱/색변경하여 만든 것이 아니며, 별도 생성 원화의 정규화 과정**.
- `Candidates/VenomBee_Sting_Continuity_v3_315x253_RGBA.png` 및 `Candidates/VenomBee_Recover_Continuity_v3_315x253_RGBA.png`가 **이번에 제작한 유일한 신규 전투 키포즈 PNG**.
- 기존 Prepare·Idle2·이전 Sting/Recover **실제 파일을 모두 읽기전용으로 참조**, 참조 SHA를 `Evidence/QA.json`에 기록. 기존 20프레임 원본은 수정0, 원래 ZIP 보존(이번 ZIP에 전량 중복 포함하지 않음).

## 3. 가독성/10FPS·미술 한계

- `Review/Prepare_Sting_Recover_315_128_40_Comparison.png`: **기존 Prepare + 새 Sting/Recover**의 315px 본규격과 128/40 실제 비율 축소, 이전 Sting/Recover와 전후 비교.
- `Review/VenomBee_10FPS_128px_10Slots_REVIEW_ONLY.gif`, `...40px...`: **10슬롯 × 각100ms = 정확히1초**. 구성: Prepare 00–01 / Sting 02–03 / Recover 04–08 / Prepare 09의 반복 표시. **서로 다른 신규 애니메이션 프레임 10장이 아니라 두 신규 키포즈와 기존 Prepare를 반복한 오프라인 타임라인**. 09 안정은 Prepare를 시각 참고로 사용했으며 실제 게임의 마지막 프레임/Idle 보장이 아니다.
- 새 Sting은 몸과 복부가 앞으로 뻗고 +X 공격 방향이 보다 선명해졌지만, 몸통 회전 시 **복부 뒤 독침이 실제 오른쪽 적중 접점에 닿는다고 증명하지 못함**.
- 새 Recover는 Prepare에 가까운 체형·날개 각도·다리 자세로 복귀하도록 의도했지만, 생성 원화 기반이라 **날개 뿌리·다리 절지의 각 프레임 정확한 동일 모델 정합/자연스러운 연속성은 여전히 미술 검토가 필요**. 상용 게임용 완성 관절 작화 PASS로 기록하지 않는다.
- **판정:** `NEW_FILE_SHA_PASS / PNG_DIMENSION_ALPHA_PASS / ZIP_CRC_MANIFEST_PASS / CONTINUITY_ART_REVIEW_PENDING / ACTUAL_TARGET_CONTACT_NOT_VERIFIED / RUNTIME_NOT_TESTED / USER_ART_APPROVAL_PENDING`.

## 4. SourceDeclaration · 권리 · 원본 보호

- `Evidence/SourceDeclaration.md`: 생성형 신규 원본·Crop/Resize·기존 외부 원본과 차이를 기록.
- **OpenGameArt 기존 활성 PilotBee의 CC BY3.0 vs 동봉 LICENSE.TXT CC BY4.0 충돌은 미해결 `RIGHTS_HOLD`**. 생성형 신규 이미지는 독립적 저작권 법적 보증이나 기존 외부 스프라이트의 사용권 소멸을 의미하지 않음.
- 기존 공식 Chapter2 몬스터/보스/환경, Wolf/Bear/Fox, 승인 Owl/Organic Spider, Unity·Scene/Prefab/MonsterDefinition/Sprite GUID/meta/AI·독·드롭·Spawn·Save/UserData, `main` **변경0**. 신규 20프레임 양산0 / 원격 본편 Merge·Push0.
- **이번 회차 Codex 실행·추가 검수 요청0** (사용량 8% 보존). ChatGPT 작업공간에서 제작·검증. **바울 대표 Windows `F:\Downloads`에 자동 복사되었다고 주장하지 않음.**

**다음 승인:** 바울 대표가 새 두 포즈의 미술 품질 확인 → 필요할 경우 해당 두 PNG만 국소 수정, 또는 승인 → 한도 여유 생길 때 Codex 최소 Delta/실게임 통합 여부 별도 검토. **이번 답변에서 미술 최종 승인이나 게임 적용을 선행 처리하지 않음**.
