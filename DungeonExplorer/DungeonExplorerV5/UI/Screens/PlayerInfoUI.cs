using System;
using System.Collections.Generic;
using System.Text;
using DungeonExplorerV5.Core;
using DungeonExplorerV5.UI.Utils;

namespace DungeonExplorerV5.UI.Screens
{
    // === PLAYER INFO UI ===
    // Unified player statistics panel for exploration and menus.
    public static class PlayerInfoUI
    {
        // === BASIC STATS BLOCK ===
        public static void DrawBasicStats(Player player)
        {
            ConsoleHelper.WriteLineColored($"{player.Name}  (Lvl {player.Progression.Level})", ConsoleColor.White);

            // HP
            ConsoleHelper.DrawPlayerHealthBar(player.Health, player.MaxHealth);
            Console.WriteLine();

            // XP
            ConsoleHelper.WriteLineColored("XP: ", ConsoleColor.Cyan);
            Console.WriteLine($"{player.Progression.Experience}/{player.Progression.XPToNextLevel}");

            // Coins
            ConsoleHelper.WriteLineColored("Coins: ", ConsoleColor.Yellow);
            Console.WriteLine(player.Wallet.Coins);
            Console.WriteLine();

            // Equipped weapon
            ConsoleHelper.WriteLineColored("Weapon:", ConsoleColor.Gray);
            var weapon = player.Inventory.GetEquippedWeapon();

            if (weapon != null)
            {
                Console.ForegroundColor = ItemColor.GetColor(weapon.Rarity);
                Console.WriteLine($"{weapon.Name}  (+{weapon.AttackBonus} ATK)");
                Console.ResetColor();
            }
            else
                Console.WriteLine("None");

            // Equipped armor
            ConsoleHelper.WriteLineColored("Armor:", ConsoleColor.Gray);
            var armor = player.Inventory.GetEquippedArmor();

            if (armor != null)
            {
                Console.ForegroundColor = ItemColor.GetColor(armor.Rarity);
                Console.WriteLine($"{armor.Name}  (+{armor.DefenseBonus} DEF)");
                Console.ResetColor();
            }
            else
                Console.WriteLine("None");

            Console.WriteLine();
        }

        // === FULL PANEL ===
        public static void Show(GameContext context)
        {
            ConsoleHelper.Separator();
            ConsoleHelper.WriteLineColored("PLAYER STATUS", ConsoleColor.Cyan);
            ConsoleHelper.Separator();

            DrawBasicStats(context.Player);

            ConsoleHelper.Separator();
        }
    }
}
