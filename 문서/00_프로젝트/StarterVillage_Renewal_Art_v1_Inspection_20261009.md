# StarterVillage 리뉴얼 아트 v1 검수 및 통합 준비 — 2026-10-09

## 1. 최종 판정과 조사 범위

**NEEDS_ART_CORRECTION**. PNG 무결성은 PASS, 환경 역할 15/15 및 캔버스 규격은 대응 가능하다. 그러나 기존 건물 좌표로 조립하면 지붕과 문 사이에 투명 틈이 생겨 즉시 통합 준비 완료로 판정하지 않는다. 별도로 Manifest의 좌·우 길 Sprite 대응은 LOCAL과 반대이므로 **NEEDS_HANDOFF_MAPPING** 문제를 함께 해결해야 한다. 파일명 차이 자체를 FAIL로 처리하지 않았다.

- 실행 범위: 원본 ZIP 읽기, Python/Pillow PNG 디코딩·해시·알파·반복 합성, LOCAL Scene/Prefab/meta/설치 코드의 정적 대조 및 이 문서 작성만 수행했다.
- Unity Editor 실행·MCP 조회·Import·Scene 재생성·Play·공용 코드 수정 없음. CURRENT_STATUS, PNG, meta, GUID, Save, Quest, Inventory, Audio/TTS는 변경하지 않는다. 사용자 미술 승인은 **PENDING**이다.
- 시작 HEAD: `91f4cf78740bec2dc6e703fb346ff6615fe0902b`. 최근5개: 91f4cf7 / eaefb0c / ff9a2e3 / fbbc7e9 / e75e7b7. 시작 tracked 수정75개, 최상위 미추적25항목, 전체 porcelain562행, staged0개를 보호 기준으로 기록했다.
- 확인 순서: git status/log/HEAD → AGENTS → PROJECT_CONTEXT → CURRENT_STATUS → CODEX_WORKFLOW → StarterVillage Baseline → 관련 LOCAL Scene/Prefab/meta/코드. 기준 조사 문서는 [StarterVillage_Renewal_Baseline_20261009.md](StarterVillage_Renewal_Baseline_20261009.md)이며 실제 LOCAL을 우선했다.
- LOCAL Scene SHA256 `c86036a1c4dd3472ebf2efc810e9b9886f411e6feee0ff7a3c313910b6540105`: Baseline과 동일. 환경 렌더러284개, 고유Sprite15종, Scene 환경Collider11개를 재확인했다. 남문 활성 울타리16개와 합계300개다.

## 2. ZIP 정보 및 전체 파일 목록

원본: `F:\Downloads\Limitless_StarterVillage_Renewal_Art_v1.zip`, 크기368,540bytes.

ZIP SHA256: `9a6b3415c806cc0022550abbf2126af0e7309b76c96b8b57865bd1439e129b34`.

전체22파일 = **PNG19개(게임용15 + Preview4) + 문서3개**. CRC/testzip 정상, PNG19/19 디코딩 정상, 손상0, 공백0, 완전 동일 바이트/픽셀 중복0. 게임용15개는 모두 실제 PNG 8bit RGBA(color type6)다. Ground3개는 완전 불투명으로 지면 역할에 적합하며, Buildings/Props12개는 투명 영역을 갖는다. Preview RGB는 게임용 투명 Sprite가 아니므로 FAIL 사유가 아니다.

아래 투명률은 Alpha=0 비율, bbox는 좌상단 원점 `[left,top,right,bottom)`이다. 모든 PNG 정상 디코딩이며 표의 SHA는 ZIP 내부 원본 바이트 기준이다.

