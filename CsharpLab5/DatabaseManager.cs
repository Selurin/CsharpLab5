using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
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
            {
                throw new FileNotFoundException($"Файл {FilePath} не найден.");
            }

            _clients.Clear();
            _orders.Clear();
            _services.Clear();
            _typeServices.Clear();

            using (var fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = WorkbookFactory.Create(fs);

                ISheet clientsSheet = workbook.GetSheet("Клиенты");
                for (int i = 1; i <= clientsSheet.LastRowNum; i++)
                {
                    IRow row = clientsSheet.GetRow(i);
                    if (row == null)
                    {
                        continue;
                    }
                    _clients.Add(new Client(
                        (int)row.GetCell(0).NumericCellValue,
                        row.GetCell(1).StringCellValue,
                        row.GetCell(2).StringCellValue,
                        row.GetCell(3).StringCellValue,
                        row.GetCell(4).StringCellValue
                    ));
                }

                ISheet ordersSheet = workbook.GetSheet("Заказы");
                for (int i = 1; i <= ordersSheet.LastRowNum; i++)
                {
                    IRow row = ordersSheet.GetRow(i);
                    if (row == null)
                    {
                        continue;
                    }
                    DateTime date = DateUtil.GetJavaDate(row.GetCell(2).NumericCellValue);
                    _orders.Add(new Order(
                        (int)row.GetCell(0).NumericCellValue,
                        (int)row.GetCell(1).NumericCellValue,
                        DateOnly.FromDateTime(date),
                        (int)row.GetCell(3).NumericCellValue,
                        (int)row.GetCell(4).NumericCellValue
                    ));
                }

                ISheet servicesSheet = workbook.GetSheet("Услуги");
                for (int i = 1; i <= servicesSheet.LastRowNum; i++)
                {
                    IRow row = servicesSheet.GetRow(i);
                    if (row == null)
                    {
                        continue;
                    }
                    _services.Add(new Service(
                        (int)row.GetCell(0).NumericCellValue,
                        (int)row.GetCell(1).NumericCellValue,
                        row.GetCell(2).StringCellValue,
                        (int)row.GetCell(3).NumericCellValue
                    ));
                }

                ISheet typeServicesSheet = workbook.GetSheet("Типы услуг");
                for (int i = 1; i <= typeServicesSheet.LastRowNum; i++)
                {
                    IRow row = typeServicesSheet.GetRow(i);
                    if (row == null)
                    {
                        continue;
                    }
                    _typeServices.Add(new TypeService(
                        (int)row.GetCell(0).NumericCellValue,
                        row.GetCell(1).StringCellValue
                    ));
                }
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
                        where c.City == "г. Москва"
                        orderby o.Count * s.Price descending
                        select $"{c.Fam} {c.Imya} (заказ #{o.OrderId} на сумму {o.Count * s.Price} руб.)";

            string result = query.FirstOrDefault();

            if (result == null)
            {
                return "Заказы клиентов из г. Москва не найдены.";
            }

            return result;
        }

        public int GetJulyPolygraphyUnitsCount()
        {
            var query = from o in _orders
                        join s in _services on o.ServiceId equals s.ServiceId
                        join t in _typeServices on s.TypeId equals t.TypeId
                        where t.Name == "Полиграфия" && o.OrderDate.Month == 7 && o.OrderDate.Year == 2018
                        select o.Count;

            return query.Sum();
        }

        public void SaveToExcel()
        {
            IWorkbook workbook = new HSSFWorkbook();
            ICellStyle dateStyle = workbook.CreateCellStyle();
            dateStyle.DataFormat = workbook.CreateDataFormat().GetFormat("dd.MM.yyyy");

            ISheet clientsSheet = workbook.CreateSheet("Клиенты");
            IRow cHeader = clientsSheet.CreateRow(0);
            cHeader.CreateCell(0).SetCellValue("Код клиента");
            cHeader.CreateCell(1).SetCellValue("Фамилия");
            cHeader.CreateCell(2).SetCellValue("Имя");
            cHeader.CreateCell(3).SetCellValue("Отчество");
            cHeader.CreateCell(4).SetCellValue("Место жительства");
            for (int i = 0; i < _clients.Count; i++)
            {
                IRow row = clientsSheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(_clients[i].Id);
                row.CreateCell(1).SetCellValue(_clients[i].Fam);
                row.CreateCell(2).SetCellValue(_clients[i].Imya);
                row.CreateCell(3).SetCellValue(_clients[i].Otch);
                row.CreateCell(4).SetCellValue(_clients[i].City);
            }

            ISheet ordersSheet = workbook.CreateSheet("Заказы");
            IRow oHeader = ordersSheet.CreateRow(0);
            oHeader.CreateCell(0).SetCellValue("Код заказа");
            oHeader.CreateCell(1).SetCellValue("Код клиента");
            oHeader.CreateCell(2).SetCellValue("Дата заказа");
            oHeader.CreateCell(3).SetCellValue("Код услуги");
            oHeader.CreateCell(4).SetCellValue("Количество");
            for (int i = 0; i < _orders.Count; i++)
            {
                IRow row = ordersSheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(_orders[i].OrderId);
                row.CreateCell(1).SetCellValue(_orders[i].ClientId);
                ICell dateCell = row.CreateCell(2);
                dateCell.SetCellValue(_orders[i].OrderDate.ToDateTime(TimeOnly.MinValue));
                dateCell.CellStyle = dateStyle;
                row.CreateCell(3).SetCellValue(_orders[i].ServiceId);
                row.CreateCell(4).SetCellValue(_orders[i].Count);
            }

            ISheet servicesSheet = workbook.CreateSheet("Услуги");
            IRow sHeader = servicesSheet.CreateRow(0);
            sHeader.CreateCell(0).SetCellValue("Код услуги");
            sHeader.CreateCell(1).SetCellValue("Код типа");
            sHeader.CreateCell(2).SetCellValue("Название");
            sHeader.CreateCell(3).SetCellValue("Стоимость");
            for (int i = 0; i < _services.Count; i++)
            {
                IRow row = servicesSheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(_services[i].ServiceId);
                row.CreateCell(1).SetCellValue(_services[i].TypeId);
                row.CreateCell(2).SetCellValue(_services[i].Name);
                row.CreateCell(3).SetCellValue(_services[i].Price);
            }

            ISheet typeServicesSheet = workbook.CreateSheet("Типы услуг");
            IRow tHeader = typeServicesSheet.CreateRow(0);
            tHeader.CreateCell(0).SetCellValue("Код типа");
            tHeader.CreateCell(1).SetCellValue("Название");
            for (int i = 0; i < _typeServices.Count; i++)
            {
                IRow row = typeServicesSheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(_typeServices[i].TypeId);
                row.CreateCell(1).SetCellValue(_typeServices[i].Name);
            }

            using (var fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }
        }
    }
}
