namespace DungeonExplorerV5.Dungeon
{
    public class Room
    {
        public bool IsWall { get; set; }
        public bool IsVisited { get; set; }
        public bool IsVisible { get; set; }
        public bool IsCleared { get; set; }

        public bool HasEnemy { get; set; }
        public bool HasStairsDown { get; set; }
        public bool HasStairsUp { get; set; }
        public bool HasLoot { get; set; }

        public Room(bool isWall = false)
        {
            IsWall = isWall;
            IsVisited = false;
            IsVisible = false;
            IsCleared = false;
        }
    }
}
