using InventorySystem.Core;

class Program
{
    static void Main()
    {
        try
        {
            var game = new Game();
            game.RunGame();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}