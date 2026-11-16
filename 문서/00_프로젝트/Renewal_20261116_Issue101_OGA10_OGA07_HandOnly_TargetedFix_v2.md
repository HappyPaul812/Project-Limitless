> 2026-11-16 EOD 선별 인계: 출처 `origin/docs/art-renewal`, Git blob `785333a31f324e064169aa02bddf69c15b67f416`. 이하 보고의 승인/미적용/PENDING 표시는 작성 당시 상태입니다. 최신 결재는 EOD 보고 우선: Stoat KeyPose·Idle4·Attack4v2·Hit4 미술 승인/Unity미적용, NPC02/17·10/07 로컬적용, Issue85 Import/GUID HOLD. NPC 손 한정 지시는 기획 해석 오류였으며 최신 정정대로 인계합니다. 기존 제품·미완료 자산을 새로 적용하지 않습니다.

# LIMITLESS / 리뉴얼 아트 / Issue #101 / 2026-11-16 손 부분만 최소 보정 v2

## 요청·범위
- **바울 대표:** OGA10 파티 관리인과 OGA07 주민1의 기존 디자인은 전반적으로 만족하며 필요하면 **손 부분만** 최소 보정. 기존 얼굴·의상·색감·몸통·실루엣·포즈는 보호. 애니메이션/초상화/추가 NPC/Unity 적용 금지.
- 기존 [Issue #101](https://github.com/HappyPaul812/Project-Limitless/issues/101) 512 RGBA 원본 **각각 별도 보존**. 2개 기존 시안을 재생성해 전면 변경하지 않고 손의 손가락 경계/주먹 마디 표현만 국소 수정했습니다.
- 편집 방식: 이미지 생성 도구로 손 형태를 검토했지만 재생성 결과는 신체 다른 부분까지 바뀌므로 **최종 파일에 전신 재생성 결과를 채택하지 않았습니다.** 기존 원본의 손 내부에만 미세 선/마디를 추가하는 국소 RGBA 오버레이 작업. 손 위치·피부색 마스크 범위로 엄격하게 제한했습니다.

## 수정된 실제 파일 (신규 NPC 캐릭터 추가0 · 기존 NPC 2종 수정 후보)
| NPC / StableID | 원본 SHA256 (v1) | 수정본 파일 / SHA256 | 변화 픽셀 / 512² | 손 외 변화 |
|---|---|---|---|---|
| OGA10 / `starter-village-party-manager` | `7a53cbb71544b69e996a06c7b576c9498d0565cc701a9123a6ad9485cd1d18a1` | `Candidates_512/OGA10_PartyManager_HandFix_v2_512_RGBA.png` / **`6be1a2d777a312dedd392b8ad6fbb2ba77b350d46d7bd2113d9d11bf09c7e6f3`** | 178 (0.0679%) | 0 |
| OGA07 / `starter-village-resident-01` | `940aa98590333b1423ed5f49101c9cf5a146a0079c8e354fc33e0ce943dff798` | `Candidates_512/OGA07_Resident01_HandFix_v2_512_RGBA.png` / **`640b82a236afd5ca6025d5eb28970009f40941d39eadedeeeea0e180d5034d28`** | 127 (0.0484%) | 0 |

- OGA10 변화 영역 총 bbox `(152,232)-(366,329)`는 펼친 손/반대편 손 **두 곳의 합집합**일 뿐 이 사각형 전체 수정이 아님. 피부 마스크로 제한한 총 178 픽셀만 바뀜.
- OGA07 총 bbox `(179,263)-(304,329)`도 잡은 손/반대 손 **두 곳의 합집합**일 뿐 사각형 전체 수정 아님. 127 픽셀만 바뀜.
- 원본 512×512 크기, RGBA, bbox/캔버스 4면의 alpha 접촉 수 모두 이전과 동일. 눈, 얼굴, 옷, 자세, 발·소품·기타 그래픽의 **원본 RGBA 픽셀 동일**. 이전 파일 해시와 변경 파일 해시를 별도 보존.
- 수정 세부: OGA10 환영 손가락의 분리선·주먹 마디 아주 약하게 정리. OGA07 천 잡은 손가락·내려놓은 주먹 마디 아주 약하게 정리. 큰 실루엣·손 크기·관절 위치 변화 없음. **40px에서는 수정의 차이가 작을 수 있음**.

## 파일·실측 검수
- **ZIP 실제 경로 (ChatGPT 실행 컨테이너)** `/mnt/data/Limitless_NPC_101_HandFix_v2.zip`. 사용자 Windows `F:\Downloads`로 자동 복사·Unity 적용하지 않았습니다.
- ZIP **695127 bytes**, **SHA256 `acbc523c6de831bc0d6b6556c36cd28cce297858218e38ca61e5414387063a64`**. **13 entries, ZIP CRC PASS, Manifest 11/11 바이트·SHA256 PASS**.
- 포함: 후보 마스터 PNG2, 개별 실제 크기 128px·40px LANCZOS 프리뷰4, `Review_OFFLINE/Issue101_v2_Hands_BeforeAfter_512_OFFLINE.png` 전후/손 4배 확대 비교판1, `Review_OFFLINE/Issue101_v2_128_40_ActualScale_OFFLINE.png` 실제128/40 크기 비교판1, `SourceDeclaration.md`, `README.md`, `Issue101_v2_QA.json`, `Manifest.json`, `SHA256SUMS.txt` 총13파일.
- QA 보고에는 원본/수정본 SHA, 변경영역 픽셀 count·bbox, 손 허용 영역 밖 픽셀 변화0, 바깥 alpha 접촉0을 기록. 비교판은 **오프라인**, Unity GameView로 주장하지 않습니다.

## 기존 승인품·미술/기술 게이트
- 기승인 **OGA02/03/06/09/16/17** 아트·게임 반영본 변경0. **OGA20 남문경비 태온 유사성 HOLD** 유지. OGA11/13/19 주민 신규 제작0.
- 원본 Unity6000.6.5f1/Scene·Prefab·기존 GUID/meta/StableID/Quest/Collider/Sorting/Save/UserData/Discord/기존 몬스터·펫·환경·`main` **변경0**. Issue85 Import/Push HOLD 유지, 새 대형 QA 복제0.
- 신규 512 수정 시안이므로 **`HAND_ONLY_V2_TARGETED / FILE_TECH_PASS / USER_ART_APPROVAL_PENDING / NOT_UNITY_APPLIED`**. 바울 대표 미술 승인 전 Unity Import 및 Codex 적용 금지. 승인 후 기존 32px·PPU28 실제 캐릭터 월드스케일과 발접지·이름표를 Codex가 최소 DeltaQA로 확인해야 함. 512 마스터를 그대로 PPU28로 가져오지 말 것.
- `docs/art-renewal` 문서 보고만 갱신. `main` 직접 commit/Push/Merge0, 바이너리 GitHub 업로드0.
