using System.Collections.Generic;

namespace CloudChasersSaveManager.Models
{
    internal class GameItem
    {
        public int ItemId { get; set; }

        public string ItemName { get; set; }

        public string ItemDesc { get; set; }

        public string ItemIcon { get; set; }

        public long WaterWorth { get; set; }

        public bool Consumeable { get; set; }

        public bool InventoryItem { get; set; }

        public bool ConsumeForHumans { get; set; }

        public KeyValuePair<int, int> ConsumeHealth { get; set; }

        public KeyValuePair<int, int> ConsumeWater { get; set; }

        public KeyValuePair<bool, int> Immunity { get; set; }

        public KeyValuePair<bool, int> GetRemoveSickness { get; set; }

        public KeyValuePair<bool, int> GetRemoveFracture { get; set; }

        public long ItemType { get; set; }

        public long WaterWastage { get; set; }

        public long SpeedImpact { get; set; }

        public long MaxHealth { get; set; }

        public long WaterStorage { get; set; }

        public long InventoryRows { get; set; }

        public long GliderSpeed { get; set; }

        public long GliderNet { get; set; }

        public long ItemValue { get; set; }

        public long MaxStack { get; set; }

        public long IndexItemInList { get; set; }
    }
}
