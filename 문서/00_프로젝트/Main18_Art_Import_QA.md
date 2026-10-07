# Main18 Art Import QA

2026-10-08. 기술 Gate와 사람의 미술 검토는 별도 판정이다. 원본 17개 모두 **PASS_WITH_NOTE**, **FAIL_BLOCKING 0**, **USER_ART_REVIEW_REQUIRED 17**. 자동 시각적 완성 PASS로 승격하지 않았다.

ZIP SHA256: `4a3dca636251e4f6a12ce9f007b27be5babb36b98c0b5c9b88bf4a73aaa5de2b`

| 원본 | 크기·모드 | Alpha 최소/최대 | 기술 Gate | 구조 |
|---|---|---|---|---|
| UI/Item_Cooling_Remedy.png | 128×128 RGBA | [0, 255] | PASS_WITH_NOTE | 단일 PNG |
| UI/Status_Overheat.png | 128×128 RGBA | [0, 255] | PASS_WITH_NOTE | 단일 PNG |
| Environment/ObsidianScar_AshPatch_01.png | 512×512 RGBA | [0, 205] | PASS_WITH_NOTE | 단일 PNG |
| Environment/ObsidianScar_Cliff_Blocker_01.png | 512×512 RGBA | [0, 255] | PASS_WITH_NOTE | 단일 PNG |
| Environment/ObsidianScar_Fissure_Edge.png | 512×512 RGBA | [0, 255] | PASS_WITH_NOTE | 단일 PNG |
| Environment/ObsidianScar_Fissure_Glow.png | 512×512 RGBA | [0, 140] | PASS_WITH_NOTE | 단일 PNG |
| Environment/ObsidianScar_Ground_Base.png | 512×512 RGBA | [255, 255] | PASS_WITH_NOTE | 단일 PNG |
| Environment/ObsidianScar_Ground_Cracked_01.png | 512×512 RGBA | [255, 255] | PASS_WITH_NOTE | 단일 PNG |
| Environment/ObsidianScar_Ground_Cracked_02.png | 512×512 RGBA | [255, 255] | PASS_WITH_NOTE | 단일 PNG |
| Environment/ObsidianScar_HeatVent_01.png | 512×512 RGBA | [0, 255] | PASS_WITH_NOTE | 단일 PNG |
| Environment/ObsidianScar_Obsidian_Slab_01.png | 512×512 RGBA | [0, 255] | PASS_WITH_NOTE | 단일 PNG |
| Environment/ObsidianScar_Obsidian_Spire_01.png | 512×512 RGBA | [0, 255] | PASS_WITH_NOTE | 단일 PNG |
| VFX/Overheat_Apply.png | 512×512 RGBA | [0, 210] | PASS_WITH_NOTE | 단일 PNG |
| VFX/Overheat_Burst.png | 512×512 RGBA | [0, 245] | PASS_WITH_NOTE | 단일 PNG |
| VFX/Overheat_High.png | 512×512 RGBA | [0, 235] | PASS_WITH_NOTE | 단일 PNG |
| Monsters/Obsidian_Beetle_Sprite_Sheet.png | 1256×1256 RGBA | [0, 255] | PASS_WITH_NOTE | 16 / 빈 Frame 0 |
| Monsters/Scorching_Watcher_Sprite_Sheet.png | 1256×1256 RGBA | [0, 255] | PASS_WITH_NOTE | 16 / 빈 Frame 0 |

Manifest 17개 파일명·크기·Alpha 범위와 실제 PNG가 일치한다. 파일 해시 중복 0, 두 Monster의 빈 셀 0. Alpha 경계·투명 픽셀 수·Frame별 Bounds/셀 끝 접촉은 [JSON](Main18_Art_Source_Audit.json)에 기록했다. Alpha 경계 접촉만으로 잘림이라고 단정하지 않는다.

Unity 실조회 17/17: Sprite, Point, Uncompressed, maxTextureSize2048, NPOT None, Mipmap off, Alpha Is Transparency. Monster는 1256²·314²·16 Frame·314PPU·발밑 Pivot(157,0). 이름과 Slice ID를 보존한다. Environment/UI/VFX128PPU, Ground는 원본 반복 배치, Props는 월드 크기를 별도 적용한다. [실제 Importer 설정](Main18_Importer_Settings.txt).

행은 Idle00–03 / Attack04–07 / Hit·Strong08–11 / Defeat12–15. KO 마지막 Frame 유지와 기존 왼쪽 반전 정책을 재사용한다. 새 Animator Controller 0, 임의 Walk/프레임 생성 0. PNG 복사본은 원본 SHA256와 17/17 일치, 재색칠·Crop·새 이미지 생성 0.

환경 Props, Cooling Remedy 및 반복 지면에서 **원본의 흰 가이드 형태 테두리·점선과 타일 경계가 보인다**. 이를 숨기려고 원본을 수정하지 않았다. UI 축소 가독성, 몬스터 실루엣·방향·공격/피격/KO 움직임, 투명 가장자리와 VFX 타이밍의 최종 미술 승인은 사용자 검토가 필요하다. 이 미술 미완료는 게임 진행 Blocker와 구분한다.

Apply/High/Burst는 단일 PNG다. 납품에 peak/release Frame 정보가 없어 .28초 단일 Pulse를 사용한다. Stack2 High, Stack3 피해 적용 시 Burst, Battle Runtime VFX이며 Save에 기록하지 않는다.

[원본 Contact Sheet](QA_증거/Main18/Art_Source_Contact.png) · [납품 Manifest](QA_증거/Main18/Art_Manifest.txt) · [Field09 1920](QA_증거/Main18/Field09_1920.png) · [과열2 HUD1280](QA_증거/Main18/Battle_Heat2_1280.png). 1280/1600/1920은 카메라 RenderTexture를 통한 백그라운드 캡처이며 실제 OS 해상도·창 포커스를 바꾸지 않았다.
