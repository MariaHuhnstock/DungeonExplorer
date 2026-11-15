using DungeonExplorerV5.Combat.Enemies;
using DungeonExplorerV5.Core;
using DungeonExplorerV5.UI.Utils;

namespace DungeonExplorerV5.UI.Combat
{
    public static class CombatUI
    {
        public static void DrawHeader(Enemy enemy)
        {
            Console.Clear();
            ConsoleHelper.Separator();
            ConsoleHelper.WriteLineColored(
                $"         COMBAT — {enemy.Name.ToUpper()}",
                ConsoleColor.Red);
            ConsoleHelper.Separator();
            Console.WriteLine();
        }

        public static void DrawEnemy(Enemy enemy)
        {
            ConsoleHelper.WriteLineColored(enemy.Name, ConsoleColor.Red);
            ConsoleHelper.DrawEnemyHealthBar(
                enemy.Name,
                enemy.Stats.Health,
                enemy.Stats.MaxHealth
            );
        }

        public static void DrawPlayer(Player player)
        {
            ConsoleHelper.WriteLineColored(
                $"{player.Name} — Lvl {player.Progression.Level}",
                ConsoleColor.Cyan);

            ConsoleHelper.DrawPlayerHealthBar(player.Health, player.MaxHealth);
            Console.WriteLine();
        }

        public static void ShowPlayerAttack(int dmg)
        {
            ConsoleHelper.WriteLineColored($"You hit for {dmg} damage!", ConsoleColor.Yellow);
        }

        public static void ShowEnemyAttack(string enemyName, int dmg)
        {
            ConsoleHelper.WriteLineColored($"{enemyName} hits you for {dmg}!", ConsoleColor.Red);
        }

        public static void ShowPotionUsed()
        {
            ConsoleHelper.WriteLineColored("You used a potion!", ConsoleColor.Green);
        }

        public static void ShowNoPotion()
        {
            ConsoleHelper.WriteLineColored("You have no potion!", ConsoleColor.Red);
        }

        public static void ShowEscapeSuccess()
        {
            ConsoleHelper.WriteLineColored("You escaped!", ConsoleColor.Green);
        }

        public static void ShowEnemyDefeated(string name)
        {
            ConsoleHelper.WriteLineColored($"{name} was defeated!", ConsoleColor.Magenta);
        }
    }
}
