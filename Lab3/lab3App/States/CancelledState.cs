using DeliverySystem.Models;

namespace DeliverySystem.States
{
    public class CancelledState : OrderState
    {
        public override void Approve(Order order)
            => throw new InvalidOperationException("Невозможно подтвердить: заказ отменен");

        public override void Cancel(Order order, string reason = "")
            => throw new InvalidOperationException("Невозможно отменить: заказ уже отменен");

        public override void CompletePreparation(Order order)
            => throw new InvalidOperationException("Невозможно завершить приготовление: заказ отменен");

        public override void AssignCourier(Order order)
            => throw new InvalidOperationException("Невозможно назначить курьера: заказ отменен");

        public override void StartDelivery(Order order)
            => throw new InvalidOperationException("Невозможно начать доставку: заказ отменен");

        public override void CompleteDelivery(Order order)
            => throw new InvalidOperationException("Невозможно завершить доставку: заказ отменен");

        public override string GetStatus()
        {
            return "Отменен";
        }
    }
}