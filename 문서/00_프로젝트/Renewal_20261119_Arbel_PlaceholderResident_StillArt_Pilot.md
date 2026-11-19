# LIMITLESS / Chapter2 / 아르벨 임시 주민 교체용 독립 정지 원화 납품 — 2026-11-19

## 대상 확인 및 범위
- 대표 직접 지시: 아르벨 단색 사각형 주민 1명만 독립 원화로 교체. 미술 파일 납품 단계이며 Unity 적용 금지.
- [Issue #111 실제 감사](https://github.com/HappyPaul812/Project-Limitless/issues/111#issuecomment-6941300553) 및 [복구본 상세 보고](https://github.com/HappyPaul812/Project-Limitless/blob/ba59838bf923832362f3b1fccf4897864cc678fa/문서/00_프로젝트/Issue111_ResidualNpc_ApprovedReferences_20261117.md) 참조. Arbel (1.5,-1.5) 활성 VillageNpcPlaceholder 프리팹 GUID 4c4988789a0e2a049b860bfa288d71bc, Scene 인스턴스 VillageNpc, StableID/VillageNpcRole 없음, 표시명 마을 주민, 기존 대사 “어서 오세요. 여기는 우리의 첫 번째 마을입니다.” PlaceholderVisual 1×1 런타임 노란 사각형. 별도 arbel-resident-west와 다른 대상. 기존 실제 Unity 감사 결과를 근거로 위치/역할 확인; 이번 세션에서 PC 화면을 직접 열거나 재실행하지 않았음.
- 기존 담당 인계/Library 검색 범위에서 해당 주민의 독립 승인 원화 미발견. PC 및 모든 브랜치 부재 보증 아님. #111에 처리 결정이 남아 있었으므로 같은 이슈에 본 결과를 기록. 이번 교체용 아트만 제작, 새 ID/역할/대사/퀘스트 확정 및 임시 주민 삭제/숨김 금지.

## 실제 산출물
- `Arbel_PlaceholderResident.png` 512×512 투명 RGBA 정지 1포즈, 124405 bytes, SHA256 `9f2a425f7ab40cb106b13cf812c43b2e2ecf5dcf48ddd79b84f1d8ef0d28822c`. alpha>8 bbox (174,58,337,470), 투명 215784픽셀, 외곽 alpha 접촉0.
- 생성 원본 `a_clean_high_quality_chibi_style_rpg_character_il.png` 1254² RGBA SHA256 `79fe8d0815e8b294a7c6c3d76884e6ef544be886bb052c1bed0590e086aaa970`; 주요 연결 알파 영역 보존, 분리 잔여 알파 제거, crop (355,1,850,1240), 비율 유지 축소 및 y470 기준 정렬. 새 외부 게임 자산 합성0.
- 적갈색 짧은 곱슬머리·수염 없는 젊은 인상·좁은 체형, 적자색 튜닉·크림 셔츠/바지·물병. #113 분양의 묶은 머리/환영 손짓, #117 관리의 회색빛 머리/수염/작업조끼와 도구, 은행의 안경/정장/장부, 장비상인의 고글/앞치마와 구분. OGA11 서쪽 주민 참조의 모자/채소바구니와 구분. 물병은 건조 지역 생활 소품이며 새 직업/신상/서사 확정 아님. 기존 단순 평면 NPC보다 세밀한 화풍과 머리 모티프 일부 유사성은 사용자 미술 검토 대상, 40px 세부 판독 제한.
- 실물 OGA17·OGA16·승인 #113·#117·레온·OGA11 및 신규 원화의 512/128/40 동일 캔버스 오프라인 비교판 `Arbel_Actual_512_128_40.png` SHA256 `03dd0fe56cf067bcfbb967984a206c31e8c1f5e550f64e6c4b55864b139e300e`. 실제 PNG 원본 읽기 전용, 전후 SHA 일치; 128/40 실제 LANCZOS 축소. 실제 Unity 화면이 아님.
- 전체 ZIP `Limitless_Arbel_Resident.zip` 1050091 bytes, SHA256 `968f8f6ca8ba4dc0c5b59b162e333a2d0ea7075f88c45391008f86fc3ee95143`, 9 entries, CRC PASS / Manifest7/7 / SHA256SUMS8/8 PASS. PNG 전용 ZIP `Arbel_Resident_PNG.zip` 123716 bytes, SHA256 `c7cecdd7bb2351baf72e681f0e468e6735541b32d0bf4da8168927fa7e607c40`, CRC PASS. 모두 /mnt/data 작업환경 생성; Windows F: 자동 복사 없음.

## 승인 및 보호
- FILE_TECH_PASS / USER_ART_REVIEW_PENDING / NOT_UNITY_APPLIED. 현재 임시 주민 인스턴스 그대로이며 사용자 승인 후 별도 Codex 최소 적용 및 기존 역할/이름/대화/좌표 보존, PlaceholderVisual과 SpriteRenderer 상충·발접지·스케일·주민 Label 및 기존 상호작용 검증 필요. 기존 StableID가 없으므로 임의 신설 금지; 필요 시 별도 기획 승인.
- 기존 승인17종 BYTE_KEEP, 펫 분양/관리 전용 원화 미적용 유지, 독립 외형 후보9는 이번 범위 제외. 불씨망령 HOLD_UNTIL_NPC_RENEWAL_COMPLETE. Unity6000.6.5f1, #85 HOLD, Scene/Prefab/Save/UserData/Discord 보호. 초상화/애니메이션/Unity 적용0, main 직접 commit/Push/Merge0.