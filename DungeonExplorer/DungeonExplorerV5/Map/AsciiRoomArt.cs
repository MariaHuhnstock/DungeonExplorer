namespace DungeonExplorerV5.Map
{
    // === ASCII ROOM ART ===
    // Provides symbols + colors for map rendering.
    public static class AsciiRoomArt
    {
        // === CELL STRUCT ===
        public struct Cell
        {
            public string Text;
            public ConsoleColor Color;

            public Cell(string text, ConsoleColor color)
            {
                Text = text;
                Color = color;
            }
        }

        // === SYMBOL SELECTION ===
        public static Cell GetSymbol(Room r, bool isPlayer)
        {
            if (!r.Revealed)
                return new Cell(" ", ConsoleColor.Black);

            if (isPlayer)
                return new Cell("P", ConsoleColor.Yellow);

            if (r.MerchantHere)
                return new Cell("M", ConsoleColor.Magenta);

            if (r.HasStairsDown)
                return new Cell("S", ConsoleColor.Green);

            if (r.Enemy != null && r.Enemy.IsAlive)
                return new Cell("E", ConsoleColor.Red);

            if (r.IsCleared)
                return new Cell("·", ConsoleColor.DarkGray);

            return new Cell("?", ConsoleColor.Gray);
        }

        // === ASCII BLOCK PARTS ===
        public static string GetTop(Room r)
            => "┌" + (r.WallNorth ? "───" : "   ") + "┐";

        public static string GetBottom(Room r)
            => "└" + (r.WallSouth ? "───" : "   ") + "┘";

        public static (string Left, string Right) GetSides(Room r)
            => (r.WallWest ? "│" : " ", r.WallEast ? "│" : " ");
    }
}
