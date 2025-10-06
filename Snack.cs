using System;

namespace VendingMachineApp
{
    public class Snack : Product
    {
        public int Calories { get; }

        public Snack(string code, string name, int price, int calories) : base(code, name, price)
        {
            Calories = calories;
        }

        public override string ToString()
        {
            return base.ToString() + $" ({Calories} kcal)";
        }
    }
}
