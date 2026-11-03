# Main24 F2 후속 아트 통합 델타 검수 — Owl Attack4 / Bee TargetReview / Spider Idle8·Walk8

**2026-11-03 / LOCAL_ART_REVIEW_DONE / USER_VISUAL_REVIEW_PENDING / NOT_UNITY_APPLIED / REMOTE_NOT_PUSHED**

바울 대표 제공 세 ZIP을 각각 직접 열어 읽기 전용 검수했습니다. 파일 무결성은 모두 통과했으나 **최종 모션/제품 적용 승인으로 확대하지 않습니다**. Owl은 공격 대비가 실제 개선됐지만 디자인 연속성·추가 냉기 궤적 보완, Bee는 실제 새 포즈가 없어 표적 계약 HOLD, Spider는 원화 보존·새 모션 확인 후 Walk 접지/관절과 GIF 시간 표기 보완 판정입니다.

## 최신성·범위

GitHub 공통규칙·main CODEX_LATEST(과거 스냅샷)·docs/art-new SPRITE_ART_LATEST와 #74 신규 보고 38a53da7/be260c8c, docs/art-renewal WORLD_ART_RENEWAL_LATEST와 41e3672b/73399593, #74/#60/#63/#82/#83 최신 본문/댓글을 읽었습니다. 실제 로컬 Codex 최신은 F2 왕복 인계 **24232ccdacba93aab0961e263bb055a132a13bd0**, 이를 출발점으로 사용했습니다. 원격 문서의 F2 탐험 '81PASS' 표현은 이전 #80 수치이며 #83의 새 왕복90/23을 대체하지 않습니다.

- branch: `codex/art-owl-bee-spider-review-20261103`
- worktree: `F:/study/codex/Project-Limitless/Temp/ArtOwlBeeSpiderReview20261103/Worktree`
- 검사/추출/비교판: `F:/study/codex/Project-Limitless/Temp/ArtOwlBeeSpiderReview20261103/`
- 아트 원본 덮어쓰기·재생성0, Unity 실행/Scene/Prefab/BuildSettings/Packages 변경0, 개인 Save/UserData 열람·복사·해시·수정0. Main24 #80의81/14, #83의90/23, 저장85/72/72, Main23, 그루터기21 **재실행0**.
- 검수 문서/JSON 기록만 로컬 커밋, HTML·PNG·GIF·ZIP 실물은 Temp 보존. GitHub 바이너리 업로드/Push/main commit/merge·Discord66 작업0.

## 1. 세 ZIP 공통 기술 결과

| 실물 파일 (F:/Downloads/) | bytes / entries=files | ZIP SHA256 | CRC·경로·중복 | Manifest / SUMS |
|---|---:|---|---|---|
| `Limitless_FrostOwl_Attack4_Review_20261103.zip` | 3,153,329 / 18 | `5ba6ecffad2e3ccf5a07c0fda00041073f1db6938a439fdeea170eae9abf250b` | CRC PASS / unsafe0 / 중복0 | 새 Attack4 4/4 + 이전 Idle8 참조8/8, SUMS17/17 PASS |
| `LIMITLESS_VenomBee_TargetReview.zip` | 709,946 / 9 | `450b78e4c68b812afff4f9b30d6ff0d54a5d74cb898feb5770cc308a0e9d7bd0` | CRC PASS / unsafe0 / 중복0 | 동봉 payload 7/7, SUMS7/7 PASS |
| `LIMITLESS_Spider_Idle8_Walk8.zip` | 7,953,765 / 33 | `d4c7c73a7c830f2e7884a664ec45759b673434ed5d70fcc49fc52458ea4524db` | CRC PASS / unsafe0 / 중복0 | 동봉 payload 31/31, SUMS31/31 PASS |

