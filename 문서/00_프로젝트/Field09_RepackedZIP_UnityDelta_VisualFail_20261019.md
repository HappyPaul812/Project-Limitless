# Field09 ZIP 무손실 재포장 및 Unity 보완본 단독 검수 (2026-10-19)

**ZIP_REPACK_PASS / PYTHON_DOTNET_EXPLORER_EXTRACTION_PASS / UNITY_IMPORT40_PASS / MAIN18_DELTA92_PASS / VISUAL_FAIL / ORIGINAL_APPLY_BLOCKED**.

브랜치 `codex/field09-minimal-fix-unity-integration-20261019`, 이전 포맷 진단 `008d32f18e96c4f04b53edb6a0993b2b29958332`에서 이어서 수행. 재포장 커밋 `e2abbe754bf457f15f7151fff2bd891c3ea98031`, 격리 후보 연결 커밋 `ea98ec1916adfee967209f6f50f06a74dcb4688d`. 최신 기획 `Field09_Zip_Header_Repair_And_MonsterWolf_Chapter2_Quality_Gate_20261019.md`와 기존 조건부 통합 정본을 적용했습니다. 몬스터·Wolf 제작 및 Main10/Field06/07 재작업0.

## 1. ZIP 복구 완료 — PNG·내부 문서 바이트 변경0

원래 손상 ZIP(7dfa2417a8135e29e3e72c7d7921d3ade76fbedfdb376f764c09b02892296139) 보존. 혼합Endian 구조의13 payload를 검증해 추출했고 PNG8은 Manifest fixedSHA와 전부 동일합니다. 원본공식8 SHA도 그대로 일치했습니다. 표준 Python ZIP_STORED writer로 새파일을 작성했으며 그림·Manifest/CSV/README/Codex안내 포함13개 payload의 **전체바이트를 유지**했습니다. 헤더수기패치·PNG재생성·리터칭·API0.

- 새파일: `F:/Downloads/Limitless_Field09_ObsidianScar_Minimal_Fix_v1_repacked.zip`
- 새 SHA256: **`6d3d866e75c696d451919d80789a89f12b429ac72f22d8e3f245a91bfc43a012`**
- 크기: **2,995,422 bytes**, 13 entries, PNG8(512×512 RGBA).
- Python zipfile.testzip+실제extract13, .NET ZipFile.ExtractToDirectory13, **Windows Explorer 압축폴더 Shell.Application.CopyHere13** 모두실행. 각결과13파일 SHA/CRC를표준중앙디렉터리와대조·PNG8 Manifest일치. Explorer창/진행창/포커스전환 없이 동일압축폴더엔진 사용. Expand-Archive를Explorer검증으로대체하지않았습니다.
- 재개용표준ZIP은 이branch `Art_Deliveries/Limitless_Field09_ObsidianScar_Minimal_Fix_v1_repacked.zip`에도 보존했습니다. 원래손상archive및원본공식 PNG/meta/GUID 변경0.

## 2. 새 Asset8만 격리 연결·동일 카메라 실제 Unity 화면

새폴더 `_Project/Art/Environment/Chapter2/ObsidianScarMinimalFix_v1/` PNG8+새GUIDmeta8+폴더meta1. Scene은 **8개Sprite GUID참조만** 변경, 이외문자열동일 검증. 원본9Renderer수·Transform·Scale·Tint·Sorting·Tiled21×15/9×15·Collider/Bounds 유지. 기존 Resources/Main18/Environment 원본10과meta는불변, Runtime직접원본경로요구코드없음(다른필드Ash재사용은보호).

Unity6000.6.5f1 격리Batch Before **15 PASS/0FAIL**, After **40 PASS/0FAIL**: 실제Renderer연결8/Importer512·PPU128·Pivot256/Point·NoMip/9개환경/Collider동일/신규Console오류0. 1280×720,1600×900,1920×1080 동일camera·Zoom으로전체/접경캡처와6개확대자료 확보. PNG합성Preview가아닌 실제RenderTexture입니다.

