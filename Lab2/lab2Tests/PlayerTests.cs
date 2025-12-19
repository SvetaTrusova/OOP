using Microsoft.VisualStudio.TestTools.UnitTesting;
using InventorySystem.Models;

namespace lab2Tests
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void Player_Creation_SetsPropertiesCorrectly()
        {
            // Arrange & Act
            var player = new Player("Hero", 5, 150, 20, 1000);

            // Assert
            Assert.AreEqual("Hero", player.Name);
            Assert.AreEqual(5, player.Level);
            Assert.AreEqual(150, player.MaxHealth);
            Assert.AreEqual(150, player.Health);
            Assert.AreEqual(1000, player.Gold);
            Assert.IsNotNull(player.Inventory);
            Assert.AreEqual(20, player.Inventory.Capacity);
        }

        [TestMethod]
        public void SpendGold_WhenHasEnoughGold_ReturnsTrueAndReducesGold()
        {
            // Arrange
            var player = new Player("Test", 1, 100, 10, 100);

            // Act
            bool result = player.SpendGold(50);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(50, player.Gold);
        }

        [TestMethod]
        public void SpendGold_WhenHasNotEnoughGold_ReturnsFalse()
        {
            // Arrange
            var player = new Player("Test", 1, 100, 10, 30);

            // Act
            bool result = player.SpendGold(50);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(30, player.Gold);
        }

        [TestMethod]
        public void CanAfford_ReturnsCorrectValue()
        {
            // Arrange
            var player = new Player("Test", 1, 100, 10, 100);

            // Act & Assert
            Assert.IsTrue(player.CanAfford(50));
            Assert.IsTrue(player.CanAfford(100));
            Assert.IsFalse(player.CanAfford(150));
        }

        [TestMethod]
        public void AddGold_IncreasesGold()
        {
            // Arrange
            var player = new Player("Test", 1, 100, 10, 100);

            // Act
            player.AddGold(50);

            // Assert
            Assert.AreEqual(150, player.Gold);
        }

        [TestMethod]
        public void Heal_IncreasesHealth()
        {
            // Arrange
            var player = new Player("Test", 1, 100, 10, 100);
            player.TakeDamage(40); // Health: 60

            // Act
            player.Heal(20);

            // Assert
            Assert.AreEqual(80, player.Health);
        }

        [TestMethod]
        public void Heal_DoesNotExceedMaxHealth()
        {
            // Arrange
            var player = new Player("Test", 1, 100, 10, 100);
            player.TakeDamage(30); // Health: 70

            // Act
            player.Heal(50); // Should only heal to 100

            // Assert
            Assert.AreEqual(100, player.Health);
        }

        [TestMethod]
        public void TakeDamage_ReducesHealth()
        {
            // Arrange
            var player = new Player("Test", 1, 100, 10, 100);

            // Act
            player.TakeDamage(30);

            // Assert
            Assert.AreEqual(70, player.Health);
        }

        [TestMethod]
        public void TakeDamage_DoesNotGoBelowZero()
        {
            // Arrange
            var player = new Player("Test", 1, 100, 10, 100);

            // Act
            player.TakeDamage(150);

            // Assert
            Assert.AreEqual(0, player.Health);
        }
    }
}