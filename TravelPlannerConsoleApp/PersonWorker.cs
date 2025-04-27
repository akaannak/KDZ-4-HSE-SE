/*
 * БПИ245-1
 * Калюжная Анна Дмитриевна
 * Вариант 1
 */
using TravelPlannerCore;
using TravelPlannerData;
namespace TravelPlannerConsoleApp
{
    /// <summary>
    /// Класс для взаимодействия с пользователем
    /// </summary>
    public class PersonWorker
    {
        /// <summary>
        /// Парсер для беззнакового числа. Если ввод некорректен, выводит сообщение об ошибке и запрашивает ввод повторно.
        /// </summary>
        /// <returns>Беззнаковое число</returns>
        public static uint ReadNumber()
        {
            uint k;
            while (!uint.TryParse(Console.ReadLine(), out k))
            {
                Console.WriteLine("Вы ввели некорректные данные. Повторите попытку");
            }
            return k;
        }
        /// <summary>
        /// Парсер для беззнакового числа в определенном диапазоне. Если ввод некорректен, выводит сообщение об ошибке и запрашивает ввод повторно.
        /// </summary>
        /// <param name="n">Правая граница числа</param>
        /// <returns>Безннаковое число в определенном диапазоне</returns>
        public static int ReadNumber(int n)
        {
            int k;
            while (!int.TryParse(Console.ReadLine(), out k) || k > n || k < 1)
            {
                Console.WriteLine("Вы ввели некорректные данные.");
                Console.WriteLine($"Повторите попытку и введите число от 1 до {n}");
            }
            return k;
        }
        /// <summary>
        /// Парсер для строки только из букв. Если ввод некорректен, выводит сообщение об ошибке и запрашивает ввод повторно.
        /// </summary>
        /// <returns>Строку, состоящую только из букв</returns>
        public static string ReadNaming()
        {
            string str = string.Empty;
            do
            {
                str = Console.ReadLine()!;
                Console.WriteLine("Вы ввели некорректные данные.");
                Console.WriteLine("Строка состоит не только из букв. Повторите попытку");
            }
            while (string.IsNullOrEmpty(str) && str.Any(c => !char.IsLetter(c)));
            return str;
        }
        /// <summary>
        /// Парсер для даты. Если ввод некорректен, выводит сообщение об ошибке и запрашивает ввод повторно.
        /// </summary>
        /// <returns>Дата</returns>
        public static DateTime ReadDateTime()
        {
            DateTime dt;
            while (!DateTime.TryParse(Console.ReadLine(), out dt))
            {
                Console.WriteLine("Вы ввели некорректные данные. Повторите попытку");
            }
            return dt;
        }
        /// <summary>
        /// Парсер для числа типа decimal. Если ввод некорректен, выводит сообщение об ошибке и запрашивает ввод повторно.
        /// </summary>
        /// <returns>Число типа decimal</returns>
        public static decimal ReadBudget()
        {
            decimal summa;
            while (!decimal.TryParse(Console.ReadLine(), out summa))
            {
                Console.WriteLine("Вы ввели некорректные данные. Повторите попытку");
            }
            return summa;
        }
        /// <summary>
        /// Добавление новой поездки
        /// </summary>
        /// <returns>Новую поездку</returns>
        public static Trip AddNewTrip()
        {
            Console.WriteLine("Введите название страны");
            string country = Console.ReadLine()!;
            Console.WriteLine("Введите название города");
            string city = Console.ReadLine()!;
            Console.WriteLine("Введите дату начала поездки");
            DateTime dateStarting = ReadDateTime();
            Console.WriteLine("Введите дату окончания поездки");
            DateTime dateEnding = ReadDateTime();
            Console.WriteLine("Какое количество достопримечательностей вы хотите посетить?");
            uint number = ReadNumber();
            List<string> attractions = new();
            Console.WriteLine("Введите достопримечательности. Каждую с новой строки:");
            for (int i = 0; i < number; i++)
            {
                attractions.Add(Console.ReadLine()!);
            }
            Console.WriteLine("Какой предполагаемый бюджет у поездки?");
            decimal budget = ReadBudget();
            Trip trip = new Trip(Trip.counter, country, city, dateStarting, dateEnding, attractions, budget);
            (trip.Lon, trip.Lat) = TripService.GetCoordinates(city, country);
            Weather weather = new();
            weather.GetWeather(trip.Lon.ToString(), trip.Lat.ToString(), dateStarting);
            trip.Weather = weather;
            Trip.counter ++;
            return trip;
        }
        /// <summary>
        /// Метод для работы с меню
        /// </summary>
        /// <param name="k">Номер меню</param>
        /// <param name="fileWorker">Экземпляр класса для работы с файлами</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Значение не соответствует допустимому диапазону (например, если введен несуществующий номер поездки).
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Переданный аргумент имеет значение null, например, при попытке найти поездку по пустому идентификатору.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Переданный аргумент имеет недопустимое значение, например, неверный формат даты или номера поездки.
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        /// Не удается найти поездку по заданному идентификатору, или если указанный ключ не существует в коллекции.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// Файл не найден по указанному пути при попытке загрузить данные из файла (CSV/JSON).
        /// </exception>
        /// <exception cref="IOException">
        /// Ошибки ввода/вывода, например, если файл занят другим процессом или возникла ошибка при записи данных.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// У программы нет прав на доступ к файлу (например, если файл доступен только для чтения).
        /// </exception>
        /// <exception cref="Exception">
        /// Общий блок для перехвата других ошибок, которые могут возникнуть в процессе выполнения метода.
        /// </exception>

