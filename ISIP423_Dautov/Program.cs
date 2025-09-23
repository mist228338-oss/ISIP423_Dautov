using System;
using System.Collections.Generic;
using System.Text;

class TextAnalyzer
{
    static List<TextStatistics> allStatistics = new List<TextStatistics>();

    static void Main()
    {
        Console.WriteLine("Добро пожаловать в анализатор текста!");

        while (true)
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1 - Анализ нового текста");
            Console.WriteLine("2 - Просмотр статистики по прошлым текстам");
            Console.WriteLine("3 - Выход");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AnalyzeNewText();
                    break;
                case "2":
                    ShowPreviousStatistics();
                    break;
                case "3":
                    Console.WriteLine("До свидания!");
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break; }}}
    static void AnalyzeNewText()
    {
        // Запрос текста у пользователя
        string text;
        do
        {
            Console.WriteLine("\nВведите текст (не менее 100 символов):");
            text = Console.ReadLine();

            if (text == null || text.Length < 100)
            {
                Console.WriteLine("Текст должен содержать не менее 100 символов. Попробуйте снова.");
            }
        } while (text == null || text.Length < 100);
        // Создание объекта для хранения статистики
        TextStatistics stats = new TextStatistics();
        stats.OriginalText = text;

        // Анализ текста
        AnalyzeText(text, stats);

        // Добавление статистики в общий список
        allStatistics.Add(stats);

        // Вывод результатов анализа
        DisplayCurrentStatistics(stats);
    }