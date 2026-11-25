using System;
using ClosedXML.Excel;
using System.Linq;
namespace CsharpLab5
{
    public class DatabaseManager
    {
        private string _filePath;
        private List<Client> _clients;
        private List<Order> _orders;
        private List<Service> _services;
        private List<TypeService> _typeServices;

        public string FilePath
        {
            get => _filePath;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Путь к файлу не может быть пустым.");
                _filePath = value;
            }
        }

        public IReadOnlyList<Client> Clients => _clients.AsReadOnly();
        public IReadOnlyList<Order> Orders => _orders.AsReadOnly();
        public IReadOnlyList<Service> Services => _services.AsReadOnly();
        public IReadOnlyList<TypeService> TypeServices => _typeServices.AsReadOnly();

        public DatabaseManager(string filePath)
        {
            FilePath = filePath;
            _clients = new List<Client>();
            _orders = new List<Order>();
            _services = new List<Service>();
            _typeServices = new List<TypeService>();

            LoadFromExcel();
        }

        public void LoadFromExcel()
        {
            if (!File.Exists(FilePath))
                throw new FileNotFoundException($"Файл {FilePath} не найден.");
            _clients.Clear();
            _orders.Clear();
            _services.Clear();
            _typeServices.Clear();

            using (var workbook = new XLWorkbook(FilePath))
            {
                var clientsSheet = workbook.Worksheet("Клиенты");
                _clients = clientsSheet.RowsUsed()
                    .Skip(1)
                    .Select(row => new Client(
                    row.Cell(1).GetValue<int>(),
                    row.Cell(2).GetValue<string>(),
                    row.Cell(3).GetValue<string>(),
                    row.Cell(4).GetValue<string>(),
                    row.Cell(5).GetValue<string>()
                )).ToList();

                var ordersSheet = workbook.Worksheet("Заказы");
                _orders = ordersSheet.RowsUsed()
                    .Skip(1)
                    .Select(row => new Order(
                    row.Cell(1).GetValue<int>(),
                    row.Cell(2).GetValue<int>(),
                    row.Cell(3).GetValue<DateOnly>(),
                    row.Cell(4).GetValue<int>(),
                    row.Cell(5).GetValue<int>()
                )).ToList();
                var servicesSheet = workbook.Worksheet("Услуги");
                _services = servicesSheet.RowsUsed()
                    .Skip(1)
                    .Select(row => new Service(
                    row.Cell(1).GetValue<int>(),
                    row.Cell(2).GetValue<int>(),
                    row.Cell(3).GetValue<string>(),
                    row.Cell(4).GetValue<int>()
                )).ToList();

                var typeServicesSheet = workbook.Worksheet("Типы услуг");
                _typeServices = typeServicesSheet.RowsUsed()
                    .Skip(1)
                    .Select(row => new TypeService(
                    row.Cell(1).GetValue<int>(),
                    row.Cell(2).GetValue<string>()
                )).ToList();
            }
        }

        public void PrintAllData()
        {
            Console.WriteLine("----Клиенты----");
            _clients.ForEach(c => Console.WriteLine(c));

            Console.WriteLine("----Заказы----");
            _orders.ForEach(o => Console.WriteLine(o));

            Console.WriteLine("----Услуги----");
            _services.ForEach(s => Console.WriteLine(s));

            Console.WriteLine("----Типы услуг----");
            _typeServices.ForEach(t => Console.WriteLine(t));
        }

        public bool DeleteByKey(int tableNumber, int id)
        {
            int countBefore = _clients.Count + _orders.Count + _services.Count + _typeServices.Count;

            switch (tableNumber)
            {
                case 1:
                    _clients = (from c in _clients
                                where c.Id != id
                                select c).ToList();
                    break;
                case 2:
                    _orders = (from o in _orders
                               where o.OrderId != id
                               select o).ToList();
                    break;
                case 3:
                    _services = (from s in _services
                                 where s.ServiceId != id
                                 select s).ToList();
                    break;
                case 4:
                    _typeServices = (from t in _typeServices
                                     where t.TypeId != id
                                     select t).ToList();
                    break;
                default:
                    throw new ArgumentException("Неверный номер таблицы.");
            }

            _services = (from s in _services
                         join t in _typeServices on s.TypeId equals t.TypeId
                         select s).ToList();

            _orders = (from o in _orders
                       join c in _clients on o.ClientId equals c.Id
                       join s in _services on o.ServiceId equals s.ServiceId
                       select o).ToList();

            int countAfter = _clients.Count + _orders.Count + _services.Count + _typeServices.Count;
            return countAfter < countBefore;
        }

