using InventorySystem.Enums;
using InventorySystem.Models;
using System.Globalization;

namespace InventorySystem.Services
{
    public class TxtItemLoader
    {
        public List<Weapon> Weapons { get; private set; } = new();
        public List<Armor> Armors { get; private set; } = new();
        public List<Potion> Potions { get; private set; } = new();
        public List<QuestItem> QuestItems { get; private set; } = new();

        public void LoadItems(string dataDirectory)
        {
            if (!Directory.Exists(dataDirectory))
                throw new DirectoryNotFoundException($"Директория с данными не найдена: {dataDirectory}");

            LoadWeapons(Path.Combine(dataDirectory, "Weapons.txt"));
            LoadArmors(Path.Combine(dataDirectory, "Armors.txt"));
            LoadPotions(Path.Combine(dataDirectory, "Potions.txt"));
            LoadQuestItems(Path.Combine(dataDirectory, "QuestItems.txt"));
        }

        private void LoadWeapons(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Файл оружия не найден: {filePath}");
                return;
            }

            var lines = File.ReadAllLines(filePath);
            int itemCount = 1;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//"))
                    continue;

                try
                {
                    var parts = ParseLine(line);
                    if (parts.Count < 5) continue;

                    var id = $"W{itemCount}";
                    var name = parts[1];
                    var weaponType = Enum.Parse<WeaponType>(parts[2]);
                    var rarity = Enum.Parse<Rarity>(parts[3]);
                    
                    if (!int.TryParse(parts[4], out int damage))
                        damage = 10;
                    
                    var price = parts.Count > 5 && int.TryParse(parts[5], out int p) ? p : 100;

                    var weapon = new Weapon(id, name, weaponType, rarity, damage, price);
                    Weapons.Add(weapon);
                    itemCount++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка загрузки оружия из строки '{line}': {ex.Message}");
                }
            }
        }

        private void LoadArmors(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Файл брони не найден: {filePath}");
                return;
            }

            var lines = File.ReadAllLines(filePath);
            int itemCount = 1;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//"))
                    continue;

                try
                {
                    var parts = ParseLine(line);
                    if (parts.Count < 5) continue;

                    var id = $"A{itemCount}";
                    var name = parts[1];
                    var armorType = Enum.Parse<ArmorType>(parts[2]);
                    var rarity = Enum.Parse<Rarity>(parts[3]);
                    
                    if (!int.TryParse(parts[4], out int defense))
                        defense = 5;
                    
                    var price = parts.Count > 5 && int.TryParse(parts[5], out int p) ? p : 50;

                    var slot = GetEquipmentSlotFromArmorType(armorType);
                    
                    var armor = new Armor(id, name, armorType, rarity, defense, slot, price);
                    Armors.Add(armor);
                    itemCount++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка загрузки брони из строки '{line}': {ex.Message}");
                }
            }
        }

        private EquipmentSlot GetEquipmentSlotFromArmorType(ArmorType armorType)
        {
            return armorType switch
            {
                ArmorType.Helmet => EquipmentSlot.Head,
                ArmorType.Chest => EquipmentSlot.Body,
                ArmorType.Boots => EquipmentSlot.Feet,
                ArmorType.Shield => EquipmentSlot.Hands,
                _ => EquipmentSlot.Body
            };
        }

        private void LoadPotions(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Файл зелий не найден: {filePath}");
                return;
            }

            var lines = File.ReadAllLines(filePath);
            int itemCount = 1;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//"))
                    continue;

                try
                {
                    var parts = ParseLine(line);
                    if (parts.Count < 5) continue;

                    var id = $"P{itemCount}";
                    var name = parts[1];
                    var rarity = Enum.Parse<Rarity>(parts[2]);
                    
                    if (!int.TryParse(parts[3], out int healAmount))
                    {
                        Console.WriteLine($"Неверное значение healAmount в строке: {line}");
                        continue;
                    }
                    
                    var price = parts.Count > 4 && int.TryParse(parts[4], out int p) ? p : 20;

                    var potion = new Potion(id, name, rarity, healAmount, price, maxStack: 10, quantity: 1);
                    Potions.Add(potion);
                    itemCount++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка загрузки зелья из строки '{line}': {ex.Message}");
                }
            }
        }

        private void LoadQuestItems(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Файл квестовых предметов не найден: {filePath}");
                return;
            }

            var lines = File.ReadAllLines(filePath);
            int itemCount = 1;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//"))
                    continue;

                try
                {
                    var parts = ParseLine(line);
                    if (parts.Count < 6) continue;

                    var id = $"Q{itemCount}";
                    var name = parts[1];
                    var questType = Enum.Parse<QuestItemType>(parts[2]);
                    var rarity = Enum.Parse<Rarity>(parts[3]);
                    var effectDescription = parts[4];
                    
                    var price = parts.Count > 5 && int.TryParse(parts[5], out int p) ? p : 0;

                    var questItem = new QuestItem(id, name, questType, rarity, effectDescription, price, maxStack: 1);
                    QuestItems.Add(questItem);
                    itemCount++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка загрузки квестового предмета из строки '{line}': {ex.Message}");
                }
            }
        }

        private List<string> ParseLine(string line)
        {
            return line.Split('|')
                      .Select(part => part.Trim('"').Trim())
                      .ToList();
        }
    }
} 