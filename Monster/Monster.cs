using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using QFramework.Utils;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;


public enum MonsterTier
{
    Rookie = 0,
    Adventurer = 1,
    Hero = 2
}



//This is really a dictionary disguised as a list. For editor/database use only since search is inefficient.
//[JsonObject]

//public class MonsterDataList : IList<Monster>
//{
//    [JsonProperty("Data")]
//    private List<Monster> _monsterList = new();
//    public IEnumerator<Monster> GetEnumerator()
//    {
//        return _monsterList.GetEnumerator();
//    }

//    IEnumerator IEnumerable.GetEnumerator()
//    {
//        return GetEnumerator();
//    }

//    public void Add(Monster item)
//    {
//        if (Contains(item))
//        {
//            Debug.LogError("Duplicate Monster Id");
//            return;
//        }
//        _monsterList.Add(item);
//    }

//    public void Clear()
//    {
//        _monsterList.Clear();
//    }

//    public bool Contains(Monster item)
//    {
//        return item!=null && _monsterList.Any(monster => monster.MonsterId == item.MonsterId);
//    }

//    public void CopyTo(Monster[] array, int arrayIndex)
//    {
//        _monsterList.CopyTo(array,arrayIndex);
//    }

//    public bool Remove(Monster item)
//    {
//        if (item == null) return false;
//        foreach (var monster in _monsterList.Where(monster => monster.MonsterId == item.MonsterId))
//        {
//            _monsterList.Remove(monster);
//            return true;
//        }

//        return false;
//    }

//    public int Count => _monsterList.Count;
//    public bool IsReadOnly => false;
//    public int IndexOf(Monster item)
//    {
//        return _monsterList.IndexOf(item);
//    }

//    public void Insert(int index, Monster item)
//    {
//        if (Contains(item))
//        {
//            Debug.LogError("Duplicate Monster Id");
//            return;
//        }
//        _monsterList.Add(item);
//    }

//    public void RemoveAt(int index)
//    {
//        _monsterList.RemoveAt(index);
//    }

//    public Monster this[int index]
//    {
//        get => _monsterList[index];
//        set => _monsterList[index] = value;
//    }
//}

[JsonObject]
public class MonsterDb
{
    //[JsonIgnore]
    //public Dictionary<string, Monster> dictMonsters = new Dictionary<string, Monster>();
    [JsonProperty("monsters")]
    public List<Monster> monsters = new ();

    //public bool TryAddMonster(string id, Monster monster)
    //{
    //    if (_dictMonsters.TryAdd(id, monster))
    //    {
    //        return true;
    //    }
    //    Debug.LogError("Monster already there");
    //    return false;

    //}

    //public bool TryGetMonster(string id, out Monster monster)
    //{
    //    monster = null;
    //    if (!_dictMonsters.TryGetValue(id, out var val)) return false;
    //    monster = val;
    //    return true;

    //}

}

public class MonsterResourceManager : Singleton<MonsterResourceManager>
{
    public static readonly string kRootPath = Application.streamingAssetsPath + "/MonsterDataBase";
    public MonsterDb DataBase;

    protected override void Init()
    {
        var path = kRootPath + "/test.json";
        var bytes = File.ReadAllBytes(path);
        var configData = Encoding.UTF8.GetString(bytes);
        DataBase = JsonConvert.DeserializeObject<MonsterDb>(configData);
    }

    protected override void Dispose()
    {
        
    }

    public Monster LookupMonster(string id)
    {
        var res = DataBase.monsters.Find(x => x.MonsterId == id);
        if (res == null)
            Debug.LogError($"The monster with id {id} is not found in the database.");
        return res;
    }

    public async Task<T> LoadAsset<T>(string id)
    {
        return await Addressables.LoadAssetAsync<T>(id).Task;
    }

    public async Task<AnimatorController> LoadAnimatorControllerForMonster(string id)
    {
        return await Addressables.LoadAssetAsync<AnimatorController>($"AnimCtrl_{id}").Task;
    }
    
    public async Task<AnimatorController> LoadIconForMonster(string id)
    {
        return await Addressables.LoadAssetAsync<AnimatorController>($"Icon_{id}").Task;
    }

    
}


public enum MonsterElement
{
    Fire = 0,
    Water = 1,
    Earth = 2,
    Air = 3
}



[JsonObject]

public class Stat
{
    [JsonProperty("Base")]
    private int m_StatBase;
    [JsonProperty("LevelUp")]
    private int m_StatLevelUp;

    [JsonIgnore]
    public int Base {
        get => m_StatBase;
#if UNITY_EDITOR
        set => m_StatBase = value;
#endif
    }
    [JsonIgnore]
    public int LevelUp
    {
        get => m_StatLevelUp;
#if UNITY_EDITOR
        set => m_StatLevelUp = value;
#endif
    }