실제 비압축 용량은 Audit.json에 ZIP별 기록했습니다. 절대경로·상위이동·드라이브/콜론·symlink·대소문자 정규화 중복을 검사한 뒤 전용 디렉터리에만 추출했습니다. 세 ZIP 모두 Manifest와 SUMS가 **동봉**되었습니다. Owl Manifest는 전체 파일 목록이 아니라 신규 Attack4와 기존 보호 SHA 참조이며, 나머지 파일은 SUMS17개로 확인했습니다. Bee/Spider는 독립 Manifest로 이전 합본 참조 누락 문제가 없습니다.

총 PNG41개 중 **새 SHA34개만 decode/크기·mode·alpha 검사**, 기존 동일 SHA7개(Bee5 + Spider 원화/Idle00)는 이전 QA 재사용. GIF9개 decode/모든 프레임·실시간·순서 확인. 원본 스프라이트 PNG는 RGBA/투명, 보드4개는 RGB 비교 자료이며 GIF는 배경이 합성된 팔레트 프리뷰입니다. RGB 보드/불투명 GIF를 투명 게임 Sprite로 판정하지 않았습니다. 새 코어 프레임 가장자리 알파 접촉0, 불투명 사각 매트·잘림의 새 결함은 관찰되지 않았습니다. 시간 규격 문제는 아래 Spider 항목으로 분리합니다.

## 2. Frost Owl Attack4 — 실제 새 포즈, 최종 REVISE

**REVISE / ANIM_REVISE_CONTINUE / DESIGN_CONTINUITY_HOLD / VFX_SCOPE_HOLD**

4개 모두 이전 Attack와 다른 SHA/실제 픽셀입니다. 준비00은 날개를 안쪽으로 모으고, 발동01은 크게 펼치며, 후속02는 날개를 내리고, 복귀03은 중간 높이로 돌아옵니다. 이전의 비슷한 날갯짓4장보다 준비/발동 대비가 커졌고 **40px에서도 접힘→펼침→하강이 구분**됩니다. 단순 이동 파일 재납품이 아닙니다.

| 신규 파일 (Candidate/Attack/) | SHA256 | 이전 동일 인덱스 대비 RGBA 변화 |
|---|---|---:|
| `FrostOwl_v3_Attack_00_512_RGBA_CANDIDATE.png` | `f6274c6c1bb343d9650d8b5537eabc84690c07bb90cf3af6b1d61846e2a592c6` | 123,216px |
| `FrostOwl_v3_Attack_01_512_RGBA_CANDIDATE.png` | `1915cfd62c4592711d8ffc29786d463d5709df5fda787401c96f0e1d48ca7cd5` | 133,071px |
| `FrostOwl_v3_Attack_02_512_RGBA_CANDIDATE.png` | `79e238611a387ade3f245d7eacacfadd16eaba63abd9fb789d9f5ac1be0ad2b7` | 133,862px |
| `FrostOwl_v3_Attack_03_512_RGBA_CANDIDATE.png` | `187829c7237cda0bc85802069d3dd5d5bf1ad98266044143ae1756cace96005a` | 118,115px |

- 4 PNG 각각512×512 RGBA, 2048×512 시트4셀은 개별 PNG와 RGBA 완전 일치. alpha>0 bbox는 00[68,112,439,413], 01[20,42,492,468], 02[26,122,484,446], 03[50,104,465,443] (exclusive 끝점). 4면 접촉0; 과거 clip-safe 두 프레임을 다시 고치지 않았습니다.
- 원본 GIF4프레임 **120/130/120/130ms**, 주기500ms/평균8FPS, 무한루프. PNG00→01→02→03 실제 순서와 일치(불투명영역 RGB 비교). 4포즈여서 급격한 펼침/하강과 03→00/기존 Idle의 연결감은 남아 있습니다.
- **승인 정지원화/Idle00 `81a31cf37cb834799ab5c9428da612ab13a5a496025805685daefed7b5d3c467`와 이전 Idle8 SHA8개 전부 보존**. 이번 ZIP에는 Idle8 실물이 아니라 보호 SHA만 동봉. 이전 로컬 Idle8과 대조했으며 동일 자산 전수 품질 QA는 반복하지 않았습니다.
- 최종 보류 이유: 새 얼굴·눈썹/머리깃, 촘촘하고 부드러운 깃털·몸통/꼬리 형태가 승인 v3의 기존 Idle과 달라 한 캐릭터의 연속 모션으로 자연스럽게 이어진다고 확정하기 어렵습니다. 특히 발동01의 밝은 냉기 궤적/입자는 원본에 비해 큰 추가 표현이며 **이번 VFX 비범위와 구별해 별도 미술 결정이 필요**합니다. 독립 VFX/Skill을 구현한 것은 아니나 이미지에 합쳐진 효과를 몰래 승인하지 않습니다.
- 다음 최소 작업: 승인 얼굴·몸통 기준과 프레임 anchor를 유지하며 현재 날개 대비만 살리고, 새 냉기 궤적 범위를 대표가 판단하도록 분리합니다. Idle8/정지원화·상단수정 재제작/Hit·Defeat·SFX·게임 적용0.

