using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ParallaxBg : MonoBehaviour
{
    [SerializeField] private RectTransform _far;
    [SerializeField] private RectTransform _mid;
    [SerializeField] private ParallaxPanningGroup _near;

    //For each unit the view moved, how much should the bg move
    private const float FarRatio = 0.05f;
    private const float MidRatio = 0.2f;
    private const float NearRatio = 0.6f;

    private Vector3 _nearPos;
    private Vector3 _midPos;
    private Vector3 _farPos;
    
   
    public void Init(Vector3 nearp, Vector3 midp, Vector3 farp)
    {
        _nearPos = nearp;
        _midPos = midp;
        _farPos = farp;
    }

    public void StartPanning(float duration)
    {
        _near.StartPanning(duration);
    }

    public void UpdateTransform(Func<Vector3,Vector2> GetScreenPos)
    {
        _near.transform.localPosition = new Vector3(GetScreenPos(_nearPos).x,0);
        _mid.transform.localPosition = new Vector3(GetScreenPos(_midPos).x,0);
        _far.transform.localPosition = new Vector3(GetScreenPos(_farPos).x, 0);
    }



}
