# Main20 Art Pack v2 재검수 — 2026-10-09

**최종 판정: NEEDS_ART_CORRECTION.** Boss 공용3행의 방출 표현 제거와 Phase2 Overlay의 프레임 정렬은 PASS. 다만 `DeepCore_Cliff_Boundary.png`에 회색 사각 가이드선이 실제 PNG 픽셀로 남아 있어 Handoff의 가이드선 금지와 충돌한다. 이 파일 수정 후 재검수해야 한다. Phase2 Overlay 채택은 기술적으로 가능하지만 사용자 승인 전에는 최종 확정하지 않는다. USER_ART_REVIEW_REQUIRED / UNITY_IMPORT_PENDING / IMPLEMENTATION_PENDING.

## 1. 범위·원본·보호

- 시작 LOCAL HEAD `cad75deecbc52d18312a5e0875025fc605b47114`. 최근 commit cad75de /14327fd /7a78aa8 /f8ca63a /0483167 확인. 원격 조회·Push0.
- 시작 staged0, 기존 tracked 수정75·미추적 top-level25, 전체 status562행 보존. Chapter1·2 Side9 구현과 기존 Save/TTS/Audio/아트 보호.
- [Art Handoff](Main20_Art_Handoff.md) → [v1 검수](Main20_ArtPack_Inspection_20261009.md) → [Design QA](Main20_Design_QA.md) → [Story/Quest9](../03_스토리/Chapter2_Main20_심부의_거신.md)를 지정 순서로 확인. v1 문서는 이력 그대로 유지.
- 검사는 ZIP을 읽기 전용 메모리로 열어 CRC·PNG signature/IHDR·Pillow verify 및 전체 decode, alpha·16셀·byte SHA256을 검사. 배경 합성·타일 반복·Row3 전후·Overlay16프레임을 비대화형 파생 이미지로 검토. 이미지 비교는 사용자 미술 승인/Unity Runtime 검증과 구분.
- 파생 검사 이미지/JSON은 세션 visualization 공간 `main20_v2`, 검사 스크립트는 ignored Temp에만 저장. 원본 게임 PNG를 수정하거나 Unity에 복사하지 않음. Unity Import/Scene/C#/meta/GUID/Save 변경0, Play/포커스 조작/TTS API/Audio 변경0.

| ZIP | bytes | SHA256 | PNG / 기타 | 무결성 |
|---|---:|---|---|---|
| v1 `F:\Downloads\Limitless_Main20_DeepCore_VeinfireColossus_Art_Pack.zip` | 2,304,572 | `660bd5ada46b27c7099770441dbf32a516b922402389c0bf57a7f6982797ef3e` |17 /1 Manifest|PASS|
| v2 `F:\Downloads\Limitless_Main20_DeepCore_VeinfireColossus_Art_Pack_v2.zip` | 5,099,833 | `5be1678d6be207e1dc81414b5392bd00d429bc1af580d78f185863eb0c40fef4` |21 /2 text|PASS|

v2 파일23개 = **게임용RGBA PNG17(Boss2/VFX7/환경8) + QA용RGB PNG4 + Manifest.txt + QA/Review Notes.md**. Manifest의17개는 게임용 집합이고 QA4는 별도 목록으로 기재되어 수량 충돌 없음. CRC 오류0·PNG 손상0·중복 상대 경로0·위험 경로0·디렉터리 엔트리0. PNG21개의 SHA256은 서로 다름.

## 2. v1→v2 변경 목록

역할별 비교: 게임용PNG 수정5 /byte 미수정12 /게임 역할 추가0·삭제0. 이름 변경8개 중6개는 원본byte 동일, 기본Boss와Arena2개는 rename+수정. Manifest 수정1. 신규 QA PNG4+검토노트1. 경로 집합만 비교하면8경로 제거·13경로 추가이지만8개는 아래 rename 대응이므로 게임 자산 삭제로 해석하지 않음. VFX7 원본byte 모두 보존, Trace는 존재·역할 보존하며 테두리 픽셀 수정.

