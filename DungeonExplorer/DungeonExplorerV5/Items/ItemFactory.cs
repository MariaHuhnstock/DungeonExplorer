using System;
using System.Collections.Generic;
using DungeonExplorerV5.Items;
using DungeonExplorerV5.Items.Consumables;
using DungeonExplorerV5.Items.Currency;
using DungeonExplorerV5.Items.Equipment;
using DungeonExplorerV5.Items.Potion;

namespace DungeonExplorerV5.Save
{
    // === ITEM FACTORY ===
    // Creates items from save IDs and generates IDs for saving.
    // All static weapons/armor are defined here as templates.
    public static class ItemFactory
    {
        // === STATIC ITEM TEMPLATES ===
        // These items are cloned when needed so template data stays untouched.
        private static readonly Dictionary<string, IItem> StaticItems = new()
        {
            // Common
            { "iron_sword",   new Weapon("iron_sword",   "Iron Sword",   3, 25, ItemRarity.Common) },
            { "rusty_dagger", new Weapon("rusty_dagger", "Rusty Dagger", 2, 18, ItemRarity.Common) },
            { "wooden_club",  new Weapon("wooden_club",  "Wooden Club",  1, 10, ItemRarity.Common) },

            { "leather_armor", new Armor("leather_armor", "Leather Armor", 2, 20, ItemRarity.Common) },
            { "cloth_tunic",   new Armor("cloth_tunic",   "Cloth Tunic",   1, 10, ItemRarity.Common) },

            // Uncommon
            { "steel_sword",    new Weapon("steel_sword",    "Steel Sword",    5, 45, ItemRarity.Uncommon) },
            { "hunter_axe",     new Weapon("hunter_axe",     "Hunter Axe",     6, 48, ItemRarity.Uncommon) },
            { "thorn_dagger",   new Weapon("thorn_dagger",   "Thorn Dagger",   4, 40, ItemRarity.Uncommon) },

            { "chain_vest",       new Armor("chain_vest",       "Chain Vest",       4, 40, ItemRarity.Uncommon) },
            { "reinforced_tunic", new Armor("reinforced_tunic", "Reinforced Tunic", 3, 35, ItemRarity.Uncommon) },

            // Rare
            { "crystal_blade", new Weapon("crystal_blade", "Crystal Blade", 8, 80, ItemRarity.Rare) },
            { "obsidian_axe",  new Weapon("obsidian_axe",  "Obsidian Axe", 10, 90, ItemRarity.Rare) },
            { "serpent_fang",  new Weapon("serpent_fang",  "Serpent Fang", 9, 85, ItemRarity.Rare) },

            { "dragon_scale", new Armor("dragon_scale", "Dragon Scale", 7, 75, ItemRarity.Rare) },
            { "shadow_cloak", new Armor("shadow_cloak", "Shadow Cloak", 6, 70, ItemRarity.Rare) },

            // Epic
            { "phoenix_blade", new Weapon("phoenix_blade", "Phoenix Blade", 14, 150, ItemRarity.Epic) },
            { "storm_hammer",  new Weapon("storm_hammer",  "Storm Hammer",  16, 170, ItemRarity.Epic) },
            { "soul_ripper",   new Weapon("soul_ripper",   "Soul Ripper",   15, 160, ItemRarity.Epic) },

            { "titan_armor",  new Armor("titan_armor",  "Titan Armor",  12, 150, ItemRarity.Epic) },
            { "shadow_plate", new Armor("shadow_plate", "Shadow Plate", 11, 145, ItemRarity.Epic) },

            // Legendary
            { "excalibur",      new Weapon("excalibur",      "Excalibur",      22, 300, ItemRarity.Legendary) },
            { "hellfire_blade", new Weapon("hellfire_blade", "Hellfire Blade", 24, 320, ItemRarity.Legendary) },
            { "worldbreaker",   new Weapon("worldbreaker",   "Worldbreaker",   26, 350, ItemRarity.Legendary) },

            { "celestial_plate", new Armor("celestial_plate", "Celestial Plate", 18, 300, ItemRarity.Legendary) },
            { "demonic_skin",    new Armor("demonic_skin",    "Demonic Skin",    20, 330, ItemRarity.Legendary) }
        };

        // === CREATE FROM ID ===
        // Converts a string ID from the save file into a real item.
        public static IItem CreateFromId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            // Static templates → clone
            if (StaticItems.ContainsKey(id))
                return Clone(StaticItems[id]);

            // Dynamic coin ID: "coins_100"
            if (id.StartsWith("coins_"))
            {
                if (int.TryParse(id[6..], out int amount))
                    return new Coin(amount);

                throw new Exception($"Invalid coin item ID: {id}");
            }

            // Dynamic potion ID: "potion_small_floor3"
            if (id.StartsWith("potion_"))
                return ParsePotion(id);

            throw new Exception($"Unknown item ID: {id}");
        }

        // === PARSE POTION ===
        // Converts dynamic potion IDs into HealthPotion objects.
        private static IItem ParsePotion(string id)
        {
            // potion_small_floor5 → ["potion", "small", "floor5"]
            var parts = id.Split('_');

            if (parts.Length < 3)
                throw new Exception($"Invalid potion ID format: {id}");

            PotionTier tier = parts[1].ToLower() switch
            {
                "small" => PotionTier.Small,
                "medium" => PotionTier.Medium,
                "large" => PotionTier.Large,
                _ => PotionTier.Small
            };

            string floorText = parts[2].Replace("floor", "");
            int floor = int.TryParse(floorText, out int f) ? f : 1;

            return new HealthPotion(tier, floor);
        }

        // === CLONE TEMPLATE ITEM ===
        // Ensures each created item is an independent copy.
        private static IItem Clone(IItem item)
        {
            return item switch
            {
                Weapon w => new Weapon(w.Id, w.Name, w.AttackBonus, w.BasePrice, w.Rarity),
                Armor a => new Armor(a.Id, a.Name, a.DefenseBonus, a.BasePrice, a.Rarity),
                Coin c => new Coin(c.Amount),
                HealthPotion hp => new HealthPotion(hp.Tier, hp.Floor),

                _ => throw new Exception($"Clone missing for type: {item.GetType().Name}")
            };
        }

        // === BUILD SAVE ID ===
        // Converts an item back into a string ID for saving.
        public static string GetSaveId(IItem item)
        {
            return item switch
            {
                Coin c => $"coins_{c.Amount}",
                HealthPotion hp => $"potion_{hp.Tier.ToString().ToLower()}_floor{hp.Floor}",

                // Weapons & armor already use their Id as save ID
                _ => item.Id
            };
        }
    }
}
