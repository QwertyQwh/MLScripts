using System;
using System.Collections.Generic;

namespace QFramework.Utils.Patterns
{
    public interface IRecyclable
    {
        bool IsRecycled { get; set; }
        void Recycled();
    }


    public static class ObjectPool
    {
        private interface IPool
        {
            void InternalCleanup();
            void Free(object val);
        }

        private class Pool<T> : IPool where T : new()
        {
            internal static readonly Pool<T> Instance = new Pool<T>();
            private Stack<T> m_Stack = new Stack<T>(8);
            private int m_Count;

            private Pool()
            {
                s_Pools.Add(typeof(T), this);
            }

            public void InternalCleanup()
            {
                try
                {
                    if (m_Count != m_Stack.Count)
                    {
                        throw new Exception($"{typeof(T)} may be memory leak. {m_Stack.Count}/{m_Count}");
                    }
                }
                finally
                {
                    m_Count -= m_Stack.Count;
                    m_Stack.Clear();
                    m_Stack.TrimExcess();
                }
            }

            public void Free(object val)
            {
                InternalFree((T) val);
            }

            internal T InternalNew()
            {
                if (m_Stack.Count > 0)
                {
                    var ret = m_Stack.Pop();
                    if (ret is IRecyclable obj)
                    {
                        obj.IsRecycled = false;
                    }

                    return ret;
                }

                ++m_Count;

                return new T();
            }

            private void InternalFree(T val)
            {
                if (val is IRecyclable obj)
                {
                    obj.IsRecycled = true;
                    obj.Recycled();
                }

                m_Stack.Push(val);
            }
        }

        private static readonly Dictionary<Type, IPool> s_Pools = new Dictionary<Type, IPool>();

        public static T New<T>() where T : new()
        {
            return Pool<T>.Instance.InternalNew();
        }

        public static void Free<T>(T target) where T : new()
        {
            Pool<T>.Instance.Free(target);
        }

        public static void FreeByType(object target)
        {
            var type = target.GetType();
            if (s_Pools.TryGetValue(type, out var pool))
            {
                pool.Free(target);
            }
        }

        public static void Cleanup<T>() where T : new()
        {
            Pool<T>.Instance.InternalCleanup();
        }

        public static void Cleanup()
        {
            foreach (var pool in s_Pools)
            {
                pool.Value.InternalCleanup();
            }
        }
    }
}