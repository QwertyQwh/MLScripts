using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Rotorz.Games.Collections;
using UnityEditor;
using UnityEngine;

public class MonsterEditor : EditorWindow
{
    private string kTitle = "MonsterEditor";
    private const float kMinHeight = 500f;
    private const float kMinWidth = 1200f;
    private MonsterDb mDatabase;
    private Vector2 mScrollPosition;
    private Vector2 mInspectorScrollPosition;
    private string mdbRoot => MonsterResourceManager.kRootPath;
    private DataBaseEditorConfig mEditorConfig;
    private MonsterListAdaptor mModulesAdaptor;
    private Monster mSelectedModule;
    public static float sListWidth = 250f;
    public bool Modified = false;


    [MenuItem("Database/Monster Editor", false)]
    private static void ShowLevelEditor()
    {
        var window = GetWindow<MonsterEditor>();
        window.titleContent = new GUIContent(window.kTitle);
        window.Show();
        
    }

    private void OnEnable()
    {
        minSize = new Vector2(kMinWidth, kMinHeight);

        mEditorConfig = DataBaseEditorConfig.Load();

        if (mEditorConfig.kWorkingPath != null)
            LoadFromFile(mEditorConfig.kWorkingPath);
    }

    private void OnGUI()
    {
        using (new GUIDisable(false))
        {
            HandleShorcuts();
            DrawTopToolBar();

            if (mDatabase == null) return;
            DrawMultList();
        }
    }


    private void OnDestroy()
    {
    }

    private void DrawTopToolBar()
    {
        using (new GUIHorizontal(EditorStyles.toolbar))
        {
            if (GUILayout.Button("Create", EditorStyles.toolbarButton))
                OnNewClick();
            if (GUILayout.Button("Load", EditorStyles.toolbarButton))
                OnLoadClick();
            if (GUILayout.Button("Save", EditorStyles.toolbarButton))
                OnSaveClick();
            if (GUILayout.Button("Save as", EditorStyles.toolbarButton))
                OnSaveAsClick();

            GUILayout.FlexibleSpace();
        }
    }

    private void InitListAdapters()
    {
        mModulesAdaptor = new MonsterListAdaptor(mDatabase.monsters, GetModuleName);
        mModulesAdaptor.CanDragToOtherList = false;
        mModulesAdaptor.AddModuleType<Monster>();
        mModulesAdaptor.OnCreateModule = OnCreateModule;
        mModulesAdaptor.OnModuleSelect = OnModuleSelect;
    }

    private void OnModuleSelect(Monster data)
    {
        GUI.FocusControl(null);
        mSelectedModule = data;
    }


    private string GetModuleName(Monster monster)
    {
        return monster.MonsterId;
    }


    private void DrawMultList()
    {
        using (new GUIHorizontal())
        {
            var columnWidth = GUILayout.Width(sListWidth);


            using (var scroll = new GUIScrollView(mScrollPosition))
            {
                mScrollPosition = scroll.ScrollPosition;

                using (new GUIVertical(columnWidth))
                {
                    ReorderableListGUI.Title("monsters");
                    ReorderableListGUI.ListField(mModulesAdaptor,
                        ReorderableListFlags.DisableContextMenu);
                }
            }

            using (var scroll = new GUIScrollView(mInspectorScrollPosition))
            {
                mInspectorScrollPosition = scroll.ScrollPosition;
            }


            columnWidth = GUILayout.Width(position.width - 250 - 30);
            using (new GUIVertical(columnWidth))
            {
                if (mSelectedModule != null)
                    MonsterDataDrawer.DrawModule(mSelectedModule);
                else
                    MonsterDataDrawer.ClearCanvas();
            }
        }
    }


    private void OnNewClick()
    {
        Debug.Log(mdbRoot);
        var path = EditorUtility.SaveFilePanel("Create", mdbRoot, "", "json");
        if (string.IsNullOrEmpty(path))
            return;
        mDatabase = new MonsterDb();
        SaveToFile(path);
        LoadFromFile(path);
    }

    private void OnSaveAsClick()
    {
        var path = EditorUtility.SaveFilePanel("Save", mdbRoot, "", "json");
        if (string.IsNullOrEmpty(path))
            return;

        SaveToFile(path, true);
        LoadFromFile(path);
    }

    private void OnSaveClick()
    {

        if (HasDuplicateId())
        {
            EditorUtility.DisplayDialog("Error", "There are duplicate monster ids.", "I'll fix it", "Cancel");
            return;
        }
        if (EditorUtility.DisplayDialog("Are you sure?", "Confirm Save?", "Ok", "Cancel")) SaveToFile();
    }

    private void OnLoadClick()
    {
        var path = EditorUtility.OpenFilePanel("Create", mdbRoot, "json");
        if (string.IsNullOrEmpty(path))
            return;

        LoadFromFile(path);
    }

    private void LoadFromFile(string path)
    {
        if (path == null)
            return;
        mEditorConfig.kWorkingPath = path;
        var fileName = Path.GetFileName(path);
        kTitle = fileName;
        titleContent = new GUIContent(kTitle);

        var bytes = File.ReadAllBytes(path);
        var configData = Encoding.UTF8.GetString(bytes);
        mDatabase = JsonConvert.DeserializeObject<MonsterDb>(configData);


        //InitModuleList();
        InitListAdapters();
    }

    private void SaveToFile()
    {
        SaveToFile(mEditorConfig.kWorkingPath);
    }

    private void SaveToFile(string path, bool saveOther = false)
    {
        if (path == null)
            return;

        if (!saveOther)
            if (!File.Exists(path))
                mDatabase = new MonsterDb();

        
        File.WriteAllText(path, JsonConvert.SerializeObject(mDatabase, Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        }));
    }


    private bool HasDuplicateId()
    {
        var unique = new HashSet<string>();
        foreach (var monster in mDatabase.monsters)
        {
            unique.Add(monster.MonsterId);
        }

        return unique.Count != mDatabase.monsters.Count;
    }
    private void SaveAll()
    {
        SaveToFile();
        mEditorConfig.Save();
    }

    private void OnCreateModule(Type t)
    {
        var module = Activator.CreateInstance(t);
        if (module is Monster monster)
        {
            while(mDatabase.monsters.Any(item=>item.MonsterId ==  monster.MonsterId))
            {
                monster.MonsterId += "_1";
            }
            //gamojiData.GamojiId = "New Gamoji";
            //gamojiData.Description = "Write your description here";
            //gamojiData.IconAddress = "Name of the addressable icon";
            //gamojiData.ConnectedGamoji = "";
            mDatabase.monsters.Add(monster);
        }
    }

    private void HandleShorcuts()
    {
        var evt = Event.current;
        if (evt.control && evt.keyCode == KeyCode.S)
        {
            evt.Use();
            SaveAll();
        }
    }
}