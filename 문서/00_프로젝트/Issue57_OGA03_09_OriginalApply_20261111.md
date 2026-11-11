> EOD 선별 보관: 이 문서는 해당 작성 시점 기록입니다. 최신 승인/본편 상태는 EOD_20261111_Integration_PushAudit.md 및 각 담당 LATEST의 EOD 정정이 우선합니다. PNG·QA 원시파일·검사기는 이 문서 복사에 포함하지 않습니다. 현재 원본 제품은469b219f이며 EOD Push는 안전 감사 HOLD입니다.

# Issue57 OGA03·OGA09 원본 로컬 적용 — 2026-11-11

**OGA03_OGA09_LOCAL_ORIGINAL_APPLIED / UNITY_IMPORT_DELTA_QA_PASS / USER_REALPLAY_PENDING / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**

바울 대표의 이번 원본 적용 지시를 승인 범위로 실행했습니다. [기존46PASS 보고](https://github.com/HappyPaul812/Project-Limitless/issues/57#issuecomment-6789060267) 및 격리 CODEX_LATEST를 확인했고 기존46PASS/0FAIL을 재사용했습니다. 전수 반복0·재그림0·기능변경0.

## 브랜치·안전 동기화

- 검증 브랜치 `codex/ch1-oga03-09-unity-delta-20261111`, 제품 **548cf1e275684d8c4a647685fb6f08be0c495be7**.
- 별도 적용 브랜치 **codex/ch1-oga03-09-original-apply-20261111**, worktree `F:/study/codex/Project-Limitless/Temp/Npc57Apply20261111/Worktree`.
- 실제 원본 시작main **21ecf2129efa9d9467161d972fcb0a676122223c**. 제품 커밋이 직계 후손이며 diff는 승인 제품5파일뿐입니다. 충돌/신규경로충돌/참조충돌0 확인 뒤 `merge --ff-only --no-autostash` 및 merge.autoStash=false로 **제품548cf1e2까지 원본 FF 완료**.
- 최종 원본 main은 **548cf1e275684d8c4a647685fb6f08be0c495be7**입니다. 이 보고·CODEX_LATEST/CURRENT_STATUS·감사근거는 별도 적용 브랜치의 로컬 Docs 커밋으로만 보관합니다. 자동 승인 검토가 문서·증거·검사기11파일의 추가FF를 “5파일만 적용 범위 초과”로 거절하여 실행하지 않았습니다. 원본 main의 기존 CODEX_LATEST/CURRENT_STATUS는 수정0이며 갱신본은 적용 브랜치에 있습니다. main직접commit/원격Push/Merge0.

## 정확한 제품5파일

기준 `Unity/Client/Assets/_Project/`:

1. 신규 `Resources/VillageNpcSprites/Eldiran/OGA03_GeneralShop_Original_Approved.png`
2. 신규 위 PNG `.meta`, GUID **a89ccd7e102e6d147a600c49950e6c18**
3. 신규 `Resources/VillageNpcSprites/Eldiran/OGA09_VillageRepresentative_Original_Approved.png`
4. 신규 위 PNG `.meta`, GUID **96ebc18201939fc49916c36943bf96f4**
5. 수정 `Scripts/NPC/VillageNpcAppearanceCatalog.cs`의 기존 경로5곳만 교체.

OGA03 상인 `starter-village-general-shop`, `safezone-catacomb-supply`, `arbel-shop` / OGA09 대표 `starter-village-main-guide`, `arbel-leon` 매핑. StableID/표시이름/역할/Collider/이름표/퀘스트/상점/가격 로직 변경0. 기존 구형Sprite·GUID/meta 보존, Scene/Prefab 변경0. OGA06/16·F2 B352·승인 전투배경·냉기아트 변경0.

승인 PNG 바이트SHA 유지:
- OGA03 **09b2d97643a7aaefd51930eb374c48bf358020f015cc4cce9e7ef4903853151d**, 61882bytes,512RGBA, PPU**351.75**, Pivot**(252,40)px**.
- OGA09 **ca0336ab6bbc7aaeb16169037e73ee39ced1b674f48324baeb75b1debfa29fcf**, 59877bytes,512RGBA, PPU**347.375**, Pivot**(270.5,40)px**.
- Point/FullRect/MipmapOFF/Uncompressed 유지. 기존 alpha월드높이32/28·발바닥0을 유지한 검증meta 그대로. 재생성/resize/재인코딩0. 실제5파일은 승인커밋과 일치(PNG실바이트, 텍스트EOL 정규화만 허용).

## 신규 최소 QA — 격리6PASS + 원본8PASS / 0FAIL

Unity **6000.6.5f1**. 기존 동일제품 QA복제본에 apply worktree5파일 일치를 먼저 확인한 뒤 Npc57ApplyPreflight.Run을 숨김 Batch/EditMode로 실행했습니다. PPU/Pivot/512규격2, 상인3ID·대표2ID 링크2, GUID2일치1, Play미진입/컴파일완료1 = **6PASS**, Batch exit0. 기존46 Runtime/거래/대화/화면 재실행0.

원본FF 뒤 원본 Editor에서 해당 PNG2와 매핑C#만 표적 Import. 컴파일/DomainReload 완료 후 원본프로젝트/버전1, PNG/PPU/Pivot2, 상인3·대표2 리소스링크2, 승인GUID1, OGA06/16 매핑보존1, 기존 Scene/선택/PlayOFF보존1 = **8PASS**. Console error**0**. 원본 새Play/Scene전환/Selection/OS포커스 조작0. 격리 환경의 MCP assembly reference EndOfStream 로그는 Editor환경이슈로 구분하며 게임컴파일FAIL로 집계하지 않았습니다. 원본Console오류0과 혼동하지 않습니다.

**총 신규14 assertion PASS/0FAIL**, 기존46별도재사용. 원본에서 실제 구매/퀘스트를 실행하거나 사용자Save를 생성한 것이 아닙니다. 실제 플레이기능/960·1280화면은 기존46근거로 동일 제품임을 확인했습니다.

## 보호 결과

사전 **8314파일/1,220,300,074bytes** SHA 백업/보호, 실제dirty**302**/untracked**550**. 원본반영 전 changed0/status byte-exact동일. 제품반영+Import 뒤 기존보호8314 중 허용한 매핑코드1만변경, **나머지8313 SHA동일**, 사용자dirty/untracked status byte-exact동일. 신규4파일은 승인제품 일치. 사용자Save/UserData·기존Scene/Prefab/GUID/meta/승인자산/Quest 보호. 최종검사에서도 승인 코드1 외 기존8313파일 SHA가 동일했습니다. 완료문서는 원본에 동기화하지 않아 기존 CODEX_LATEST/CURRENT_STATUS도 보존되었습니다.

원본 Editor 버전6000.6.5f1, 기존 외부worktree Bootstrap 경로·dirtyfalse·PlayOFF·선택null·비포커스 유지, 컴파일idle. 개인Save전체목록/내용·라이선싱포함 원로그는 보고커밋에 포함하지 않았습니다.

## 화면과 대표 실플레이 확인 위치

아래 두 화면은 **기존 격리46PASS GameView 재사용**, 원본에서 새Play로 촬영했다고 주장하지 않습니다. 신규캡처/미술재승인 요청0. 근접 플레이어 이름표의 기존 가림은 유지하며 이름표/캐릭터 위치를 임의 변경하지 않았습니다.

화면 근거(로컬 작업브랜치 보관, 이번 복사 제외): `../../Evidence/Npc57Apply20261111/After_starter-village-general-shop_960x720.png`
화면 근거(로컬 작업브랜치 보관, 이번 복사 제외): `../../Evidence/Npc57Apply20261111/After_starter-village-main-guide_1280x720.png`

- **시작마을(World_StarterVillage) 시장 서쪽** 잡화상인, 좌표**(-5.8,1.2)**: 갈색모자·앞치마·상품가방, E→물건을본다→기존 구매/판매 확인. Tab/마우스/Esc 동일.
- **중앙광장** 주민대표, 좌표**(0,1.5)**: 회색머리·남색긴외투·지팡이. E로 기존 마을안내, 신규진행이면 Main01 대화→남문경비 안내 확인.
- 재사용처는 Field_03 안전지대 보급상인, Arbel 잡화상인·레온. 기능과 이름표 숨김정책은 기존 그대로입니다.

추가 배포승인 대기없음. 다음은 대표의 실제 게임 수용 확인이며 Codex가 손플레이 완료로 대신 표시하지 않습니다. 이번 완료 후 다른 작업을 자동 시작하지 않습니다.
