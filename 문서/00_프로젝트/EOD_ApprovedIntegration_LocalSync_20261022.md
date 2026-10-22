# 2026-10-22 EOD 승인 통합·로컬 동기화

**DELTA_QA_PASS / PR_PENDING / ORIGINAL_SYNC_PENDING**. Issue #53 및 EOD SafetyContract 실행. 이 문서는 실제 마감 증거가 추가되기 전 동기화 완료를 주장하지 않습니다.

| 범위 | 판정·처리 |
|---|---|
| Bear v4 / Fox v7 | 승인 제품6파일 선별, Bear→Fox 공유 카탈로그 의미병합. 기존 원본 파일 재복사0. 기존154/162 QA 재사용 |
| Village v10 | 승인PNG9 바이트 동일, 별도 Sprite9·Tile9와 meta 총39파일. 기존49 보존. Scene·통행·OPEN16 수정0 |
| 담당 문서 | 10/22 변경된 planning/art-new/art-renewal/tts 문서22 및 승인 QA 보고·근거 선별. 기존 History 보존 |
| 신규 QA | 정적10·격리 Unity15 PASS/0 FAIL. 신규 Import/Tile 참조/합쳐진 Bear·Fox resource closure만 확인 |
| 사용자 보호 | 912파일190,738,290bytes SHA백업, 보호7179파일. 원본Editor·Play·포커스 조작0 |

제품45파일(기존 승인6+신규등록39). 승인 PNG 내용 변경0. Village는 256px, PPU128, 하단중앙 pivot, Point/무압축/무mipmap Sprite와 collider 없는 Tile로 등록합니다. 이후 Scene 배치에는 2unit footprint와 Tile anchor를 설계해야 하며 이번 범위에는 포함하지 않습니다. 기존49는 검증된 납품 캐시에서 SHA 재확인했으며 신규제품 경로가 분리되어 대체0입니다. 현재 승인 ZIP 실제 SHA·CRC를 다시 확인하고 캐시/신규9·기존49 SHA가 ZIP 실물과 동일함을 확인했습니다. ZIP SHA f50e312eea52a6bca8272d78532224e2c66a6b40c4b1c8be5cd164b52b2bcee5, 원본 전체 보호는 별도7179 SHA로 확인합니다.

격리 Unity6000.6.5f1에서 신규등록 및 공유카탈로그15검사 완료. 검증 하네스 초기 TextureImporter API 컴파일오류는 settings API로 수정 후 최종 컴파일오류0. 기존 URP/Search 환경진단은 별도 이슈이며 원본 자연Import·손플레이 미검증. 실제 움직임은 승인된 Bear154/Fox162 근거를 재사용하고 전수재생0입니다. 새 미술 승인을 다시 내리지 않았습니다.

OPEN16 미해결16 유지, Main23 Quest/NPC/마을 전체구현0, Main24 SkillSave #46 미구현. TTS READY104/152(세린67/태온37), Main21 KEEP WAV7, 미완 BGM/SFX는 제품0. Fox v6 사용0. Wolf/Main22/F1/서리들쥐 기존승인 범위 재적용/전체QA0.

백업 원문·index·refs·patch·Save/UserData는 로컬 Temp/EOD20261022에만 보관합니다. 공개본은 SHA·판정 요약만 포함하며 개인경로/로그 라이선스정보를 제거합니다. reset-hard/clean/stash/forcePush/main직접commit0. 별도 복사본 FF 리허설 뒤 원본FF만 허용합니다. 이미 디스크에 있는6개 승인 파일은 bytes를 재작성하지 않고 index에 인식시킵니다.

[허용목록](Evidence/EOD20261022/ApprovedChangeAllowlist.json) · [신규 검증](Evidence/EOD20261022/IntegrationDeltaQA.json) · [공유 코드 증거](Evidence/EOD20261022/SharedCompanionCodeMergeProof.md) · [타일 출처](Evidence/EOD20261022/VillageRegistrationSource.json).
