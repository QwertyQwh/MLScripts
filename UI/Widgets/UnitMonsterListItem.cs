using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using QFramework.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitMonsterListItem : UnitSelectable
{
    [SerializeField] private Sprite m_SprtBgNormal;
    [SerializeField] private Sprite m_SprtBgSelected;
    [SerializeField] private Image m_ImgBg;
    [SerializeField] private Image m_ImgFist;
    [SerializeField] private TMP_Text m_TxtName;

    public BattleMonster Monster;
    private Func<BattleMonster, Task> m_OnSelect;
    public void SetData(BattleMonster monster, Func<BattleMonster, Task> Select)
    {
        Monster = monster;
        m_TxtName.text = monster.MonsterId;
        m_OnSelect = Select;
        m_ImgFist.SetActiveEx(false);
    }

    private void SetBattleActive(bool active)
    {
        m_ImgFist.SetActiveEx(active);
    }

    public void NotifyBattleActive(string id)
    {
        SetBattleActive(id == Monster.InstanceId);
    }

    public override void OnFocus()
    {
        base.OnFocus();
        m_ImgBg.sprite = m_SprtBgSelected;
    }

    public override void OnDefocus()
    {
        base.OnDefocus();
        m_ImgBg.sprite = m_SprtBgNormal;
    }

    public override async void OnSelect()
    {
        base.OnSelect();
        await m_OnSelect?.Invoke(Monster);
    }
}
