namespace DungeonExplorerV5.Dungeon
{
    public static class VisibilitySystem
    {
        public static void UpdateVisibility(DungeonFloor floor, int px, int py)
        {
            for (int x = 0; x < floor.Width; x++)
            {
                for (int y = 0; y < floor.Height; y++)
                {
                    floor.Rooms[x, y].IsVisible =
                        (Math.Abs(x - px) <= 1 && Math.Abs(y - py) <= 1);
                }
            }
        }
    }
}
