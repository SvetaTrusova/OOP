using InventorySystem.Enums;
using InventorySystem.Interfaces;

namespace InventorySystem.Models
{
    public class Potion : Item, IUsable
    {
        public int HealAmount { get; private set; }
        public int Quantity { get; set; }
        public int MaxStack { get; private set; }

        public Potion(string id, string name, Rarity rarity, int healAmount, int price, int maxStack = 10, int quantity = 1)
            : base(id, name, ItemType.Potion, rarity, price)
        {
            HealAmount = healAmount;
            Quantity = quantity;
            MaxStack = maxStack;
        }

        public void Use()
        {
            if (Quantity > 0)
            {
                Quantity--;
                Console.WriteLine($"Использовано {Name}. Восстановлено {HealAmount}");

                if (Quantity == 0)
                {
                    Console.WriteLine($"{Name} теперь нет");
                }
            }
            else
            {
                Console.WriteLine($"{Name} закончилось :(");
            }
        }

        protected override string MakeDescription()
        {
            return $"Восстанавливает {HealAmount}. (Осталось: {Quantity}) - {Price} монет";
        }
    }
}