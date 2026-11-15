using DungeonExplorerV5.Items;

namespace DungeonExplorerV5.Shop
{
    public static class PriceCalculator
    {
        public static int GetBuyPrice(IItem item)
        {
            float rarityMult = GetRarityMultiplier(item.Rarity);
            return (int)(item.BasePrice * rarityMult);
        }

        public static int GetSellPrice(IItem item)
        {
            return (int)(GetBuyPrice(item) * 0.35f); // 35% of buy price
        }

        private static float GetRarityMultiplier(ItemRarity rarity)
        {
            return rarity switch
            {
                ItemRarity.Common => 1.0f,
                ItemRarity.Uncommon => 1.3f,
                ItemRarity.Rare => 2.0f,
                ItemRarity.Epic => 3.5f,
                ItemRarity.Legendary => 5.0f,
                _ => 1.0f
            };
        }
    }
}
