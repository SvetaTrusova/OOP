using DeliverySystem.Models;

namespace DeliverySystem.States
{
    public class CourierAssignedState : OrderState
    {
        public override void Approve(Order order)
            => throw new InvalidOperationException("Невозможно подтвердить: курьер уже назначен");

        public override void Cancel(Order order, string reason = "")
        {
            Console.WriteLine($"Заказ {order.Id} отменен после назначения курьера. Причина: {reason}");
            order.ChangeState(new CancelledState());
        }

        public override void CompletePreparation(Order order)
            => throw new InvalidOperationException("Невозможно завершить приготовление: заказ уже готов");

        public override void AssignCourier(Order order)
            => throw new InvalidOperationException("Невозможно назначить курьера: курьер уже назначен");

        public override void StartDelivery(Order order)
        {
            Console.WriteLine($"Курьер начал доставку заказа {order.Id}");
            order.ChangeState(new InDeliveryState());
        }

        public override void CompleteDelivery(Order order)
            => throw new InvalidOperationException("Невозможно завершить доставку: доставка еще не началась");

        public override string GetStatus()
        {
            return "Курьер назначен";
        }
    }
}