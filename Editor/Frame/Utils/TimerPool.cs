using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;
using QFramework.Utils.Patterns;

namespace QFramework.Utils
{
    public static class TimerPool
    {
        private static Queue<Timer> m_TimerQueue = new Queue<Timer>();
        public static ConcurrentQueue<Timer> callBackTimers = new ConcurrentQueue<Timer>();

        public static void AddPool(Timer timer)
        {
            m_TimerQueue.Enqueue(timer);
        }

        public static void Update()
        {
            while (callBackTimers.TryDequeue(out var timer))
            {
                timer.ExcuteOnMainThread();
            }
        }

        /// <summary>
        /// Start Timer
        /// </summary>
        /// <param name="duration">总时长 毫秒</param>
        /// <param name="interval">间隔 毫秒</param>
        /// <param name="callback">回调函数</param>
        /// <param name="ignoreTimeScale">忽略时间缩放</param>
        /// <returns></returns>
        public static Timer Start(float duration, float interval, Action<int> callback, bool ignoreTimeScale = false)
        {
            Timer timer;
            if (m_TimerQueue.Count > 0)
                timer = m_TimerQueue.Dequeue();
            else
                timer = new Timer();

            timer.Start(duration, interval == 0 ? duration : interval, callback, ignoreTimeScale);
            return timer;
        }
    }

    public class Timer : IRecyclable
    {
        private System.Timers.Timer timer;
        private float m_Duration;
        private float m_Interval;
        private Action<int> m_Callback;
        private static float timeScale = 1;
        private int m_TickCount;

        public bool Isvaild { get; private set; }
        public bool IsRecycled { get; set; }

        public Timer()
        {
            timer = new System.Timers.Timer();
            timer.Elapsed += OnElapsed;
            Isvaild = false;
            timer.Enabled = false;
        }

        public static void SetTimeScale(float scale)
        {
            timeScale = scale;
        }

        public void Start(float duration, float interval, Action<int> callback, bool ignoreTimeScale)
        {
            float tickTimeScale = ignoreTimeScale ? 1f : timeScale;
            m_TickCount = 0;
            m_Interval = interval / tickTimeScale;
            m_Duration = duration / tickTimeScale;
            m_Callback = callback;
            SetInterval();
            timer.AutoReset = m_Interval < m_Duration;
            timer.Enabled = true;
            Isvaild = true;
            IsRecycled = false;
        }

        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            m_TickCount++;
            timer.Enabled = (m_Duration - m_TickCount * m_Interval) > 0;

            if (timer.Enabled)
            {
                SetInterval();
            }

            Isvaild = timer.Enabled;
            TimerPool.callBackTimers.Enqueue(this);
        }

        public void ExcuteOnMainThread()
        {
            m_Callback?.Invoke(m_TickCount);
            if (!Isvaild)
            {
                m_Callback = null;
            }
        }

        private void SetInterval()
        {
            timer.Interval = Math.Min(m_Interval, m_Duration - m_TickCount * m_Interval);
        }

        /// <summary>
        /// Should nullify reference after close
        /// </summary>
        public void Close()
        {
            if (IsRecycled)
            {
                throw new Exception($"Timer Error！Already Recycled!");
            }

            timer.Enabled = false;
            Recycled();
        }

        public void Recycled()
        {
            m_Callback = null;
            IsRecycled = true;
            Isvaild = false;
            TimerPool.AddPool(this);
        }
    }
}