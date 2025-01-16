#define DEBUG_BATTLE
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;


//Battle Stage:
// 1. Start Turn
// 2. Input 
// 3. Attack: Combo fixed, calc damage, play attack anim, etc.
// 4. End turn: Enemy Turn (same logic w/o stage 1&2)
public enum BattleSide
{
    Self = 0,
    Enemy = 1,
}

public enum BattlePhase
{
    Start,
    Input,
    Attack,
    End
}

public class MonsterLeagueBattle : MonsterLeaguePage
{
    [SerializeField] private RectTransform _rootScene;
    [SerializeField] private MonsterController _ctrlEnemy;
    [SerializeField] private MonsterController _ctrlSelf;
    [SerializeField] private MonsterLeagueBattleHUD _hud;
    [SerializeField] private ParallaxBg _bg;
    [SerializeField] private GameObject _pageWin;
    [SerializeField] private GameObject _pageLost;
    [SerializeField] private PixelProgressBar _hpSelf;
    [SerializeField] private PixelProgressBar _hpEnemy;

    public Action OnEndBattle;
    private bool _hasEnded = false;
    private BattlePhase _phase;

    private Vector3 _cameraPos = new(0,2,100);
    private const float CameraFocalLength = 10;
    private const float CameraFov = 90;
    //A trick to force position the monsters
    private const float HorizonLevel = 45;
    private float CameraWidth => CameraFocalLength*MathF.Tan(CameraFov/2)*2;
    private float CameraHeight => CameraWidth*192/256;
    private float CameraUnit => CameraWidth / 256;

    /// <summary>
    /// Project the object Pos from 3d space onto screen
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private Vector2 GetScreenPos(Vector3 pos)
    {
        var scale = GetScreenScale(pos);
        var screenPos = (pos - _cameraPos) * scale;
        //Debug.Log($"[Screen] worldPos: {pos} cameraPos: {_cameraPos} screenPos:{screenPos}");
        return new Vector2((int)(screenPos.x / CameraUnit), (int)(screenPos.y / CameraUnit)-HorizonLevel);
    }

    

    private float GetScreenScale(Vector3 pos)
    {
        return  CameraFocalLength/(_cameraPos.z - pos.z) ;
    }

    public override void ExitPage()
    {
        base.ExitPage();
        _hasEnded = false;
    }

    protected override void RegisterInputs()
    {
        base.RegisterInputs();
        _hud.OnInputComplete += OnInputComplete;
        _ctrlSelf.onDeath += ShowLose;
        _ctrlEnemy.onDeath += ShowWin;
        _ctrlSelf.onHpUpdate += (float progress) => OnHpUpdate(BattleSide.Self, progress);
        _ctrlEnemy.onHpUpdate += (float progress) => OnHpUpdate(BattleSide.Enemy, progress);
    }
     
    public async Task Init(BattleMonster ours, BattleMonster theirs)
    {
        ours.Reset();
        theirs.Reset();
        _hud.Init(ours, theirs);
        PhaseInput(BattleSide.Self);
        _hpEnemy.SetProgress(1);
        _hpSelf.SetProgress(1);
        await _ctrlSelf.SetData(ours,new Vector3(-15,0,80),1);
        await _ctrlEnemy.SetData(theirs,new Vector3(5,0,60),1, true);
        _bg.Init(new Vector3(0,0,60), new Vector3(0,0,0),new Vector3(0,0,-200));
        StartParallax();
    }

    protected override void UnregisterInputs()
    {
        base.UnregisterInputs();
        _hud.OnInputComplete -= OnInputComplete;
    }

