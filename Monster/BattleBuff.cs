using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleBuff
{
    protected int _remainingTurns;// negative means infinite turns
    protected BattleMonster _owner;
    protected BattleBuff(int turns)
    {
        _remainingTurns = turns;
    }

    public virtual void AttachTo(BattleMonster monster)
    {
        _owner = monster;
        monster.AttachBuff(this);
    }
    
    protected abstract void Apply();

    public void CountDown()
    {
        if (--_remainingTurns != 0) return;
        Dispose();
        _owner.RemoveBuff(this);
    }

    protected abstract void Dispose();
}

public abstract class StatBuff : BattleBuff
{
    protected StatBuff(int turns) : base(turns)
    {
        
    }

    public override void AttachTo(BattleMonster monster)
    {
        base.AttachTo(monster);
        Apply();
    }
}

public class DefenseBuff : StatBuff
{
    private readonly float _multiplier;
    private readonly int _offset;
    public DefenseBuff(int turns,float multiplier,int offset) : base(turns)
    {
        _multiplier = multiplier;
        _offset = offset;
    }

    public override void AttachTo(BattleMonster monster)
    {
        base.AttachTo(monster);
    }

    protected override void Apply()
    {
        _owner.BattleDefense += Mathf.CeilToInt(_owner.Defense * (_multiplier - 1) + _offset);
    }

    protected override void Dispose()
    {
        _owner.BattleDefense -= Mathf.CeilToInt(_owner.Defense * (_multiplier - 1) + _offset);
    }


}
