# LIMITLESS / Chapter1·2 리뉴얼 아트 / Issue #95 — OGA06 치유사·OGA16 은행 안내인 신규 정지 NPC 2종 실제 납품

**일자:** 2026-11-10 · **브랜치:** `docs/art-renewal`  
**상태:** `OGA06_OGA16_2_ART_PILOTS_DELIVERED / PNG_TECH_PASS / USER_ART_REVIEW_PENDING / LIVE_ORIGINAL_PNG_VISUAL_COMPARISON_PENDING / RUNTIME_SPEC_PENDING / NOT_UNITY_APPLIED`

## 1. 이전 작업 승인 게이트

바울 대표가 직접 **“맹독뱀 VenomSnake v3 원본을 최종 미술 승인합니다. Issue #43에 승인 기록을 남기고, 다음 작업인 Issue #95 치유사 OGA06·은행 안내인 OGA16 오리지널 NPC 2종 제작을 진행하세요.”**라고 결정했습니다.

- [Issue #43 정식 승인 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/43#issuecomment-6758854877).
- 승인 원본 `VenomSnake_KeyArt_v3.png`, 1254×1254 RGBA, SHA256 `7001d199b279cb8e541fc603bd62351ab460d3b44da91bc91a2d65dc099f8fd0` 그대로 보호.
- `VENOM_SNAKE_V3_USER_ART_APPROVED_AS_IS / NOT_UNITY_APPLIED`. 단일 포즈 아트 최종 승인이지 Idle/Attack 전량 애니메이션·본편 적용 승인이 아님.

## 2. 새 오리지널 NPC 2종: 실제 파일

| 역할 / stable ID | 신규 PNG | 규격 / SHA256 | 디자인 |
|---|---|---|---|
| 치유사 / `starter-village-healer` | `Candidates/OGA06_Healer_Original_512_RGBA.png` | 512×512 RGBA / `67aef89677e9b9f174f30b2c2a9e854eaee4e61814774402bd94a18691bd401f` | 온화한 중년 남성 치료사, 약초색 외투·크림색 치료 모자·치료 표식·약 가방. 미엘과 다른 성별·머리·복장 |
| 은행 안내인 / `starter-village-bank` | `Candidates/OGA16_Bank_Original_512_RGBA.png` | 512×512 RGBA / `1199c57d3c279b51af83452c8b580814d6456417ea8e3f3b83ea9fdc6c5f8dcd` | 단정한 머리 묶음·안경·남색 정장·손에 든 장부로 수납/보관 업무 표현 |

- **이번에 신규 제작된 정지 캐릭터 PNG는 정확히 2장.** 둘 다 독립 도형·경로에서 신규 SVG를 직접 제작하여 CairoSVG로 렌더링한 오리지널 일러스트; 이전 외부 OGA PNG 리컬러·트레이싱·합성 아님. `Candidates/`에 **원본 SVG2**도 소스로 포함.
- 원본 PNG 가시 알파 경계: **OGA06 0픽셀, OGA16 0픽셀**. 512 마스터 원본 바이트와 128·40 투명 축소PNG 각각 SHA/Manifest로 검증. 40px에서는 역할별 겉옷 실루엣/장부/약 가방이 주된 식별 요소이며 미세 얼굴 특징 소실은 자연스러운 축소 한계.
- 화풍 비교 `Review/Eldiran_OGA03_OGA09_OGA06_OGA16_Style_512_OFFLINE.png`은 과거 **2026-10-23 제작된 미승인** OGA03 상인/OGA09 주민대표 **실제 개별 PNG와 신규 두 장을 동일 512px 캔버스**로 배치. 기존 OGA03·OGA09 PNG 원본 바이트 변경0. OGA20 경비·미엘·태온/오르나와의 실제 공식 게임 화소 비교는 제공되지 않아 완료 주장하지 않음.
- `Review/OGA06_OGA16_128_40_SameCanvas_OFFLINE.png`, `Review/OGA06_OGA16_40px_LightDarkVillage_OFFLINE.png`: 밝음/어두움/숲색 바닥과 128·40 축소 가독성 참고. **Unity GameView 아님**.

## 3. 원래 게임 계약·미확인 항목

GitHub main `Unity/Client/Assets/_Project/Scripts/NPC/VillageNpcAppearanceCatalog.cs` 및 PNG `.meta` 텍스트 실제 확인:
- `starter-village-healer` → `VillageNpcSprites/Eldiran/OGA06_Healer`, `safezone-catacomb-healer`, `arbel-healer` 동적 재사용. 기존 OGA06 .meta GUID `a1060000000000000000000000000003`, **PPU28, Pivot(0.5,0), Point Filter**, .meta blob SHA `5e59a7eef3233900ca329a2b112a5354cb4543db`.
- `starter-village-bank` → `VillageNpcSprites/Eldiran/OGA16_Bank`, `arbel-pet-management` 동적 재사용. 기존 OGA16 .meta GUID `a1160000000000000000000000000006`, **PPU28, Pivot(0.5,0), Point Filter**, blob SHA `0d49b6bd9cc94b19132fe047c89fdd9a625c2906`.
- 현재 게임 OGA06/16 **바이너리 PNG는 GitHub fetch_file이 UnicodeDecodeError로 반환**했으므로 실제 기존 게임 원본의 정확한 이미지 크기/화소·동월드배율 원본 vs 새 후보 비교 **NOT_VERIFIED**. 임의 원본을 꾸며 사용하지 않음.
- 따라서 512 고해상도 PNG는 **마스터 원화일 뿐 그대로 PPU28에 넣을 Sprite 아님**. 기존 원본의 실제 픽셀 크기·접지 위치를 Codex가 읽기전용 확인하고 승인 후 새 **게임용 cell PNG(별도)**를 PPU28/Pivot(0.5,0)·Nameplate offset1.02·원래 Collider/상호작용 그대로 맞춰 격리 QA해야 함. 사용자 승인 전에 제품 Resources 교체 금지.

## 4. 실제 ZIP·SHA·납품 위치

- **ZIP** `LIMITLESS_ELDIRAN_NPC_OGA06_OGA16_ORIGINAL_PILOTS_20261110.zip`
- **569,127 bytes / ZIP SHA256 `89088854c3965299a206b9123e56c9ae072ced885731083e37a2347413e6c885` / 18 entries / ZIP CRC PASS / Manifest 16/16 bytes+SHA256 PASS.**
- ZIP 내부: 신규 PNG 2, SVG 소스 2, 128·40 투명 PNG4, 이전 OGA03/09 미승인 원형 KEEP2, 스타일 비교판1, 크기비교판1, 밝음·어두움·마을 비교판1, `SourceDeclaration.md`, `DesignNotes_KO.md`, `Evidence/QA_Results.json`, `Manifest.json`, `SHA256SUMS.txt`.
- **ChatGPT Library 실제 저장**: `/LIMITLESS_Art_Delivery/Issue95_Eldiran_NPC_20261110/`에 통합 ZIP·신규 512 PNG2·화풍 비교판·128/40 비교판·밝음/어둠 판·40px 독립 2장, **총 8개**. Windows `F:\Downloads` 자동 복사/원격 GitHub 바이너리 Push 없음.

## 5. 기존 것 보호·다음 게이트

- 승인 VenomSnake v3·Organic Spider·VenomBee·Wolf/Bear/Fox·Chapter2 공식 몬스터/보스·환경·BattleBG 등 보호. 이전 OGA03·OGA09·OGA20 후보 덮어쓰기0, 나머지 10 NPC 일괄 양산0, 초상화/애니메이션 제작0.
- Unity **6000.6.5f1**, `VillageNpcAppearanceCatalog` stable ID, 기존 OGA06/OGA16 GUID/meta/Resources, `StarterVillageHubInstaller`, E·F·패드A/마우스 상호작용, Collider, Nameplate, Scene/Prefab, Quest, Save/UserData, `main` **변경0**.
- Issue #96의 HP/MP 기존 오리지널 아이콘 발견 여부 미확정. 물약 PNG2 신규 제작0, Kenney 리컬러·기능 변경 없음.
- **다음 단계:** 바울 대표가 신규 OGA06/16 512/128/40을 미술 승인/보류 판정 → 승인 후에만 Codex 기존 원본 바이너리·PPU·월드크기 읽기전용 검증 및 최소 격리 Runtime 교체 QA(기능동결). main 직접 commit/Push/merge 금지.

**최종 상태:** `OGA06_OGA16_TWO_ORIGINAL_ART_PILOTS_DELIVERED / TECH_PASS / USER_ART_REVIEW_PENDING / IMPORT_SPEC_PENDING / NOT_UNITY_APPLIED`.
