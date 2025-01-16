using System.Collections.Generic;
using System.Threading.Tasks;
using QFramework.Extensions;
using UnityEngine;

    public interface IRecyclable
    {
        void OnGet();

        void OnRecycle();
    }

    public class BehaviorPool<T> where T : MonoBehaviour, new()
    {
        protected Queue<T> m_LeisurePool;
        protected HashSet<T> m_WorkingPool;
        private Transform m_TfRoot;
        private T m_Prefab;

        public BehaviorPool(Transform tf, T prefab)
        {
            m_TfRoot = tf;
            m_TfRoot.gameObject.SetActive(false);
            m_LeisurePool = new Queue<T>();
            m_WorkingPool = new HashSet<T>();
            m_Prefab = prefab;
        }

        //QNote: The idea is to check whether a resource is already loaded, and if yes, we can run a sync operation.  Question: How do we know as a caller whether a resrouce is loaded A: query to resource manager before requesting resources
        public T Get()
        {
            T leisure;
            if (m_LeisurePool.Count > 0)
                leisure = m_LeisurePool.Dequeue();
            else
                leisure = GameObject.Instantiate(m_Prefab,m_TfRoot); 

            if (leisure is IRecyclable)
                (leisure as IRecyclable).OnGet();

            m_WorkingPool.Add(leisure);
            return leisure;
        }

        public void Recycle(T behavior)
        {
            if (behavior is IRecyclable)
                (behavior as IRecyclable).OnRecycle();

            m_WorkingPool.Remove(behavior);
            m_LeisurePool.Enqueue(behavior);
            behavior.transform.SetParent(m_TfRoot, false);
        }

        public void RecycleAllWorkingPool()
        {
            foreach (var v in m_WorkingPool)
            {
                if (v is IRecyclable)
                    (v as IRecyclable).OnRecycle();
                m_LeisurePool.Enqueue(v);
                v.transform.SetParent(m_TfRoot, false); ;
            }
            m_WorkingPool.Clear();
        }

        public void Clear()
        {
            foreach (var v in m_WorkingPool)
            {
                v.SetActiveEx(false);
                v.transform.SetParent(null);
                Object.Destroy(v);
            }

            m_WorkingPool.Clear();
            m_WorkingPool = null;

            while (m_LeisurePool.Count > 0)
            {
                var obj = m_LeisurePool.Dequeue();
                obj.SetActiveEx(false);
                obj.transform.SetParent(null);
                Object.Destroy(obj);
            }
            m_LeisurePool.Clear();
            m_LeisurePool = null;
        }
    }
