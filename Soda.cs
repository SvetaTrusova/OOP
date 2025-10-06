using System;

namespace VendingMachineApp
{
    public class Soda : Product
    {
        public string Volume { get; }

        public Soda(string code, string name, int price, string volume) : base(code, name, price)
        {
            Volume = volume;
        }

        public override string ToString()
        {
            return base.ToString() + $" ({Volume})";
        }
    }
}
