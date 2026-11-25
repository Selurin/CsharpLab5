using System;

public class Client
{
    private int _id;
    private string _fam;
    private string _imya;
    private string _otch;
    private string _city;
    private const string DefaultText = "Не задано";

    public Client() : this(0,DefaultText,DefaultText,DefaultText,DefaultText)
    {
  
    }

    public Client(int id, string fam, string imya, string otch, string city)
    {
        _id = id;
        _fam = fam;
        _imya = imya;
        _otch = otch;
        _city = city;
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
}

public class Order
{
    private int _orderId;
    private int _clientId;
    private DateOnly _orderDate;
    private int _serviceId;
    private int _count;

    public Order() : this(0, 0, DateOnly.FromDateTime(DateTime.Now), 0, 0)
    {

    }

    public Order(int orderId, int clientId, DateOnly orderDate, int serviceId, int count)
    {
        _orderId = orderId;
        _clientId = clientId;
        _orderDate = orderDate;
        _serviceId = serviceId;
        _count = count;
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
}

public class Service
{
    private int _serviceId;
    private int _typeId;
    private string _name;
    private int _price;

    public Service() : this(0, 0, string.Empty, 0)
    {

    }

    public Service(int serviceId, int typeId, string name, int price)
    {
        _serviceId = serviceId;
        _typeId = typeId;
        _name = name;
        _price = price;
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
}

public class TypeService
{
    private int _typeId;
    private string _name;

    public TypeService() : this(0, string.Empty)
    {

    }

    public TypeService(int typeId, string name)
    {
        _typeId = typeId;
        _name = name;
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
}