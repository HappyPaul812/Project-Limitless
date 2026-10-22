# LIMITLESS / Chapter3 Main23 / 서릿길 마을 VariantTiles v9 — 비파괴 대체 family 파일럿

**결과**: `CANDIDATES_DELIVERED / OFFLINE_TECH_QA_PASS / ART_HOLD_REVIEW_PENDING / NOT_UNITY_INTEGRATED`

## 파일·보존

- 실물: `Limitless_Chapter3_FrostwayVillage_Environment_GameArt_VariantTiles_v9.zip` / SHA256 `afea1ed0b357a5bdce9e962c7969b81b77d29e1ed11b7b9dc170d57747fbeb79` / 21,738,388 bytes / ZIP 73 entries / CRC PASS.
- 소스: v8 ZIP SHA256 `6a9fa1c3c0c1ffef3ac43d3541a28bbc0a16310d2e0f8c267ec7177d731d712f` 실물 확인.
- v8 **공식 후보 기존43 PNG byte-for-byte 보존**, 기존 변형02/03 6 PNG도 동일. 총 v8 게임PNG 49/49 원래 경로·원래 bytes·SHA 유지. `Candidate/Tiles/`에만 신규 9 PNG 추가.
- 기존 01/SkillShop/Roof Gable/StonePath/건물·포탈·표식·F1/Wolf/Mouse 및 Unity Scene/Save/UserData/GUID/meta 변경0. v9 후보를 기존 공식 PNG에 덮어쓴 적 없음. GitHub에는 QA Markdown과 인계만 기록, PNG·ZIP 미업로드.
- OPEN16: 담당 아트에 원본 CSV 16행+XY/조건이 전달되지 않아 **미해결16·완화0·해결0**. 기존 다른 자산 수정 없음.

## 9개 후보 SHA256

| family | 후보 | SHA256 |
|---|---|---|
| Earth | `Candidate/Tiles/Frostway_Ground_Earth_01_v9_CANDIDATE.png` | `70cc14876d7da7a4cc1ffd616ff62971b1b181cd1c6f9d64287a54af3afd8dd6` |
| Earth | `Candidate/Tiles/Frostway_Ground_Earth_02_v9_CANDIDATE.png` | `bd82fcba2294b2e67abce52d8aeb7de0e15be5421a432fece130d4979238f51b` |
| Earth | `Candidate/Tiles/Frostway_Ground_Earth_03_v9_CANDIDATE.png` | `74cbc2d75a4ba92a7d497790aaf75cc69228939edeb66775b249e0df9d3c0e4a` |
| ThinFrost | `Candidate/Tiles/Frostway_Ground_ThinFrost_01_v9_CANDIDATE.png` | `5fbce4d42edb70a9d54317245f8d3a42f1b2aa1b839aea8ebdeb13cc28acaa80` |
| ThinFrost | `Candidate/Tiles/Frostway_Ground_ThinFrost_02_v9_CANDIDATE.png` | `0420859875968b353f46ab481a35c6b546f4424f86f6e8652a04c35694f90038` |
| ThinFrost | `Candidate/Tiles/Frostway_Ground_ThinFrost_03_v9_CANDIDATE.png` | `3b8276c79ec79d78e3c46e8e52a84e54802f4b720c27649b9c947fc0c453308d` |
| Gravel | `Candidate/Tiles/Frostway_Ground_Gravel_01_v9_CANDIDATE.png` | `9bcc96b4f61b5383ac78bb836969e28e75e307d94bd2ef7ab70d502ef7497b25` |
| Gravel | `Candidate/Tiles/Frostway_Ground_Gravel_02_v9_CANDIDATE.png` | `cca046c079551a3a92ad3423d75dc061cd01e1d22ee8bd432fdd5839fa2a06ee` |
| Gravel | `Candidate/Tiles/Frostway_Ground_Gravel_03_v9_CANDIDATE.png` | `c6f8aac658c99b98727d52d9f623687e6cb4f4b42cd32982fe68ba32b1b9ce83` |

## 미술 방향·01 고정 원인 점검

- v8 고정01은 풀·돌·자갈 큰 모티프가 한 위치에 반복되고 02/03은 작은 입자·다른 텍스처로 이어져, 01↔02/03 사각 타일 구획이 눈에 띄었다.
- v9은 Earth/ThinFrost/Gravel 각각 **대체01·02·03을 모두 신규 후보**로 구성. 기존 v8 01의 채색·질감/스몰모티프를 참고한 동일한 소스 기반 텍스처 퀼팅, 움직인 자연물 소품, 동일한 톤 관리·바탕 입도로 세 변형을 함께 설계. 새 공통 seamless soil border를 합성하여 v8의 원본01 1px 테두리 자체도 재사용하지 않는다.
- v8/v9 정확히 같은 타일ID/배율 비교에서 기존01 고유 대형 실루엣에 따른 구획이 완화되는 방향이지만, 새9종의 배경질감·모티프 위치도 함께 수정했으므로 **'원본01 보존이 유력한 기여 원인'이며 단독 인과관계가 증명된 것은 아님**.
- 이 후보는 **알고리즘 기반 소스 텍스처 재구성 미술 파일럿**. 실제 Unity·수작업 최종 픽셀 마감·대표 미술 승인 없음. 큰 돌·풀 반복은 줄어도 **작은 텍스처 패턴 또는 Gravel의 미세 격자감**은 남아 보일 수 있으므로 미술 HOLD 유지. ThinFrost 서리 농도 또한 실제 화면 가독성 최종 확인 필요.

## 오프라인 기술 검증

- 신작 9개 모두 256×256 RGBA, alpha255, 독립 파일. Decode 9/9 PASS.
- family별 3×3 순서 있는 수평9·수직9 = **18/family, 총54 인접 조합 54/54 max RGBA edge difference 0**. 경계 픽셀 합격과 실제 타일 사각 구획 제거는 다름.
- ZIP `SHA256SUMS.txt` 72/72 검증, ZIP CRC PASS, v8 원본 게임PNG 49/49 SHA 일치.
- 전후 비교: `Preview/{Earth,ThinFrost,Gravel}_V8_vs_V9_{5x5,8x8}_SameLayout_128pxCell_OFFLINE.png` 총6. **동일 배열ID / 동일 셀128px / 동일 LANCZOS / 왼쪽 v8·오른쪽 v9**. 사진/게임 캡처가 아닌 PIL 오프라인 비교.
- `QA/Offline_QA_v9.json`: 54개 테스트/49원본 SHA/비교 배열·크기·보호 등 원본 증거. `Manifest.json`, `Manifest.md`, `SHA256SUMS.txt` 동봉.

## 승인·기술 게이트

1. 바울 대표 v9 원본 후보9·동일5×5/8×8 비교 미술 검토. 별도 승인 전 후보를 공식 타일로 **승격 금지**.
2. Codex는 후속 명시 지시 후 격리 Unity에서 16:9/4:3/21:9, 5×5/8×8 동배율, TileGrid/Sorting/PPU/Collider/휠체어 동선 및 OPEN16 근거를 변경분만 확인. 기존 승인 QA 전수 재실행 금지.
3. main 직접 commit/Push/merge 금지; Main23 본편/원본 Scene/Save 자동 적용0.
