using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InfiniteBg : MonoBehaviour
{
    [SerializeField] private RectTransform _First;
    [SerializeField] private RectTransform _Second;
    private float _tileWidth => _First.rect.width;
    public float ScreenWidth;
    private float _floatingOffset;
    private float _pan;
    private float _firstOffset ;
    private float _secondOffset ;

    // Start is called before the first frame update
    public void Init(float MaxOffset, float duration)
    {
        DOVirtual.Float(0, -MaxOffset, duration, (val) => { _floatingOffset = val; }).SetEase(Ease.InOutCubic).SetLoops(-1,LoopType.Yoyo);
        _firstOffset = -_tileWidth;
        _secondOffset = 0;
    }

    public void StartPanning(float length,float duration)
    {
        DOVirtual.Float(_pan, _pan - length, duration, (val) => { _pan = val; }).SetEase(Ease.InOutCubic);
    }

    // Update is called once per frame
    void Update()
    {
        if (_First.localPosition.x <= -ScreenWidth - _tileWidth)
        {
            _firstOffset += _tileWidth * 2;
        }
        if (_Second.localPosition.x <= -ScreenWidth - _tileWidth)
        {
            _secondOffset += _tileWidth * 2;
        }
        _First.localPosition = new Vector3(_pan + _floatingOffset + _firstOffset, 0);
        _Second.localPosition = new Vector3(_pan + _floatingOffset + _secondOffset, 0);
    }
}
