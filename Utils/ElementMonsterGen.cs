using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEngine;


/// <summary>
/// Given the list of colors, find their average and compare to the color of each element. Generate a random monster with that element as attrib    
/// </summary>
public class ElementMonsterGen: IMonsterGenerator
{
    public static readonly Dictionary<Color,MonsterElement> DictElementColors = new ();

    public static readonly System.Random Random = new();

    public ElementMonsterGen()
    {
        DictElementColors.Add(Color.red,MonsterElement.Fire);
        DictElementColors.Add(Color.blue,MonsterElement.Water);
        DictElementColors.Add(Color.green,MonsterElement.Earth);
        DictElementColors.Add(Color.white,MonsterElement.Air);
    }

    public Monster Generate(List<Color> cols, MonsterTier tier)
    {
        var averaged = Color.black;
        foreach (var col in cols)
        {
            averaged += col;
        }

        averaged /= cols.Count; 
        //Default Monster
        var monster = new Monster("Slimon");
        var closest = ColorUtils.MatchColorWithRgb(averaged, DictElementColors.Keys.ToList());
        if (closest != null)
        {
            var element = DictElementColors[(Color)closest];
            var filtered = MonsterResourceManager.Instance.DataBase.monsters.Where(x =>
                x.Element == element && x.Tier == tier);
            if (filtered.Any())
            {
                monster = filtered.SelectRandom(Random);
            }
        }
        return monster;
    }
}
