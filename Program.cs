using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachineApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var vm = new VendingMachine();

            // Несколько товаров по умолчанию
            vm.AddProduct(ProductFactory.Create("soda", "A1", "Cola", 55, "0.5L"), 5);
            vm.AddProduct(ProductFactory.Create("soda", "A2", "Orange Soda", 50, "0.5L"), 3);
            vm.AddProduct(ProductFactory.Create("snack", "B1", "Chips", 45, "200"), 6);
            vm.AddProduct(ProductFactory.Create("snack", "B2", "Chocolate", 65, "250"), 4);

            Console.WriteLine("=== Вендiнг-автомат (консоль) ===\n");

            while (true)
            {
                Console.WriteLine("1) Посмотреть ассортимент");
                Console.WriteLine("2) Оплатить и купить товар");
                Console.WriteLine("3) Отменить операцию и вернуть монеты (во время ввода)");
                Console.WriteLine("4) Режим администратора");
                Console.WriteLine("0) Выход");
                Console.Write("Выберите пункт: ");
                var key = Console.ReadLine();
                Console.WriteLine();

                switch (key)
                {
                    case "1":
                        PrintProducts(vm);
                        break;
                    case "2":
                        PurchaseFlow(vm);
                        break;
                    case "3":
                        Console.WriteLine("Операция отменена. (Если вы были в процессе ввода — монеты вернутся)");
                        break;
                    case "4":
                        AdminFlow(vm);
                        break;
                    case "0":
                        Console.WriteLine("Выход. Спасибо!");
                        return;
                    default:
                        Console.WriteLine("Неверный пункт меню.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static void PrintProducts(VendingMachine vm)
        {
            Console.WriteLine("Доступные товары:");
            foreach (var (product, qty) in vm.ListProducts())
            {
                Console.WriteLine($"{product} — в наличии: {qty}");
            }
            Console.WriteLine();
        }

        static void PurchaseFlow(VendingMachine vm)
        {
            PrintProducts(vm);
            Console.Write("Введите код товара: ");
            var code = Console.ReadLine()?.Trim().ToUpper();
            if (string.IsNullOrEmpty(code)) { Console.WriteLine("Код не введён."); return; }

            if (!vm.TryGetProduct(code, out var p, out var qty)) { Console.WriteLine("Товар не найден."); return; }

            Console.WriteLine($"Вы выбрали: {p.Name} — {p.Price}₽. Внесите монеты.");
            Console.WriteLine("Доступные номиналы: " + string.Join(", ", Coins.CoinRegistry) + " (в рублях)");
            Console.WriteLine("Вводите номиналы по одному. Для завершения введите слово 'done'. Для отмены — 'cancel'.");

            var inserted = new Dictionary<int, int>();
            while (true)
            {
                Console.Write("Вставить монету: ");
                var line = Console.ReadLine()?.Trim().ToLower();
                if (line == "done") break;
                if (line == "cancel") { Console.WriteLine("Операция отменена. Монеты возвращены."); return; }

                if (int.TryParse(line, out var denom) && Coins.CoinRegistry.Contains(denom))
                {
                    if (!inserted.ContainsKey(denom)) inserted[denom] = 0;
                    inserted[denom]++;
                    int sum = inserted.Sum(kv => kv.Key * kv.Value);
                    Console.WriteLine($"Внесено: {sum}₽");
                    if (sum >= p.Price)
                    {
                        // Попытка покупки
                        var (success, change, message) = vm.Buy(code, inserted);
                        Console.WriteLine(message);
                        if (success)
                        {
                            Console.WriteLine("Спасибо за покупку!");
                            return;
                        }
                        else
                        {
                            // Если неудача (например, нельзя выдать сдачу) — предложить продолжить или отменить
                            Console.WriteLine("1) Продолжить ввод монет\n2) Отмена и возврат монет");
                            var opt = Console.ReadLine()?.Trim();
                            if (opt == "2") { Console.WriteLine("Возврат монет:"); PrintInserted(inserted); return; }
                            else { Console.WriteLine("Продолжайте вводить монеты."); continue; }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Неправильный номинал. Попробуйте снова.");
                }
            }

            // Если пользователь закончил ввод (done) и сумма меньше цены — возврат
            int total = inserted.Sum(kv => kv.Key * kv.Value);
            if (total < p.Price)
            {
                Console.WriteLine($"Внесено {total}₽, чего недостаточно для покупки {p.Price}₽. Возврат монет:");
                PrintInserted(inserted);
            }
        }

        static void PrintInserted(Dictionary<int, int> inserted)
        {
            if (inserted == null || inserted.Count == 0) { Console.WriteLine("Нет монет."); return; }
            foreach (var kv in inserted.OrderByDescending(k => k.Key)) Console.WriteLine($"{kv.Key}₽ x{kv.Value}");
        }

        static void AdminFlow(VendingMachine vm)
        {
            Console.Write("Введите пароль администратора: ");
            var pw = Console.ReadLine()?.Trim();
            if (pw != "admin123") { Console.WriteLine("Неверный пароль."); return; }

            while (true)
            {
                Console.WriteLine("--- Режим администратора ---");
                Console.WriteLine("1) Показать ассортимент");
                Console.WriteLine("2) Пополнить товар");
                Console.WriteLine("3) Добавить новый товар");
                Console.WriteLine("4) Показать состояние монет");
                Console.WriteLine("5) Забрать собранные средства");
                Console.WriteLine("0) Выйти из режима администратора");
                Console.Write("Выберите: ");
                var cmd = Console.ReadLine()?.Trim();
                switch (cmd)
                {
                    case "1":
                        PrintProducts(vm);
                        break;
                    case "2":
                        Console.Write("Код товара: ");
                        var code = Console.ReadLine()?.Trim().ToUpper();
                        if (!vm.TryGetProduct(code, out var prod, out var q)) { Console.WriteLine("Не найден."); break; }
                        Console.Write("Кол-во для добавления: ");
                        if (!int.TryParse(Console.ReadLine(), out var add)) { Console.WriteLine("Неверно."); break; }
                        vm.AddProduct(prod, add);
                        Console.WriteLine("Пополнено.");
                        break;
                    case "3":
                        Console.Write("Тип (soda/snack/other): ");
                        var type = Console.ReadLine()?.Trim();
                        Console.Write("Код (например C1): "); var codeNew = Console.ReadLine()?.Trim().ToUpper();
                        Console.Write("Название: "); var name = Console.ReadLine();
                        Console.Write("Цена (руб): "); if (!int.TryParse(Console.ReadLine(), out var price)) { Console.WriteLine("Неверна цена"); break; }
                        Console.Write("Extra (для soda: объем, для snack: kcal): "); var extra = Console.ReadLine();
                        Console.Write("Кол-во: "); if (!int.TryParse(Console.ReadLine(), out var qty)) { Console.WriteLine("Неверно"); break; }
                        var newProd = ProductFactory.Create(type, codeNew, name, price, extra);
                        vm.AddProduct(newProd, qty);
                        Console.WriteLine("Товар добавлен.");
                        break;
                    case "4":
                        Console.WriteLine("Состояние монет в автомате: " + vm.CoinState());
                        break;
                    case "5":
                        var (total, coins) = vm.CollectMoney();
                        Console.WriteLine($"Забрано: {total}₽. Монеты: {string.Join(", ", coins.Select(kv => kv.Key + "₽ x" + kv.Value))}");
                        break;
                    case "0":
                        Console.WriteLine("Выход из администратора.");
                        return;
                    default:
                        Console.WriteLine("Неверная команда");
                        break;
                }
            }
        }
    }
}
