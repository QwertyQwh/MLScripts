using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public static class TweenerExtension 
{
    /// <summary>
    /// Use with caution, this assumes the given tweener is a float tweener 
    /// </summary>
    /// <param name="tweener"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    //public static float EstimateFloatAtTime(this Tweener tweener, float time,e)
    //{
    //    if (tweener is not TweenerCore<float, float, FloatOptions>)
    //    {
    //        Debug.LogError("The tweener is not of type float, please check!");
    //        return 0;
    //    }
    //    var cast = (TweenerCore<float, float, FloatOptions>)tweener;
    //    var delta = cast.endValue - cast.startValue;
    //    var percentComplete = time / delta;

    //    return tweener(percentComplete) * delta + startVal
    //    tweener.
    //    var t = seq.fullPosition;
    //    seq.Goto(time);
    //    T val = (T)seq.;
    //    Tweener tt;
    //    seq.Goto(t);
    //    return val;
    //}

    //public static T PeekValue<T>(this Sequence seq, float timeAhead)
    //{
    //    var t = (seq.fullPosition + timeAhead) % seq.fullPosition;
    //    return seq.GetValueAtTime<T>(t);
    //}
}
