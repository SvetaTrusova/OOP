using DeliverySystem.Builders;
using DeliverySystem.Builders.Interfaces;
using DeliverySystem.Enums;
using DeliverySystem.Interfaces;
using DeliverySystem.Mediators;
using DeliverySystem.Models;

namespace DeliverySystem.Core
{
    public class DeliverySystemDemo
    {
        private DeliverySystemApp _system;
        private DataLoader _dataLoader;
        private ICustomerBuilder _customerBuilder;
        private IDishBuilder _dishBuilder;

        public DeliverySystemDemo()
        {
            _customerBuilder = new CustomerBuilder();
            _dishBuilder = new DishBuilder();

            _dataLoader = new DataLoader(_customerBuilder, _dishBuilder);
        }

        public void Run()
        {
            Console.WriteLine("=== СИСТЕМА ДОСТАВКИ ЕДЫ ===\n");

            InitializeSystem();
            LoadData();
            DemonstrateSystem();
            SaveData();
        }

        private void InitializeSystem()
        {
            var mediator = new OrderProcessingMediator();
            _system = new DeliverySystemApp(mediator, _customerBuilder, _dishBuilder);
        }

        private void LoadData()
        {
            _dataLoader.EnsureDataDirectory();

            var customers = _dataLoader.LoadCustomers();
            foreach (var customer in customers)
            {
                _system.Customers.Add(customer);
                Console.WriteLine($"Загружен клиент: {customer.Name}, Категория: {customer.Category}");
            }

            var menu = _dataLoader.LoadMenu();
            foreach (var dish in menu)
            {
                _system.Menu.Add(dish);
                Console.WriteLine($"Загружено блюдо: {dish.Name}, Цена: {dish.Price} руб.");
            }
        }

        private void DemonstrateSystem()
        {
            Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ РАБОТЫ СИСТЕМЫ ===\n");

            if (_system.Customers.Count == 0)
            {
                throw new InvalidOperationException("Нет клиентов для демонстрации");
            }

            if (_system.Menu.Count == 0)
            {
                throw new InvalidOperationException("Нет блюд в меню для демонстрации");
            }

            var firstCustomer = _system.Customers.First();
            var pizza = _system.Menu.First(d => d.Name.Contains("Пепперони"));
            var cola = _system.Menu.First(d => d.Name.Contains("Кола"));

            Console.WriteLine("1) СОЗДАНИЕ И ОБРАБОТКА ЗАКАЗОВ");

            var pizzaItem1 = new OrderItemBuilder()
                .SetDish(pizza)
                .SetQuantity(1)
                .SetInstructions("Без перца")
                .Build();

            var order1 = _system.CreateOrder(
                customerId: firstCustomer.Id,
                deliveryAddress: firstCustomer.Address,
                isFastDelivery: false,
                specialPreferences: "Без перца",
                items: new List<OrderItem> { pizzaItem1 }
            );
            Console.WriteLine($"Создан заказ номер {order1.Id} для {firstCustomer.Name}");

            var pizzaItem2 = new OrderItemBuilder()
                .SetDish(pizza)
                .SetQuantity(2)
                .SetInstructions("Острая")
                .MakeSpicy()
                .Build();

            var colaItem = new OrderItemBuilder()
                .SetDish(cola)
                .SetQuantity(3)
                .Build();

            var order2 = _system.CreateOrder(
                customerId: firstCustomer.Id,
                deliveryAddress: firstCustomer.Address,
                isFastDelivery: true,
                items: new List<OrderItem> { pizzaItem2, colaItem }
            );
            Console.WriteLine($"Создан заказ номер {order2.Id} для {firstCustomer.Name}");

            _system.ApproveOrder(order1.Id);
            _system.CompletePreparation(order1.Id);
            _system.AssignCourier(order1.Id);
            _system.StartDelivery(order1.Id);
            _system.CompleteDelivery(order1.Id);

            Console.WriteLine("\n2) ОТМЕНА ЗАКАЗА");

            var pizzaItem3 = new OrderItemBuilder()
                .SetDish(pizza)
                .SetQuantity(1)
                .Build();

            var order3 = _system.CreateOrder(
                customerId: firstCustomer.Id,
                deliveryAddress: firstCustomer.Address,
                items: new List<OrderItem> { pizzaItem3 }
            );
            Console.WriteLine($"Создан заказ номер {order3.Id} для демонстрации отмены");

            _system.CancelOrder(order3.Id, "Передумал клиент");

            Console.WriteLine("\n3) РАСЧЕТ СТОИМОСТИ");

            var total1 = _system.CalculateOrderTotal(order1.Id);
            var total2 = _system.CalculateOrderTotal(order2.Id);

            Console.WriteLine($"Стоимость заказа номер {order1.Id}: {total1:F2} рублей");
            Console.WriteLine($"Стоимость заказа номер {order2.Id}: {total2:F2} рублей");

            Console.WriteLine("\n4) ПРОВЕРКА СКИДОК И БЕСПЛАТНОЙ ДОСТАВКИ");

            foreach (var customer in _system.Customers)
            {
                Console.WriteLine($"\nКлиент: {customer.Name}");
                Console.WriteLine($"    Категория: {customer.Category}");

                var directDiscount = customer.GetDiscountPercentage() * 100;
                var directFreeDelivery = customer.HasFreeDelivery();

                Console.WriteLine($"    Cкидка: {directDiscount:F0}%");
                Console.WriteLine($"    Бесплатная доставка: {(directFreeDelivery ? "Да" : "Нет")}");
            }

            Console.WriteLine("\n5) СТАТИСТИКА СИСТЕМЫ");

            var activeOrders = _system.GetActiveOrders();
            Console.WriteLine($"Активных заказов: {activeOrders.Count}");

            var customerOrders = _system.GetCustomerOrders(firstCustomer.Id);
            Console.WriteLine($"Заказов у клиента {firstCustomer.Name}: {customerOrders.Count}");

            var completedCount = _system.GetCompletedOrdersCount();
            Console.WriteLine($"Завершенных заказов: {completedCount}");

            var allOrdersCount = _system.GetOrdersCount();
            Console.WriteLine($"Всего заказов в системе: {allOrdersCount}");

            Console.WriteLine("\n6) СТАТУСЫ ЗАКАЗОВ:");
            foreach (var order in _system.Orders)
            {
                var status = _system.GetOrderStatus(order.Id);
                Console.WriteLine($"Заказ номер {order.Id}: {status}");
            }
        }

        private void SaveData()
        {
            _dataLoader.SaveCustomers(_system.Customers);
            _dataLoader.SaveMenu(_system.Menu);
            _dataLoader.SaveOrdersSummary(_system.Orders);

            Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ ЗАВЕРШЕНА ===\n");
            Console.WriteLine("Данные сохранены в папке 'Data'.");
        }
    }
}