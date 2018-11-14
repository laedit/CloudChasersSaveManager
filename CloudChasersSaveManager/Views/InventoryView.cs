using System.Collections.Generic;
using Terminal.Gui;

namespace CloudChasersSaveManager
{
    internal class InventoryView : View
    {
        private IList<string> _items;
        public void SetItems(IList<string> newItems)
        {
            _items = newItems;
            SetNeedsDisplay();
        }

        public int Columns { get; set; }

        public InventoryView()
        {
            Columns = 1;
        }

        public InventoryView(int columns)
        {
            Columns = columns;
        }

        public override void Redraw(Rect region)
        {
            if (_items != null && _items.Count > 0)
            {
                int index = 0;
                var columnWidth = Frame.Width / Columns;
                for (int i = 0; index < _items.Count; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        if (index < _items.Count)
                        {
                            Move(j * columnWidth, i);
                            Driver.AddStr(_items[index]);
                            index++;
                        }
                    }
                }
            }
            else
            {
                Move(0, 0);
                Driver.AddStr("Empty");
            }
        }
    }
}