    private void StartParallax()
    {
        DOVirtual.Vector3(new Vector3(-10, 12f, 100), new Vector3(10, 12f, 100), 4, (val) => { _cameraPos = val; })
            .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    void Update()
    {
        _bg.UpdateTransform(GetScreenPos);
        _ctrlSelf.UpdateTransform(GetScreenPos,GetScreenScale);
        _ctrlEnemy.UpdateTransform(GetScreenPos,GetScreenScale);
    }
    
    private void OnInputComplete(BattleAction action, int combo)
    {
        switch (action)
        {
            case BattleAction.Attack:
                PhaseAttack(BattleSide.Self,DamageSystem.CalcDamage(_ctrlSelf.monster,_ctrlEnemy.monster,combo) );
                break;
            case BattleAction.Magic:
                PhaseAttack(BattleSide.Self,50);
                break;
            case BattleAction.Escape:
                ShowLose();
                break;
            case BattleAction.Defense:
                DefenseBuff buff = new DefenseBuff(1, 2, 0);
                buff.AttachTo(_ctrlSelf.monster);
                PhaseEndTurn(BattleSide.Self);
                break;
        }
    }

    private void ShowWin()
    {
        _pageWin.SetActive(true);
        _hasEnded = true;
        MonsterLeagueInput.Instance.oPressed += BackToTitle;
    }

    private void ShowLose()
    {
        _pageLost.SetActive(true);
        _hasEnded = true;
        MonsterLeagueInput.Instance.oPressed += BackToTitle;
    }


    private void BackToTitle()
    {
        _pageLost.SetActive(false);
        _pageWin.SetActive(false);
        MonsterLeagueInput.Instance.oPressed -= BackToTitle;
        OnEndBattle?.Invoke();
    }

    private void OnHpUpdate(BattleSide side, float left)
    {
        if (side == BattleSide.Self)
            _hpSelf.SetProgress(left);
        else
            _hpEnemy.SetProgress(left);
    }



    #region phases
    private void PhaseStartTurn(BattleSide whoseTurn)
    {
#if DEBUG_BATTLE
        Debug.Log($"[Battle] Starting turn: {whoseTurn}");
#endif
        _phase = BattlePhase.Start;
        (whoseTurn == BattleSide.Self ? _ctrlSelf.monster : _ctrlEnemy.monster).CountdownBuffs();
        PhaseInput(whoseTurn);
    }
    private void PhaseInput(BattleSide whoseTurn)
    {
#if DEBUG_BATTLE
        Debug.Log($"[Battle] Input phase: {whoseTurn}");
#endif
        _phase = BattlePhase.Input;
        if (whoseTurn == BattleSide.Self)
            _hud.BeginInput();
        else
        {
            //Auto calculate damage for enemy
            PhaseAttack(BattleSide.Enemy,DamageSystem.CalcDamage(_ctrlEnemy.monster,_ctrlSelf.monster,2));
        }
    }
    private void PhaseAttack(BattleSide whoseTurn, int damage)
    {
        Debug.Log($"[Battle] Attack phase: {whoseTurn}");
        _phase = BattlePhase.Attack;
        if (whoseTurn == BattleSide.Self)
        {
            _ctrlSelf.OnAttack(_ctrlEnemy,
                () =>
                {
                    _bg.StartPanning(2f);
                    _ctrlEnemy.OnHit(damage);
                },
                () =>
                {
                    PhaseEndTurn(whoseTurn);
                    DeFocus(1);
                });
            FocusOnTarget((RectTransform)_ctrlEnemy.transform,2f,1);
        }
        else
            _ctrlEnemy.OnAttack(_ctrlSelf, 
                ()=>_ctrlSelf.OnHit(damage),
                ()=>PhaseEndTurn(whoseTurn));
    }

    private void PhaseEndTurn(BattleSide whoseTurn)
    {
        Debug.Log($"[Battle] End phase: {whoseTurn}");
        _phase = BattlePhase.End;
        if (_hasEnded)
            return;
        PhaseStartTurn(1-whoseTurn);
    }

    private void FocusOnTarget(RectTransform target,float scale,float duration)
    {
        var targetX = math.clamp(-scale * (target.localPosition.x), -(scale - 1) * 128, (scale - 1) * 128);
        var targetY = math.clamp(-scale * (target.localPosition.y +20) , -(scale - 1) * 96, (scale - 1) * 96);
        _rootScene.DOScale(Vector3.one*scale,duration);
        _rootScene.DOLocalMove(new Vector3(targetX, targetY, 0),
        duration, true);
    }

    private void DeFocus(float duration)
    {
        _rootScene.DOScale(Vector3.one, duration);
        _rootScene.DOLocalMove(Vector3.zero, duration);
    }
    

    #endregion


}
