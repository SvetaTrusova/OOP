using DeliverySystem.Models;

namespace DeliverySystem.Strategies
{
    public class StandardDeliveryStrategy : BaseOrderCalculationStrategy
    {
        private const decimal StandardDeliveryPrice = 100.0m;
        private const decimal FreeDeliveryThreshold = 3000.0m;

        protected override decimal CalculateDeliveryPrice(Order order)
        {
            return CalculateDeliveryPriceCore(order, StandardDeliveryPrice);
        }

        protected override bool IsFreeDeliveryEligible(Order order)
        {
            if (CheckFreeDeliveryForCustomer(order))
            {
                return true;
            }

            if (CheckFreeDeliveryByThreshold(order, FreeDeliveryThreshold))
            {
                return true;
            }

            return false;
        }
    }
}