## 3. VenomBee TargetReview — 새 포즈0, 표적 해결 아님

**HOLD / TARGET_CONTRACT_HOLD / MOTION_REVISE_CONTINUE / FULL_COMBAT_READY_NO / RIGHTS_HOLD**

실물 구성은 Source_KEEP Attack3 + 정상Idle2 + TEST_ONLY 스토리보드1 + README/독립Manifest/SUMS입니다. **이번 ZIP에는 GIF·시트·새 Sting/Recover가 없습니다.** 새 PNG는 스토리보드1장뿐이며 실제 스프라이트5개는 #82와 SHA/바이트 완전 동일합니다.

| Source_KEEP / 비교 | SHA256 | 상태 |
|---|---|---|
| `Attack_Prepare_REPAINT_v2_315x253.png` | `e337921236b7b947f92bbb5d88824bc6729d6668a050a85fc2f519ff4df26212` | KEEP / 이전 동일 |
| `Attack_Recover_REPAINT_v2_315x253.png` | `2afc69e803334b1ededdadb39e786f297d8fe3d9918e72997943c9f1593c740e` | KEEP / 이전 동일 |
| `Attack_Sting_REPAINT_v2_315x253.png` | `391ed94e50ffc4ddcf3bc4442e9913e0b947cc9d5d9a57aaf865f46556e021ce` | KEEP / 이전 동일 |
| `KEEP_Idle_Folded_238x215.png` | `d17114176c13c2911cd533a0e7a363a1f4ae2ca110b540bcec5d63a2aa941605` | KEEP / 이전 동일 |
| `KEEP_Idle_Spread_238x215.png` | `d35f4583c0fa119396a40a6637f48b36b677fe00327640c655d86bde3dd47820` | KEEP / 이전 동일 |
| `VenomBee_Attack_Target_Storyboard_TEST_ONLY.png` | `d4ed4ba32de6b8656925f7b95afeb17ef7fcef7f7d1a2b72984a74b2d8723dc4` | 신규 비교자료 / 아트 포즈 아님 |

315×253 RGBA Attack3의 복부 뒤침 구조는 보존됐습니다. Sting에서 침끝이 우하향으로 보이지만, 스토리보드의 오른쪽 가상표적에 도달하는 몸 이동·접촉점/각도·프레임 타이밍은 제시되지 않았습니다. 실제 그림에서도 침끝은 표적보다 왼쪽·아래이며, 보드 자체가 contact angle NOT_VERIFIED라고 표시합니다. **TEST_ONLY 방향 가설은 실제 Runtime 표적 계약이 아닙니다.**

Prepare→Sting→Recover의 몸통/복부·날개/다리 변화와 회수 급전환은 이전과 똑같습니다. 40px에서 오른쪽을 향한 머리와 낮게 향한 후방 침이 타격 대상으로 어떻게 연결되는지 여전히 모호합니다. 새 개선이라고 보고할 근거가 없습니다. 비교판의 이번3장10FPS 재생은 **검수용 PNG 순서 재생**이지 새 GIF 납품 주장이 아닙니다.

