using DeliverySystem.Models;

namespace DeliverySystem.Builders.Interfaces
{
    public interface IBuilder<T> where T : class
    {
        T Build();
        bool CanBuild();
    }
}