| v1 상대 경로 | v2 상대 경로 | 비교 |
|---|---|---|
| Boss/Veinfire_Colossus_Phase2_Overlay.png | Boss/Veinfire_Colossus_Phase2_Overlay.png | MODIFIED |
| Boss/Veinfire_Colossus_Sheet.png | Boss/Veinfire_Colossus_Sprite_Sheet.png | MODIFIED + RENAMED |
| Environment/DeepCore_BossArena_Ground.png | Environment/DeepCore_Arena_PressureMark.png | MODIFIED + RENAMED |
| Environment/DeepCore_CollapsedColossus.png | Environment/DeepCore_CollapsedCore.png | BYTE_IDENTICAL + RENAMED |
| Environment/DeepCore_ColossusTrace.png | Environment/DeepCore_ColossusTrace.png | MODIFIED |
| Environment/DeepCore_CooledRift_Overlay.png | Environment/DeepCore_HeatRecession.png | BYTE_IDENTICAL + RENAMED |
| Environment/DeepCore_DeepCliff_Blocker.png | Environment/DeepCore_Cliff_Boundary.png | BYTE_IDENTICAL + RENAMED |
| Environment/DeepCore_Ground_Base.png | Environment/DeepCore_Ground_Base.png | MODIFIED |
| Environment/DeepCore_Rift_01.png | Environment/DeepCore_Rift_01.png | BYTE_IDENTICAL |
| Environment/DeepCore_Rift_02.png | Environment/DeepCore_Rift_02.png | BYTE_IDENTICAL |
| Manifest.txt | Manifest.txt | MODIFIED |
| VFX/Veinfire_CoreCondensation_VFX.png | VFX/Veinfire_CoreCondensation_Telegraph.png | BYTE_IDENTICAL + RENAMED |
| VFX/Veinfire_CoreEruption_VFX.png | VFX/Veinfire_CoreEruption_VFX.png | BYTE_IDENTICAL |
| VFX/Veinfire_CoreResonance_VFX.png | VFX/Veinfire_CoreResonance_Telegraph.png | BYTE_IDENTICAL + RENAMED |
| VFX/Veinfire_CoreWave_VFX.png | VFX/Veinfire_CoreWave_VFX.png | BYTE_IDENTICAL |
| VFX/Veinfire_HeatPressure_VFX.png | VFX/Veinfire_HeatPressureInjection_VFX.png | BYTE_IDENTICAL + RENAMED |
| VFX/Veinfire_HeatWave_VFX.png | VFX/Veinfire_HeatWave_VFX.png | BYTE_IDENTICAL |
| VFX/Veinfire_MoltenStrike_VFX.png | VFX/Veinfire_MoltenStrike_VFX.png | BYTE_IDENTICAL |

신규: `QA/Main20_Boss_Row3_BeforeAfter.png`, `QA/Main20_Environment_BeforeAfter.png`, `QA/Main20_Ground_Tiling_BeforeAfter.png`, `QA/Main20_Phase2_Composite_16Frames.png`, `QA/Main20_v2_Review_Notes.md`.

## 3. 전체 파일 기술 정보

게임PNG17은 실제 PNG8bit RGBA(IHDR color type6), QA PNG4는 RGB8(type2)이며 배경·라벨이 합성된 검토용으로 Unity 투명 Sprite 대상이 아니다. 표에서 QA alpha255는 RGBA 변환시의 불투명 관측이며 원본에 alpha 채널이 있다는 뜻이 아니다. Ground/Arena의 불투명은 정상 지면 계약, 게임 투명PNG15개에는 실제 투명 영역 있음. 해상도·RGBA/opaque 설명은 Manifest17개 모두 실제와 일치. 별도 README 없음.

