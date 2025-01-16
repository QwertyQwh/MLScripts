using System;

    public abstract class Singleton<T>
    {
        public static T Instance { get; private set; }
        public static bool Exists => Instance != null;
        protected abstract void Init();
        protected abstract void Dispose();

        public static T CreateInstance()
        {
            if (Instance == null)
            {
                Instance = Activator.CreateInstance<T>();
                (Instance as Singleton<T>)?.Init();
                return Instance;
            }
            throw new Exception($"{typeof(T)} instance already exists.");
        }

        public static void DestroyInstance()
        {
            (Instance as Singleton<T>)?.Dispose();
            Instance = default;
        }
    }
