using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class MonsterLeagueCreation : MonsterLeaguePage
{
    private List<Color> m_CapColors = new ();
    [SerializeField] private Image m_Color1;
    [SerializeField] private Image m_Color2;
    [SerializeField] private Image m_Color3;
    [SerializeField] private Image m_Orbit1;
    [SerializeField] private Image m_Orbit2;
    [SerializeField] private Image m_Orbit3;
    [SerializeField] private GameObject m_RtBtns;
    [SerializeField] private CanvasGroup m_Canvas;
    [SerializeField] private MonsterController m_Monster;
    [SerializeField] private CameraLimitedFPS m_CamColorCapture;
    private List<Image> m_ColorPanels =new();
    private List<Image> m_Orbits =new();
    public Action<BattleMonster> OnGenerationComplete;
    private IMonsterGenerator m_monsterGen;
    void Start()
    {
        m_ColorPanels.Add(m_Color1);
        m_ColorPanels.Add(m_Color2);
        m_ColorPanels.Add(m_Color3);
        m_Orbits.Add(m_Orbit1);
        m_Orbits.Add(m_Orbit2);
        m_Orbits.Add(m_Orbit3);
        m_monsterGen = new ElementMonsterGen();
    }

    protected override void  RegisterInputs()
    {
        MonsterLeagueInput.Instance.xPressed += OnCaptureColor;
        MonsterLeagueInput.Instance.sqrPressed += OnReCapture;
        MonsterLeagueInput.Instance.oPressed += OnClearColors;
    }
    protected override void UnregisterInputs()
    {
        MonsterLeagueInput.Instance.xPressed -= OnCaptureColor;
        MonsterLeagueInput.Instance.sqrPressed -= OnReCapture;
        MonsterLeagueInput.Instance.oPressed -= OnClearColors;
    }

    public override void EnterPage()
    {
        base.EnterPage();
        m_RtBtns.SetActive(true);
        OnClearColors();
    }

    private void OnClearColors()
    {
        m_CapColors.Clear();
        RefreshColorPanels();
    }

    private void OnReCapture()
    {
        if (m_CapColors.Count > 0)
        {
            m_CapColors.RemoveAt(m_CapColors.Count - 1);
            //Waiting for user to capture a new color from the scene here. 
            OnCaptureColor();
        }
    }

    private void OnCaptureColor()
    {
        //MonsterLeagueUIManager.Instance.PickScreenColor(OnColorCaptured);
        m_CamColorCapture.CaptureColor(OnColorCaptured);
    }

    private void OnColorCaptured(Color col)
    {
        if (m_CapColors.Count < 3)
        {
            m_CapColors.Add(col);
        }
        RefreshColorPanels();
        if (m_CapColors.Count == 3)
        {
            StartGeneration();
        }
        else
        {
            UpdateSilhouette();
        }
    }

    private void StartGeneration()
    {
        m_RtBtns.SetActive(false);
        MonsterLeagueInput.Instance.IsInputDisabled = true;
        var battleMon = UpdateSilhouette();
        FlashGeneratingTxt(()=> OnGenerationComplete?.Invoke(battleMon));
    }

    private BattleMonster UpdateSilhouette()
    {
        var monster = m_monsterGen.Generate(m_CapColors, MonsterTier.Rookie);
        var battleMon = new BattleMonster(monster);
#pragma warning disable CS4014 
        m_Monster.SetData(battleMon, Vector3.zero);
#pragma warning restore CS4014 
        return battleMon;
    }



    private void FlashGeneratingTxt(Action callback)
    {
        var seq = DOTween.Sequence();
        seq.Append(m_Canvas.DOFade(1, 1)).Append(m_Canvas.DOFade(0, 1)).SetLoops(3)
            .OnComplete(() =>
            {
                MonsterLeagueInput.Instance.IsInputDisabled = false;
                callback.Invoke();
            });
    }

    private void RefreshColorPanels()
    {
        for (int i = 0; i < m_CapColors.Count; i++)
        {
            m_ColorPanels[i].color = m_CapColors[i];
            m_Orbits[i].color = m_CapColors[i];

        }

        for (int i = m_CapColors.Count; i < m_ColorPanels.Count; i++)
        {
            m_ColorPanels[i].color = Color.clear;
            m_Orbits[i].color = Color.white;
        }
    }

    void Update()
    {
        
    }
}