| 상대 경로 | 실제 형식 / 해상도 | bytes | alpha min~max / 완전투명pixel | SHA256 |
|---|---|---:|---|---|
| Boss/Veinfire_Colossus_Phase2_Overlay.png | PNG RGBA8 /1256×1256 | 266,722 | 0~129 /1,417,084 | `f566f96662d156ed76495a10aa62512530110a7fe5c684a042799fb0e9da28f6` |
| Boss/Veinfire_Colossus_Sprite_Sheet.png | PNG RGBA8 /1256×1256 | 540,211 | 0~255 /758,138 | `bc8a290bd0742a9043e9b431a6faec772c5698e57c96aac6e9c7f7e36a8f5f6f` |
| Environment/DeepCore_Arena_PressureMark.png | PNG RGBA8 /512×512 | 457,445 | 255~255 /0 | `3b5cc0a7cbc820045c7d67827f336d54cd6579b15b7970c18f2c6601e8dba1a7` |
| Environment/DeepCore_Cliff_Boundary.png | PNG RGBA8 /512×512 | 230,335 | 0~255 /115,034 | `e020a8681401005c8664fc12a5afdc7d61a1725a6c2bd8f5aad6178fa14a5c9c` |
| Environment/DeepCore_CollapsedCore.png | PNG RGBA8 /512×512 | 33,594 | 0~255 /186,854 | `a06c5ec34ae1771374b4a8c02b24d99531c4749d9125fb79b4261b6d5fe6b99a` |
| Environment/DeepCore_ColossusTrace.png | PNG RGBA8 /512×512 | 200,590 | 0~255 /121,727 | `f463f7a33ff6370b8d34af76c6d446771fabcc4c44b6412b4002f98d6a1b3b48` |
| Environment/DeepCore_Ground_Base.png | PNG RGBA8 /512×512 | 455,477 | 255~255 /0 | `54e8fb28dff8877ed635177360d36ae189843f1a6d33a0e3623285ea0ac1cd78` |
| Environment/DeepCore_HeatRecession.png | PNG RGBA8 /512×512 | 23,353 | 0~107 /184,291 | `518280749821145dd2cb7a1fb47dc3681ec15cffc356789359f34b84a9b901d1` |
| Environment/DeepCore_Rift_01.png | PNG RGBA8 /512×512 | 29,675 | 0~234 /169,477 | `bd2f8820cad8979ba4054aea96338d7d58a14ed2b5253d2a837f4f2df6291b61` |
| Environment/DeepCore_Rift_02.png | PNG RGBA8 /512×512 | 29,504 | 0~234 /170,777 | `948a3ef40f6ae3b9f51c082635e45553ed607a4020afce3c173922fdcad411e8` |
| Manifest.txt | UTF-8 text | 7,587 | — | `ec87bf03cb90d2209333938218d1cb837f4a1cb4ca4300ccd400f287a85b052c` |
| QA/Main20_Boss_Row3_BeforeAfter.png | PNG RGB8 /1256×712 | 247,683 | 255~255 /0 | `6870f8cffc4477b4aba107bb64fe1f43f70ecd8668c6d6f5a60498aa34a87df3` |
| QA/Main20_Environment_BeforeAfter.png | PNG RGB8 /2080×565 | 864,941 | 255~255 /0 | `b42aeb9f0c3847f5798c277d37bca2fc997a9b16747a935f5f75f737a6388e8b` |
| QA/Main20_Ground_Tiling_BeforeAfter.png | PNG RGB8 /2048×1024 | 1,034,521 | 255~255 /0 | `b6e72d24aea2ad8d80bde6c8f6c53b4fb3700ec5224b1420b6297519ae4ae90d` |
| QA/Main20_Phase2_Composite_16Frames.png | PNG RGB8 /1256×1256 | 567,744 | 255~255 /0 | `659ec47d19dff0102bf1c19e04f5e22d54845236b616f9919719b34945efd639` |
| QA/Main20_v2_Review_Notes.md | UTF-8 text | 2,597 | — | `b0421b1f0fc3397bd95cc717cf0784669f038a4309933bedafe80e460a31c074` |
| VFX/Veinfire_CoreCondensation_Telegraph.png | PNG RGBA8 /512×512 | 19,874 | 0~164 /209,659 | `67b91c92b3ed0cdaa1fa90985076a50dfa297512d30826bef113480041a13a90` |
| VFX/Veinfire_CoreEruption_VFX.png | PNG RGBA8 /512×512 | 49,117 | 0~169 /160,592 | `f163c75daeab7b9f54b077223c29376a9a646326fc2196a468bfb0cd504d6ab9` |
| VFX/Veinfire_CoreResonance_Telegraph.png | PNG RGBA8 /512×512 | 15,032 | 0~150 /200,063 | `ac41ef8a18c3b1ee1c1bca5ee53bdf94e2cb0910749c1298b26aa8d4df66fc54` |
| VFX/Veinfire_CoreWave_VFX.png | PNG RGBA8 /512×512 | 16,522 | 0~181 /176,731 | `373a8062c16ce899f32642fbe3388bd9a7ef86d7f3b4e0427a05ccaef085a463` |
| VFX/Veinfire_HeatPressureInjection_VFX.png | PNG RGBA8 /512×512 | 16,816 | 0~159 /198,964 | `e6e59ccc4bd2412796535757c928261b34f938b30439681125c4c50df601fefc` |
| VFX/Veinfire_HeatWave_VFX.png | PNG RGBA8 /512×512 | 50,529 | 0~155 /121,733 | `2a8eb0379b6e96e56cccdc2ba5ec0298a014fb8539c3ed9b5a666d2d49d51b2c` |
| VFX/Veinfire_MoltenStrike_VFX.png | PNG RGBA8 /512×512 | 26,072 | 0~230 /201,233 | `bc4bd18f45d9aba93f3ace5ce823e1db7c9b6220a95b03dacdb4eef985fc1de7` |

