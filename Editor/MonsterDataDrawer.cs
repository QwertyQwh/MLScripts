using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Rotorz.Games.Collections;
using Unity.Android.Gradle.Manifest;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityObject = UnityEngine.Object;


public class MonsterDataDrawer
{
    private static GUILayoutOption kPropertyWidth = GUILayout.Width(167);
    private static List<GameObject> sObjectPool;
    private static List<GameObject> sPosMarkers;
    private static Sprite loadedSprite;
    private static string loadedSpriteAddr = "";

    public static void Dispose()
    {
        sObjectPool?.ForEach(v => UnityObject.DestroyImmediate(v));
        sObjectPool?.Clear();
        sPosMarkers?.ForEach(v => UnityObject.DestroyImmediate(v));
        sPosMarkers?.Clear();
        sObjectPool = null;
        sPosMarkers = null;
    }

    public static void DrawModule(Monster data)
    {
        if (data.IconAddress != loadedSpriteAddr)
        {
            //Try if asset exists only when the iconAddr changes
            var res = Addressables.LoadResourceLocationsAsync(data.IconAddress).Result;
            if (res != null && res.Count != 0)
            {
                Addressables.LoadAssetAsync<Sprite>(data.IconAddress).Completed += obj =>
                {
                    loadedSprite = obj.Result;
                    loadedSpriteAddr = data.IconAddress;
                };
            }
            else
            {
                loadedSprite = null;
                loadedSpriteAddr = "";
            }
        }


        DrawTitle(data);
        DrawModuleName(data);
        DrawProperties(data);
    }


    public static void ClearCanvas()
    {
        using (new GUIHorizontal())
        {
            EditorGUILayout.HelpBox("Select a Monster to start.", MessageType.Info);
        }
    }

    private static void DrawModuleName(Monster data)
    {

        using (new GUIHorizontal(EditorStyles.helpBox))
        {
            data.MonsterId = EditorGUILayout.TextField("Monster Id", data.MonsterId);
            if (loadedSprite)
                GUILayout.Box(loadedSprite.texture, GUILayout.Height(100));
            else
                GUILayout.Box("The address entered does not correspond to a sprite.", GUILayout.Height(100));
        }
    }

    private static void DrawTitle(Monster data)
    {
        ReorderableListGUI.Title($"{data.MonsterId}");
    }

    private static void DrawProperties(Monster data)
    {
        using (new GUIHorizontal(EditorStyles.helpBox))
        {
            data.StatHP.Base = EditorGUILayout.IntField("Base StatHP", data.StatHP.Base);
            data.StatHP.LevelUp = EditorGUILayout.IntField("StatHP LevelUp", data.StatHP.LevelUp);


        }
        using (new GUIHorizontal(EditorStyles.helpBox))
        {
            data.StatAttack.Base = EditorGUILayout.IntField("Base StatAttack", data.StatAttack.Base);
            data.StatAttack.LevelUp = EditorGUILayout.IntField("StatAttack LevelUp", data.StatAttack.LevelUp);
  

        }

        using (new GUIHorizontal(EditorStyles.helpBox))
        {
            data.StatDefense.Base = EditorGUILayout.IntField("Base StatDefense", data.StatDefense.Base);
            data.StatDefense.LevelUp = EditorGUILayout.IntField("StatDefense LevelUp", data.StatDefense.LevelUp);
        }

        using (new GUIHorizontal(EditorStyles.helpBox))
        {
            data.StatMagic.Base = EditorGUILayout.IntField("Base StatMagic", data.StatMagic.Base);
            data.StatMagic.LevelUp = EditorGUILayout.IntField("StatDefense StatMagic", data.StatMagic.LevelUp);
        }

        using (new GUIVertical(EditorStyles.helpBox))
        {
            data.Tier = (MonsterTier)EditorGUILayout.EnumPopup("Monster Tier", data.Tier);
            data.Element = (MonsterElement)EditorGUILayout.EnumPopup("Monster Element Type", data.Element);
            //using (new GUIDisable())
            //{
            //    data.UnityAddress = EditorGUILayout.TextField("Unity Address", data.GamojiId);
            //}

            //data.FilterId = EditorGUILayout.TextField("FilterId", data.FilterId);
            //data.Description = EditorGUILayout.TextField("Description", data.Description);

            //data.ConnectedGamoji = EditorGUILayout.TextField("Connected Gamoji", data.ConnectedGamoji);
            //data.InteractionType =
            //    (Enums.InteractionType)EditorGUILayout.EnumPopup("Interaction Type", data.InteractionType);
            //data.Category = (Enums.GamojiCategory)EditorGUILayout.EnumPopup("Catergory", data.Category);
            //data.Position = EditorGUILayout.Vector3Field("Position", data.Position);
            //EditorGUI.ObjectField(new Rect(500, 500, 100, 100), "", obj.Result, typeof(Sprite), false);
        }


        SceneView.RepaintAll();
    }


    protected static float3 Float3Field(string label, float3 vector)
    {
        float3 result;
        using (new GUIHorizontal())
        {
            GUILayout.Label(label);
            result = EditorGUILayout.Vector3Field("", vector, GUILayout.Width(150f));
        }

        return result;
    }

    private static string GetDescription(FieldInfo field)
    {
        var descriptions = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (descriptions.Length == 0)
            return null;
        return (descriptions[0] as DescriptionAttribute).Description;
    }

    private static string GetDescription(Type t)
    {
        var descriptions = t.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (descriptions.Length == 0)
            return null;
        return (descriptions[0] as DescriptionAttribute).Description;
    }

    private static bool HasAttribute<T>(FieldInfo field) where T : Attribute
    {
        var attributes = field.GetCustomAttributes(typeof(T), false);
        return attributes.Length > 0;
    }

    private static bool HasAttribute<T>(Type t) where T : Attribute
    {
        return t.GetCustomAttributes(typeof(T), false).Length > 0;
    }
    //Range attribute drawer

    //private static IntRangeAttribute GetIntRange(FieldInfo field)
    //{
    //    var attributes = field.GetCustomAttributes(typeof(IntRangeAttribute), false);
    //    if (attributes.Length == 0)
    //        return null;
    //    return attributes[0] as IntRangeAttribute;
    //}

    //private static FloatRangeAttribute GetFloatRange(FieldInfo field)
    //{
    //    var attributes = field.GetCustomAttributes(typeof(FloatRangeAttribute), false);
    //    if (attributes.Length == 0)
    //        return null;
    //    return attributes[0] as FloatRangeAttribute;
    //}
}