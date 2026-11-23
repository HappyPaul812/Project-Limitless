# LIMITLESS / 리뉴얼 아트 / Issue #124 — Ch1·2 실사용 비고유 전투 VFX·아이콘 집중 감사

- **일자:** 2026-11-23 (KST) / **담당:** `docs/art-renewal`
- **의뢰:** [Issue #124](https://github.com/HappyPaul812/Project-Limitless/issues/124) 2026-11-23 최신 발주
- **결과:** `TARGETED_STATIC_REFERENCE_AUDIT_COMPLETE / 3_PRIORITY_GROUPS_IDENTIFIED / PILOT_NOT_PRODUCED / UNITY_NOT_RUN / REPLACEMENT_APPROVAL_PENDING`
- **직접 검사한 정본:** GitHub 복구 체크포인트 commit `f64082914e114096b6b9393263e7a36f7d829a55` (`backup/2026-11-19-safe-checkpoint`, 복구 기록 #121). 7,502 Git blob 파일 목록 전체, `truncated=false`. 과거 #57 [첫 실사](Renewal_NonOriginal_ActiveVsStored_Audit_NPCPilot2_20261023.md)와 [Issue #57](https://github.com/HappyPaul812/Project-Limitless/issues/57)의 54행 감사 요약 재사용. 같은 자료를 처음부터 재감사/Unity 전수QA 하지 않았음.
- **중요한 한계:** 사용자 PC의 현재 로컬 HEAD/Scene/Prefab/실제 Unity GameView를 직접 열어 확인하지 못했음. 복구본의 serialized GUID·MonsterDefinition loot·Resources 동적 로드·UI 제시 코드가 `STATIC_REFERENCE_CONFIRMED`임을 증명하는 것이며 `LIVE_RENDER_CONFIRMED`가 아님. Dynamic Resources는 Scene의 Sprite GUID가 없더라도 사용할 수 있으므로 GUID 없음만으로 미사용 판정 금지.

## 1. 상위 우선순위 최대 3개

| 우선 | 군 / 항목 | 실사용 위치 및 입증된 연결 | 사용권·출처 | 교체 난이도 / 중복 제작 | 판정 |
|---|---|---|---|---|---|
| **P1** | **Game-icons.net White 9PNG: Path trait 5 + 재료 아이콘4** | `_Project/Resources/PathDefinitions/01~05`의 `traitIcon` GUID5와 `UI/PathSelectionController.cs`의 `path.TraitIcon` 직접 연결. `ItemDefinitions/GrassSlimeGel, VenomBeeStinger, ForestSpiderSilk, VenomSnakeScale`의 `icon` GUID4, 해당 `MonsterDefinitions/01~04` loot item ID, `InventoryPresenter.BuildItemRows`의 `item.Icon` 제시 경로 연결 | 패키지 `ThirdParty/GameIconsNet/license.txt` 및 사이트 개별 아이콘: **CC BY 3.0 상용 허용·저작자 표시 필요**. Lorc(재료4 + cog), Delapouite(eye-target/mesh-network/heart-shield), Skoll(sound-waves). 출처 https://game-icons.net | **중간**: 9 PNG 개별 대체, 가독성·32/50px UI/투명도/아이콘 tint, Path/Item serialized 참조 교체 시 GUID·인벤토리·전리품 회귀 검증 필요. 자체 길 공식 `pathSymbol` 5와 다르며 재제작 금지. #96 HP/MP 물약2·#116 Ch3 잡템6 제외. 현재 복구본의 trait·material GUID는 여전히 외부를 참조하므로 동일 항목의 승인 독립 대체본 매핑은 **미발견** | `ACTIVE_SERIALIZED_GUID_AND_UI_CODE_MAPPED / LIVE_NOT_VERIFIED` |
| **P2** | **PVFX Foundry / SpectralBloom 정화 VFX** | `BattleSceneController.cs` 정화 선택(1545행 부근) → `BattleCleanseVisuals.LoadFrames()` → `Resources.Load<Texture2D>(BattleSkillEffects/SpectralBloom/spectral_bloom_sheet)` → `BattleActionPresenter`의 정화 연출. 버튼은 동일 시트 peak5를 고정 사용 | 원본 `Art/Effects/ThirdParty/PVFX_Foundry_0.3.0/LICENSE.txt` 범위 내 **CC0 1.0**, 표기 권장 `VFX from PVFX Foundry by Pixel VFX Studio`, 의무 크레딧은 아님 | **중간**: 96×96, 5열, 실제16프레임(시트 사용영역5×4), 20FPS, 상태 정화 **release index7**·버튼 **peak index5**, EffectSize132×132/Tint 유지. 핵심 타이밍 보호. `Resources/BattleSkillEffects/SpectralBloom`은 원본 팩 보관이 아니라 런타임용 복사본. 승인 오리지널 대체 PNG는 복구본의 동일 매핑에서 미발견 | `ACTIVE_DYNAMIC_RESOURCE_CODE_MAPPED / GAMEVIEW_STYLE_NEED_NOT_VERIFIED` |
| **P3** | **Kenney BattleIcons UI 아이콘군 21PNG** | `BattleUiIconCatalog.cs` `KenneyBattleIcons/...` 경로 다수 매핑, `BattleSceneController.cs` 전투 명령 버튼의 Attack(sword), Skill(star), Defend(shield), Flee(exitRight), Cancel(arrowLeft) 코드 호출까지 확인. `ThirdParty/Kenney/BoardGameIcons/Resources` 14PNG + `GameIcons/Resources` 7PNG | 양 팩 LICENSE.txt의 **CC0**, 상용 사용 허용, 출처 표기 선택 | **중간~높음**: 버튼/스킬/쿨다운 전체 iconId·캐시·화면/색상 구분 회귀 영향. 개별 21개 모두 실게임 동시 노출 입증은 아님. 기존 자체 BattleItemIcons/스킬 연출에서 파생한 일부 독립 아이콘은 중복 제작 금지 | `ACTIVE_DYNAMIC_RESOURCE_CODE_MAPPED / ALL_21_ONSCREEN_NOT_VERIFIED` |

### P1 실물 GUID 직접 대조 (최신 복구본)
아래 `GUID`는 각 `Assets/ThirdParty/GameIconsNet/.../*.png.meta`에서 직접 읽고 실제 `*.asset` 참조 값과 같음을 확인. 나열된 것은 **확실한 직렬화 참조9종**이며 단지 ThirdParty 디렉터리에 있다는 의미가 아님.

| 구분 | 연결된 게임 데이터 / 슬롯 | 외부 PNG (GameIconsNet 아래) | GUID | 원작자 |
|---|---|---|---|---|
| 길 / 시각 | `PathDefinitions/01_VisionPath.asset:traitIcon` | `Path/White/eye-target.png` | `c94e7ffd3ad84d729b36c90c4ca0f80e` | Delapouite |
| 길 / 청각 | `02_HearingPath.asset:traitIcon` | `Path/White/sound-waves.png` | `e54a40b325884dfe94b051148ef04d14` | Skoll |
| 길 / 지적 | `03_IntellectualPath.asset:traitIcon` | `Path/White/mesh-network.png` | `b5ddb75c1df7465fb0494dfd5c76a6e7` | Delapouite |
| 길 / 지체 | `04_MobilityPath.asset:traitIcon` | `Path/White/cog.png` | `765c9dee6e494a2cbc30f76d637b5066` | Lorc |
| 길 / 마음 | `05_EmotionalScarPath.asset:traitIcon` | `Path/White/heart-shield.png` | `ecc84d2dd627400cb76d063c617fff46` | Delapouite |
| 전리품 / 슬라임 젤 | `ItemDefinitions/GrassSlimeGel.asset:icon` | `Material/White/dripping-goo.png` | `40b87c1443b1efa4f82e58cbfd197337` | Lorc |
| 전리품 / 독침벌 독침 | `ItemDefinitions/VenomBeeStinger.asset:icon` | `Material/White/wasp-sting.png` | `73b52f1f2998f6646a226eed86602ed4` | Lorc |
| 전리품 / 숲거미 실 | `ItemDefinitions/ForestSpiderSilk.asset:icon` | `Material/White/spider-web.png` | `fa18969478fc18e4c9f6dc15069b6999` | Lorc |
| 전리품 / 맹독뱀 비늘 | `ItemDefinitions/VenomSnakeScale.asset:icon` | `Material/White/dorsal-scales.png` | `aeb1ca89347241c4289e2bbb53e91c8a` | Lorc |

**원작자 확인 링크:** https://game-icons.net/1x1/lorc/dripping-goo.html , https://game-icons.net/1x1/lorc/wasp-sting.html , https://game-icons.net/1x1/lorc/spider-web.html , https://game-icons.net/1x1/lorc/dorsal-scales.html , https://game-icons.net/1x1/delapouite/eye-target.html , https://game-icons.net/1x1/delapouite/mesh-network.html , https://game-icons.net/1x1/delapouite/heart-shield.html , https://game-icons.net/1x1/lorc/cog.html , https://game-icons.net/1x1/skoll/sound-waves.html .
사이트 저작자 표기는 2026-11-23 조회 기준이며, 본 감사에서 GitHub PNG와 사이트 이미지를 화소대조한 것은 아님.

## 2. 보관·미확인·완료 항목과 분리

| 대상 | 증거 및 상태 | 처리 |
|---|---|---|
| `Art/Effects/ThirdParty/PVFX_Foundry_0.3.0` 원본 ZIP/원본 시트·manifest | 원본 팩 보관, 별도 `_Project/Resources/BattleSkillEffects/` 런타임 사본 사용. 경로가 다름 | **STORED_SOURCE_KEEP**, 삭제 금지 |
| `ThirdParty/GameIconsNet/Path/Black` 5 PNG | 이번 대상으로 확인한 `PathDefinitions/traitIcon` 5는 **White** GUID에 연결; Black5는 해당 슬롯 참조 없음. 다른 Scene/동적 로드 전수는 안 함 | `STORED_IN_SCOPED_DEFS / GLOBAL_USAGE_NOT_VERIFIED`; 지금 제작·삭제 금지 |
| PVFX 추가 효과 `WarmExplosion/SolarShrapnelCharge/ElectricImpact/ArcaneParry/EarthRupture/FrostNova/RadiantHeal` | 코드와 Resources에 존재하며 Fireball·Thunderbolt·방어 등 프레젠터 호출도 확인. 단 일부 화면/프레임의 제품 실제 노출 및 필요성은 별도 | **후순위 KEEP / 이번 상위3 대상 외**, 신규 제작 금지 |
| Game-icons.net의 `pathSymbol` 5 | `PathDefinitions`의 `pathSymbol`은 독립 프로젝트 고유 문장 GUID; `traitIcon`과 다른 슬롯 | 이미 자체 제작·승인분 **KEEP** |
| Kenney RPGBase·Schwarnhild·Xariami ThirdParty 원본 | 이전 #57 장기 목록의 **보관 사실** 외 현재 활성 Renderer/GUID를 이번 세 범위에서 증명하지 못함. Xariami 라이선스 근거 미확보 | `NOT_VERIFIED`, 새 제품 도입·삭제 금지 |
| HP/MP 작은물약 #96 · 시작마을 NPC·승인 전투 BG #91 · 공식 몬스터·펫·불씨망령 | #124에서 명시적으로 보호/감사 제외 | **완료 항목 재조사·재제작0** |

## 3. 소량 VFX 파일럿 게이트 판정

- **이번 회차 새 PNG·GIF 없음.** `SpectralBloom`은 정적 호출 경로와 16프레임 계약까지 확인되어 기술 규격은 확보되었지만, 기존 CC0 연출의 **실제 GameView 시각적 교체 필요성**, 접근성/가독성 결함, 이미 승인된 변경 후보 여부를 **제품 기준**으로 최종 확인하지 못했다. #124가 정한 “실제 참조·교체 필요성 **확정**된 VFX 하나만” 조건을 넘겨 판단하지 않음.
- Game-icons.net 9종은 **실참조 확정·저작자 표시 중요도 P1**이나 #124의 이미지 파일럿 허용 범위가 **VFX 1개**이므로 아이콘 9개를 임의 제작하지 않음.
- 후속 조건: Codex가 안전한 별도 격리 환경에서 **정화 VFX 기존 게임 화면(시작/peak5/release7/종료), UI 64px 가독성**을 읽기·캡처 전용으로 검증해 시각 교체 필요성을 확정하면 **SpectralBloom 1개만** 96×96×16 / 5열 / 20FPS 파일럿 발주. 기존 Player Save와 스킬 효과·타이밍·레이어/이름표·입력·전투 결과 유지. 단순 CC0라는 이유만으로 전투 전체 VFX를 동시에 원본화하지 않음.

## 4. 보호·인계·결론

- 이번 실행은 **GitHub 문서 작성만**, `docs/art-renewal` 외 변경 없음. Unity **6000.6.5f1**, Issue **#85 HOLD**, Scene/Prefab/Texture/.meta/GUID, Resource/Runtime/전투 피해·발동 프레임, ThirdParty 라이선스·원본, Save/UserData, Discord, `main` 모두 **변경0**.
- 판단은 11/19 remote 복구본 확인이며 최신 사용자 PC 실물/실제 Unity 재생 증거는 `NOT_VERIFIED`. 근거 없는 실플레이 PASS 숫자 생성 금지. 11/19 #121 NPC50PASS·불씨망령30PASS는 **기존 기록 참고**일 뿐 이번 감사의 신규 QA로 합산하지 않음.
- **결론:** **확실한 외부 직렬화 아이콘 참조 9개** + **PVFX SpectralBloom 런타임 코드 매핑** + **Kenney BattleIcons 코드 매핑**으로 상위3군 확정. GameView 렌더 검증 전 외부 사용자 노출 확정/실제 대체 필요성 최종 승인 아님. 위험 우선순위 P1 CC BY 저작자/크레딧 정리, P2 VFX 미술판단, P3 Kenney 추후 필요시 최소교체. 오리지널 파일럿 및 제품 교체는 **추가 미술/기술 승인 필요**.
