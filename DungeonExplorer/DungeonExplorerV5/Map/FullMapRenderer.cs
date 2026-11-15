using DungeonExplorerV5.Dungeon;

namespace DungeonExplorerV5.UI.Map
{
    public static class FullMapRenderer
    {
        public static void Draw(DungeonFloor floor)
        {
            Console.Clear();
            Console.WriteLine("=== FULL MAP ===");

            for (int y = 0; y < floor.Height; y++)
            {
                for (int x = 0; x < floor.Width; x++)
                {
                    var r = floor.Rooms[x, y];

                    if (r.IsWall)
                        Console.Write("█");
                    else if (r.HasStairsDown)
                        Console.Write("S");
                    else
                        Console.Write(".");
                }
                Console.WriteLine();
            }
        }
    }
}
