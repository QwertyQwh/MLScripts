using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// This class should be adapted for use of dynamic loading, i.e. calling onselected(id) where the id is assigned to each selectable when spawned.
/// </summary>
public class UnitSelectableList : MonoBehaviour
{
    public List<UnitSelectable> SelectableList = new ();
    private int m_SelectedIndex = 0;
    public Action<UnitSelectable> OnSelected;
    public int Count => SelectableList.Count;
    public UnitSelectable ActiveItem => SelectableList[m_SelectedIndex];
    //Invoked when focused element changes, used when external elements dependency, typically slider
    public Action<int> OnFocusShift;

    public void AddItem(UnitSelectable item)
    {
        SelectableList.Add(item);
    }

    public void Clear()
    {
        SelectableList.Clear();
    }
    public void Reset()
    {
        RegisterInputEvents();
        OnSelect(0);
    }

    public void DisableInput()
    {
        UnRegisterInputEvents();
    }

    void OnDisable()
    {
        //UnRegisterInputEvents();
    }
    private void RegisterInputEvents()
    {
        MonsterLeagueInput.Instance.leftPressed += OnSelectPrevious;
        MonsterLeagueInput.Instance.upPressed += OnSelectPrevious;
        MonsterLeagueInput.Instance.rightPressed += OnSelectNext;
        MonsterLeagueInput.Instance.downPressed += OnSelectNext;
        MonsterLeagueInput.Instance.oPressed += OnPress;
    }

    private void UnRegisterInputEvents()
    {
        MonsterLeagueInput.Instance.leftPressed -= OnSelectPrevious;
        MonsterLeagueInput.Instance.upPressed -= OnSelectPrevious;
        MonsterLeagueInput.Instance.rightPressed -= OnSelectNext;
        MonsterLeagueInput.Instance.downPressed -= OnSelectNext;
        MonsterLeagueInput.Instance.oPressed -= OnPress;
    }

    private void OnSelect(int id)
    {
        if (id < 0 || id >= SelectableList.Count )
        {
            return;
        }

        OnFocusShift?.Invoke(id);
        //Debug.Log($"selected:{id}");

        m_SelectedIndex = id; 
        SelectableList.ForEach((item)=>item.OnNotifyFocus(id));
    }
    private void OnSelectPrevious()
    {
        if (!gameObject.activeSelf) return;
        OnSelect(m_SelectedIndex-1);
    }
    private void OnSelectNext()
    {
        if (!gameObject.activeSelf) return;
        OnSelect(m_SelectedIndex+1);
    }

    private void OnPress()
    {
        if (!gameObject.activeSelf) return;
        var cur = SelectableList[m_SelectedIndex];
        cur.OnSelect();
        OnSelected?.Invoke(cur);
    }
}
