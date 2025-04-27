/*
 * БПИ245-1
 * Калюжная Анна Дмитриевна
 * Вариант 1
 */
using TravelPlannerCore;
namespace TravelPlannerData
{
    /// <summary>
    /// Работа с CSV
    /// </summary>
    public class CsvWorker
    {
        /// <summary>
        /// Читает данные из CSV файла и добавляет их в список путешествий (List<Trip>).
        /// </summary>
        /// <param name="pathToCSV">Путь к CSV файлу, содержащему данные о путешествиях.</param>
        /// <exception cref="FormatException">Выбрасывается, если данные в строках имеют неправильный формат (например, неверный формат даты или числа).</exception>
        /// <exception cref="IndexOutOfRangeException">Выбрасывается, если строка CSV файла не содержит достаточное количество столбцов.</exception>
        /// <exception cref="Exception">Общая ошибка, если возникает неожиданная ошибка в процессе обработки.</exception>
        public static void ReadCsv(string pathToCSV)
        {

            string[] rowData = File.ReadAllLines(pathToCSV);
            string[] splittedRow = new string[rowData.Length];
            string[] splittedDataInQuotes = new string[7];
            for (int i = 1; i < rowData.Length; i++)
            {
                try {
                    splittedDataInQuotes = rowData[i].Split('"');
                    string concatStr = "";
                    for (int j = 0; j < splittedDataInQuotes.Length; j++)
                    {
                        if (j % 2 == 1)
                        {
                            concatStr += splittedDataInQuotes[j].Replace(',', ';');
                        }
                        else
                        {
                            concatStr += splittedDataInQuotes[j];
                        }
                    }
                    splittedRow = concatStr.Split(',');
                    try
                    {
                        int id = int.Parse(splittedRow[0].Replace(';', ','));
                        string country = splittedRow[1].Replace(';', ',');
                        string city = splittedRow[2].Replace(';', ',');
                        DateTime startindDate = DateTime.Parse(splittedRow[3].Replace(';', ','));
                        DateTime endingDate = DateTime.Parse(splittedRow[4].Replace(';', ','));
                        decimal budget = decimal.Parse(splittedRow[5].Replace(';', ','));
                        List<string> attractions = splittedRow[6].Replace(';', ',').Split(',').ToList();
                        Trip.trips.Add(new Trip(id, country, city, startindDate, endingDate, attractions, budget));
                        Trip.counter++;
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
            }
        }
        /// <summary>
        /// Записывает данные в файл CSV по указанному пути.
        /// </summary>
        /// <param name="path">Путь к файлу, в который нужно записать данные.</param>
        /// <param name="data">Список строк, которые будут записаны в файл.</param>
        /// <exception cref="ArgumentException">Вызывается, если путь к файлу является недействительным.</exception>
        /// <exception cref="UnauthorizedAccessException">Вызывается, если приложение не имеет прав на доступ к файлу.</exception>
        /// <exception cref="DirectoryNotFoundException">Вызывается, если указанный каталог не существует.</exception>
        /// <exception cref="IOException">Вызывается при других ошибках ввода-вывода, например, при отсутствии свободного места на диске.</exception>
        /// <exception cref="Exception">Общие ошибки выполнения.</exception>
        public static void WriteToCsv(string path, List<string> data)
        {
            try
            {
                File.WriteAllLines(path, data);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Генерирует список строк, представляющих данные о путешествиях в формате CSV.
        /// </summary>
        /// <returns>Список строк, каждая из которых представляет данные о путешествии в формате CSV, с значениями, заключенными в кавычки и разделенными запятыми.</returns>
        public static List<string> MadeDataForCsv()
        {
            List <string> data = new List<string>();
            foreach (Trip trip in Trip.trips)
            {
                data.Add($"\"{trip.Country}\", \"{trip.City}\", \"{trip.StartDate}\", \"{trip.EndDate}\", \"{trip.GetAttractions()}\", \"{trip.Budget}\"");
            }
            return data;
        }
    }
}
