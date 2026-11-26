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
}