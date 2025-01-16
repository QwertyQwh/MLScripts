using System.Collections;
using System.Collections.Generic;
using QFramework.Extensions;
using UnityEngine;
using UnityEngine.UI;


public enum BattleAction
{
    Attack,
    Defense,
    Magic,
    Escape
}

public class UnitBattleAction : UnitSelectable
{
    [SerializeField] private Animator m_AnimFocus;
    [SerializeField] private Animator m_AnimSelect;
    [SerializeField] private Image m_Color;
    [SerializeField] private Image m_Grayed;
    private Button btn;
    public BattleAction action;

    void Start()
    {
        
    }

    void Update()
    {
    }

    public override void OnFocus()
    {
        //If a focus animation is defined, activate it;
        m_AnimFocus?.SetActiveEx(true);
        m_Grayed?.SetActiveEx(false);
        if (m_AnimSelect)
        {
            m_AnimSelect?.SetActiveEx(true);
            m_AnimSelect?.Play("Base.Freeze");
        }
    }

    public override void OnDefocus()
    {
        m_AnimFocus?.SetActiveEx(false);
        m_Grayed?.SetActiveEx(true);
        //Unity object cannot use null propagation
        if (m_AnimSelect)
            m_AnimSelect.SetActiveEx(false);
    }

    public override void OnSelect()
    {
        if(m_AnimSelect)
            m_AnimSelect?.Play("Base.Act");
    }
}