|상대 경로/파일명|해상도|인코딩 모드|bytes|투명률 / 알파 bbox|SHA256|
|---|---|---|---:|---|---|
|`Buildings/LL_C1_SV_House_Door_128_v1.png`|128×128|RGBA|770|63.06% / [28, 12, 102, 122]|`0dc2e0e730795eb31593dbfb4550236998e5d407ae604dd1b8fa861707236aa3`|
|`Buildings/LL_C1_SV_House_Roof_Center_128_v1.png`|128×128|RGBA|1510|40.31% / [1, 13, 128, 126]|`80a7ec479711b411a33b1b6c2bfc323026e9b05376a15fdfaf18e38a9db8026d`|
|`Buildings/LL_C1_SV_House_Wall_Window_128_v1.png`|128×128|RGBA|1156|26.82% / [12, 0, 117, 116]|`0d1798f29dab559de21c100837cbec9b589ecfa13e59c660c8bc3d57116326f0`|
|`ChangeNotes.md`|—|문서|1040|—|`4862c63deb5d34a2081e0f4154d07335bc5308d76ea159e2cb9673e4eb628a26`|
|`Ground/LL_C1_SV_Ground_Grass_128_v1.png`|128×128|RGBA|1822|0.00% / [0, 0, 128, 128]|`8e0761503ce0f5cbbb98c2c1c53bec66ef5299dcadf54c37bc7331f70a4eaca9`|
|`Ground/LL_C1_SV_Path_Left_128_v1.png`|128×128|RGBA|2084|0.00% / [0, 0, 128, 128]|`b83394b92485fadc023583ece8e1838542b33c72e8e6676f0e2ca526f9933188`|
|`Ground/LL_C1_SV_Path_Right_128_v1.png`|128×128|RGBA|2060|0.00% / [0, 0, 128, 128]|`1f49d6b60a9e8ce8de560040697ed48c7bc87a5100cf65700925c3d582d5c389`|
|`Manifest.json`|—|문서|7727|—|`5fcfafe3c78516c020be7b1108d3078cbb226ac68b68fd8cc904f50291d11ffe`|
|`Manifest.md`|—|문서|6745|—|`4ea35aee3f2436dd78a83756a5ee6f62e34e6af880e8dfb02d936a93fd8c26ef`|
|`Preview/LL_C1_SV_Asset_ContactSheet_v1.png`|1600×1940|RGB|150738|0.00% / [0, 0, 1600, 1940]|`046c6561583c7a540b34d927383e1244c541ed9d7eea19238df228dc30501026`|
|`Preview/LL_C1_SV_Layout_19x13_CoordinateStudy_v1.png`|2432×1664|RGB|127337|0.00% / [0, 0, 2432, 1664]|`6a402fe102236a0d3e731a0b7b0d7d677daf29e608641f9674aec09d9241cffa`|
|`Preview/LL_C1_SV_NPC_Reference_MARKERS_ONLY_v1.png`|1216×832|RGB|92087|0.00% / [0, 0, 1216, 832]|`7f9df01d6380992810c68dcd7adbabe69eaa797595420f42a4e467f38388c2ce`|
|`Preview/LL_C1_SV_Path_Seam_6x6_v1.png`|768×768|RGBA|33608|0.00% / [0, 0, 768, 768]|`8aec3a7c972258f163638064ebe32881195af7f22f7e5c72d2817f7ed26eb77c`|
|`Props/LL_C1_SV_Barrel_128_v1.png`|128×128|RGBA|1171|71.13% / [34, 26, 97, 113]|`6d18034b20f59f3187e1df93ba04cb3a5ab66b21a46796f3d97c8926d441e885`|
|`Props/LL_C1_SV_Bush_A_128_v1.png`|128×128|RGBA|1400|64.32% / [15, 23, 112, 108]|`a9b911ac9a7882a063079b05be4a6298ebfffbd2fba5635daff048a051c2c162`|
|`Props/LL_C1_SV_Bush_B_128_v1.png`|128×128|RGBA|1819|53.70% / [11, 28, 123, 124]|`d2baf8a0e0936301fed2980dcfda42020d04e3bf7c7f5e4354e92615a152c0e6`|
|`Props/LL_C1_SV_Campfire_128_v1.png`|128×128|RGBA|1306|85.74% / [24, 45, 104, 115]|`ad59983110d2d5dc644991b6f15a1fadf4e7ddf29640660ad0b1a02edb915c11`|
|`Props/LL_C1_SV_Chest_128_v1.png`|128×128|RGBA|875|55.83% / [19, 26, 110, 111]|`3c98523f8afba1baabd736b1f507a4608f03ce05f7426a3bc633250a32e8178c`|
|`Props/LL_C1_SV_Fence_Wood_128_v1.png`|128×128|RGBA|727|52.54% / [0, 18, 128, 122]|`4ba8c430a0ffd3e2097173482235fb5a4c89e503281bf9243213315d3942175a`|
|`Props/LL_C1_SV_Rock_128_v1.png`|128×128|RGBA|1221|66.97% / [16, 36, 115, 111]|`e45c7143116276f0a2896aba4e5199221954ba1afad6a135d8ec6d2f358122a7`|
|`Props/LL_C1_SV_Tree_Big_255x256_v1.png`|255×256|RGBA|5148|66.36% / [43, 42, 216, 250]|`2a4b1549b60827f9cfc53bf62a02aa727b7af6a8841968b0d4d4e46b181cb31f`|
|`Props/LL_C1_SV_Tree_Medium_128x156_v1.png`|128×156|RGBA|3366|62.93% / [16, 27, 113, 152]|`5b5c8cd56ec18da4a754baf4fe4924876a2347e75c3dfa4adf1b1a41f893b945`|

