using DungeonExplorerV5.Items;

namespace DungeonExplorerV5.Items.Equipment
{
    public class Weapon : IItem
    {
        // === BASIC INFO ===
        public string Id { get; }
        public string Name { get; }

        // === STATS ===
        public int AttackBonus { get; }
        public int BasePrice { get; }
        public ItemRarity Rarity { get; }

        // Weapons never stack
        public bool IsStackable => false;

        // === CONSTRUCTOR ===
        public Weapon(string id, string name, int attackBonus, int basePrice, ItemRarity rarity)
        {
            Id = id;
            Name = name;
            AttackBonus = attackBonus;
            BasePrice = basePrice;
            Rarity = rarity;
        }
    }
}
