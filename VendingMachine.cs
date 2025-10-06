using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachineApp
{
    public class VendingMachine
    {
        private readonly Dictionary<string, (Product product, int qty)> inventory = new Dictionary<string, (Product, int)>();
        private readonly CoinRegister coinRegister = new CoinRegister();

        private int collectedFromSales = 0;

        public VendingMachine()
        {
            coinRegister.LoadCoins(10, 10);
            coinRegister.LoadCoins(5, 20);
            coinRegister.LoadCoins(2, 30);
            coinRegister.LoadCoins(1, 50);
        }

        public void AddProduct(Product p, int qty)
        {
            if (inventory.ContainsKey(p.Code))
            {
                var tup = inventory[p.Code];
                inventory[p.Code] = (tup.product, tup.qty + qty);
            }
            else inventory[p.Code] = (p, qty);
        }

        public bool TryGetProduct(string code, out Product p, out int qty)
        {
            if (inventory.TryGetValue(code, out var tup))
            {
                p = tup.product; qty = tup.qty; return true;
            }
            p = null; qty = 0; return false;
        }

        public IEnumerable<(Product product, int qty)> ListProducts()
        {
            return inventory.Values.Select(v => (v.product, v.qty));
        }

        public (bool success, Dictionary<int, int> change, string message) Buy(string code, IDictionary<int, int> insertedCoins)
        {
            if (!inventory.TryGetValue(code, out var tup))
                return (false, null, "Товар не найден");
            if (tup.qty <= 0)
                return (false, null, "Товара нет в наличии");

            int price = tup.product.Price;
            int inserted = insertedCoins.Sum(kv => kv.Key * kv.Value);

            if (inserted < price)
                return (false, null, $"Недостаточно денег. Нужно {price}₽, внесено {inserted}₽");

            int changeAmount = inserted - price;

            if (!coinRegister.TryMakeChange(changeAmount, insertedCoins, out var change))
            {
                return (false, null, "Нельзя выдать сдачу. Попробуйте внести точную сумму или другие монеты.");
            }

            inventory[code] = (tup.product, tup.qty - 1);
            coinRegister.AcceptInserted(insertedCoins);
            coinRegister.DispenseChange(change);

            collectedFromSales += price;

            return (true, change, $"Выдано: {tup.product.Name}. Сдача: {FormatChange(change)}");
        }

        private string FormatChange(IDictionary<int, int> change)
        {
            if (change == null || change.Count == 0) return "0₽";
            return string.Join(", ", change.OrderByDescending(k => k.Key).Select(kv => $"{kv.Key}₽ x{kv.Value}"));
        }

        public (int total, Dictionary<int, int> coins) CollectMoney()
        {
            return (coinRegister.CollectAll(out var coins), coins);
        }

        public string CoinState() => coinRegister.ToString();

        public int SalesTotal() => collectedFromSales;
    }
}
