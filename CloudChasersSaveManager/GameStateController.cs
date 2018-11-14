using CloudChasersSaveManager.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Terminal.Gui;

namespace CloudChasersSaveManager
{
    internal class GameStateController
    {
        private readonly SaveFile _saveFile;
        private readonly List<GameItem> _items;

        private readonly MainWindow _mainWindow;
        private readonly GameStateViewModel _gameState;

        private readonly float _gliderMaxHealth;
        private readonly float _maxWater;

        public GameStateController(SaveFile saveFile, List<GameItem> items)
        {
            Application.Init();

            _saveFile = saveFile;
            _items = items;

            // TODO move maxes to GameState once click Events are Commands
            // it should enforce these kind of rules
            // base 20 + items in inventory with maxHealth
            _gliderMaxHealth = 20
                + saveFile.GliderBodyInventory.Sum(item => _items.Find(gi => gi.ItemId == item.Value).MaxHealth)
                 + saveFile.GliderNetInventory.Sum(item => _items.Find(gi => gi.ItemId == item.Value).MaxHealth)
                  + saveFile.GliderWingInventory.Sum(item => _items.Find(gi => gi.ItemId == item.Value).MaxHealth);

            // base 100 + items in inventory with waterStorage
            _maxWater = 100 + saveFile.MainInventory.Sum(item => _items.Find(gi => gi.ItemId == item.Value).WaterStorage);

            _gameState = new GameStateViewModel(saveFile, items, _gliderMaxHealth, _maxWater);

            _mainWindow = new MainWindow(_gameState);

            _mainWindow.HealAllButtonClick = HealAll;

            _mainWindow.FillLifesButtonClick = FillLifes;
            _mainWindow.FillWaterButtonClick = FillWater;

            _mainWindow.RestorePreviousSaveButtonClick = RestorePreviousSave;

            _mainWindow.SaveAndExitButtonClick = SaveAndExit;
        }

        private void HealAll()
        {
            // Amelia health
            _saveFile.FractureSickAmelia = new KeyValuePair<bool, bool>(false, false);
            _gameState.Amelia.HasFracture = false;
            _gameState.Amelia.IsSick = false;

            // Francisco health
            _saveFile.FractureSickFrancisco = new KeyValuePair<bool, bool>(false, false);
            _gameState.Francisco.HasFracture = false;
            _gameState.Francisco.IsSick = false;

            FillLifes();

            FillWater();
        }

        private void FillLifes()
        {
            // Amelia health
            _saveFile.HealthAmelia = 10;
            _gameState.Amelia.Health = 1;

            // Francisco health
            _saveFile.HealthFrancisco = 15;
            _gameState.Francisco.Health = 1;

            // Glider health
            _saveFile.HealthGlider = _gliderMaxHealth;
            _gameState.Glider.Health = 1;
        }

        private void FillWater()
        {
            _saveFile.CurrentWater = _maxWater;
            _gameState.Water = 1;
        }

        private void RestorePreviousSave()
        {
            if (1 == MessageBox.Query(107, 7, "Are you sure to restore the backuped save?", "This will override current modifications and will replace your genuine save if you haven't modified it.", "No", "Yes"))
            {
                // TODO - exit or reload data after?
                FileHelper.RestoreSaveFile();
            }
        }

        private void SaveAndExit()
        {
            _saveFile.FractureSickAmelia = new KeyValuePair<bool, bool>(_gameState.Amelia.HasFracture, _gameState.Amelia.IsSick);
            _saveFile.FractureSickFrancisco = new KeyValuePair<bool, bool>(_gameState.Francisco.HasFracture, _gameState.Francisco.IsSick);
            FileHelper.ReplaceSaveFile(_saveFile);
            Application.RequestStop();
        }

        internal void Run()
        {
            Application.Top.Add(_mainWindow);
            Application.Run();
        }
    }
}
