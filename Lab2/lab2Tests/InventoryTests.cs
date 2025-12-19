using Microsoft.VisualStudio.TestTools.UnitTesting;
using InventorySystem.Core;
using InventorySystem.Enums;
using InventorySystem.Models;

namespace lab2Tests
{
    [TestClass]
    public class InventoryTests
    {
        [TestMethod]
        public void AddItem_WhenSpaceAvailable_AddsItem()
        {
            // Arrange
            var inventory = new Inventory(5);
            var weapon = new Weapon("w1", "Sword", WeaponType.Sword, 
                Rarity.Common, 10, 100);

            // Act
            bool result = inventory.AddItem(weapon);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, inventory.Items.Count);
            Assert.AreEqual(weapon, inventory.Items[0]);
        }

        [TestMethod]
        public void AddItem_WhenInventoryFull_ReturnsFalse()
        {
            // Arrange
            var inventory = new Inventory(2);
            var item1 = new Weapon("w1", "Sword", WeaponType.Sword, 
                Rarity.Common, 10, 100);
            var item2 = new Armor("a1", "Helmet", ArmorType.Helmet, 
                Rarity.Common, 5, EquipmentSlot.Head, 50);
            var item3 = new Potion("p1", "Potion", Rarity.Common, 20, 30);

            inventory.AddItem(item1);
            inventory.AddItem(item2);

            // Act
            bool result = inventory.AddItem(item3);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(2, inventory.Items.Count);
        }

        [TestMethod]
        public void RemoveItem_WhenItemExists_RemovesItem()
        {
            // Arrange
            var inventory = new Inventory(5);
            var weapon = new Weapon("w1", "Sword", WeaponType.Sword, 
                Rarity.Common, 10, 100);
            inventory.AddItem(weapon);

            // Act
            bool result = inventory.RemoveItem("w1");

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(0, inventory.Items.Count);
        }

        [TestMethod]
        public void RemoveItem_WhenItemNotExists_ReturnsFalse()
        {
            // Arrange
            var inventory = new Inventory(5);

            // Act
            bool result = inventory.RemoveItem("nonexistent");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void BuyItem_WhenPlayerHasMoney_AddsItemAndDeductsGold()
        {
            // Arrange
            var inventory = new Inventory(5);
            var player = new Player("Test", 1, 100, 10, 200);
            var weapon = new Weapon("w1", "Sword", WeaponType.Sword, 
                Rarity.Common, 10, 100);

            // Act
            bool result = inventory.BuyItem(weapon, player);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, inventory.Items.Count);
            Assert.AreEqual(100, player.Gold); // 200 - 100 = 100
        }

        [TestMethod]
        public void BuyItem_WhenPlayerHasNoMoney_ReturnsFalse()
        {
            // Arrange
            var inventory = new Inventory(5);
            var player = new Player("Test", 1, 100, 10, 50);
            var weapon = new Weapon("w1", "Sword", WeaponType.Sword, 
                Rarity.Common, 10, 100);

            // Act
            bool result = inventory.BuyItem(weapon, player);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(0, inventory.Items.Count);
            Assert.AreEqual(50, player.Gold);
        }

        [TestMethod]
        public void BuyItem_WhenInventoryFull_ReturnsFalse()
        {
            // Arrange
            var inventory = new Inventory(1);
            var player = new Player("Test", 1, 100, 10, 200);
            var item1 = new Weapon("w1", "Sword", WeaponType.Sword, 
                Rarity.Common, 10, 100);
            var item2 = new Armor("a1", "Helmet", ArmorType.Helmet, 
                Rarity.Common, 5, EquipmentSlot.Head, 50);

            inventory.BuyItem(item1, player);

            // Act
            bool result = inventory.BuyItem(item2, player);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(1, inventory.Items.Count);
            Assert.AreEqual(100, player.Gold); // Только первый предмет куплен
        }
    }
}