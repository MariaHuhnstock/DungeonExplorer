using System;
using System.Collections.Generic;
using DungeonExplorerV5.Items;
using DungeonExplorerV5.Items.Consumables;
using DungeonExplorerV5.Items.Currency;
using DungeonExplorerV5.Items.Equipment;
using DungeonExplorerV5.Items.Potion;

namespace DungeonExplorerV5.Generators
{
    // === ITEM GENERATOR ===
    // Creates random loot, scaled equipment, coins, and potions.
    public static class ItemGenerator
    {
        private static readonly Random rng = new();

        // === POTIONS ===
        public static HealthPotion CreatePotion(PotionTier tier, int floor)
        {
            return new HealthPotion(tier, floor);
        }

        // === RANDOM ITEM (used for dungeon loot) ===
        public static IItem CreateRandomItem(int floor)
        {
            double roll = rng.NextDouble();

            // ~40% → potion
            if (roll < 0.4)
            {
                PotionTier tier =
                    floor >= 5 ? PotionTier.Large :
                    floor >= 3 ? PotionTier.Medium :
                    PotionTier.Small;

                return new HealthPotion(tier, floor);
            }

            // ~30% → weapon
            if (roll < 0.7)
                return GetRandomScaledWeapon(floor);

            // ~30% → armor
            return GetRandomScaledArmor(floor);
        }

        // === COINS ===
        public static int CreateCoinsAmount(int floor)
        {
            int min = 5 + floor * 2;
            int max = 10 + floor * 4;
            return rng.Next(min, max + 1);
        }

        // === WEAPON GENERATION ===
        public static Weapon GetRandomScaledWeapon(int floor)
        {
            var pool = BuildWeaponPoolForFloor(floor);
            var template = pool[rng.Next(pool.Count)];
            return CloneWeapon(template);
        }

        // === ARMOR GENERATION ===
        public static Armor GetRandomScaledArmor(int floor)
        {
            var pool = BuildArmorPoolForFloor(floor);
            var template = pool[rng.Next(pool.Count)];
            return CloneArmor(template);
        }

        // === WEAPON POOL SELECTION ===
        private static List<Weapon> BuildWeaponPoolForFloor(int floor)
        {
            List<Weapon> list = new();

            list.AddRange(ItemPools.CommonWeapons);
            if (floor >= 3) list.AddRange(ItemPools.UncommonWeapons);
            if (floor >= 5) list.AddRange(ItemPools.RareWeapons);
            if (floor >= 7) list.AddRange(ItemPools.EpicWeapons);
            if (floor >= 9) list.AddRange(ItemPools.LegendaryWeapons);

            return list;
        }

        // === ARMOR POOL SELECTION ===
        private static List<Armor> BuildArmorPoolForFloor(int floor)
        {
            List<Armor> list = new();

            list.AddRange(ItemPools.CommonArmor);
            if (floor >= 3) list.AddRange(ItemPools.UncommonArmor);
            if (floor >= 5) list.AddRange(ItemPools.RareArmor);
            if (floor >= 7) list.AddRange(ItemPools.EpicArmor);
            if (floor >= 9) list.AddRange(ItemPools.LegendaryArmor);

            return list;
        }

        // === CLONE HELPERS ===
        private static Weapon CloneWeapon(Weapon w)
        {
            return new Weapon(
                w.Id,
                w.Name,
                w.AttackBonus,
                w.BasePrice,
                w.Rarity
            );
        }

        private static Armor CloneArmor(Armor a)
        {
            return new Armor(
                a.Id,
                a.Name,
                a.DefenseBonus,
                a.BasePrice,
                a.Rarity
            );
        }
    }
}
