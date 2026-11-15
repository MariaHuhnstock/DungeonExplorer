using System;
using System.Collections.Generic;
using System.Text;
using DungeonExplorerV5.Combat.Enemies;
using DungeonExplorerV5.Core;
using DungeonExplorerV5.Map;
using DungeonExplorerV5.Player.Inventory;

namespace DungeonExplorerV5.Save
{
    public static class SaveConverter
    {
        // === BUILD SAVE DATA ===
        // Creates a full snapshot of player, inventory and the whole floor.
        public static SaveData Build(GameContext ctx, int slot)
        {
            var p = ctx.Player;
            var floor = ctx.FloorManager.CurrentFloor;

            var data = new SaveData
            {
                Slot = slot,
                SaveDate = System.DateTime.Now,

                // player
                PlayerName = p.Name,
                PlayerHealth = p.Health,
                PlayerMaxHealth = p.MaxHealth,
                PlayerLevel = p.Progression.Level,
                PlayerXP = p.Progression.Experience,
                Coins = p.Wallet.Coins,

                // equipped
                EquippedWeaponId = p.Inventory.GetEquippedWeapon()?.Id,
                EquippedArmorId = p.Inventory.GetEquippedArmor()?.Id,

                // inventory snapshot
                Inventory = BuildInventory(p.Inventory),

                // dungeon info
                DungeonLevel = ctx.CurrentDungeonLevel,
                PlayerX = ctx.FloorManager.PlayerX,
                PlayerY = ctx.FloorManager.PlayerY,

                // full room grid
                Rooms = ExtractRooms(floor)
            };

            return data;
        }

        // === BUILD INVENTORY SNAPSHOT ===
        private static List<SavedItem> BuildInventory(Inventory inv)
        {
            var list = new List<SavedItem>();

            foreach (var s in inv.Slots)
            {
                if (s.Item == null)
                    continue;

                list.Add(new SavedItem
                {
                    ItemId = s.Item.Id,
                    Quantity = s.Quantity,
                    IsEquipped = s.IsEquipped
                });
            }

            return list;
        }

        // === EXTRACT ROOM GRID ===
        // Converts every room on the floor into a SavedRoom.
        private static SavedRoom[][] ExtractRooms(DungeonFloor floor)
        {
            var map = new SavedRoom[floor.Width][];

            for (int x = 0; x < floor.Width; x++)
            {
                map[x] = new SavedRoom[floor.Height];

                for (int y = 0; y < floor.Height; y++)
                {
                    var r = floor.Rooms[x, y];

                    var sr = new SavedRoom
                    {
                        Description = r.Description,

                        // state
                        IsCleared = r.IsCleared,
                        Visited = r.Visited,
                        Revealed = r.Revealed,
                        IsVisible = r.IsVisible,

                        MerchantHere = r.MerchantHere,
                        HasStairsDown = r.HasStairsDown,

                        // walls
                        WallNorth = r.WallNorth,
                        WallSouth = r.WallSouth,
                        WallWest = r.WallWest,
                        WallEast = r.WallEast,

                        // loot + enemy
                        CoinLoot = r.CoinLoot,
                        EnemyId = r.Enemy?.Id,
                        ItemIds = new List<string>()
                    };

                    foreach (var item in r.Items)
                        sr.ItemIds.Add(item.Id);

                    map[x][y] = sr;
                }
            }

            return map;
        }

        // === APPLY FULL SAVE DATA ===
        // Restores player, floor, rooms and inventory.
        public static void Apply(SaveData data, GameContext ctx)
        {
            // player
            ctx.Player.SetNameDirect(data.PlayerName);
            ctx.Player.SetHealthDirect(data.PlayerHealth, data.PlayerMaxHealth);
            ctx.Player.Wallet.Coins = data.Coins;
            ctx.Player.Progression.SetLevelAndXP(data.PlayerLevel, data.PlayerXP);

            // dungeon level
            ctx.CurrentDungeonLevel = data.DungeonLevel;

            // create a new floor and then overwrite everything with saved data
            var floor = new DungeonFloor(data.DungeonLevel);
            ctx.FloorManager.SetCurrentFloor(floor);

            // FIX: pass dungeonLevel into ApplyRooms so enemies get correct stats
            ApplyRooms(data.Rooms, floor, data.DungeonLevel);

            ctx.FloorManager.PlayerX = data.PlayerX;
            ctx.FloorManager.PlayerY = data.PlayerY;

            // inventory
            ctx.Player.Inventory.ClearAll();

            foreach (var entry in data.Inventory)
            {
                var item = ItemDatabase.GetItemById(entry.ItemId);
                if (item == null)
                    continue;

                ctx.Player.Inventory.AddItem(item, entry.Quantity);

                if (entry.IsEquipped)
                    ctx.Player.Inventory.EquipItem(item);
            }

            // failsafe re-equip by id if needed
            if (!string.IsNullOrEmpty(data.EquippedWeaponId))
                ctx.Player.Inventory.EquipItem(ItemDatabase.GetItemById(data.EquippedWeaponId));

            if (!string.IsNullOrEmpty(data.EquippedArmorId))
                ctx.Player.Inventory.EquipItem(ItemDatabase.GetItemById(data.EquippedArmorId));
        }

        // === APPLY ROOM SNAPSHOT ===
        // Restores all rooms from SavedRoom grid, including enemies and items.
        private static void ApplyRooms(SavedRoom[][] saved, DungeonFloor floor, int dungeonLevel)
        {
            int width = floor.Width;
            int height = floor.Height;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var sr = saved[x][y];
                    var r = floor.Rooms[x, y];

                    // basic room state
                    r.Description = sr.Description;
                    r.IsCleared = sr.IsCleared;
                    r.Visited = sr.Visited;
                    r.Revealed = sr.Revealed;
                    r.IsVisible = sr.IsVisible;

                    // special flags
                    r.MerchantHere = sr.MerchantHere;
                    r.HasStairsDown = sr.HasStairsDown;

                    // walls
                    r.WallNorth = sr.WallNorth;
                    r.WallSouth = sr.WallSouth;
                    r.WallWest = sr.WallWest;
                    r.WallEast = sr.WallEast;

                    // coin loot
                    r.CoinLoot = sr.CoinLoot;

                    // enemy restore
                    if (!string.IsNullOrEmpty(sr.EnemyId))
                    {
                        var enemy = EnemyGenerator.CreateEnemyFromId(sr.EnemyId);

                        // important: scale enemy to the dungeon level stored in the save
                        enemy.ScaleWithFloor(dungeonLevel);

                        r.Enemy = enemy;
                    }
                    else
                    {
                        r.Enemy = null;
                    }

                    // items restore
                    r.Items.Clear();
                    foreach (var id in sr.ItemIds)
                    {
                        var item = ItemDatabase.GetItemById(id);
                        if (item != null)
                            r.Items.Add(item);
                    }
                }
            }
        }
    }
}