        public void Add<T>(T item) where T : class
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Добавляемый объект не может быть null.");
            }

            switch (item)
            {
                case Client newClient:
                    bool clientExists = (from c in _clients
                                         where c.Id == newClient.Id
                                         select c).Any();
                    if (clientExists)
                    {
                        throw new ArgumentException($"Клиент с ID {newClient.Id} уже существует.");
                    }
                    _clients.Add(newClient);
                    break;

                case TypeService newType:
                    bool typeExists = (from t in _typeServices
                                       where t.TypeId == newType.TypeId
                                       select t).Any();
                    if (typeExists)
                    {
                        throw new ArgumentException($"Тип услуги с ID {newType.TypeId} уже существует.");
                    }
                    _typeServices.Add(newType);
                    break;

                case Service newService:
                    bool serviceExists = (from s in _services
                                          where s.ServiceId == newService.ServiceId
                                          select s).Any();
                    if (serviceExists)
                    {
                        throw new ArgumentException($"Услуга с ID {newService.ServiceId} уже существует.");
                    }

                    bool parentTypeExists = (from t in _typeServices
                                             where t.TypeId == newService.TypeId
                                             select t).Any();
                    if (!parentTypeExists)
                    {
                        throw new ArgumentException($"Тип услуги с ID {newService.TypeId} не найден.");
                    }
                    _services.Add(newService);
                    break;

                case Order newOrder:
                    bool orderExists = (from o in _orders
                                        where o.OrderId == newOrder.OrderId
                                        select o).Any();
                    if (orderExists)
                    {
                        throw new ArgumentException($"Заказ с ID {newOrder.OrderId} уже существует.");
                    }

                    bool orderClientExists = (from c in _clients
                                              where c.Id == newOrder.ClientId
                                              select c).Any();
                    if (!orderClientExists)
                    {
                        throw new ArgumentException($"Клиент с ID {newOrder.ClientId} не найден.");
                    }

                    bool orderServiceExists = (from s in _services
                                               where s.ServiceId == newOrder.ServiceId
                                               select s).Any();
                    if (!orderServiceExists)
                    {
                        throw new ArgumentException($"Услуга с ID {newOrder.ServiceId} не найдена.");
                    }
                    _orders.Add(newOrder);
                    break;

                default:
                    throw new ArgumentException($"Тип {typeof(T).Name} не поддерживается базой данных.");
            }
        }

        public List<Service> GetMidPriceServices()
        {
            var query = from s in _services
                        where s.Price >= 5000 && s.Price <= 25000
                        orderby s.Price descending, s.Name ascending
                        select s;

            return query.ToList();
        }

        public List<string> GetOutdoorAndInternetServices()
        {
            var query = from s in _services
                        join t in _typeServices on s.TypeId equals t.TypeId
                        where t.Name == "Наружняя реклама" || t.Name == "Интернет-маркетинг"
                        orderby t.Name ascending, s.Price ascending
                        select $"{t.Name}: «{s.Name}» — {s.Price} руб.";

            return query.ToList();
        }

        public string GetTopMoscowClient()
        {
            var query = from o in _orders
                        join c in _clients on o.ClientId equals c.Id
                        join s in _services on o.ServiceId equals s.ServiceId
                        where c.City == "Москва"
                        orderby o.Count * s.Price descending
                        select $"{c.Fam} {c.Imya} (заказ #{o.OrderId} на сумму {o.Count * s.Price} руб.)";

            string result = query.FirstOrDefault();

            if (result == null)
            {
                return "Заказы клиентов из г. Москва не найдены.";
            }

            return result;
        }

        public int GetAutumnVideoUnitsCount()
        {
            var query = from o in _orders
                        join s in _services on o.ServiceId equals s.ServiceId
                        join t in _typeServices on s.TypeId equals t.TypeId
                        where t.Name == "Полиграфия" && o.OrderDate.Month >= 9 && o.OrderDate.Month <= 11
                        select o.Count;

            return query.Sum();
        }
    }
}
