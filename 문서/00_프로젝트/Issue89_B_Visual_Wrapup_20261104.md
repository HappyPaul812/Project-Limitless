# Issue #89 B안 대표 시각 검토 마무리 — 2026-11-04

**B_LAYOUT_CONDITIONAL_ORIGINAL_APPLY_CANDIDATE / TEXT_ASSISTED_IDENTIFICATION_ACCEPTABLE_IN_CAPTURE / OFFICIAL_ICON_PENDING / VISUAL_PARTIAL_REMAINS / ORIGINAL_NOT_APPLIED**

[총괄 기획 결정 6624214599](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6624214599)을 읽었습니다. B의 후속 기준 채택을 유지하며 원본 적용은 별도 승인 전 보류합니다. 이번은 **기존 실제 Unity6000.6.5f1 캡처 재검토·문서화**입니다. Unity 실행0,50PASS·99PASS 재실행0,새 기능 검사PASS 추가0. 과거50/99는 각자의 기존 결과로 보존합니다.

## 바울 대표 확인용 실물

- [대표 B 비교판](Evidence/Issue89BVisualWrapup20261104/Issue89_B_Representative_ExistingUnity.png): 설원960/40 한기·동상·괴사,1280/128 동상·괴사,1680 HUD. 개요는 축소 표시이며 1:1 판단 자료와 구분합니다.
- [설원40px 세 상태·괴사1/2중첩 1:1 상세](Evidence/Issue89BVisualWrapup20261104/Issue89_B_Snow40_States_HUD_Native.png): 실제 HUD와 VFX·문자 크롭을 원픽셀 그대로 배치. 1.5배 HUD는 관찰 보조이며 실제 글자가 커졌다는 증거로 사용하지 않습니다.
- [열람판](Evidence/Issue89BVisualWrapup20261104/index.html), [소스 SHA](Evidence/Issue89BVisualWrapup20261104/PriorEvidenceSHA.json), [크롭 좌표·픽셀 일치](Evidence/Issue89BVisualWrapup20261104/CropProvenance.json), [파생 파일 SHA](Evidence/Issue89BVisualWrapup20261104/SHA256.json).

실제 PC 열람 경로 `F:/study/codex/Project-Limitless/Temp/Issue89BVisualWrapup20261104/index.html`. 원래 캡처는 `Temp/Issue89LayoutDelta20261104/Evidence/*_B.png`; 카메라/Canvas 직접 캡처를 재사용했으며 새 GameView 촬영·새 VFX 제작·승인PNG 편집이 아닙니다. 전장 설원은 기존 F2 격리 카메라 소스를 표시한 시험 배경입니다.

## 밝은 설원과 40px 판정

|항목|실제 캡처 관찰|판정|
|---|---|---|
|한기40px VFX|옅은 흰 안개/고리. 휠체어와 가시경계8px 분리되어 독립 위치는 보이나 설원과 단독 대비는 약함|VFX 단독 명확성 미완료|
|한기 이름·턴|VFX 아래 검은 외곽선의 `한기 2턴`, 어두운 HUD의 황색 `한기 2턴`이 둘 다 읽힘|문자 포함 상태 식별 가능(해당 정지 캡처)|
|동상40px|밝은 결정·가시 형태, `동상 2턴` 표시|문자 포함 식별 가능; 임시 소형 아이콘 단독은 보류|
|동결괴사40px|검푸른 결정·가시 형태, `동결괴사 1/3` 대 `동결괴사 2/3 · 다음 중첩 즉시 KO`|1/2중첩 문자로 구별 가능; 색에만 의존하지 않음|
|960 HUD|첫행 행동중/한기/동상,둘째행 괴사/굳건한자리. 한기·동상2턴과 괴사1/3 또는2/3! 보존|이름·턴·중첩을 담은 두행 기준 유지|
|임시 HUD 아이콘|14UI 약12.12화면px에 승인512 VFX를 그대로 축소하여 잔점처럼 보임|세 아이콘 단독 즉시구분 불충분,OFFICIAL_ICON_PENDING|

