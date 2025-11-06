namespace san40_u5an40.ConsoleDisplayFramework;

/// <summary>
/// Контроллер, необходимый для ввода строковых данных
/// </summary>
public class ControllerReadLine : IControllable<string>
{
    private string menuMessage;
    private bool isExit = false;

    private StringBuilder currentInput = new();
    private bool isShowText;

    /// <summary>
    /// Создание экземпляра контроллера
    /// </summary>
    /// <param name="message">
    /// Подпись для ввода данных<br>
    /// Например <example>"Введите данные: "</example>
    /// </param>
    /// <param name="isShowText">Индикатор того, необходимо ли отображать вводимый данные</param>
    public ControllerReadLine(string message, bool isShowText = false)
    {
        if (string.IsNullOrWhiteSpace(message))
            menuMessage = "Введите данные: ";
        else
            menuMessage = message;

        this.isShowText = isShowText;
    }

    /// <summary>
    /// Значение отражающее прекратил ли свою работу контроллер
    /// </summary>
    public bool IsExit => isExit;

    /// <summary>
    /// Значение, хранимое контроллером
    /// </summary>
    public string ControlValue => currentInput.ToString();

    /// <summary>
    /// Вывод контроллера в консоль
    /// </summary>
    public void Print() =>
        Console.WriteLine(menuMessage + (isShowText ? currentInput.ToString() : string.Empty));

    /// <summary>
    /// Метод, запускающий ввод данных контроллером
    /// </summary>
    public void StartControl()
    {
        isExit = false;

        while (!isExit)
        {
            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter)
            {
                isExit = true;
                return;
            }

            if (key.Key == ConsoleKey.Delete || key.Key == ConsoleKey.Backspace)
            {
                var temp = new StringBuilder();

                for (int i = 0; i < currentInput.Length - 1; i++)
                    temp.Append(currentInput[i]);

                currentInput = temp;
                continue;
            }

            currentInput.Append(key.KeyChar);
        }
    }
}