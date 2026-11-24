# 2026-11-24 · Issue #123·#124 신규 아트 통합 기술 검수

**스킬 버튼 기술 PASS / F4 배치 PARTIAL / 최종 미술 승인과 본편 적용 HOLD.** Unity 6000.6.5f1 호환 QA 복제본 `Temp/q124`를 재사용했습니다. 이전 #124 제품·방어 보고 기준 `97bcc020aba378b5d57056e108f1a9efe2372df0`. [F4 기획 지시](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7119016777) · [스킬 기획 지시](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7119017275)

## 실물 무결성

ZIP SHA256 두 건 일치, CRC PASS, 스킬 Manifest 9/9·SUMS 10/10, F4 Manifest 6/6·SUMS 7/7. 승인 공격·방어 참조 PNG는 제품 격리 브랜치의 실제 파일과 바이트 일치했습니다. 아래 세 Unity 후보는 각각 고유 GUID로 임포트됐고 Assets 내 GUID 소유자가 한 개씩입니다.

| PNG | 크기·알파 | SHA256 |
|---|---|---|
| SkillButton_Original_128_RGBA.png | 128×128 RGBA, 0–255 | `dd9b8a971751d597d97c216ae83b44db68b0caabaf0ea32503e8b13100baab00` |
| F4_WhiteConifer_384x512_RGBA_CANDIDATE.png | 384×512 RGBA, 0–255 | `398b07335d84b0702a54e1bfae109bce75c60679f8e6a6fda6ccad6cd4386fad` |
| F4_DirectionalSnowdrift_384x192_RGBA_CANDIDATE.png | 384×192 RGBA, 0–168 | `1ae4ae973495d7d5c09efa3c7b6604233e6d4e66e74ca4d2d0e275e3bef630b1` |

세 원본 모두 캔버스 네 변의 최대 알파 0.

## 기본 스킬 버튼

- 실제 Battle 씬에서 기존 Kenney 별과 후보를 같은 Image에 순서대로 표시했습니다. 후보 연결은 실행 중 QA 오브젝트에만 적용했습니다. 제품의 18px 기준 레이아웃을 보존하고 30/40px 배율에서 활성·비활성 화면을 렌더링했습니다.
- **PASS** · 공용 스킬 메뉴 열기, 기존 투사 스킬 항목 및 돌아가기, 닫은 후 버튼 복귀. 실제 공격 대상 선택 단계에서는 스킬 버튼이 비활성 상태이고 후보 Sprite는 표시됐습니다.
- **가독성 PASS** · 금속과 금색 테두리는 공격·방어와 같은 계열입니다. 30px에서도 중앙 별이 검·방패와 구분됩니다. Kenney 별이 더 단순하며, 후보의 30px 작은 장식은 덜 분명합니다. 투사의 난도·회심의 일격·회오리 베기 개별 아이콘은 기존 모양 그대로 표시됐습니다. 전체 직업과 길 능력 아이콘 전수 비교는 범위 밖입니다.
- 로컬 `Temp/i123124-review/Skill_Unity_30_40_Comparison.png`, `Skill_Menu_Open.png`, 납품 `Attack_Defend_Skill_128_40_30_Actual_Comparison.png`, `unity.log` 692–951행.

## F4 백색 침엽림

- **Import PASS** · 128 PPU, 침엽수 3×4 WU 및 하단 Pivot (192,10), 풍설 3×1.5 WU 및 중앙 Pivot (192,96). 침엽수 알파 경계는 하단 10px을 남겨 시험 접지선과 맞습니다. 시험 Sorting은 지면 -30, 풍설 -24, 수목 0. 풍설은 길 밖 단일 오버레이이며 해당 렌더러에 Collider를 두지 않았습니다. 실제 캐릭터 가림과 제품 Sorting은 미검증입니다.
- **비교 PASS** · 납품 1:1 비교판에서 F2 승인 그루터기, F3 눈·저상 암반·경계석과 눈의 청회색 음영이 이어집니다. 같은 128 PPU에서 침엽수 3×4 WU는 F2 그루터기 2.5×3 WU 및 F3 저상 암반 2.5×1.5 WU보다 높습니다. 풍설은 가로로 비스듬하며 눈 위에 얹는 반투명 표현입니다. 지형 위 실제 대비는 정식 씬에서 미검증입니다.
- **배치 PARTIAL** · Unity 시험 배치에서 수목과 풍설의 전체 Sprite bounds를 4.5 WU 통행로 밖으로 배치했습니다. 남쪽 F3 방향 입구와 북쪽 F5 방향 출구, 북진과 가로 풍설을 배치판에 각각 표시했습니다.
- **정식 이동 HOLD** · 기준 저장소의 북부 씬은 F1·서릿길마을·F2 후보입니다. F2의 서릿길마을 출발 스폰은 (0,-8,0)이며 후보 진입·복귀 코드는 확인했습니다. F3·F4·F5 씬 및 실제 F3→F4→F5 전환 좌표는 확인되지 않았습니다. Main25 F3, Main26 F4 및 이후 F5 정본과 대표 북진 지시를 적용했습니다. 도보·휠체어 실주행, 실제 Collider와 출입구는 미검증입니다.
- 로컬 `Temp/i123124-review/F4_Northbound_Unity_Review.png`, `F4_Unity_Pilot_Raw.png`, 납품 `F4_F2_F3_Actual_1to1_30_40_Comparison.png`. 검수 렌더는 조명에서 원화가 어두워지는 문제를 피하기 위해 QA 카메라용 Unlit 재질로만 출력했습니다. 씬 저장은 없었습니다.

제품 씬·코드·에셋, 승인 원화, #85 후보, 원본 Save/UserData 및 main을 보호했습니다. 제품 클린 Import·실제 F4 이동·미술 승인 및 본편 적용은 HOLD입니다. 제품 커밋·main 병합·원격 Push 없음.

## 검수 화면 무결성

- F4_Northbound_Unity_Review.png · SHA256 5c5c9de84ee4a598d2367fbaed9101cf191306fedd62df7e6ec223b7e68e4f40
- Skill_Unity_30_40_Comparison.png · SHA256 8e5cf771304363c896cde08e5ecb88994ad805bb1ce3905ec5ff7747c30712ba
