using DeliverySystem.Models;

namespace DeliverySystem.States
{
    public class PendingState : OrderState
    {
        public override void Approve(Order order)
        {
            order.ChangeState(new PreparingState());
        }

        public override void Cancel(Order order, string reason = "")
        {
            Console.WriteLine($"Заказ {order.Id} отменен. Причина: {reason}");
            order.ChangeState(new CancelledState());
        }

        public override void CompletePreparation(Order order)
            => throw new InvalidOperationException("Невозможно завершить приготовление: заказ еще не подтвержден");

        public override void AssignCourier(Order order)
            => throw new InvalidOperationException("Невозможно назначить курьера: заказ еще не подтвержден");

        public override void StartDelivery(Order order)
            => throw new InvalidOperationException("Невозможно начать доставку: заказ еще не подтвержден");

        public override void CompleteDelivery(Order order)
            => throw new InvalidOperationException("Невозможно завершить доставку: заказ еще не подтвержден");

        public override string GetStatus()
        {
            return "Ожидает подтверждения";
        }
    }
}