using System;
using DungeonExplorerV5.Core;
using DungeonExplorerV5.StateMachine;
using DungeonExplorerV5.StateMachine.States;

namespace DungeonExplorerV5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Dungeon Explorer V3";

            // === INITIALIZE GAME CONTEXT ===
            var context = new GameContext();

            // === INITIALIZE STATE MACHINE ===
            context.StateMachine = new GameStateMachine(context);

            // === ENTER START MENU ===
            context.StateMachine.ChangeState(new StartMenuState());

            // === MAIN GAME LOOP ===
            while (true)
            {
                context.StateMachine.Update();
                context.UpdateAutosave(context.CurrentSaveSlot);
            }
        }
    }
}
