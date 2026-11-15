using System.Collections.Generic;
using DungeonExplorerV5.Items;

namespace DungeonExplorerV5.Items.Equipment
{
    public static class ItemPools
    {
        // === COMMON ITEMS ===
        public static readonly List<Weapon> CommonWeapons = new()
        {
            new Weapon("iron_sword",   "Iron Sword",   3, 25, ItemRarity.Common),
            new Weapon("rusty_dagger", "Rusty Dagger", 2, 18, ItemRarity.Common),
            new Weapon("wooden_club",  "Wooden Club",  1, 10, ItemRarity.Common)
        };

        public static readonly List<Armor> CommonArmor = new()
        {
            new Armor("leather_armor", "Leather Armor", 2, 20, ItemRarity.Common),
            new Armor("cloth_tunic",   "Cloth Tunic",   1, 10, ItemRarity.Common)
        };


        // === UNCOMMON ITEMS ===
        public static readonly List<Weapon> UncommonWeapons = new()
        {
            new Weapon("steel_sword", "Steel Sword", 5, 45, ItemRarity.Uncommon),
            new Weapon("hunter_axe",  "Hunter Axe",  6, 48, ItemRarity.Uncommon),
            new Weapon("thorn_dagger","Thorn Dagger",4, 40, ItemRarity.Uncommon)
        };

        public static readonly List<Armor> UncommonArmor = new()
        {
            new Armor("chain_vest",       "Chain Vest",       4, 40, ItemRarity.Uncommon),
            new Armor("reinforced_tunic", "Reinforced Tunic", 3, 35, ItemRarity.Uncommon)
        };


        // === RARE ITEMS ===
        public static readonly List<Weapon> RareWeapons = new()
        {
            new Weapon("crystal_blade", "Crystal Blade", 8, 80, ItemRarity.Rare),
            new Weapon("obsidian_axe",  "Obsidian Axe", 10, 90, ItemRarity.Rare),
            new Weapon("serpent_fang",  "Serpent Fang", 9, 85, ItemRarity.Rare)
        };

        public static readonly List<Armor> RareArmor = new()
        {
            new Armor("dragon_scale", "Dragon Scale", 7, 75, ItemRarity.Rare),
            new Armor("shadow_cloak", "Shadow Cloak", 6, 70, ItemRarity.Rare)
        };


        // === EPIC ITEMS ===
        public static readonly List<Weapon> EpicWeapons = new()
        {
            new Weapon("phoenix_blade", "Phoenix Blade", 14, 150, ItemRarity.Epic),
            new Weapon("storm_hammer",  "Storm Hammer",  16, 170, ItemRarity.Epic),
            new Weapon("soul_ripper",   "Soul Ripper",   15, 160, ItemRarity.Epic)
        };

        public static readonly List<Armor> EpicArmor = new()
        {
            new Armor("titan_armor",  "Titan Armor",  12, 150, ItemRarity.Epic),
            new Armor("shadow_plate", "Shadow Plate", 11, 145, ItemRarity.Epic)
        };


        // === LEGENDARY ITEMS ===
        public static readonly List<Weapon> LegendaryWeapons = new()
        {
            new Weapon("excalibur",      "Excalibur",      22, 300, ItemRarity.Legendary),
            new Weapon("hellfire_blade", "Hellfire Blade", 24, 320, ItemRarity.Legendary),
            new Weapon("worldbreaker",   "Worldbreaker",   26, 350, ItemRarity.Legendary)
        };

        public static readonly List<Armor> LegendaryArmor = new()
        {
            new Armor("celestial_plate", "Celestial Plate", 18, 300, ItemRarity.Legendary),
            new Armor("demonic_skin",    "Demonic Skin",    20, 330, ItemRarity.Legendary)
        };
    }
}
