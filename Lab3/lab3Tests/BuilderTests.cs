using Xunit;
using Moq;
using DeliverySystem.Builders;
using DeliverySystem.Models;
using DeliverySystem.Enums;
using DeliverySystem.Interfaces;
using DeliverySystem.Mediators;


namespace DeliverySystem.Tests
{
    public class BuilderTests
    {
        [Fact]
        public void CustomerBuilder_BuildValidCustomer_Success()
        {
            // Arrange & Act
            var customer = new CustomerBuilder()
                .SetName("Иван Иванов")
                .SetPhone("+79123456789")
                .SetAddress("ул. Ленина, 10, кв. 5")
                .SetPremium()
                .Build();

            // Assert
            Assert.NotNull(customer);
            Assert.Equal("Иван Иванов", customer.Name);
            Assert.Equal(CustomerCategory.Premium, customer.Category);
            Assert.True(customer.Id > 0);
        }

        [Fact]
        public void CustomerBuilder_BuildWithoutName_ThrowsException()
        {
            // Arrange
            var builder = new CustomerBuilder()
                .SetPhone("+79123456789")
                .SetAddress("ул. Ленина, 10");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }

        [Fact]
        public void DishBuilder_BuildValidDish_Success()
        {
            // Arrange & Act
            var dish = new DishBuilder()
                .SetName("Пицца Пепперони")
                .SetDescription("Острая пицца")
                .SetPrice(799.99m)
                .SetPreparationTime(20)
                .Build();

            // Assert
            Assert.NotNull(dish);
            Assert.Equal("Пицца Пепперони", dish.Name);
            Assert.Equal(799.99m, dish.Price);
            Assert.True(dish.Id > 0);
        }

        [Fact]
        public void DishBuilder_SetNegativePrice_ThrowsException()
        {
            // Arrange
            var builder = new DishBuilder();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => builder.SetPrice(-100));
        }

        [Fact]
        public void OrderBuilder_BuildValidOrder_Success()
        {
            // Arrange
            var mediatorMock = new Mock<IOrderMediator>();
            var customer = new Customer(1, "Иван", "+7912", "ул. Ленина, 10");
            var dish = new Dish(1, "Пицца", "", 500, 15);

            // Act
            var order = new OrderBuilder()
                .SetCustomer(customer)
                .SetDeliveryAddress("ул. Пушкина, 20")
                .SetMediator(mediatorMock.Object)
                .AddDish(dish, 2, "Без перца")
                .Build();

            // Assert
            Assert.NotNull(order);
            Assert.Equal(customer, order.Customer);
            Assert.Equal(2, order.Items[0].Quantity);
        }

        [Fact]
        public void OrderItemBuilder_BuildWithModifications_Success()
        {
            // Arrange
            var dish = new Dish(1, "Пицца", "", 500, 15);

            // Act
            var orderItem = new OrderItemBuilder()
                .SetDish(dish)
                .SetQuantity(2)
                .MakeSpicy()
                .AddExtraCheese()
                .Build();

            // Assert
            Assert.NotNull(orderItem);
            Assert.Equal(dish, orderItem.Dish);
            Assert.Equal(2, orderItem.Quantity);
            Assert.Contains("Острое", orderItem.SpecialInstructions);
            Assert.Contains("Дополнительный сыр", orderItem.SpecialInstructions);
        }
    }
}