기존 Idle10+Attack10 총20은 이번 ZIP 미동봉·수정0, 정상Idle2 SHA KEEP. PilotBee 웹 CC BY3.0/동봉4.0 출처 충돌은 **RIGHTS_HOLD 유지**. 다음은 실제 공격자/표적 슬롯·몸 이동·접촉점/타이밍 계약을 읽기 확인한 후 필요한 Sting/Recover만 보완하는 것입니다. 표적 미확정 상태의 추가 재작화/전체20양산·게임 교체0.

## 4. Organic Spider v5 Idle8 / Walk8 — 파일럿 실변경, 보행 REVISE

**REVISE / MOTION_REVISE_CONTINUE / IDLE_BREATHING_CANDIDATE / WALK_CONTACT_HOLD / FPS_LABEL_REVISE**

승인 정지원화 `ForestSpider_v5_APPROVED_BYTE_KEEP.png`와 `ForestSpider_Idle_00.png`는 **`c600cd2264f6a0323771288666d16b49aa213ad938036d0746ed6f7f1d50adb9` 그대로**입니다. 승인 원화 재심사/덮어쓰기0. 신규 Idle01~07 + Walk00~07 **실제 새15개**가 있으며, 16개 모두512×512 RGBA입니다. 개별 전체 SHA는 Audit.json과 HTML 파일표에 기록했습니다.

- 원화 대비 RGBA 변화: Idle01~07 **86,831~87,255px**, Walk00~07 **96,732~98,592px**. 제작자의 국소 비강체 변형 설명과 실제 다리/무릎/몸 움직임이 대응합니다. 새 파일 있다는 사실을 새 관절 보행 완성으로 해석하지 않습니다.
- **Idle:** 몸통 호흡·미세 다리 흔들림 후보로는 읽힙니다. 40px에서는 변화가 매우 작고 유기형 실루엣은 유지됩니다. Idle 한정 KEEP_CANDIDATE/대표 미술 검토 대상으로 남깁니다. 원화 디자인은 기존 승인 그대로입니다.
- **Walk:** Idle보다 발끝·무릎 이동이 커졌으나 여러 다리가 늘어나거나 흔들리는 변형이 주로 보입니다. 앞다리 확대/128/40 비교에서 **각 다리의 디딤→들기→착지 교대와 관절 회전이 명확하지 않고**, 40px에서 접지 보행보다 좌우/상하 흔들림에 가깝게 읽힙니다. 실제 지면·보행 속도 계약이 없으므로 바닥 고정/미끄러짐 해결 PASS는 줄 수 없습니다. 특정 다리 재작화와 지지/스윙 구간을 먼저 읽히게 해야 합니다.
- 128/40 silhouette 유지·새 캔버스 잘림/사각 매트는 관찰되지 않았습니다. 가려진 다리를 포함해 '8다리 독립 관절 모두 정상'이라고 주장하지 않습니다. 실제 World/Battle PPU256·scale.7·10FPS/접지는 Unity 미실행·NOT_VERIFIED.
- 2048×1024 4×2 시트2장: Walk8/8 및 Idle01~07은 개별 PNG와 RGBA 완전 동일. **Idle00 셀만 완전 투명 alpha0의 RGB24,661px 차이**, 알파 변화0·가시픽셀 변화0이므로 **가시 렌더 동일 / 무해한 저장 표현 차이**, 모션 결함으로 잘못 분류하지 않았습니다. 승인 PNG 자체는 byte exact입니다.
- GIF8개 모두00→07 순서/8프레임/loop0 확인. **10FPS4개는100ms×8=800ms/10FPS 정확. ‘8FPS’4개는120ms×8=960ms/8.333FPS**라 명세125ms와 다릅니다. GIF10ms 단위에서 정확한 평균8FPS가 필요하면120/130ms 교대가 최소 수정입니다. 파일을 몰래 수정하지 않았습니다. 8FPS는 애초 임시 검토값이며 기존 게임10FPS를 바꾸지 않습니다.
- Attack8/Shoot8 제작·검수0, Chapter4 메카닉 Spider v3 ARCHIVE_ONLY 보호, 전체32 양산/본편 교체0.

