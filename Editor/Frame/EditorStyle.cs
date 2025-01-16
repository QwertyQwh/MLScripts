using UnityEditor;
using UnityEngine;
using System;

public class EditorStyle
{
    public static readonly GUIStyle foldout = new GUIStyle("foldout");
    public static readonly GUIStyle flowBackground = new GUIStyle("flow background");
    public static readonly GUIStyle subFlowBackground = new GUIStyle("flow background");

    public static readonly GUIStyle H1Label = new GUIStyle("label");
    public static readonly GUIStyle H2Label = new GUIStyle("label");
    public static readonly GUIStyle H3Label = new GUIStyle("label");
    public static readonly GUIStyle H4Label = new GUIStyle("label");
    public static readonly GUIStyle H5Label = new GUIStyle("label");
    public static readonly GUIStyle sceneViewLabel = new GUIStyle("label");
    public static readonly GUIStyle block = new GUIStyle("MeTransitionBlock");
    public static readonly GUIStyle leftBlock = new GUIStyle("MeTransitionBlock");
    public static readonly GUIStyle rightBlock = new GUIStyle("MeTransitionBlock");
    public static readonly GUIStyle label = new GUIStyle(EditorStyles.label); 

    public static readonly GUIContent PlusContent = EditorGUIUtility.IconContent("Toolbar Plus");
    public static readonly GUIContent PlusMoreContent = EditorGUIUtility.IconContent("Toolbar Plus More");
    public static readonly GUIContent MinusContent = EditorGUIUtility.IconContent("Toolbar Minus");
    public static readonly GUIContent SettingContent = EditorGUIUtility.IconContent("SettingsIcon");

    public static readonly GUIContent add = new GUIContent("+");
    public static readonly GUIContent delete = new GUIContent("×");
    public static readonly GUIContent up = new GUIContent("↑");
    public static readonly GUIContent down = new GUIContent("↓");

    public static readonly GUIStyle timeBlockRight = new GUIStyle("MeTimeLabel");
    public static readonly GUIStyle timeBlockLeft = new GUIStyle("MeTimeLabel");
    public static readonly GUIStyle toolbarLabel = new GUIStyle(EditorStyles.toolbarPopup);
    public static readonly GUIStyle backgroundEven = new GUIStyle("OL EntryBackEven");
    public static readonly GUIStyle backgroundOdd = new GUIStyle("OL EntryBackOdd");
    public static readonly GUIStyle backgroundSelected = new GUIStyle("OL SelectedRow");

    public static readonly GUIStyle toolbarCreateAddNewDropDown = new GUIStyle("ToolbarCreateAddNewDropDown");

    public static readonly GUIStyle windowBackground = new GUIStyle("OL box NoExpand");
    public static readonly GUIStyle box = new GUIStyle("OL Box");
    public static readonly GUIStyle inBigTitle = new GUIStyle("IN BigTitle");
    public static readonly GUIStyle button = new GUIStyle(EditorStyles.toolbarButton);
    public static readonly GUIStyle draggingHandle = new GUIStyle("RL DragHandle");
    public static readonly GUIStyle boxBackground = new GUIStyle("RL Background");
    public static readonly GUIStyle elementBackground = new GUIStyle("RL Element");

    public static readonly GUIContent playContent = EditorGUIUtility.IconContent("Animation.Play", "Play the animation clip.");
    public static readonly GUIContent prevKeyContent = EditorGUIUtility.IconContent("Animation.PrevKey", "Go to previous keyframe.");
    public static readonly GUIContent nextKeyContent = EditorGUIUtility.IconContent("Animation.NextKey", "Go to next keyframe.");
    public static readonly GUIContent firstKeyContent = EditorGUIUtility.IconContent("Animation.FirstKey", "Go to the beginning of the animation clip.");
    public static readonly GUIContent lastKeyContent = EditorGUIUtility.IconContent("Animation.LastKey", "Go to the end of the animation clip.");
    public static readonly GUIContent loopContent = EditorGUIUtility.IconContent("preAudioLoopOff");

