using System;
using DungeonExplorerV5.Combat.Enemies;
using DungeonExplorerV5.Core;

namespace DungeonExplorerV5.Combat
{
    public static class CombatCalculator
    {
        // ============================
        // DAMAGE CALCULATIONS (NEU!)
        // ============================

        /// <summary>
        /// Calculates how much damage the player deals to the enemy.
        /// </summary>
        public static int CalculatePlayerDamage(Player player, Enemy enemy)
        {
            int attack = player.GetAttackPower();        // player total ATK
            int defense = enemy.Stats.Defense;          // enemy DEF

            int dmg = attack - defense;
            if (dmg < 1) dmg = 1;

            return dmg;
        }

        /// <summary>
        /// Calculates how much damage the enemy deals to the player.
        /// </summary>
        public static int CalculateEnemyDamage(Player player, Enemy enemy)
        {
            int attack = enemy.Stats.Attack;            // enemy ATK
            int defense = player.GetDefensePower();     // player DEF

            int dmg = attack - defense;
            if (dmg < 1) dmg = 1;

            return dmg;
        }

        // ============================
        // EXISTING METHODS (von dir)
        // ============================

        public static CombatResult PlayerAttack(Player player, Enemy enemy)
        {
            var result = new CombatResult();

            int dmgToEnemy = CalculatePlayerDamage(player, enemy);
            enemy.Stats.Health -= dmgToEnemy;
            if (enemy.Stats.Health < 0) enemy.Stats.Health = 0;

            result.DamageToEnemy = dmgToEnemy;

            if (enemy.Stats.Health == 0)
            {
                result.CombatEnded = true;
                result.PlayerWon = true;
                return result;
            }

            int dmgToPlayer = CalculateEnemyDamage(player, enemy);
            player.TakeDamage(dmgToPlayer);

            result.DamageToPlayer = dmgToPlayer;

            if (player.Health <= 0)
            {
                result.CombatEnded = true;
                result.PlayerDied = true;
            }

            return result;
        }

        public static CombatResult TryEscape(Player player, Enemy enemy)
        {
            var result = new CombatResult();

            bool escaped = new Random().NextDouble() < 0.5;
            if (escaped)
            {
                result.CombatEnded = true;
                result.PlayerFled = true;
                return result;
            }

            int dmg = CalculateEnemyDamage(player, enemy);
            player.TakeDamage(dmg);

            result.DamageToPlayer = dmg;

            if (player.Health <= 0)
            {
                result.CombatEnded = true;
                result.PlayerDied = true;
            }

            return result;
        }
    }

    public class CombatResult
    {
        public int DamageToEnemy { get; set; }
        public int DamageToPlayer { get; set; }

        public bool CombatEnded { get; set; }
        public bool PlayerWon { get; set; }
        public bool PlayerFled { get; set; }
        public bool PlayerDied { get; set; }

        public bool EnemyDied => PlayerWon;
    }
}