## 4. 정본16개 대응

**basename 정확 일치15/16**(v1은7/16). 정본 Phase2 완성시트1개는 미납품이고 Handoff에 명시된 Overlay 대안1개로 대응 가능. 게임 역할16개는 모두 대응 후보가 있으며 추가Trace1개는 정본 목록을 임의 확대하지 않고 목표3에 사용 가능한 추가 납품으로 보존. Phase2 대안 승인 대기까지 정본 완전충족16/16로 기록하지 않는다.

| 정본 파일명 | v2 상대 경로 | 판정 / 역할 |
|---|---|---|
| Veinfire_Colossus_Sprite_Sheet.png | Boss/Veinfire_Colossus_Sprite_Sheet.png | PASS, 기본16셀·공용3행 |
| Veinfire_Colossus_Phase2_Sprite_Sheet.png | Boss/Veinfire_Colossus_Phase2_Overlay.png | PARTIAL, 기술적 Overlay 대안 PASS·사용자 채택 대기 |
| Veinfire_HeatPressureInjection_VFX.png | VFX/Veinfire_HeatPressureInjection_VFX.png | PASS, 열압 주입·단일 직접피해 후 생존 과열1 |
| Veinfire_MoltenStrike_VFX.png | VFX/Veinfire_MoltenStrike_VFX.png | PASS, 용융 강타·단일 직접피해/Burn |
| Veinfire_CoreCondensation_Telegraph.png | VFX/Veinfire_CoreCondensation_Telegraph.png | PASS, 열핵 응축·피해0·파동 예고 |
| Veinfire_CoreWave_VFX.png | VFX/Veinfire_CoreWave_VFX.png | PASS, 열핵 파동·전체 직접피해·상태추가0 |
| Veinfire_CoreResonance_Telegraph.png | VFX/Veinfire_CoreResonance_Telegraph.png | PASS, 열핵 공명·피해0·분출+과열 예고 |
| Veinfire_CoreEruption_VFX.png | VFX/Veinfire_CoreEruption_VFX.png | PASS, 열핵 분출·전체 직접피해 후 생존 과열1 |
| Veinfire_HeatWave_VFX.png | VFX/Veinfire_HeatWave_VFX.png | PASS, 열파·전체 직접피해·상태추가0 |
| DeepCore_Ground_Base.png | Environment/DeepCore_Ground_Base.png | PARTIAL, 경계밝기 개선·반복 띠 잔존 |
| DeepCore_Rift_01.png | Environment/DeepCore_Rift_01.png | PASS, 주요 hot 균열 |
| DeepCore_Rift_02.png | Environment/DeepCore_Rift_02.png | PASS, 대체 hot 균열·전용cooled 없음 |
| DeepCore_Arena_PressureMark.png | Environment/DeepCore_Arena_PressureMark.png | PARTIAL, 압흔 완화·경계 연결 시각검토 |
| DeepCore_Cliff_Boundary.png | Environment/DeepCore_Cliff_Boundary.png | FAIL, 회색 사각 가이드선 픽셀 잔존 |
| DeepCore_CollapsedCore.png | Environment/DeepCore_CollapsedCore.png | PASS, 승리 후 잔해 |
| DeepCore_HeatRecession.png | Environment/DeepCore_HeatRecession.png | PARTIAL, Rift01만 대응·전체Field 냉각은 별도 표현 |

