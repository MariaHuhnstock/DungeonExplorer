using System;
using System.Collections.Generic;
using System.Text;
using DungeonExplorerV5.Core;
using DungeonExplorerV5.Map;
using DungeonExplorerV5.UI.Menus;
using DungeonExplorerV5.UI.Screens;
using DungeonExplorerV5.UI.Utils;

namespace DungeonExplorerV5.StateMachine.States
{
    // Handles free-roam exploration, movement, encounters and interactions.
    public class ExplorationState : IGameState
    {
        // === ENTER STATE ===
        // reveal room + neighbors
        public void Enter(GameContext context)
        {
            var r = CurrentRoom(context);
            r.Revealed = true;
            r.Visited = true;
            RevealAdjacent(context);
        }

        // === UPDATE LOOP ===
        public void Update(GameContext context)
        {
            Console.Clear();

            var floor = context.FloorManager.CurrentFloor;
            var room = CurrentRoom(context);

            // minimap
            MiniMapRenderer.DrawMiniMap(
                floor,
                context.FloorManager.PlayerX,
                context.FloorManager.PlayerY);

            room.Revealed = true;
            room.Visited = true;
            RevealAdjacent(context);

            // player info panel
            PlayerInfoUI.Show(context);

            // auto coin pickup
            if (room.CoinLoot > 0)
            {
                int amount = room.CoinLoot;
                room.CoinLoot = 0;
                context.Player.Wallet.AddCoins(amount);

                ConsoleHelper.WriteLineColored(
                    $">>> You picked up {amount} coins! <<<\n",
                    ConsoleColor.Yellow);
            }

            // enemy encounter
            if (room.Enemy != null && room.Enemy.IsAlive)
            {
                context.StateMachine.ChangeState(new CombatState(room.Enemy));
                return;
            }

            // movement + action menu
            ShowMoveMenu(context, room);
        }

        // === MOVE / ACTION MENU ===
        private void ShowMoveMenu(GameContext context, Room room)
        {
            // controls
            ConsoleHelper.WriteLineColored("W/A/S/D Move", ConsoleColor.Gray);
            ConsoleHelper.WriteLineColored("L Loot", ConsoleColor.Yellow);
            ConsoleHelper.WriteLineColored("I Inventory", ConsoleColor.Magenta);
            ConsoleHelper.WriteLineColored("M Merchant", ConsoleColor.Yellow);
            ConsoleHelper.WriteLineColored("T Stairs", ConsoleColor.Green);
            ConsoleHelper.WriteLineColored("TAB Map", ConsoleColor.Gray);
            ConsoleHelper.WriteLineColored("ESC Menu", ConsoleColor.Red);

            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.Tab:
                    // full map view
                    FullMapRenderer.DrawFullMap(
                        context.FloorManager.CurrentFloor,
                        context.FloorManager.PlayerX,
                        context.FloorManager.PlayerY);

                    ConsoleHelper.WriteLineColored("Press any key to return...", ConsoleColor.DarkGray);
                    Console.ReadKey(true);
                    return;

                // movement
                case ConsoleKey.W: TryMove(context, 0, -1); break;
                case ConsoleKey.S: TryMove(context, 0, 1); break;
                case ConsoleKey.A: TryMove(context, -1, 0); break;
                case ConsoleKey.D: TryMove(context, 1, 0); break;

                // loot
                case ConsoleKey.L:
                    LootRoom(context, room);
                    break;

                // inventory
                case ConsoleKey.I:
                    InventoryMenu.Show(context.Player);
                    break;

                // merchant
                case ConsoleKey.M:
                    if (room.MerchantHere)
                    {
                        var shop = new Shop.Shop(context.CurrentDungeonLevel);
                        ShopMenu.Show(context, shop, context.CurrentDungeonLevel);
                    }
                    break;

                // stairs
                case ConsoleKey.T:
                    if (room.HasStairsDown)
                        FloorDown(context);
                    break;

                // main menu
                case ConsoleKey.Escape:
                    context.StateMachine.ChangeState(new StartMenuState());
                    break;
            }
        }

        // === MOVEMENT HANDLING ===
        private void TryMove(GameContext context, int dx, int dy)
        {
            var floor = context.FloorManager.CurrentFloor;

            int newX = context.FloorManager.PlayerX + dx;
            int newY = context.FloorManager.PlayerY + dy;

            // out of bounds
            if (!floor.IsInside(newX, newY))
                return;

            var cur = CurrentRoom(context);
            var next = floor.GetRoom(newX, newY);

            // wall check
            bool blocked =
                (dx == 1 && cur.WallEast) ||
                (dx == -1 && cur.WallWest) ||
                (dy == 1 && cur.WallSouth) ||
                (dy == -1 && cur.WallNorth);

            if (blocked)
            {
                ConsoleHelper.WriteLineColored("A wall blocks your way.", ConsoleColor.DarkYellow);
                ConsoleHelper.WaitForKey();
                return;
            }

            // update position
            context.FloorManager.PlayerX = newX;
            context.FloorManager.PlayerY = newY;

            next.Revealed = true;
            next.Visited = true;

            RevealAdjacent(context);
            context.AutoSave(context.CurrentSaveSlot);
        }

        // === REVEAL NEIGHBORS ===
        private void RevealAdjacent(GameContext context)
        {
            var f = context.FloorManager.CurrentFloor;
            int x = context.FloorManager.PlayerX;
            int y = context.FloorManager.PlayerY;

            int[,] dirs = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

            // reveal 4 adjacent cells
            for (int i = 0; i < 4; i++)
            {
                int nx = x + dirs[i, 0];
                int ny = y + dirs[i, 1];

                if (f.IsInside(nx, ny))
                    f.GetRoom(nx, ny).Revealed = true;
            }
        }

        // === LOOT ROOM ===
        private void LootRoom(GameContext context, Room room)
        {
            // no loot
            if (room.Items.Count == 0)
            {
                ConsoleHelper.WriteLineColored("Nothing here.", ConsoleColor.DarkGray);
                ConsoleHelper.WaitForKey();
                return;
            }

            // collect items
            foreach (var item in room.Items)
            {
                context.Player.Inventory.AddItem(item);

                Console.ForegroundColor = ItemColor.GetColor(item.Rarity);
                Console.WriteLine($"Found: {item.Name}");
                Console.ResetColor();
            }

            room.Items.Clear();
            room.IsCleared = true;

            context.AutoSave(context.CurrentSaveSlot);
            ConsoleHelper.WaitForKey();
        }

        // === STAIRS DOWN ===
        private void FloorDown(GameContext context)
        {
            ConsoleHelper.WriteLineColored("You descend deeper...", ConsoleColor.Green);
            ConsoleHelper.WaitForKey();

            context.CurrentDungeonLevel++;
            context.FloorManager.GoToNextFloor();

            // scale potions for new depth
            context.Player.Inventory.UpgradeHealthPotions(context.CurrentDungeonLevel);
        }

        // === UTILITY ===
        private Room CurrentRoom(GameContext ctx)
            => ctx.FloorManager.CurrentFloor.GetRoom(
                ctx.FloorManager.PlayerX,
                ctx.FloorManager.PlayerY);
    }
}
