using System;

namespace DungeonExplorerV5.Dungeon
{
    public static class MapMaker
    {
        private static readonly Random rng = new();

        public static void GenerateMaze(DungeonFloor floor)
        {
            int w = floor.Width;
            int h = floor.Height;

            // Carve starting point
            int sx = rng.Next(1, w - 2);
            int sy = rng.Next(1, h - 2);

            floor.Rooms[sx, sy].IsWall = false;

            Carve(floor, sx, sy);
        }

        private static void Carve(DungeonFloor f, int x, int y)
        {
            var dirs = new[]
            {
                (1,0),( -1,0),(0,1),(0,-1)
            };

            // Shuffle
            for (int i = 0; i < dirs.Length; i++)
            {
                int r = rng.Next(i, dirs.Length);
                (dirs[i], dirs[r]) = (dirs[r], dirs[i]);
            }

            foreach (var (dx, dy) in dirs)
            {
                int x2 = x + dx * 2;
                int y2 = y + dy * 2;

                if (x2 < 1 || x2 >= f.Width - 1 ||
                    y2 < 1 || y2 >= f.Height - 1)
                    continue;

                if (!f.Rooms[x2, y2].IsWall)
                    continue;

                // Carve passage
                f.Rooms[x + dx, y + dy].IsWall = false;
                f.Rooms[x2, y2].IsWall = false;

                Carve(f, x2, y2);
            }
        }
    }
}
