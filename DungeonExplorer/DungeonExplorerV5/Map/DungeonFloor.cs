using System;
using DungeonExplorerV5.Map.Generation;

namespace DungeonExplorerV5.Dungeon
{
    public class DungeonFloor
    {
        public int Width { get; }
        public int Height { get; }

        public Room[,] Rooms { get; }
        private readonly Random rng = new();

        public int StartX { get; private set; }
        public int StartY { get; private set; }

        public int StairsX { get; private set; }
        public int StairsY { get; private set; }

        public int FloorLevel { get; }

        public DungeonFloor(int floorLevel, int width = 9, int height = 9)
        {
            FloorLevel = floorLevel;
            Width = width;
            Height = height;

            Rooms = new Room[width, height];

            // Fill with walls
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    Rooms[x, y] = new Room(isWall: true);

            // Generate maze + rooms
            MapMaker.GenerateMaze(this);

            // Spawn stairs
            PlaceStairs();

            // Start position = stairs up OR random non-wall tile
            StartX = 1;
            StartY = 1;
        }

        public Room GetRoom(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return null;

            return Rooms[x, y];
        }

        private void PlaceStairs()
        {
            while (true)
            {
                int x = rng.Next(1, Width - 1);
                int y = rng.Next(1, Height - 1);

                if (!Rooms[x, y].IsWall)
                {
                    StairsX = x;
                    StairsY = y;
                    Rooms[x, y].HasStairsDown = true;
                    return;
                }
            }
        }
    }
}
