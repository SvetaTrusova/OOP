using DeliverySystem.Interfaces;
using DeliverySystem.Models;
using DeliverySystem.Strategies;

namespace DeliverySystem.Factories
{
    public static class OrderCalculatorFactory
    {
        public static IOrderCalculationStrategy CreateCalculator(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            if (order.IsFastDelivery)
            {
                return new ExpressDeliveryStrategy();
            }
            else
            {
                return new StandardDeliveryStrategy();
            }
        }
    }
}