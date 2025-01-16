using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ParallaxPanningGroup : MonoBehaviour
{
    [SerializeField] private RectTransform _root;
    [SerializeField] private RectTransform _tileL;
    private float TileWidth => _tileL.rect.width;

    void Start()
    {

    }
    public void StartPanning(float duration)
    {
        _root.DOLocalMove(new Vector3(-_tileL.rect.width, 0), duration, true).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            _root.localPosition = Vector3.zero;
        });
    }
}
