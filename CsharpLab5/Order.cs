using System;
namespace CsharpLab5
{
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
}