Manifest.json의 게임용15행 해시·해상도·개수·활성 배치 합계300은 실제 내용과 일치한다. PPU/Pivot/Filtering은 PNG에 적용된 Unity 설정이 아니라 **납품 권장 계약**이다. Manifest.md와 ChangeNotes.md는 실제 Unity 미검증, 건물0.5칸 중첩, NPC/HUD 미포함을 명시한다. 좌·우 길의 의미 설명은 옳으나 기존 Sprite ID 대응표가 LOCAL과 충돌한다.

## 3. 기존 환경15종 교체 매핑

표는 Manifest를 무조건 복사한 것이 아니라 **LOCAL 좌표를 반영한 권장 대응**이다. 신규 Sprite 이름은 해당 PNG 확장자를 제외한 stem 권장안이며 실제 Sprite Asset은 아직 없다. 기존 환경 모두 Default Sorting Layer(ID0), Simple, Scale1, 회전0. 모든 기존 Collider를 보존하며 자동 알파 윤곽 Collider를 생성하지 않는다.

|기존 Sprite|신규 PNG / Sprite stem|기존 해상도|신규 해상도|PPU / 정규화 Pivot|Order / 활성 수|Collider size/offset|단순 교체·추가 구현 판정|
|---|---|---|---|---|---|---|---|
|`tiles_grass_4_0`|`Ground/LL_C1_SV_Ground_Grass_128_v1.png`|128×128 사용셀 (시트1280×768)|128x128|128 / (0.5,0.5)|-10 / 247|없음 유지|규격 유지 가능 / Sprite 연결 필요|
|`tiles_grass_5_4`|`Ground/LL_C1_SV_Path_Right_128_v1.png`|128×128 사용셀 (시트1280×768)|128x128|128 / (0.5,0.5)|-8 / 8|없음 유지|Manifest 반대: LOCAL X=+0.5 / 대응 정정 필요|
|`tiles_grass_4_4`|`Ground/LL_C1_SV_Path_Left_128_v1.png`|128×128 사용셀 (시트1280×768)|128x128|128 / (0.5,0.5)|-8 / 8|없음 유지|Manifest 반대: LOCAL X=-0.5 / 대응 정정 필요|
|`house_tiles_new_4_4`|`Buildings/LL_C1_SV_House_Roof_Center_128_v1.png`|128×128 사용셀 (시트768×640)|128x128|128 / (0.5,0.5)|2 / 2|없음 유지|캔버스 동일 / 접합 수정 전 단순 교체 보류|
|`house_tiles_new_1_3`|`Buildings/LL_C1_SV_House_Wall_Window_128_v1.png`|128×128 사용셀 (시트768×640)|128x128|128 / (0.5,0.5)|2 / 4|없음 유지|캔버스 동일 / 접합 수정 전 단순 교체 보류|
|`house_tiles_new_1_1`|`Buildings/LL_C1_SV_House_Door_128_v1.png`|128×128 사용셀 (시트768×640)|128x128|128 / (0.5,0.5)|2 / 2|(1.5,0.3)/(0,-0.35) 유지|캔버스 동일 / 접합 수정 전 단순 교체 보류|
|`fence_tiles_2_2`|`Props/LL_C1_SV_Fence_Wood_128_v1.png`|128×128 사용셀 (시트640×640)|128x128|128 / (0.5,0.5)|2 / 20|(0.9,0.2)/(0,-0.35) 유지|규격 유지 가능 / Sprite 연결 필요|
|`tree_medium`|`Props/LL_C1_SV_Tree_Medium_128x156_v1.png`|128×156|128x156|128 / (0.5,0.5)|4 / 2|(0.4,0.25)/(0,-0.5) 유지|규격 유지 가능 / Sprite 연결 필요|
|`tree_big`|`Props/LL_C1_SV_Tree_Big_255x256_v1.png`|255×256|255x256|128 / (0.5,0.5)|4 / 1|(0.45,0.3)/(0,-0.75) 유지|규격 유지 가능 / Sprite 연결 필요|
|`bush_01`|`Props/LL_C1_SV_Bush_A_128_v1.png`|128×128|128x128|128 / (0.5,0.5)|1 / 1|없음 유지|규격 유지 가능 / Sprite 연결 필요|
|`bush_02`|`Props/LL_C1_SV_Bush_B_128_v1.png`|128×128|128x128|128 / (0.5,0.5)|1 / 1|없음 유지|규격 유지 가능 / Sprite 연결 필요|
|`rock_01`|`Props/LL_C1_SV_Rock_128_v1.png`|128×128|128x128|128 / (0.5,0.5)|1 / 1|없음 유지|규격 유지 가능 / Sprite 연결 필요|
|`Wooden_Barrel_Type_A`|`Props/LL_C1_SV_Barrel_128_v1.png`|128×128|128x128|128 / (0.5,0.5)|3 / 1|(0.35,0.3)/(0,-0.3) 유지|규격 유지 가능 / Sprite 연결 필요|
|`Wooden_Chest_Type_A`|`Props/LL_C1_SV_Chest_128_v1.png`|128×128|128x128|128 / (0.5,0.5)|3 / 1|(0.45,0.3)/(0,-0.3) 유지|규격 유지 가능 / Sprite 연결 필요|
|`Campfire_Type_A`|`Props/LL_C1_SV_Campfire_128_v1.png`|128×128|128x128|128 / (0.5,0.5)|2 / 1|없음 유지|규격 유지 가능 / Sprite 연결 필요|

