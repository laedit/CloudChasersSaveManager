using CloudChasersSaveManager.Binding;
using CloudChasersSaveManager.ViewModels;
using System.Collections.Generic;
using Terminal.Gui;

namespace CloudChasersSaveManager
{
    internal class CharacterView : FrameView
    {
        //private float _health;
        //private readonly ProgressBar _healthBar;
        protected readonly InventoryView _items;

        //public void SetItems(IList<string> newItems)
        //{
        //    _items.SetItems(newItems);
        //}

        //public float Health
        //{
        //    get => _health;
        //    set
        //    {
        //        _health = value;
        //        _healthBar.Fraction = _health;
        //        _healthBar.SetNeedsDisplay();
        //    }
        //}

        protected readonly Binder _binder;

        public CharacterView(CharacterViewModel bindSubject)
            : base(bindSubject.Name)
        {
            Width = 24;
            Height = 10;

            _binder = Binder.From(bindSubject);
            _binder.Bind<string>(nameof(bindSubject.Name), newName => this.Title = newName);

            this.Add(new Label("Health:")
            {
                X = 1,
                Y = 0,
                Width = 20,
                Height = 1
            });
            var healthBar = new ProgressBar
            {
                X = 1,
                Y = 1,
                Width = 20,
                Height = 1
            };
            this.Add(healthBar);
            _binder.Bind<float>(nameof(bindSubject.Health), newHealth => healthBar.Fraction = newHealth);

            _items = new InventoryView
            {
                X = 1,
                Y = 3,
                Width = 20
            };
            this.Add(_items);
            _binder.Bind<IList<string>>(nameof(bindSubject.Inventory), _items.SetItems);
        }
    }
}
