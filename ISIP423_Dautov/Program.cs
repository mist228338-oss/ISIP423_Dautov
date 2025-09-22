using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreManagementConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Store store = new Store();
            store.Run();
        }
    }

    class Product
    {
        private static int nextId = 1000; // Начинаем с 1000, чтобы гарантировать, что код начинается с 1

        public string Code { get; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public bool InStock => Quantity > 0;

        public Product(string name, decimal price, int quantity, string category)
        {
            Code = "1" + (nextId++).ToString();
            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
        }

        public override string ToString()
        {
            return $"Код: {Code}\nНазвание: {Name}\nЦена: {Price} руб.\nКоличество: {Quantity}\nВ наличии: {(InStock ? "Да" : "Нет")}\nКатегория: {Category}";
        }
    }

    class Store
    {
        private List<Product> products = new List<Product>();
        private readonly List<string> categories = new List<string> { "Электроника", "Одежда", "Продукты" };

        public void Run()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== СИСТЕМА УЧЁТА ТОВАРОВ ===");
                Console.WriteLine("1. Добавить товар");
                Console.WriteLine("2. Удалить товар");
                Console.WriteLine("3. Заказать поставку товара");
                Console.WriteLine("4. Продать товар");
                Console.WriteLine("5. Поиск товаров");
                Console.WriteLine("6. Показать все товары");
                Console.WriteLine("7. Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddProduct();
                        break;
                    case "2":
                        RemoveProduct();
                        break;
                    case "3":
                        OrderSupply();
                        break;
                    case "4":
                        SellProduct();
                        break;
                    case "5":
                        SearchProducts();
                        break;
                    case "6":
                        ShowAllProducts();
                        break;
                    case "7":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void AddProduct()
        {
            Console.Clear();
            Console.WriteLine("=== ДОБАВЛЕНИЕ ТОВАРА ===");

            Console.Write("Название товара: ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Название не может быть пустым!");
                Console.ReadKey();
                return;
            }

            Console.Write("Цена товара: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
            {
                Console.WriteLine("Некорректная цена!");
                Console.ReadKey();
                return;
            }

            Console.Write("Количество товара: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 0)
            {
                Console.WriteLine("Некорректное количество!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Доступные категории:");
            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {categories[i]}");
            }

            Console.Write("Выберите категорию (номер): ");
            if (!int.TryParse(Console.ReadLine(), out int categoryIndex) || categoryIndex < 1 || categoryIndex > categories.Count)
            {
                Console.WriteLine("Некорректный выбор категории!");
                Console.ReadKey();
                return;
            }

            string category = categories[categoryIndex - 1];

            Product product = new Product(name, price, quantity, category);
            products.Add(product);

            Console.WriteLine($"Товар успешно добавлен! Код товара: {product.Code}");
            Console.ReadKey();
        }

        private void RemoveProduct()
        {
            Console.Clear();
            Console.WriteLine("=== УДАЛЕНИЕ ТОВАРА ===");

            Console.Write("Введите код товара для удаления: ");
            string code = Console.ReadLine();

            Product product = products.FirstOrDefault(p => p.Code == code);
            if (product == null)
            {
                Console.WriteLine("Товар с таким кодом не найден!");
                Console.ReadKey();
                return;
            }

            products.Remove(product);
            Console.WriteLine("Товар успешно удален!");
            Console.ReadKey();
        }

        private void OrderSupply()
        {
            Console.Clear();
            Console.WriteLine("=== ЗАКАЗ ПОСТАВКИ ТОВАРА ===");

            Console.Write("Введите код товара: ");
            string code = Console.ReadLine();

            Product product = products.FirstOrDefault(p => p.Code == code);
            if (product == null)
            {
                Console.WriteLine("Товар с таким кодом не найден!");
                Console.ReadKey();
                return;
            }

            Console.Write("Введите количество для заказа: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Некорректное количество!");
                Console.ReadKey();
                return;
            }

            product.Quantity += quantity;
            Console.WriteLine($"Поставка успешно оформлена! Новое количество: {product.Quantity}");
            Console.ReadKey();
        }

        private void SellProduct()
        {
            Console.Clear();
            Console.WriteLine("=== ПРОДАЖА ТОВАРА ===");

            Console.Write("Введите код товара: ");
            string code = Console.ReadLine();

            Product product = products.FirstOrDefault(p => p.Code == code);
            if (product == null)
            {
                Console.WriteLine("Товар с таким кодом не найден!");
                Console.ReadKey();
                return;
            }

            if (!product.InStock)
            {
                Console.WriteLine("Товара нет в наличии!");
                Console.ReadKey();
                return;
            }

            Console.Write("Введите количество для продажи: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Некорректное количество!");
                Console.ReadKey();
                return;
            }

            if (product.Quantity < quantity)
            {
                Console.WriteLine($"Недостаточно товара! Доступно: {product.Quantity}");
                Console.ReadKey();
                return;
            }

            product.Quantity -= quantity;
            decimal total = product.Price * quantity;

            Console.WriteLine($"Продажа успешно оформлена! Сумма: {total} руб.");
            Console.WriteLine($"Остаток товара: {product.Quantity}");
            Console.ReadKey();
        }

        private void SearchProducts()
        {
            Console.Clear();
            Console.WriteLine("=== ПОИСК ТОВАРОВ ===");
            Console.WriteLine("1. По коду");
            Console.WriteLine("2. По названию");
            Console.WriteLine("3. По категории");
            Console.Write("Выберите тип поиска: ");

            string choice = Console.ReadLine();
            List<Product> results = new List<Product>();

            switch (choice)
            {
                case "1":
                    Console.Write("Введите код товара: ");
                    string code = Console.ReadLine();
                    results = products.Where(p => p.Code.Contains(code)).ToList();
                    break;
                case "2":
                    Console.Write("Введите название товара: ");
                    string name = Console.ReadLine();
                    results = products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case "3":
                    Console.WriteLine("Доступные категории:");
                    for (int i = 0; i < categories.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {categories[i]}");
                    }
                    Console.Write("Выберите категорию (номер): ");
                    if (!int.TryParse(Console.ReadLine(), out int categoryIndex) || categoryIndex < 1 || categoryIndex > categories.Count)
                    {
                        Console.WriteLine("Некорректный выбор категории!");
                        Console.ReadKey();
                        return;
                    }
                    string category = categories[categoryIndex - 1];
                    results = products.Where(p => p.Category == category).ToList();
                    break;
                default:
                    Console.WriteLine("Неверный выбор!");
                    Console.ReadKey();
                    return;
            }

            if (results.Count == 0)
            {
                Console.WriteLine("Товары не найдены!");
            }
            else
            {
                Console.WriteLine($"Найдено товаров: {results.Count}");
                Console.WriteLine("====================================");
                foreach (var product in results)
                {
                    Console.WriteLine(product);
                    Console.WriteLine("====================================");
                }
            }

            Console.ReadKey();
        }

        private void ShowAllProducts()
        {
            Console.Clear();
            Console.WriteLine("=== ВСЕ ТОВАРЫ ===");

            if (products.Count == 0)
            {
                Console.WriteLine("Товары отсутствуют!");
            }
            else
            {
                foreach (var product in products)
                {
                    Console.WriteLine(product);
                    Console.WriteLine("====================================");
                }
            }

            Console.ReadKey();
        }
    }
}