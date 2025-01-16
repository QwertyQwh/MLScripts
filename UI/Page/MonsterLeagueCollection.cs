using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using QFramework.Extensions;
using UnityEngine;

public class MonsterLeagueCollection : MonsterLeaguePage
{
    [SerializeField] private MonsterLeagueCollectionDetail m_DetailPage;
    [SerializeField] private MonsterLeagueCollectionList m_ListPage;

    public Action OnBackHome;

    public void Init()
    {
        m_ListPage.Init(MonsterLeagueSaveManager.Instance.Monsters,OnMonsterSelect);
        m_ListPage.SetActiveEx(true);
        m_DetailPage.SetActiveEx(false);
    }

    public override void EnterPage()
    {
        base.EnterPage();
        Init();
    }

    protected override void RegisterInputs()
    {
        base.RegisterInputs();
        m_ListPage.OnBackHome = OnBackHome;;
        m_ListPage.RegisterInputs();
    }


    protected override void UnregisterInputs()
    {
        base.UnregisterInputs();
        m_ListPage.OnBackHome -= OnBackHome;;
        m_ListPage.UnregisterInputs();
    }

    public override void ExitPage()
    {
        base.ExitPage();
        m_ListPage.Exit();
    }

    private async Task OnMonsterSelect(BattleMonster monster)
    {
        m_ListPage.ToDetail();
        await m_DetailPage.Init(monster, OnDetailBack);
    }

    private void OnDetailBack()
    {
        m_DetailPage.SetActiveEx(false);
        m_ListPage.SetActiveEx(true);
    }
}
