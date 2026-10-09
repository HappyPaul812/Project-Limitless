# Main20 Art Pack 검수 — 2026-10-09

**최종 판정: NEEDS_ART_CORRECTION. 사용자 미술 승인 미완료 / Unity 통합 미착수.** PNG 디코딩과 기본 시트의 기하 규격은 통과했지만 Skill/Hit 공용 행의 공격 방출 표현은 정본과 충돌한다. 파일명·Phase2 overlay·단일 이미지 VFX의 Handoff 매핑도 별도로 확정해야 한다. 이 문서는 원본을 수정하거나 재납품 완료로 기록하지 않는다.

## 1. 범위와 ZIP 원본

- 시작 HEAD: `9920f5c2deb356c7e44b1a1b94c533967bd8753e`. 저장된 origin/main과 동일, ahead0/behind0. 원격 조회/Push0.
- 시작 기존 LOCAL: tracked 수정75개·미추적25개·staged0. reset/revert/checkout0, 기존 사용자 파일 보존.
- 원본: `F:\Downloads\Limitless_Main20_DeepCore_VeinfireColossus_Art_Pack.zip`
- ZIP 크기: **2,304,572 bytes**.
- ZIP SHA256: `660bd5ada46b27c7099770441dbf32a516b922402389c0bf57a7f6982797ef3e`.
- ZIP 파일18개 = **PNG17개（Boss2 / VFX7 / Environment8）+ Manifest.txt1**. 디렉터리 엔트리0·CRC 오류0·중복 상대 경로0·상위 경로 탈출/절대 경로 엔트리0.
- 정본: [Main20 Art Handoff](Main20_Art_Handoff.md) · [정식 Story/Objective9](../03_스토리/Chapter2_Main20_심부의_거신.md) · [Design QA](Main20_Design_QA.md).

ZIP은 읽기 전용으로 메모리에 열었다. PNG signature/IHDR·Pillow verify 후 재열기/전체 load·알파 통계·Boss16셀·ZIP CRC를 검사했다. 전체17PNG, Boss 셀16개와 Phase2 합성, 지면2×2 반복, Main19 실루엣을 백그라운드 이미지 조회로 검토했다. 원본 파일을 풀어 Unity에 복사하지 않았다. 검사 코드/검토용 파생 이미지는 세션 visualization 공간에만 두며 원본 납품 PNG나 게임 Asset이 아니다. Play Mode·OS 입력·포커스 전환0.

## 2. 전체 파일 목록과 기술 검사

PNG17개 모두 **실제 PNG / 8bit RGBA（IHDR color type6）/ signature PASS / verify·전체 decode PASS**. Ground와 Arena만 완전 불투명이며 나머지15개에 실제 투명 영역이 있다. PNG 확장자만 검사한 결과가 아니다. SHA는 ZIP에서 읽은 각 파일의 원본 byte 기준이다.

