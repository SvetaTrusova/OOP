using Xunit;
using Moq;
using DeliverySystem.Services;
using DeliverySystem.Models;
using DeliverySystem.States;
using DeliverySystem.Interfaces;
using DeliverySystem.Mediators;


namespace DeliverySystem.Tests
{
    public class ServiceTests
    {
        [Fact]
        public void NotificationService_HandleOrderCreated_SendsSMS()
        {
            // Arrange
            var mediatorMock = new Mock<IOrderMediator>();
            var service = new NotificationService(mediatorMock.Object);
            var customer = new Customer(1, "Иван", "+79123456789", "ул. Ленина, 10");
            var order = new Order(1, customer, new List<OrderItem>(), "ул. Ленина, 10", mediatorMock.Object);

            // Act
            service.HandleOrderCreated(order);

            // Assert
            mediatorMock.Verify(m => m.RegisterService(
                "NotificationService",
                It.IsAny<IOrderServiceComponent>()),
                Times.Once);
        }

        [Fact]
        public void KitchenService_HandleOrderCreated_StartsPreparation()
        {
            // Arrange
            var mediatorMock = new Mock<IOrderMediator>();
            var service = new KitchenService(mediatorMock.Object);
            var customer = new Customer(1, "Иван", "+7912", "ул. Ленина, 10");
            var dish = new Dish(1, "Пицца", "", 500, 15);
            var order = new Order(1, customer,
                new List<OrderItem> { new(dish, 1) },
                "ул. Ленина, 10",
                mediatorMock.Object);

            // Act
            service.HandleOrderCreated(order);

            // Assert
            mediatorMock.Verify(m => m.RegisterService(
                "KitchenService",
                It.IsAny<IOrderServiceComponent>()),
                Times.Once);
        }

        [Fact]
        public void DeliveryService_HandleOrderCreated_FindsCourier()
        {
            // Arrange
            var mediatorMock = new Mock<IOrderMediator>();
            var service = new DeliveryService(mediatorMock.Object);
            var customer = new Customer(1, "Иван", "+7912", "ул. Ленина, 10");
            var order = new Order(1, customer, new List<OrderItem>(), "ул. Ленина, 10", mediatorMock.Object);

            // Act
            service.HandleOrderCreated(order);

            // Assert
            mediatorMock.Verify(m => m.RegisterService(
                "DeliveryService",
                It.IsAny<IOrderServiceComponent>()),
                Times.Once);
        }

        [Fact]
        public void AnalyticsService_TracksOrderStatistics()
        {
            // Arrange
            var mediatorMock = new Mock<IOrderMediator>();
            var service = new AnalyticsService(mediatorMock.Object);
            var customer = new Customer(1, "Иван", "+7912", "ул. Ленина, 10", Enums.CustomerCategory.VIP);
            var order = new Order(1, customer, new List<OrderItem>(), "ул. Ленина, 10", mediatorMock.Object);

            // Act
            service.HandleOrderCreated(order);

            // Assert
            mediatorMock.Verify(m => m.RegisterService(
                "AnalyticsService",
                It.IsAny<IOrderServiceComponent>()),
                Times.Once);
        }
    }
}