csharp
using System;
using System.Collections.Generic;
using System.Linq;

// Абстрактный класс Person (Абстракция)
public abstract class Person
{
    protected string _name;
    protected int _age;
    protected string _email;
    protected string _id;

    public Person(string name, int age, string email)
    {
        _name = name;
        _age = age;
        _email = email;
        _id = GenerateId();
    }
    public string Name => _name;
    public int Age => _age;
    public string Email => _email;
    public string Id => _id;

    private string GenerateId()
    {
        return Guid.NewGuid().ToString().Substring(0, 8);
    }

    public abstract string DisplayInfo();
    public override string ToString() => DisplayInfo();
}

// Интерфейс для отображения информации
public interface IDisplayable
{
    string DisplayInfo();
}

// Класс Student (Наследование)
public class Student : Person, IDisplayable
{
    private string _studentId;
    private List<Course> _courses;

    public Student(string name, int age, string email, string studentId)
        : base(name, age, email)
    {
        _studentId = studentId;
        _courses = new List<Course>();
    }

    public string StudentId => _studentId;
    public IReadOnlyList<Course> Courses => _courses.AsReadOnly();

    public bool EnrollCourse(Course course)
    {
        if (!_courses.Contains(course))
        {
            _courses.Add(course);
            course.AddStudent(this);
            return true;
        }
        return false;
    }

    public string GetCoursesInfo()
    {
        if (!_courses.Any())
            return "Студент не записан на курсы";

        var coursesInfo = _courses.Select(c =>
            $"- {c.Name} (Преподаватель: {c.Teacher?.Name ?? "Не назначен"})");
        return string.Join("\n", coursesInfo);
    }

    public override string DisplayInfo()
    {
        return $"Студент: {_name} (ID: {_studentId}), Возраст: {_age}, Email: {_email}";
    }
}
