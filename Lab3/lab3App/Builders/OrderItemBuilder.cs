using DeliverySystem.Models;
using DeliverySystem.Builders.Interfaces;

namespace DeliverySystem.Builders
{
    public class OrderItemBuilder : IOrderItemBuilder
    {
        private Dish _dish;
        private int _quantity = 1;
        private string _specialInstructions;
        private bool _isSpicy = false;
        private bool _extraCheese = false;
        private bool _withoutAllergens = false;
        private bool _extraSauce = false;
        private string _portionSize = "standard";

        public IOrderItemBuilder SetDish(Dish dish)
        {
            _dish = dish ?? throw new ArgumentNullException(nameof(dish));
            return this;
        }

        public IOrderItemBuilder SetQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Количество должно быть положительным");
            _quantity = quantity;
            return this;
        }

        public IOrderItemBuilder SetInstructions(string instructions)
        {
            _specialInstructions = instructions ?? "";
            return this;
        }

        public IOrderItemBuilder MakeSpicy()
        {
            _isSpicy = true;
            return this;
        }

        public IOrderItemBuilder AddExtraCheese()
        {
            _extraCheese = true;
            return this;
        }

        public IOrderItemBuilder WithoutAllergens()
        {
            _withoutAllergens = true;
            return this;
        }

        public IOrderItemBuilder AddExtraSauce()
        {
            _extraSauce = true;
            return this;
        }

        public IOrderItemBuilder MakePortionSmall()
        {
            _portionSize = "small";
            return this;
        }

        public IOrderItemBuilder MakePortionLarge()
        {
            _portionSize = "large";
            return this;
        }


        public OrderItem Build()
        {
            if (!CanBuild())
                throw new InvalidOperationException("Невозможно создать элемент заказа. Блюдо не указано.");

            var instructions = BuildFullInstructions();

            return new OrderItem(_dish, _quantity, instructions);
        }

        public bool CanBuild()
        {
            return _dish != null && _quantity > 0;
        }

        private string BuildFullInstructions()
        {
            var instructions = new List<string>();

            if (!string.IsNullOrWhiteSpace(_specialInstructions))
                instructions.Add(_specialInstructions);

            if (_isSpicy)
                instructions.Add("Острое");

            if (_extraCheese)
                instructions.Add("Дополнительный сыр");

            if (_withoutAllergens)
                instructions.Add("Без аллергенов");

            if (_extraSauce)
                instructions.Add("Дополнительный соус");

            if (_portionSize != "standard")
                instructions.Add($"Размер порции: {_portionSize}");

            return string.Join(", ", instructions);
        }
    }
}