using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ColorUtils
{
    public static Color GenRandColor()
    {
        return Random.ColorHSV();
    }

    public static float GetHue(Color color)
    {
        Color.RGBToHSV(color, out var h, out _, out _);
        return h;
    }

    public static float GetHueDist(Color col1, Color col2)
    {
        var hue1 = GetHue(col1);
        var hue2 = GetHue(col2);
        var delta = math.abs(hue1 - hue2);
        return math.min(delta, 1 - delta);
        
    }

    public static float GetRgbDist(Color col1, Color col2)
    {
        var dist = Vector3.Distance(new Vector3(col1.r, col1.g, col1.b), new Vector3(col2.r, col2.g, col2.b));
        return dist;
    }

    public static Color? MatchColorWithHue(Color color, List<Color> colors)
    {
        if (colors == null || colors.Count == 0)
        {
            Debug.LogError("The Colors list provided is invalid");
            return null;
        }

        var closest = colors.OrderByDescending(x => GetHueDist (color, x) ).Last();
        return closest;
    }

    public static Color? MatchColorWithRgb(Color color, List<Color> colors)
    {
        if (colors == null || colors.Count == 0)
        {
            Debug.LogError("The Colors list provided is invalid");
            return null;
        }

        var closest = colors.OrderByDescending(x => GetRgbDist(color, x)).Last();
        return closest;
    }
}
