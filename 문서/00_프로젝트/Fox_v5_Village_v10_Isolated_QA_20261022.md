| 모듈 | 기술 / 메타데이터 | 미술·한 줄 사유 |
|---|---|---|
| Fox Run6 v5 | **기술 FAIL / CSV 미검증** | **HOLD(미검증·판정 유보)** — Manifest 자신의 SHA·크기·CRC 불일치. Unity/미술 단계 진입하지 않음 |
| Village v10 | **기술 PASS** | **Gravel 01·02·03 각각 PASS 권고** — 동일 배치에서 자갈 미세 격자 완화, 큰 사각 구획 재악화는 뚜렷하지 않음. 사용자 최종 승인 대기 |

# Fox v5 / Village v10 독립 격리 QA

[대표 확인용 비교 자료](Evidence/IsolatedArtV5QA20261022/index.html) · [실제 Unity 5×5 비교](Evidence/IsolatedArtV5QA20261022/Compare_Gravel_5x5.png) · [8×8 비교](Evidence/IsolatedArtV5QA20261022/Compare_Gravel_8x8.png)

## 1. ZIP 실제 SHA256 및 무결성

| ZIP | 실제 SHA256 | ZIP CRC | Manifest | SHA256SUMS |
|---|---|---|---|---|
| Fox v5 / 3,131,578bytes | `5f6da110fe64f280528d9768bc9154d4175c0724f8541d72aa9ebbecf1cd44df` | PASS | **37/38 일치·자기 참조 1건 FAIL** | 38/38 PASS |
| Village v10 / 11,472,703bytes | `f50e312eea52a6bca8272d78532224e2c66a6b40c4b1c8be5cd164b52b2bcee5` | PASS | Gravel 지정 해시 **3/3 PASS** | 64/64 PASS |

신규 ZIP의 외부 기대 SHA는 이번 지시에 없으므로 실측값을 기록했습니다. 이전 버전 출처 SHA는 Manifest와 기존 검수 기록이 일치합니다. Fox의 최상위 폴더 `Limitless_Beast_Fox_Run6_v5/`는 논리 경로만 정규화했으며 ZIP·원본 바이트는 변경하지 않았습니다. Village Manifest는 전체58 PNG가 아닌 새 Gravel3만 해시로 지정합니다. 나머지 파일 무결성은 SHA256SUMS와 이전 버전별 실측 대조를 따릅니다. [전체 파일별 결과](Evidence/IsolatedArtV5QA20261022/PackageQA.json).

## 2. Fox — 무결성 게이트에서 중단

`Manifest.json`의 `file_hashes["Manifest.json"]`에 기록된 값이 실제 같은 파일과 다릅니다.

| 항목 | Manifest 기대값 | 실제 파일 |
|---|---|---|
| SHA256 | `376fee362f408da91366cfec88dc5bf1b30489db5127fc1835de829843e6f176` | `7b7e17c241864b4694081675713cd6adf2e3ee2b74ecb3938decf39fc02bfc3c` |
| bytes | 9,184 | 9,345 |
| CRC32 | `7540cc8e` | `8ce870ab` |

ZIP CRC 및 SHA256SUMS 통과가 이 Manifest 불일치를 상쇄하지 않습니다. [원문 Manifest](Evidence/IsolatedArtV5QA20261022/Fox_Manifest.json) / [정확한 실패 근거](Evidence/IsolatedArtV5QA20261022/Fox_IntegrityFailure.json).

ZIP 파일명 목록에는 PNG6·Strip·Atlas·GIF/APNG·128/40px Preview·bbox CSV·좌표 계약이 존재합니다. **파일 존재 확인과 파일별 무결성 검사만 수행**했습니다. 프레임 디코딩/alpha/Strip·Atlas 셀 일치, Frame03 변경·00/01/02/04/05 보존, bbox CSV 헤더/좌표계/열 순서, 02→03→04/05→00, 실제 Unity 12FPS·128/40px와 미술 검수는 **무결성 실패 후 미실행**입니다. Metadata FAIL이나 Frame03 미술 FAIL로 확대하지 않습니다. Fox 모션 자료는 새로 만들지 않았습니다.

**담당자 보완:** Manifest 해시 목록에서 자기 자신을 제외하고 최종 Manifest를 확정한 뒤 SHA256SUMS→ZIP을 재생성하십시오. PNG를 바꾸거나 원본을 덮어쓸 필요가 있다는 근거는 없습니다. 보정 ZIP의 실제 SHA/CRC/Manifest/SUMS 게이트 통과 후에만 Fox 후속 CSV·Unity·Frame03 미술을 재개합니다. 당 검수에서 ZIP 수리·PNG 재생성은 하지 않았습니다.

## 3. Village — 변경 범위와 기술 PASS

