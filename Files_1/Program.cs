using System;
using System.IO;
using System.Text;

namespace Files
{
    class Student	            // Класс студент
    {
        string firstname;		// Имя
        string lastname;		// Фамилия
        string address;		    // Адрес
        string phone;			// Телефон
        DateTime birthday;		// Дата рождения
        int number;             // Номер зачетки

        // Свойства
        public string Firstname { get => firstname; set => firstname = value; }
        public string Lastname { get => lastname; set => lastname = value; }
        public string Address { get => address; set => address = value; }
        public string Phone { get => phone; set => phone = value; }
        public DateTime Birthday { get => birthday; set => birthday = value; }
        public int Number { get => number; set => number = Math.Abs(value); }


        // Поверхностное копирование объекта
        public Student Clone()
        {
            // Вызываем функцию базового класса (Object)
            // для поверхностного копирования объекта
            return (Student)MemberwiseClone();
        }
        // Ввод данных
        public void Input()
        {
            Console.WriteLine("*****Ввод данных о студенте:******");
            Console.Write("Имя: ");
            Firstname = Console.ReadLine();
            Console.Write("Фамилия: ");
            Lastname = Console.ReadLine();
            Console.Write("Адрес: ");
            Address = Console.ReadLine();
            Console.Write("Телефон: ");
            Phone = Console.ReadLine();
            Console.Write("Дата рождения: ");
            try
            {
                // Считывание даты
                Birthday = Convert.ToDateTime(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Ошибка ввода, используем текущую дату");
                Birthday = DateTime.Now;
            }
            Console.Write("Номер зачетки: ");
            try
            {
                Number = Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Ошибка ввода, используется номер 0");
                Number = 0;
            }
            Console.WriteLine("**********************************");
        }
        // Вывод данных
        public void Print()
        {
            Console.WriteLine("*****Вывод данных о студенте:*****");
            Console.WriteLine("Имя: {0}", Firstname);
            Console.WriteLine("Фамилия: {0}", Lastname);
            Console.WriteLine("Адрес: {0}", Address);
            Console.WriteLine("Телефон: {0}", Phone);
            Console.WriteLine("Дата рождения: {0}", Birthday.ToShortDateString());
            Console.WriteLine("Номер зачетки: {0}", Number);
            Console.WriteLine("**********************************");
        }

        // Запись в файл
        public void Write(BinaryWriter bw)
        {
            // Все данные записываются по отдельности
            bw.Write(Firstname);
            bw.Write(Lastname);
            bw.Write(Address);
            bw.Write(Phone);
            bw.Write(Birthday.Year);
            bw.Write(Birthday.Month);
            bw.Write(Birthday.Day);
            bw.Write(Number);
        }

        // Статический метод для чтения из файла информации
        // и создания нового объекта на ее основе
        public static Student Read(BinaryReader br)
        {
            // Считывание производится в порядке, 
            // соответствующем записи
            Student st = new Student();
            st.Firstname = br.ReadString();
            st.Lastname = br.ReadString();
            st.Address = br.ReadString();
            st.Phone = br.ReadString();
            int year = br.ReadInt32();
            int month = br.ReadInt32();
            int day = br.ReadInt32();
            st.Birthday = new DateTime(year, month, day);
            st.Number = br.ReadInt32();
            Console.WriteLine("\n------\n" + br.BaseStream.Position.ToString() + "\n------\n");

            return st;
        }
    }

    // Класс Group
    class Group : ICloneable
    {
        // Название группы
        string groupname;
        // Массив студентов
        Student[] st;
        private const int MAX_STUDENTS_IN_GROUP = 30;

        // Свойства
        public string GroupName
        {
            get
            {
                return groupname;
            }
            set
            {
                groupname = value;
            }
        }
        public Student[] Students
        {
            get
            {
                return st;
            }
            set
            {
                st = value;
            }
        }

