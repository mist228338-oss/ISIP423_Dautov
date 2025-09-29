using System;
using System.Collections.Generic;
using System.Text;

class TextStatistics
{
    public int WordCount { get; set; }
    public string ShortestWord { get; set; }
    public int SentenceCount { get; set; }
    public int ParagraphCount { get; set; }
    public int VowelCount { get; set; }
    public int ConsonantCount { get; set; }
    public string LongestWord { get; set; }
    public Dictionary<char, int> LetterFrequency { get; set; }

    public TextStatistics()
    {
        LetterFrequency = new Dictionary<char, int>();
        ShortestWord = "";
        LongestWord = "";
    }
}

class Program
{
    private static List<TextStatistics> allStatistics = new List<TextStatistics>();

    static void Main(string[] args)
    {
        Console.WriteLine("Программа для анализа текста");
        Console.WriteLine("Нажмите любую клавишу для начала...");
        Console.ReadKey();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Введите текст (не менее 100 символов):");
            Console.WriteLine("(Для завершения ввода нажмите Enter дважды подряд)");

            // Многострочный ввод текста
            StringBuilder textBuilder = new StringBuilder();
            string line;
            int emptyLineCount = 0;

            while (true)
            {
                line = Console.ReadLine();

                // Проверка на двойное нажатие Enter (пустая строка)
                if (string.IsNullOrWhiteSpace(line))
                {
                    emptyLineCount++;
                    if (emptyLineCount >= 2 || textBuilder.Length > 0)
                        break;
                    else
                        continue;
                }
                else
                {
                    emptyLineCount = 0;
                    textBuilder.AppendLine(line);
                }
            }

            string text = textBuilder.ToString().Trim();

            if (text.Length < 100)
            {
                Console.WriteLine($"Текст должен содержать минимум 100 символов. Сейчас: {text.Length}. Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
                continue;
            }

            TextStatistics stats = AnalyzeText(text);
            allStatistics.Add(stats);

            Console.Clear();
            DisplayStatistics(stats);

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();

            Console.Clear();
            Console.WriteLine("Хотите ввести новый текст? (да/нет)");
            string response = Console.ReadLine().ToLower();

            if (response != "да" && response != "д" && response != "yes" && response != "y")
                break;
        }

        DisplayAllStatistics();

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }

    static TextStatistics AnalyzeText(string text)
    {
        TextStatistics stats = new TextStatistics();

        // Подсчет абзацев (разделяются пустыми строками)
        string[] paragraphs = text.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
        stats.ParagraphCount = paragraphs.Length;

        // Подсчет слов и поиск самого короткого/длинного слова
        string[] words = SplitIntoWords(text);
        stats.WordCount = words.Length;
        if (words.Length > 0)
        {
            stats.ShortestWord = words[0];
            stats.LongestWord = words[0];
            foreach (string word in words)
            {
                if (word.Length < stats.ShortestWord.Length)
                    stats.ShortestWord = word;
                if (word.Length > stats.LongestWord.Length)
                    stats.LongestWord = word;
            }
        }

        // Подсчет предложений
        char[] sentenceSeparators = { '.', '!', '?', ';' };
        int sentenceCount = 0;
        bool inSentence = false;

        foreach (char c in text)
        {
            if (char.IsLetter(c) || char.IsDigit(c))
            {
                if (!inSentence)
                {
                    inSentence = true;
                }
            }
            else if (Array.Exists(sentenceSeparators, sep => sep == c) && inSentence)
            {
                sentenceCount++;
                inSentence = false;
            }
        }

        // Учет последнего предложения, если текст не заканчивается разделителем
        if (inSentence)
        {
            sentenceCount++;
        }

        stats.SentenceCount = sentenceCount;

        // Подсчет гласных и согласных
        string vowels = "аеёиоуыэюяaeiou";
        string consonants = "бвгджзйклмнпрстфхцчшщbcdfghjklmnpqrstvwxyz";

        foreach (char c in text.ToLower())
        {
            if (char.IsLetter(c))
            {
                if (vowels.Contains(c.ToString()))
                    stats.VowelCount++;
                else if (consonants.Contains(c.ToString()))
                    stats.ConsonantCount++;
            }
        }

        // Статистика частоты букв
        foreach (char c in text.ToLower())
        {
            if (char.IsLetter(c))
            {
                if (stats.LetterFrequency.ContainsKey(c))
                    stats.LetterFrequency[c]++;
                else
                    stats.LetterFrequency[c] = 1;
            }
        }

        return stats;
    }

    static string[] SplitIntoWords(string text)
    {
        List<string> words = new List<string>();
        StringBuilder currentWord = new StringBuilder();

        foreach (char c in text)
        {
            if (char.IsLetter(c) || c == '\'' || c == '-')
            {
                currentWord.Append(c);
            }
            else if (currentWord.Length > 0)
            {
                words.Add(currentWord.ToString());
                currentWord.Clear();
            }
        }

        if (currentWord.Length > 0)
        {
            words.Add(currentWord.ToString());
        }

        return words.ToArray();
    }

    static void DisplayStatistics(TextStatistics stats)
    {
        Console.WriteLine("=== Статистика текста ===");
        Console.WriteLine($"Количество абзацев: {stats.ParagraphCount}");
        Console.WriteLine($"Количество слов: {stats.WordCount}");
        Console.WriteLine($"Самое короткое слово: {stats.ShortestWord}");
        Console.WriteLine($"Самое длинное слово: {stats.LongestWord}");
        Console.WriteLine($"Количество предложений: {stats.SentenceCount}");
        Console.WriteLine($"Гласные буквы: {stats.VowelCount}");
        Console.WriteLine($"Согласные буквы: {stats.ConsonantCount}");

        Console.WriteLine("\nЧастота букв:");
        // Сортировка букв по частоте (по убыванию)
        var sortedLetters = new List<KeyValuePair<char, int>>(stats.LetterFrequency);
        for (int i = 0; i < sortedLetters.Count - 1; i++)
        {
            for (int j = i + 1; j < sortedLetters.Count; j++)
            {
                if (sortedLetters[i].Value < sortedLetters[j].Value)
                {
                    var temp = sortedLetters[i];
                    sortedLetters[i] = sortedLetters[j];
                    sortedLetters[j] = temp;
                }
            }
        }

        foreach (var entry in sortedLetters)
        {
            Console.WriteLine($"{entry.Key}: {entry.Value}");
        }
    }

    static void DisplayAllStatistics()
    {
        if (allStatistics.Count == 0)
        {
            Console.WriteLine("Нет данных для отображения.");
            return;
        }

        Console.WriteLine("=== Статистика по всем текстам ===");
        for (int i = 0; i < allStatistics.Count; i++)
        {
            Console.WriteLine($"\nТекст #{i + 1}:");
            Console.WriteLine($"- Абзацев: {allStatistics[i].ParagraphCount}");
            Console.WriteLine($"- Слов: {allStatistics[i].WordCount}");
            Console.WriteLine($"- Предложений: {allStatistics[i].SentenceCount}");
            Console.WriteLine($"- Гласные/Согласные: {allStatistics[i].VowelCount}/{allStatistics[i].ConsonantCount}");
            Console.WriteLine($"- Самое короткое слово: {allStatistics[i].ShortestWord}");
            Console.WriteLine($"- Самое длинное слово: {allStatistics[i].LongestWord}");
        }
    }
}