using DeliverySystem.Interfaces;
using DeliverySystem.Models;


namespace DeliverySystem.Services
{
    public class NotificationService : IOrderServiceComponent
    {
        private IOrderMediator Mediator { get; }

        public NotificationService(IOrderMediator mediator)
        {
            Mediator = mediator;
            Mediator.RegisterService("NotificationService", this);
        }

        public string GetServiceName() => "NotificationService";

        public void HandleOrderCreated(Order order)
        {
            Console.WriteLine($"Сервис уведомлений: Отправка подтверждения для заказа номер {order.Id}");
            SendSMS(order.Customer.Phone,
                $"Ваш заказ номер {order.Id} принят. Спасибо, {order.Customer.Name}!");
        }

        public void HandleOrderStateChanged(Order order, IOrderState oldStatus, IOrderState newStatus)
        {
            if (newStatus.GetStatus() == "Завершен")
            {
                Console.WriteLine($"Сервис уведомлений: Уведомление о завершении заказа номер {order.Id}");
                SendSMS(order.Customer.Phone,
                    $"Ваш заказ номер {order.Id} доставлен. Приятного аппетита!");
            }
            else
            {
                Console.WriteLine($"Сервис уведомлений: Заказ номер {order.Id} изменил статус на {newStatus.GetStatus()}");
                SendSMS(order.Customer.Phone,
                    $"Статус заказа номер {order.Id}: {oldStatus.GetStatus()} -> {newStatus.GetStatus()}");
            }
        }

        private void SendSMS(string phone, string message)
        {
            Console.WriteLine($"SMS на {phone}: {message}");
        }
    }
}