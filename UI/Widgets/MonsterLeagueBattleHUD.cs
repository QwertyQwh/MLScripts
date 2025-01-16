using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class MonsterLeagueBattleHUD : MonoBehaviour
{
    [SerializeField] private RectTransform _TBigBar;
    [SerializeField] private RectTransform _TSmallBar;
    [SerializeField] private RectTransform _TFrame;
    [SerializeField] private MonsterLeagueBattleRoulette _roulette;
    [SerializeField] private UnitSelectableList _menu;
    [SerializeField] private GameObject _iconEndTurn;

    private const float k_PopDist = 67f;
    private Vector3 _PopTarget = Vector3.zero;
    private Vector3 _PopInit = Vector3.zero;

    public Action<BattleAction,int> OnInputComplete;
    private BattleMonster _ours;
    private BattleMonster _theirs;
    private bool _isRouletteSpinning;

    void Awake()
    {
        //Assuming the three components have the same localposition
        _PopTarget = _TBigBar.localPosition;
        //Offset the layout 
        _PopInit = _TBigBar.localPosition-new Vector3(0, k_PopDist,0);
    }
    

    public void Init(BattleMonster ours, BattleMonster theirs)
    {
        _ours  = ours;
        _theirs = theirs;
        _TBigBar.localPosition = _PopInit;
        _TSmallBar.localPosition = _PopInit;
        _TFrame.localPosition = _PopInit;
        for (int i = 0; i <_menu.Count; i++)
        {
            _menu.SelectableList[i].ItemId = i;
        }
    }

    private void RegisterInputs()
    {
        Debug.Log("attaching menu action");
        _menu.OnSelected += OnMenuSelect;
        MonsterLeagueInput.Instance.xPressed += StopRoulette;
    }

    private void UnregisterInputs()
    {
        MonsterLeagueInput.Instance.xPressed -= StopRoulette;
        _menu.OnSelected -= OnMenuSelect;
    }
    private void StopRoulette()
    {
        if (_isRouletteSpinning)
            _roulette.StopRoulette();
    }


    private void OnMenuSelect(UnitSelectable selected)
    {
        _menu.DisableInput();
        switch ((selected as UnitBattleAction).action)
        {
            case BattleAction.Attack:
                _iconEndTurn.SetActive(true);
                _isRouletteSpinning = true;
                _roulette.StartSpin((int combo) =>
                {
                    PlayPopOut(); 
                    OnInputComplete?.Invoke(BattleAction.Attack, combo); 
                    _iconEndTurn.SetActive(false);
                    _isRouletteSpinning = false;
                });
                break;
            case BattleAction.Defense:
                PlayPopOut();
                OnInputComplete?.Invoke(BattleAction.Defense,-1);
                break;
            case BattleAction.Magic:
                PlayPopOut();
                OnInputComplete?.Invoke(BattleAction.Magic,-1);
                break;
            case BattleAction.Escape:
                OnInputComplete?.Invoke(BattleAction.Escape,-1);
                break;

        }
    }

    public void BeginInput()
    {
        //1.popin 2. initiate roulette 3.wait for menu input 4. if attack/magic wait for roulette to complete
        Debug.Log("input menu popping up");
        _menu.Reset();
        _roulette.Init(_ours,_theirs);
        PlayPopIn();
    }
    private void PlayPopIn(Action onFinish = null)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_TBigBar.DOLocalMove(_PopTarget, 1f, true).SetEase(Ease.OutExpo))
            .Join(_TSmallBar.DOLocalMove(_PopTarget, 1f, true).SetDelay(0.4f).SetEase(Ease.OutExpo))
            .Join(_TFrame.DOLocalMove(_PopTarget, 1f, true).SetDelay(0.3f).SetEase(Ease.OutExpo));
        onFinish += RegisterInputs;
        seq.onComplete += () => onFinish?.Invoke();
    }

    private void PlayPopOut(Action onFinish = null)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_TFrame.DOLocalMove(_PopInit, 1f, true).SetEase(Ease.InExpo)).
            Join(_TSmallBar.DOLocalMove(_PopInit, 1f, true).SetDelay(0.4f).SetEase(Ease.InExpo)).
            Join(_TBigBar.DOLocalMove(_PopInit, 1f, true).SetDelay(0.6f).SetEase(Ease.InExpo));
        _iconEndTurn.SetActive(false);
        onFinish += UnregisterInputs;
        seq.OnComplete(()=>onFinish?.Invoke());
;    }


}
