using DungeonExplorerV5.Core;
using DungeonExplorerV5.Combat;
using DungeonExplorerV5.Combat.Enemies;
using DungeonExplorerV5.UI.Combat;
using DungeonExplorerV5.UI.Screens;

namespace DungeonExplorerV5.StateMachine.States
{
    /// <summary>
    /// Handles combat flow between player and a single enemy.
    /// Contains NO UI and NO combat logic – 
    /// delegates UI to CombatUI and logic to CombatLogic.
    /// </summary>
    public class CombatState : IGameState
    {
        private readonly Enemy _enemy;

        public CombatState(Enemy enemy)
        {
            _enemy = enemy;
        }

        // === ENTER COMBAT ===
        public void Enter(GameContext context)
        {
            DrawCombatScreen(context);
        }

        // === MAIN LOOP ===
        public void Update(GameContext context)
        {
            var player = context.Player;

            // If player died → Game Over
            if (!player.IsAlive)
            {
                context.StateMachine.ChangeState(new GameOverState());
                return;
            }

            // If enemy died → give reward & return to exploration
            if (!_enemy.IsAlive)
            {
                HandleEnemyDefeated(context);
                return;
            }

            // Refresh display
            DrawCombatScreen(context);

            // Input (clean)
            var choice = CombatInput.GetChoice();

            switch (choice)
            {
                case CombatChoice.Attack:
                    HandleAttack(context);
                    break;

                case CombatChoice.Potion:
                    HandlePotion(context);
                    break;

                case CombatChoice.Run:
                    HandleRun(context);
                    break;
            }
        }

        // === ATTACK FLOW ===
        private void HandleAttack(GameContext context)
        {
            var player = context.Player;

            var result = CombatCalculator.PlayerAttack(player, _enemy);

            // feedback
            CombatUI.ShowPlayerAttack(result.DamageToEnemy);
            if (result.DamageToPlayer > 0)
                CombatUI.ShowEnemyAttack(_enemy.Name, result.DamageToPlayer);

            // check combat end
            if (result.CombatEnded)
            {
                ResolveCombatEnd(context, result);
                return;
            }
        }

        // === POTION FLOW ===
        private void HandlePotion(GameContext context)
        {
            var player = context.Player;

            bool used = player.Inventory.UseHealthPotion(player);

            if (used)
                CombatUI.ShowPotionUsed();
            else
                CombatUI.ShowNoPotion();

            // Enemy counterattack only if alive
            if (_enemy.IsAlive)
            {
                var result = CombatCalculator.PlayerAttack(player, _enemy); // WRONG — needs enemy attack
            }

            // Enemy counterattack
            int dmg = CombatCalculator.CalculateEnemyDamage(player, _enemy);
            player.TakeDamage(dmg);
            CombatUI.ShowEnemyAttack(_enemy.Name, dmg);
        }

        // === RUN ATTEMPT ===
        private void HandleRun(GameContext context)
        {
            var player = context.Player;
            var result = CombatCalculator.TryEscape(player, _enemy);

            if (result.PlayerFled)
            {
                CombatUI.ShowEscapeSuccess();
                ExitCombatWithoutKill(context);
                return;
            }

            // failed escape → take damage
            if (result.DamageToPlayer > 0)
                CombatUI.ShowEnemyAttack(_enemy.Name, result.DamageToPlayer);

            if (result.PlayerDied)
            {
                context.StateMachine.ChangeState(new GameOverState());
                return;
            }
        }

        // === ENEMY DEFEATED ===
        private void HandleEnemyDefeated(GameContext context)
        {
            CombatUI.ShowEnemyDefeated(_enemy.Name);

            var floor = context.FloorManager.CurrentFloor;
            var room = floor.GetRoom(context.FloorManager.PlayerX, context.FloorManager.PlayerY);
            room.Enemy = null;
            room.IsCleared = true;

            context.AutoSave(context.CurrentSaveSlot);
            context.StateMachine.ChangeState(new ExplorationState());
        }

        // === COMBAT END RESOLUTION ===
        private void ResolveCombatEnd(GameContext context, CombatResult result)
        {
            if (result.PlayerDied)
            {
                context.StateMachine.ChangeState(new GameOverState());
                return;
            }

            if (result.PlayerWon)
            {
                HandleEnemyDefeated(context);
                return;
            }

            if (result.PlayerFled)
            {
                ExitCombatWithoutKill(context);
                return;
            }
        }

        // === LEAVE COMBAT WITHOUT KILLING ENEMY ===
        private void ExitCombatWithoutKill(GameContext context)
        {
            var room = context.FloorManager
                              .CurrentFloor
                              .GetRoom(context.FloorManager.PlayerX, context.FloorManager.PlayerY);

            room.Enemy = null;
            room.IsCleared = false;

            context.StateMachine.ChangeState(new ExplorationState());
        }

        // === REDRAW SCREEN ===
        private void DrawCombatScreen(GameContext context)
        {
            CombatUI.DrawHeader(_enemy);
            CombatUI.DrawEnemy(_enemy);
            CombatUI.DrawPlayer(context.Player);
        }
    }
}
