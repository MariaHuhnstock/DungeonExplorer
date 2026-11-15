using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonExplorerV5.Combat.Enemies
{
    public class Orc : Enemy
    {
        // === ORC SETUP ===
        // Stronger base stats; scaled automatically depending on floor.
        public Orc(int floor)
            : base("orc", "Orc", new EnemyStats(20, 6, 10, 15))
        {
            ScaleWithFloor(floor);
        }
    }
}
