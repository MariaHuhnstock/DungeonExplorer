using System;
using System.Collections.Generic;
using System.Text;
using DungeonExplorerV5.Core;
using DungeonExplorerV5.Map;
using DungeonExplorerV5.UI.Utils;

namespace DungeonExplorerV5.UI.Screens
{
    // === EXPLORATION UI ===
    public static class ExplorationUI
    {
        // Draws the main exploration screen including the minimap.
        public static void Draw(GameContext context, Room room)
        {
            Console.Clear();

            // === MINIMAP ===
            MiniMapRenderer.DrawMiniMap
            (
                context.FloorManager.CurrentFloor,
                context.FloorManager.PlayerX,
                context.FloorManager.PlayerY
            );

            ConsoleHelper.Separator();
            ConsoleHelper.WriteLineColored(
                $"Location: [{context.FloorManager.PlayerX},{context.FloorManager.PlayerY}]",
                ConsoleColor.Cyan);
            ConsoleHelper.Separator();

            Console.WriteLine(room.Description + "\n");

            // === PLAYER PANEL ===
            PlayerInfoUI.DrawBasicStats(context.Player);
        }
    }
}
