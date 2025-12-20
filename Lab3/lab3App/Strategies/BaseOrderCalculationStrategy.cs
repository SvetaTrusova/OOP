using DeliverySystem.Interfaces;
using DeliverySystem.Models;

namespace DeliverySystem.Strategies
{
    public abstract class BaseOrderCalculationStrategy : IOrderCalculationStrategy
    {
        protected const decimal DefaultTaxRate = 0.1m;

        public virtual decimal CalculateTotal(Order order)
        {
            ValidateOrder(order);

            decimal subtotal = CalculateSubtotal(order);
            decimal tax = CalculateTax(subtotal);
            decimal deliveryPrice = CalculateDeliveryPrice(order);
            decimal customerDiscount = CalculateCustomerDiscount(order, subtotal);

            decimal additionalCharges = CalculateAdditionalCharges(order, subtotal);
            decimal additionalDiscounts = CalculateAdditionalDiscounts(order, subtotal);

            return subtotal + tax + deliveryPrice + additionalCharges
                   - customerDiscount - additionalDiscounts;
        }

        protected abstract decimal CalculateDeliveryPrice(Order order);

        protected abstract bool IsFreeDeliveryEligible(Order order);

        protected virtual decimal CalculateSubtotal(Order order)
            => order.CalculateSubtotal();

        protected virtual decimal CalculateTax(decimal subtotal)
            => subtotal * DefaultTaxRate;

        protected virtual decimal CalculateCustomerDiscount(Order order, decimal subtotal)
            => subtotal * order.Customer.GetDiscountPercentage();

        protected virtual decimal CalculateAdditionalCharges(Order order, decimal subtotal) => 0m;

        protected virtual decimal CalculateAdditionalDiscounts(Order order, decimal subtotal) => 0m;


        protected decimal CalculateDeliveryPriceCore(Order order, decimal baseDeliveryPrice)
        {
            return IsFreeDeliveryEligible(order) ? 0m : baseDeliveryPrice;
        }

        protected virtual void ValidateOrder(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            if (order.Customer == null)
            {
                throw new InvalidOperationException("У заказа должен быть покупатель");
            }

            if (order.Items == null || !order.Items.Any())
            {
                throw new InvalidOperationException("В заказе должен быть хотя бы один продукт");
            }
        }

        protected virtual bool CheckFreeDeliveryByThreshold(Order order, decimal threshold)
        {
            decimal subtotal = CalculateSubtotal(order);
            return subtotal >= threshold;
        }

        protected virtual bool CheckFreeDeliveryForCustomer(Order order)
        {
            return order.Customer.HasFreeDelivery();
        }
    }
}