        // Конструктор, получающий название группы и количество студентов
        public Group(string gn, int n)
        {
            groupname = gn;
            // По умолчанию в группе 10 студентов
            if (n < 0 || n > MAX_STUDENTS_IN_GROUP)
                n = MAX_STUDENTS_IN_GROUP;
            st = new Student[n];
            // Создаем студентов
            for (int i = 0; i < n; i++)
                st[i] = new Student();
        }

        // Аналог конструктора копирования
        public Group(Group gr)
        {
            // Создаем массив студентов 
            st = new Student[gr.st.Length];
            // Передираем название группы
            groupname = gr.groupname;
            // Передираем каждого индивидуума
            for (int i = 0; i < gr.st.Length; i++)
                st[i] = gr.st[i].Clone();
        }

        // Заполняем группу
        public void Input()
        {
            for (int i = 0; i < st.Length; i++)
            {
                Console.WriteLine("{0}.", i + 1);
                st[i].Input();
            }
        }
        // Изменение данных конкретного студента
        public void InputAt(int n)
        {
            if (st == null || n >= st.Length || n < 0)
                return;

            st[n].Input();
        }

        // Вывод списка группы
        public void Print()
        {
            Console.WriteLine("Группа {0}:", groupname);

            for (int i = 0; i < st.Length; i++)
            {
                Console.WriteLine("{0}.", i + 1);
                st[i].Print();
            }
        }

        // Вывод информации о конкретном студенте
        public void PrintAt(int n)
        {
            if (st == null || n >= st.Length || n < 0)
                return;

            st[n].Print();
        }

        // "Глубокое" копирование, реализация функции из интерфейса IClonable
        public object Clone()
        {
            // Создание новой группы
            Group gr = new Group(groupname, this.st.Length);
            // Передираем каждого индивидуума
            for (int i = 0; i < this.st.Length; i++)
                gr.st[i] = this.st[i].Clone();
            // Возврат независимой копии группы
            return gr;
        }

        // Запись в файл
        public void Write(BinaryWriter bw)
        {
            // Сохраняем название группы
            bw.Write(groupname);
            // Сохраняем количество студентов
            bw.Write(st.Length);

            // Для сохранения студента вызывается
            // соответствующий метод из класса Student
            for (int i = 0; i < st.Length; i++)
                st[i].Write(bw);
        }

        // Статический метод для чтения из файла информации
        // и создания нового объекта на ее основе
        public static Group Read(BinaryReader br)
        {
            string gn = br.ReadString();
            int n = br.ReadInt32();

            Student[] st = new Student[n];

            // Для считывания студента вызывается соотв. метод из класса Student
            for (int i = 0; i < n; i++)
                st[i] = Student.Read(br);

            // Создаем пустую группу
            Group gr = new Group(gn, n);
            // Записываем в нее студентов
            gr.st = st;

            return gr;
        }
    }

    // Тестирование
    class Test
    {
        static void Main()
        {
            // Группа
            Group gr = new Group("AT181", 2);

            gr.Input();
            Console.WriteLine("\nData in group:\n");
            gr.Print();

            // Создаем поток для создания файла и/или записи в него
            FileStream fs = new FileStream(@"d:\group.txt", FileMode.OpenOrCreate, FileAccess.Write);

            // Создаем двоичный поток для записи
            BinaryWriter bw = new BinaryWriter(fs, Encoding.UTF8);

            // Пишем данные
            gr.Write(bw);
            // Закрываем потоки
            bw.Close();
            fs.Close();

            // Создаем поток для чтения из файла
            FileStream fsR = new FileStream("d:\\group.txt", FileMode.Open, FileAccess.Read);
            // Создаем двоичный поток для чтения
            BinaryReader br = new BinaryReader(fsR, Encoding.UTF8);
            Group gr1;
            // Читаем данные
            gr1 = Group.Read(br);
            // Закрываем потоки
            br.Close();
            fsR.Close();

            gr1.Print();


        }
    }
}