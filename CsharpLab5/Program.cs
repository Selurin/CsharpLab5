using System;
using System.Text;

namespace CsharpLab5
{
    internal class Program
    {
        private const string DefaultFilePath = "LR5-var7.xls";

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            DatabaseManager db = TryInitialLoad();

            bool isRunning = true;
            while (isRunning)
            {
                PrintMainMenu();
                Console.Write("Выберите пункт меню: ");
                string choice = Console.ReadLine()?.Trim() ?? string.Empty;
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            db = HandleLoadDatabase();
                            break;
                        case "2":
                            EnsureLoaded(db);
                            HandleViewData(db);
                            break;
                        case "3":
                            EnsureLoaded(db);
                            HandleDelete(db);
                            break;
                        case "4":
                            EnsureLoaded(db);
                            HandleAdd(db);
                            break;
                        case "5":
                            EnsureLoaded(db);
                            HandleQueriesMenu(db);
                            break;
                        case "6":
                            EnsureLoaded(db);
                            db.SaveToExcel();
                            Console.WriteLine($"Изменения успешно сохранены в файл: {db.FilePath}");
                            break;
                        case "0":
                            isRunning = false;
                            Console.WriteLine("Завершение работы программы.");
                            break;
                        default:
                            Console.WriteLine("Неизвестный пункт меню. Введите число от 0 до 6.");
                            break;
                    }
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine($"Ошибка файла: {ex.Message}");
                }
                catch (IOException)
                {
                    Console.WriteLine("Ошибка доступа к файлу: закройте таблицу в Excel и повторите попытку.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }

        private static DatabaseManager TryInitialLoad()
        {
            if (File.Exists(DefaultFilePath))
            {
                try
                {
                    DatabaseManager db = new DatabaseManager(DefaultFilePath);
                    Console.WriteLine($"База данных автоматически загружена из файла {DefaultFilePath}.");
                    return db;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Не удалось загрузить {DefaultFilePath} при старте: {ex.Message}");
                }
            }

            return null;
        }

        private static void EnsureLoaded(DatabaseManager db)
        {
            if (db == null)
            {
                throw new InvalidOperationException("База данных не загружена. Сначала выполните пункт 1.");
            }
        }

        private static void PrintMainMenu()
        {
            Console.WriteLine("\n================ РЕКЛАМНОЕ АГЕНТСТВО ================");
            Console.WriteLine("1. Чтение (загрузка) базы данных из Excel");
            Console.WriteLine("2. Просмотр базы данных");
            Console.WriteLine("3. Удаление элемента по ключу");
            Console.WriteLine("4. Добавление элемента");
            Console.WriteLine("5. Выполнение LINQ-запросов");
            Console.WriteLine("6. Сохранение изменений в Excel");
            Console.WriteLine("0. Выход");
            Console.WriteLine("=====================================================");
        }

        private static DatabaseManager HandleLoadDatabase()
        {
            Console.Write($"Введите путь к файлу Excel (Enter — {DefaultFilePath}): ");
            string path = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(path))
            {
                path = DefaultFilePath;
            }

            DatabaseManager db = new DatabaseManager(path);
            Console.WriteLine($"База данных успешно загружена из {db.FilePath}.");
            return db;
        }

        private static void HandleViewData(DatabaseManager db)
        {
            Console.WriteLine("Что вывести на экран?");
            Console.WriteLine("1. Клиенты");
            Console.WriteLine("2. Заказы");
            Console.WriteLine("3. Услуги");
            Console.WriteLine("4. Типы услуг");
            Console.WriteLine("5. Все таблицы целиком");
            Console.Write("Выбор: ");
            string choice = Console.ReadLine()?.Trim() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    PrintList("Клиенты", db.Clients);
                    break;
                case "2":
                    PrintList("Заказы", db.Orders);
                    break;
                case "3":
                    PrintList("Услуги", db.Services);
                    break;
                case "4":
                    PrintList("Типы услуг", db.TypeServices);
                    break;
                case "5":
                    db.PrintAllData();
                    break;
                default:
                    Console.WriteLine("Неверный выбор таблицы.");
                    break;
            }
        }

        private static void PrintList<T>(string title, IReadOnlyList<T> items)
        {
            Console.WriteLine($"\n---- {title} (всего: {items.Count}) ----");
            foreach (T item in items)
            {
                Console.WriteLine(item);
            }
        }

        private static void HandleDelete(DatabaseManager db)
        {
            Console.WriteLine("Выберите таблицу для удаления:");
            Console.WriteLine("1. Клиенты");
            Console.WriteLine("2. Заказы");
            Console.WriteLine("3. Услуги");
            Console.WriteLine("4. Типы услуг");

            if (TryReadInt("Номер таблицы: ", out int tableNumber))
            {
                if (TryReadInt("Введите ID удаляемой записи: ", out int id))
                {
                    bool deleted = db.DeleteByKey(tableNumber, id);
                    if (deleted)
                    {
                        Console.WriteLine("Запись и связанные с ней данные успешно удалены.");
                    }
                    else
                    {
                        Console.WriteLine($"Запись с ID {id} не найдена.");
                    }
                }
            }
        }

