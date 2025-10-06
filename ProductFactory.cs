using System;

namespace VendingMachineApp
{
    public static class ProductFactory
    {
        public static Product Create(string type, string code, string name, int price, string extra = null)
        {
            switch (type.ToLower())
            {
                case "soda":
                    return new Soda(code, name, price, extra ?? "0.5L");
                case "snack":
                    int kcal = 0;
                    if (int.TryParse(extra, out var k)) kcal = k;
                    return new Snack(code, name, price, kcal);
                default:
                    return new GenericProduct(code, name, price);
            }
        }
    }

    public class GenericProduct : Product
    {
        public GenericProduct(string code, string name, int price) : base(code, name, price)
        {
        }
    }
}
