using System;
using System.Collections.Generic;
using DungeonExplorerV5.Items;
using DungeonExplorerV5.Items.Equipment;
using DungeonExplorerV5.Player.Inventory;

namespace DungeonExplorerV5.Utils
{
    public static class InventorySorter
    {
        public static void Sort(List<InventorySlot> slots)
        {
            slots.Sort((a, b) =>
            {
                // Sort by rarity (DESC)
                int rarityCompare = b.Item.Rarity.CompareTo(a.Item.Rarity);
                if (rarityCompare != 0)
                    return rarityCompare;

                // Sort by bonus strength (DESC)
                int aBonus = GetItemBonus(a.Item);
                int bBonus = GetItemBonus(b.Item);

                int bonusCompare = bBonus.CompareTo(aBonus);
                if (bonusCompare != 0)
                    return bonusCompare;

                // Sort alphabetically (ASC)
                return string.Compare(a.Item.Name, b.Item.Name, StringComparison.OrdinalIgnoreCase);
            });
        }

        // Extracts "item strength" depending on type
        private static int GetItemBonus(IItem item)
        {
            return item switch
            {
                Weapon w => w.AttackBonus,
                Armor a => a.DefenseBonus,
                _ => 0 // Potions & misc have no power bonus
            };
        }
    }
}
