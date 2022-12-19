using System.Collections.Generic;
using LemonUI;
using LemonUI.Menus;
using LemonUI.TimerBars;


namespace VeoAutoMod
{
    static class UIManager
    {
        private static TimerBarCollection timers = new TimerBarCollection();
        private static ObjectPool pool = new ObjectPool();

        public static void Create()
        {
            pool.Add(timers);
        }

        public static void Update()
        {
            pool.Process();
        }

        public static NativeMenu CreateMenu(string title, string subtitle)
        {
            NativeMenu menu = new NativeMenu(title, subtitle);
            pool.Add(menu);
            return menu;
        }

        public static void RemoveMenu(NativeMenu menu)
        {
           pool.Remove(menu);
        }

        public static TimerBar CreateTimerBar(string title, string info)
        {
            TimerBar bar = new TimerBar(title, info);
            timers.Add(bar);
            return bar;
        }

        public static void RemoveTimerBar(TimerBar bar)
        {           
            timers.Remove(bar);
        }
    }
}
