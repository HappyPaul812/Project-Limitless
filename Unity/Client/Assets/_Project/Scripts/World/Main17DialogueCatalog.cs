using System.Collections.Generic;
using ProjectLimitless.Core;
using ProjectLimitless.UI;

namespace ProjectLimitless.World
{
    /// <summary>Main17의 고정 본문과 ID입니다. 세린은 현장 조사자, 다른 동료는 실제 활성 명단에 있을 때만 발화합니다.</summary>
    public static class Main17DialogueCatalog
    {
        private static DialogueLine Line(int index, string speaker, string text, int number = 1)
        {
            string name = speaker == "player" ? GameSessionData.PlayerName : speaker == CompanionRosterService.SerinId ? "세린" : speaker == CompanionRosterService.TaeonId ? "태온" : speaker == CompanionRosterService.MielId ? "미엘" : speaker == CompanionRosterService.PaulId ? "폴" : "";
            string token = speaker == "" ? "direction" : speaker.Replace("companion_", "");
            return new DialogueLine(speaker, name, text, "main17_" + Chapter2Main17Flow.Sites[index] + "_" + token + "_" + number.ToString("00"));
        }
        public static DialogueLine[] Get(int index)
        {
            var lines = new List<DialogueLine>();
            switch (index)
            {
                case 1:
                    lines.Add(Line(index, "", "<지문> 붉은 흙 사이로 길게 갈라진 틈이 이어진다. 틈 가까이에는 검게 식은 돌과 짙은 재가 남아 있다."));
                    lines.Add(Line(index, "player", "황야보다 틈이 깊어요. 발을 디딜 바닥부터 확인하죠.")); break;
                case 2:
                    lines.Add(Line(index, CompanionRosterService.SerinId, "바닥에서 반복되는 신호가 잡힙니다. 황야보다 강하지만, 같은 간격인지는 더 비교해야겠어요."));
                    lines.Add(Line(index, "player", "재가 움직이는 때도 함께 기록하죠.")); break;
                case 3:
                    lines.Add(Line(index, "", "<지문> 협곡 안쪽으로 향하던 발자국이 균열 앞에서 북쪽과 남쪽으로 갈라진다."));
                    lines.Add(Line(index, "player", "이곳을 피해 돌아간 흔적이군요. 우리도 바닥이 이어지는 쪽으로 가죠.")); break;
                case 4:
                    lines.Add(Line(index, CompanionRosterService.SerinId, "황야에서 잡힌 신호를 이곳에서도 기록하고 있습니다. 혼자 확인한 것보다 함께 비교하면 차이가 분명하겠네요."));
                    lines.Add(Line(index, "player", "저희도 서쪽으로 갈수록 진동이 강해지는 걸 확인했어요.")); break;
                case 5:
                    lines.Add(Line(index, CompanionRosterService.SerinId, "진동이 올 때 재도 움직입니다. 황야와 비슷해요. 무엇이 먼저인지, 어디서 시작됐는지는 아직 모르겠습니다."));
                    lines.Add(Line(index, "player", "같은 현상일 가능성은 기록하되 원인은 구분해서 남겨 두죠.")); break;
                case 6:
                    lines.Add(Line(index, CompanionRosterService.SerinId, "서쪽 틈 가까이에서 더 강해집니다. 표면의 흔들림 뒤에 약한 신호가 한 번 더 잡혀요."));
                    lines.Add(Line(index, "player", "아래쪽의 반응도 따로 기록하죠. 발생 지점이라고 단정할 수는 없어요.")); break;
                case 7:
                    lines.Add(Line(index, "", "<지문> 균열 아래에서 따뜻한 공기가 올라온다. 재가 흩어지며 균열도마뱀이 길을 막는다."));
                    lines.Add(Line(index, CompanionRosterService.SerinId, "돌아갈 길을 확보하죠. 싸우는 동안 틈 가장자리는 피하겠습니다.")); break;
                case 9:
                    lines.Add(Line(index, "", "<지문> 길을 막던 생물이 사라져도 지면의 반복 신호와 틈의 열기는 남아 있다."));
                    lines.Add(Line(index, CompanionRosterService.SerinId, "아직 계속됩니다. 처치로 이 현상이 멎지는 않았어요."));
                    lines.Add(Line(index, "player", "황야에서와 같군요. 전투 결과와 지면 변화는 나눠 기록하죠.")); break;
                case 10:
                    lines.Add(Line(index, "", "<지문> 서쪽 끝의 깊은 균열 너머에 검고 매끄러운 암석이 드러난다. 아래쪽으로 이어진 경사는 끝을 확인할 수 없다."));
                    lines.Add(Line(index, CompanionRosterService.SerinId, "더 아래쪽 반응이 있습니다. 하지만 지금 위치만으로 안전한 경로를 판단할 수는 없겠어요.")); break;
                case 11:
                    lines.Add(Line(index, "player", "오늘 확인한 사실부터 정리하죠. 내려갈 준비를 갖춘 뒤 다시 살펴보겠습니다."));
                    lines.Add(Line(index, CompanionRosterService.SerinId, "동의합니다. 기록은 충분하고, 돌아갈 길도 남아 있네요.")); break;
            }
            // 대화 참여를 위해 저장 파티를 바꾸지 않습니다. 활성 동료의 짧은 확인만 덧붙입니다.
            if (index == 11 && CompanionRosterService.IsActivePartyMember(CompanionRosterService.TaeonId)) lines.Add(Line(index, CompanionRosterService.TaeonId, "우선 안전한 곳으로 돌아가겠습니다. 준비 없이 더 들어가는 것은 위험합니다."));
            if (index == 3 && CompanionRosterService.IsActivePartyMember(CompanionRosterService.MielId)) lines.Add(Line(index, CompanionRosterService.MielId, "가장자리에 남은 흔적도 피해서 돌아가요."));
            if (index == 5 && CompanionRosterService.IsActivePartyMember(CompanionRosterService.PaulId)) lines.Add(Line(index, CompanionRosterService.PaulId, "확인한 것과 추측한 것을 섞지 않으면, 다음에는 비교가 쉬워지겠네요."));
            return lines.ToArray();
        }
    }
}
