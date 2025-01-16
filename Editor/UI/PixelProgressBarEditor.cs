using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;
[CustomEditor(typeof(PixelProgressBar))]
public class PixelProgressBarEditor : ImageEditor
{
    private SerializedProperty m_NumPixels;

    protected override void OnEnable()
    {
        base.OnEnable();

        m_NumPixels = serializedObject.FindProperty(nameof(m_NumPixels));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();


        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_NumPixels);
        if (EditorGUI.EndChangeCheck())
        {
            //if (m_NumPixels.intValue>0)
            //{
            //    var style = XStyleSheet.GetColorStyle(m_ColorStyle.intValue);
            //    m_Color.colorValue = style.color;
            //}
        }
        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }
}
