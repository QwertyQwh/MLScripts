using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMonsterListArrow : MonoBehaviour{
    [SerializeField] private Sprite m_ImgPressed;
    [SerializeField] private Sprite m_ImgNormal;
    [SerializeField] private Sprite m_ImgDisabled;
    [SerializeField] private Image m_Display;



    public void Enable(bool active)
    {
        m_Display.sprite = active ? m_ImgNormal : m_ImgDisabled;
    }
}
