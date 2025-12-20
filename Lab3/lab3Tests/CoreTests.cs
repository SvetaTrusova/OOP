using Xunit;
using Moq;
using DeliverySystem.Core;
using DeliverySystem.Models;
using DeliverySystem.Enums;
using DeliverySystem.Interfaces;
using DeliverySystem.Mediators;

namespace DeliverySystem.Tests
{
    public class CoreTests
    {
        private readonly DeliverySystemApp _system;

        public CoreTests()
        {
            var mediatorMock = new Mock<IOrderMediator>();
            _system = new DeliverySystemApp(
                mediatorMock.Object,
                new DeliverySystem.Builders.CustomerBuilder(),
                new DeliverySystem.Builders.DishBuilder());
        }

        [Fact]
        public void CreateCustomer_AddsCustomerToList()
        {
            // Act
            var customer = _system.CreateCustomer(
                "Иван Иванов",
                "+79123456789",
                "ул. Ленина, 10");

            // Assert
            Assert.Single(_system.Customers);
            Assert.Equal("Иван Иванов", customer.Name);
        }



        [Fact]
        public void ApproveOrder_ChangesStatusToPreparing()
        {
            // Arrange 
            var customer = _system.CreateCustomer(
                "Иван",
                "+79123456789",
                "ул. Ленина, 10");

            var dish = _system.CreateDish("Пицца", "", 500, 15);
            var order = _system.CreateOrder(
                customer.Id,
                "ул. Пушкина, 20");

            // Act
            _system.ApproveOrder(order.Id);

            // Assert
            Assert.Equal("Готовится", _system.GetOrderStatus(order.Id));
        }

        [Fact]
        public void CalculateOrderTotal_IncludesTaxAndDelivery()
        {
            // Arrange 
            var customer = _system.CreateCustomer(
                "Иван",
                "+79123456789",
                "ул. Ленина, 10");

            var dish = _system.CreateDish("Пицца", "", 1000, 15);
            var order = _system.CreateOrder(
                customer.Id,
                "ул. Пушкина, 20");

            // Act
            var total = _system.CalculateOrderTotal(order.Id);

            // Assert
            var expectedSubtotal = 2000m;
            var tax = expectedSubtotal * 0.1m;
            var delivery = 100m;
            var expectedTotal = expectedSubtotal + tax + delivery;

            Assert.Equal(expectedTotal, total);
        }

        [Fact]
        public void GetCustomerDiscount_VIP_Returns10Percent()
        {
            // Arrange
            var customer = _system.CreateCustomer(
                "VIP Клиент",
                "+79123456789",
                "ул. Ленина, 10",
                CustomerCategory.VIP);

            // Act
            var discount = _system.GetCustomerDiscountPercentage(customer.Id);

            // Assert
            Assert.Equal(0.10m, discount);
        }
    }
}