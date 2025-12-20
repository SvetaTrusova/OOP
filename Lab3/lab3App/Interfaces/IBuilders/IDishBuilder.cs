using DeliverySystem.Models;

namespace DeliverySystem.Builders.Interfaces
{
    public interface IDishBuilder : IBuilder<Dish>
    {
        IDishBuilder SetId(int id);
        IDishBuilder SetName(string name);
        IDishBuilder SetDescription(string description);
        IDishBuilder SetPrice(decimal price);
        IDishBuilder SetPreparationTime(int minutes);
    }
}