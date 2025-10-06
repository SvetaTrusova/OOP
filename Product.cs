using System;

namespace VendingMachineApp
{
    public abstract class Product
    {
        public string Code { get; }
        public string Name { get; }
        public int Price { get; }

        protected Product(string code, string name, int price)
        {
            Code = code;
            Name = name;
            Price = price;
        }

        public override string ToString()
        {
            return $"[{Code}] {Name} — {Price}₽";
        }
    }
}
