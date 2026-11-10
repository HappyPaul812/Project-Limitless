# LIMITLESS / Chapter1 리뉴얼 / 2026-11-10 / VenomSnake v3 단일 포즈 최종 미술 후보 납품

**브랜치** `docs/art-renewal` · **관련 Issue** [#43](https://github.com/HappyPaul812/Project-Limitless/issues/43) · **현재 판정** `VENOM_SNAKE_ART_CANDIDATE_FINALIZED / USER_ART_REVIEW_PENDING / NOT_UNITY_APPLIED`.

## 1. 실제 기준 후보·버전 결정

- **v1 / DISCARDED_AS_DESIGN:** `Limitless_Monster_Renewal_Pilot_VenomSnake_v1.zip` 과거 사용자가 독사의 긴장감·고유성 부족으로 시안 반려. 역사 파일 삭제0.
- **v2 / HOLD_REFERENCE:** `Limitless_Monster_Renewal_Pilot_VenomSnake_v2.zip` 실제 32×32 Idle·Walk·Attack·Hit·Defeat ×4=20 프레임. 일부 외형 개선에도 공식 Ch2 고품질 몬스터 화풍·화소 밀도 대비 기준 미달. 사용 금지/파일 삭제가 아닌 과거 비교 참조. 이번 v2 첫 Idle 프레임은 원본 v2 ZIP의 `Frames/LL_VenomSnake_Idle_00_32x32.png`를 실제 추출해 비교판에만 표시.
- **v3 / KEEP / FINAL_SINGLE_POSE_CANDIDATE:** `VenomSnake_KeyArt_v3.png` 실제 라이브러리 원본 **1254×1254 RGBA, 2,285,408bytes, SHA256 `7001d199b279cb8e541fc603bd62351ab460d3b44da91bc91a2d65dc099f8fd0`**. [#43 Codex 2026-10-21 정적 단일 포즈 PASS 권고](https://github.com/HappyPaul812/Project-Limitless/issues/43#issuecomment-6305728893) 및 기존 납품 SHA와 이번 **실제 원본 SHA가 완전 일치**. **사용자 최종 미술 승인과는 별개**.
- v4 새 미술 원화 제작/승인 없음. 이번은 기존 v3를 **재드로잉하지 않고 최종 후보로 동결**, 512·128·40px 비교판/QA/출처/ZIP을 실제로 만드는 단계다.

## 2. 실물 파일·검수

- **ZIP** `LIMITLESS_VENOMSNAKE_SINGLEPOSE_CLOSEOUT_20261110.zip`: **3,619,505bytes**, SHA256 **`5c708234539a7751c1646508feb5fc66e8a5e11439b222494145e99923980b8b`**. **ZIP 14 entries / CRC testzip PASS / Manifest 12/12 bytes+SHA256 PASS / SHA256SUMS 13/13 PASS**. SourceDeclaration/DesignNotes/README/QA JSON 포함.
- **최종 유일 원화:** `Candidate/VenomSnake_KeyArt_v3_FINAL_CANDIDATE_SOURCE_KEEP.png`은 기존 v3 PNG의 **바이트 완전 복사**이며 SHA 동일 `7001d199b279cb8e541fc603bd62351ab460d3b44da91bc91a2d65dc099f8fd0`. Unity Game Sprite/씬 변경 없이 오프라인만.
- **512·128·40px 실축소 PNG:** `Preview/VenomSnake_v3_{512,128,40}px_OFFLINE_RGBA.png`. 원본 알파 피사체 bbox를 잘라 **동일 종횡비·여백 유지 축소**하고 512 또는 128/40 투명 캔버스 중앙에 배치. 소스 원화 바이트/색상 원본 파일의 변경 없음.
- **실제 전후 비교:** `Review/VenomSnake_v2_vs_v3_OFFLINE.png`은 **v2 실32×32 Idle00**와 v3 원화의 오프라인 슬롯 비교, `Review/VenomSnake_v3_512_128_40_OFFLINE.png`와 `Review/VenomSnake_v3_Dark_Light_Forest_128_40_OFFLINE.png`는 크기별/단색 배경 가독성 보드. **정식 Unity GameView·PPU 동일 월드스케일·기존 게임 Snake 실재생 비교가 아님**.
- **납품 경로:** ChatGPT 라이브러리 `/LIMITLESS_Art_Delivery/VenomSnake_Issue43_20261110/`에 ZIP·원본 PNG·두 비교판·명암판·40px PNG **6개 업로드 후 재목록 PASS**. 이 대화 ZIP 실제 파일도 `/mnt/data/LIMITLESS_VENOMSNAKE_SINGLEPOSE_CLOSEOUT_20261110.zip`. 사용자 Windows `F:\Downloads` 자동복사 및 GitHub PNG/ZIP 바이너리 Push는 없음.

## 3. 원화/명암 QA와 미술 승인 게이트

- **실루엣:** S자 목, 오른쪽 머리 방향, 둥근 몸통 코일, 독니·녹황색 독성 무늬 구별. 작은 40px에서는 세부 비늘이 뭉치므로 몸통/머리 실루엣 위주로 판단해야 함.
- **과도한 공포감:** 기존 v3의 크게 벌린 입·송곳니는 일반 판타지 몬스터 수준의 적대감을 나타내지만 일부 사용자에게 과하게 느껴질 여지가 있음. **이번에는 v3 실물 PASS 권고·출처 보존을 우선하여 얼굴을 바꾸지 않음**. 바울 대표 최종 시각판정에서 부담이 확인되면 **입/눈 부위만 별도 국소수정** 가능. 아무 수정 없이 '공포감 완화 작업 PASS'라고 주장하지 않음.
- **기술 잔여:** 원본 v3 alpha nonzero bbox `[0,23,1236,1224]`; 왼쪽 x0의 알파>0 **25px** 유지. 이번 QA의 8연결 컴포넌트 alpha>0 **184개**, 면적 1~4px 176개. 역사 검수의 **소형 알파 군집548건**은 별도 threshold/탐색 방식의 기존 기록이므로 숫자를 임의 수정하지 않음. 원본을 alpha-only로 편집하거나 완전 제거했다고 주장하지 않는다.
- `PPU/Pivot/Scale/Runtime/sheet/frame/Hit/독/Spawn`은 이번 실물 미술 검토 범위 밖, `NOT_VERIFIED`. **미술 사용자 최종 승인 전 Idle/Attack 프레임 양산/본게임 교체 금지**.
- **조건부 다음:** **NPC 치유사 OGA06·은행 안내인 OGA16은 이번 회차 신규 제작 0**, 다음 턴 착수 가능하되 원본 Unity·NPC ID·상호작용 기능 보호, 최소 투자 원칙 지속.

## 4. 보호/마감

기존 VenomBee·ForestSpider·Wolf/Bear/Fox·Ch2 공식몬스터/보스·환경·Field11·NPC 기능, Unity 6000.6.5f1 원본/Scene/Prefab/meta/GUID/Quest/MonsterDefinition/Save/UserData, `main` **변경 0**. `docs/art-renewal` 문서 보고 이외 게임 파일 커밋 0 / main 직접 Push·병합 0.

**이번 작업 종료 상태:** `VENOM_SNAKE_ART_CANDIDATE_FINALIZED / USER_ART_REVIEW_PENDING / NOT_UNITY_APPLIED`.
