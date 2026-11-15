using System;
using System.Collections.Generic;
using System.Text;
using DungeonExplorerV5.Core;
using DungeonExplorerV5.UI.Utils;

namespace DungeonExplorerV5.StateMachine.States
{
    public class GameOverState : IGameState
    {
        // === ENTER STATE ===
        // show game over message
        public void Enter(GameContext context)
        {
            Console.Clear();
            ConsoleHelper.Separator();
            ConsoleHelper.WriteLineColored("                     GAME OVER", ConsoleColor.Red);
            ConsoleHelper.Separator();
        }

        // === UPDATE LOOP ===
        // show stats + wait for user input
        public void Update(GameContext context)
        {
            Console.WriteLine();
            ConsoleHelper.WriteLineColored("                   FINAL STATISTICS", ConsoleColor.Yellow);
            ConsoleHelper.Separator();

            Console.WriteLine($"  Player:            {context.Player.Name}");
            Console.WriteLine($"  Dungeon Level:     {context.CurrentDungeonLevel}");
            Console.WriteLine($"  Coins Carried:     {context.Player.Wallet.Coins}");
            Console.WriteLine();

            ConsoleHelper.Separator();
            ConsoleHelper.WriteLineColored("Press R -> Return to Main Menu", ConsoleColor.DarkYellow);
            ConsoleHelper.WriteLineColored("Press Q -> Quit Game", ConsoleColor.DarkRed);
            ConsoleHelper.Separator();

            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.R:
                    // Reset player + dungeon progress
                    context.Player = null;
                    context.CurrentDungeonLevel = 1;
                    context.StateMachine.ChangeState(new StartMenuState());
                    return;

                case ConsoleKey.Q:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