## 5. Boss 공용3행·Phase2

기하 PASS: 기본·Overlay 모두1256×1256,314×314,4열4행16셀. 기본16셀 비어있음0·외곽alpha>0 pixel0·아래bbox 끝302(여백12px) 동일, 명백한 셀 잘림/인접셀 침범 없음. v1 기본00~09/12~15 픽셀 완전 동일, **10/11만 변경**.

**3행08~11 PASS(시각/정적 공유 적합성).** v1의10 대형 원형 방출·손불꽃과11 방출선 제거. 08/09 자세를 활용한 몸통 반응/작은 이동으로10/11 교체, 본체에서 바깥으로 발사하는 효과가 없어 피격에도 사용 가능한 중립 긴장 자세. Skill 특수공격은 이미 분리된 VFX7로 표현할 수 있음; VFX 자체는 수정되지 않았으므로 새 방출 애니메이션 납품이라고 주장하지 않음. 공용행의 실제8FPS 피격/스킬 재생 자연스러움은 PARTIAL(미검증·사용자 확인).

Phase2는 **Overlay**이며 독립 완성 본체시트가 아니다. v1 고정 붉은 원형 영역 대신 v2 각 프레임의 본체 열핵·붉은 균열을 강조한다. 자체 생성16셀 alpha 합성 및 납품 QA16셀 합성 조회로 대응 확인. 기본 몸체를 공유해 캐릭터 비율 유지. Overlay가 본체 투명부에 나타나는 pixel은 v1총15,187→v2총0. 살아있는00~11 overlay의 붉은 본체 영역 대응 비율97.36~99.39%(R>1.3G,R>1.3B,base alpha>0 휴리스틱). 이 색상 지표만으로 정렬 PASS한 것이 아니라 합성 이미지도 검사했다.

| frame | 기본 bbox(좌상단,우하단exclusive) | 기본 v1 동일 | Overlay alpha>0 pixel | max alpha | 본체 밖 Overlay pixel |
|---:|---|---|---:|---:|---:|
| 00 | [11, 32, 302, 302] | YES | 8,842 | 129 | 0 |
| 01 | [16, 15, 298, 302] | YES | 9,963 | 129 | 0 |
| 02 | [11, 42, 302, 302] | YES | 10,684 | 129 | 0 |
| 03 | [11, 50, 302, 302] | YES | 10,161 | 129 | 0 |
| 04 | [11, 32, 302, 302] | YES | 9,941 | 129 | 0 |
| 05 | [14, 15, 299, 302] | YES | 10,186 | 129 | 0 |
| 06 | [11, 39, 302, 302] | YES | 15,633 | 129 | 0 |
| 07 | [11, 43, 302, 302] | YES | 13,286 | 129 | 0 |
| 08 | [11, 30, 302, 302] | YES | 10,873 | 129 | 0 |
| 09 | [11, 30, 293, 302] | YES | 17,716 | 129 | 0 |
| 10 | [13, 30, 295, 302] | NO | 18,069 | 129 | 0 |
| 11 | [10, 30, 301, 302] | NO | 10,531 | 129 | 0 |
| 12 | [11, 65, 302, 302] | YES | 7,667 | 47 | 0 |
| 13 | [11, 28, 302, 302] | YES | 6,900 | 21 | 0 |
| 14 | [11, 21, 302, 302] | YES | 0 | 0 | 0 |
| 15 | [11, 121, 302, 302] | YES | 0 | 0 | 0 |

