using DungeonExplorerV5.Player;
using DungeonExplorerV5.Player.Inventory;

namespace DungeonExplorerV5.Core
{
    public class Player
    {
        public string Name { get; private set; }

        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public bool IsAlive => Health > 0;

        public Inventory Inventory { get; private set; }
        public EquipmentManager Equipment { get; private set; }
        public Wallet Wallet { get; private set; }
        public PlayerProgression Progression { get; private set; }

        public Player(string name)
        {
            Name = string.IsNullOrWhiteSpace(name) ? "Nameless Hero" : name;

            MaxHealth = 20;
            Health = MaxHealth;

            Inventory = new Inventory();
            Equipment = new EquipmentManager(Inventory);
            Wallet = new Wallet();
            Progression = new PlayerProgression();
        }

        public void TakeDamage(int amount)
        {
            amount = Math.Max(0, amount);
            Health = Math.Clamp(Health - amount, 0, MaxHealth);
        }

        public void Heal(int amount)
        {
            Health = Math.Clamp(Health + amount, 0, MaxHealth);
        }

        // === Combat Integration ===
        public int GetAttackPower()
        {
            int baseAtk = 2 + (Progression.Level - 1);
            return baseAtk + Inventory.GetAttackBonus();
        }

        public int GetDefensePower()
        {
            int baseDef = 1 + (Progression.Level - 1);
            return baseDef + Inventory.GetDefenseBonus();
        }
    }
}
