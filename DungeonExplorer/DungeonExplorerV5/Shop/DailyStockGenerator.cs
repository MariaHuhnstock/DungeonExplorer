using System;
using System.Collections.Generic;
using DungeonExplorerV5.Generators;
using DungeonExplorerV5.Items;
using DungeonExplorerV5.Items.Consumables;
using DungeonExplorerV5.Items.Equipment;
using DungeonExplorerV5.Items.Generators;
using DungeonExplorerV5.Items.Potion;

namespace DungeonExplorerV5.Shop
{
    public static class DailyStockGenerator
    {
        private static readonly Random rng = new();

        public static List<IItem> GenerateDailyStock(int floor)
        {
            var items = new List<IItem>();

            int weaponCount = rng.Next(1, 2 + floor / 2);
            int armorCount = rng.Next(1, 2 + floor / 3);
            int potionCount = rng.Next(1, 3);

            // === WEAPONS ===
            for (int i = 0; i < weaponCount; i++)
            {
                var weapon = ItemGenerator.CreateRandomWeapon(GetRarityForFloor(floor));
                items.Add(weapon);
            }

            // === ARMOR ===
            for (int i = 0; i < armorCount; i++)
            {
                var armor = ItemGenerator.CreateRandomArmor(GetRarityForFloor(floor));
                items.Add(armor);
            }

            // === POTIONS ===
            for (int i = 0; i < potionCount; i++)
            {
                PotionTier tier = floor switch
                {
                    <= 2 => PotionTier.Small,
                    <= 4 => PotionTier.Medium,
                    _ => PotionTier.Large
                };

                items.Add(new HealthPotion(tier, floor));
            }

            return items;
        }

        private static ItemRarity GetRarityForFloor(int floor)
        {
            double roll = rng.NextDouble();

            return floor switch
            {
                <= 1 => roll < 0.85 ? ItemRarity.Common : ItemRarity.Uncommon,
                <= 3 => roll < 0.60 ? ItemRarity.Common :
                         roll < 0.90 ? ItemRarity.Uncommon : ItemRarity.Rare,
                <= 5 => roll < 0.40 ? ItemRarity.Common :
                         roll < 0.75 ? ItemRarity.Uncommon :
                         roll < 0.95 ? ItemRarity.Rare : ItemRarity.Epic,
                _ => roll < 0.20 ? ItemRarity.Common :
                     roll < 0.60 ? ItemRarity.Uncommon :
                     roll < 0.85 ? ItemRarity.Rare :
                     roll < 0.97 ? ItemRarity.Epic : ItemRarity.Legendary
            };
        }
    }
}
