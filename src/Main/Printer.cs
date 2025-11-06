namespace san40_u5an40.ConsoleDisplayFramework;

/// <summary>
/// Класс для асинхронного вывода дисплея и осуществления ввода данных с помощью указанного контроллера
/// </summary>
/// <typeparam name="T">Тип возвращаемых данных из контроллера</typeparam>
public class Printer<T>
{
    private IPrintable upperDisplay;
    private IControllable<T> controlPanel;
    private bool isUpdateInput;             // Указывает на то, необходимо ли обновлять дисплей при изменении данных ввода, хранимых в контроллере

    /// <summary>
    /// Конструктор для создания экземпляра Printer
    /// </summary>
    /// <param name="printable">Печатаемый объект, используемый в качестве верхнего дисплея</param>
    /// <param name="controllable">Объект контроллера, осуществляющий ввод данных из консоли</param>
    /// <param name="isUpdateInput">Значение указывающее на то, стоит ли обновлять дисплей при изменении данных ввода</param>
    public Printer(IPrintable printable, IControllable<T> controllable, bool isUpdateInput = true) => 
        (this.upperDisplay, this.controlPanel, this.isUpdateInput) = (printable, controllable, isUpdateInput);

    /// <summary>
    /// Запускает дисплей и контроллер
    /// </summary>
    /// <returns>Данные контроллера в момент завершения его работы</returns>
    public async Task<T> ShowAsync()
    {
        if (Console.InputEncoding != Encoding.Unicode || Console.OutputEncoding != Encoding.Unicode)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
        }

#if WINDOWS
        if (Console.CursorVisible == true)
            Console.CursorVisible = false;
#endif

        Task startControl = new(controlPanel.StartControl);
        startControl.Start();

        await Task.WhenAll(StartPrintPanelsAsync(), startControl);

        return controlPanel.ControlValue;
    }

    // Метод печати верхнего дисплея и контроллера, обновляющий дисплей при изменении размера консоли или значения контроллера
    private async Task StartPrintPanelsAsync()
    {
        int consoleSize = Console.WindowWidth;
        T controlValue = controlPanel.ControlValue;

        PrintAllPanels();

        while (!controlPanel.IsExit)
        {
            if (Console.WindowWidth != consoleSize || IsChangedControlValue(controlValue))
            {
                PrintAllPanels();

                consoleSize = Console.WindowWidth;
                controlValue = controlPanel.ControlValue;
            }   

            await Task.Delay(200);
        }
    }

    // Метод печати верхнего дисплея вместе с контроллером ввода данных
    private void PrintAllPanels()
    {
        Console.Clear();

        upperDisplay.Print();
        Console.Write('\n');
        controlPanel.Print();
    }

    // Возвращает значение, требуется ли обновить дисплей в связи с изменениями данных контроллера
    private bool IsChangedControlValue(T controlValue)
    {
        if (isUpdateInput)
            return !controlPanel.ControlValue!.Equals(controlValue);

        return false;
    }
}
