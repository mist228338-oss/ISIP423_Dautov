using System;
using System.Globalization;

namespace DailyExpensesTracker
{
    class Program
    {
        struct Expense
        {
            public string Name;
            public decimal Amount;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("💰 Трекер ежедневных расходов");
            Console.WriteLine("=============================");

            int operationsCount = GetOperationsCount();

            Expense[] expenses = InputExpenses(operationsCount);

            bool exit = false;
            while (!exit)
            {
                DisplayMenu();
                int choice = GetMenuChoice();

                switch (choice)
                {
                    case 1:
                        DisplayExpenses(expenses);
                        break;
                    case 2:
                        ShowStatistics(expenses);
                        break;
                    case 3:
                        SortExpensesByPrice(ref expenses);
                        Console.WriteLine("✅ Данные отсортированы по цене!");
                        break;
                    case 4:
                        ConvertCurrency(expenses);
                        break;
                    case 5:
                        SearchByName(expenses);
                        break;
                    case 6:
                        exit = true;
                        Console.WriteLine("👋 До свидания!");
                        break;
                    default:
                        Console.WriteLine("❌ Неверный выбор меню!");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        static int GetOperationsCount()
        {
            int count;
            do
            {
                Console.Write("Введите количество операций (2-40): ");
                if (int.TryParse(Console.ReadLine(), out count) && count >= 2 && count <= 40)
                {
                    return count;
                }
                Console.WriteLine("❌ Ошибка! Введите число от 2 до 40.");
            } while (true);
        }

        static Expense[] InputExpenses(int count)
        {
            Expense[] expenses = new Expense[count];
            Console.WriteLine($"\nВведите {count} трат в формате: Название; Сумма");
            Console.WriteLine("Пример: Влажные салфетки \"Лента\"; 235");

            for (int i = 0; i < count; i++)
            {
                bool validInput = false;
                while (!validInput)
                {
                    Console.Write($"{i + 1}. ");
                    string input = Console.ReadLine();

                    if (TryParseExpense(input, out Expense expense))
                    {
                        expenses[i] = expense;
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("❌ Неверный формат! Пример: Продукты; 150.50");
                    }
                }
            }

            return expenses;
        }

        static bool TryParseExpense(string input, out Expense expense)
        {
            expense = new Expense();
            var parts = input.Split(';', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
                return false;

            expense.Name = parts[0].Trim();
            if (string.IsNullOrWhiteSpace(expense.Name))
                return false;

            if (decimal.TryParse(parts[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount) && amount > 0)
            {
                expense.Amount = amount;
                return true;
            }

            return false;
        }

        static void DisplayMenu()
        {
            Console.WriteLine("\n📋 Главное меню:");
            Console.WriteLine("1. Вывод данных");
            Console.WriteLine("2. Статистика (среднее, макс, мин, сумма)");
            Console.WriteLine("3. Сортировка по цене");
            Console.WriteLine("4. Конвертация валюты");
            Console.WriteLine("5. Поиск по названию");
            Console.WriteLine("6. Выход");
            Console.Write("Выберите пункт меню (1-6): ");
        }

        static int GetMenuChoice()
        {
            if (int.TryParse(Console.ReadLine(), out int choice))
                return choice;
            return 0;
        }

        static void DisplayExpenses(Expense[] expenses)
        {
            Console.WriteLine("\n📊 Список расходов:");
            Console.WriteLine("====================");
            Console.WriteLine("№  Название\t\tСумма (руб)");
            Console.WriteLine("--------------------------------");

            decimal total = 0;
            for (int i = 0; i < expenses.Length; i++)
            {
                Console.WriteLine($"{i + 1,-2} {expenses[i].Name,-20} {expenses[i].Amount,10:F2}");
                total += expenses[i].Amount;
            }

            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Итого:\t\t\t{total,10:F2} руб");
        }

        // 2. Статистика
        static void ShowStatistics(Expense[] expenses)
        {
            if (expenses.Length == 0)
            {
                Console.WriteLine("❌ Нет данных для статистики!");
                return;
            }

            decimal total = 0;
            decimal max = decimal.MinValue;
            decimal min = decimal.MaxValue;
            string maxName = "", minName = "";

            foreach (var expense in expenses)
            {
                total += expense.Amount;

                if (expense.Amount > max)
                {
                    max = expense.Amount;
                    maxName = expense.Name;
                }

                if (expense.Amount < min)
                {
                    min = expense.Amount;
                    minName = expense.Name;
                }
            }

            decimal average = total / expenses.Length;

            Console.WriteLine("\n📈 Статистика расходов:");
            Console.WriteLine("========================");
            Console.WriteLine($"Общая сумма: {total:F2} руб");
            Console.WriteLine($"Средний чек: {average:F2} руб");
            Console.WriteLine($"Максимальная трата: {max:F2} руб ({maxName})");
            Console.WriteLine($"Минимальная трата: {min:F2} руб ({minName})");
            Console.WriteLine($"Количество операций: {expenses.Length}");
        }

        // 3. Сортировка по цене (пузырьковая)
        static void SortExpensesByPrice(ref Expense[] expenses)
        {
            for (int i = 0; i < expenses.Length - 1; i++)
            {
                for (int j = 0; j < expenses.Length - i - 1; j++)
                {
                    if (expenses[j].Amount > expenses[j + 1].Amount)
                    {
                        // Обмен значениями
                        Expense temp = expenses[j];
                        expenses[j] = expenses[j + 1];
                        expenses[j + 1] = temp;
                    }
                }
            }
        }

        // 4. Конвертация валюты
        static void ConvertCurrency(Expense[] expenses)
        {
            Console.WriteLine("\n💱 Конвертация валюты");
            Console.WriteLine("=====================");
            Console.WriteLine("Доступные валюты:");
            Console.WriteLine("1. Доллар США (USD)");
            Console.WriteLine("2. Евро (EUR)");
            Console.WriteLine("3. Фунт стерлингов (GBP)");
            Console.WriteLine("4. Другая валюта (ввести курс вручную)");
            Console.Write("Выберите валюту (1-4): ");

            decimal exchangeRate = 0;
            string currencySymbol = "";

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        exchangeRate = 99.0m; 
                        currencySymbol = "USD";
                        break;
                    case 2:
                        exchangeRate =101.0m; 
                        currencySymbol = "EUR";
                        break;
                    case 3:
                        exchangeRate = 113.0m; 
                        currencySymbol = "GBP";
                        break;
                    case 4:
                        Console.Write("Введите курс рубля к валюте (1 руб = X валюты): ");
                        if (decimal.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out exchangeRate) && exchangeRate > 0)
                        {
                            Console.Write("Введите символ валюты: ");
                            currencySymbol = Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("❌ Неверный курс!");
                            return;
                        }
                        break;
                    default:
                        Console.WriteLine("❌ Неверный выбор!");
                        return;
                }
            }
            else
            {
                Console.WriteLine("❌ Неверный ввод!");
                return;
            }

            Console.WriteLine($"\n💱 Расходы в {currencySymbol}:");
            Console.WriteLine("====================");

            decimal totalRub = 0;
            decimal totalConverted = 0;

            foreach (var expense in expenses)
            {
                decimal convertedAmount = expense.Amount * exchangeRate;
                Console.WriteLine($"{expense.Name,-20} {expense.Amount,8:F2} руб = {convertedAmount,8:F2} {currencySymbol}");
                totalRub += expense.Amount;
                totalConverted += convertedAmount;
            }

            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Итого: {totalRub:F2} руб = {totalConverted:F2} {currencySymbol}");
            Console.WriteLine($"Курс: 1 руб = {exchangeRate} {currencySymbol}");
        }

        static void SearchByName(Expense[] expenses)
        {
            Console.WriteLine("\n🔍 Поиск по названию");
            Console.WriteLine("====================");
            Console.Write("Введите часть названия для поиска: ");
            string searchTerm = Console.ReadLine().ToLower();

            bool found = false;
            Console.WriteLine("\nРезультаты поиска:");
            Console.WriteLine("-------------------");

            foreach (var expense in expenses)
            {
                if (expense.Name.ToLower().Contains(searchTerm))
                {
                    Console.WriteLine($"{expense.Name,-20} {expense.Amount,8:F2} руб");
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("❌ Ничего не найдено!");
            }
        }
    }
}