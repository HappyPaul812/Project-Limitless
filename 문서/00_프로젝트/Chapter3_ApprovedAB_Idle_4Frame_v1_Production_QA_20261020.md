# LIMITLESS Chapter3 — 서리들쥐 A · 설빙의 폭군 B Idle 4프레임 v1 제작·QA (2026-10-20)

**담당:** Chapter3 이후 신규 아트 총괄 / **작업 브랜치:** `docs/art-new` / **연계 Issue:** [#30](https://github.com/HappyPaul812/Project-Limitless/issues/30)

**상태:** `IDLE_4FRAMES_DELIVERED / OFFLINE_TECH_QA_PASS / USER_ANIMATION_ART_REVIEW_PENDING / NOT_UNITY_INTEGRATED`

## 1. 최신 계약과 원본 보존

- 바울 대표 미술 승인 기준: `Limitless_Chapter3_ApprovedAB_1Pose_ArtRework_v2.zip`, ZIP SHA256 `0b1a3711bed9616b828c83e51070f35ed161e2d8a4d93de9cb949d318dc46919`.
- 승인 서리들쥐 A PNG SHA256 `0516914ddad36af5854065d66d01b9664329aa89eea8b394480eed6af92e32c2`.
- 승인 설빙의 폭군 B PNG SHA256 `f1a3c5f91a5fef69cd881e27b578c32253d0124d5d8628a8ca748f7479333830`.
- **Codex 기술 최종 계약** 우선: 2048×512 RGBA, 가로 4셀×512×512, 8 FPS(125ms/셀; 0.5초 루프), PPU512, Pivot 하단 중앙(0.5,0), 우향 1방향, VisualScale: Mouse=1·Tyrant=3. 이전 담당의 PPU314/1256 규격은 **폐기된 임시 조사값**.
- 기획 정본: `docs/planning/문서/00_프로젝트/Chapter3_Idle2_And_Field09_Phase2Fixv2_Execution_20261020.md`; Codex Issue #30 최신 계약 댓글.
- 승인 원본 PNG/ZIP은 **수정 없이** 보존했고 그 픽셀을 기준으로 복수 프레임을 별도 새 PNG에 생성. 원본 512px 이미지를 기준 캔버스 그대로 유지했으며 결과 프레임 00은 승인 1포즈와 RGBA 픽셀상 정확히 동일하다.

## 2. 신규 아트 실제 산출 (ChatGPT 첨부 납품)

**납품:** `Limitless_Chapter3_FrostFieldMouse_FrostTyrant_Idle_v1.zip`  
**ZIP SHA256:** `6c01e55c147ab57147c767f43503553164197cc037a3834ecd2d15d10d96a23c`  
**ZIP 크기:** 6,953,861 bytes. 실제 PNG/ZIP은 해당 ChatGPT 대화 다운로드 첨부이며 **GitHub 브랜치에 바이너리를 등록하지 않았음**.

| 파일 | 포맷·실제 크기 | SHA256 |
|---|---|---|
| `Sprites/FrostFieldMouse_A_Idle_4Frames.png` | 2048×512 RGBA | `5afebfa1f95ca5673162513fdea63b025546aa836550c0ceb17f0334659988f2` |
| `Sprites/FrostTyrant_B_Idle_4Frames.png` | 2048×512 RGBA | `9d9b8d05be1813c013a5d784d749ce2d8ef751541ac36ce90712f3255cb53f43` |

ZIP 내부 총 12개 파일:
1. `Sprites/FrostFieldMouse_A_Idle_4Frames.png`
2. `Sprites/FrostTyrant_B_Idle_4Frames.png`
3. `Preview/FrostFieldMouse_A_4Frame_ContactSheet.png`
4. `Preview/FrostTyrant_B_4Frame_ContactSheet.png`
5. `Preview/FrostFieldMouse_A_Idle_8FPS_Exact.apng`
6. `Preview/FrostTyrant_B_Idle_8FPS_Exact.apng`
7. `Preview/FrostFieldMouse_A_Idle_8FPS_Loop.gif`
8. `Preview/FrostTyrant_B_Idle_8FPS_Loop.gif`
9. `QA/Display_128px_Comparison.png`
10. `QA/Offline_QA.json`
11. `Manifest.md`
12. `SHA256SUMS.txt`

## 3. 아트 방식과 검수

- 서로 다른 4프레임은 승인 원본의 **지역별 비강체 변형**을 적용해 생성했다. 캐릭터를 통째로 위아래로 옮기거나 동일 그림 네 장을 복사한 것이 아니다.
- 서리들쥐 A: 몸통·등의 미세 호흡, 머리/수염, 꼬리의 2차 국소 동작. 날카로운 발톱·긴 꼬리·승인 색상 유지.
- 설빙의 폭군 B: 흉곽·어깨의 무거운 호흡, 두개골/뿔과 전완의 작은 2차 반응. 장대한 체급감·발톱과 뿔 실루엣 보존.
- 각 프레임에서 **캔버스 하부 y=438~511 영역을 승인 PNG와 픽셀 단위로 동일하게 유지**했으며, Alpha bbox 영역이 캔버스 가장자리에 접촉하지 않음을 검사. 4개 프레임은 각각 서로 다른 RGBA 해시.
- 실제 QA 비교: 서리들쥐 Frame1/2/3은 승인 원본 대비 변경 픽셀 각각 86,696/82,012/86,604; 폭군은 135,355/129,689/134,738. Frame0은 0 (픽셀 동일). 많은 변경 픽셀은 원본 전체 재설계가 아니라 색상 보간을 동반한 국소 변형 때문이다.
- 2 PNG 2048×512 RGBA/4×512px 분할, 각 4프레임 8FPS. **정확한 125ms/프레임 APNG 2개**를 제공하며, GIF 2개는 포맷 제약상 120/130/120/130ms로 총 0.5초 재생.  실제 128px 전프레임 비교 이미지 및 ZIP CRC·SHA256SUMS 검사 **PASS**.
- **오프라인 미술 QA 제한:** 원본 모양을 기반으로 제작했으나 실제 게임 128px 표시에서 움직임 자연스러움/속도/호흡 강약/관절 왜곡 없는지 사용자 검토 필요. 픽셀아트 최종 손수정 합격을 주장하지 않음.
- **Unity 미실시:** Editor Import·PPU/Pivot 적용·전투 UI 실표시·Animator/Runtime·빌드/Save 회귀를 실행하지 않았다. 이미지 메타/씬/코드/GUID/기존 승인 캐릭터/몬스터/F1/서릿길 마을/리뉴얼 원본 변경 0.
- **범위 준수:** Attack/Hit/Defeat/추가 방향/다른 몬스터 대량 생성 안 함. `main` 직접 Push·병합·커밋 없음.

## 4. 사용자 승인 및 후속

**USER_ANIMATION_ART_REVIEW_PENDING.** 대표가 GIF로 움직임을 판단 후 필요 시 해당 두 PNG만 최소 보완. 승인 전 기존 게임용 원본·메타 교체/Unity 통합 금지. 승인 시 Codex가 PPU512/Pivot bottom-center/VisualScale1·3로 별도 Import/Runtime 검증.
