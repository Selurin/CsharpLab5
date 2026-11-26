using System;

namespace CsharpLab5
{
    public class Client
    {
        private int _id;
        private string _fam;
        private string _imya;
        private string _otch;
        private string _city;
        private const string DefaultText = "Не задано";

        public Client() : this(1, DefaultText, DefaultText, DefaultText, DefaultText)
        {

        }

        public Client(int id, string fam, string imya, string otch, string city)
        {
            Id = id;
            Fam = fam;
            Imya = imya;
            Otch = otch;
            City = city;
        }

        public int Id
        {
            get { return _id; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("ID клиента должен быть больше 0.");
                }
                _id = value;
            }
        }

        public string Fam
        {
            get { return _fam; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Фамилия не может быть пустой.");
                }
                _fam = value;
            }
        }

        public string Imya
        {
            get { return _imya; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Имя не может быть пустым.");
                }
                _imya = value;
            }
        }

        public string Otch
        {
            get { return _otch; }
            set { _otch = value ?? string.Empty; }
        }

        public string City
        {
            get { return _city; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Город не может быть пустым.");
                }
                _city = value;
            }
        }
        public override string ToString()
        {
            return $"ID: {_id}, Фамилия: {_fam}, Имя: {_imya}, Отчество: {_otch}, Город: {_city}";
        }
    }

    public class Order
    {
        private int _orderId;
        private int _clientId;
        private DateOnly _orderDate;
        private int _serviceId;
        private int _count;

        public Order() : this(1, 1, DateOnly.FromDateTime(DateTime.Now), 1, 1)
        {

        }

        public Order(int orderId, int clientId, DateOnly orderDate, int serviceId, int count)
        {
            OrderId = orderId;
            ClientId = clientId;
            OrderDate = orderDate;
            ServiceId = serviceId;
            Count = count;
        }

        public int OrderId
        {
            get { return _orderId; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("ID заказа должен быть больше 0.");
                }
                _orderId = value;
            }
        }

        public int ClientId
        {
            get { return _clientId; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("ID клиента должен быть больше 0.");
                }
                _clientId = value;
            }
        }

        public DateOnly OrderDate
        {
            get { return _orderDate; }
            set { _orderDate = value; }
        }

        public int ServiceId
        {
            get { return _serviceId; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("ID услуги должен быть больше 0.");
                }
                _serviceId = value;
            }
        }

        public int Count
        {
            get { return _count; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Количество должно быть больше 0.");
                }
                _count = value;
            }
        }

        public override string ToString()
        {
            return $"ID заказа: {_orderId}, ID клиента: {_clientId}, Дата заказа: {_orderDate}," +
                $" ID услуги: {_serviceId}, Количество: {_count}";
        }
    }

    public class Service
    {
        private int _serviceId;
        private int _typeId;
        private string _name;
        private int _price;

        public Service() : this(1, 1, "Не задано", 0)
        {

        }

        public Service(int serviceId, int typeId, string name, int price)
        {
            ServiceId = serviceId;
            TypeId = typeId;
            Name = name;
            Price = price;
        }

        public int ServiceId
        {
            get { return _serviceId; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("ID услуги должен быть больше 0.");
                }
                _serviceId = value;
            }
        }

        public int TypeId
        {
            get { return _typeId; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("ID типа услуги должен быть больше 0.");
                }
                _typeId = value;
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Название услуги не может быть пустым.");
                }
                _name = value;
            }
        }

        public int Price
        {
            get { return _price; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Цена не может быть отрицательной.");
                }
                _price = value;
            }
        }

        public override string ToString()
        {
            return $"ID услуги: {_serviceId}, ID типа услуги: {_typeId}, Название: {_name}, Цена: {_price}";
        }
    }

    public class TypeService
    {
        private int _typeId;
        private string _name;

        public TypeService() : this(1, "Не задано")
        {

        }

        public TypeService(int typeId, string name)
        {
            TypeId = typeId;
            Name = name;
        }

        public int TypeId
        {
            get { return _typeId; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("ID типа услуги должен быть больше 0.");
                }
                _typeId = value;
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Название типа услуги не может быть пустым.");
                }
                _name = value;
            }
        }
        public override string ToString()
        {
            return $"ID типа услуги: {_typeId}, Название: {_name}";
        }
    }
}