### 좌·우 길 대응 오류 — NEEDS_HANDOFF_MAPPING

- LOCAL `tiles_grass_5_4`(fileID1662121770, rect640,512,128,128)는 **X=+0.5** 열8개다. Manifest는 이를 Path_Left에 연결하고 X=-0.5라고 설명한다.
- LOCAL `tiles_grass_4_4`(fileID278596618, rect512,512,128,128)는 **X=-0.5** 열8개다. Manifest는 이를 Path_Right에 연결한다.
- Manifest대로 원본 Sprite ID를 치환한300개 합성에서는 중앙 잔디 띠와 양옆 흙길이 생긴다. 좌표 의미에 따라 위 표처럼 두 대응을 바꾸면 중앙 흙길이 연결된다. YAML GUID/fileID와 LOCAL Sprite meta의 rect를 대조했다.
- 조치: 아트 담당 Manifest.json/.md 기존 Sprite 대응을 정정하고 후속 통합 담당은 실제 좌표와 Sprite ID를 재확인한다. **길 PNG 재제작·좌표 이동은 필요하지 않다.** 원본 ZIP은 이번에 수정하지 않았다.

## 4. 추가 자산과 범위

게임용 추가·누락0개, 필수 교체15개, 신규 장식/예비 환경0개, NPC 이미지0개, Main12 기록 오브젝트0개다. Preview4개(Contact Sheet/전체배치/NPC marker reference/길 seam)와 문서3개는 부가 검수 자료이며 게임용 Import 대상이 아니다.

NPC 실물 대신 표시 위치 marker를 사용한 납품 Preview는 실제 NPC 크기·상호작용 회귀의 증거가 아니다. Main12 기록 Sprite가 없는 것은 기존15종 요구 범위의 누락이 아니다. 새 시설·통행로·문 아치·실내를 추가하지 않는다.

## 5. 타일 반복 및 연결 판정

