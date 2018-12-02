using CloudChasersSaveManager.Binding;
using CloudChasersSaveManager.Models;
using CloudChasersSaveManager.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudChasersSaveManager.ViewModels
{
    internal class GameStateViewModel : ViewModel
    {
        private SaveFile _saveFile;

        private float _gliderMaxHealth;
        private float _maxWater;

        private float _waterFraction;

        public float Water
        {
            get => _waterFraction;
            set => SetField(ref _waterFraction, value);
        }

        private IList<Tuple<int, string>> _inventory;

        public IList<Tuple<int, string>> Inventory
        {
            get => _inventory;
            set => SetField(ref _inventory, value);
        }

        public HumanCharacterViewModel Amelia { get; private set; }

        public HumanCharacterViewModel Francisco { get; private set; }

        public CharacterViewModel Glider { get; private set; }

        public Func<string> PromptSelectSaveFile { get; set; }

        public Func<string> PromptSelectItemsFile { get; set; }

        public Func<bool> PromptDisclaimer { get; set; }

        public Action CloseApplication { get; set; }

        public Func<bool> PromptRestoreSave { get; set; }

        public Action InformBackupRestored { get; set; }

        public GameStateViewModel()
        {
            Amelia = new HumanCharacterViewModel("Amelia");
            Francisco = new HumanCharacterViewModel("Francisco");
            Glider = new CharacterViewModel("Glider");
        }

        private void InitializeData(SaveFile saveFile, IEnumerable<GameItem> items)
        {
            _saveFile = saveFile;

            // base 20 + items in inventory with maxHealth
            _gliderMaxHealth = 20
                + _saveFile.GliderBodyInventory.Sum(item => items.FirstOrDefault(gi => gi.ItemId == item.Value).MaxHealth)
                 + _saveFile.GliderNetInventory.Sum(item => items.FirstOrDefault(gi => gi.ItemId == item.Value).MaxHealth)
                  + _saveFile.GliderWingInventory.Sum(item => items.FirstOrDefault(gi => gi.ItemId == item.Value).MaxHealth);

            // base 100 + items in inventory with waterStorage
            _maxWater = 100 + _saveFile.MainInventory.Sum(item => items.FirstOrDefault(gi => gi.ItemId == item.Value).WaterStorage);

            Amelia.Health = _saveFile.HealthAmelia / 10;
            Amelia.HasFracture = _saveFile.FractureSickAmelia.Key;
            Amelia.IsSick = _saveFile.FractureSickAmelia.Value;
            Amelia.Inventory = _saveFile.AmeliaInventory.GetNames().ToList();

            Francisco.Health = _saveFile.HealthFrancisco / 15;
            Francisco.HasFracture = _saveFile.FractureSickFrancisco.Key;
            Francisco.IsSick = _saveFile.FractureSickFrancisco.Value;
            Francisco.Inventory = _saveFile.FranciscoInventory.GetNames().ToList();

            Glider.Health = _saveFile.HealthGlider / _gliderMaxHealth;
            Glider.Inventory = _saveFile.GliderNetInventory.Concat(_saveFile.GliderWingInventory)
                                .Concat(_saveFile.GliderBodyInventory).GetNames().ToList();

            var inventoryMax = 8;
            if (_saveFile.FranciscoInventory.Select(kvp => items.FirstOrDefault(item => item.ItemId == kvp.Value)).Any(gi => gi.InventoryRows > 0))
            {
                inventoryMax += 4;
            }
            if (_saveFile.AmeliaInventory.Select(kvp => items.FirstOrDefault(item => item.ItemId == kvp.Value)).Any(gi => gi.InventoryRows > 0))
            {
                inventoryMax += 4;
            }
            var inventory = _saveFile.MainInventory.GetNamesAndIds().ToList();
            for (int i = inventory.Count; i < inventoryMax; i++)
            {
                inventory.Add(new Tuple<int, string>(-1, "Empty"));
            }
            Inventory = inventory;

            Water = _saveFile.CurrentWater / _maxWater;
        }

        internal async Task Initialize()
        {
            SaveFile saveFile = null;
            var itemsLoaded = false;
            string saveFilePath = Settings.Default.SaveFilePath;
            string itemsFilePath = Settings.Default.ItemsFilePath;

            await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(saveFilePath))
                {
                    saveFilePath = FileHelper.GetSavePath();
                }
                saveFile = FileHelper.LoadSave(saveFilePath);
                if (string.IsNullOrEmpty(itemsFilePath))
                {
                    itemsFilePath = FileHelper.GetItemsPath();
                }
                itemsLoaded = GameItems.Load(itemsFilePath);
            });

            if (!Settings.Default.SkipDisclaimer)
            {
                Settings.Default.SkipDisclaimer = PromptDisclaimer();
            }

            while (saveFile == null)
            {
                saveFilePath = PromptSelectSaveFile();
                saveFile = FileHelper.LoadSave(saveFilePath);
            }
            Settings.Default.SaveFilePath = saveFilePath;

            while (!itemsLoaded)
            {
                itemsFilePath = PromptSelectItemsFile();
                itemsLoaded = GameItems.Load(itemsFilePath);
            }
            Settings.Default.ItemsFilePath = itemsFilePath;
            Settings.Default.Save();

            InitializeData(saveFile, GameItems.GetAll());
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
            if (PromptRestoreSave())
            {
                FileHelper.RestoreSaveFile();
                InformBackupRestored();
                CloseApplication();
            }
        }

        internal void SaveAndExit()
        {
            _saveFile.FractureSickAmelia = new KeyValuePair<bool, bool>(Amelia.HasFracture, Amelia.IsSick);
            _saveFile.FractureSickFrancisco = new KeyValuePair<bool, bool>(Francisco.HasFracture, Francisco.IsSick);

            _saveFile.MainInventory.Clear();
            for (int i = 0; i < Inventory.Count; i++)
            {
                var currentItem = Inventory[i];
                if(currentItem.Item1 != -1)
                {
                    _saveFile.MainInventory.Add(new KeyValuePair<int, int>(i, currentItem.Item1));
                }
            }
            
            FileHelper.ReplaceSaveFile(_saveFile);
            CloseApplication();
        }
    }
}
