# LIMITLESS — 2026-10-21 Main23 서릿길 마을 v2 / Wolf Run6 v3 합동 검수: ZIP 동일성 FAIL, 게임 미술 후속 분리

**책임 브랜치:** `docs/planning`. **근거:** 바울 대표가 전달한 Codex 보고 화면 2장. **검수 Codex 브랜치:** `codex/qa-frostway-village-wolf-run6-20261021`, QA 커밋 접두 `c61f2b10`(정확한 SHA는 해당 Codex 로컬 보고 재확인). 사용자 원본 4,351파일/Save/UserData/Editor/Git status 불변; 이번에는 게임 적용/Main23 Quest 구현/Push/PR/main 병합 없음.

## 0. 두 ZIP 모두 **ZIP byte identity mismatch** → 이전 납품 정본 그대로라고 주장하지 말 것

아트 담당 GitHub report 예상 vs 실제 Codex 로컬 파일 SHA256:
- Village v2 `F:\Downloads\Limitless_Chapter3_FrostwayVillage_Environment_ArtPilot_v2.zip`: 담당 문서 예상 `2142fe641c64a2ead8ad37309258d49727f35e5aaea74e429cbd6bae546154ad`; 사용자 Codex 실물 QA 보고값 `19dfad3cc6f7e27b03e1dcfc194cdb1bf56d9e133b48f303288eff98886833d`.
- Wolf Run6 v3 `F:\Downloads\Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtRework_v3.zip`: 담당 문서 예상 `171b954ddeef2b37d1bfaeb197b8a9f028293494457b66664c1d83509b87b5e0`; 사용자 Codex 실물 QA 보고값 `f7eb4122b150e351900a4ab7ea82c802809605bd0444be89e36f6bf167dcd008`.
- **Codex CRC·내부 Manifest·PNG 디코딩·개별 이미지 검사 통과**, 그러나 전체 ZIP 바이트 동일성 검사는 FAIL. 이것이 자동으로 PNG 변조를 뜻하는 것은 아니다(압축 옵션/ZIP 엔트리 metadata/파일 조합 등이 다를 수 있음). 임의로 원인을 추정해 PASS 전환하지 말 것. 각 아트 담당은 **실제 바울 대표에게 전달된 ZIP 바이너리의 SHA 기준**으로 이전 보고값과 정확히 대조, ZIP entry name/size/compressedSize/CRC/mtime/내부 PNG sha/Manifest 표·바이트 차이를 보고하고 원인을 식별할 것. 자기 세션에서 사용자 F: 접근이 없으면 단정하지 말고 Codex의 실제 2개 read-only 인벤토리 증거를 의뢰해 회신받아 기록. 기존 다운로드 ZIP 삭제·재압축·덮어쓰기 없이 **새 납품 v3/v4에서는 자신이 최종 실납품한 파일 SHA와 검수 SHA를 동일하게 맞추기**.
- 결론 `A_ZIP_REPORTED_SHA_MISMATCH/B_ZIP_REPORTED_SHA_MISMATCH / INTERNAL_FILE_QA_PASS / ROOT_CAUSE_PENDING`. 현재 기존 ZIP의 정식 원본 편입 금지.

## A. 신규 아트 docs/art-new — Village Art Pilot v2 콘셉트 HOLD / Game-Ready FAIL

- Codex 판정: ZIP 내부 기술 PASS, **콘셉트 미술 HOLD**, **게임용 Sprite 자산 완성도 FAIL**, **Unity GameView 미실행·오프라인 확인만**.
- `Tiles/`의 동굴길/지면 등 5개 콘셉트 이미지는 존재하나 **독립 반복가능 Ground/얇은 서리/돌길 타일이 부족**. `Buildings/`과 `Props/`는 **합성시트가 아닌 실제 개별 건물·지붕·벽·문·상점·여관·길표지·장작/랜턴/상자·동쪽 비작동 포탈 석조물 Sprite로 분해되지 않음**. 서비스 표시의 명확성/접근성도 부족.
- `21:9` 레이아웃 프리뷰에서 **16:10용 미리보기 영역이 들어가거나 화면이 잘리는 문제**, 보고서의 별도 16:10 추가/명확한 재생성 지시. **16:9/4:3/21:9에서 원본 조각 손실 없이 containment**와 목적별 레이아웃 간선 포켓/북문·남문·7×7WU 동쪽 미래 비작동 광장 표시. 실제 UI/Collider/월드 단위 비율은 Codex 기술계약과 구분.
- 원본 콘셉트11 PNG, 기존 승인 F1 6종 SHA 보존. **새 대형 콘셉트만 또 재패키징하지 말고** 최소 **Unity 사용 가능한 독립 Sprite PNG/Tiles v3 실제 파일**을 제작:
  - 지면 흙·자갈/부분 서리, 돌길, 인접 경계 (실제 반복타일/알파 Overlay, 2×2/3×3 seam 검사);
  - 주택/여관/일반상점·스킬상점에 쓸 수 있는 **건물 단위** 지붕/벽/문과 서비스 표식(투명 RGBA Sprite);
  - 장작/보급상자/따뜻한 랜턴/길표지/남·북 입구 표식, 동쪽 미래 비활성 포탈 돌기단(비작동·Trigger 없음) **각 개별 Sprite**;
  - 1:1 게임 크기/기존 공식 Chapter2·승인 F1 미술 품질 대조; 2×2 타일 반복, alpha bbox/고립픽셀·sprite-cell 침범, 건물 지붕 문 접합, 통행 주길/서비스 접근/휠체어 시각.
