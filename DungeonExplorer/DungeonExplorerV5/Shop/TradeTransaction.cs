using System.Linq;
using DungeonExplorerV5.Core;
using DungeonExplorerV5.Items.Consumables;
using DungeonExplorerV5.Player.Inventory;

namespace DungeonExplorerV5.Shop
{
    public static class TradeTransaction
    {
        public static int SellAll(Player player)
        {
            var sellable = player.Inventory.Slots
                .Where(s => !s.IsEquipped && s.Item is not HealthPotion)
                .ToList();

            int total = 0;

            foreach (var slot in sellable)
            {
                int valuePerUnit = PriceCalculator.GetSellPrice(slot.Item);
                int quantity = slot.Quantity;

                total += valuePerUnit * quantity;

                player.Inventory.RemoveItem(slot.Item, quantity);
            }

            player.Wallet.AddCoins(total);
            return total;
        }
    }
}
