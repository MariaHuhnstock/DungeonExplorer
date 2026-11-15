using System;

namespace DungeonExplorerV5.Combat.Enemies
{
    public static class EnemyGenerator
    {
        private static readonly Random rng = new();

        public static Enemy CreateEnemyFromId(string id)
        {
            return id switch
            {
                "goblin" => new Goblin(1),
                "orc" => new Orc(1),
                _ => null
            };
        }

        public static Enemy CreateRandomEnemy(int floor)
            => GenerateEnemy(floor);

        public static Enemy GenerateEnemy(int floor)
            => GenerateEnemy(floor, 1.0);

        public static Enemy GenerateEnemy(int floor, double difficultyFactor)
        {
            double orcChance = 0.2 + difficultyFactor * 0.6;

            Enemy e = rng.NextDouble() < orcChance
                ? new Orc(floor)
                : new Goblin(floor);

            // Enemy already scales itself:
            // Goblin(floor) → calls ScaleWithFloor(floor)

            return e;
        }
    }
}