|검사|판정|관찰/실측 및 한계|
|---|---|---|
|잔디3×3·5×5|PARTIAL|독립 합성에서 빈틈/투명선 없음. 무늬 주기와 가로 이음이 보임. 상하 끝 RGB 평균차8.188/255, 내부 인접행 평균차2.512로 완전 무봉제라고 PASS하지 않음|
|길 좌우·세로6×6|PARTIAL|올바른 좌표 매핑이면 불투명 흙길이 연결됨. 중앙 좌우 끝 평균차2.875. 세로 이음 좌5.776/우4.859로 반복 무늬 존재; 실제 카메라 가독성은 사용자 검토|
|잔디↔길|PARTIAL|양옆 잔디 부분과 흙 띠 역할 구분 가능. 픽셀 패턴과 상단Y=1.5 직선 종단 자연스러움은 미술 승인 대기|
|길 모서리/교차/종단 모듈|NOT_APPLICABLE|현재 LOCAL은 직선2열16개만 사용. 별도 corner/end PNG 없음. 현 교체 필수 누락으로 취급하지 않으며 확장 때 별도 계약 필요|
|울타리 가로5개|PASS|양쪽 끝128행 RGBA가128/128 정확히 일치, 중앙 연결선 끊김 없음. 남문 중앙 공백 보존 가능|
|Unity 필터/축소/서브픽셀|NOT_VERIFIED|Point/PPU128 권장만 확인. 실제 카메라/픽셀 정렬/지원 화면 비율 테스트 미실행|

RGB 평균차는 경계 비교 참고 지표이며 예술적 품질의 자동 통과 임계값이 아니다. 조립 방향이 없는 Path_Left의 가로 자기 반복을 잘못된 실패 검사로 적용하지 않았다.

## 6. 건물·소품 조립 및 잘림

**건물 조립 FAIL.** 두 채 모두 지붕(-4.5/+4.5,4.5), 문(-4.5/+4.5,3.5), 벽(-5/-4 또는4/5,3.5)다. 각각128px/PPU128/Center이며 벽·문이0.5WU=64px씩 겹친다.

- 중앙 세로 픽셀에서 지붕 마지막 불투명행125, 아래 문/벽 조합 첫 불투명행은 합성140이므로 **14px 투명 틈**이 남는다(0.109375WU). LOCAL 좌표 그대로 두 가지 순서(문을 나중/벽을 나중)로 합성해도 지붕 아래 잔디가 드러난다. 이는 같은 Order의 순서 변경만으로 해결되지 않는다.
- 문/벽 중첩은 순서에 따라 문 가장자리 표현이 달라진다. 모두 Order2인 실제 Unity 동일 Order 최종 순서는 NOT_VERIFIED다. 미확인 순서를 임의 확정하거나 Scene Sorting을 수정하지 않았다.
- 요청 수정: 기존128²/PPU128/Pivot/좌표/Collider 계약 안에서 **건물3부품의 지붕-벽-문 알파 접합과64px 겹침**을 보정하고 위 두 순서/실제 좌표 합성 확인. 통짜 집/새 건물 구조 변경을 요구하지 않는다.
- Roof 오른쪽 끝X127의Y108…116에 픽셀이 닿고 반대쪽에는 여백이 있다: 외곽 잘림 의심 **PARTIAL**, 자연스러운 외곽 마감 확인 대상이다. 벽 상단/울타리 양끝 접촉은 모듈 접합 의도로 분리했다. 나머지 독립 소품은 canvas 내부 여백과 닫힌 실루엣을 갖고 명백한 canvas 잘림 없음.
- 상자/통/불/바위/수목의 역할 식별 및 투명 배경은 정적 검사 PASS. 모닥불은 정적 단일 PNG이며 애니메이션/광원 신규 요구 없음. 최종 크기·그림체·인게임 미술 승인은 별도다.

## 7. PPU·Pivot·Collider 및 통행

신규15개 모두 기존 사용셀/canvas와 동일하여 PPU128/Center 유지 시 기존 world 크기·좌표를 보존할 수 있다. Point, Mipmap Off, Sprite Single, 무압축, Alpha 투명도 유지 권장. 크기 변화로 NPC 접근 통로를 강제로 줄일 사유는 현재 없다. 이 결과는 캔버스 계약의 PASS이며 그림과 충돌의 완전 일치 PASS가 아니다.

