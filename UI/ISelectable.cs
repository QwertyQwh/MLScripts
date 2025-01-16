using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    void OnFocus();
    void OnDefocus();
    void OnSelect();
}