| ZIP 상대 경로 | 해상도 / 형식 | bytes | alpha min~max | 완전 투명 pixel 수 | SHA256 |
| --- | --- | ---: | --- | ---: | --- |
| Boss/Veinfire_Colossus_Phase2_Overlay.png | 1256×1256 / PNG RGBA8 | 92,185 | 0~188 | 1,283,380 | `ff5510979f3f52973f2e02fc7e3161179055f3e6a134701e97c946c19202c64e` |
| Boss/Veinfire_Colossus_Sheet.png | 1256×1256 / PNG RGBA8 | 599,377 | 0~255 | 749,076 | `3cd5ca46555c652e223513a7f7a19fb101d624cd9f1f028aba074db60ca5cdb3` |
| Environment/DeepCore_BossArena_Ground.png | 512×512 / PNG RGBA8 | 437,147 | 255~255 | 0 | `c5ebe7d89da637a12966161c8a41c487224a8bb88df40d0c22f480643876b735` |
| Environment/DeepCore_CollapsedColossus.png | 512×512 / PNG RGBA8 | 33,594 | 0~255 | 186,854 | `a06c5ec34ae1771374b4a8c02b24d99531c4749d9125fb79b4261b6d5fe6b99a` |
| Environment/DeepCore_ColossusTrace.png | 512×512 / PNG RGBA8 | 205,942 | 0~255 | 120,340 | `921aeb6ae1fd84231146aa820468db14a472b411664775abce8edd4173c103db` |
| Environment/DeepCore_CooledRift_Overlay.png | 512×512 / PNG RGBA8 | 23,353 | 0~107 | 184,291 | `518280749821145dd2cb7a1fb47dc3681ec15cffc356789359f34b84a9b901d1` |
| Environment/DeepCore_DeepCliff_Blocker.png | 512×512 / PNG RGBA8 | 230,335 | 0~255 | 115,034 | `e020a8681401005c8664fc12a5afdc7d61a1725a6c2bd8f5aad6178fa14a5c9c` |
| Environment/DeepCore_Ground_Base.png | 512×512 / PNG RGBA8 | 457,666 | 255~255 | 0 | `0d3361435c911a54223d48b2158adcb8b7e4780a87f078d1ce8bb5812ac6495b` |
| Environment/DeepCore_Rift_01.png | 512×512 / PNG RGBA8 | 29,675 | 0~234 | 169,477 | `bd2f8820cad8979ba4054aea96338d7d58a14ed2b5253d2a837f4f2df6291b61` |
| Environment/DeepCore_Rift_02.png | 512×512 / PNG RGBA8 | 29,504 | 0~234 | 170,777 | `948a3ef40f6ae3b9f51c082635e45553ed607a4020afce3c173922fdcad411e8` |
| Manifest.txt | UTF-8 text | 6,111 | — | — | `0dc20297daafc50679e727a9d2a6cab150426e98c96ab9f56475b87c649d989d` |
| VFX/Veinfire_CoreCondensation_VFX.png | 512×512 / PNG RGBA8 | 19,874 | 0~164 | 209,659 | `67b91c92b3ed0cdaa1fa90985076a50dfa297512d30826bef113480041a13a90` |
| VFX/Veinfire_CoreEruption_VFX.png | 512×512 / PNG RGBA8 | 49,117 | 0~169 | 160,592 | `f163c75daeab7b9f54b077223c29376a9a646326fc2196a468bfb0cd504d6ab9` |
| VFX/Veinfire_CoreResonance_VFX.png | 512×512 / PNG RGBA8 | 15,032 | 0~150 | 200,063 | `ac41ef8a18c3b1ee1c1bca5ee53bdf94e2cb0910749c1298b26aa8d4df66fc54` |
| VFX/Veinfire_CoreWave_VFX.png | 512×512 / PNG RGBA8 | 16,522 | 0~181 | 176,731 | `373a8062c16ce899f32642fbe3388bd9a7ef86d7f3b4e0427a05ccaef085a463` |
| VFX/Veinfire_HeatPressure_VFX.png | 512×512 / PNG RGBA8 | 16,816 | 0~159 | 198,964 | `e6e59ccc4bd2412796535757c928261b34f938b30439681125c4c50df601fefc` |
| VFX/Veinfire_HeatWave_VFX.png | 512×512 / PNG RGBA8 | 50,529 | 0~155 | 121,733 | `2a8eb0379b6e96e56cccdc2ba5ec0298a014fb8539c3ed9b5a666d2d49d51b2c` |
| VFX/Veinfire_MoltenStrike_VFX.png | 512×512 / PNG RGBA8 | 26,072 | 0~230 | 201,233 | `bc4bd18f45d9aba93f3ace5ce823e1db7c9b6220a95b03dacdb4eef985fc1de7` |

Manifest.txt는18개 실제 파일 중 PNG17개 전부의 이름·목적·해상도·Alpha·사용처를 설명한다. 기본 Boss4행·Phase2 overlay 대응·KO 소거·static VFX·접근성 설명이 있다. 외부 실행 지시로 취급하지 않았다. 별도 README/CSV/JSON 없음. Manifest의 자체 검수 주장만으로 PASS하지 않고 실제 파일을 대조했다. 시트 동작별 정확한 타격 시점/Phase2 대안 승인/가로strip 대신static 사용 승인은 제공하지 않는다.

## 3. 정본16개 대응 매트릭스

**basename 정확 일치7/16, 정본 파일명 미존재9/16.** 아래는 후속 통합용 **제안 매핑**이며 원본 rename·정본 수정·승인 확정이 아니다. 미일치 납품10개는 이름/방식이 다른 대응 후보9개 + 정본에 없는 추가Trace1개다. 동일 역할 후보를 찾았다고 완전한 정본 충족16으로 올리지 않는다.

