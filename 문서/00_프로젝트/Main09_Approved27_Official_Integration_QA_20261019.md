# Main09 승인 복구 음성27 정식 연결 및 원본 디스크 적용 (2026-10-19)

**USER_APPROVED_27 / OFFICIAL_CATALOG_CONNECTED / DELTA_RUNTIME_PASS / ORIGINAL_DISK_APPLIED / ORIGINAL_EDITOR_RUNTIME_NOT_VERIFIED / MAIN_NOT_MERGED.**

브랜치 `codex/main09-dialogue-voice-fix-20261019`. 선행 최종 검수 `6e1a93c60df5e6e7f7fa88bc2e50f75f7425e968`, 재회 순서 기능 `3f35b54bc9f668696d5eec7d1c88b72c8a15e036`, 정식 음성 기능 `0f6c05db8a80a44b292f570be1a3df359e4bb145`. 기획 `docs/planning:Main09_Voice27_Approval_Field06_Art_v1_Handoff_20261019.md`와 바울 대표의27개 직접 청취 승인에 따릅니다. 과거 ASR20일치/7보류는 이력이며 현재27개 모두 승인 상태입니다.

- 기존 후보 Manifest WAV/PCM SHA27/27 그대로 사용했습니다. TTS/API/음성 생성0, 원음 변조0. 기존 `Story/Main09` WAV/meta54개는 그대로 두고 `Story/Main09Approved20261019`에 WAV27/meta27+foldermeta1, 새55파일을 추가했습니다. 승인 SHA와 정식 WAV byte 일치27/27입니다.
- StoryVoiceCatalog290개 중 Main09 27개 GUID만 교체했습니다. 나머지263개 entry의 ID/화자/GUID/순서가 byte 기준 동일합니다. Main08/18/19/20/21 및 기타 승인 음성 변경0. 새 GUID와 meta, speakerId/원문/PCM은 [승인27개 매핑](Evidence/Main09_Approved27_20261019/Approved27_Mapping.csv)에 있습니다.
- 폴 재회 인사 `paul002` 첫 발화, 원래 `paul001`은 step2 경로 설명 뒤1회인 기존 수정 그대로입니다. Quest6목표/보상35·35/폴 합류/Save 포맷·Scene 변경0.

## 변경분 실제 Unity QA

기존550 PASS/0 FAIL을 재사용하고 전체6단계/음성 전체 길이를 재반복하지 않았습니다. 기존 격리 Client의 **정식 Resources Catalog**를 사용했으며 후보 검수 때의 메모리 Catalog 대체를 사용하지 않았습니다.

- **261 PASS / 0 FAIL**, 컴파일 오류0, 신규 Runtime Console 오류0(기존 Search/URP 환경 이력 구분).
- 전체27개 정식 clip 경로·GUID·원문·speaker·AudioClip.GetData PCM·timeSamples/isPlaying·Player무음·Next/Stop 정리를 확인했습니다.
- 사용자 원본 Save의 복사본(Main09 step2)을 Continue하여 실제 재회 대화 입력→step3 자연 진행→Bootstrap Continue의 Quest/Party/좌표 동일 및 잔류 음성0을 검증했습니다. 원본 Save 사용/수정0, 가상 입력/백그라운드만 사용했습니다.

## 원본 적용과 보호

- 적용 전 원본 code/Catalog가 기존 main blob과 일치하고 사용자 충돌0임을 확인했습니다. 원본 Assets/Save/Settings4180 SHA 재확인 후 기존 원음·UserData·Settings와 변경2파일 **121파일을 SHA 백업**했습니다.
- 원본 디스크에는 새55파일 + Catalog/대사 코드2파일만 반영했습니다. 기존4178파일 예상 외 변경0, UserData23파일·기존 원음/이미지/GUID/meta·Field05 그대로입니다. [원본 적용 결과](Evidence/Main09_Approved27_20261019/OriginalApply.json), [백업 SHA](Evidence/Main09_Approved27_20261019/BackupSHA256.json).
- 원본 Editor/Play/Refresh/포커스 조작0. 원본에서 사람이 실제 재생하는 확인은 미검증입니다. 평소 Unity 자연 Import 후 Main09 재회 대사를 확인하시면 됩니다.
- 원본 local main HEAD `675988c9f2c5c507d9a696b0378be27c0bd3309b`는 유지했습니다. 승인 기능이 디스크의 미커밋 변경으로 반영된 상태이며, 기존 dirty는 삭제/정리하지 않았습니다. **Push/PR/main 병합0**, 별도 Git 통합 승인 뒤 진행합니다. Field06 미술 검수와 별도 커밋/브랜치입니다.
