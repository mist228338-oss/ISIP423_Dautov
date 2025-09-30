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
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Ошибка ввода!");
                    continue;
                }

                switch (choice)
                {
                    case 1: AddBook(); break;
                    case 2: DeleteBook(); break;
                    case 3: SearchBooks(); break;
                    case 4: SortBooks(); break;
                    case 5: ShowPriceExtremes(); break;
                    case 6: ShowAuthorStats(); break;
                    case 0: return;
                    default: Console.WriteLine("Неверный вариант!"); break;
                }
            }
        }
        static void InitializeSampleData()
        {
            _books.AddRange(new[]
            {
                new Book("Властелин Колец", "Толкин", Genre.Fantasy, 1954, 1500),
                new Book("1984", "Оруэлл", Genre.ScienceFiction, 1949, 800),
                new Book("Убийство в Восточном экспрессе", "Кристи", Genre.Mystery, 1934, 700),
                new Book("Дюна", "Герберт", Genre.ScienceFiction, 1965, 950),
                new Book("Дракула", "Стокер", Genre.Horror, 1897, 650)
            });
        }

        static void AddBook()
        {
            Console.WriteLine("\n--- Добавление книги ---");

            Console.Write("Название: ");
            var title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Название не может быть пустым!");
                return;
            }

            Console.Write("Автор: ");
            var author = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(author))
            {
                Console.WriteLine("Автор не может быть пустым!");
                return;
            }