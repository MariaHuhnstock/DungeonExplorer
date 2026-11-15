using DungeonExplorerV5.Items;

namespace DungeonExplorerV5.Items.Equipment
{
    public class Armor : IItem
    {
        // Identification and display name
        public string Id { get; }
        public string Name { get; }

        // Defensive value provided when equipped
        public int DefenseBonus { get; }

        // Base gold value for shop pricing
        public int BasePrice { get; }

        // Rarity level for loot weighting and UI
        public ItemRarity Rarity { get; }

        // Armor pieces cannot stack
        public bool IsStackable => false;

        // === CONSTRUCTOR ===
        // Creates an armor item with fixed stats and pricing.
        public Armor(string id, string name, int defenseBonus, int basePrice, ItemRarity rarity)
        {
            Id = id;
            Name = name;
            DefenseBonus = defenseBonus;
            BasePrice = basePrice;
            Rarity = rarity;
        }
    }
}