## 3. 시각 FAIL — 적용 중단 사유

1. **지면2종 반복선이 사라지지 않았습니다.** 최외곽 RGBA seam은0차이이나12px평균결합구간이 넓은 회색가로·세로띠로 보입니다. 중앙12..499 영역은원본동일이지만 게임Tiled표현은불합격입니다. GroundBase와Cracked01을 각각 edge-neighborhood까지수정해야 하며, x=-1.5의서로다른지면접합은 별도합격판정이필요합니다. x≈-2.5등반복선과Overlay끝을혼동하지않습니다.
2. **6개오브젝트 바깥 가이드 잔존**: 실제확대화면과후보PNG픽셀로확인. Cliff y457/x18, Edge y480, Glow y469~470/x489~490, HeatVent y22/487~488, Slab y458/x493, Spire y488~489 주변. 좌표는512×512·위쪽원점0기준·재확인대상이며 바위하이라이트를무조건삭제하는mask가아닙니다. 중심선만지우고인접한회색/흰색프린지를남긴결과로, threshold만조정해임의재작업하지않았습니다.
3. 바위/열기/균열실루엣은화면상표현되지만 가이드실패가남아 최종미술합격으로올리지않습니다. Tint/새덮개Quad/원본복귀로실패를숨기지않았습니다. **PNG 픽셀추가변경0, 원본적용0**.

## 4. Main18 기능 Delta — 시각 판정과 분리

최종 **92 PASS/0FAIL**, 기존500전수반복0.13목표정의·조사좌표,세린·공식몬스터프레임참조,실제BattleStatusEffectRuntime의0→1→2/세번째8%피해·reset,냉각약서비스소비/과열제거,휠체어물리이동/서쪽E·패드A안내,Main19물리Gate linecast/후속Field10정의보존,Save/Continue 위치·Quest·냉각약,Field08왕복서비스·준비전Gate·복귀Spawn을확인했습니다. 실제13단계연속Story·지정전투승리까지재플레이한새500PASS가아닙니다.

초기delta fixture의Scene ID누락에따른자동저장오류는검증용초기세션을실제전환처럼기록해해소했습니다. WindowsGPU Editor재시도1회가초기로드중종료되어이미지없는기능QA만-nographics로재실행했고최종exit0입니다. 최초로그는Temp/DeltaAttempt1/Delta2.log에보존, 실제게임C#수정0. 기존UnityEditor.Search/URP환경기준선과신규오류0을구분했습니다.

## 5. 원본 보호·인계

원본 **4339파일 SHA불일치0 / UserData23백업 / Git status raw동일 / index변경0**. 원본UnityEditor PID2524가열려있음을읽기확인했으며 해당Editor/Play/Refresh/포커스조작0. 원본Field09 Scene·공식10PNG/meta/GUID/Save·Settings·Main09/10·Field06/07·Field08·음성·몬스터원본모두변경0. 원본적용은시각FAIL때문에차단되었으며 Editor가열린것을불합격대신사유로삼지않습니다. local/origin/main `675988c9f2c5c507d9a696b0378be27c0bd3309b`, Push/PR/main병합0.

다음재개: **PNG8의가이드프린지와두Ground반복띠를납품담당이한정보완한새후보**를받아새ManifestSHA검증→같은worktree의Renderer8참조만교체→실패영역화면우선검수→필요delta만실행→0FAIL후충돌/백업/SHA조건으로원본반영. 현재재포장ZIP은열기문제만해결했으므로미술합격본으로재배포하지않습니다. 외부메시지는보내지않고정확한수정대상을문서로준비했습니다.

[실제 전후 화면](Evidence/Field09_MinimalFix_Repacked_20261019/index.html) · [ZIP3방식검증](Evidence/Field09_MinimalFix_Repacked_20261019/RepackValidation.json) · [남은픽셀대상](Evidence/Field09_MinimalFix_Repacked_20261019/RemainingArtTargets.json) · [보호SHA](Evidence/Field09_MinimalFix_Repacked_20261019/ProtectionFinal.json).
