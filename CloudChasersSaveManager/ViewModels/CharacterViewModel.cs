using CloudChasersSaveManager.Binding;
using System.Collections.Generic;

namespace CloudChasersSaveManager.ViewModels
{
    internal class CharacterViewModel : ViewModel
    {

        private IList<string> _inventory;

        public IList<string> Inventory
        {
            get => _inventory;
            set => SetField(ref _inventory, value);
        }

        private float _health;

        public float Health
        {
            get => _health;
            set => SetField(ref _health, value);
        }

        public string Name { get; }

        public CharacterViewModel(string name)
        {
            Name = name;
        }
    }
}
