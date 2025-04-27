/*
 * БПИ245-1
 * Калюжная Анна Дмитриевна
 * Вариант 1
 */
using System.Text.Json;
using TravelPlannerCore;

namespace TravelPlannerData
{
    /// <summary>
    /// Класс для работы с JSON
    /// </summary>
    public class JsonWorker
    {
        /// <summary>
        /// Читает данные из JSON-файла и десериализует их в список объектов типа <see cref="Trip"/>.
        /// </summary>
        /// <param name="jsonPath">Путь к JSON-файлу, содержащему данные поездок.</param>
        /// <exception cref="UnauthorizedAccessException">Если нет прав доступа к файлу.</exception>
        /// <exception cref="JsonException">Если произошла ошибка при десериализации данных JSON.</exception>
        /// <exception cref="Exception">Если произошла любая другая ошибка.</exception>
        public static void ReadJson(string jsonPath)
        {
            try
            {
                string json = File.ReadAllText(jsonPath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };
                List<Trip>? trips = JsonSerializer.Deserialize<List<Trip>>(json, options);
                if (trips != null && trips.Count > 0)
                {
                    Trip.trips = trips;
                }
                else
                {
                    Console.WriteLine("Список поездок пуст.");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (JsonException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Сериализует список поездок в JSON и сохраняет его в указанный файл.
        /// </summary>
        /// <param name="jsonPath">Путь, по которому будет сохранен JSON-файл.</param>
        /// <exception cref="UnauthorizedAccessException">Если нет прав доступа для записи в файл.</exception>
        /// <exception cref="DirectoryNotFoundException">Если указанная директория не найдена.</exception>
        /// <exception cref="IOException">Если произошла ошибка ввода-вывода при записи в файл.</exception>
        /// <exception cref="JsonException">Если произошла ошибка при сериализации объекта в JSON.</exception>
        /// <exception cref="Exception">Если произошла любая другая ошибка.</exception>
        public static void WriteJson(string jsonPath)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                string json = JsonSerializer.Serialize(Trip.trips, options);
                File.WriteAllText(jsonPath, json);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (JsonException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
