# Issue #93 재발 수정 — Main11 음성·Field03 입구

**수정·표적 검증 완료 / LOCAL_MAIN_APPLIED / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**

- 기준 main: `706e70102ccc350f49cd881ca128c3afbe5d9d04`.
- 구현: `71919894d15765c31a6ebb5758d2ade4cb5ba90e`, 로컬 main에 FF 반영 완료.
- 브랜치: `codex/fix-20261105-main11-voice-field03-entrance`. 거리 안내 정합·마감 문서는 같은 브랜치 후속 커밋이며 최종 SHA는 Git HEAD와 Issue #93 결과 댓글을 기준으로 한다.
- Unity **6000.6.5f1** 고정. 원격 Push·병합, main 직접 커밋 없음.

## 실제 원인과 음성 복구

카탈로그의 ID→동일명 WAV GUID 연결은 정상이었지만 WAV의 **실제 발화 내용**이 잘못 분할되어 있었다. 이전 검증의 ID 일치만으로는 이 오류를 발견할 수 없었다.

| ID | 수정 전 실제 발화 | 수정 후 |
|---|---|---|
| main11_miel_001 | 모두 상태를 확인하고 움직여요. | 뒤쪽을 보세요. 길이 열린 것 같아요. |
| main11_miel_002 | 뒤쪽을 보세요. 길이 열린 것 같아요. | 오래된 흔적이지만 누군가 머물렀던 자리였겠네요. |
| main11_miel_003 | 오래된 흔적이지만… | 쓰이지 않은 안치 공간이 많아요. 위층과는 다르네요. |

`F:/Downloads/Voice/limitless_story_tts_pre_serin.py`의 무음 길이 기반 통합 음성 분할 방식과 실제 PCM을 대조했다. 납품 원본은 로컬 `Google TTS/Limitless_TTS_Output_PreSerin/`에 남아 있었다. 문장 경계 대신 문장 사이·감탄사 주변 무음이 선택되면서 번호 밀림·감탄사만 남은 파일·두 문장 합침이 생겼다.

Main11 28개를 대조해 미엘 **001~005**, 폴 **001·002·003·008·009**만 복구했다. 정상 **18개는 바이트 그대로** 유지했다. 미엘 006·007과 태온 10개 등 정상 음성을 재생성하지 않았다.

- 기존 WAV 전체 복사 또는 원본 통합 WAV의 확인된 PCM 구간만 추출했다. 24kHz/모노/16bit, 리샘플링·DSP·TTS 생성/API 호출 **0**.
- 자막·대본·TTS ID·카탈로그·기존 meta/GUID·동행 조건은 변경하지 않았다.
- 문장 분리는 ASR 단어 순서와 -45dB 무음 구간을 함께 확인했다. 각 원본 SHA256·시작/끝 샘플·결과 SHA256은 `Evidence/Issue93VoiceEntrance20261105/repair-proof.json`에 기록했다.
- 필수 세 상황은 실제 B1 조사·B2 조사·보스 후 대화 흐름에서 검증했다. 보스 후 상태는 복제 Save의 목표 fixture이며 보스전 전체를 재실행하지 않았다.
- 실제 `AudioSource.clip`의 ID/GUID, 재생 샘플 진행, 비영 출력, Unity에서 읽은 PCM의 재전사를 확인했다. 사람의 새 청취 승인으로 표기하지 않는다. ASR의 조사·발음 표기 오차는 음성 재생성 사유로 사용하지 않았다.

## 입구 수정

기존 `MainQuest10FieldFlow`가 만들던 불투명 Mark Quad의 Sorting Order 5와 큰 목표판이 플레이어를 덮었다. Player Sorting을 전체 위로 올리는 방식은 사용하지 않았다.

- 승인된 `LL_C1_D01B1_Stone_Descent_Stairs_192x160_v1.png`를 참조하는 `Field03CatacombEntrance.asset`을 추가했다. 원본 PNG는 수정·복제·재제작하지 않았다.
- 회색 문 Quad 대신 지면 Sorting -1의 내려가는 계단을 입구 앞 대기 위치와 분리해 배치했다.
- 가까운 입구의 큰 목표판은 숨기고 작은 조작 안내를 계단 옆에 표시한다. 안내 전환은 실제 입구 거리 3m로 일치시키고 3.5m에서는 기존 Navigation을 유지한다.
- 실제 입구 상호작용 좌표 `(0.55,-4.85)`, 반경1.6, 귀환 Spawn `(0.55,-2.65)`, Scene/Bounds/Collider, 안전지대와 Quest 순서는 유지했다.