- 기존 게임 PNG **49/49** 및 그 안의 원본43 모두 v9와 바이트 동일. 기존 SkillShop/RoofGable/StonePath를 포함합니다.
- Earth·ThinFrost 후보 **6/6** SHA·바이트 동일. 이전 PASS 권고를 재사용하며 신규 독립 미술 QA로 계산하지 않습니다.
- 후보 이미지 변경은 **Gravel01/02/03 세 개뿐**입니다. 이름의 `_v9_`가 `_v10_`으로 바뀌었습니다. Preview·Manifest·QA 등 부속 문서/미리보기 구성도 달라졌으므로 ZIP 전체가 단 3파일만 변경됐다고 표현하지 않습니다.
- Gravel3: **256×256 RGBA, alpha 전부255**. 좌우/상하 ordered 3×3 조합 **18경계 모두 정확히 일치**합니다.
- 기존 v9 ZIP 전체를 재검수하지 않고 이전 검수에서 추출·해시 기록한 원본 폴더와 대조했습니다. 관련49+9 PNG의 저장된 v9 SHA를 확인했습니다.

| 후보 | SHA256 | 미술 판정·관찰 |
|---|---|---|
| Gravel01 | `8a36b719f2411f1d590e8ff9dd51ac589836ca4824d4f8ca43d1eb0fda1f839b` | **PASS 권고** — 작은 자갈의 고른 줄·간격이 약해지고 밀도 변화가 자연스러워짐 |
| Gravel02 | `3fc485cf3f6ebf1a692e2c06d23b95044dafa071baa225dd1bf127b5a580cb99` | **PASS 권고** — 인접01·03과 입자/명암이 조화되고 눈에 띄는 사각 명암 구획 없음 |
| Gravel03 | `58f171d88a1974e6ad011400e7e7f9d205fd8d39c4b70a3ce704715f4c16e5b8` | **PASS 권고** — 자갈 군집이 이전 미세 격자를 덜 강조하며 family 안 질감 편차가 과하지 않음 |

**Gravel family 전체 PASS 권고.** 동일3종이므로 반복 자체가 없어졌다는 뜻은 아닙니다. 현재 실제128px 셀 표시에서 이전 미세 격자가 눈에 띄게 줄었고, 큰 사각 구획의 뚜렷한 재악화는 관찰하지 못했습니다. 따뜻한 갈색 계열과 Earth/ThinFrost 옆 색감 조화도 유지됩니다. 경계 픽셀 PASS와 별도로 위 실제 화면을 보고 미술 판정했습니다.

Earth·ThinFrost 이전 PASS 권고6종을 합쳐 **후보9종 전체를 대표 최종 승인 대상으로 올릴 수 있습니다.** 아직 최종 승인·공식 타일 승격·실제 제품 교체는 아닙니다. OPEN16이 남은 기존 마을 게임아트49종 전체 승인을 뜻하지 않습니다.

## 4. 실제 Unity 자료·범위

Unity **6000.6.5f1 / Direct3D12 / 숨김 Batch 실제 Play Mode**. Engine frame **1→3**, realtime **0.908→2.536초**로 시간 진행을 기록했습니다. Camera→RenderTexture PNG **22장**이며 **Editor GameView UI 캡처가 아닙니다.** 원본 Editor 조작·foreground·키보드/마우스 조작은 0입니다.

PPU128·Point·압축없음·Mip없음·Pivot 하단중앙, 256px 타일을 화면128px로 표시했습니다. 카메라 ortho8.4375/높이1080을 고정하여 1920×1080(16:9), 1440×1080(4:3), 2520×1080(21:9)로 비교했습니다. 각5×5/8×8에서 `System.Random(602210+n)`의 같은 Tile ID/좌표를 v9·v10에 사용했습니다.

| 자료 | 링크 |
|---|---|
| 동일5×5 / 좌v9·우v10 | [비교](Evidence/IsolatedArtV5QA20261022/Compare_Gravel_5x5.png) |
| 동일8×8 / 좌v9·우v10 | [비교](Evidence/IsolatedArtV5QA20261022/Compare_Gravel_8x8.png) |
| v10 8×8 세 화면비 | [16:9](Evidence/IsolatedArtV5QA20261022/Gravel_v10_Mix8x8_16x9.png) · [4:3](Evidence/IsolatedArtV5QA20261022/Gravel_v10_Mix8x8_4x3.png) · [21:9](Evidence/IsolatedArtV5QA20261022/Gravel_v10_Mix8x8_21x9.png) |
| 건물·길·세 지면 색감 문맥 | [전체](Evidence/IsolatedArtV5QA20261022/Village_Connections_16x9.png) |
| 휠체어 문 앞/뒤 Sorting proxy | [앞](Evidence/IsolatedArtV5QA20261022/Village_DoorFront.png) · [뒤](Evidence/IsolatedArtV5QA20261022/Village_DoorBehind.png) |

