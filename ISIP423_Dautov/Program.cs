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