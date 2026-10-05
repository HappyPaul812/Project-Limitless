## 2026-10-06 Main03 008~012 최종 반영·연속 Runtime 사람 청취 PASS

008/010/011 새 원본·009 **NeFix 최종본**을 기존 Unity WAV 내용만 교체.012는 지정 원본과 이미 동일하여 재작성0/해결유지.5건 Source 사용자청취PASS + 이번 사용자 **“5개 모두 Runtime 청취 PASS”** 확인: 의미·전체문장·음량PASS,009 `네.` 포함.**TTS_REGEN_REQUIRED4→0**, NEEDS_LISTENING244 기존관리범위유지.331 Matrix에 완료flag 기록.005/006/007/003/004 기존PASS와001/002 보호,Player 정상무음.

LOCAL Runtime/Manifest 본문5건 줄바꿈 포함일치·Source/Unity WAV SHA동일·실제Unity PCM5개 전샘플오차0.실제 첫Battle→QA전용승리종료→정식복귀→008→Player→009→Player→010→011→012→단서001→002→Main03종료 연속PASS.전략전투/물리입력검증 아님.기존 QA 도구로 PlayUnfocused·격리Save/Settings 사용,포커스전환0.기술검사 227 PASS/FAIL0(7 TRACE·9 AUDIO는별도),Player Voice0/Portrait0·태온복원·UI Wrap/동적높이/본문잘림0/Footer·Portrait비중첩·끝까지Next/빠른Next/최종정리/Quest완료/격리자동Save읽기PASS.

[최종 적용QA](Main03_Supp008_012_Final_적용_QA.md)·[5건 Source/PCM 감사](Main03_Supp008_012_Final_Audit.csv)·[이번 연속 실행로그](Main03_Supp008_012_Final_Runtime_Results.txt).보호 3042파일 비교:변경은 Unity008~011 WAV4개뿐,meta/GUID/Importer/Catalog/게임·Editor C#/Save/Settings/Scene/Packages불변.종료cleanBootstrap EditMode/audit해제/격리설정·Play옵션복원.CompileError0/ConsoleError0·Warning0 조회;C#변경없어강제재컴파일0.초기 준비스크립트 실행실패 후 조기QA를중단했고 WAV교체후전체재실행PASS,조기실행은최종증거에포함하지않음.

다음 권장:244 관리범위의미확인Voice를별도청취.이번5건추가TTS불필요.마지막선행관련commit `e355b44c9b2a87b7072d504d95db33560ebf7b7c`,이번완료commit은최종보고참조.직접변경만Stage,원본TTS폴더/다른WAV Stage0·GitHubPush0.아래는이전이력이다.

# Main03 태온 008~012 최종 원본 반영·연속 QA (2026-10-06)

문서화→기존 WAV 내용 교체→격리 백그라운드 검증. LOCAL Source of Truth, GitHub Push/TTS API/새 Voice Design 없음.

008/010/011은 Supp008_011 폴더,009는 **Supp009_NeFix** 최종 원본을 사용한다.012는 Supp012 원본과 현재 Unity 파일이 같으면 재작성하지 않는다. 모두 제작 세션 사용자 원본 청취 PASS;009 첫 단어 `네.` 포함 전체 PASS. Runtime/Manifest5건 줄바꿈 포함 일치. 코드/본문/Catalog/ID/화자/Registry/meta/GUID/Importer 유지.005/006/007/003/004/001/002 및 Player 정상 무음 보호.

Editor 확인: clean Bootstrap EditMode, scene dirty=false, is_focused=false, compiling=false, audit directory=null. 기존 Main03RemainingVoiceAudit/Partial9FixedSpriteAudit의 PlayUnfocused·격리 Save/Settings·종료 복원 경로를 재사용한다. 사용자 저장에는 쓰지 않으며 Scene Asset 저장/foreground/OS 입력 없음.

실제 첫 조우→전투 진입→QA 전용 승리 종료/정식 결과 복귀→008→Player→009→Player→010→011→012→실제 단서001→002→Main03 완료를 한 Sequence로 검증한다. 각 Source/Resolve ID·참조·PCM 전체 샘플·재생 완료 대기·Next·Player Voice/Portrait0·UI 영역·끝부분·Quest/격리 자동 Save 읽기 확인. 전투 전략/물리 입력 검증은 아니다.

METADATA_PASS/USER_LISTENING_PASS/Runtime 재생 및 PCM PASS와 사람의 RUNTIME_LISTENING_PASS를 구분한다. 원본 사용자 청취와 PCM 일치로 동일 내용 재생을 증명해도 이번 사람이 Unity를 직접 들었다는 증거를 만들지 않는다.008~011 새 Runtime 의미 청취를 사용자가 확인하기 전에는 기존 확정 재생성4건을 완료로 닫지 않고 원본 적용 완료/Runtime 청취 대기로 둔다.012 기존 Runtime 의미PASS 유지.244 NEEDS_LISTENING 기존 관리 범위 유지.

## 반영/검증 결과

후속 결과는 아래에 기록한다.

## 최종 판정

위 계획의 Runtime 청취 대기는 해소됐다. 사용자5건 Runtime PASS 답변 채택, 재생성 필요0.

정적 Catalog Story229+Intro18=247: ID중복0/Clip GUID중복0. 대상5건 Missing0/Speaker mismatch0/Manifest Text mismatch0.012 byte동일 유지,009 Unity hash는 NeFix와 동일하며 Batch009와 다름.
