using InventorySystem.Core;

namespace InventorySystem.Models
{
    public class Player
    {
        public string Name { get; private set; }
        public int Level { get; private set; }
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }
        public int Gold { get; private set; }
        public Inventory Inventory { get; private set; }

        public Player(string name, int level, int maxHealth, int inventoryCapacity, int startingGold = 1000)
        {
            Name = name;
            Level = level;
            MaxHealth = maxHealth;
            Health = MaxHealth;
            Gold = startingGold;
            Inventory = new Inventory(inventoryCapacity);
        }

        public bool CanAfford(int price) => Gold >= price;

        public bool SpendGold(int amount)
        {
            if (CanAfford(amount))
            {
                Gold -= amount;
                return true;
            }
            return false;
        }

        public void AddGold(int amount) => Gold += amount;

        public void Heal(int amount)
        {
            Health = Math.Min(Health + amount, MaxHealth);
            Console.WriteLine($"{Name} восстановил {amount}. Текущее здоровье: {Health}/{MaxHealth}");
        }

        public void TakeDamage(int amount)
        {
            Health = Math.Max(Health - amount, 0);
            Console.WriteLine($"{Name} получил {amount} урона. Текущее здоровье: {Health}/{MaxHealth}");

            if (Health == 0)
            {
                Console.WriteLine($"{Name} повержен!");
            }
        }
    }
}