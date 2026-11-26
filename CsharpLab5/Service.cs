using System;
namespace CsharpLab5
{
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
}

