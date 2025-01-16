using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterLeagueSaveManager : Singleton<MonsterLeagueSaveManager>
{

    public List<BattleMonster> Monsters = new();
    public string ActiveMonsterId { get; set; }

    public BattleMonster ActiveMonster => Monsters.First(monster => monster.InstanceId == ActiveMonsterId);


    protected override void Init()
    {
        //Monsters.Add(new BattleMonster(new Monster("Slimon")));
        //Monsters.Add(new BattleMonster(new Monster("Mimic")));
        //Monsters.Add(new BattleMonster(new Monster("Roy")));
        var active = new BattleMonster(new Monster("Slimon"));
        ActiveMonsterId = active.InstanceId;
        Monsters.Add(active);
        
    }

    protected override void Dispose()
    {
    }
}
