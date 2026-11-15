using System.Collections.Generic;
using System.Linq;
using DungeonExplorerV5.Items;

namespace DungeonExplorerV5.Shop
{
    public class ShopInventory
    {
        public List<IItem> Items { get; private set; }
        public Dictionary<string, int> Quantities { get; private set; }

        public ShopInventory(List<IItem> items)
        {
            Items = new List<IItem>();
            Quantities = new Dictionary<string, int>();

            foreach (var item in items)
                Add(item);
        }

        public void Add(IItem item)
        {
            if (Quantities.ContainsKey(item.Id))
            {
                Quantities[item.Id]++;
            }
            else
            {
                Quantities[item.Id] = 1;
                Items.Add(item);
            }
        }

        public void RemoveOne(IItem item)
        {
            if (!Quantities.ContainsKey(item.Id))
                return;

            Quantities[item.Id]--;

            if (Quantities[item.Id] <= 0)
            {
                Quantities.Remove(item.Id);
                Items.RemoveAll(i => i.Id == item.Id);
            }
        }

        public void ReplaceAll(List<IItem> newItems)
        {
            Items.Clear();
            Quantities.Clear();

            foreach (var item in newItems)
                Add(item);
        }
    }
}
