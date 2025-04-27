/*
 * БПИ245-1
 * Калюжная Анна Дмитриевна
 * Вариант 1
 */
using Spectre.Console;
using SkiaSharp;
using System.Text.Json;
using System.Globalization;
namespace TravelPlannerCore
{
    /// <summary>
    /// Класс для работы с поездками
    /// </summary>
    public class TripService
    {
        /// <summary>
        /// Отображает интерактивную таблицу с данными о поездках, с возможностью фильтрации и сортировки.
        /// </summary>
        /// <param name="trip">Список объектов <see cref="Trip"/>, которые будут отображены в таблице.</param>
        /// <remarks>
        /// Пользователь может добавлять и удалять фильтры и сортировки с помощью команд в меню:
        /// <list type="bullet">
        ///     <item><description>f: Добавить фильтр</description></item>
        ///     <item><description>d: Удалить фильтр</description></item>
        ///     <item><description>s: Добавить сортировку</description></item>
        ///     <item><description>x: Удалить сортировку</description></item>
        ///     <item><description>q: Выход</description></item>
        /// </list>
        /// Метод выполняет следующие шаги:
        /// <ol>
        ///     <li>Создаёт таблицу и отображает ее пользователю.</li>
        ///     <li>Обрабатывает введенные фильтры и сортировки.</li>
        ///     <li>Отображает обновленную таблицу после применения фильтров и сортировок.</li>
        /// </ol>
        /// </remarks>
        /// <exception cref="ArgumentException">Выбрасывается, если фильтр или сортировка заданы некорректно (например, поле не найдено).</exception>
        /// <exception cref="KeyNotFoundException">Выбрасывается, если фильтр или сортировка для удаления не найдены в коллекции.</exception>
        /// <exception cref="InvalidOperationException">Выбрасывается, если происходит ошибка при отображении таблицы.</exception>
        public static void ShowInteractiveTable(List<Trip> trip)
        {
            var trips = trip;
            var filters = new Dictionary<string, string>();
            var sorters = new List<(string Field, bool Ascending)>();
            bool exit = false;

            while (!exit)
            {

                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .AddColumn(new TableColumn("[bold]ID[/]").Centered())
                    .AddColumn(new TableColumn("[bold]Страна[/]").Centered())
                    .AddColumn(new TableColumn("[bold]Город[/]").Centered())
                    .AddColumn(new TableColumn("[bold]Дата начала[/]").Centered())
                    .AddColumn(new TableColumn("[bold]Дата конца[/]").Centered())
                    .AddColumn(new TableColumn("[bold]Погода[/]").Centered())
                    .AddColumn(new TableColumn("[bold]Бюджет[/]").Centered());
                var filteredTrips = trips.AsEnumerable();
                foreach (var filter in filters)
                {
                    filteredTrips = filter.Key switch
                    {
                        "Страна" => filteredTrips.Where(p => p.Country.Contains(filter.Value, StringComparison.OrdinalIgnoreCase)),
                        "Город" => filteredTrips.Where(p => p.City.Contains(filter.Value, StringComparison.OrdinalIgnoreCase)),
                        "Дата начала" => filteredTrips.Where(p => p.StartDate.ToString("yyyy-MM-dd").Contains(filter.Value)),
                        "Дата конца" => filteredTrips.Where(p => p.EndDate.ToString("yyyy-MM-dd").Contains(filter.Value)),
                        "Бюджет" => filteredTrips.Where(p => p.Budget.ToString(CultureInfo.InvariantCulture).Contains(filter.Value)),
                        _ => filteredTrips
                    };
                }
                IOrderedEnumerable<Trip>? sortedTrips = null;

                foreach (var sorter in sorters)
                {
                    sortedTrips = sorter switch
                    {
                        ("Страна", true) => sortedTrips == null ? filteredTrips.OrderBy(p => p.Country) : sortedTrips.ThenBy(p => p.Country),
                        ("Страна", false) => sortedTrips == null ? filteredTrips.OrderByDescending(p => p.Country) : sortedTrips.ThenByDescending(p => p.Country),
                        ("Город", true) => sortedTrips == null ? filteredTrips.OrderBy(p => p.City) : sortedTrips.ThenBy(p => p.City),
                        ("Город", false) => sortedTrips == null ? filteredTrips.OrderByDescending(p => p.City) : sortedTrips.ThenByDescending(p => p.City),
                        ("Дата начала", true) => sortedTrips == null ? filteredTrips.OrderBy(p => p.StartDate) : sortedTrips.ThenBy(p => p.StartDate),
                        ("Дата начала", false) => sortedTrips == null ? filteredTrips.OrderByDescending(p => p.StartDate) : sortedTrips.ThenByDescending(p => p.StartDate),
                        ("Дата конца", true) => sortedTrips == null ? filteredTrips.OrderBy(p => p.EndDate) : sortedTrips.ThenBy(p => p.EndDate),
                        ("Дата конца", false) => sortedTrips == null ? filteredTrips.OrderByDescending(p => p.EndDate) : sortedTrips.ThenByDescending(p => p.EndDate),
                        ("Бюджет", true) => sortedTrips == null ? filteredTrips.OrderBy(p => p.Budget) : sortedTrips.ThenBy(p => p.Budget),
                        ("Бюджет", false) => sortedTrips == null ? filteredTrips.OrderByDescending(p => p.Budget) : sortedTrips.ThenByDescending(p => p.Budget),
                        _ => sortedTrips
                    };
                }

                var finalTrips = sortedTrips ?? filteredTrips;


                foreach (var tripItem in finalTrips)
                {
                    table.AddRow(
                        tripItem.Id.ToString(),
                        tripItem.Country,
                        tripItem.City,
                        tripItem.StartDate.ToString(),
                        tripItem.EndDate.ToString(),
                        tripItem.Weather.ToString(),
                        $"${tripItem.Budget:F2}"
                    );
                }

                var panel = new Panel($"Фильтры: {Markup.Escape(filters.Count > 0 ? string.Join(", ", filters.Select(f => $"{f.Key}: {f.Value}")) : "Нет")}\n" +
            $"Сортировки: {Markup.Escape(sorters.Count > 0 ? string.Join(", ", sorters.Select(s => $"{s.Field} ({(s.Ascending ? "ASC" : "DESC")})")) : "Нет")}"
        )
                    .Border(BoxBorder.Double)
                    .Header("Панель управления")
                    .Expand();

                var layout = new Layout("Root")
                    .SplitRows(
                        new Layout("Таблица").Ratio(3).Update(table),
                        new Layout("Панель управления").Ratio(1).Update(panel)
                    );

                AnsiConsole.Write(layout);

                AnsiConsole.MarkupLine("\n[bold cyan]Команды:[/] f: Добавить фильтр | d: Удалить фильтр | s: Добавить сортировку | x: Удалить сортировку | q: Выход");
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.F:
                        string newFilter = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Отфильтровать по:")
                                .AddChoices("Id", "Страна", "Город", "Дата начала", "Дата конца", "Бюджет"));
                        string filterValue = AnsiConsole.Ask<string>($"Введите фильтр [{newFilter}]: ");
                        filters[newFilter] = filterValue;
                        break;

                    case ConsoleKey.D:
                        if (filters.Count > 0)
                        {
                            string filterToRemove = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                    .Title("Выберите фильтр для удаления:")
                                    .AddChoices(filters.Keys.ToArray()));

                            filters.Remove(filterToRemove);
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Нет активных фильтров![/]");
                        }
                        break;

