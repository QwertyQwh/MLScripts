using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SudoBattleServer :Singleton<SudoBattleServer>
{

    protected override void Init()
    {
    }

    protected override void Dispose()
    {
    }
    #region msg receiver

    public TAck SendXXXReq<TAck>()
    {
        return default;
    }


    #endregion

    #region msg sender

    public Action OnDealingDamage;

    #endregion

}
