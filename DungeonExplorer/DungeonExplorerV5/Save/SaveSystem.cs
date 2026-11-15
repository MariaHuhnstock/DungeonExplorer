using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace DungeonExplorerV5.Save
{
    // === SAVE SYSTEM ===
    // Handles JSON save/load + slot availability.
    public static class SaveSystem
    {
        // Builds the filename for a save slot.
        private static string SavePath(int slot)
            => $"save_slot_{slot}.json";

        // === SAVE ===
        // Write SaveData → JSON file.
        public static void SaveGame(SaveData data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(SavePath(data.Slot), json);
        }

        // === LOAD ===
        // Read JSON → SaveData.
        public static SaveData LoadGame(int slot)
        {
            string path = SavePath(slot);

            if (!File.Exists(path))
                return null;

            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<SaveData>(json);
        }

        // === SLOT CHECK ===
        // Returns which slots contain a save.
        public static List<bool> GetAllSlots(int slots)
        {
            var list = new List<bool>();

            for (int i = 1; i <= slots; i++)
                list.Add(File.Exists(SavePath(i)));

            return list;
        }
    }
}
