using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.UI;

namespace ProjectLimitless.World
{
    /// <summary>Main18 본문과 충돌 없는 Stable ID입니다. Player/지문은 무음이며 NPC 음성은 새로 생성하거나 연결하지 않습니다.</summary>
    public static class Main18DialogueCatalog
    {
        public static DialogueLine[] Get(int index, string path = null, bool allCompanions = false)
        {
            path = path ?? GameSessionData.SelectedPlayerPathId;
            var lines = new List<DialogueLine>();
            string site = index == 13 ? "closed_route" : Chapter2Main18Flow.Targets[index].Replace("field09_main18_", "").Replace("arbel-", "");
            void Add(string speaker, string text, string branch = "")
            {
                string id = speaker == "serin" ? CompanionRosterService.SerinId : speaker == "taeon" ? CompanionRosterService.TaeonId
                    : speaker == "miel" ? CompanionRosterService.MielId : speaker == "paul" ? CompanionRosterService.PaulId
                    : speaker == "leon" ? "arbel-leon" : speaker;
                string name = speaker == "player" ? GameSessionData.PlayerName : speaker == "serin" ? "세린"
                    : speaker == "leon" ? "레온" : speaker == "taeon" ? "태온" : speaker == "miel" ? "미엘" : speaker == "paul" ? "폴" : "";
                lines.Add(new DialogueLine(id, name, text, "main18_" + site + "_" + (branch == "" ? "" : branch + "_")
                    + (speaker == "" ? "direction" : speaker) + "_" + (lines.Count(x => x.SpeakerId == id) + 1).ToString("00")));
            }
            switch (index)
            {
                case 1:
                    Add("leon", "붉은 균열 안쪽까지 들어가셨군요. 상태부터 말씀해 주세요.");
                    Add("player", "바위가 검게 굳어 있고, 틈 안쪽에서 열이 계속 올라옵니다.");
                    Add("serin", "진동도 끊기지 않았어요. 오히려 더 깊은 쪽에서 간격이 짧아졌습니다.");
                    Add("leon", "그렇다면 준비 없이 다시 들어가면 안 되겠군요.");
                    Add("leon", "열이 몸에 남을 때 쓰는 냉각약을 준비했습니다. 세 병은 먼저 가져가세요.");
                    Add("leon", "더 필요하면 잡화 상인에게 구할 수 있게 해두겠습니다."); break;
                case 2:
                    Add("", "더 깊은 곳은 열기가 너무 강합니다. 아르벨에서 준비를 마쳐야 합니다."); break;
                case 4:
                    Add("", "<지문> 검게 굳은 지면 사이에 매끄러운 흑요석 판이 이어진다. 깊은 틈 안쪽에서 약한 붉은빛이 새어 나온다.");
                    Add("player", "돌은 식어 굳었지만 안쪽의 열은 남아 있군요.");
                    Add("serin", "지면 아래 반응은 계속됩니다. 원인과 표면의 변화는 구분해서 기록하죠."); break;
                case 5:
                    Add("", "<지문> 좁은 틈에서 뜨거운 공기가 간헐적으로 올라온다. 가까운 흑요석 갑충이 몸을 돌린다.");
                    Add("serin", "열이 올라오는 간격이 짧아요. 물러날 공간부터 확보하겠습니다."); break;
                case 7:
                    Add("serin", "방금 공격은 상처보다 몸에 열을 남기는 쪽이었어요.");
                    Add("player", "한 번에 끝나는 열이 아니군요.");
                    Add("serin", "네. 열이 겹쳐 쌓입니다.");
                    Add("serin", "세 번째까지 쌓이면 한꺼번에 터져요. 두 겹이 보이면 냉각약을 쓸지 판단하는 게 좋겠습니다.");
                    Add("player", "정화로 없앨 수 있는 상태와는 다릅니까?");
                    Add("serin", "네. 이 열은 정화로는 빠지지 않습니다."); break;
                case 8:
                    string observation = path == "path.vision" ? "붉은빛이 강해지는 균열과 지면이 흔들리는 위치가 이어져 있습니다."
                        : path == "path.hearing" ? "낮은 울림이 두 번씩 반복됩니다. 서쪽으로 갈수록 간격이 짧아집니다."
                        : path == "path.intellectual" ? "열기 분출과 진동의 간격이 일정합니다. 무작위 현상은 아닌 것 같습니다."
                        : path == "path.mobility" ? "깊은 균열 사이에도 무게를 버틸 수 있는 검은 판들이 이어져 있습니다."
                        : "갑작스러운 열기가 반복되지만 패턴을 알고 나니 다음 분출을 예상할 수 있습니다.";
                    Add("player", observation, path.Replace("path.", "").Replace('-', '_'));
                    Add("serin", "저도 같은 방향을 보고 있었어요. 더 깊은 곳으로 갈수록 반응이 빨라집니다."); break;
                case 9:
                    Add("serin", "저 앞의 개체, 주변 열기가 움직임에 맞춰 올라갑니다.");
                    Add("serin", "앞의 갑충보다 열을 모으는 속도가 훨씬 빨라요.");
                    Add("player", "지나가려면 상대해야겠군요.");
                    if (allCompanions || CompanionRosterService.IsActivePartyMember(CompanionRosterService.TaeonId))
                        Add("taeon", "한 사람에게 열이 몰리지 않게 상태를 계속 확인하겠습니다.");
                    if (allCompanions || CompanionRosterService.IsActivePartyMember(CompanionRosterService.MielId))
                        Add("miel", "두 겹까지 쌓인 사람부터 보세요. 필요하면 바로 식혀야 합니다.");
                    if (allCompanions || CompanionRosterService.IsActivePartyMember(CompanionRosterService.PaulId))
                        Add("paul", "광역 열기와 과열은 별개군요. 숫자부터 놓치지 않으면 되겠습니다."); break;
                case 11:
                    Add("serin", "쓰러뜨렸는데도 주변 열기가 줄지 않아요.");
                    Add("player", "이 녀석이 원인은 아니군요.");
                    Add("serin", "네. 원인이라기보다 수문장에 가까워 보여요.");
                    Add("serin", "진동도 계속 더 깊은 곳에서 올라옵니다."); break;
                case 12:
                    Add("serin", "진동이 저 아래에서 올라옵니다.");
                    Add("serin", "이제 중심부가 멀지 않은 것 같아요.");
                    Add("player", "다음에는 더 깊이 들어가야겠군요."); break;
                case 13: Add("", "더 깊은 열기 때문에 지금은 지나갈 수 없습니다."); break;
            }
            return lines.ToArray();
        }
    }
}
