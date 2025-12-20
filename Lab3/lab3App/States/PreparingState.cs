using DeliverySystem.Models;

namespace DeliverySystem.States
{
    public class PreparingState : OrderState
    {
        public override void Approve(Order order)
            => throw new InvalidOperationException("Невозможно подтвердить: заказ уже готовится");

        public override void Cancel(Order order, string reason = "")
        {
            Console.WriteLine($"Заказ {order.Id} отменен во время приготовления. Причина: {reason}");
            order.ChangeState(new CancelledState());
        }

        public override void CompletePreparation(Order order)
        {
            Console.WriteLine($"Заказ {order.Id} готов к передаче на доставку");
            order.ChangeState(new ReadyForDeliveryState());
        }

        public override void AssignCourier(Order order)
            => throw new InvalidOperationException("Невозможно назначить курьера: заказ еще не готов");

        public override void StartDelivery(Order order)
            => throw new InvalidOperationException("Невозможно начать доставку: заказ еще не готов");

        public override void CompleteDelivery(Order order)
            => throw new InvalidOperationException("Невозможно завершить доставку: заказ еще не готов");

        public override string GetStatus()
        {
            return "Готовится";
        }
    }
}