using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonExplorerV5.Combat.Enemies
{
    public class EnemyStats
    {
        // Core combat stats
        public int MaxHealth { get; set; }   // Maximum HP
        public int Health { get; set; }      // Current HP
        public int Damage { get; set; }      // Base attack damage

        // Rewards given to the player after defeating the enemy
        public int CoinReward { get; set; }
        public int XPReward { get; set; }

        // === CONSTRUCTOR ===
        // Initializes stats with given values; Health starts full.
        public EnemyStats(int health, int damage, int coinReward, int xpReward)
        {
            MaxHealth = health;
            Health = health;
            Damage = damage;
            CoinReward = coinReward;
            XPReward = xpReward;
        }
    }
}
