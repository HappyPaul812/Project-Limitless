using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectLimitless.Core;
using UnityEngine;

public static class Issue118ProgressionAudit
{
    static readonly int[] Levels = {17,18,18,19,20,21,22};
    static readonly int[] Exp = {60,68,70,76,84,125,210};
    static readonly int[] Talent = {16,18,18,20,22,32,60};
    static readonly int[] LootTenths = {40,45,45,54,63,220,0};
    static readonly int[] First = {0,2,3,4,4};
    static readonly int[] Second = {1,1,2,3,-1};

    public static void Main()
    {
        var root = Directory.GetCurrentDirectory();
        var stats = new List<string>{"level,job,hp,attack,defense,mp,mpRecovery,agility"};
        foreach (int level in new[] {16,17,18,19,20,21,22,23})
        foreach (string job in new[] {"guardian","healer","fighter","sharpshooter","mage"})
        {
            var s = CharacterGrowthCalculator.Calculate(job,level);
            stats.Add($"{level},{job},{CharacterGrowthCalculator.CalculateMaxHp(job,s)},{CharacterGrowthCalculator.CalculateAttack(job,s)},{CharacterGrowthCalculator.CalculateDefense(job,s)},{CharacterGrowthCalculator.CalculateMaxMp(job,s)},{CharacterGrowthCalculator.CalculateMpRecovery(job,s)},{s.Agility}");
        }
        File.WriteAllLines(Path.Combine(root,"Stats.csv"), stats);
        var rows = new List<string>{"startLevel,startExp,perField,side,beforeLevel,beforeExp,beforeTalent,afterBossLevel,afterBossExp,afterBossTalent,endLevel,endExp,endTalent,lootTenths,expTo22,expTo23,preExpTo22,preLootTenths"};
        foreach (int start in new[] {16,17})
        foreach (int rest in new[] {0,ExperienceProgression.RequiredExp(start)/2})
        foreach (bool side in new[] {false,true})
        for (int n=0;n<=30;n++)
            rows.Add(Simulate(start,rest,n,side));
        File.WriteAllLines(Path.Combine(root,"Routes.csv"),rows);
        Console.WriteLine("Issue118 progression completed, no session or save used.");
    }

    static string Simulate(int start,int rest,int n,bool side)
    {
        int level=start, xp=rest, wallet=0, loot=0;
        void Award(int exp,int talent)
        {
            var gain = ExperienceProgression.Add(level,xp,exp);
            level=gain.Level;
            xp=gain.CurrentExperience;
            wallet+=talent;
        }
        void Battle(params int[] monsters)
        {
            int exp = monsters.Sum(m => ExperienceProgression.MonsterExperience(Exp[m],level,Levels[m]));
            Award(exp,monsters.Sum(m=>Talent[m]));
            loot+=monsters.Sum(m=>LootTenths[m]);
        }
        int Missing(int goal) => level >= goal ? 0 :
            Enumerable.Range(level,goal-level).Sum(ExperienceProgression.RequiredExp)-xp;
        for (int field=0;field<5;field++)
        {
            Battle(field == 4 ? 5 : First[field]);
            for(int i=0;i<n;i++)
            {
                if(i%2==1 && Second[field]>=0) Battle(First[field],Second[field]);
                else Battle(First[field]);
            }
            switch(field)
            {
                case 0: Award(230,85); if(side) Award(90,30); break;
                case 1: Award(130,50); if(side) Award(200,75); break;
                case 2: Award(140,55); if(side) Award(120,45); break;
                case 3: Award(150,60); if(side) Award(135,50); break;
                case 4: Award(160,65); if(side) Award(145,55); break;
            }
        }
        string before=$"{level},{xp},{wallet}";
        int preDeficit=Missing(22), preLoot=loot;
        Battle(6);
        string after=$"{level},{xp},{wallet}";
        Award(350,165);
        return $"{start},{rest},{n},{(side?1:0)},{before},{after},{level},{xp},{wallet},{loot},{Missing(22)},{Missing(23)},{preDeficit},{preLoot}";
    }
}
