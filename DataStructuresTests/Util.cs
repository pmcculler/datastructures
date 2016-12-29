using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataStructuresTests
{
    public class Util
    {
        static string _book;
        static readonly Random random = new Random();

        // call ahead of time if you want to do timing with later methods.
        public static void SetupBook()
        {
            if (_book == null)
            {
                _book = File.ReadAllText(@"..\..\pg100.txt");
            }
        }
        public static char[] GetLargeArray()
        {
            SetupBook();
            return _book.ToCharArray();
        }
        public static string[] GetManyEnglishStrings()
        {
            SetupBook();
            string[] sentences = _book.Split('.', '?', '!');
            return sentences;
        }
        public static string[] GetManyEnglishWords()
        {
            SetupBook();
            string[] words = _book.Split(new[] { '.', '?', '!', '\'', '"', '\r', '\n', ' ', ';', ':', '(', ')', '-', '/', '<', '>', ']', '[', ',' }, StringSplitOptions.RemoveEmptyEntries);
            return words;
        }
        public static string[] GetManyEnglish2Grams()
        {
            SetupBook();
            string[] words = _book.Split(new[] { '.', '?', '!', '\'', '"', '\r', '\n', ' ', ';', ':', '(', ')', '-', '/', '<', '>', ']', '[', ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> grams = new List<string>();

            for (int i = 0; i < words.Length - 1; i++)
                grams.Add(words[i] + words[i + 1]);

            return grams.ToArray();
        }

        public static string RandomString(int length = 0)
        {
            if (length == 0)
                return string.Empty;
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < length; i++)
            {
                builder.Append(((char)random.Next(255 - 33) + 33));
            }
            return builder.ToString();
        }
    }
}
