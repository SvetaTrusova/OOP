using DeliverySystem.Interfaces;
using DeliverySystem.Models;
using DeliverySystem.Enums;
using DeliverySystem.Factories;

namespace DeliverySystem.Services
{
    public class AnalyticsService : IOrderServiceComponent
    {
        private IOrderMediator Mediator { get; }

        public int TotalOrdersProcessed { get; private set; }
        public decimal TotalRevenue { get; private set; }
        public Dictionary<CustomerCategory, int> OrdersByCategory { get; private set; }

        public AnalyticsService(IOrderMediator mediator)
        {
            Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            Mediator.RegisterService("AnalyticsService", this);

            TotalOrdersProcessed = 0;
            TotalRevenue = 0m;
            OrdersByCategory = new Dictionary<CustomerCategory, int>
            {
                { CustomerCategory.Regular, 0 },
                { CustomerCategory.Premium, 0 },
                { CustomerCategory.VIP, 0 },
                { CustomerCategory.SuperVIP, 0 }
            };
        }

        public string GetServiceName() => "AnalyticsService";

        public void HandleOrderCreated(Order order)
        {
            TotalOrdersProcessed++;

            OrdersByCategory[order.Customer.Category] = OrdersByCategory.ContainsKey(order.Customer.Category)
                ? OrdersByCategory[order.Customer.Category] + 1
                : 1;

            Console.WriteLine($"Аналитика: Новый заказ номер {order.Id}. Всего заказов: {TotalOrdersProcessed}");
            Console.WriteLine($"Аналитика: Клиент: {order.Customer.Name}, Категория: {order.Customer.Category}");

            PrintStatistics();
        }

        public void HandleOrderStateChanged(Order order, IOrderState oldStatus, IOrderState newStatus)
        {
            if (newStatus.GetStatus() == "Завершен")
            {
                var calculator = OrderCalculatorFactory.CreateCalculator(order);
                var orderTotal = calculator.CalculateTotal(order);

                TotalRevenue += orderTotal;
                Console.WriteLine($"Аналитика: Заказ номер {order.Id} завершен. Общая выручка: {TotalRevenue:F2} рублей");

                var processingTime = DateTime.Now - order.OrderTime;
                Console.WriteLine($"Аналитика: Время обработки заказа: {processingTime.TotalMinutes:F1} минут");

                Console.WriteLine($"Аналитика: Сумма заказа: {orderTotal:F2} рублей, " +
                            $"Быстрая доставка: {(order.IsFastDelivery ? "Да" : "Нет")}");

                PrintStatistics();
            }
        }

        public int GetOrdersCountByCategory(CustomerCategory category)
        {
            return OrdersByCategory.ContainsKey(category) ? OrdersByCategory[category] : 0;
        }

        private void PrintStatistics()
        {
            Console.WriteLine("Статистика заказов по категориям клиентов:");
            foreach (var category in OrdersByCategory)
            {
                Console.WriteLine($"    {category.Key}: {category.Value} заказов");
            }
            Console.WriteLine($"Общее количество заказов: {TotalOrdersProcessed}");
            Console.WriteLine($"Общая выручка: {TotalRevenue:F2} рублей");
        }
    }
}