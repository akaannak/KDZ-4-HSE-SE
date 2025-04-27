/*
 * БПИ245-1
 * Калюжная Анна Дмитриевна
 * Вариант 1
 */
namespace TravelPlannerConsoleApp
{
    /// <summary>
    /// Класс для вывода меню
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// Метод для вывода главного меню
        /// </summary>
        public static void ShowMenu()
        {
            Console.WriteLine("\nВведите номер пункта меню для запуска действия:");
            Console.WriteLine("\t1. Ввести данные");
            Console.WriteLine("\t2. Просмотреть список всех поездок");
            Console.WriteLine("\t3. Просмотреть поездки на карте мира");
            Console.WriteLine("\t4. Добавить новый маршрут");
            Console.WriteLine("\t5. Редактировать маршрут");
            Console.WriteLine("\t6. Удалить маршрут");
            Console.WriteLine("\t7. Просмотреть информацию о достопримечательностях определенной поездки");
            Console.WriteLine("\t8. Ручной запрос погоды для определенной поездки");
            Console.WriteLine("\t9. Сгенерировать PDF-очет о поездке");
            Console.WriteLine("\t10. Завершить работу программы (изменения будут автоматически сохранены)\n");
        }
        /// <summary>
        /// Метод для вывода подменю для первого пункта
        /// </summary>
        public static void ShowMenuOne()
        {
            Console.WriteLine("\nКаким образом вы хотите ввести данные?");
            Console.WriteLine("\t1. Через консоль");
            Console.WriteLine("\t2. Загрузить данные из файла (*csv, *json).\n");
        }
        /// <summary>
        /// Меню для запроса ID
        /// </summary>
        public static void AskForId()
        {
            Console.WriteLine("\nВведите ID поездки\n");
        }
        /// <summary>
        /// Меню для запроса количества поездок
        /// </summary>
        public static void AskForCount()
        {
            Console.WriteLine("\nКакое количество поездок вы хотите ввести?\n");
        }
    }
}
