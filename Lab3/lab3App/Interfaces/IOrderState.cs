using DeliverySystem.Models;

namespace DeliverySystem.Interfaces
{
    public interface IOrderState
    {
        void Approve(Order order);
        void Cancel(Order order, string reason = "");
        void CompletePreparation(Order order);
        void AssignCourier(Order order);
        void StartDelivery(Order order);
        void CompleteDelivery(Order order);
        string GetStatus();
    }
}