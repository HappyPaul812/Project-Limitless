using ProjectLimitless.Core;
using ProjectLimitless.UI;
namespace ProjectLimitless.World
{
    /// <summary>승인 원문/고유 ID를 보존합니다. 미제작 음성은 텍스트로 진행합니다.</summary>
    public static class Main21DialogueCatalog
    {
        public static DialogueLine Line(string id)
        {
            switch(id)
            {
                case "main21_heat_serin_01": return new DialogueLine("companion_serin", "세린", "바람이 달라졌어요. 아직 뜨겁긴 하지만, 전처럼 밀어붙이는 느낌은 아닙니다.", "main21_heat_serin_01");
                case "main21_pulse_serin_01": return new DialogueLine("companion_serin", "세린", "울림은 남아 있어요. 하지만 간격이 훨씬 길어졌습니다.", "main21_pulse_serin_01");
                case "main21_pulse_player_01": return new DialogueLine("player", "플레이어", "아직 위험한 곳이 있는지 확인해야겠군요.", "main21_pulse_player_01");
                case "main21_route_paul_01": return new DialogueLine("companion_paul", "폴", "가장 짧은 길이 꼭 안전한 길은 아니겠죠. 바닥부터 확인해 봅시다.", "main21_route_paul_01");
                case "main21_route_serin_01": return new DialogueLine("companion_serin", "세린", "중앙 쪽은 아직 일정한 간격으로 흔들립니다.", "main21_route_serin_01");
                case "main21_route_paul_02": return new DialogueLine("companion_paul", "폴", "암반 쪽은 겉만 식었습니다. 보급 마차가 지나가기엔 위험하겠어요.", "main21_route_paul_02");
                case "main21_route_player_01": return new DialogueLine("player", "플레이어", "옛 우회로가 조금 멀어도, 사람들이 안전하게 지나갈 수 있겠군요.", "main21_route_player_01");
                case "main21_route_taeon_01": return new DialogueLine("companion_taeon", "태온", "여긴 지난번에 흔들리던 곳과 다릅니다. 제가 다시 확인하겠습니다.", "main21_route_taeon_01");
                case "main21_arbel_leon_01": return new DialogueLine("arbel-leon", "레온", "주민들이 지날 수 있는 길을 찾아주셨군요. 정말 큰 도움이 됐습니다.", "main21_arbel_leon_01");
                case "main21_aid_miel_01": return new DialogueLine("companion_miel", "미엘", "다치신 분들은 제가 살펴볼게요. 폴 씨는 물자 수량을 확인해 주시겠어요?", "main21_aid_miel_01");
                case "main21_aid_paul_01": return new DialogueLine("companion_paul", "폴", "이미 확인했습니다. 이번에는 다행히 약보다 붕대가 더 많이 필요하겠네요.", "main21_aid_paul_01");
                case "main21_aid_miel_02": return new DialogueLine("companion_miel", "미엘", "그 말이 이렇게 반가울 줄은 몰랐네요.", "main21_aid_miel_02");
                case "main21_carriage_leon_01": return new DialogueLine("arbel-leon", "레온", "초보 마을로 돌아갈 마차를 마련했습니다. 준비되셨으면 출발하시죠.", "main21_carriage_leon_01");
                case "main21_north_direction_01": return new DialogueLine("", "", "<지문> 서부의 열기와는 다른 차가운 바람이 마을 북쪽에서 불어옵니다.", "main21_north_direction_01");
                case "main21_haren_haren_01": return new DialogueLine("story_haren", "하렌", "태온. 너 아직 멀었어.", "main21_haren_haren_01");
                case "main21_haren_taeon_01": return new DialogueLine("companion_taeon", "태온", "형? 오랜만에 만나서 첫마디가 그거야?", "main21_haren_taeon_01");
                case "main21_haren_haren_02": return new DialogueLine("story_haren", "하렌", "다친 데는 없고?", "main21_haren_haren_02");
                case "main21_haren_taeon_02": return new DialogueLine("companion_taeon", "태온", "응. 괜찮아.", "main21_haren_taeon_02");
                case "main21_haren_haren_03": return new DialogueLine("story_haren", "하렌", "그럼 됐어.", "main21_haren_haren_03");
                case "main21_prepare_serin_01": return new DialogueLine("companion_serin", "세린", "이번엔 북쪽이군요. 먼저 길부터 살펴봐야겠어요.", "main21_prepare_serin_01");
                case "main21_prepare_player_01": return new DialogueLine("player", "플레이어", "준비가 끝나면 출발하죠.", "main21_prepare_player_01");
                case "main21_pulse_hearing_player_01": return new DialogueLine("player", "플레이어", "잠깐 멈췄다가 다시 오는 간격이 전보다 길어졌어요.", "main21_pulse_hearing_player_01");
                case "main21_route_mobility_player_01": return new DialogueLine("player", "플레이어", "폭과 경사, 바닥을 함께 확인해야겠어요. 휠체어와 보급 마차가 모두 지날 수 있도록요.", "main21_route_mobility_player_01");
                default: throw new System.ArgumentException("Main21 대사 ID 누락: "+id);
            }
        }
        public static DialogueLine[] Get(int index)
        {
            switch(index)
            {
                case 0: return new[] { Line("main21_heat_serin_01") };
                case 1: return GameSessionData.SelectedPlayerPathId == "path.hearing" ? new[] { Line("main21_pulse_hearing_player_01"), Line("main21_pulse_serin_01"), Line("main21_pulse_player_01") } : new[] { Line("main21_pulse_serin_01"), Line("main21_pulse_player_01") };
                case 3: return GameSessionData.SelectedPlayerPathId == "path.mobility" ? new[] { Line("main21_route_mobility_player_01"), Line("main21_route_paul_01"), Line("main21_route_serin_01"), Line("main21_route_paul_02") } : new[] { Line("main21_route_paul_01"), Line("main21_route_serin_01"), Line("main21_route_paul_02") };
                case 4: return new[] { Line("main21_route_player_01") };
                case 6: return new[] { Line("main21_route_taeon_01") };
                case 7: return new[] { Line("main21_arbel_leon_01") };
                case 8: return new[] { Line("main21_aid_miel_01"), Line("main21_aid_paul_01"), Line("main21_aid_miel_02") };
                case 9: return new[] { Line("main21_carriage_leon_01") };
                case 10: return new[] { Line("main21_north_direction_01") };
                case 11: return new[] { Line("main21_haren_haren_01"), Line("main21_haren_taeon_01"), Line("main21_haren_haren_02"), Line("main21_haren_taeon_02"), Line("main21_haren_haren_03") };
                case 12: return new[] { Line("main21_prepare_serin_01"), Line("main21_prepare_player_01") };
                default: return new DialogueLine[0];
            }
        }
    }
}