KO Overlay 소거 PASS:00~11 alpha최대129→12최대47→13최대21→14/15완전투명. **전체Boss 열빛소거/붕괴 흐름은 PARTIAL**: 기본14는 열핵이 밝고 상체가 다시 커지는 기존 그림 그대로이며 Overlay 소거만으로 본체까지 소거되지는 않는다. 마지막15 잔해의 잔열은 허용되나13→14→15 실제 연속 자연스러움은 사용자 확인 필요.

**Overlay 채택 가능성 PASS(기술), 최종채택 PARTIAL(승인 대기)**. 같은16 Rect/PPU314/pivot(0.5,0)·동일scale/flip/frame index로 본체와 동기화할 수 있다. 현재 MonsterSpriteSheetAnimation.ShowFrame은 본체Image/SpriteRenderer만 갱신하므로 동기화 연결은 후속 소규모 구현이 필요하며 이미 지원된다고 주장하지 않는다. KO frame도 같은index로 동기화. 전체1256 그림을314셀 위에 얹거나 자체시간을 독립재생하면 안 된다. 불필요하게 Phase2 완성시트 재제작을 요구하지 않음. 노출된열핵의 의미/강화 정도·고유외형은 사용자 미술 승인 대기.

## 6. VFX7 판정

전체 존재·PNG decode/투명도·역할매핑 **PASS7/7**, 파일별 정적 단일512×512·원본byte 동일. 4조각 slicing 금지. Handoff의4프레임 이하에는1프레임이 포함되지만 권장128/256px strip 대신512 static인 방식 선택은 통합 설정에 명시한다.

응축은 안쪽으로 모이는 분절된 타원 고리, 공명은 바깥 방향 살과 둥근 분절 고리로 구분 가능: **형태 구분 PASS / 실제화면 구분 PARTIAL**. 파동은 넓고 낮은 타원, 분출은 방사 균열/압력형: **형태 구분 PASS / 실제화면 PARTIAL**. 색상만으로 전달하지 말고 다음Boss행동까지 응축→파동/공명→분출+과열1 HUD텍스트를 유지한다.

PNG 조회상 넓은 흰색 섬광·strobe 프레임 없음: 정적표현 PASS. static PNG만으로 점멸 주기·최종전투 합성 밝기를 판단할 수 없어 접근성Runtime은 PARTIAL. Presenter는 Single center Image/RaycastOff, 완만한 opacity/scale, 과도한 섬광·화면Shake 금지; Telegraph는 안정프레임+지속HUD, 실제공격은 별도damage타이밍과 연결. Manifest의 MoltenStrike 'melee impact'는 아트 배치 설명이며 게임 공격 분류를 확정하지 않는다.

## 7. Field11 환경 판정

| 검사 | 판정 | v1 대비 관측 / 한계 |
|---|---|---|
| Ground 반복 경계 | PARTIAL | 상하24px RGB평균21.145→16.845, 중심평균19.700 동일. 좌우/상하edge MAE0 유지. 2×2에서 밝은선 완화됐으나 어두운수평띠·주기적 패턴은 남음. 수정 수행 PASS, 자연반복 최종승인 대기 |
| Arena→Ground 연결 | PARTIAL | 균일한 방사열선 약화·분절 확인. Ground3×3 중앙에Arena 합성하면 테두리띠/압흔 패턴 전환은 보임. 실제크기/배치와 경계 자연스러움 사용자 검토 |
| 거신 흔적 | PARTIAL | 추가Trace 보존, 연속붉은 contour의채도/alpha 완화 확인. 압흔 존재·목표3 매핑PASS, 어두운실제Field 식별은 미검증 |
| 절벽/통행 구분 | FAIL | Cliff 그림 암석·틈은 읽히지만 **회색 사각 가이드선**이 sprite에 남음. v1byte 그대로이며 rename만 됨. Handoff '배경에 가이드선을 굽지 않음' 위반. Collider/통행폭은 이미지로 검증불가 |
| 승리 후 잔해 | PASS(표현) | CollapsedCore byte동일·투명 검은암석 잔해 존재, 목표7 대응. 실제 승리표현·Continue는 미구현 |
| 열기 완화 | PARTIAL | HeatRecession byte동일, alpha Rift01상관0.99994/Rift02 -0.02853. 동일transform에서Rift01 숨김/교체, hot환경표현 감소·잔해로 보완. Rift02/전체Field 냉각이미지 아님 |

