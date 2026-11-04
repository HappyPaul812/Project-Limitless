# LIMITLESS / Chapter1·2 리뉴얼 / Issue #63 — VenomBee 신규 Prepare·IdleFolded·IdleSpread 원화 3종 납품

**기준일:** 2026-11-04 · **담당 브랜치:** `docs/art-renewal` · **상태:** `3_NEW_ORIGINAL_PNG_CANDIDATES_TECH_PASS / USER_VISUAL_REVIEW_PENDING / NOT_UNITY_APPLIED`

## 1. 근거 및 변경 범위

- GitHub [Issue #63 최신 총괄 기획 지시](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6625568292), [사용자 승인 Sting·Recover v3](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6624979503), [PilotBee 사용 중단 방침](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6625120631), [격리 v3 연결 검수](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6625234830) 참고.
- 최종 게임에서 **외부 PilotBee 및 그 파생 원화를 사용하지 않는다**는 기획 결정을 반영. 이 회차에 **새 독립 이미지 후보 3개만** 제작: Prepare (공격 준비), IdleFolded (날개 접기), IdleSpread (날개 펼치기).
- 이미 승인된 **Sting v3** `2850020146718f30ac320cd6bd0c42dcf31332ed2820a718d06bcc76949a2d67` 및 **Recover v3** `69d0944b3565fe2e4cc92034e51df2006847103a366db18d21221561fa5a19bb`은 참조 전용으로 로드하고 **바이트 변경·재인코딩 0**.
- 이전 PilotBee Prepare/Idle KEEP는 **과거 검수/권리 이력 보존용**일 뿐 신규 게임 후보에 포함하지 않음.

## 2. 실제 PNG 3개·SHA256

| 신규 독립 아트 후보 | 셀 크기 | 파일명 | SHA256 |
|---|---|---|---|
| Prepare | 315×253 RGBA | `VenomBee_Prepare_ORIGINAL_v1_315x253_RGBA.png` | `c6c3e514e1ce7535de754357bbd27a343dc7a9d396a62c4c2a0ea0c54d6998ce` |
| IdleFolded | 238×215 RGBA | `VenomBee_IdleFolded_ORIGINAL_v1_238x215_RGBA.png` | `044b1bb44b8cc368efb1e254b5309f2c994b2bed7bb5c7f9b52923a6638cc311` |
| IdleSpread | 238×215 RGBA | `VenomBee_IdleSpread_ORIGINAL_v1_238x215_RGBA.png` | `382e742c8df629d995dfbded57200b82712e7350e1b6391cc4036ce72dcc2174` |

- 각 PNG **RGBA/가시 알파 네 변 접촉0**, 315×253 Attack Prepare와 238×215 Idle 두 장은 **기존 셀 계약을 참조한 오프라인 후보**. 실제 Sprite PPU/pivot/배포 Atlas 여부는 **NOT_TESTED**.
- `Review/VenomBee_ApprovedV3_vs_New3_{512,128,40}px_Comparison.png`: 승인 Sting/Recover v3 **원본 바이트를 읽기 전용으로 비교 렌더에만 사용**; 신규 3장과 동방향/체형·날개·복부 뒤 독침·다리 확인. 원본 공격/대기 **서로 다른 크기 315×253 vs 238×215** 때문에 비교판 외형 크기가 다를 수 있으며 실제 게임 PPU 동일성 검증은 아님.
- `Review/VenomBee_Attack10_to_NewIdle2_Concept_{128,40}px_10FPS.gif`: 12슬롯×100ms(Attack10 구간 Prepare2·Sting2·Recover6 + IdleFolded·IdleSpread), **OFFLINE 개념판**. 실제 10개 독립 Attack PNG나 Unity 공격 타이밍/Hit/독/Idle 적용 검증 결과가 아님.

## 3. 납품 파일/무결성

- **ZIP** `LIMITLESS_VENOMBEE_ORIGINAL_3POSE_ISSUE63_20261104.zip`
- **676,141 bytes / SHA256 `172df8f2d658b02f6859554284ab4b719175b2ce86b922fe5270f4ae9803b325` / 13 entries / ZIP CRC PASS / Manifest 11/11 SHA+bytes PASS**.
- 실제 신규 PNG3, 512·128·40 비교3, 128·40 10FPS 개념 GIF2, SourceDeclaration JSON, QA JSON, README, Manifest, SHA256SUMS가 ZIP에 포함.
- ChatGPT 라이브러리 **`/LIMITLESS_Art_Delivery/`에 ZIP+개별 PNG3+128/40 비교2+10FPS GIF2 총 8개 업로드 성공**. 사용자 Windows `F:\Downloads` 자동 복사나 GitHub 바이너리 Push 없음.

## 4. 출처·권리·시각 품질의 정확한 판정

- **신규 이미지 출처:** 이번 대화의 이미지 생성 결과 3종(이전 PilotBee 픽셀을 직접 사용하거나 트레이스·리컬러한 결과가 아님). 원본 이미지 파일명/원본 SHA는 ZIP의 `Evidence/SourceDeclaration.json`에 각 작품별 기록. 처리 단계는 투명영역 크롭, 알파를 고려한 비율유지 축소, 역할별 기존 셀에 중앙 정렬, alpha<8 고립 잔상 최소 정리.
- 기존 OpenGameArt PilotBee 원본의 **CC BY 3.0 vs 첨부 LICENSE BY 4.0 표기 충돌**은 과거 출처 이력으로 보존. **완전한 사용 중단·배포/런타임 참조 제거는 검증하지 않았으므로**, 잔존하는 경우에 한해서 `LEGACY_RIGHTS_HOLD_IF_PRESENT`. 새 이미지 독자 생성 사실이 법률상 비침해·독립 저작권의 확정적 보증은 아님.
- 새 Prepare/IdleFolded/IdleSpread가 승인 Sting·Recover v3와 같은 개체처럼 보이도록 색감·복부 줄무늬·날개/독침 형태를 맞춘 1차 시안. **동일 6다리의 관절 정합, IdleFolded↔IdleSpread 크기/포즈 연속성, 40px 인지성은 대표 미술 검토 대기**. GIF는 비교/방향 제시일 뿐 완성 애니메이션 PASS 아님.

## 5. 보호·미승인

- 승인 Sting·Recover v3, Owl, Organic Spider v5/Idle8/Walk8/Attack3, Chapter2 공식 몬스터·보스/펫, Unity 6000.6.5f1 게임 원본/Prefab/Scene/Sprite GUID/meta/MonsterDefinition/AI/Spawn/독/Drop/Save/UserData, `main` 변경0.
- **본편 적용0 / 기존 외부 원본 삭제0 / Codex 사용량 추가 검수 실행0 / Push·병합0**.
- **최종 상태:** `3_NEW_PNG_FILE_TECH_PASS / ART_USER_REVIEW_PENDING / LEGACY_PILOTBEE_BUILD_REFERENCE_CHECK_PENDING / EXISTING_STING_RECOVER_V3_BYTE_KEEP / NOT_UNITY_APPLIED`. 신규3장 사용자 미술 승인 후에만 최소 격리 Runtime/권리 잔존 참조 검수로 이동.
