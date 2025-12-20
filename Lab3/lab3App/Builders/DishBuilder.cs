using DeliverySystem.Models;
using DeliverySystem.Builders.Interfaces;

namespace DeliverySystem.Builders
{
    public class DishBuilder : IDishBuilder
    {
        private int _id;
        private string _name;
        private string _description;
        private decimal _price;
        private int _preparationTime;

        private static int _lastDishId = 0;

        public IDishBuilder SetId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID блюда должен быть положительным числом");
            _id = id;
            return this;
        }

        public IDishBuilder SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название блюда не может быть пустым");
            _name = name.Trim();
            return this;
        }

        public IDishBuilder SetDescription(string description)
        {
            _description = description ?? "";
            return this;
        }

        public IDishBuilder SetPrice(decimal price)
        {
            if (price < 0)
                throw new ArgumentException("Цена не может быть отрицательной");
            _price = price;
            return this;
        }

        public IDishBuilder SetPreparationTime(int minutes)
        {
            if (minutes < 0)
                throw new ArgumentException("Время приготовления не может быть отрицательным");
            _preparationTime = minutes;
            return this;
        }

        public Dish Build()
        {
            if (!CanBuild())
                throw new InvalidOperationException("Невозможно создать блюдо. Проверьте обязательные поля.");

            if (_id <= 0)
            {
                _id = ++_lastDishId;
            }

            return new Dish(_id, _name, _description, _price, _preparationTime);
        }

        public bool CanBuild()
        {
            return !string.IsNullOrWhiteSpace(_name) &&
                   _price > 0 &&
                   _preparationTime >= 0;
        }
    }
}