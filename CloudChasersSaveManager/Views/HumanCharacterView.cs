﻿using CloudChasersSaveManager.ViewModels;
using Terminal.Gui;

namespace CloudChasersSaveManager
{
    internal class HumanCharacterView : CharacterView
    {
        //private readonly CheckBox _hasFracture;
        //private readonly CheckBox _isSick;

        //public bool HasFracture
        //{
        //    get => _hasFracture.Checked;
        //    set {
        //        _hasFracture.Checked = value;
        //        _hasFracture.SetNeedsDisplay();
        //    }
        //}

        //public bool IsSick
        //{
        //    get => _isSick.Checked;
        //    set
        //    {
        //        _isSick.Checked = value;
        //        _isSick.SetNeedsDisplay();
        //    }
        //}

        public HumanCharacterView(HumanCharacterViewModel bindSubject)
        : base(bindSubject)
        {
            var hasFracture = new CheckBox("Has fracture")
            {
                X = 1,
                Y = 3,
                Width = 20,
                Height = 1
            };
            this.Add(hasFracture);
            _binder.Bind<bool>(nameof(bindSubject.HasFracture), newValue => hasFracture.Checked = newValue);

            var isSick = new CheckBox("Is sick")
            {
                X = 1,
                Y = 4,
                Width = 20,
                Height = 1
            };
            this.Add(isSick);
            _binder.Bind<bool>(nameof(bindSubject.IsSick), newValue => isSick.Checked = newValue);

            _items.Y = 6;
        }
    }
}
