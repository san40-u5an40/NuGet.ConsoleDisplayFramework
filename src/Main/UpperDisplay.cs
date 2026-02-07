namespace san40_u5an40.ConsoleDisplayFramework
{
    /// <summary>
    /// Перечисление типами выравнивания верхнего дисплея
    /// </summary>
    public enum ConsoleDisplayAlignment { Left, Center, Right }

    /// <summary>
    /// Класс, предназначенный для создания верхнего консольного дисплея\
    /// Пример:\
    /// <example>
    /// /*-------------------------*/
    /// /*-----Пример дисплея------*/
    /// /*-------------------------*/
    /// </example>
    /// </summary>
    public class UpperDisplay : IPrintable
    {
        private List<ConsoleLineInfo> displayLines = new();
        private const int DISPLAY_SPACE = 5;                       // Небольшое пространство между концом дисплея и краем консольного окна, чтобы дисплейные строки не переносились
        private const int MINIMAL_CONSOLE_LENGTH = 8;              // Минимальная длина дисплея, при которой возможна его корректная работа

        /// <summary>
        /// Длина дисплея
        /// </summary>
        public int Length => Console.WindowWidth - DISPLAY_SPACE;

        /// <summary>
        /// Добавляет дисплейную строку
        /// </summary>
        /// <param name="charDisplay">Дисплейный символ, заполняющий свободное пространство между строкой и краями дисплея</param>
        /// <param name="message">Строка для отображения в дисплейной линии</param>
        /// <param name="alignmentType">Тип выравнивания</param>
        public UpperDisplay Append(char charDisplay, string message, ConsoleDisplayAlignment alignmentType = ConsoleDisplayAlignment.Center)
        {
            displayLines.Add(new ConsoleLineInfo(alignmentType, charDisplay, message));
            return this;
        }

        /// <summary>
        /// Добавляет дисплейную строку без надписи
        /// </summary>
        /// <param name="charDisplay">Дисплейный символ, заполняющий свободное пространство между строкой и краями дисплея</param>
        public UpperDisplay AppendEmpty(char charDisplay)
        {
            displayLines.Add(new ConsoleLineInfo(null, charDisplay, string.Empty));
            return this;
        }

        /// <summary>
        /// Метод для печати консольного дисплея
        /// </summary>
        public void Print()
        {
            if (displayLines.Count == 0 || Length < MINIMAL_CONSOLE_LENGTH)
                return;

            foreach (ConsoleLineInfo line in displayLines)
            {
                var resultString = new StringBuilder();

                resultString.Append("/*");

                switch (line.AlignmentType)
                {
                    case ConsoleDisplayAlignment.Left:
                        resultString.AppendLeftMessage(line, Length);
                        break;

                    case ConsoleDisplayAlignment.Center:
                        resultString.AppendCenterMessage(line, Length);
                        break;

                    case ConsoleDisplayAlignment.Right:
                        resultString.AppendRightMessage(line, Length);
                        break;

                    default:
                        resultString.AppendEmptyMessage(line, Length);
                        break;
                }

                resultString.Append("*/");

                Console.WriteLine(resultString.ToString());
            }
        }
    }
}

namespace san40_u5an40.ConsoleDisplayFramework.Data
{
    /// <summary>
    /// Запись, хранящая информацию о дисплейной строке
    /// </summary>
    /// <param name="AlignmentType">Тип выравнивания текста</param>
    /// <param name="CharDisplay">Дисплейный символ, заполняющий свободное пространство между строкой и краями дисплея</param>
    /// <param name="Message">Строка для отображения в дисплейной линии</param>
    public readonly record struct ConsoleLineInfo(ConsoleDisplayAlignment? AlignmentType, char CharDisplay, string Message);

    /// <summary>
    /// Класс с методам расширения для StringBuilder
    /// </summary>
    public static class StringBuilderExtension
    {
        /// <summary>
        /// Добавляет надпись указанной длины, выравненную по левому краю и заполненную указанным символом, при наличии свободного пространства 
        /// </summary>
        /// <param name="stringBuilder">Экземпляр класса StringBuilder</param>
        /// <param name="line">Запись с информацией о строке</param>
        /// <param name="lineLength">Длина добавляемой строки</param>
        public static StringBuilder AppendLeftMessage(this StringBuilder stringBuilder, ConsoleLineInfo line, int lineLength)
        {
            var resultLine = line.Message.Reduce(lineLength);

            stringBuilder
                .Append(resultLine.Message)
                .Append(new string(line.CharDisplay, resultLine.Remainder));

            return stringBuilder;
        }

        /// <summary>
        /// Добавляет надпись указанной длины, выравненную по центру и заполненную указанным символом, при наличии свободного пространства 
        /// </summary>
        /// <param name="stringBuilder">Экземпляр класса StringBuilder</param>
        /// <param name="line">Запись с информацией о строке</param>
        /// <param name="lineLength">Длина добавляемой строки</param>
        public static StringBuilder AppendCenterMessage(this StringBuilder stringBuilder, ConsoleLineInfo line, int lineLength)
        {
            var resultLine = line.Message.Reduce(lineLength);

            stringBuilder
                .Append(new string(line.CharDisplay, resultLine.Remainder / 2))
                .Append(resultLine.Message)
                .Append(new string(line.CharDisplay, resultLine.Remainder / 2));

            if (resultLine.Remainder % 2 != 0)
                stringBuilder.Append(line.CharDisplay);

            return stringBuilder;
        }

        /// <summary>
        /// Добавляет надпись указанной длины, выравненную по правому краю и заполненную указанным символом, при наличии свободного пространства 
        /// </summary>
        /// <param name="stringBuilder">Экземпляр класса StringBuilder</param>
        /// <param name="line">Запись с информацией о строке</param>
        /// <param name="lineLength">Длина добавляемой строки</param>
        public static StringBuilder AppendRightMessage(this StringBuilder stringBuilder, ConsoleLineInfo line, int lineLength)
        {
            var resultLine = line.Message.Reduce(lineLength);

            stringBuilder
                .Append(new string(line.CharDisplay, resultLine.Remainder))
                .Append(resultLine.Message);

            return stringBuilder;
        }

        /// <summary>
        /// Добавляет строку указанной длины и заполненную заданным символом, при наличии свободного пространства 
        /// </summary>
        /// <param name="stringBuilder">Экземпляр класса StringBuilder</param>
        /// <param name="line">Запись с информацией о строке</param>
        /// <param name="lineLength">Длина добавляемой строки</param>
        public static StringBuilder AppendEmptyMessage(this StringBuilder stringBuilder, ConsoleLineInfo line, int lineLength)
        {
            stringBuilder.Append(new string(line.CharDisplay, lineLength));
            return stringBuilder;
        }
    }
}