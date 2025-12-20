using DeliverySystem.Enums;


namespace DeliverySystem.Models
{
    public class Customer
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Phone { get; private set; }
        public string Address { get; private set; }
        public CustomerCategory Category { get; private set; }

        public Customer(int id, string name, string phone, string address, CustomerCategory category = CustomerCategory.Regular)
        {
            Id = id;
            Name = name;
            Phone = phone;
            Address = address;
            Category = category;
        }

        public decimal GetDiscountPercentage()
        {
            if (Category == CustomerCategory.Premium) return 0.05m;
            if (Category == CustomerCategory.VIP) return 0.10m;
            if (Category == CustomerCategory.SuperVIP) return 0.15m;
            return 0.00m;
        }

        public bool HasFreeDelivery()
        {
            return Category == CustomerCategory.VIP || Category == CustomerCategory.SuperVIP;
        }
    }
}