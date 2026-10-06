# LIMITLESS 수정 완료 Voice 개발 기록 영상 — 2026-10-06

최종 영상: `F:\Downloads\Limitless_DevJourney_2026-10-06\Limitless_DevJourney_FixedVoice_2026-10-06.mp4`

Unity 공식 Recorder5.1.7의 Game View/Game Audio 원본으로 녹화했다. LOCAL/GitHub main 시작 HEAD `162ae1eeb36f03589306bddadb73891291450954`를 read-only 조회해 일치 확인했다. GitHub Push는 수행하지 않았다.

| 녹화 항목 | 결과 |
|---|---|
| Duration | 328.133333초 / 5분28.13초 |
| Resolution / FPS | 1920×1080 / 30fps 고정 / 9,844프레임 |
| Video | H.264 MP4 |
| Audio | AAC 48kHz Stereo / 192kbps 지정 |
| 파일 크기 | 51,823,681bytes / 49.42MiB |
| 실제 Stream 길이 | Video328.133333 / Audio328.129000초 / 차이4.33ms |
| Decode / 끝부분 | 전체 decode exit0 / 오류0 / 끝327.5초 Field 정상 |
| 검은 화면 | blackdetect 지속0.3초 이상 검출0 |
| 음량 | mean -18.6dB / peak -0.4dB |
| SHA256 | `bd85e87e6c7c33b2ae2565c33701c4881b780a389c819e94bb7378ff057aa2cf` |

## 실제 사용 Voice 및 화면

- Intro `opening_001~006` / Storyteller6개. 각 문장 전체 발화 포함, 자연 종료 뒤 건너뛰기로 생성 화면 진입.
- Main03 AfterBattleConversation 태온 `main03_taeon_supp_008~012` / 5개. Player2페이지 무음 포함.
- Main04 첫 조우10페이지: 미엘 `main04_miel_supp_001~006`, 태온 `main04_taeon_supp_001~003` / Character9개와 Player1페이지 무음.
- Main04 전투 전: 태온 `main04_taeon_supp_004` “또 옵니다. 제가 앞을 맡겠습니다.”, 미엘 `main04_miel_supp_007`, Player “갑시다.” 정상 무음.
- 총 Intro6 + Character16 = Voice22개. [최종 AAC 원본 정합23행](LIMITLESS_DevJourney_FixedVoice_2026_10_06_AAC_QA.csv)은 위22개와 실제 전투 BGM `Steel_and_Sunlight`를 포함한다.
- Main04 미엘008은 정상 원본이지만 전투 후 NPC 대화에 속하므로 이번에는 포함하지 않는다. 전투 후 NPC 대화를 전혀 시작하지 않았다.

| 사용자 요청 QA | 결과 및 근거 |
|---|---|
| Intro Voice | PASS: 최종 AAC 앞·중간·끝과 원본 일치6/6 |
| Character Voice | PASS: 최종 AAC 원본 일치16/16 |
| BGM / Battle Audio | PASS: 실제 Field BGM 및 전투 BGM 기록. 전투 BGM 3개 구간 correlation0.9979이상. 현재 전투는 별도 공격 SFX AudioSource가 없으므로 효과음을 합성하지 않음 |
| Subtitle / Voice 의미 | PASS: 기존 사용자 Runtime 청취 PASS 원본과 동일한 WAV, 최종 AAC 정합, 실제 Subtitle 프레임 및 Runtime 순서 대조 |
| Voice cutoff | PASS: 자연 종료 뒤 Next. 모든 원본 마지막 발화 창까지 AAC 정합. 편집 컷은 발화 밖 |
| Voice 순서 | PASS: CSV 실제 MP4 시작 시간과 Main03/04 정식 Sequence 동일 |
| Portrait / Player | PASS: 미엘 Portrait_Miel / 태온 Portrait_Taeon. Main04 Runtime 관찰 및 최종214초 Player 화면 이름/본문만 표시, Voice·Portrait 없음 |
| Dialogue UI | PASS: 화자/본문/Footer/Portrait 비중첩. 전체 문장 및 긴 Main03 후반 자막 확인 |
| Character Creation | 포함: 성별 남→여→남, 청각→지체 길, 수호자→마도사. 최종 남성·지체·마도사 |
| Sprite Preview | 포함: 실제 선택 UI와 수호자/마도사 자동 Sprite 변화, 최종 확인 화면 |
| Story Dialogue / Field | 포함: 위 PASS 구간 및 실제 초원 |
| Battle | 포함: 실제 Background/Turn/Skill UI, 치유의 빛·파이어 볼·썬더볼트·일반 공격, 피해 및 적 행동 |
| Victory | PASS: 실제 정상 명령11회로 승리, EXP32/탈렌트11/아이템·Lv1→2 결과 표시 |
| Field 복귀 | PASS: 실제 VictoryReturn 버튼, 이후 짧은 이동 및 약5초 Field 유지, 추가 NPC 대화0 |
| 미해결 Voice 미노출 | PASS: 미엘009·태온006·미엘010·태온007·미엘011 모두 진입/재생0. TTS_REGEN_REQUIRED5 유지 |

