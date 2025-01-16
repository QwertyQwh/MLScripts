using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class MonsterLeagueTitle : MonsterLeaguePage
{
    private static readonly Color k_grayed = new Color(0.1981f, 0.1981f, 0.1981f, 0.4156f);
    private static readonly Color k_selected = Color.white;
    private int m_SelectedIndex;

    [SerializeField] private TMP_Text m_TxtCreate;
    [SerializeField] private TMP_Text m_TxtScan;
    [SerializeField] private TMP_Text m_TxtOptions;
    [SerializeField] private Image m_ImgSubtitle;
    [SerializeField] private CanvasGroup m_canvas;
    [SerializeField] private List<Color> m_SubtitleColors;
    public Action OnCreate;
    public Action OnBattle;
    public Action OnCollection;

    private List<TMP_Text> m_Txts = new ();
    // Start is called before the first frame update
    void Start()
    {
        m_Txts.Add(m_TxtCreate);
        m_Txts.Add(m_TxtScan);
        m_Txts.Add(m_TxtOptions);
        m_SelectedIndex = 0;
        SetTxtSelected(m_SelectedIndex);
        PlayColorFlash();

    }

    protected override void RegisterInputs()
    {
        Debug.Log("title input enabled");
        MonsterLeagueInput.Instance.upPressed += OnUpPressed;
        MonsterLeagueInput.Instance.downPressed += OnDownPressed;
        MonsterLeagueInput.Instance.oPressed += OnOPressed;
    }

    protected override void UnregisterInputs()
    {
        Debug.Log("title input disabled");
        MonsterLeagueInput.Instance.upPressed -= OnUpPressed;
        MonsterLeagueInput.Instance.downPressed -= OnDownPressed;
        MonsterLeagueInput.Instance.oPressed -= OnOPressed;
    }


    public override void EnterPage()
    {
        base.EnterPage();
        MonsterLeagueUIManager.Instance.PlayFlash();
        PlayTitleFadeIn();
    }

    private void OnOPressed()
    {
        OnTxtEnter();
    }

    private void OnDownPressed()
    {
        m_SelectedIndex = Math.Clamp(m_SelectedIndex + 1, 0, m_Txts.Count-1);
        SetTxtSelected(m_SelectedIndex);
    }

    private void OnUpPressed()
    {
        m_SelectedIndex = Math.Clamp(m_SelectedIndex - 1, 0, m_Txts.Count-1);
        SetTxtSelected(m_SelectedIndex);
    }

    private void SetTxtSelected(int index)
    {
        for (var i = 0; i < m_Txts.Count; i++)
            m_Txts[i].color = index == i ? k_selected : k_grayed;
    }

    private void OnTxtEnter()
    {
        switch (m_SelectedIndex)
        {
            case 0:
                OnCreate?.Invoke();
                break;
            case 1:
                OnBattle?.Invoke();
                break;
            case 2:
                OnCollection?.Invoke();
                break;
        }
    }


    void PlayColorFlash()
    {
        var seq = DOTween.Sequence().SetLoops(-1);
        m_ImgSubtitle.color = m_SubtitleColors[^1];
        foreach (var anim in m_SubtitleColors.Select(col => m_ImgSubtitle.DOColor(col, 2).SetEase(Ease.Linear)))
        {
            seq.Append(anim);
        }
    }

    void PlayTitleFadeIn()
    {
        m_canvas.DOFade(1, 2).SetDelay(3);
    }




}