제품 변경 파일은 Main11 WAV10개, `MainQuest10FieldFlow.cs`, `QuestNavigationPresenter.cs`, 신규 환경 표시 asset/meta이다. 기존 숲거미 Attack8·Main11 동행 지문·B1/B2 귀환 코드는 변경하지 않았다.

## 실제 검증 결과

| 신규 검사 | 결과 |
|---|---|
| Unity 음성 재생·출력·입구 E/패드A·기존 B1 귀환 | **65 PASS / 0 FAIL** |
| 최종 F 진입·근거리/3.5m 안내 정합 | **9 PASS / 0 FAIL** |
| 수정 후 실제 게임 프레임·960/1280·도보/휠체어 | **18 PASS / 0 FAIL** |
| 범위·PCM SHA·정상 음성·GUID·기존 기능 보호 | **19 PASS / 0 FAIL** |

위 검사는 중복이 있으므로 합산하지 않는다. 음성10개의 Unity PCM 재전사도 원래 대사에 대응했다. 전후 검증 플레이어 빌드 2회 모두 Succeeded/errors0. 기존 전체 QA와 이전 121/37/15 검사는 재실행·합산하지 않았다.

배치 Editor/Player의 직접 GameView 캡처는 검은 화면이어서 해당 이미지를 제외했다. **사용자가 이 대화에서 검증 창 표시·활성화를 명시적으로 허용한 뒤**, 별도 제품명·저장 위치의 검증 플레이어에서 실제 화면을 캡처했다. 두 검증 창은 캡처 후 자동 종료했다. 원본 Unity 창은 조작하지 않았다. 입구 회귀 장면은 복제 Save에 Main10 입구 조사 목표 fixture를 구성해 재현했다.

| 조건 | 수정 전 | 수정 후 |
|---|---|---|
| 도보 960×720 | [전](../../Temp/Issue93Voice/Evidence/before-screen/entrance-path.hearing-960.png) | [후](../../Temp/Issue93Voice/Evidence/after-screen/entrance-path.hearing-960.png) |
| 휠체어 960×720 | [전](../../Temp/Issue93Voice/Evidence/before-screen/entrance-path.mobility-960.png) | [후](../../Temp/Issue93Voice/Evidence/after-screen/entrance-path.mobility-960.png) |
| 도보 1280×720 | [전](../../Temp/Issue93Voice/Evidence/before-screen/entrance-path.hearing-1280.png) | [후](../../Temp/Issue93Voice/Evidence/after-screen/entrance-path.hearing-1280.png) |
| 휠체어 1280×720 | [전](../../Temp/Issue93Voice/Evidence/before-screen/entrance-path.mobility-1280.png) | [후](../../Temp/Issue93Voice/Evidence/after-screen/entrance-path.mobility-1280.png) |

네 수정 후 화면을 직접 열어 전신 가시성·큰 목표판 제거·계단·조작 안내를 확인했다. 캡처는 오프라인 합성물이 아니며 로컬 전용이다. 실제 사용자 미술 최종 승인으로 승격하지 않는다.

## 원본 보호와 인계

원본 Save01·02·03은 복제본만 시험했다. 사전 보호998파일 중 허용된 잘못된 WAV10개만 수정하고 **나머지988개와 원본 Save는 동일**하다. 사용자 tracked dirty302/untracked550, 승인 이미지, 정상TTS18·기존meta28·카탈로그·Scene·Packages·설정은 보호했다. 격리 검증용 Packages overlay·제품명/그래픽 API 설정·실행기 임시 C#은 제품 커밋에 포함하지 않았다.

5시간 사용 한도로 마감이 중단된 뒤 재개 시, 완료한 빌드·음성·화면·입력 검사를 다시 실행하지 않고 결과를 재사용했다. 남은 거리 정합 커밋과 안전FF·문서·Issue 보고만 마감했다. 원본 Editor의 최종 import/컴파일 확인은 별도 후속 상태 조회로 기록한다.

원본·복제 Save와 원본 WAV 백업은 비공개 `Temp/Issue93Voice/Private/`, 실행/캡처/로그는 `Temp/Issue93Voice/`, 공개 가능한 QA 텍스트는 `Evidence/Issue93VoiceEntrance20261105/`에 보존한다. 새 TTS 생성이나 추가 미술 제작은 필요하지 않다.
