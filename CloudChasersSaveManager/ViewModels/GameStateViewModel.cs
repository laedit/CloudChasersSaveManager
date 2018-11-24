using CloudChasersSaveManager.Binding;
using CloudChasersSaveManager.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terminal.Gui;

namespace CloudChasersSaveManager.ViewModels
{
    internal class GameStateViewModel : ViewModel
    {
        private SaveFile _saveFile;
        private List<GameItem> _items;

        private float _gliderMaxHealth;
        private float _maxWater;

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

        public HumanCharacterViewModel Amelia { get; private set; }

        public HumanCharacterViewModel Francisco { get; private set; }

        public CharacterViewModel Glider { get; private set; }

        public Func<string> PromptSelectSaveFile { get; set; }

        public Func<string> PromptSelectItemsFile { get; set; }

        public Func<bool> PromptDisclaimer { get; set; }

        public GameStateViewModel()
        {
            Amelia = new HumanCharacterViewModel("Amelia");
            Francisco = new HumanCharacterViewModel("Francisco");
            Glider = new CharacterViewModel("Glider");
        }

        private void InitiazeData(SaveFile saveFile, List<GameItem> items)
        {
            _saveFile = saveFile;
            _items = items;

            // base 20 + items in inventory with maxHealth
            _gliderMaxHealth = 20
                + _saveFile.GliderBodyInventory.Sum(item => _items.Find(gi => gi.ItemId == item.Value).MaxHealth)
                 + _saveFile.GliderNetInventory.Sum(item => _items.Find(gi => gi.ItemId == item.Value).MaxHealth)
                  + _saveFile.GliderWingInventory.Sum(item => _items.Find(gi => gi.ItemId == item.Value).MaxHealth);

            // base 100 + items in inventory with waterStorage
            _maxWater = 100 + _saveFile.MainInventory.Sum(item => _items.Find(gi => gi.ItemId == item.Value).WaterStorage);

            Amelia.Health = _saveFile.HealthAmelia / 10;
            Amelia.HasFracture = _saveFile.FractureSickAmelia.Key;
            Amelia.IsSick = _saveFile.FractureSickAmelia.Value;
            Amelia.Inventory = GetNames(_saveFile.AmeliaInventory);

            Francisco.Health = _saveFile.HealthFrancisco / 15;
            Francisco.HasFracture = _saveFile.FractureSickFrancisco.Key;
            Francisco.IsSick = _saveFile.FractureSickFrancisco.Value;
            Francisco.Inventory = GetNames(_saveFile.FranciscoInventory);

            Glider.Health = _saveFile.HealthGlider / _gliderMaxHealth;
            Glider.Inventory = GetNames(_saveFile.GliderNetInventory).Concat(GetNames(_saveFile.GliderWingInventory).Concat(GetNames(_saveFile.GliderBodyInventory))).ToList();

            Inventory = GetNames(_saveFile.MainInventory);

            Water = _saveFile.CurrentWater / _maxWater;
        }

        internal async Task Initialize()
        {
            SaveFile saveFile = null;
            List<GameItem> items = null;
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
                items = FileHelper.LoadItems(itemsFilePath);
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

            while (items == null)
            {
                itemsFilePath = PromptSelectItemsFile();
                items = FileHelper.LoadItems(itemsFilePath);
            }
            Settings.Default.ItemsFilePath = itemsFilePath;
            Settings.Default.Save();

            InitiazeData(saveFile, items);
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
