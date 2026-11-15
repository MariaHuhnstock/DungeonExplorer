using System;
using System.Collections.Generic;
using System.Text;
using DungeonExplorerV5.Core;

namespace DungeonExplorerV5.StateMachine
{
    // === STATE INTERFACE ===
    // base contract for all game states
    public interface IGameState
    {
        // called once when entering the state
        void Enter(GameContext context);

        // called every frame while the state is active
        void Update(GameContext context);
    }
}