## 5. 단일 OFFLINE 비교판

**`F:/study/codex/Project-Limitless/Temp/ArtOwlBeeSpiderReview20261103/Review/index.html`**

HTML SHA256 **`d08aea841659a8840d04aa9ddcf03605e9b392971e999212b4748eea560b9e20`**. 이미지/GIF·배경 모두 data URI 내장, 네트워크 의존0.

- Owl: 보존Idle8 / 이전Attack4 / 이번Attack4, 각128·40 실제 크기와4배 참고, 납품 원본GIF, 전후 보드.
- Bee: 이전#82 / 이번Source_KEEP 동일성, TEST_ONLY 표적 보드, 128/40 순서 재생·동일SHA 표기.
- Spider: 승인원화 / Idle8 / Walk8, 실제128·40/다리부위 비교, 납품8.33·10FPS 원본GIF8개.
- 각 파일명·전체 SHA256·KEEP/REVISE/HOLD·OFFLINE / Unity GameView 아님 명시. PNG 프레임 앞뒤/슬라이더/재생·정지·밝음/어둠/실제 Field03 지면PNG 참고 배경 지원. 배경은 OFFLINE 참고이지 Scene 재현 아님.
- 별도 프로필 headless 브라우저에서 이미지 decode·8시퀀스 프레임수·앞뒤 제어·실제 시간 진행·128/40폭·배경·가로폭 **37 PASS / 0 FAIL**. 개인 브라우저/OS 포커스 변경0. GIF9개의 실제 순서는 PNG 불투명부 RGB 최소오차 대조로 독립 확인했습니다. 이 검수 도구의 최초 경로 분기 오류는 수정 후9/9 일치로 재계산했으며 잘못된 중간 결과는 최종 근거로 사용하지 않습니다.

[ZIP/파일별 상세 Audit](Evidence/ArtOwlBeeSpiderReview20261103/Audit.json), [델타/시트/40px 지표](Evidence/ArtOwlBeeSpiderReview20261103/DeltaAnalysis.json), [GIF 순서](Evidence/ArtOwlBeeSpiderReview20261103/GIFOrder.json), [브라우저37체크](Evidence/ArtOwlBeeSpiderReview20261103/BrowserQA.json), [판정요약](Evidence/ArtOwlBeeSpiderReview20261103/ReviewSummary.json), [원본보호](Evidence/ArtOwlBeeSpiderReview20261103/ProtectionAfter.json).

## 6. 최종 미술 승인·다음 최소 작업

1. **Owl:** 바울 대표가 새 날개 동작의 대비는 수용할지, 승인 v3와 다른 얼굴/깃털·체형 및 냉기 궤적은 어떻게 정리할지 판단해야 합니다. 현 상태 최종 Attack 승인 아님. 기존 Idle/정지·상단수정은 KEEP.
2. **Spider:** 승인 정지 원화는 계속 KEEP. Idle 호흡은 별도 미술 검토 가능하나 Walk 지지발/스윙·관절·40px 보행 가독성을 보완한 후 Idle8+Walk8 전체 승인 여부를 판단해야 합니다. GIF8FPS 표기는 바로잡아야 합니다. Attack/Shoot 확장 보류.
3. **Bee:** 포즈/회수 개선0이므로 표적방향 해결 승인을 요청할 단계가 아닙니다. 실제 표적·접촉 계약과 권리 충돌 정리 전 HOLD 유지.

원본main **ab47b931fec92976170f9f50aabde1822d78bb90**, 기본status344 / 상세-uall858 전후 동일. 입력 ZIP3와 사용한 이전/승인 참조SHA 보존. 기존 Main24 #83 worktree/커밋·90/23 결과 및 #82·#80·저장·Main23 결과 재사용, 재실행0. **LOCAL_ART_REVIEW_DONE / USER_VISUAL_REVIEW_PENDING / NOT_UNITY_APPLIED.**

