using NStack;
using Terminal.Gui;

namespace CloudChasersSaveManager
{
    internal sealed class StateButton : Button
    {
        public bool Disabled { get; set; }

        public StateButton(ustring text, bool is_default = false) : base(text, is_default)
        {
        }

        public StateButton(int x, int y, ustring text) : base(x, y, text)
        {
        }

        public StateButton(int x, int y, ustring text, bool is_default) : base(x, y, text, is_default)
        {
        }

        public override void Redraw(Rect region)
        {
            var csNormal = ColorScheme.Normal; // FIXME use Driver.SetAttribute? But it is overriden by base.
            var csHotNormal = ColorScheme.HotNormal;
            if (Disabled)
            {
                ColorScheme.Normal = Colors.Menu.Focus;
                ColorScheme.HotNormal = Colors.Menu.Focus;
            }

            base.Redraw(region);
            ColorScheme.Normal= csNormal;
            ColorScheme.HotNormal = csHotNormal;
        }

        public override bool CanFocus
        {
            get => Disabled ? false : base.CanFocus;
            set => base.CanFocus = value;
        }

        public override bool MouseEvent(MouseEvent me)
        {
            if (Disabled) return false;

            return base.MouseEvent(me);
        }

        public override bool ProcessColdKey(KeyEvent kb)
        {
            if (Disabled) return false;

            return base.ProcessColdKey(kb);
        }

        public override bool ProcessHotKey(KeyEvent kb)
        {
            if (Disabled) return false;

            return base.ProcessHotKey(kb);
        }

        public override bool ProcessKey(KeyEvent kb)
        {
            if (Disabled) return false;

            return base.ProcessKey(kb);
        }
    }
}
