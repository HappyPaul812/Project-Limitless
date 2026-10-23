# 대표 일반 첨부 재대조 계약

대상은 실제 감시기가 이미 수령한 Discord 대표 계정 일반 PNG 1건입니다. 새로운 업로드나 수동 업무 생성으로 과거 사실을 대체하지 않습니다. 실제 Discord 작성자(사람·허용 owner)·서버·채널·첨부 ID·원래 자동 보고를 공식 SDK로 대조하고, 원본/저장 바이트의 SHA·PNG CRC·규격을 확인한 뒤 해당 업무만 TEST_ONLY로 표시합니다. 기존 3건은 바꾸지 않습니다.

TEST_ONLY는 payload와 evidence에 영속 기록하고 approve에서 명시적으로 거부합니다. 기존 업무 BLOCKED/NOT_VERIFIED/USER_REVIEW_PENDING 유지, 원래 수령 이벤트는 수정하지 않습니다. 같은 이벤트의 재대조는 새 업무나 같은 결과 보고를 중복 생성하지 않습니다. 시험 결과는 기존 아트-납품 채널에 별도 TEST_ONLY 보고로 전송합니다. 실제 AI/Unity/제품/Push/merge는 없습니다.

Windows 자동 시작은 조사/설계만 수행하고 등록하지 않습니다.
