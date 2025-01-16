using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraLimitedFPS : MonoBehaviour
{
    public Camera Cam;
    public int Fps;
    // Start is called before the first frame update
    private float spf => 1 / (float)Fps;
    private float accum = 0;
    private Texture2D captured;
    void Start()
    {
        captured = new Texture2D(1, 1, TextureFormat.RGBA32, false, true);
    }

    public void CaptureColor(Action<Color> OnCaptureColor)
    {
        RenderTexture.active = Cam.targetTexture;
        captured.ReadPixels(new Rect(31, 31, 1, 1), 0, 0, false);
        OnCaptureColor.Invoke(captured.GetPixel(0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        accum += Time.deltaTime;
        if (accum > spf)
        {
            Cam.Render();
            accum = 0;
        }
    }
}
