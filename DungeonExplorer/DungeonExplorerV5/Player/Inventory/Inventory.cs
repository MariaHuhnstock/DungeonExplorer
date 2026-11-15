using System;
using System.Collections.Generic;
using System.Linq;
using DungeonExplorerV5.Items;
using DungeonExplorerV5.Items.Consumables;
using DungeonExplorerV5.Items.Equipment;

namespace DungeonExplorerV5.Player.Inventory
{
    public class Inventory
    {
        public List<InventorySlot> Slots { get; private set; } = new();

        // === ADD ITEM ===
        public void AddItem(IItem item, int amount = 1)
        {
            var slot = Slots.FirstOrDefault(s => s.Item.Id == item.Id && !s.IsEquipped);

            if (slot != null)
                slot.Quantity += amount;
            else
                Slots.Add(new InventorySlot(item, amount));
        }

        // === REMOVE ITEM ===
        public void RemoveItem(IItem item, int amount = 1)
        {
            var slot = Slots.FirstOrDefault(s => s.Item.Id == item.Id);

            if (slot == null)
                return;

            slot.Quantity -= amount;

            if (slot.Quantity <= 0)
                Slots.Remove(slot);
        }

        // === GET EQUIPPED ITEMS ===

        public Weapon GetEquippedWeapon()
            => Slots.FirstOrDefault(s => s.IsEquipped && s.Item is Weapon)?.Item as Weapon;

        public Armor GetEquippedArmor()
            => Slots.FirstOrDefault(s => s.IsEquipped && s.Item is Armor)?.Item as Armor;

        // === UPDATE ATTACK/DEF ===
        public int GetAttackBonus()
        {
            var w = GetEquippedWeapon();
            return w?.AttackBonus ?? 0;
        }

        public int GetDefenseBonus()
        {
            var a = GetEquippedArmor();
            return a?.DefenseBonus ?? 0;
        }

        // === POTIONS ===
        public bool UseHealthPotion(Player player)
        {
            var potionSlot = Slots.FirstOrDefault(s => s.Item is HealthPotion);

            if (potionSlot == null)
                return false;

            var potion = (HealthPotion)potionSlot.Item;

            player.Heal(potion.HealAmount);

            RemoveItem(potion, 1);
            return true;
        }
    }
}
