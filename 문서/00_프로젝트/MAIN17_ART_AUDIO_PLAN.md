# Main17 외부 Art / Battle BGM 적용 계획

2026-10-06 기준 dbaed55. 기존103변경·보상60/50·TTS14·밸런스44승리 보호.

1. Source ZIP(정확한 English.zip) 감사: PNG10/Manifest1, 실행 Art9 및 Preview1. 손상/중복 검사와 원본SHA256 기록. 실제 입력1254×1254를128×128이라고 추정하거나 PNG를Resize/Crop하지 않는다. 기존 PPU128/Point/무압축/Alpha·좌표·Collider 기준을 적용하며 새 Art는 Import/Renderer/Material만 조정한다.
2. 실제 Field08 시각 슬롯 교체. 깊은 틈 Collider와 북/남 우회로, Rock/Canyon 경계·Spawn·Exit·조사지점은 유지한다. 투명한 Prop과 불투명한 Terrain을 구분하고 Preview는Runtime미사용. 타일 경계는 원본 수정 대신 Renderer/UV/Wrap의 반복 정책으로 처리한다.
3. 실제 Blade_and_Gambit 원본을동일hash로Import. Chapter2 서부 일반/Story 배정, 기존Warden/전용Boss 예외 우선. Field07/08 원래탐색곡 유지. 기존단일Source/Mixer에서 Fade 전환0.20/0.75/0.9초를 구현하여 중복 시스템을 만들지 않는다.
4. 대표Main17 Runtime+Art/BGM영향범위QA, Field07/일반Encounter/Story/전용BossResolver·전환/단일Source/Loop. 3해상도배경캡처와Collider그림정합, Save/Continue보상·Party·Fox 보호. 기존5Job44승리·309/180PASS를재사용.
5. Art / Audio / 최종QA 문서로 commit분리. 외부대기는실제적용·검증뒤에만DELIVERED/APPLIED로 바꾼다. 사람음악청취2건·실물입력3건·TTS14는별도대기. Main18/Field09/Elite/Overheat/새TTS/음악생성/PNG수정/Push0.
