using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using FMOD;
using QFramework.Extensions;
using QFramework.Utils;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MonsterController : MonoBehaviour
{
    public BattleMonster monster;
    [SerializeField] private Animator _animator;
    [SerializeField] private PixelProgressBar _HPBar;
    [SerializeField] private TextMeshProUGUI _txtDamage;
    private Vector3 _originalPos;
    private Vector3 _originalScale;
    private float _origParallaxRatio;
    public Action onDeath;
    public Action<float> onHpUpdate;
    private Vector3 _animOffset;

    private const float PreAttackTime = 1f;
    private const float AttackTime = 0.9f;
    private const float AfterAttackTime = .4f;
    [NonSerialized]
    public Vector3 battlePos;
    [NonSerialized]
    public float battleScale;


    void Awake()
    {
        _originalPos = transform.localPosition;
        _originalScale = transform.localScale;
    }

    void Start()
    {

    }

    /// <summary>
    /// Set the controller data. Also needs to know if the monster is flipped in battle mode (self = false, enemy  = true)
    /// <param name="monster"></param>
    /// <param name="isFlipped"></param>
    /// </summary>
    public async Task SetData(BattleMonster monster, Vector3 pos , float scale = 1, bool isFlipped = false)
    {
        this.monster = monster;
        battlePos = pos;
        battleScale = scale;
        _animator.runtimeAnimatorController =
            await MonsterResourceManager.Instance.LoadAnimatorControllerForMonster(monster.proto.MonsterId);
        if (isFlipped)
        {
            _animator.transform.localScale = new Vector3(-1,1);
        }
    }

    private void ShowDamage(int damage)
    {
        _txtDamage.text = $"-{damage.ToString()}";
        _txtDamage.SetActiveEx(true);
        _txtDamage.DOColor(Color.red, 0.1f).SetLoops(15, loopType: LoopType.Yoyo).OnComplete(()=>
        {
            _txtDamage.SetActiveEx(false);
        });
    }

    private void UpdateHPBar(int target)
    {
        int temp = monster.BattleHP;
        monster.BattleHP = target;
        DOTween.To(() => temp,
            (int val) =>
            {
                temp = val;
                onHpUpdate?.Invoke((float)val / monster.HP);
            },
            target,
            0.8f).SetEase(Ease.OutExpo);

    }

    public void OnHit(int damage)
    {
        int hpLeft = monster.BattleHP - damage;
        ShowDamage(damage);
        UpdateHPBar(hpLeft);
        if (hpLeft < 0)
        {
            OnDeath();
            return;
        }
        _animator.Play("Base.Hit");
    }

    private void OnDeath()
    {
        onDeath?.Invoke();
        _animator.Play("Base.Die");
    }

    public void OnAttack(MonsterController target, Action beginAttackCallback,Action endAttackCallback)
    {
        //var targetRect = target.transform as RectTransform;
        //Debug.Log($"IsOnTheLeft:{transform.localPosition.x < target.transform.localPosition.x} Position: {targetRect.localPosition.x}  Width: {targetRect.rect.width / 2}");
        var targetPos = target.battlePos;
        var isTargetFartherAway = targetPos.z<battlePos.z;
        var AttackEasing = isTargetFartherAway ? Ease.OutExpo : Ease.InExpo;
        var isTargetOntheLeft = targetPos.x < battlePos.x;
        var targetX = isTargetOntheLeft ? targetPos.x + 10 : targetPos.x -10;
        targetPos = new Vector3(targetX, targetPos.y, targetPos.z);
        _animator.Play("Base.Walk");
        
        DOVirtual.Vector3(Vector3.zero,targetPos- battlePos, PreAttackTime, (val) =>
        {
            _animOffset = val;
        }).SetEase(AttackEasing).OnComplete(() =>
        {
            _animator.Play("Base.Attack");
            beginAttackCallback?.Invoke();
            //We need to decide how much time we want the monster to linger after attack
            TimerPool.Start(AttackTime * 1000, AttackTime * 1000, (int val) => FinishAttackNReturn(val,  endAttackCallback));
        });


        
    }

    public void FinishAttackNReturn(int _, Action endAttackCallback)
    {
        DOVirtual.Vector3(_animOffset, Vector3.zero, AfterAttackTime,(val) => { _animOffset = val; }).SetEase(Ease.Linear).OnComplete(() => endAttackCallback());
    }

    public void UpdateTransform(Func<Vector3, Vector2> GetScreenPos, Func<Vector3, float> GetScreenScale)
    {
        var truePos = battlePos + _animOffset;
        transform.localPosition = GetScreenPos(truePos);
        //Debug.Log($"WorldPos:{battlePos + _animOffset} localPos: {transform.localPosition}");
        //Make them bigger just for visual
        transform.localScale = 2*GetScreenScale(truePos)*Vector3.one;
    }

    void Update()
    {
        //Debug.Log($"animoffset:{_animOffset}");
        //Use Tween when possible. Use update only when the Anim function is too complicated.
        //transform.localPosition = _animOffset + new Vector3((_originalPos .x+_parallaxOffsetx) *_parallaxRatio, _originalPos.y,0);
    }


}
