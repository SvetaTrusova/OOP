using DeliverySystem.Models;
using DeliverySystem.Enums;
using DeliverySystem.Builders.Interfaces;

namespace DeliverySystem.Builders
{
    public class CustomerBuilder : ICustomerBuilder
    {
        private int _id;
        private string _name;
        private string _phone;
        private string _address;
        private CustomerCategory _category = CustomerCategory.Regular;
        private static int _lastCustomerId = 0;

        public ICustomerBuilder SetId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID клиента должен быть положительным числом");
            _id = id;
            return this;
        }

        public ICustomerBuilder SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя клиента не может быть пустым");
            _name = name.Trim();
            return this;
        }

        public ICustomerBuilder SetPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("Телефон клиента не может быть пустым");

            phone = phone.Trim();
            if (!IsValidPhone(phone))
                throw new ArgumentException("Некорректный формат телефона");

            _phone = phone;
            return this;
        }

        public ICustomerBuilder SetAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Адрес клиента не может быть пустым");

            address = address.Trim();
            if (address.Length < 5)
                throw new ArgumentException("Адрес слишком короткий");

            _address = address;
            return this;
        }

        public ICustomerBuilder SetCategory(CustomerCategory category)
        {
            _category = category;
            return this;
        }

        public ICustomerBuilder SetPremium()
        {
            _category = CustomerCategory.Premium;
            return this;
        }

        public ICustomerBuilder SetVIP()
        {
            _category = CustomerCategory.VIP;
            return this;
        }

        public ICustomerBuilder SetSuperVIP()
        {
            _category = CustomerCategory.SuperVIP;
            return this;
        }

        public Customer Build()
        {
            if (!CanBuild())
                throw new InvalidOperationException("Невозможно создать клиента. Проверьте обязательные поля!");

            if (_id <= 0)
            {
                _id = ++_lastCustomerId;
            }
            return new Customer(_id, _name, _phone, _address, _category);
        }

        public bool CanBuild()
        {
            return !string.IsNullOrWhiteSpace(_name) &&
                   !string.IsNullOrWhiteSpace(_phone) &&
                   !string.IsNullOrWhiteSpace(_address);
        }

        private bool IsValidPhone(string phone)
        {

            var digitsOnly = new string(phone.Where(char.IsDigit).ToArray());
            return digitsOnly.Length >= 10 && digitsOnly.Length <= 15;
        }
    }
}