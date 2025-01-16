using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QFramework.Extensions;
using QFramework.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static UnityEngine.Rendering.DebugUI;


//We want to keep three icons at any time frame, the roulette sequence itself will be abstractly represented, and we set the data for three displays as it rotate.
public class MonsterLeagueBattleRoulette : MonoBehaviour
{
    [SerializeField] 
    private PixelProgressBar m_RouletteCountdown;
    [SerializeField]
    private List<UnitRouletteEntry> _entries = new();
    [SerializeField]
    private Sprite[] _multipliers;
    [SerializeField]
    private TextMeshProUGUI _txtCombo;

    [SerializeField] private Animator _AnimShatter;

    private float _speedMult = 1f;
    private float Speed => SpeedBase * _speedMult;
    private float _timeMult = 1f;
    private float _timeLeft = 1f;
    private float TotalTime => TimeBase * _timeMult;
    private RouletteSeq _rouletteSeq = new();
    private Action<int> OnRouletteComplete;
    private bool _isSpinning = false;
    private int _combo = 0;
    private bool _isSpamCooling = false;
    private Timer _pauser;

    public int Combo
    {
        get => _combo;
        set
        {
            _combo = value;
            _txtCombo.text = $"{_combo}x";
        }
}

    //Arbitrary consts
    private const float SpeedBase = 50f;
    private const float EntryInterval = 15;
    private const float UpperBound = 23;
    private const float TimeBase = 10f;
    private const float SelectTolerance = 5f;
    private const float SpamCooldown = 200f; //0.2s

    public void Init(BattleMonster ours, BattleMonster theirs)
    {
        //Three aspects: roulette speed, roulette entries, total time
        _rouletteSeq.Init(ours, theirs);
        //spin faster if their level is higher than ours, then clamp
        var levelDiff = theirs.Level - ours.Level;
        _speedMult += Math.Clamp(levelDiff,0,10) * 0.1f;
        for (var i = 0; i < _entries.Count; i++)
        {
            _entries[i].transform.localPosition = new Vector3(0, (i - 1) * EntryInterval);
            var next = _rouletteSeq.MoveNext(out var entryId);
            _entries[i].SetData(next, entryId, _multipliers[(int)next]);
            _entries[i].SetActiveEx(true);
        }
        _speedMult = 1f;
        _timeMult = 1f;
        _timeLeft = TotalTime;
        m_RouletteCountdown.SetProgress(1);
        Combo = 0;
    }

    public void StartSpin(Action<int> onComplete)
    {
        _txtCombo.SetActiveEx(true);
        _isSpinning = true;
        OnRouletteComplete = onComplete;
        MonsterLeagueInput.Instance.oPressed += OnUserSelect;
    }



    private void OnUserSelect()
    {
        if(_isSpamCooling)
            return;
        foreach (var unit in _entries.Where(unit => Math.Abs(unit.transform.localPosition.y) < SelectTolerance && unit.isActiveAndEnabled))
        {
            
            _AnimShatter.Play($"Base.{unit.entry}",-1,0);
            PauseRoulette(300f);
            unit.SetActiveEx(false);
            Entry2Combo(unit);
        }
        _isSpamCooling = true;
        TimerPool.Start(SpamCooldown, SpamCooldown, (val) => { _isSpamCooling = false; });
    }

    private void Entry2Combo(UnitRouletteEntry unit)
    {
        switch (unit.entry)
        {
            case RouletteEntry.One:
                Combo += 1;
                break;
            case RouletteEntry.Two:
                Combo += 2;
                break;
            case RouletteEntry.Three:
                Combo += 3; 
                _rouletteSeq.RemoveById(unit.entryId);
                //spin faster
                _speedMult += 1f / _rouletteSeq.TotalCount;
                break;
            case RouletteEntry.Four:
                Combo += 4; 
                _rouletteSeq.RemoveById(unit.entryId);
                //spin faster
                _speedMult +=  1f/_rouletteSeq.TotalCount;
                break;
            case RouletteEntry.Five:
                _rouletteSeq.RemoveById(unit.entryId);
                //spin faster
                _speedMult +=  1f/ _rouletteSeq.TotalCount;
                Combo += 5;
                break;
            case RouletteEntry.Six:
                _rouletteSeq.RemoveById(unit.entryId);
                //spin faster
                _speedMult +=  1f/ _rouletteSeq.TotalCount;
                Combo += 6;
                break;
            case RouletteEntry.Clear:
                Combo  = 0;
                break;
            case RouletteEntry.End:
                StopRoulette();
                break;
        }
    }

    public void StopRoulette()
    {
        _isSpinning = false;
        _pauser?.Close();
        _pauser = null;
        MonsterLeagueInput.Instance.oPressed -= OnUserSelect;
        _txtCombo.SetActiveEx(false);
        OnRouletteComplete?.Invoke(Combo);
    }

    private void PauseRoulette(float duration)
    {
        _isSpinning = false;
        _pauser = TimerPool.Start(duration, duration, (_) => { _isSpinning = true; });
    }
    

    void Update()
    {
        if (!_isSpinning)
            return;
        //Move the roulette
        foreach (var unit in _entries)
        {
            var pos = unit.transform.localPosition;
            var newY = pos.y + Time.deltaTime * Speed;
            //Once the uppermost entry hit a boundary we would move it to the bottom, i.e. y+=51
            unit.transform.localPosition = newY < UpperBound ? new Vector3(pos.x, newY) : new Vector3(pos.x, newY - EntryInterval * 3);
            if (newY < UpperBound) continue;
            var next = _rouletteSeq.MoveNext(out var entryId);
            unit.SetData(next, entryId, _multipliers[(int)next]);
            unit.SetActiveEx(true);
        }
        //Time meter
        _timeLeft -= Time.deltaTime;
        m_RouletteCountdown.SetProgress(_timeLeft/TotalTime);
        if (_timeLeft <= 0)
            StopRoulette();
    }
}
