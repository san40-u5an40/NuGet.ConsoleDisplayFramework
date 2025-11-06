namespace san40_u5an40.ConsoleDisplayFramework.Interfaces;

/// <summary>
/// Интерфейс, определяющий требуемое поведение контроллера дисплея
/// </summary>
/// <typeparam name="T">Тип возвращаемых данных контроллером</typeparam>
public interface IControllable<T> : IPrintable
{
    public void StartControl();
    public bool IsExit { get; }
    public T ControlValue { get; }
}
