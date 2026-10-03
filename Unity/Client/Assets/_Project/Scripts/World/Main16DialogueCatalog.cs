using ProjectLimitless.Core;
using ProjectLimitless.UI;

namespace ProjectLimitless.World
{
    /// <summary>Main16의 확정된 본문과 별도 분기 ID입니다. Voice가 없어도 기존 수동 Next 대화로 진행합니다.</summary>
    public static class Main16DialogueCatalog
    {
        private static DialogueLine Line(string id, string speaker, string text)
        {
            string name = speaker == "player" ? (string.IsNullOrWhiteSpace(GameSessionData.PlayerName) ? "플레이어" : GameSessionData.PlayerName)
                : speaker == "companion_serin" ? "세린" : speaker == "companion_taeon" ? "태온"
                : speaker == "companion_miel" ? "미엘" : speaker == "companion_paul" ? "폴"
                : speaker == "arbel-leon" ? "레온" : "현장 관찰";
            return new DialogueLine(speaker, name, text, id);
        }

        /// <summary>첫 발견자의 우선권만 분기합니다. 완료 콜백은 같은 Target ID를 통지하므로 Quest 결과는 같습니다.</summary>
        public static DialogueLine[] Get(string scene, bool hearing)
        {
            switch (scene)
            {
                case "start": return new[]
                {
                    Line("main16_start_leon_01", "arbel-leon", "바람이 거의 없는데 재가 움직였다는 증언이 있습니다. 큰 불길을 보지 못했는데 땅은 뜨겁다고 하고요. 검은 연기 같은 형체를 봤다는 소문도 들립니다."),
                    Line("main16_start_leon_02", "arbel-leon", "평소 같으면 잘못 본 거라고 생각했겠지만, 요즘은 그렇게 넘기기가 어렵군요."),
                    Line("main16_start_paul_01", "companion_paul", "확인할 수 있는 건 직접 확인하죠. 소문이 사실인지부터요.")
                };
                case "ash": return hearing ? new[]
                {
                    Line("main16_ash_hearing_player_01", "player", "진동하고 재가 움직이는 간격이 같아요."),
                    Line("main16_ash_hearing_serin_01", "companion_serin", "저도 그렇게 잡혀요."),
                    Line("main16_ash_hearing_serin_02", "companion_serin", "다만 어느 쪽이 먼저인지는 아직 모르겠습니다.")
                } : new[]
                {
                    Line("main16_ash_default_observation_01", "", "세린이 장치를 조절해 지면 신호를 확인한다."),
                    Line("main16_ash_default_serin_01", "companion_serin", "땅이 울릴 때마다 움직여요."),
                    Line("main16_ash_default_taeon_01", "companion_taeon", "진동과 관련이 있다는 말씀이십니까?"),
                    Line("main16_ash_default_serin_02", "companion_serin", "같이 일어나는 건 맞아요. 어느 쪽이 원인인지는 모르겠습니다."),
                    Line("main16_ash_default_player_01", "player", "움직이는 재와 바닥의 변화를 함께 기록해 두죠.")
                };
                case "vibration": return hearing ? new[]
                {
                    Line("main16_vibration_hearing_player_01", "player", "서쪽으로 갈수록 신호가 강해져요. 반복되는 간격도 앞에서 확인한 것과 달라요."),
                    Line("main16_vibration_hearing_serin_01", "companion_serin", "네. 방향은 같습니다. 다만 어디에서 시작되는지는 아직 모르겠습니다.")
                } : new[]
                {
                    Line("main16_vibration_default_serin_01", "companion_serin", "서쪽에서 오는 진동이 더 강해요. 간격에도 변화가 있습니다. 발생원은 아직 모르겠습니다."),
                    Line("main16_vibration_default_player_01", "player", "그러면 서쪽 흔적과 비교하며 확인하죠.")
                };
                case "tracks": return new[]
                {
                    Line("main16_tracks_paul_01", "companion_paul", "여기까지는 계속 서쪽으로 갔습니다."),
                    Line("main16_tracks_miel_01", "companion_miel", "그런데 여기서 전부 방향을 바꿨네요."),
                    Line("main16_tracks_taeon_01", "companion_taeon", "무언가를 피한 것으로 보입니다.")
                };
                case "witness_ground": return new[]
                {
                    Line("main16_witness_observation_01", "", "갈라진 지면과 그 위의 재를 조사한다. 발밑에 약한 진동이 전해진다."),
                    Line("main16_witness_observation_02", "", "바람은 거의 없는데 주변의 재가 한곳으로 모인다."),
                    Line("main16_witness_observation_03", "", "모인 재 속에 작은 불씨가 섞인다. 검은 연기와 재가 느슨한 형체를 이루기 시작한다.")
                };
                case "witness_emerge": return new[]
                {
                    Line("main16_witness_observation_04", "", "검은 연기와 재 속 붉은 불씨가 움직인다. 불씨망령이 모습을 드러낸다. 정확한 정체는 알 수 없다."),
                    Line("main16_witness_taeon_01", "companion_taeon", "모두 준비하세요. 저 형체가 다가옵니다.")
                };
                case "retry": return new[] { Line("main16_retry_taeon_01", "companion_taeon", "아직 그 형체가 남아 있습니다. 준비가 되면 다시 상대하죠.") };
                case "afterimage":
                    DialogueLine[] first = hearing ? new[]
                    {
                        Line("main16_afterimage_hearing_player_01", "player", "아직 계속돼요."),
                        Line("main16_afterimage_hearing_serin_01", "companion_serin", "네. 저도 잡힙니다.")
                    } : new[]
                    {
                        Line("main16_afterimage_default_serin_01", "companion_serin", "아직 계속됩니다."),
                        Line("main16_afterimage_default_player_01", "player", "형체는 사라졌는데 진동은 남았군요.")
                    };
                    var result = new System.Collections.Generic.List<DialogueLine>(first)
                    {
                        Line("main16_afterimage_paul_01", "companion_paul", "그러면 저게 진동을 만든 건 아니군요."),
                        Line("main16_afterimage_taeon_01", "companion_taeon", "적어도 모든 원인은 아니라는 뜻이겠습니다."),
                        Line("main16_afterimage_observation_01", "", "균열과 열기, 진동은 남아 있다. 균열 안쪽에서 미세한 따뜻한 공기 흐름이 올라온다. 표면에는 그을림이 있지만 큰 산불 흔적은 없다."),
                        Line("main16_afterimage_taeon_02", "companion_taeon", "열기의 일부가 지하에서 올라오는 것일 수 있습니다. 아직 가설입니다.")
                    };
                    return result.ToArray();
                case "canyon":
                    var route = new System.Collections.Generic.List<DialogueLine>
                    {
                        Line("main16_canyon_observation_01", "", "서쪽 끝에 붉게 갈라진 절벽과 재, 강한 아지랑이가 보인다. 가칭 붉은 균열 협곡의 입구다. 오늘은 안쪽으로 들어가지 않는다.")
                    };
                    if (hearing)
                    {
                        route.Add(Line("main16_canyon_hearing_player_01", "player", "협곡 방향에서 오는 진동이 더 강해요."));
                        route.Add(Line("main16_canyon_hearing_serin_01", "companion_serin", "네. 그 방향과 반복이 같이 잡힙니다. 정확한 발생 위치는 아직 모릅니다."));
                    }
                    else
                    {
                        route.Add(Line("main16_canyon_default_serin_01", "companion_serin", "진동은 붉은 균열 협곡 방향에서 더 강해요. 정확한 발생 위치는 아직 모르겠습니다."));
                        route.Add(Line("main16_canyon_default_player_01", "player", "입구 위치를 기록하고 아르벨에 알리죠."));
                    }
                    route.Add(Line("main16_canyon_taeon_01", "companion_taeon", "오늘은 여기까지 확인하는 편이 좋겠습니다."));
                    route.Add(Line("main16_canyon_paul_01", "companion_paul", "찬성입니다. 확인과 무모함은 다른 일이니까요."));
                    route.Add(Line("main16_canyon_miel_01", "companion_miel", "아르벨에 먼저 알려야겠어요."));
                    return route.ToArray();
                case "report": return new[]
                {
                    Line("main16_report_player_01", "player", "바람 없이 재가 움직였고 진동과 동시에 일어났습니다. 불씨망령을 발견해 쓰러뜨렸지만 진동은 남았습니다. 더 서쪽에는 붉은 균열 협곡 입구가 있습니다."),
                    Line("main16_report_taeon_01", "companion_taeon", "열기의 일부가 지하에서 올라올 가능성은 있습니다. 가설일 뿐이며 재와 진동의 원인, 형체의 정체는 아직 모릅니다."),
                    Line("main16_report_leon_01", "arbel-leon", "그러면 동물들만 변하고 있는 게 아니군요."),
                    Line("main16_report_paul_01", "companion_paul", "네. 적어도 이제는 그렇게 보는 편이 맞겠습니다.")
                };
                default: return System.Array.Empty<DialogueLine>();
            }
        }
    }
}
