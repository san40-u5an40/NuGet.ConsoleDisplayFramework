namespace san40_u5an40.ConsoleDisplayFramework;

/// <summary>
/// Контроллер, необходимый для паузы перед следующим шагом выполнения программы
/// </summary>
public class ControllerPressEnter : IControllable<bool>
{
    private string menuMessage;
    private bool isExit = false;

    /// <summary>
    /// Создание экземпляра контроллера
    /// </summary>
    /// <param name="menuMessage">
    /// Подпись для ввода данных<br>
    /// Например <example>"Нажмите [enter] для продолжения: "</example>
    /// </param>
    public ControllerPressEnter(string? menuMessage = null) =>
        this.menuMessage = menuMessage ?? "Нажмите [enter] для продолжения...";

    /// <summary>
    /// Значение отражающее прекратил ли свою работу контроллер
    /// </summary>
    public bool IsExit => isExit;

    /// <summary>
    /// Значение, хранимое контроллером
    /// </summary>
    public bool ControlValue => true;

    // Метод вывода данных контроллера в консоль
    void IPrintable.Print() =>
        Console.Write(menuMessage);

    // Метод управления контроллером из консоли
    void IControllable<bool>.StartControl()
    {
        isExit = false;

        while (!isExit)
        {
            ConsoleKey key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.Enter)
                isExit = true;
        }
    }
}