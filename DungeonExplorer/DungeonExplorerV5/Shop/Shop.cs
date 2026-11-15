using DungeonExplorerV5.Core;
using DungeonExplorerV5.Items;

namespace DungeonExplorerV5.Shop
{
    public class Shop
    {
        public ShopInventory Inventory { get; }
        private bool _refreshedOnce = false;

        public Shop(int floor)
        {
            Inventory = new ShopInventory(DailyStockGenerator.GenerateDailyStock(floor));
        }

        public bool Buy(Player player, IItem item)
        {
            int price = PriceCalculator.GetBuyPrice(item);
            if (!player.Wallet.SpendCoins(price))
                return false;

            player.Inventory.AddItem(item);
            Inventory.RemoveOne(item);
            return true;
        }

        public bool Sell(Player player, IItem item)
        {
            int price = PriceCalculator.GetSellPrice(item);
            player.Wallet.AddCoins(price);

            player.Inventory.RemoveItem(item, 1);
            return true;
        }

        public bool Refresh(Player player, int floor)
        {
            if (_refreshedOnce)
                return false;

            int cost = 20 + 5 * floor;
            if (!player.Wallet.SpendCoins(cost))
                return false;

            Inventory.ReplaceAll(DailyStockGenerator.GenerateDailyStock(floor));
            _refreshedOnce = true;

            return true;
        }
    }
}
