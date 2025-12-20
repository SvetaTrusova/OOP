using DeliverySystem.Models;

namespace DeliverySystem.Interfaces
{
    public interface IOrderServiceComponent
    {
        void HandleOrderCreated(Order order);
        void HandleOrderStateChanged(Order order, IOrderState oldStatus, IOrderState newStatus);
        string GetServiceName();
    }
}