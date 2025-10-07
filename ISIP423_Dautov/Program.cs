using System;
using System.Collections.Generic;
using System.Linq;

public abstract class Person
{
    protected string _name;
    protected int _age;
    protected string _email;
    public string Name => _name;
    public int Age => _age;
    public string Email => _email;

    public Person(string name, int age, string email)
    {
        _name = name;
        _age = age;
        _email = email;
    }

    public abstract string DisplayInfo();
}

public class Student : Person
{
    private string _studentId;
    private List<Course> _courses = new List<Course>();
    public string StudentId => _studentId;

    public Student(string name, int age, string email, string studentId) : base(name, age, email)
        => _studentId = studentId;

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

    public string GetCoursesInfo() => _courses.Any() ?
        string.Join("\n", _courses.Select(c => $"- {c.Name}")) : "Нет курсов";

    public override string DisplayInfo() =>
        $"Студент: {_name} (ID: {_studentId}), Возраст: {_age}";
}

public class Teacher : Person
{
    private string _teacherId;
    private string _department;
    public string TeacherId => _teacherId;
    public string Department => _department;

    public Teacher(string name, int age, string email, string teacherId, string department)
        : base(name, age, email)
    {
        _teacherId = teacherId;
        _department = department;
    }

    public override string DisplayInfo() =>
        $"Преподаватель: {_name} (ID: {_teacherId}), Кафедра: {_department}";
}

public class Course
{
    private string _name;
    private string _courseCode;
    private Teacher _teacher;
    private List<Student> _students = new List<Student>();

    public string Name => _name;
    public string CourseCode => _courseCode;
    public Teacher Teacher => _teacher;
    public IReadOnlyList<Student> Students => _students.AsReadOnly();

    public Course(string name, string courseCode)
    {
        _name = name;
        _courseCode = courseCode;
    }

    public bool AssignTeacher(Teacher teacher) => (_teacher = teacher) != null;
    public bool AddStudent(Student student) => !_students.Contains(student);

    public string DisplayInfo() =>
        $"Курс: {_name} (Код: {_courseCode}), Преподаватель: {_teacher?.Name ?? "Нет"}, Студентов: {_students.Count}";
}

public class UniversitySystem
{
    private Dictionary<string, Student> _students = new Dictionary<string, Student>();
    private Dictionary<string, Teacher> _teachers = new Dictionary<string, Teacher>();
    private Dictionary<string, Course> _courses = new Dictionary<string, Course>();

    public bool AddStudent(string name, int age, string email, string studentId) =>
        _students.TryAdd(studentId, new Student(name, age, email, studentId));

    public bool AddTeacher(string name, int age, string email, string teacherId, string department) =>
        _teachers.TryAdd(teacherId, new Teacher(name, age, email, teacherId, department));

    public bool AddCourse(string name, string courseCode) =>
        _courses.TryAdd(courseCode, new Course(name, courseCode));

    public Student GetStudent(string id) => _students.GetValueOrDefault(id);
    public Teacher GetTeacher(string id) => _teachers.GetValueOrDefault(id);
    public Course GetCourse(string code) => _courses.GetValueOrDefault(code);

    public List<Student> GetAllStudents() => _students.Values.ToList();
    public List<Teacher> GetAllTeachers() => _teachers.Values.ToList();
    public List<Course> GetAllCourses() => _courses.Values.ToList();

    public bool EnrollStudent(string studentId, string courseCode)
    {
        var student = GetStudent(studentId);
        var course = GetCourse(courseCode);
        return student != null && course != null && student.EnrollCourse(course);
    }

    public bool AssignTeacher(string teacherId, string courseCode)
    {
        var teacher = GetTeacher(teacherId);
        var course = GetCourse(courseCode);
        return teacher != null && course != null && course.AssignTeacher(teacher);
    }
}

class Program
{
    static UniversitySystem uni = new UniversitySystem();

