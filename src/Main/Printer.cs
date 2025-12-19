namespace san40_u5an40.ConsoleDisplayFramework;

/// <summary>
/// Класс для асинхронного вывода дисплея и осуществления ввода данных с помощью указанного контроллера
/// </summary>
public class Printer
{
    private UpperDisplay upperDisplay;

    /// <summary>
    /// Конструктор для создания экземпляра Printer с верхним дисплеем
    /// </summary>
    /// <param name="upperDisplay">Верхний дисплей</param>
    public Printer(UpperDisplay upperDisplay) =>
        this.upperDisplay = upperDisplay;

    /// <summary>
    /// Метод вывода контроллера вместе с верхним дисплеем экземпляра
    /// </summary>
    /// <typeparam name="T">Тип возвращаемых данных контроллером</typeparam>
    /// <param name="controlPanel">Контроллер ввода</param>
    /// <param name="isUpdateInput">Необходимость обновлять дисплей при изменении значения контроллера</param>
    /// <returns>Значение, введённое пользователем в контроллер</returns>
    public async Task<T> ShowAsync<T>(IControllable<T> controlPanel, bool isUpdateInput = true) =>
        await ShowAsync(controlPanel, isUpdateInput, upperDisplay);

    /// <summary>
    /// Метод вывода контроллера
    /// </summary>
    /// <typeparam name="T">Тип возвращаемых данных контроллером</typeparam>
    /// <param name="controlPanel">Контроллер ввода</param>
    /// <param name="isUpdateInput">Необходимость обновлять дисплей при изменении значения контроллера</param>
    /// <param name="upperDisplay">Ве</param>
    /// <returns>Значение, введённое пользователем в контроллер</returns>
    public static async Task<T> ShowAsync<T>(IControllable<T> controlPanel, bool isUpdateInput = true, UpperDisplay? upperDisplay = null)
    {
        SetupUnicode();

        Task startControl = new(controlPanel.StartControl);
        startControl.Start();
        await Task.WhenAll(StartPrintPanelsAsync(controlPanel, isUpdateInput, upperDisplay), startControl);

        return controlPanel.ControlValue;

        // Локальная функция установки юникодовской кодировки в консоли
        static void SetupUnicode()
        {
            if (Console.InputEncoding != Encoding.Unicode || Console.OutputEncoding != Encoding.Unicode)
            {
                Console.OutputEncoding = Encoding.Unicode;
                Console.InputEncoding = Encoding.Unicode;
            }
        }

        // Локальная функция печати верхнего дисплея и контроллера, обновляющий печать при изменении размера консоли или значения контроллера
        static async Task StartPrintPanelsAsync(IControllable<T> controlPanel, bool isUpdateInput, UpperDisplay? upperDisplay)
        {
            int consoleSize = Console.WindowWidth;
            T controlValue = controlPanel.ControlValue;

            PrintAllPanels(controlPanel, upperDisplay);

            while (!controlPanel.IsExit)
            {
                if (Console.WindowWidth != consoleSize || IsChangedControlValue(controlPanel, controlValue, isUpdateInput))
                {
                    PrintAllPanels(controlPanel, upperDisplay);

                    consoleSize = Console.WindowWidth;
                    controlValue = controlPanel.ControlValue;
                }

                await Task.Delay(50);
            }
        }

        // Локальная функция печати верхнего дисплея вместе с контроллером ввода данных
        static void PrintAllPanels(IControllable<T> controlPanel, UpperDisplay? upperDisplay)
        {
            Console.Clear();
#if WINDOWS
            if (Console.CursorVisible == true)
                Console.CursorVisible = false;
#endif

            if (upperDisplay != null)
            {
                upperDisplay.Print();
                Console.Write('\n');
            }

            controlPanel.Print();
        }

        // Возвращает значение, требуется ли обновить дисплей в связи с изменениями данных контроллера
        static bool IsChangedControlValue(IControllable<T> controlPanel, T controlValue, bool isUpdateInput)
        {
            if (isUpdateInput)
                return !controlPanel.ControlValue!.Equals(controlValue);

            return false;
        }
    }
}
