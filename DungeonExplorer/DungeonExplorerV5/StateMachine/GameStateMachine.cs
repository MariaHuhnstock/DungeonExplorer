using System;
using System.Collections.Generic;
using System.Text;
using DungeonExplorerV5.Core;

namespace DungeonExplorerV5.StateMachine
{
    public class GameStateMachine
    {
        private IGameState current;
        private readonly GameContext context;

        // === INIT ===
        // store reference to global game context
        public GameStateMachine(GameContext ctx)
        {
            context = ctx;
        }

        // === STATE SWITCH ===
        // activates new state and triggers Enter()
        public void ChangeState(IGameState state)
        {
            if (state == null)
                return;

            current = state;
            current.Enter(context);
        }

        // === UPDATE LOOP ===
        // runs Update() on active state
        public void Update()
        {
            current?.Update(context);
        }
    }
}
