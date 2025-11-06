namespace san40_u5an40.ConsoleDisplayFramework;

/// <summary>
/// Контроллер, необходимый для ввода бинарных значений: True or False
/// </summary>
public class ControllerYNChoice : IControllable<bool>
{
    private string? menuMessage = null;
    private bool isExit = false;

    private bool choice = false;

    /// <summary>
    /// Создание экземпляра контроллера
    /// </summary>
    /// <param name="menuMessage">Сообщение для отображения над контроллером</param>
    public ControllerYNChoice(string? menuMessage = null) =>
        this.menuMessage = menuMessage ?? "Подтвердите выбор: ([Y]/[N])";

    /// <summary>
    /// Значение отражающее прекратил ли свою работу контроллер
    /// </summary>
    public bool IsExit => isExit;

    /// <summary>
    /// Значение, хранимое контроллером
    /// </summary>
    public bool ControlValue => choice;

    /// <summary>
    /// Вывод контроллера в консоль
    /// </summary>
    public void Print() =>
        Console.WriteLine(menuMessage);

    /// <summary>
    /// Метод, запускающий ввод данных контроллером
    /// </summary>
    public void StartControl()
    {
        isExit = false;

        while (!isExit)
        {
            ConsoleKey key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.Y || key == ConsoleKey.N)
            {
                choice = key == ConsoleKey.Y;
                isExit = true;
            }
        }
    }
}