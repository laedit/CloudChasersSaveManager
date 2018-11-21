using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace CloudChasersSaveManager
{
    internal static class FileHelper
    {
        internal static void RestoreSaveFile()
        {
            var savePath = GetSavePath();
            var backupPath = GetBackupPath();
            if (File.Exists(backupPath))
            {
                File.Copy(backupPath, savePath, true);
                File.Delete(backupPath);
            }
        }

        internal static void ReplaceSaveFile(SaveFile saveFile)
        {
            var savePath = GetSavePath();
            File.Copy(savePath, GetBackupPath(), true);
            using (Stream stream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                IFormatter formatter = new BinaryFormatter();

                formatter.Serialize(stream, saveFile);
            }
        }

        internal static SaveFile GetSave()
        {
            return LoadSave(GetSavePath());
        }

        internal static SaveFile LoadSave(string savePath)
        {
            if (File.Exists(savePath))
            {
                using (Stream stream = new FileStream(savePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    IFormatter formatter = new BinaryFormatter();

                    try
                    {
                        return (SaveFile)formatter.Deserialize(stream);
                    }
                    catch { }
                }
            }
            return null;
        }

        internal static bool BackupExists()
        {
            return File.Exists(GetBackupPath());
        }

        private static string GetBackupPath()
        {
            return GetSavePath() + ".back";
        }

        private static string GetSavePath()
        {
            return Path.Combine(Environment.ExpandEnvironmentVariables("%USERPROFILE%"), @"AppData\LocalLow\Blindflug_Studios\Cloud Chasers - Journey of Hope\saveGame.gd");
        }

        internal static string GetItemsPath()
        {
            var gameFolderPath = GetGameFolderPath();
            if (!string.IsNullOrEmpty(gameFolderPath))
            {
                return Path.Combine(gameFolderPath, "CloudChasers_Win_Data", "StreamingAssets", "JSON", "items.json");
            }

            return null;
        }

        internal static List<GameItem> GetItems()
        {
            return LoadItems(GetItemsPath());
        }

        internal static List<GameItem> LoadItems(string itemsFilePath)
        {
            if (!string.IsNullOrEmpty(itemsFilePath) && File.Exists(itemsFilePath))
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<GameItem>>(File.ReadAllText(itemsFilePath));
                }
                catch { }
            }

            return null;
        }

        internal static string GetGameFolderPath()
        {
            // #1 Get game folder path from registry
            var folderPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 751670", "InstallLocation", null);
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
            {
                // check the 64 bits registry
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (var key = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 751670"))
                {
                    folderPath = (string)key.GetValue("InstallLocation");
                }

                if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
                {
                    // #2 Read steam librariess file and check libraries for the game folder
                    var steamPath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", null);
                    if (!string.IsNullOrEmpty(steamPath) && Directory.Exists(steamPath))
                    {
                        var libraryfoldersFilePath = Path.Combine(steamPath, "steamapps", "libraryfolders.vdf");
                        if (File.Exists(libraryfoldersFilePath))
                        {
                            foreach (var line in File.ReadAllLines(libraryfoldersFilePath))
                            {
                                var keyValue = line.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries).Select(entry => entry.Replace("\"", "")).ToArray();
                                if (keyValue.Length == 2)
                                {
                                    if (int.TryParse(keyValue[0], out int _) && Directory.Exists(keyValue[1]))
                                    {
                                        var gamePath = Path.Combine(keyValue[1], "SteamApps", "common", "CloudChasers");
                                        if (Directory.Exists(gamePath))
                                        {
                                            return gamePath;
                                        }
                                    }
                                }

                            }
                        }

                    }
                    return null;
                }
            }

            return folderPath;
        }
    }
}
