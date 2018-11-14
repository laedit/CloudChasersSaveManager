using CloudChasersSaveManager.Binding;
using CloudChasersSaveManager.ViewModels;
using System;
using System.Collections.Generic;
using Terminal.Gui;

namespace CloudChasersSaveManager
{

    internal class MainWindow : Window
    {
        private readonly Button _saveAndExitButton;
        public Action SaveAndExitButtonClick { set => _saveAndExitButton.Clicked = value; }

        private readonly Button _restorePreviousSaveButton;
        public Action RestorePreviousSaveButtonClick { set => _restorePreviousSaveButton.Clicked = value; }

        private readonly Button _healAllButton;
        public Action HealAllButtonClick { set => _healAllButton.Clicked = value; }

        private readonly Button _fillLifesButton;
        public Action FillLifesButtonClick { set => _fillLifesButton.Clicked = value; }

        private readonly Button _fillWaterButton;
        public Action FillWaterButtonClick { set => _fillWaterButton.Clicked = value; }

        public MainWindow(GameStateViewModel bindSubject)
            : base("Cloud Chasers save manager")
        {
            var binder = Binder.From(bindSubject);

            var franciscoStateView = new HumanCharacterView(bindSubject.Francisco) { X = 1, Y = 1 };
            this.Add(franciscoStateView);

            var gliderStateView = new CharacterView(bindSubject.Glider) { X = 33, Y = 1 };
            this.Add(gliderStateView);

            var ameliaStateView = new HumanCharacterView(bindSubject.Amelia) { X = 66, Y = 1 };
            this.Add(ameliaStateView);

            var waterFrame = new FrameView("Water") { X = 1, Y = 12, Width = 89, Height = 5 };
            this.Add(waterFrame);
            var waterProgressBar = new ProgressBar { Y = 1, Width = 86 };
            waterFrame.Add(waterProgressBar);
            binder.Bind<float>(nameof(bindSubject.Water), newValue => waterProgressBar.Fraction = newValue);

            var inventoryFrame = new FrameView("Inventory") { X = 1, Y = 19, Width = 89, Height = 6 };
            this.Add(inventoryFrame);
            var inventoryView = new InventoryView(4);
            inventoryFrame.Add(inventoryView);
            binder.Bind<IList<string>>(nameof(bindSubject.Inventory), inventoryView.SetItems);

            _healAllButton = new Button("Heal all")
            {
                X = 100,
                Y = 2
            };
            this.Add(_healAllButton);

            _fillLifesButton = new Button("Fill lifes")
            {
                X = 100,
                Y = 4
            };
            this.Add(_fillLifesButton);

            _fillWaterButton = new Button("Fill water")
            {
                X = 100,
                Y = 13
            };
            this.Add(_fillWaterButton);

            _restorePreviousSaveButton = new Button("Restore save")
            {
                X = 100,
                Y = 23
            };
            this.Add(_restorePreviousSaveButton);

            _saveAndExitButton = new Button("Save & Exit")
            {
                X = 100,
                Y = 25
            };
            this.Add(_saveAndExitButton);
        }
    }
}
