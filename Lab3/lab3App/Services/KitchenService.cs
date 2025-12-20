using DeliverySystem.Interfaces;
using DeliverySystem.Models;

namespace DeliverySystem.Services
{
    public class KitchenService : IOrderServiceComponent
    {
        private IOrderMediator Mediator { get; }

        public KitchenService(IOrderMediator mediator)
        {
            Mediator = mediator;
            Mediator.RegisterService("KitchenService", this);
        }

        public string GetServiceName() => "KitchenService";

        public void HandleOrderCreated(Order order)
        {
            Console.WriteLine($"Кухня: Начало приготовления заказа номер {order.Id}");

            var preparationTime = order.Items.Sum(i => i.Dish.PreparationTime);
            Console.WriteLine($"Кухня: ** Прошло {preparationTime} минут **");

            Console.WriteLine($"Кухня: Заказ номер {order.Id} готов");
        }

        public void HandleOrderStateChanged(Order order, IOrderState oldStatus, IOrderState newStatus)
        {
            if (newStatus.GetStatus() == "В доставке")
            {
                Console.WriteLine($"Кухня: Заказ номер {order.Id} передан на доставку");
            }
        }
    }
}