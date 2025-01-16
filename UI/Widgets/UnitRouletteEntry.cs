using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitRouletteEntry : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Image _ImgMultiplier;
    public RouletteEntry entry;
    public int entryId;
    void Start()
    {
        
    }

    public void SetData(RouletteEntry given,int id,Sprite icon)
    {
        _ImgMultiplier.sprite = icon;
        this.entry = given;
        entryId = id;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
