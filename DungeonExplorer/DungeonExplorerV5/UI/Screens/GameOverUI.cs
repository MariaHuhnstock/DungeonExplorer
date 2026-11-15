using DungeonExplorerV5.Core;
using DungeonExplorerV5.UI.Utils;

namespace DungeonExplorerV5.UI.Screens
{
    public static class GameOverScreen
    {
        public static void Show(GameContext context)
        {
            Console.Clear();
            ConsoleHelper.Separator();
            ConsoleHelper.WriteLineColored("            GAME OVER", ConsoleColor.Red);
            ConsoleHelper.Separator();

            Console.WriteLine($"Player: {context.Player.Name}");
            Console.WriteLine($"Dungeon Level: {context.CurrentDungeonLevel}");
            Console.WriteLine($"Coins: {context.Player.Wallet.Coins}");

            ConsoleHelper.Separator();
            ConsoleHelper.WriteLineColored("[R] Return to Main Menu", ConsoleColor.Yellow);
            ConsoleHelper.WriteLineColored("[Q] Quit Game", ConsoleColor.Red);
        }
    }
}
