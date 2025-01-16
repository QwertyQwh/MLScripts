using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Extensions
{
    public static class TransformExtensions
    {

        public static void SetVisible(this Transform tf, bool visible)
        {
            tf.localScale = visible ? Vector3.one : Vector3.zero;
        }
        public static bool IsVisible(this Transform tf)
        {
            return tf.localScale != Vector3.zero;
        }

        public static T GetOrAddComponent<T>(this Component child) where T : Component
        {
            T result = child.GetComponent<T>();
            if (result == null)
            {
                result = child.gameObject.AddComponent<T>();
            }

            return result;
        }

        public static void AttachParent(this Transform child,
            Transform parent,
            Vector3 localPosition = default(Vector3),
            bool setLayer = false)
        {
            AttachParent(child, parent, localPosition, Quaternion.identity, Vector3.one, setLayer);
        }

        public static void AttachParent(
            this Transform child,
            Transform parent,
            Vector3 localPosition,
            Quaternion localRotation,
            Vector3 localScale,
            bool setLayer = false)
        {
            child.transform.SetParent(parent);
            RectTransform rectTransform = child as RectTransform;
            if (null != rectTransform)
            {
                //   rectTransform.anchoredPosition = localPosition;
                localPosition.z = 0f;
                rectTransform.localPosition = localPosition;
                rectTransform.localScale = localScale;
            }
            else
            {
                child.localPosition = localPosition;
                child.localScale = localScale;
            }

            child.localRotation = localRotation;
            if (setLayer)
            {
                child.SetLayerRecursively(parent.gameObject.layer);
            }
        }

        public static void DetachParent(this Transform child, bool resetPosition, bool resetScale = true)
        {
            child.parent = null;

            if (resetPosition)
                child.ResetLocalPosition();
            if (resetScale)
                child.ResetLocalScale();
        }

        public static void SetLayerRecursively(this Transform trans, int layer)
        {
            trans.gameObject.layer = layer;
            for (int i = 0; i < trans.childCount; i++)
            {
                trans.GetChild(i).SetLayerRecursively(layer);
            }
        }

        public static void ResetLocalPosition(this Transform child)
        {
            child.localPosition = Vector3.zero;
            child.localRotation = Quaternion.identity;
        }

        public static void ResetLocalScale(this Transform child)
        {
            child.localScale = Vector3.one;
        }

        public static void Reset(this Transform child)
        {
            ResetLocalPosition(child);
            ResetLocalScale(child);
        } 

        public static T GetComponentInChildren<T>(
            this Transform trans,
            string name,
            bool ignoreCase = true,
            bool includeInactive = true) where T : Component
        {
            var children = trans.GetComponentsInChildren<T>(includeInactive);
            foreach (T t in children)
            {
                if (0 == string.Compare(t.name, name, ignoreCase))
                {
                    return t;
                }
            }

            return null;
        }

        public static void SetActive(this Transform tf, bool active)
        {
            tf.gameObject.SetActiveEx(active);
        }

        public static Transform GetTransformInChildren(this Transform trans, string name)
        {
            var allChild = trans.GetComponentsInChildren<Transform>();
            for (var i = 0; i < allChild.Length; i++)
            {
                if (name == allChild[i].name)
                {
                    return allChild[i].transform;
                }
            }

            return null;
        }

        public static string GetPath(this Transform trans)
        {
            string path = trans.name;
            var tf = trans;
            while (null != tf.parent)
            {
                tf = tf.parent;
                path = path.Insert(0, $"{tf.name}/");
            }

            return path;
        }

        public static void SetX(this Transform trans, float x)
        {
            trans.position = new Vector3(x, trans.position.y, trans.position.z);
        }

        public static void SetY(this Transform trans, float y)
        {
            trans.position = new Vector3(trans.position.x, y, trans.position.z);
        }

        public static void SetZ(this Transform trans, float z)
        {
            trans.position = new Vector3(trans.position.x, trans.position.y, z);
        }

        public static List<T> GetAllComponents<T>(this Transform parent) where T : Component
        {
            List<T> list = new List<T>();
            GetAllComponents<T>(parent, list);
            return list;
        }

        public static void GetAllComponents<T>(this Transform parent, List<T> list) where T : Component
        {
            int childCount = parent.childCount;
            T[] components = parent.GetComponents<T>();
            int len = components.Length;
            for (int i = 0; i < len; i++)
            {
                T component = components[i];
                if (list.Contains(component))
                {
                    continue;
                }
                list.Add(component);
            }
            if (childCount == 0)
            {
                return;
            }
            for (int i = 0; i < childCount; i++)
            {
                Transform child = parent.GetChild(i);
                GetAllComponents<T>(child, list);
            }
        }
    }
}