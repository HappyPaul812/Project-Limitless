# Issue57 — Chapter1 네 몬스터 출처 감사 최종 마감

2026-11-10. **ORIGINAL_CONFIRMED_4_OF_4 / USER_OWNER_CONFIRMATION_RECORDED / PROVENANCE_AUDIT_CLOSED / PRODUCT_FILES_CHANGED_0 / REMOTE_NOT_PUSHED**.

## 최신 바울 대표 결정

Codex가 실제 사용 PNG `Unity/Client/Assets/_Project/Art/Monsters/01_GrassSlime/01_GrassSlime_Walk.png`의 존재와 게임 연결을 보여 드린 뒤, 바울 대표가 **“그래 이거야. 우리가 만든거야”**라고 해당 파일의 팀 제작 출처를 직접 확인했습니다. 이어 **“zip은 애초에 없었으니까. 알아서 해줘.”**라고 정정했습니다.
이에 따라 기존 증거 ZIP 확보를 완료 조건에서 제외하고 **현재 활성 PNG에 대한 소유자의 직접 출처 확인**을 근거로 슬라임을 ORIGINAL_CONFIRMED로 갱신합니다. 미존재 ZIP을 계속 요구하지 않으며, 미실행 픽셀 검사를 PASS로 바꾸지 않습니다. 앞선 입력 부재 HOLD는 최신 대표 결정으로 해소됐습니다.

## 네 몬스터 최종 분류

| 실제 ID | 최종 분류 | 출처 확정 근거 |
|---|---|---|
| grass_slime | ORIGINAL_CONFIRMED | 현재 활성 PNG를 직접 지목한 바울 대표의 팀 제작 확인 + 기존 사용자 제공/등록/변환 문서 |
| monster_silent_echo | ORIGINAL_CONFIRMED | 대표 팀 제작 진술 + 최신 기획의 Library 원본↔활성 PNG SHA 완전 일치 기록 재사용 |
| monster_seal_guardian | ORIGINAL_CONFIRMED | 동일, 추가 이미지 검사 없이 기존 증거 재사용 |
| monster_silent_warden | ORIGINAL_CONFIRMED | 동일, 추가 이미지 검사 없이 기존 증거 재사용 |

이전 감사의 이끼갑충·그늘박쥐·묘지 망자 ORIGINAL_CONFIRMED 3종과 합하면 대상 Chapter1 잔여7종은 모두 ORIGINAL_CONFIRMED입니다. 승인 Wolf/Bear/Fox 3종과 Bee/Spider/VenomSnake·공식 Chapter2 몬스터의 기존 보호 상태도 유지합니다. 이 결론을 게임 전체 NPC/아이콘/VFX의 출처 감사 완료나 법적 독점권 보증으로 확대하지 않습니다.

## 슬라임 현재 파일과 제작 이력

- 활성 파일: `F:/study/codex/Project-Limitless/Unity/Client/Assets/_Project/Art/Monsters/01_GrassSlime/01_GrassSlime_Walk.png`.
- 활성 SHA256: **`1c422147f0a8fe7ee234e0ab771ab098ac32c44338d5efecc4f3f08a3174de34`**, 마감 시 다시 읽은 값이 기존 감사와 동일합니다.
- GUID: `a4c90ca490a24b93a71de44a7f1e1762`. PNG·meta 변경0.
- Library 원화와 보관 시각은 기획 정본의 `귀여운 숲 슬라임 16종 스프라이트 시트.png`, 2026-08-19 16:06:30 KST 기록을 인용합니다. 직접 로컬 Library 픽셀 검사 결과가 아닙니다.
- 준비 commit `ce32537ad3d628d735323eed57333580e512b91b` → 최초 게임 등록 `139fba10c70292d79145f72524f6867172df4774`(2026-08-19 16:35:12 KST).
- 당시 `문서/08_몬스터/첫_몬스터_초원_슬라임.md`는 사용자 제공1254×1254 그림을 리사이즈 없이 우/하 투명2px로1256×1256에 맞췄다고 기록합니다.

## 검사 상태를 출처 분류와 구분

| 항목 | 실제 상태 |
|---|---|
| 현재 활성 PNG SHA와 기존 감사 비교 | MATCH |
| 바울 대표의 현재 파일 팀 제작 확인 | CONFIRMED |
| 증거 ZIP SHA·해제 | NOT_RUN — 대표가 ZIP은 애초에 없었다고 정정 |
| Library1254²↔현행1256² 좌상단 RGBA 비교 | NOT_RUN |
| 우/하2px Alpha 전수 검사 | NOT_RUN |
| PIXEL PASS/FAIL | 판정하지 않음. 이번 출처 마감의 필수 조건에서 제외 |
| B2 이미지·기존 Unity QA 재검사 | NOT_RUN — 기존 증거 재사용 |

출처 분류 변경의 근거는 **소유자가 해당 현행 파일의 제작 주체를 직접 확인한 것**입니다. 바이트/픽셀 동일성 검사를 완료한 것으로 기록하지 않습니다. 재제작·교체·추가 승인 요청은 필요하지 않습니다.

## 문서와 보호

현재 판정표 `Audit10.csv`와 `files.json`의 네 몬스터 분류를 갱신했고, `REPORT.md` 및 `SLIME_FINAL_PROVENANCE.md`에는 이 최신 마감을 가리키는 표식을 추가했습니다. 이전 UNKNOWN/HOLD는 당시 증거 수준의 이력으로 남깁니다. 작업 브랜치 CODEX_LATEST를 갱신하며 원본 main 문서는 변경하지 않습니다.
브랜치 **`codex/ch1-monster-provenance-final`**. 구현 commit 없음, 제품 파일 변경 **0건**. 원본 local main `4ae186e305d1762d8a08002cf309a5371a7c7e72` 불변. Unity6000.6.5f1 유지, PNG/GUID/meta/Scene/Prefab/Save/UserData·사용자 변경 쓰기0. 원본 main 반영·직접 commit·원격 Push·Merge0.
