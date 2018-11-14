namespace CloudChasersSaveManager.ViewModels
{
    internal class HumanCharacterViewModel : CharacterViewModel
    {
        private bool _isSick;

        public bool IsSick
        {
            get => _isSick;
            set => SetField(ref _isSick, value);
        }

        private bool _hasFracture;

        public bool HasFracture
        {
            get => _hasFracture;
            set => SetField(ref _hasFracture, value);
        }

        public HumanCharacterViewModel(string name)
            : base(name)
        {

        }
    }
}
