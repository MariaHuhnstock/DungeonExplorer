using DungeonExplorerV5.Items;
using DungeonExplorerV5.Items.Potion;

namespace DungeonExplorerV5.Items.Consumables
{
    public class HealthPotion : IItem
    {
        // Basic identification
        public string Id { get; }
        public string Name => $"{Tier} Potion";

        // Potion tier (Small/Medium/Large)
        public PotionTier Tier { get; }

        // Amount of HP restored when used
        public int HealAmount { get; }

        // Floor level used for ID generation or scaling
        public int Floor { get; }

        // Base buy/sell value depending on tier
        public int BasePrice => 10 * (int)Tier;

        // IItem metadata
        public bool IsStackable => true;
        public ItemRarity Rarity => ItemRarity.Common;

        // === CONSTRUCTOR ===
        // Creates a potion with fixed heal amount based on tier.
        public HealthPotion(PotionTier tier, int floor)
        {
            Tier = tier;
            Floor = floor;

            HealAmount = tier switch
            {
                PotionTier.Small => 20,
                PotionTier.Medium => 40,
                PotionTier.Large => 80,
                _ => 20
            };

            // Unique ID including tier + floor
            Id = $"potion_{tier.ToString().ToLower()}_f{floor}";
        }
    }
}
