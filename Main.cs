using System;
using System.Windows.Forms;
using GTA;


namespace VeoAutoMod
{
    public class Main : Script
    {
        static int time = 0;

        public Main()
        {
            bool IsDebug = false;

            UIManager.Create();            
            World.Create(IsDebug);
            DealerManager.Create();

            Tick += onTick;
            KeyDown += onKeyDown;

            time = Game.GameTime;
        }

        private void onTick(object sender, EventArgs e)
        {
            int delta = Game.GameTime - time;
            Objects.Timer.UpdateTimers(delta);

            UIManager.Update();            
            World.Update();

            time = Game.GameTime;
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.U && e.Shift)
            {
                DealerManager.ToggleMenu();
            }
        }
    }
}

