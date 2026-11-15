using System;
using System.Collections.Generic;
using System.Text;
using DungeonExplorerV5.Save;
using DungeonExplorerV5.StateMachine;

namespace DungeonExplorerV5.Core
{
    public class GameContext
    {
        // Core references
        public PlayerNamespace.Player Player { get; set; }
        public Managers.FloorManager FloorManager { get; set; }

        // Current dungeon depth + active save slot
        public int CurrentDungeonLevel { get; set; } = 1;
        public int CurrentSaveSlot { get; set; } = 1;

        // Main state machine controlling game flow
        public GameStates.GameStateMachine StateMachine { get; set; }

        // Timestamp for autosave interval checking
        private DateTime lastAutoSave = DateTime.Now;

        // === AUTOSAVE UPDATE ===
        // Triggers autosave every 20 seconds if a valid slot is set.
        public void UpdateAutosave(int slot)
        {
            if (slot <= 0)
                return;

            if ((DateTime.Now - lastAutoSave).TotalSeconds < 20)
                return;

            AutoSave(slot);
            lastAutoSave = DateTime.Now;
        }

        // === AUTOSAVE EXECUTION ===
        // Converts current game state into a SaveData object and writes it to disk.
        public void AutoSave(int slot)
        {
            var data = SaveConverter.Build(this, slot);
            SaveSystem.SaveGame(data);
        }
    }
}
