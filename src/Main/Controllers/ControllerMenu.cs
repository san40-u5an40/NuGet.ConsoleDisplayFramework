namespace san40_u5an40.ConsoleDisplayFramework;

/// <summary>
/// Контроллер, необходимый для отображения меню с множественным выбором
/// </summary>
public class ControllerMenu : IControllable<int>
{
    private string? menuMessage;
    private bool isExit = false;

    private List<string> items = new();
    private int currentValue = 1;

    /// <summary>
    /// Создание контроллера меню
    /// </summary>
    /// <param name="menuMessage">Подпись меню</param>
    public ControllerMenu(string? menuMessage = null) =>
        this.menuMessage = menuMessage;

    /// <summary>
    /// Стартовое значение меню
    /// </summary>
    public int StartValue
    {
        get => field;
        set
        {
            field = value;
            currentValue = value;
        }
    } = 1;

    /// <summary>
    /// Цвет выделенного пункта меню
    /// </summary>
    public ConsoleColor CurrentValueColor { get; set; } = ConsoleColor.DarkRed;

    /// <summary>
    /// Значение отражающее прекратил ли свою работу контроллер
    /// </summary>
    public bool IsExit => isExit;

    /// <summary>
    /// Значение, хранимое контроллером
    /// </summary>
    public int ControlValue => currentValue;

    /// <summary>
    /// Добавление пункта меню
    /// </summary>
    /// <param name="itemDescription">Значение пункта меню</param>
    /// <returns>Экземпляр класса (builder-паттерн)</returns>
    public ControllerMenu AddItem(string itemDescription)
    {
        items.Add(itemDescription);
        return this;
    }

    // Метод вывода данных контроллера в консоль
    void IPrintable.Print()
    {
        if (items.Count == 0)
            return;

        if (!IsValidConsoleWidth())
        {
            Console.WriteLine("Увеличьте консольное окно для просмотра меню!");
            return;
        }

        if (menuMessage != null)
            Console.WriteLine(menuMessage);

        for (int i = 0; i < items.Count; i++)
        {
            int position = StartValue + i;

            if (position == currentValue)
                Console.WriteColor(position + ") " + items[i] + " ←\n", CurrentValueColor);
            else
                Console.WriteLine(position + ") " + items[i]);
        }
    }

    // Проверка размера консольного окна на валидность
    private bool IsValidConsoleWidth()
    {
        int displaySpace = 5;
        int maxLength = items.MaxBy(p => p.Length)!.Length + displaySpace;

        if (Console.WindowWidth < maxLength)
            return false;

        return true;
    }

    // Метод управления контроллером из консоли
    void IControllable<int>.StartControl()
    {
        isExit = false;

        while (!isExit)
        {
            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.DownArrow || key == ConsoleKey.UpArrow)
                ChangeItem(key);

            if (key == ConsoleKey.Enter)
                isExit = true;
        }
    }

    // Метод смены корректного пункта меню
    private void ChangeItem(ConsoleKey key)
    {
        if (key == ConsoleKey.UpArrow && currentValue > StartValue)
            currentValue--;

        if (key == ConsoleKey.DownArrow && currentValue < StartValue + items.Count - 1)
            currentValue++;
    }
}