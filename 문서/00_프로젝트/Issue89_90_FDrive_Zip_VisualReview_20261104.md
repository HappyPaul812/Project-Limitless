# Issue #89·#90 Windows F: 원본 ZIP 표적 시각 검수 / 2026-11-04

## 범위와 근거
총괄 기획 최신 댓글 #89 issuecomment-6620046106, #90 issuecomment-6620046594 및 `docs/planning`의 `20261104_Issue89_90_LibraryZip_ArtReview.md`(6f313419ff83c9e915962ae65410e559aaff836a)를 읽고 실제 F: 파일을 열었습니다. CRC/Manifest/내부 SHA 재검수와 기존 Unity QA는 반복하지 않았습니다.

| 실제 ZIP | bytes | 전체 SHA256 | 대조 |
|---|---:|---|---|
| F:\Downloads\LIMITLESS_SPIDER_ATTACK3_ISSUE90.zip | 4122683 | 91411d159d401f1772e80e848d78550dd62552f65c1a76d659c15d8b0df44d3e | 보고값 일치 |
| F:\Downloads\Limitless_Ch3_ColdVFX_Issue89_Pilot_20261104.zip | 1535561 | 83b4efeee62cdb4cabf7dbcc4d2aea21cbefdb4507e03f53b8c59d8079e9e4ba | 보고값 일치 |

F_DRIVE_ZIP_SHA_MATCH: 두 보관 사본과 바이트 동일성 확인. 미술 승인·실게임 검증을 뜻하지 않습니다.

## Spider 실제 프레임 판독
ZIP Attack3의 Prepare00/Contact01/Recover02 512 RGBA를 동일 좌표·동일 배율로 보았습니다. 모두 대체로 우향 3/4이며 명백한 좌우 반전은 없습니다. 다만 승인 v5/Idle00보다 두부가 화면 오른쪽으로 치우쳐 기존 자세와 시점 차이가 큽니다. Contact에서 두부/눈은 올라가고 복부는 낮아지며 앞다리가 오른쪽으로 펼쳐집니다. Recover에서는 복부 상단이 높아지고 두부도 높은 위치에 남습니다. 따라서 단순 준비→전진→원위치 복귀로 동일 모델이 자연스럽게 연결된다고 승인할 근거는 부족합니다.

좌상단 원점, bbox 우·하단 제외 좌표입니다. 전체 실루엣 알파 가중 중심은 몸통 중심이나 Pivot이 아닙니다.

| 프레임 | alpha bbox | 실루엣 중심 x,y |
|---|---|---|
| Prepare | (19,114)-(493,414) | 249.15,273.97 |
| Contact | (19,147)-(493,414) | 238.34,275.24 |
| Recover | (27,97)-(484,414) | 247.78,251.50 |

하단 y=413은 같지만 같은 지지발이 고정됐다는 뜻은 아닙니다. Contact→Recover 중심은 약 (+9.44,-23.74)px 이동하며, 상단은 50px 높아집니다. 이것은 자세 변동의 보조 수치이고 실제 물리 중심/발 미끄러짐 판정이 아닙니다.

관절: Contact의 화면 오른쪽으로 길게 뻗은 아래 앞다리는 Prepare의 턱 아래 교차 다리와 굽힘·겹침이 크게 다르고, Recover에서는 아래로 굽은 다른 실루엣이 됩니다. 두부 옆 오른쪽 위 앞다리도 Contact의 가로 전개→Recover의 아래 굽힘으로 바뀌어 동일 다리의 어깨/무릎/발끝을 세 장에서 일관되게 대응시키기 어렵습니다. 절단/분리나 다리 수 오류를 확정하지는 않습니다. 표적·접점 데이터가 없어 실제 타격 접촉은 NOT_VERIFIED입니다.

승인 전 최소 대상은 Contact01 중심의 두부 방향·복부 높이·앞다리 관절 대응, Recover02의 준비/승인 Idle로 돌아오는 높이와 지지발입니다. 아트 담당이 동일 몸통 기준점과 동일 다리 연결을 표시한 세 포즈 비교로 먼저 설명하고, 필요한 부분만 소량 수정하는 것이 적절합니다. 기존 승인 v5/Idle8/Walk8 재작화·나머지 Attack5장/Shoot8 제작은 하지 않았습니다. 3장은 키포즈이며 10FPS 인접 완성 프레임으로 간주하지 않았습니다.

## 냉기 최소 시각 검토
동봉 설원/어두운 배경 128px 비교와 실제 40px PNG 세 장을 밝은 #eceff2/어두운 #172231 단색에 합성해 직접 보았습니다. 모두 OFFLINE이며 실제 F2 배경 텍스처·Unity 화면 검증은 아닙니다.

한기는 낮은 안개·바닥 고리·눈송이로 구분되지만 밝은 배경에서 흰 연무와 작은 눈송이가 옅어집니다. 최소 보완 후보는 바닥 고리/연무 경계의 국소 명암이며 전체 재색칠은 필요하다고 판단하지 않았습니다. 동상과 동결괴사는 128px에서는 결정 군집과 검푸른 균열이 구분되지만 40px에서는 둘 다 촘촘한 방사형 가시 덩어리로 읽힙니다. 색 차이는 있으나 형태만으로 빠르고 안정적인 구분을 승인하기 어렵습니다. 동상의 붙는 결정 군집과 동결괴사의 분리된 균열/큰 쐐기 실루엣을 최소 범위에서 구분하는 후보 검토가 필요합니다. 별도 상태명·아이콘·턴/중첩 숫자 병행 원칙을 유지하며 PNG에 글자를 넣지 않았습니다.

## 보호·판정·후속
- USER_VISUAL_APPROVAL_PENDING / SPIDER_DIRECTION_JOINT_CONTINUITY_REVIEW / TARGET_CONTACT_NOT_VERIFIED / CHILL_SNOW_CONTRAST_REVIEW / 40PX_SHAPE_DISTINCTION_REVIEW.
- Unity 실행/QA/Import/전체 애니메이션 제작/본편 적용0. ProjectVersion은 실제 6000.6.5f1 (3ff58d469c8a) 확인만 했습니다. 개인 Save/UserData는 열람·해시·복사·변경하지 않았습니다. 승인 아트·main·사용자 미커밋 파일 수정0.
- 증거는 원본 workspace `Temp/Issue8990LocalReview20261104/`의 Spider_native512.png, Cold_actual40_light_dark.png, measurements.json, inspect.py, measure.py입니다. 원본 ZIP에서 진단용 사본/비교판만 만들었으며 납품 아트를 수정하지 않았습니다. 바이너리 원격 업로드0.
- 다음은 위 표적 항목의 미술 승인/소량 보완 판단이며 새 Unity QA와 제품 적용은 이번 범위가 아닙니다.
- 담당 로컬 브랜치 codex/issue89-90-local-zip-review-20261104, 출발 634a91d3e4174c87761c0d377cfa5a695ca79e7f. LOCAL_COMMITTED / REMOTE_NOT_PUSHED 상태는 실제 커밋 후 Issue 댓글 SHA로 기록합니다. GitHub 문서 Push/PR/merge0.
