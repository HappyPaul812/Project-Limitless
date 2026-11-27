# Issue #125 · Main26 최종 승인 음성 22개 연결 · 2026-11-27

제품 `0a2cead1270f758f0776444b908db3ac487a6dd4` · 브랜치 `codex/issue125-main26-voice-20261127` · LOCAL_COMMITTED / REMOTE_NOT_PUSHED.

[대표 청취 승인](https://github.com/HappyPaul812/Project-Limitless/issues/125#issuecomment-7216680752) · [최신 기획 정정](https://github.com/HappyPaul812/Project-Limitless/issues/125#issuecomment-7216754712). Main26 제품 `bbee953023f8b1d5f0f63ca918838aea8ed64593` 및 보고 `fa433a0a33e99ac2f1c2328222bb4c96c238155f`를 포함한 격리 worktree를 사용했습니다.

## 실물 및 연결 검증

- 원본 폴더 `F:\Downloads\Voice\Chapter3_StageOnly_72\Main26\`. 고정 manifest SHA256 `c76f46f4e4b3831b6adf6e1705c342eb3e090ddbe17503d1ffa8bed59934a1f2`가 제작기 납품 보고와 일치합니다.
- WAV 22/22가 sidecar의 WAV SHA256·원문 SHA256·화자·Voice·모델과 일치했습니다. PCM 24kHz/16bit/mono와 파일 길이를 확인했습니다. Stage ID를 원문과 화자로 대조해 제품 대사 순서를 확인하고, 기존 개별 ID가 비어 있던 발화 22개에만 표의 Runtime ID를 부여했습니다.
- 기존 규칙에 따라 `Assets/_Project/Audio/Voice/Story/Main26/`에 승인 WAV를 바이트 그대로 복사하고 `StoryVoiceCatalog`에 추가했습니다. 기존 카탈로그 331행 및 게임 음성 폴더의 기존 WAV 389개와 메타를 보존했습니다. 제작 원본 Main24 10개 및 Main25 9개도 동결 manifest의 해시와 일치했습니다.
- 기존 대사 원문·화자·순서, 퀘스트·기능·Save/UserData, #85/#131/#132, 기존 승인 음성 181개 및 TTS 원본을 보호했습니다. 합성/API 호출·main 병합·Push는 없었습니다. 기존 38 PASS의 전체 재검증은 생략했습니다.

## Unity 표적 결과

Unity 6000.6.5f1 백그라운드 F4 Play Mode 종료 0. 실제 Main26 상호작용 처리기로 쉼터 조사 → 정령 전투 직전 → 주민 철수 → 차 대화 및 마무리 순서로 검증했습니다. 실제 StoryVoiceCatalog와 화자 매칭, AudioSource Clip, 재생 상태 및 샘플 위치 증가를 22/22 확인했습니다. 주민·플레이어 무음 4개에서 이전 클립 정리도 확인했습니다. 기기 출력 청취는 별도 수행하지 않았습니다. Editor Search 인덱스 예외가 기록됐으며 음성 검수는 정상 완료했습니다. 합성 체크포인트를 사용해 선행 전투와 기존 38 PASS 재검증은 생략했습니다.

## 대사 재생 순서 및 파일 SHA256

| Runtime ID | 화자 ID | Voice | WAV SHA256 |
|---|---|---|---|
| main26_miel_001 | companion_miel | Sulafat | `bc23b811b2d389f1f78ab090b2d65f95920a5b5957a58bb77978e75266abf34f` |
| main26_serin_001 | companion_serin | Erinome | `5d859d66adfd3287eb6ff9f939e6ebd80729eb259216969137929666d885ac7a` |
| main26_paul_001 | companion_paul | Achird | `c1b49209248c00752fe08205c9e7e96176e9cc211b991a5382eb85edda8ae3a0` |
| main26_miel_002 | companion_miel | Sulafat | `e622a53bc6c6dc36ef0662c5f7651c8e49013edfa5507f3b58cc4da6f5ff509f` |
| main26_haren_001 | story_haren | Alnilam | `c2bc99e82c448d88f282beb735ef6348c225f2a6c8d8afbcb663b797c4714d47` |
| main26_taeon_001 | companion_taeon | Algieba | `140772b88fbde8e9e762c979da3298c06fa0328727b4ea4ea4e8c3234a6fbefe` |
| main26_haren_002 | story_haren | Alnilam | `f8997adaf731975bf3e0e69a8807db0d9e53a61979b813fbb35a0738f91642fb` |
| main26_taeon_002 | companion_taeon | Algieba | `8ffc708bfd80d7df07272a7178ef197d1d4abcbcc3a0daca98f1f405f1180d4d` |
| main26_serin_002 | companion_serin | Erinome | `4a130caa4e02ca9c207f20803c677e0fb585a4c84cb4d00b39de1d403842bab3` |
| main26_paul_002 | companion_paul | Achird | `a450b4a3246b3ee6ab8de05966840b421709cd78f3d668004c1d9ea5fc75ac7f` |
| main26_miel_003 | companion_miel | Sulafat | `efdd873138228cf74dfa335e73cefe1195765468b8d4de14a2dfa897abd1731e` |
| main26_haren_003 | story_haren | Alnilam | `30235ff50525ebcb8138a29dc93f285082ca06cb08a2df8427ea9d3536c32645` |
| main26_serin_003 | companion_serin | Erinome | `8a6fc0651d97393bc02e4bd83d085947b996cca647d0b36a6d01842b7c19ca3f` |
| main26_miel_004 | companion_miel | Sulafat | `09d5d5cc76120a2971003fdd7c4fbb0d952b6b0cc7cfd934e526dd0ae3a75b65` |
| main26_serin_004 | companion_serin | Erinome | `40a9ebaffc6e1942951169c1e5c8724fabe0c3673945518dcabeaf29b9351a3c` |
| main26_miel_005 | companion_miel | Sulafat | `8aa89d83de14e318ac03b30b9f2ef8e9d667f3488d2ffe8c68ad3b0b5c5cf1a0` |
| main26_serin_005 | companion_serin | Erinome | `00c2a640d51838691e7828e87b7c8eeb93e4a728a8dd7fd0afc841512956ebed` |
| main26_miel_006 | companion_miel | Sulafat | `98a05b47f4cca3916bfc1ffa1118b09eb76e90c3102975051cd0c46638a1db77` |
| main26_serin_006 | companion_serin | Erinome | `4e6a86b62bf61c59abf897a3f0ed35ba0beeae1f5c6d2316866b85dca3f09cdb` |
| main26_miel_007 | companion_miel | Sulafat | `65042377cf0220338d196a4715142c19098fa7fefe3edb7498ba64131d420538` |
| main26_taeon_003 | companion_taeon | Algieba | `d6de094f26cef527ba2322f8cf0db465cd7ada122db4340526c4aaf7abcdf175` |
| main26_haren_004 | story_haren | Alnilam | `efb7ea233a31de70facc818e3e8b0589915d00b238bb1f7f331c9c88a7f46365` |
