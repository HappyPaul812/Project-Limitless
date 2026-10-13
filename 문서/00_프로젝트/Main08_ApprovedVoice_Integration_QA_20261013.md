# Main08 승인 기존 음성 정식 연결·적용 QA — 2026-10-13

**APPROVED_SIX_APPLIED_TO_BRANCH_AND_ORIGINAL / 기술177 PASS·FAIL0 / 전체 QA PARTIAL.** 바울 대표의 직접 청취 “전부 정상”과 최신 docs/planning `ef8ec03e30f3004a8bbcae6851ba8530ef6a4139`의 승인 기록을 기준으로 기존복구후보6개를 실제 게임Catalog에 연결했습니다. 브랜치 `codex/main08-approved-voice-apply-20261013`, 후보검증 기준9cfbe9e. **원본 프로젝트 디스크에도 반영 완료**, 원본 main HEAD는ff022e6 유지, 직접Commit/Push/병합0입니다. 신규TTS/API0, 원본Editor Play/포커스 강제조작0입니다.

## 승인 집합과 실제 연결

| Stable ID | 정본 및 승인 발화 | 원음 재사용/분리 | 런타임 |
|---|---|---|---|
| `main08_taeon_supp_002` | 여기부터 흔적이 거의 없습니다. | 기존 main08_taeon_supp_003 동일 바이트 복사 | GUID/경로/PCM PASS |
| `main08_taeon_supp_003` | 평소라면 그렇겠죠. | 기존 main08_taeon_supp_004 동일 바이트 복사 | GUID/경로/PCM PASS |
| `main08_miel_supp_001` | 몬스터가 적다는 건 좋은 일 아닌가요? | 기존 main08_miel_supp_002 동일 바이트 복사 | GUID/경로/PCM PASS |
| `main08_taeon_supp_004` | 그런데 조금 전까지 이쪽으로 이어지던 흔적들이 전부 방향을 바꾸고 있습니다. | 기존 main08_taeon_supp_005 동일 바이트 복사 | GUID/경로/PCM PASS |
| `main08_miel_supp_002` | 없어진 게 아니네요. | 미엘003 합본의 승인된 무손실 분리 | GUID/경로/PCM PASS |
| `main08_miel_supp_003` | 다들 이쪽을 피해서 지나가고 있어요. | 미엘003 합본의 승인된 무손실 분리 | GUID/경로/PCM PASS |

- 원본 출처와 교체 대상ID가 겹치므로 **기존 WAV를 덮어쓰는 방식으로 납품하지 않았습니다.** 승인본6개를 `Assets/_Project/Audio/Voice/Story/Main08/ApprovedRecovery20261013`에 등록하고 새GUID로Catalog 해당6행만 교정했습니다. 안정ID·화자·fileID8300000은그대로입니다. Catalog의 새GUID를 역치환하면 기존본문과동일(줄바꿈 정규화만)이며 다른Catalog항목은변경0입니다.
- 새 WAV6개는 청취 승인 후보의 SHA와 완전히 동일합니다. 신규meta6개와폴더meta1개를추가했고, 기존원본WAV19개·출처제작팩·기존GUID/meta는 **바이트 변경0**입니다. 이를 통해 태온003/004·미엘002/003을 원음출처로 재사용하면서 그 원래파일도보호했습니다. 기존태온005 원음도유지합니다. Game C#·Scene·대사·Quest·Save규격·BGM변경0입니다.
- 실제변경파일: 위새폴더의6WAV/7meta, `Unity/Client/Assets/_Project/Resources/Audio/Voice/Story/StoryVoiceCatalog.asset`의6GUID참조, 관련QA/상태/인계문서입니다. [ID별원래GUID·새GUID·원음SHA·승인SHA](Evidence/Main08_ApprovedApply_20261013/Main08_Approved_Applied_Matrix.csv).

## 새 최종 등록 Runtime 실행

