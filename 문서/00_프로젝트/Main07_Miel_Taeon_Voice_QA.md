# Main07 기존 미엘·태온 Voice 감사와 PCM 복구

## 2026-10-07 최신 정본

미엘의 실제 청취 증거를 기존 원본과 독립 오프라인 ASR로 확인했다. Stable ID/본문/Catalog/GUID 순서는 정상이며 변경하지 않는다. 원인은 기존 chunk의 **WRONG_SEGMENT_BOUNDARY / SEGMENT_INDEX_SHIFT**다. 과거 생성 코드가 없어 실제 API 호출·분할 알고리즘을 특정하지 않는다.

|정식 ID|Expected Text|기존 실제 PCM|복구 원본/방법|
|---|---|---|---|
|main07_miel_001|돌아오셨네요. 그리고… 처음 보는 분도 계시네요.|돌아오셨네요.|PreSerin/miel/main07_miel_001 + 002 전체 rawPCM 연결|
|main07_miel_002|혹시 다치신 곳은 없으세요?|그리고… 처음 보는 분도 계시네요.|PreSerin/miel/main07_miel_003 전체 rawPCM 재배치|
|main07_miel_003|저분은 이제 괜찮으세요. 그러니까 폴 씨도 확인해야죠.|혹시 다치신 곳은 없으세요?|PreSerin/miel/main09_miel_002의 0~4.72초 sample slice|

정식003의 원본은 다른 Main 이름의 파일에 들어 있으며 뒤에는 “빛은 이쪽에서 보였어요”가 있다. ASR 첫 페이지 종료4.56초/다음 페이지 시작5.50초와 RMS -45dB·10ms 기준 긴 무음4.44~4.99초를 대조하여4.72초를 선택했다. 새 무음/교차 페이드/재인코딩/리샘플링은 하지 않는다. 원본과 수정 전6개 사본은 보호한다. Main09 Unity WAV는 변경하지 않는다.

태온001 “바퀴가 빠지셨는데요.”,002 “옵니다.”,003 “저희도 같은 현상을 따라 여기까지 왔습니다.”는 각각 해당 페이지와 대응한다. ASR의 문장부호와 “현생” 인식 오차를 발화 오류로 단정하지 않고 **태온 WAV 변경0**이다. Early7와 Paul20은 이 작업의 복구 대상이 아니며 byte 보호한다.

[원본 형식·길이·PCM SHA·무음·본문 감사](Main07_Miel_Taeon_Original_PCM_Audit.json) · [원본 ASR 단어 경계](Main07_Miel_Taeon_Original_ASR.json) · [복구 sample·SHA 증거](Main07_Miel_PCM_Recovery_Proof.json) · [복구 후 ASR](Main07_Miel_Taeon_Recovered_ASR.json).

복구 후 독립 ASR은3개가 각각 Expected Text와 대응한다. 복구001 ASR의 “하...”는 사람 청취 없이 실제 불필요 발화라고 확정하지 않는다. 사람 청취 확인 대기와 Runtime 기술 검증은 구분한다. 신규 TTS API0/재생성 필요0이며 청취가 추가 오류를 밝히면 재평가한다.

최종 Runtime과 청취 결과는 [통합 QA](Main07_Integrated_Retry_Growth_Voice_QA.md)에 기록한다. 이전 Paul/Early7 QA 기록을 삭제하거나 기존244개를 일괄 청취 통과로 승격하지 않는다.
