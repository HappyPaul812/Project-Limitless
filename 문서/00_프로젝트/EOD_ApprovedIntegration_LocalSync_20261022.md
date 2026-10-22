# 2026-10-22 EOD 승인 통합·로컬 동기화

**PR54_MERGED / ORIGINAL_SYNC_COMPLETE / USER_FILES_PRESERVED**. Issue #53 및 EOD SafetyContract 실행. 아래는 PR54 실제 병합·원본동기화 완료 시점의 스냅샷입니다. 문서 후속PR의 최종 SHA는 Issue53 및 로컬 증거에 후속 기록합니다.

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


## 실제 마감 증거

## 2026-10-22 EOD 마감 — PR54 병합·원본 SYNC_COMPLETE 확인

[PR54](https://github.com/HappyPaul812/Project-Limitless/pull/54) merge `e2801286ccd26fe2060d9fee658ac272da4b4f2b`: 승인 Bear v4·Fox v7 공유 코드 양쪽 보존6파일 + 마을 최종승인9 Sprite/Tile 신규등록39파일 = 제품45파일, 담당 문서·근거152파일, 총197파일 통합. 신규 정적10/격리Unity15 PASS, 기존154/162/Wolf/Main22 전체QA 재실행0.

실제 GitHub main=origin/main=원본local main=e2801286, index=HEAD/staged0 확인. 백업912파일190,738,290bytes SHA불일치0, 보호7179파일 중 승인문서6개 외 변경0. Save/UserData/개인설정/Editor·미커밋 작업 보존. 이미 원본적용 Bear/Fox6파일 재작성0. dirty310→308/untracked554→550은 승인6경로 Git 편입이며 그 밖 상태 동일. 원본Editor/Play/포커스0, 자연Import·손플레이는 미검증.

이 문서는 PR54 완료 시점 스냅샷입니다. 문서 후속PR 이후 최종3자 SHA는 로컬 `Temp/EOD20261022/OriginalLocalSync_Proof.json` 및 Issue #53 완료기록을 정본으로 확인합니다. 마지막 관련 제품통합 commit e2801286. 추가제품 변경0.

OPEN16 미해결16 유지, Main23 전체·Main24 Save #46 HOLD. 미승인TTS READY104/152·Main21 KEEP WAV7·미완BGM/SFX 게임 적용0. 다음은 별도 승인 후 해당 범위만 진행합니다. 기존 승인기능 재구현·전체QA 반복 금지.


[실제동기화 스냅샷](Evidence/EOD20261022/OriginalLocalSync_Proof.json) · [원본보호](Evidence/EOD20261022/UserOriginalProtectionProof.json) · [복사본 리허설](Evidence/EOD20261022/SimulationResult.json).
