using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonExplorerV5.Save
{
    // === SAVE DATA ROOT ===
    // Complete snapshot of all persistent game state.
    public class SaveData
    {
        // Meta
        public int Slot { get; set; }
        public DateTime SaveDate { get; set; }

        // === PLAYER ===
        public string PlayerName { get; set; }
        public int PlayerHealth { get; set; }
        public int PlayerMaxHealth { get; set; }
        public int PlayerLevel { get; set; }
        public int PlayerXP { get; set; }
        public int Coins { get; set; }

        // === INVENTORY ===
        public List<SavedItem> Inventory { get; set; } = new();
        public string EquippedWeaponId { get; set; }
        public string EquippedArmorId { get; set; }

        // === DUNGEON META ===
        public int DungeonLevel { get; set; }
        public int PlayerX { get; set; }
        public int PlayerY { get; set; }

        // === MAP SNAPSHOT ===
        public SavedRoom[][] Rooms { get; set; }
    }

    // === SAVE ITEM ENTRY ===
    // Represents one inventory slot.
    public class SavedItem
    {
        public string ItemId { get; set; }
        public int Quantity { get; set; }
        public bool IsEquipped { get; set; }
    }

    // === ROOM SNAPSHOT ===
    // Stores full room information for save/load.
    public class SavedRoom
    {
        public string Description { get; set; }

        // Enemy + items
        public string EnemyId { get; set; }
        public List<string> ItemIds { get; set; } = new();

        // State flags
        public bool IsCleared { get; set; }
        public bool Visited { get; set; }
        public bool Revealed { get; set; }
        public bool IsVisible { get; set; }

        // Special markers
        public bool MerchantHere { get; set; }
        public bool HasStairsDown { get; set; }

        // Walls
        public bool WallNorth { get; set; }
        public bool WallSouth { get; set; }
        public bool WallWest { get; set; }
        public bool WallEast { get; set; }

        // Loot
        public int CoinLoot { get; set; }
    }
}