| 정본 파일명 | ZIP 실제 상대 경로 | 판정 / 연결 의미 |
| --- | --- | --- |
| Veinfire_Colossus_Sprite_Sheet.png | Boss/Veinfire_Colossus_Sheet.png | NAME_DIFF; 기본4×4 규격 PASS, 공용3행 표현 수정 필요 |
| Veinfire_Colossus_Phase2_Sprite_Sheet.png | Boss/Veinfire_Colossus_Phase2_Overlay.png | FULL_SHEET_MISSING / ALTERNATIVE_MAPPING_REQUIRED; overlay이며 독립 본체 시트가 아님 |
| Veinfire_HeatPressureInjection_VFX.png | VFX/Veinfire_HeatPressure_VFX.png | NAME_DIFF; 열압 주입90% 단일+생존 대상 과열1 |
| Veinfire_MoltenStrike_VFX.png | VFX/Veinfire_MoltenStrike_VFX.png | EXACT; 용융 강타125% 단일+공용Burn |
| Veinfire_CoreCondensation_Telegraph.png | VFX/Veinfire_CoreCondensation_VFX.png | NAME_DIFF; 열핵 응축, 피해0·다음열핵파동 예고 |
| Veinfire_CoreWave_VFX.png | VFX/Veinfire_CoreWave_VFX.png | EXACT; 열핵 파동80% 전체 직접, 상태 부여0 |
| Veinfire_CoreResonance_Telegraph.png | VFX/Veinfire_CoreResonance_VFX.png | NAME_DIFF; 열핵 공명, 피해0·다음열핵분출 전체과열1 예고 |
| Veinfire_CoreEruption_VFX.png | VFX/Veinfire_CoreEruption_VFX.png | EXACT; 열핵 분출60% 전체 직접→생존 대상 과열1 |
| Veinfire_HeatWave_VFX.png | VFX/Veinfire_HeatWave_VFX.png | EXACT; 열파55% 전체 직접, 상태 부여0 |
| DeepCore_Ground_Base.png | Environment/DeepCore_Ground_Base.png | EXACT; 기본 지면 |
| DeepCore_Rift_01.png | Environment/DeepCore_Rift_01.png | EXACT; 균열 조사/열맥 |
| DeepCore_Rift_02.png | Environment/DeepCore_Rift_02.png | EXACT; 대체 균열/외곽 |
| DeepCore_Arena_PressureMark.png | Environment/DeepCore_BossArena_Ground.png | NAME_DIFF; Boss 열린 공간/방사 압흔 |
| DeepCore_Cliff_Boundary.png | Environment/DeepCore_DeepCliff_Blocker.png | NAME_DIFF; 절벽/Boundary용 Prop, Collider 별도 |
| DeepCore_CollapsedCore.png | Environment/DeepCore_CollapsedColossus.png | NAME_DIFF; 승리 후 잔해 |
| DeepCore_HeatRecession.png | Environment/DeepCore_CooledRift_Overlay.png | NAME_DIFF / PARTIAL_COVERAGE; Rift01에 대응하는 낮은열기 표현, 전체환경 cooled 변형은 아님 |

추가 납품 `Environment/DeepCore_ColossusTrace.png`는 실제 존재한다. 초기 프롬프트 파일 자체는 이번 입력에 없으므로 프롬프트 전문의 경위를 검증했다고 주장하지 않는다. 정식 Handoff에는 거신 이동 흔적 요구는 있지만 이 파일명이16개 목록에 없었다. 별도 새 필수 자산으로 추가하지 않고 **follow_colossus_trace의 시각 단서에 추가 납품을 활용하는 제안**으로 기록한다. 다른 원본 삭제/교체0.

## 4. Boss 시트 기하·행·Phase 대응

