using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterLeagueCollectionDetail : MonoBehaviour
{
    [SerializeField] private MonsterController m_MonsterCtrl;
    [SerializeField] private Image m_Element;
    [SerializeField] private TMP_Text m_TxtLevel;
    [SerializeField] private TMP_Text m_TxtName;
    [SerializeField] private TMP_Text m_TxtAtk;
    [SerializeField] private TMP_Text m_TxtDef;
    [SerializeField] private TMP_Text m_TxtMag;
    [SerializeField] private PixelProgressBar m_Hp;
    [SerializeField] private List<Sprite> m_IconElements = new List<Sprite>();


    private static readonly string k_LevelPrefix = "Lv:";
    private static readonly string k_AtkPrefix = "Attack:";
    private static readonly string k_DefPrefix = "Defense:";
    private static readonly string k_MagPrefix = "Magic:";

    private BattleMonster _monster;
    private Action OnBack;
    public async Task Init(BattleMonster monster,Action onback)
    {
        await m_MonsterCtrl.SetData(monster,Vector3.zero);
        m_Element.sprite = m_IconElements[(int)monster.Element];
        m_TxtLevel.text = $"{k_LevelPrefix}{monster.Level}";
        m_TxtAtk.text = $"{k_AtkPrefix}{monster.Attack}";
        m_TxtDef.text = $"{k_DefPrefix}{monster.Defense}";
        m_TxtMag.text = $"{k_MagPrefix}{monster.Magic}";
        m_TxtName.text = monster.MonsterId;
        m_Hp.SetProgress(monster.BattleHP/(float)monster.HP);
        OnBack = onback;
        gameObject.SetActive(true);
        RegisterInputs();
        _monster = monster;
    }


    public void RegisterInputs()
    {
        MonsterLeagueInput.Instance.xPressed += OnBackPressed;
        MonsterLeagueInput.Instance.oPressed += OnUseItem;
    }

    private void OnUseItem()
    {
        //Heal up a little bit
        _monster.BattleHP += 50;
        m_Hp.SetProgress(_monster.BattleHP / (float)_monster.HP);
    }


    public void UnregisterInputs()
    {
        MonsterLeagueInput.Instance.xPressed -= OnBackPressed;
        MonsterLeagueInput.Instance.oPressed -= OnUseItem;
    }

    private void OnBackPressed()
    {
        UnregisterInputs();
        OnBack?.Invoke();
    }

}