    static void Main()
    {
        InitSampleData();
        while (true)
        {
            Console.WriteLine("\n1. Студенты\n2. Преподаватели\n3. Курсы\n4. Все данные\n5. Записать на курс\n0. Выход");
            switch (Console.ReadLine())
            {
                case "1": ManageStudents(); break;
                case "2": ManageTeachers(); break;
                case "3": ManageCourses(); break;
                case "4": ShowAll(); break;
                case "5": EnrollStudent(); break;
                case "0": return;
            }
        }
    }

    static void InitSampleData()
    {
        uni.AddTeacher("Иван Петров", 45, "ivan@mail.ru", "T001", "Информатика");
        uni.AddStudent("Алексей Иванов", 20, "alex@mail.ru", "S001");
        uni.AddCourse("Программирование", "CS101");
        uni.AssignTeacher("T001", "CS101");
    }

    static void ManageStudents()
    {
        while (true)
        {
            Console.WriteLine("\n1. Добавить\n2. Список\n3. Найти\n0. Назад");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("Имя Возраст Email ID: ");
                    var data = Console.ReadLine().Split();
                    if (data.Length == 4) uni.AddStudent(data[0], int.Parse(data[1]), data[2], data[3]);
                    break;
                case "2":
                    uni.GetAllStudents().ForEach(s => Console.WriteLine(s.DisplayInfo()));
                    break;
                case "3":
                    Console.Write("ID: ");
                    var student = uni.GetStudent(Console.ReadLine());
                    Console.WriteLine(student?.DisplayInfo() ?? "Не найден");
                    break;
                case "0": return;
            }
        }
    }

    static void ManageTeachers()
    {
        while (true)
        {
            Console.WriteLine("\n1. Добавить\n2. Список\n3. Найти\n0. Назад");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("Имя Возраст Email ID Кафедра: ");
                    var data = Console.ReadLine().Split();
                    if (data.Length == 5) uni.AddTeacher(data[0], int.Parse(data[1]), data[2], data[3], data[4]);
                    break;
                case "2":
                    uni.GetAllTeachers().ForEach(t => Console.WriteLine(t.DisplayInfo()));
                    break;
                case "3":
                    Console.Write("ID: ");
                    var teacher = uni.GetTeacher(Console.ReadLine());
                    Console.WriteLine(teacher?.DisplayInfo() ?? "Не найден");
                    break;
                case "0": return;
            }
        }
    }

    static void ManageCourses()
    {
        while (true)
        {
            Console.WriteLine("\n1. Добавить\n2. Список\n3. Найти\n0. Назад");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("Название Код: ");
                    var data = Console.ReadLine().Split();
                    if (data.Length == 2) uni.AddCourse(data[0], data[1]);
                    break;
                case "2":
                    uni.GetAllCourses().ForEach(c => Console.WriteLine(c.DisplayInfo()));
                    break;
                case "3":
                    Console.Write("Код: ");
                    var course = uni.GetCourse(Console.ReadLine());
                    Console.WriteLine(course?.DisplayInfo() ?? "Не найден");
                    break;
                case "0": return;
            }
        }
    }

    static void ShowAll()
    {
        Console.WriteLine("\n--- СТУДЕНТЫ ---");
        uni.GetAllStudents().ForEach(s => Console.WriteLine(s.DisplayInfo()));
        Console.WriteLine("\n--- ПРЕПОДАВАТЕЛИ ---");
        uni.GetAllTeachers().ForEach(t => Console.WriteLine(t.DisplayInfo()));
        Console.WriteLine("\n--- КУРСЫ ---");
        uni.GetAllCourses().ForEach(c => Console.WriteLine(c.DisplayInfo()));
    }

    static void EnrollStudent()
    {
        Console.Write("ID студента и код курса: ");
        var data = Console.ReadLine().Split();
        if (data.Length == 2)
            Console.WriteLine(uni.EnrollStudent(data[0], data[1]) ? "Успешно!" : "Ошибка");
    }
}