| 검사 | 결과 |
| --- | --- |
| 기본 시트1256×1256 / 314×314 / 4열4행 | PASS |
| 기본16프레임 모두 내용 존재 / 셀 경계 | PASS:16/16 비어 있지 않음, 셀 외곽 alpha>0 pixel0 |
| 기본 프레임 잘림/이웃 셀 침범 | 경계 검출상 없음, 시각 조회상 명백한 셀 잘림 없음 |
| 기본 발밑 | 모든 프레임 alpha bbox 아래끝302, 아래 투명 여백12px; 좌/우 최소 여백10px 이상 |
| Row1 Idle4 | 00~03 본체 자세 존재. 폭/상체 높이/세부 실루엣 변화가 있어 반복 시 크기·체격 흔들림은 사용자 검토 필요, 자연loop PASS는 미확인 |
| Row2 Attack4 | 04~07 강타 동작. 단일 타격 의미는 사용 가능 후보, 실제 impact frame은 미확정 |
| Row3 Skill/Hit4 | **FAIL: frame10의 큰 열핵 원형 방출·양손 불꽃과 frame11 방출선이 특정 공격처럼 보임. PlaySkill/PlayHit 공용 행에 완성형 방출을 넣지 않는 정본과 충돌** |
| Row4 KO4 | 12~15 및 마지막frame15 잔해 존재. 13~14에서 상체/열핵이 다시 커지는 동작이 있어 Collapse 흐름·약화 정도 사용자 검토 필요 |
| 정본 Phase2 완성 시트 | **FAIL（미납품）**:같은 규격의 본체 시트가 아닌 overlay가 납품됨 |
| 납품 Phase2 overlay1256/314/4×4 | 기하 PASS. 전체alpha최대188, 독립 본체 없음,14/15셀 완전 투명은Manifest의 KO fade와 일치 |
| Phase2 비율·프레임 대응 | 같은 본체를 쓰므로 캔버스/기본 비율은 유지 가능. 동작별 열맥 정렬 및 열핵노출 의미는 미확인, PASS로 처리하지 않음 |

### Boss 셀별 경계 근거

좌표는 각314px 셀 내부의 좌상단 기준이며 bbox 오른쪽/아래 값은 exclusive다.

| frame | 기본 alpha bbox | 기본 보이는 pixel | 기본 경계 pixel | overlay 보이는 pixel | overlay 경계 pixel |
| ---: | --- | ---: | ---: | ---: | ---: |
| 00 | [11, 32, 302, 302] | 54,670 | 0 | 21,482 | 0 |
| 01 | [16, 15, 298, 302] | 54,740 | 0 | 21,486 | 0 |
| 02 | [11, 42, 302, 302] | 56,020 | 0 | 21,482 | 0 |
| 03 | [11, 50, 302, 302] | 50,490 | 0 | 21,498 | 0 |
| 04 | [11, 32, 302, 302] | 54,037 | 0 | 21,482 | 0 |
| 05 | [14, 15, 299, 302] | 49,564 | 0 | 21,490 | 0 |
| 06 | [11, 39, 302, 302] | 52,577 | 0 | 21,482 | 0 |
| 07 | [11, 43, 302, 302] | 47,561 | 0 | 21,482 | 0 |
| 08 | [11, 30, 302, 302] | 52,592 | 0 | 21,483 | 0 |
| 09 | [11, 30, 293, 302] | 48,098 | 0 | 21,479 | 0 |
| 10 | [11, 43, 302, 302] | 56,425 | 0 | 21,482 | 0 |
| 11 | [11, 36, 302, 302] | 53,327 | 0 | 21,492 | 0 |
| 12 | [11, 65, 302, 302] | 49,884 | 0 | 18,150 | 0 |
| 13 | [11, 28, 302, 302] | 53,199 | 0 | 18,186 | 0 |
| 14 | [11, 21, 302, 302] | 59,339 | 0 | 0 | 0 |
| 15 | [11, 121, 302, 302] | 35,937 | 0 | 0 | 0 |

### overlay 정합의 관측값과 한계

overlay00~11의 알파 bbox는 모두 셀좌표 `(73,68)~(243,222)`로 같은 위치다. 기본 본체의 bbox와 동작은 프레임마다 달라진다. 합성 조회에서 열빛이 추가되지만, 프레임별 노출된 열핵/사지 균열에 맞게 따라간다는 근거는 부족하다. alpha>=16 기준 기본 본체alpha<16 영역으로 overlay가 나오는 pixel은 frame05=110,06=290,08=184,12=74개다. **이는 glow 외곽일 수도 있으므로 숫자만으로 정렬 불량을 확정하지 않는다.** 아트 담당이 프레임 대응을 설명하고 실제크기 합성에서 검토해야 한다.

