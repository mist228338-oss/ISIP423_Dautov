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
public class Teacher : Person, IDisplayable
{
    private string _teacherId;
    private string _department;
    private List<Course> _courses;

    public Teacher(string name, int age, string email, string teacherId, string department)
        : base(name, age, email)
    {
        _teacherId = teacherId;
        _department = department;
        _courses = new List<Course>();
    }

    public string TeacherId => _teacherId;
    public string Department => _department;
    public IReadOnlyList<Course> Courses => _courses.AsReadOnly();

    public bool AssignCourse(Course course)
    {
        if (!_courses.Contains(course))
        {
            _courses.Add(course);
            return true;
        }
        return false;
    }

    public string GetCoursesInfo()
    {
        if (!_courses.Any())
            return "Преподаватель не ведет курсы";

        var coursesInfo = _courses.Select(c =>
            $"- {c.Name} (Студентов: {c.Students.Count})");
        return string.Join("\n", coursesInfo);
    }

    public override string DisplayInfo()
    {
        return $"Преподаватель: {_name} (ID: {_teacherId}), Возраст: {_age}, Кафедра: {_department}, Email: {_email}";
    }
}

// Класс Course
public class Course : IDisplayable
{
    private string _name;
    private string _courseCode;
    private int _credits;
    private Teacher _teacher;
    private List<Student> _students;

    public Course(string name, string courseCode, int credits)
    {
        _name = name;
        _courseCode = courseCode;
        _credits = credits;
        _students = new List<Student>();
    }

    public string Name => _name;
    public string CourseCode => _courseCode;
    public int Credits => _credits;
    public Teacher Teacher => _teacher;
    public IReadOnlyList<Student> Students => _students.AsReadOnly();

    public bool AssignTeacher(Teacher teacher)
    {
        if (_teacher != teacher)
        {
            _teacher = teacher;
            teacher.AssignCourse(this);
            return true;
        }
        return false;
    }

    public bool AddStudent(Student student)
    {
        if (!_students.Contains(student))
        {
            _students.Add(student);
            return true;
        }
        return false;
    }

    public string GetStudentsInfo()
    {
        if (!_students.Any())
            return "На курс не записаны студенты";

        var studentsInfo = _students.Select(s =>
            $"- {s.Name} (ID: {s.StudentId})");
        return string.Join("\n", studentsInfo);
    }

    public string DisplayInfo()
    {
        string teacherInfo = _teacher?.Name ?? "Не назначен";
        return $"Курс: {_name} (Код: {_courseCode}), Кредиты: {_credits}, Преподаватель: {teacherInfo}, Студентов: {_students.Count}";
    }
}

public class UniversitySystem
{
    private Dictionary<string, Student> _students;
    private Dictionary<string, Teacher> _teachers;
    private Dictionary<string, Course> _courses;

