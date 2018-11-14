using System;

namespace CloudChasersSaveManager
{
    class Program
    {
        static void Main()
        {
            Console.Title = "Cloud Chasers save manager";

            var saveFile = FileHelper.GetSave();
            var items = FileHelper.GetItems();

            var gsc = new GameStateController(saveFile, items);
            gsc.Run();

            // TODO 
            // - Disclaimer on launch(Cloud Chasers Save Manager is not endorsed, sponsored, affiliated with or otherwise authorized by Blindflug Studios.) (with checkbox to not see again?)
            // - check files found, if not OpenDialog
        }
    }
}
