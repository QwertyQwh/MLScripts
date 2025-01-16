using System.Collections;
using System.Collections.Generic;
using QFramework.Utils;
using UnityEngine;

//The enums are directly used to reference anim controller, DO NOT RENAME
public enum RouletteEntry
{
    Six, Five, Four, Three, Two, One, Clear, End
}



public class RouletteSeq
{
    private int curIndex = -1;
    private List<RouletteEntry> _entries = new();
    private List<int> _skippedIds = new();
    public int TotalCount => _entries.Count;
    

    public void Init(BattleMonster ours, BattleMonster theirs)
    {
        _entries = new();
        _skippedIds = new();
        var advantageElem = Monster.ElementDominates(ours.Element, theirs.Element);
        //test
        Debug.Log($"advantaged?{advantageElem} ours: {ours.Element} theirs: {theirs.Element}");
        for (int i = 0; i < 20; i++)
        {
            _entries.Add(GenerateRandomEntry(advantageElem));
        }

        curIndex = -1;
    }

    private RouletteEntry GenerateRandomEntry(int advantaged)
    {
        var rand = Random.Range(0, 101);
        return advantaged switch
        {
            0 => rand switch
            {
                <= 20 => RouletteEntry.End,
                <= 40 => RouletteEntry.Clear,
                <= 80 => RouletteEntry.One,
                <= 90 => RouletteEntry.Two,
                <= 95 => RouletteEntry.Three,
                <= 99 => RouletteEntry.Four,
                _ => RouletteEntry.Six
            },
            1 => rand switch
            {
                <= 10 => RouletteEntry.End,
                <= 20 => RouletteEntry.Clear,
                <= 60 => RouletteEntry.One,
                <= 70 => RouletteEntry.Two,
                <= 80 => RouletteEntry.Three,
                <= 90 => RouletteEntry.Four,
                <= 95 => RouletteEntry.Five,
                _ => RouletteEntry.Six
            },
            -1 => rand switch
            {
                <= 35 => RouletteEntry.End,
                <= 70 => RouletteEntry.Clear,
                <= 90 => RouletteEntry.One,
                <= 95 => RouletteEntry.Two,
                <= 99 => RouletteEntry.Three,
                _ => RouletteEntry.Four
            },
            _ => RouletteEntry.One
        };
    }

    public RouletteEntry MoveNext(out int index)
    {
        
        //loop to the start if cur entry is the last
        if (++curIndex >= _entries.Count)
            curIndex = 0;
        index = curIndex;
        if (!_skippedIds.Contains(curIndex)) return _entries[curIndex];
        var next = MoveNext(out index);
        return next;
    }

    public void RemoveById(int index)
    {
        _skippedIds.Add(index);
    }

    public RouletteEntry PeekNext(out int index)
    {
        index = curIndex + 1 < _entries.Count ? curIndex + 1 : 0;
        return _entries[index];
    }
}
