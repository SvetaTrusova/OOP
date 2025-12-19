using Microsoft.VisualStudio.TestTools.UnitTesting;
using InventorySystem.Enums;
using InventorySystem.Models;

namespace lab2Tests
{
    [TestClass]
    public class UpgradableTests
    {
        [TestMethod]
        public void Weapon_Upgrade_IncreasesDamageAndPrice()
        {
            // Arrange
            var weapon = new Weapon("w1", "Sword", WeaponType.Sword, 
                Rarity.Common, 100, 100);
            int initialDamage = weapon.Damage;
            int initialPrice = weapon.Price;

            // Act
            weapon.Upgrade();

            // Assert
            Assert.AreEqual(2, weapon.Level);
            Assert.IsTrue(weapon.Damage > initialDamage);
            Assert.IsTrue(weapon.Price > initialPrice);
            Assert.IsTrue(weapon.CanUpgrade());
        }

        [TestMethod]
        public void Armor_Upgrade_IncreasesDefenseAndPrice()
        {
            // Arrange
            var armor = new Armor("a1", "Helmet", ArmorType.Helmet, 
                Rarity.Common, 50, EquipmentSlot.Head, 80);
            int initialDefense = armor.Defense;
            int initialPrice = armor.Price;

            // Act
            armor.Upgrade();

            // Assert
            Assert.AreEqual(2, armor.Level);
            Assert.IsTrue(armor.Defense > initialDefense);
            Assert.IsTrue(armor.Price > initialPrice);
            Assert.IsTrue(armor.CanUpgrade());
        }

        [TestMethod]
        public void CanUpgrade_WhenMaxLevel_ReturnsFalse()
        {
            // Arrange
            var weapon = new Weapon("w1", "Sword", WeaponType.Sword, 
                Rarity.Common, 100, 100);
            
            // Upgrade to max level (max is 5 by default)
            for (int i = 0; i < 4; i++) // Start at level 1, need 4 upgrades to reach 5
            {
                weapon.Upgrade();
            }

            // Act & Assert
            Assert.AreEqual(5, weapon.Level);
            Assert.IsFalse(weapon.CanUpgrade());
        }

        [TestMethod]
        public void Weapon_EquipAndUnequip_WorksCorrectly()
        {
            // Arrange
            var weapon = new Weapon("w1", "Sword", WeaponType.Sword, 
                Rarity.Common, 10, 100);

            // Act
            weapon.Equip();

            // Assert
            Assert.IsTrue(weapon.IsEquipped);

            // Act
            weapon.Unequip();

            // Assert
            Assert.IsFalse(weapon.IsEquipped);
        }

        [TestMethod]
        public void Armor_EquipAndUnequip_WorksCorrectly()
        {
            // Arrange
            var armor = new Armor("a1", "Helmet", ArmorType.Helmet, 
                Rarity.Common, 5, EquipmentSlot.Head, 50);

            // Act
            armor.Equip();

            // Assert
            Assert.IsTrue(armor.IsEquipped);

            // Act
            armor.Unequip();

            // Assert
            Assert.IsFalse(armor.IsEquipped);
        }
    }
}