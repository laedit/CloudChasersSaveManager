using CloudChasersSaveManager.Models;
using System;
using System.Linq;
using Terminal.Gui;

namespace CloudChasersSaveManager.Views
{
    internal sealed class ItemSelector : Window
    {
        public GameItem SelectedItem { get; private set; }

        public ItemSelector()
            : base("Item selection")
        {
            var itemsList = GameItems.GetAll().ToList();
            itemsList.Sort(new Comparison<GameItem>((gi1, gi2) => gi1.ItemName.CompareTo(gi2.ItemName)));
            var itemsNames = itemsList.Select(item => item.ItemName).ToList();
            var itemsDescriptions = itemsList.Select(item => item.ItemDesc.Replace(". ", ".\n")).ToList();

            var listView = new ListView(new Rect(0, 0, 30, 25), itemsNames);
            Add(listView);

            var labelDescription = new LabelFix(new Rect(32, 0, 30, 25), itemsDescriptions[0])
            {
                TextAlignment = TextAlignment.Centered,
            };
            Add(labelDescription);

            listView.SelectedChanged += () =>
            {
                labelDescription.Text = itemsDescriptions[listView.SelectedItem];
                this.SetNeedsDisplay();
            };

            Add(new Button("ok")
            {
                Clicked = () =>
                {
                    SelectedItem = itemsList[listView.SelectedItem];
                    Application.RequestStop();
                },
                X = Pos.Center(),
                Y = Pos.Bottom(this) - 3
            });
        }
    }
}
