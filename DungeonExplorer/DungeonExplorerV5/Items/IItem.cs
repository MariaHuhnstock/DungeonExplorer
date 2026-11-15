namespace DungeonExplorerV5.Items
{
    // === ITEM INTERFACE ===
    // Base contract for all item types.
    public interface IItem
    {
        // === IDENTIFIER ===
        // Unique string used for saves, shop, and comparisons.
        string Id { get; }

        // === DISPLAY NAME ===
        string Name { get; }

        // === STACKING ===
        // True for items that can accumulate in one slot.
        bool IsStackable { get; }

        // === SHOP VALUE ===
        // Base gold value used in pricing.
        int BasePrice { get; }

        // === RARITY ===
        // Loot tier used for color coding + generators.
        ItemRarity Rarity { get; }
    }
}
