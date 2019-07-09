using AngleSharp.Parser.Html;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Parser.Core
{
    class ParserWorker
    {
        Parser parser;
        HtmlLoader loader;
        bool isActive;

        #region Properties

        public Parser Parser
        {
            get
            {
                return parser;
            }
            set
            {
                parser = value;
                loader = new HtmlLoader(value);
            }
        }

        public bool IsActive
        {
            get
            {
                return isActive;
            }
        }

        #endregion

        public event Action<object, string> OnNewData;
        public event Action<object> OnCompleted;

        public ParserWorker(Parser parser)
        {
            this.parser = parser;
        }

        public void Start()
        {
            isActive = true;
            Worker();
        }

        public void Abort()
        {
            isActive = false;
        }

        private async void Worker()
        {
            var result = new List<string>();
            loader = new HtmlLoader(parser);
            string temp = null;

            for (int i = 0; i < parser.WordsCount; i++)
            {
                if (!isActive)
                {
                    OnCompleted?.Invoke(this);
                    return;
                }

                var source = await loader.GetSourceByPageId(parser.Words[i]);
                var domParser = new HtmlParser();
                var document = await domParser.ParseAsync(source);

                temp = ParseWord(document, parser.Words[i]);

                result.Add(temp);
                OnNewData?.Invoke(this, temp);
                temp = null;
            }

            File.WriteAllLines("Result.txt", result);
            
            OnCompleted?.Invoke(this);
            isActive = false;
        }

        public string CutString(AngleSharp.Dom.IHtmlCollection<AngleSharp.Dom.IElement> str)
        {
            string temp = str.ElementAt(0).TextContent;
            string result = null;
            string[] t = temp.Split(',');
            for (int i = 0; i < t.Length && i < 2; i++)
                result = result + ", " + t[i];
            result = result.Remove(0, 1);
            return result;
        }

        public string ParseWord(AngleSharp.Dom.Html.IHtmlDocument document, string Word)
        {
            // Само слово на английском
            string temp = Word;

            // Британская транскрипция
            var t = document.QuerySelectorAll("span.transcription");
            if (t.Count() > 1)
                temp = temp + " " + t.ElementAt(1).TextContent;
            else
                temp = temp + " |NoTranscription|";

            // Перевод слова
            t = document.QuerySelectorAll("span.t_inline_en");
            if (t.Count() != 0)
                temp = temp + " " + CutString(t);
            else
                temp = temp + " NoTranslation";

            // Пример с этим словом
            t = document.QuerySelectorAll("p.ex_o");
            if (t.Count() != 0)
                temp = temp + " " + t.ElementAt(0).TextContent;

            return temp;
        }
    }
}