- Unity **6000.6.5f1**, 격리 프로젝트 `Temp/IntegratedQA20261013/Client`, 실행결과 `Temp/Main08Approved20261013/Runtime`. Catalog가참조한 **실제AudioClip.GetData의PCM16SHA**를청취승인본과대조해6/6 PASS, 실제GUID/fileID/새등록경로6/6 PASS입니다. 후보파일만읽고재생을가정한것이아닙니다.
- Trace01~03의 **9개음성+Player1페이지** 전체를실제Interact/DialoguePresenter로재생·진행했습니다. 자막·화자·초상화Sprite·ClipID·자연재생종료·자동Next없음, 빠른Next단일전환·이전Clip정리·단일VoiceSource·Pause/Resume·Player무음·Quest목표실제전진·SaveVersion1·실제BootstrapContinue의Quest snapshot복원·잔류Clip0이PASS입니다. 승인6개와기존경계태온001의발화정합은기존청취/ASR/PCM자료와대조했습니다.
- Voice100/35/0·MasterMute·2D Sourcevolume1/VoiceGroup·단일MixerGain PASS. Listener.volume0은검증복사본출력만막아스피커를울리지않기위한것입니다. 바울대표의최종청취승인과별개로원본실행중스피커출력을직접청취했다고보고하지않습니다. 원본Audio설정/UserData는쓰지않았습니다.
- 물리키보드/패드실물조작은NOT_VERIFIED: 격리Presenter의자동AdvanceAction만꺼서사용자외부입력과분리했고, 실제공용AdvanceFromInput 경로를직접호출해같은프레임중복/빠른Next를검사했습니다. 이동고정·일반조우Suppress3600초는QA복사본의결정적대화fixture입니다. **실제게임에서조우를막는코드를추가한것이아닙니다.**
- 정본Unity6000.5.7f1 실행은NOT_VERIFIED, 기존UnityEditor.Search 예외로Console전체PARTIAL이며최종run신규Runtime오류0/Compile오류0입니다. 최초장시간probe 실패와일반조우중단probe는Temp에보존하고,최종동일GUID/PCM 등록에서재실행177PASS를근거로삼았습니다.

## 반드시 구분할 기존 OPEN/미승인 범위

1. **태온005/006은승인6개에없고기존발화불일치가남아있습니다.** 005정본은“초원에서는무언가를피해밀려오는것처럼보였습니다.”이나현재WAV의이전ASR는태온004긴문장입니다. 006정본은“여기서는그방향이조금더분명합니다.”이나현재WAV에는005+006 두문장이함께검출됐습니다. 이번에도그기존Clip을재생하고양파일이변경되지않았음을확인했습니다. 따라서 **Trace03모든발화내용PASS라고보고하지않으며의미미해결2건OPEN**입니다. 추가분리·재사용후보청취승인이별도필요하고임의교체0입니다.
2. 최종등록첫장시간검증에서일반몬스터조우로 **대화중Battle 전환**이발생했습니다. 이후이미파괴된Source에QAhelper가접근한실패이며소스음성/새Catalog GUID 결함이아닙니다. 기존MonsterEncounterService.TryRaise에는일반WorldModal차단이없고C#은이번전후동일했습니다. 대화중일반조우정책/중단보호는별도기술OPEN으로기록하며이번에전투규칙을임의변경하지않았습니다. 음성 QA의조우억제fixture가이실물행동문제를해결했다는뜻은아닙니다.
3. **실행중인원본Editor메모리의즉시재Import/Clip갱신은NOT_VERIFIED.** 디스크에는6WAV·새meta·Catalog가실제반영됐습니다. 원본Play를멈추거나강제Refresh/Scene재로드/포커스전환하지않았습니다. 바울대표의다음정상적인Editor Import/Play 재시작후반영상태를확인할수있으며기존Play캐시를강제로바꾸지않았습니다.

## 원본 보호·복구

- 목적지/출처중복을합친원음7WAV+7meta와Catalog+meta 총16개를 **적용전에전체바이트백업**, 승인입력6WAV도별도보관했습니다. [백업Manifest](Evidence/Main08_ApprovedApply_20261013/backup_manifest.json), 실제복구폴더 `Temp/Main08Approved20261013/OriginalBackup/AssetOriginal`, `CatalogOriginal`, `ApprovedInput`.
- 기존보호 5538파일SHA동일, 기존WAV·GUID/meta/Save/UserData/설정/Scene/BGM/TTS·사용자미커밋파일변경0. **허용된기존파일변경은Catalog1개뿐**, 추가파일은승인6WAV+7meta뿐입니다. 원본main HEAD변경0, 원래미커밋항목모두유지. 새Catalog변경과새등록폴더만원본에미커밋으로추가됐고원본에서stage/Commit하지않았습니다.
- 원복이필요하면기존Catalog를`OriginalBackup/CatalogOriginal/StoryVoiceCatalog.asset`에서복구할수있습니다. 신규승인등록파일은원음과별도이므로원복Catalog는원래GUID를해석합니다. 이원복은이번에실행하지않았고현재는승인본연결상태입니다.
- 최종판정 **승인6개정식적용완료 / 기술PASS / 전체PARTIAL(별도태온005·006·일반조우중단·정본버전/실물검증)**. 신규TTS생성필요없음. 후속은추가경계2음성청취후처리결정및대화중일반조우정책검토입니다. 자동Push/main병합은하지않습니다.

[177개새검사](Evidence/Main08_ApprovedApply_20261013/checks.txt), [원본실제반영·보호](Evidence/Main08_ApprovedApply_20261013/root_applied.json), [최종등록Manifest](Evidence/Main08_ApprovedApply_20261013/registered_manifest.json), [QA실행코드](Evidence/Main08_ApprovedApply_20261013/Main08ApprovedVoiceAudit.cs.txt).
