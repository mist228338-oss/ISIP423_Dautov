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