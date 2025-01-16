using System;
using UnityEngine;
using Object = System.Object;


    public abstract class SingletonBehavior<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static bool DoesExist { get { return Instance != null; } }
        public static T Instance { get; private set; }

        protected virtual void Start()
        {
            if (DoesExist)
                throw new Exception($"singleton = [{typeof(T)}] repeated allocation!");
            Instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            if (Instance != null)
            {
                Destroy(Instance);
            }
            Instance = null;
        }

    }