    public UniversitySystem()
    {
        _students = new Dictionary<string, Student>();
        _teachers = new Dictionary<string, Teacher>();
        _courses = new Dictionary<string, Course>();
    }
    public bool AddStudent(string name, int age, string email, string studentId)
    {
        if (_students.ContainsKey(studentId))
            return false;
        var student = new Student(name, age, email, studentId);
        _students[studentId] = student;
        return true;
    }
    public Student GetStudent(string studentId)
    {
        return _students.GetValueOrDefault(studentId);
    }
    public List<Student> GetAllStudents()
    {
        return _students.Values.ToList();
    }
    public bool AddTeacher(string name, int age, string email, string teacherId, string department)
    {
        if (_teachers.ContainsKey(teacherId))
            return false;

        var teacher = new Teacher(name, age, email, teacherId, department);
        _teachers[teacherId] = teacher;
        return true;
    }
    public Teacher GetTeacher(string teacherId)
    {
        return _teachers.GetValueOrDefault(teacherId);
    }
    public List<Teacher> GetAllTeachers()
    {
        return _teachers.Values.ToList();
    }
    public bool AddCourse(string name, string courseCode, int credits)
    {
        if (_courses.ContainsKey(courseCode))
            return false;

        var course = new Course(name, courseCode, credits);
        _courses[courseCode] = course;
        return true;
    }
    public Course GetCourse(string courseCode)
    {
        return _courses.GetValueOrDefault(courseCode);
    }
    public List<Course> GetAllCourses()
    {
        return _courses.Values.ToList();
    }
    public bool EnrollStudentInCourse(string studentId, string courseCode)
    {
        var student = GetStudent(studentId);
        var course = GetCourse(courseCode);

        if (student != null && course != null)
        {
            return student.EnrollCourse(course);
        }
        return false;
    }
    public bool AssignTeacherToCourse(string teacherId, string courseCode)
    {
        var teacher = GetTeacher(teacherId);
        var course = GetCourse(courseCode);

        if (teacher != null && course != null)
        {
            return course.AssignTeacher(teacher);
        }
        return false;
    }
}
public class UniversityConsole
{
    private UniversitySystem _university;
    public UniversityConsole()
    {
        _university = new UniversitySystem();
        InitializeSampleData();
    }
    private void InitializeSampleData()
    {
        _university.AddTeacher("Иван Петров", 45, "ivan.petrov@university.ru", "T001", "Компьютерные науки");
        _university.AddTeacher("Мария Сидорова", 38, "maria.sidorova@university.ru", "T002", "Математика");
        _university.AddStudent("Алексей Иванов", 20, "alex.ivanov@university.ru", "S001");
        _university.AddStudent("Екатерина Смирнова", 19, "ekaterina.smirnova@university.ru", "S002");
        _university.AddStudent("Дмитрий Кузнецов", 21, "dmitry.kuznetsov@university.ru", "S003");
        _university.AddCourse("Программирование на Python", "CS101", 4);
        _university.AddCourse("Алгебра и геометрия", "MATH201", 3);
        _university.AddCourse("Базы данных", "CS202", 4);
        _university.AssignTeacherToCourse("T001", "CS101");
        _university.AssignTeacherToCourse("T001", "CS202");
        _university.AssignTeacherToCourse("T002", "MATH201");
        _university.EnrollStudentInCourse("S001", "CS101");
        _university.EnrollStudentInCourse("S001", "MATH201");
        _university.EnrollStudentInCourse("S002", "CS101");
        _university.EnrollStudentInCourse("S003", "CS202");
    }
    public void DisplayMenu()
    {
        Console.WriteLine("\n" + new string('=', 50));
        Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ УНИВЕРСИТЕТОМ");
        Console.WriteLine(new string('=', 50));
        Console.WriteLine("1. Управление студентами");
        Console.WriteLine("2. Управление преподавателями");
        Console.WriteLine("3. Управление курсами");
        Console.WriteLine("4. Показать все данные");
        Console.WriteLine("5. Записать студента на курс");
        Console.WriteLine("6. Назначить преподавателя на курс");
        Console.WriteLine("0. Выход");
        Console.WriteLine(new string('=', 50));
    }
    public void Run()
    {
        while (true)
        { DisplayMenu();
            Console.Write("Выберите действие: ");
            string choice = Console.ReadLine()?.Trim() ?? "";
            switch (choice)
            {
                case "1":
                    ManageStudents();
                    break;
                case "2":
                    ManageTeachers();
                    break;
                case "3":
                    ManageCourses();
                    break;
                case "4":
                    ShowAllData();
                    break;
                case "5":
                    EnrollStudent();
                    break;
                case "6":
                    AssignTeacher();
                    break;
                case "0":
                    Console.WriteLine("Выход из системы...");
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }
    private void ManageStudents()
    {
        while (true)
        {
            Console.WriteLine("\n--- Управление студентами ---");
            Console.WriteLine("1. Добавить студента");
            Console.WriteLine("2. Показать всех студентов");
            Console.WriteLine("3. Найти студента по ID");
            Console.WriteLine("4. Показать курсы студента");
            Console.WriteLine("0. Назад");

            Console.Write("Выберите действие: ");
            string choice = Console.ReadLine()?.Trim() ?? "";
            switch (choice)
            {
                case "1":
                    AddStudent();
                    break;
                case "2":
                    ShowAllStudents();
                    break;
                case "3":
                    FindStudent();
                    break;
                case "4":
                    ShowStudentCourses();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }
    }
    private void ManageTeachers()
    {
        while (true)
        {
            Console.WriteLine("\n--- Управление преподавателями ---");
            Console.WriteLine("1. Добавить преподавателя");
            Console.WriteLine("2. Показать всех преподавателей");
            Console.WriteLine("3. Найти преподавателя по ID");
            Console.WriteLine("4. Показать курсы преподавателя");
            Console.WriteLine("0. Назад");
            Console.Write("Выберите действие: ");
            string choice = Console.ReadLine()?.Trim() ?? "";
            switch (choice)
            {
                case "1":
                    AddTeacher();
                    break;
                case "2":
                    ShowAllTeachers();
                    break;
                case "3":
                    FindTeacher();
                    break;
                case "4":
                    ShowTeacherCourses();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }
    }
    private void ManageCourses()
    {
        while (true)
        {
            Console.WriteLine("\n--- Управление курсами ---");
            Console.WriteLine("1. Добавить курс");
            Console.WriteLine("2. Показать все курсы");
            Console.WriteLine("3. Найти курс по коду");
            Console.WriteLine("4. Показать студентов курса");
            Console.WriteLine("0. Назад");
            Console.Write("Выберите действие: ");
            string choice = Console.ReadLine()?.Trim() ?? "";

            switch (choice)
            {
                case "1":
                    AddCourse();
                    break;
                case "2":
                    ShowAllCourses();
                    break;
                case "3":
                    FindCourse();
                    break;
                case "4":
                    ShowCourseStudents();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }
    }   
    private void AddStudent()
    {
        Console.WriteLine("\n--- Добавление студента ---");
        Console.Write("Имя: ");
        string name = Console.ReadLine() ?? "";
        Console.Write("Возраст: ");
        int age = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Email: ");
        string email = Console.ReadLine() ?? "";
        Console.Write("ID студента: ");
        string studentId = Console.ReadLine() ?? "";

        if (_university.AddStudent(name, age, email, studentId))
        {
            Console.WriteLine("Студент успешно добавлен!");
        }
        else
        {
            Console.WriteLine("Ошибка: студент с таким ID уже существует.");
        }
    }