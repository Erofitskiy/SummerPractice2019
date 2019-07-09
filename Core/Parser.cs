using System.IO;

namespace Parser.Core
{
    public class Parser
    {

        public Parser()
        {
            Words = File.ReadAllLines("text.txt");
            WordsCount = Words.Length;

        }

        public string[] Words { get; set; } = null;

        public string BaseUrl { get; set; } = "https://wooordhunt.ru";

        public string Prefix { get; set; } = "word/{CurrentWord}";

        public int WordsCount { get; set; } = 0;

    }
}
