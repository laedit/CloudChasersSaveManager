using CloudChasersSaveManager.Binding;
using System.Collections.Generic;
using System.Linq;

namespace CloudChasersSaveManager.ViewModels
{
    internal class GameStateViewModel : ViewModel
    {
        private readonly SaveFile _saveFile;
        private readonly List<GameItem> _items;

        private float _waterFraction;

        public float Water
        {
            get => _waterFraction;
            set => SetField(ref _waterFraction, value);
        }

        private IList<string> _inventory;

        public IList<string> Inventory
        {
            get => _inventory;
            set => SetField(ref _inventory, value);
        }

        public HumanCharacterViewModel Amelia { get; }

        public HumanCharacterViewModel Francisco { get; }

        public CharacterViewModel Glider { get; }

        public GameStateViewModel(SaveFile saveFile, List<GameItem> items, float gliderMaxHealth, float maxWater)
        {
            _saveFile = saveFile;
            _items = items;

            Amelia = new HumanCharacterViewModel("Amelia")
            {
                Health = saveFile.HealthAmelia / 10,
                HasFracture = saveFile.FractureSickAmelia.Key,
                IsSick = saveFile.FractureSickAmelia.Value,
                Inventory = GetNames(saveFile.AmeliaInventory)
            };

            Francisco = new HumanCharacterViewModel("Francisco")
            {
                Health = saveFile.HealthFrancisco / 15,
                HasFracture = saveFile.FractureSickFrancisco.Key,
                IsSick = saveFile.FractureSickFrancisco.Value,
                Inventory = GetNames(saveFile.FranciscoInventory)
            };

            Glider = new CharacterViewModel("Glider")
            {
                Health = saveFile.HealthGlider / gliderMaxHealth,
                Inventory = GetNames(saveFile.GliderNetInventory).Concat(GetNames(saveFile.GliderWingInventory).Concat(GetNames(saveFile.GliderBodyInventory))).ToList()
            };

            Inventory = GetNames(saveFile.MainInventory);

            Water = saveFile.CurrentWater / maxWater;
        }

        private List<string> GetNames(List<KeyValuePair<int, int>> rawInventory)
        {
            return rawInventory.Select(kvp => _items.Find(item => item.ItemId == kvp.Value).ItemName).ToList();
        }

    }
}
