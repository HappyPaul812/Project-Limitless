# LIMITLESS / Chapter2 / Issue #109 — Main13 아르벨 지역 책임자 레온 정지 원화 납품

## 근거 및 기존 원화 확인
- 대표 최종 승인: Main07 숲길 부상자 기존 512 PNG 유지(sha256 `90c727b95bee6b2586333d44ebb05b8077664a7e5877084cbbe4a1010676b432`). 이번 작업에서 재제작·수정하지 않음.
- [Issue #109](https://github.com/HappyPaul812/Project-Limitless/issues/109), [Chapter2 Main12~14 정본](https://github.com/HappyPaul812/Project-Limitless/blob/main/문서/03_스토리/Chapter2_도입부_Main12_14.md), [VillageNpcAppearanceCatalog.cs](https://github.com/HappyPaul812/Project-Limitless/blob/main/Unity/Client/Assets/_Project/Scripts/NPC/VillageNpcAppearanceCatalog.cs), 담당 최신 인계 확인. Main13 「메마르는 땅」 목표 meet_arbel_warden, StableID arbel-leon, 아르벨 지역 책임자. 현재 원격 main 매핑은 OGA09_VillageRepresentative 공유.
- GitHub main 재귀 트리 7600개 항목(응답 truncated=false), DialoguePortraitDefinitions 및 Portraits 경로, 담당 인계와 접근 가능한 Library를 조사했으며 레온 전용 기승인 원화/정식 초상화를 찾지 못함. 기존 음성 파일은 확인됨. 사용자 로컬 전체 및 다른 브랜치 전수 검증은 아님. **기존 그림과 일치 여부는 공식 레온 초상화 미발견으로 미검증**. 현 신규 디자인은 제안 원화이며 별도 승인 필요.

## 산출물
- `Main13_Leon.png` 512×512 투명 RGBA 정지 1포즈, 155385 bytes, sha256 `ff7cfd081e7727a7ab228bae0a5ece879727b98594514c932d4a616396bc8fdc`.
- 생성 시안 `사막의_지도자_레온.png` sha256 `095aea6f892c2e06ef89943e5f79a2dad5ab929987c9140ce3b949cd52bb8bb4`. 외부 고립 알파 제거 후 종횡비 유지 축소·중앙 정렬. alpha>8 bbox (158,57,354,469), 캔버스 테두리 알파 픽셀 0.
- 적갈색 어깨천, 청록색 실용복, 가죽 허리주머니, 수염과 묶은 머리로 지역 책임자의 인상과 건조 지역 생활복 표현. OGA09의 은회색 머리/푸른 옷/지팡이와 구별. OGA20의 방어구·장병기와 구별. Main07과 회색 앞머리 일부 유사하나 수염·묶은 머리·복장·선 자세로 차별화. 부상 및 신상·퀘스트 임의 설정 없음. **OGA09와 비교할 때 기존 평면 화풍 대비 시안은 세밀한 명암이 있어 미술 승인 필요**.
- 직접 비교판 `Main13_Leon_Comparison.png` sha256 `f9019d550ee702121356e343d84dad0a6ce1cbb7864d801b9083f037cba55b7c`, 실제 OGA09·OGA20 후보·승인 Main07·신규 레온 원화 512/128/40px 동일 LANCZOS 축소. 원본 파일 변경 없음. 40px 세부 판독 제한. Unity GameView 검증 아님.
- `Limitless_Main13_Leon.zip` 732482 bytes, sha256 `dfaaf5fe453dadeaa268a24edbdcefafb6a9b0266db4c6e6251131b741d6504a`, 9파일, CRC PASS, Manifest 7/7 및 SHA256SUMS 8/8 바이트/해시 일치.
- `Main13_Leon_PNG.zip` 154865 bytes, sha256 `8564b42547848f24117da7518b889f703aa8de02811c96577b1ae77ea9d6759d`, 1파일 CRC PASS.
- ChatGPT Library `/LIMITLESS_Art_Delivery/Issue109_Main13_Leon_20261117/`에 PNG·ZIP2·비교판 총4파일 저장 성공. 로컬 Windows F: 복사 및 Unity 적용 아님.

## 보호 및 승인
- FILE_TECH_PASS / USER_ART_REVIEW_PENDING / NOT_UNITY_APPLIED. 실게임 PPU/Pivot/명찰 및 배치는 미검증. 대표 미술 승인 후에만 Codex 별도 최소 Delta QA로 arbel-leon 매핑만 독립 전용 Sprite로 교체 검토.
- 기존 OGA09·Main04·Main07·세린·승인 NPC 원본, Scene/Quest/StableID/GUID/meta/Save/UserData/Discord 보호. Unity6000.6.5f1·Issue85 HOLD 유지. 초상화·애니메이션·추가 NPC·Unity 적용 0. main 직접 commit/Push/Merge 0.
