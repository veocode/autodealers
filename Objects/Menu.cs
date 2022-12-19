using System.Collections.Generic;
using LemonUI.Menus;


namespace VeoAutoMod.Objects
{
    abstract class Menu
    {
        protected NativeMenu nativeMenu;
        protected List<NativeItem> items = new List<NativeItem>();

        public Menu(string title = "", string subtitle = "")
        {
            nativeMenu = UIManager.CreateMenu(title, subtitle);
            nativeMenu.Visible = false;

            CreateItems();

            if (items.Count > 0)
            {
                foreach (NativeItem item in items) nativeMenu.Add(item);
            }
        }

        public abstract void CreateItems();

        public void Destroy()
        {
            UIManager.RemoveMenu(nativeMenu);
            nativeMenu = null;
            items.Clear();
            items = null;
        }

        protected void BeforeShow() { }

        public Menu Show()
        {
            if (nativeMenu != null)
            {
                BeforeShow();
                nativeMenu.Visible = true;
            }
            return this;
        }        

        public bool Visible { get => nativeMenu.Visible; }

        public Menu Hide()
        {
            if (nativeMenu != null)
            {
                nativeMenu.Visible = false;
            }
            return this;
        }

        public Menu Toggle()
        {
            if (nativeMenu.Visible)
            {
                return Hide();
            } else
            {
                return Show();
            }
        }

        public Menu SetTitle(string title, string subtitle)
        {
            if (nativeMenu != null)
            {
                nativeMenu.Title.Text = title;
                nativeMenu.Subtitle = subtitle;
            }
            return this;
        }
    }
}