이번 MP4에 대해 신규 사용자 사람 청취 PASS를 받은 것은 아니다. 의미 판정은 이미 사용자 청취 PASS가 확정된 원본과 최종 MP4 AAC의 동일성 및 Subtitle/Sequence 대조에 근거하며, 새 의미 QA 승격은 수행하지 않았다. MP4 전체를 ffmpeg로 디코딩 재생하고 실제 프레임을 추출해 검사했다.

## 촬영 경로와 컷

기존 사용자 Save는 Main04 전투 후였다. 해당 Save/Settings를 보존하고 격리 슬롯에서 정식 새 게임→UI 생성→Main01 대화→Main02 실제 몬스터 승리/흔적 조사→Main03 첫 대화/실제 Story Battle 승리→후반 PASS 대화→단서 조사→Main04로 진행했다. Main03 단서001/002 등 미검증 구간은 녹화를 끈 상태에서 정상 진행했으며 Quest 상태를 주입하지 않았다.

Title/Intro/생성 선택의 긴 대기와 대사 자연 종료 후 대기를 컷 편집했다. Main03 후반과 Main04 첫 조우 사이에는 녹화 밖 정상 진행으로 인한 컷이 있다. 최초 Main04 전투는 이전 부상으로 패배했으며 첫 조우 화면만 보존했다. 정식 패배 복귀/회복 및 이동 중 일반 몬스터 정상 승리 뒤 Main04 재도전 전투를 새로 촬영했다. 최초 첫 조우9Voice→재도전의 전투 전004/007→실제 승리/결과 버튼/Field를 연결했으므로 이 영상은 하나의 무편집 연속 플레이를 주장하지 않는다.

편집 구간: Title/Intro/생성 원본 `(118–178),(194–228),(236–247),(269–277),(308–330),(338–360)`초 → Main03후반31.7666초 → Main04첫조우 원본 `(18–26),(51.8–91.8)`초 → 정상 재도전91.3666초. 원본 Game Audio만 정상 속도로 사용했다. 외부 음성/효과음/가짜 장면 추가0. 처음 연결한 초안의 서로 다른 MP4 time_base로 video640.9초/audio328.1초 문제가 검출되어 초안을 Temp에 보존하고 각 입력 PTS/time_base를 정규화해 재인코딩했다. 위 최종4.33ms 정합 및 전체 decode가 수정 후 결과다.

## 보호 및 Editor 정리

- HP/MP/Level/Quest/보상/적/강제승리/QA EndBattle/위치 순간이동 조작0. 정식 Button.onClick 및 PlayerController 방향 입력만 전달.
- 임시 Editor 녹화 입력 도구 삭제. Recorder 임시 설치 전 manifest/lock 원래 바이트 복원. Scene/게임 기능 수정0.
- 시작 시 기존 Unity Assets/ProjectSettings/Packages/UserData **3,048파일 SHA256 전부 일치 / 변경·누락0 / 새파일0**. 사용자 수동 Sprite, WAV/meta/GUID/Importer/Catalog/Registry, Save/Settings 보존.
- 기존 영상 덮어쓰기0, 기존 unrelated Git 변경102건 보존. 영상은 Downloads에만 저장, 저장소에는 QA 문서만 반영.
- 종료 clean Bootstrap Edit Mode / is_focused=false / 격리 Save 해제 / 컴파일 오류0. Recorder 정리 후 ImportWorker HW0/HW2 충돌2건을 관찰했으나 동기 Refresh/재컴파일 후 idle·compiling=false·Console Error0을 재확인했다. 오류 이력을 최종 영상 QA와 구분한다.
- 백그라운드 검증만 사용, OS 포커스·Unity foreground·Computer Use 조작0.

다음 권장 작업은 미해결 Main04 전투 후 Voice5개 재생성 및 별도 사용자 Runtime 청취 검증이다. 사용자 직접 확인은 최종 MP4 시청이며, 새 Voice 의미 PASS 요청은 이번 범위에 포함하지 않는다. 관련 선행commit `162ae1eeb36f03589306bddadb73891291450954`, 이번 Docs commit hash는 최종 보고를 참조한다.
