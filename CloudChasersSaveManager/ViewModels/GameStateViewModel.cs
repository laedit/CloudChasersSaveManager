using CloudChasersSaveManager.Binding;
using System.Collections.Generic;
using System.Linq;
using Terminal.Gui;

namespace CloudChasersSaveManager.ViewModels
{
    internal class GameStateViewModel : ViewModel
    {
        private readonly SaveFile _saveFile;
        private readonly List<GameItem> _items;

        private readonly float _gliderMaxHealth;
        private readonly float _maxWater;

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

        public GameStateViewModel(SaveFile saveFile, List<GameItem> items)
        {
            _saveFile = saveFile;
            _items = items;

            // base 20 + items in inventory with maxHealth
            _gliderMaxHealth = 20
                + saveFile.GliderBodyInventory.Sum(item => _items.Find(gi => gi.ItemId == item.Value).MaxHealth)
                 + saveFile.GliderNetInventory.Sum(item => _items.Find(gi => gi.ItemId == item.Value).MaxHealth)
                  + saveFile.GliderWingInventory.Sum(item => _items.Find(gi => gi.ItemId == item.Value).MaxHealth);

            // base 100 + items in inventory with waterStorage
            _maxWater = 100 + saveFile.MainInventory.Sum(item => _items.Find(gi => gi.ItemId == item.Value).WaterStorage);

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
                Health = saveFile.HealthGlider / _gliderMaxHealth,
                Inventory = GetNames(saveFile.GliderNetInventory).Concat(GetNames(saveFile.GliderWingInventory).Concat(GetNames(saveFile.GliderBodyInventory))).ToList()
            };

            Inventory = GetNames(saveFile.MainInventory);

            Water = saveFile.CurrentWater / _maxWater;
        }

        internal void HealAll()
        {
            // Amelia health
            _saveFile.FractureSickAmelia = new KeyValuePair<bool, bool>(false, false);
            Amelia.HasFracture = false;
            Amelia.IsSick = false;

            // Francisco health
            _saveFile.FractureSickFrancisco = new KeyValuePair<bool, bool>(false, false);
            Francisco.HasFracture = false;
            Francisco.IsSick = false;

            FillLifes();

            FillWater();
        }

        internal void FillLifes()
        {
            // Amelia health
            _saveFile.HealthAmelia = 10;
            Amelia.Health = 1;

            // Francisco health
            _saveFile.HealthFrancisco = 15;
            Francisco.Health = 1;

            // Glider health
            _saveFile.HealthGlider = _gliderMaxHealth;
            Glider.Health = 1;
        }

        internal void FillWater()
        {
            _saveFile.CurrentWater = _maxWater;
            Water = 1;
        }

        internal void RestorePreviousSave()
        {
            if (1 == MessageBox.Query(107, 7, "Are you sure to restore the backuped save?", "This will override current modifications and will replace your genuine save if you haven't modified it.", "No", "Yes"))
            {
                FileHelper.RestoreSaveFile();
                MessageBox.Query(107, 7, "Backup restored", "The restore is complete and the application will exit.", "Ok");
                Application.RequestStop();
            }
        }

        internal void SaveAndExit()
        {
            _saveFile.FractureSickAmelia = new KeyValuePair<bool, bool>(Amelia.HasFracture, Amelia.IsSick);
            _saveFile.FractureSickFrancisco = new KeyValuePair<bool, bool>(Francisco.HasFracture, Francisco.IsSick);
            FileHelper.ReplaceSaveFile(_saveFile);
            Application.RequestStop();
        }

        private List<string> GetNames(List<KeyValuePair<int, int>> rawInventory)
        {
            return rawInventory.Select(kvp => _items.Find(item => item.ItemId == kvp.Value).ItemName).ToList();
        }
    }
}
