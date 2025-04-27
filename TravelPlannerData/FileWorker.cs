/*
 * БПИ245-1
 * Калюжная Анна Дмитриевна
 * Вариант 1
 */
namespace TravelPlannerData
{
    public class FileWorker
    {
        /// <summary>
        /// Инициализация "входного" пути
        /// </summary>
        private string _inputPath = string.Empty;
        /// <summary>
        /// Инициализация "выходного" пути
        /// </summary>
        private string _outputPath = string.Empty;
        /// <summary>
        /// Хранение неккоретных значений для пути
        /// </summary>
        static char[] invalidPathChars = Path.GetInvalidPathChars();
        /// <summary>
        /// Получает или задает путь к файлу для ввода данных. Путь должен быть корректным и указывать на файл с расширением .csv, .json или .txt.
        /// </summary>
        /// <exception cref="Exception">Если путь содержит некорректные символы или если расширение файла не является .csv, .json или .txt, выбрасывается исключение с соответствующим сообщением.</exception>
        /// <exception cref="FileNotFoundException">Если файл не найден по указанному пути, выбрасывается исключение.</exception>
        /// <exception cref="DirectoryNotFoundException">Если указанная директория не существует, выбрасывается исключение.</exception>
        /// <exception cref="UnauthorizedAccessException">Если у пользователя нет прав для доступа к файлу, выбрасывается исключение.</exception>
        /// <exception cref="ArgumentNullException">Если передан null в качестве пути, выбрасывается исключение.</exception>
        public string InputPath
        {
            get { return _inputPath; }
            set
            {
                try
                {
                    if (value.IndexOfAny(invalidPathChars) != -1)
                    { 
                        throw new Exception("Некорректные данные");
                    }
                    if ((value.Split(".")[^1] != "csv") | (value.Split(".")[^1] != "json") | (value.Split(".")[^1] != "txt"))
                    {
                        throw new Exception("Файл не csv/json");
                    }
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Файл не найден");
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("Неверная директория");
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("Недостаточно прав для чтения файла");
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine("Может быть файл null");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
                _inputPath = value;
            }
        }
        /// <summary>
        /// Получает или задает путь к файлу для вывода данных.
        /// </summary>
        /// <exception cref="Exception">Если путь содержит некорректные символы, выбрасывается исключение с соответствующим сообщением.</exception>
        /// <exception cref="ArgumentNullException">Если передан null в качестве пути, выбрасывается исключение.</exception>
        public string OutputPath
        {
            get { return _outputPath; }
            set
            {
                try
                {
                    if (value.IndexOfAny(invalidPathChars) != -1)
                    {
                        throw new Exception("Некорректные данные");
                    }
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine("Может быть файл null");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
                _outputPath = value;
            }
        }
    }
}
