using System;
using System.Collections.Generic;
using System.Text;
using DungeonExplorerV5.UI.Utils;

namespace DungeonExplorerV5.UI.Menus
{
    // === COMBAT MENU ===
    public static class CombatMenu
    {
        public enum CombatChoice { Attack, Potion, Run }

        // reads combat input and returns the selected action
        public static CombatChoice GetChoice()
        {
            ConsoleHelper.Separator();
            ConsoleHelper.WriteLineColored("A) Attack", ConsoleColor.White);
            ConsoleHelper.WriteLineColored("H) Use Potion", ConsoleColor.Cyan);
            ConsoleHelper.WriteLineColored("R) Run", ConsoleColor.Yellow);
            ConsoleHelper.Separator();

            while (true)
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.A:
                    case ConsoleKey.NumPad1:
                        return CombatChoice.Attack;

                    case ConsoleKey.H:
                    case ConsoleKey.NumPad2:
                        return CombatChoice.Potion;

                    case ConsoleKey.R:
                    case ConsoleKey.NumPad3:
                        return CombatChoice.Run;

                    default:
                        // invalid key → wait for next input
                        continue;
                }
            }
        }
    }
}
