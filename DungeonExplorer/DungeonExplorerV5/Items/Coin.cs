using DungeonExplorerV5.Items;

namespace DungeonExplorerV5.Items.Currency
{
    public class Coin : IItem
    {
        // === BASIC INFO ===
        public string Id => "coin";
        public string Name => "Coins";

        // === STACKING + VALUE ===
        public bool IsStackable => true;
        public int BasePrice => 1;
        public ItemRarity Rarity => ItemRarity.Common;

        // === AMOUNT IN STACK ===
        public int Amount { get; }

        // === CONSTRUCTOR ===
        public Coin(int amount)
        {
            Amount = amount;
        }
    }
}
