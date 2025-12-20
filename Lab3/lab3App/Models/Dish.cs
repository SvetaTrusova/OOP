namespace DeliverySystem.Models
{
    public class Dish
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public int PreparationTime { get; private set; }
        public string Description { get; private set; }

        public Dish(int id, string name, string description, decimal price, int preparationTime)
        {
            Id = id;
            Name = name;
            Price = price;
            PreparationTime = preparationTime;
            Description = description;
        }
    }
}