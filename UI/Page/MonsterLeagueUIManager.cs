using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using QFramework.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class MonsterLeagueUIManager : SingletonBehavior<MonsterLeagueUIManager>
{
    [SerializeField] private RectTransform m_TRoot;
    [SerializeField] private InfiniteBg m_InfiniteBg;
    private Texture2D m_txtrScreenShot;
    [SerializeField] private MonsterLeagueSplash m_PageSplash;
    [SerializeField] private MonsterLeagueTitle m_PageTitle;
    [SerializeField] private MonsterLeagueCreation m_PageCreation;
    [SerializeField] private MonsterLeagueGen m_PageGen;
    [SerializeField] private MonsterLeagueBattle m_PageBattle;
    [SerializeField] private MonsterLeagueCollection m_PageCollection;
    [SerializeField] private GameObject m_Glitch;
    [SerializeField] private CanvasGroup m_Flash;
    private Color m_captureColor;
    private Action<Color> m_onColorCaptured;
    
    protected override void Start()
    {
        base.Start();
#if UNITY_EDITOR
        var stereo = new RenderTexture(Screen.width, Screen.height,1);
#else
        var stereo = new RenderTexture(XRSettings.eyeTextureDesc);
#endif
        m_txtrScreenShot = new Texture2D(stereo.width, stereo.height, TextureFormat.RGB24, false);
        RegisterCallbacks();
        m_InfiniteBg.Init(30,6);
        EnterSplash();
        MonsterResourceManager.CreateInstance();
        SudoBattleServer.CreateInstance();
        MonsterLeagueSaveManager.CreateInstance();
    }

    void RegisterCallbacks()
    {
        m_PageGen.OnBackHome += GenToTitle;
        m_PageTitle.OnCreate += TitleToCreation;
        m_PageTitle.OnBattle += TitleToBattle;
        m_PageTitle.OnCollection += TitleToCollection;
        m_PageCreation.OnGenerationComplete += CreationToGen;
        m_PageBattle.OnEndBattle += BattleToTitle;
        m_PageCollection.OnBackHome += CollectionToTitle;
    }

    

    #region PageTransitions
    private void TitleToCollection()
    {
        m_PageTitle.ExitPage();
        m_PageCollection.EnterPage();
        m_InfiniteBg.StartPanning(500,2);
    }

    private void CollectionToTitle()
    {
        m_PageCollection.ExitPage();
        m_PageTitle.EnterPage();
        m_InfiniteBg.StartPanning(500,2);
    }

    void EnterSplash()
    {
        m_PageSplash.EnterPage();
        m_PageSplash.Init(SplashToTitle);
    }

    void SplashToTitle()
    {
        m_PageSplash.ExitPage();
        m_PageTitle.EnterPage();
    }

    async void TitleToBattle()
    {
        m_PageTitle.ExitPage();
        m_PageBattle.EnterPage();
        m_InfiniteBg.StartPanning(500,2);
        await m_PageBattle.Init(MonsterLeagueSaveManager.Instance.ActiveMonster, new BattleMonster(new Monster()));
    }

    void BattleToTitle()
    {
        m_PageBattle.ExitPage();
        m_PageTitle.EnterPage();
        m_InfiniteBg.StartPanning(500,2);
    }
    void TitleToCreation()
    {
        m_PageTitle.ExitPage();
        m_PageCreation.EnterPage();
        m_InfiniteBg.StartPanning(500,2);
    }
    private void GenToTitle()
    {
        m_PageGen.ExitPage();
        m_PageTitle.EnterPage();
        m_InfiniteBg.StartPanning(500,2);
    }

    private async void CreationToGen(BattleMonster monster)
    {
        m_PageCreation.ExitPage();
        m_PageGen.EnterPage();
        m_InfiniteBg.StartPanning(500,2);
        await m_PageGen.Init(monster);
    }


    #endregion


    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q) || OVRInput.GetDown(OVRInput.Button.One))
        //{
        //}
        TimerPool.Update();
    }

    public void PickScreenColor(Action<Color> callback)
    {
        RenderPipelineManager.endCameraRendering += TakeShot;
        m_onColorCaptured = callback;
    }

    private void TakeShot(ScriptableRenderContext context, Camera camera) //:Texture2D
    {
        if (!camera.CompareTag(TagHandle.GetExistingTag("CenterEyeCam")))
        {
            return;
        }
        UIManager.Instance.LogText("ButtonPressed");
        m_txtrScreenShot.ReadPixels(new Rect(0, 0, m_txtrScreenShot.width, m_txtrScreenShot.height), 0, 0);
        m_txtrScreenShot.Apply();

        var offset = UIManager.Instance.PickerOffset;
        m_captureColor = m_txtrScreenShot.GetPixelBilinear(offset.x + .5f, offset.y + .5f);
        UIManager.Instance.LogColor(m_captureColor);
        m_onColorCaptured?.Invoke(m_captureColor);
        m_onColorCaptured = null;

        RenderPipelineManager.endCameraRendering -= TakeShot;

        //colorGrabbed = new Color(captureColor.r + colorSaturationAmount, captureColor.g + colorSaturationAmount, captureColor.b + colorSaturationAmount);
        // Destroy(tex);
    }

    public void PlayFlash(TweenCallback callback = null)
    {
        var seq = DOTween.Sequence();
        seq.Append(m_Flash.DOFade(1, 1)).AppendInterval(2f).Append(m_Flash.DOFade(0, 1)).OnComplete(callback);
    }

    public void SetGlitchStatus(bool isOn)
    {
        m_Glitch.SetActive(isOn);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        MonsterResourceManager.DestroyInstance();
        SudoBattleServer.DestroyInstance();
        MonsterLeagueSaveManager.DestroyInstance();
        
    }
}