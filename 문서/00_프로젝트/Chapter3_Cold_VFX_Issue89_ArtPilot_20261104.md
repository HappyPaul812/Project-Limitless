# LIMITLESS / Chapter3 / Issue #89 — 한기·동상·동결괴사 VFX 첫 시각 파일럿 (2026-11-04)

**담당**: Chapter3 이후 메인 신규 아트 · 브랜치 `docs/art-new`  
**현재 판정**: `ART_CANDIDATE / OFFLINE_PNG_FILE_QA_PASS / USER_VISUAL_REVIEW_PENDING / UNITY_NOT_TESTED / NOT_UNITY_APPLIED`.  
**근거**: [Issue #89 발주](https://github.com/HappyPaul812/Project-Limitless/issues/89), [#88 기획](https://github.com/HappyPaul812/Project-Limitless/issues/88), [냉기 계열 VFX 기획 정본](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/10_전투/Chapter3_냉기계열_상태이상_VFX_기획안_20261104.md), 기존 [냉기·화로 계약](https://github.com/HappyPaul812/Project-Limitless/blob/main/문서/10_전투/Chapter3_냉기_보스기믹_20261012.md).

## 실제 납품 ZIP (신규 게임 아트 후보 PNG 총 3개)

- **파일**: `Limitless_Ch3_ColdVFX_Issue89_Pilot_20261104.zip`
- **실물 ZIP SHA256**: `83b4efeee62cdb4cabf7dbcc4d2aea21cbefdb4507e03f53b8c59d8079e9e4ba`
- **실물 크기**: **1,535,561 bytes / 17 entries**.
- **ZIP CRC**: Python zipfile `testzip() == None` PASS; 내부 `SHA256SUMS.txt`의 **16/16 파일** ZIP 바이트 실제 SHA256 대조 PASS. `QA/QA_Result.json`에 검사 항목 기록.
- **전달 위치**: ChatGPT **파일 라이브러리** `/LIMITLESS/Chapter3_Issue89_ColdVFX_Pilot_20261104/Limitless_Ch3_ColdVFX_Issue89_Pilot_20261104.zip`에 실물 등록 및 목록 재조회 성공. 사용자 Windows `F:\Downloads` 자동 복사 / 사용자 PC 사본 SHA 확인은 하지 않음. **GitHub에는 Markdown 보고/인계만 커밋하며 ZIP/PNG 바이너리 미업로드**.
- 사용자 검토용 비교판 2개도 **동일 Library 폴더에 개별 PNG 등록 성공**: `Issue89_ColdVFX_Comparison.png`와 `Issue89_Background_Comparison.png`.
- 이번 ZIP의 게임 VFX 후보는 **독립 정지 PNG 총3장**, 애니메이션·VFX SpriteSheet·Particle·Unity .meta·Scene·Prefab 추가 제작 **0**.

| 상태 | 실제 신규 후보 파일 (`Candidate/`) | 크기/모드 | SHA256 |
|---|---|---|---|
| **한기** | `VFX_Ch3_Chill_Oneshot_01_CANDIDATE_512_RGBA.png` | 512×512 투명 RGBA | `dd7bd4bb73d6e5c4b3d02521da2e0f37e0ba4347597ee97c125d3e8fd693d793` |
| **동상** | `VFX_Ch3_Frostbite_Sustain_01_CANDIDATE_512_RGBA.png` | 512×512 투명 RGBA | `49937d84472a2adc59da04a17c43d22a37d3d63992a45e26581d534f7b93e035` |
| **동결괴사** | `VFX_Ch3_FrostNecrosis_Stack2_01_CANDIDATE_512_RGBA.png` | 512×512 투명 RGBA | `f1e69de225e6958f813fd155a7a06a7aea34e6ba7f039e7665e1e614b6868a4a` |

각 PNG 알파 채널에 완전 투명 배경 및 실제 유효 효과 픽셀이 있으며, 512 캔버스 안에 양의 여백을 두고 배치. 128×128 / 40×40 실 이미지 LANCZOS 축소 총6개 `Preview/{CHILL,FROSTBITE,NECROSIS}_{128,40}px_ACTUAL.png` 수록. 크기와 SHA는 `Manifest.json`과 `SHA256SUMS.txt`에 실제 실물별로 기록.

## 대표 미술 확인 자료

- `Preview/Issue89_ColdVFX_ThreeStates_512_128_40_OFFLINE.png`: **1840×1330**; 3종 독립 형태/512 원본/128·40 실축소를 한 장에 표시. 비교 보드에만 상태명/정보를 표시, **게임 VFX 본체 PNG에는 상태 문자·중첩 숫자 미내장**.
- `Preview/Issue89_ColdVFX_Snow_DarkBG_SameScale_OFFLINE.png`: **1780×1000**; 밝은 설원/어두운 전투 배경 위 동일 128px 실 PNG를 별도 합성한 OFFLINE 가독성 목업. **실제 GameView·Battle HUD/Unity 3비율 테스트가 아님**.
- `Docs/DesignNotes.md`: 한기=흐르는 냉기/눈송이·2턴 행동불가, 동상=붙어 있는 결정/균열·행동불가+HP 감소, 동결괴사=검푸른 결빙 균열·보스 독립 중첩 위험2/3. 색상에만 의존 금지, 별도 아이콘·상태명·턴/중첩 표시, HUD·입력 방해 금지, 3번째 즉시 KO는 **상태3/3을 오래 유지하는 효과로 만들지 않음**.
- `Docs/SourceDeclaration.md`: 기존 승인 캐릭터/게임 아트 수정/재색칠0, 본 대화 생성 독립 이미지 3종(각1254² 투명 PNG)에서 크롭·저알파 노이즈 정리·512² 규격화, 외부 제3자 스톡/상업 라이선스 리소스 수록0. 원 생성 그림 해시는 `Manifest.json.source_declaration`에 기재. PNG에 외부 상표/글자 없음.
- `README_KR.md`, `Manifest.json`, `QA/QA_Result.json`, `SHA256SUMS.txt` 포함. 파일별 SHA256와 정확한 상대경로는 ZIP 내 제공.

## 기술·기획 경계

- **파일 QA PASS**: 3/3 PNG decode·512²·RGBA·투명 알파, 6/6 실제 축소 이미지, ZIP CRC 및 SHA256SUMS16/16. **상태별 시각 인지·40px 실게임 읽힘·밝은 배경에서 한기 대비·캐릭터/HP/MP/스킬 UI 가림은 Codex Unity/사용자 시각 검증 전**. 40px은 상세 문양 식별이 약하므로 상태명·서로 다른 형태의 별도 아이콘/경고가 필요. `USER_VISUAL_REVIEW_PENDING` / `UNITY_NOT_TESTED`.
- 신규 **512px은 이번 후보 PNG 실물 캔버스**일 뿐 승인 게임 VFX UI/월드 PPU가 아니다. `PPU/Pivot/Sorting/WorldScale/Duration/FPS=SPEC_PENDING`; 정지 PNG이므로 애니 FPS 적용 없음. Unity6000.6.5f1 격리 실행/전투 HUD 렌더/기존 F2 스프라이트 동일배율 검증 **0**.
- 냉기 상태 3종은 **서로 독립**: 한기는 2턴 행동불가, 동상 행동불가+HP 감소 (틱/피해량 TBD), 동결괴사는 1→2중첩 후 세 번째 적용 즉시 기존 KO. 새 3종은 원샷/유지/2중첩의 **각 1개 대표 정지 시안**일 뿐 실제 상태 효과의 모든 생명주기를 구현하지 않음. 한기→동상→동결괴사 자동 진화 연출0.
- 기존 정화Ⅰ·정화Ⅱ·어묵탕·화로·온기의 불꽃/해빙 연고 계약, Codex #88 냉기 공통 런타임 격리 63 PASS(상태 ID/틱 미확정 포함) **변경0**. 본 회차에서 해제/HP 틱/보스 능력치·KO/Save 새로운 수치나 ID 생성0.
- 기존 승인 Frost Owl v3 Idle8/Attack4·NaturalBroken Stump v4·환경43/승인 Ground9, F2/전투 기능·이전 미승인 Owl IceArc VFX 후보 보존. **원본 Unity/Scene/Prefab/코드/meta/Save/UserData/BuildSettings/Packages**, `main` 직접 commit/Push/merge0.
- **바울 대표 최종 미술 승인 별도 필요**. 승인된 시안만 Codex의 Unity **격리** HUD 가림·512/128/40 실게임 표기·실 Scale/PPU/Pivot·접근성/가독성 DeltaQA 후 본편 적용은 다시 명시적 승인 필요. 본 보고는 `ART_CANDIDATE / OFFLINE_FILE_QA_PASS / USER_VISUAL_REVIEW_PENDING / NOT_UNITY_APPLIED` 상태로 완료.
