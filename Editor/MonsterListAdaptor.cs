using System;
using System.Collections.Generic;
using System.ComponentModel;
using Rotorz.Games.Collections;
using Rotorz.Games.UnityEditorExtensions;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

public class MonsterListAdaptor : GenericListAdaptor<Monster>, IReorderableListDropTarget
{
    public bool CanDragToOtherList = true;
    public Action<Type> OnCreateModule;
    public Action<Monster> OnModuleDelete;
    public Action<Monster> OnModuleSelect;

    private static MonsterListAdaptor sSelectedList;
    private static Monster sSelectedItem;
    private static Vector2 sMouseDownPosition;
    private static Monster sRenamingItem;
    private static readonly string sGamojiTypeName = typeof(Monster).FullName;

    private const float kMouseDragThresholdInPixels = 0.6f;
    private readonly List<Type> mTypes = new();
    private readonly Func<Monster, string> GetNameFunc;


    private class ModuleDraggedItem
    {
        public static readonly string TypeName = typeof(Monster).FullName;

        public readonly int Index;
        public readonly Monster monster;
        public readonly MonsterListAdaptor ListAdaptor;

        public ModuleDraggedItem(MonsterListAdaptor list, int index, Monster monster)
        {
            ListAdaptor = list;
            Index = index;
            this.monster = monster;
        }
    }

    public MonsterListAdaptor(IList<Monster> list, Func<Monster, string> getNameFunc)
        : base(list, null, 16f)
    {
        GetNameFunc = getNameFunc;
    }

    bool IReorderableListDropTarget.CanDropInsert(int insertionIndex)
    {
        if (!ReorderableListControl.CurrentListPosition.Contains(Event.current.mousePosition))
            return false;

        var obj = UnityEditor.DragAndDrop.GetGenericData(ModuleDraggedItem.TypeName);
        if (!(obj is ModuleDraggedItem))
            return false;

        return true;
    }

    void IReorderableListDropTarget.ProcessDropInsertion(int insertionIndex)
    {
        if (Event.current.type == EventType.DragPerform)
        {
            var draggedItem = UnityEditor.DragAndDrop.GetGenericData(sGamojiTypeName) as ModuleDraggedItem;

            if (draggedItem.ListAdaptor == this)
            {
                Move(draggedItem.Index, insertionIndex);
            }
            else
            {
                List.Insert(insertionIndex, draggedItem.monster);
                sSelectedList = this;
            }
        }
    }

    public override void DrawItemBackground(Rect position, int index)
    {
        if (this == sSelectedList && List[index] == sSelectedItem)
        {
            var restoreColor = GUI.color;
            GUI.color = ExtraEditorStyles.Skin.SelectedHighlightColor;
            GUI.DrawTexture(position, EditorGUIUtility.whiteTexture);
            GUI.color = restoreColor;
        }
    }

    public override void DrawItem(Rect position, int index)
    {
        var moduleId = List[index];

        var controlID = GUIUtility.GetControlID(FocusType.Passive);

        switch (Event.current.GetTypeForControl(controlID))
        {
            case EventType.KeyUp:
                if (Event.current.keyCode == KeyCode.F2)
                    if (sSelectedItem != null)
                    {
                        sRenamingItem = sSelectedItem;

                        Event.current.Use();
                    }

                break;
            case EventType.MouseDown:
                var totalItemPosition = ReorderableListGUI.CurrentItemTotalPosition;
                if (totalItemPosition.Contains(Event.current.mousePosition))
                {
                    sSelectedList = this;
                    sSelectedItem = moduleId;

                    OnModuleSelect?.Invoke(moduleId);
                }

                
                var draggableRect = totalItemPosition;
                draggableRect.x = position.x;
                draggableRect.width = position.width;

                if (Event.current.button == 0 && draggableRect.Contains(Event.current.mousePosition))
                {
                    sSelectedList = this;
                    sSelectedItem = moduleId;

                    GUIUtility.hotControl = controlID;
                    sMouseDownPosition = Event.current.mousePosition;
                    Event.current.Use();
                }

                break;

            case EventType.MouseDrag:
                if (!CanDragToOtherList)
                    return;

                if (GUIUtility.hotControl == controlID)
                {
                    GUIUtility.hotControl = 0;

                    if (Vector2.Distance(sMouseDownPosition, Event.current.mousePosition) >=
                        kMouseDragThresholdInPixels)
                    {
                        var item = new ModuleDraggedItem(this, index, moduleId);

                        UnityEditor.DragAndDrop.PrepareStartDrag();
                        UnityEditor.DragAndDrop.objectReferences = new UnityObject[0];
                        UnityEditor.DragAndDrop.paths = new string[0];
                        UnityEditor.DragAndDrop.SetGenericData(ModuleDraggedItem.TypeName, item);
                        UnityEditor.DragAndDrop.StartDrag("test");
                    }

                    Event.current.Use();
                }

                break;

            case EventType.Repaint:
                if (sRenamingItem == moduleId)
                {
                    //sRenamingItem.Name = GUI.TextArea(position, s_RenamingItem.Name);
                }
                else
                {
                    EditorStyles.label.Draw(position, GetNameFunc.Invoke(moduleId), false, false, false, false);
                }

                break;
        }
    }

    public void AddModuleType<T>()
    {
        var t = typeof(T);
        if (!mTypes.Contains(t))
            mTypes.Add(t);
    }

    public override void Add()
    {
        var menu = new GenericMenu();

        foreach (var t in mTypes)
            //if (mDatabase != null) 
            //{
            //    //var modeData = LevelEditor.LevelModeDict;
            //    //foreach(var data in modeData)
            //    //{
            //    //        menu.AddItem(new GUIContent(GetDescription(t)), false, () => { OnCreateModule(t); });
            //    //}
            //}
            //else
            menu.AddItem(new GUIContent(GetDescription(t)), false, () => { OnCreateModule(t); });
        menu.ShowAsContext();
    }

    public override void Remove(int index)
    {
        if (EditorUtility.DisplayDialog("Are you sure?", "Confirm Deletion?", "Ok", "Cancel"))
        {
            OnModuleDelete?.Invoke(List[index]);
            base.Remove(index);
        }
    }

    private static string GetDescription(Type t)
    {
        var descriptions = t.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (descriptions.Length == 0)
            return null;
        return (descriptions[0] as DescriptionAttribute).Description;
    }
}