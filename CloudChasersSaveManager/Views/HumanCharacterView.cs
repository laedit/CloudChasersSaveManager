using CloudChasersSaveManager.ViewModels;
using Terminal.Gui;

namespace CloudChasersSaveManager.Views
{
    internal class HumanCharacterView : CharacterView
    {
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
            hasFracture.Toggled += (s, e) => bindSubject.HasFracture = hasFracture.Checked;
            this.Add(hasFracture);
            _binder.Bind<bool>(nameof(bindSubject.HasFracture),
                newValue => {
                    hasFracture.Checked = newValue;
                    hasFracture.SetNeedsDisplay();
                });

            var isSick = new CheckBox("Is sick")
            {
                X = 1,
                Y = 4,
                Width = 20,
                Height = 1
            };
            isSick.Toggled += (s, e) => bindSubject.IsSick = isSick.Checked;
            this.Add(isSick);
            _binder.Bind<bool>(nameof(bindSubject.IsSick), 
                newValue =>
                {
                    isSick.Checked = newValue;
                    isSick.SetNeedsDisplay();
                });

            _items.Y = 6;
        }
    }
}
