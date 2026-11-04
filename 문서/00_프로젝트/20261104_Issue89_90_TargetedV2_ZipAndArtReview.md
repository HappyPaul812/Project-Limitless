# LIMITLESS / 2026-11-04 / 냉기 VFX #89 v2·숲거미 #90 v2 실제 파일 및 시각 검수

**지시:** 바울 대표가 Windows 로컬 경로 `F:\Downloads\Limitless_Ch3_ColdVFX_Issue89_MinimalFollowup_20261104.zip`, `F:\Downloads\LIMITLESS_SPIDER_ATTACK3_TARGETED_ISSUE90_v2.zip` 전달.

**중요 증거 경계:** 본 기획 세션이 직접 검증한 바이트는 **ChatGPT Library에 저장된 동일 이름의 ZIP 사본**입니다. Windows `F:` 원본 자체의 SHA 일치는 **아직 확인하지 않음**. 같은 바이트인지 확인이 필요하면 Codex가 F: 실물 해시를 1회만 대조. 아래 CRC/SUMS 등은 Library 사본 검증 결과입니다.

## 1. 파일 무결성

| 항목 | Chapter3 VFX #89 v2 | ForestSpider Attack #90 v2 |
|---|---|---|
| ZIP | `Limitless_Ch3_ColdVFX_Issue89_MinimalFollowup_20261104.zip` | `LIMITLESS_SPIDER_ATTACK3_TARGETED_ISSUE90_v2.zip` |
| 바이트 | 1,434,793 | 1,719,271 |
| SHA256 | `9c07f5ff782c6e272796c4f003b816a39806ab35248c2353884325ac3c24a1eb` | `804f8ebf85dbe7f7301cbfdecc6e85adc4e7d8888e23c3ee132bfca95854499b` |
| ZIP 항목·CRC | 23 / PASS | 12 / PASS |
| SHA256SUMS | 22/22 PASS | 10/10 PASS |
| Manifest | #89 변경 후보·정본 SHA·스펙·보호 주석 기록, 파일별 전수목록은 SUMS로 검증 | 파일별 SHA+크기 10/10 PASS |
| 신규 후보 | 한기/동상/동결괴사 512² 투명 RGBA 각1개 | Contact01/Recover02 512² 투명 RGBA 각1개 |
| 가시 알파 네 변 | 3PNG 모두 0 | 2PNG 모두 0 |
| Windows F: 직접 해시 | NOT_VERIFIED | NOT_VERIFIED |

GitHub 제작 증거: [#89 담당 납품](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6620838373), [#90 담당 납품](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6621055012).

## 2. 기획 시각 판정 — 실제 이미지 확인

### #89 냉기 v2
- 보관 라이브러리 `Issue89_v1_v2_128_40_BeforeAfter_OFFLINE.png`, `Issue89_Snow_DarkBG_SameScale_OFFLINE.png`의 **같은 배율·같은 배경** 비교판을 직접 열람.
- **한기:** 옅은 연무/고리 가장자리 대비가 v1보다 개선되지만 **밝은 설원 40px에서는 여전히 연무가 약함**. 상태 아이콘/한기 명칭·남은 차단 턴 표시와 반드시 함께 제시.
- **동상:** 고정된/붙는 결정 군집, 과하게 뾰족한 위쪽 끝 일부 축소. 40px에서 여전히 빽빽한 청색 결정 뭉치여서 동결괴사와 **실루엣 구분이 충분한지 정식 Unity 판단 전 미확정**.
- **동결괴사:** 검푸른 하단 큰 쐐기와 중앙 균열이 독특하게 읽힘. 다만 40px 시각 차이를 색상만으로 보장하지 않음; **1/3·2/3 + 위험 텍스트 UI 필수**.
- **결론:** `OFFLINE_V2_TARGETED_REVIEW_ACCEPTABLE / USER_FINAL_ART_APPROVAL_PENDING / UNITY_HUD_40PX_NOT_VERIFIED`. 새 정지 그림을 반복 재제작하기보다 **Unity 격리 한정 40/128px 실제 Battle HUD 시인성 1회**를 다음 게이트로 권고. 사용자가 직접 최종 미술 승인 전에 이미지 사용·본편 적용을 승인했다고 해석하지 않음.

### #90 ForestSpider Contact/Recover v2
- `ForestSpider_Attack_Targeted_512_BeforeAfter.png`, `128_BeforeAfter.png`, `40_BeforeAfter.png`, `JointGuide.png` 직접 열람.
- **Contact01:** 기존 우향 공격을 살리면서 머리·복부·앞다리 연결의 변화가 국소적으로 보임. 128/40에서 기존 캐릭터 인지도 유지.
- **Recover02:** 과도하게 올라갔던 복부 상단이 낮아지고 두부/지지다리의 복귀 자세가 더 자연스럽지만, **실제 발 하나하나의 지면고정/어깨-무릎 대응은 정지 이미지로 확정 불가**.
- 기존 Prepare00·승인 v5·Idle8·Walk8 보존. **Attack8 중 3개 키포즈 시안만 존재**, 중간5프레임 및 Shoot8 미제작; Attack8 10FPS 완성·실 공격접점 증거0.
- **결론:** `CONTACT_RECOVER_V2_KEYPOSE_VISUAL_CANDIDATE / USER_FINAL_ART_APPROVAL_PENDING / TARGET_CONTACT_NOT_VERIFIED / ATTACK8_INCOMPLETE`. 아트 전면 재작화 요구X; **3키포즈 정적 미술 승인과 10FPS 중간프레임 제작 승인 게이트 분리**. 완료된 모션이라고 주장하지 않음.

## 3. 후속 게이트

1. 두 담당의 납품과 Codex 기존 검수는 보호. 현재 사용자 F: 원본 2 ZIP 해시만 필요한 경우 표적 비교, CRC/manifest/이미 검증한 기존 QA 전수 반복 금지.
2. 바울 대표의 **시각 승인**이 없으면 `USER_ART_APPROVAL_PENDING` 유지. 메인 VFX는 승인 시 격리 Unity6000.6.5f1에서 Battle HUD·실제 40px/128px·밝은 F2 설원·어두운 배경에서 리소스 표시만 표적 QA. 정식 냉기 상태·해제·VFX 런타임 자동 연결 금지.
3. Spider는 **키포즈 최종 승인 후에만** 기존 10FPS/PPU256/scale0.7/우향 기준 8프레임 연결용 부족5장 제작 발주. 실제 Contact와 지지발/미끄러짐은 Codex 격리 애니메이션 QA를 통해 별도 확인. 승인 Walk8 재제작 금지.
4. 새 파일 승인/격리 QA/원본 Unity 적용/remote main 병합은 각각 독립적. 원본 Editor·씬·프리팹·사용자 Save/UserData·승인 원화·기존 냉기 Stage1 63PASS 보호. `main` 직접 커밋·Push·병합 없음.

**현재 상태:** `LIBRARY_V2_ZIP_SHA_CRC_PASS / VISUAL_REVISION_IMPROVED / USER_ART_APPROVAL_PENDING / ORIGINAL_NOT_APPLIED / F_DRIVE_SHA_NOT_VERIFIED`.
