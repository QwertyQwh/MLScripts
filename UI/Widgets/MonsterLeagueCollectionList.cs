using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using QFramework.Extensions;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterLeagueCollectionList : MonoBehaviour
{
    [SerializeField] private UnitMonsterListItem m_PrefabItem;
    [SerializeField] private RectTransform m_RtContent;
    [SerializeField] private RectTransform m_RtPool;
    [SerializeField] private UnitMonsterListArrow m_UpArrow;
    [SerializeField] private UnitMonsterListArrow m_DownArrow;
    [SerializeField] private UnitSelectableList m_list;

    private BehaviorPool<UnitMonsterListItem> m_pool;
    private List<BattleMonster> m_monsters;
    public Action OnBackHome;


    public void Init(List<BattleMonster> monsters, Func<BattleMonster, Task> OnSelect)
    {
        m_pool ??= new BehaviorPool<UnitMonsterListItem>(m_RtPool, m_PrefabItem);
        m_list.Clear();
        m_list.OnFocusShift += OnListFocusShift;
        m_monsters = monsters;
        for (int i = 0;i<monsters.Count;i++)
        {
            var item = m_pool.Get();
            item.transform.SetParent(m_RtContent);
            item.ItemId = i;
            item.SetActiveEx(true);
            item.SetData(monsters[i], OnSelect);
            item.NotifyBattleActive(MonsterLeagueSaveManager.Instance.ActiveMonsterId);
            m_list.AddItem(item);
        }
        m_list.Reset();

    }

    private void OnListFocusShift(int id)
    {
        //Moved to top element
        m_UpArrow.Enable(id != 0);
        m_DownArrow.Enable(id != m_list.Count-1);
        
    }


    public void RegisterInputs()
    {
        //MonsterLeagueInput.Instance.oPressed += OnSelectPressed;
        MonsterLeagueInput.Instance.xPressed += OnHomePressed;
        MonsterLeagueInput.Instance.sqrPressed += OnSetActivePressed;
        
    }



    public void UnregisterInputs()
    {
        //MonsterLeagueInput.Instance.oPressed -= OnSelectPressed;
        MonsterLeagueInput.Instance.xPressed -= OnHomePressed;
        MonsterLeagueInput.Instance.sqrPressed -= OnSetActivePressed;
    }
    private void OnSetActivePressed()
    {
        var selected = ((UnitMonsterListItem)(m_list.ActiveItem));
        MonsterLeagueSaveManager.Instance.ActiveMonsterId = selected.Monster.InstanceId;
        m_list.SelectableList.ForEach((item)=> ((UnitMonsterListItem)item).NotifyBattleActive(selected.Monster.InstanceId));
    }

    private void OnHomePressed()
    {
        if (!gameObject.activeSelf) return;
        OnBackHome?.Invoke();
    }

    public void Exit()
    {
        m_list.DisableInput();
        m_pool.RecycleAllWorkingPool();
    }

    public void ToDetail()
    {
        gameObject.SetActive(false);

    }
    

}
