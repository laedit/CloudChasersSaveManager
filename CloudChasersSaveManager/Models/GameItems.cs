using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudChasersSaveManager.Models
{
    internal static class GameItems
    {
        private static IEnumerable<GameItem> Items;

        internal static bool Load(string itemsFilePath)
        {
            var items = FileHelper.LoadItems(itemsFilePath);
            if (items == null)
            {
                return false;
            }

            Items = items;
            return true;
        }

        internal static IEnumerable<GameItem> GetAll()
        {
            return Items;
        }

        internal static IEnumerable<string> GetNames(this IEnumerable<KeyValuePair<int, int>> rawInventory)
        {
            return rawInventory.Select(kvp => Items.FirstOrDefault(item => item.ItemId == kvp.Value).ItemName);
        }

        internal static IEnumerable<Tuple<int, string>> GetNamesAndIds(this IEnumerable<KeyValuePair<int, int>> rawInventory)
        {
            return rawInventory.Select(kvp =>
            {
                var foundItem = Items.FirstOrDefault(item => item.ItemId == kvp.Value);
                return new Tuple<int, string>(foundItem.ItemId, foundItem.ItemName);
            });
        }
    }
}
