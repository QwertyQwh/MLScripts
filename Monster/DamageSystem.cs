using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageSystem 
{
    public static int CalcDamage(BattleMonster attacker, BattleMonster defender,int combo)
    {
        var damage = attacker.BattleAttack - defender.BattleDefense;
        damage *= combo;
        damage = Math.Clamp(damage, 1, 999);
        return damage;
    }
}
