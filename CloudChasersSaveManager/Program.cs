using CloudChasersSaveManager.ViewModels;
using System;
using Terminal.Gui;

namespace CloudChasersSaveManager
{
    class Program
    {
        static void Main()
        {
            Console.Title = "Cloud Chasers Save Manager";

            Application.Init();

            var gsvm = new GameStateViewModel();
            var mainWindow = new MainWindow(gsvm);
            Application.Top.Add(mainWindow);
            gsvm.Initialize();
            Application.Run();
        }
    }
}
