using InventorySystem.Models;
using InventorySystem.Interfaces;

namespace InventorySystem.Core
{
    public class Inventory
    {
        public int Capacity { get; private set; }
        public List<Item> Items { get; private set; }

        public Inventory(int capacity)
        {
            Capacity = capacity;
            Items = new List<Item>();
        }

        public bool BuyItem(Item item, Player player)
        {
            if (!CanAddItem(item))
            {
                Console.WriteLine($"Нельзя купить {item.Name}! Нет места в инвентаре.");
                return false;
            }

            if (player.Gold < item.Price)
            {
                Console.WriteLine($"Нельзя купить {item.Name}! Недостаточно золота. Нужно: {item.Price}, есть: {player.Gold}");
                return false;
            }

            if (player.SpendGold(item.Price))
            {
                AddItem(item);
                Console.WriteLine($"Куплено: {item.Name} за {item.Price} золота");
                Console.WriteLine($"Остаток: {player.Gold} золота");
                return true;
            }

            return false;
        }

        public bool SellItem(string itemId, Player player)
        {
            var item = Items.Find(i => i.Id == itemId);
            if (item != null)
            {
                int sellPrice = CalculateSellPrice(item);

                Items.Remove(item);
                player.AddGold(sellPrice);

                Console.WriteLine($"Продано: {item.Name} за {sellPrice} золота");
                Console.WriteLine($"Теперь у вас: {player.Gold} золота");

                return true;
            }

            Console.WriteLine($"Предмет с ID {itemId} не найден");
            return false;
        }

        private int CalculateSellPrice(Item item)
        {
            if (item is IUpgradable upgradable && upgradable.Level > 1)
            {
                return (int)(item.Price * 0.8);
            }
            return (int)(item.Price * 0.7);
        }

        public bool AddItem(Item item)
        {
            if (CanAddItem(item))
            {
                Items.Add(item);
                return true;
            }
            return false;
        }

        public bool CanAddItem(Item item)
        {
            // Проверяем, можно ли стакать предмет
            if (item is IUsable usableItem)
            {
                var existingItem = Items.Find(i => i.Name == item.Name && i is IUsable);
                if (existingItem != null && existingItem is IUsable existingUsable)
                {
                    return existingUsable.Quantity < existingUsable.MaxStack;
                }
            }
            
            // Проверяем общее количество
            return Items.Count < Capacity;
        }

        public bool RemoveItem(string itemId)
        {
            var item = Items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                Items.Remove(item);
                Console.WriteLine($"Предмет {item.Name} удален из инвентаря");
                return true;
            }
            return false;
        }

        public void UseItem(string itemId)
        {
            var item = Items.Find(i => i.Id == itemId);
            if (item != null)
            {
                if (item is IUsable usable)
                {
                    usable.Use();
                    
                    if (usable.Quantity == 0)
                    {
                        RemoveItem(itemId);
                    }
                }
                else if (item is IEquipable equipable)
                {
                    if (equipable.IsEquipped)
                    {
                        equipable.Unequip();
                    }
                    else
                    {
                        equipable.Equip();
                    }
                }
                else if (item is QuestItem questItem)
                {
                    questItem.Use();
                }
                else
                {
                    Console.WriteLine($"Предмет {item.Name} нельзя использовать");
                }
            }
            else
            {
                Console.WriteLine($"Предмет с ID {itemId} не найден");
            }
        }

        public void UpgradeItem(string itemId)
        {
            var item = Items.Find(i => i.Id == itemId) as IUpgradable;
            if (item != null)
            {
                if (item.CanUpgrade())
                {
                    item.Upgrade();
                }
                else
                {
                    Console.WriteLine($"Нельзя улучшить {((Item)item).Name} дальше");
                }
            }
            else
            {
                Console.WriteLine($"Предмет с ID {itemId} нельзя улучшить или не найден");
            }
        }

        public void DisplayInventory(Player player)
        {
            Console.WriteLine($"\n=== ИНВЕНТАРЬ [{Items.Count}/{Capacity}] ===");

            var groupedItems = Items.GroupBy(i => i.GetType());

            foreach (var group in groupedItems)
            {
                Console.WriteLine($"\n--- {GetTypeName(group.Key.Name)} ---");
                foreach (var item in group)
                {
                    var equipped = item is IEquipable equipable && equipable.IsEquipped ? "[ЭКИПИРОВАНО]" : "";
                    var quantity = item is IUsable usable ? $" x{usable.Quantity}" : "";
                    var questInfo = item is QuestItem quest ? $" [{quest.QuestItemType}]" : "";
                    
                    var sellPrice = CalculateSellPrice(item);

                    Console.WriteLine($"- {item.Id}: {item.Name}{quantity} {equipped}{questInfo}");
                    Console.WriteLine($"  Описание: {item.Description}");
                    Console.WriteLine($"  Цена: {item.Price} золота | Продажа: {sellPrice} золота");
                }
            }
            
            Console.WriteLine($"\nЗолото игрока: {player.Gold}");
        }

        private string GetTypeName(string typeName)
        {
            return typeName switch
            {
                "Weapon" => "Оружие",
                "Armor" => "Броня",
                "Potion" => "Зелья",
                "QuestItem" => "Квестовые предметы",
                _ => typeName
            };
        }
    }
}