검수용 동일 건물/길/Collider/Sorting proxy에서 v9·v10 통행 **각6/6**, 문 앞/뒤 캡처는 픽셀 동일입니다. 지면만 변경했고 proxy geometry 변경은 없습니다. **실제 본편 Village 전체 통행·모듈식 문 모든 조합·퀘스트·Main23 구현을 검증한 것으로 확대하지 않습니다.** 이전49 PNG 보존과 이번 동일 문맥 자료에서 새 회귀를 관찰하지 못했습니다.

Import 설정 **68/68 PASS**, C# 컴파일 오류0·NullReference0·MissingReference0·완료 마커 확인. 기존 **Editor SearchDatabase 초기화 ArgumentOutOfRangeException 1건 OPEN**입니다. Console 전체 오류0으로 표현하지 않습니다. 제품 자산·코드·Scene에는 쓰지 않았습니다. [Unity 요약](Evidence/IsolatedArtV5QA20261022/UnitySummary.json).

## 5. OPEN16

**유지16 / 완화0 / 해결0, Gravel 변경과 무관16.** 원문 CSV SHA `1ede22bc757a459120268021f97fcd5387da0b94db852d7da7e92c57c5a22525`, 좌표 JSON SHA `ad8f028f2afb4079853eac8b5f5201803a6e29cd7d1f0f6b85e155f52f64f48f`를 재확인했습니다. 항목별 파일·원문 이유·art/use·좌상단/max-exclusive 좌표·재현조건·v10 SHA를 [인계 CSV](Evidence/IsolatedArtV5QA20261022/OPEN16_v10_Handoff.csv)와 [JSON](Evidence/IsolatedArtV5QA20261022/OPEN16.json)에 보존했습니다.

GeneralShop, StoneArch/StoneFrame/WoodFrame, Roof_Shingle_B, Wall_Plain, Portalsite_LowWall/StonePlinth, Bench/Fence, SupplyBox_Food/Brace/Small/Wood/Sacks, Washline의 기존 source fragment 대상16 모두 v9와 바이트 동일합니다. 새 Gravel의 반복 완화로 이 원화 결함을 해결 처리하지 않습니다.

## 6. 보호·Git·다음 단계

원본 Unity 관련 **4,402파일 SHA 전부 동일**, 사용자 dirty/untracked **862파일 SHA 전부 동일**, 원본 `main` HEAD **`510c021b4159167e211be60ca90e1cb7328bafe9`**, status/index 바이트 동일. 기존 Bear 적용 파일을 포함한 시작 상태를 그대로 보호했으며 Bear/Wolf 재생·재검수·재적용은 0입니다. Save/UserData·원본 Editor·포커스·제품 쓰기0, Main22/F1/서리들쥐/기존 main 동기화 재검증0. [보호 증거](Evidence/IsolatedArtV5QA20261022/OriginalProtection.json).

작업 브랜치 `codex/qa-village-v10-fox-v5-20261022`. 문서·QA 증거만 로컬 커밋하며 Push/PR/main 직접 commit/merge는 하지 않습니다. 원본 CURRENT_STATUS/사용자 문서에는 쓰지 않고 별도 worktree의 상태 문서만 갱신합니다.

근거는 사용자 최신 지시, ZIP 동봉 보고서/Manifest·실측, 이전v9/Foxv4 실측 자료, 읽기 전용 조회한 GitHub [기획 인계](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/팀운영/인계/PLANNING_LATEST.md), [리뉴얼 인계](https://github.com/HappyPaul812/Project-Limitless/blob/docs/art-renewal/문서/00_프로젝트/팀운영/인계/WORLD_ART_RENEWAL_LATEST.md), [신규아트 인계](https://github.com/HappyPaul812/Project-Limitless/blob/docs/art-new/문서/00_프로젝트/팀운영/인계/SPRITE_ART_LATEST.md)입니다. 조회한 원격 인계는 Foxv4/Villagev9 기록 중심이며 신규 납품 ZIP의 실증 결과와 혼동하지 않습니다.

다음은 **Fox 패키징 정정본 후 무결성→CSV→실제 Unity→Frame03 미술 재개**, **Village 후보9종 대표 최종 미술 승인**입니다. Village 승인 전 제품 적용은 계속 금지이며 OPEN16 보완은 별도 표적 작업으로 남깁니다.

QA 증거·보고 로컬 commit: `5a51296e0fda076cda7772d7a5e2806481cd9ae6`. 후속 CURRENT_STATUS·원문 줄바꿈 보존 설정 커밋은 최종 보고에 기재합니다.
