using DungeonExplorerV5.Dungeon;

namespace DungeonExplorerV5.UI.Map
{
    public static class MiniMapRenderer
    {
        public static void Draw(DungeonFloor floor, int px, int py)
        {
            Console.WriteLine("=== MAP ===");

            for (int y = 0; y < floor.Height; y++)
            {
                for (int x = 0; x < floor.Width; x++)
                {
                    var r = floor.Rooms[x, y];

                    if (x == px && y == py)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("P");
                        Console.ResetColor();
                    }
                    else if (!r.IsVisited)
                    {
                        Console.Write(" ");
                    }
                    else if (r.HasStairsDown)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("S");
                        Console.ResetColor();
                    }
                    else if (r.IsWall)
                    {
                        Console.Write("█");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
