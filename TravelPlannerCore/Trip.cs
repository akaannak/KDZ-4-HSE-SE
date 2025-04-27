/*
 * БПИ245-1
 * Калюжная Анна Дмитриевна
 * Вариант 1
 */
using System.Text.Json.Serialization;

namespace TravelPlannerCore
{
    /// <summary>
    /// Класс для работы с поездками
    /// </summary>
    public class Trip
    {
        static public int counter;

        static public List<Trip> trips = new List<Trip>();
        private int _id;
        private string? _country;
        private string? _city;
        private DateTime _startDate;
        private DateTime _endDate;
        private List<string> _attractions;
        decimal _budget;

        private double _lat;
        private double _lon;

        private Weather _weather;
        /// <summary>
        /// Конструктор класса с поездками
        /// </summary>
        /// <param name="id">ID поездки</param>
        /// <param name="country">Страна поездки</param>
        /// <param name="city">Город поездки</param>
        /// <param name="startDate">Время начала поездки</param>
        /// <param name="endDate">Время окончания поездки</param>
        /// <param name="attractions">Достопримечательности поездки</param>
        /// <param name="budget">Бюджет поездки</param>
        [JsonConstructor]
        public Trip(int id, string country, string city, DateTime startDate,
            DateTime endDate, List<string> attractions, decimal budget)
        {
            _id = id;
            _country = country;
            _city = city;
            _startDate = startDate;
            _endDate = endDate;
            _attractions = attractions;
            _budget = budget;
            _weather = new();
        }
        /// <summary>
        /// Свойство для возвращения ID поездки
        /// </summary>
        public int Id
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// Cвойство для возвращния страны поездки
        /// </summary>
        public string Country => _country ?? string.Empty;
        /// <summary>
        /// Свойство для возвращения города поездки
        /// </summary>
        public string City => _city ?? string.Empty;
        /// <summary>
        /// Свойство для возвращения даты старта поездки
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
        }
        /// <summary>
        /// Свойство для возвращения даты конца поездки 
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }
        }
        /// <summary>
        /// Свойство для возвращения достопримечательностей
        /// </summary>
        public List<string> Attractions
        {
            get
            {
                return _attractions;
            }
        }
        /// <summary>
        /// Метод для получения достопримечательностей в строку
        /// </summary>
        /// <returns>Строка достопримечательностей</returns>
        public string GetAttractions()
        {
            string attractions = string.Empty;
            foreach (string attraction in _attractions)
            {
                attractions += $"{attraction},";
            }
            return attractions.TrimEnd(',', ' ');
        }
        /// <summary>
        /// Свойство для получения бюджета
        /// </summary>
        public decimal Budget
        {
            get
            {
                return _budget;
            }
        }
        /// <summary>
        /// Получает или задает значение широты.
        /// </summary>
        public double Lat
        {
            get { return _lat; }
            set { _lat = value; }
        }
        /// <summary>
        /// Получает или задает значение долготы.
        /// </summary>
        public double Lon
        {
            get { return _lon; }
            set { _lon = value; }
        }

        /// <summary>
        /// Получает или задает объект, представляющий информацию о погоде.
        /// </summary>
        public Weather Weather{
            get { return _weather; }
            set { _weather = value; }
        }
        /// <summary>
        /// Находит индекс поездки в списке по идентификатору поездки.
        /// </summary>
        /// <param name="tripId">Идентификатор поездки в виде строки.</param>
        /// <returns>Индекс поездки в списке <see cref="trips"/>.</returns>
        /// <exception cref="ArgumentException">Выбрасывается, если идентификатор поездки является пустым или null.</exception>
        /// <exception cref="KeyNotFoundException">Выбрасывается, если поездка с данным идентификатором не найдена.</exception>
        public static int FindTripNumber(string tripId)
        {
            if (string.IsNullOrEmpty(tripId))
            {
                throw new ArgumentException("Идентификатор поездки не может быть пустым", nameof(tripId));
            }

            for (int i = 0; i < trips.Count; i++)
            {
                if (trips[i].Id.ToString() == tripId)
                {
                    return i;
                }
            }
            throw new KeyNotFoundException("Поездка не найдена");
        }
        /// <summary>
        /// Изменяет поездку в списке по заданному индексу.
        /// </summary>
        /// <param name="number">Номер поездки в списке.</param>
        /// <param name="newtrip">Новая поездка, которая будет заменять старую.</param>
        /// <exception cref="ArgumentOutOfRangeException">Выбрасывается, если номер поездки выходит за пределы списка.</exception>
        /// <exception cref="ArgumentNullException">Выбрасывается, если передана пустая или null новая поездка.</exception>
        public static void ChangeTrip(int number, Trip newtrip)
        {
            if (number < 0 || number >= trips.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(number), "Номер поездки выходит за пределы списка.");
            }
            if (newtrip == null)
            {
                throw new ArgumentNullException(nameof(newtrip), "Новая поездка не может быть null.");
            }
            trips[number] = newtrip;
        }
        /// <summary>
        /// Удаляет поездку из списка по заданному индексу.
        /// </summary>
        /// <param name="number">Номер поездки в списке.</param>
        /// <exception cref="ArgumentOutOfRangeException">Выбрасывается, если номер поездки выходит за пределы списка.</exception>
        public static void DeleteTrip(int number)
        {
            if (number < 0 || number >= trips.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(number), "Номер поездки выходит за пределы списка.");
            }
            trips.Remove(trips[number]);
        }
        /// <summary>
        /// Переопределение метода TOString()
        /// </summary>
        /// <returns>Отформатированная строка</returns>
        public override string ToString()
        {

            return $"Страна: {Country}\nГород: {City}\nДата начала: {StartDate}\nДата окончания: {EndDate}\nДостопримечательности:\n {GetAttractions()}Бюджет: {Budget}\n";
        }
    }
}
