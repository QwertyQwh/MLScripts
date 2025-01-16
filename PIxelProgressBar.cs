using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelProgressBar : Image
{
    [SerializeField]
    private int m_NumPixels;


    protected override void OnEnable()
    {
        base.OnEnable();
        type = Type.Filled;
    }

    public void SetProgress(float progress)
    {
        fillAmount = (int)(progress*m_NumPixels)/(float)m_NumPixels;
    }
}