**절벽 수정 근거**: 실제512PNG의alpha bbox(16,53,496,458), 상단점선 y≈53~56·왼쪽직선 x≈16~19·하단직선 y≈455~457이 암석 외곽과 무관한사각을 형성. 좌상단기준(x20,y53)RGBA(85,85,88,255), (x200,y457)(70,72,73,255)로 실제불투명 픽셀임을 확인. 검사기가 그린bbox선이 아니며 검사코드는원본위에경계선을그리지 않았다. v1에서 이부분을필수수정으로분류하지못했으나이번전체검토에서정본충돌로명시. 원본수정은하지않고아트담당재납품요청대상으로남김.

Ground/Arena/Trace 추가수정은 사용자 시각 검토 후 결정한다. Rift02 냉각판은 아직필수추가아트로확정하지않음. 이번필수수정은 기존Cliff의가이드선제거이며추가필수파일0.

## 8. Main20 Objective9 아트 대응

| objectiveId / target | 아트 / 후속 연결 |
|---|---|
| enter_deep_core /field11_main20_entry | Ground/Rift01·02/수정Cliff, Entry/Spawn/Bounds |
| inspect_core_rift /field11_main20_core_rift | Rift01 또는02 조사Site·Marker |
| follow_colossus_trace /field11_main20_trace | 추가ColossusTrace 단서·ReachLocation |
| reach_colossus_arena /field11_main20_arena | ArenaPressureMark와Ground 연결 |
| confront_veinfire_colossus /field11_main20_confront | 기본Boss Idle·정식전투전대화 |
| defeat_veinfire_colossus /field11_main20_veinfire_colossus | 기본+승인Overlay/VFX7·StoryEncounter |
| inspect_collapsed_core /field11_main20_collapsed_core | CollapsedCore·KO15, 승리후조사 |
| confirm_heat_recession /field11_main20_heat_recession | Rift01→HeatRecession·hot표현감소·잔해, 약한진동유지 |
| return_from_deep_core /field11_main20_return | Ground/수정Cliff·Field10귀환Exit |

9개 시각 역할 대응PASS, 실제진행/전투/Continue는전부미구현. Main19 실루엣은 먼배경 예고이며 Main20 정식Boss와 별도Asset으로 유지한다.

## 9. Unity 통합 준비·미결 계약

**통합 사전 조사·설정 목록 작성 완료 / 실제 Import·구현 준비 최종승인 미완료.** Cliff수정→재검수→사용자미술검토·Overlay대안승인→미결기술계약결정→별도구현 지시 순서.

