using Xunit;
using Moq;
using DeliverySystem.Strategies;
using DeliverySystem.Models;
using DeliverySystem.Enums;
using DeliverySystem.Interfaces;
using DeliverySystem.Mediators;

namespace DeliverySystem.Tests
{
    public class StrategyTests
    {
        [Fact]
        public void StandardDeliveryStrategy_RegularCustomerSmallOrder_AddsDeliveryFee()
        {
            // Arrange
            var strategy = new StandardDeliveryStrategy();
            var customer = new Customer(1, "Иван", "+7912", "ул. Ленина, 10", CustomerCategory.Regular);
            var dish = new Dish(1, "Бургер", "", 300, 10);
            var items = new List<OrderItem> { new(dish, 1) };
            var order = new Order(1, customer, items, "ул. Ленина, 10", new Mock<IOrderMediator>().Object);

            // Act
            var total = strategy.CalculateTotal(order);

            // Assert
            var subtotal = 300m;
            var tax = subtotal * 0.1m;
            var delivery = 100m;
            var expected = subtotal + tax + delivery;

            Assert.Equal(expected, total);
        }

        [Fact]
        public void StandardDeliveryStrategy_VIPCustomer_FreeDelivery()
        {
            // Arrange
            var strategy = new StandardDeliveryStrategy();
            var customer = new Customer(1, "VIP", "+7912", "ул. Ленина, 10", CustomerCategory.VIP);
            var dish = new Dish(1, "Стейк", "", 2000, 30);
            var items = new List<OrderItem> { new(dish, 1) };
            var order = new Order(1, customer, items, "ул. Ленина, 10", new Mock<IOrderMediator>().Object);

            // Act
            var total = strategy.CalculateTotal(order);

            // Assert
            var subtotal = 2000m;
            var tax = subtotal * 0.1m;
            var discount = subtotal * 0.10m;
            var expected = subtotal + tax - discount;

            Assert.Equal(expected, total);
        }

        [Fact]
        public void ExpressDeliveryStrategy_AddsExpressCharge()
        {
            // Arrange
            var strategy = new ExpressDeliveryStrategy();
            var customer = new Customer(1, "Иван", "+7912", "ул. Ленина, 10", CustomerCategory.Regular);
            var dish = new Dish(1, "Пицца", "", 1000, 15);
            var items = new List<OrderItem> { new(dish, 1) };
            var order = new Order(1, customer, items, "ул. Ленина, 10", new Mock<IOrderMediator>().Object, true);

            // Act
            var total = strategy.CalculateTotal(order);

            // Assert
            var subtotal = 1000m;
            var tax = subtotal * 0.1m;
            var delivery = 500m;
            var serviceCharge = subtotal * 0.02m;
            var expected = subtotal + tax + delivery + serviceCharge;

            Assert.Equal(expected, total);
        }

        [Fact]
        public void ExpressDeliveryStrategy_SuperVIP_LargeOrder_FreeExpressDelivery()
        {
            // Arrange
            var strategy = new ExpressDeliveryStrategy();
            var customer = new Customer(1, "SuperVIP", "+7912", "ул. Ленина, 10", CustomerCategory.SuperVIP);
            var dish = new Dish(1, "Дорогое блюдо", "", 6000, 45);
            var items = new List<OrderItem> { new(dish, 1) };
            var order = new Order(1, customer, items, "ул. Ленина, 10", new Mock<IOrderMediator>().Object, true);

            // Act
            var total = strategy.CalculateTotal(order);

            // Assert
            var subtotal = 6000m;
            var tax = subtotal * 0.1m;
            var discount = subtotal * 0.15m;
            var serviceCharge = subtotal * 0.02m;
            var expected = subtotal + tax + serviceCharge - discount;

            Assert.Equal(expected, total);
        }
    }
}