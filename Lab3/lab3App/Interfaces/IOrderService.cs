using DeliverySystem.Models;

namespace DeliverySystem.Interfaces
{
    public interface IOrderService
    {
        Order CreateOrder(Order order);
        Order GetOrder(int orderId);
        void UpdateOrderState(int orderId, IOrderState newState);
        decimal CalculateOrderTotal(int orderId);
    }
}