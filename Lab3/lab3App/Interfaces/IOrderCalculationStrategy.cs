using DeliverySystem.Models;

namespace DeliverySystem.Builders.Interfaces
{
    public interface IOrderItemBuilder : IBuilder<OrderItem>
    {
        IOrderItemBuilder SetDish(Dish dish);
        IOrderItemBuilder SetQuantity(int quantity);
        IOrderItemBuilder SetInstructions(string instructions);

        IOrderItemBuilder MakeSpicy();
        IOrderItemBuilder AddExtraCheese();
        IOrderItemBuilder WithoutAllergens();
        IOrderItemBuilder AddExtraSauce();
        IOrderItemBuilder MakePortionSmall();
        IOrderItemBuilder MakePortionLarge();
    }
}