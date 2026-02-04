namespace san40_u5an40.ConsoleDisplayFramework;

/// <summary>
/// Контроллер, необходимый для ввода строковых данных
/// </summary>
public class ControllerReadLine : IControllable<string>
{
    private string menuMessage;

    private char[] currentInput = new char[0];
    private bool isExit = false;

    private bool isShowText;

    /// <summary>
    /// Создание экземпляра контроллера
    /// </summary>
    /// <param name="message">
    /// Подпись для ввода данных<br>
    /// Например <example>"Введите данные: "</example>
    /// </param>
    /// <param name="isShowText">Индикатор того, необходимо ли отображать вводимые данные</param>
    public ControllerReadLine(string? message = null, bool isShowInput = false) =>
        (this.menuMessage, this.isShowText) = (message ?? "Введите данные: ", isShowInput);

    /// <summary>
    /// Значение отражающее прекратил ли свою работу контроллер
    /// </summary>
    public bool IsExit => isExit;

    /// <summary>
    /// Значение, хранимое контроллером
    /// </summary>
    public string ControlValue => new string(currentInput).Replace("\0", string.Empty);

    // Метод вывода данных контроллера в консоль
    void IPrintable.Print() =>
        Console.WriteLine(menuMessage + (isShowText ? new string(currentInput) : string.Empty));

    // Метод управления контроллером из консоли
    void IControllable<string>.StartControl()
    {
        isExit = false;

        currentInput = new char[128];
        int currentPosition = 0;

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
                if (currentPosition > 0)
                    currentInput[currentPosition--] = '\0';
                continue;
            }

            if(currentPosition >= currentInput.Length)
                IncreaseArray(ref currentInput);

            currentInput[currentPosition++] = key.KeyChar;
        }

        // Локальная функция увеличения размера массива символов
        static void IncreaseArray(ref char[] arr)
        {
            char[] temp = arr;

            arr = new char[arr.Length + 128];
            for (int i = 0; i < temp.Length && temp[i] != '\0'; i++)
                arr[i] = temp[i];
        }
    }
}