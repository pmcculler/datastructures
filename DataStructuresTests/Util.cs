using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresTests
{
    public class Util
    {
        static string book = null;

        // call ahead of time if you want to do timing with later methods.
        static public void setupBook()
        {
            if (book == null)
            {
                book = File.ReadAllText(@"C:\Users\Patrick\Documents\pg100.txt");
            }
        }
        public static char[] getLargeArray()
        {
            setupBook();
            return book.ToCharArray();
        }
        public static string[] getManyEnglishStrings()
        {
            setupBook();
            string[] sentences = book.Split(new char[] { '.', '?', '!' });
            return sentences;
        }
        public static string[] getManyEnglishWords()
        {
            setupBook();
            string[] words = book.Split(new char[] { '.', '?', '!', '\'', '"', '\r', '\n', ' ', ';', ':', '(', ')', '-', '/', '<', '>', ']', '[', ',' }, StringSplitOptions.RemoveEmptyEntries);
            return words;
        }
        public static string[] getManyEnglish2Grams()
        {
            setupBook();
            string[] words = book.Split(new char[] { '.', '?', '!', '\'', '"', '\r', '\n', ' ', ';', ':', '(', ')', '-', '/', '<', '>', ']', '[', ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> grams = new List<string>();

            for (int i = 0; i < words.Length - 1; i++)
                grams.Add(words[i] + words[i + 1]);

            return grams.ToArray();
        }
    }
}
