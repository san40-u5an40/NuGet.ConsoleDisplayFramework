namespace san40_u5an40.ConsoleDisplayFramework;

/// <summary>
/// Контроллер, необходимый для отображения меню с множественным выбором
/// </summary>
public class ControllerMenu : IControllable<int>
{
    private string? menuMessage = null;
    private bool isExit = false;

    private Dictionary<int, string> menuItems;
    private int currentMenuItem;

    /// <summary>
    /// Создание экземпляра контроллера
    /// </summary>
    /// <param name="menuItems">Коллекция пунктов меню</param>
    /// <param name="startItemValue">Начальное значение меню</param>
    /// <param name="menuMessage">Подпись меню</param>
    /// <exception cref="ArgumentException">
    /// Ошибка переданной коллекции пунктов меню:<br>
    ///  - Указана пустая коллекция<br>
    ///  - Коллекция не содержит указанное стартовое значение<br>
    ///  - Коллекция должна быть непрерывной
    /// </exception>
    public ControllerMenu(Dictionary<int, string> menuItems, int startItemValue, string? menuMessage = null)
    {
        var result = IsValid(menuItems, startItemValue);
        if (!result.IsValid)
            throw new ArgumentException(result.ErrorMessage);

        this.menuMessage = menuMessage;
        this.menuItems = menuItems;
        currentMenuItem = startItemValue;

        // Локальная функция проверки переданной коллекции на валидность
        static (bool IsValid, string? ErrorMessage) IsValid(Dictionary<int, string> menuItems, int startItemValue)
        {
            if (menuItems.Count == 0)
                return (false, "Для составления меню необходимо передать непустую коллекцию!");

            if (!menuItems.ContainsKey(startItemValue))
                return (false, "Необходимо указать стартовое значение меню, которое в нём содержится!");

            menuItems = menuItems
                .OrderBy(p => p.Key)
                .ToDictionary();

            int minKey = menuItems.MinBy(p => p.Key).Key;
            int maxKey = menuItems.MaxBy(p => p.Key).Key;

            if (maxKey - minKey + 1 != menuItems.Count)
                return (false, "Необходимо указать коллекцию с непрерывно возрастающей последовательностью чисел!");

            return (true, null);
        }
    }

    /// <summary>
    /// Значение отражающее прекратил ли свою работу контроллер
    /// </summary>
    public bool IsExit => isExit;

    /// <summary>
    /// Значение, хранимое контроллером
    /// </summary>
    public int ControlValue => currentMenuItem;

    /// <summary>
    /// Вывод контроллера в консоль
    /// </summary>
    public void Print()
    {
        if (!IsValidConsoleWidth())
        {
            Console.WriteLine("Увеличьте консольное окно для просмотра меню!");
            return;
        }

        if (menuMessage != null)
            Console.WriteLine(menuMessage);

        foreach (var item in menuItems)
        {
            if (item.Key == currentMenuItem)
                ConsoleExtension.WriteColor(item.Key + ") " + item.Value + " ←\n", ConsoleColor.DarkRed);
            else
                Console.WriteLine(item.Key + ") " + item.Value);
        }
    }

    // Проверка размера консольного окна на валидность
    private bool IsValidConsoleWidth()
    {
        int displaySpace = 6;
        int maxLength = menuItems.MaxBy(p => p.Value.Length).Value.Length + displaySpace;

        if (Console.WindowWidth < maxLength)
            return false;

        return true;
    }

    /// <summary>
    /// Метод, запускающий ввод данных контроллером
    /// </summary>
    public void StartControl()
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
        if (key == ConsoleKey.UpArrow && menuItems.ContainsKey(currentMenuItem - 1))
            currentMenuItem--;

        if (key == ConsoleKey.DownArrow && menuItems.ContainsKey(currentMenuItem + 1))
            currentMenuItem++;
    }
}