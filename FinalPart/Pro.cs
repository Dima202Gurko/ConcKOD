using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;

public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public int Rating { get; set; }
}

public class StudentService
{
    private string _filePath;
    private List<Student> _students;

    public StudentService(string filePath)
    {
        _filePath = filePath;
        LoadData();
    }

    private void LoadData()
    {
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            _students = JsonConvert.DeserializeObject<List<Student>>(json) ?? new List<Student>();
        }
        else
        {
            _students = new List<Student>();
        }
    }

    public void SaveData()
    {
        string json = JsonConvert.SerializeObject(_students, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(_filePath, json);
    }

    public void DisplayStudents()
    {
        foreach (var student in _students)
        {
            Console.WriteLine($"ID: {student.Id}, ФИО: {student.FullName}, Рейтинг: {student.Rating}");
        }
    }

    public void SortByRatingUp(bool ascending = true)
    {
        _students = ascending ? _students.OrderBy(s => s.Rating).ToList() : _students.OrderByDescending(s => s.Rating).ToList();
    }
    public void SortByRatingDown()
    {
        _students = _students.OrderByDescending(s => s.Rating).ToList();
    }


    public void SortByName()
    {
        _students = _students.OrderBy(s => s.FullName).ToList();
    }

    public void UpdateRatings(string newFilePath)
    {
        if (!File.Exists(newFilePath))
        {
            Console.WriteLine("Файл с новыми баллами не найден.");
            return;
        }

        string json = File.ReadAllText(newFilePath);
        var newScores = JsonConvert.DeserializeObject<List<Student>>(json) ?? new List<Student>();

        foreach (var newStudent in newScores)
        {
            var existingStudent = _students.FirstOrDefault(s => s.Id == newStudent.Id);
            if (existingStudent != null)
            {
                existingStudent.Rating += newStudent.Rating;
            }
            else
            {
                _students.Add(newStudent);
            }
        }

        SaveData();
    }
    class Program
    {
        static bool life = true;
        static bool lifeMenu = true;
        static void Main(string[] arg)
        {

            while (life)
            {
                try
                {
                    Console.WriteLine("1.Зайти в систему рейтинга учеников.\n2.Выйти из приложения.");
                    switch (int.Parse(Console.ReadLine()))
                    {
                        case 1:
                            Enter();
                            break;
                        case 2:
                            life = false;
                            break;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Неверный формат ответа");
                    Console.WriteLine(ex.Message);
                }

            }
        }

        public static void Enter()
        {
            try
            {
                while (lifeMenu)
                {
                    Console.WriteLine("");
                    Console.WriteLine("1.Вывести рейтинг активностей.\n2.Вывести отсортированный рейтинг по возрастанию.\n3.Вывести отсортированный рейтинг по убыванию.\n4.Вывести отсортированный рейтинг по алфавиту.\n5.Обновление базы рейтинга.\n6.Очистить консоль.\n7.Выйти из меню рейтинга.");
                    StudentService StuSe = new StudentService("Data_Example.json");
                    switch (int.Parse(Console.ReadLine()))
                    {
                        case 1:
                            StuSe.DisplayStudents();
                            break;
                        case 2:
                            StuSe.SortByRatingUp();
                            StuSe.DisplayStudents();
                            break;
                        case 3:
                            StuSe.SortByRatingDown();
                            StuSe.DisplayStudents();
                            break;
                        case 4:
                            StuSe.SortByName();
                            StuSe.DisplayStudents();
                            break;
                        case 5:
                            StuSe.UpdateRatings("Data_Example_1.json");
                            StuSe.DisplayStudents();
                            break;
                        case 6:
                            Console.Clear();
                            break;
                        case 7:
                            return;
                        default:
                            Console.Clear();
                            break;
                    } 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Неверный формат ответа");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
