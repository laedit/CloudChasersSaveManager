using CloudChasersSaveManager.ViewModels;
using System;
using Terminal.Gui;

namespace CloudChasersSaveManager
{
    class Program
    {
        static void Main()
        {
            Console.Title = "Cloud Chasers save manager";

            // TODO Disclaimer on launch(Cloud Chasers Save Manager is not endorsed, sponsored, affiliated with or otherwise authorized by Blindflug Studios.)
            // with checkbox to not see again?

            // TODO Check files found, if not OpenDialog

            var saveFile = FileHelper.GetSave();
            var items = FileHelper.GetItems();

            Application.Init();
            
            var mainWindow = new MainWindow(new GameStateViewModel(saveFile, items));
            Application.Top.Add(mainWindow);
            Application.Run();
        }
    }
}