        public static void WorkingWithMenu(int k, FileWorker fileWorker)
        {
            
            switch (k)
            {
                case 1:
                    Menu.ShowMenuOne();
                    int k1 = ReadNumber(2);
                    switch (k1)
                    {
                        case 1:
                            Trip.trips.Clear();
                            Trip.counter = 0;
                            Menu.AskForCount();
                            uint number = ReadNumber();
                            for (int i = 0; i < number; i++)
                            {
                                Trip.trips.Add(AddNewTrip());
                            }
                            break;
                        case 2:
                            Trip.trips.Clear();
                            Trip.counter = 0;
                            Console.WriteLine("Введите абсолютный путь до файла:");
                            
                            fileWorker.InputPath = Console.ReadLine()!;
                            if (fileWorker.InputPath.Split(".")[^1] == "csv")
                            {
                                CsvWorker.ReadCsv(fileWorker.InputPath);
                            } else if (fileWorker.InputPath.Split(".")[^1] == "json")
                            {
                                JsonWorker.ReadJson(fileWorker.InputPath);
                            }
                            break;
                    }
                    break;
                case 2:
                    if (Trip.trips != null && Trip.trips.Count > 0)
                    {
                        try
                        {
                            TripService.ShowInteractiveTable(Trip.trips);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    } else
                    {
                        Console.WriteLine("Сначала введите данные!");
                    }
                    break;
                case 3:
                    if (Trip.trips != null && Trip.trips.Count > 0)
                    {
                        try
                        {
                            TripService.ShowRoutePointsMap(Trip.trips);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Сначала введите данные!");
                    }
                    break;
                case 4:
                    Trip.trips.Add(AddNewTrip());    
                    break;
                case 5:
                    Menu.AskForId();
                    try
                    {
                        Trip.ChangeTrip(Trip.FindTripNumber(Console.ReadLine()!), AddNewTrip());
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (ArgumentNullException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (KeyNotFoundException e){
                        Console.WriteLine(e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;
                case 6:
                    Menu.AskForId();
                    try
                    {   
                        Trip.DeleteTrip(Trip.FindTripNumber(Console.ReadLine()!));
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (KeyNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;
                case 7:
                    if (Trip.trips != null && Trip.trips.Count > 0)
                    {
                        Menu.AskForId();
                        try
                        {
                            int id = Trip.FindTripNumber(Console.ReadLine()!);
                            if (Trip.trips[id].Attractions != null && Trip.trips[id].Attractions.Count > 0)
                            {
                                foreach (string name in Trip.trips[id].Attractions)
                                {
                                    TripService.Find2GIS(Trip.trips[id].Country, Trip.trips[id].City, name);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Нет достопримечательностей для просмотра");
                            }
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        catch (KeyNotFoundException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    } else
                    {
                        Console.WriteLine("Сначала введите данные!");
                    }
                    break;
                case 8:
                    if (Trip.trips != null && Trip.trips.Count > 0)
                    {
                        Menu.AskForId();
                        try
                        {
                            int tripNumber = Trip.FindTripNumber(Console.ReadLine()!);
                            Console.WriteLine("Введите дату");
                            DateTime date = ReadDateTime();
                            Weather weather = new();
                            weather.GetWeather(Trip.trips[tripNumber].Lon.ToString(), Trip.trips[tripNumber].Lat.ToString(), date);
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        catch (KeyNotFoundException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    } else
                    {
                        Console.WriteLine("Сначала введите данные!");
                    }
                    break;
                case 9:
                    break;
                case 10:
                    if (Trip.trips != null && Trip.trips.Count > 0)
                    {
                        if (fileWorker.InputPath != string.Empty)
                        {
                            if (fileWorker.InputPath.Split(".")[^1] == "csv")
                            {
                                fileWorker.OutputPath = fileWorker.InputPath;
                                CsvWorker.WriteToCsv(fileWorker.OutputPath, CsvWorker.MadeDataForCsv());
                            }
                            else if (fileWorker.InputPath.Split(".")[^1] == "json")
                            {
                                fileWorker.OutputPath = fileWorker.InputPath;
                                JsonWorker.WriteJson(fileWorker.InputPath);
                            }
                            
                        }
                        else
                        {
                            Console.WriteLine("Введите абсолютный путь до файла:");
                            fileWorker.OutputPath = Console.ReadLine()!;
                            JsonWorker.WriteJson(fileWorker.OutputPath);
                        }
                    }
                        Environment.Exit(0);
                    break;

            }
        }
    }
}