Handoff에는 overlay 대안이 있었지만 제작 전 어느 방식을 택할지 확인하도록 되어 있다. 이번 납품은 overlay 대안을 선택한 형태이나 선택 승인 근거는 입력에 없다. 사용자 요청의 '두 완성시트' 검사와는 구분한다. 완성 Phase2 시트 재납품 또는 overlay 대안 승인·매핑 확정 중 하나가 필요하다. 본 검사에서 자동 합성한 preview는 비교용이며 Phase2 원본 Asset으로 채택하지 않는다.

### 아트 수정 우선순위

1. **필수 수정:**기본 시트3행08~11을 피격과Skill이 공유 가능한 중심부 반응/긴장 자세로 수정하고 완성형 방출/실제타격 표현은 별도VFX에 둔다. 기존 공용PlayHit를 공격 전용처럼 바꾸는 우회 구현은 하지 않는다.
2. **필수 매핑 결정:**Phase2 완성시트 또는overlay 중 정본 계약을 명시. overlay 선택 시12개 살아있는 동작셀의 정렬·노출된열핵·KO 소거를 증명한다. 불일치하면 해당overlay를 재납품한다.
3. **시각 검토 후 수정:**Idle체격흔들림, KO13~14의열핵확대, 지나치게 골렘/갑옷처럼보이는지, 화염/노출정도의고유디자인을 사용자 판단으로 확인한다. 특정게임복제여부/미적합격을 자동확정하지 않는다.

## 5. VFX·환경 세부 검사

### VFX7종

모두512×512 RGBA의 **단일 static1프레임**이며 가로4프레임 strip이 아니다. 1프레임은 '4프레임 이하'에 포함되지만 권장128/256px 셀·가로strip과 제작 방식이 달라 **STATIC_MAPPING_REQUIRED**로 기록한다. 4조각으로 잘못 slice하지 않는다. 손상0·실제 투명 영역7/7, 정본7행동과 Manifest 사용처가 대응한다. 파일명 `_VFX`만 보고 예고를 실제 공격으로 처리하면 안 된다.

- 응축은 안쪽으로 모이는 타원링, 공명은 바깥살/방사링으로 형태가 다르다. 둘 다 피해0이며 다음 Boss 행동까지 HUD 텍스트를 유지한다. 색이나 VFX 재생 시간만으로 판정하지 않는다.
- 파동은 낮은 넓은 링, 분출은 방사 압력, 열파는 넓은 수평 열기, 주입은 좁은 선형 압력, 강타는 암석 파편·충격으로 구분된다. 실제 해상도와 어두운 배경에서의 가독성은 미검증이다.
- 열파 alpha bbox가 좌우 캔버스에 닿지만 가장자리 alpha최대4, alpha>=16 pixel0이다. 미세 glow tail은 있으나 본체가 뚜렷하게 잘렸다고 FAIL하지 않는다.
- static PNG만으로 '빠른 Flash 없음'을 실행 검증할 수 없다. 후속 Presenter에서 완만한 Scale/Opacity와 흰색 strobe·강한 반복 shake 금지를 지켜야 한다.

### 환경8종

Ground/Arena는 불투명 base, 나머지6종은 투명 Prop/Overlay로 정상이다. 환경에는16셀 slice를 적용하지 않는다. 전체 decode PASS. Rift02 좌우 경계 alpha최대2/7, alpha>=16 pixel0이므로 미세 glow 외에 명백한 강한 잘림은 검출되지 않았다.

Ground 양끝 pixel 차이 MAE는 좌우0·상하0으로 맞지만 **2×2 반복에서 격자처럼 밝은 경계띠와 반복 패턴이 보인다**. 수학적 edge 일치와 시각적 seam 최소화를 구분한다. 최종 지면 사용 전 사용자 검토가 필요하며 경계띠가 거슬리면 아트 담당의 수정 대상이다. 본 검사에서 PNG를 feather하거나 수정하지 않았다.

Arena는 중앙 방사 균열이 압흔 역할을 하고, Trace는 검은 눌림/부서진 흔적, Cliff는 암석 기둥과 틈, Collapsed는 잔해를 표현한다. 압흔이 마법진처럼 보이는지, Trace 붉은 외곽이 디버그 가이드처럼 보이는지, 전체가 너무 어두운지는 사용자 미술 검토 대상이다.

