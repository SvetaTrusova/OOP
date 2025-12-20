using Xunit;
using Moq;
using DeliverySystem.States;
using DeliverySystem.Models;
using DeliverySystem.Interfaces;
using DeliverySystem.Mediators;

namespace DeliverySystem.Tests
{
    public class StateTests
    {
        [Fact]
        public void Order_CompleteStateFlow_WorksCorrectly()
        {
            // Arrange
            var mediatorMock = new Mock<IOrderMediator>();
            var customer = new Customer(1, "Иван", "+7912", "ул. Ленина, 10");
            var dish = new Dish(1, "Пицца", "", 500, 15);
            var items = new List<OrderItem> { new(dish, 1) };

            var order = new Order(1, customer, items, "ул. Ленина, 10", mediatorMock.Object);

            // Act & Assert
            Assert.Equal("Ожидает подтверждения", order.GetStatus());

            order.Approve();
            Assert.Equal("Готовится", order.GetStatus());

            order.CompletePreparation();
            Assert.Equal("Готов к доставке", order.GetStatus());

            order.AssignCourier();
            Assert.Equal("Курьер назначен", order.GetStatus());

            order.StartDelivery();
            Assert.Equal("В доставке", order.GetStatus());

            order.CompleteDelivery();
            Assert.Equal("Выполнен", order.GetStatus());
        }

        [Fact]
        public void Order_Cancel_PendingOrder_Succeeds()
        {
            // Arrange
            var mediatorMock = new Mock<IOrderMediator>();
            var customer = new Customer(1, "Иван", "+7912", "ул. Ленина, 10");
            var dish = new Dish(1, "Пицца", "", 500, 15);
            var items = new List<OrderItem> { new(dish, 1) };

            var order = new Order(1, customer, items, "ул. Ленина, 10", mediatorMock.Object);

            // Act
            order.Cancel();

            // Assert
            Assert.Equal("Отменен", order.GetStatus());
        }

        [Fact]
        public void Order_Cancel_DeliveringOrder_ThrowsException()
        {
            // Arrange
            var mediatorMock = new Mock<IOrderMediator>();
            var customer = new Customer(1, "Иван", "+7912", "ул. Ленина, 10");
            var dish = new Dish(1, "Пицца", "", 500, 15);
            var items = new List<OrderItem> { new(dish, 1) };

            var order = new Order(1, customer, items, "ул. Ленина, 10", mediatorMock.Object);
            order.Approve();
            order.CompletePreparation();
            order.AssignCourier();
            order.StartDelivery();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => order.Cancel());
        }

        [Fact]
        public void OrderState_GetStatus_ReturnsCorrectStatus()
        {
            // Arrange
            var pending = new PendingState();
            var preparing = new PreparingState();
            var ready = new ReadyForDeliveryState();
            var cancelled = new CancelledState();
            var completed = new CompletedState();

            // Act & Assert
            Assert.Equal("Ожидает подтверждения", pending.GetStatus());
            Assert.Equal("Готовится", preparing.GetStatus());
            Assert.Equal("Готов к доставке", ready.GetStatus());
            Assert.Equal("Отменен", cancelled.GetStatus());
            Assert.Equal("Выполнен", completed.GetStatus());
        }
    }
}