using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MonsterLeagueGen : MonsterLeaguePage
{
    public Action OnBackHome;
    [SerializeField] private MonsterController _monster;


    protected override void RegisterInputs()
    {
        base.RegisterInputs();
        MonsterLeagueInput.Instance.oPressed += OnBackHome;
    }

    protected override void UnregisterInputs()
    {
        base.UnregisterInputs();
        MonsterLeagueInput.Instance.oPressed -= OnBackHome;
    }

    public async Task Init(BattleMonster monster)
    {
        MonsterLeagueSaveManager.Instance.Monsters.Add(monster);
        await _monster.SetData(monster, Vector3.zero);
    }

}