    public Stat(int baseState, int growth)
    {
        m_StatBase = baseState;
        m_StatLevelUp = growth;
    }

    public Stat(Stat another)
    {
        m_StatBase = another.m_StatBase;
        m_StatLevelUp = another.m_StatLevelUp;
    }

    
    public int GetStat(int level)
    {
        return m_StatBase + level * m_StatLevelUp;
    }


}

//List of Anims:
//Idle, Walk?, StatAttack, Hit, Dead
[JsonObject,Description("Monster")]
public class Monster
{
    [JsonProperty("Id")]
    public string MonsterId;

    [JsonProperty("Tier")]
    [JsonConverter(typeof(StringEnumConverter))]
    private MonsterTier m_tier;
    [JsonIgnore]
    public MonsterTier Tier
    {
        get =>m_tier;
#if UNITY_EDITOR
        set => m_tier = value;
#endif
    }
    public Stat StatHP;
    public Stat StatAttack;
    public Stat StatDefense;
    public Stat StatMagic;
    [JsonConverter(typeof(StringEnumConverter))]
    public MonsterElement Element;

    [JsonIgnore]
    public string IconAddress => $"Icon_{MonsterId}";




    //>0 if ours dominate theirs, = 0 if none, <0 if ours is dominated by theirs
    public static int ElementDominates(MonsterElement ours, MonsterElement theirs)
    {
        return ours switch
        {
            MonsterElement.Water when theirs == MonsterElement.Fire => 1,
            MonsterElement.Fire when theirs == MonsterElement.Fire => -1,
            MonsterElement.Earth when theirs == MonsterElement.Water => 1,
            MonsterElement.Water when theirs == MonsterElement.Earth => -1,
            MonsterElement.Air when theirs == MonsterElement.Earth => 1,
            MonsterElement.Earth when theirs == MonsterElement.Air => -1,
            MonsterElement.Fire when theirs == MonsterElement.Air => 1,
            MonsterElement.Air when theirs == MonsterElement.Fire => -1,
            _ => 0
        };
    }
    

    //Generate an empty monster template (for editor use only)
    public Monster()
    {
        StatHP = new Stat(100, 10);
        StatAttack = new Stat(5, 5);
        StatDefense = new Stat(1, 1);
        StatMagic = new Stat(1, 1);
        m_tier = MonsterTier.Rookie;
        Element = MonsterElement.Earth;
        MonsterId = "Slimon";
    }

    public Monster(Monster another)
    {
        StatAttack = new Stat(another.StatAttack);
        StatDefense = new Stat(another.StatDefense);
        StatMagic = new Stat(another.StatMagic);
        StatHP = new Stat(another.StatHP);
        m_tier = another.Tier;
        Element = another.Element;
        MonsterId = another.MonsterId;
    }
    //Generate a monster based on its id 
    public Monster(string id):this(MonsterResourceManager.Instance.LookupMonster(id))
    {

    }

}

//A wrapper for battles 
public class BattleMonster
{
    public Monster proto;
    //Helper Attrib
    public int HP => proto.StatHP.GetStat(Level);
    public int Attack => proto.StatAttack.GetStat(Level);
    public int Defense => proto.StatDefense.GetStat(Level);
    public int Magic => proto.StatMagic.GetStat(Level);
    public MonsterElement Element => proto.Element;
    //These are the realtime stats in battle
    private int _battleHP;
    public int BattleHP
    {
        get => _battleHP;
        set => _battleHP = Math.Clamp(value,0,HP);
    }
    public int BattleAttack;
    public int BattleDefense;
    public int BattleMagic;
    public string MonsterId => proto.MonsterId;

    public string InstanceId;
    public int Level { get; }

    private List<BattleBuff> _buffs = new ();

    public BattleMonster(Monster proto)
    {
        this.proto = proto;
        Level = 1;
        InstanceId = Guid.NewGuid().ToString();
        Reset();
    }

    public void Reset()
    {
        BattleHP = HP;
        BattleAttack = Attack;
        BattleDefense = Defense;
        BattleMagic = Magic;
    }

    public void RemoveBuff(BattleBuff buff)
    {
        var success = _buffs.Remove(buff);
    }

    public void AttachBuff(BattleBuff buff)
    {
        _buffs.Add(buff);
    }

    public void CountdownBuffs()
    {
        //Iterate in reverse order as we might remove item
        for (int i = _buffs.Count-1; i >= 0; i--)
        {
            _buffs[i].CountDown();
        }

    }

}

