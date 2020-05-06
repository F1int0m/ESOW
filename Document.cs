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

        public Document(string title, string content, string translate, Difficult diff)
        {
            Title = title;
            Content = content;
            TranslatedContent = translate;
            Difficult = diff;
        }
    }


    public enum Difficult  
    {
        Custom,
        Easy,
        Medium,
        Hard,
        UHard
    }
}
