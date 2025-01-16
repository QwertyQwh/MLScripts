using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class UIManager : SingletonBehavior<UIManager>
{
    // Start is called before the first frame update
    public RectTransform Picker;
    public Image ColorLogger;
    public TMP_Text TextLogger;

    public Vector2 PickerOffset =>
        new Vector2(Picker.anchoredPosition.x / Screen.width, Picker.localPosition.y / Screen.height);
    protected override void Start()
    {
        base.Start();
#if UNITY_EDITOR
        TextLogger.gameObject.SetActive(false);
        ColorLogger.gameObject.SetActive(false);
#endif
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void LogColor(Color col)
    {
        ColorLogger.color = col;
    }

    public void LogText(string txt)
    {
        TextLogger.text = txt;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
