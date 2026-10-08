using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.UI;

namespace ProjectLimitless.World
{
    /// <summary>Main19 승인 본문과 Stable ID 정본입니다. 새 음성은 모두 대기이며 Player·지문은 무음입니다.</summary>
    public static class Main19DialogueCatalog
    {
        public static DialogueLine[] Get(int index,string path=null,bool allCompanions=false)
        {
            path=path??GameSessionData.SelectedPlayerPathId;
            var lines=new List<DialogueLine>();
            string site=index==12?"closed_route":Chapter2Main19Flow.Targets[index].Replace("field10_main19_","");
            void Add(string who,string text,string branch="")
            {
                string id=who=="serin"?CompanionRosterService.SerinId:who=="paul"?CompanionRosterService.PaulId:
                    who=="miel"?CompanionRosterService.MielId:who=="taeon"?CompanionRosterService.TaeonId:who;
                string name=who=="serin"?"세린":who=="paul"?"폴":who=="miel"?"미엘":who=="taeon"?"태온":who=="player"?GameSessionData.PlayerName:"";
                lines.Add(new DialogueLine(id,name,text,"main19_"+site+"_"+(branch==""?"":branch+"_")+(who==""?"direction":who)+"_"+(lines.Count(x=>x.SpeakerId==id)+1).ToString("00")));
            }
            switch(index)
            {
                case 2:
                    Add("serin","발자국 하나의 간격이 너무 넓어요.\n여러 개체가 아니라, 큰 하나가 지나간 흔적에 가깝습니다.");
                    Add("player","감시자보다 더 큰 게 있다는 뜻입니까?");
                    Add("serin","단정은 이르지만, 적어도 여기까지 남은 압력은 그 수준입니다.");break;
                case 3:
                    Add("paul","불길이 스친 정도가 아니네요.\n안쪽부터 녹았다가 다시 굳은 것처럼 보입니다.");
                    Add("miel","가까이 오래 있으면 사람도 버티기 어렵겠어요.");
                    Add("paul","네. 다음에는 관찰보다 대비가 먼저일 것 같습니다.");break;
                case 5:
                    // A 승리 뒤 다음 조사 첫 페이지로 전투 관찰을 이어갑니다. 별도 목표·자동 Dialogue는 추가하지 않습니다.
                    Add("serin","둘이 우연히 모인 게 아니에요.\n같은 맥동에 밀려 한쪽으로 몰린 것처럼 움직였습니다.");
                    string observation=path=="path.vision"?"붉은 균열이 밝아지는 지점과 흔적의 진행 방향이 겹칩니다.":
                        path=="path.hearing"?"낮은 울림이 두 번 끊어졌다가 다시 이어집니다.\n안쪽일수록 간격이 짧아져요.":
                        path=="path.intellectual"?"진동 간격과 열기 분출 시점이 비슷합니다.\n무작위 현상으로 보기 어렵습니다.":
                        path=="path.mobility"?"갈라진 지면 사이에도 버틸 만한 길이 이어집니다.\n무게가 큰 존재가 지나간 흔적일 수도 있습니다.":
                        "갑작스러운 열기에도 반복되는 리듬이 있어요.\n패턴을 알면 다음 움직임을 예측할 수 있을 것 같습니다.";
                    Add("player",observation,path.Replace("path.","").Replace('-','_'));
                    Add("serin","네. 더 깊은 쪽일수록 반응이 빨라집니다.");break;
                case 8:
                    Add("player","이건 발자국이라기보다, 길을 눌러 만든 자국 같군.");
                    Add("serin","네. 여기서부터는 '무언가가 지나갔다'가 아니라,\n'무언가가 이 길을 쓰고 있다'에 가깝습니다.");break;
                case 10:
                    Add("serin","저 존재가 모든 일의 원인이라고 단정할 수는 없어요.\n하지만 움직일 때마다 열기와 진동이 함께 치솟습니다.");
                    Add("player","지금 덤빌 상대는 아니군요.");
                    if(allCompanions||CompanionRosterService.IsActivePartyMember(CompanionRosterService.TaeonId))Add("taeon","준비를 갖추고 다시 오겠습니다.");
                    if(allCompanions||CompanionRosterService.IsActivePartyMember(CompanionRosterService.MielId))Add("miel","다친 채로 밀어붙일 상대는 아니에요.");
                    if(allCompanions||CompanionRosterService.IsActivePartyMember(CompanionRosterService.PaulId))Add("paul","다음에는 관찰보다 대응이 먼저겠네요.");break;
                case 11:
                    Add("serin","저게 중심부에 가까운 존재라면, 다음에는 피할 수 없을 거예요.");
                    Add("player","다음에는 끝을 볼 준비를 하고 오죠.");break;
                case 12:Add("","<지문> 더 깊은 심부는 열기와 진동이 너무 강해 지금은 바로 들어갈 수 없습니다.");break;
            }
            return lines.ToArray();
        }
    }
}
