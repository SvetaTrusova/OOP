using DeliverySystem.Interfaces;
using DeliverySystem.Models;
using DeliverySystem.States;
using DeliverySystem.Enums;


namespace DeliverySystem.Mediators
{
    public class OrderProcessingMediator : IOrderMediator
    {
        public Dictionary<string, IOrderServiceComponent> Services { get; private set; }
        public Dictionary<string, List<string>> StatusHandlers { get; private set; }

        public OrderProcessingMediator()
        {
            Services = new Dictionary<string, IOrderServiceComponent>();
            StatusHandlers = InitializeStatusHandlers();
        }

        public void RegisterService(string serviceName, IOrderServiceComponent component)
        {
            if (Services.ContainsKey(serviceName))
            {
                throw new InvalidOperationException($"Сервис '{serviceName}' уже зарегистрирован");
            }

            Services[serviceName] = component;
            Console.WriteLine($"Медиатор: Сервис '{serviceName}' зарегистрирован");
        }

        public void NotifyOrderCreated(Order order)
        {
            ValidateOrder(order);

            Console.WriteLine($"Медиатор: Создан заказ номер {order.Id} для {order.Customer.Name}");

            var processingChain = GetProcessingChainForCustomer(order.Customer.Category);

            foreach (var serviceName in processingChain)
            {
                if (Services.ContainsKey(serviceName))
                {
                    var service = Services[serviceName];
                    service.HandleOrderCreated(order);
                }
                else
                {
                    throw new InvalidOperationException($"Сервис '{serviceName}' не найден");
                }
            }

            Console.WriteLine($"Медиатор: Заказ номер {order.Id} принят в обработку");
        }

        public void NotifyOrderStateChanged(Order order, IOrderState oldState, IOrderState newState)
        {
            ValidateOrder(order);

            var oldStatus = oldState?.GetStatus() ?? "Неизвестно";
            var newStatus = newState?.GetStatus() ?? "Неизвестно";

            Console.WriteLine($"Медиатор: Состояние заказа номер {order.Id} изменено: {oldStatus} -> {newStatus}");

            var servicesToNotify = GetServicesForStatusChange(newStatus);

            foreach (var serviceName in servicesToNotify)
            {
                if (Services.ContainsKey(serviceName))
                {
                    var service = Services[serviceName];
                    service.HandleOrderStateChanged(order, oldState, newState);
                }
            }

            if (newStatus == "Выполнен")
            {
                ProcessCompletedOrder(order);
            }
        }

        private Dictionary<string, List<string>> InitializeStatusHandlers()
        {
            return new Dictionary<string, List<string>>
            {
                ["Ожидает подтверждения"] = new List<string> { "NotificationService" },
                ["Готовится"] = new List<string> { "NotificationService", "KitchenService" },
                ["Готов к доставке"] = new List<string> { "NotificationService", "KitchenService", "DeliveryService" },
                ["Курьер назначен"] = new List<string> { "NotificationService", "DeliveryService" },
                ["В доставке"] = new List<string> { "NotificationService", "DeliveryService" },
                ["Выполнен"] = new List<string> { "NotificationService", "DeliveryService", "AnalyticsService" },
                ["Отменен"] = new List<string> { "NotificationService", "AnalyticsService", "KitchenService", "DeliveryService" }
            };
        }

        private List<string> GetProcessingChainForCustomer(CustomerCategory category)
        {
            var baseChain = new List<string>
            {
                "NotificationService",
                "KitchenService",
                "DeliveryService"
            };

            if (category == CustomerCategory.VIP ||
                category == CustomerCategory.SuperVIP ||
                category == CustomerCategory.Premium)
            {
                if (!baseChain.Contains("AnalyticsService"))
                    baseChain.Add("AnalyticsService");
            }

            return baseChain;
        }

        private List<string> GetServicesForStatusChange(string newStatus)
        {
            if (StatusHandlers.ContainsKey(newStatus))
            {
                return StatusHandlers[newStatus];
            }

            return new List<string> { "NotificationService" };
        }

        private void ProcessCompletedOrder(Order order)
        {
            Console.WriteLine($"Медиатор: Заказ номер {order.Id} завершен. Секундочку...");

            if (Services.ContainsKey("AnalyticsService"))
            {
                Console.WriteLine($"Медиатор: Аналитика для заказа номер {order.Id} обновлена");
            }
        }

        private void ValidateOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order), "Заказ не может быть null");

            if (order.Customer == null)
                throw new ArgumentException("Заказ должен иметь клиента");
        }
    }
}