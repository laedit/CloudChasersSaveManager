using CloudChasersSaveManager.Binding;
using CloudChasersSaveManager.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using Terminal.Gui;

namespace CloudChasersSaveManager
{
    internal class MainWindow : Window
    {
        private const int ButtonsAlignment = 99;

        public MainWindow(GameStateViewModel bindSubject)
            : base("Cloud Chasers Save Manager")
        {
            var binder = Binder.From(bindSubject);

            var franciscoStateView = new HumanCharacterView(bindSubject.Francisco) { X = 1, Y = 1 };
            this.Add(franciscoStateView);

            var gliderStateView = new CharacterView(bindSubject.Glider) { X = 33, Y = 1 };
            this.Add(gliderStateView);

            var ameliaStateView = new HumanCharacterView(bindSubject.Amelia) { X = 66, Y = 1 };
            this.Add(ameliaStateView);

            var waterFrame = new FrameView("Water") { X = 1, Y = 12, Width = 89, Height = 5 };
            this.Add(waterFrame);
            var waterProgressBar = new ProgressBar { X = 1, Y = 1, Width = 85 };
            waterFrame.Add(waterProgressBar);
            binder.Bind<float>(nameof(bindSubject.Water), newValue => waterProgressBar.Fraction = newValue);

            var inventoryFrame = new FrameView("Inventory") { X = 1, Y = 19, Width = 89, Height = 6 };
            this.Add(inventoryFrame);
            var inventoryView = new InventoryView(4);
            inventoryFrame.Add(inventoryView);
            binder.Bind<IList<string>>(nameof(bindSubject.Inventory), inventoryView.SetItems);

            var healAllButton = new Button("Heal all")
            {
                X = ButtonsAlignment,
                Y = 2
            };
            this.Add(healAllButton);
            healAllButton.Clicked = bindSubject.HealAll;

            var fillLifesButton = new Button("Fill lifes")
            {
                X = ButtonsAlignment,
                Y = 4
            };
            this.Add(fillLifesButton);
            fillLifesButton.Clicked = bindSubject.FillLifes;

            var fillWaterButton = new Button("Fill water")
            {
                X = ButtonsAlignment,
                Y = 13
            };
            this.Add(fillWaterButton);
            fillWaterButton.Clicked = bindSubject.FillWater;

            if (FileHelper.BackupExists())
            {
                var restorePreviousSaveButton = new Button("Restore backup")
                {
                    X = ButtonsAlignment,
                    Y = 23
                };
                this.Add(restorePreviousSaveButton);
                restorePreviousSaveButton.Clicked = bindSubject.RestorePreviousSave;
            }

            var saveAndExitButton = new Button("Save & Exit")
            {
                X = ButtonsAlignment,
                Y = 25
            };
            this.Add(saveAndExitButton);
            saveAndExitButton.Clicked = bindSubject.SaveAndExit;

            bindSubject.PromptSelectSaveFile = PromptSelectSaveFile;
            bindSubject.PromptSelectItemsFile = PromptSelectItemsFile;
            bindSubject.PromptDisclaimer = PromptDisclaimer;
            bindSubject.CloseApplication = CloseApplicetion;
            bindSubject.PromptRestoreSave = PromptRestoreSave;
            bindSubject.InformBackupRestored = InformBackupRestored;
        }

        private bool PromptDisclaimer()
        {
            var neverShowAgain = false;
            var dialog = new Dialog("Disclaimer", 60, 12)
            {
                new Views.LabelFix(new Rect(0, 1, 54, 3), "Cloud Chasers Save Manager is not endorsed,\nsponsored, affiliated with or otherwise\nauthorized by Blindflug Studios")
                {
                    TextAlignment = TextAlignment.Centered
                }
            };

            var neverShowAgainCheckbox = new CheckBox("Please do not show me that again")
            {
                X = Pos.Center(),
                Y = 5
            };
            dialog.Add(neverShowAgainCheckbox);

            var ok = new Button("Ok")
            {
                Clicked = () =>
                {
                    neverShowAgain = neverShowAgainCheckbox.Checked;
                    CloseApplicetion();
                }
            };
            dialog.AddButton(ok);

            Application.Run(dialog);
            return neverShowAgain;
        }

        private string PromptSelectSaveFile()
        {
            var workDir = Environment.CurrentDirectory;
            Environment.CurrentDirectory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).Parent.FullName;
            var od = new OpenDialog("The save file cannot be located, please provide it:", "Save File");
            do
            {
                Application.Run(od);
            } while (od.FilePaths.Count == 0 || !File.Exists(od.FilePaths[0]));
            Environment.CurrentDirectory = workDir;
            return od.FilePaths[0];
        }

        private string PromptSelectItemsFile()
        {
            var od = new OpenDialog("The items file cannot be located, please provide it:", "Save File");
            do
            {
                Application.Run(od);
            } while (od.FilePaths.Count == 0 || !File.Exists(od.FilePaths[0]));
            return od.FilePaths[0];
        }

        private void CloseApplicetion()
        {
            Application.RequestStop();
        }

        private bool PromptRestoreSave()
        {
            return 1 == MessageBox.Query(107, 7, "Are you sure to restore the backuped save?", "This will override current modifications and will replace your genuine save if you haven't modified it.", "No", "Yes");
        }

        private void InformBackupRestored()
        {
            MessageBox.Query(107, 7, "Backup restored", "The restore is complete and the application will exit.", "Ok");
        }
    }
}
