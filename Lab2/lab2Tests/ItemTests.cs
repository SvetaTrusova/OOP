using Microsoft.VisualStudio.TestTools.UnitTesting;
using InventorySystem.Models;
using InventorySystem.Enums;

namespace lab2Tests
{
    [TestClass]
    public class ItemTests
    {
        [TestMethod]
        public void Weapon_Creation_SetsPropertiesCorrectly()
        {
            // Arrange & Act
            var weapon = new Weapon("w1", "Steel Sword", WeaponType.Sword, 
                Rarity.Common, 15, 100);

            // Assert
            Assert.AreEqual("w1", weapon.Id);
            Assert.AreEqual("Steel Sword", weapon.Name);
            Assert.AreEqual(WeaponType.Sword, weapon.WeaponType);
            Assert.AreEqual(Rarity.Common, weapon.Rarity);
            Assert.AreEqual(15, weapon.Damage);
            Assert.AreEqual(100, weapon.Price);
            Assert.AreEqual(ItemType.Weapon, weapon.Type);
        }

        [TestMethod]
        public void Armor_Creation_SetsPropertiesCorrectly()
        {
            // Arrange & Act
            var armor = new Armor("a1", "Iron Helmet", ArmorType.Helmet, 
                Rarity.Uncommon, 10, EquipmentSlot.Head, 75);

            // Assert
            Assert.AreEqual("a1", armor.Id);
            Assert.AreEqual("Iron Helmet", armor.Name);
            Assert.AreEqual(ArmorType.Helmet, armor.ArmorType);
            Assert.AreEqual(Rarity.Uncommon, armor.Rarity);
            Assert.AreEqual(10, armor.Defense);
            Assert.AreEqual(EquipmentSlot.Head, armor.Slot);
            Assert.AreEqual(75, armor.Price);
            Assert.AreEqual(ItemType.Armor, armor.Type);
        }

        [TestMethod]
        public void Potion_Creation_SetsPropertiesCorrectly()
        {
            // Arrange & Act
            var potion = new Potion("p1", "Health Potion", Rarity.Common, 
                25, 50, maxStack: 10);

            // Assert
            Assert.AreEqual("p1", potion.Id);
            Assert.AreEqual("Health Potion", potion.Name);
            Assert.AreEqual(25, potion.HealAmount);
            Assert.AreEqual(50, potion.Price);
            Assert.AreEqual(1, potion.Quantity);
            Assert.AreEqual(10, potion.MaxStack);
            Assert.AreEqual(ItemType.Potion, potion.Type);
        }

        [TestMethod]
        public void QuestItem_Creation_SetsPropertiesCorrectly()
        {
            // Arrange & Act
            var questItem = new QuestItem("q1", "Ancient Key", QuestItemType.Key, 
                Rarity.Rare, "Opens ancient doors", 200);

            // Assert
            Assert.AreEqual("q1", questItem.Id);
            Assert.AreEqual("Ancient Key", questItem.Name);
            Assert.AreEqual(QuestItemType.Key, questItem.QuestItemType);
            Assert.AreEqual("Opens ancient doors", questItem.EffectDescription);
            Assert.AreEqual(200, questItem.Price);
            Assert.AreEqual(1, questItem.Quantity);
            Assert.AreEqual(ItemType.Quest, questItem.Type);
        }

        [TestMethod]
        public void Potion_Use_ReducesQuantity()
        {
            // Arrange
            var potion = new Potion("p1", "Health Potion", Rarity.Common, 
                20, 30, maxStack: 5, quantity: 3);

            // Act
            potion.Use();

            // Assert
            Assert.AreEqual(2, potion.Quantity);
        }

        [TestMethod]
        public void QuestItem_UseSpecial_ReducesQuantity()
        {
            // Arrange
            var questItem = new QuestItem("q1", "Key", QuestItemType.Key, 
                Rarity.Common, "Opens door", 50);

            // Act
            questItem.UseSpecial();

            // Assert
            Assert.AreEqual(0, questItem.Quantity);
            Assert.IsFalse(questItem.CanUse());
        }
    }
}