40px은 전장 VFX 전체 캔버스 크기입니다. HUD 아이콘이40px이라는 뜻이 아닙니다. 기존960 B의 선언글자14UI→약12.124화면px,자동축소off라는 측정값을 인용하며 이번에 새 측정을 실행하지 않았습니다. 2턴은 기존 합성 검증 상태값으로, 일반 플레이에서 남은 턴이 실시간 갱신되는 동작을 이번 정지 화면만으로 증명하지 않습니다. 한기·동상의 중첩 숫자를 새로 만들지 않고 기존 턴정보를 유지합니다. 괴사는1/3·2/3만 유지하며 지속3/3을 추가하지 않습니다.

판정은 정상 시력의 해당 캡처 관찰 범위입니다. 저시력·색각·스크린리더·다른 디스플레이에서 충분하다는 접근성 인증이 아닙니다. 동일 합성 HUD에 냉기3종을 함께 표시한 시험이므로 한 장에 서로 다른 세 상태가 모두 보입니다. 일반게임의 동시상태 규칙을 새로 확정한 것이 아닙니다.

## 정식 아이콘과 보완 범위

정식 독립 상태 아이콘 3종은 이번 납품/fixture에 없습니다. 기존 `Cold89LayoutProbe.cs`가 `ApprovedColdIconResolver`에 승인VFX3Sprite를 연결하는 **임시 QA 표시**입니다. 이름에 Approved가 있다고 정식 아이콘 승인을 뜻하지 않습니다. 기존 승인VFX를 정식 상태아이콘으로 승격하지 않았습니다.

현재960 B는 상태명·턴·중첩 텍스트가 읽히므로 추가 레이아웃/텍스트 변경 없이 기존 두행을 보존하는 것이 최소 변경입니다. 한기의 단독 대비와 정식 아이콘 미제작은 PNG를 재작화하거나 캡처 글자를 덧칠해서 해결됐다고 할 수 없어 남은 항목으로 기록합니다. 전장 `한기 2턴`과 HUD 이름/턴을 필수로 유지하고, 아이콘만 남기는 축약은 하지 않는 것을 원본 적용 후보의 조건으로 명시합니다.

1280/1680은 본체 뒤·하향24px VFX와1680 HUD경계8px를 기존 캡처에서 확인했습니다. 다만 넓은 화면의 기존 한행 HUD는 작은 글자/배지 간 밀집이 남아 있습니다. 이 확인을960 두행 수준의 가독성 PASS로 확대하지 않습니다. 필요 시 B 후속 표적 후보는 **960 두행/자동축소off 규칙을 해당 해상도의 상태영역에도 제한 적용하고 괴사 숫자와 다음KO 문구를 보전하는 배치·텍스트 델타**뿐입니다. 이번 사용자가 지정한 설원40px 이름/턴 확인은 추가실행 없이 가능했으므로 다른 해상도 UI를 임의 수정·재시험하지 않았습니다.

## 원본 적용 후보와 승인 게이트

**B 레이아웃은 문자 보조 표시를 유지하는 조건부 원본 적용 후보로 정리합니다.** 최종 완제품 시각PASS·정식아이콘완료·원본 적용승인으로 승격하지 않습니다. 바울 대표의 실물 최종 시각 판단/별도 원본 적용 승인을 기다립니다. 그 전까지 원본게임 적용0.

남은 항목: 정식 상태아이콘3종 미제작/미승인,한기VFX단독 설원대비,1280·1680 밀집HUD 문자,타체형/도보/3인 HUD/동적 부여·해제/성능/접근성. 전부 이번에 새로 구현하거나 전체QA로 반복하지 않았습니다.

## 보호·브랜치

기존99PASS Evidence101개·승인PNG3 SHA를 읽기 전용 대조해101/101·3/3 불변. 기존B Evidence33파일SHA도 보존하며 출력은 새폴더에만 작성했습니다. 원본 Editor/Unity/Save/UserData/Scene/Prefab/승인아트/사용자미커밋파일 수정0. 원본main634a91d3e4174c87761c0d377cfa5a695ca79e7f·tracked dirty308유지. Unity버전6000.6.5f1/3ff58d469c8a 고정,엔진실행0·포커스조작0.

브랜치 `codex/issue89-b-visual-wrapup-20261104`,직전d6aad98d29b701fe19e0858a24ee11a0cd6b9115. CODEX_LATEST/CURRENT_STATUS 갱신. 이번 실제commit은Issue89댓글에 기록합니다. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. main직접commit/Push/병합0. 원격에는 댓글·경로·SHA만 보고하며 이미지 업로드했다고 주장하지 않습니다.
