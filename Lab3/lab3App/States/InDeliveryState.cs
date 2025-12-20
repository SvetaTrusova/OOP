using DeliverySystem.Models;

namespace DeliverySystem.States
{
    public class InDeliveryState : OrderState
    {
        public override void Approve(Order order)
            => throw new InvalidOperationException("Невозможно подтвердить: заказ уже в доставке");

        public override void Cancel(Order order, string reason = "")
            => throw new InvalidOperationException("Невозможно отменить: заказ уже в доставке");

        public override void CompletePreparation(Order order)
            => throw new InvalidOperationException("Невозможно завершить приготовление: заказ уже в доставке");

        public override void AssignCourier(Order order)
            => throw new InvalidOperationException("Невозможно назначить курьера: курьер уже назначен");

        public override void StartDelivery(Order order)
            => throw new InvalidOperationException("Невозможно начать доставку: доставка уже началась");

        public override void CompleteDelivery(Order order)
        {
            Console.WriteLine($"Заказ {order.Id} доставлен успешно!");
            order.ChangeState(new CompletedState());
        }

        public override string GetStatus()
        {
            return "В доставке";
        }
    }
}