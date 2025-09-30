using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryApp
{
    public enum Genre
    {
        Fantasy,
        ScienceFiction,
        Mystery,
        Romance,
        Horror
    }
    public class Book
    {
        private static int _nextId = 1;

        public int Id { get; }
        public string Title { get; }
        public string Author { get; }
        public Genre Genre { get; }
        public int Year { get; }
        public decimal Price { get; }
        public Book(string title, string author, Genre genre, int year, decimal price)
        {
            Id = _nextId++;
            Title = title;
            Author = author;
            Genre = genre;
            Year = year;
            Price = price;
        }

        public override string ToString()
        {
            return $"ID: {Id}\nНазвание: {Title}\nАвтор: {Author}\nЖанр: {Genre}\nГод: {Year}\nЦена: {Price:C}\n";
        }
    }
    class Program
    {
        private static List<Book> _books = new List<Book>();

        static void Main()
        {
            InitializeSampleData();

            while (true)
            {
                Console.WriteLine("\n=== Библиотечная система ===");
                Console.WriteLine("1. Добавить книгу");
                Console.WriteLine("2. Удалить книгу по ID");
                Console.WriteLine("3. Поиск книг");
                Console.WriteLine("4. Сортировка книг");
                Console.WriteLine("5. Самая дорогая/дешевая книга");
                Console.WriteLine("6. Статистика по авторам");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");
