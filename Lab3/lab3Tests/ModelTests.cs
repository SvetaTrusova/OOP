using Xunit;
using Moq;
using DeliverySystem.Models;
using DeliverySystem.Enums;
using DeliverySystem.Interfaces;
using DeliverySystem.Mediators;


namespace DeliverySystem.Tests
{
    public class ModelTests
    {
        [Theory]
        [InlineData(CustomerCategory.Regular, 0.00)]
        [InlineData(CustomerCategory.Premium, 0.05)]
        [InlineData(CustomerCategory.VIP, 0.10)]
        [InlineData(CustomerCategory.SuperVIP, 0.15)]
        public void Customer_GetDiscountPercentage_ReturnsCorrectValue(
            CustomerCategory category, decimal expectedDiscount)
        {
            // Arrange
            var customer = new Customer(1, "Иван", "+7912", "ул. Ленина, 10", category);

            // Act
            var discount = customer.GetDiscountPercentage();

            // Assert
            Assert.Equal(expectedDiscount, discount);
        }

        [Theory]
        [InlineData(CustomerCategory.Regular, false)]
        [InlineData(CustomerCategory.Premium, false)]
        [InlineData(CustomerCategory.VIP, true)]
        [InlineData(CustomerCategory.SuperVIP, true)]
        public void Customer_HasFreeDelivery_ReturnsCorrectValue(
            CustomerCategory category, bool expected)
        {
            // Arrange
            var customer = new Customer(1, "Иван", "+7912", "ул. Ленина, 10", category);

            // Act
            var hasFreeDelivery = customer.HasFreeDelivery();

            // Assert
            Assert.Equal(expected, hasFreeDelivery);
        }

        [Fact]
        public void Order_CalculateSubtotal_ReturnsCorrectSum()
        {
            // Arrange
            var mediatorMock = new Mock<IOrderMediator>();
            var customer = new Customer(1, "Иван", "+7912", "ул. Ленина, 10");
            var dish1 = new Dish(1, "Пицца", "", 500, 15);
            var dish2 = new Dish(2, "Кола", "", 150, 2);

            var items = new List<OrderItem>
            {
                new(dish1, 2),
                new(dish2, 3)
            };

            var order = new Order(1, customer, items, "ул. Ленина, 10", mediatorMock.Object);

            // Act
            var subtotal = order.CalculateSubtotal();

            // Assert
            Assert.Equal(1450m, subtotal);
        }

        [Fact]
        public void OrderItem_GetTotalPrice_ReturnsPriceTimesQuantity()
        {
            // Arrange
            var dish = new Dish(1, "Пицца", "", 500, 15);
            var orderItem = new OrderItem(dish, 3);

            // Act
            var total = orderItem.GetTotalPrice();

            // Assert
            Assert.Equal(1500m, total);
        }

        [Fact]
        public void Dish_Properties_AreSetCorrectly()
        {
            // Arrange & Act
            var dish = new Dish(1, "Пицца", "Вкусная пицца", 799.99m, 20);

            // Assert
            Assert.Equal(1, dish.Id);
            Assert.Equal("Пицца", dish.Name);
            Assert.Equal("Вкусная пицца", dish.Description);
            Assert.Equal(799.99m, dish.Price);
            Assert.Equal(20, dish.PreparationTime);
        }
    }
}