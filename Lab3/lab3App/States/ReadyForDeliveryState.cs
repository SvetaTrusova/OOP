using DeliverySystem.Models;

namespace DeliverySystem.States
{
    public class ReadyForDeliveryState : OrderState
    {
        public override void Approve(Order order)
            => throw new InvalidOperationException("Невозможно подтвердить: заказ уже готов к доставке");

        public override void Cancel(Order order, string reason = "")
        {
            Console.WriteLine($"Заказ {order.Id} отменен перед доставкой. Причина: {reason}");
            order.ChangeState(new CancelledState());
        }

        public override void CompletePreparation(Order order)
            => throw new InvalidOperationException("Невозможно завершить приготовление: заказ уже готов к доставке");

        public override void AssignCourier(Order order)
        {
            Console.WriteLine($"Курьер назначен для заказа {order.Id}");
            order.ChangeState(new CourierAssignedState());
        }

        public override void StartDelivery(Order order)
            => throw new InvalidOperationException("Невозможно начать доставку: сначала назначьте курьера");

        public override void CompleteDelivery(Order order)
            => throw new InvalidOperationException("Невозможно завершить доставку: доставка еще не началась");

        public override string GetStatus()
        {
            return "Готов к доставке";
        }
    }
}