- Boss/Overlay: Sprite Multiple·1256원본보존(MaxTextureSize2048이상/NPOT None)·Point·무압축·mipmapOff·PPU314/pivot(0.5,0). 좌상단행우선Slice의UnityRect=(col×314,(3-row)×314,314,314). Idle/Attack/Hit/Defeat4개씩, 기존8FPS선례·KO마지막유지. 기존PlaySkill/PlayHit공용을공격전용으로변경하지않음.
- Overlay: 작은frame동기화경로·Phaseflag·KO14/15소거·scale/flip/pivot일치, 별도Image/Renderer RaycastOff. 기본Sprite 전체캔버스와Overlay셀 혼동금지. 사용자승인전채택확정0.
- VFX: Sprite Single/center, static512원본보존,7행동명시Presenter매핑·부드러운scale/opacity·예고HUD와실제공격타임라인분리.
- Field: 기존Main19 PPU128·Point·무압축·Single선례,Ground center/Cliff·잔해발밑/Rift-cooled같은pivot/scale. Default sortingOrder선례Ground-20/Rift-18/흔적-17~-16/환경VFX-15/Cliff-8; 실제Player가림검증후결정. Geometry/Bounds/Camera viewport/Collider는별도설계,이미지에서자동확정0.
- **Phase2 전환중미실행예고/첫행동cursor**: 기존파동예고보존·취소·분출교체·Queue리셋·전환턴추가중어느것도임의선택하지않음. HP600경계1회,상태/Party/턴큐유지,이미예고한정보를거짓으로만들지않는계약선결정.
- **용융강타속성/대상**: 단일125%,생존대상공용Burn은정본. 근접/원거리·물리/마법분류와사거리는미결;Manifest 'melee'로확정하지않음. 기존Taunt·TargetResolver와후열규칙확인필요.
- **Boss EXP140**: BaseExperience140에기존레벨차배율적용경계. 최종항상140여부미결. 기존규칙기준BossLv15: PlayerLv≤8=210/9~11=175/12~18=140/19~21=70/≥22=0. 공용EXP를변경하지않음. BossTalent40과QuestEXP100/Talent80은별도한번지급,일반Loot0유지.
- **Field10↔Field11**: Main19완료Gate,왕복Exit/Spawn·비중첩·Bounds·BuildSettings/FieldConnection/Navigation등록. 현재Field11미구현,기존Field10차단을이번에변경하지않음.
- **The Weight of Crowns**: [Audio Handoff](Main20_Audio_Handoff.md)의승인원본·사용자청취상태확인,Encounter별BGM override/복귀Paths of Cracked Earth연결은후속. WAV/TTS/MP3복사·Import0.
- **Quest9/Boss패턴/Save·Continue**: 새정본ID,4/5행동·HP50%1회·공용Overheat/Burn·Taunt/Guard·IsBoss/Flee차단·승리만목표6·패배재도전·7~9표현/일회대화복원·보상중복방지. 기존Version1/5슬롯·Side9·추적·인벤토리·Party/Formation/Beast보호. 전투중중간Phase저장지원으로확대하지않음.

## 10. 최종 판정·사용자 확인·검증 마무리

**NEEDS_ART_CORRECTION**: 기본3행·Overlay 정렬이라는 v1 핵심 기술 문제는 해결했지만 Cliff 가이드선은 미해결이다. Phase2는 기술적으로 Overlay 채택이 가능하며 사용자 선택을 기다린다. 정본 이름15/16과 역할 대응을 문서화했으나 Overlay를 완성 시트로 PASS시키지 않는다.

사용자 미술 검토: Boss 고유 외형/Main19 연결·Idle 체격 변화·공용3행 피격/Skill·KO13/14의 재확대/잔열·Overlay 열핵 노출·어두운 배경 VFX 구분·Ground 반복 띠/Arena 연결/Trace 식별·cooled 강도. 자동 검사와 별개이며 승인 완료0. 정적 이미지21개는 모두 열리지만 최종 미술 합격이나 Runtime 통과를 의미하지 않는다.

이번 변경 문서는 이 보고서와 CURRENT_STATUS뿐이다. Git 관리/미추적 파일 및 사용자 Save/TTS 보호 기준4303파일 SHA256과 원본 ZIP2개 SHA256을 작업 전후 대조하여 CURRENT_STATUS 외 기존 파일 변경0을 확인했다. 이전 v1 문서/Handoff/Design QA/Story/Side9 코드·Asset·QA 자료는 불변이다. Markdown 링크·대응16/Objective9/Manifest17/전체23·PNG21 집계와 git diff --check를 검증했다. 마지막 관련 commit은 이 문서를 포함한 `Docs: Main20 아트 v2 재검수와 통합 준비 기록` 참조. GitHub Push0.
