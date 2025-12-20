namespace DeliverySystem.Models
{
    public class OrderItem
    {
        public Dish Dish { get; private set; }
        public int Quantity { get; private set; }
        public string SpecialInstructions { get; private set; }

        public OrderItem(Dish dish, int quantity, string specialInstructions = "")
        {
            Dish = dish;
            Quantity = quantity;
            SpecialInstructions = specialInstructions;
        }

        public decimal GetTotalPrice()
        {
            return Dish.Price * Quantity;
        }
    }
}