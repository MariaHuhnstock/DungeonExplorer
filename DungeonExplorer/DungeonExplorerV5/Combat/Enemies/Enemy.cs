using DungeonExplorerV5.Combat.Enemies;

namespace DungeonExplorerV5.Combat.Enemies
{
    public abstract class Enemy
    {
        public string Id { get; protected set; }
        public string Name { get; protected set; }

        public EnemyStats Stats { get; protected set; }

        // === DIRECT PROPERTIES ===
        public int Health
        {
            get => Stats.Health;
            protected set => Stats.Health = Math.Max(0, value);
        }

        public int MaxHealth
        {
            get => Stats.MaxHealth;
            protected set => Stats.MaxHealth = Math.Max(1, value);
        }

        public int Attack
        {
            get => Stats.Damage;
            protected set => Stats.Damage = Math.Max(1, value);
        }

        public int CoinReward
        {
            get => Stats.CoinReward;
            protected set => Stats.CoinReward = value;
        }

        public int XPReward
        {
            get => Stats.XPReward;
            protected set => Stats.XPReward = value;
        }

        public bool IsAlive => Health > 0;

        protected Enemy(string id, string name, EnemyStats stats)
        {
            Id = id;
            Name = name;
            Stats = stats;
        }

        // === DAMAGE HANDLING ===
        public void TakeDamage(int amount)
        {
            Health -= amount;
            if (Health < 0)
                Health = 0;
        }

        // === FLOOR SCALING ===
        public void ScaleWithFloor(int floor)
        {
            if (floor <= 1)
                return;

            double hpFactor = 1.0 + 0.30 * (floor - 1);
            double dmgFactor = 1.0 + 0.25 * (floor - 1);
            double coinFactor = 1.0 + 0.20 * (floor - 1);
            double xpFactor = 1.0 + 0.25 * (floor - 1);

            MaxHealth = (int)(MaxHealth * hpFactor);
            Health = MaxHealth;

            Attack = (int)(Attack * dmgFactor);
            CoinReward = (int)(CoinReward * coinFactor);
            XPReward = (int)(XPReward * xpFactor);

            if (Health < 1) Health = 1;
            if (Attack < 1) Attack = 1;
        }
    }
}
