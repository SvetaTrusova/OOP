using DeliverySystem.Interfaces;
using DeliverySystem.Models;

namespace DeliverySystem.Services
{
    public class DeliveryService : IOrderServiceComponent
    {
        private IOrderMediator Mediator { get; }

        public DeliveryService(IOrderMediator mediator)
        {
            Mediator = mediator;
            Mediator.RegisterService("DeliveryService", this);
        }

        public string GetServiceName() => "DeliveryService";

        public void HandleOrderCreated(Order order)
        {
            Console.WriteLine($"Служба доставки: Поиск курьера для заказа номер {order.Id}");

            if (order.Customer.HasFreeDelivery())
            {
                Console.WriteLine($"Служба доставки: Приоритетный курьер c беслпатной доставкой назначен для VIP заказа номер {order.Id}");
            }
            else
            {
                Console.WriteLine($"Служба доставки: ** Прошло n-ое кол-во времени **");
                Console.WriteLine($"Служба доставки: Курьер найден для заказа номер {order.Id}");
            }
        }

        public void HandleOrderStateChanged(Order order, IOrderState oldStatus, IOrderState newStatus)
        {
            if (newStatus.GetStatus() == "В доставке")
            {
                Console.WriteLine($"Служба доставки: Курьер забрал заказ номер {order.Id}");

                Console.WriteLine($"Служба доставки: ** Прошло n-ое кол-во времени **");
                Console.WriteLine($"Служба доставки: Заказ номер {order.Id} доставлен");
            }
        }
    }
}