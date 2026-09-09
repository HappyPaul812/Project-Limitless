using System;

namespace ProjectLimitless.Core
{
    public enum OpeningIntroVisual { Light, World, Stillness, Gift, Limit, People, Paths, Closing, Title }

    [Serializable]
    public readonly struct OpeningIntroSlide
    {
        public OpeningIntroSlide(string text, float duration, OpeningIntroVisual visual)
        { Text = text; Duration = duration; Visual = visual; }
        public string Text { get; }
        public float Duration { get; }
        public OpeningIntroVisual Visual { get; }
    }

    /// <summary>
    /// 오프닝 문장·표시 시간·연출 종류를 한곳에 둡니다. UI 코드와 서사를 나누면 문장이나 향후
    /// 배경 일러스트를 교체할 때 입력·Fade·Scene 전환 로직을 다시 작성하지 않아도 됩니다.
    /// </summary>
    public static class OpeningIntroSequence
    {
        public static readonly OpeningIntroSlide[] Slides =
        {
            new OpeningIntroSlide("태초에, 신은 세상을 창조했다.", 6f, OpeningIntroVisual.Light),
            new OpeningIntroSlide("그 세상은 완전했다.", 5f, OpeningIntroVisual.World),
            new OpeningIntroSlide("아픔도 없었고,\n슬픔도 없었으며,\n부족함도 없었다.", 7f, OpeningIntroVisual.World),
            new OpeningIntroSlide("모든 존재는 강했고,\n누구의 도움도 필요로 하지 않았다.", 7f, OpeningIntroVisual.World),
            new OpeningIntroSlide("완벽한 존재는 누구도 필요로 하지 않는다.", 7f, OpeningIntroVisual.Stillness),
            new OpeningIntroSlide("그러나 시간이 흐를수록\n세상은 조금씩 멈춰 갔다.", 7f, OpeningIntroVisual.Stillness),
            new OpeningIntroSlide("누구도 서로를 필요로 하지 않았기 때문이다.", 7f, OpeningIntroVisual.Stillness),
            new OpeningIntroSlide("신은 자신이 만든 세상을 바라보았다.", 6f, OpeningIntroVisual.Gift),
            new OpeningIntroSlide("그리고 세상에 하나의 선물을 남겼다.", 6f, OpeningIntroVisual.Gift),
            new OpeningIntroSlide("Limit", 5f, OpeningIntroVisual.Limit),
            new OpeningIntroSlide("사람들은 서로 달라졌다.", 5f, OpeningIntroVisual.People),
            new OpeningIntroSlide("혼자서는 할 수 없는 일이 생겼고,\n때로는 누군가의 손이 필요해졌다.", 8f, OpeningIntroVisual.People),
            new OpeningIntroSlide("그리고 아주 오랜 시간이 흐른 뒤\n사람들은 조금씩 깨닫기 시작했다.", 8f, OpeningIntroVisual.Paths),
            new OpeningIntroSlide("한계는 단지 약함이 아니었다.", 6f, OpeningIntroVisual.Paths),
            new OpeningIntroSlide("서로를 만나게 하는 이유였고,\n서로 다른 힘을 이어 주는 시작이었다.", 8f, OpeningIntroVisual.Paths),
            new OpeningIntroSlide("모든 사람에게는 한계가 있다.", 6f, OpeningIntroVisual.Closing),
            new OpeningIntroSlide("그리고 모든 사람에게는\n그 너머로 나아갈 길이 있다.", 7f, OpeningIntroVisual.Closing),
            new OpeningIntroSlide("이제, 당신의 길을 선택할 시간이다.", 7f, OpeningIntroVisual.Closing),
            new OpeningIntroSlide("LIMITLESS", 8f, OpeningIntroVisual.Title),
        };
    }

    /// <summary>같은 OpeningIntro Scene이 새 캐릭터 시작인지 다시 보기인지 구분하는 실행 중 정보입니다.</summary>
    public static class OpeningIntroLaunchContext
    {
        public static bool IsReplay { get; private set; }
        public static void BeginNewCharacter() => IsReplay = false;
        public static void BeginReplay() => IsReplay = true;
    }
}
