using DeliverySystem.Models;
using DeliverySystem.Interfaces;


namespace DeliverySystem.Interfaces
{
    public interface IOrderMediator
    {
        void RegisterService(string serviceName, IOrderServiceComponent component);
        void NotifyOrderCreated(Order order);
        void NotifyOrderStateChanged(Order order, IOrderState oldState, IOrderState newState);
    }
}