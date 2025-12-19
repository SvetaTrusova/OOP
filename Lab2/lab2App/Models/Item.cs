using InventorySystem.Enums;
using InventorySystem.Interfaces;

namespace InventorySystem.Models
{
    public abstract class Item : IItem
    {
        public string Id { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public ItemType Type { get; protected set; }
        public Rarity Rarity { get; protected set; }
        public int Price { get; protected set; }

        protected Item(string id, string name, ItemType type, Rarity rarity, int price)
        {
            Id = id;
            Name = name;
            Type = type;
            Rarity = rarity;
            Price = price;
            Description = MakeDescription();
        }

        protected virtual string MakeDescription()
        {
            return $"{Rarity} {Type} - {Price} монет";
        }

        public override string ToString()
        {
            return $"{Name} ({Rarity}) - {Price} монет";
        }
    }
}