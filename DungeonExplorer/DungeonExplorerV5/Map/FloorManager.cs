using DungeonExplorerV5.Core;
using DungeonExplorerV5.Map;

namespace DungeonExplorerV5.Dungeon
{
    public class FloorManager
    {
        public DungeonFloor CurrentFloor { get; private set; }

        public int PlayerX { get; private set; }
        public int PlayerY { get; private set; }

        private readonly GameContext _context;

        public FloorManager(GameContext context)
        {
            _context = context;
        }

        public void LoadFloor(int level)
        {
            CurrentFloor = new DungeonFloor(level);
            PlayerX = CurrentFloor.StartX;
            PlayerY = CurrentFloor.StartY;
        }

        public bool TryMove(int dx, int dy)
        {
            int nx = PlayerX + dx;
            int ny = PlayerY + dy;

            var room = CurrentFloor.GetRoom(nx, ny);

            if (room == null || room.IsWall)
                return false;

            PlayerX = nx;
            PlayerY = ny;

            room.IsVisited = true;

            VisibilitySystem.UpdateVisibility(CurrentFloor, PlayerX, PlayerY);

            return true;
        }

        public bool IsOnStairs()
        {
            return PlayerX == CurrentFloor.StairsX &&
                   PlayerY == CurrentFloor.StairsY;
        }
    }
}
