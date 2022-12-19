using System;
using System.Collections.Generic;

namespace VeoAutoMod.Objects
{
    class Timer
    {
        private int delay, time = 0;
        private bool isStopped = false;
        private Action callback;

        private static List<Timer> timers = new List<Timer>();

        public static Timer Create(int delay, Action callback)
        {
            Timer timer = new Timer(delay, callback);
            timers.Add(timer);
            return timer;
        }

        public static void UpdateTimers(int interval)
        {
            List<Timer> timersToRemove = new List<Timer>();

            foreach(Timer timer in timers)
            {
                if (!timer.Update(interval))
                {
                    timersToRemove.Add(timer);
                }
            }

            foreach (Timer timer in timersToRemove)
            {
                timers.Remove(timer);
            }
        }

        public Timer(int delay, Action callback)
        {
            this.delay = delay;
            this.callback = callback;
            this.time = 0;
        }

        public bool Update(int interval)
        {
            if (isStopped) return false;
            time += interval;

            if (time >= delay)
            {
                callback();
                return false;
            }

            return true;
        }

        public int GetSecondsPassed()
        {
            return (int)Math.Round((double)(time / 1000));
        }

        public void Stop()
        {
            isStopped = true;
        }

    }
}

