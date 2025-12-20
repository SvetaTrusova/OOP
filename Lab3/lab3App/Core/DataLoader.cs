using DeliverySystem.Builders;
using DeliverySystem.Builders.Interfaces;
using DeliverySystem.Enums;
using DeliverySystem.Models;


namespace DeliverySystem.Core
{
    public class DataLoader
    {
        private string _dataDirectory = "Data";
        private ICustomerBuilder _customerBuilder;
        private IDishBuilder _dishBuilder;

        public DataLoader(ICustomerBuilder customerBuilder, IDishBuilder dishBuilder)
        {
            _customerBuilder = customerBuilder ?? new CustomerBuilder();
            _dishBuilder = dishBuilder ?? new DishBuilder();
        }

        public List<Customer> LoadCustomers()
        {
            var customers = new List<Customer>();
            var filePath = Path.Combine(_dataDirectory, "customers.txt");

            if (!File.Exists(filePath))
                return customers;

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length >= 4)
                {
                    var category = Enum.Parse<Enums.CustomerCategory>(parts[3]);

                    var customer = _customerBuilder
                        .SetName(parts[0])
                        .SetPhone(parts[1])
                        .SetAddress(parts[2])
                        .SetCategory(category)
                        .Build();

                    customers.Add(customer);
                }
            }

            return customers;
        }

        public List<Dish> LoadMenu()
        {
            var menu = new List<Dish>();
            var filePath = Path.Combine(_dataDirectory, "menu.txt");

            if (!File.Exists(filePath))
                return menu;

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length >= 4)
                {
                    var price = decimal.Parse(parts[2]);
                    var prepTime = int.Parse(parts[3]);
                    var description = parts.Length > 4 ? parts[4] : "";

                    var dish = _dishBuilder
                        .SetName(parts[0])
                        .SetDescription(description)
                        .SetPrice(price)
                        .SetPreparationTime(prepTime)
                        .Build();

                    menu.Add(dish);
                }
            }

            return menu;
        }

        public void EnsureDataDirectory()
        {
            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
            }
        }

        public void SaveCustomers(List<Customer> customers)
        {
            EnsureDataDirectory();
            var filePath = Path.Combine(_dataDirectory, "customers.txt");
            var lines = customers.Select(c =>
                $"{c.Name}|{c.Phone}|{c.Address}|{c.Category}");
            File.WriteAllLines(filePath, lines);
        }

        public void SaveMenu(List<Dish> menu)
        {
            EnsureDataDirectory();
            var filePath = Path.Combine(_dataDirectory, "menu.txt");
            var lines = menu.Select(d =>
                $"{d.Name}|{d.Description}|{d.Price}|{d.PreparationTime}");
            File.WriteAllLines(filePath, lines);
        }

        public void SaveOrdersSummary(List<Order> orders)
        {
            EnsureDataDirectory();
            var filePath = Path.Combine(_dataDirectory, "orders_summary.txt");

            var orderLines = new List<string>
            {
                $"Всего заказов: {orders.Count}",
                $"Выполнено: {orders.Count(o => o.GetStatus() == "Выполнен")}",
                $"Отменено: {orders.Count(o => o.GetStatus() == "Отменен")}",
                ""
            };

            foreach (var order in orders)
            {
                orderLines.Add($"Заказ номер {order.Id}: {order.Customer.Name}, {order.GetStatus()}");
            }

            File.WriteAllLines(filePath, orderLines);
        }
    }
}