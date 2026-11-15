using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonExplorerV5.Combat.Enemies
{
    public class Goblin : Enemy
    {
        // === GOBLIN SETUP ===
        // Base goblin stats; scaled automatically by floor depth.
        public Goblin(int floor)
            : base("goblin", "Goblin", new EnemyStats(12, 3, 5, 8))
        {
            ScaleWithFloor(floor);
        }
    }
}