나무큰255×256의 발끝은 Center 기준 약Y=-0.953WU, 기존 collider 하단=-0.9로 약0.053WU 장식 여유가 있다. 중간나무 발끝≈-0.578은 collider[-0.625,-0.375] 안쪽이다. 상자/통 발끝≈-0.367/-0.383은 기존 collider[-0.45,-0.15]와 대응한다. 울타리 발끝≈-0.453과 기존 하단=-0.45 차이는 약0.003WU다. 이를 이유로 Collider를 재생성하지 않는다.

지붕·벽에는 원래 Collider가 없고 문의 얕은 박스(1.5,0.3)만 있다. 새 그림 전체가 통행 불가 건물로 보이면 실제 통행 공간과 시각 외형이 어긋날 수 있으므로 건물 주변/뒤쪽 이동은 후속 Runtime 검토 후보다. 기존 저장 좌표를 막는 Collider 확대나 건물 위치 이동은 별도 설계 승인 없이 하지 않는다.

남문 울타리16개는X=-9…-2 및2…9/Y=-5.7, 신규alpha 시각 공백 X[-1.5,1.5] 폭3 유지 가능. 기존 충돌 간격[-1.55,1.55], WorldBounds 남쪽 개방[-1.5,1.5], Exit(0,-6.5) Trigger(2.5,0.9)를 보존한다. Spawn(0,-4.65)와 Trigger가 겹치지 않는다. 실제 Player 끼임/대각선 이동은 NOT_VERIFIED다.

## 8. NPC12명·Quest·Save 영향

LOCAL HubInstaller의 Runtime 좌표/Stable ID와 Baseline을 대조했다. 아래 접두사는 모두 `starter-village-`이며 임의로 ID/직무/외형을 교체하지 않는다.

|Stable ID(접두사 포함)|좌표|정적 겹침 후보|
|---|---|---|
|starter-village-main-guide|(0,1.5)|없음|
|starter-village-gate-guard|(2.4,-5.1)|없음|
|starter-village-general-shop|(-5.8,1.2)|없음|
|starter-village-equipment-shop|(5.8,1.2)|없음|
|starter-village-healer|(-4.4,2)|건물 벽/문|
|starter-village-bank|(4.4,2)|건물 벽/문|
|starter-village-party-manager|(-6.4,-2.1)|BushWest|
|starter-village-training-guide|(6.4,-2.1)|BushEast|
|starter-village-resident-01|(-3.2,-1.2)|없음|
|starter-village-resident-02|(3.2,-2.6)|Barrel|
|starter-village-resident-03|(-7.2,0)|TreeWest|
|starter-village-resident-04|(7.2,0)|TreeEast|

겹침은32²/PPU28/발Pivot(0.5,0)의1.142857WU canvas와 신규 환경alpha의 보수적 정적 검사다. NPC 실루엣 실제 렌더링을 대체하지 않는다. 환경Order 최대4 < NPC5, 이름7/그림자6, QuestMarker Canvas21을 유지하면 환경이 body/표시를 앞에서 덮는 정렬 위험을 줄일 수 있다. 같은 Default Layer 유지가 조건이며 Runtime 판정은 NOT_VERIFIED다. Interaction radius2/대화 종료3, NPC body(0.5,0.34) offset(0,0.17), 이름Y1.02/MarkerY1.92를 보호한다.

