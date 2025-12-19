using InventorySystem.Enums;
using InventorySystem.Constants;
using InventorySystem.Interfaces;

namespace InventorySystem.Models
{
    public class Armor : Item, IEquipable, IUpgradable
    {
        public ArmorType ArmorType { get; private set; }
        public int Defense { get; private set; }
        public EquipmentSlot Slot { get; private set; }
        public bool IsEquipped { get; set; }
        public int Level { get; private set; }
        public int MaxLevel { get; private set; }

        public Armor(string id, string name, ArmorType armorType, Rarity rarity, int defense, EquipmentSlot slot, int price)
            : base(id, name, ItemType.Armor, rarity, price)
        {
            ArmorType = armorType;
            Defense = defense;
            Slot = slot;
            Level = UpgradeConstants.START_LEVEL;
            MaxLevel = UpgradeConstants.DEFAULT_MAX_LEVEL;
            IsEquipped = false;
        }

        public void Equip()
        {
            if (!IsEquipped)
            {
                IsEquipped = true;
                Console.WriteLine($"{Name} надето");
            }
        }

        public void Unequip()
        {
            if (IsEquipped)
            {
                IsEquipped = false;
                Console.WriteLine($"{Name} снято");
            }
        }

        public bool CanUpgrade()
        {
            return Level < MaxLevel;
        }

        public void Upgrade()
        {
            if (CanUpgrade())
            {
                Level++;
                Defense = (int)(Defense * UpgradeConstants.DEFENSE_MULTIPLIER);
                Price = (int)(Price * UpgradeConstants.PRICE_MULTIPLIER);
                Console.WriteLine($"{Name} улучшено до уровня {Level}! Защита: {Defense}, цена: {Price} монет");
            }
        }

        protected override string MakeDescription()
        {
            return $"{ArmorType} - {Defense} защиты (Уровень {Level}) - {Price} монет";
        }
    }
}