    public static readonly GUIContent visibleOn = EditorGUIUtility.IconContent("animationvisibilitytoggleon", "Go to the end of the animation clip.");
    public static readonly GUIContent visibleOff = EditorGUIUtility.IconContent("animationvisibilitytoggleoff", "Go to the end of the animation clip.");

    public static readonly GUIStyle offLeft = new GUIStyle("MeTransOffLeft");
    public static readonly GUIStyle offRight = new GUIStyle("MeTransOffRight");
    public static readonly GUIStyle onLeft = new GUIStyle("MeTransOnLeft");
    public static readonly GUIStyle onRight = new GUIStyle("MeTransOnRight");
    public static readonly GUIStyle offOn = new GUIStyle("MeTransOff2On");
    public static readonly GUIStyle onOff = new GUIStyle("MeTransOn2Off");
    public static readonly GUIStyle background = new GUIStyle("MeTransitionBack");
    public static readonly GUIStyle header = new GUIStyle("MeTransitionHead");

    public static readonly GUIStyle inspectorTitlebar = new GUIStyle("IN Title");

    public const float kElementHeight = 20;
    public const float kGroupButtonHeight = 15;
    public const float kHierarchyWidth = 250;
    public const int kIntFieldWidth = 30;
    public const float kTickGap = 10;
    public const int kTickUnit = 5;
    public const float kRuleStart = 10;

    public const float kToolbarHeight = 18;
    public const float kPropertyWidth = 253f;//233f;

    public enum ELabel
    {
        H1,
        H2,
        H3,
        H4,
        H5,
    }

    private static bool bInitialized { get; set; }

    public static void Init()
    {
        if (!bInitialized)
        {
            label.alignment = TextAnchor.MiddleCenter;
            label.stretchHeight = true;
            H1Label.fontSize = 25;
            H1Label.alignment = TextAnchor.MiddleCenter;
            H1Label.richText = true;
            H2Label.fontSize = 20;
            H3Label.richText = true;
            H3Label.fontSize = 15;
            H4Label.richText = true;
            H4Label.fontSize = 11;
            H5Label.normal.textColor = Color.cyan;
            H5Label.fontSize = 8;
            H5Label.contentOffset = Vector2.up * 3f;
            subFlowBackground.padding = new RectOffset(3, 3, 3, 3);
            sceneViewLabel.richText = true;
            sceneViewLabel.fontSize = 10;
            EditorStyles.toolbarTextField.alignment = TextAnchor.MiddleCenter;

            var offset = new RectOffset(0, 0, 0, 0);
            backgroundEven.margin = offset;
            backgroundEven.padding = offset;
            backgroundEven.border = offset;
            backgroundSelected.border = offset;
            backgroundSelected.margin = offset;
            backgroundSelected.padding = offset;

            foldout.margin.top += 2;
            foldout.margin.bottom += 2;
            foldout.margin.left -= 10;
            foldout.margin.right += 2;
            foldout.fixedHeight = kElementHeight;

            windowBackground.margin = offset;
            windowBackground.padding = offset;
            windowBackground.stretchHeight = false;
            windowBackground.padding.top = 1;

            offLeft.fixedHeight = 0;
            offLeft.stretchHeight = true;
            //
            offRight.fixedHeight = 0;
            offRight.stretchHeight = true;
            //
            onLeft.fixedHeight = 0;
            onLeft.stretchHeight = true;
            //
            onRight.fixedHeight = 0;
            onRight.stretchHeight = true;

            bInitialized = true;
        }
    }

    public static void Label(Rect position, ELabel style, string content)
    {
        Label(position, style, new GUIContent(content));
    }

    public static void Label(Rect position, ELabel style, GUIContent content)
    {
        Vector2 size = GetStyle(style).CalcSize(content);
        EditorGUI.LabelField(new Rect(position.size / 2f - size / 2f, size), content, EditorStyle.H1Label);
    }

    private static GUIStyle GetStyle(ELabel style)
    {
        switch(style)
        {
            default: throw new Exception($"style = {0} not found!");
            case ELabel.H1: return H1Label;
            case ELabel.H2: return H2Label;
            case ELabel.H3: return H3Label;
            case ELabel.H4: return H4Label;
            case ELabel.H5: return H5Label;
        }
    }
}
