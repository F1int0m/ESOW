﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESOW
{
    public class Document
    {
        public string Title { get; private set; }
        public Difficult Difficult { get; set; }
        public string Content { get; private set; }
        public string TranslatedContent { get; private set; }
        public bool IsOurText { get; private set; }
        public int WordsCount { get; private set; }

        public override string ToString()
        {
            return Title;
        }

        public Document(string title, string content, string translate, Difficult diff, int wordsCount, bool isOurText)
        {
            Title = title;
            Content = content;
            TranslatedContent = translate;
            Difficult = diff;
            WordsCount = wordsCount;
            IsOurText = isOurText;
        }
    }


    public enum Difficult  
    {
        Default,
        Custom,
        A2,
        B1,
        B2,
        C1
    }
}