- 제작 해상도·PPU/Pivot/Sorting 최종값은 Codex Issue #45 최신 Main23 기술 사전점검 계약에서 읽어 확정하고 **확인되지 않은 256px·PPU 숫자를 임의 강제 금지**. 실제 게임에 쓰지 못하는 대형 Moodboard와 실 Sprite를 디렉터리 별도 분류하고 독립 빌드 사용가능 자산 비율·부족 목록 QA. 새 후보는 `GAME_READY_ART_CANDIDATE / USER_FINAL_ART_APPROVAL_PENDING / UNITY_RUNTIME_NOT_APPLIED`; Unity Scene/Main23 5목표 작성/원본/main 변동0.

## B. Chapter1·2 리뉴얼 docs/art-renewal — Wolf Run6 v3 Tail 개선 / 모션 HOLD

- Codex 판정: ZIP 내부 기술 PASS, **프레임 00/01/02/05 PNG Byte-SHA 동일**, v3 Frame03/04 각각 8,198/7,615픽셀 수정, 꼬리 복원 확인, **격리 Unity 실제 12FPS 재생 PASS**(게임 공식 Wolf 컴패니언 교체는 하지 않음). **모션 미술 HOLD**: **Frame04 뒷다리 자세**, 중간 프레임 사이 신체 비율·발 접지/05→00 루프의 어색함, 시각적 '달리기' 완성도 부족. Editor Search 기능 예외1건 별도 기록됨(애니메이션 결함으로 단정하지 말고 환경 경고 분리).
- v5 **승인 단일 KeyArt** 및 기존 v2/v3 Run PNG, 기존 공식 ThirdParty Wolf/BeastCompanionSystem/GUID는 원본 유지.
- 리뉴얼은 **Run6 v4 최소 미술재작업**: 03·04 꼬리 회복된 부분을 유지하되 Frame04 **뒷다리 해부학·접지**, 주변 중간 Frame02~05와 크기/가슴·어깨/허리/발끝 정합, 05→00 정상 루프 연결을 실제 12FPS에서 자연스럽게 만든다. 기존 00/01/02/05를 무조건 다시 그리지 않고, 부자연스러운 루프 해결에 정말 필요한 포즈만 변경하고 변경기록 명확히. Alpha 꼬리 배경삭제만 반복하거나 6장 단순 XY 이동·scale pseudo motion 금지. 512×384 RGBA6, 12FPS LEFT, 3072×384 Strip/3×2 Atlas/128px·40px 미리보기, 변경영역 동배율 비교, GIF/APNG/좌향, ZIP CRC+SHA Manifest.
- v4 완성 후 Codex **바뀐 포즈들만** 격리 실 Unity 12FPS 후속 DeltaQA, 대표 모션 최종 승인 후에만 정식 펫 런타임 교체; Wolf Run v3은 `TECH_PASS/UNITY_ISOLATED_PLAYBACK_PASS/ART_HOLD/USER_MOTION_APPROVAL_PENDING`.

## 공통 후속 / 사용자 보고 원칙

- 이번 스크린샷에 나온 Codex 검수는 **이전 본편 QA를 재실행하지 않은 2자산 합동 QA**, Main22 136PASS/F1 64PASS/서리들쥐 Battle40PASS 재검사 하지 말 것.
- 담당별 `docs/art-new`, `docs/art-renewal` 작업을 다시 각각 선별 진행하고, **ZIP 파일명/해시 mismatch는 두 담당이 원인 조사**; 개선본 제작과 병렬 해결 가능하지만 byte 동일성 정리 전 구 ZIP 정식 승인/적용 금지.
- Codex Main23 기술 계약과 서릿길 SafeZone·NPC·스킬 저장 [#46](https://github.com/HappyPaul812/Project-Limitless/issues/46) 이슈를 그대로 이용, 새로운 Scene/Skill 스키마/Item 가격·Loot/Quest 보상·TTS 자동 제작 금지.
- ZIP 실물은 바울 대표 `F:\Downloads` 전달분 우선, 기획세션 직접 읽었다고 주장 금지. 담당 세션은 자신의 sandbox 실물만 실제 검사했다면 그 범위를 정확히 보고해야 함.
- 결과 보고: 신규 Package ZIP SHA가 대표 실물과 정확히 일치, 각 PNG sha/비교이미지, 기술 vs 미술/게임 준비도, Branch·Commit·승인 게이트·사용자 원본/Save·UserData/Editor/승인본 보존, main 직접 Push/merge 없음.
