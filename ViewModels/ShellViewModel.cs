using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Caliburn.Micro;
using Training.Models;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Documents;
using DocumentFormat.OpenXml.Office2013.Word;
using System.Data.SqlClient;
using System.Data;

namespace Training.ViewModels
{
    public class ShellViewModel : Screen
    {
        private string _name;
        private int _count = 1;
        public string _dataTime;
        private BindableCollection<PersonModel> _people = new BindableCollection<PersonModel>();

        public ShellViewModel()
        {
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
                NotifyOfPropertyChange(() => Count);
            }
        }

        public string DataTime
        {
            get
            {
                return _dataTime;
            }
            set
            {
                _dataTime = value;
                NotifyOfPropertyChange(() => DataTime);
            }
        }

        public BindableCollection<PersonModel> People
        {
            get
            {
                return _people;
            }
            set
            {
                _people = value;
            }
        }

        public void AddText()
        {
            People.Add(new PersonModel { Count = _count++, Name = _name, DataTime = DateTime.Now.ToString() });
        }

        public void ClearText()
        {
            People.Clear();
            _count = 1;
        }

        public void Save()
        {
            using (StreamWriter sw = new StreamWriter(@"D:\Test.doc"))
            {

                for (var i = 0; i < People.Count; i++)
                {
                    sw.WriteLine(People[i].Count + " " + People[i].Name + " " + People[i].DataTime);
                }

            }

        }

        public void Download()
        {
            try
            {
                string[] text = File.ReadAllLines(@"D:\Test.doc");
                for (var i = 0; i < text.Length; i++)
                {
                    string[] data = text[i].Split(" ");
                    PersonModel model = new PersonModel { Count = int.Parse(data[0]), Name = data[1], DataTime = data[2] + " " + data[3] };
                    People.Add(model);
                }
            }
            catch (Exception)
            {
                MessageBox.Show(
                     "Ошибка! Файл не найден в системе");
            }

        }

        public void DownloadBase()
        {
            try
            {
                string connectString = "Data Source=.\\SQLEXPRESS; Initial Catalog= Test; Integrated Security=true;"; //Объявление переменной со строкой подключения к базе данных:

                SqlConnection myConnection = new SqlConnection(connectString); // Создание объекта для подключения к БД:

                myConnection.Open(); // Соединение с БД:

                string query = "SELECT * FROM Users ORDER BY Id"; //Запрос на получение данных из таблицы с факультетами: Моя ошибка была в том что после From надо указывать название таблицы и дальше id

                SqlCommand command = new SqlCommand(query, myConnection); // Создание объекта, выполняющего запрос к БД:

                SqlDataReader reader = command.ExecuteReader(); // Получение объекта для чтения данных из БД, содержащих несколько строк и столбцов:

                List<string[]> data1 = new List<string[]>(); // Создание списка List для хранения полученных данных. Каждая строка будет представлена элементом списка, а столбец – элементом строкового массива string[]:

                while (reader.Read()) // В цикле построчно читаем данные строки (каждого её столбца), предварительно создав новый элемент списка List:
                {
                    People.Add(new PersonModel { Count = (int)reader[0], Name = reader[1].ToString(), DataTime = reader[2].ToString() });
                }

                reader.Close(); // Закрываем SqlDataReader:

                myConnection.Close(); // Разрываем соединение с базой данных:
            }

            catch (Exception)
            {
                MessageBox.Show(
                     "Ошибка! База для загрузки не найдена");
            }
        }

        public void SaveBase() // код взят с Метанит.сом 
        {
            try
            {
            string connectionString = "Data Source=.\\SQLEXPRESS; Initial Catalog= Test; Integrated Security=true;";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
        //  command.CommandText = @"INSERT INTO Users VALUES (@Id, @Name, @Date)";
            command.CommandText = @"INSERT INTO Users VALUES (@Name, @Date)";
         // command.Parameters.Add("@Id", SqlDbType.Int);
            command.Parameters.Add("@Name", SqlDbType.NVarChar, 50);
            command.Parameters.Add("@Date", SqlDbType.DateTime);
                for (var i = 0; i < People.Count; i++)
                {
               //   command.Parameters["@Id"].Value = People[i].Count;
                    command.Parameters["@Name"].Value = People[i].Name;
                    command.Parameters["@Date"].Value = People[i].DataTime;
                    command.ExecuteNonQuery();
                }
            }

            catch
            {
                MessageBox.Show("Ошибка! Невозможно выполнить сохранение.");
            }
        }

        public void ClearBase()
        {
            string connectionString = "Data Source=.\\SQLEXPRESS; Initial Catalog= Test; Integrated Security=true;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = @"DELETE Users ";
                command.ExecuteNonQuery();
            }
        }
    }
}





