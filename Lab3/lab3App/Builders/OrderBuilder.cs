using DeliverySystem.Models;
using DeliverySystem.States;
using DeliverySystem.Builders.Interfaces;
using DeliverySystem.Interfaces;
using DeliverySystem.Mediators;

namespace DeliverySystem.Builders
{
    public class OrderBuilder : IOrderBuilder
    {
        private Customer _customer;
        private string _deliveryAddress;
        private bool _isFastDelivery = false;
        private string _specialPreferences = "";
        private readonly List<OrderItem> _items = new();
        private IOrderMediator _mediator;
        private static int _lastOrderId = 0;

        public IOrderBuilder SetCustomer(Customer customer)
        {
            _customer = customer ?? throw new ArgumentNullException(nameof(customer), "Клиент не может быть null");
            return this;
        }

        public IOrderBuilder SetDeliveryAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Адрес доставки не может быть пустым");

            _deliveryAddress = address.Trim();
            return this;
        }

        public IOrderBuilder SetFastDelivery(bool isFastDelivery = true)
        {
            _isFastDelivery = isFastDelivery;

            if (isFastDelivery && _items.Count > 5)
            {
                Console.WriteLine("Внимание: Быстрая доставка для большого заказа может быть ограничена");
            }

            return this;
        }

        public IOrderBuilder SetSpecialPreferences(string preferences)
        {
            _specialPreferences = preferences ?? "";
            return this;
        }

        public IOrderBuilder SetMediator(IOrderMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            return this;
        }

        public IOrderBuilder AddItem(OrderItem item)
        {
            _items.Add(item ?? throw new ArgumentNullException(nameof(item), "Элемент заказа не может быть null"));
            return this;
        }

        public IOrderBuilder AddDish(Dish dish, int quantity, string instructions = "")
        {
            if (dish == null)
                throw new ArgumentNullException(nameof(dish), "Блюдо не может быть null");
            if (quantity <= 0)
                throw new ArgumentException("Количество должно быть положительным числом", nameof(quantity));

            _items.Add(new OrderItem(dish, quantity, instructions));
            return this;
        }

        public bool Validate()
        {
            if (_customer == null)
                throw new InvalidOperationException("Не указан клиент");

            if (string.IsNullOrWhiteSpace(_deliveryAddress))
                throw new InvalidOperationException("Не указан адрес доставки");

            if (_mediator == null)
                throw new InvalidOperationException("Не указан медиатор");

            if (_items.Count == 0)
                throw new InvalidOperationException("Заказ не содержит блюд");

            ValidateDeliveryAddress();
            ValidateItems();

            return true;
        }

        private void ValidateDeliveryAddress()
        {
            if (_deliveryAddress.Length < 5)
                throw new InvalidOperationException("Адрес доставки слишком короткий");
        }

        private void ValidateItems()
        {
            foreach (var item in _items)
            {
                if (item.Quantity > 100)
                    throw new InvalidOperationException($"Слишком большое количество для блюда '{item.Dish.Name}': {item.Quantity}");

                if (item.Dish.Price <= 0)
                    throw new InvalidOperationException($"Некорректная цена для блюда '{item.Dish.Name}'");
            }
        }

        public Order Build()
        {
            Validate();

            int orderId = GenerateOrderId();
            var order = new Order(orderId, _customer, _items, _deliveryAddress,
                                 _mediator, _isFastDelivery, _specialPreferences);

            return order;
        }

        public bool CanBuild()
        {
            return _customer != null &&
                    !string.IsNullOrWhiteSpace(_deliveryAddress) &&
                    _mediator != null &&
                    _items.Count > 0;
        }

        private int GenerateOrderId()
        {
            return ++_lastOrderId;
        }
    }
}