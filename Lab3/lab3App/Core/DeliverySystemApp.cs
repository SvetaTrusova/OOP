using DeliverySystem.Builders;
using DeliverySystem.Builders.Interfaces;
using DeliverySystem.Enums;
using DeliverySystem.Factories;
using DeliverySystem.Interfaces;
using DeliverySystem.Mediators;
using DeliverySystem.Models;
using DeliverySystem.Services;

namespace DeliverySystem.Core
{
    public class DeliverySystemApp
    {
        private IOrderMediator Mediator { get; }
        private ICustomerBuilder CustomerBuilder { get; }
        private IDishBuilder DishBuilder { get; }

        public List<Customer> Customers { get; private set; }
        public List<Dish> Menu { get; private set; }
        public List<Order> Orders { get; private set; }

        public DeliverySystemApp(
            IOrderMediator mediator,
            ICustomerBuilder customerBuilder,
            IDishBuilder dishBuilder)
        {
            Mediator = mediator ?? new OrderProcessingMediator();
            CustomerBuilder = customerBuilder ?? new CustomerBuilder();
            DishBuilder = dishBuilder ?? new DishBuilder();

            Customers = new List<Customer>();
            Menu = new List<Dish>();
            Orders = new List<Order>();

            RegisterServices();
        }

        private void RegisterServices()
        {
            var notificationService = new NotificationService(Mediator);
            var kitchenService = new KitchenService(Mediator);
            var deliveryService = new DeliveryService(Mediator);
            var analyticsService = new AnalyticsService(Mediator);
        }


        public Customer CreateCustomer(string name, string phone, string address, CustomerCategory category = CustomerCategory.Regular)
        {
            var customer = CustomerBuilder
                .SetName(name)
                .SetPhone(phone)
                .SetAddress(address)
                .SetCategory(category)
                .Build();

            Customers.Add(customer);
            return customer;
        }

        public Customer FindCustomer(int id)
        {
            return Customers.Find(c => c.Id == id);
        }

        public List<Customer> GetCustomersByCategory(CustomerCategory category)
        {
            return Customers.Where(c => c.Category == category).ToList();
        }

        public Dish CreateDish(string name, string description, decimal price, int preparationTime)
        {
            var dish = DishBuilder
                .SetName(name)
                .SetDescription(description)
                .SetPrice(price)
                .SetPreparationTime(preparationTime)
                .Build();

            Menu.Add(dish);
            return dish;
        }

        public Dish FindDish(int id)
        {
            return Menu.Find(d => d.Id == id);
        }

        public Order CreateOrder(int customerId, string deliveryAddress,
            bool isFastDelivery = false, string specialPreferences = "",
            List<OrderItem> items = null)
        {
            var customer = FindCustomer(customerId);
            if (customer == null)
                throw new ArgumentException($"Клиент с ID {customerId} не найден", nameof(customerId));

            var orderBuilder = new OrderBuilder()
                .SetCustomer(customer)
                .SetDeliveryAddress(deliveryAddress)
                .SetFastDelivery(isFastDelivery)
                .SetSpecialPreferences(specialPreferences)
                .SetMediator(Mediator);

            if (items != null && items.Any())
            {
                foreach (var item in items)
                {
                    orderBuilder.AddItem(item);
                }
            }
            else
            {
                throw new ArgumentException("Заказ должен содержать хотя бы одно блюдо");
            }

            var order = orderBuilder.Build();
            Orders.Add(order);
            return order;
        }

        public Order FindOrder(int id)
        {
            return Orders.Find(o => o.Id == id);
        }

        public List<Order> GetCustomerOrders(int customerId)
        {
            return Orders.Where(o => o.Customer.Id == customerId).ToList();
        }

        public List<Order> GetActiveOrders()
        {
            return Orders.Where(o => o.GetStatus() != "Выполнен" && o.GetStatus() != "Отменен").ToList();
        }

        public List<Order> GetOrdersByStatus(string status)
        {
            return Orders.Where(o => o.GetStatus() == status).ToList();
        }

        public void ApproveOrder(int orderId)
        {
            var order = FindOrder(orderId);
            if (order == null)
                throw new ArgumentException($"Заказ с ID {orderId} не найден", nameof(orderId));

            order.Approve();
        }

        public void CancelOrder(int orderId, string reason = "По желанию клиента")
        {
            var order = FindOrder(orderId);
            if (order == null)
                throw new ArgumentException($"Заказ с ID {orderId} не найден", nameof(orderId));

            order.Cancel();
        }

        public void CompletePreparation(int orderId)
        {
            var order = FindOrder(orderId);
            if (order == null)
                throw new ArgumentException($"Заказ с ID {orderId} не найден", nameof(orderId));

            order.CompletePreparation();
        }

        public void AssignCourier(int orderId)
        {
            var order = FindOrder(orderId);
            if (order == null)
                throw new ArgumentException($"Заказ с ID {orderId} не найден", nameof(orderId));

            order.AssignCourier();
        }

        public void StartDelivery(int orderId)
        {
            var order = FindOrder(orderId);
            if (order == null)
                throw new ArgumentException($"Заказ с ID {orderId} не найден", nameof(orderId));

            order.StartDelivery();
        }

        public void CompleteDelivery(int orderId)
        {
            var order = FindOrder(orderId);
            if (order == null)
                throw new ArgumentException($"Заказ с ID {orderId} не найден", nameof(orderId));

            order.CompleteDelivery();
        }

        public string GetOrderStatus(int orderId)
        {
            var order = FindOrder(orderId);
            return order?.GetStatus() ?? "Заказ не найден";
        }

        public decimal CalculateOrderTotal(int orderId)
        {
            var order = FindOrder(orderId);
            if (order == null)
                throw new ArgumentException($"Заказ с ID {orderId} не найден", nameof(orderId));

            var calculator = OrderCalculatorFactory.CreateCalculator(order);
            return calculator.CalculateTotal(order);
        }

        public int GetOrdersCount() => Orders.Count;

        public int GetCompletedOrdersCount() => Orders.Count(o => o.GetStatus() == "Выполнен");

        public int GetActiveOrdersCount() => Orders.Count(o => o.GetStatus() != "Выполнен" && o.GetStatus() != "Отменен");

        public bool CanHaveFreeDelivery(int customerId)
        {
            var customer = FindCustomer(customerId);
            return customer?.HasFreeDelivery() ?? false;
        }

        public decimal GetCustomerDiscountPercentage(int customerId)
        {
            var customer = FindCustomer(customerId);
            return customer?.GetDiscountPercentage() ?? 0;
        }

        public void ClearAllData()
        {
            Customers.Clear();
            Menu.Clear();
            Orders.Clear();
        }
    }
}