                    case ConsoleKey.S:
                        string newSorter = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Отсортировать:")
                                .AddChoices("Id", "Страна", "Город", "Дата начала", "Дата конца", "Бюджет"));
                        bool ascending = AnsiConsole.Confirm("Сортировать по возрастанию?");
                        sorters.Add((newSorter, ascending));
                        break;

                    case ConsoleKey.X:
                        if (sorters.Count > 0)
                        {
                            string sorterToRemove = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                    .Title("Выберите сортировку для удаления:")
                                    .AddChoices(sorters.Select(s => $"{s.Field} ({(s.Ascending ? "ASC" : "DESC")})").ToArray()));

                            sorters.RemoveAll(s => $"{s.Field} ({(s.Ascending ? "ASC" : "DESC")})" == sorterToRemove);
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Нет активных сортировок![/]");
                        }
                        break;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.Q:
                        exit = true;
                        break;

                }
            }
        }

        /// <summary>
        /// Отображает карту с маршрутами, основанную на данных поездок, и сохраняет обновленную карту в указанный файл.
        /// </summary>
        /// <param name="choors">Список объектов <see cref="Trip"/>, содержащих информацию о поездках, которые будут отображены на карте.</param>
        ///  <exception cref="ArgumentNullException">Выбрасывается, если параметр <paramref name="choors"/> равен null.</exception>
        /// <exception cref="FileNotFoundException">Выбрасывается, если исходный файл карты по пути <paramref name="mapPath"/> не найден.</exception>
        /// <exception cref="IOException">Выбрасывается, если произошла ошибка при записи нового изображения в файл.</exception>
        public static void ShowRoutePointsMap(List<Trip> choors)
        {
            string mapPath = @"../../../../map.jpg";
            string mapOutputPath = @"../../../../new_map.png";
            DrawMapWithCities(mapPath, mapOutputPath, choors);
        }
        /// <summary>
        /// Отображает на карте мира, соответствующие поездкам из списка, и сохраняет обновленное изображение карты в файл.
        /// </summary>
        /// <param name="mapPath">Путь к исходному изображению карты, на которой будут отмечены города.</param>
        /// <param name="outputPath">Путь к файлу, в который будет сохранено обновленное изображение карты с отмеченными городами.</param>
        /// <param name="choors">Список объектов <see cref="Trip"/>, содержащих информацию о поездках и соответствующих им городах, которые будут отображены на карте.</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если параметр <paramref name="choors"/> равен null.</exception>
        /// <exception cref="FileNotFoundException">Выбрасывается, если исходный файл карты по пути <paramref name="mapPath"/> не найден.</exception>
        /// <exception cref="IOException">Выбрасывается, если произошла ошибка при записи обновленной карты в файл по пути <paramref name="outputPath"/>.</exception>
        /// <exception cref="InvalidOperationException">Выбрасывается, если возникают ошибки при рисовании на изображении (например, неправильные координаты).</exception>
        static void DrawMapWithCities(string mapPath, string outputPath, List<Trip> choors)
        {
            using SKBitmap mapBitmap = SKBitmap.Decode(mapPath);
            using SKCanvas canvas = new SKCanvas(mapBitmap);
            int mapWidth = mapBitmap.Width;
            int mapHeight = mapBitmap.Height;

            using SKPaint textPaint = new SKPaint
            {
                Color = SKColors.Black,
                IsAntialias = true,
            };

            using SKPaint cityPaint = new SKPaint
            {
                Color = SKColors.Red,
                IsAntialias = true
            };

            foreach (var city in choors)
            {
                (float x, float y) = ConvertGeoToPixel(city.Lat, city.Lon, mapWidth, mapHeight);

                canvas.DrawCircle(x, y, 5, cityPaint);
            }

            SaveBitmapToFile(mapBitmap, outputPath);
            var image = new CanvasImage(outputPath);
            AnsiConsole.Write(image);
        }

        /// <summary>
        /// Преобразует географические координаты (широту и долготу) в пиксельные координаты на изображении карты.
        /// </summary>
        /// <param name="lat">Широта в градусах. Значения должны быть в пределах от -90 до 90.</param>
        /// <param name="lon">Долгота в градусах. Значения должны быть в пределах от -180 до 180.</param>
        /// <param name="mapWidth">Ширина карты в пикселях.</param>
        /// <param name="mapHeight">Высота карты в пикселях.</param>
        static (float X, float Y) ConvertGeoToPixel(double lat, double lon, int mapWidth, int mapHeight)
        {
            float x = (float)((lon + 180) / 360.0 * mapWidth);
            double latRad = lat * Math.PI / 180.0;
            float y = (float)((0.5 - Math.Log(Math.Tan(Math.PI / 4 + latRad / 2)) / (2 * Math.PI)) * mapHeight);
            return (x, y);
        }
        /// <summary>
        /// Сохраняет изображение в файл в формате PNG.
        /// </summary>
        /// <param name="bitmap">Объект <see cref="SKBitmap"/>, представляющий изображение, которое нужно сохранить.</param>
        /// <param name="outputPath">Путь к файлу, в который будет сохранено изображение. Путь должен быть действительным и доступным для записи.</param>
        /// <exception cref="UnauthorizedAccessException">
        /// Выбрасывается, если у процесса нет прав на запись в указанный файл или каталог.
        /// </exception>
        /// <exception cref="IOException">
        /// Выбрасывается, если возникла ошибка при записи файла, например, если файл занят другим процессом.
        /// </exception>
        /// <exception cref="Exception">
        /// Ловит все остальные исключения, которые могут возникнуть при выполнении операции сохранения изображения.
        /// </exception>
        static void SaveBitmapToFile(SKBitmap bitmap, string outputPath)
        {
            try
            {
                using SKImage image = SKImage.FromBitmap(bitmap);
                using SKData data = image.Encode(SKEncodedImageFormat.Png, 100);
                File.WriteAllBytes(outputPath, data.ToArray());
            }
            catch (UnauthorizedAccessException ex)
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
        /// Получает географические координаты (широту и долготу) города в указанной стране через API.
        /// </summary>
        /// <param name="city">Название города, для которого нужно получить координаты.</param>
        /// <param name="country">Название страны, где находится город.</param>
        /// <returns>
        /// Кортеж с двумя значениями: широта и долгота (в градусах). Если координаты не найдены или произошла ошибка, возвращает (0, 0).
        /// </returns>
        /// <exception cref="HttpRequestException">
        /// Выбрасывается при ошибках HTTP запроса, например, если API не доступен.
        /// </exception>
        /// <exception cref="JsonException">
        /// Выбрасывается, если не удалось правильно разобрать ответ от API (например, ошибка в формате JSON).
        /// </exception>
        /// <exception cref="Exception">
        /// Выбрасывается в случае других ошибок, например, если координаты не найдены в ответе API.
        /// </exception>
        public static (double lat, double lon) GetCoordinates(string city, string country)
        {
            using HttpClient client = new();
            string apiKey = "674986a2-969f-4bd5-b21a-9fa39f76ce17";
            try
            {
                string query = Uri.EscapeDataString($"{city}, {country}");
                string url = $"https://catalog.api.2gis.com/3.0/items?q={query}&key={apiKey}&fields=items.point";
                HttpResponseMessage response = client.GetAsync(url).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                JsonDocument jsonDoc = JsonDocument.Parse(responseBody);
                JsonElement root = jsonDoc.RootElement;

                if (!root.TryGetProperty("result", out JsonElement result) || !result.TryGetProperty("items", out JsonElement items) || items.ValueKind != JsonValueKind.Array || items.GetArrayLength() == 0)
                {
                    throw new Exception("Координаты не найдены.");
                }
                JsonElement location = items[0];

                if (location.TryGetProperty("point", out JsonElement point) && point.TryGetProperty("lat", out JsonElement latProp) && point.TryGetProperty("lon", out JsonElement lonProp))
                {
                    double lat = latProp.GetDouble();
                    double lon = lonProp.GetDouble();
                    return (lat, lon);
                }
                else
                {
                    throw new Exception("Координаты не найдены.");
                }

            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return (0, 0);
            }
            catch (JsonException ex)
            {
                Console.WriteLine(ex.Message);
                return (0, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (0, 0);
            }
        }
        /// <summary>
        /// Выводит информацию о месте (название, адрес, рейтинг, описание) в консоль.
        /// </summary>
        /// <param name="placeName">Название места.</param>
        /// <param name="address">Адрес места.</param>
        /// <param name="rating">Рейтинг места (в числовом формате).</param>
        /// <param name="description">Описание места.</param>
        public static void WritePlaceInfo(string placeName, string address, double rating, string description)
        {
            Console.WriteLine($"Название: {placeName}");
            Console.WriteLine($"Адрес: {address}");
            Console.WriteLine($"Рейтинг: {rating}");
            Console.WriteLine($"Описание: {description}");
        }
        /// <summary>
        /// Ищет информацию о достопримечательности по названию, городу и стране с использованием API 2GIS.
        /// </summary>
        /// <param name="country">Страна, в которой находится достопримечательность.</param>
        /// <param name="city">Город, в котором находится достопримечательность.</param>
        /// <param name="name">Название достопримечательности.</param>
        /// <returns>
        /// Этот метод не возвращает значение, но может выводить информацию о месте в консоль.
        /// </returns>
        /// <exception cref="HttpRequestException">
        /// Выбрасывается, если запрос к API 2GIS не был успешным (например, проблемы с сетью или неверный URL).
        /// </exception>
        /// <exception cref="JsonException">
        /// Выбрасывается, если происходит ошибка при обработке JSON-ответа (например, ошибка парсинга).
        /// </exception>
        /// <exception cref="Exception">
        /// Выбрасывается, если не удается найти достопримечательность в ответе или возникла любая другая ошибка в процессе.
        /// </exception>
        public static void Find2GIS(string country, string city, string name)
        {
            string apiKey = "674986a2-969f-4bd5-b21a-9fa39f76ce17";
            using HttpClient client = new();

            try
            {
                string query = Uri.EscapeDataString($"{name}, {city}, {country}");
                string url = $"https://catalog.api.2gis.com/3.0/items?q={query}&key={apiKey}";
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };
                JsonDocument jsonDoc = JsonDocument.Parse(responseBody);
                JsonElement root = jsonDoc.RootElement;
                if (!root.TryGetProperty("result", out JsonElement result) || !result.TryGetProperty("items", out JsonElement items) || items.ValueKind != JsonValueKind.Array || items.GetArrayLength() == 0)
                {
                    Console.WriteLine("Достопримечательность не найдена.");
                    return;
                }
                foreach (JsonElement place in items.EnumerateArray())
                {
                    try
                    {
                        string placeName = place.TryGetProperty("name", out JsonElement nameElem)
                            ? nameElem.GetString() ?? "Неизвестно"
                            : "Неизвестно";

                        string address = place.TryGetProperty("address_name", out JsonElement addressElem)
                            ? addressElem.GetString() ?? "Адрес не указан"
                            : "Адрес не указан";

                        string description = place.TryGetProperty("description", out JsonElement descElem)
                            ? descElem.GetString() ?? "Нет описания"
                            : "Нет описания";

                        double rating = place.TryGetProperty("reviews", out JsonElement reviewsElem) &&
                                        reviewsElem.TryGetProperty("rating", out JsonElement ratingElem)
                            ? ratingElem.GetDouble()
                            : 0.0;

                        WritePlaceInfo(placeName, address, rating, description);
                    
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (HttpRequestException ex)
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