        private static void HandleAdd(DatabaseManager db)
        {
            Console.WriteLine("Выберите таблицу для добавления:");
            Console.WriteLine("1. Клиенты");
            Console.WriteLine("2. Заказы");
            Console.WriteLine("3. Услуги");
            Console.WriteLine("4. Типы услуг");
            Console.Write("Номер таблицы: ");
            string choice = Console.ReadLine()?.Trim() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    if (TryReadInt("ID клиента: ", out int clientId))
                    {
                        Console.Write("Фамилия: ");
                        string fam = Console.ReadLine() ?? string.Empty;
                        Console.Write("Имя: ");
                        string imya = Console.ReadLine() ?? string.Empty;
                        Console.Write("Отчество: ");
                        string otch = Console.ReadLine() ?? string.Empty;
                        Console.Write("Город (например, г. Пермь): ");
                        string city = Console.ReadLine() ?? string.Empty;

                        db.Add(new Client(clientId, fam, imya, otch, city));
                        Console.WriteLine("Клиент добавлен.");
                    }
                    break;

                case "2":
                    if (TryReadInt("ID заказа: ", out int orderId))
                    {
                        if (TryReadInt("ID клиента: ", out int orderClientId))
                        {
                            if (TryReadDate("Дата заказа (дд.мм.гггг): ", out DateOnly orderDate))
                            {
                                if (TryReadInt("ID услуги: ", out int orderServiceId))
                                {
                                    if (TryReadInt("Количество: ", out int count))
                                    {
                                        db.Add(new Order(orderId, orderClientId, orderDate, orderServiceId, count));
                                        Console.WriteLine("Заказ добавлен.");
                                    }
                                }
                            }
                        }
                    }
                    break;

                case "3":
                    if (TryReadInt("ID услуги: ", out int serviceId))
                    {
                        if (TryReadInt("ID типа услуги: ", out int serviceTypeId))
                        {
                            Console.Write("Название услуги: ");
                            string serviceName = Console.ReadLine() ?? string.Empty;

                            if (TryReadInt("Стоимость: ", out int price))
                            {
                                db.Add(new Service(serviceId, serviceTypeId, serviceName, price));
                                Console.WriteLine("Услуга добавлена.");
                            }
                        }
                    }
                    break;

                case "4":
                    if (TryReadInt("ID типа услуги: ", out int typeId))
                    {
                        Console.Write("Название типа услуги: ");
                        string typeName = Console.ReadLine() ?? string.Empty;

                        db.Add(new TypeService(typeId, typeName));
                        Console.WriteLine("Тип услуги добавлен.");
                    }
                    break;

                default:
                    Console.WriteLine("Неверный номер таблицы.");
                    break;
            }
        }

        private static void HandleQueriesMenu(DatabaseManager db)
        {
            bool inSubmenu = true;
            while (inSubmenu)
            {
                Console.WriteLine("\n---------------- ПОДМЕНЮ ЗАПРОСОВ ----------------");
                Console.WriteLine("1. [1 таблица, перечень] Услуги стоимостью от 5 000 до 25 000 руб.");
                Console.WriteLine("2. [2 таблицы, перечень] Услуги категорий «Наружняя реклама» и «Интернет-маркетинг»");
                Console.WriteLine("3. [3 таблицы, 1 значение] Клиент из г. Москва с самым дорогим заказом");
                Console.WriteLine("4. [3 таблицы, 1 значение] Суммарное количество заказанных единиц полиграфии за июль 2018");
                Console.WriteLine("0. Назад в главное меню");
                Console.WriteLine("--------------------------------------------------");
                Console.Write("Выберите запрос: ");
                string queryChoice = Console.ReadLine()?.Trim() ?? string.Empty;
                Console.WriteLine();

                switch (queryChoice)
                {
                    case "1":
                        List<Service> midServices = db.GetMidPriceServices();
                        PrintList("Результат запроса 1", midServices);
                        break;
                    case "2":
                        List<string> typedServices = db.GetOutdoorAndInternetServices();
                        PrintList("Результат запроса 2", typedServices);
                        break;
                    case "3":
                        Console.WriteLine("Результат запроса 3:");
                        Console.WriteLine(db.GetTopMoscowClient());
                        break;
                    case "4":
                        Console.WriteLine($"Результат запроса 4: {db.GetJulyPolygraphyUnitsCount()} ед.");
                        break;
                    case "0":
                        inSubmenu = false;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор запроса. Введите число от 0 до 4.");
                        break;
                }
            }
        }

        private static bool TryReadInt(string prompt, out int value)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out value))
            {
                return true;
            }
            else
            {
                Console.WriteLine("Неверное значение! Ожидалось целое число.");
                return false;
            }
        }

        private static bool TryReadDate(string prompt, out DateOnly value)
        {
            Console.Write(prompt);
            if (DateOnly.TryParse(Console.ReadLine(), out value))
            {
                return true;
            }
            else
            {
                Console.WriteLine("Неверное значение! Ожидалась дата в формате дд.мм.гггг.");
                return false;
            }
        }
    }
}