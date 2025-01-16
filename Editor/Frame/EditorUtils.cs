using System;
using UnityEngine;
using System.ComponentModel;
using System.Collections.Generic;
using UnityEditor;

class GUIHorizontal : IDisposable
{
    public GUIHorizontal(params GUILayoutOption[] options) { GUILayout.BeginHorizontal(options); }
    public GUIHorizontal(GUIStyle style, params GUILayoutOption[] options) { GUILayout.BeginHorizontal(style, options); }
    void IDisposable.Dispose() { GUILayout.EndHorizontal(); }
}

class GUIVertical : IDisposable
{
    public GUIVertical(params GUILayoutOption[] options) { GUILayout.BeginVertical(options); }
    public GUIVertical(GUIStyle style, params GUILayoutOption[] options) { GUILayout.BeginVertical(style, options); }
    void IDisposable.Dispose() { GUILayout.EndVertical(); }
}

class GUIArea : IDisposable
{
    public GUIArea(Rect rect) { GUILayout.BeginArea(rect); }
    void IDisposable.Dispose() { GUILayout.EndArea(); }
}

class GUIScrollView : IDisposable
{
    public Vector2 ScrollPosition;
    public GUIScrollView(Vector2 scrollPosition) { ScrollPosition = GUILayout.BeginScrollView(scrollPosition, GUIStyle.none, GUIStyle.none); }
    public GUIScrollView(Vector2 scrollPosition, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar) { ScrollPosition = GUILayout.BeginScrollView(scrollPosition, horizontalScrollbar, verticalScrollbar); }
    void IDisposable.Dispose() { GUILayout.EndScrollView(); }
}

class GUIDisable : IDisposable
{
    private bool mCondition;

    public GUIDisable(bool condition = true) 
    {
        mCondition = condition;
        if(mCondition)
            GUI.enabled = false; 
    }
    void IDisposable.Dispose() 
    {
        if(mCondition)
            GUI.enabled = true; 
    }
}

class GUIColor : IDisposable
{
    private Color mOldColor;
    private bool mEnable;

    public GUIColor(Color color, bool enable)
    {
        mEnable = enable;
        if(mEnable)
        {
            mOldColor = GUI.color;
            GUI.color = color;
        }
    }

    void IDisposable.Dispose()
    {
        if(mEnable)
            GUI.color = mOldColor;
    }
}

class HandleColor : IDisposable
{
    private Color mOldColor;

    public HandleColor(Color color)
    {
        mOldColor = Handles.color;
        Handles.color = color;
    }

    void IDisposable.Dispose()
    {
        Handles.color = mOldColor;
    }
}

static class EditorUtils
{
    public static bool AnalysisFov(string fovStr, out int[] indexArr, out float[] fovArr)
    {
        if (string.IsNullOrEmpty(fovStr))
        {
            indexArr = null;
            fovArr = null;
            return false;
        }

        var arr = fovStr.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        var fovIndexList = new List<int>();
        var fovValList = new List<float>();

        for (var i = 0; i < arr.Length; i++)
        {
            var fov = arr[i].Split('_');
            var key = fov[0];
            var val = fov[1];
            /*if (i > 0 && val == arr[i - 1].Split('_')[1])
            {
                continue;
            }*/

            fovIndexList.Add(int.Parse(key));
            fovValList.Add(Mathf.Round(float.Parse(val)));
        }

        indexArr = fovIndexList.ToArray();
        fovArr = fovValList.ToArray();
        return true;
    }

    public static string GetDescription(this Enum e)
    {
        var fieldInfo = e.GetType().GetField(e.ToString());
        var descriptions = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (descriptions.Length == 0)
            return null;
        return (descriptions[0] as DescriptionAttribute).Description;
    }
}