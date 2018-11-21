using CloudChasersSaveManager.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;
using Terminal.Gui;

namespace CloudChasersSaveManager
{
    class Program
    {
        static void Main()
        {
            Console.Title = "Cloud Chasers save manager";

            Application.Init();

            // TODO Disclaimer on launch(Cloud Chasers Save Manager is not endorsed, sponsored, affiliated with or otherwise authorized by Blindflug Studios.)
            // with checkbox to not see again?

            var gsvm = new GameStateViewModel();
            var mainWindow = new MainWindow(gsvm);
            gsvm.Initialize();
            Application.Top.Add(mainWindow);
            Application.Run();
        }
    }
}
