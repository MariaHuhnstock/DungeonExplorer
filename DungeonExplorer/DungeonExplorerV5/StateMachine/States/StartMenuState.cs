using System;
using System.Collections.Generic;
using System.Text;
using DungeonExplorerV5.Core;
using DungeonExplorerV5.Map;
using DungeonExplorerV5.Save;
using DungeonExplorerV5.UI.Menus;

namespace DungeonExplorerV5.StateMachine.States
{
    // === START MENU STATE ===
    public class StartMenuState : IGameState
    {
        // draw the initial menu screen
        public void Enter(GameContext context)
        {
            StartMenuUI.DrawStartScreen();
        }

        // handle menu input
        public void Update(GameContext context)
        {
            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.N:
                    StartNewGame(context);
                    break;

                case ConsoleKey.L:
                    LoadGame(context);
                    break;

                case ConsoleKey.Q:
                    Environment.Exit(0);
                    break;

                default:
                    StartMenuUI.DrawStartScreen();
                    break;
            }
        }

        // === NEW GAME ===
        // pick a save slot → create player → initialize floor
        private void StartNewGame(GameContext context)
        {
            Console.Clear();
            Console.Write("Enter hero name: ");

            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
                name = "Nameless Hero";

            // choose save slot
            int chosenSlot = AskForSaveSlot("Select save slot for NEW GAME (1-3): ");
            context.CurrentSaveSlot = chosenSlot;

            // create player
            context.Player = new Player(name);
            context.CurrentDungeonLevel = 1;

            // build first dungeon floor
            context.FloorManager = new FloorManager();
            context.FloorManager.GenerateNewFloor(1);

            // spawn at start tile
            context.FloorManager.PlayerX = context.FloorManager.CurrentFloor.StartX;
            context.FloorManager.PlayerY = context.FloorManager.CurrentFloor.StartY;

            // initialize save slot
            context.AutoSave(context.CurrentSaveSlot);

            // switch into exploration
            context.StateMachine.ChangeState(new ExplorationState());
        }

        // === LOAD GAME ===
        // choose slot → load JSON → rebuild world + player
        private void LoadGame(GameContext context)
        {
            var slots = SaveSystem.GetAllSlots(3);
            StartMenuUI.DrawLoadMenu(slots);

            Console.Write("\nSelect save slot to LOAD (1-3) or X to cancel: ");
            string input = Console.ReadLine();

            if (input.Trim().ToLower() == "x")
                return;

            if (!int.TryParse(input, out int slot) || slot < 1 || slot > 3)
                return;

            var data = SaveSystem.LoadGame(slot);
            if (data == null)
            {
                Console.WriteLine("No save found in that slot.");
                Console.ReadKey(true);
                return;
            }

            context.CurrentSaveSlot = slot;

            // prepare new player + floor before applying save data
            context.Player = new Player(data.PlayerName);

            context.FloorManager = new FloorManager();
            context.FloorManager.GenerateNewFloor(data.DungeonLevel);

            // apply full save state (inventory, stats, map, position)
            SaveConverter.Apply(data, context);

            Console.Clear();
            Console.WriteLine($"Game loaded from slot {slot}!");
            Console.ReadKey(true);

            context.StateMachine.ChangeState(new ExplorationState());
        }

        // === SLOT INPUT ===
        // asks for a valid slot number 1–3
        private int AskForSaveSlot(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine();

                if (int.TryParse(input, out int slot) && slot >= 1 && slot <= 3)
                    return slot;

                Console.WriteLine("Invalid slot. Please choose 1, 2, or 3.");
            }
        }
    }
}
