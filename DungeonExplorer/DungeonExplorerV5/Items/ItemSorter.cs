using System;
using System.Collections.Generic;
using DungeonExplorerV5.Items;
using DungeonExplorerV5.Items.Currency;
using DungeonExplorerV5.Items.Consumables;
using DungeonExplorerV5.Items.Equipment;
using DungeonExplorerV5.InventorySystem;

namespace DungeonExplorerV5.Utils
{
    // === ITEM SORTER ===
    public static class ItemSorter
    {
        // === PUBLIC: SORT BARE ITEMS ===
        // Sorts a list of IItem according to type and stats.
        public static void SortItems(List<IItem> items)
        {
            if (items == null) return;
            items.Sort(CompareItems);
        }

        // === PUBLIC: SORT INVENTORY SLOTS ===
        // Same logic, but uses slot.Item as key.
        public static void SortSlots(List<InventorySlot> slots)
        {
            if (slots == null) return;

            slots.Sort((a, b) => CompareItems(a.Item, b.Item));
        }

        // === CORE COMPARISON LOGIC ===
        // 1) Category (Weapon → Armor → Potion → Coin → Other)
        // 2) Stat-based sorting within the same category
        // 3) Alphabetical fallback
        private static int CompareItems(IItem a, IItem b)
        {
            if (a == null && b == null) return 0;
            if (a == null) return 1;
            if (b == null) return -1;

            int tA = GetTypeOrder(a);
            int tB = GetTypeOrder(b);

            // --- Step 1: Category ---
            if (tA != tB)
                return tA.CompareTo(tB);

            // --- Step 2: Stat comparisons ---

            // Weapon → sort by AttackBonus DESC
            if (a is Weapon wa && b is Weapon wb)
                return wb.AttackBonus.CompareTo(wa.AttackBonus);

            // Armor → sort by DefenseBonus DESC
            if (a is Armor aa && b is Armor ab)
                return ab.DefenseBonus.CompareTo(aa.DefenseBonus);

            // Potion → heal amount DESC
            if (a is HealthPotion pa && b is HealthPotion pb)
                return pb.HealAmount.CompareTo(pa.HealAmount);

            // Coins → amount DESC
            if (a is Coin ca && b is Coin cb)
                return cb.Amount.CompareTo(ca.Amount);

            // --- Step 3: Name fallback ---
            return string.Compare(a.Name, b.Name, StringComparison.Ordinal);
        }

        // === CATEGORY ORDER ===
        // Assigns a stable numeric sort priority to item types.
        private static int GetTypeOrder(IItem item)
        {
            if (item is Weapon) return 1;
            if (item is Armor) return 2;
            if (item is HealthPotion) return 3;
            if (item is Coin) return 4;

            return 99; // Unknown types end up last
        }
    }
}
