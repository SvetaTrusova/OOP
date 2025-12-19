using InventorySystem.Enums;
using InventorySystem.Constants;
using InventorySystem.Interfaces;

namespace InventorySystem.Models
{
    public class Weapon : Item, IEquipable, IUpgradable
    {
        public WeaponType WeaponType { get; private set; }
        public int Damage { get; private set; }
        public EquipmentSlot Slot { get; }
        public bool IsEquipped { get; set; }
        public int Level { get; private set; }
        public int MaxLevel { get; private set; }

        public Weapon(string id, string name, WeaponType weaponType, Rarity rarity, int damage, int price)
            : base(id, name, ItemType.Weapon, rarity, price)
        {
            WeaponType = weaponType;
            Damage = damage;
            Level = UpgradeConstants.START_LEVEL;
            MaxLevel = UpgradeConstants.DEFAULT_MAX_LEVEL;
            IsEquipped = false;
            Slot = EquipmentSlot.Weapon;
        }

        public void Equip()
        {
            if (!IsEquipped)
            {
                IsEquipped = true;
                Console.WriteLine($"{Name} экипировано");
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

        public void Upgrade()
        {
            if (CanUpgrade())
            {
                Level++;
                Damage = (int)(Damage * UpgradeConstants.DAMAGE_MULTIPLIER);
                Price = (int)(Price * UpgradeConstants.PRICE_MULTIPLIER);
                Console.WriteLine($"{Name} улучшено до уровня {Level}! Урон: {Damage}, Цена: {Price} монет");
            }
        }

        public bool CanUpgrade()
        {
            return Level < MaxLevel;
        }

        protected override string MakeDescription()
        {
            return $"{WeaponType} - {Damage} урона (Уровень {Level}) - {Price} монет";
        }
    }
}