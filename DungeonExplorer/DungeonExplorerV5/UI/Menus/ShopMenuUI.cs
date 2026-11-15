using System;
using System.Collections.Generic;
using DungeonExplorerV5.Core;
using DungeonExplorerV5.Items;
using DungeonExplorerV5.Items.Consumables;
using DungeonExplorerV5.Items.Currency;
using DungeonExplorerV5.Items.Equipment;
using DungeonExplorerV5.Player.Inventory;
using DungeonExplorerV5.Shop;
using DungeonExplorerV5.UI.Utils;
using DungeonExplorerV5.Utils;

namespace DungeonExplorerV5.UI.Menus
{
    public static class ShopMenu
    {
        public static void Show(GameContext context, Shop.Shop shop, int floor)
        {
            Player player = context.Player;

            while (true)
            {
                Console.Clear();
                ConsoleHelper.Separator();
                ConsoleHelper.WriteLineColored("MERCHANT", ConsoleColor.Yellow);
                ConsoleHelper.Separator();

                Console.WriteLine($"Coins: {player.Wallet.Coins}");

                SortShopItems(shop);
                Console.WriteLine();

                for (int i = 0; i < shop.Inventory.Items.Count; i++)
                {
                    var item = shop.Inventory.Items[i];
                    int qty = shop.Inventory.Quantities[item.Id];
                    int price = PriceCalculator.GetBuyPrice(item);

                    Console.ForegroundColor = GetRarityColor(item);
                    Console.WriteLine($"{i + 1}. {item.Name} {GetBonusText(item)} x{qty} — {price}g");
                    Console.ResetColor();
                }

                Console.WriteLine();
                Console.WriteLine("(B) Buy   (S) Sell   (A) Sell All   (I) Inventory   (R) Refresh   (X) Exit");
                Console.Write("Choice: ");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.B:
                        BuyItem(player, shop);
                        break;

                    case ConsoleKey.S:
                        SellMenu(player, shop);
                        break;

                    case ConsoleKey.A:
                        {
                            int total = TradeTransaction.SellAll(player);
                            ConsoleHelper.WriteLineColored($"You gained {total} coins!", ConsoleColor.Green);
                            ConsoleHelper.WaitForKey();
                            break;
                        }

                    case ConsoleKey.I:
                        ShowPlayerInventory(context);
                        break;

                    case ConsoleKey.R:
                        if (shop.Refresh(player, floor))
                            ConsoleHelper.WriteLineColored("Stock refreshed!", ConsoleColor.Green);
                        else
                            ConsoleHelper.WriteLineColored("Not enough coins!", ConsoleColor.Red);

                        ConsoleHelper.WaitForKey();
                        break;

                    case ConsoleKey.X:
                        return;
                }
            }
        }

        private static void BuyItem(Player player, Shop.Shop shop)
        {
            ConsoleHelper.WriteLineColored("Enter item number:", ConsoleColor.DarkGray);

            if (!int.TryParse(Console.ReadLine(), out int idx))
                return;

            idx--;
            if (idx < 0 || idx >= shop.Inventory.Items.Count)
                return;

            var item = shop.Inventory.Items[idx];

            if (shop.Buy(player, item))
                ConsoleHelper.WriteLineColored("Purchase successful!", ConsoleColor.Green);
            else
                ConsoleHelper.WriteLineColored("Not enough coins!", ConsoleColor.Red);

            ConsoleHelper.WaitForKey();
        }

        private static void SellMenu(Player player, Shop.Shop shop)
        {
            var slots = player.Inventory.Slots;

            if (slots.Count == 0)
            {
                ConsoleHelper.WriteLineColored("You have nothing to sell.", ConsoleColor.Red);
                ConsoleHelper.WaitForKey();
                return;
            }

            Console.Clear();
            ConsoleHelper.Separator();
            ConsoleHelper.WriteLineColored("SELL ITEMS", ConsoleColor.Magenta);
            ConsoleHelper.Separator();

            for (int i = 0; i < slots.Count; i++)
            {
                var s = slots[i];

                // Prevent selling equipped items
                if (s.IsEquipped)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"{i + 1}. {s.Item.Name} [EQUIPPED]");
                    Console.ResetColor();
                    continue;
                }

                int value = PriceCalculator.GetSellPrice(s.Item);

                Console.ForegroundColor = GetRarityColor(s.Item);
                Console.WriteLine($"{i + 1}. {s.Item.Name} {GetBonusText(s.Item)} x{s.Quantity} — [{value}g]");
                Console.ResetColor();
            }

            Console.WriteLine("(X) Cancel");
            Console.Write("Choice: ");

            string input = Console.ReadLine()?.Trim();
            if (input?.ToLower() == "x")
                return;

            if (!int.TryParse(input, out int idxSell))
                return;

            idxSell--;
            if (idxSell < 0 || idxSell >= slots.Count)
                return;

            var target = slots[idxSell];

            if (target.IsEquipped)
            {
                ConsoleHelper.WriteLineColored("You cannot sell equipped items!", ConsoleColor.Red);
                ConsoleHelper.WaitForKey();
                return;
            }

            shop.Sell(player, target.Item);

            ConsoleHelper.WriteLineColored("Item sold!", ConsoleColor.Green);
            ConsoleHelper.WaitForKey();
        }

        private static string GetBonusText(IItem item)
        {
            return item switch
            {
                Weapon w => $"(ATK +{w.AttackBonus})",
                Armor a => $"(DEF +{a.DefenseBonus})",
                HealthPotion h => $"(Heals {h.HealAmount})",
                Coin c => $"({c.Amount} coins)",
                _ => ""
            };
        }

        private static void ShowPlayerInventory(GameContext context)
        {
            Console.Clear();
            ConsoleHelper.WriteLineColored("=== YOUR INVENTORY ===", ConsoleColor.Cyan);

            var inv = context.Player.Inventory;

            var sorted = new List<InventorySlot>(inv.Slots);
            InventorySorter.Sort(sorted);

            int index = 1;

            foreach (var slot in sorted)
            {
                string bonus = GetBonusText(slot.Item);
                string equipped = slot.IsEquipped ? " [E]" : "";

                Console.ForegroundColor = GetRarityColor(slot.Item);
                Console.WriteLine($"{index}. {slot.Item.Name}{equipped} {bonus} x{slot.Quantity}");
                Console.ResetColor();

                index++;
            }

            ConsoleHelper.WriteLineColored("\nPress any key to return to the shop...", ConsoleColor.DarkGray);
            Console.ReadKey(true);
        }

        private static ConsoleColor GetRarityColor(IItem item)
        {
            return item.Rarity switch
            {
                ItemRarity.Common => ConsoleColor.White,
                ItemRarity.Uncommon => ConsoleColor.Green,
                ItemRarity.Rare => ConsoleColor.Blue,
                ItemRarity.Epic => ConsoleColor.Magenta,
                ItemRarity.Legendary => ConsoleColor.DarkYellow,
                _ => ConsoleColor.White
            };
        }

        private static void SortShopItems(Shop.Shop shop)
        {
            ItemSorter.SortItems(shop.Inventory.Items);
        }
    }
}
