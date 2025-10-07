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


