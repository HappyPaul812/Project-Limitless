# Field09 v2 납품 독립 검증 — 기술 PASS / 시각 FAIL

2026-10-20. `codex/field09-v2-delta-qa-20261020`. [Issue #29](https://github.com/HappyPaul812/Project-Limitless/issues/29) 및 `docs/art-renewal` 최신 v2 납품 문서를 직접 조회했습니다. 납품 커밋 `4babb98b1c90bbd001dda1cff4be46097d02cfc7`의 ZIP을 GitHub raw에서 직접 다운로드했습니다.

## 납품 파일 검사

- ZIP 2,659,837 bytes / SHA256 `30f102c223aa127a7b6a290ef9fa503354cb9e0104faac03e6c395c2b247b8b8`: 납품값 일치.
- Python `zipfile.testzip()==None`, 14 entries 실제 해제. .NET ZipFile 및 Windows Explorer 압축폴더 엔진 `Shell.Application.CopyHere`도 창·진행창·포커스 전환 없이 해제했고, 14파일 전체 SHA256 일치. Explorer 창을 열어 수동 확인한 것은 아닙니다.
- PNG8 RGBA 512×512 디코딩·크기·CRC·후보 SHA·원본 공식 SHA·변경 픽셀 수·bbox가 Manifest와 모두 일치. 원본 AshPatch/Cracked02 및 meta/GUID 수정0. v1 단순 재포장은 아닙니다.

## 실제 Unity 검사

기존 격리 Client와 v1 검증 Scene·동일 RenderTexture/Camera/Scale을 재사용했습니다. Unity 6000.6.5f1 Batch 숨김 실행, 원본 Editor는 Bootstrap/Play OFF/비포커스 상태를 읽기 조회만 했습니다. 원본 Editor·GameView·OS 입력 조작0.

최종 **40 PASS / 0 FAIL**: PNG8 실제 Sprite 연결, PPU128·중앙 Pivot·Point·NoMip, 환경9, Collider 동일, 1280×720·1600×900·1920×1080 전체/접합 화면 및 소품6 확대 렌더. 신규 Console 오류0, 기존 Search/URP 환경 진단은 별도 유지. 컴파일 오류0.

첫 시도는 재사용 Client의 Scene이 후속 QA에서 공식 Sprite 참조로 변경돼 후보 연결 assertion에서 중단됐습니다. 격리 Scene을 기존 v1 검증본으로 복구한 후 최종 실행은 정상 종료했습니다. 첫 로그도 보존했습니다. 제품 코드 수정으로 통과시키지 않았습니다.

## 시각 불합격 근거

**지면 반복선6곳과 가이드6종 모두 제거 완료로 인정할 수 없습니다.** v1의 넓은 회색띠는 v2에서 가늘어졌지만 직선 격자와 대칭 무늬가 남습니다. GroundBase/Cracked01 이종 접합도 x=-1.5 부근에서 수직 경계가 보입니다. 외곽 RGBA seam0은 실제 화면 합격을 보장하지 않습니다.

`World_1280.png` 대표 검토 bbox: 세로 `(238,0,248,720)`, `(483,0,493,720)`, `(975,0,985,720)`; 가로 `(0,83,1280,93)`, `(0,328,1280,338)`, `(0,574,1280,584)`. 위쪽 원점0, 끝 좌표 제외. 반복선 전체 개수를 새로 한정하는 목록이 아니라 기존 실패 영역을 재검토하는 대표6곳입니다.

| PNG | 잔여 가이드 검토 영역 (512×512 위쪽 원점0) |
|---|---|
| Cliff Blocker | y456~457 하단 / x16~19 왼쪽 |
| Fissure Edge | y31~39 상단 점선 / x491~495 오른쪽 / y477~480 하단 |
| Fissure Glow | y36~42 상단 점선 / x489~495 오른쪽 / y468~475 하단 |
| HeatVent | y22~25 상단 점선 |
| Obsidian Slab | y457~459 하단 / x491~495 오른쪽 |
| Obsidian Spire | y21~24 상단 / y488~490 하단 |

표는 국소 검토 bbox이며 삭제용 mask가 아닙니다. 실제 잔여 RGBA 표본은 Result.json에 있습니다. Glow와 Edge의 확대 카메라는 배치 중심이 같고 서로 겹쳐 보이므로 각 PNG 원본 픽셀도 함께 직접 확인했습니다. 바위 하이라이트 전체 제거를 요구하는 판정이 아닙니다. 후보 PNG에 추가 리터칭/재생성0.

## 기능·원본 보호

기존 Main18 **92 PASS** 및 이전 Import40 결과는 역사 증거로 유지하며 기능 전수 재실행0. 이번 새40은 변경 PNG Import·연결·렌더에 한정합니다. 원본과 격리 Runtime C#164개는 BOM/줄바꿈만 정규화하면 차이0입니다. 새 Quest/Heat/Save 로직 변경0이며 새 기능92PASS를 주장하지 않습니다.

원본 프로젝트 전용 Assets·UserData·Settings·Packages **3,546파일 SHA256 변경0**. 공식 아트·Main18·음성·Save 보호. 사용자 작업 트리 되돌리기·원본 Scene 적용·Refresh·Push·PR·main 병합0. 보고서와 CURRENT_STATUS는 전용 격리 브랜치에만 기록합니다.

**결론: ZIP_INDEPENDENT_VALIDATED / UNITY_IMPORT_DELTA_PASS / UNITY_VISUAL_FAIL / ORIGINAL_NOT_APPLIED.** 시각 FAIL이 남아 승인 요청 단계로 넘기지 않습니다. 다음 작업은 리뉴얼 담당의 해당 bbox 국소 수정본 납품 후 실패 영역만 재검증입니다. 합격 후에도 원본 Unity 적용과 main 병합은 바울 대표의 별도 승인이 필요합니다. 외부 댓글/메시지 전송0.

[v1/v2 실제 화면 비교](Evidence/Field09V2_20261020/index.html) · [검사 결과·잔여 bbox](Evidence/Field09V2_20261020/Result.json) · [ZIP 독립 검사](Evidence/Field09V2_20261020/ArchiveValidation.json) · [원본 보호](Evidence/Field09V2_20261020/ProtectionFinal.json)
