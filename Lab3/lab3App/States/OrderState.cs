using DeliverySystem.Interfaces;
using DeliverySystem.Models;

namespace DeliverySystem.States
{
    public abstract class OrderState : IOrderState
    {
        public abstract void Approve(Order order);
        public abstract void Cancel(Order order, string reason = "");
        public abstract void CompletePreparation(Order order);
        public abstract void AssignCourier(Order order);
        public abstract void StartDelivery(Order order);
        public abstract void CompleteDelivery(Order order);
        public abstract string GetStatus();
    }
}