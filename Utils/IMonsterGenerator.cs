using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEngine;

public interface IMonsterGenerator
{
    public Monster Generate(List<Color> cols, MonsterTier tier);

}
