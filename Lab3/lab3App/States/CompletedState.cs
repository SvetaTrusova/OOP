using DeliverySystem.Models;

namespace DeliverySystem.States
{
    public class CompletedState : OrderState
    {
        public override void Approve(Order order)
            => throw new InvalidOperationException("Невозможно подтвердить: заказ уже выполнен");

        public override void Cancel(Order order, string reason = "")
            => throw new InvalidOperationException("Невозможно отменить: заказ уже выполнен");

        public override void CompletePreparation(Order order)
            => throw new InvalidOperationException("Невозможно завершить приготовление: заказ уже выполнен");

        public override void AssignCourier(Order order)
            => throw new InvalidOperationException("Невозможно назначить курьера: заказ уже выполнен");

        public override void StartDelivery(Order order)
            => throw new InvalidOperationException("Невозможно начать доставку: заказ уже выполнен");

        public override void CompleteDelivery(Order order)
            => throw new InvalidOperationException("Невозможно завершить доставку: заказ уже выполнен");

        public override string GetStatus()
        {
            return "Выполнен";
        }
    }
}