Cooled overlay와 Rift01 알파 상관계수0.99994, Rift02 -0.02853이다. **Rift01 hot→cooled 교체 후보**이며 Rift02 전체에 맞는 냉각 변형으로 사용하면 안 된다. hot 그림 위에 덧씌우기만 하면 붉은 열기를 제거하지 못하므로 hot 장식 숨김/교체와 잔해 등 복합 표현이 필요하다. Rift02의 같은 위치 냉각 변형까지 요구한다면 추가 아트가 필요하지만 이번에 새 필수 파일을 확정하지 않는다.

## 6. Quest Objective9별 아트 대응

| Objective | 납품 아트 대응 / 제한 |
| --- | --- |
| enter_deep_core | Ground_Base + Rift01/02 + DeepCliff_Blocker; Entry/Spawn/Bounds는 후속 구현 |
| inspect_core_rift | Rift01/02 조사 오브젝트, 조사 Marker/자막은 별도 Runtime |
| follow_colossus_trace | 추가 납품 ColossusTrace, Ground 위 흔적 단서; 새 필수 Asset 추가 없음 |
| reach_colossus_arena | BossArena_Ground, 이동 영역과 Collider는 별도 |
| confront_veinfire_colossus | Colossus_Sheet Idle/정식 본체; Main19 실루엣 픽셀 재사용 안 함 |
| defeat_veinfire_colossus | 기본/Phase2 표현+VFX7, 공용3행 수정·Phase2 매핑 선행 필요 |
| inspect_collapsed_core | CollapsedColossus 및 KO 마지막 프레임, 승리 후 일회 배치 |
| confirm_heat_recession | Rift01→CooledRift_Overlay + hot VFX 숨김/열빛 감소 + 잔해; 전체 냉각/약한 진동은 Runtime 표현 |
| return_from_deep_core | Ground/Cliff로 안전한 귀환 통로 표현 가능, 실제 Exit/Spawn/Navigation은 별도 |

9목표의 시각 역할은 대응 후보를 찾았다. 실제 조사/승리/귀환 가능 여부를 이미지 검사만으로 PASS하지 않는다. Geometry/좌표/정확한 Arena 크기는 미확정이며 환경 그림만으로 Collider를 자동 생성하지 않는다.

## 7. Unity 통합 전 설정·수정 대상

실제 Import 없음. 다음은 LOCAL 소스 선례를 읽은 정적 분석이며 Editor 상태 확인/Runtime PASS가 아니다.

| 대상 | 준비 설정 / 연결 |
| --- | --- |
| Boss 기본 | Sprite Multiple·Point·무압축·sRGB/Alpha transparency·mipmapOff·PPU314·pivot(0.5,0)·MaxTextureSize2048 이상, 원본1256 보존 |
| Boss Slice | 좌상단 행 우선00~15, UnityRect(x=col×314,y=(3-row)×314,w=h=314), 4개씩 ExplicitIdle/Attack/Hit/Defeat 배열; 8FPS 선례·KO 마지막 유지 |
| Phase2 overlay 선택 시 | Multiple·동일 Slice/PPU/Pivot·본체와 동일 RectTransform/Scale·RaycastOff. 별도 Image/Renderer의 같은 frame index 동기화·Phase flag·KO 소거 필요. 전체1256 이미지를314 본체 셀 위에 얹으면 안 됨 |
| Field 환경 | Sprite Single·Point·무압축·PPU128 선례·Alpha transparency·mipmapOff. 지면 center pivot/Tiled 후보, Cliff/잔해 발밑 pivot 권장; Rift/cooled는 같은 center pivot/scale/좌표 |
| Field Sorting | TagManager에는 Default만 존재. 새 SortingLayer 불필요; Main19 선례 Ground-20, Rift-18, 흔적-17~-16, 환경VFX-15, Cliff-8 참고. 실제 Player 가림/좌표는 후속 QA |
| Battle VFX | Sprite Single·center anchor·전투 UI의 별도 Image·RaycastOff·원본 색/alpha 보존·PPU128 선례. Presenter에 static1frame/transform/opacity 매핑 추가, 잘못된4분할 금지 |
| Telegraph | Boss Runtime 예고 flag→지속 HUD 텍스트→다음 자신의 행동에서 공격→flag 정리. Boss 패턴/타임라인과 일치, 실시간 반응속도 조건 없음 |

