/*
 * БПИ245-1
 * Калюжная Анна Дмитриевна
 * Вариант 1
 */
using System.Globalization;
using System.Text.Json;

namespace TravelPlannerCore
{
    /// <summary>
    /// Класс погоды
    /// </summary>
    public class Weather
    {
        private string ?_weatherDisc;
        private double _weatherTemp;
        private double _weatherHumidity;
        private double _weatherCloudiness;
        private double _weatherWindSpeed;
        private double _weaterRain;
        private double _weatherFeelsLike;
        
        /// <summary>
        /// Конструктор погоды
        /// </summary>
        public Weather()
        {
        }
        /// <summary>
        /// Переопределение конструктора
        /// </summary>
        /// <param name="weatherDisc">Описание</param>
        /// <param name="weatherTemp">Температура</param>
        /// <param name="weatherHumidity">Влажность</param>
        /// <param name="weatherCloudiness">Облачность</param>
        /// <param name="weatherWindSpeed">Скорость ветра</param>
        /// <param name="weaterRain">Осадки</param>
        /// <param name="weatherFeelsLike">Ощущение температуры</param>
        public Weather(string weatherDisc, double weatherTemp, double weatherHumidity, double weatherCloudiness, double weatherWindSpeed, double weaterRain, double weatherFeelsLike)
        {
            _weatherDisc = weatherDisc;
            _weatherTemp = weatherTemp;
            _weatherHumidity = weatherHumidity;
            _weatherCloudiness = weatherCloudiness;
            _weatherWindSpeed = weatherWindSpeed;
            _weaterRain = weaterRain;
            _weatherFeelsLike = weatherFeelsLike;
         }
        /// <summary>
        /// Описание погодных условий для данного места.
        /// </summary>
        public string? WeatherDisc
        {
            get { return _weatherDisc; }
            set { _weatherDisc = value; }
        }

        /// <summary>
        /// Температура воздуха в месте назначения (в градусах Цельсия).
        /// </summary>
        public double WeatherTemp
        { 
            get { return _weatherTemp; } 
            set { _weatherTemp = value; }
        }
        /// <summary>
        /// Влажность воздуха в месте назначения (в процентах).
        /// </summary>
        public double WeatherHumidity
        { 
            get { return _weatherHumidity; }
            set { _weatherHumidity = value; }
        }

        /// <summary>
        /// Процент облачности в месте назначения.
        /// </summary>
        public double WeatherCloudiness
        { 
            get { return _weatherCloudiness; } 
            set { _weatherCloudiness = value; }
        }
        /// <summary>
        /// Скорость ветра в месте назначения (в м/с).
        /// </summary>
        public double WeatherWindSpeed
        {
            get { return _weatherWindSpeed; }
            set { _weatherWindSpeed = value; }
        }
        /// <summary>
        /// Количество осадков в месте назначения (в мм).
        /// </summary>
        public double WeatherRain 
        { 
            get { return _weaterRain; }
            set { _weaterRain = value; }
        }
        /// <summary>
        /// Ощущаемая температура воздуха (в градусах Цельсия).
        /// </summary>
        public double WeatherFeelsLike
        {
            get { return _weatherFeelsLike; }
            set { _weatherFeelsLike = value; }
        }
        /// <summary>
        /// Переопределение метода TOString()
        /// </summary>
        /// <returns>Отформатированную строку</returns>
        public override string ToString()
        { 
            return $"Описание: {WeatherDisc}\nТемпература: {WeatherTemp}°C (ощущается как {WeatherFeelsLike} °C)\nВлажность: {WeatherHumidity}%\nОблачность: {WeatherCloudiness}%\nВетер: {WeatherWindSpeed}м/с\nОсадки: {WeatherRain}мм\n";
        }
        /// <summary>
        /// Получает данные о погоде с API OpenWeatherMap для заданных географических координат (долготы и широты) на указанную дату.
        /// </summary>
        /// <param name="lon">Долгота местоположения, для которого необходимо получить информацию о погоде.</param>
        /// <param name="lat">Широта местоположения, для которого необходимо получить информацию о погоде.</param>
        /// <param name="date">Дата, для которой запрашиваются данные о погоде (на данный момент не используется, но включена для возможного будущего использования).</param>
        /// <exception cref="HttpRequestException">Выбрасывается в случае ошибки при отправке HTTP-запроса к API OpenWeatherMap.</exception>
        /// <exception cref="JsonException">Выбрасывается в случае ошибки при разборе JSON-ответа от API.</exception>
        /// <exception cref="ArgumentException">Выбрасывается в случае, если переданы некорректные аргументы (например, пустой API-ключ или неверные координаты).</exception>
        /// <exception cref="Exception">Общая ошибка, которая может возникнуть в процессе работы метода.</exception>
        public void GetWeather(string lon, string lat, DateTime date)
        {
            try
            {
                long timestamp = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
                string apiKey = "644a9dc136140e024c56a538d8dd8cba";
                using HttpClient client = new();
                string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={lat.ToString(CultureInfo.InvariantCulture)}&lon={lon.ToString(CultureInfo.InvariantCulture)}&units=metric&lang=ru&appid={apiKey}";
                string url = string.Format(apiUrl, lat, lon, timestamp, apiKey);
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();

                string responseBody = response.Content.ReadAsStringAsync().Result;
                JsonDocument json = JsonDocument.Parse(responseBody);
                var root = json.RootElement;

                string description = root.GetProperty("weather")[0].GetProperty("description").GetString() ?? "Нет данных";
                double temp = root.GetProperty("main").GetProperty("temp").GetDouble();
                double feelsLike = root.GetProperty("main").GetProperty("feels_like").GetDouble();
                double humidity = root.GetProperty("main").GetProperty("humidity").GetDouble();
                double windSpeed = root.GetProperty("wind").GetProperty("speed").GetDouble();
                double cloudiness = root.GetProperty("clouds").GetProperty("all").GetDouble();

                double rain = root.TryGetProperty("rain", out JsonElement rainElement) &&
                              rainElement.TryGetProperty("1h", out JsonElement rainAmount)
                    ? rainAmount.GetDouble()
                    : 0.0;
                WeatherDisc = description;
                WeatherTemp = temp;
                WeatherFeelsLike = feelsLike;
                WeatherHumidity = humidity;
                WeatherWindSpeed = windSpeed;
                WeatherCloudiness = cloudiness;
                WeatherRain = rain;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (JsonException e)
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
        }

    }
}
