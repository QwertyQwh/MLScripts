using System.Collections;
using System.Collections.Generic;
using QFramework.Extensions;
using UnityEngine;

public class MonsterLeaguePage : MonoBehaviour
{
    public virtual void EnterPage()
    {
        this.SetActiveEx(true);
        RegisterInputs();
    }

    public virtual void ExitPage()
    {
        this.SetActiveEx(false);
        UnregisterInputs();
    }

    protected virtual void RegisterInputs(){}
    protected virtual void UnregisterInputs() { }
}
