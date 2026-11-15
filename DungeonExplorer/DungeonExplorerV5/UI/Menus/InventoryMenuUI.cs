using System;
using System.Collections.Generic;
using System.Text;
using DungeonExplorerV5.Items.Equipment;
using DungeonExplorerV5.Player.Inventory;
using DungeonExplorerV5.UI.Utils;

namespace DungeonExplorerV5.UI.Menus
{
    public static class InventoryMenu
    {
        // === MAIN ENTRY ===
        // Shows the player's inventory in sorted slot order.
        // Allows selecting items to equip/use/drop.
        public static void Show(Player player)
        {
            while (true)
            {
                Console.Clear();
                ConsoleHelper.Separator();
                ConsoleHelper.WriteLineColored("INVENTORY", ConsoleColor.Cyan);
                ConsoleHelper.Separator();

                var slots = player.Inventory.Slots;

                // No items → return immediately
                if (slots.Count == 0)
                {
                    Console.WriteLine("You have no items.");
                    ConsoleHelper.WaitForKey();
                    return;
                }

                // --- list stored items with rarity colors ---
                for (int i = 0; i < slots.Count; i++)
                {
                    var s = slots[i];
                    string text = s.GetDisplayName();

                    Console.ForegroundColor = ItemColor.GetColor(s.Item.Rarity);
                    Console.WriteLine($"{i + 1}. {text}");
                    Console.ResetColor();
                }

                // --- user selection ---
                Console.WriteLine();
                ConsoleHelper.WriteLineColored(
                    "Select item number to use/equip, or press X to exit.",
                    ConsoleColor.DarkGray
                );

                string input = Console.ReadLine()?.Trim().ToLower();
                if (input == "x")
                    return;

                if (!int.TryParse(input, out int index))
                    continue;

                index--;
                if (index < 0 || index >= slots.Count)
                    continue;

                // Open item action menu
                HandleItemAction(player, slots[index]);
            }
        }

        // === ITEM ACTION MENU ===
        // Provides available actions depending on item type.
        private static void HandleItemAction(Player player, InventorySlot slot)
        {
            while (true)
            {
                Console.Clear();
                ConsoleHelper.Separator();
                ConsoleHelper.WriteLineColored(slot.GetDisplayName(), ConsoleColor.Yellow);
                ConsoleHelper.Separator();

                // Determine available actions
                bool isEquipment = slot.Item is Weapon || slot.Item is Armor;
                bool isPotion = slot.Item is DungeonExplorerV3.Items.Consumables.HealthPotion;

                if (isEquipment)
                    Console.WriteLine("(E) Equip");

                if (isPotion)
                    Console.WriteLine("(U) Use Potion");

                Console.WriteLine("(D) Drop Item");
                Console.WriteLine("(B) Back");

                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    // Equip selected weapon/armor
                    case ConsoleKey.E:
                        if (isEquipment)
                        {
                            player.Inventory.EquipItem(slot.Item);
                            ConsoleHelper.WriteLineColored("Item equipped!", ConsoleColor.Green);
                            ConsoleHelper.WaitForKey();
                            return;
                        }
                        break;

                    // Consume potion
                    case ConsoleKey.U:
                        if (isPotion)
                        {
                            bool used = player.Inventory.UseHealthPotion(player);

                            if (!used)
                                ConsoleHelper.WriteLineColored("No potion available!", ConsoleColor.Red);

                            ConsoleHelper.WaitForKey();
                            return;
                        }
                        break;

                    // Remove item from inventory
                    case ConsoleKey.D:
                        player.Inventory.RemoveItem(slot.Item);
                        ConsoleHelper.WriteLineColored("Item removed.", ConsoleColor.DarkYellow);
                        ConsoleHelper.WaitForKey();
                        return;

                    // Go back to the inventory menu
                    case ConsoleKey.B:
                        return;
                }
            }
        }
    }
}
