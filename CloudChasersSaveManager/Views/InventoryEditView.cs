using System;
using System.Collections.Generic;
using Terminal.Gui;

namespace CloudChasersSaveManager.Views
{
    internal class InventoryEditView : View
    {
        public void SetItems(IList<Tuple<int, string>> newItems)
        {
            RemoveAll();
            if (newItems != null)
            {
                int index = 0;
                var columnWidth = Frame.Width / Columns;
                for (int i = 0; index < newItems.Count; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        if (index < newItems.Count)
                        {
                            var currentItem = newItems[index];
                            var itemButton = new Button(j * columnWidth, i, currentItem.Item2);
                            itemButton.Id = index.ToString();
                            itemButton.Clicked = () =>
                                {
                                    var iss = new ItemSelector();
                                    Application.Run(iss);
                                    itemButton.Text = iss.SelectedItem.ItemName;
                                    newItems[int.Parse(itemButton.Id.ToString())] = new Tuple<int, string>(iss.SelectedItem.ItemId, iss.SelectedItem.ItemName);
                                };
                            Add(itemButton);
                            index++;
                        }
                    }
                }
            }
        }

        public int Columns { get; set; }

        public InventoryEditView()
        {
            Columns = 1;
        }

        public InventoryEditView(int columns)
        {
            Columns = columns;
        }
    }
}