- Main01: 대표→경비→Field01; Main05: 마을 도착 `starter_village_main05_return` 및 대표/경비 보고; Main12: 잡화상 및 `village_main12_records`(-3,2.5). 기록 지점 주변1WU 보수적 영역에 신규 환경alpha 겹침 없음. 조사 지점은 기존 라벨/Navigation이며 새 Sprite가 필수는 아니다.
- Chapter1 Side5: `side_c1_fence_resin`, `side_c1_forest_first_aid`, `side_c1_grave_fragment`, `side_c1_night_wing_report`, `side_c1_reinforce_gate`. 경비/잡화상/대표 접점과 Item/Quest 상태를 유지하며 이번에 수정하지 않았다.
- 상점·치유·은행·파티·훈련의 Stable ID와 표시를 보존한다. 두 공용 주택 그림만으로 상점/치유소를 구별할 수 없으므로 기존 NPC 이름·마커 및 출입구 형태의 읽힘을 사용자 검토한다. 색상만으로 기능을 추가하지 않는다.
- 신규 시작(0,-5), Field01 귀환 Spawn(0,-4.65), 출구(0,-6.5), Field01 도착(0,4.7)/귀환Trigger(0,6.65), Scene/Spawn ID 보존. World19×13 및 viewport Bounds 보존.
- Save는 Bounds 안의 실제좌표를 복원하며 장애물 점유까지 검사하지 않는다. 따라서 신규 Collider/위치 변경이 없는 참조 교체 계획을 우선한다. 실제 Main/Side 진행, 상점/치유, 왕복, Save/Continue는 **NOT_VERIFIED**다. 사용자 Save 내용은 수정/테스트 로드하지 않았다.

## 9. 무료 에셋 의존 제거 가능 범위

현재 환경15종은 Schwarnhild12Sprite/8PNG + Xariami3Sprite/3PNG다. 수정 후 신규15종을 Scene284곳과 남문 활성16곳에 연결하면 **이 마을의 활성 환경300개**에서 해당 무료환경 직접 Sprite 의존을 교체할 수 있다. 남문 prefab의 비활성 X±10 울타리2개까지 향후 참조를 정리해야 prefab 전체 의존도 끊긴다.

ThirdParty 원본 폴더/PNG/meta/GUID 삭제는 범위 밖이다. 다른 Scene 및 기존 StarterVillageSceneGenerator 등 생성기에서 원본 참조가 남을 수 있으며 신규 Scene 재생성 때 다시 무료환경이 연결되지 않도록 후속 별도 수정 대상이다. 무료에셋 폴더 전체 제거 가능 판정은 하지 않는다. 공유 Eldiran NPC12종은 이번에 그대로 남는다. 라이선스 최신 검증/권리 승인을 수행하지 않았고 Baseline의 로컬 기록을 자동 법률 승인으로 확대하지 않는다.

## 10. 시각 품질 및 사용자 승인

로컬 승인 샘플 `F:\Downloads\Limitless_StarterVillage_Renewal_Art_Phase1A_v01.zip`도 읽기 전용 비교했다. Grass Base/Fence Wood/Bush A는 대응 샘플과 해상도 및 디코딩 RGBA 픽셀이 완전 동일하다. 나무는 다른 캔버스의 파생 제작이므로 동일 파일로 판정하지 않는다.

따뜻한 녹색/갈색, 목재·수목·소품의 뚜렷한 픽셀 외형은 Contact Sheet에서 같은 방향으로 보인다(정적 관찰). 그러나 전체 납품의 미술 최종 승인은 샘플 승인과 별개다. 승인 대기: 잔디 밀도/반복 피로도, 길 상단 끝과 지면 연결, 수정 건물 조립/지붕 외곽, NPC와 수목의 체감 크기, 상점/치유소/남문 가독성, 이름/QuestMarker 대비, 안전하고 평온한 분위기, 카메라 지원 비율별 가독성. NPC가 없는 합성으로 캐릭터 조화를 PASS하지 않는다.

## 11. 독립 합성 증거

PNG를 Assets 밖 로컬 검수 폴더에만 합성했다. 아래 파일은 검수 보조물로 **Git Commit/Unity Import 대상이 아니며 공유 저장소에 동봉되지 않는다**. 문서의 수치/좌표/판정은 이 파일 없이도 읽을 수 있다. 합성은 Runtime 화면이 아니고 NPC/HUD/실제 Sorting/Collider 물리/카메라를 포함하지 않는다.

