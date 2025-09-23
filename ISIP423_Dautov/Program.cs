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
    static void AnalyzeText(string text, TextStatistics stats)
    {
        // Подсчет количества предложений
        stats.SentenceCount = CountSentences(text);

        // Разделение текста на слова
        string[] words = SplitTextIntoWords(text);
        stats.WordCount = words.Length;

        // Поиск самого короткого и самого длинного слова
        if (words.Length > 0)
        {
            stats.ShortestWord = FindShortestWord(words);
            stats.LongestWord = FindLongestWord(words);
        }

        // Подсчет гласных и согласных
        CountVowelsAndConsonants(text, stats);

        // Создание статистики по частоте букв
        stats.LetterFrequency = CalculateLetterFrequency(text);
    }
     
    // Добавляем последнее слово, если оно есть
        if (currentWord.Length > 0)
        {
            words.Add(currentWord.ToString());
        }

return words.ToArray();
    }
    
    static string FindShortestWord(string[] words)
{
    string shortest = words[0];

    for (int i = 1; i < words.Length; i++)
    {
        if (words[i].Length < shortest.Length)
        {
            shortest = words[i];
        }
    }

    return shortest;
}

static string FindLongestWord(string[] words)
{
    string longest = words[0];

    for (int i = 1; i < words.Length; i++)
    {
        if (words[i].Length > longest.Length)
        {
            longest = words[i];
        }
    }

    return longest;
}