소스 근거: `MonsterSpriteSheetAnimation.cs`는 PlaySkill/PlayHit가 ExplicitHitFrames를 공유하고 Defeat 마지막 프레임을 유지한다. `Main18ContentBuilder.cs`는314PPU·4×4 발밑 slice·8FPS 선례다. `Main19ArtContentBuilder.cs`는128PPU·Point·무압축·Default sortingOrder를 사용한다. `BattleSceneController.cs`의 bossTelegraphText는 파수꾼 전용이며 `BattleActionPresenter.cs`에 Main20 static VFX 자동 매핑은 없다.

후속 수정 대상과 이유만 기록한다: Main20 전용 Phase/고정 패턴/지속 예고 상태, Controller의 Boss 행동·HUD 연결, Presenter의7static VFX, overlay 채택 시 애니메이션 frame 동기화 경로, Field11 표현의 Quest/Continue 상태 복원. 기존 공용3행 피격 규칙·다른 Boss·과열/Burn/SaveVersion은 변경하지 않는다. Phase2 완성 시트 채택 시에는 overlay Renderer 대신 배열 교체 경로가 필요하다.

Main19 실루엣은 먼 배경 예고 장식이며 다른 이미지/용도다. 넓은 어깨·준인간형·규모감 연결은 읽을 수 있지만 배경 실루엣과 Battle 본체를 같은 Asset으로 사용하지 않는다. 최종 연결감과 고유 외형은 사용자 검토를 남긴다. 이미지 비교만으로 제작 과정에서 픽셀 복제가 없었음을 증명했다고 주장하지 않는다.

## 8. 최종 판정·남은 작업·보호 검증

**NEEDS_ART_CORRECTION**. 이미지 열기와 기본16셀 slice는 가능하지만 무수정 정식 통합 준비 완료/사용자 미술 승인 완료로 표기하지 않는다. 별도 **NEEDS_HANDOFF_MAPPING** 항목은 기본 파일 별칭·Phase2 대안·Telegraph 명칭·환경4별칭·static VFX 방식이다. 최종 판정은 아트 수정을 우선한 하나로 기록한다.

- 수정/재납품 필수: Skill/Hit 공용3행의 완성형 방출 표현 수정. Phase2는 완성 시트 재납품 또는 overlay 대안 승인·정합 검토가 필요하다.
- 추가 Art: Trace는 이미 납품되어 추가 요청 불필요. Rift02/전체 cooled 변형은 실제 배치의 냉각 표현 요구에 따라 후속 검토하며 현재 새 필수 추가0. 기존 파일 임의 삭제0.
- 사용자 직접 검토: 고유 Boss 외형·Main19 연결·Idle/KO 흐름·Phase2 열핵 노출/정렬·공유행 수정본·어두운 배경 VFX 구분·Ground 반복 경계·압흔/Trace 테두리·hot/cooled/잔해 가독성. 모두 USER_ART_REVIEW_REQUIRED이며 자동 승격0.
- 순서: 아트 수정과 Phase2/매핑 계약 확인→수정 ZIP 재검수→사용자 미술 승인→별도 Main20 Unity 구현. 후속 구현 준비에는 신규 GUID/meta·원본 byte 보존·16셀 등록·VFX/HUD·Field Bounds/Sorting/Exit·Quest/Continue·기존 자산 보호 검증이 포함된다. 이번에는 착수하지 않는다.
- 이번 변경 문서2개: 이 보고서와 CURRENT_STATUS 상단 요약. Main20 기존 정본/Handoff/QA 수정0. Unity 전체 Assets/Packages/ProjectSettings/UserData·Main18 원본/checkpoint·Main18/19 Manifest·Main20 정본문서·ZIP의 보호3360파일 경로/SHA256 집계를 작업 전후 대조했으며 동일했다.
- 원본 ZIP/PNG 변경0·Unity Import0·Scene/C# 변경0·meta/GUID/Save 변경0·TTS/API0·Play0·Push0. 문서 링크/파일 목록18/PNG17/정본 매핑16/목표9를 작성 후 검증해 통과했고 git diff --check도 통과했다. 문서만 commit하며 기존 LOCAL 변경은 포함하지 않는다.
