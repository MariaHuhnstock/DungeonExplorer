using DungeonExplorerV5.Items;

namespace DungeonExplorerV5.Player.Inventory
{
    public class InventorySlot
    {
        public IItem Item { get; set; }
        public int Quantity { get; set; }
        public bool IsEquipped { get; set; }

        public InventorySlot(IItem item, int qty)
        {
            Item = item;
            Quantity = qty;
            IsEquipped = false;
        }
    }
}
