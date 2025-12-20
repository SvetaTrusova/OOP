using DeliverySystem.Models;
using DeliverySystem.Enums;

namespace DeliverySystem.Builders.Interfaces
{
    public interface ICustomerBuilder : IBuilder<Customer>
    {
        ICustomerBuilder SetId(int id);
        ICustomerBuilder SetName(string name);
        ICustomerBuilder SetPhone(string phone);
        ICustomerBuilder SetAddress(string address);
        ICustomerBuilder SetCategory(CustomerCategory category);
        ICustomerBuilder SetPremium();
        ICustomerBuilder SetVIP();
        ICustomerBuilder SetSuperVIP();
    }
}