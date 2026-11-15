using System;
using System.Collections.Generic;
using System.Text;
using DungeonExplorerV5.Items;

namespace DungeonExplorerV5.UI.Utils
{
    public static class ItemColor
    {
        // Returns the console color used for displaying items based on rarity.
        public static ConsoleColor GetColor(ItemRarity rarity)
        {
            return rarity switch
            {
                ItemRarity.Common => ConsoleColor.White,
                ItemRarity.Uncommon => ConsoleColor.Green,
                ItemRarity.Rare => ConsoleColor.Blue,
                ItemRarity.Epic => ConsoleColor.Magenta,
                ItemRarity.Legendary => ConsoleColor.DarkYellow,
                _ => ConsoleColor.White
            };
        }
    }
}
