using System.Collections;
using System.Collections.Generic;
using QFramework.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelectable : MonoBehaviour,ISelectable
{
    public int ItemId;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnNotifyFocus(int id)
    {
        if(id == ItemId) { OnFocus();}
        else
        {
            OnDefocus();
        }
    }

    public virtual void OnFocus()
    {
    }

    public virtual void OnDefocus()
    {

    }

    public virtual void OnSelect()
    {
    }

    public virtual void Disable()
    {

    }
}
