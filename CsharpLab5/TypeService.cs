using System;
namespace CsharpLab5
{
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
