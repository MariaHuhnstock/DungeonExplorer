using System.Linq;
using DungeonExplorerV5.Items.Equipment;

namespace DungeonExplorerV5.Player.Inventory
{
    public class EquipmentManager
    {
        private readonly Inventory _inventory;

        public EquipmentManager(Inventory inv)
        {
            _inventory = inv;
        }

        public void Equip(InventorySlot slot)
        {
            if (slot.Item is Weapon)
            {
                // unequip old
                foreach (var s in _inventory.Slots.Where(s => s.IsEquipped && s.Item is Weapon))
                    s.IsEquipped = false;

                slot.IsEquipped = true;
            }

            if (slot.Item is Armor)
            {
                foreach (var s in _inventory.Slots.Where(s => s.IsEquipped && s.Item is Armor))
                    s.IsEquipped = false;

                slot.IsEquipped = true;
            }
        }
    }
}
