using DeliverySystem.Models;
using DeliverySystem.Enums;

namespace DeliverySystem.Strategies
{
    public class ExpressDeliveryStrategy : BaseOrderCalculationStrategy
    {
        private const decimal ExpressDeliveryPrice = 500.0m;
        private const decimal ExpressFreeDeliveryThreshold = 5000.0m;
        private const decimal ServiceChargeRate = 0.02m;


        protected override decimal CalculateAdditionalCharges(Order order, decimal subtotal)
        {
            decimal serviceCharge = subtotal * ServiceChargeRate;

            return serviceCharge;
        }

        protected override decimal CalculateDeliveryPrice(Order order)
        {
            return CalculateDeliveryPriceCore(order, ExpressDeliveryPrice);
        }

        protected override bool IsFreeDeliveryEligible(Order order)
        {
            if (CheckFreeDeliveryForCustomer(order) &&
                order.Customer.Category == CustomerCategory.SuperVIP)
            {
                return true;
            }

            if (CheckFreeDeliveryByThreshold(order, ExpressFreeDeliveryThreshold))
            {
                return true;
            }

            return false;
        }
    }
}