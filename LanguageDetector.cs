using System;

public static class LanguageDetector
{
    public static string DetectLanguage(string text)
    {
        if (ContainsCyrillic(text))
        {
            return "Russian";
        }
        else if (ContainsLatin(text))
        {
            return "English";
        }
        else
        {
            return "Unknown";
        }
    }

    private static bool ContainsCyrillic(string text)
    {
        foreach (char c in text)
        {
            if (IsCyrillicCharacter(c))
            {
                return true;
            }
        }
        return false;
    }

    private static bool ContainsLatin(string text)
    {
        foreach (char c in text)
        {
            if (IsLatinCharacter(c))
            {
                return true;
            }
        }
        return false;
    }

    private static bool IsCyrillicCharacter(char c)
    {
        // Проверка, что символ - кириллический
        return (c >= 'А' && c <= 'я') || c == 'ё' || c == 'Ё';
    }

    private static bool IsLatinCharacter(char c)
    {
        // Проверка, что символ - латинский
        return (c >= 'A' && c <= 'z');
    }
}

//public class Program
//{
//    public static void Main()
//    {
//        string text1 = "Пример текста на русском языке";
//        string text2 = "Example text in English";
//        string text3 = "Пример текста смешанного";

//        string language1 = LanguageDetector.DetectLanguage(text1);
//        string language2 = LanguageDetector.DetectLanguage(text2);
//        string language3 = LanguageDetector.DetectLanguage(text3);

//        Console.WriteLine($"Text 1 language: {language1}");
//        Console.WriteLine($"Text 2 language: {language2}");
//        Console.WriteLine($"Text 3 language: {language3}");
//    }
//}
