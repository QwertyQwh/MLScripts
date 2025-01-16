using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MonsterLeagueSplash : MonsterLeaguePage
{
    [SerializeField] private CanvasGroup m_Logo;
    [SerializeField] private RectTransform m_meteor;

    public void Init(TweenCallback callback)
    {
        PlayFade(callback);
    }

    public void PlayFade(TweenCallback callback)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(m_Logo.DOFade(1, 1))
            .AppendInterval(2f)
            .Join(m_meteor.DOLocalMove(new Vector3(-100,-100),3).SetEase(Ease.InSine))
            .Append(m_Logo.DOFade(0, 1))
            .OnComplete(callback);
    }


}
