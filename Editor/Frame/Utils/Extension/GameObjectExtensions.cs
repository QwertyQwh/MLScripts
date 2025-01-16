using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Extensions
{
    public static class GameObjectExtensions
    {
        public static bool IsNull(this Component comp)
        {
            return comp == null;
        }

        public static void SetActiveEx(this GameObject go, bool active)
        {
            if (go.activeSelf.Equals(active)) { return; }
            go.SetActive(active);
        }

        public static void SetActiveEx(this Component comp, bool active)
        {
            comp.gameObject.SetActiveEx(active);
        }


        public static void SetActiveScale(this Component comp, bool active)
        {
            comp.gameObject.transform.localScale = active?Vector3.one : Vector3.zero;
        }

        public static void SetActiveScale(this GameObject comp, bool active, float scale = 1)
        {
            comp.transform.localScale = active ? new Vector3(scale,scale,scale) : Vector3.zero;
        }

        public static void SetParent(this GameObject go, GameObject parent)
        {
            var transform = go.transform;
            transform.parent = parent.transform;
        }

        public static void SetAsFirstSibling(this GameObject go)
        {
            go.transform.SetAsFirstSibling();
        }

        public static void SetAsLastSibling(this GameObject go)
        {
            go.transform.SetAsLastSibling();
        }

        public static string GetAllPath(this GameObject go)
        {
            var list = new List<string> { go.name };
            var parent = go.transform.parent;
            while (null != parent)
            {
                list.Add(parent.name);
                parent = parent.transform.parent;
            }

            list.Reverse();
            return string.Join("/", list.ToArray());
        }
    }
}