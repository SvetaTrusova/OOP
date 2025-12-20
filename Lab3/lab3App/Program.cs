using DeliverySystem.Core;

class Program
{
    static void Main()
    {
        try
        {
            var deliverySystem = new DeliverySystemDemo();
            deliverySystem.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}