- [Manifest 원본 대응 — 중앙 잔디 오류](C:/Users/windows/.codex/visualizations/2026/10/09/01a11ec3-5c81-7c13-aed1-db24a0e98dff/village_v1/local_300_composite.png)
- [LOCAL 좌·우 대응 정정안 — 환경300개](C:/Users/windows/.codex/visualizations/2026/10/09/01a11ec3-5c81-7c13-aed1-db24a0e98dff/village_v1/corrected_mapping_300_composite.png)
- [건물 문/벽 순서2안 — 지붕 틈 유지](C:/Users/windows/.codex/visualizations/2026/10/09/01a11ec3-5c81-7c13-aed1-db24a0e98dff/village_v1/building_orders.png)
- [잔디3×3](C:/Users/windows/.codex/visualizations/2026/10/09/01a11ec3-5c81-7c13-aed1-db24a0e98dff/village_v1/grass_3x3.png)
- [잔디5×5](C:/Users/windows/.codex/visualizations/2026/10/09/01a11ec3-5c81-7c13-aed1-db24a0e98dff/village_v1/grass_5x5.png)
- [길 연결6×6](C:/Users/windows/.codex/visualizations/2026/10/09/01a11ec3-5c81-7c13-aed1-db24a0e98dff/village_v1/path_straight_6x6.png)
- [울타리5개](C:/Users/windows/.codex/visualizations/2026/10/09/01a11ec3-5c81-7c13-aed1-db24a0e98dff/village_v1/fence_5.png)

## 12. 후속 Unity 통합 계획과 남은 작업

1. 건물3부품의 접합/중첩 및 Roof 외곽을 기존좌표 합성으로 보정·재검수한다. 나머지15종 전체 재제작이나 길 PNG 재제작은 요구하지 않는다.
2. Manifest 좌·우 기존Sprite 대응을 위 LOCAL 매핑으로 정정하고 승인한다. 잔디 반복 PARTIAL은 샘플 동일성을 고려해 사용자 수용/보정 여부를 결정한다.
3. 사용자 미술 최종 승인 및 Main20 담당과 작업 충돌 해소 후 별도 구현 지시를 받는다. 이번 작업은 구현 착수 승인이 아니다.
4. 다음 작업 시작 시 HEAD/status/현재 LOCAL Scene·Prefab·Generator 재확인. 사용자 변경을 동결/보호하고 `_Project` 전용 환경 경로에 신규15PNG를 Import(PPU128/Center/Point/무압축/MipmapOff/Single). 원본 무료PNG/meta/GUID는 유지한다.
5. 환경Sprite 참조15종을 Scene284개 및 SouthGate 활성16개/비활성2개에 정확히 연결한다. 실제좌표/Scale/Sorting/Collider/Bounds/Spawn/Save 구조는 유지한다. 재생성 방지를 위한 Generator·남문Prefab 참조 변경만 별도 검토한다. NPC catalog/공용Resources는 유지한다.
6. 별도 통합 때 백그라운드 컴파일·참조 검증, 실제 동일Order 건물/피벗/카메라 검증, Main01/05/12·Side5·상점/치유·NPC 접근·남문 왕복·Save/Continue 회귀를 실행한다. foreground 필요 시 사용자 허락 없는 포커스 전환은 하지 않는다.

현재 **역할/크기 매핑15/15, 활성300개 참조 교체 설계 가능 / 전체 즉시 통합은 보류**다. 필수 수정은 건물 접합 및 길 대응표다. 신규 추가 아트는 현재15종 교체에 필수로 요청하지 않으며 Main12 오브젝트/길 모서리/시설 간판은 별도 승인 범위다.

## 13. 작업 보호 및 Git

이 문서만 Stage/Commit 대상이다. CURRENT_STATUS와 기존 v1/v2/Main20 및 StarterVillage Baseline 문서는 변경하지 않는다. 원본 ZIP 및 기존 tracked/미추적/Save/TTS 파일은 시작 시 SHA256 기준으로 종료 시 재검증한다. 기존 수정/미추적 목록을 reset/revert/삭제하지 않는다. Unity 관련 구현 파일 변경0개. GitHub Push는 수행하지 않는다. Commit SHA는 최종 작업 보고에 기록한다.

종료 전 보호 검증: 시작 보호 파일4,378개 SHA256 불일치0개, 원본 ZIP SHA256 동일, 기존 status 제거0행/추가1행(이 문서) 확인. 전체 사용자 기존 diff의 공백 경고는 수정하지 않고 Commit 대상 문서만 별도 diff-check한다.
